using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.Data.SQLite.Properties;
using GalleryServerPro.ErrorHandler;

namespace GalleryServerPro.Data.SQLite
{
	/// <summary>
	/// Provides the implementation for retrieving and persisting gallery data to a SQLite database.
	/// </summary>
	public class SQLiteGalleryServerProProvider : Provider.DataProvider
	{
		#region Private Fields

		// This variable should reference the current version of the database schema required by this provider.
		// During InitializeDataStore, this value is compared against the schema version stored in the database
		// The database is upgraded if appropriate.
		private const GalleryDataSchemaVersion _databaseSchemaVersion = GalleryDataSchemaVersion.V2_3_3421;

		private static readonly object _sharedLock = new object();
		private const string _httpTransactionId = "SQLiteTran";
		private static string _applicationName;
		private static string _connectionStringName;
		private static string _connectionString;
		private const string _exSql = "SQL";

		#endregion

		#region Enum Declarations

		/// <summary>
		/// References the name of a SQL upgrade script embedded in this assembly that can be used to upgrade the 
		/// database schema.
		/// </summary>
		private enum GalleryDataSchemaUpgradeScript
		{
			/// <summary>
			/// Gets the script file that upgrades the database from 2.1.3162 to 2.3.3421.
			/// </summary>
			SqlUpgrade_2_1_3162_to_2_3_3421 = 0
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the friendly name used to refer to the provider during configuration.
		/// </summary>
		/// <value>The friendly name used to refer to the provider during configuration.</value>
		/// <returns>The friendly name used to refer to the provider during configuration.</returns>
		public override string Name
		{
			get { return ((System.Configuration.Provider.ProviderBase)this).Name; }
		}

		/// <summary>
		/// Gets a brief, friendly description suitable for display in administrative tools or other user interfaces (UIs).
		/// </summary>
		/// <value>A brief, friendly description suitable for display in administrative tools or other UIs.</value>
		/// <returns>A brief, friendly description suitable for display in administrative tools or other UIs.</returns>
		public override string Description
		{
			get { return ((System.Configuration.Provider.ProviderBase)this).Description; }
		}

		/// <summary>
		/// Gets or sets the name of the connection string.
		/// </summary>
		/// <value>The name of the connection string.</value>
		public override string ConnectionStringName
		{
			get
			{
				return _connectionStringName;
			}
		}

		/// <summary>
		/// Gets or sets the name of the application to store and retrieve Gallery Server data for.
		/// </summary>
		/// <value>
		/// The name of the application to store and retrieve Gallery Server data for.
		/// </value>
		public override string ApplicationName
		{
			get
			{
				return _applicationName;
			}
			set
			{
				_applicationName = value;
			}
		}

		#endregion

		#region Data Provider Methods

		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="name">The friendly name of the provider.</param>
		/// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The name of the provider is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The name of the provider has a length of zero.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.
		/// </exception>
		public override void Initialize(string name, NameValueCollection config)
		{
			// Initialize values from web.config.
			if (config == null)
				throw new ArgumentNullException("config");

			if (name == null || name.Length == 0)
				name = "SQLiteGalleryServerProProvider";

			if (String.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "SQLite Gallery data provider");
			}

			// Initialize the abstract base class.
			base.Initialize(name, config);

			if (config["applicationName"] == null || config["applicationName"].Trim() == "")
			{
				_applicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
			}
			else
			{
				_applicationName = config["applicationName"];
			}

			// Initialize SQLiteConnection.
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

			if (connectionStringSettings == null || connectionStringSettings.ConnectionString.Trim() == "")
			{
				throw new ProviderException("Connection string cannot be blank.");
			}

			_connectionStringName = connectionStringSettings.Name;
			_connectionString = connectionStringSettings.ConnectionString;
		}

		#endregion

		#region Gallery Methods

		/// <summary>
		/// Gets an <see cref="IGallery" /> representing the current gallery. The return value is the same reference 
		/// as the parameter.
		/// </summary>
		/// <param name="gallery">An <see cref="IGallery"/> object to populate with the information about the
		/// current gallery. This parameter is required because the library that implements this interface does not have
		/// the ability to instantiate any object that implements <see cref="IGallery"/>.
		/// information from the data store.</param>
		/// <returns>Returns an <see cref="IGallery" /> representing the current gallery. The returned object is the
		/// same object in memory as the <paramref name="gallery"/> parameter.</returns>
		public override IGallery Gallery_GetCurrentGallery(IGallery gallery)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = @"
SELECT GalleryId, Description, DateAdded
FROM [gs_Gallery]
WHERE GalleryId = @GalleryId";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
					if (cn.State == ConnectionState.Closed)
						cn.Open();

					using (IDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							gallery.Id = Convert.ToInt32(dr["GalleryId"]);
							gallery.Description = dr["Description"].ToString();
							gallery.CreationDate = Convert.ToDateTime(dr["DateAdded"]);
							break;
						}
					}
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
					cn.Dispose();
			}

			return gallery;
		}

		#endregion

		#region Album methods

		/// <summary>
		/// Persist the specified album to the data store. Return the ID of the album.
		/// </summary>
		/// <param name="album">An instance of <see cref="IAlbum"/> to persist to the data store.</param>
		/// <returns>
		/// Return the ID of the album. If this is a new album and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.
		/// </returns>
		public override int Album_Save(IAlbum album)
		{
			SQLiteCommand cmd = null;

			try
			{
				if (album.IsNew)
				{
					cmd = GetCommandAlbumInsert(album);

					if (cmd.Connection.State == ConnectionState.Closed)
						cmd.Connection.Open();

					int id = Convert.ToInt32(cmd.ExecuteScalar());

					if (album.Id != id)
						album.Id = id;
				}
				else
				{
					cmd = GetCommandAlbumUpdate(album);

					if (cmd.Connection.State == ConnectionState.Closed)
						cmd.Connection.Open();

					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if ((!IsTransactionInProgress()) && (cmd != null) && (cmd.Connection != null) && (cmd.Connection.State == ConnectionState.Open))
					cmd.Connection.Dispose();

				if (cmd != null)
					cmd.Dispose();
			}

			return album.Id;
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the child albums contained within the album specified by
		/// albumId parameter. If no matching objects are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the album for which to return the child albums
		/// contained within.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object containing a field named ID that contains the IDs of all albums
		/// directly within the album represented by albumId.
		/// </returns>
		public override System.Data.IDataReader Album_GetDataReaderChildAlbumsById(int albumId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT AlbumId
FROM [gs_Album]
WHERE AlbumParentId = @AlbumId AND FKGalleryId = @GalleryId
";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@AlbumId", albumId);
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the child gallery objects contained within the album specfified by
		/// albumId parameter. If no matching objects are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the desired album.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object containing all relevant fields for the gallery objects.
		/// </returns>
		public override System.Data.IDataReader Album_GetDataReaderChildMediaObjectsById(int albumId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT 
	MediaObjectId, FKAlbumId, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, 
	CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM [gs_MediaObject]
WHERE FKAlbumId = @AlbumId
";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@AlbumId", albumId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the album for the specified albumId. If no matching object
		/// is found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the desired album.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all album fields.
		/// </returns>
		public override System.Data.IDataReader Album_GetDataReaderAlbumById(int albumId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT
	AlbumId, FKGalleryId as GalleryId, AlbumParentId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, 
	Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
FROM [gs_Album]
WHERE AlbumId = @AlbumId AND FKGalleryId = @GalleryId
";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@AlbumId", albumId);
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the top-level album in the gallery. This method is guaranteed
		/// to return an instance with one record representing the top-level album, since a default root
		/// album is created if one does not exist.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all album fields.
		/// </returns>
		public override System.Data.IDataReader Album_GetDataReaderRootAlbum(int galleryId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT
	AlbumId, AlbumParentId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, 
	Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
FROM [gs_Album]
WHERE AlbumParentId = 0 AND FKGalleryId = @GalleryId";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Permanently delete the specified album from the data store, including any
		/// child albums and media objects (cascading delete). This action cannot be undone.
		/// </summary>
		/// <param name="album">The <see cref="IAlbum"/> to delete from the data store.</param>
		public override void Album_Delete(IAlbum album)
		{
			DeleteAlbum(album);
		}

		#endregion

		#region Media Object methods

		/// <summary>
		/// Persist the specified media object to the data store. Return the ID of the media object.
		/// </summary>
		/// <param name="mediaObject">An instance of <see cref="IGalleryObject"/> to persist to the data store.</param>
		/// <returns>
		/// Return the ID of the media object. If this is a new media object and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.
		/// </returns>
		public override int MediaObject_Save(IGalleryObject mediaObject)
		{
			SQLiteTransaction tran = null;
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (!IsTransactionInProgress())
					tran = cn.BeginTransaction();

				if (mediaObject.IsNew)
				{
					using (SQLiteCommand cmd = GetCommandMediaObjectInsert(mediaObject, cn))
					{
						int id = Convert.ToInt32(cmd.ExecuteScalar());

						if (mediaObject.Id != id)
							mediaObject.Id = id;
					}

					// Insert metadata items, if any, into MediaObjectMetadata table.
					InsertMetadataItems(mediaObject, cn);
				}
				else
				{
					using (SQLiteCommand cmd = GetCommandMediaObjectUpdate(mediaObject, cn))
					{
						cmd.ExecuteNonQuery();
					}

					// Update metadata items, if necessary, in MediaObjectMetadata table.
					UpdateMetadataItems(mediaObject, cn);
				}

				// Commit the transaction if it's the one we created in this method.
				if (tran != null)
					tran.Commit();
			}
			catch
			{
				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();

				if (!IsTransactionInProgress())
					cn.Dispose();
			}

			return mediaObject.Id;
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the media object for the specified mediaObjectId. If no matching object
		/// is found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the desired media object.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all media object fields.
		/// </returns>
		public override System.Data.IDataReader MediaObject_GetDataReaderMediaObjectById(int mediaObjectId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT
	mo.MediaObjectId, mo.FKAlbumId, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
	mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
	mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.ExternalHtmlSource, mo.ExternalType, mo.Seq, 
	mo.CreatedBy, mo.DateAdded, mo.LastModifiedBy, mo.DateLastModified, mo.IsPrivate
FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumId = a.AlbumId
WHERE mo.MediaObjectId = @MediaObjectId AND a.FKGalleryId = @GalleryId;";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@MediaObjectId", mediaObjectId);
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the metadata items for the specified mediaObjectId. If no matching object
		/// is found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the desired media object.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all metadata items.
		/// </returns>
		public override System.Data.IDataReader MediaObject_GetDataReaderMetadataItemsByMediaObjectId(int mediaObjectId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT
	md.MediaObjectMetadataId, md.FKMediaObjectId, md.MetadataNameIdentifier, md.Description, md.Value
FROM [gs_MediaObjectMetadata] md JOIN [gs_MediaObject] mo ON md.FKMediaObjectId = mo.MediaObjectId
	JOIN [gs_Album] a ON mo.FKAlbumId = a.AlbumId
WHERE md.FKMediaObjectId = @MediaObjectId AND a.FKGalleryId = @GalleryId;";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@MediaObjectId", mediaObjectId);
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Permanently delete the specified media object from the data store. This action cannot
		/// be undone.
		/// </summary>
		/// <param name="mediaObject">The <see cref="IGalleryObject"/> to delete from the data store.</param>
		public override void MediaObject_Delete(IGalleryObject mediaObject)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = @"
DELETE FROM [gs_MediaObjectMetadata]
WHERE FKMediaObjectId = @MediaObjectId;

DELETE FROM [gs_MediaObject]
WHERE MediaObjectId = @MediaObjectId;";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@MediaObjectId", mediaObject.Id);

					if (cn.State == ConnectionState.Closed)
						cn.Open();

					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
				{
					cn.Dispose();
				}
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the hash keys for all media objects in the specified gallery.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with one field named "HashKey" containing the hash keys
		/// for all media objects in the current gallery.
		/// </returns>
		public override System.Data.IDataReader MediaObject_GetAllHashKeys(int galleryId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT [gs_MediaObject].HashKey
FROM [gs_MediaObject] INNER JOIN [gs_Album] ON [gs_MediaObject].FKAlbumId = [gs_Album].AlbumId
WHERE [gs_Album].FKGalleryId = @GalleryId;";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		#endregion

		#region Synchronize methods

		/// <summary>
		/// Persist the synchronization information to the data store.
		/// </summary>
		/// <param name="synchStatus">An <see cref="ISynchronizationStatus"/> object containing the synchronization information
		/// to persist to the data store.</param>
		/// <exception>Throws a GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException if the data
		/// store indicates another synchronization is already in progress for this gallery.</exception>
		public override void Synchronize_SaveStatus(ISynchronizationStatus synchStatus)
		{
			SQLiteTransaction tran = null;
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					if (cn.State == ConnectionState.Closed)
						cn.Open();

					if (!IsTransactionInProgress())
						tran = cn.BeginTransaction();

					string sql = "SELECT EXISTS(SELECT * FROM [gs_Synchronize] WHERE FKGalleryId = @GalleryId AND SynchId <> @SynchId AND (SynchState = 1 OR SynchState = 2))";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
					cmd.Parameters.AddWithValue("@SynchId", synchStatus.SynchId);
					if (Convert.ToBoolean(cmd.ExecuteScalar()))
					{
						throw new GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException();
					}

					sql = "SELECT EXISTS(SELECT * FROM [gs_Synchronize] WHERE FKGalleryId = @GalleryId)";
					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
					if (Convert.ToBoolean(cmd.ExecuteScalar()))
					{
						sql = @"
UPDATE [gs_Synchronize]
SET SynchId = @SynchId,
	FKGalleryId = @GalleryId,
	SynchState = @SynchState,
	TotalFiles = @TotalFiles,
	CurrentFileIndex = @CurrentFileIndex
WHERE FKGalleryId = @GalleryId";
						cmd.Parameters.Clear();
						cmd.CommandText = sql;
						cmd.Parameters.AddWithValue("@SynchId", synchStatus.SynchId);
						cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
						cmd.Parameters.AddWithValue("@SynchState", (int)synchStatus.Status);
						cmd.Parameters.AddWithValue("@TotalFiles", synchStatus.TotalFileCount);
						cmd.Parameters.AddWithValue("@CurrentFileIndex", synchStatus.CurrentFileIndex);
						cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
						cmd.ExecuteNonQuery();
					}
					else
					{
						sql = "INSERT INTO [gs_Synchronize] (SynchId, FKGalleryId, SynchState, TotalFiles, CurrentFileIndex)"
							+ " VALUES (@SynchId, @GalleryId, @SynchState, @TotalFiles, @CurrentFileIndex)";
						cmd.CommandText = sql;
						cmd.Parameters.Clear();
						cmd.Parameters.AddWithValue("@SynchId", synchStatus.SynchId);
						cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
						cmd.Parameters.AddWithValue("@SynchState", (int)synchStatus.Status);
						cmd.Parameters.AddWithValue("@TotalFiles", synchStatus.TotalFileCount);
						cmd.Parameters.AddWithValue("@CurrentFileIndex", synchStatus.CurrentFileIndex);
						cmd.ExecuteNonQuery();
					}

					// Commit the transaction if it's the one we created in this method.
					if (tran != null)
						tran.Commit();
				}
			}
			catch
			{
				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();

				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		/// <summary>
		/// Retrieve the most recent synchronization information from the data store and set the relevant properties
		/// on the <paramref name="synchStatus"/> parameter. The return value is the same reference as the parameter.
		/// </summary>
		/// <param name="synchStatus">An <see cref="ISynchronizationStatus"/> object to populate with the most recent synchronization
		/// information from the data store.</param>
		/// <returns>
		/// Returns an <see cref="ISynchronizationStatus"/> object with updated properties based on what was retrieved
		/// from the data store.
		/// </returns>
		public override ISynchronizationStatus Synchronize_UpdateStatusFromDataStore(ISynchronizationStatus synchStatus)
		{
			bool updatedStatus = false;

			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = @"
SELECT SynchId, FKGalleryId, SynchState, TotalFiles, CurrentFileIndex
FROM [gs_Synchronize]
WHERE FKGalleryId = @GalleryId;";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
					if (cn.State == ConnectionState.Closed)
						cn.Open();

					using (IDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							synchStatus.SynchId = dr["SynchId"].ToString();
							synchStatus.Status = (SynchronizationState)Enum.Parse(typeof(SynchronizationState), dr["SynchState"].ToString());
							synchStatus.TotalFileCount = Convert.ToInt32(dr["TotalFiles"]);
							synchStatus.CurrentFileIndex = Convert.ToInt32(dr["CurrentFileIndex"]);

							updatedStatus = true;
							break;
						}
					}
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
					cn.Dispose();
			}

			if (!updatedStatus)
			{
				throw new System.Data.RowNotInTableException(string.Format(CultureInfo.CurrentCulture, Resources.Synchronize_UpdateStatusFromDataStore_ExMsg2, synchStatus.GalleryId));
			}

			return synchStatus;
		}

		#endregion

		#region App Error methods

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the application errors. If no matching objects are found
		/// in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all application error fields.
		/// </returns>
		public override IDataReader AppError_GetAppErrors()
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT
  AppErrorId, FKGalleryId, [TimeStamp], ExceptionType, Message, Source, TargetSite, StackTrace, ExceptionData, 
  InnerExType, InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
  FormVariables, Cookies, SessionVariables, ServerVariables
FROM gs_AppError
WHERE FKGalleryId = @GalleryId
";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Persist the specified application error to the data store. Return the ID of the error.
		/// </summary>
		/// <param name="appError">The application error to persist to the data store.</param>
		/// <returns>
		/// Return the ID of the error. If this is a new error object and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.
		/// </returns>
		public override int AppError_Save(IAppError appError)
		{
			SQLiteCommand cmd = null;

			try
			{
				if (appError.AppErrorId == int.MinValue)
				{
					cmd = GetCommandAppErrorInsert(appError);

					if (cmd.Connection.State == ConnectionState.Closed)
						cmd.Connection.Open();

					appError.AppErrorId = Convert.ToInt32(cmd.ExecuteScalar());
				}
				else
				{
					throw new DataException("Cannot update an existing AppError object. Only new objects can be persisted to the data store.");
				}
			}
			finally
			{
				if ((!IsTransactionInProgress()) && (cmd != null) && (cmd.Connection != null) && (cmd.Connection.State == ConnectionState.Open))
					cmd.Connection.Dispose();

				if (cmd != null)
					cmd.Dispose();
			}

			return appError.AppErrorId;

		}

		/// <summary>
		/// Delete the application error from the data store.
		/// </summary>
		/// <param name="appErrorId">The value that uniquely identifies this application error (<see cref="IAppError.AppErrorId"/>).</param>
		public override void AppError_Delete(int appErrorId)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = "DELETE FROM [gs_AppError] WHERE AppErrorId = @AppErrorId";

					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@AppErrorId", appErrorId);

					if (cn.State == ConnectionState.Closed)
						cn.Open();

					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
				{
					cn.Dispose();
				}
			}
		}

		/// <summary>
		/// Permanently delete all errors from the data store.
		/// </summary>
		public override void AppError_ClearLog()
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = "DELETE FROM [gs_AppError] WHERE FKGalleryId = @GalleryId";

					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

					if (cn.State == ConnectionState.Closed)
						cn.Open();

					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
				{
					cn.Dispose();
				}
			}
		}

		private static SQLiteCommand GetCommandAppErrorInsert(IAppError appError)
		{
			const string sql = @"
INSERT INTO [gs_AppError]
  (FKGalleryId, TimeStamp, ExceptionType, Message, Source, TargetSite, StackTrace, ExceptionData, InnerExType, 
  InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
  FormVariables, Cookies, SessionVariables, ServerVariables)
VALUES (@GalleryId, @TimeStamp, @ExceptionType, @Message, @Source, @TargetSite, @StackTrace, @ExceptionData, @InnerExType, 
  @InnerExMessage, @InnerExSource, @InnerExTargetSite, @InnerExStackTrace, @InnerExData, @Url,
  @FormVariables, @Cookies, @SessionVariables, @ServerVariables);SELECT last_insert_rowid();";
			SQLiteCommand cmd = new SQLiteCommand(sql, GetDBConnectionForGallery());

			cmd.Parameters.AddWithValue("@GalleryId", appError.GalleryId);

			if (appError.TimeStamp > DateTime.MinValue)
				cmd.Parameters.AddWithValue("@TimeStamp", appError.TimeStamp);
			else
				cmd.Parameters.AddWithValue("@TimeStamp", DBNull.Value);

			cmd.Parameters.AddWithValue("@ExceptionType", appError.ExceptionType);
			cmd.Parameters.AddWithValue("@Message", appError.Message);
			cmd.Parameters.AddWithValue("@Source", appError.Source);
			cmd.Parameters.AddWithValue("@TargetSite", appError.TargetSite);
			cmd.Parameters.AddWithValue("@StackTrace", appError.StackTrace);
			cmd.Parameters.AddWithValue("@ExceptionData", ErrorHandler.Error.Serialize(appError.ExceptionData));
			cmd.Parameters.AddWithValue("@InnerExType", appError.InnerExType);
			cmd.Parameters.AddWithValue("@InnerExMessage", appError.InnerExMessage);
			cmd.Parameters.AddWithValue("@InnerExSource", appError.InnerExSource);
			cmd.Parameters.AddWithValue("@InnerExTargetSite", appError.InnerExTargetSite);
			cmd.Parameters.AddWithValue("@InnerExStackTrace", appError.InnerExStackTrace);
			cmd.Parameters.AddWithValue("@InnerExData", ErrorHandler.Error.Serialize(appError.InnerExData));
			cmd.Parameters.AddWithValue("@Url", appError.Url);
			cmd.Parameters.AddWithValue("@FormVariables", ErrorHandler.Error.Serialize(appError.FormVariables));
			cmd.Parameters.AddWithValue("@Cookies", ErrorHandler.Error.Serialize(appError.Cookies));
			cmd.Parameters.AddWithValue("@SessionVariables", ErrorHandler.Error.Serialize(appError.SessionVariables));
			cmd.Parameters.AddWithValue("@ServerVariables", ErrorHandler.Error.Serialize(appError.ServerVariables));

			return cmd;
		}

		#endregion

		#region General methods

		/// <summary>
		/// Perform any needed data store operations to get Gallery Server ready to go. This includes verifying the
		/// database has the minimum default records in certain tables and/or upgrading the database to the version
		/// required by this provider. If no records are configured for the current gallery ID, they are created as
		/// necessary. Thus, this method can be used to create a new gallery. The gallery ID that is used is the one
		/// specified in the config file (galleryserverpro.config).
		/// </summary>
		public override void InitializeDataStore()
		{
			lock (_sharedLock)
			{
				VerifySchemaVersion();
			}

			Initialize();
		}

		/// <summary>
		/// Return gallery objects that match the specified search string. A gallery object is considered a match when
		/// all search terms are found in the relevant fields.
		/// For albums, the title and summary fields are searched. For media objects, the title, original filename,
		/// and metadata are searched. The contents of documents are not searched (e.g. the text of a Word or PDF file).
		/// If no matches are found, <paramref name="matchingAlbumIds"/> and <paramref name="matchingMediaObjectIds"/>
		/// will be empty, not null collections.
		/// </summary>
		/// <param name="searchTerms">A string array of search terms. Specify a single word for each item of the array, or
		/// combine words in an element to force a phase match. Items with more than one word indicate an exact
		/// phrase match is required. Example: There are three items where item 1="cat", item 2="0 step", and item 3="Mom".
		/// This method will match all gallery objects that contain the strings "cat", "0 step", and "Mom". It will also
		/// match partial words, such as Mom on steps at cathedral</param>
		/// <param name="matchingAlbumIds">The album IDs for all albums that match the search terms.</param>
		/// <param name="matchingMediaObjectIds">The media object IDs for all media objects that match the search terms.</param>
		/// <example>
		/// 	<para>Example 1</para>
		/// 	<para>The search terms are three elements: "cat", "step", and "Mom". All gallery objects that contain all
		/// three strings will be returned, such as an image with the caption "Mom and cat sitting on steps" (Notice the
		/// successful partial match between step and steps. However, the inverse is not true - searching for "steps"
		/// will not match "step".) Also matched would be an image with a caption "Mom at cathedral" and the exposure
		/// compensation metadata is "0 step".</para>
		/// 	<para>Example 2</para>
		/// 	<para>The search terms are two elements: "at the beach" and "Joey". All gallery objects that contain the
		/// phrase "at the beach" and "Joey" will be returned, such as a video with the caption "Joey at the beach with Mary".
		/// An image with the caption "Joey on the beach at Mary's house" will not match because the phrase "at the beach"
		/// is not present.
		/// </para>
		/// </example>
		public override void SearchGallery(string[] searchTerms, out List<int> matchingAlbumIds, out List<int> matchingMediaObjectIds)
		{
			// 1. Create a temporary table tmpSearchTerms and insert the search terms into it, prepending and appending the wildcard
			//    character (%). Ex: If @SearchTerms = "cat videos,dog,fish", tmpSearchTerms will get 3 records: %cat videos%,
			//    %dog%, %fish%.
			// 2. Create a second temporary table tmpSearchResults to hold intermediate search results.
			// 3. Insert into tmpSearchResults all albums where the title matches one of more search terms. There will be one record
			//    inserted for each matching search term. Ex: If @SearchTerms = "cat videos,dog,fish" and the album title =
			//    "My dog and cat videos", there will be two records inserted into tmpSearchResults, one with matchingSearchTerm =
			//    "%cat videos%" and the other "%dog%" (gotype='a', id=album ID,fieldname='Album.Title' for both).
			// 4. Repeat the above step for other fields: Album.Summary, MediaObject.Title,MediaObject.OriginalFilename, and
			//    all media object metadata for each media object
			// 5. Select those records from tmpSearchResults where we made a successful match for EVERY search term for each album or
			//    media object.

			DbTransaction tran = null;
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (!IsTransactionInProgress())
					tran = cn.BeginTransaction();

				int galleryId = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					#region Create temporary tables and populate search table.

					// 1a. Create temporary tables to hold intermediate search results. FYI: About temporary tables in SQLite 
					// from http://www.sqlite.org/lang_createtable.html:
					// "If the "TEMP" or "TEMPORARY" keyword occurs in between "CREATE" and "TABLE" then the table that is created 
					// is only visible within that same database connection and is automatically deleted when the database 
					// connection is closed. Any indices created on a temporary table are also temporary. Temporary tables and 
					// indices are stored in a separate file distinct from the main database file."
					string sql = @"
						CREATE TEMP TABLE [tmpSearchTerms] (
						[SearchTerm] NVARCHAR(3000) NOT NULL);

						CREATE TEMP TABLE [tmpSearchResults] (
						[goType] CHAR(1) NOT NULL,
						[id] INTEGER NOT NULL,
						[fieldName] NVARCHAR(50) NOT NULL,
						[matchingSearchTerm] NVARCHAR(3000) NOT NULL);
						";

					cmd.CommandText = sql;
					cmd.ExecuteNonQuery();

					// Delete any data from the temp tables. They should already be empty, since we just created them, but there
					// is the remote possibility that they already exist from a previous search during this request, so just to
					// be safe, let's delete the data.
					sql = @"
						DELETE FROM [tmpSearchTerms];
						DELETE FROM [tmpSearchResults];";

					cmd.CommandText = sql;
					cmd.ExecuteNonQuery();

					// 1b. Insert search terms into the search term temporary table.
					sql = @"INSERT INTO tmpSearchTerms (searchTerm) VALUES (@SearchTerm);";
					cmd.CommandText = sql;
					cmd.Parameters.Add("@SearchTerm", DbType.String);

					foreach (string searchTerm in searchTerms)
					{
						cmd.Parameters["@SearchTerm"].Value = String.Concat("%", searchTerm, "%");
						cmd.ExecuteNonQuery();
					}

					#endregion

					#region Search album title

					sql = @"
						INSERT INTO tmpSearchResults
						SELECT 'a', gs_Album.AlbumId, 'Album.Title', tmpSearchTerms.searchTerm
						FROM gs_Album, tmpSearchTerms
						WHERE (gs_Album.FKGalleryId = @GalleryId) AND (gs_Album.Title LIKE tmpSearchTerms.searchTerm)";

					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", galleryId);
					cmd.ExecuteNonQuery();

					#endregion

					#region Search media object title

					sql = @"
						INSERT INTO tmpSearchResults
						SELECT 'm', gs_MediaObject.MediaObjectId, 'MediaObject.Title', tmpSearchTerms.searchTerm
						 FROM gs_MediaObject INNER JOIN gs_Album
						 ON gs_Album.AlbumId = gs_MediaObject.FKAlbumId CROSS JOIN tmpSearchTerms
						 WHERE (gs_Album.FKGalleryId = @GalleryId) AND (gs_MediaObject.Title LIKE tmpSearchTerms.searchTerm)";

					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", galleryId);
					cmd.ExecuteNonQuery();

					#endregion

					#region Search media object original filename

					sql = @"
						INSERT INTO tmpSearchResults
						SELECT 'm', gs_MediaObject.MediaObjectId, 'MediaObject.OriginalFilename', tmpSearchTerms.searchTerm
						 FROM gs_MediaObject INNER JOIN gs_Album ON gs_Album.AlbumId =
						gs_MediaObject.FKAlbumId CROSS JOIN tmpSearchTerms
						 WHERE (gs_Album.FKGalleryId = @GalleryId) AND (gs_MediaObject.OriginalFilename LIKE tmpSearchTerms.searchTerm)";

					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", galleryId);
					cmd.ExecuteNonQuery();

					#endregion

					#region Search media object metadata

					sql = @"
						INSERT INTO tmpSearchResults
						SELECT DISTINCT 'm', gs_MediaObject.MediaObjectId, 'MediaObjectMetadata', tmpSearchTerms.searchTerm
						 FROM gs_MediaObjectMetadata INNER JOIN gs_MediaObject
						 ON gs_MediaObjectMetadata.FKMediaObjectId = gs_MediaObject.MediaObjectId
						INNER JOIN gs_Album
						 ON gs_Album.AlbumId = gs_MediaObject.FKAlbumId CROSS JOIN tmpSearchTerms
						 WHERE (gs_Album.FKGalleryId = @GalleryId) AND (gs_MediaObjectMetadata.Value LIKE tmpSearchTerms.searchTerm)";

					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", galleryId);
					cmd.ExecuteNonQuery();

					#endregion

					#region Retrieve search results from temporary table

					sql = @"
						SELECT sr.gotype, sr.id
						FROM tmpSearchTerms AS st INNER JOIN (SELECT DISTINCT gotype, id,
						matchingSearchTerm FROM tmpSearchResults) AS sr ON st.searchTerm =
						sr.matchingSearchTerm
						GROUP BY sr.gotype, sr.id
						HAVING (COUNT(*) >= (SELECT COUNT(*) FROM tmpSearchTerms));

						DROP TABLE [tmpSearchTerms];
						DROP TABLE [tmpSearchResults];";

					cmd.Parameters.Clear();
					cmd.CommandText = sql;

					using (IDataReader dr = cmd.ExecuteReader())
					{
						matchingAlbumIds = new List<int>();
						matchingMediaObjectIds = new List<int>();

						while (dr.Read())
						{
							string galleryObjectType = dr.GetString(0);
							if (galleryObjectType == "a")
							{
								matchingAlbumIds.Add(dr.GetInt32(1));
							}
							else if (galleryObjectType == "m")
							{
								matchingMediaObjectIds.Add(dr.GetInt32(1));
							}
							else
							{
								throw new DataException(String.Format(CultureInfo.CurrentCulture, "The first column returned by the SELECT statement in GalleryServerPro.Data.SQLite.SQLiteGalleryServerProProvider.SearchGallery must return 'a' (for album) or 'm' (for media object). Instead, it returned {0}.", galleryObjectType));
							}
						}
					}

					#endregion

					#region Cleanup

					// Commit the transaction if it's the one we created in this method.
					if (tran != null)
						tran.Commit();

					#endregion
				}
			}
			catch
			{
				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();

				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		/// <summary>
		/// Begins a new database transaction. All subsequent database actions occur within the context of this transaction.
		/// Use <see cref="CommitTransaction"/> to commit this transaction or <see cref="RollbackTransaction" /> to abort it. If a transaction
		/// is already in progress, then this method returns without any action, which preserves the original transaction.
		/// </summary>
		/// <remarks>Transactions are supported only when the client is a web application.This is because the 
		/// transaction is stored in the HTTP context Items property. If the client is not a web application, then 
		/// <see cref="System.Web.HttpContext.Current" /> is null. When this happens, this method returns without taking any action.</remarks>
		public override void BeginTransaction()
		{
			// Create new connection and transaction and place in HTTP context.
			if (System.Web.HttpContext.Current == null)
				return;

			if (IsTransactionInProgress())
				return;

			SQLiteConnection cn = GetDBConnectionForGallery();
			if (cn.State == ConnectionState.Closed)
				cn.Open();

			SQLiteTransaction tran = cn.BeginTransaction();

			System.Web.HttpContext.Current.Items[_httpTransactionId] = tran;
		}

		/// <summary>
		/// Commits the current transaction, if one exists. A transaction is created with the <see cref="BeginTransaction"/> method.
		/// If there is not an existing transaction, no action is taken. If this method is called when a datareader is open, the
		/// actual commit is delayed until all datareaders are disposed.
		/// </summary>
		/// <remarks>Transactions are supported only when the client is a web application.This is because the 
		/// transaction is stored in the HTTP context Items property. If the client is not a web application, then 
		/// <see cref="System.Web.HttpContext.Current" /> is null. When this happens, this method returns without taking any action.</remarks>
		public override void CommitTransaction()
		{
			// Look in HTTP context for previously created connection and transaction. Commit transaction.
			if (System.Web.HttpContext.Current == null)
				return;

			SQLiteTransaction tran = (SQLiteTransaction)System.Web.HttpContext.Current.Items[_httpTransactionId];
			if (tran == null)
				return;

			tran.Commit(); // This closes the connection and nulls out the Connection property on the transaction.

			System.Web.HttpContext.Current.Items.Remove(_httpTransactionId);
		}

		/// <summary>
		/// Aborts the current transaction, if one exists. A transaction is created with the <see cref="BeginTransaction"/> method.
		/// If there is not an existing transaction, no action is taken.
		/// </summary>
		/// <remarks>Transactions are supported only when the client is a web application.This is because the 
		/// transaction is stored in the HTTP context Items property. If the client is not a web application, then 
		/// <see cref="System.Web.HttpContext.Current" /> is null. When this happens, this method returns without taking any action.</remarks>
		public override void RollbackTransaction()
		{
			// Look in HTTP context for previously created connection and transaction. Abort transaction.
			if (System.Web.HttpContext.Current == null)
				return;

			SQLiteTransaction tran = (SQLiteTransaction)System.Web.HttpContext.Current.Items[_httpTransactionId];
			if (tran == null)
				return;

			tran.Rollback(); // This closes the connection and nulls out the Connection property on the transaction.

			System.Web.HttpContext.Current.Items.Remove(_httpTransactionId);
		}

		/// <summary>
		/// Imports the Gallery Server Pro data into the current database, overwriting any existing data. Does not import the actual media
		/// files; they must be imported manually with a utility such as Windows Explorer. This method makes changes only to the database tables;
		/// no files in the media objects directory are affected. If both the <paramref name="importMembershipData"/> and <paramref name="importGalleryData"/>
		/// parameters are false, then no action is taken.
		/// </summary>
		/// <param name="filePath">The full file path to the XML file containing the data (e.g. "D:\mybackups\GalleryServerBackup_2008-06-22_141336.xml").</param>
		/// <param name="importMembershipData">If set to <c>true</c>, user accounts and other membership data will be imported.
		/// Current membership data will first be deleted.</param>
		/// <param name="importGalleryData">If set to <c>true</c>, albums, media objects, and other gallery data will be imported.
		/// Current gallery data will first be deleted.</param>
		public override void ImportGalleryData(string filePath, bool importMembershipData, bool importGalleryData)
		{
			DataUtility.ImportData(filePath, importMembershipData, importGalleryData, _connectionString);
		}

		/// <summary>
		/// Exports the Gallery Server Pro data in the current database to an XML file at the location specified by the <paramref name="filePath"/>
		/// parameter. Does not export the actual media files; they must be copied manually with a utility such as Windows Explorer. This method
		/// does not make any changes to the database tables or the files in the media objects directory.
		/// </summary>
		/// <param name="filePath">The full file path specifying where the XML file will be written to. If a file with the same name already
		/// exists, it will first be deleted. Example: "D:\mybackups\GalleryServerBackup_2008-06-22_141336.xml".</param>
		/// <param name="exportMembershipData">If set to <c>true</c>, user accounts and other membership data will be exported.</param>
		/// <param name="exportGalleryData">If set to <c>true</c>, albums, media objects, and other gallery data will be exported.</param>
		public override void ExportGalleryData(string filePath, bool exportMembershipData, bool exportGalleryData)
		{
			DataUtility.ExportData(filePath, exportMembershipData, exportGalleryData, _connectionString);
		}

		/// <summary>
		/// Validates that the backup file specified in the <see cref="IBackupFile.FilePath"/> property of the <paramref name="backupFile"/>
		/// parameter is valid and populates the remaining properties with information about the file.
		/// </summary>
		/// <param name="backupFile">An instance of <see cref="IBackupFile"/> that with only the <see cref="IBackupFile.FilePath"/>
		/// property assigned. The remaining properties should be uninitialized since they will be assigned in this method.</param>
		public override void ValidateBackupFile(ref IBackupFile backupFile)
		{
			Util.ValidateBackupFile(ref backupFile);
		}

		#endregion

		#region Security Methods

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the roles in the current gallery. If no matching objects
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing the roles in the current gallery.
		/// </returns>
		public override System.Data.IDataReader Roles_GetDataReaderRoles()
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
	AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, 
	AllowDeleteMediaObject, AllowSynchronize, HideWatermark, AllowAdministerSite
FROM gs_Role
WHERE (FKGalleryId = @GalleryId)";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the root album IDs associated with the specified role name. If no matching data
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="roleName">The role name for which root album IDs should be returned.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing the root album IDs associated with the specified role name.
		/// </returns>
		public override System.Data.IDataReader Roles_GetDataReaderRoleRootAlbums(string roleName)
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
SELECT gs_Role_Album.FKAlbumId
FROM gs_Role_Album INNER JOIN gs_Album ON gs_Role_Album.FKAlbumId = gs_Album.AlbumId
WHERE (gs_Role_Album.FKRoleName = @RoleName) AND (gs_Album.FKGalleryId = @GalleryId);";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@RoleName", roleName);
				cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (IsTransactionInProgress())
					return cmd.ExecuteReader();
				else
					return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing all album IDs associated with the specified role name. If no matching data
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="roleName">The role name for which all album IDs should be returned.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing all album IDs associated with the specified role name.
		/// </returns>
		public override System.Data.IDataReader Roles_GetDataReaderRoleAllAlbums(string roleName)
		{
			return GetDataReaderRoleAllAlbums(roleName);
		}

		/// <summary>
		/// Persist this gallery server role to the data store. The list of top-level albums this role applies to, which is stored
		/// in the <see cref="IGalleryServerRole.RootAlbumIds"/> property, must also be saved. The data provider automatically
		/// repopulates the <see cref="IGalleryServerRole.AllAlbumIds"/> property.
		/// </summary>
		/// <param name="role">An instance of IGalleryServerRole to persist to the data store.</param>
		public override void Role_Save(IGalleryServerRole role)
		{
			SaveRole(role);
		}

		/// <summary>
		/// Permanently delete this gallery server role from the data store, including the list of role/album relationships
		/// associated with this role. This action cannot be undone.
		/// </summary>
		/// <param name="role">An instance of <see cref="IGalleryServerRole"/> to delete from the data store.</param>
		public override void Role_Delete(IGalleryServerRole role)
		{
			// Delete a gallery server role. This procedure only deletes it from the custom gallery server tables,
			// not the ASP.NET role membership table(s). The web application code that invokes this procedure also
			// uses the standard ASP.NET technique to delete the role from the membership table(s).
			// First delete the records from the role/album association table, then delete the role.
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = @"
DELETE FROM [gs_Role_Album]
WHERE FKRoleName = @RoleName;

DELETE FROM [gs_Role]
WHERE FKGalleryId = @GalleryId AND RoleName = @RoleName;";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@RoleName", role.RoleName);
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);

					if (cn.State == ConnectionState.Closed)
						cn.Open();

					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		#endregion

		#region Private Methods

		private static SQLiteCommand GetCommandAlbumUpdate(IAlbum album)
		{
			const string sql = @"
UPDATE [gs_Album]
SET
	AlbumParentId = @AlbumParentId,
	Title = @Title,
	DirectoryName = @DirectoryName,
	Summary = @Summary,
	ThumbnailMediaObjectId = @ThumbnailMediaObjectId,
	Seq = @Seq,
	DateStart = @DateStart,
	DateEnd = @DateEnd,
	LastModifiedBy = @LastModifiedBy,
	DateLastModified = @DateLastModified,
	OwnedBy = @OwnedBy,
	OwnerRoleName = @OwnerRoleName,
	IsPrivate = @IsPrivate
WHERE (AlbumId = @AlbumId);";
			SQLiteCommand cmd = new SQLiteCommand(sql, GetDBConnectionForGallery());

			cmd.Parameters.AddWithValue("@AlbumParentId", album.Parent.Id);
			cmd.Parameters.AddWithValue("@Title", album.Title);
			cmd.Parameters.AddWithValue("@DirectoryName", album.DirectoryName);
			cmd.Parameters.AddWithValue("@Summary", album.Summary);
			cmd.Parameters.AddWithValue("@ThumbnailMediaObjectId", album.Thumbnail.MediaObjectId);
			cmd.Parameters.AddWithValue("@Seq", album.Sequence);

			if (album.DateStart > DateTime.MinValue)
				cmd.Parameters.AddWithValue("@DateStart", album.DateStart);
			else
				cmd.Parameters.AddWithValue("@DateStart", DBNull.Value);

			if (album.DateEnd > DateTime.MinValue)
				cmd.Parameters.AddWithValue("@DateEnd", album.DateEnd);
			else
				cmd.Parameters.AddWithValue("@DateEnd", DBNull.Value);

			cmd.Parameters.AddWithValue("@LastModifiedBy", album.LastModifiedByUserName);
			cmd.Parameters.AddWithValue("@DateLastModified", album.DateLastModified);
			cmd.Parameters.AddWithValue("@OwnedBy", album.OwnerUserName);
			cmd.Parameters.AddWithValue("@OwnerRoleName", album.OwnerRoleName);
			cmd.Parameters.AddWithValue("@IsPrivate", album.IsPrivate);
			cmd.Parameters.AddWithValue("@AlbumId", album.Id);

			return cmd;
		}

		private static SQLiteCommand GetCommandAlbumInsert(IAlbum album)
		{
			const string sql = @"
INSERT INTO [gs_Album] (AlbumParentId, FKGalleryId, Title, DirectoryName, 
Summary, ThumbnailMediaObjectId, Seq, DateStart, DateEnd, 
CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, 
OwnerRoleName, IsPrivate)
VALUES (@AlbumParentId, @GalleryId, @Title, @DirectoryName, 
@Summary, @ThumbnailMediaObjectId, @Seq, @DateStart, @DateEnd, 
@CreatedBy, @DateAdded, @LastModifiedBy, @DateLastModified, @OwnedBy, 
@OwnerRoleName, @IsPrivate);SELECT last_insert_rowid();";
			SQLiteCommand cmd = new SQLiteCommand(sql, GetDBConnectionForGallery());

			cmd.Parameters.AddWithValue("@AlbumParentId", album.Parent.Id);
			cmd.Parameters.AddWithValue("@GalleryId", album.GalleryId);
			cmd.Parameters.AddWithValue("@Title", album.Title);
			cmd.Parameters.AddWithValue("@DirectoryName", album.DirectoryName);
			cmd.Parameters.AddWithValue("@Summary", album.Summary);
			cmd.Parameters.AddWithValue("@ThumbnailMediaObjectId", album.Thumbnail.MediaObjectId);
			cmd.Parameters.AddWithValue("@Seq", album.Sequence);

			if (album.DateStart > DateTime.MinValue)
				cmd.Parameters.AddWithValue("@DateStart", album.DateStart);
			else
				cmd.Parameters.AddWithValue("@DateStart", DBNull.Value);

			if (album.DateEnd > DateTime.MinValue)
				cmd.Parameters.AddWithValue("@DateEnd", album.DateEnd);
			else
				cmd.Parameters.AddWithValue("@DateEnd", DBNull.Value);

			cmd.Parameters.AddWithValue("@CreatedBy", album.CreatedByUserName);
			cmd.Parameters.AddWithValue("@DateAdded", album.DateAdded);
			cmd.Parameters.AddWithValue("@LastModifiedBy", album.LastModifiedByUserName);
			cmd.Parameters.AddWithValue("@DateLastModified", album.DateLastModified);
			cmd.Parameters.AddWithValue("@OwnedBy", album.OwnerUserName);
			cmd.Parameters.AddWithValue("@OwnerRoleName", album.OwnerRoleName);
			cmd.Parameters.AddWithValue("@IsPrivate", album.IsPrivate);

			return cmd;
		}

		private static void DeleteAlbum(IAlbum album)
		{
			/* Delete the specified album and its objects, including any child albums.  We delete all objects manually
			 * by building up a temporary table with the list of albums that must be deleted, and then finally deleting
			 * all related records in the gs_Album, gs_MediaObject, and gs_Role_Album tables.*/

			SQLiteConnection cn = GetDBConnectionForGallery();
			DbTransaction tran = null;

			try
			{
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (!IsTransactionInProgress())
				{
					tran = cn.BeginTransaction();
				}

				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					// First, create a table to hold this album ID and all child album IDs, then
					// insert the album into our temporary table.
					string sql = @"
CREATE TEMP TABLE [tmpAlbum] (aid INTEGER, apid INTEGER, processed INTEGER);

INSERT INTO [tmpAlbum] 
	SELECT AlbumId, AlbumParentId, 0 FROM [gs_Album] WHERE AlbumId = @AlbumId;";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@AlbumId", album.Id);
					cmd.ExecuteNonQuery();

					/* Set up a loop where we insert the children of the first album, and their children, and so on, until no 
						children are left. The end result is that the table is filled with info about the album and all his descendents.
						The processed field in tmpAlbum represents the # of levels from the bottom. Thus the records
						with the MAX processed value is the ID of the album passed to this function, and the records with the MIN level (should always be 1)
						represent the most distant descendents. */
					sql = @"
INSERT INTO [tmpAlbum]
	SELECT AlbumId, AlbumParentId, -1
	FROM [gs_Album] WHERE AlbumParentId IN (SELECT aid FROM [tmpAlbum] WHERE processed = 0);
UPDATE [tmpAlbum] SET processed = processed + 1;
SELECT EXISTS (SELECT * FROM [tmpAlbum] WHERE processed = 0);
";
					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					bool foundRecords;
					do
					{
						foundRecords = Convert.ToBoolean(cmd.ExecuteScalar());
					} while (foundRecords);

					/* At this point tmpAlbum contains info about the album and all its descendents. Delete all media objects 
					 * and roles associated with these albums, and then delete the albums.
					 * Only delete albums that are not the root album (apid <> 0). */

					sql = @"
	DELETE FROM [gs_MediaObject] WHERE FKAlbumId IN (SELECT aid FROM [tmpAlbum]);
	DELETE FROM [gs_Role_Album] WHERE FKAlbumId IN (SELECT aid FROM [tmpAlbum]);
	DELETE FROM [gs_Album] WHERE AlbumId IN (SELECT aid FROM [tmpAlbum] WHERE apid <> 0);
	DROP TABLE [tmpAlbum];
";
					cmd.Parameters.Clear();
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery();

					// Commit the transaction if it's the one we created in this method.
					if (tran != null)
						tran.Commit();
				}
			}
			catch
			{
				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
				{
					tran.Dispose();
				}

				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		private static SQLiteCommand GetCommandMediaObjectInsert(IGalleryObject mediaObject, SQLiteConnection cn)
		{
			const string sql = @"
INSERT INTO [gs_MediaObject] (HashKey, FKAlbumId, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
 ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
 OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Title, Seq, CreatedBy, 
 DateAdded, LastModifiedBy, DateLastModified, IsPrivate)
VALUES (@HashKey, @FKAlbumId, @ThumbnailFilename, @ThumbnailWidth, @ThumbnailHeight,
 @ThumbnailSizeKB, @OptimizedFilename, @OptimizedWidth, @OptimizedHeight, @OptimizedSizeKB,
 @OriginalFilename, @OriginalWidth, @OriginalHeight, @OriginalSizeKB, @ExternalHtmlSource, @ExternalType, @Title, @Seq, @CreatedBy, 
 @DateAdded, @LastModifiedBy, @DateLastModified, @IsPrivate);SELECT last_insert_rowid();";

			SQLiteCommand cmd = cn.CreateCommand();
			cmd.CommandText = sql;

			cmd.Parameters.AddWithValue("@HashKey", mediaObject.Hashkey);
			cmd.Parameters.AddWithValue("@FKAlbumId", mediaObject.Parent.Id);
			cmd.Parameters.AddWithValue("@ThumbnailFilename", mediaObject.Thumbnail.FileName);
			cmd.Parameters.AddWithValue("@ThumbnailWidth", mediaObject.Thumbnail.Width);
			cmd.Parameters.AddWithValue("@ThumbnailHeight", mediaObject.Thumbnail.Height);
			cmd.Parameters.AddWithValue("@ThumbnailSizeKB", mediaObject.Thumbnail.FileSizeKB);
			cmd.Parameters.AddWithValue("@OptimizedFilename", mediaObject.Optimized.FileName);
			cmd.Parameters.AddWithValue("@OptimizedWidth", mediaObject.Optimized.Width);
			cmd.Parameters.AddWithValue("@OptimizedHeight", mediaObject.Optimized.Height);
			cmd.Parameters.AddWithValue("@OptimizedSizeKB", mediaObject.Optimized.FileSizeKB);
			cmd.Parameters.AddWithValue("@OriginalFilename", mediaObject.Original.FileName);
			cmd.Parameters.AddWithValue("@OriginalWidth", mediaObject.Original.Width);
			cmd.Parameters.AddWithValue("@OriginalHeight", mediaObject.Original.Height);
			cmd.Parameters.AddWithValue("@OriginalSizeKB", mediaObject.Original.FileSizeKB);
			cmd.Parameters.AddWithValue("@ExternalHtmlSource", mediaObject.Original.ExternalHtmlSource);

			if (mediaObject.Original.ExternalType == GalleryServerPro.Business.MimeTypeCategory.NotSet)
				cmd.Parameters.AddWithValue("@ExternalType", String.Empty);
			else
				cmd.Parameters.AddWithValue("@ExternalType", mediaObject.Original.ExternalType.ToString());

			cmd.Parameters.AddWithValue("@Title", mediaObject.Title);
			cmd.Parameters.AddWithValue("@Seq", mediaObject.Sequence);
			cmd.Parameters.AddWithValue("@CreatedBy", mediaObject.CreatedByUserName);
			cmd.Parameters.AddWithValue("@DateAdded", mediaObject.DateAdded);
			cmd.Parameters.AddWithValue("@LastModifiedBy", mediaObject.LastModifiedByUserName);
			cmd.Parameters.AddWithValue("@DateLastModified", mediaObject.DateLastModified);
			cmd.Parameters.AddWithValue("@IsPrivate", mediaObject.IsPrivate);

			return cmd;
		}

		private static SQLiteCommand GetCommandMediaObjectUpdate(IGalleryObject mediaObject, SQLiteConnection cn)
		{
			const string sql = @"
UPDATE [gs_MediaObject]
SET HashKey = @HashKey, FKAlbumId = @FKAlbumId,
 ThumbnailFilename = @ThumbnailFilename, ThumbnailWidth = @ThumbnailWidth, 
 ThumbnailHeight = @ThumbnailHeight, ThumbnailSizeKB = @ThumbnailSizeKB,
 OptimizedFilename = @OptimizedFilename, OptimizedWidth = @OptimizedWidth,
 OptimizedHeight = @OptimizedHeight, OptimizedSizeKB = @OptimizedSizeKB, 
 OriginalFilename = @OriginalFilename, OriginalWidth = @OriginalWidth,
 OriginalHeight = @OriginalHeight, OriginalSizeKB = @OriginalSizeKB, 
 ExternalHtmlSource = @ExternalHtmlSource, ExternalType = @ExternalType,
 Title = @Title, Seq = @Seq, LastModifiedBy = @LastModifiedBy, 
 DateLastModified = @DateLastModified, IsPrivate = @IsPrivate
WHERE MediaObjectId = @MediaObjectId;";

			SQLiteCommand cmd = cn.CreateCommand();
			cmd.CommandText = sql;

			cmd.Parameters.AddWithValue("@HashKey", mediaObject.Hashkey);
			cmd.Parameters.AddWithValue("@FKAlbumId", mediaObject.Parent.Id);
			cmd.Parameters.AddWithValue("@ThumbnailFilename", mediaObject.Thumbnail.FileName);
			cmd.Parameters.AddWithValue("@ThumbnailWidth", mediaObject.Thumbnail.Width);
			cmd.Parameters.AddWithValue("@ThumbnailHeight", mediaObject.Thumbnail.Height);
			cmd.Parameters.AddWithValue("@ThumbnailSizeKB", mediaObject.Thumbnail.FileSizeKB);
			cmd.Parameters.AddWithValue("@OptimizedFilename", mediaObject.Optimized.FileName);
			cmd.Parameters.AddWithValue("@OptimizedWidth", mediaObject.Optimized.Width);
			cmd.Parameters.AddWithValue("@OptimizedHeight", mediaObject.Optimized.Height);
			cmd.Parameters.AddWithValue("@OptimizedSizeKB", mediaObject.Optimized.FileSizeKB);
			cmd.Parameters.AddWithValue("@OriginalFilename", mediaObject.Original.FileName);
			cmd.Parameters.AddWithValue("@OriginalWidth", mediaObject.Original.Width);
			cmd.Parameters.AddWithValue("@OriginalHeight", mediaObject.Original.Height);
			cmd.Parameters.AddWithValue("@OriginalSizeKB", mediaObject.Original.FileSizeKB);
			cmd.Parameters.AddWithValue("@ExternalHtmlSource", mediaObject.Original.ExternalHtmlSource);

			if (mediaObject.Original.ExternalType == GalleryServerPro.Business.MimeTypeCategory.NotSet)
				cmd.Parameters.AddWithValue("@ExternalType", String.Empty);
			else
				cmd.Parameters.AddWithValue("@ExternalType", mediaObject.Original.ExternalType.ToString());

			cmd.Parameters.AddWithValue("@Title", mediaObject.Title);
			cmd.Parameters.AddWithValue("@Seq", mediaObject.Sequence);
			cmd.Parameters.AddWithValue("@CreatedBy", mediaObject.CreatedByUserName);
			cmd.Parameters.AddWithValue("@DateAdded", mediaObject.DateAdded);
			cmd.Parameters.AddWithValue("@LastModifiedBy", mediaObject.LastModifiedByUserName);
			cmd.Parameters.AddWithValue("@DateLastModified", mediaObject.DateLastModified);
			cmd.Parameters.AddWithValue("@IsPrivate", mediaObject.IsPrivate);
			cmd.Parameters.AddWithValue("@MediaObjectId", mediaObject.Id);

			return cmd;
		}

		private void SaveRole(IGalleryServerRole role)
		{
			PersistRoleAlbumRelationshipsToDataStore(role);

			PersistRoleToDataStore(role);

			ReloadAllAlbumIds(role);
		}

		/// <summary>
		/// Save the list of root album IDs to the data store. The table gs_Role_Album contains one record for each role/album
		/// relationship. This procedure adds and deletes records as needed.
		/// </summary>
		/// <param name="role">The gallery server role containing the list of root Album IDs to persist to the data store.</param>
		private void PersistRoleAlbumRelationshipsToDataStore(IGalleryServerRole role)
		{
			// Step 1: Copy the list of root album IDs to a new list. We'll be removing items from the list as we process them,
			// so we don't want to mess with the actual list attached to the object.
			List<int> roleAlbumRelationshipsToPersist = new List<int>();
			foreach (int albumId in role.RootAlbumIds)
			{
				roleAlbumRelationshipsToPersist.Add(albumId);
			}


			// Step 2: Get a datareader containing a list of all root album IDs in the data store. The result set contains a single
			// column of integers named "FKAlbumId".
			List<int> roleAlbumRelationshipsToDelete = new List<int>();
			using (IDataReader dr = Roles_GetDataReaderRoleRootAlbums(role.RoleName))
			{
				// Step 3: Iterate through each role/album relationship that is stored in the data store. If it is in our list, then
				// remove it from the list (see step 5 why). If not, the user must have unchecked it so add it to a list of 
				// relationships to be deleted.
				while (dr.Read())
				{
					if (roleAlbumRelationshipsToPersist.Contains(dr.GetInt32(0)))
					{
						roleAlbumRelationshipsToPersist.Remove(dr.GetInt32(0));
					}
					else
					{
						roleAlbumRelationshipsToDelete.Add(dr.GetInt32(0));
					}
				}
			}

			SQLiteConnection cn = GetDBConnectionForGallery();
			DbTransaction tran = null;

			try
			{
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (!IsTransactionInProgress())
					tran = cn.BeginTransaction();

				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					// Step 4: Delete the records we accumulated in our list.
					const string sql = @"
DELETE FROM [gs_Role_Album]
WHERE FKRoleName = @RoleName AND FKAlbumId = @AlbumId;";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@RoleName", role.RoleName);
					cmd.Parameters.Add("@AlbumId", DbType.Int32);

					foreach (int albumId in roleAlbumRelationshipsToDelete)
					{
						cmd.Parameters["@AlbumId"].Value = albumId;
						cmd.ExecuteNonQuery();
					}

					// Step 5: Any items still left in the roleAlbumRelationshipsToPersist list must be new ones checked by the user. Add them.
					cmd.CommandText = @"
INSERT INTO [gs_Role_Album] (FKRoleName, FKAlbumId)
VALUES (@RoleName, @AlbumId);";

					foreach (int albumId in roleAlbumRelationshipsToPersist)
					{
						cmd.Parameters["@AlbumId"].Value = albumId;
						cmd.ExecuteNonQuery();
					}
				}

				// Commit the transaction if it's the one we created in this method.
				if (tran != null)
					tran.Commit();
			}
			catch
			{
				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();

				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		private static void PersistRoleToDataStore(IGalleryServerRole role)
		{
			// Update the existing role, or insert if it doesn't exist.
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = @"
INSERT OR REPLACE INTO [gs_Role] (FKGalleryId, RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
	AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, AllowDeleteMediaObject, 
	AllowSynchronize, HideWatermark, AllowAdministerSite)
VALUES (@GalleryId, @RoleName, @AllowViewAlbumsAndObjects, @AllowViewOriginalImage, @AllowAddChildAlbum,
	@AllowAddMediaObject, @AllowEditAlbum, @AllowEditMediaObject, @AllowDeleteChildAlbum, @AllowDeleteMediaObject, 
	@AllowSynchronize, @HideWatermark, @AllowAdministerSite);";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@GalleryId", ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId);
					cmd.Parameters.AddWithValue("@RoleName", role.RoleName);
					cmd.Parameters.AddWithValue("@AllowViewAlbumsAndObjects", role.AllowViewAlbumOrMediaObject);
					cmd.Parameters.AddWithValue("@AllowViewOriginalImage", role.AllowViewOriginalImage);
					cmd.Parameters.AddWithValue("@AllowAddChildAlbum", role.AllowAddChildAlbum);
					cmd.Parameters.AddWithValue("@AllowAddMediaObject", role.AllowAddMediaObject);
					cmd.Parameters.AddWithValue("@AllowEditAlbum", role.AllowEditAlbum);
					cmd.Parameters.AddWithValue("@AllowEditMediaObject", role.AllowEditMediaObject);
					cmd.Parameters.AddWithValue("@AllowDeleteChildAlbum", role.AllowDeleteChildAlbum);
					cmd.Parameters.AddWithValue("@AllowDeleteMediaObject", role.AllowDeleteMediaObject);
					cmd.Parameters.AddWithValue("@AllowSynchronize", role.AllowSynchronize);
					cmd.Parameters.AddWithValue("@HideWatermark", role.HideWatermark);
					cmd.Parameters.AddWithValue("@AllowAdministerSite", role.AllowAdministerSite);

					if (cn.State == ConnectionState.Closed)
						cn.Open();
					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		private static void ReloadAllAlbumIds(IGalleryServerRole role)
		{
			role.ClearAllAlbumIds();

			using (IDataReader dr = GetDataReaderRoleAllAlbums(role.RoleName))
			{
				while (dr.Read())
				{
					role.AddToAllAlbumIds(dr.GetInt32(0));
				}
				dr.Close();
			}
		}

		private static IDataReader GetDataReaderRoleAllAlbums(string roleName)
		{
			// Retrieve all the album IDs that are affected by the specified role name. The album IDs that are stored in
			// the gs_Role_Album table only hold the highest ranking album ID. If the role is applied to the root album, 
			// then we can just return all albums in the gallery. If not, we need to drill down and retrieve all
			// the children. Follow these basic steps:
			// 1. Find out if the role applies to the root album. If it does, return all album ID's for the current gallery. 
			//    Otherwise, continue with next step.
			// 2. Create a temporary table to hold our working set of data.
			// 3. Insert the top level album IDs.
			// 4. Continue drilling down, level by level, until we reach a level where there are no more child albums.
			// 5. Retrieve the list of album IDs from the temporary table.
			DbTransaction tran = null;
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (!IsTransactionInProgress())
					tran = cn.BeginTransaction();

				int galleryId = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					string sql = "SELECT EXISTS (SELECT * FROM gs_Role_Album ra INNER JOIN gs_Album a ON ra.FKAlbumId = a.AlbumId WHERE (a.AlbumParentId = 0) AND (ra.FKRoleName = @RoleName ) AND (a.FKGalleryId = @GalleryId))";
					cmd.CommandText = sql;
					cmd.Parameters.AddWithValue("@RoleName", roleName);
					cmd.Parameters.AddWithValue("@GalleryId", galleryId);

					bool roleAffectsAllAlbums = Convert.ToBoolean(cmd.ExecuteScalar());

					if (roleAffectsAllAlbums)
					{
						// The role affects all albums, so we can just get a list of all albums rather than drill down album by album.
						sql = "SELECT AlbumId FROM gs_Album WHERE FKGalleryId = @GalleryId";
						cmd.Parameters.Clear();
						cmd.CommandText = sql;
						cmd.Parameters.AddWithValue("@GalleryId", galleryId);

						if (IsTransactionInProgress())
							return cmd.ExecuteReader();
						else
						{
							// Commit the transaction if it's the one we created in this method.
							if (tran != null)
								tran.Commit();
							return cmd.ExecuteReader(CommandBehavior.CloseConnection);
						}
					}
					else
					{
						// The role applies to an album or albums below the root album, so we need to drill down and retrieve all
						// the children. Start by creating a temporary table to hold our data.
						string tempTableName = "tmp" + Guid.NewGuid().ToString().Replace("-", String.Empty);
						sql = String.Format(@"
CREATE TEMP TABLE {0} (
[AlbumId] INTEGER NOT NULL,
[AlbumParentId] INTEGER NOT NULL,
[AlbumDepth] INTEGER NOT NULL);

INSERT INTO {0} (AlbumId, AlbumParentId, AlbumDepth)
SELECT FKAlbumId, 0, 1
FROM gs_Role_Album INNER JOIN gs_Album ON gs_Role_Album.FKAlbumId = gs_Album.AlbumId
WHERE (gs_Role_Album.FKRoleName = @RoleName) AND (gs_Album.FKGalleryId = @GalleryId);", tempTableName);
						cmd.Parameters.Clear();
						cmd.CommandText = sql;
						cmd.Parameters.AddWithValue("@RoleName", roleName);
						cmd.Parameters.AddWithValue("@GalleryId", galleryId);
						cmd.ExecuteNonQuery();

						sql = String.Format(@"
INSERT INTO {0} (AlbumId, AlbumParentId, AlbumDepth)
SELECT a.AlbumId, a.AlbumParentId, al.AlbumDepth + 1
FROM gs_Album a JOIN {0} al ON a.AlbumParentId = al.AlbumId
WHERE al.AlbumDepth = (SELECT MAX(AlbumDepth) FROM {0})
", tempTableName);
						cmd.Parameters.Clear();
						cmd.CommandText = sql;
						int numRows;
						do
						{
							numRows = cmd.ExecuteNonQuery();
						} while (numRows > 0);

						sql = String.Format("SELECT AlbumId FROM {0};", tempTableName);
						cmd.CommandText = sql;

						if (IsTransactionInProgress())
							return cmd.ExecuteReader();
						else
						{
							// Commit the transaction if it's the one we created in this method.
							if (tran != null)
								tran.Commit();
							return cmd.ExecuteReader(CommandBehavior.CloseConnection);
						}
					}
				}
			}
			catch
			{
				if (tran != null)
					tran.Rollback();
				throw;
			}
			finally
			{
				if (tran != null)
					tran.Dispose();
			}
		}

		/// <summary>
		/// Insert all metadata items from the data store for the specified media object. Assumes no existing metadata record exists
		/// that matches the MediaObjectMetadataId value of each metadata item. Each metadata item is inserted and the newly
		/// assigned MediaObjectMetadataId value is assigned to the item's MediaObjectMetadataId property.
		/// </summary>
		/// <param name="mediaObject">The media object for which to insert all metadata items to the data store.</param>
		/// <param name="cn">An open database connection.</param>
		private static void InsertMetadataItems(IGalleryObject mediaObject, SQLiteConnection cn)
		{
			// Insert meta data items, if any, into MediaObjectMetadata table.
			if (mediaObject.MetadataItems.Count > 0)
			{
				const string sql = @"
INSERT INTO [gs_MediaObjectMetadata] (FKMediaObjectId, MetadataNameIdentifier, Description, Value)
VALUES (@FKMediaObjectId, @MetadataNameIdentifier, @Description, @Value);SELECT last_insert_rowid();";

				using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
				{
					cmd.Parameters.AddWithValue("FKMediaObjectId", mediaObject.Id);
					cmd.Parameters.Add("@MetadataNameIdentifier", DbType.Int32);
					cmd.Parameters.Add("@Description", DbType.String);
					cmd.Parameters.Add("@Value", DbType.String);

					foreach (IGalleryObjectMetadataItem metaDataItem in mediaObject.MetadataItems)
					{
						cmd.Parameters["@MetadataNameIdentifier"].Value = (int)metaDataItem.MetadataItemName;
						cmd.Parameters["@Description"].Value = metaDataItem.Description;
						cmd.Parameters["@Value"].Value = metaDataItem.Value;

						// Assign newly assigned ID to the metadata ID property.
						metaDataItem.MediaObjectMetadataId = Convert.ToInt32(cmd.ExecuteScalar(), NumberFormatInfo.InvariantInfo);
					}
				}
			}
		}

		/// <summary>
		/// Persist each each metadata item that has HasChanges = true to the data store. If all items are marked for updating
		/// (mediaObject.RegenerateMetadataOnSave = true), then all metadata items are deleted from the data store and then inserted based
		/// on the current metadata items. If one or more items has HasChanges = false, then each item with HasChanges = true is
		/// processed according to the following rules: (1) If the metadata value is null or an empty string, it is deleted from the
		/// data store and removed from the MetadataItems collection. (2) If the item's MediaObjectMetadataId = int.MinValue, the
		/// item is assumed to be new and is inserted. (3) Any item not falling into the previous two categories, but HasChanges = true,
		/// is assumed to be pre-existing and an update stored procedure is executed.
		/// </summary>
		/// <param name="mediaObject">The media object for which to update metadata items in the data store.</param>
		/// <param name="cn">An open database connection.</param>
		private static void UpdateMetadataItems(IGalleryObject mediaObject, SQLiteConnection cn)
		{
			if (mediaObject.RegenerateMetadataOnSave)
			{
				// User wants to replace all metadata items. Delete them all from the data store, then insert the ones we have.
				DeleteMetadataItems(mediaObject, cn);

				InsertMetadataItems(mediaObject, cn);
			}
			else
			{
				IGalleryObjectMetadataItemCollection metadataItemsToSave = mediaObject.MetadataItems.GetItemsToSave();
				if (metadataItemsToSave.Count == 0)
				{
					return; // Nothing to save
				}

				// There is at least one item to persist to the data store.
				SQLiteCommand cmdUpdate = null;
				SQLiteCommand cmdInsert = null;
				try
				{
					cmdUpdate = GetCommandMediaObjectMetadataUpdate(cn);
					cmdUpdate.Parameters["@FKMediaObjectId"].Value = mediaObject.Id;

					cmdInsert = GetCommandMediaObjectMetadataInsert(cn);
					cmdInsert.Parameters["@FKMediaObjectId"].Value = mediaObject.Id;

					foreach (IGalleryObjectMetadataItem metaDataItem in metadataItemsToSave)
					{
						if (String.IsNullOrEmpty(metaDataItem.Value))
						{
							// There is no value, so let's delete this item.
							DeleteMetadataItem(metaDataItem, cn);

							// Remove it from the collection.
							mediaObject.MetadataItems.Remove(metaDataItem);
						}
						else if (metaDataItem.MediaObjectMetadataId == int.MinValue)
						{
							// Insert the item.
							cmdInsert.Parameters["@MetadataNameIdentifier"].Value = (int)metaDataItem.MetadataItemName;
							cmdInsert.Parameters["@Description"].Value = metaDataItem.Description;
							cmdInsert.Parameters["@Value"].Value = metaDataItem.Value;

							// Assign newly assigned ID to the metadata ID property.
							metaDataItem.MediaObjectMetadataId = Convert.ToInt32(cmdInsert.ExecuteScalar(), NumberFormatInfo.InvariantInfo);
						}
						else
						{
							// Update the item.
							cmdUpdate.Parameters["@MetadataNameIdentifier"].Value = (int)metaDataItem.MetadataItemName;
							cmdUpdate.Parameters["@Description"].Value = metaDataItem.Description;
							cmdUpdate.Parameters["@Value"].Value = metaDataItem.Value;
							cmdUpdate.Parameters["@MediaObjectMetadataId"].Value = metaDataItem.MediaObjectMetadataId;

							cmdUpdate.ExecuteNonQuery();
						}
					}
				}
				finally
				{
					if (cmdUpdate != null)
						cmdUpdate.Dispose();

					if (cmdInsert != null)
						cmdInsert.Dispose();
				}
			}
		}

		/// <summary>
		/// Delete the specified metadata item from the data store. No error occurs if the record does not exist in the data store.
		/// </summary>
		/// <param name="metaDataItem">The metadata item to delete from the data store.</param>
		/// <param name="cn">An open database connection.</param>
		private static void DeleteMetadataItem(IGalleryObjectMetadataItem metaDataItem, SQLiteConnection cn)
		{
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
DELETE [gs_MediaObjectMetadata]
WHERE MediaObjectMetadataId = @MediaObjectMetadataId;";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@MediaObjectMetadataId", metaDataItem.MediaObjectMetadataId);
				cmd.ExecuteNonQuery();
			}
		}

		private static SQLiteCommand GetCommandMediaObjectMetadataUpdate(SQLiteConnection cn)
		{
			const string sql = @"
UPDATE [gs_MediaObjectMetadata]
SET FKMediaObjectId = @FKMediaObjectId,
 MetadataNameIdentifier = @MetadataNameIdentifier,
 Description = @Description,
 Value = @Value
WHERE MediaObjectMetadataId = @MediaObjectMetadataId;";
			SQLiteCommand cmd = new SQLiteCommand(sql, cn);

			cmd.Parameters.Add("@FKMediaObjectId", DbType.Int32);
			cmd.Parameters.Add("@MetadataNameIdentifier", DbType.Int32);
			cmd.Parameters.Add("@Description", DbType.String);
			cmd.Parameters.Add("@Value", DbType.String);
			cmd.Parameters.Add("@MediaObjectMetadataId", DbType.Int32);

			return cmd;
		}

		private static SQLiteCommand GetCommandMediaObjectMetadataInsert(SQLiteConnection cn)
		{
			const string sql = @"
INSERT INTO [gs_MediaObjectMetadata] (FKMediaObjectId, MetadataNameIdentifier, Description, Value)
VALUES (@FKMediaObjectId, @MetadataNameIdentifier, @Description, @Value);SELECT last_insert_rowid();";
			SQLiteCommand cmd = new SQLiteCommand(sql, cn);

			cmd.Parameters.Add("@FKMediaObjectId", DbType.Int32);
			cmd.Parameters.Add("@MetadataNameIdentifier", DbType.Int32);
			cmd.Parameters.Add("@Description", DbType.String);
			cmd.Parameters.Add("@Value", DbType.String);

			return cmd;
		}

		private static void DeleteMetadataItems(IGalleryObject mediaObject, SQLiteConnection cn)
		{
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
DELETE FROM [gs_MediaObjectMetadata]
WHERE FKMediaObjectId = @MediaObjectId;";
				cmd.CommandText = sql;
				cmd.Parameters.AddWithValue("@MediaObjectId", mediaObject.Id);
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Perform any needed data store operations to get Gallery Server ready to run. This includes verifying the 
		/// database has the minimum default records in certain tables. If no records are configured for the current
		/// gallery ID, they are created as necessary. Thus, this method can be used to create a new gallery.
		/// The gallery ID that is used is the one specified in the config file (galleryserverpro.config).
		/// </summary>
		private static void Initialize()
		{
			// This SQL performs the following tasks:
			// 1. Verifies that the Gallery table has a record to represent the current gallery.
			// 2. Verifies that the Album table has a record to represent the root album.
			// 3. Verifies that the Synchronize table has a record for the current gallery. This table stores the current state of a synchronization,
			//	if one is in progress. When a synchronization is not in progress, the SynchState field should be zero	for this gallery.

			string sql = string.Format(@"
				INSERT OR IGNORE INTO gs_Gallery (GalleryId, Description, DateAdded)
				VALUES (@GalleryId, 'My Gallery', datetime('now'));

				INSERT INTO gs_Album (AlbumParentId, FKGalleryId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, Seq, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate)
				SELECT 0, @GalleryId, '{0}', '','{1}', 0, 0, 'System', datetime('now'), 'System', datetime('now'), '', '', '0'
				WHERE NOT EXISTS (SELECT * FROM [gs_Album] WHERE AlbumParentId = 0 AND FKGalleryId = @GalleryId);

				DELETE FROM gs_Synchronize WHERE FKGalleryId = @GalleryId;

				INSERT INTO gs_Synchronize (SynchId, FKGalleryId, SynchState, TotalFiles, CurrentFileIndex)
				VALUES ('', @GalleryId, 0, 0, 0);

				DELETE FROM gs_SchemaVersion;

				INSERT OR IGNORE INTO gs_SchemaVersion (SchemaVersion)
				VALUES ('{2}');
				",
				 Resources.Root_Album_Default_Title, // 0
				 Resources.Root_Album_Default_Summary, // 1
				 Util.ConvertGalleryDataSchemaVersionToString(_databaseSchemaVersion)); // 2

			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					cmd.CommandText = sql;
					cmd.Parameters.Add("@GalleryId", DbType.Int32).Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;
					if (cn.State == ConnectionState.Closed)
						cn.Open();

					cmd.ExecuteNonQuery();
				}
			}
			finally
			{
				if (IsTransactionInProgress())
					cn.Dispose();
			}

		}

		/// <summary>
		/// Get a reference to the database connection used for gallery data. If a transaction is currently in progress, and the
		/// connection string of the transaction connection is the same as the connection string for the Gallery Data provider,
		/// then the connection associated with the transaction is returned, and it will already be open. If no transaction is in progress,
		/// a new <see cref="SQLiteConnection"/> is created and returned. It will be closed and must be opened by the caller
		/// before using.
		/// </summary>
		/// <returns>A <see cref="SQLiteConnection"/> object.</returns>
		/// <remarks>The transaction is stored in <see cref="System.Web.HttpContext.Current"/>. That means transaction support is limited
		/// to web applications. For other types of applications, there is no transaction support unless this code is modified.</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		private static SQLiteConnection GetDBConnectionForGallery()
		{
			// Look in the HTTP context bag for a previously created connection and transaction. Return if found and its connection
			// string matches that of the gallery connection string; otherwise return a fresh connection.
			if (System.Web.HttpContext.Current != null)
			{
				SQLiteTransaction tran = (SQLiteTransaction)System.Web.HttpContext.Current.Items[_httpTransactionId];
				if ((tran != null) && (String.Equals(tran.Connection.ConnectionString, _connectionString)))
					return tran.Connection;
			}

			return new SQLiteConnection(_connectionString);
		}

		/// <summary>
		/// Determines whether a database transaction is in progress for the Gallery data provider.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if a database transaction is in progress; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>A transaction is considered in progress if an instance of <see cref="SQLiteTransaction"/> is found in the
		/// <see cref="System.Web.HttpContext.Current"/> Items property and its connection string is equal to the Gallery data 
		/// provider's connection string. Note that this implementation of <see cref="SQLiteGalleryServerProProvider"/> adds a 
		/// <see cref="SQLiteTransaction"/> to <see cref="System.Web.HttpContext.Current"/> in the <see cref="BeginTransaction"/>
		/// method. If an instance of <see cref="System.Web.HttpContext.Current"/> does not exist (for example, if the calling 
		/// application is not a web application), this method always returns false.</remarks>
		private static bool IsTransactionInProgress()
		{
			if (System.Web.HttpContext.Current == null)
				return false;

			SQLiteTransaction tran = (SQLiteTransaction)System.Web.HttpContext.Current.Items[_httpTransactionId];

			if ((tran != null) && (String.Equals(tran.Connection.ConnectionString, _connectionString)))
				return true;
			else
				return false;
		}

		private static string GetDataSchemaVersionString()
		{
			SQLiteConnection cn = GetDBConnectionForGallery();
			try
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					const string sql = @"SELECT SchemaVersion FROM gs_SchemaVersion";
					cmd.CommandText = sql;

					if (cn.State == ConnectionState.Closed)
						cn.Open();

					return cmd.ExecuteScalar().ToString();
				}
			}
			finally
			{
				if (!IsTransactionInProgress())
					cn.Dispose();
			}
		}

		/// <summary>
		/// Check the current version of the database schema, upgrading if necessary. This function is useful when the administrator
		/// upgrades Gallery Server Pro to a newer version which requires a database upgrade. This function executes the
		/// necessary SQL script to upgrade the database. If the version required by this provider does not match the database version, 
		/// and the database cannot be upgraded to the desired version, this function logs a message to the error log and returns
		/// without taking any action.
		/// </summary>
		private static void VerifySchemaVersion()
		{
			const GalleryDataSchemaVersion requiredDataSchemaVersion = _databaseSchemaVersion;
			GalleryDataSchemaVersion dataSchemaVersionOfDb = Util.ConvertGalleryDataSchemaVersionToEnum(GetDataSchemaVersionString());

			if (requiredDataSchemaVersion == dataSchemaVersionOfDb)
				return;

			if (dataSchemaVersionOfDb == GalleryDataSchemaVersion.Unknown)
			{
				string msg = String.Format("The database structure has a version ({0}) that is not one of the recognized schema versions included in a Gallery Server Pro release. Because of this, Gallery Server Pro cannot determine whether or how to upgrade the data schema, so it will not make an attempt. This is an information message only and does not necessarily represent a problem. This version of Gallery Server Pro is designed to work with data schema version {1}.", GetDataSchemaVersionString(), Util.ConvertGalleryDataSchemaVersionToString(requiredDataSchemaVersion));

				ErrorHandler.CustomExceptions.DataException ex = new ErrorHandler.CustomExceptions.DataException(msg);
				try { Error.Record(ex); } catch { }
				return;
			}

			if (requiredDataSchemaVersion < dataSchemaVersionOfDb)
			{
				string msg = String.Format("The database structure is a more recent version ({0}) than the application is designed for {1}. Gallery Server Pro will attempt to ignore this difference, and hopefully it will not cause an issue.", GetDataSchemaVersionString(), Util.ConvertGalleryDataSchemaVersionToString(requiredDataSchemaVersion));

				ErrorHandler.CustomExceptions.DataException ex = new ErrorHandler.CustomExceptions.DataException(msg);
				try { Error.Record(ex); } catch { }
				return;
			}

			switch (dataSchemaVersionOfDb)
			{
				case GalleryDataSchemaVersion.V2_1_3162:
					if (requiredDataSchemaVersion == GalleryDataSchemaVersion.V2_3_3421)
					{
						ExecuteSqlUpgrade(GalleryDataSchemaUpgradeScript.SqlUpgrade_2_1_3162_to_2_3_3421);
					}
					break;
					default:
						string msg = String.Format("The database structure cannot be upgraded from version {0} to version {1}. This is an information message only and does not necessarily represent a problem.", GetDataSchemaVersionString(), Util.ConvertGalleryDataSchemaVersionToString(requiredDataSchemaVersion));

						ErrorHandler.CustomExceptions.DataException ex = new ErrorHandler.CustomExceptions.DataException(msg);
						try { Error.Record(ex); } catch {}
					break;
			}
		}

		/// <summary>
		/// Executes the SQL script represented by the specified <paramref name="script"/>. The actual SQL file is stored as an
		/// embedded resource in the current assembly.
		/// </summary>
		/// <param name="script">The script to execute. This value is used to lookup the SQL file stored as an embedded resource
		/// in the current assembly.</param>
		private static void ExecuteSqlUpgrade(GalleryDataSchemaUpgradeScript script)
		{
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			using (System.IO.Stream stream = asm.GetManifestResourceStream(String.Format("GalleryServerPro.Data.SQLite.{0}.sql", script)))
			{
				ExecuteSqlInStream(stream);
			}
		}

		/// <summary>
		/// Execute the SQL statements in the specified stream.
		/// </summary>
		/// <param name="stream">A stream containing a series of SQL statements separated by the word GO.</param>
		/// <returns>Returns true if the SQL executed without error; otherwise returns false.</returns>
		/// <remarks>This function is copied from the install wizard in the web project.</remarks>
		private static void ExecuteSqlInStream(System.IO.Stream stream)
		{
			const int timeout = 600; // Timeout for SQL Execution (seconds)
			System.IO.StreamReader sr = null;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			try
			{
				sr = new System.IO.StreamReader(stream);
				using (SQLiteConnection cn = GetDBConnectionForGallery())
				{
					cn.Open();

					while (!sr.EndOfStream)
					{
						if (sb.Length > 0) sb.Remove(0, sb.Length); // Clear out string builder

						using (SQLiteCommand cmd = cn.CreateCommand())
						{
							while (!sr.EndOfStream)
							{
								string s = sr.ReadLine();
								if (s != null && s.ToUpperInvariant().Trim().Equals("GO"))
								{
									break;
								}

								sb.AppendLine(s);
							}

							// Execute T-SQL against the target database
							cmd.CommandText = sb.ToString();
							cmd.CommandTimeout = timeout;

							cmd.ExecuteNonQuery();
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (!ex.Data.Contains(_exSql))
				{
					ex.Data.Add(_exSql, sb.ToString());
				}
				throw;
			}
			finally
			{
				if (sr != null)
					sr.Close();
			}
		}

		#endregion
	}
}

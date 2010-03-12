using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Provides functionality for retrieving and persisting information to the SQL Server data store.
	/// </summary>
	public class SqlDataProvider : Provider.DataProvider
	{
		#region Private Fields

		// This variable should reference the current version of the database schema required by this provider.
		// During InitializeDataStore, this value is compared against the schema version stored in the database
		// The database is upgraded if appropriate.
		private const GalleryDataSchemaVersion _databaseSchemaVersion = GalleryDataSchemaVersion.V2_3_3421;

		private static readonly object _sharedLock = new object();
		
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
			SqlUpgrade_2_1_3162_to_2_3_3421_SqlServer2000 = 0,
			SqlUpgrade_2_1_3162_to_2_3_3421_SqlServer2005 = 1
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
		/// Gets the name of the connection string.
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

		#region Internal methods

		/// <summary>
		/// Get a reference to an unopened database connection.
		/// </summary>
		/// <returns>A SqlConnection object.</returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		internal static SqlConnection GetDbConnection()
		{
			return new SqlConnection(_connectionString);
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
				name = "SqlServerGalleryServerProProvider";

			if (String.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "SQL Server gallery data provider");
			}

			// Initialize the abstract base class.
			base.Initialize(name, config);

			if (config["applicationName"] == null || config["applicationName"].Trim() == "")
			{
				_applicationName = String.Empty; // If we had a reference to System.Web, we could use HostingEnvironment.ApplicationVirtualPath
			}
			else
			{
				_applicationName = config["applicationName"];
			}

			// Get connection string.
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

			if (connectionStringSettings == null || connectionStringSettings.ConnectionString.Trim() == "")
			{
				throw new ProviderException("Connection string cannot be blank.");
			}

			_connectionStringName = connectionStringSettings.Name;
			_connectionString = connectionStringSettings.ConnectionString;

			// Get the SQL Server schema.
			Util.SqlServerSchema = ConfigurationManager.AppSettings["SqlServerSchema"] ?? "dbo.";
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
			using (System.Data.IDataReader dr = GetDataReaderGallerySelect())
			{
				while (dr.Read())
				{
					gallery.Id = Convert.ToInt32(dr["GalleryId"]);
					gallery.Description = dr["Description"].ToString();
					gallery.CreationDate = Convert.ToDateTime(dr["DateAdded"]);
					break;
				}
			}

			return gallery;
		}

		private static System.Data.IDataReader GetDataReaderGallerySelect()
		{
			System.Data.IDataReader dr = GetCommandGallerySelect().ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		private static SqlCommand GetCommandGallerySelect()
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_GallerySelect"), GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
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
			return Album.Save(album);
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
			return Album.GetDataReaderChildAlbumsById(albumId);
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
			return Album.GetDataReaderChildGalleryObjectsById(albumId);
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
			return Album.GetDataReaderAlbumById(albumId);
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
			return Album.GetDataReaderRootAlbum(galleryId);
		}

		/// <summary>
		/// Permanently delete the specified album from the data store, including any
		/// child albums and media objects (cascading delete). This action cannot be undone.
		/// </summary>
		/// <param name="album">The <see cref="IAlbum"/> to delete from the data store.</param>
		public override void Album_Delete(IAlbum album)
		{
			Album.Delete(album);
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
			return MediaObject.Save(mediaObject);
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
			return MediaObject.GetDataReaderMediaObjectById(mediaObjectId);
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
			return MediaObject.GetDataReaderMetadataItemsByMediaObjectId(mediaObjectId);
		}

		/// <summary>
		/// Permanently delete the specified media object from the data store. This action cannot
		/// be undone.
		/// </summary>
		/// <param name="mediaObject">The <see cref="IGalleryObject"/> to delete from the data store.</param>
		public override void MediaObject_Delete(IGalleryObject mediaObject)
		{
			MediaObject.Delete(mediaObject);
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the hash keys for all media objects in the specified gallery.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with one field named "HashKey" containing the hash keys
		/// for all media objects in the current gallery.
		/// </returns>
		public override System.Data.IDataReader  MediaObject_GetAllHashKeys(int galleryId)
		{
			return MediaObject.GetHashKeys(galleryId);
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
			Synchronize.SaveStatus(synchStatus);
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
			return Synchronize.UpdateStatusFromDataStore(synchStatus);
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

			Application.Initialize();
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
			Application.SearchGallery(searchTerms, out matchingAlbumIds, out matchingMediaObjectIds);
		}

		/// <summary>
		/// Begins a new database transaction. All subsequent database actions occur within the context of this transaction.
		/// Use <see cref="CommitTransaction"/> to commit this transaction or RollbackTransaction to abort it. If a transaction
		/// is already in progress, then this method returns without any action, which preserves the original transaction.
		/// <note type="implementnotes">The SQL Server provider does not currently implement this method.</note>
		/// </summary>
		public override void BeginTransaction()
		{
		}

		/// <summary>
		/// Commits the current transaction, if one exists. A transaction is created with the <see cref="BeginTransaction"/> method.
		/// If there is not an existing transaction, no action is taken. If this method is called when a datareader is open, the
		/// actual commit is delayed until all datareaders are disposed.
		/// <note type="implementnotes">The SQL Server provider does not currently implement this method.</note>
		/// </summary>
		public override void CommitTransaction()
		{
		}

		/// <summary>
		/// Aborts the current transaction, if one exists. A transaction is created with the <see cref="BeginTransaction"/> method.
		/// If there is not an existing transaction, no action is taken.
		/// <note type="implementnotes">The SQL Server provider does not currently implement this method.</note>
		/// </summary>
		public override void RollbackTransaction()
		{
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
			DataUtility.ImportData(filePath, importMembershipData, importGalleryData);
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
			DataUtility.ExportData(filePath, exportMembershipData, exportGalleryData);
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
			return Role.GetDataReaderRoles();
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
			return Role.GetDataReaderRoleRootAlbums(roleName);
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
			return Role.GetDataReaderRoleAllAlbums(roleName);
		}

		/// <summary>
		/// Persist this gallery server role to the data store. The list of top-level albums this role applies to, which is stored
		/// in the <see cref="IGalleryServerRole.RootAlbumIds"/> property, must also be saved. The data provider automatically
		/// repopulates the <see cref="IGalleryServerRole.AllAlbumIds"/> property.
		/// </summary>
		/// <param name="role">An instance of IGalleryServerRole to persist to the data store.</param>
		public override void Role_Save(IGalleryServerRole role)
		{
			Role.Save(role);
		}

		/// <summary>
		/// Permanently delete this gallery server role from the data store, including the list of role/album relationships
		/// associated with this role. This action cannot be undone.
		/// </summary>
		/// <param name="role">An instance of <see cref="IGalleryServerRole"/> to delete from the data store.</param>
		public override void Role_Delete(IGalleryServerRole role)
		{
			Role.Delete(role);
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
		public override System.Data.IDataReader AppError_GetAppErrors()
		{
			return Error.GetDataReaderAppErrors();
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
			return Error.Save(appError);
		}

		/// <summary>
		/// Delete the application error from the data store.
		/// </summary>
		/// <param name="appErrorId">The value that uniquely identifies this application error (<see cref="IAppError.AppErrorId"/>).</param>
		public override void AppError_Delete(int appErrorId)
		{
			Error.Delete(appErrorId);
		}

		/// <summary>
		/// Permanently delete all errors from the data store.
		/// </summary>
		public override void AppError_ClearLog()
		{
			Error.DeleteAll();
		}

		#endregion


		#region Private Methods

		private static string GetDataSchemaVersionString()
		{
			using (SqlConnection cn = GetDbConnection())
			{
				using (SqlCommand cmd = cn.CreateCommand())
				{
					string sql = String.Concat("SELECT ", Util.GetSqlName("gs_GetVersion"), "() AS SchemaVersion");
					cmd.CommandText = sql;

					if (cn.State == ConnectionState.Closed)
						cn.Open();

					return cmd.ExecuteScalar().ToString();
				}
			}
		}

		/// <summary>
		/// Check the current version of the database schema, upgrading if necessary. This function is useful when the administrator
		/// upgrades Gallery Server Pro to a newer version which requires a database upgrade. This is the function that executes the
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
				try { ErrorHandler.Error.Record(ex); } catch { }
				return;
			}

			if (requiredDataSchemaVersion < dataSchemaVersionOfDb)
			{
				string msg = String.Format("The database structure is a more recent version ({0}) than the application is designed for {1}. Gallery Server Pro will attempt to ignore this difference, and hopefully it will not cause an issue.", GetDataSchemaVersionString(), Util.ConvertGalleryDataSchemaVersionToString(requiredDataSchemaVersion));

				ErrorHandler.CustomExceptions.DataException ex = new ErrorHandler.CustomExceptions.DataException(msg);
				try { ErrorHandler.Error.Record(ex); } catch { }
				return;
			}

			switch (dataSchemaVersionOfDb)
			{
				case GalleryDataSchemaVersion.V2_1_3162:
					if (requiredDataSchemaVersion == GalleryDataSchemaVersion.V2_3_3421)
					{
						if (Util.GetSqlVersion() <= SqlVersion.Sql2000)
						{
							ExecuteSqlUpgrade(GalleryDataSchemaUpgradeScript.SqlUpgrade_2_1_3162_to_2_3_3421_SqlServer2000);
						}
						else
						{
							ExecuteSqlUpgrade(GalleryDataSchemaUpgradeScript.SqlUpgrade_2_1_3162_to_2_3_3421_SqlServer2005);
						}
					}
					break;
				default:
					string msg = String.Format("The database structure cannot be upgraded from version {0} to version {1}. This is an information message only and does not necessarily represent a problem.", GetDataSchemaVersionString(), Util.ConvertGalleryDataSchemaVersionToString(requiredDataSchemaVersion));

					ErrorHandler.CustomExceptions.DataException ex = new ErrorHandler.CustomExceptions.DataException(msg);
					try { ErrorHandler.Error.Record(ex); } catch { }
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
			string scriptLocation = String.Format("GalleryServerPro.Data.SqlServer.{0}.sql", script);
			using (System.IO.Stream stream = asm.GetManifestResourceStream(scriptLocation))
			{
				if (stream == null)
					throw new ArgumentException(String.Format("Unable to find embedded resource file named {0}", scriptLocation));

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
				using (SqlConnection cn = GetDbConnection())
				{
					cn.Open();

					while (!sr.EndOfStream)
					{
						if (sb.Length > 0) sb.Remove(0, sb.Length); // Clear out string builder

						using (SqlCommand cmd = cn.CreateCommand())
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

using System;
using System.Data;
using System.Data.SqlClient;

using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Contains functionality for persisting / retrieving album information to / from the SQL Server data store.
	/// </summary>
  internal class Album
  {
		private Album() { }

		#region Public Static Methods

		/// <summary>
		/// Persist the specified album to the data store. Return the ID of the album.
		/// </summary>
		/// <param name="album">An instance of <see cref="IAlbum" /> to persist to the data store.</param>
		/// <returns>Return the ID of the album. If this is a new album and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.</returns>
		public static int Save(IAlbum album)
		{
			PersistToDataStore(album);

			return album.Id;
		}

		/// <summary>
		/// Permanently delete the specified album from the data store, including any
		/// child albums and media objects (cascading delete). This action cannot be undone.
		/// </summary>
		/// <param name="album">The <see cref="IAlbum" /> to delete from the data store.</param>
		public static void Delete(IAlbum album)
		{
			DeleteFromDataStore(album);
		}

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the album for the specified albumId. If no matching object
		/// is found in the data store, an empty <see cref="IDataReader" /> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the desired album.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object with all album fields.</returns>
		public static IDataReader GetDataReaderAlbumById(int albumId)
		{
			IDataReader dr = GetCommandAlbumSelectById(albumId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the top-level album in the gallery. This method is guaranteed
		/// to return an instance with one record representing the top-level album, since a default root
		/// album is created if one does not exist.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object with all album fields.</returns>
		public static IDataReader GetDataReaderRootAlbum(int galleryId)
		{
			IDataReader dr = GetCommandRootAlbum(galleryId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the child gallery objects contained within the album specfified by
		/// albumId parameter. If no matching objects are found in the data store, an empty IDataReader is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the desired album.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object containing all relevant fields for the gallery objects.</returns>
		public static IDataReader GetDataReaderChildGalleryObjectsById(int albumId)
		{
			IDataReader dr = GetCommandChildMediaObjectsById(albumId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the child albums contained within the album specified by
		/// albumId parameter. If no matching objects are found in the data store, an empty <see cref="IDataReader" /> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the album for which to return the child albums
		/// contained within.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object containing a field named ID that contains the IDs of all albums
		/// directly within the album represented by albumId.</returns>
		public static IDataReader GetDataReaderChildAlbumsById(int albumId)
		{
			IDataReader dr = GetCommandChildAlbumsById(albumId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		#endregion

		#region Private Static Methods

		private static void DeleteFromDataStore(IAlbum album)
		{
			SqlCommand cmd = GetCommandAlbumDelete(album.Id);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			//int rv = Convert.ToInt32(cmd.Parameters["@RV"].Value, System.Globalization.NumberFormatInfo.CurrentInfo);
			cmd.Connection.Close();

			//if (rv != 0)
			//  throw new ApplicationException("Error in stored procedure DeleteAlbum");
		}

		private static void PersistToDataStore(IAlbum album)
		{
			if (album.IsNew)
			{
				SqlCommand cmd = GetCommandAlbumInsert(album);
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
				cmd.Connection.Close();

				int ID = Convert.ToInt32(cmd.Parameters["@Identity"].Value, System.Globalization.NumberFormatInfo.CurrentInfo);

				if (album.Id != ID)
					album.Id = ID;
			}
			else
			{
				SqlCommand cmd = GetCommandAlbumUpdate(album);
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
				cmd.Connection.Close();
			}
		}

		#region SqlCommand Factory Methods

		private static SqlCommand GetCommandAlbumInsert(IAlbum album)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AlbumInsert"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@AlbumParentId", SqlDbType.Int, 0, "AlbumParentId"));
			cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, dataStoreConfig.AlbumTitleLength, "Title"));
			cmd.Parameters.Add(new SqlParameter("@DirectoryName", SqlDbType.NVarChar, dataStoreConfig.AlbumDirectoryNameLength, "DirectoryName"));
			cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.NVarChar, dataStoreConfig.AlbumSummaryLength, "Summary"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailMediaObjectId", SqlDbType.Int, 0, "ThumbnailMediaObjectId"));
			cmd.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int, 0, "Seq"));
			cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.DateTime, 0, "DateStart"));
			cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.DateTime, 0, "DateEnd"));
			cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, dataStoreConfig.CreatedByLength, "CreatedBy"));
			cmd.Parameters.Add(new SqlParameter("@DateAdded", SqlDbType.DateTime, 0, "DateAdded"));
			cmd.Parameters.Add(new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, dataStoreConfig.LastModifiedByLength, "LastModifiedBy"));
			cmd.Parameters.Add(new SqlParameter("@DateLastModified", SqlDbType.DateTime, 0, "DateLastModified"));
			cmd.Parameters.Add(new SqlParameter("@OwnedBy", SqlDbType.NVarChar, dataStoreConfig.OwnedByLength, "OwnedBy"));
			cmd.Parameters.Add(new SqlParameter("@OwnerRoleName", SqlDbType.NVarChar, dataStoreConfig.OwnerRoleNameLength, "OwnerRoleName"));
			cmd.Parameters.Add(new SqlParameter("@IsPrivate", SqlDbType.Bit, 0, "IsPrivate"));
			SqlParameter prm = new SqlParameter("@Identity", SqlDbType.Int, 0, "AlbumId");
			prm.Direction = ParameterDirection.Output;
			cmd.Parameters.Add(prm);

			cmd.Parameters["@GalleryId"].Value = album.GalleryId;
			cmd.Parameters["@AlbumParentId"].Value = album.Parent.Id;
			cmd.Parameters["@Title"].Value = album.Title;
			cmd.Parameters["@DirectoryName"].Value = album.DirectoryName;
			cmd.Parameters["@Summary"].Value = album.Summary;
			cmd.Parameters["@ThumbnailMediaObjectId"].Value = album.Thumbnail.MediaObjectId;
			cmd.Parameters["@Seq"].Value = album.Sequence;

			if (album.DateStart > DateTime.MinValue)
				cmd.Parameters["@DateStart"].Value = album.DateStart;
			else
				cmd.Parameters["@DateStart"].Value = DBNull.Value;

			if (album.DateEnd > DateTime.MinValue)
				cmd.Parameters["@DateEnd"].Value = album.DateEnd;
			else
				cmd.Parameters["@DateEnd"].Value = DBNull.Value;

			cmd.Parameters["@CreatedBy"].Value = album.CreatedByUserName;
			cmd.Parameters["@DateAdded"].Value = album.DateAdded;
			cmd.Parameters["@LastModifiedBy"].Value = album.LastModifiedByUserName;
			cmd.Parameters["@DateLastModified"].Value = album.DateLastModified;
			cmd.Parameters["@OwnedBy"].Value = album.OwnerUserName;
			cmd.Parameters["@OwnerRoleName"].Value = album.OwnerRoleName;
			cmd.Parameters["@IsPrivate"].Value = album.IsPrivate;

			return cmd;
		}

		private static SqlCommand GetCommandAlbumUpdate(IAlbum album)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AlbumUpdate"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int, 0, "AlbumId"));
			cmd.Parameters.Add(new SqlParameter("@AlbumParentId", SqlDbType.Int, 0, "AlbumParentId"));
			cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, dataStoreConfig.AlbumTitleLength, "Title"));
			cmd.Parameters.Add(new SqlParameter("@DirectoryName", SqlDbType.NVarChar, dataStoreConfig.AlbumDirectoryNameLength, "DirectoryName"));
			cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.NVarChar, dataStoreConfig.AlbumSummaryLength, "Summary"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailMediaObjectId", SqlDbType.Int, 0, "ThumbnailMediaObjectId"));
			cmd.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int, 0, "Seq"));
			cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.DateTime, 0, "DateStart"));
			cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.DateTime, 0, "DateEnd"));
			cmd.Parameters.Add(new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, dataStoreConfig.LastModifiedByLength, "LastModifiedBy"));
			cmd.Parameters.Add(new SqlParameter("@DateLastModified", SqlDbType.DateTime, 0, "DateLastModified"));
			cmd.Parameters.Add(new SqlParameter("@OwnedBy", SqlDbType.NVarChar, dataStoreConfig.OwnedByLength, "OwnedBy"));
			cmd.Parameters.Add(new SqlParameter("@OwnerRoleName", SqlDbType.NVarChar, dataStoreConfig.OwnerRoleNameLength, "OwnerRoleName"));
			cmd.Parameters.Add(new SqlParameter("@IsPrivate", SqlDbType.Bit, 0, "IsPrivate"));

			cmd.Parameters["@AlbumId"].Value = album.Id;
			cmd.Parameters["@AlbumParentId"].Value = album.Parent.Id;
			cmd.Parameters["@Title"].Value = album.Title;
			cmd.Parameters["@DirectoryName"].Value = album.DirectoryName;
			cmd.Parameters["@Summary"].Value = album.Summary;
			cmd.Parameters["@ThumbnailMediaObjectId"].Value = album.ThumbnailMediaObjectId;
			cmd.Parameters["@Seq"].Value = album.Sequence;

			if (album.DateStart > DateTime.MinValue)
				cmd.Parameters["@DateStart"].Value = album.DateStart;
			else
				cmd.Parameters["@DateStart"].Value = DBNull.Value;

			if (album.DateEnd > DateTime.MinValue)
				cmd.Parameters["@DateEnd"].Value = album.DateEnd;
			else
				cmd.Parameters["@DateEnd"].Value = DBNull.Value;

			cmd.Parameters["@LastModifiedBy"].Value = album.LastModifiedByUserName;
			cmd.Parameters["@DateLastModified"].Value = album.DateLastModified;
			cmd.Parameters["@OwnedBy"].Value = album.OwnerUserName;
			cmd.Parameters["@OwnerRoleName"].Value = album.OwnerRoleName;
			cmd.Parameters["@IsPrivate"].Value = album.IsPrivate;

			return cmd;
		}

		private static SqlCommand GetCommandAlbumDelete(int albumId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AlbumDelete"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int, 0, "AlbumId"));
			cmd.Parameters["@AlbumId"].Value = albumId;
			//SqlParameter prm = cmd.Parameters.Add("@RV", SqlDbType.Int);
			//prm.Direction = ParameterDirection.ReturnValue;

			return cmd;
		}

		private static SqlCommand GetCommandAlbumSelectById(int albumId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AlbumSelect"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int));

			cmd.Parameters["@AlbumId"].Value = albumId;

			cmd.Connection.Open();

			return cmd;
		}
		
		private static SqlCommand GetCommandChildMediaObjectsById(int albumId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_SelectChildMediaObjects"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int));
			cmd.Parameters["@AlbumId"].Value = albumId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandChildAlbumsById(int albumId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_SelectChildAlbums"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters["@AlbumId"].Value = albumId;
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandRootAlbum(int galleryId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_SelectRootAlbum"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters["@GalleryId"].Value = galleryId;
			
			cmd.Connection.Open();

			return cmd;
		}
		
		#endregion 

		#endregion  
  }
}

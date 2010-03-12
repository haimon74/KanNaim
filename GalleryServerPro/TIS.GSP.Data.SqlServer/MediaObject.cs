using System;
using System.Data;
using System.Data.SqlClient;

using GalleryServerPro.Configuration;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Contains functionality for persisting / retrieving media object information to / from the SQL Server data store.
	/// </summary>
	internal class MediaObject
	{
		private MediaObject() { }

		#region Public Static Methods

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the media object for the specified mediaObjectId. If no matching object
		/// is found in the data store, an empty <see cref="IDataReader" /> is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the desired media object.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object with all media object fields.</returns>
		public static IDataReader GetDataReaderMediaObjectById(int mediaObjectId)
		{
			IDataReader dr = GetCommandMediaObjectSelectById(mediaObjectId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the metadata items for the specified mediaObjectId. If no matching object
		/// is found in the data store, an empty <see cref="IDataReader" /> is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the desired media object.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object with all metadata items.</returns>
		public static IDataReader GetDataReaderMetadataItemsByMediaObjectId(int mediaObjectId)
		{
			IDataReader dr = GetCommandMediaObjectMetadataSelectByMediaObjectId(mediaObjectId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Persist the specified media object to the data store. Return the ID of the media object.
		/// </summary>
		/// <param name="mediaObject">An instance of <see cref="IGalleryObject" /> to persist to the data store.</param>
		/// <returns>Return the ID of the media object. If this is a new media object and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.</returns>
		public static int Save(IGalleryObject mediaObject)
		{
			int mediaObjectId = mediaObject.Id;

			if (mediaObject.IsNew)
			{
				// Insert new record into MediaObject table.
				SqlCommand cmd = GetCommandMediaObjectInsert(mediaObject);
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
				cmd.Connection.Close();

				mediaObjectId = Convert.ToInt32(cmd.Parameters["@Identity"].Value, System.Globalization.NumberFormatInfo.InvariantInfo);

				if (mediaObject.Id != mediaObjectId)
				{
					mediaObject.Id = mediaObjectId;
				}

				// Insert metadata items, if any, into MediaObjectMetadata table.
				InsertMetadataItems(mediaObject);
			}
			else
			{
				SqlCommand cmd = GetCommandMediaObjectUpdate(mediaObject);
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
				cmd.Connection.Close();

				// Update metadata items, if necessary, in MediaObjectMetadata table.
				UpdateMetadataItems(mediaObject);
			}

			return mediaObjectId;
		}

		/// <summary>
		/// Permanently delete the specified media object from the data store. This action cannot
		/// be undone. This action also deletes the related metadata items.
		/// </summary>
		/// <param name="mediaObject">The <see cref="IGalleryObject" /> to delete from the data store.</param>
		public static void Delete(IGalleryObject mediaObject)
		{
			// Related metadata items in the MediaObjectMetadataItem table are deleted
			// via a cascade delete rule configured between this table and the MediaObject table.
			SqlCommand cmd = GetCommandMediaObjectDelete(mediaObject.Id);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the hash keys for all media objects in the specified gallery.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>Returns an <see cref="IDataReader" /> object with one field named "HashKey" containing the hash keys
		/// for all media objects in the current gallery.</returns>
		public static IDataReader GetHashKeys(int galleryId)
		{
			IDataReader dr = GetCommandMediaObjectSelectHashKeys(galleryId).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}
		
		#endregion

		#region Private Static Methods

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
		private static void UpdateMetadataItems(IGalleryObject mediaObject)
		{
			if (mediaObject.RegenerateMetadataOnSave)
			{
				// User wants to replace all metadata items. Delete them all from the data store, then insert the ones we have.
				DeleteMetadataItems(mediaObject);

				InsertMetadataItems(mediaObject);
			}
			else
			{
				IGalleryObjectMetadataItemCollection metadataItemsToSave = mediaObject.MetadataItems.GetItemsToSave();
				if (metadataItemsToSave.Count == 0)
				{
					return; // Nothing to save
				}

				// There is at least one item to persist to the data store.
				SqlCommand cmdUpdate = GetCommandMediaObjectMetadataUpdate();
				cmdUpdate.Parameters["@FKMediaObjectId"].Value = mediaObject.Id;

				SqlCommand cmdInsert = GetCommandMediaObjectMetadataInsert();
				cmdInsert.Parameters["@FKMediaObjectId"].Value = mediaObject.Id;

				cmdUpdate.Connection.Open();
				cmdInsert.Connection.Open();

				foreach (IGalleryObjectMetadataItem metaDataItem in metadataItemsToSave)
				{
					if (String.IsNullOrEmpty(metaDataItem.Value))
					{
						// There is no value, so let's delete this item.
						DeleteMetadataItem(metaDataItem);

						// Remove it from the collection.
						mediaObject.MetadataItems.Remove(metaDataItem);
					}
					else if (metaDataItem.MediaObjectMetadataId == int.MinValue)
					{
						// Insert the item.
						cmdInsert.Parameters["@MetadataNameIdentifier"].Value = (int) metaDataItem.MetadataItemName;
						cmdInsert.Parameters["@Description"].Value = metaDataItem.Description;
						cmdInsert.Parameters["@Value"].Value = metaDataItem.Value;

						cmdInsert.ExecuteNonQuery();

						// Assign newly assigned ID to the metadata ID property.
						metaDataItem.MediaObjectMetadataId = Convert.ToInt32(cmdInsert.Parameters["@Identity"].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
					}
					else
					{
						// Update the item.
						cmdUpdate.Parameters["@MetadataNameIdentifier"].Value = (int) metaDataItem.MetadataItemName;
						cmdUpdate.Parameters["@Description"].Value = metaDataItem.Description;
						cmdUpdate.Parameters["@Value"].Value = metaDataItem.Value;
						cmdUpdate.Parameters["@MediaObjectMetadataId"].Value = metaDataItem.MediaObjectMetadataId;

						cmdUpdate.ExecuteNonQuery();
					}
				}
				cmdUpdate.Connection.Close();
				cmdInsert.Connection.Close();
			}
		}

		/// <summary>
		/// Delete all metadata items from the data store for the specified media object.
		/// </summary>
		/// <param name="mediaObject">The media object for which to delete all metadata items from the data store.</param>
		private static void DeleteMetadataItems(IGalleryObject mediaObject)
		{
			SqlCommand cmd = GetCommandMediaObjectMetadataDeleteByMediaObjectId();
			cmd.Parameters["@MediaObjectId"].Value = mediaObject.Id;
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		/// <summary>
		/// Insert all metadata items from the data store for the specified media object. Assumes no existing metadata record exists
		/// that matches the MediaObjectMetadataId value of each metadata item. Each metadata item is inserted and the newly 
		/// assigned MediaObjectMetadataId value is assigned to the item's MediaObjectMetadataId property.
		/// </summary>
		/// <param name="mediaObject">The media object for which to insert all metadata items to the data store.</param>
		private static void InsertMetadataItems(IGalleryObject mediaObject)
		{
			// Insert meta data items, if any, into MediaObjectMetadata table.
			if (mediaObject.MetadataItems.Count > 0)
			{
				SqlCommand cmd = GetCommandMediaObjectMetadataInsert();
				cmd.Parameters["@FKMediaObjectId"].Value = mediaObject.Id;
				cmd.Connection.Open();

				foreach (IGalleryObjectMetadataItem metaDataItem in mediaObject.MetadataItems)
				{
					cmd.Parameters["@MetadataNameIdentifier"].Value = (int) metaDataItem.MetadataItemName;
					cmd.Parameters["@Description"].Value = metaDataItem.Description;
					cmd.Parameters["@Value"].Value = metaDataItem.Value;

					cmd.ExecuteNonQuery();

					// Assign newly assigned ID to the metadata ID property.
					metaDataItem.MediaObjectMetadataId = Convert.ToInt32(cmd.Parameters["@Identity"].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
				}
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Delete the specified metadata item from the data store. No error occurs if the record does not exist in the data store.
		/// </summary>
		/// <param name="metaDataItem">The metadata item to delete from the data store.</param>
		private static void DeleteMetadataItem(IGalleryObjectMetadataItem metaDataItem)
		{
			SqlCommand cmd = GetCommandMediaObjectMetadataDelete();
			cmd.Parameters["@MediaObjectMetadataId"].Value = metaDataItem.MediaObjectMetadataId;
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		#region Methods to generate SqlCommand objects

		private static SqlCommand GetCommandMediaObjectSelectHashKeys(int galleryId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectSelectHashKeys"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@GalleryId"].Value = galleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectSelectById(int mediaObjectId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectSelect"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@MediaObjectId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@MediaObjectId"].Value = mediaObjectId;
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectMetadataSelectByMediaObjectId(int mediaObjectId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectMetadataSelect"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@MediaObjectId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@MediaObjectId"].Value = mediaObjectId;
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectInsert(IGalleryObject mediaObject)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectInsert"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@HashKey", SqlDbType.Char, dataStoreConfig.MediaObjectHashKeyLength, "HashKey"));
			cmd.Parameters.Add(new SqlParameter("@FKAlbumId", SqlDbType.Int, 0, "FKAlbumId"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailFilename", SqlDbType.NVarChar, dataStoreConfig.MediaObjectFileNameLength, "ThumbnailFilename"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailWidth", SqlDbType.Int, 0, "ThumbnailWidth"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailHeight", SqlDbType.Int, 0, "ThumbnailHeight"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailSizeKB", SqlDbType.Int, 0, "ThumbnailSizeKB"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedFilename", SqlDbType.NVarChar, dataStoreConfig.MediaObjectFileNameLength, "OptimizedFilename"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedWidth", SqlDbType.Int, 0, "OptimizedWidth"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedHeight", SqlDbType.Int, 0, "OptimizedHeight"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedSizeKB", SqlDbType.Int, 0, "OptimizedSizeKB"));
			cmd.Parameters.Add(new SqlParameter("@OriginalFilename", SqlDbType.NVarChar, dataStoreConfig.MediaObjectFileNameLength, "OriginalFilename"));
			cmd.Parameters.Add(new SqlParameter("@OriginalWidth", SqlDbType.Int, 0, "OriginalWidth"));
			cmd.Parameters.Add(new SqlParameter("@OriginalHeight", SqlDbType.Int, 0, "OriginalHeight"));
			cmd.Parameters.Add(new SqlParameter("@OriginalSizeKB", SqlDbType.Int, 0, "OriginalSizeKB"));
			cmd.Parameters.Add(new SqlParameter("@ExternalHtmlSource", SqlDbType.NVarChar, dataStoreConfig.MediaObjectExternalHtmlSourceLength, "ExternalHtmlSource"));
			cmd.Parameters.Add(new SqlParameter("@ExternalType", SqlDbType.NVarChar, dataStoreConfig.MediaObjectExternalTypeLength, "ExternalType"));
			cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, dataStoreConfig.MediaObjectTitleLength, "Title"));
			cmd.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int, 0, "Seq"));
			cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, dataStoreConfig.CreatedByLength, "CreatedBy"));
			cmd.Parameters.Add(new SqlParameter("@DateAdded", SqlDbType.DateTime, 0, "DateAdded"));
			cmd.Parameters.Add(new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, dataStoreConfig.LastModifiedByLength, "LastModifiedBy"));
			cmd.Parameters.Add(new SqlParameter("@DateLastModified", SqlDbType.DateTime, 0, "DateLastModified"));
			cmd.Parameters.Add(new SqlParameter("@IsPrivate", SqlDbType.Bit, 0, "IsPrivate"));
			SqlParameter prm = cmd.Parameters.Add(new SqlParameter("@Identity", SqlDbType.Int, 0, "MOID"));
			prm.Direction = ParameterDirection.Output;

			cmd.Parameters["@HashKey"].Value = mediaObject.Hashkey;
			cmd.Parameters["@FKAlbumId"].Value = mediaObject.Parent.Id;
			cmd.Parameters["@ThumbnailFilename"].Value = mediaObject.Thumbnail.FileName;
			cmd.Parameters["@ThumbnailWidth"].Value = mediaObject.Thumbnail.Width;
			cmd.Parameters["@ThumbnailHeight"].Value = mediaObject.Thumbnail.Height;
			cmd.Parameters["@ThumbnailSizeKB"].Value = mediaObject.Thumbnail.FileSizeKB;
			cmd.Parameters["@OptimizedFilename"].Value = mediaObject.Optimized.FileName;
			cmd.Parameters["@OptimizedWidth"].Value = mediaObject.Optimized.Width;
			cmd.Parameters["@OptimizedHeight"].Value = mediaObject.Optimized.Height;
			cmd.Parameters["@OptimizedSizeKB"].Value = mediaObject.Optimized.FileSizeKB;
			cmd.Parameters["@OriginalFilename"].Value = mediaObject.Original.FileName;
			cmd.Parameters["@OriginalWidth"].Value = mediaObject.Original.Width;
			cmd.Parameters["@OriginalHeight"].Value = mediaObject.Original.Height;
			cmd.Parameters["@OriginalSizeKB"].Value = mediaObject.Original.FileSizeKB;
			cmd.Parameters["@ExternalHtmlSource"].Value = mediaObject.Original.ExternalHtmlSource;
			cmd.Parameters["@ExternalType"].Value = mediaObject.Original.ExternalType;
			cmd.Parameters["@Title"].Value = mediaObject.Title;
			cmd.Parameters["@Seq"].Value = mediaObject.Sequence;
			cmd.Parameters["@CreatedBy"].Value = mediaObject.CreatedByUserName;
			cmd.Parameters["@DateAdded"].Value = mediaObject.DateAdded;
			cmd.Parameters["@LastModifiedBy"].Value = mediaObject.LastModifiedByUserName;
			cmd.Parameters["@DateLastModified"].Value = mediaObject.DateLastModified;
			cmd.Parameters["@IsPrivate"].Value = mediaObject.IsPrivate;

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectUpdate(IGalleryObject mediaObject)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectUpdate"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@MediaObjectId", SqlDbType.Int, 0, "MediaObjectId"));
			cmd.Parameters.Add(new SqlParameter("@HashKey", SqlDbType.Char, dataStoreConfig.MediaObjectHashKeyLength, "HashKey"));
			cmd.Parameters.Add(new SqlParameter("@FKAlbumId", SqlDbType.Int, 0, "FKAlbumId"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailFilename", SqlDbType.NVarChar, dataStoreConfig.MediaObjectFileNameLength, "ThumbnailFilename"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailWidth", SqlDbType.Int, 0, "ThumbnailWidth"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailHeight", SqlDbType.Int, 0, "ThumbnailHeight"));
			cmd.Parameters.Add(new SqlParameter("@ThumbnailSizeKB", SqlDbType.Int, 0, "ThumbnailSizeKB"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedFilename", SqlDbType.NVarChar, dataStoreConfig.MediaObjectFileNameLength, "OptimizedFilename"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedWidth", SqlDbType.Int, 0, "OptimizedWidth"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedHeight", SqlDbType.Int, 0, "OptimizedHeight"));
			cmd.Parameters.Add(new SqlParameter("@OptimizedSizeKB", SqlDbType.Int, 0, "OptimizedSizeKB"));
			cmd.Parameters.Add(new SqlParameter("@OriginalFilename", SqlDbType.NVarChar, dataStoreConfig.MediaObjectFileNameLength, "OriginalFilename"));
			cmd.Parameters.Add(new SqlParameter("@OriginalWidth", SqlDbType.Int, 0, "OriginalWidth"));
			cmd.Parameters.Add(new SqlParameter("@OriginalHeight", SqlDbType.Int, 0, "OriginalHeight"));
			cmd.Parameters.Add(new SqlParameter("@OriginalSizeKB", SqlDbType.Int, 0, "OriginalSizeKB"));
			cmd.Parameters.Add(new SqlParameter("@ExternalHtmlSource", SqlDbType.NVarChar, dataStoreConfig.MediaObjectExternalHtmlSourceLength, "ExternalHtmlSource"));
			cmd.Parameters.Add(new SqlParameter("@ExternalType", SqlDbType.NVarChar, dataStoreConfig.MediaObjectExternalTypeLength, "ExternalType"));
			cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, dataStoreConfig.MediaObjectTitleLength, "Title"));
			cmd.Parameters.Add(new SqlParameter("@Seq", SqlDbType.Int, 0, "Seq"));
			cmd.Parameters.Add(new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, dataStoreConfig.LastModifiedByLength, "LastModifiedBy"));
			cmd.Parameters.Add(new SqlParameter("@DateLastModified", SqlDbType.DateTime, 0, "DateLastModified"));
			cmd.Parameters.Add(new SqlParameter("@IsPrivate", SqlDbType.Bit, 0, "IsPrivate"));
			// Not specifying CreatedBy or DateAdded because those should only get populated during the INSERT.

			cmd.Parameters["@MediaObjectId"].Value = mediaObject.Id;
			cmd.Parameters["@HashKey"].Value = mediaObject.Hashkey;
			cmd.Parameters["@FKAlbumId"].Value = mediaObject.Parent.Id;
			cmd.Parameters["@ThumbnailFilename"].Value = mediaObject.Thumbnail.FileName;
			cmd.Parameters["@ThumbnailWidth"].Value = mediaObject.Thumbnail.Width;
			cmd.Parameters["@ThumbnailHeight"].Value = mediaObject.Thumbnail.Height;
			cmd.Parameters["@ThumbnailSizeKB"].Value = mediaObject.Thumbnail.FileSizeKB;
			cmd.Parameters["@OptimizedFilename"].Value = mediaObject.Optimized.FileName;
			cmd.Parameters["@OptimizedWidth"].Value = mediaObject.Optimized.Width;
			cmd.Parameters["@OptimizedHeight"].Value = mediaObject.Optimized.Height;
			cmd.Parameters["@OptimizedSizeKB"].Value = mediaObject.Optimized.FileSizeKB;
			cmd.Parameters["@OriginalFilename"].Value = mediaObject.Original.FileName;
			cmd.Parameters["@OriginalWidth"].Value = mediaObject.Original.Width;
			cmd.Parameters["@OriginalHeight"].Value = mediaObject.Original.Height;
			cmd.Parameters["@OriginalSizeKB"].Value = mediaObject.Original.FileSizeKB;
			cmd.Parameters["@ExternalHtmlSource"].Value = mediaObject.Original.ExternalHtmlSource;
			cmd.Parameters["@ExternalType"].Value = mediaObject.Original.ExternalType;
			cmd.Parameters["@Title"].Value = mediaObject.Title;
			cmd.Parameters["@Seq"].Value = mediaObject.Sequence;
			cmd.Parameters["@LastModifiedBy"].Value = mediaObject.LastModifiedByUserName;
			cmd.Parameters["@DateLastModified"].Value = mediaObject.DateLastModified;
			cmd.Parameters["@IsPrivate"].Value = mediaObject.IsPrivate;

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectDelete(int mediaObjectId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectDelete"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@MediaObjectId", SqlDbType.Int, 0, "MediaObjectId"));

			cmd.Parameters["@MediaObjectId"].Value = mediaObjectId;

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectMetadataDelete()
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectMetadataDelete"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@MediaObjectMetadataId", SqlDbType.Int, 0, "MediaObjectMetadataId"));

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectMetadataDeleteByMediaObjectId()
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectMetadataDeleteByMediaObjectId"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@MediaObjectId", SqlDbType.Int, 0, "MediaObjectId"));

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectMetadataUpdate()
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectMetadataUpdate"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@FKMediaObjectId", SqlDbType.Int, 0, "FKMediaObjectId"));
			cmd.Parameters.Add(new SqlParameter("@MetadataNameIdentifier", SqlDbType.Int, 0, "MetadataNameIdentifier"));
			cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, dataStoreConfig.MediaObjectMetadataDescriptionLength, "Description"));
			cmd.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar, dataStoreConfig.MediaObjectMetadataValueLength, "Value"));
			cmd.Parameters.Add(new SqlParameter("@MediaObjectMetadataId", SqlDbType.Int, 0, "@MediaObjectMetadataId"));

			return cmd;
		}

		private static SqlCommand GetCommandMediaObjectMetadataInsert()
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_MediaObjectMetadataInsert"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@FKMediaObjectId", SqlDbType.Int, 0, "FKMediaObjectId"));
			cmd.Parameters.Add(new SqlParameter("@MetadataNameIdentifier", SqlDbType.Int, 0, "MetadataNameIdentifier"));
			cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, dataStoreConfig.MediaObjectMetadataDescriptionLength, "Description"));
			cmd.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar, dataStoreConfig.MediaObjectMetadataValueLength, "Value"));
			SqlParameter prm = cmd.Parameters.Add(new SqlParameter("@Identity", SqlDbType.Int, 0, "MediaObjectDataId"));
			prm.Direction = ParameterDirection.Output;

			return cmd;
		}
		
		#endregion
 
		#endregion
	}
}

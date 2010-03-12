using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Entity;
using GalleryServerPro.Web.Pages;

namespace Gsp
{
	/// <summary>
	/// Summary description for Gallery
	/// </summary>
	[ScriptService]
	[WebService(Namespace = "http://www.galleryserverpro.com/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class Gallery : WebService
	{
		#region Private Fields

		private static ISynchronizationStatus _synchStatus = SynchronizationStatus.Instance;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="Gallery"/> class.
		/// </summary>
		static Gallery()
		{
			if (!AppSetting.Instance.IsInitialized)
			{
				Util.InitializeApplication();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Gallery"/> class.
		/// </summary>
		public Gallery()
		{
			// Ensure the app is initialized. This should have been done in the static constructor, but if anything went wrong
			// there, it may not be initialized, so we check again.
			if (!AppSetting.Instance.IsInitialized)
			{
				Util.InitializeApplication();
			}
		}

		#endregion

		#region Public Web Methods

		/// <summary>
		/// Get information for the specified media object, including its previous and next media object.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object.</param>
		/// <param name="displayType">The type of display object to receive (thumbnail, optimized, original).</param>
		/// <returns>Returns an instance of MediaObjectWebEntity containing information for the specified media object,
		/// including its previous and next media object.</returns>
		[WebMethod(EnableSession = true)]
		public MediaObjectWebEntity GetMediaObjectHtml(int mediaObjectId, DisplayObjectType displayType)
		{
			return GalleryPage.GetMediaObjectHtml(Factory.LoadMediaObjectInstance(mediaObjectId), displayType, true);
		}

		/// <summary>
		/// Permanently deletes the specified media object from the file system and data store. No action is taken if the
		/// user does not have delete permission.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object to be deleted.</param>
		[WebMethod(EnableSession = true)]
		public void DeleteMediaObject(int mediaObjectId)
		{
			IGalleryObject mo = Factory.LoadMediaObjectInstance(mediaObjectId);
			if (GalleryPage.IsUserAuthorized(SecurityActions.DeleteMediaObject, mo.Parent.Id))
			{
				mo.Delete();
				HelperFunctions.PurgeCache();
			}
		}

		/// <summary>
		/// Update the media object with the specified title and persist to the data store. The title is validated before
		/// saving, and may be altered to conform to business rules, such as removing HTML tags. The validated title is returned.
		/// If the user is not authorized to edit the title, no action is taken and the original title as stored in the data
		/// store is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object whose title is to be updated.</param>
		/// <param name="title">The title to be saved to the media object.</param>
		/// <returns>Returns the validated title.</returns>
		[WebMethod(EnableSession = true)]
		public string UpdateMediaObjectTitle(int mediaObjectId, string title)
		{
			IGalleryObject mo = Factory.LoadMediaObjectInstance(mediaObjectId);
			if (GalleryPage.IsUserAuthorized(SecurityActions.EditMediaObject, mo.Parent.Id))
			{
				string previousTitle = mo.Title;
				mo.Title = Util.CleanHtmlTags(title);

				if (mo.Title != previousTitle)
					GalleryObjectController.SaveGalleryObject(mo);

				HelperFunctions.PurgeCache();
			}

			return mo.Title;
		}

		/// <summary>
		/// Update the album with the specified properties in the albumEntity parameter. The title is validated before
		/// saving, and may be altered to conform to business rules, such as removing HTML tags. After the object is persisted
		/// to the data store, the albumEntity parameter is updated with the latest properties from the album object and returned.
		/// If the user is not authorized to edit the album, no action is taken.
		/// </summary>
		/// <param name="albumEntity">An AlbumWebEntity instance containing data to be persisted to the data store.</param>
		/// <returns>Returns an AlbumWebEntity instance containing the data as persisted to the data store. Some properties,
		/// such as the Title property, may be slightly altered to conform to validation rules.</returns>
		[WebMethod(EnableSession = true)]
		public AlbumWebEntity UpdateAlbumInfo(AlbumWebEntity albumEntity)
		{
			return AlbumController.UpdateAlbumInfo(albumEntity);
		}

		/// <summary>
		/// Retrieve album information for the specified album ID. Returns an object with empty properties if the user
		/// does not have permission to view the specified album.
		/// </summary>
		/// <param name="albumId">The album ID for which to retrieve information.</param>
		/// <returns>Returns AlbumWebEntity object containing information about the requested album.</returns>
		[WebMethod(EnableSession = true)]
		public AlbumWebEntity GetAlbumInfo(int albumId)
		{
			AlbumWebEntity albumEntity = new AlbumWebEntity();

			if (GalleryPage.IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, albumId))
			{
				IAlbum album = Factory.LoadAlbumInstance(albumId, false);
				albumEntity.Title = album.Title;
				albumEntity.Summary = album.Summary;
				albumEntity.DateStart = album.DateStart;
				albumEntity.DateEnd = album.DateEnd;
				albumEntity.IsPrivate = album.IsPrivate;
			}

			return albumEntity;
		}

		/// <summary>
		/// Get a list of metadata items corresponding to the specified mediaObjectId. Guaranteed to not return null.
		/// </summary>
		/// <param name="mediaObjectId">The ID of the media object for which to return its metadata items.</param>
		/// <returns>Returns a generic list of MetadataItemWebEntity objects that contain the metadata for the 
		/// specified media object.</returns>
		[WebMethod(EnableSession = true)]
		public List<MetadataItemWebEntity> GetMetadataItems(int mediaObjectId)
		{
			List<MetadataItemWebEntity> metadataItems = new List<MetadataItemWebEntity>();

			IGalleryObject mediaObject = Factory.LoadMediaObjectInstance(mediaObjectId);
			if (Util.IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, RoleController.GetGalleryServerRolesForUser(), mediaObject.Parent.Id, ((IAlbum)mediaObject.Parent).IsPrivate))
			{
				foreach (IGalleryObjectMetadataItem metadata in mediaObject.MetadataItems)
				{
					metadataItems.Add(new MetadataItemWebEntity(metadata.Description, metadata.Value));
				}
			}

			return metadataItems;
		}

		/// <summary>
		/// Permanently deletes the specified application error item from the error log. No action is taken if the
		/// user does not have delete permission.
		/// </summary>
		/// <param name="appErrorId">The ID that uniquely identifies the media object to be deleted.</param>
		[WebMethod(EnableSession = true)]
		public void DeleteAppError(int appErrorId)
		{
			if (GalleryPage.UserIsAdministrator)
			{
				GalleryServerPro.ErrorHandler.Error.Delete(appErrorId);
				HelperFunctions.PurgeCache();
			}
		}

		/// <summary>
		/// Stores the user's preference for showing or hiding the metadata popup window. The information is saved
		/// to the user's profile.
		/// </summary>
		/// <param name="isVisible">if set to <c>true</c> the metadata popup window is visible; otherwise <c>false</c>.</param>
		[WebMethod(EnableSession = true)]
		public void SetMetaDataVisibility(bool isVisible)
		{
			ProfileEntity profile = ProfileController.GetProfile();
			profile.ShowMediaObjectMetadata = isVisible;
			ProfileController.SaveProfile(profile);
		}

		///// <summary>
		///// Recycles the current web application.
		///// </summary>
		//[WebMethod(EnableSession = true)]
		//public void RecycleApp()
		//{
		//  Util.ForceAppRecycle();
		//}

		#region Synchronize Web Services

		/// <summary>
		/// Synchronize the specified album with the physical directory on the hard drive.
		/// </summary>
		/// <param name="albumId">The album id for the album to synchronize.</param>
		/// <param name="synchId">A GUID that uniquely indentifies the synchronization. If another synchronization is in 
		/// progress, a <see cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException" /> exception is thrown.</param>
		/// <param name="isRecursive">If set to <c>true</c> the synchronization continues drilling down into directories
		/// below the current one.</param>
		/// <param name="overwriteThumb">if set to <c>true</c> the thumbnail image for each media object is deleted and overwritten with a new one
		/// based on the original file. Applies to all media objects.</param>
		/// <param name="overwriteOpt">if set to <c>true</c> the optimized image for each media object is deleted and overwritten with a new one
		/// based on the original file. Only relevant for images.</param>
		/// <param name="regenerateMetadata">if set to <c>true</c> the existing metadata for each media object is replaced with
		/// the metadata stored within the original media object file.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException">
		/// Thrown if another synchronization is in progress.</exception>
		[WebMethod(EnableSession = true)]
		public void Synchronize(int albumId, string synchId, bool isRecursive, bool overwriteThumb, bool overwriteOpt, bool regenerateMetadata)
		{
			// Refresh the synch status static variable. Each time we access the Instance property of the singleton, it gets its
			// properties refreshed with the latest values from the data store.

			#region Check user authorization

			bool isUserAuthenticated = Util.IsAuthenticated;
			if (!isUserAuthenticated)
				return;

			if (!Util.IsUserAuthorized(SecurityActions.Synchronize, RoleController.GetGalleryServerRolesForUser(), albumId, false))
				return;

			#endregion
			
			lock (_synchStatus)
			{
				_synchStatus = SynchronizationStatus.Instance;
			}

			SynchronizationManager synchMgr = new SynchronizationManager();

			synchMgr.IsRecursive = isRecursive;
			synchMgr.OverwriteThumbnail = overwriteThumb;
			synchMgr.OverwriteOptimized = overwriteOpt;
			synchMgr.RegenerateMetadata = regenerateMetadata;

			try
			{
				synchMgr.Synchronize(synchId, Factory.LoadAlbumInstance(albumId, true), Util.UserName);
			}
			catch (Exception ex)
			{
				AppErrorController.LogError(ex);
				throw;
			}
		}

		/// <summary>
		/// Gets information about the current synchronization. If no synchronization is in progress, returns information about
		/// the most recent synchronization.
		/// </summary>
		/// <returns>Returns information about the current synchronization.</returns>
		[WebMethod]
		public SynchStatusWebEntity GetCurrentStatus()
		{
			SynchStatusWebEntity synchStatusWeb = new SynchStatusWebEntity();

			synchStatusWeb.SynchId = _synchStatus.SynchId;
			synchStatusWeb.TotalFileCount = _synchStatus.TotalFileCount;
			synchStatusWeb.CurrentFileIndex = _synchStatus.CurrentFileIndex + 1;

			if ((_synchStatus.CurrentFilePath != null) && (_synchStatus.CurrentFileName != null))
				synchStatusWeb.CurrentFile = System.IO.Path.Combine(_synchStatus.CurrentFilePath, _synchStatus.CurrentFileName);

			synchStatusWeb.Status = _synchStatus.Status.ToString();
			synchStatusWeb.StatusForUI = GetFriendlyStatusText(_synchStatus.Status);
			synchStatusWeb.PercentComplete = CalculatePercentComplete(_synchStatus);

			// Update the Skipped Files, but only when the synch is complete.
			lock (_synchStatus)
			{
				if (_synchStatus.Status == SynchronizationState.Complete)
				{
					if (_synchStatus.SkippedMediaObjects.Count > GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch)
					{
						// We have a large number of skipped media objects. We don't want to send it all to the browsers, because it might take
						// too long or cause an error if it serializes to a string longer than int.MaxValue, so let's trim it down.
						_synchStatus.SkippedMediaObjects.RemoveRange(GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch,
						                                             _synchStatus.SkippedMediaObjects.Count -
						                                             GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch);
					}
					synchStatusWeb.SkippedFiles = _synchStatus.SkippedMediaObjects;
				}
			}
			return synchStatusWeb;
		}

		/// <summary>
		/// Terminates the synchronization with the specified <paramref name="taskId"/>.
		/// </summary>
		/// <param name="taskId">The task id (also knows as synch ID) representing the synchronization to cancel.</param>
		[WebMethod]
		public void TerminateTask(string taskId)
		{
			lock (_synchStatus)
			{
				if (_synchStatus.SynchId == taskId)
				{
					_synchStatus.ShouldTerminate = true;
				}
			}
		}

		#endregion

		#endregion

		#region Private Methods
		
		private static string GetFriendlyStatusText(SynchronizationState status)
		{
			switch (status)
			{
				case SynchronizationState.AnotherSynchronizationInProgress: return Resources.GalleryServerPro.Task_Synch_Progress_Status_SynchInProgressException_Hdr;
				case SynchronizationState.Complete: return status.ToString();
				case SynchronizationState.Error: return status.ToString();
				case SynchronizationState.PersistingToDataStore: return Resources.GalleryServerPro.Task_Synch_Progress_Status_PersistingToDataStore_Hdr;
				case SynchronizationState.SynchronizingFiles: return Resources.GalleryServerPro.Task_Synch_Progress_Status_SynchInProgress_Hdr;
				default: throw new System.ComponentModel.InvalidEnumArgumentException("The GetFriendlyStatusText() method in synchronize.aspx encountered a SynchronizationState enum value it was not designed for. This method must be updated.");
			}
		}

		private static int CalculatePercentComplete(ISynchronizationStatus synchStatus)
		{
			if (synchStatus.Status == SynchronizationState.SynchronizingFiles)
				return (int)(((double)synchStatus.CurrentFileIndex / (double)synchStatus.TotalFileCount) * 100);
			else
				return 100;
		}

		/// <summary>
		/// Generate a comma-delimited string containing a list of the file extensions that are in the specified filenames.
		/// If more than one filename has the same extension, the extension is listed just once in the output.
		/// </summary>
		/// <param name="filenames">A list of filenames.</param>
		/// <returns>Returns a comma-delimited string containing a list of the file extensions that are in the specified filenames.</returns>
		private static string GetFileExtensions(System.Collections.Specialized.StringCollection filenames)
		{
			System.Configuration.CommaDelimitedStringCollection fileExts = new System.Configuration.CommaDelimitedStringCollection();

			foreach (string filename in filenames)
			{
				string fileExt = System.IO.Path.GetExtension(filename);
				if (!fileExts.Contains(fileExt))
				{
					fileExts.Add(fileExt);
				}
			}

			return fileExts.ToString();
		}

		#endregion

	}
}

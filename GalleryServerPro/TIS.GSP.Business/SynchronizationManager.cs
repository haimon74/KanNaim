using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.Business.Properties;
using GalleryServerPro.ErrorHandler;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Contains functionality for synchronizing the media object files on the hard drive with the records in the data store.
	/// </summary>
	public class SynchronizationManager
	{
		#region Private Static Fields

		private static readonly string _thumbnailRootPath = AppSetting.Instance.ThumbnailPath;
		private static readonly string _optimizedRootPath = AppSetting.Instance.OptimizedPath;
		private static readonly long _optimizedTriggerSizeKB = ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedImageTriggerSizeKB;
		private static readonly int _optimizedMaxLength = ConfigManager.GetGalleryServerProConfigSection().Core.MaxOptimizedLength;
		private static readonly string _thumbnailPrefix = ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailFileNamePrefix;
		private static readonly string _optimizedPrefix = ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedFileNamePrefix;
		private static readonly int _mediaObjectPhysicalPathLength = AppSetting.Instance.MediaObjectPhysicalPath.Length;

		#endregion

		#region Private Fields

		private bool _isRecursive;
		private bool _overwriteOptimized;
		private bool _overwriteThumbnail;
		private bool _regenerateMetadata;
		private string _userName;
		private int _lastTransactionCommitFileIndex;

		private ISynchronizationStatus _synchStatus;

		// Read-only lists pulled from data store at beginning
		private Dictionary<String, IAlbum> _albumsFromDataStore;
		private Dictionary<String, IGalleryObject> _mediaObjectsFromDataStore;

		#endregion

		#region Constructor

		/// <summary>
		/// Instantiates a new SynchronizationManager object, with the properties IsRecursive, OverwriteOptimized,
		/// and OverwriteThumbnail all defaulted to true. The property RegenerateMetadata is set to false.
		/// </summary>
		public SynchronizationManager()
		{
			this._isRecursive = true;
			this._overwriteOptimized = true;
			this._overwriteThumbnail = true;
			this._regenerateMetadata = false;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Indicates whether the synchronization continues drilling down into directories
		/// below the current one. The default value is true.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the synchronization procedure recursively
		/// synchronizes all directories within the current one; otherwise, <c>false</c>.
		/// </value>
		public bool IsRecursive
		{
			get { return this._isRecursive; }
			set { this._isRecursive = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether an optimized image is deleted and overwritten with a new one
		/// based on the original file. Only relevant for images. The default value is true.
		/// </summary>
		/// <value><c>true</c> if optimized images are overwritten during a synchronization; otherwise, <c>false</c>.</value>
		public bool OverwriteOptimized
		{
			get { return this._overwriteOptimized; }
			set { this._overwriteOptimized = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to replace the existing metadata for each media object with
		/// the metadata stored within the media object file. The default value is false.
		/// </summary>
		/// <value><c>true</c> if metadata is to be regenerated during a synchronization; otherwise, <c>false</c>.</value>
		public bool RegenerateMetadata
		{
			get { return _regenerateMetadata; }
			set { _regenerateMetadata = value; }
		}

		/// <summary>
		/// Gets or sets the user name for the logged on user. This is used for the audit fields in the album and media
		/// objects.
		/// </summary>
		/// <value>The user name for the logged on user.</value>
		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether a thumbnail image is deleted and overwritten with a new one
		/// based on the original file. Applies to all media objects. The default value is true.
		/// </summary>
		/// <value><c>true</c> if thumbnail images are overwritten during a synchronization; otherwise, <c>false</c>.</value>
		public bool OverwriteThumbnail
		{
			get { return this._overwriteThumbnail; }
			set { this._overwriteThumbnail = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Synchronize the media object library, starting with the root album. Optionally specify that only the 
		/// specified album is synchronized. If <see cref="IsRecursive" /> = true, then child albums are recursively synchronized;
		/// otherwise, only the root album (or the specified album if that overload is used) is synchronized.
		/// </summary>
		/// <param name="synchId">A GUID that uniquely indentifies the synchronization. If another synchronization is in 
		/// progress, a <see cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException" /> exception is thrown.</param>
		/// <param name="userName">The user name for the logged on user. This is used for the audit fields in the album 
		/// and media objects.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException">
		/// Thrown if another synchronization is in progress.</exception>
		public void Synchronize(string synchId, string userName)
		{
			this.Synchronize(synchId, Factory.LoadRootAlbumInstance(), userName);
		}

		/// <summary>
		/// Synchronize the media object library, starting with the root album. Optionally specify that only the 
		/// specified album is synchronized. If <see cref="IsRecursive" /> = true, then child albums are recursively synchronized;
		/// otherwise, only the root album (or the specified album if that overload is used) is synchronized.
		/// </summary>
		/// <param name="synchId">A GUID that uniquely indentifies the synchronization. If another synchronization is in 
		/// progress, a <see cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException" /> exception is thrown.</param>
		/// <param name="userName">The user name for the logged on user. This is used for the audit fields in the album 
		/// and media objects.</param>
		/// <param name="album">The album to synchronize.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException">
		/// Thrown if another synchronization is in progress.</exception>
		public void Synchronize(string synchId, IAlbum album, string userName)
		{
			Factory.GetDataProvider().BeginTransaction();

			try
			{
				Initialize(synchId, album, userName); // Will throw SynchronizationInProgressException if another is in progress. Will be caught by upstream code.

				DirectoryInfo albumDirectory = new DirectoryInfo(album.FullPhysicalPathOnDisk);

				// Update this album.
				album.IsSynchronized = true;

				// Synchronize the files in this album. No recursive action.
				SynchronizeMediaObjectFiles(albumDirectory, album);

				// Synchronize any external media objects previously added. No recursive action.
				SynchronizeExternalMediaObjects(album);

				if (this.IsRecursive)
				{
					// Synchronize the child directories and their files. Acts recursively.
					SynchronizeChildDirectories(albumDirectory, album);
				}

				// Persist synchronized objects to the data store and delete the unsynchronized ones.
				DeleteUnsynchronizedObjects();

				Album.AssignAlbumThumbnail(album, false, true, this._userName);

				DeleteOrphanedImages(album);
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationTerminationRequestedException)
			{
				// The user has cancelled the synchronization. Swallow the exception and return.
				Factory.GetDataProvider().RollbackTransaction();

				return;
			}
			catch
			{
				Factory.GetDataProvider().RollbackTransaction();
				throw;
			}
			finally
			{
				// Set the list of hash keys to null to clean up memory.
				MediaObjectHashKeys.Clear();

				HelperFunctions.PurgeCache();

				if (this._synchStatus != null)
					this._synchStatus.Finish(synchId);
			}

			Factory.GetDataProvider().CommitTransaction();
		}

		#endregion

		#region Private methods

		private bool Initialize(string synchId, IAlbum album, string userName)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentNullException("userName");

			this._userName = userName;

			#region Set up the _synchStatus instance

			this._synchStatus = SynchronizationStatus.Instance;

			// Tell that status instance we are starting a new synchronization. It will throw
			// SynchronizationInProgressException if another is in progress.
			this._synchStatus.Start(synchId, CountFiles(album.FullPhysicalPathOnDisk));

			#endregion

			#region Populate the _albumsFromDataStore and _mediaObjectsFromDataStore dictionary objects and set each to IsSynchronized = false

			this._albumsFromDataStore = new Dictionary<String, IAlbum>();
			this._mediaObjectsFromDataStore = new Dictionary<String, IGalleryObject>(this._synchStatus.TotalFileCount);

			// Fill _albums and _mediaObjects with the albums and media objects for this album as currently stored 
			// in the data store. We'll be comparing these objects with those we find on the hard drive. Act recursively
			// if IsRecursive = true. Set IsSynchronized = false for each object. (We'll be setting it back to true
			// as we synchronize each object.)
			album.IsSynchronized = false;
			album.RegenerateThumbnailOnSave = this.OverwriteThumbnail;

			this._albumsFromDataStore.Add(album.FullPhysicalPathOnDisk, album);

			foreach (IGalleryObject mediaObject in album.GetChildGalleryObjects(GalleryObjectType.MediaObject))
			{
				mediaObject.IsSynchronized = false;
				mediaObject.RegenerateThumbnailOnSave = this.OverwriteThumbnail;
				mediaObject.RegenerateOptimizedOnSave = this.OverwriteOptimized;
				mediaObject.RegenerateMetadataOnSave = this.RegenerateMetadata;

				if (!String.IsNullOrEmpty(mediaObject.Hashkey))
				{
					this._mediaObjectsFromDataStore.Add(mediaObject.Hashkey, mediaObject);
				}
			}

			if (this._isRecursive)
			{
				AddChildAlbumsAndGalleryObjectsAndSetToUnsynchronized(this._albumsFromDataStore, this._mediaObjectsFromDataStore, album);
			}

			#endregion

			// Clear the list of hash keys so we're starting with a fresh load from the data store.
			MediaObjectHashKeys.Clear();

			return true;
		}

		/// <summary>
		/// Add the child albums and media objects as stored on disk to the specified dictionary objects. Set
		/// IsSynchronized = false for each album and media object. This will be set to true as each is processed.
		/// This method calls itself recursively if IsRecursive = true.
		/// </summary>
		/// <param name="albums">A Dictionary object containing relevant albums for this synchronization. The album specified
		/// in the parentAlbum parameter will be added to this object.</param>
		/// <param name="mediaObjects">A Dictionary object containing relevant media objects for this synchronization.
		/// Media objects within the parentAlbum parameter will be added to this object.</param>
		/// <param name="parentAlbum">The album used as the source for populating the albums and mediaObjects
		/// parameters.</param>
		private void AddChildAlbumsAndGalleryObjectsAndSetToUnsynchronized(Dictionary<string, IAlbum> albums, Dictionary<string, IGalleryObject> mediaObjects, IAlbum parentAlbum)
		{
			foreach (IAlbum childAlbum in parentAlbum.GetChildGalleryObjects(GalleryObjectType.Album))
			{
				childAlbum.IsSynchronized = false;
				childAlbum.RegenerateThumbnailOnSave = this.OverwriteThumbnail;

				try
				{
					// There can be situations where the database becomes corrupt, and the same album has two records. When this happens,
					// the following line will fail. Instead of letting the exception cause the synch to fail, we swallow the exception.
					// This will cause the album that caused the exception to eventually be deleted, which is what we want.
					albums.Add(childAlbum.FullPhysicalPathOnDisk, childAlbum);
				}
				catch (System.ArgumentException) { }

				foreach (IGalleryObject mediaObject in childAlbum.GetChildGalleryObjects(GalleryObjectType.MediaObject))
				{
					if (!String.IsNullOrEmpty(mediaObject.Hashkey))
					{
						mediaObject.IsSynchronized = false;
						mediaObject.RegenerateOptimizedOnSave = this.OverwriteOptimized;
						mediaObject.RegenerateThumbnailOnSave = this.OverwriteThumbnail;
						mediaObject.RegenerateMetadataOnSave = this.RegenerateMetadata;

						try
						{
							// There may be situations where the database becomes corrupt, and the same media object has two records. When this happens,
							// the following line will fail. Instead of letting the exception cause the synch to fail, we swallow the exception.
							// This will cause the media object that caused the exception to eventually be deleted, which is what we want.
							mediaObjects.Add(mediaObject.Hashkey, mediaObject);
						}
						catch (System.ArgumentException) { }
					}
				}

				if (this._isRecursive)
				{
					AddChildAlbumsAndGalleryObjectsAndSetToUnsynchronized(albums, mediaObjects, childAlbum);
				}
			}

		}

		/// <summary>
		/// Get the number of files in the specified directory path, including any subdirectories if
		/// IsRecursive = true. But don't count any optimized or thumbnail files.
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.DirectoryNotFoundException">Thrown if the specified directory does not exist.</exception>
		private int CountFiles(string directoryPath)
		{
			if (String.IsNullOrEmpty(directoryPath))
				throw new ArgumentNullException("directoryPath");

			int countTotal, countThumbnail, countOptimized;

			countTotal = Directory.GetFiles(directoryPath).Length;
			countThumbnail = Directory.GetFiles(directoryPath, ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailFileNamePrefix + "*").Length;
			countOptimized = Directory.GetFiles(directoryPath, ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedFileNamePrefix + "*").Length;
			string[] dirs = Directory.GetDirectories(directoryPath);
			if (this._isRecursive)
			{
				foreach (string dir in dirs)
				{
					countTotal += CountFiles(dir);
				}
			}
			int totalNumFiles = countTotal - countThumbnail - countOptimized;

			// If we compute a number < 0, then just return 0.
			return (totalNumFiles < 0 ? 0 : totalNumFiles);
		}

		/// <summary>
		/// Ensure the directories and media object files within parentDirectory have corresponding albums 
		/// and media objects. An exception is thrown if parentAlbum.FullPhysicalPathOnDisk does not equal
		/// parentDirectory.FullName. If IsRecursive = true, this method recursively calls itself.
		/// </summary>
		/// <param name="parentDirectory">A DirectoryInfo instance corresponding to the FullPhysicalPathOnDisk
		/// property of parentAlbum.</param>
		/// <param name="parentAlbum">An album instance. Directories under the parentDirectory parameter will be
		/// added (or updated if they already exist) as child albums of this instance.</param>
		private void SynchronizeChildDirectories(DirectoryInfo parentDirectory, IAlbum parentAlbum)
		{
			#region Parameter validation

			if (parentDirectory == null)
				throw new ArgumentNullException("parentDirectory");

			if (parentAlbum == null)
				throw new ArgumentNullException("parentAlbum");

			if (parentDirectory.FullName != parentAlbum.FullPhysicalPath)
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.SynchronizationManager_SynchronizeChildDirectories_Ex_Msg));

			#endregion

			// Recursively traverse all subdirectories and their files and synchronize each object we find.
			// Skip any hidden directories.
			foreach (DirectoryInfo subdirectory in parentDirectory.GetDirectories())
			{
				if ((subdirectory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					this._synchStatus.SkippedMediaObjects.Add(new KeyValuePair<string, string>(subdirectory.FullName.Remove(0, _mediaObjectPhysicalPathLength + 1), Resources.SynchronizationStatus_Hidden_Directory_Msg));
					continue;
				}

				IAlbum childAlbum = SynchronizeDirectory(subdirectory, parentAlbum);
				SynchronizeMediaObjectFiles(subdirectory, childAlbum);

				SynchronizeExternalMediaObjects(childAlbum);

				SynchronizeChildDirectories(subdirectory, childAlbum);
			}
		}

		private void SynchronizeMediaObjectFiles(DirectoryInfo directory, IAlbum album)
		{
			#region Parameter validation

			if (album == null)
				throw new ArgumentNullException("album");

			if (directory.FullName != album.FullPhysicalPath)
				throw new ArgumentException("Error in SynchronizeMediaObjectFiles().");

			#endregion

			//Update the media object table in the database with the file attributes of all
			//files in the directory passed to this function. Skip any hidden files.

			foreach (FileInfo file in directory.GetFiles())
			{
				if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					this._synchStatus.SkippedMediaObjects.Add(new KeyValuePair<string, string>(file.FullName.Remove(0, _mediaObjectPhysicalPathLength + 1), Resources.SynchronizationStatus_Hidden_File_Msg));
					continue;
				}

				#region Process thumbnail or optimized image

				if (file.Name.StartsWith(_thumbnailPrefix, StringComparison.OrdinalIgnoreCase))
				{
					// We have a thumbnail image. If we are storing thumbnails in a different directory, delete the file. The user may have just 
					// specified a new thumbnail path, and we need to delete all the previous thumbnails from their original location.
					if (_thumbnailRootPath != AppSetting.Instance.MediaObjectPhysicalPath)
					{
						File.Delete(file.FullName);
					}
					continue;
				}

				if (file.Name.StartsWith(_optimizedPrefix, StringComparison.OrdinalIgnoreCase))
				{
					// We have an optimized image. If we are storing optimized images in a different directory, delete the file. The user may have 
					// just specified a new optimized path, and we need to delete all the previous optimized images from their original location.
					if (_optimizedRootPath != AppSetting.Instance.MediaObjectPhysicalPath)
					{
						File.Delete(file.FullName);
					}
					continue;
				}

				#endregion

				IGalleryObject mediaObject = null;
				// See if this file is an existing media object. First look in the album's children. If not there, search the hash
				// keys - maybe it was moved from another directory.
				foreach (IGalleryObject existingMO in album.GetChildGalleryObjects(GalleryObjectType.MediaObject))
				{
					if (existingMO.Original.FileNamePhysicalPath == file.FullName)
					{
						mediaObject = existingMO;
						break;
					}
				}

				if ((mediaObject != null) || ((mediaObject == null) && this._mediaObjectsFromDataStore.TryGetValue(HelperFunctions.GetHashKey(file), out mediaObject)))
				{
					// Found an existing media object matching the file on disk. Update properties, but only if its file extension
					// is enabled. (If this is a media object that had been added to Gallery Server but its file type was 
					// subsequently disabled, we do not want to synchronize it - we want its info in the data store to be deleted.)
					if (HelperFunctions.IsFileAuthorizedForAddingToGallery(file.Name))
					{
						UpdateExistingMediaObject(album, mediaObject);
					}
				}
				else
				{
					// No media object exists for this file. Create a new one.
					CreateNewMediaObject(album, file);
				}

				int newFileIndex = this._synchStatus.CurrentFileIndex + 1;
				if (newFileIndex < this._synchStatus.TotalFileCount)
				{
					UpdateStatus(newFileIndex, file.DirectoryName, file.Name);
				}

				if (this._synchStatus.ShouldTerminate)
				{
					this._synchStatus.ShouldTerminate = false;
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationTerminationRequestedException();
				}
			}
		}

		private void UpdateStatus(int currentFileIndex, string filepath, string filename)
		{
			this._synchStatus.CurrentFileIndex = currentFileIndex;
			this._synchStatus.CurrentFileName = filename;
			this._synchStatus.CurrentFilePath = filepath.Remove(0, _mediaObjectPhysicalPathLength).TrimStart(new char[] { Path.DirectorySeparatorChar });
		}

		private void CreateNewMediaObject(IAlbum album, FileInfo file)
		{
			try
			{
				IGalleryObject mediaObject = Factory.CreateMediaObjectInstance(file, album);
				HelperFunctions.UpdateAuditFields(mediaObject, this._userName);
				mediaObject.Save();

				Core core = ConfigManager.GetGalleryServerProConfigSection().Core;
				if (!core.MediaObjectPathIsReadOnly && (core.DiscardOriginalImageDuringImport) && (mediaObject is Business.Image))
				{
					((Business.Image)mediaObject).DeleteHiResImage();
					mediaObject.Save();
				}

				mediaObject.IsSynchronized = true;

				System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(mediaObject.Hashkey));
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedMediaObjectTypeException)
			{
				this._synchStatus.SkippedMediaObjects.Add(new KeyValuePair<string, string>(file.FullName.Remove(0, _mediaObjectPhysicalPathLength + 1), Resources.SynchronizationStatus_Disabled_File_Type_Msg));
			}
		}

		private void UpdateExistingMediaObject(IAlbum album, IGalleryObject mediaObject)
		{
			if (mediaObject.Parent.Id != album.Id)
			{
				mediaObject.Parent = album;
			}

			// If the generated hash key is the same as the one already assigned, then do nothing. Otherwise,
			// generate a guaranteed unique hash key with the GetHashKeyUnique function.
			if (mediaObject.Hashkey != HelperFunctions.GetHashKey(mediaObject.Original.FileInfo))
			{
				mediaObject.Hashkey = HelperFunctions.GetHashKeyUnique(mediaObject.Original.FileInfo);
			}

			// Check for existence of thumbnail.
			if (!File.Exists(mediaObject.Thumbnail.FileNamePhysicalPath))
			{
				mediaObject.RegenerateThumbnailOnSave = true;
			}

			Image image = mediaObject as Image;
			if (image != null)
				EvaluateOptimizedImage(image);
			else
				UpdateNonImageWidthAndHeight(mediaObject);

			HelperFunctions.UpdateAuditFields(mediaObject, this._userName);
			mediaObject.Save();
			mediaObject.IsSynchronized = true;
		}

		private void SynchronizeExternalMediaObjects(IAlbum album)
		{
			foreach (IGalleryObject mediaObject in album.GetChildGalleryObjects(GalleryObjectType.External))
			{
				// Check for existence of thumbnail.
				if (this.OverwriteThumbnail || !File.Exists(mediaObject.Thumbnail.FileNamePhysicalPath))
				{
					mediaObject.RegenerateThumbnailOnSave = true;
					HelperFunctions.UpdateAuditFields(mediaObject, this._userName);
					mediaObject.Save();
					mediaObject.IsSynchronized = true;
				}
			}
		}

		/// <summary>
		/// Find, or create if necessary, the album corresponding to the specified directory and set it as the 
		/// child of the parentAlbum parameter.
		/// </summary>
		/// <param name="directory">The directory for which to obtain a matching album object.</param>
		/// <param name="parentAlbum">The album that contains the album at the specified directory.</param>
		/// <returns>Returns an album object corresponding to the specified directory and having the specified
		/// parent album.</returns>
		private IAlbum SynchronizeDirectory(DirectoryInfo directory, IAlbum parentAlbum)
		{
			#region Parameter validation

			if (directory == null)
				throw new ArgumentNullException("directory");

			if (parentAlbum == null)
				throw new ArgumentNullException("parentAlbum");

			if (directory.Parent.FullName != parentAlbum.FullPhysicalPathOnDisk.TrimEnd(new char[] { Path.DirectorySeparatorChar }))
				throw new ArgumentException("Error in SynchronizeDirectory().");

			#endregion

			IAlbum childAlbum;
			if (this._albumsFromDataStore.TryGetValue(directory.FullName, out childAlbum))
			{
				// Found the album. Update properties.
				childAlbum.IsSynchronized = true;
				childAlbum.IsPrivate = (parentAlbum.IsPrivate ? true : childAlbum.IsPrivate); // Only set to private if parent is private
				childAlbum.RegenerateThumbnailOnSave = this.OverwriteThumbnail;
			}
			else
			{
				// No album exists for this directory. Create a new one.
				childAlbum = Factory.CreateAlbumInstance();
				childAlbum.Parent = parentAlbum;

				string directoryName = directory.Name;
				childAlbum.Title = directoryName;
				//childAlbum.ThumbnailMediaObjectId = 0; // not needed
				childAlbum.DirectoryName = directoryName;
				childAlbum.FullPhysicalPathOnDisk = Path.Combine(parentAlbum.FullPhysicalPathOnDisk, directoryName);
				childAlbum.IsPrivate = parentAlbum.IsPrivate;
			}

			if (childAlbum.IsNew || childAlbum.HasChanges)
			{
				HelperFunctions.UpdateAuditFields(childAlbum, this._userName);
				childAlbum.Save();
			}

			// Commit the transaction to the database for every 100 media objects that are processed.
			if ((this._synchStatus.CurrentFileIndex - this._lastTransactionCommitFileIndex) >= 100)
			{
				HelperFunctions.CommitTransaction();
				HelperFunctions.BeginTransaction();
				this._lastTransactionCommitFileIndex = this._synchStatus.CurrentFileIndex;
			}

			return childAlbum;
		}

		private void DeleteUnsynchronizedObjects()
		{
			// Save each synchronized object, and delete each unsynchronized object.
			this._synchStatus.Status = SynchronizationState.PersistingToDataStore;

			// Delete unsynchronized albums.
			foreach (KeyValuePair<String, IAlbum> albumKeyValue in this._albumsFromDataStore)
			{
				IAlbum album = albumKeyValue.Value;

				if (!album.IsSynchronized)
				{
					album.DeleteFromGallery();
				}
			}

			// Delete unsynchronized  media objects.
			foreach (KeyValuePair<String, IGalleryObject> mediaObjectKeyValue in this._mediaObjectsFromDataStore)
			{
				IGalleryObject galleryObject = mediaObjectKeyValue.Value;

				if (!galleryObject.IsSynchronized)
				{
					galleryObject.DeleteFromGallery();
				}
			}
		}

		/// <summary>
		/// Delete any thumbnail and optimized images that do not have matching media objects.
		/// This can occur when a user manually transfers (e.g. uses Windows Explorer)
		/// original images to a new directory and leaves the thumbnail and optimized
		/// images in the original directory or when a user deletes the original media file in 
		/// Explorer. This function *only* deletes files that begin the the thumbnail and optimized
		/// prefix (e.g. zThumb_, zOpt_).
		/// </summary>
		/// <param name="album">The album whose directory is to be processed for orphaned image files.</param>
		private void DeleteOrphanedImages(IAlbum album)
		{
			if (album == null)
				throw new ArgumentNullException("album");

			// STEP 1: Get list of directories that may contain thumbnail or optimized images for the current album
			string originalPath = album.FullPhysicalPathOnDisk;
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPathOnDisk, AppSetting.Instance.ThumbnailPath);
			string optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPathOnDisk, AppSetting.Instance.OptimizedPath);

			List<string> albumPaths = new List<string>(3);

			// The original path may contain thumbnails or optimized images when the thumbnail/optimized path is the same as the original path
			if ((AppSetting.Instance.ThumbnailPath == AppSetting.Instance.MediaObjectPhysicalPath) || (AppSetting.Instance.OptimizedPath == AppSetting.Instance.MediaObjectPhysicalPath))
				albumPaths.Add(originalPath);

			if (!albumPaths.Contains(thumbnailPath))
				albumPaths.Add(thumbnailPath);

			if (!albumPaths.Contains(optimizedPath))
				albumPaths.Add(optimizedPath);


			string thumbnailPrefix = ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailFileNamePrefix;
			string optimizedPrefix = ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedFileNamePrefix;

			IGalleryObjectCollection mediaObjects = album.GetChildGalleryObjects(GalleryObjectType.MediaObject);

			// STEP 2: Loop through each path and make sure all thumbnail and optimized files in each directory have 
			// matching media objects. Delete any files that do not.
			foreach (string albumPath in albumPaths)
			{
				if (!Directory.Exists(albumPath))
					return;

				DirectoryInfo directory = new DirectoryInfo(albumPath);

				// Loop through each file in the directory.
				foreach (FileInfo file in directory.GetFiles())
				{
					if ((file.Name.StartsWith(thumbnailPrefix, StringComparison.OrdinalIgnoreCase)) || (file.Name.StartsWith(optimizedPrefix, StringComparison.OrdinalIgnoreCase)))
					{
						// This file is a thumbnail or optimized file. Check to see if any media object in this album
						// refers to it.
						bool foundMediaObject = false;
						foreach (IGalleryObject mediaObject in mediaObjects)
						{
							if ((mediaObject.Optimized.FileName == file.Name) || (mediaObject.Thumbnail.FileName == file.Name))
							{
								foundMediaObject = true;
								break;
							}
						}

						if (!foundMediaObject)
						{
							// No media object in this album refers to this thumbnail or optimized image. Smoke it!
							try
							{
								file.Delete();
							}
							catch (IOException ex)
							{
								// An exception occurred, probably because the account ASP.NET is running under does not
								// have permission to delete the file. Let's record the error, but otherwise ignore it.
								Error.Record(ex);
							}
							catch (System.Security.SecurityException ex)
							{
								// An exception occurred, probably because the account ASP.NET is running under does not
								// have permission to delete the file. Let's record the error, but otherwise ignore it.
								Error.Record(ex);
							}
							catch (UnauthorizedAccessException ex)
							{
								// An exception occurred, probably because the account ASP.NET is running under does not
								// have permission to delete the file. Let's record the error, but otherwise ignore it.
								Error.Record(ex);
							}
						}
					}
				}
			}

			// Now recursively loop through any child albums.
			IGalleryObjectCollection childAlbums = album.GetChildGalleryObjects(GalleryObjectType.Album);
			if (this.IsRecursive)
			{
				foreach (IAlbum childAlbum in childAlbums)
				{
					DeleteOrphanedImages(childAlbum);
				}
			}
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Check that the optimized image exists. <paramref name="mediaObject"/> *must* be an <see cref="Image"/> type.
		/// If "overwrite compressed" option is selected, also check whether it the optimized version is really needed.
		/// </summary>
		/// <param name="mediaObject">The media object whose optimized image is to be checked.</param>
		/// <remarks>Note that the ValidateSave() method in the GalleryObject class also checks for the existence of 
		/// the thumbnail and optimized images. However, we need to do it here because the UpdateAuditFields method
		/// that is called after this function is executed updates the audit fields only when HasChanges = true. If 
		/// we don't check for these images, then the media object might have HasChanges = false, which causes the 
		/// audit fields to remain unchanged. But then if ValidateSave updates them, we'll get an error because the 
		/// GalleryObject class doesn't update the audit fields (it knows nothing about the current user.)</remarks>
		private void EvaluateOptimizedImage(Image mediaObject)
		{
			if (mediaObject == null)
				return;

			// Check for existence of optimized image.
			if (!File.Exists(mediaObject.Optimized.FileNamePhysicalPath))
			{
				// Optimized image doesn't exist, but maybe we don't need it anyway. Check for this possibility.
				if (DoesOriginalExceedOptimizedTriggers(mediaObject))
				{
					mediaObject.RegenerateOptimizedOnSave = true; // Yup, we need to generate the opt. image.
				}
				else
				{
					// The original isn't big enough to need an optimized image, so make sure the optimized properties
					// are the same as the original's properties.
					mediaObject.Optimized.FileName = mediaObject.Original.FileName;
					mediaObject.Optimized.Width = mediaObject.Original.Width;
					mediaObject.Optimized.Height = mediaObject.Original.Height;
					mediaObject.Optimized.FileSizeKB = mediaObject.Original.FileSizeKB;
				}
			}
			else
			{
				// We have an image where the optimized image exists. But perhaps the user changed some optimized trigger settings
				// and we no longer need the optimized image. Check for this possibility, and if true, update the optimized properties
				// to be the same as the original. Note: We only check if user selected the "overwrite compresssed" option - this is 
				// because checking the dimensions of an image is very resource intensive, so we'll only do this if necessary.
				if (this.OverwriteOptimized && !DoesOriginalExceedOptimizedTriggers(mediaObject))
				{
					mediaObject.Optimized.FileName = mediaObject.Original.FileName;
					mediaObject.Optimized.Width = mediaObject.Original.Width;
					mediaObject.Optimized.Height = mediaObject.Original.Height;
					mediaObject.Optimized.FileSizeKB = mediaObject.Original.FileSizeKB;
				}
			}
		}

		/// <summary>
		/// Update the width and height values to the default values specified for audio, video, and generic objects in
		/// galleryserverpro.config. This method has no effect on <see cref="Image"/> or <see cref="ExternalMediaObject"/> objects.
		/// </summary>
		/// <param name="mediaObject">The <see cref="IGalleryObject"/> whose <see cref="DisplayObject.Width"/> and 
		/// <see cref="DisplayObject.Height"/> properties of the <see cref="IGalleryObject.Original"/> property is to be 
		/// updated with the current values in galleryserverpro.config.</param>
		private static void UpdateNonImageWidthAndHeight(IGalleryObject mediaObject)
		{
			if (mediaObject is Video)
			{
				mediaObject.Original.Width = ConfigManager.GetGalleryServerProConfigSection().Core.DefaultVideoPlayerWidth;
				mediaObject.Original.Height = ConfigManager.GetGalleryServerProConfigSection().Core.DefaultVideoPlayerHeight;
			}
			else if ((mediaObject is GenericMediaObject) && (mediaObject.MimeType.TypeCategory == MimeTypeCategory.Other))
			{
				// We want to update the width and height only when the TypeCategory is Other. If we don't check for this, we might
				// assign a width and height to a corrupt JPG that is being treated as a GenericMediaObject.
				mediaObject.Original.Width = ConfigManager.GetGalleryServerProConfigSection().Core.DefaultGenericObjectWidth;
				mediaObject.Original.Height = ConfigManager.GetGalleryServerProConfigSection().Core.DefaultGenericObjectHeight;
			}
			else if (mediaObject is Audio)
			{
				mediaObject.Original.Width = ConfigManager.GetGalleryServerProConfigSection().Core.DefaultAudioPlayerWidth;
				mediaObject.Original.Height = ConfigManager.GetGalleryServerProConfigSection().Core.DefaultAudioPlayerHeight;
			}
		}

		private static bool DoesOriginalExceedOptimizedTriggers(IGalleryObject mediaObject)
		{
			// Note: This function also exists in the ImageOptimizedCreator class.

			// Test 1: Is the file size of the original greater than OptimizedImageTriggerSizeKB?
			bool isOriginalFileSizeGreaterThanTriggerSize = false;

			if (mediaObject.Original.FileSizeKB > _optimizedTriggerSizeKB)
			{
				isOriginalFileSizeGreaterThanTriggerSize = true;
			}

			// Test 2: Is the width or length of the original greater than the MaxOptimizedLength?
			bool isOriginalLengthGreaterThanMaxAllowedLength = false;

			int originalWidth = mediaObject.Original.Bitmap.Width;
			int originalHeight = mediaObject.Original.Bitmap.Height;
			if ((originalWidth > _optimizedMaxLength) || (originalHeight > _optimizedMaxLength))
			{
				isOriginalLengthGreaterThanMaxAllowedLength = true;
			}

			return (isOriginalFileSizeGreaterThanTriggerSize | isOriginalLengthGreaterThanMaxAllowedLength);
		}

		#endregion
	}
}

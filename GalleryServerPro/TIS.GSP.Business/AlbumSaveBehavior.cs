using System;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for persisting an album to the data store and file system.
	/// </summary>
	public class AlbumSaveBehavior : ISaveBehavior
	{
		IAlbum _albumObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="AlbumSaveBehavior"/> class.
		/// </summary>
		/// <param name="albumObject">The album object.</param>
		public AlbumSaveBehavior(IAlbum albumObject)
		{
			this._albumObject = albumObject;
		}

		/// <summary>
		/// Persist the object to which this behavior belongs to the data store. Also persist to the file system, if
		/// the object has a representation on disk, such as albums (stored as directories) and media objects (stored
		/// as files). New objects with ID = int.MinValue will have a new <see cref="IGalleryObject.Id"/> assigned
		/// and <see cref="IGalleryObject.IsNew"/> set to false.
		/// All validation should have taken place before calling this method.
		/// </summary>
		public void Save()
		{
			if (this._albumObject.IsVirtualAlbum)
				return; // Don't save virtual albums.

			// Must save to disk first, since the method queries properties that might be updated when it is
			// saved to the data store.
			PersistToFileSystemStore(this._albumObject);

			// Save to the data store.
			Factory.GetDataProvider().Album_Save(this._albumObject);

			// Update the album's thumbnail if necessary.
			//IAlbum parentAlbum = (IAlbum)this._albumObject.Parent;
			//if (parentAlbum.ThumbnailMediaObjectId == 0)
			//{
			//  Album.AssignAlbumThumbnail(parentAlbum, true, false);
			//}
		}

		/// <summary>
		/// Update the directory on disk with the current name and location of the album. A new directory is
		/// created for new albums, and the directory is moved to the location specified by FullPhysicalPath if
		/// that property is different than FullPhysicalPathOnDisk.
		/// </summary>
		/// <param name="album">The album to persist to disk.</param>
		private static void PersistToFileSystemStore(IAlbum album)
		{
			if (album.IsNew)
			{
				System.IO.Directory.CreateDirectory(album.FullPhysicalPath);

				// Create directory for thumbnail cache, if needed.
				string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPath, AppSetting.Instance.ThumbnailPath);
				if (thumbnailPath != album.FullPhysicalPath)
				{
					System.IO.Directory.CreateDirectory(thumbnailPath);
				}

				// Create directory for optimized image cache, if needed.
				string optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPath, AppSetting.Instance.OptimizedPath);
				if (optimizedPath != album.FullPhysicalPath)
				{
					System.IO.Directory.CreateDirectory(optimizedPath);
				}
			}
			else if (album.FullPhysicalPathOnDisk != album.FullPhysicalPath)
			{
				// We need to move the directory to its new location or change its name. Verify that the containing directory doesn't already
				// have a directory with the new name. If it does, alter it slightly to make it unique.
				System.IO.DirectoryInfo di = System.IO.Directory.GetParent(album.FullPhysicalPath);

				string newDirName = HelperFunctions.ValidateDirectoryName(di.FullName, album.DirectoryName);
				if (album.DirectoryName != newDirName)
				{
					album.DirectoryName = newDirName;
				}

				// Now we are guaranteed to have a "safe" directory name, so proceed with the move/rename.
				System.IO.Directory.Move(album.FullPhysicalPathOnDisk, album.FullPhysicalPath);

				// Rename directory for thumbnail cache, if needed.
				string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPath, AppSetting.Instance.ThumbnailPath);
				if (thumbnailPath != album.FullPhysicalPath)
				{
					string currentThumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPathOnDisk, AppSetting.Instance.ThumbnailPath);

					RenameDirectory(currentThumbnailPath, thumbnailPath);
				}

				// Rename directory for optimized image cache, if needed.
				string optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPath, AppSetting.Instance.OptimizedPath);
				if (optimizedPath != album.FullPhysicalPath)
				{
					string currentOptimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(album.FullPhysicalPathOnDisk, AppSetting.Instance.OptimizedPath);

					RenameDirectory(currentOptimizedPath, optimizedPath);
				}
			}
		}

		private static void RenameDirectory(string oldDirPath, string newDirPath)
		{
			if (System.IO.Directory.Exists(oldDirPath))
			{
				System.IO.Directory.Move(oldDirPath, newDirPath);
			}
			else if (!System.IO.Directory.Exists(newDirPath))
			{
				System.IO.Directory.CreateDirectory(newDirPath);
			}
		}
	}
}

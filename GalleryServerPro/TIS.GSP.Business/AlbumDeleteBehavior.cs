using System;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for deleting an album from the data store.
	/// </summary>
	public class AlbumDeleteBehavior : IDeleteBehavior
	{
		IAlbum _albumObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="AlbumDeleteBehavior"/> class.
		/// </summary>
		/// <param name="albumObject">The album object.</param>
		public AlbumDeleteBehavior(IAlbum albumObject)
		{
			this._albumObject = albumObject;
		}

		/// <summary>
		/// Delete the object to which this behavior belongs from the data store and optionally the file system.
		/// </summary>
		/// <param name="deleteFromFileSystem">Indicates whether to delete the file or directory from the hard drive in addition
		/// to deleting it from the data store. When true, the object is deleted from both the data store and hard drive. When
		/// false, only the record in the data store is deleted.</param>
		public void Delete(bool deleteFromFileSystem)
		{
			if (deleteFromFileSystem)
			{
				DeleteFromFileSystem(this._albumObject);
			}

			Factory.GetDataProvider().Album_Delete(this._albumObject);
		}

		private static void DeleteFromFileSystem(IAlbum album)
		{
			string albumPath = album.FullPhysicalPath;
			if (album.IsRootAlbum)
			{
				DeleteRootAlbumDirectory(albumPath);
			}
			else
			{
				DeleteAlbumDirectory(albumPath);
			}
		}

		private static void DeleteAlbumDirectory(string albumPath)
		{
			// Delete the directory (recursive).
			if (System.IO.Directory.Exists(albumPath))
			{
				System.IO.Directory.Delete(albumPath, true);
			}

			// Delete files and folders from thumbnail cache, if needed.
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPath, AppSetting.Instance.ThumbnailPath);
			if (thumbnailPath != albumPath)
			{
				if (System.IO.Directory.Exists(thumbnailPath))
				{
					System.IO.Directory.Delete(thumbnailPath, true);
				}
			}

			// Delete files and folders from optimized image cache, if needed.
			string optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPath, AppSetting.Instance.OptimizedPath);
			if (optimizedPath != albumPath)
			{
				if (System.IO.Directory.Exists(optimizedPath))
				{
					System.IO.Directory.Delete(optimizedPath, true);
				}
			}
		}

		private static void DeleteRootAlbumDirectory(string albumPath)
		{
			// User is trying to delete the root album. We only want to delete any subdirectories and files,
			// but not the folder itself.
			DeleteChildFilesAndDirectories(albumPath);

			// Delete files and folders from thumbnail cache, if needed.
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPath, AppSetting.Instance.ThumbnailPath);
			if (thumbnailPath != albumPath)
			{
				DeleteChildFilesAndDirectories(thumbnailPath);
			}

			// Delete files and folders from optimized image cache, if needed.
			string optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPath, AppSetting.Instance.OptimizedPath);
			if (optimizedPath != albumPath)
			{
				DeleteChildFilesAndDirectories(optimizedPath);
			}
		}

		private static void DeleteChildFilesAndDirectories(string albumPath)
		{
			string[] aFiles = System.IO.Directory.GetFiles(albumPath);
			string[] aFolders = System.IO.Directory.GetDirectories(albumPath);

			// Delete each directory
			for (int i = 0; i <= aFolders.GetUpperBound(0); i++)
			{
				System.Diagnostics.Debug.Assert(System.IO.Directory.Exists(aFolders[i]));
				System.IO.Directory.Delete(aFolders[i], true);
			}

			// Delete each file.
			for (int i = 0; i <= aFiles.GetUpperBound(0); i++)
			{
				System.Diagnostics.Debug.Assert(System.IO.File.Exists(aFiles[i]));
				System.IO.File.Delete(aFiles[i]);
			}
		}

	}
}

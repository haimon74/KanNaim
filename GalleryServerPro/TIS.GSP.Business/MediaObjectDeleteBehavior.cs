using System;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for deleting a media object from the data store.
	/// </summary>
	public class MediaObjectDeleteBehavior : IDeleteBehavior
	{
		IGalleryObject _galleryObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaObjectDeleteBehavior"/> class.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		public MediaObjectDeleteBehavior(IGalleryObject galleryObject)
		{
			this._galleryObject = galleryObject;
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
				DeleteFromFileSystem(this._galleryObject);
			}

			Factory.GetDataProvider().MediaObject_Delete(this._galleryObject);
		}

		private static void DeleteFromFileSystem(IGalleryObject galleryObject)
		{
			// Delete thumbnail file.
			if (System.IO.File.Exists(galleryObject.Thumbnail.FileNamePhysicalPath))
			{
				System.IO.File.Delete(galleryObject.Thumbnail.FileNamePhysicalPath);
			}

			// Delete optimized file.
			if (System.IO.File.Exists(galleryObject.Optimized.FileNamePhysicalPath))
			{
				System.IO.File.Delete(galleryObject.Optimized.FileNamePhysicalPath);
			}

			// Delete original file.
			if (System.IO.File.Exists(galleryObject.Original.FileNamePhysicalPath))
			{
				System.IO.File.Delete(galleryObject.Original.FileNamePhysicalPath);
			}
		}
	}
}

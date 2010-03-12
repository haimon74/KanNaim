using System;
using System.Globalization;
using System.IO;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Properties;
using GalleryServerPro.Business.Metadata;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// The Image class represents a media object within Gallery Server Pro that is an image.
	/// </summary>
	public class Image : GalleryObject
	{
		#region Private Fields

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of an <see cref="Image"/> object.
		/// </summary>
		/// <param name="imageFile">A <see cref="System.IO.FileInfo"/> object containing the original image for this object.</param>
		/// <param name="parentAlbum">The album that contains this object. May be null.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException">Thrown when 
		/// <paramref name="imageFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="imageFile"/> is specified (not null) and its file extension does not correspond to an image MIME
		/// type, as determined by the MIME type definition in the configuration file.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException">Thrown when the 
		/// .NET Framework is unable to load an image file into the <see cref="System.Drawing.Bitmap"/> class. This is 
		/// probably because it is corrupted, not an image supported by the .NET Framework, or the server does not have 
		/// enough memory to process the image. The file cannot, therefore, be handled using the <see cref="Image"/> 
		/// class; use <see cref="GenericMediaObject"/> instead. This exception is thrown only when <paramref name="imageFile"/>
		/// is specified (non-null).</exception>
		/// <remarks>This constructor does not verify that <paramref name="imageFile"/> refers to a file type that is enabled in the 
		/// configuration file.</remarks>
		internal Image(System.IO.FileInfo imageFile, IAlbum parentAlbum)
			: this(int.MinValue, parentAlbum != null ? parentAlbum.Id : int.MinValue, parentAlbum, string.Empty, string.Empty, string.Empty,
			       int.MinValue, int.MinValue, int.MinValue, string.Empty, int.MinValue, int.MinValue,
			       int.MinValue, string.Empty, int.MinValue, int.MinValue, int.MinValue, int.MinValue,
			       String.Empty, DateTime.MinValue, String.Empty, DateTime.MinValue, parentAlbum != null ? parentAlbum.IsPrivate : false, false, imageFile)
		{
		}

		/// <summary>
		/// Initializes a new instance of an <see cref="Image" /> object.
		/// </summary>
		/// <param name="id">The <see cref="GalleryObject.Id">ID</see> that uniquely identifies this object. Specify int.MinValue for a new object.</param>
		/// <param name="parentId">The <see cref="IGalleryObject.Id">ID</see> of the album that contains this object.</param>
		/// <param name="parentAlbum">The album that contains this object.</param>
		/// <param name="title">The title of this image.</param>
		/// <param name="hashKey">The hash key that uniquely identifies the original image file.</param>
		/// <param name="thumbnailFilename">The filename of the thumbnail image.</param>
		/// <param name="thumbnailWidth">The width (px) of the thumbnail image.</param>
		/// <param name="thumbnailHeight">The height (px) of the thumbnail image.</param>
		/// <param name="thumbnailSizeKb">The size (KB) of the thumbnail image.</param>
		/// <param name="optimizedFilename">The filename of the optimized image.</param>
		/// <param name="optimizedWidth">The width (px) of the optimized image.</param>
		/// <param name="optimizedHeight">The height (px) of the optimized image.</param>
		/// <param name="optimizedSizeKb">The size (KB) of the optimized image.</param>
		/// <param name="originalFilename">The filename of the original image.</param>
		/// <param name="originalWidth">The width (px) of the original image.</param>
		/// <param name="originalHeight">The height (px) of the original image.</param>
		/// <param name="originalSizeKb">The size (KB) of the original image.</param>
		/// <param name="sequence">An integer that represents the order in which this image should appear when displayed.</param>
		/// <param name="createdByUsername">The user name of the account that originally added this object to the data store.</param>
		/// <param name="dateAdded">The date this image was added to the data store.</param>
		/// <param name="lastModifiedByUsername">The user name of the account that last modified this object.</param>
		/// <param name="dateLastModified">The date this object was last modified.</param>
		/// <param name="isPrivate">Indicates whether this object should be hidden from un-authenticated (anonymous) users.</param>
		/// <param name="isInflated">A bool indicating whether this object is fully inflated.</param>
		/// <param name="imageFile">A <see cref="FileInfo"/> object containing the original image for this object. This is intended to be 
		/// specified when creating a new media object from a file. Specify null when instantiating an object for an existing database
		/// record.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException">Thrown when
		/// <paramref name="imageFile"/> is specified (not null) and the file it refers to is not in the same directory
		/// as the parent album's directory.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="imageFile"/> is specified (not null) and its file extension does not correspond to an image MIME
		/// type, as determined by the MIME type definition in the configuration file.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException">Thrown when the 
		/// .NET Framework is unable to load an image file into the <see cref="System.Drawing.Bitmap"/> class. This is 
		/// probably because it is corrupted, not an image supported by the .NET Framework, or the server does not have 
		/// enough memory to process the image. The file cannot, therefore, be handled using the <see cref="Image"/> 
		/// class; use <see cref="GenericMediaObject"/> instead. This exception is thrown only when <paramref name="imageFile"/>
		/// is specified (non-null).</exception>
		/// <remarks>This constructor does not verify that <paramref name="imageFile"/> refers to a file type that is enabled in the 
		/// configuration file.</remarks>
		internal Image(int id, int parentId, IAlbum parentAlbum, string title, string hashKey, string thumbnailFilename, int thumbnailWidth, int thumbnailHeight, int thumbnailSizeKb, string optimizedFilename, int optimizedWidth, int optimizedHeight, int optimizedSizeKb, string originalFilename, int originalWidth, int originalHeight, int originalSizeKb, int sequence, string createdByUsername, DateTime dateAdded, string lastModifiedByUsername, DateTime dateLastModified, bool isPrivate, bool isInflated, FileInfo imageFile)
		{
			System.Diagnostics.Debug.Assert(((originalFilename.Length > 0) || (imageFile != null)), "Invalid Image constructor arguments: The original filename or a FileInfo reference to the original file must be passed to the Image constructor.");

			this.Id = id;
			this.Title = title;
			this.Sequence = sequence;
			this.Hashkey = hashKey;
			this.CreatedByUserName = createdByUsername;
			this.DateAdded = dateAdded;
			this.LastModifiedByUserName = lastModifiedByUsername;
			this.DateLastModified = dateLastModified;
			this.IsPrivate = isPrivate;

			// Set parent to album instance if present, otherwise create default Album instance if the album ID
			// is specified. If neither are specified, do not set parent (client will, however, need to set the
			// parent before saving).
			if (parentAlbum != null)
				this.Parent = parentAlbum;
			else if (parentId != int.MinValue)
				this.Parent = Factory.CreateAlbumInstance(parentId);

			string parentPhysicalPath = this.Parent.FullPhysicalPathOnDisk;

			// Thumbnail image
			this.Thumbnail = DisplayObject.CreateInstance(this, thumbnailFilename, thumbnailWidth, thumbnailHeight, DisplayObjectType.Thumbnail, new ImageThumbnailCreator(this));
			this.Thumbnail.FileSizeKB = thumbnailSizeKb;
			if (thumbnailFilename.Length > 0)
			{
				// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
				string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(parentPhysicalPath, AppSetting.Instance.ThumbnailPath);
				this.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, thumbnailFilename);
			}

			// Optimized image
			this.Optimized = DisplayObject.CreateInstance(this, optimizedFilename, optimizedWidth, optimizedHeight, DisplayObjectType.Optimized, new ImageOptimizedCreator(this));
			this.Optimized.FileSizeKB = optimizedSizeKb;
			if (optimizedFilename.Length > 0)
			{
				// Calcululate the full file path to the optimized image. If the optimized filename is equal to the original filename, then no
				// optimized version exists, and we'll just point to the original. If the names are different, then there is a separate optimized
				// image file, and it is stored in either the album's physical path or an alternate location (if optimizedPath config setting is specified).
				string optimizedPath = parentPhysicalPath;

				if (optimizedFilename != originalFilename)
					optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(parentPhysicalPath, AppSetting.Instance.OptimizedPath);

				this.Optimized.FileNamePhysicalPath = System.IO.Path.Combine(optimizedPath, optimizedFilename);
			}

			// Original image
			this.Original = DisplayObject.CreateInstance(this, originalFilename, originalWidth, originalHeight, DisplayObjectType.Original, new ImageOriginalCreator(this));
			this.Original.ExternalHtmlSource = String.Empty;
			this.Original.ExternalType = MimeTypeCategory.NotSet;

			if (imageFile != null)
			{
				this.Hashkey = HelperFunctions.GetHashKeyUnique(imageFile);
				this.Original.FileInfo = imageFile; // Will throw InvalidMediaObjectException if the file's directory is not the same as the album's directory.

				if (this.Original.MimeType.TypeCategory != MimeTypeCategory.Image)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedMediaObjectTypeException(this.Original.FileInfo);
				}

				try
				{
					this.Original.Width = this.Original.Bitmap.Width;
					this.Original.Height = this.Original.Bitmap.Height;
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException)
				{
					// .NET Framework can't generate a bitmap from this image, so change the thumbnail creator to a generic one and null out
					// the optimized object.
					this.Original.Dispose();
					this.Parent.Remove(this);
					MediaObjectHashKeys.Instance.HashKeys.Remove(this.Hashkey);
					throw;
				}

				int fileSize = (int)(imageFile.Length / 1024);
				this.Original.FileSizeKB = (fileSize < 1 ? 1 : fileSize); // Very small files should be 1, not 0.

				// Get metadata from the image file.
				if (Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.EnableImageMetadata)
				{
					try
					{
						MediaObjectMetadataExtractor metadata = new MediaObjectMetadataExtractor(imageFile.FullName);
						this.MetadataItems.AddRange(metadata.GetGalleryObjectMetadataItemCollection());
					}
					catch (OutOfMemoryException)
					{
						// Normally, the Dispose method is called during the Image_Saved event. But when we get this exception, it
						// never executes and therefore doesn't release the file lock. So we explicitly do so here and then 
						// throw a new exception.
						this.Original.Dispose();
						this.Parent.Remove(this);
						throw new GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException(this);
					}
				}
				this.RegenerateMetadataOnSave = false;

				// If there is a title in the metadata, use that for the title of this media object. Otherwise, use the filename.
				if (String.IsNullOrEmpty(title))
				{
					IGalleryObjectMetadataItem metadataTitle;
					if (this.MetadataItems.TryGetMetadataItem(FormattedMetadataItemName.Title, out metadataTitle))
					{
						this.Title = metadataTitle.Value;
					}
					else
					{
						this.Title = imageFile.Name;
					}
				}
			}
			else
			{
				this.Original.FileNamePhysicalPath = System.IO.Path.Combine(parentPhysicalPath, originalFilename);
				this.Original.FileSizeKB = originalSizeKb;
			}

			this.SaveBehavior = Factory.GetMediaObjectSaveBehavior(this);
			this.DeleteBehavior = Factory.GetMediaObjectDeleteBehavior(this);

			this.IsInflated = isInflated;

			// Setting the previous properties has caused HasChanges = true, but we don't want this while
			// we're instantiating a new object. Reset to false.
			this.HasChanges = false;

			// Set up our event handlers.
			//this.Saving += new EventHandler(Image_Saving); // Don't need
			this.Saved += Image_Saved;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Inflate the current object by loading all properties from the data store. If the object is already inflated 
		/// (<see cref="GalleryObject.IsInflated"/>=true), no action is taken.
		/// </summary>
		public override void Inflate()
		{
			// If this is not a new object, and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((!this.IsNew) && (!this.IsInflated))
			{
				Factory.LoadImageInstance(this);

				if ((!this.IsInflated) || (this.HasChanges))
					throw new System.InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Image_Inflate_Ex_Msg, this.IsInflated, this.HasChanges));
			}
		}

		/// <summary>
		/// Remove the original high resolution image, if this object has one. The original file is deleted and the optimized
		/// version is renamed to match that of the original. Several properties are updated to reflect the change. Although
		/// the image files are modified on disk, the image properties are not persisted to the database. It is expected that
		/// the caller performs this step.
		/// </summary>
		public void DeleteHiResImage()
		{
			// Delete the hi-res image.
			if (!this.Original.FileName.Equals(this.Optimized.FileName, StringComparison.OrdinalIgnoreCase))
			{
				string originalPath = this.Original.FileNamePhysicalPath;
				string originalExtension = Path.GetExtension(originalPath); // Ex: .bmp
				string optimizedExtension = Path.GetExtension(this.Optimized.FileNamePhysicalPath); // Ex: .jpg

				// Delete the original this file
				File.Delete(originalPath);

				if (!originalExtension.Equals(optimizedExtension, StringComparison.OrdinalIgnoreCase))
				{
					// The original has a different file extension than the optimized, so update the original file name with 
					// the extension from the optimized file. For example, this can happen when the original does not end with
					// the .jpeg extension (it may be JPG, BMP, TIF, etc).
					originalPath = Path.ChangeExtension(originalPath, optimizedExtension);

					// Now validate that the new path is not already used by an existing file. For example, we might be renaming
					// zOpt_photo.jpeg to photo.jpg. If photo.jpg is already in use, we need to change it to something else.
					string dirPath = Path.GetDirectoryName(originalPath);
					string filename = Path.GetFileName(originalPath);
					string newFilename = HelperFunctions.ValidateFileName(dirPath, filename);

					if (!newFilename.Equals(filename, StringComparison.OrdinalIgnoreCase))
						originalPath = Path.Combine(dirPath, newFilename);
				}

				// Rename the optimized file to the original file. This is required because
				// optimized file names can be slightly different than the original file names. For example, optimized thiss
				// are prefixed with "zOpt_" and are always a JPEG file type, while the original does not have a special prefix
				// and may be BMP, TIF, etc.
				File.Move(this.Optimized.FileNamePhysicalPath, originalPath);

				this.Original.Dispose();

				this.Original.FileInfo = new System.IO.FileInfo(originalPath);
				//this.Original.FileName = Path.GetFileName(originalPath);
				//this.Original.FileNamePhysicalPath = originalPath;

				this.Hashkey = HelperFunctions.GetHashKeyUnique(this.Original.FileInfo);

				this.Optimized.FileInfo = this.Original.FileInfo;
				//this.Optimized.FileName = this.Original.FileName;
				//this.Optimized.FileNamePhysicalPath = this.Original.FileNamePhysicalPath;

				this.Original.Width = this.Optimized.Width;
				this.Original.Height = this.Optimized.Height;
				this.Original.FileSizeKB = this.Optimized.FileSizeKB;
			}
		}
		#endregion

		#region Protected Methods

		/// <summary>
		/// This method verifies the optimized image maps to an existing file on disk. If not, set the
		///  <see cref="GalleryObject.RegenerateThumbnailOnSave" />
		/// property to true so that the thumbnail image is created during the <see cref="GalleryObject.Save" /> operation.
		/// </summary>
		protected override void CheckForOptimizedImage()
		{
			if (!System.IO.File.Exists(this.Optimized.FileNamePhysicalPath))
			{
				this.RegenerateOptimizedOnSave = true;
			}
		}

		#endregion

		#region Event Handlers

		void Image_Saved(object sender, EventArgs e)
		{
			// This event is fired when the Save() method is called, after all data is saved.

			#region Assign DisplayObject.MediaObjectId

			// If the MediaObjectId has not yet been assigned, do so now. This will occur after a media object is first
			// saved, since that is when the ID is generated.
			if (this.Thumbnail.MediaObjectId == int.MinValue)
			{
				this.Thumbnail.MediaObjectId = this.Id;
			}

			if (this.Optimized.MediaObjectId == int.MinValue)
			{
				this.Optimized.MediaObjectId = this.Id;
			}

			if (this.Original.MediaObjectId == int.MinValue)
			{
				this.Original.MediaObjectId = this.Id;
			}

			#endregion

			// Dispose of the bitmap of the original file. This bitmap is used only during saving, so we'll call Dispose()
			// here to free the resources. (To ensure all scenarios are covered, we also call Dispose() in the 
			// destructor of the DisplayObject.)
			this.Original.Dispose();
		}

		#endregion
	}
}

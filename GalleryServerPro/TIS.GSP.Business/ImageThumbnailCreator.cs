using System;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for creating and saving the thumbnail image files associated with <see cref="Image" /> gallery objects.
	/// </summary>
	public class ImageThumbnailCreator : IDisplayObjectCreator
	{
		private Image _imageObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageThumbnailCreator"/> class.
		/// </summary>
		/// <param name="imageObject">The image object.</param>
		public ImageThumbnailCreator(Image imageObject)
		{
			this._imageObject = imageObject;
		}

		/// <summary>
		/// Generate the file for this display object and save it to the file system. The routine may decide that
		/// a file does not need to be generated, usually because it already exists. However, it will always be
		/// created if the relevant flag is set on the parent IGalleryObject. (Example: If
		/// IGalleryObject.OverwriteThumbnail = true, the thumbnail file will always be created.) No data is
		/// persisted to the data store.
		/// </summary>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException">Thrown when the original
		/// image associated with this gallery object cannot be loaded into a <see cref="Bitmap"/> class because it is an 
		/// incompatible or corrupted image type.</exception>
		public void GenerateAndSaveFile()
		{
			// If necessary, generate and save the thumbnail version of the original image.
			if (!(IsThumbnailImageRequired()))
			{
				return; // No thumbnail image required.
			}

			// All thumbnails should be JPEG format. (My tests show that making GIFs from GIF originals resulted in poor quality thumbnail
			// GIFs, so all thumbnails are JPEG, even those from GIFs.)
			ImageFormat imgFormat = ImageFormat.Jpeg;

			// Determine path where thumbnail should be saved. If no thumbnail path is specified in the config file,
			// use the same directory as the original.
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(this._imageObject.Original.FileInfo.DirectoryName, AppSetting.Instance.ThumbnailPath);

			// Determine name of new file and, for a new media object, makes sure it is unique in the directory. (Example: If original = puppy.jpg, thumbnail = zThumb_puppy.jpg)
			string newFilename = this.GenerateNewFilename(thumbnailPath, imgFormat);

			// Combine the directory and filename to create the full path to the file.
			string newFilePath = Path.Combine(thumbnailPath, newFilename);

			// Don't call Dispose() on originalBitmap unless an exception occurs. That is because it is a reference to a 
			// bitmap of the original image, and there is code in the Image class's Saved event that calls Dispose().
			Bitmap originalBitmap = null;
			int newWidth, newHeight;
			try
			{
				// Get reference to the bitmap from which the optimized image will be generated.
				originalBitmap = this._imageObject.Original.Bitmap;
				ImageHelper.CalculateThumbnailWidthAndHeight(originalBitmap.Width, originalBitmap.Height, out newWidth, out newHeight, false);

				// Get JPEG quality value (0 - 100). This is ignored if imgFormat = GIF.
				int jpegQuality = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailImageJpegQuality;

				// Generate the new image and save to disk.
				ImageHelper.SaveImageFile(originalBitmap, newFilePath, imgFormat, newWidth, newHeight, jpegQuality);
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException)
			{
				if (originalBitmap != null)
					originalBitmap.Dispose();

				throw;
			}

			this._imageObject.Thumbnail.Width = newWidth;
			this._imageObject.Thumbnail.Height = newHeight;
			this._imageObject.Thumbnail.FileName = newFilename;
			this._imageObject.Thumbnail.FileNamePhysicalPath = newFilePath;

			int fileSize = (int)(this._imageObject.Thumbnail.FileInfo.Length / 1024);

			this._imageObject.Thumbnail.FileSizeKB = (fileSize < 1 ? 1 : fileSize); // Very small files should be 1, not 0.
		}

		private bool IsThumbnailImageRequired()
		{
			// We must create a thumbnail image in the following circumstances:
			// 1. The file corresponding to a previously created thumbnail image file does not exist.
			//    OR
			// 2. The overwrite flag is true.
			//    OR
			// 3. There is a request to rotate the image.

			bool thumbnailImageMissing = IsThumbnailImageFileMissing(); // Test 1

			bool overwriteFlag = this._imageObject.RegenerateThumbnailOnSave; // Test 2

			bool rotateIsRequested = (this._imageObject.Rotation != RotateFlipType.RotateNoneFlipNone);

			return (thumbnailImageMissing || overwriteFlag || rotateIsRequested);
		}

		private bool IsThumbnailImageFileMissing()
		{
			// Does the thumbnail image file exist? (Maybe it was accidentally deleted or moved by the user,
			// or maybe it's a new object.)
			bool thumbnailImageExists = false;
			if (File.Exists(this._imageObject.Thumbnail.FileNamePhysicalPath))
			{
				// Thumbnail image file exists.
				thumbnailImageExists = true;
			}

			bool thumbnailImageIsMissing = !thumbnailImageExists;

			return thumbnailImageIsMissing;
		}

		/// <summary>
		/// Determine name of new file and, for a new media object, makes sure it is unique in the directory. (Example: If original = puppy.jpg, 
		/// thumbnail = zThumb_puppy.jpg)
		/// </summary>
		/// <param name="thumbnailPath">The path to the directory where the thumbnail file is to be created.</param>
		/// <param name="imgFormat">The image format of the thumbnail.</param>
		/// <returns>Returns the name of the new thumbnail file name and, for a new media object, makes sure it is unique in the directory.</returns>
		private string GenerateNewFilename(string thumbnailPath, ImageFormat imgFormat)
		{
			string filenamePrefix = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailFileNamePrefix;
			string nameWithoutExtension = Path.GetFileNameWithoutExtension(this._imageObject.Original.FileInfo.Name);
			string thumbnailFilename = string.Format(CultureInfo.CurrentCulture, "{0}{1}.{2}", filenamePrefix, nameWithoutExtension, imgFormat.ToString().ToLower(CultureInfo.CurrentCulture));
			
			if (this._imageObject.IsNew)
			{
				// Since this is a new media object, make sure the file is unique in the directory. For existing media objects, we'll just
				// overwrite the file (if it exists).
				thumbnailFilename = HelperFunctions.ValidateFileName(thumbnailPath, thumbnailFilename);
			}

			return thumbnailFilename;
		}
	}
}

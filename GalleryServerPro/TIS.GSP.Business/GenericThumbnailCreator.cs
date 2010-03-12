using System;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Properties;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for creating and saving the thumbnail image files associated with <see cref="GenericMediaObject" /> gallery objects.
	/// </summary>
	public class GenericThumbnailCreator : IDisplayObjectCreator
	{
		private IGalleryObject _galleryObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericThumbnailCreator"/> class.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		public GenericThumbnailCreator(IGalleryObject galleryObject)
		{
			this._galleryObject = galleryObject;
		}

		/// <summary>
		/// Generate the thumbnail image for this display object and save it to the file system. The routine may decide that
		/// a file does not need to be generated, usually because it already exists. However, it will always be
		/// created if the relevant flag is set on the parent <see cref="IGalleryObject" />. (Example: If
		/// <see cref="IGalleryObject.RegenerateThumbnailOnSave" /> = true, the thumbnail file will always be created.) No data is
		/// persisted to the data store.
		/// </summary>
		public void GenerateAndSaveFile()
		{
			// If necessary, generate and save the thumbnail version of the original image.
			if (!(IsThumbnailImageRequired()))
			{
				return; // No thumbnail image required.
			}

			// All thumbnails should be JPEG format. (Making GIFs from GIF originals resulted in poor quality thumbnail
			// GIFs, so all thumbnails are JPEG, even those from GIFs.)
			ImageFormat imgFormat = ImageFormat.Jpeg;

			// Determine path where thumbnail should be saved. If no thumbnail path is specified in the config file,
			// use the same directory as the original.
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(this._galleryObject.Original.FileInfo.DirectoryName, AppSetting.Instance.ThumbnailPath);

			// Determine name of new file and make sure it is unique in the directory. (Example: If original = puppy.jpg, thumbnail = zThumb_puppy.jpg)
			string newFilename = HelperFunctions.ValidateFileName(thumbnailPath, this.GenerateNewFilename(imgFormat));
			
			// Combine the directory and filename to create the full path to the file.
			string newFilePath = Path.Combine(thumbnailPath, newFilename);

			// Get reference to the bitmap from which the thumbnail image will be generated.
			int newWidth, newHeight;
			using (Bitmap originalBitmap = GetGenericThumbnailBitmap(this._galleryObject.MimeType))
			{
				ImageHelper.CalculateThumbnailWidthAndHeight(originalBitmap.Width, originalBitmap.Height, out newWidth, out newHeight, true);

				// Get JPEG quality value (0 - 100). This is ignored if imgFormat = GIF.
				int jpegQuality = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailImageJpegQuality;

				// Generate the new image and save to disk.
				GalleryServerPro.Business.ImageHelper.SaveImageFile(originalBitmap, newFilePath, imgFormat, newWidth, newHeight, jpegQuality);
			}

			this._galleryObject.Thumbnail.Width = newWidth;
			this._galleryObject.Thumbnail.Height = newHeight;
			this._galleryObject.Thumbnail.FileName = newFilename;
			this._galleryObject.Thumbnail.FileNamePhysicalPath = newFilePath;

			int fileSize = (int)(this._galleryObject.Thumbnail.FileInfo.Length / 1024);

			this._galleryObject.Thumbnail.FileSizeKB = (fileSize < 1 ? 1 : fileSize); // Very small files should be 1, not 0.
		}

		private static Bitmap GetGenericThumbnailBitmap(IMimeType mimeType)
		{
			Bitmap thumbnailBitmap = null;

			switch (mimeType.MajorType.ToUpperInvariant())
			{
				case "AUDIO": thumbnailBitmap = Resources.GenericThumbnailImage_Audio; break;
				case "VIDEO": thumbnailBitmap = Resources.GenericThumbnailImage_Video; break;
				case "IMAGE": thumbnailBitmap = Resources.GenericThumbnailImage_Image; break;
				case "APPLICATION": thumbnailBitmap = GetGenericThumbnailBitmapByFileExtension(mimeType.Extension); break;
				default: thumbnailBitmap = Resources.GenericThumbnailImage_Unknown; break;
			}

			return thumbnailBitmap;
		}

		private static Bitmap GetGenericThumbnailBitmapByFileExtension(string fileExtension)
		{
			Bitmap thumbnailBitmap = null;

			switch (fileExtension)
			{
				case ".doc":
				case ".dot":
				case ".docm":
				case ".dotm":
				case ".dotx":
				case ".docx": thumbnailBitmap = Resources.GenericThumbnailImage_Doc; break;
				case ".xls":
				case ".xlam":
				case ".xlsb":
				case ".xlsm":
				case ".xltm":
				case ".xltx":
				case ".xlsx": thumbnailBitmap = Resources.GenericThumbnailImage_Excel; break;
				case ".ppt":
				case ".pps":
				case ".pptx":
				case ".potm":
				case ".ppam":
				case ".ppsm": thumbnailBitmap = Resources.GenericThumbnailImage_PowerPoint; break;
				case ".pdf": thumbnailBitmap = Resources.GenericThumbnailImage_PDF; break;
				default: thumbnailBitmap = Resources.GenericThumbnailImage_Unknown; break;
			}
			return thumbnailBitmap;
		}

		private bool IsThumbnailImageRequired()
		{
			// We must create a thumbnail image in the following circumstances:
			// 1. The file corresponding to a previously created thumbnail image file does not exist.
			//    OR
			// 2. The overwrite flag is true.

			bool thumbnailImageMissing = IsThumbnailImageFileMissing(); // Test 1

			bool overwriteFlag = this._galleryObject.RegenerateThumbnailOnSave; // Test 2

			return (thumbnailImageMissing || overwriteFlag);
		}

		private bool IsThumbnailImageFileMissing()
		{
			// Does the thumbnail image file exist? (Maybe it was accidentally deleted or moved by the user,
			// or maybe it's a new object.)
			bool thumbnailImageExists = false;
			bool objectExistsInDataStore = !this._galleryObject.IsNew;

			if (objectExistsInDataStore)
			{
				if (File.Exists(this._galleryObject.Thumbnail.FileNamePhysicalPath))
				{
					// Thumbnail image file exists.
					thumbnailImageExists = true;
				}
			}

			bool thumbnailImageIsMissing = !thumbnailImageExists;

			return thumbnailImageIsMissing;
		}

		private string GenerateNewFilename(ImageFormat imgFormat)
		{
			string filenamePrefix = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailFileNamePrefix;
			string nameWithoutExtension = Path.GetFileNameWithoutExtension(this._galleryObject.Original.FileInfo.Name);
			string thumbnailFilename = string.Format(CultureInfo.CurrentCulture, "{0}{1}.{2}", filenamePrefix, nameWithoutExtension, imgFormat.ToString().ToLower(CultureInfo.CurrentCulture));

			return thumbnailFilename;
		}
	}
}
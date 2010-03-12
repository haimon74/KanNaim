using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using GalleryServerPro.Business.Interfaces;
using System.Globalization;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for creating and saving the thumbnail image files associated with <see cref="Image" /> gallery objects.
	/// </summary>
	public class ImageOptimizedCreator : IDisplayObjectCreator
	{
		private Image _imageObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageOptimizedCreator"/> class.
		/// </summary>
		/// <param name="imageObject">The image object.</param>
		public ImageOptimizedCreator(Image imageObject)
		{
			this._imageObject = imageObject;
		}

		/// <summary>
		/// Generate the file for this display object and save it to the file system. The routine may decide that
		/// a file does not need to be generated, usually because it already exists. However, it will always be
		/// created if the relevant flag is set on the parent <see cref="IGalleryObject" />. (Example: If
		/// <see cref="IGalleryObject.RegenerateThumbnailOnSave" /> = true, the thumbnail file will always be created.) No data is
		/// persisted to the data store.
		/// </summary>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException">Thrown when the original
		/// image associated with this gallery object cannot be loaded into a <see cref="Bitmap"/> class because it is an 
		/// incompatible or corrupted image type.</exception>
		public void GenerateAndSaveFile()
		{
			// If necessary, generate and save the optimized version of the original image.
			if (!(IsOptimizedImageRequired()))
			{
				bool rotateIsRequested = (this._imageObject.Rotation != RotateFlipType.RotateNoneFlipNone);

				if (rotateIsRequested || ((this._imageObject.IsNew) && (String.IsNullOrEmpty(this._imageObject.Optimized.FileName))))
				{
					// One of the following is true:
					// 1. The original is being rotated and there isn't a separate optimized image.
					// 2. This is a new object that doesn't need a separate optimized image.
					// In either case, set the optimized properties equal to the original properties.
					this._imageObject.Optimized.FileName = this._imageObject.Original.FileName;
					this._imageObject.Optimized.Width = this._imageObject.Original.Width;
					this._imageObject.Optimized.Height = this._imageObject.Original.Height;
					this._imageObject.Optimized.FileSizeKB = this._imageObject.Original.FileSizeKB;
				}
				return; // No optimized image required.
			}

			// All optimized images should be JPEG format. (Making GIFs from GIF originals resulted in poor quality images
			// GIFs, so we'll create JPEGs, even those from GIFs.)
			ImageFormat imgFormat = ImageFormat.Jpeg;

			// Determine path where optimized image should be saved. If no optimized path is specified in the config file,
			// use the same directory as the original.
			string optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(this._imageObject.Original.FileInfo.DirectoryName, AppSetting.Instance.OptimizedPath);

			// Determine name of new file and, for a new media object, makes sure it is unique in the directory. (Example: If original = puppy.jpg, thumbnail = zOpt_puppy.jpg)
			string newFilename = this.GenerateNewFilename(optimizedPath, imgFormat);

			string newFilePath = Path.Combine(optimizedPath, newFilename);

			// Don't call Dispose() on originalBitmap unless an exception occurs. That is because it is a reference to a 
			// bitmap of the original image, and there is code in the Image class's Saved event that calls Dispose().
			Bitmap originalBitmap = null;
			int newWidth, newHeight;
			try
			{
				// Get reference to the bitmap from which the optimized image will be generated.
				originalBitmap = this._imageObject.Original.Bitmap;
				ImageHelper.CalculateOptimizedWidthAndHeight(originalBitmap, out newWidth, out newHeight);

				// Get JPEG quality value (0 - 100). This is ignored if imgFormat = GIF.
				int jpegQuality = ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedImageJpegQuality;
			
				// Generate the new image and save to disk.
				ImageHelper.SaveImageFile(originalBitmap, newFilePath, imgFormat, newWidth, newHeight, jpegQuality);
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException)
			{
				if (originalBitmap != null)
					originalBitmap.Dispose();
				
				throw;
			}

			this._imageObject.Optimized.Width = newWidth;
			this._imageObject.Optimized.Height = newHeight;
			this._imageObject.Optimized.FileName = newFilename;
			this._imageObject.Optimized.FileNamePhysicalPath = newFilePath;

			int fileSize = (int) (this._imageObject.Optimized.FileInfo.Length / 1024);

			this._imageObject.Optimized.FileSizeKB = (fileSize < 1 ? 1 : fileSize); // Very small files should be 1, not 0.
		}

		private bool IsOptimizedImageRequired()
		{
			// We must create an optimized image in the following circumstances:
			// 1. The file corresponding to a previously created optimized image file does not exist.
			//    OR
			// 2. The overwrite flag is true.
			//    OR
			// 3. There is a request to rotate the image.
			//    AND
			// 4. The size of width/height dimensions of the original exceed the optimized triggers.
			//    OR
			// 5. The original image is not a JPEG.
			// In other words: image required = ((1 || 2 || 3) && (4 || 5))

			bool optimizedImageMissing = IsOptimizedImageFileMissing(); // Test 1

			bool overwriteFlag = this._imageObject.RegenerateOptimizedOnSave; // Test 2

			bool rotateIsRequested = (this._imageObject.Rotation != RotateFlipType.RotateNoneFlipNone); // Test 3

			bool originalExceedsOptimizedDimensionTriggers = false;
			bool isOriginalNonJpegImage = false;
			if (optimizedImageMissing || overwriteFlag || rotateIsRequested)
			{
				// Only need to run test 3 and 4 if test 1 or test 2 is true.
				originalExceedsOptimizedDimensionTriggers = DoesOriginalExceedOptimizedDimensionTriggers(); // Test 4

				isOriginalNonJpegImage = IsOriginalNonJpegImage(); // Test 5
			}

			return ((optimizedImageMissing || overwriteFlag || rotateIsRequested) && (originalExceedsOptimizedDimensionTriggers || isOriginalNonJpegImage));
		}

		private bool IsOriginalNonJpegImage()
		{
			// Return true if the original image is not a JPEG.
			string[] jpegImageTypes = new string[] { ".JPG", ".JPEG" };
			string originalFileExtension = Path.GetExtension(this._imageObject.Original.FileName).ToUpperInvariant();

			bool isOriginalNonJpegImage = false;
			if (Array.IndexOf<string>(jpegImageTypes, originalFileExtension) < 0)
			{
				isOriginalNonJpegImage = true;
			}

			return isOriginalNonJpegImage;
		}

		private bool DoesOriginalExceedOptimizedDimensionTriggers()
		{
			// Test 1: Is the file size of the original greater than OptimizedImageTriggerSizeKB?
			bool isOriginalFileSizeGreaterThanTriggerSize = false;
			long optimizedTriggerSizeKB = (long)GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedImageTriggerSizeKB;
			if (this._imageObject.Original.FileSizeKB > optimizedTriggerSizeKB)
			{
				isOriginalFileSizeGreaterThanTriggerSize = true;
			}

			// Test 2: Is the width or length of the original greater than the MaxOptimizedLength?
			bool isOriginalLengthGreaterThanMaxAllowedLength = false;
			int optimizedMaxLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MaxOptimizedLength;
			int originalWidth = this._imageObject.Original.Bitmap.Width;
			int originalHeight = this._imageObject.Original.Bitmap.Height;
			if ((originalWidth > optimizedMaxLength) || (originalHeight > optimizedMaxLength))
			{
				isOriginalLengthGreaterThanMaxAllowedLength = true;
			}

			return (isOriginalFileSizeGreaterThanTriggerSize | isOriginalLengthGreaterThanMaxAllowedLength);
		}

		private bool IsOptimizedImageFileMissing()
		{
			// Does the optimized image file exist? (Maybe it was accidentally deleted or moved by the user,
			// or maybe it's a new object.)
			bool optimizedImageExists = false;
			//bool objectExistsInDataStore = !this._imageObject.IsNew;

			// Does this image object specify that a separate optimized image should exist? When the optimized and original filenames
			// are equal, that means there isn't a separate optimized image.
			bool imageSpecifiesOptimizedVersion = (this._imageObject.Optimized.FileName != this._imageObject.Original.FileName);

			// Does a file exist matching the value in the optimized filepath variable?
			optimizedImageExists = File.Exists(this._imageObject.Optimized.FileNamePhysicalPath);

			// The optimized image is considered to be missing if the image objects says there should be one and we don't find
			// on one the hard drive. (Note that later, when we analyze the dimensions, we may decide
			// we don't need to create an optimized image anyway.)
			bool optimizedImageIsMissing = (!optimizedImageExists && imageSpecifiesOptimizedVersion);

			return optimizedImageIsMissing;
		}

		/// <summary>
		/// Determine name of new file and, for a new media object, makes sure it is unique in the directory. (Example: If original = puppy.jpg, 
		/// thumbnail = zOpt_puppy.jpg)
		/// </summary>
		/// <param name="optimizedPath">The path to the directory where the optimized file is to be created.</param>
		/// <param name="imgFormat">The image format of the thumbnail.</param>
		/// <returns>Returns the name of the new thumbnail file name and, for a new media object, makes sure it is unique in the directory.</returns>
		private string GenerateNewFilename(string optimizedPath, ImageFormat imgFormat)
		{
			string filenamePrefix = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.OptimizedFileNamePrefix;
			string nameWithoutExtension = Path.GetFileNameWithoutExtension(this._imageObject.Original.FileInfo.Name);
			string optimizedFilename = string.Format(CultureInfo.CurrentCulture, "{0}{1}.{2}", filenamePrefix, nameWithoutExtension, imgFormat.ToString().ToLower(CultureInfo.CurrentCulture));

			if (this._imageObject.IsNew)
			{
				// Since this is a new media object, make sure the file is unique in the directory. For existing media objects, we'll just
				// overwrite the file (if it exists).
				optimizedFilename = HelperFunctions.ValidateFileName(optimizedPath, optimizedFilename);
			}

			return optimizedFilename;
		}
	}
}

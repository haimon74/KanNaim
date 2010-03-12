using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Properties;

namespace GalleryServerPro.Business.Metadata
{
	///<summary>Provides access to and encapsulates functionality around the metadata in media objects 
	/// such as JPEG, TIFF, and PNG image files. Supports any media object that is recognized by the .NET Framework's
	/// System.Drawing.Image class and contains valid metadata, such as EXIF data.
	///</summary>
	public class MediaObjectMetadataExtractor
	{
		#region Private Fields

		// Contains an image's System.Drawing.Image.PropertyItems property.
		private System.Drawing.Imaging.PropertyItem[] _propertyItems;

		private readonly string _imageFilePath;
		private int _width, _height;
		private IGalleryObjectMetadataItemCollection _metadataItems;

		private Dictionary<RawMetadataItemName, MetadataItem> _rawMetadata;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaObjectMetadataExtractor"/> class. This object can
		/// interact with the metadata contained in the specified media object.
		/// </summary>
		/// <param name="imageFilePath">The path, either absolute or relative, that indicates the
		/// location of the image file on disk. This value is used in the <see cref="FileStream" /> constructor.
		/// (e.g. C:\folder1\folder2\sunset.jpg, sunset.jpg).</param>
		/// <exception cref="OutOfMemoryException">
		/// Thrown when the <paramref name="imageFilePath"/> is an image that is too large to be loaded into memory.</exception>
		public MediaObjectMetadataExtractor(string imageFilePath)
		{
			this._imageFilePath = imageFilePath;
			ExtractImagePropertyItems(imageFilePath);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the raw metadata associated with the current media object.
		/// </summary>
		/// <value>The raw metadata associated with the current media object.</value>
		public Dictionary<RawMetadataItemName, MetadataItem> RawMetadata
		{
			get
			{
				if (this._rawMetadata == null)
				{
					FillRawMetadataDictionary();
				}
				return this._rawMetadata;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a collection of <see cref="IGalleryObjectMetadataItem" /> objects. The collection includes one item for each
		/// <see cref="FormattedMetadataItemName" /> value, unless that metadata item does not exist in the image's metadata. In that case, no item
		/// is generated. 
		/// </summary>
		/// <returns>Returns a <see cref="IGalleryObjectMetadataItemCollection" /> object.</returns>
		/// <remarks>The collection is created the first time this method is called. Subsequent calls return the cached collection
		/// rather than regenerating it from the image file.</remarks>
		public IGalleryObjectMetadataItemCollection GetGalleryObjectMetadataItemCollection()
		{
			if (this._metadataItems == null)
			{
				this._metadataItems = new GalleryObjectMetadataItemCollection();

				// The AddWpfBitmapMetadata function requires .NET Framework 3.0 and running under Full Trust, so only call if 
				// these conditions are satisfied. There is also a config setting that enables this functionality, so query that
				// as well. (The config setting allows it to be disabled due to the reliability issues found with the WPF classes.)
				if ((AppSetting.Instance.AppTrustLevel == ApplicationTrustLevel.Full)
						&& (AppSetting.Instance.IsDotNet3Installed)
						&& (Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.EnableWpfMetadataExtraction))
				{
					AddWpfBitmapMetadata(_metadataItems);
				}

				AddExifMetadata(_metadataItems);
			}

			RemoveInvalidHtmlAndScripts(_metadataItems);

			return _metadataItems;
		}

		/// <summary>
		/// Return the specified raw (unformatted) metadata item. If no matching item is found, null is returned.
		/// </summary>
		/// <param name="metaItemName">The metadata item name to return.</param>
		/// <returns>Returns the specified raw (unformatted) meta item. If no matching item is found, null is returned.</returns>
		public MetadataItem GetRawMetadataItem(RawMetadataItemName metaItemName)
		{
			return this.RawMetadata[metaItemName];
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Removes any HTML and javascript from the metadata values that are not allowed.
		/// </summary>
		/// <param name="metadataItems">The metadata items.</param>
		private static void RemoveInvalidHtmlAndScripts(IEnumerable<IGalleryObjectMetadataItem> metadataItems)
		{
			foreach (IGalleryObjectMetadataItem metadataItem in metadataItems)
			{
				metadataItem.Value = HtmlValidator.Clean(metadataItem.Value);
			}
		}

		/// <summary>
		/// Fill the class-level _rawMetadata dictionary with MetadataItem objects created from the
		/// PropertyItems property of the image. Skip any items that are not defined in the 
		/// RawMetadataItemName enumeration.
		/// </summary>
		private void FillRawMetadataDictionary()
		{
			this._rawMetadata = new Dictionary<RawMetadataItemName, MetadataItem>();

			foreach (System.Drawing.Imaging.PropertyItem itemIterator in this._propertyItems)
			{
				RawMetadataItemName metadataName = (RawMetadataItemName)itemIterator.Id;
				if (Enum.IsDefined(typeof(RawMetadataItemName), metadataName))
				{
					if (!this._rawMetadata.ContainsKey(metadataName))
					{
						MetadataItem metadataItem = new MetadataItem(itemIterator);
						if (metadataItem.Value != null)
							this._rawMetadata.Add(metadataName, metadataItem);
					}
				}
			}
		}

		/// <summary>
		/// Extract the property items of the specified image to the class-level field variable.
		/// </summary>
		/// <param name="imageFilePath">The path, either absolute or relative, that indicates the
		/// location of the image file on disk. This value is used in the FileStream constructor.
		/// (e.g. C:\folder1\folder2\sunset.jpg, sunset.jpg).</param>
		private void ExtractImagePropertyItems(string imageFilePath)
		{
			if (String.IsNullOrEmpty(imageFilePath))
				throw new ArgumentNullException("imageFilePath");

			if (AppSetting.Instance.AppTrustLevel == ApplicationTrustLevel.Full)
			{
				GetPropertyItemsUsingFullTrustTechnique(imageFilePath);
			}
			else
			{
				GetPropertyItemsUsingLimitedTrustTechnique(imageFilePath);
			}
		}

		private void GetPropertyItemsUsingFullTrustTechnique(string imageFilePath)
		{
			// This technique is fast but requires full trust. Can only be called when app is running under full trust.
			if (AppSetting.Instance.AppTrustLevel != ApplicationTrustLevel.Full)
				throw new InvalidOperationException("The method MediaObjectMetadataExtractor.GetPropertyItemsUsingFullTrustTechnique can only be called when the application is running under full trust. The application should have already checked for this before calling this method. The developer needs to modify the source code to fix this.");

			using (Stream stream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				try
				{
					using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream, true, false))
					{
						try
						{
							this._propertyItems = image.PropertyItems;
						}
						catch (NotImplementedException ex)
						{
							// Some images, such as wmf, throw this exception. We'll make a note of it and set our field to an empty array.
							LogError(ex);
							this._propertyItems = new System.Drawing.Imaging.PropertyItem[0];
						}
					}
				}
				catch (ArgumentException ex)
				{
					LogError(ex);
					this._propertyItems = new System.Drawing.Imaging.PropertyItem[0];
				}
			}
		}

		private void GetPropertyItemsUsingLimitedTrustTechnique(string imageFilePath)
		{
			// This technique is not as fast as the one in the method GetPropertyItemsUsingFullTrustTechnique() but in works in limited
			// trust environments.
			try
			{
				using (System.Drawing.Image image = new System.Drawing.Bitmap(imageFilePath))
				{
					try
					{
						this._propertyItems = image.PropertyItems;
					}
					catch (NotImplementedException ex)
					{
						// Some images, such as wmf, throw this exception. We'll make a note of it and set our field to an empty array.
						LogError(ex);
						this._propertyItems = new System.Drawing.Imaging.PropertyItem[0];
					}
				}
			}
			catch (ArgumentException ex)
			{
				LogError(ex);
				this._propertyItems = new System.Drawing.Imaging.PropertyItem[0];
			}
		}

		private void AddExifMetadata(IGalleryObjectMetadataItemCollection metadataItems)
		{
			foreach (FormattedMetadataItemName metadataItemName in Enum.GetValues(typeof(FormattedMetadataItemName)))
			{
				IGalleryObjectMetadataItem mdi = null;
				switch (metadataItemName)
				{
					case FormattedMetadataItemName.Author: mdi = GetStringMetadataItem(RawMetadataItemName.Artist, FormattedMetadataItemName.Author, Resources.Metadata_Author); break;
					case FormattedMetadataItemName.CameraModel: mdi = GetStringMetadataItem(RawMetadataItemName.EquipModel, FormattedMetadataItemName.CameraModel, Resources.Metadata_CameraModel); break;
					case FormattedMetadataItemName.ColorRepresentation: mdi = GetColorRepresentationMetadataItem(); break;
					case FormattedMetadataItemName.Comment: mdi = GetStringMetadataItem(RawMetadataItemName.ExifUserComment, FormattedMetadataItemName.Comment, Resources.Metadata_Comment); break;
					case FormattedMetadataItemName.Copyright: mdi = GetStringMetadataItem(RawMetadataItemName.Copyright, FormattedMetadataItemName.Copyright, Resources.Metadata_Copyright); break;
					case FormattedMetadataItemName.DatePictureTaken: mdi = GetDatePictureTakenMetadataItem(); break;
					case FormattedMetadataItemName.Description: mdi = GetStringMetadataItem(RawMetadataItemName.ImageDescription, FormattedMetadataItemName.Description, Resources.Metadata_Description); break;
					case FormattedMetadataItemName.Dimensions: mdi = GetDimensionsMetadataItem(); break;
					case FormattedMetadataItemName.EquipmentManufacturer: mdi = GetStringMetadataItem(RawMetadataItemName.EquipMake, FormattedMetadataItemName.EquipmentManufacturer, Resources.Metadata_EquipmentManufacturer); break;
					case FormattedMetadataItemName.ExposureCompensation: mdi = GetExposureCompensationMetadataItem(); break;
					case FormattedMetadataItemName.ExposureProgram: mdi = GetExposureProgramMetadataItem(); break;
					case FormattedMetadataItemName.ExposureTime: mdi = GetExposureTimeMetadataItem(); break;
					case FormattedMetadataItemName.FlashMode: mdi = GetFlashModeMetadataItem(); break;
					case FormattedMetadataItemName.FNumber: mdi = GetFNumberMetadataItem(); break;
					case FormattedMetadataItemName.FocalLength: mdi = GetFocalLengthMetadataItem(); break;
					case FormattedMetadataItemName.Height: mdi = GetHeightMetadataItem(); break;
					case FormattedMetadataItemName.HorizontalResolution: mdi = GetXResolutionMetadataItem(); break;
					case FormattedMetadataItemName.IsoSpeed: mdi = GetStringMetadataItem(RawMetadataItemName.ExifISOSpeed, FormattedMetadataItemName.IsoSpeed, Resources.Metadata_IsoSpeed); break;
					case FormattedMetadataItemName.Keywords: break; // No way to access keywords through Exif, so just skip this one
					case FormattedMetadataItemName.LensAperture: mdi = GetApertureMetadataItem(); break;
					case FormattedMetadataItemName.LightSource: mdi = GetLightSourceMetadataItem(); break;
					case FormattedMetadataItemName.MeteringMode: mdi = GetMeteringModeMetadataItem(); break;
					case FormattedMetadataItemName.Rating: break; // No way to access rating through Exif, so just skip this one
					case FormattedMetadataItemName.Subject: break; // No way to access rating through Exif, so just skip this one
					case FormattedMetadataItemName.SubjectDistance: mdi = GetStringMetadataItem(RawMetadataItemName.ExifSubjectDist, FormattedMetadataItemName.SubjectDistance, Resources.Metadata_SubjectDistance, String.Concat("{0} ", Resources.Metadata_SubjectDistance_Units)); break;
					case FormattedMetadataItemName.Title: mdi = GetStringMetadataItem(RawMetadataItemName.ImageTitle, FormattedMetadataItemName.Title, Resources.Metadata_Title); break;
					case FormattedMetadataItemName.VerticalResolution: mdi = GetYResolutionMetadataItem(); break;
					case FormattedMetadataItemName.Width: mdi = GetWidthMetadataItem(); break;

					default: throw new System.ComponentModel.InvalidEnumArgumentException(string.Format(CultureInfo.CurrentCulture, "The FormattedMetadataItemName enumeration value {0} is not being processed in MediaObjectMetadataExtractor.AddExifMetadata().", metadataItemName.ToString()));
				}
				if ((mdi != null) && (!metadataItems.Contains(mdi)))
				{
					metadataItems.Add(mdi);
				}
			}
		}

		/// <summary>
		/// Gets a metadata item containing the date the picture was taken. The date format conforms to the IETF RFC 1123 specification,
		/// which means it uses this format string: "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'" (e.g. "Mon, 17 Apr 2006 21:38:09 GMT"). See 
		/// the DateTimeFormatInfo.RFC1123Pattern property for more information about the format. Returns null if no date is found 
		/// in the metadata.
		/// </summary>
		/// <returns>Returns a metadata item containing the date the picture was taken. Returns null if no date is found 
		/// in the metadata.</returns>
		private IGalleryObjectMetadataItem GetDatePictureTakenMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifDTOrig, out rawMdi))
			{
				DateTime convertedDateTimeValue = ConvertExifDateTimeToDateTime(rawMdi.Value.ToString());
				if (convertedDateTimeValue > DateTime.MinValue)
				{
					mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.DatePictureTaken, Resources.Metadata_DatePictureTaken, convertedDateTimeValue.ToString("R", CultureInfo.InvariantCulture), true);
				}
			}
			return mdi;
		}

		/// <summary>
		/// Convert an EXIF-formatted timestamp to the .NET DateTime type.
		/// </summary>
		/// <param name="exifDateTime">An EXIF-formatted timestamp. The format is YYYY:MM:DD HH:MM:SS with time shown 
		/// in 24-hour format and the date and time separated by one blank character (0x2000). The character 
		/// string length is 20 bytes including the NULL terminator. When the field is empty, it is treated 
		/// as unknown.</param>
		/// <returns>Returns the EXIF-formatted timestamp as a .NET DateTime type.</returns>
		private static DateTime ConvertExifDateTimeToDateTime(string exifDateTime)
		{
			DateTime convertedDateTimeValue = DateTime.MinValue;

			if (String.IsNullOrEmpty(exifDateTime))
				return convertedDateTimeValue;

			exifDateTime = exifDateTime.Replace(' ', ':');
			string[] ymdhms = exifDateTime.Split(':');

			if (ymdhms.Length == 6)
			{
				bool foundYear, foundMonth, foundDay, foundHour, foundMinute, foundSecond;
				int year, month, day, hour, minute, second;

				foundYear = Int32.TryParse(ymdhms[0], out year);
				foundMonth = Int32.TryParse(ymdhms[1], out month);
				foundDay = Int32.TryParse(ymdhms[2], out day);
				foundHour = Int32.TryParse(ymdhms[3], out hour);
				foundMinute = Int32.TryParse(ymdhms[4], out minute);
				foundSecond = Int32.TryParse(ymdhms[5], out second);

				if (foundYear && foundMonth && foundDay && foundHour && foundMinute && foundSecond)
				{
					try
					{
						convertedDateTimeValue = new DateTime(year, month, day, hour, minute, second);
					}
					catch (ArgumentOutOfRangeException) { }
					catch (ArgumentException) { }
				}
			}

			return convertedDateTimeValue;
		}

		private IGalleryObjectMetadataItem GetFocalLengthMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifFocalLength, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					float value = ((Fraction)rawMdi.Value).ToSingle();
					string formattedValue = String.Concat(Math.Round(value), " ", Resources.Metadata_FocalLength_Units);
					mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.FocalLength, Resources.Metadata_FocalLength, formattedValue, true);
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetExposureCompensationMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifExposureBias, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					float value = ((Fraction)rawMdi.Value).ToSingle();
					string formattedValue = String.Concat(value.ToString("##0.# ", CultureInfo.InvariantCulture), Resources.Metadata_ExposureCompensation_Suffix);
					mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.ExposureCompensation, Resources.Metadata_ExposureCompensation, formattedValue, true);
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetFNumberMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifFNumber, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					float value = ((Fraction)rawMdi.Value).ToSingle();
					string formattedValue = value.ToString("f/##0.#", CultureInfo.InvariantCulture);
					mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.FNumber, Resources.Metadata_FNumber, formattedValue, true);
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetMeteringModeMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifMeteringMode, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Int64)
				{
					MeteringMode meterMode = (MeteringMode)(Int64)rawMdi.Value;
					if (MetadataEnumHelper.IsValidMeteringMode(meterMode))
					{
						mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.MeteringMode, Resources.Metadata_MeteringMode, meterMode.ToString(), true);
					}
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetLightSourceMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifLightSource, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Int64)
				{
					LightSource lightSource = (LightSource)(Int64)rawMdi.Value;
					if (MetadataEnumHelper.IsValidLightSource(lightSource))
					{
						// Don't bother with it if it is "Unknown"
						if (lightSource != LightSource.Unknown)
						{
							mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.LightSource, Resources.Metadata_LightSource, lightSource.ToString(), true);
						}
					}
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetApertureMetadataItem()
		{
			// The aperture is the same as the F-Number if present; otherwise it is calculated from ExifAperture.
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			string aperture = String.Empty;

			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifFNumber, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					float exifFNumber = ((Fraction)rawMdi.Value).ToSingle();
					aperture = exifFNumber.ToString("f/##0.#", CultureInfo.InvariantCulture);
				}
			}

			if ((String.IsNullOrEmpty(aperture)) && (RawMetadata.TryGetValue(RawMetadataItemName.ExifAperture, out rawMdi)))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					float exifAperture = ((Fraction)rawMdi.Value).ToSingle();
					float exifFNumber = (float)Math.Round(Math.Pow(Math.Sqrt(2), exifAperture), 1);
					aperture = exifFNumber.ToString("f/##0.#", CultureInfo.InvariantCulture);
				}
			}

			if (!String.IsNullOrEmpty(aperture))
			{
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.LensAperture, Resources.Metadata_LensAperture, aperture, true);
			}

			return mdi;
		}

		private IGalleryObjectMetadataItem GetXResolutionMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			string resolutionUnit = String.Empty;

			if (RawMetadata.TryGetValue(RawMetadataItemName.ResolutionXUnit, out rawMdi))
			{
				resolutionUnit = rawMdi.Value.ToString();
			}

			if ((String.IsNullOrEmpty(resolutionUnit)) && (RawMetadata.TryGetValue(RawMetadataItemName.ResolutionUnit, out rawMdi)))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Int64)
				{
					ResolutionUnit resUnit = (ResolutionUnit)(Int64)rawMdi.Value;
					if (MetadataEnumHelper.IsValidResolutionUnit(resUnit))
					{
						resolutionUnit = resUnit.ToString();
					}
				}
			}

			if (RawMetadata.TryGetValue(RawMetadataItemName.XResolution, out rawMdi))
			{
				string xResolution;
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					xResolution = Math.Round(((Fraction)rawMdi.Value).ToSingle(), 2).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					xResolution = rawMdi.Value.ToString();
				}

				string xResolutionString = String.Concat(xResolution, " ", resolutionUnit);
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.HorizontalResolution, Resources.Metadata_HorizontalResolution, xResolutionString, true);
			}

			return mdi;
		}

		private IGalleryObjectMetadataItem GetYResolutionMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			string resolutionUnit = String.Empty;

			if (RawMetadata.TryGetValue(RawMetadataItemName.ResolutionYUnit, out rawMdi))
			{
				resolutionUnit = rawMdi.Value.ToString();
			}

			if ((String.IsNullOrEmpty(resolutionUnit)) && (RawMetadata.TryGetValue(RawMetadataItemName.ResolutionUnit, out rawMdi)))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Int64)
				{
					ResolutionUnit resUnit = (ResolutionUnit)(Int64)rawMdi.Value;
					if (MetadataEnumHelper.IsValidResolutionUnit(resUnit))
					{
						resolutionUnit = resUnit.ToString();
					}
				}
			}

			if (RawMetadata.TryGetValue(RawMetadataItemName.YResolution, out rawMdi))
			{
				string yResolution;
				if (rawMdi.ExtractedValueType == ExtractedValueType.Fraction)
				{
					yResolution = Math.Round(((Fraction)rawMdi.Value).ToSingle(), 2).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					yResolution = rawMdi.Value.ToString();
				}

				string yResolutionString = String.Concat(yResolution, " ", resolutionUnit);
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.VerticalResolution, Resources.Metadata_VerticalResolution, yResolutionString, true);
			}

			return mdi;
		}

		private IGalleryObjectMetadataItem GetDimensionsMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			int width = GetWidth();
			int height = GetHeight();

			if ((width > 0) && (height > 0))
			{
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.Dimensions, Resources.Metadata_Dimensions, String.Concat(width, " x ", height), true);
			}

			return mdi;
		}

		/// <summary>
		/// Get the height of the media object. Extracted from RawMetadataItemName.ExifPixXDim for compressed images and
		/// from RawMetadataItemName.ImageHeight for uncompressed images. The value is stored in a private class level variable
		/// for quicker subsequent access.
		/// </summary>
		/// <returns>Returns the height of the media object.</returns>
		private int GetWidth()
		{
			if (_width > 0)
				return _width;

			MetadataItem rawMdi = null;
			int width = int.MinValue;
			bool foundWidth = false;

			// Compressed images store their width in ExifPixXDim. Uncompressed images store their width in ImageWidth.
			// First look in ExifPixXDim since most images are likely to be compressed ones. If we don't find that one,
			// look for ImageWidth. If we don't find that one either (which should be unlikely to ever happen), then just give 
			// up and return null.
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifPixXDim, out rawMdi))
			{
				foundWidth = Int32.TryParse(rawMdi.Value.ToString(), out width);
			}

			if ((!foundWidth) && (RawMetadata.TryGetValue(RawMetadataItemName.ImageWidth, out rawMdi)))
			{
				foundWidth = Int32.TryParse(rawMdi.Value.ToString(), out width);
			}

			if (foundWidth)
				this._width = width;

			return width;
		}

		/// <summary>
		/// Get the width of the media object. Extracted from RawMetadataItemName.ExifPixYDim for compressed images and
		/// from RawMetadataItemName.ImageWidth for uncompressed images. The value is stored in a private class level variable
		/// for quicker subsequent access.
		/// </summary>
		/// <returns>Returns the width of the media object.</returns>
		private int GetHeight()
		{
			if (_height > 0)
				return _height;

			MetadataItem rawMdi = null;
			int height = int.MinValue;
			bool foundHeight = false;

			// Compressed images store their width in ExifPixXDim. Uncompressed images store their width in ImageWidth.
			// First look in ExifPixXDim since most images are likely to be compressed ones. If we don't find that one,
			// look for ImageWidth. If we don't find that one either (which should be unlikely to ever happen), then just give 
			// up and return null.
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifPixYDim, out rawMdi))
			{
				foundHeight = Int32.TryParse(rawMdi.Value.ToString(), out height);
			}

			if ((!foundHeight) && (RawMetadata.TryGetValue(RawMetadataItemName.ImageHeight, out rawMdi)))
			{
				foundHeight = Int32.TryParse(rawMdi.Value.ToString(), out height);
			}

			if (foundHeight)
				this._height = height;

			return height;
		}

		private IGalleryObjectMetadataItem GetWidthMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			int width = GetWidth();

			if (width > 0)
			{
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.Width, Resources.Metadata_Width, String.Concat(width, " ", Resources.Metadata_Width_Units), true);
			}

			return mdi;
		}

		private IGalleryObjectMetadataItem GetHeightMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			int height = GetHeight();

			if (height > 0)
			{
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.Height, Resources.Metadata_Height, String.Concat(height, " ", Resources.Metadata_Height_Units), true);
			}

			return mdi;
		}

		private IGalleryObjectMetadataItem GetFlashModeMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifFlash, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Int64)
				{
					FlashMode flashMode = (FlashMode)(Int64)rawMdi.Value;
					if (MetadataEnumHelper.IsValidFlashMode(flashMode))
					{
						mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.FlashMode, Resources.Metadata_FlashMode, flashMode.ToString(), true);
					}
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetExposureProgramMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifExposureProg, out rawMdi))
			{
				if (rawMdi.ExtractedValueType == ExtractedValueType.Int64)
				{
					ExposureProgram expProgram = (ExposureProgram)(Int64)rawMdi.Value;
					if (MetadataEnumHelper.IsValidExposureProgram(expProgram))
					{
						mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.ExposureProgram, Resources.Metadata_ExposureProgram, expProgram.ToString(), true);
					}
				}
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetExposureTimeMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			const Single NUM_SECONDS = 1; // If the exposure time is less than this # of seconds, format as fraction (1/350 sec.); otherwise convert to Single (2.35 sec.)
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifExposureTime, out rawMdi))
			{
				string exposureTime = String.Empty;
				if ((rawMdi.ExtractedValueType == ExtractedValueType.Fraction) && ((Fraction)rawMdi.Value).ToSingle() > NUM_SECONDS)
				{
					exposureTime = Math.Round(((Fraction)rawMdi.Value).ToSingle(), 2).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					exposureTime = rawMdi.Value.ToString();
				}

				string exposureTimeString = String.Concat(exposureTime, " ", Resources.Metadata_ExposureTime_Units);
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.ExposureTime, Resources.Metadata_ExposureTime, exposureTimeString, true);
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetColorRepresentationMetadataItem()
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(RawMetadataItemName.ExifColorSpace, out rawMdi))
			{
				string value = rawMdi.Value.ToString().Trim();
				string formattedValue = (value == "1" ? Resources.Metadata_ColorRepresentation_sRGB : Resources.Metadata_ColorRepresentation_Uncalibrated);
				mdi = new GalleryObjectMetadataItem(int.MinValue, FormattedMetadataItemName.ColorRepresentation, Resources.Metadata_ColorRepresentation, formattedValue, true);
			}
			return mdi;
		}

		private IGalleryObjectMetadataItem GetStringMetadataItem(RawMetadataItemName sourceRawMetadataName, FormattedMetadataItemName destinationFormattedMetadataName, string metadataDescription)
		{
			return GetStringMetadataItem(sourceRawMetadataName, destinationFormattedMetadataName, metadataDescription, "{0}");
		}

		private IGalleryObjectMetadataItem GetStringMetadataItem(RawMetadataItemName sourceRawMetadataName, FormattedMetadataItemName destinationFormattedMetadataName, string metadataDescription, string formatString)
		{
			IGalleryObjectMetadataItem mdi = null;
			MetadataItem rawMdi = null;
			if (RawMetadata.TryGetValue(sourceRawMetadataName, out rawMdi))
			{
				mdi = new GalleryObjectMetadataItem(int.MinValue, destinationFormattedMetadataName, metadataDescription, String.Format(CultureInfo.CurrentCulture, formatString, rawMdi.Value.ToString().TrimEnd(new char[] { '\0' })), true);
			}
			return mdi;
		}

		/// <summary>
		/// Add items to the specified collection from metadata accessed through the Windows Presentation Foundation (WPF)
		/// classes. This includes the following items: Title, Author, Data taken, Camera model, Camera manufacturer, Keywords,
		/// Rating, Comment, Copyright, Subject. If any of these items are null, they are not added.
		/// </summary>
		/// <param name="metadataItems">The collection of IGalleryObjectMetadataItem objects to add to.</param>
		/// <exception cref="System.Security.SecurityException">This function requires running under Full Trust, and will
		/// throw a security exception if it doesn't have it.</exception>
		private void AddWpfBitmapMetadata(IGalleryObjectMetadataItemCollection metadataItems)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("GalleryServerPro.Business.Wpf");

			// Get reference to static WpfMetadataExtractor.AddWpfBitmapMetadata() method.
			Type[] parmTypes = new Type[2];
			parmTypes[0] = typeof(string);
			parmTypes[1] = typeof(IGalleryObjectMetadataItemCollection);
			Type metadataExtractor = assembly.GetType("GalleryServerPro.Business.Wpf.WpfMetadataExtractor");
			System.Reflection.MethodInfo addMetadataMethod = metadataExtractor.GetMethod("AddWpfBitmapMetadata", parmTypes);

			// Prepare parameters to pass to BitmapDecoder.Create() method.
			object[] parameters = new object[2];
			parameters[0] = this._imageFilePath;
			parameters[1] = metadataItems;

			try
			{
				addMetadataMethod.Invoke(null, parameters);
			}
			catch (System.Reflection.TargetInvocationException ex)
			{
				LogError(ex);
			}
		}

		#endregion

		#region Private Static Methods

		private static void LogError(Exception ex)
		{
			GalleryServerPro.ErrorHandler.Error.Record(ex);
			HelperFunctions.PurgeCache();
		}

		#endregion
	}
}

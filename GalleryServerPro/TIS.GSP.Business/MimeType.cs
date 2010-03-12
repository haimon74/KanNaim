using System;
using System.Data;
using System.Collections.Generic;
using GalleryServerPro.Business.Interfaces;
using System.Globalization;
using GalleryServerPro.Business.Properties;
using GalleryServerPro.Configuration;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Represents a mime type associated with a file's extension.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("{_majorType}/{_subtype} ({_extension})")]
	public class MimeType : IMimeType
	{
		#region Private Fields

		private readonly string _extension;
		private readonly MimeTypeCategory _typeCategory;
		private readonly string _majorType;
		private readonly string _subtype;
		private readonly bool _allowAddToGallery;
		private readonly Dictionary<string, string> _browserMimeTypes;

		private static IList<IMimeType> _mimeTypes;
		private static readonly object syncObj = new object();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MimeType"/> class.
		/// </summary>
		/// <param name="extension">A string representing the file's extension, including the period (e.g. ".jpg", ".avi").
		/// It is not case sensitive.</param>
		/// <param name="fullMimeType">The full mime type. This is the <see cref="MajorType"/> concatenated with the <see cref="Subtype"/>, with a '/' between them
		/// (e.g. image/jpeg, video/quicktime).</param>
		/// <param name="browserId">The id of the browser for the default browser as specified in the .Net Framework's browser definition file. 
		/// This should always be the string "default", which means it will match all browsers. Once this instance is created, additional
		/// values that specify more specific browsers or browser families can be added to the private _browserMimeTypes member variable.</param>
		/// <param name="browserMimeType">The MIME type that can be understood by the browser for displaying this media object. The value will be applied
		/// to the browser specified in <paramref name="browserId"/>. Specify null or <see cref="String.Empty" /> if the MIME type appropriate for the 
		/// browser is the same as <paramref name="fullMimeType"/>.</param>
		/// <param name="allowAddToGallery">Indicates whether a file having this MIME type can be added to Gallery Server Pro.</param>
		private MimeType(string extension, string fullMimeType, string browserId, string browserMimeType, bool allowAddToGallery)
		{
			#region Validation

			if (String.IsNullOrEmpty(extension))
				throw new ArgumentNullException("extension");

			if (String.IsNullOrEmpty(fullMimeType))
				throw new ArgumentNullException("fullMimeType");

			if (String.IsNullOrEmpty(browserId))
				throw new ArgumentNullException("browserId");

			if (!browserId.Equals("default", StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.MimeType_Ctor_Ex_Msg2, browserId));
			}

			// If browserMimeType is specified, it better be valid.
			if (!String.IsNullOrEmpty(browserMimeType))
			{
				ValidateMimeType(browserMimeType);
			}

			// Validate fullMimeType and separate it into its major and sub types.
			string majorType;
			string subType;
			ValidateMimeType(fullMimeType, out majorType, out subType);

			#endregion

			MimeTypeCategory mimeTypeCategory = MimeTypeCategory.Other;
			try
			{
				mimeTypeCategory = (MimeTypeCategory)Enum.Parse(typeof(MimeTypeCategory), majorType, true);
			}
			catch (ArgumentException) {	/* Swallow exception so that we default to MimeTypeCategory.Other */	}

			this._extension = extension;
			this._typeCategory = mimeTypeCategory;
			this._majorType = majorType;
			this._subtype = subType;
			this._allowAddToGallery = allowAddToGallery;

			this._browserMimeTypes = new Dictionary<string, string>(1);
			if (String.IsNullOrEmpty(browserMimeType))
			{
				// When no browserMimeType is specified, default to the regular MIME type.
				browserMimeType = fullMimeType;
			}

			this._browserMimeTypes.Add(browserId, browserMimeType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MimeType"/> class with the specified MIME type category. The <see cref="MajorType" /> property is
		/// assigned the string representation of the <paramref name="mimeType"/>. Remaining properties are set to empty strings or false 
		/// (<see cref="AllowAddToGallery" />). This constructor is intended to be used to help describe an external media object, which is
		/// not represented by a locally stored file but for which it is useful to describe its general type (audio, video, etc).
		/// </summary>
		/// <param name="mimeType">Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
		/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg").</param>
		private MimeType(MimeTypeCategory mimeType)
		{
			this._typeCategory = mimeType;
			this._majorType = mimeType.ToString();
			this._extension = String.Empty;
			this._subtype = String.Empty;
			this._allowAddToGallery = false;

			this._browserMimeTypes = new Dictionary<string, string>(0);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the file extension this mime type is associated with.
		/// </summary>
		/// <value>The file extension this mime type is associated with.</value>
		public string Extension
		{
			get
			{
				return this._extension;
			}
		}

		/// <summary>
		/// Gets the type category this mime type is associated with (e.g. image, video, other).
		/// </summary>
		/// <value>
		/// The type category this mime type is associated with (e.g. image, video, other).
		/// </value>
		public MimeTypeCategory TypeCategory
		{
			get
			{
				return this._typeCategory;
			}
		}

		/// <summary>
		/// Gets the major type this mime type is associated with (e.g. image, video).
		/// </summary>
		/// <value>
		/// The major type this mime type is associated with (e.g. image, video).
		/// </value>
		public string MajorType
		{
			get
			{
				return this._majorType;
			}
		}

		/// <summary>
		/// Gets the subtype this mime type is associated with (e.g. jpeg, quicktime).
		/// </summary>
		/// <value>
		/// The subtype this mime type is associated with (e.g. jpeg, quicktime).
		/// </value>
		public string Subtype
		{
			get
			{
				return this._subtype;
			}
		}

		/// <summary>
		/// Gets the full mime type. This is the <see cref="MajorType"/> concatenated with the <see cref="Subtype"/>, with a '/' between them
		/// (e.g. image/jpeg, video/quicktime).
		/// </summary>
		/// <value>The full mime type.</value>
		public string FullType
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, "{0}/{1}", this._majorType.ToString().ToLower(CultureInfo.CurrentCulture), this._subtype);
			}
		}

		/// <summary>
		/// Gets a value indicating whether objects of this MIME type can be added to Gallery Server Pro.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if objects of this MIME type can be added to Gallery Server Pro; otherwise, <c>false</c>.
		/// </value>
		public bool AllowAddToGallery
		{
			get
			{
				return this._allowAddToGallery;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the most specific MIME type that matches one of the browser ids. The MIME type will be a value that
		/// can be understood by the specified browser, and will often correspond to the <see cref="FullType"/>, <see cref="Subtype"/>, and
		/// <see cref="MajorType"/> properties. But it will be different if the user specified an alternate or browser-specific
		/// MIME type in the configuration file.
		/// </summary>
		/// <param name="browserIds">An <see cref="System.Array"/> of browser ids for the current browser. This is a list of strings
		/// that represent the various categories of browsers the current browser belongs to. This is typically populated by
		/// calling ToArray() on the Request.Browser.Browsers property. </param>
		/// <returns>
		/// Returns a string that represents a browser-friendly MIME type. This value may be used when
		/// generating HTML content that requires a MIME type, especially the type attribute of the object tag.
		/// </returns>
		/// <example>The MIME type for a wave file is "audio/wav". Since some browsers do not recognize
		/// "&lt;object type='audio/wav' ...", we can specify browserMimeType = "application/x-mplayer2" in the
		/// configuration file (within the //galleryServerPro/galleryObject/mimeTypes/mimeType element).
		/// This method will then return "application/x-mplayer2", which will allow downstream code to generate HTML
		/// like this: "&lt;object type='application/x-mplayer2' ...", which is a value the browser understands.
		/// </example>
		public string GetFullTypeForBrowser(Array browserIds)
		{
			if (browserIds == null)
				throw new ArgumentNullException("browserIds");

			if (browserIds.Length == 0)
				throw new ArgumentOutOfRangeException(Resources.MimeType_GetFullTypeForBrowser_Ex_Msg);

			// We want to loop through each browserId, starting with the most specific id and ending with the most
			// general (id="Default"). Reverse order if needed.
			if ((browserIds.Length > 0) && (browserIds.GetValue(0).ToString().Equals("default", StringComparison.OrdinalIgnoreCase)))
			{
				lock (browserIds)
				{
					// Check again now that we have a lock. If it is still in the wrong order, reverse.
					if ((browserIds.Length > 0) && (browserIds.GetValue(0).ToString().Equals("default", StringComparison.OrdinalIgnoreCase)))
					{
						Array.Reverse(browserIds);
					}
				}
			}

			string matchingMimeType = null;
			foreach (string browserId in browserIds)
			{
				if (this._browserMimeTypes.TryGetValue(browserId, out matchingMimeType))
				{
					break;
				}
			}

			if (String.IsNullOrEmpty(matchingMimeType))
			{
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.MimeType_GetFullTypeForBrowser_Ex_Msg2, this._extension, this.FullType));
			}

			return matchingMimeType;
		}

		#endregion

		#region Public static methods

		/// <summary>
		/// Initializes a new instance of the <see cref="MimeType"/> class with the specified MIME type category. The <see cref="MajorType" /> property is
		/// assigned the string representation of the <paramref name="mimeType"/>. Remaining properties are set to empty strings or false 
		/// (<see cref="AllowAddToGallery" />). This method is intended to be used to help describe an external media object, which is
		/// not represented by a locally stored file but for which it is useful to describe its general type (audio, video, etc).
		/// </summary>
		/// <param name="mimeType">Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
		/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg").</param>
		/// <returns>Returns a new instance of <see cref="IMimeType"/>.</returns>
		public static IMimeType CreateInstance(MimeTypeCategory mimeType)
		{
			return new MimeType(mimeType);
		}

		/// <summary>
		/// Return a MimeType object corresponding to the file's extension. The mapping between file extension and 
		/// MIME type is declared in the configuration file. If no matching MIME type can be found, 
		/// this method returns null.
		/// </summary>
		/// <param name="fileExtension">A string representing the file's extension, including the period (e.g. ".jpg", ".avi").
		/// It is not case sensitive.</param>    
		/// <returns>Returns the MIME type corresponding to the specified file extension, or null if no matching MIME
		/// type is found.</returns>
		public static IMimeType LoadInstance(string fileExtension)
		{
			LoadInstances();

			if (_mimeTypes == null)
				throw new BusinessException("GalleryServerPro.Business.MimeType.LoadInstances() should have assigned an instance to the _mimeTypes member variable, but instead it remained null.");

			IMimeType mimeType = null;
			foreach (IMimeType mimeTypeIterator in _mimeTypes)
			{
				if (mimeTypeIterator.Extension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
				{
					mimeType = mimeTypeIterator;
					break;
				}
			}
			return mimeType;
		}


		/// <summary>
		/// Return a MimeType object corresponding to the extension of the specified file. The mapping between file
		/// extension and MIME type is declared in the configuration file. If no matching MIME type can be found, 
		/// this method returns null.
		/// </summary>
		/// <param name="filePath">A string representing the filename or the path to the file 
		/// (e.g. "C:\mypics\myprettypony.jpg", "myprettypony.jpg"). It is not case sensitive.</param>
		/// <returns>Returns the MIME type corresponding to the specified filepath, or null if no matching MIME
		/// type is found.</returns>
		/// <exception cref="System.ArgumentException">Thrown if filepath parameter contains one or more of 
		/// the invalid characters defined in System.IO.Path.InvalidPathChars, or contains a wildcard character.</exception>
		public static IMimeType LoadInstanceByFilePath(string filePath)
		{
			LoadInstances();

			if (_mimeTypes == null)
				throw new BusinessException("GalleryServerPro.Business.MimeType.LoadInstances() should have assigned an instance to the _mimeTypes member variable, but instead it remained null.");

			string extension = System.IO.Path.GetExtension(filePath);

			IMimeType mimeType = null;
			if (!(String.IsNullOrEmpty(extension)))
			{
				foreach (IMimeType mimeTypeIterator in _mimeTypes)
				{
					if (mimeTypeIterator.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))
					{
						mimeType = mimeTypeIterator;
						break;
					}
				}
			}

			return mimeType;
		}

		/// <summary>
		/// Return a MimeType object corresponding to the extension of the specified file. The mapping between file
		/// extension and MIME type is declared in the configuration file. If no matching MIME type can be found, 
		/// this method returns null.
		/// </summary>
		/// <param name="file">A FileSystemInfo object for which to determine the applicable MIME type. It is valid
		/// to pass a FileInfo or DirectoryInfo object since FileSystemInfo is the base class for both of these.</param>
		/// <returns>Returns the MIME type corresponding to the specified filepath, or null if no matching MIME
		/// type is found.</returns>
		public static IMimeType LoadInstance(System.IO.FileSystemInfo file)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			return LoadInstance(file.Extension);
		}

		/// <summary>
		/// Return a collection of all MIME types supported by Gallery Server. This list of MIME types and their
		/// mapping to file extensions is declared in the configuration file.
		/// </summary>
		/// <returns>Returns a collection of all MIME types supported by Gallery Server.</returns>
		/// <remarks>The collection should be considered read only. There is no mechanism for persisting changes to
		/// the data store, and since the collection is a static variable that is shared across threads, modifying 
		/// the collection may result in InvalidOperationException exceptions ("Collection was modified; enumeration 
		/// operation may not execute").</remarks>
		public static IList<IMimeType> LoadInstances()
		{
			if (_mimeTypes == null)
			{
				lock (syncObj)
				{
					if (_mimeTypes == null)
					{
						_mimeTypes = new List<IMimeType>();

						foreach (Configuration.MimeType mimeTypeConfig in ConfigManager.GetGalleryServerProConfigSection().GalleryObject.MimeTypes)
						{
							if (mimeTypeConfig.BrowserId.Equals("default", StringComparison.OrdinalIgnoreCase))
							{
								// Create a new MIME type for the default case.
								_mimeTypes.Add(new MimeType(mimeTypeConfig.FileExtension, mimeTypeConfig.Type, mimeTypeConfig.BrowserId, mimeTypeConfig.BrowserMimeType, mimeTypeConfig.AllowAddToGallery));
							}
							else
							{
								// The configuration file contains a browser ID other than "default". Find the instance we previously
								// created for the default case, and add the browser ID and browser MIME type to that one.
								MimeType myType = LoadInstance(mimeTypeConfig.FileExtension) as MimeType;

								if (myType == null)
									throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resources.MimeType_LoadInstances_Ex_Msg, mimeTypeConfig.FileExtension));

								myType._browserMimeTypes.Add(mimeTypeConfig.BrowserId, mimeTypeConfig.BrowserMimeType);
							}
						}
					}
				}
			}

			return _mimeTypes;
		}

		#endregion

		#region Private methods

		private static void ValidateMimeType(string fullMimeType)
		{
			string majorType;
			string subType;
			ValidateMimeType(fullMimeType, out majorType, out subType);
		}

		private static void ValidateMimeType(string fullMimeType, out string majorType, out string subType)
		{
			int slashLocation = fullMimeType.IndexOf("/", StringComparison.Ordinal);
			if (slashLocation < 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.MimeType_Ctor_Ex_Msg, fullMimeType), fullMimeType);
			}

			majorType = fullMimeType.Substring(0, slashLocation);
			subType = fullMimeType.Substring(slashLocation + 1);

			if ((String.IsNullOrEmpty(majorType)) || (String.IsNullOrEmpty(subType)))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.MimeType_Ctor_Ex_Msg, fullMimeType), fullMimeType);
			}
		}

		#endregion

	}
}

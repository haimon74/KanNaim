using System;
using System.Collections;
using System.Globalization;
using System.Web;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using System.Collections.Specialized;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web
{
	/// <summary>
	/// Provides functionality for generating the HTML that can be sent to a client browser to render a
	/// particular media object. Objects implementing this interface use the HTML templates in the configuration
	/// file. Replaceable parameters in the template are indicated by the open and close brackets, such as 
	/// {Width}. These parameters are replaced with the relevant values.
	/// TODO: Add caching functionality to speed up the ability to generate HTML.
	/// </summary>
	public class MediaObjectHtmlBuilder : IMediaObjectHtmlBuilder
  {
    #region Private Fields

    private int _mediaObjectId;
		private int _albumId;
    private IMimeType _mimeType;
		private readonly StringCollection _cssClasses;
    private string _mediaObjectPhysicalPath;
    private int _width;
    private int _height;
    private string _title;
    private bool? _autoStartMediaObject;
    private readonly ArrayList _browsers;
		private DisplayObjectType _displayType;
		private bool _isPrivate;
		private readonly string _externalHtmlSource;
    
    #endregion

    #region Constructors

		public MediaObjectHtmlBuilder(string externalHtmlSource)
		{
			this._externalHtmlSource = externalHtmlSource;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaObjectHtmlBuilder"/> class.
		/// </summary>
		/// <param name="mediaObjectId">The media object id.</param>
		/// <param name="albumId">The album id.</param>
		/// <param name="mimeType">The MIME type.</param>
		/// <param name="mediaObjectPhysicalPath">The media object physical path.</param>
		/// <param name="width">The width of the media object, in pixels.</param>
		/// <param name="height">The height of the media object, in pixels.</param>
		/// <param name="title">The title.</param>
		/// <param name="browsers">
		/// An <see cref="System.Collections.ArrayList"/> of browser ids for the current browser. This is a list of strings,
		/// ordered from most general to most specific, that represent the various categories of browsers the current
		/// browser belongs to. This is typically populated with the Request.Browser.Browsers property.
    /// </param>
		/// <param name="displayType">The display type.</param>
		/// <param name="isPrivate">if set to <c>true</c> the media object is private.</param>
		public MediaObjectHtmlBuilder(int mediaObjectId, int albumId, IMimeType mimeType, string mediaObjectPhysicalPath, int width, int height, string title, ArrayList browsers, DisplayObjectType displayType, bool isPrivate)
		{
			#region Validation

			if (mediaObjectId <= 0)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MediaObjectHtmlBuilder_Ctor_InvalidMediaObjectId_Msg, mediaObjectId));

			if (albumId <= 0)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MediaObjectHtmlBuilder_Ctor_InvalidAlbumId_Msg, albumId));

			if (mimeType == null)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MediaObjectHtmlBuilder_Ctor_InvalidMimeType_Msg));

			if (width < 0)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MediaObjectHtmlBuilder_Ctor_InvalidWidth_Msg, width));

			if (height < 0)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MediaObjectHtmlBuilder_Ctor_InvalidHeight_Msg, height));

			if (title == null)
				title = String.Empty;

			if ((browsers == null) || (browsers.Count < 1))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MediaObjectHtmlBuilder_Ctor_InvalidBrowsers_Msg));
      
			#endregion 
     
			this._mediaObjectId = mediaObjectId;
			this._albumId = albumId;
			this._mimeType = mimeType;
			this._mediaObjectPhysicalPath = mediaObjectPhysicalPath;
			this._width = width;
			this._height = height;
			this._title = title;
			this._browsers = browsers;
			this._displayType = displayType;
			this._cssClasses = new StringCollection();
			this._isPrivate = isPrivate;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the unique identifier for this media object.
		/// </summary>
		/// <value>The unique identifier for this media object.</value>
		public int MediaObjectId
		{
			get
			{
				return this._mediaObjectId;
			}
			set
			{
				this._mediaObjectId = value;
			}
		}

		/// <summary>
		/// Gets or sets the unique identifier for album containing the media object.
		/// </summary>
		/// <value>The unique identifier for album containing the media object.</value>
		public int AlbumId
		{
			get
			{
				return this._albumId;
			}
			set
			{
				this._albumId = value;
			}
		}

		/// <summary>
		/// Gets or sets the MIME type of this media object.
		/// </summary>
		/// <value>The MIME type of this media object.</value>
		public IMimeType MimeType
		{
			get
			{
				return this._mimeType;
			}
			set
			{
				this._mimeType = value;
			}
		}

		/// <summary>
		/// Gets or sets the list of CSS classes to apply to the HTML output of this media object. Each class must
		/// represent a valid, pre-existing CSS class that can be accessed by the client browser. The classes will
		/// be transformed into a space-delimited string and used to replace the {CssClasses} replacement parameter,
		/// if one exists, in the HTML template.
		/// </summary>
		/// <value>
		/// The list of CSS classes to apply to the HTML output of this media object.
		/// </value>
		public StringCollection CssClasses
		{
			get
			{
				return this._cssClasses;
			}
		}

		/// <summary>
		/// Gets or sets the physical path to this media object, including the object's name. Example:
		/// C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets\sonorandesert.jpg
		/// </summary>
		/// <value>
		/// The physical path to this media object, including the object's name.
		/// </value>
		public string MediaObjectPhysicalPath
		{
			get
			{
				return this._mediaObjectPhysicalPath;
			}
			set
			{
				this._mediaObjectPhysicalPath = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of this object, in pixels.
		/// </summary>
		/// <value>The width of this object, in pixels.</value>
		public int Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of this object, in pixels.
		/// </summary>
		/// <value>The height of this object, in pixels.</value>
		public int Height
		{
			get
			{
				return this._height;
			}
			set
			{
				this._height = value;
			}
		}

		/// <summary>
		/// Gets or sets the title for this gallery object.
		/// </summary>
		/// <value>The title for this gallery object.</value>
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to automatically begin playing the media object as soon as
		/// possible in the client browser. This setting is applicable only to objects that can be played, such
		/// as audio and video files.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if Gallery Server Pro is to automatically begin playing the media object as soon as
		/// possible in the client browser; otherwise, <c>false</c>.
		/// </value>
		public bool AutoStartMediaObject
		{
			get
			{
				if (!this._autoStartMediaObject.HasValue)
				{
					this._autoStartMediaObject = WebsiteController.GetGalleryServerProConfigSection().Core.AutoStartMediaObject;
				}

				return this._autoStartMediaObject.Value;
			}
			set
			{
				this._autoStartMediaObject = value;
			}
		}

		/// <summary>
		/// An <see cref="System.Collections.ArrayList"/> of browser ids for the current browser. This is a list of strings,
		/// ordered from most general to most specific, that represent the various categories of browsers the current
		/// browser belongs to. This is typically populated with the Request.Browser.Browsers property.
		/// </summary>
		/// <value>
		/// The <see cref="System.Collections.ArrayList"/> of browser ids for the current browser.
		/// </value>
		public ArrayList Browsers
		{
			get
			{
				return this._browsers;
			}
			//set
			//{
			//  this._browsers = value;
			//}
		}

		/// <summary>
		/// Gets or sets the type of the display object.
		/// </summary>
		/// <value>The display type.</value>
		public DisplayObjectType DisplayType
		{
			get
			{
				return this._displayType;
			}
			set
			{
				this._displayType = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the media object is marked as private. Private albums and media
		/// objects are hidden from anonymous (unauthenticated) users.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is private; otherwise, <c>false</c>.
		/// </value>
		public bool IsPrivate
		{
			get
			{
				return this._isPrivate;
			}
			set
			{
				this._isPrivate = value;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Generate the HTML that can be sent to a browser to render the media object. If the configuration file
		/// does not specify an HTML template for this MIME type, an empty string is returned. If the media object is
		/// an image but cannot be displayed in a browser (such as TIF), then return an empty string.
		/// </summary>
		/// <returns>
		/// Returns a string of valid HTML that can be sent to a browser to render the media object, or an empty
		/// string if the media object cannot be displayed in a browser.
		/// </returns>
		/// <remarks>The HTML templates used to generate the HTML code in this method are stored in the Gallery
		/// Server Pro configuration file, specifically at this location:
		/// GalleryServerPro/galleryObject/mediaObjects/mediaObject</remarks>
		public string GenerateHtml()
		{
			if (!String.IsNullOrEmpty(this._externalHtmlSource))
			{
				return this._externalHtmlSource;
			}

			if ((this.MimeType.MajorType.ToUpperInvariant() == "IMAGE") && (IsImageBrowserIncompatible()))
				return String.Empty; // Browsers can't display this image.

			string htmlOutput = GetHtmlOutputFromConfig();
			if (String.IsNullOrEmpty(htmlOutput))
				return String.Empty; // No HTML rendering info in config file.

			htmlOutput = htmlOutput.Replace("{MediaObjectUrl}", GetMediaObjectUrl());
			htmlOutput = htmlOutput.Replace("{MimeType}", this.MimeType.GetFullTypeForBrowser(this.Browsers));
			htmlOutput = htmlOutput.Replace("{Width}", this.Width.ToString(CultureInfo.InvariantCulture));
			htmlOutput = htmlOutput.Replace("{Height}", this.Height.ToString(CultureInfo.InvariantCulture));
			htmlOutput = htmlOutput.Replace("{Title}", this.Title);
			htmlOutput = htmlOutput.Replace("{TitleNoHtml}", HtmlScrubber.RemoveAllTags(this.Title, true));
			string[] cssClasses = new string[this.CssClasses.Count];
			this.CssClasses.CopyTo(cssClasses, 0);
			htmlOutput = htmlOutput.Replace("{CssClass}", String.Join(" ", cssClasses));

			bool autoStartMediaObject = WebsiteController.GetGalleryServerProConfigSection().Core.AutoStartMediaObject;

			// Replace {AutoStartMediaObjectText} with "true" or "false".
			htmlOutput = htmlOutput.Replace("{AutoStartMediaObjectText}", autoStartMediaObject.ToString().ToLowerInvariant());

			// Replace {AutoStartMediaObjectInt} with "1" or "0".
			htmlOutput = htmlOutput.Replace("{AutoStartMediaObjectInt}", autoStartMediaObject ? "1" : "0");

			if (htmlOutput.Contains("{MediaObjectAbsoluteUrlNoHandler}"))
				htmlOutput = ReplaceMediaObjectAbsoluteUrlNoHandlerParameter(htmlOutput);

			if (htmlOutput.Contains("{MediaObjectRelativeUrlNoHandler}"))
				htmlOutput = ReplaceMediaObjectRelativeUrlNoHandlerParameter(htmlOutput);

			return htmlOutput;
		}

		/// <summary>
		/// Generate the ECMA script (javascript) that can be sent to a browser to assist with rendering the media object. 
		/// If the configuration file does not specify a scriptOutput template for this MIME type, an empty string is returned.
		/// </summary>
		/// <returns>Returns the ECMA script (javascript) that can be sent to a browser to assist with rendering the media object.</returns>
		public string GenerateScript()
		{
			if ((this.MimeType.MajorType.ToUpperInvariant() == "IMAGE") && (IsImageBrowserIncompatible()))
				return String.Empty; // Browsers can't display this image.

			string scriptOutput = GetScriptOutputFromConfig();
			if (String.IsNullOrEmpty(scriptOutput))
				return String.Empty; // No ECMA script rendering info in config file.

			scriptOutput = scriptOutput.Replace("{MediaObjectUrl}", GetMediaObjectUrl());
			scriptOutput = scriptOutput.Replace("{MimeType}", this.MimeType.GetFullTypeForBrowser(this.Browsers));
			scriptOutput = scriptOutput.Replace("{Width}", this.Width.ToString(CultureInfo.InvariantCulture));
			scriptOutput = scriptOutput.Replace("{Height}", this.Height.ToString(CultureInfo.InvariantCulture));
			scriptOutput = scriptOutput.Replace("{Title}", this.Title);
			scriptOutput = scriptOutput.Replace("{TitleNoHtml}", HtmlScrubber.RemoveAllTags(this.Title, true));
			string[] cssClasses = new string[this.CssClasses.Count];
			this.CssClasses.CopyTo(cssClasses, 0);
			scriptOutput = scriptOutput.Replace("{CssClass}", String.Join(" ", cssClasses));

			bool autoStartMediaObject = WebsiteController.GetGalleryServerProConfigSection().Core.AutoStartMediaObject;

			// Replace {AutoStartMediaObjectText} with "true" or "false".
			scriptOutput = scriptOutput.Replace("{AutoStartMediaObjectText}", autoStartMediaObject.ToString().ToLowerInvariant());

			// Replace {AutoStartMediaObjectInt} with "1" or "0".
			scriptOutput = scriptOutput.Replace("{AutoStartMediaObjectInt}", autoStartMediaObject ? "1" : "0");

			if (scriptOutput.Contains("{MediaObjectAbsoluteUrlNoHandler}"))
				scriptOutput = ReplaceMediaObjectAbsoluteUrlNoHandlerParameter(scriptOutput);

			if (scriptOutput.Contains("{MediaObjectRelativeUrlNoHandler}"))
				scriptOutput = ReplaceMediaObjectRelativeUrlNoHandlerParameter(scriptOutput);

			return scriptOutput;
		}

		/// <summary>
		/// Generate the URL to the media object. For example, for images this url can be assigned to the src attribute of an img tag.
		/// (ex: /galleryserverpro/handler/getmediaobject.ashx?moid=34&amp;aid=8&amp;mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&amp;mtc=1&amp;dt=1&amp;isp=false)
		/// The query string parameter will be encrypted if that option is enabled.
		/// </summary>
		/// <returns>Gets the URL to the media object.</returns>
		public string GenerateUrl()
		{
			return GetMediaObjectUrl();
		}

		/// <summary>
		/// Replace the replacement parameter {MediaObjectAbsoluteUrlNoHandler} with an URL that points directly to the media object
		/// (ex: /gallery/videos/birthdayvideo.wmv). A BusinessException is thrown if the media objects directory is not
		/// within the web application directory. Note that using this parameter completely bypasses the HTTP handler that 
		/// normally streams the media object. The consequence is that there is no security check when the media object request
		/// is made and no watermarks are applied, even if watermark functionality is enabled. This option should only be
		/// used when it is not important to restrict access to the media objects.
		/// </summary>
		/// <param name="htmlOutput">A string representing the HTML that will be sent to the browser for the current media object.
		/// It is based on the htmlOutput setting in the galleryserverpro.config file.</param>
		/// <returns>Returns the htmlOutput parameter with the {MediaObjectAbsoluteUrlNoHandler} string replaced by the URL to the media
		/// object.</returns>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.BusinessException">Thrown when the media objects 
		/// directory is not within the web application directory.</exception>
		private string ReplaceMediaObjectAbsoluteUrlNoHandlerParameter(string htmlOutput)
		{
			string appPath = AppSetting.Instance.PhysicalApplicationPath;

			if (!this.MediaObjectPhysicalPath.StartsWith(appPath, StringComparison.OrdinalIgnoreCase))
				throw new BusinessException(String.Format(CultureInfo.CurrentCulture, "Expected this.MediaObjectPhysicalPath (\"{0}\") to start with AppSetting.Instance.PhysicalApplicationPath (\"{1}\"), but it did not. If the media objects are not stored within the Gallery Server Pro web application, you cannot use the MediaObjectAbsoluteUrlNoHandler replacement parameter. Instead, use MediaObjectRelativeUrlNoHandler and specify the virtual path to your media object directory in the HTML template in the galleryserverpro.config file. For example: htmlOutput=\"&lt;a href=&quot;http://yoursite.com/media{{MediaObjectRelativeUrlNoHandler}}&quot;&gt;Click to open&lt;/a&gt;\"", this.MediaObjectPhysicalPath, appPath));

			string relativePath = this.MediaObjectPhysicalPath.Remove(0, appPath.Length).Trim(new char[] { System.IO.Path.DirectorySeparatorChar });

			string directUrl = String.Concat(WebsiteController.GetAppRootPathUrl(), "/", relativePath.Replace("\\", "/"));

			return htmlOutput.Replace("{MediaObjectAbsoluteUrlNoHandler}", directUrl);
		}

		/// <summary>
		/// Replace the replacement parameter {MediaObjectRelativeUrlNoHandler} with an URL that is relative to the media objects
		/// directory and which points directly to the media object (ex: /videos/birthdayvideo.wmv). Note 
		/// that using this parameter completely bypasses the HTTP handler that normally streams the media object. The consequence 
		/// is that there is no security check when the media object request is made and no watermarks are applied, even if 
		/// watermark functionality is enabled. This option should only be used when it is not important to restrict access to 
		/// the media objects.
		/// </summary>
		/// <param name="htmlOutput">A string representing the HTML that will be sent to the browser for the current media object.
		/// It is based on the htmlOutput setting in the galleryserverpro.config file.</param>
		/// <returns>Returns the htmlOutput parameter with the {MediaObjectRelativeUrlNoHandler} string replaced by the URL to the media
		/// object.</returns>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.BusinessException">Thrown when the current media object's
		/// physical path does not start with the same text as AppSetting.Instance.MediaObjectPhysicalPath.</exception>
		/// <remarks>Typically this parameter is used instead of {MediaObjectAbsoluteUrlNoHandler} when the media objects directory 
		/// is outside of the web application. If the user wants to allow direct access to the media objects using this parameter, 
		/// she must first configure the media objects directory as a virtual directory in IIS. Then the path to this virtual directory 
		/// must be manually entered into one or more HTML templates in galleryserverpro.config, so that it prepends the relative
		/// url returned from this method.</remarks>
		/// <example>If the media objects directory has been set to D:\media and a virtual directory named gallery has been configured 
		/// in IIS that is accessible via http://yoursite.com/gallery, then you can configure the HTML template like this:
		/// htmlOutput="&lt;a href=&quot;http://yoursite.com/gallery{MediaObjectRelativeUrlNoHandler}&quot;&gt;Click to open&lt;/a&gt;"
		/// </example>
		private string ReplaceMediaObjectRelativeUrlNoHandlerParameter(string htmlOutput)
		{
			string moPath = AppSetting.Instance.MediaObjectPhysicalPath;

			if (!this.MediaObjectPhysicalPath.StartsWith(moPath, StringComparison.OrdinalIgnoreCase))
				throw new BusinessException(String.Format(CultureInfo.CurrentCulture, "Expected this.MediaObjectPhysicalPath (\"{0}\") to start with AppSetting.Instance.MediaObjectPhysicalPath (\"{1}\"), but it did not.", this.MediaObjectPhysicalPath, moPath));

			string relativePath = this.MediaObjectPhysicalPath.Remove(0, moPath.Length).Trim(new char[] { System.IO.Path.DirectorySeparatorChar });

			string relativeUrl = String.Concat("/", relativePath.Replace("\\", "/"));

			return htmlOutput.Replace("{MediaObjectRelativeUrlNoHandler}", relativeUrl);
		}

		/// <summary>
		/// Gets the HTML template information from the configuration file. If the configuration file
		/// does not specify an HTML template for the MIME type of this media object, an empty string is returned.
		/// </summary>
		/// <returns>Returns the HTML template information from the configuration file.</returns>
		private string GetHtmlOutputFromConfig()
		{
			MediaObject mocfg = WebsiteController.GetGalleryServerProConfigSection().GalleryObject.MediaObjects.FindByMimeType(this.MimeType.FullType);

			if (mocfg == null)
			{
				mocfg = WebsiteController.GetGalleryServerProConfigSection().GalleryObject.MediaObjects.FindByMimeType(string.Format(CultureInfo.CurrentCulture, "{0}/*", this.MimeType.MajorType.ToString().ToLowerInvariant()));
			}

			string htmlOutput = String.Empty;
			if (mocfg != null)
			{
				Browser bw = mocfg.Browsers.FindClosestMatchById(this.Browsers);
				htmlOutput = bw.HtmlOutput;
			}

			return htmlOutput;
		}

		/// <summary>
		/// Gets the ECMA script template information from the configuration file. If the configuration file
		/// does not specify an ECMA script template for the MIME type of this media object, an empty string is returned.
		/// </summary>
		/// <returns>Returns the ECMA script template information from the configuration file.</returns>
		private string GetScriptOutputFromConfig()
		{
			MediaObject mocfg = WebsiteController.GetGalleryServerProConfigSection().GalleryObject.MediaObjects.FindByMimeType(this.MimeType.FullType);

			if (mocfg == null)
			{
				mocfg = WebsiteController.GetGalleryServerProConfigSection().GalleryObject.MediaObjects.FindByMimeType(string.Format(CultureInfo.CurrentCulture, "{0}/*", this.MimeType.MajorType.ToString().ToLowerInvariant()));
			}

			string scriptOutput = String.Empty;
			if (mocfg != null)
			{
				Browser bw = mocfg.Browsers.FindClosestMatchById(this.Browsers);
				scriptOutput = bw.ScriptOutput;
			}

			return scriptOutput;
		}

		/// <summary>
		/// Determines if the image can be displayed in a standard web browser. For example, JPG, JPEG, PNG & GIF images can
		/// display, WMF and TIF cannot.
		/// </summary>
		/// <returns>Returns true if the image cannot be displayed in a standard browser (e.g. WMF, TIF); returns false if it can
		/// (e.g. JPG, JPEG, PNG & GIF).</returns>
		private bool IsImageBrowserIncompatible()
		{
			string[] browserCompatibleImageTypes = WebsiteController.GetGalleryServerProConfigSection().Core.ImageTypesStandardBrowsersCanDisplay.ToUpperInvariant().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			string originalFileExtension = System.IO.Path.GetExtension(this.MediaObjectPhysicalPath).ToUpperInvariant();

			bool isBrowserIncompatible = false;
			if (Array.IndexOf<string>(browserCompatibleImageTypes, originalFileExtension) < 0)
			{
				isBrowserIncompatible = true; // Image not in the list of images types a browser can render
			}

			return isBrowserIncompatible;
		}

		private string GetMediaObjectUrl()
		{
			return GetMediaObjectUrl(this.MediaObjectId, this.AlbumId, this.MimeType, this.MediaObjectPhysicalPath, this.DisplayType, this.IsPrivate);
		}
 
		#endregion

		#region Public Static Methods

		public static string GenerateUrl(int mediaObjectId, int albumId, IMimeType mimeType, string mediaObjectPhysicalPath, DisplayObjectType displayType, bool isPrivate)
		{
			return GetMediaObjectUrl(mediaObjectId, albumId, mimeType, mediaObjectPhysicalPath, displayType, isPrivate);
		}

		#endregion

		#region Private Static Methods

		private static string GetMediaObjectUrl(int mediaObjectId, int albumId, IMimeType mimeType, string mediaObjectPhysicalPath, DisplayObjectType displayType, bool isPrivate)
		{
			string queryString = string.Format(CultureInfo.CurrentCulture, "moid={0}&aid={1}&mo={2}&mtc={3}&dt={4}&isp={5}", mediaObjectId, albumId, Uri.EscapeDataString(mediaObjectPhysicalPath), (int)mimeType.TypeCategory, (int)displayType, isPrivate.ToString());
			
			// If necessary, encrypt, then URL encode the query string.
			if (WebsiteController.GetGalleryServerProConfigSection().Core.EncryptMediaObjectUrlOnClient)
				queryString = HttpUtility.UrlEncode(HelperFunctions.Encrypt(queryString));
			
			return string.Concat(WebsiteController.GetAppRootPathUrl(), "/handler/getmediaobject.ashx?", queryString);
		}

		#endregion

	}
}

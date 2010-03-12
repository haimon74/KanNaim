using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using GalleryServerPro.Configuration.Properties;

namespace GalleryServerPro.Configuration
{
	/// <summary>
	/// The galleryServerPro custom configuration section in galleryserverpro.config.
	/// </summary>
	public class GalleryServerProConfigSettings : ConfigurationSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GalleryServerProConfigSettings"/> class.
		/// </summary>
		public GalleryServerProConfigSettings() { }

		/// <summary>
		/// Gets a reference to the core element defined within the galleryServerPro section of galleryServerPro.config.
		/// </summary>
		[ConfigurationProperty("core", IsDefaultCollection = false, IsRequired = true)]
		public Core Core
		{
			get
			{
				return (Core)base["core"];
			}
		}

		/// <summary>
		/// Gets a reference to the galleryObject element defined within the galleryServerPro section of galleryServerPro.config.
		/// </summary>
		[ConfigurationProperty("galleryObject", IsDefaultCollection = false, IsRequired = true)]
		public GalleryObject GalleryObject
		{
			get
			{
				return (GalleryObject)base["galleryObject"];
			}
		}

		/// <summary>
		/// Gets a reference to the dataStore element defined within the galleryServerPro section of galleryServerPro.config.
		/// </summary>
		[ConfigurationProperty("dataStore", IsDefaultCollection = false, IsRequired = true)]
		public DataStore DataStore
		{
			get
			{
				return (DataStore)base["dataStore"];
			}
		}

		/// <summary>
		/// Gets a reference to the dataProvider element defined within the galleryServerPro section of galleryServerPro.config.
		/// </summary>
		[ConfigurationProperty("dataProvider", IsDefaultCollection = false, IsRequired = true)]
		public DataProvider DataProvider
		{
			get
			{
				return (DataProvider)base["dataProvider"];
			}
		}

	}

	/// <summary>
	/// Provides read/write access to the galleryServerPro/core section of galleryserverpro.config.
	/// </summary>
	public class Core : ConfigurationElement
	{
		private enum CoreAttributes
		{
			galleryId,
			mediaObjectPath,
			mediaObjectPathIsReadOnly,
			pageHeaderText,
			pageHeaderTextUrl,
			showLogin,
			showSearch,
			showErrorDetails,
			enableExceptionHandler,

			defaultAlbumDirectoryNameLength,
			synchAlbumTitleAndDirectoryName,
			emptyAlbumThumbnailBackgroundColor,
			emptyAlbumThumbnailText,
			emptyAlbumThumbnailFontName,
			emptyAlbumThumbnailFontSize,
			emptyAlbumThumbnailFontColor,
			emptyAlbumThumbnailWidthToHeightRatio,

			maxAlbumThumbnailTitleDisplayLength,
			maxMediaObjectThumbnailTitleDisplayLength,
			allowHtmlInTitlesAndCaptions,
			allowUserEnteredJavascript,
			allowedHtmlTags,
			allowedHtmlAttributes,
			allowCopyingReadOnlyObjects,
			allowManageOwnAccount,
			allowDeleteOwnAccount,
			mediaObjectTransitionType,
			mediaObjectTransitionDuration,
			slideshowInterval,

			mediaObjectDownloadBufferSize,
			encryptMediaObjectUrlOnClient,
			encryptionKey,
			allowUnspecifiedMimeTypes,
			imageTypesStandardBrowsersCanDisplay,
			silverlightFileTypes,

			allowAnonymousHiResViewing,
			enableImageMetadata,
			enableWpfMetadataExtraction,
			enableMediaObjectDownload,
			enableMediaObjectZipDownload,
			enablePermalink,
			enableSlideShow,
			maxThumbnailLength,
			thumbnailImageJpegQuality,
			thumbnailClickShowsOriginal,
			thumbnailWidthBuffer,
			thumbnailHeightBuffer,
			thumbnailFileNamePrefix,
			thumbnailPath,

			maxOptimizedLength,
			optimizedImageJpegQuality,
			optimizedImageTriggerSizeKB,
			optimizedFileNamePrefix,
			optimizedPath,

			originalImageJpegQuality,
			discardOriginalImageDuringImport,

			applyWatermark,
			applyWatermarkToThumbnails,
			watermarkText,
			watermarkTextFontName,
			watermarkTextFontSize,
			watermarkTextWidthPercent,
			watermarkTextColor,
			watermarkTextOpacityPercent,
			watermarkTextLocation,
			watermarkImagePath,
			watermarkImageWidthPercent,
			watermarkImageOpacityPercent,
			watermarkImageLocation,

			sendEmailOnError,
			emailFromName,
			emailFromAddress,
			emailToName,
			emailToAddress,
			smtpServer,
			smtpServerPort,
			sendEmailUsingSsl,

			autoStartMediaObject,
			defaultVideoPlayerWidth,
			defaultVideoPlayerHeight,

			defaultAudioPlayerWidth,
			defaultAudioPlayerHeight,

			defaultGenericObjectWidth,
			defaultGenericObjectHeight,

			maxUploadSize,
			allowAddLocalContent,
			allowAddExternalContent,
			allowAnonymousBrowsing,
			pageSize,
			pagerLocation,
			maxNumberErrorItems,
			enableSelfRegistration,
			requireEmailValidationForSelfRegisteredUser,
			requireApprovalForSelfRegisteredUser,
			useEmailForAccountName,
			usersToNotifyWhenAccountIsCreated,
			defaultRolesForSelfRegisteredUser,
			enableUserAlbum,
			userAlbumParentAlbumId,
			userAlbumNameTemplate,
			userAlbumSummaryTemplate,
			redirectToUserAlbumAfterLogin,
			jQueryScriptPath,

			membershipProviderName,
			roleProviderName,

			productKey
		}

		/// <summary>
		/// The galleryServerPro/core custom configuration section in galleryserverpro.config.
		/// </summary>
		public Core()
		{
		}

		#region Public Properties

		/// <summary>
		/// Attempt to assign the <paramref name="value"/> to the property named <paramref name="key"/> in the current instance.
		/// Returns false if the property cannot be successfully assigned. This may occur if a property named <paramref name="key"/>
		/// is not found or the <paramref name="value"/> cannot be converted to the data type of the property.
		/// </summary>
		/// <param name="key">A string containing a property name of the <see cref="Core"/> class. Case insensitive.</param>
		/// <param name="value">The value to be assigned to the property specified in <paramref name="key"/>.</param>
		/// <returns>Returns <c>true</c> if the property is successfully assigned; otherwise returns <c>false</c>.</returns>
		public bool TrySet(string key, string value)
		{
			try
			{
				PropertyInfo pi = typeof(Core).GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

				this[key] = Convert.ChangeType(value, pi.PropertyType);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// The value that uniquely identifies the gallery. Each web application is associated with a single gallery.
		/// </summary>
		[ConfigurationProperty("galleryId", DefaultValue = 1, IsRequired = true)]
		[IntegerValidator(MinValue = 1)]
		public int GalleryId
		{
			get { return Convert.ToInt32(this[CoreAttributes.galleryId.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.galleryId.ToString()] = value; }
		}

		/// <summary>
		/// The path to a directory containing the media objects. The path may be relative to the root of the web application
		/// (e.g. \gs\mediaobjects), a full path to a local resource (e.g. C:\mymedia), or a UNC path to a local or network
		/// resource (e.g. \\mynas\media). Mapped drives present a security risk and are not supported. The initial and 
		/// trailing slashes are	optional. For relative paths, the directory separator character can be either a forward 
		/// or backward slash.
		/// </summary>
		/// <remarks>The path is returned exactly how it appears in the configuration file.</remarks>
		[ConfigurationProperty("mediaObjectPath", DefaultValue = @"gs\mediaobjects", IsRequired = true)]
		[StringValidator(MinLength = 1)]
		public string MediaObjectPath
		{
			get { return this[CoreAttributes.mediaObjectPath.ToString()].ToString(); }
			set { this[CoreAttributes.mediaObjectPath.ToString()] = value; }
		}

		/// <summary>
		/// Specifies that the directory containing the media objects should never be written to by Gallery Server Pro. 
		/// This is useful when configuring the gallery to expose an existing media library and the administrator will not
		/// add, move, or copy objects using the Gallery Server Pro UI. Objects can be added or removed to the gallery 
		/// only by the synchronize function. Functions that do not require modifying the original files are still 
		/// available, such as editing captions and summaries, rearranging items, and the security system. Configuring 
		/// a read-only gallery requires setting the thumbnail and optimized paths to a different directory, disabling 
		/// user albums (<see cref="EnableUserAlbum"/>), and disabling the album title / directory name synchronization 
		/// setting (<see cref="SynchAlbumTitleAndDirectoryName"/>). This class does not enforce these business rules; 
		/// validation must be performed by the caller.
		/// </summary>
		/// <value><c>true</c> if the media objects directory is read-only; <c>false</c> if it can be written to.</value>
		[ConfigurationProperty("mediaObjectPathIsReadOnly", DefaultValue = false, IsRequired = true)]
		public bool MediaObjectPathIsReadOnly
		{
			get { return Convert.ToBoolean(this[CoreAttributes.mediaObjectPathIsReadOnly.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectPathIsReadOnly.ToString()] = value; }
		}

		/// <summary>
		/// The header text that appears at the top of each web page. Also known as the gallery title.
		/// </summary>
		[ConfigurationProperty("pageHeaderText", DefaultValue = "My Media Gallery", IsRequired = true)]
		public string PageHeaderText
		{
			get { return this[CoreAttributes.pageHeaderText.ToString()].ToString(); }
			set { this[CoreAttributes.pageHeaderText.ToString()] = value; }
		}

		/// <summary>
		/// The URL the user will be directed to when she clicks the page header text (gallery title). Optional. If not 
		/// present, no link will be rendered. Examples: "http://www.mysite.com", "/" (the root of the web site),
		/// "~/" (the root of the current application).
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), ConfigurationProperty("pageHeaderTextUrl", DefaultValue = "~/", IsRequired = true)]
		public string PageHeaderTextUrl
		{
			get { return this[CoreAttributes.pageHeaderTextUrl.ToString()].ToString(); }
			set { this[CoreAttributes.pageHeaderTextUrl.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to show the login controls at the top right of each page. When false, no login controls
		/// are shown, but the user can navigate directly to the login page to log on.
		/// </summary>
		/// <value><c>true</c> if login controls are visible; otherwise, <c>false</c>.</value>
		[ConfigurationProperty("showLogin", DefaultValue = true, IsRequired = true)]
		public bool ShowLogin
		{
			get { return Convert.ToBoolean(this[CoreAttributes.showLogin.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.showLogin.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to show the search box at the top right of each page.
		/// </summary>
		/// <value><c>true</c> if the search box is visible; otherwise, <c>false</c>.</value>
		[ConfigurationProperty("showSearch", DefaultValue = true, IsRequired = true)]
		public bool ShowSearch
		{
			get { return Convert.ToBoolean(this[CoreAttributes.showSearch.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.showSearch.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to show the full details of any unhandled exception that occurs within the gallery. This can reveal
		/// secure information to the user, so it should only be used for debugging purposes. When false, a generic error 
		/// message is given to the user. This setting has no effect when enableExceptionHandler="false".
		/// </summary>
		/// <value><c>true</c> if error details are displayed in the browser; <c>false</c> if a generic error message is displayed.</value>
		[ConfigurationProperty("showErrorDetails", DefaultValue = false, IsRequired = true)]
		public bool ShowErrorDetails
		{
			get { return Convert.ToBoolean(this[CoreAttributes.showErrorDetails.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.showErrorDetails.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to use Gallery Server Pro's internal exception handling mechanism. When true, unhandled exceptions
		/// are transferred to a custom error page and, if showErrorDetails="true", details about the error are displayed to the
		/// user. When false, the error is recorded and the exception is rethrown, allowing application-level error handling to
		/// handle it. This may include code in global.asax. The customErrors element in web.config may be used to manage error
		/// handling when this setting is false (the customErrors setting is ignored when this value is true).
		/// </summary>
		/// <value><c>true</c> if Gallery Server Pro's internal exception handling mechanism manages unhandled exceptions; 
		/// <c>false</c> if unhandled exceptions are allowed to propagate to the parent application, allowing for application
		/// level error handling code to manage the error.</value>
		[ConfigurationProperty("enableExceptionHandler", DefaultValue = true, IsRequired = true)]
		public bool EnableExceptionHandler
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableExceptionHandler.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableExceptionHandler.ToString()] = value; }
		}

		/// <summary>
		/// The maximum length of directory name when a user creates an album. By default, directory names are the same as the
		/// album's title, but are truncated when the title is longer than the value specified here.
		/// </summary>
		[ConfigurationProperty("defaultAlbumDirectoryNameLength", DefaultValue = 20, IsRequired = true)]
		[IntegerValidator(MinValue = 1, MaxValue = 255)]
		public int DefaultAlbumDirectoryNameLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultAlbumDirectoryNameLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultAlbumDirectoryNameLength.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to update the directory name corresponding to an album when the album's title is changed. When 
		/// true, modifying the title of an album causes the directory name to change to the same value. If the 
		/// title is longer than the value specified in DefaultAlbumDirectoryNameLength, the directory name is truncated. You 
		/// may want to set this to false if you have a directory structure that you do not want Gallery Server Pro to alter. 
		/// Note that even if this setting is false, directories will still be moved or copied when the user moves or copies
		/// an album. Also, Gallery Server Pro always modifies the directory name when it is necessary to 
		/// make it unique within a parent directory. For example, this may happen if you give two sibling albums the same title 
		/// or you move/copy an album into a directory containing another album with the same name.
		/// </summary>
		[ConfigurationProperty("synchAlbumTitleAndDirectoryName", DefaultValue = true, IsRequired = true)]
		public bool SynchAlbumTitleAndDirectoryName
		{
			get { return Convert.ToBoolean(this[CoreAttributes.synchAlbumTitleAndDirectoryName.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.synchAlbumTitleAndDirectoryName.ToString()] = value; }
		}

		/// <summary>
		/// The color used for the background of the GIF image generated by Gallery Server when creating a default
		/// thumbnail image for a newly created album or an album without any objects. The color can be specified as
		/// hex (e.g. #336699), RGB (e.g. 127,55,95), or one of the System.Color.KnownColor enum values (e.g. Maroon).
		/// </summary>
		[ConfigurationProperty("emptyAlbumThumbnailBackgroundColor", DefaultValue = "#369", IsRequired = true)]
		public string EmptyAlbumThumbnailBackgroundColor
		{
			get { return this[CoreAttributes.emptyAlbumThumbnailBackgroundColor.ToString()].ToString(); }
			set { this[CoreAttributes.emptyAlbumThumbnailBackgroundColor.ToString()] = value; }
		}

		/// <summary>
		/// The default text written on the GIF image generated by Gallery Server when creating a default thumbnail image 
		/// for a newly created album or an album without any objects. The GIF is 
		/// dynamically generated by the application when it is needed and is never actually stored on the hard drive.
		/// </summary>
		[ConfigurationProperty("emptyAlbumThumbnailText", DefaultValue = "Empty", IsRequired = true)]
		public string EmptyAlbumThumbnailText
		{
			get { return this[CoreAttributes.emptyAlbumThumbnailText.ToString()].ToString(); }
			set { this[CoreAttributes.emptyAlbumThumbnailText.ToString()] = value; }
		}

		/// <summary>
		/// The font used for text written on the GIF image generated by Gallery Server when creating a default
		/// thumbnail image for a newly created album or an album without any objects. The font must be installed on 
		/// the web server. If the font is not installed, a generic sans serif font will be substituted.
		/// </summary>
		[ConfigurationProperty("emptyAlbumThumbnailFontName", DefaultValue = "Arial", IsRequired = true)]
		public string EmptyAlbumThumbnailFontName
		{
			get { return this[CoreAttributes.emptyAlbumThumbnailFontName.ToString()].ToString(); }
			set { this[CoreAttributes.emptyAlbumThumbnailFontName.ToString()] = value; }
		}

		/// <summary>
		/// The size, in pixels, of the font used for text written on the GIF image generated by Gallery Server when 
		/// creating a default thumbnail image for a newly created album or an album without any objects. 
		/// </summary>
		[ConfigurationProperty("emptyAlbumThumbnailFontSize", DefaultValue = 12, IsRequired = true)]
		[IntegerValidator(MinValue = 6, MaxValue = 100)]
		public int EmptyAlbumThumbnailFontSize
		{
			get { return Convert.ToInt32(this[CoreAttributes.emptyAlbumThumbnailFontSize.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.emptyAlbumThumbnailFontSize.ToString()] = value; }
		}

		/// <summary>
		/// The color of the text specified in property EmptyAlbumThumbnailText. The color can be specified as
		/// hex (e.g. #336699), RGB (e.g. 127,55,95), or one of the System.Color.KnownColor enum values (e.g. Maroon).
		/// </summary>
		[ConfigurationProperty("emptyAlbumThumbnailFontColor", DefaultValue = "White", IsRequired = true)]
		public string EmptyAlbumThumbnailFontColor
		{
			get { return this[CoreAttributes.emptyAlbumThumbnailFontColor.ToString()].ToString(); }
			set { this[CoreAttributes.emptyAlbumThumbnailFontColor.ToString()] = value; }
		}

		/// <summary>
		/// The ratio of the width to height of the default thumbnail image for an album that does not have a thumbnail
		/// image specified. The length of the longest side of the image is set by the MaxThumbnailLength property, and the
		/// length of the remaining side is calculated using this ratio. A ratio or more than 1.00 results in the width
		/// being greater than the height (landscape), while a ratio less than 1.00 results in the width being less
		/// than the height (portrait). Example: If MaxThumbnailLength = 115 and EmptyAlbumThumbnailWidthToHeightRatio = 1.50,
		/// the width of the default thumbnail image is 115 and the height is 77 (115 / 1.50).
		/// </summary>
		[ConfigurationProperty("emptyAlbumThumbnailWidthToHeightRatio", DefaultValue = 1.33F, IsRequired = true)]
		public Single EmptyAlbumThumbnailWidthToHeightRatio
		{
			get
			{
				Single ratio = Convert.ToSingle(this[CoreAttributes.emptyAlbumThumbnailWidthToHeightRatio.ToString()], CultureInfo.InvariantCulture);
				ValidateEmptyAlbumThumbnailWidthToHeightRatio(ratio);

				return ratio;
			}
			set
			{
				ValidateEmptyAlbumThumbnailWidthToHeightRatio(value);
				this[CoreAttributes.emptyAlbumThumbnailWidthToHeightRatio.ToString()] = value;
			}
		}

		/// <summary>
		/// Maximum # of characters to display when showing the title of an album in a thumbnail view.
		/// </summary>
		[ConfigurationProperty("maxAlbumThumbnailTitleDisplayLength", DefaultValue = 25, IsRequired = true)]
		[IntegerValidator(MinValue = 1, MaxValue = 100)]
		public int MaxAlbumThumbnailTitleDisplayLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.maxAlbumThumbnailTitleDisplayLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.maxAlbumThumbnailTitleDisplayLength.ToString()] = value; }
		}

		/// <summary>
		/// Maximum # of characters to display when showing the title of a media object in a thumbnail view.
		/// </summary>
		[ConfigurationProperty("maxMediaObjectThumbnailTitleDisplayLength", DefaultValue = 20, IsRequired = true)]
		[IntegerValidator(MinValue = 1, MaxValue = 100)]
		public int MaxMediaObjectThumbnailTitleDisplayLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.maxMediaObjectThumbnailTitleDisplayLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.maxMediaObjectThumbnailTitleDisplayLength.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to allow limited HTML tags and attributes in the titles and captions for albums and media objects.
		/// When true, the HTML tags specified in <see cref="AllowedHtmlTags"/> and the attributes in
		/// <see cref="AllowedHtmlAttributes"/> are allowed. Invalid tags are automatically removed from user
		/// input. This setting does not affect how javascript is treated; refer to <see cref="AllowUserEnteredJavascript"/>.
		/// If this value is changed from true to false, existing objects will not be immediately purged of all HTML
		/// tags. Instead, individual titles and captions are stripped of HTML as each object is edited and saved by the user.
		/// </summary>
		[ConfigurationProperty("allowHtmlInTitlesAndCaptions", DefaultValue = false, IsRequired = true)]
		public bool AllowHtmlInTitlesAndCaptions
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowHtmlInTitlesAndCaptions.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowHtmlInTitlesAndCaptions.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether javascript is allowed in user-entered text such as titles, captions, and external media 
		/// objects. When false, script tags and the string "javascript:" is automatically removed from all user input.
		/// WARNING: Enabling this option makes the gallery vulnerable to a cross site scripting attack by any user with 
		/// permission to edit captions or upload external media objects.
		/// </summary>
		[ConfigurationProperty("allowUserEnteredJavascript", DefaultValue = false, IsRequired = true)]
		public bool AllowUserEnteredJavascript
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowUserEnteredJavascript.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowUserEnteredJavascript.ToString()] = value; }
		}

		/// <summary>
		/// A comma-delimited list of HTML tags that may be present in titles and captions of albums and media objects.
		/// The attributes that are allowed are specified in <see cref="AllowedHtmlAttributes"/>.
		/// Applies only when <see cref="AllowHtmlInTitlesAndCaptions"/> is <c>true</c>. Ex: p,a,div,span,...
		/// </summary>
		[ConfigurationProperty("allowedHtmlTags", IsRequired = true)]
		public string AllowedHtmlTags
		{
			get { return this[CoreAttributes.allowedHtmlTags.ToString()].ToString(); }
			set { this[CoreAttributes.allowedHtmlTags.ToString()] = value; }
		}

		/// <summary>
		/// A comma-delimited list of attributes that HTML tags are allowed to have. These attributes, when combined with the
		/// HTML tags in <see cref="AllowedHtmlTags"/>, define the HTML that is allowed in titles and captions of 
		/// albums and media objects. Applies only when <see cref="AllowHtmlInTitlesAndCaptions"/> is <c>true</c>. Ex: href,class,style,...
		/// </summary>
		[ConfigurationProperty("allowedHtmlAttributes", IsRequired = true)]
		public string AllowedHtmlAttributes
		{
			get { return this[CoreAttributes.allowedHtmlAttributes.ToString()].ToString(); }
			set { this[CoreAttributes.allowedHtmlAttributes.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to allow the copying of objects a user has only view permissions for.
		/// </summary>
		[ConfigurationProperty("allowCopyingReadOnlyObjects", DefaultValue = false, IsRequired = true)]
		public bool AllowCopyingReadOnlyObjects
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowCopyingReadOnlyObjects.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowCopyingReadOnlyObjects.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to allow a logged-on user to manage their account. When false, the link to the account page 
		/// at the top right of each page is not shown and if the user navigates directly to the account page, they are redirected away.
		/// </summary>
		/// <value><c>true</c> if a logged-on user can manage their account; otherwise, <c>false</c>.</value>
		[ConfigurationProperty("allowManageOwnAccount", DefaultValue = true, IsRequired = true)]
		public bool AllowManageOwnAccount
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowManageOwnAccount.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowManageOwnAccount.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether a user is allowed to delete his or her own account.
		/// </summary>
		[ConfigurationProperty("allowDeleteOwnAccount", DefaultValue = true, IsRequired = true)]
		public bool AllowDeleteOwnAccount
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowDeleteOwnAccount.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowDeleteOwnAccount.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the visual transition effect to use when moving from one media object to another. This value maps to the 
		/// enumeration GalleryServerPro.Business.MediaObjectTransitionType, and must be one of the following values:
		/// None, Fade.
		/// </summary>
		[ConfigurationProperty("mediaObjectTransitionType", DefaultValue = "None", IsRequired = true)]
		[RegexStringValidator(@"[None|Fade]")]
		public string MediaObjectTransitionType
		{
			get { return this[CoreAttributes.mediaObjectTransitionType.ToString()].ToString(); }
			set { this[CoreAttributes.mediaObjectTransitionType.ToString()] = value; }
		}

		/// <summary>
		/// The duration of the transition effect, in seconds, when navigating between media objects. This 
		/// setting has no effect when mediaObjectTransitionType = "None".
		/// </summary>
		[ConfigurationProperty("mediaObjectTransitionDuration", DefaultValue = .3F, IsRequired = true)]
		public Single MediaObjectTransitionDuration
		{
			get
			{
				Single duration = Convert.ToSingle(this[CoreAttributes.mediaObjectTransitionDuration.ToString()], CultureInfo.InvariantCulture);
				ValidateMediaObjectTransitionDuration(duration);

				return duration;
			}
			set
			{
				ValidateMediaObjectTransitionDuration(value);
				this[CoreAttributes.mediaObjectTransitionDuration.ToString()] = value;
			}
		}

		/// <summary>
		/// The delay, in seconds, between images during a slide show.
		/// </summary>
		[ConfigurationProperty("slideshowInterval", DefaultValue = 3000, IsRequired = true)]
		[IntegerValidator(MinValue = 1)]
		public int SlideshowInterval
		{
			get { return Convert.ToInt32(this[CoreAttributes.slideshowInterval.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.slideshowInterval.ToString()] = value; }
		}

		/// <summary>
		/// The size of each block of bytes when transferring files to streams and vice versa. This property was originally
		/// created to specify the buffer size for downloading a media object to the client, but it is now used for all
		/// file/stream copy operations.
		/// </summary>
		[ConfigurationProperty("mediaObjectDownloadBufferSize", DefaultValue = 32768, IsRequired = true)]
		[IntegerValidator(MinValue = 1)]
		public int MediaObjectDownloadBufferSize
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectDownloadBufferSize.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectDownloadBufferSize.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether security-sensitive portions of the URL to the media object are encrypted when it is sent 
		/// to the client browser. Valid values are "true" and "false". When false, the URL to the media object is sent
		/// in plain text, such as "handler/getmediaobject.ashx?moid=34&amp;aid=8&amp;mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&amp;mtc=1&amp;dt=1&amp;isp=false"
		/// These URLs can be seen by viewing the source of the HTML page. From this URL one can determine the album ID 
		/// for this media object is 8, (aid=8), the file path to the media object on the server is 
		/// C:\gs\mypics\birthday.jpeg, and the requested image is a thumbnail (dt=1, where 1 is the value of the 
		/// GalleryServerPro.Business.DisplayObjectType enumeration for a thumbnail). For enhanced security, set 
		/// encryptMediaObjectUrlOnClient to true, which uses Triple DES encryption to encrypt the the query string.
		/// It is recommended to set this to true except when you are	troubleshooting and it is useful to see the 
		/// filename and path in the HTML source. The Triple DES algorithm uses the secret key specified in the 
		/// EncryptionKey property.
		/// </summary>
		[ConfigurationProperty("encryptMediaObjectUrlOnClient", DefaultValue = true, IsRequired = true)]
		public bool EncryptMediaObjectUrlOnClient
		{
			get { return Convert.ToBoolean(this[CoreAttributes.encryptMediaObjectUrlOnClient.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.encryptMediaObjectUrlOnClient.ToString()] = value; }
		}

		/// <summary>
		/// The secret key used for the Triple DES algorithm. Applicable when the property EncryptMediaObjectUrlOnClient = true.
		/// The string must be 24 characters in length and be sufficiently strong so that it cannot be easily cracked.
		/// An exception is thrown by the .NET Framework if the key is considered weak. Change this to a value known only
		/// to you to prevent others from being able to decrypt.
		/// </summary>
		[ConfigurationProperty("encryptionKey", DefaultValue = "Phoenix 13drycity minnea", IsRequired = true)]
		[StringValidator(MinLength = 24, MaxLength = 24)]
		public string EncryptionKey
		{
			get { return this[CoreAttributes.encryptionKey.ToString()].ToString(); }
			set { this[CoreAttributes.encryptionKey.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to allow users to upload file types not explicitly specified in the mimeTypes configuration
		/// section. When false, any file with an extension not listed in the mimeTypes section is rejected. When true,
		/// Gallery Server Pro accepts all file types regardless of their file extension.
		/// </summary>
		[ConfigurationProperty("allowUnspecifiedMimeTypes", DefaultValue = false, IsRequired = true)]
		public bool AllowUnspecifiedMimeTypes
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowUnspecifiedMimeTypes.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowUnspecifiedMimeTypes.ToString()] = value; }
		}

		/// <summary>
		/// A comma-delimited list of file extensions, including the period, indicating types of images that a standard browser can display. When
		/// the user requests an original image (high resolution), the original is sent to the browser in an &lt;img&gt; HTML tag
		/// if its extension is one of those listed here.  If not, the user is presented with a message containing instructions
		/// for downloading the image file. Typically this setting should not be changed. Ex: .jpg,.jpeg,.gif,.png
		/// </summary>
		[ConfigurationProperty("imageTypesStandardBrowsersCanDisplay", IsRequired = true)]
		public string ImageTypesStandardBrowsersCanDisplay
		{
			get { return this[CoreAttributes.imageTypesStandardBrowsersCanDisplay.ToString()].ToString(); }
			set { this[CoreAttributes.imageTypesStandardBrowsersCanDisplay.ToString()] = value; }
		}

		/// <summary>
		/// A comma-delimited list of file extensions, including the period, indicating types of files that are supported by
		/// Microsoft Silverlight and that the user wishes to be rendered using Silverlight. This setting is used to determine
		/// whether to send the Silverlight javascript files to the browser. Note that this setting is used in combination with
		/// the HTML template. That is, rendering objects in Silverlight requires that the HTML template specify Silverlight and
		/// the file type associated with that template be included in this setting. If Silverlight is not used, a slight 
		/// performance enhancement can be achieved by setting this value to an empty string. Ex: .mp3,.wma,.wmv,.asf,.asx
		/// </summary>
		[ConfigurationProperty("silverlightFileTypes", IsRequired = true)]
		public string SilverlightFileTypes
		{
			get { return this[CoreAttributes.silverlightFileTypes.ToString()].ToString(); }
			set { this[CoreAttributes.silverlightFileTypes.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether anonymous users are allowed to view the original, high resolution versions of images. When no
		/// compressed (optimized) version of an image exists, the user is allowed to view the original, regardless of this
		/// setting, since it is assumed that the original was not large enough to trigger the creation of a compressed
		/// version. This setting has no effect on non-image media objects or for logged on users. This setting overrides
		/// the <see cref="ThumbnailClickShowsOriginal"/> property. That is, if this property is <c>false</c> and
		/// <see cref="ThumbnailClickShowsOriginal"/> is <c>true</c>, the user is shown the compressed version rather than 
		/// the original.
		/// </summary>
		[ConfigurationProperty("allowAnonymousHiResViewing", DefaultValue = true, IsRequired = true)]
		public bool AllowAnonymousHiResViewing
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowAnonymousHiResViewing.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowAnonymousHiResViewing.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether Gallery Server Pro extracts and displays metadata from image files. The metadata is displayed
		/// next to the optimized version of images when the View metadata toolbar icon is invoked. If the attribute
		/// enableWpfMetadataExtraction is true, then additional metadata such as title, keywords, and rating is extracted.
		/// </summary>
		[ConfigurationProperty("enableImageMetadata", DefaultValue = true, IsRequired = true)]
		public bool EnableImageMetadata
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableImageMetadata.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableImageMetadata.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether metadata is extracted from image files using Windows Presentation Foundation (WPF) classes
		/// in .NET Framework 3.0. The WPF classes allow additional metadata to be extracted beyond those allowed by the
		/// .NET Framework 2.0, such as title, keywords, and rating. This attribute has no effect unless the following
		/// requirements are met: The attribute enableImageMetadata = "true"; .NET Framework 3.0 is installed on the web
		/// server; the web application is running in Full Trust. The WPF classes have exhibited some reliability issues
		/// during development, most notably causing the IIS worker process (w3wp.exe) to increase in memory usage and 
		/// eventually crash during uploads and synchronizations. For this reason you may want to disable this feature
		/// until a .NET Framework service pack or future version provides better performance.
		/// </summary>
		[ConfigurationProperty("enableWpfMetadataExtraction", DefaultValue = true, IsRequired = true)]
		public bool EnableWpfMetadataExtraction
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableWpfMetadataExtraction.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableWpfMetadataExtraction.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether Gallery Server Pro renders user interface objects to allow a user to download the file for a media 
		/// object. Typically, this setting is used to control whether a download button is displayed in the toolbar that
		/// appears above a media object. Note that setting this value to false does not prevent a user from downloading a
		/// media object, since a user already has access to the media object if he or she can view it in the browser. To
		/// prevent certain users from viewing media objects (and thus downloading them), use private albums, disable
		/// anonymous viewing, or configure security to prevent users from viewing the objects.
		/// </summary>
		[ConfigurationProperty("enableMediaObjectDownload", DefaultValue = true, IsRequired = true)]
		public bool EnableMediaObjectDownload
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableMediaObjectDownload.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableMediaObjectDownload.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether users are allowed to download multiple media objects in a ZIP file.
		/// </summary>
		[ConfigurationProperty("enableMediaObjectZipDownload", DefaultValue = true, IsRequired = true)]
		public bool EnableMediaObjectZipDownload
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableMediaObjectZipDownload.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableMediaObjectZipDownload.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether Gallery Server Pro renders user interface objects to provide the user with a hyperlink that
		/// links directly to the visible media object. This is convenient because AJAX callbacks are used as the user navigates
		/// the media objects in an album and the url in the browser's address bar is not updated for each media object.
		/// When true, a show permalink button is displayed in the toolbar that appears above a media object.
		/// </summary>
		[ConfigurationProperty("enablePermalink", DefaultValue = true, IsRequired = true)]
		public bool EnablePermalink
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enablePermalink.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enablePermalink.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether slide show functionality is enabled. When true, a start/pause slideshow button is displayed in the 
		/// toolbar that appears above a media object. The length of time each image is shown before automatically moving
		/// to the next one is controlled by the SlideshowInterval setting. Note that only images are shown during a slide
		/// show; other objects such as videos, audio files, and documents are skipped.
		/// </summary>
		[ConfigurationProperty("enableSlideShow", DefaultValue = true, IsRequired = true)]
		public bool EnableSlideShow
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableSlideShow.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableSlideShow.ToString()] = value; }
		}

		/// <summary>
		///	The length (in pixels) of the longest edge of a thumbnail image.  This value is used when a thumbnail 
		///	image is created. The length of the shorter side is calculated automatically based on the aspect ratio of the image.
		/// </summary>
		[ConfigurationProperty("maxThumbnailLength", DefaultValue = 125, IsRequired = true)]
		[IntegerValidator(MinValue = 10, MaxValue = 100000)]
		public int MaxThumbnailLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.maxThumbnailLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.maxThumbnailLength.ToString()] = value; }
		}

		/// <summary>
		/// The quality level that thumbnail images are stored at (0 - 100).
		/// </summary>
		[ConfigurationProperty("thumbnailImageJpegQuality", DefaultValue = 70, IsRequired = true)]
		[IntegerValidator(MinValue = 1, MaxValue = 100)]
		public int ThumbnailImageJpegQuality
		{
			get { return Convert.ToInt32(this[CoreAttributes.thumbnailImageJpegQuality.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.thumbnailImageJpegQuality.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether the original image is displayed when the user clicks the thumbnail of an image.
		/// If unchecked, the optimized image is shown instead. Not applicable for non-image media objects.
		/// If <see cref="AllowAnonymousHiResViewing"/> is <c>false</c>, the original image is never shown to 
		/// anonymous users, even if this property is <c>true</c>.
		/// </summary>
		[ConfigurationProperty("thumbnailClickShowsOriginal", DefaultValue = false, IsRequired = true)]
		public bool ThumbnailClickShowsOriginal
		{
			get { return Convert.ToBoolean(this[CoreAttributes.thumbnailClickShowsOriginal.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.thumbnailClickShowsOriginal.ToString()] = value; }
		}

		/// <summary>
		/// The length (in pixels) that is added to the width of the each thumbnail image. A larger number creates 
		/// more horizontal padding between the image and the border of the thumbnail container.
		/// </summary>
		[ConfigurationProperty("thumbnailWidthBuffer", DefaultValue = 30, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 1000)]
		public int ThumbnailWidthBuffer
		{
			get { return Convert.ToInt32(this[CoreAttributes.thumbnailWidthBuffer.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.thumbnailWidthBuffer.ToString()] = value; }
		}

		/// <summary>
		/// The length (in pixels) that is added to the height of the each thumbnail image. A larger number creates 
		/// more vertical padding between the image and the border of the thumbnail container.
		/// </summary>
		[ConfigurationProperty("thumbnailHeightBuffer", DefaultValue = 60, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 1000)]
		public int ThumbnailHeightBuffer
		{
			get { return Convert.ToInt32(this[CoreAttributes.thumbnailHeightBuffer.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.thumbnailHeightBuffer.ToString()] = value; }
		}

		/// <summary>
		/// The string that is prepended to the thumbnail filename for each media object. For example, if an image
		/// named puppy.jpg is added, and this setting is "zThumb_", the thumbnail image will be named 
		/// "zThumb_puppy.jpg".	NOTE: Any file named "zThumb_puppy.jpg" that already exists will be overwritten, 
		/// so it is important to choose a value that, when prepended to media object filenames, will not 
		/// conflict with existing media objects.
		/// </summary>
		[ConfigurationProperty("thumbnailFileNamePrefix", DefaultValue = "zThumb_", IsRequired = true)]
		public string ThumbnailFileNamePrefix
		{
			get { return this[CoreAttributes.thumbnailFileNamePrefix.ToString()].ToString(); }
			set { this[CoreAttributes.thumbnailFileNamePrefix.ToString()] = value; }
		}

		/// <summary>
		/// The path to a directory where Gallery Server should store	the thumbnail images of media objects. If no path 
		/// is specified, the directory containing the original media object is used to store the thumbnail image. 
		/// The path may be relative to the root of the web application (e.g. \gs\mediaobjects), a full path to a local 
		/// resource (e.g. C:\mymedia), or a UNC path to a local or network resource (e.g. \\mynas\media). Mapped 
		/// drives present a security risk and are not supported. The initial and trailing slashes are	optional. 
		/// For relative paths, the directory separator character can be either a forward or backward slash.
		/// </summary>
		[ConfigurationProperty("thumbnailPath", DefaultValue = "", IsRequired = true)]
		public string ThumbnailPath
		{
			get { return this[CoreAttributes.thumbnailPath.ToString()].ToString(); }
			set { this[CoreAttributes.thumbnailPath.ToString()] = value; }
		}

		/// <summary>
		///	The length (in pixels) of the longest edge of an optimized image.  This value is used when an optimized
		///	image is created. The length of the shorter side is calculated automatically based on the aspect ratio of the image.
		/// </summary>
		[ConfigurationProperty("maxOptimizedLength", DefaultValue = 640, IsRequired = true)]
		[IntegerValidator(MinValue = 10, MaxValue = 100000)]
		public int MaxOptimizedLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.maxOptimizedLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.maxOptimizedLength.ToString()] = value; }
		}

		/// <summary>
		/// The quality level that optimized JPG pictures are created with. This is a number from 1 - 100, with 1 
		/// being the worst quality and 100 being the best quality. Not applicable for non-image media objects.
		/// </summary>
		[ConfigurationProperty("optimizedImageJpegQuality", DefaultValue = 70, IsRequired = true)]
		[IntegerValidator(MinValue = 1, MaxValue = 100)]
		public int OptimizedImageJpegQuality
		{
			get { return Convert.ToInt32(this[CoreAttributes.optimizedImageJpegQuality.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.optimizedImageJpegQuality.ToString()] = value; }
		}

		/// <summary>
		/// The size (in KB) above which an image is compressed to create an optimized version.
		/// Not applicable for non-image media objects.
		/// </summary>
		[ConfigurationProperty("optimizedImageTriggerSizeKB", DefaultValue = 50, IsRequired = true)]
		[IntegerValidator(MinValue = 0)]
		public int OptimizedImageTriggerSizeKB
		{
			get { return Convert.ToInt32(this[CoreAttributes.optimizedImageTriggerSizeKB.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.optimizedImageTriggerSizeKB.ToString()] = value; }
		}

		/// <summary>
		/// The string that is prepended to the optimized filename for images. This setting is only used for image
		/// media objects where an optimized image file is created. For example, if an image named
		/// puppy.jpg is added, and this setting is "zOpt_", the optimized image will be named "zOpt_puppy.jpg".
		/// NOTE: Any file named "zOpt_puppy.jpg" that already exists will be overwritten, 
		/// so it is important to choose a value that, when prepended to media object filenames, will not 
		/// conflict with existing media objects.
		/// </summary>
		[ConfigurationProperty("optimizedFileNamePrefix", DefaultValue = "zOpt_", IsRequired = true)]
		public string OptimizedFileNamePrefix
		{
			get { return this[CoreAttributes.optimizedFileNamePrefix.ToString()].ToString(); }
			set { this[CoreAttributes.optimizedFileNamePrefix.ToString()] = value; }
		}

		/// <summary>
		/// The path to a directory where Gallery Server should store	the optimized images of media objects. If no path 
		/// is specified, the directory containing the original media object is used to store the thumbnail image. 
		/// The path may be relative to the root of the web application (e.g. \gs\mediaobjects), a full path to a local 
		/// resource (e.g. C:\mymedia), or a UNC path to a local or network resource (e.g. \\mynas\media). Mapped 
		/// drives present a security risk and are not supported. The initial and trailing slashes are	optional. 
		/// For relative paths, the directory separator character can be either a forward or backward slash.
		/// Not applicable for non-image media objects.
		/// </summary>
		[ConfigurationProperty("optimizedPath", DefaultValue = "", IsRequired = true)]
		public string OptimizedPath
		{
			get { return this[CoreAttributes.optimizedPath.ToString()].ToString(); }
			set { this[CoreAttributes.optimizedPath.ToString()] = value; }
		}

		/// <summary>
		/// The quality level that original JPG pictures are saved at. This is only used when the original is 
		/// modified by the user, such as rotation. Not applicable for non-image media objects.
		/// </summary>
		[ConfigurationProperty("originalImageJpegQuality", DefaultValue = 95, IsRequired = true)]
		[IntegerValidator(MinValue = 1, MaxValue = 100)]
		public int OriginalImageJpegQuality
		{
			get { return Convert.ToInt32(this[CoreAttributes.originalImageJpegQuality.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.originalImageJpegQuality.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether to discard the original image when it is added to the gallery. This option, when enabled, 
		/// helps reduce disk space usage. This option applies only to images, and only when they are added through an 
		/// upload or by synchronizing. Changing this setting does not affect existing media objects. When false, 
		/// users still have the option to discard the original image on the Add Objects page by unchecking the 
		/// corresponding checkbox.
		/// </summary>
		[ConfigurationProperty("discardOriginalImageDuringImport", DefaultValue = false, IsRequired = true)]
		public bool DiscardOriginalImageDuringImport
		{
			get { return Convert.ToBoolean(this[CoreAttributes.discardOriginalImageDuringImport.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.discardOriginalImageDuringImport.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether to apply a watermark to optimized and original images. If true, the text in the watermarkText
		/// property is applied to images, and the image specified in watermarkImagePath is overlayed on the image. If
		/// watermarkText is empty, or if watermarkImagePath is empty or does not refer to a valid image, that watermark
		/// is not applied. If applyWatermarkToThumbnails = true, then the watermark is also applied to thumbnails.
		/// </summary>
		[ConfigurationProperty("applyWatermark", DefaultValue = false, IsRequired = true)]
		public bool ApplyWatermark
		{
			get { return Convert.ToBoolean(this[CoreAttributes.applyWatermark.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.applyWatermark.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether to apply the text and/or image watermark to thumbnail images. This property is ignored if 
		/// applyWatermark = false.
		/// </summary>
		[ConfigurationProperty("applyWatermarkToThumbnails", DefaultValue = false, IsRequired = true)]
		public bool ApplyWatermarkToThumbnails
		{
			get { return Convert.ToBoolean(this[CoreAttributes.applyWatermarkToThumbnails.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.applyWatermarkToThumbnails.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the text to apply to images in the gallery. The text is applied in a single line.
		/// </summary>
		[ConfigurationProperty("watermarkText", DefaultValue = "", IsRequired = true)]
		public string WatermarkText
		{
			get { return this[CoreAttributes.watermarkText.ToString()].ToString(); }
			set { this[CoreAttributes.watermarkText.ToString()] = value; }
		}

		/// <summary>
		/// The font used for the watermark text. If the font is not installed on the web server, a generic font will 
		/// be substituted.
		/// </summary>
		[ConfigurationProperty("watermarkTextFontName", DefaultValue = "Verdana", IsRequired = true)]
		public string WatermarkTextFontName
		{
			get { return this[CoreAttributes.watermarkTextFontName.ToString()].ToString(); }
			set { this[CoreAttributes.watermarkTextFontName.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the height, in pixels, of the watermark text. This value is ignored if the property
		/// WatermarkTextWidthPercent is non-zero. Valid values are 0 - 10000.
		/// </summary>
		[ConfigurationProperty("watermarkTextFontSize", DefaultValue = 12, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int WatermarkTextFontSize
		{
			get { return Convert.ToInt32(this[CoreAttributes.watermarkTextFontSize.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.watermarkTextFontSize.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the percent of the overall width of the recipient image that should be covered with the
		/// watermark text. The size of the text is automatically scaled up or down to achieve the desired width. For example,
		/// a value of 50 means the text is 50% as wide as the recipient image. Valid values are 0 - 100. The text is never
		/// rendered in a font smaller than 6 pixels, so in cases of long text it may stretch wider than the percentage
		/// specified in this setting.
		/// A value of 0 turns off this feature and causes the text size to be determined by the 
		/// WatermarkTextFontSize property.
		/// </summary>
		[ConfigurationProperty("watermarkTextWidthPercent", DefaultValue = 50, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 100)]
		public int WatermarkTextWidthPercent
		{
			get { return Convert.ToInt32(this[CoreAttributes.watermarkTextWidthPercent.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.watermarkTextWidthPercent.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the color of the watermark text. The color can be specified as hex (e.g. #336699), RGB (e.g. 127,55,95),
		/// or one of the System.Color.KnownColor enum values (e.g. Maroon).
		/// </summary>
		[ConfigurationProperty("watermarkTextColor", DefaultValue = "Navy", IsRequired = true)]
		public string WatermarkTextColor
		{
			get { return this[CoreAttributes.watermarkTextColor.ToString()].ToString(); }
			set { this[CoreAttributes.watermarkTextColor.ToString()] = value; }
		}

		/// <summary>
		/// The opacity of the watermark text. This is a value from 0 to 100, with 0 being invisible and 100 being solid, 
		/// with no transparency.
		/// </summary>
		[ConfigurationProperty("watermarkTextOpacityPercent", DefaultValue = 30, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 100)]
		public int WatermarkTextOpacityPercent
		{
			get { return Convert.ToInt32(this[CoreAttributes.watermarkTextOpacityPercent.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.watermarkTextOpacityPercent.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the location for the watermark text on the recipient image. This value maps to the 
		/// enumeration System.Drawing.ContentAlignment, and must be one of the following nine values:
		/// TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight.
		/// </summary>
		[ConfigurationProperty("watermarkTextLocation", DefaultValue = "BottomCenter", IsRequired = true)]
		[RegexStringValidator(@"[TopLeft|TopCenter|TopRight|MiddleLeft|MiddleCenter|MiddleRight|BottomLeft|BottomCenter|BottomRight]")]
		public string WatermarkTextLocation
		{
			get { return this[CoreAttributes.watermarkTextLocation.ToString()].ToString(); }
			set { this[CoreAttributes.watermarkTextLocation.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the full or relative path to a watermark image to be applied to the recipient image. The image
		/// must be in a format that allows it to be instantiated in a System.Drawing.Bitmap object. Relative paths
		/// are relative to the root of the web application. The directory separator character can be either a 
		/// forward or backward slash, and, for relative paths, the initial slash is optional. The following are
		/// all valid: "/images/mywatermark.jpg", "images/mywatermark.jpg", "\images\mywatermark.jpg", 
		/// "images\mywatermark.jpg", "C:\images\mywatermark.jpg"
		/// </summary>
		[ConfigurationProperty("watermarkImagePath", DefaultValue = "", IsRequired = true)]
		public string WatermarkImagePath
		{
			get { return this[CoreAttributes.watermarkImagePath.ToString()].ToString(); }
			set { this[CoreAttributes.watermarkImagePath.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the percent of the overall width of the recipient image that should be covered with the
		/// watermark image. The size of the image is automatically scaled to achieve the desired width. For example,
		/// a value of 50 means the watermark image is 50% as wide as the recipient image. Valid values are 0 - 100.
		/// A value of 0 turns off this feature and causes the image to be rendered its actual size.
		/// </summary>
		[ConfigurationProperty("watermarkImageWidthPercent", DefaultValue = 50, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 100)]
		public int WatermarkImageWidthPercent
		{
			get { return Convert.ToInt32(this[CoreAttributes.watermarkImageWidthPercent.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.watermarkImageWidthPercent.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the opacity of the watermark image. Valid values are 0 - 100, with 0 being completely
		/// transparent and 100 completely opaque.
		/// </summary>
		[ConfigurationProperty("watermarkImageOpacityPercent", DefaultValue = 30, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 100)]
		public int WatermarkImageOpacityPercent
		{
			get { return Convert.ToInt32(this[CoreAttributes.watermarkImageOpacityPercent.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.watermarkImageOpacityPercent.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the location for the watermark image on the recipient image. This value maps to the 
		/// enumeration System.Drawing.ContentAlignment, and must be one of the following nine values:
		/// TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight.
		/// </summary>
		[ConfigurationProperty("watermarkImageLocation", DefaultValue = "Center", IsRequired = true)]
		[RegexStringValidator(@"[TopLeft|TopCenter|TopRight|MiddleLeft|MiddleCenter|MiddleRight|BottomLeft|BottomCenter|BottomRight]")]
		public string WatermarkImageLocation
		{
			get { return this[CoreAttributes.watermarkImageLocation.ToString()].ToString(); }
			set { this[CoreAttributes.watermarkImageLocation.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether the Gallery Server Pro administrator (specified in EmailToName/EmailToAddress)
		/// is sent a report when a web site error occurs.  A valid SMTP server must be specified if this
		/// is set to true (attribute SmtpServer).
		/// </summary>
		[ConfigurationProperty("sendEmailOnError", DefaultValue = true, IsRequired = true)]
		public bool SendEmailOnError
		{
			get { return Convert.ToBoolean(this[CoreAttributes.sendEmailOnError.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.sendEmailOnError.ToString()] = value; }
		}

		/// <summary>
		/// The name associated with the EmailFromAddress email address. Emails sent from Gallery Server 
		/// will appear to be sent from this person.
		/// </summary>
		[ConfigurationProperty("emailFromName", DefaultValue = "Gallery Server Admin", IsRequired = true)]
		public string EmailFromName
		{
			get { return this[CoreAttributes.emailFromName.ToString()].ToString(); }
			set { this[CoreAttributes.emailFromName.ToString()] = value; }
		}

		/// <summary>
		/// The email address associated with the EmailFromName attribute. Emails sent from Gallery Server 
		/// will appear to be sent from this email address.
		/// </summary>
		[ConfigurationProperty("emailFromAddress", DefaultValue = "yourname@yourisp.com", IsRequired = true)]
		[RegexStringValidator(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
		public string EmailFromAddress
		{
			get { return this[CoreAttributes.emailFromAddress.ToString()].ToString(); }
			set { this[CoreAttributes.emailFromAddress.ToString()] = value; }
		}

		/// <summary>
		/// The name associated with the EmailToAddress email address. Emails sent from Gallery Server 
		/// will be sent to this person. Note that the email address is set in the attribute EmailToAddress.
		/// </summary>
		[ConfigurationProperty("emailToName", DefaultValue = "Gallery Server Pro Administrator", IsRequired = true)]
		public string EmailToName
		{
			get { return this[CoreAttributes.emailToName.ToString()].ToString(); }
			set { this[CoreAttributes.emailToName.ToString()] = value; }
		}

		/// <summary>
		/// The email address associated with the EmailToName attribute. Emails sent from Gallery Server 
		/// will be sent to this email address. Note that the name associated with this address is set in
		/// the attribute EmailToName.
		/// </summary>
		[ConfigurationProperty("emailToAddress", DefaultValue = "admin@yourisp.com", IsRequired = true)]
		[RegexStringValidator(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")]
		public string EmailToAddress
		{
			get { return this[CoreAttributes.emailToAddress.ToString()].ToString(); }
			set { this[CoreAttributes.emailToAddress.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the IP address or name of the SMTP server used to send emails. (Examples: 127.0.0.1, 
		/// Godzilla, mail.yourisp.com) This value will override the SMTP server setting that may be in the 
		/// system.net mailSettings section of the web.config file (either explicitly or inherited from a 
		/// parent web.config file). Leave this setting blank to use the value in web.config or if you are 
		/// not using the email functionality.
		/// </summary>
		[ConfigurationProperty("smtpServer", DefaultValue = "", IsRequired = true)]
		public string SmtpServer
		{
			get { return this[CoreAttributes.smtpServer.ToString()].ToString(); }
			set { this[CoreAttributes.smtpServer.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the SMTP server port number used to send emails. This value will override the SMTP 
		/// server port setting that may be in the system.net mailSettings section of the web.config file 
		/// (either explicitly or inherited from a parent web.config file). Leave this setting blank to 
		/// use the value in web.config or if you are not using the email functionality. Defaults to 25 
		/// if not specified here or in web.config.
		/// </summary>
		[ConfigurationProperty("smtpServerPort", DefaultValue = "", IsRequired = true)]
		public string SmtpServerPort
		{
			get { return this[CoreAttributes.smtpServerPort.ToString()].ToString(); }
			set { this[CoreAttributes.smtpServerPort.ToString()] = value; }
		}

		/// <summary>
		/// Specifies whether e-mail functionality uses Secure Sockets Layer (SSL) to encrypt the connection.
		/// </summary>
		[ConfigurationProperty("sendEmailUsingSsl", DefaultValue = true, IsRequired = true)]
		public bool SendEmailUsingSsl
		{
			get { return Convert.ToBoolean(this[CoreAttributes.sendEmailUsingSsl.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.sendEmailUsingSsl.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether a video, audio or other dynamic object will automatically start playing in the user's browser.
		/// </summary>
		[ConfigurationProperty("autoStartMediaObject", DefaultValue = true, IsRequired = true)]
		public bool AutoStartMediaObject
		{
			get { return Convert.ToBoolean(this[CoreAttributes.autoStartMediaObject.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.autoStartMediaObject.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the default width, in pixels, of the browser object that plays a video file. Typically 
		/// this refers to the &lt;object&gt; tag that contains the video, resulting in a tag similar to this:
		/// &lt;object style="width:640px;height:480px;" ... &gt;
		/// </summary>
		[ConfigurationProperty("defaultVideoPlayerWidth", DefaultValue = 640, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int DefaultVideoPlayerWidth
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultVideoPlayerWidth.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultVideoPlayerWidth.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the default height, in pixels, of the browser object that plays a video file. Typically 
		/// this refers to the &lt;object&gt; tag that contains the video, resulting in a tag similar to this:
		/// &lt;object style="width:640px;height:480px;" ... &gt;
		/// </summary>
		[ConfigurationProperty("defaultVideoPlayerHeight", DefaultValue = 480, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int DefaultVideoPlayerHeight
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultVideoPlayerHeight.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultVideoPlayerHeight.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the default width, in pixels, of the browser object that plays an audio file. Typically 
		/// this refers to the &lt;object&gt; tag that contains the audio file, resulting in a tag similar to this:
		/// &lt;object style="width:300px;height:200px;" ... &gt;
		/// </summary>
		[ConfigurationProperty("defaultAudioPlayerWidth", DefaultValue = 300, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int DefaultAudioPlayerWidth
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultAudioPlayerWidth.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultAudioPlayerWidth.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the default height, in pixels, of the browser object that plays an audio file. Typically 
		/// this refers to the &lt;object&gt; tag that contains the audio file, resulting in a tag similar to this:
		/// &lt;object style="width:300px;height:200px;" ... &gt;
		/// </summary>
		[ConfigurationProperty("defaultAudioPlayerHeight", DefaultValue = 200, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int DefaultAudioPlayerHeight
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultAudioPlayerHeight.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultAudioPlayerHeight.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the default width, in pixels, of the browser object that displays a generic media object.
		/// A generic media object is defined as any media object that is not an image,	audio, or video file. This
		/// includes Shockwave Flash, Adobe Reader, text files, Word documents and others. The value specified here
		/// is sent to the browser as the width for the object element containing this media object, resulting in syntax 
		/// similar to this: &lt;object style="width:640px;height:480px;" ... &gt; This setting applies only to objects 
		/// rendered within the browser, such as Shockwave Flash. Objects sent to the browser via a download
		/// link, such as text files, PDF files, and Word documents, ignore this setting.
		/// </summary>
		[ConfigurationProperty("defaultGenericObjectWidth", DefaultValue = 640, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int DefaultGenericObjectWidth
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultGenericObjectWidth.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultGenericObjectWidth.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the default height, in pixels, of the browser object that displays a generic media object.
		/// A generic media object is defined as any media object that is not an image,	audio, or video file. This
		/// includes Shockwave Flash, Adobe Reader, text files, Word documents and others. The value specified here
		/// is sent to the browser as the width for the object element containing this media object, resulting in syntax 
		/// similar to this: &lt;object style="width:640px;height:480px;" ... &gt; This setting applies only to objects 
		/// rendered within the browser, such as Shockwave Flash. Objects sent to the browser via a download
		/// link, such as text files, PDF files, and Word documents, ignore this setting.
		/// </summary>
		[ConfigurationProperty("defaultGenericObjectHeight", DefaultValue = 480, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 10000)]
		public int DefaultGenericObjectHeight
		{
			get { return Convert.ToInt32(this[CoreAttributes.defaultGenericObjectHeight.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.defaultGenericObjectHeight.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the maximum size, in kilobytes, of the files that can be uploaded.
		/// Use this setting to keep users from uploading very large files and to help guard against Denial of 
		/// Service (DOS) attacks. A value of zero (0) indicates there is no restriction on upload size (unlimited).
		/// This value applies to the content length of the entire upload request, not just the file. For example, if
		/// this value is 1024 KB and the user attempts to upload two 800 KB images, the request will fail because
		/// the total content length is larger than 1024 KB. This setting is not used during synchronization.
		/// </summary>
		[ConfigurationProperty("maxUploadSize", DefaultValue = 2097151, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 2147483647)]
		public int MaxUploadSize
		{
			get { return Convert.ToInt32(this[CoreAttributes.maxUploadSize.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.maxUploadSize.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether a user can upload a physical file to the gallery, such as an image or video file stored
		/// on a local hard drive. The user must also be authenticated and a member of a role with AllowAddMediaObject 
		/// or AllowAdministerSite permission. This setting is not used during synchronization.
		/// </summary>
		[ConfigurationProperty("allowAddLocalContent", DefaultValue = true, IsRequired = true)]
		public bool AllowAddLocalContent
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowAddLocalContent.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowAddLocalContent.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether a user can add a link to external content, such as a YouTube video, to the gallery. 
		/// The user must also be authenticated and a member of a role with AllowAddMediaObject 
		/// or AllowAdministerSite permission. This setting is not used during synchronization.
		/// </summary>
		[ConfigurationProperty("allowAddExternalContent", DefaultValue = true, IsRequired = true)]
		public bool AllowAddExternalContent
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowAddExternalContent.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowAddExternalContent.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether users can view galleries without logging in. When false, users are redirected to a login
		/// page when any album is requested. Private albums are never shown to anonymous users, even when this 
		/// property is true.
		/// </summary>
		[ConfigurationProperty("allowAnonymousBrowsing", DefaultValue = true, IsRequired = true)]
		public bool AllowAnonymousBrowsing
		{
			get { return Convert.ToBoolean(this[CoreAttributes.allowAnonymousBrowsing.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.allowAnonymousBrowsing.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the number of objects to display at a time. For example, if an album has more than this number of
		/// gallery objects, paging controls appear to assist the user in navigating to them. A value of zero disables 
		/// the paging feature.
		/// </summary>
		[ConfigurationProperty("pageSize", DefaultValue = 50, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 2147483647)]
		public int PageSize
		{
			get { return Convert.ToInt32(this[CoreAttributes.pageSize.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.pageSize.ToString()] = value; }
		}

		/// <summary>
		/// Gets or sets the location for the pager used to navigate large collections of objects. This value maps to the 
		/// enumeration GalleryServerPro.Business.PagerPosition, and must be one of the following values:
		/// Top, Bottom, TopAndBottom. This value is ignored when paging is disabled (<see cref="PageSize"/> = 0).
		/// </summary>
		[ConfigurationProperty("pagerLocation", DefaultValue = "TopAndBottom", IsRequired = true)]
		[RegexStringValidator(@"[Top|Bottom|TopAndBottom]")]
		public string PagerLocation
		{
			get { return this[CoreAttributes.pagerLocation.ToString()].ToString(); }
			set { this[CoreAttributes.pagerLocation.ToString()] = value; }
		}

		/// <summary>
		/// Indicates the maximum number of error objects to persist to the data store. When the number of errors exceeds this
		/// value, the oldest item is purged to make room for the new item. A value of zero means no limit is enforced.
		/// </summary>
		[ConfigurationProperty("maxNumberErrorItems", DefaultValue = 200, IsRequired = true)]
		[IntegerValidator(MinValue = 0, MaxValue = 2147483647)]
		public int MaxNumberErrorItems
		{
			get { return Convert.ToInt32(this[CoreAttributes.maxNumberErrorItems.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.maxNumberErrorItems.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether anonymous users are allowed to create accounts.
		/// </summary>
		[ConfigurationProperty("enableSelfRegistration", DefaultValue = false, IsRequired = true)]
		public bool EnableSelfRegistration
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableSelfRegistration.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableSelfRegistration.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether e-mail verification is required when a user registers an account. When true, the account is 
		/// initially disabled and an email is sent to the user with a verification link. When clicked, user is approved 
		/// and logged on, unless <see cref="RequireApprovalForSelfRegisteredUser"/> is enabled, in which case an administrator
		/// must approve the account before the user can log on. Setting this to true reduces spam activity and guarantees that 
		/// a valid e-mail address is associated with the user. When the setting is false, an e-mail address is not required 
		/// and the user account is immediately created. This setting is ignored when 
		/// <see cref="EnableSelfRegistration">self registration</see> is disabled.
		/// </summary>
		[ConfigurationProperty("requireEmailValidationForSelfRegisteredUser", DefaultValue = true, IsRequired = true)]
		public bool RequireEmailValidationForSelfRegisteredUser
		{
			get { return Convert.ToBoolean(this[CoreAttributes.requireEmailValidationForSelfRegisteredUser.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.requireEmailValidationForSelfRegisteredUser.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether an administrator must approve newly created accounts before the user can log on. When true, 
		/// the account is disabled until it is approved by an administrator. When a user registers an account, an e-mail
		/// is sent to each user specified in <see cref="UsersToNotifyWhenAccountIsCreated"/>. Only users belonging to a
		/// role with AllowAdministerSite permission can approve a user. If <see cref="RequireEmailValidationForSelfRegisteredUser"/>
		/// is enabled, the e-mail requesting administrator approval is not sent until the user verifies the e-mail address.
		/// This setting is ignored when <see cref="EnableSelfRegistration">self registration</see> is disabled.
		/// </summary>
		[ConfigurationProperty("requireApprovalForSelfRegisteredUser", DefaultValue = false, IsRequired = true)]
		public bool RequireApprovalForSelfRegisteredUser
		{
			get { return Convert.ToBoolean(this[CoreAttributes.requireApprovalForSelfRegisteredUser.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.requireApprovalForSelfRegisteredUser.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether account names are primarily e-mail addresses. When true, certain forms, such as the self registration
		/// wizard, assume e-mail addresses are used as account names. For example, when this value is false, the self registration
		/// wizard includes fields for both an account name and an e-mail address, but when true it only requests an e-mail address.
		/// This setting is ignored when <see cref="EnableSelfRegistration">self registration</see> is disabled.
		/// </summary>
		[ConfigurationProperty("useEmailForAccountName", DefaultValue = false, IsRequired = true)]
		public bool UseEmailForAccountName
		{
			get { return Convert.ToBoolean(this[CoreAttributes.useEmailForAccountName.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.useEmailForAccountName.ToString()] = value; }
		}

		/// <summary>
		/// A comma-delimited list of roles to assign when a user registers a new account. This setting is ignored when 
		/// <see cref="EnableSelfRegistration">self registration</see> is disabled and when an account is created by an 
		/// administrator. Ex: "ReadOnly,NoWatermark"
		/// </summary>
		[ConfigurationProperty("defaultRolesForSelfRegisteredUser", DefaultValue = "", IsRequired = true)]
		public string DefaultRolesForSelfRegisteredUser
		{
			get { return this[CoreAttributes.defaultRolesForSelfRegisteredUser.ToString()].ToString(); }
			set { this[CoreAttributes.defaultRolesForSelfRegisteredUser.ToString()] = value; }
		}

		/// <summary>
		/// A comma-delimited list of account names of users to receive an e-mail notification when an account is created.
		/// When <see cref="RequireEmailValidationForSelfRegisteredUser"/> is enabled, the e-mail is not sent until the
		/// user verifies the e-mail address.
		/// Ex: "Admin, Billybob". Applies whether an account is self-created or created by an administrator.
		/// </summary>
		[ConfigurationProperty("usersToNotifyWhenAccountIsCreated", DefaultValue="", IsRequired = true)]
		public string UsersToNotifyWhenAccountIsCreated
		{
			get { return this[CoreAttributes.usersToNotifyWhenAccountIsCreated.ToString()].ToString(); }
			set { this[CoreAttributes.usersToNotifyWhenAccountIsCreated.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether each user is associated owner to a unique album. The title of the album is based on the 
		/// template in the <see cref="UserAlbumNameTemplate"/> property. The album is created when the account is created or
		/// if the album does not exist when the user logs on. It is created in the album specified in the 
		/// <see cref="UserAlbumParentAlbumId"/> property.
		/// (Not implemented: If AssignUserAsOwnerOfUserAlbum is true, then the user is given ownership of
		/// this album.)</summary>
		[ConfigurationProperty("enableUserAlbum", DefaultValue = false, IsRequired = true)]
		public bool EnableUserAlbum
		{
			get { return Convert.ToBoolean(this[CoreAttributes.enableUserAlbum.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.enableUserAlbum.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the ID of the album containing user albums. This setting is ignored when <see cref="EnableUserAlbum"/>
		/// is false. This property may have a value of zero (0) when user albums are disabled.
		/// </summary>
		[ConfigurationProperty("userAlbumParentAlbumId", IsRequired = true)]
		public int UserAlbumParentAlbumId
		{
			get { return Convert.ToInt32(this[CoreAttributes.userAlbumParentAlbumId.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.userAlbumParentAlbumId.ToString()] = value; }
		}

		///// <summary>
		///// Indicates whether to assign a user as the owner for the album that was created when an account is created. Applies
		///// only when <see cref="EnableUserAlbum"/> is true.
		///// </summary>
		//[ConfigurationProperty("assignUserAsOwnerOfUserAlbum", DefaultValue = false, IsRequired = true)]
		//public bool AssignUserAsOwnerOfUserAlbum
		//{
		//  get { return Convert.ToBoolean(this[CoreAttributes.assignUserAsOwnerOfUserAlbum.ToString()], CultureInfo.InvariantCulture); }
		//  set { this[CoreAttributes.assignUserAsOwnerOfUserAlbum.ToString()] = value; }
		//}

		/// <summary>
		/// Specifies the template to use for naming the album that is created for new users. Applies only when 
		/// <see cref="EnableUserAlbum"/> is true. The placeholder string {UserName}, if present, is replaced 
		/// by the account name.
		/// </summary>
		[ConfigurationProperty("userAlbumNameTemplate", DefaultValue = "{UserName}'s album", IsRequired = true)]
		public string UserAlbumNameTemplate
		{
			get { return this[CoreAttributes.userAlbumNameTemplate.ToString()].ToString(); }
			set { this[CoreAttributes.userAlbumNameTemplate.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the template to use for the album summary of a newly created user album. Applies only when 
		/// <see cref="EnableUserAlbum"/> is true. No placeholder strings are supported.
		/// </summary>
		[ConfigurationProperty("userAlbumSummaryTemplate", IsRequired = true)]
		public string UserAlbumSummaryTemplate
		{
			get { return this[CoreAttributes.userAlbumSummaryTemplate.ToString()].ToString(); }
			set { this[CoreAttributes.userAlbumSummaryTemplate.ToString()] = value; }
		}

		/// <summary>
		/// Indicates whether to redirect the user to his or her album after logging in. If set to false, the current page is
		/// re-loaded or, if there isn't a page, the user is shown the top level album for which the user has view access. This setting 
		/// is ignored when <see cref="EnableUserAlbum"/> is false.</summary>
		[ConfigurationProperty("redirectToUserAlbumAfterLogin", DefaultValue = false, IsRequired = true)]
		public bool RedirectToUserAlbumAfterLogin
		{
			get { return Convert.ToBoolean(this[CoreAttributes.redirectToUserAlbumAfterLogin.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.redirectToUserAlbumAfterLogin.ToString()] = value; }
		}

		/// <summary>
		/// The absolute or relative path to the jQuery script file. Relative paths can be relative to the root of the web site
		/// (e.g. /mysites/mygallery/gs/script/jquery-1.3.2.min.js) or relative to the directory containing the Gallery Server 
		/// Pro user controls and other resources (e.g. /script/jquery-1.3.2.min.js). It is NOT VALID to specify a path 
		/// relative to the root of the web application. For relative paths the initial slash is	optional and the directory 
		/// separator character can be either a forward or backward slash. Absolute paths should be a full URI 
		/// (e.g. http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js). It is not valid to specify a UNC path, 
		/// mapped drive path, or path to the local file system (e.g. "C:\scripts\jquery.js").
		/// </summary>
		/// <remarks>The path is returned exactly how it appears in the configuration file.</remarks>
		[ConfigurationProperty("jQueryScriptPath", DefaultValue = "", IsRequired = true)]
		public string JQueryScriptPath
		{
			get { return this[CoreAttributes.jQueryScriptPath.ToString()].ToString(); }
			set { this[CoreAttributes.jQueryScriptPath.ToString()] = value; }
		}

		/// <summary>
		/// The name of the Membership provider for the gallery users. Optional. When not specified, the default provider specified
		/// in web.config is used.
		/// </summary>
		/// <remarks>The name of the Membership provider for the gallery users.</remarks>
		[ConfigurationProperty("membershipProviderName", DefaultValue = "", IsRequired = true)]
		public string MembershipProviderName
		{
			get { return this[CoreAttributes.membershipProviderName.ToString()].ToString(); }
			set { this[CoreAttributes.membershipProviderName.ToString()] = value; }
		}

		/// <summary>
		/// The name of the Role provider for the gallery users. Optional. When not specified, the default provider specified
		/// in web.config is used.
		/// </summary>
		/// <remarks>The name of the Role provider for the gallery users.</remarks>
		[ConfigurationProperty("roleProviderName", DefaultValue = "", IsRequired = true)]
		public string RoleProviderName
		{
			get { return this[CoreAttributes.roleProviderName.ToString()].ToString(); }
			set { this[CoreAttributes.roleProviderName.ToString()] = value; }
		}

		/// <summary>
		/// Specifies the product key for this installation of Gallery Server Pro.
		/// </summary>
		[ConfigurationProperty("productKey", DefaultValue = "", IsRequired = true)]
		public string ProductKey
		{
			get { return this[CoreAttributes.productKey.ToString()].ToString(); }
			set { this[CoreAttributes.productKey.ToString()] = value; }
		}

		#endregion

		#region Private Methods

		private static void ValidateEmptyAlbumThumbnailWidthToHeightRatio(Single value)
		{
			if (value <= 0)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "Invalid configuration value \"{0}\". The configuration setting \"{1}\" in galleryserverpro.config must be a number greater than 0.", value, CoreAttributes.emptyAlbumThumbnailWidthToHeightRatio.ToString()));
			}
		}

		private static void ValidateMediaObjectTransitionDuration(Single value)
		{
			if (value <= 0)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "Invalid configuration value \"{0}\". The configuration setting \"{1}\" in galleryserverpro.config must be a number greater than 0. If you wish to turn off the transition effect, set the mediaObjectTransitionType configuration setting to \"None\".", value, CoreAttributes.mediaObjectTransitionDuration.ToString()));
			}
		}

		#endregion
	}

	/// <summary>
	/// Provides read/write access to the galleryServerPro/dataStore section of galleryserverpro.config.
	/// </summary>
	public class DataStore : ConfigurationElement
	{
		private enum CoreAttributes
		{
			albumTitleLength,
			albumDirectoryNameLength,
			albumSummaryLength,
			mediaObjectTitleLength,
			mediaObjectFileNameLength,
			mediaObjectHashKeyLength,
			mediaObjectExternalHtmlSourceLength,
			mediaObjectExternalTypeLength,
			mediaObjectMetadataDescriptionLength,
			mediaObjectMetadataValueLength,
			roleNameLength,
			ownedByLength,
			ownerRoleNameLength,
			createdByLength,
			lastModifiedByLength,
			errorExTypeLength,
			errorExMsgLength,
			errorExSourceLength,
			errorUrlLength
		}

		/// <summary>
		/// The galleryServerPro/dataStore custom configuration section in galleryserverpro.config.
		/// </summary>
		public DataStore() { }

		/// <summary>
		/// The maximum allowed string length for an album title.
		/// </summary>
		[ConfigurationProperty("albumTitleLength", DefaultValue = 200, IsRequired = true)]
		public int AlbumTitleLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.albumTitleLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.albumTitleLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum length for a directory name in the media objects library. Each directory name matches the title
		///	of the matching album, with the following exceptions: (1) Characters that are not valid for directory names
		///	are removed, and (2) the name is shortened (if necessary) to the maximum length specified here.
		/// </summary>
		[ConfigurationProperty("albumDirectoryNameLength", DefaultValue = 25, IsRequired = true)]
		public int AlbumDirectoryNameLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.albumDirectoryNameLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.albumDirectoryNameLength.ToString()] = value; }
		}

		/// <summary>
		/// The maximum allowed string length for an album summary.
		/// </summary>
		[ConfigurationProperty("albumSummaryLength", DefaultValue = 1500, IsRequired = true)]
		public int AlbumSummaryLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.albumSummaryLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.albumSummaryLength.ToString()] = value; }
		}

		/// <summary>
		/// The maximum allowed string length for a media object title (caption).
		/// </summary>
		[ConfigurationProperty("mediaObjectTitleLength", DefaultValue = 255, IsRequired = true)]
		public int MediaObjectTitleLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectTitleLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectTitleLength.ToString()] = value; }
		}

		/// <summary>
		/// The maximum length allowed for a media object filename. This must be the smaller of the restrictions set 
		/// by the operating system and the corresponding data types in the data store (ThumbnailFilename,
		/// OptimizedFilename, OriginalFilename).
		/// </summary>
		[ConfigurationProperty("mediaObjectFileNameLength", DefaultValue = 20, IsRequired = true)]
		public int MediaObjectFileNameLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectFileNameLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectFileNameLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum length of the hash key generated for each media object. Gallery Server generates 47-character
		///	hash keys, so this value must be greater than or equal to 47.
		/// </summary>
		[ConfigurationProperty("mediaObjectHashKeyLength", DefaultValue = 47, IsRequired = true)]
		public int MediaObjectHashKeyLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectHashKeyLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectHashKeyLength.ToString()] = value; }
		}

		/// <summary>
		/// The maximum allowed string length for a media object's external HTML source data.
		/// </summary>
		[ConfigurationProperty("mediaObjectExternalHtmlSourceLength", DefaultValue = 1000, IsRequired = true)]
		public int MediaObjectExternalHtmlSourceLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectExternalHtmlSourceLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectExternalHtmlSourceLength.ToString()] = value; }
		}

		/// <summary>
		/// The maximum allowed string length for a media object's external type.
		/// </summary>
		[ConfigurationProperty("mediaObjectExternalTypeLength", DefaultValue = 15, IsRequired = true)]
		public int MediaObjectExternalTypeLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectExternalTypeLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectExternalTypeLength.ToString()] = value; }
		}

		/// <summary>
		/// The maximum allowed string length for a metadata description.
		/// </summary>
		[ConfigurationProperty("mediaObjectMetadataDescriptionLength", DefaultValue = 100, IsRequired = true)]
		public int MediaObjectMetadataDescriptionLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectMetadataDescriptionLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectMetadataDescriptionLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for a metadata value.
		/// </summary>
		[ConfigurationProperty("mediaObjectMetadataValueLength", DefaultValue = 2000, IsRequired = true)]
		public int MediaObjectMetadataValueLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.mediaObjectMetadataValueLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.mediaObjectMetadataValueLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for a role name.
		/// </summary>
		[ConfigurationProperty("roleNameLength", DefaultValue = 256, IsRequired = true)]
		public int RoleNameLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.roleNameLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.roleNameLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the owned by user name.
		/// </summary>
		[ConfigurationProperty("ownedByLength", DefaultValue = 256, IsRequired = true)]
		public int OwnedByLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.ownedByLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.ownedByLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the owner role name.
		/// </summary>
		[ConfigurationProperty("ownerRoleNameLength", DefaultValue = 256, IsRequired = true)]
		public int OwnerRoleNameLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.ownerRoleNameLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.ownerRoleNameLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the created by user name.
		/// </summary>
		[ConfigurationProperty("createdByLength", DefaultValue = 256, IsRequired = true)]
		public int CreatedByLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.createdByLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.createdByLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the last modified by user name.
		/// </summary>
		[ConfigurationProperty("lastModifiedByLength", DefaultValue = 256, IsRequired = true)]
		public int LastModifiedByLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.lastModifiedByLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.lastModifiedByLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the exception type.
		/// </summary>
		[ConfigurationProperty("errorExTypeLength", DefaultValue = 1000, IsRequired = true)]
		public int ErrorExTypeLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.errorExTypeLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.errorExTypeLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the exception message.
		/// </summary>
		[ConfigurationProperty("errorExMsgLength", DefaultValue = 4000, IsRequired = true)]
		public int ErrorExMsgLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.errorExMsgLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.errorExMsgLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the exception source.
		/// </summary>
		[ConfigurationProperty("errorExSourceLength", DefaultValue = 1000, IsRequired = true)]
		public int ErrorExSourceLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.errorExSourceLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.errorExSourceLength.ToString()] = value; }
		}

		/// <summary>
		///	The maximum allowed string length for the URL that caused the exception.
		/// </summary>
		[ConfigurationProperty("errorUrlLength", DefaultValue = 1000, IsRequired = true)]
		public int ErrorUrlLength
		{
			get { return Convert.ToInt32(this[CoreAttributes.errorUrlLength.ToString()], CultureInfo.InvariantCulture); }
			set { this[CoreAttributes.errorUrlLength.ToString()] = value; }
		}
	}

	/// <summary>
	/// Provides read/write access to the galleryServerPro/dataProvider section of galleryserverpro.config.
	/// </summary>
	public class DataProvider : ConfigurationElement
	{
		/// <summary>
		/// Gets a reference to the providers defined within the galleryServerPro/dataProvider section of galleryserverpro.config.
		/// </summary>
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base["providers"];
			}
		}

		/// <summary>
		/// Gets a reference to the default data provider defined in the galleryServerPro/gataProvider section of galleryserverpro.config.
		/// </summary>
		[ConfigurationProperty("defaultProvider", DefaultValue = "SQLiteGalleryServerProProvider", IsRequired = true)]
		[StringValidator(MinLength = 1)]
		public string DefaultProvider
		{
			get
			{
				return (string)base["defaultProvider"];
			}
			set
			{
				base["defaultProvider"] = value;
			}
		}
	}

	/// <summary>
	/// Provides read access to the galleryServerPro/galleryObject section of galleryserverpro.config.
	/// </summary>
	public class GalleryObject : ConfigurationElement
	{
		/// <summary>
		/// Gets a reference to the mediaObjects collection defined within the galleryServerPro/galleryObject section of galleryserverpro.config.
		/// </summary>
		[ConfigurationProperty("mediaObjects", IsDefaultCollection = false, IsRequired = true)]
		[ConfigurationCollection(typeof(MediaObjectCollection), AddItemName = "mediaObject")]
		public MediaObjectCollection MediaObjects
		{
			get
			{
				return (MediaObjectCollection)base["mediaObjects"];
			}
		}

		/// <summary>
		/// Gets a reference to the mimeTypes collection defined within the galleryServerPro/galleryObject section of galleryserverpro.config.
		/// </summary>
		[ConfigurationProperty("mimeTypes", IsDefaultCollection = false, IsRequired = true)]
		[ConfigurationCollection(typeof(MediaObjectCollection), AddItemName = "mimeType")]
		public MimeTypeCollection MimeTypes
		{
			get
			{
				return (MimeTypeCollection)base["mimeTypes"];
			}
		}
	}

	/// <summary>
	/// A reference to one of the mediaObject elements within the galleryServerPro/galleryObject/mediaObjects 
	/// section of galleryserverpro.config. This element provides information about how a particular type of media object
	/// (e.g. .wmv video, .mp3 audio, .jpg image, etc.) should be rendered to the browser.
	/// </summary>
	public class MediaObject : ConfigurationElement
	{
		/// <summary>
		/// A value indicating the mime type of the current mediaObject element. This property is the key for the
		/// MediaObjectCollection, which means this value must be unique among all mediaObject elements. 
		/// An asterisk (*) acts as a wildcard. For example, "video/*" means this element contains properties that
		/// affect all videos. However, if another mediaObject element contains a more specific value, such as
		/// "video/quicktime", those properties are preferred.
		/// </summary>
		[ConfigurationProperty("mimeType", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string MimeType
		{
			get { return this["mimeType"].ToString(); }
		}

		/// <summary>
		/// Gets a reference to the browsers defined within the galleryServerPro/galleryObject/mediaObjects/mediaObject 
		/// section of galleryserverpro.config.
		/// </summary>
		[ConfigurationProperty("browsers", IsDefaultCollection = false, IsRequired = false)]
		[ConfigurationCollection(typeof(BrowserCollection), AddItemName = "browser")]
		public BrowserCollection Browsers
		{
			get
			{
				return (BrowserCollection)base["browsers"];
			}
		}
	}

	/// <summary>
	/// Provides read access to the galleryServerPro/galleryObject/mediaObjects section of galleryserverpro.config. This section
	/// is a collection of mediaObject elements that provide information about how to render media objects to
	/// the browser.
	/// </summary>
	public class MediaObjectCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Creates a new ConfigurationElement that is of type GalleryServerPro.Configuration.MediaObject. This method 
		/// is automatically called by the .Net Framework for each mediaObject element when the collection is loaded 
		/// from the configuration file.
		/// </summary>
		/// <returns>A new ConfigurationElement that is of type MediaObject.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new MediaObject();
		}

		/// <summary>
		/// Gets the element key for a mediaObject element that uniquely identifies it. 
		/// </summary>
		/// <param name="element">The ConfigurationElement to return the key for. </param>
		/// <returns>An Object that acts as the key for the specified ConfigurationElement.</returns>
		/// <exception cref="System.ArgumentException">Thrown when the element parameter is not of type
		/// MediaObject.</exception>
		protected override object GetElementKey(ConfigurationElement element)
		{
			MediaObject mediaObject = element as MediaObject;
			if (mediaObject == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Global_GetElementKey_Ex_Msg, typeof(MediaObject).ToString(), element.GetType().ToString()));
			}

			return mediaObject.MimeType;
		}

		/// <summary>
		/// Gets the MediaObject element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the MediaObject to return.</param>
		/// <returns>The MediaObject at the specified index.</returns>
		public MediaObject this[int index]
		{
			get
			{
				return (MediaObject)this.BaseGet(index);
			}
		}

		/// <summary>
		/// Gets the MediaObject element with the specified key.
		/// </summary>
		/// <param name="name">The key of the element to return.</param>
		/// <returns>The MediaObject with the specified key; otherwise, a null reference.</returns>
		public new MediaObject this[string name]
		{
			get
			{
				return (MediaObject)this.BaseGet(name);
			}
		}

		/// <summary>
		/// Gets the MediaObject element with the specified mime type. The mime type is the key for the
		/// MediaObjectCollection collection. This method calls the indexer using
		/// the overload that specifies the mime type key name.
		/// </summary>
		/// <param name="mimeType">The mime type of the element to return (e.g. "video/quicktime").</param>
		/// <returns>The MediaObject with the specified mime type; otherwise, a null reference.</returns>
		public MediaObject FindByMimeType(string mimeType)
		{
			return (MediaObject)this.BaseGet(mimeType);
		}
	}

	/// <summary>
	/// A reference to one of the browser elements within the galleryServerPro/galleryObject/mediaObjects/mediaObject/browsers/
	/// section of galleryserverpro.config. This element provides information about how the media object should be rendered for
	/// the browser.
	/// </summary>
	public class Browser : ConfigurationElement
	{
		/// <summary>
		/// Gets the id of the current browser element. This value must match the internal identifier 
		/// of the browser as specified in the .Net Framework's browser definition file. This property is the key 
		/// for the <see cref="BrowserCollection" />, which means this value must be unique among all browser elements within the
		/// parent mediaObjects element.
		/// </summary>
		[ConfigurationProperty("id", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string Id
		{
			get { return this["id"].ToString(); }
		}

		/// <summary>
		/// Gets the HTML that can be sent to a browser to render the media object. This value can vary based on the
		/// browser id and the type of media object (determined by its mime type, which in turn is determined based
		/// on its file extension). Any text contained within brackets ({}) is meant to be updated with a new value
		/// before being sent to the browser. For example, {Width} should be replaced with an integer representing the 
		/// width, in pixels, of the current media object.
		/// </summary>
		[ConfigurationProperty("htmlOutput", DefaultValue = "", IsKey = false, IsRequired = true)]
		public string HtmlOutput
		{
			get { return this["htmlOutput"].ToString(); }
		}

		/// <summary>
		/// Gets the ECMA Script that can be sent to a browser to help render the media object.
		/// </summary>
		[ConfigurationProperty("scriptOutput", DefaultValue = "", IsKey = false, IsRequired = false)]
		public string ScriptOutput
		{
			get { return this["scriptOutput"].ToString(); }
		}
	}

	/// <summary>
	/// Provides read access to the galleryServerPro/galleryObject/mediaObjects/mediaObject/browsers section of 
	/// galleryserverpro.config. This section is a collection of browser elements that provide information about how to render 
	/// the current media objects to a web browser.
	/// </summary>
	public class BrowserCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Creates a new <see cref="ConfigurationElement" /> that is of type <see cref="Browser" />. This method is automatically called 
		/// by the .Net Framework for each browser element when the collection is loaded from the configuration file.
		/// </summary>
		/// <returns>A new <see cref="ConfigurationElement" /> that is of type <see cref="Browser" />.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new Browser();
		}

		/// <summary>
		/// Gets the element key for a browser element that uniquely identifies it. 
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement" /> to return the key for. </param>
		/// <returns>An Object that acts as the key for the specified <see cref="ConfigurationElement" />.</returns>
		/// <exception cref="System.ArgumentException">Thrown when the element parameter is not of type
		/// <see cref="Browser" />.</exception>
		protected override object GetElementKey(ConfigurationElement element)
		{
			Browser browser = element as Browser;
			if (browser == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Global_GetElementKey_Ex_Msg, typeof(Browser).ToString(), element.GetType().ToString()));
			}

			return browser.Id;
		}

		/// <summary>
		/// Gets the <see cref="Browser" /> element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the <see cref="Browser" /> to return.</param>
		/// <returns>The <see cref="Browser" /> at the specified index.</returns>
		public Browser this[int index]
		{
			get
			{
				return (Browser)this.BaseGet(index);
			}
		}

		/// <summary>
		/// Gets the <see cref="Browser" /> element with the specified key.
		/// </summary>
		/// <param name="name">The key of the element to return.</param>
		/// <returns>The <see cref="Browser" /> with the specified key; otherwise, a null reference.</returns>
		public new Browser this[string name]
		{
			get
			{
				return (Browser)this.BaseGet(name);
			}
		}

		/// <summary>
		/// Gets the <see cref="Browser" /> element with the specified browser id. The id is the key for the
		/// <see cref="BrowserCollection" /> collection. This method calls the indexer using
		/// the overload that specifies the browser id key name.
		/// </summary>
		/// <param name="browserId">The browser id of the element to return (e.g. default, ie, ie6to9, gecko, 
		/// mozillafirefox). This value must match the internal identifier of the browser as specified in the .Net 
		/// Framework's browser definition file. Must be lower-case.</param>
		/// <returns>The <see cref="Browser" /> with the specified browser id; otherwise, a null reference.</returns>
		public Browser FindById(string browserId)
		{
			return (Browser)this.BaseGet(browserId);
		}

		/// <summary>
		/// Gets the most specific <see cref="Browser" /> element that matches one of the browser ids. The browser id is the key for the
		/// <see cref="BrowserCollection" /> collection. This method loops through each of the browserId items in the browserIds 
		/// parameter, starting with the most specific item, and looks for a match in the configuration file. This 
		/// method is guaranteed to return a <see cref="Browser" /> object, provided the Browsers collection, at the very least, 
		/// contains a browser element with id = "default".
		/// </summary>
		/// <param name="browserIds">An <see cref="System.Array"/> of browser ids for the current browser. This is a list of strings,
		/// ordered from most general to most specific, that represent the various categories of browsers the current
		/// browser belongs to. This is typically populated by calling ToArray() on the Request.Browser.Browsers property.
		/// </param>
		/// <returns>The <see cref="Browser" /> that most specifically matches one of the browser ids; otherwise, a null reference.</returns>
		/// <example>During a request where the client is Firefox 2.0, the Request.Browser.Browsers property returns an
		/// ArrayList with these five items: default, mozilla, gecko, mozillarv, and mozillafirefox. This method
		/// starts with the most specific item (mozillafirefox) and looks in the configuration file for the browser 
		/// elements within the current mediaObject element for a match (&lt;browser id="default" ...&gt;). If a match
		/// is found, that browser element is returned. If no match is found, the next item (mozillarv) is used as the
		/// search parameter.  This continues until a match is found. Since there should always be a browser element 
		/// with id="default", there should always eventually be a match.</example>
		public Browser FindClosestMatchById(Array browserIds)
		{
			if (browserIds == null)
				throw new ArgumentNullException("browserIds");

			if (browserIds.Length == 0)
				throw new ArgumentOutOfRangeException("browserIds", "The Array parameter \"browserIds\" must have at least one item, but it was passed with 0 items.");

			Browser matchingBrowser = null;

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

			lock (browserIds)
			{
				foreach (string browserId in browserIds)
				{
					matchingBrowser = this.FindById(browserId);

					if (matchingBrowser != null)
						break;
				}
			}

			return matchingBrowser;
		}

	}

	/// <summary>
	/// Provides read access to the galleryServerPro/galleryObject/mimeTypes section of galleryserverpro.config. This section
	/// is a collection of mimeType elements that provide mapping information between file extensions and their
	/// associated mime type.
	/// </summary>
	public class MimeTypeCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Creates a new <see cref="ConfigurationElement" /> that is of type <see cref="GalleryServerPro.Configuration.MimeType" />. This method 
		/// is automatically called by the .Net Framework for each mimeType element when the collection is loaded 
		/// from the configuration file.
		/// </summary>
		/// <returns>A new <see cref="ConfigurationElement" /> that is of type <see cref="MimeType" />.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new MimeType();
		}

		/// <summary>
		/// Gets the element key for a mimeType element that uniquely identifies it. 
		/// </summary>
		/// <param name="element">The <see cref="ConfigurationElement" /> to return the key for. </param>
		/// <returns>An Object that acts as the key for the specified <see cref="ConfigurationElement" />.</returns>
		/// <exception cref="System.ArgumentException">Thrown when the element parameter is not of type
		/// <see cref="MimeType" />.</exception>
		protected override object GetElementKey(ConfigurationElement element)
		{
			MimeType mimeType = element as MimeType;

			if (mimeType == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Global_GetElementKey_Ex_Msg, typeof(MimeType).ToString(), element.GetType().ToString()));
			}

			return String.Concat(mimeType.FileExtension, "|", mimeType.BrowserId);
		}

		/// <summary>
		/// Adds the specified MIME type.
		/// </summary>
		/// <param name="mimeType">An instance of a <see cref="MimeType"/> to remove from the collection.</param>
		public void Add(MimeType mimeType)
		{
			BaseAdd(mimeType);
		}

		/// <summary>
		/// Removes the specified MIME type from the collection.
		/// </summary>
		/// <param name="mimeType">An instance of a <see cref="MimeType"/> to remove from the collection.</param>
		public void Remove(MimeType mimeType)
		{
			BaseRemove(mimeType);
		}

		/// <summary>
		/// Gets the <see cref="MimeType" /> element at the specified index location.
		/// </summary>
		/// <param name="index">The index location of the <see cref="MimeType" /> to return.</param>
		/// <returns>The <see cref="MimeType" /> at the specified index.</returns>
		public MimeType this[int index]
		{
			get
			{
				return (MimeType)this.BaseGet(index);
			}
		}

		/// <summary>
		/// Gets the <see cref="MimeType" /> element with the specified key.
		/// </summary>
		/// <param name="mimeTypeKey">The key of the element to return.</param>
		/// <returns>The <see cref="MimeType" /> with the specified key; otherwise, a null reference.</returns>
		public new MimeType this[string mimeTypeKey]
		{
			get
			{
				return (MimeType)this.BaseGet(mimeTypeKey);
			}
		}

		/// <summary>
		/// Gets the <see cref="MimeType" /> element with the specified file extension and corresponding to the default browser 
		/// (browserId = "default"). This method calls the indexer using the overload that specifies the key name.
		/// </summary>
		/// <param name="fileExtension">The file extension of the element to return (e.g. ".jpg", ".wmv").</param>
		/// <returns>The <see cref="MimeType" /> with the specified file extension; otherwise, a null reference.</returns>
		public MimeType FindByFileExtension(string fileExtension)
		{
			// The key is a pipe-concatenated string of the file extension and browser ID (ex: ".jpg|default" or ".wav|ie50").
			return (MimeType)this.BaseGet(String.Format(CultureInfo.CurrentCulture, "{0}|default", fileExtension));
		}

		/// <summary>
		/// Gets a list of all <see cref="MimeType" /> elements with the specified file extension.
		/// </summary>
		/// <param name="fileExtension">The file extension of the element(s) to return (e.g. ".jpg", ".wmv").</param>
		/// <returns>A list of all <see cref="MimeType" /> objects with the specified file extension; otherwise, a null reference.</returns>
		public System.Collections.Generic.List<MimeType> FindAllByFileExtension(string fileExtension)
		{
			System.Collections.Generic.List<MimeType> mimeTypes = new System.Collections.Generic.List<MimeType>(1);

			// The key is a pipe-concatenated string of the file extension and browser ID (ex: ".jpg|default" or
			// ".wav|ie50"). Loop through all items and extract the .
			for (int iterator = 0; iterator < this.Count; iterator++)
			{
				MimeType mimeType = this[iterator];
				if (mimeType.FileExtension == fileExtension)
					mimeTypes.Add(mimeType);
			}

			return mimeTypes;
		}
	}

	/// <summary>
	/// A reference to one of the mimeType elements within the galleryServerPro/galleryObject/mimeTypes 
	/// section of galleryserverpro.config. This element provides mapping information between a file extension and its
	/// associated mime type.
	/// </summary>
	public class MimeType : ConfigurationElement
	{
		/// <summary>
		/// Gets or sets a value indicating the file extension of the current mimeType element. This property is the key for the
		/// <see cref="MimeTypeCollection" />, which means this value must be unique among all mimeType elements. 
		/// </summary>
		[ConfigurationProperty("fileExtension", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string FileExtension
		{
			get { return this["fileExtension"].ToString(); }
			set { this["fileExtension"] = value; }
		}

		/// <summary>
		/// Gets or sets the id of the browser for which the <see cref="BrowserMimeType" /> property applies. This value must match the 
		/// internal identifier  of the browser as specified in the .Net Framework's browser definition file. 
		/// A value of "default" means it will match all browsers. The "default" value is required for each unique
		/// FileExtension. Other values that specify more specific browsers or browser families can be added. This 
		/// property is meant to be used with the BrowserMimeType property so that the correct MIME type can be 
		/// sent to different browsers.
		/// </summary>
		[ConfigurationProperty("browserId", DefaultValue = "default", IsKey = true, IsRequired = true)]
		public string BrowserId
		{
			get { return this["browserId"].ToString(); }
			set { this["browserId"] = value; }
		}

		/// <summary>
		/// Gets or sets the MIME type that can be understood by the browser for displaying this media object. This
		/// attribute can be used when the MIME type specified in the type attribute is not recognized by the
		/// browser.
		/// </summary>
		/// <example>The MIME type for a wave file is "audio/wav". Since some browsers do not recognize
		/// "&lt;object type='audio/wav' ...", we can specify browserMimeType = "application/x-mplayer2".
		/// This will cause the resulting HTML to be: "&lt;object type='application/x-mplayer2' ...", which is
		/// a value the browser understands.</example>
		[ConfigurationProperty("browserMimeType", DefaultValue = "", IsKey = false, IsRequired = false)]
		public string BrowserMimeType
		{
			get { return this["browserMimeType"].ToString(); }
			set { this["browserMimeType"] = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating the full mime type string for the current mimeType element (e.g. "image/jpg", "video/quicktime").
		/// </summary>
		[ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
		public string Type
		{
			get { return this["type"].ToString(); }
			set { this["type"] = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this MIME type can be added to Gallery Server Pro.
		/// </summary>
		[ConfigurationProperty("allowAddToGallery", DefaultValue = false, IsKey = false, IsRequired = true)]
		public bool AllowAddToGallery
		{
			get { return Convert.ToBoolean(this["allowAddToGallery"], CultureInfo.InvariantCulture); }
			set { this["allowAddToGallery"] = value; }
		}
	}

}

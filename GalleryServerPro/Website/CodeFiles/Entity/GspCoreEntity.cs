using System;

namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains all the configuration settings in the &lt;core .../&gt; section of galleryserverpro.config.
	/// This entity is designed to be an updateable object whose properties can be changed and passed to the 
	/// <see cref="GalleryServerPro.Web.Controller.GspConfigController"/> for persisting back to the configuration file on disk.
	/// Therefore, this entity is typically used only in scenarios where we must persist changes to the config file, such as 
	/// in the Site Admin area. If the custom configuration section <see cref="GalleryServerPro.Configuration.Core"/> were 
	/// updateable, this entity would have been unnecessary.
	/// </summary>
	public class GspCoreEntity
	{
		public int galleryId;
		public string mediaObjectPath;
		public bool mediaObjectPathIsReadOnly;
		public string pageHeaderText;
		public string pageHeaderTextUrl;
		public bool showLogin;
		public bool showSearch;
		public bool showErrorDetails;
		public bool enableExceptionHandler;

		public int defaultAlbumDirectoryNameLength;
		public bool synchAlbumTitleAndDirectoryName;
		public string emptyAlbumThumbnailBackgroundColor;
		public string emptyAlbumThumbnailText;
		public string emptyAlbumThumbnailFontName;
		public int emptyAlbumThumbnailFontSize;
		public string emptyAlbumThumbnailFontColor;
		public Single emptyAlbumThumbnailWidthToHeightRatio;

		public int maxAlbumThumbnailTitleDisplayLength;
		public int maxMediaObjectThumbnailTitleDisplayLength;
		public bool allowHtmlInTitlesAndCaptions;
		public bool allowUserEnteredJavascript;
		public string allowedHtmlTags;
		public string allowedHtmlAttributes;
		public bool allowCopyingReadOnlyObjects;
		public bool allowManageOwnAccount;
		public bool allowDeleteOwnAccount;
		public string mediaObjectTransitionType;
		public Single mediaObjectTransitionDuration;
		public int slideshowInterval;

		public int mediaObjectDownloadBufferSize;
		public bool encryptMediaObjectUrlOnClient;
		public string encryptionKey;
		public bool allowUnspecifiedMimeTypes;
		public string imageTypesStandardBrowsersCanDisplay;
		public string silverlightFileTypes;

		public bool allowAnonymousHiResViewing;
		public bool enableImageMetadata;
		public bool enableWpfMetadataExtraction;
		public bool enableMediaObjectDownload;
		public bool enableMediaObjectZipDownload;
		public bool enablePermalink;
		public bool enableSlideShow;
		public int maxThumbnailLength;
		public int thumbnailImageJpegQuality;
		public bool thumbnailClickShowsOriginal;
		public int thumbnailWidthBuffer;
		public int thumbnailHeightBuffer;
		public string thumbnailFileNamePrefix;
		public string thumbnailPath;

		public int maxOptimizedLength;
		public int optimizedImageJpegQuality;
		public int optimizedImageTriggerSizeKB;
		public string optimizedFileNamePrefix;
		public string optimizedPath;

		public int originalImageJpegQuality;
		public bool discardOriginalImageDuringImport;

		public bool applyWatermark;
		public bool applyWatermarkToThumbnails;
		public string watermarkText;
		public string watermarkTextFontName;
		public int watermarkTextFontSize;
		public int watermarkTextWidthPercent;
		public string watermarkTextColor;
		public int watermarkTextOpacityPercent;
		public string watermarkTextLocation;
		public string watermarkImagePath;
		public int watermarkImageWidthPercent;
		public int watermarkImageOpacityPercent;
		public string watermarkImageLocation;

		public bool sendEmailOnError;
		public string emailFromName;
		public string emailFromAddress;
		public string emailToName;
		public string emailToAddress;
		public string smtpServer;
		public string smtpServerPort;
		public bool sendEmailUsingSsl;

		public bool autoStartMediaObject;
		public int defaultVideoPlayerWidth;
		public int defaultVideoPlayerHeight;

		public int defaultAudioPlayerWidth;
		public int defaultAudioPlayerHeight;

		public int defaultGenericObjectWidth;
		public int defaultGenericObjectHeight;

		public int maxUploadSize;
		public bool allowAddLocalContent;
		public bool allowAddExternalContent;
		public bool allowAnonymousBrowsing;
		public int pageSize;
		public string pagerLocation;
		public int maxNumberErrorItems;
		public bool enableSelfRegistration;
		public bool requireEmailValidationForSelfRegisteredUser;
		public bool requireApprovalForSelfRegisteredUser;
		public bool useEmailForAccountName;
		public string usersToNotifyWhenAccountIsCreated;
		public string defaultRolesForSelfRegisteredUser;
		public bool enableUserAlbum;
		public int userAlbumParentAlbumId;
		public string userAlbumNameTemplate;
		public string userAlbumSummaryTemplate;
		public bool redirectToUserAlbumAfterLogin;
		public string jQueryScriptPath;

		public string membershipProviderName;
		public string roleProviderName;
		//public string profileProviderName;
		
		public string productKey;
	}
}

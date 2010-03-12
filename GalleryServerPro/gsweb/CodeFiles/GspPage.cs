using System;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web
{
	/// <summary>
	/// Base page for web pages in Gallery Server Pro.
	/// </summary>
	[DataObject]
	public class GspPage : System.Web.UI.Page
	{
		#region Private Fields

		private readonly WebsiteController _webController;
		private Message _message;
		private IAlbum _album;
		private IGalleryServerRoleCollection _roles;
		private string _themePath = String.Empty;
		private bool? _isAnonymousUser;
		private bool? _userCanCreateAlbum;
		private bool? _userCanEditAlbum;
		private bool? _userCanAddMediaObject;
		private bool? _userCanEditMediaObject;
		private bool? _userCanAddAdministerSite;
		private bool? _userCanDeleteCurrentAlbum;
		private bool? _userCanDeleteMediaObject;
		private bool? _userCanSynchronize;
		private bool? _userCanViewHiResImage;
		private bool? _userCanAddMediaObjectToAtLeastOneAlbum;
		private bool? _userCanAddAlbumToAtLeastOneAlbum;

		#endregion

		#region Private Static Fields

		private static readonly System.Collections.Generic.Dictionary<int, string> _encryptedAlbumIds = new System.Collections.Generic.Dictionary<int, string>();

		#endregion

		#region Constructors

		static GspPage()
		{
			if (!AppSetting.Instance.IsInitialized)
			{
				WebsiteController.InitializeApplication();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GspPage"/> class.
		/// </summary>
		public GspPage()
		{
			this._webController = new WebsiteController();
			this._message = Message.None;
		}

		#endregion

		#region Protected Events

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			if ((!IsPostBack) && (!IsCallback))
			{
				StoreReferringPage();
			}

			base.OnInit(e);
		}

		//protected virtual void Page_Init(object sender, EventArgs e)
		//{
		//  System.Collections.Generic.Dictionary<int, IAlbum> albumCache = (System.Collections.Generic.Dictionary<int, IAlbum>)HelperFunctions.CacheManager.GetData(CacheItem.Albums.ToString());

		//  if (!IsPostBack)
		//  {
		//    StoreReferringPage();
		//  }

		//  //RegisterHiddenFields();
		//}

		//private void RegisterHiddenFields()
		//{
		//  ScriptManager.RegisterHiddenField(this, "moid", this.MediaObjectId.ToString());
		//  ScriptManager.RegisterHiddenField(this, "aid", this.AlbumId.ToString());
		//}

		//protected override void OnLoad(EventArgs e)
		//{
		//  base.OnLoad(e);

		//}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the album ID corresponding to the current album. Returns int.MinValue if ... Guaranteed to return a valid album ID. Returns the album ID
		/// of the root album if no valid media object or album is specified in the query string.
		/// </summary>
		public int AlbumId
		{
			get
			{
				return this.GetAlbum().Id;
			}
		}

		/// <summary>
		/// Gets or sets the media object ID. Attempts to retrieve value from ViewState object "moid". If viewstate object does not
		/// exist, the value is retrieved from <see cref="WebsiteController.MediaObjectId" />, which attempts to retrieve it from a query string 
		/// parameter "moid". Returns int.MinValue if no valid media object ID is found. Setting this value stores it in a ViewState
		/// object named "moid" and assigns it to <see cref="WebsiteController.MediaObjectId" />, which causes the WebSiteController's private media 
		/// object and album variables to be nulled out, allowing them to be regenerated using the new ID when calling <see cref="GetAlbum" /> or 
		/// <see cref="GetMediaObject" />.
		/// </summary>
		public int MediaObjectId
		{
			get
			{
				int moid;
				object viewstateMoid = ViewState["moid"];
				if ((viewstateMoid != null) && (Int32.TryParse(ViewState["moid"].ToString(), out moid)))
				{
					if (moid != this._webController.MediaObjectId)
					{
						this._webController.MediaObjectId = moid;
					}
					return moid;
				}
				else
				{
					return this._webController.MediaObjectId;
				}
			}
			set
			{
				ViewState["moid"] = value;
				this._webController.MediaObjectId = value;
			}
		}

		/// <summary>
		/// Get the virtual application path to the current theme's folder. Does not include the trailing slash. Ex: /galleryserverpro/App_Themes/HelixBlue
		/// </summary>
		public string ThemePath
		{
			get
			{
				if (String.IsNullOrEmpty(this._themePath))
					this._themePath = WebsiteController.GetThemePathUrl(this.Theme);

				return this._themePath;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current user is anonymous. If the user has authenticated with a user name/password, 
		/// this property is false.
		/// </summary>
		public bool IsAnonymousUser
		{
			get
			{
				if (!this._isAnonymousUser.HasValue)
				{
					this._isAnonymousUser = !HttpContext.Current.User.Identity.IsAuthenticated;
				}
				return this._isAnonymousUser.Value;
			}
		}

		/// <summary>
		/// Gets or sets the message used to disply user messages, such as "Invalid login". The value is retrieved from the
		/// "msgId" query string parameter or from a private field if it was explicitly assigned earlier in the current page's
		/// life cycle. Returns int.MinValue if the parameter is not found, it is not a valid integer, or it is &lt;= 0.
		/// Setting this property sets a private field that lives as long as the current page lifecycle. It is not persisted across
		/// postbacks or added to the querystring. Set the value only when you will use it later in the current page's lifecycle.
		/// </summary>
		public Message Message
		{
			get
			{
				if (this._message == Message.None)
				{
					int msgId = WebsiteController.GetQueryStringParameterInt32("msg");
					if (msgId > int.MinValue)
					{
						this._message = (Message)Enum.Parse(typeof(Message), msgId.ToString(CultureInfo.InvariantCulture));
					}
				}
				return this._message;
			}
			set
			{
				this._message = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to create a new album within the current album.
		/// </summary>
		public bool UserCanCreateAlbum
		{
			get
			{
				if (!this._userCanCreateAlbum.HasValue)
					EvaluateUserPermissions();

				return this._userCanCreateAlbum.Value;
			}
			set { this._userCanCreateAlbum = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to edit information about the current album.
		/// This includes changing the album's title, description, start and end dates, assigning the album's thumbnail image,
		/// and rearranging the order of objects within the album. Warning: This property may not be accurate for anonymous
		/// users when accessed from a page outside the directory containing the functionality. For example, if the root page
		/// default.aspx allows anonymous access, and you query this property from the code-behind on that page, this property 
		/// will always return true, since the logic depends on using forms authentication to restrict access to anonymous users.
		/// </summary>
		public bool UserCanEditAlbum
		{
			get
			{
				if (!this._userCanEditAlbum.HasValue)
					EvaluateUserPermissions();

				return this._userCanEditAlbum.Value;
			}
			set { this._userCanEditAlbum = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to add media objects to the current album.
		/// </summary>
		public bool UserCanAddMediaObject
		{
			get
			{
				if (!this._userCanAddMediaObject.HasValue)
					EvaluateUserPermissions();

				return this._userCanAddMediaObject.Value;
			}
			set { this._userCanAddMediaObject = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to edit the current media object. This includes 
		/// changing the media object's caption, rotating the object (if it is an image), and deleting the high resolution
		/// version of the object (applies only if it is an image).
		/// </summary>
		public bool UserCanEditMediaObject
		{
			get
			{
				if (!this._userCanEditMediaObject.HasValue)
					EvaluateUserPermissions();

				return this._userCanEditMediaObject.Value;
			}
			set { this._userCanEditMediaObject = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to administer the site. If true, the user
		/// has all possible permissions and there is nothing he or she can't do.
		/// </summary>
		public bool UserCanAdministerSite
		{
			get
			{
				if (!this._userCanAddAdministerSite.HasValue)
					EvaluateUserPermissions();

				return this._userCanAddAdministerSite.Value;
			}
			set { this._userCanAddAdministerSite = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to delete the current album.
		/// </summary>
		public bool UserCanDeleteCurrentAlbum
		{
			get
			{
				if (!this._userCanDeleteCurrentAlbum.HasValue)
					EvaluateUserPermissions();

				return this._userCanDeleteCurrentAlbum.Value;
			}
			set { this._userCanDeleteCurrentAlbum = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to delete the current media object.
		/// </summary>
		public bool UserCanDeleteMediaObject
		{
			get
			{
				if (!this._userCanDeleteMediaObject.HasValue)
					EvaluateUserPermissions();

				return this._userCanDeleteMediaObject.Value;
			}
			set { this._userCanDeleteMediaObject = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to synchronize the current album.
		/// </summary>
		public bool UserCanSynchronize
		{
			get
			{
				if (!this._userCanSynchronize.HasValue)
					EvaluateUserPermissions();

				return this._userCanSynchronize.Value;
			}
			set { this._userCanSynchronize = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user has permission to view the original high resolution version of an image.
		/// </summary>
		public bool UserCanViewHiResImage
		{
			get
			{
				if (!this._userCanViewHiResImage.HasValue)
					EvaluateUserPermissions();

				return this._userCanViewHiResImage.Value;
			}
			set { this._userCanViewHiResImage = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current user has permission to add media objects to at least one album.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if current user has permission to add media objects to at least one album; otherwise, <c>false</c>.
		/// </value>
		public bool UserCanAddMediaObjectToAtLeastOneAlbum
		{
			get
			{
				if (!this._userCanAddMediaObjectToAtLeastOneAlbum.HasValue)
					EvaluateUserPermissions();

				return this._userCanAddMediaObjectToAtLeastOneAlbum.Value;
			}
			set { this._userCanAddMediaObjectToAtLeastOneAlbum = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current user has permission to add albums to at least one album.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the current user has permission to add albums to at least one album; otherwise, <c>false</c>.
		/// </value>
		public bool UserCanAddAlbumToAtLeastOneAlbum
		{
			get
			{
				if (!this._userCanAddAlbumToAtLeastOneAlbum.HasValue)
					EvaluateUserPermissions();

				return this._userCanAddAlbumToAtLeastOneAlbum.Value;
			}
			set { this._userCanAddAlbumToAtLeastOneAlbum = value; }
		}

		/// <summary>
		/// Returns a reference to the WebsiteController class that contains general-purpose web site functionality.
		/// </summary>
		public GalleryServerPro.Web.WebsiteController WebController
		{
			get { return this._webController; }
		}

		#endregion

		#region Public Instance Methods

		/// <summary>
		/// Create a fully inflated album instance for the album containing the media object ID or album ID specified 
		/// in the query (moid or aid). An automatic security check is performed to make sure the user has view
		/// permission for the specified album. If no valid album exists, or if it does exist but the user is not 
		/// authorized to view it, the user is redirected to the home page. Album properties 
		/// are retrieved from the data store. If this album contains child objects, they are added but not inflated.
		/// </summary>
		/// <returns>Returns an IAlbum object.</returns>
		public IAlbum GetAlbum()
		{
			if (this._album == null)
			{
				// Step 1: Retrieve the album based on the 'aid' or 'moid' query string parameter.
				IAlbum tempAlbum = null;
				try
				{
					tempAlbum = this._webController.GetAlbum();
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidAlbumException)
				{
					// The 'aid' query string parm refers to an album that doesn't exist. Redirect to home page and pass along
					// a message to inform the user.
					string redirectUrl = WebsiteController.AddQueryStringParameter(WebsiteController.GetAppRootUrl(), "msg=" + (int)Message.AlbumDoesNotExist);
					System.Web.HttpContext.Current.Response.Redirect(redirectUrl, true); // Use 'true' to force immediate end to page processing
				}

				// Step 2: If we got an album from step 1, make sure the user has viewing permission.
				if (tempAlbum != null)
				{
					if (IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, tempAlbum.Id, tempAlbum.IsPrivate))
					{
						// User is authorized. Assign to page-level variable.
						this._album = tempAlbum;
					}
					else
					{
						// User does not have permission to view the album. Redirect to home page and pass along a message to inform the user.
						string redirectUrl = WebsiteController.AddQueryStringParameter(WebsiteController.GetAppRootUrl(), "msg=" + (int)Message.InsufficientPermissionCannotViewAlbum);
						System.Web.HttpContext.Current.Response.Redirect(redirectUrl, true);  // Use 'true' to force immediate end to page processing
					}
				}

				// Step 3: If we got to this point, no album or media object is specified on the query string. Load the top
				// level albums for which the logged on user has permission to view.
				if (this._album == null)
				{
					if (this.IsAnonymousUser)
					{
						// Anonymous user, not logged on. Show root album as long as it is public.
						tempAlbum = Factory.LoadRootAlbumInstance();
						if (!tempAlbum.IsPrivate)
						{
							this._album = tempAlbum;
						}
						else
						{
							// The user is not logged on and the root album is private, so show an empty album and display a message.
							this._album = Factory.CreateAlbumInstance();
							this._album.IsVirtualAlbum = true;
							this._album.Title = Resources.GalleryServerPro.Site_Virtual_Album_Title;

							this.Message = Message.NoAuthorizedAlbumForUser;
						}
					}
					else
					{
						this._album = Factory.LoadViewableRootAlbumByRoles(this.GetGalleryServerRolesForUser());

						if ((this._album.IsVirtualAlbum) && (this._album.GetChildGalleryObjects().Count == 0))
						{
							this.Message = Message.NoAuthorizedAlbumForUser;
						}
					}
				}
			}

			return this._album;
		}

		/// <summary>
		/// Create a fully inflated, properly typed media object instance based on the media object ID in the query 
		/// string. Object properties are retrieved from the data store. If a valid media object is not found, the 
		/// application is redirected to the home page and the root album is displayed.
		/// </summary>
		/// <returns>Returns a GalleryObject object that represents the relevant derived media object type
		/// (e.g. Image, Video, etc).</returns>
		public IGalleryObject GetMediaObject()
		{
			int moid;
			object viewstateMoid = ViewState["moid"];
			if ((viewstateMoid != null) && (Int32.TryParse(ViewState["moid"].ToString(), out moid)))
			{
				if (moid != this._webController.MediaObjectId)
				{
					this._webController.MediaObjectId = moid;
				}
			}

			return this._webController.GetMediaObject();
		}

		/// <summary>
		/// Get the URL to the thumbnail image of the specified gallery object. Either a media object or album may be specified. Example:
		/// /dev/gs/handler/getmediaobject.ashx?moid=34&amp;aid=8&amp;mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&amp;mtc=1&amp;dt=1&amp;isp=false
		/// The URL can be used to assign to the src attribute of an image tag (&lt;img src='...' /&gt;).
		/// </summary>
		/// <param name="galleryObject">The gallery object for which an URL to its thumbnail image is to be generated.
		/// Either a media object or album may be specified.</param>
		/// <returns>Returns the URL to the thumbnail image of the specified gallery object.</returns>
		public string GetThumbnailUrl(IGalleryObject galleryObject)
		{
			if (galleryObject is Album)
				return GetAlbumThumbnailUrl(galleryObject);
			else
				return GetMediaObjectUrl(galleryObject, DisplayObjectType.Thumbnail);
		}

		/// <summary>
		/// Get the URL to the optimized image of the specified gallery object. Example:
		/// /dev/gs/handler/getmediaobject.ashx?moid=34&amp;aid=8&amp;mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&amp;mtc=1&amp;dt=1&amp;isp=false
		/// The URL can be used to assign to the src attribute of an image tag (&lt;img src='...' /&gt;).
		/// </summary>
		/// <param name="galleryObject">The gallery object for which an URL to its optimized image is to be generated.</param>
		/// <returns>Returns the URL to the optimized image of the specified gallery object.</returns>
		public static string GetOptimizedUrl(IGalleryObject galleryObject)
		{
			return GetMediaObjectUrl(galleryObject, DisplayObjectType.Optimized);
		}

		/// <summary>
		/// Get the URL to the original image of the specified gallery object. Example:
		/// /dev/gs/handler/getmediaobject.ashx?moid=34&amp;aid=8&amp;mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&amp;mtc=1&amp;dt=1&amp;isp=false
		/// The URL can be used to assign to the src attribute of an image tag (&lt;img src='...' /&gt;).
		/// </summary>
		/// <param name="galleryObject">The gallery object for which an URL to its original image is to be generated.</param>
		/// <returns>Returns the URL to the original image of the specified gallery object.</returns>
		public static string GetOriginalUrl(IGalleryObject galleryObject)
		{
			return GetMediaObjectUrl(galleryObject, DisplayObjectType.Original);
		}

		/// <summary>
		/// Get the URL to the thumbnail, optimized, or original media object. Example:
		/// /dev/gs/handler/getmediaobject.ashx?moid=34&amp;aid=8&amp;mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&amp;mtc=1&amp;dt=1&amp;isp=false
		/// The URL can be used to assign to the src attribute of an image tag (&lt;img src='...' /&gt;).
		/// Not tested: It should be possible to pass an album and request the url to its thumbnail image.
		/// </summary>
		/// <param name="galleryObject">The gallery object for which an URL to the specified image is to be generated.</param>
		/// <param name="displayType">A DisplayObjectType enumeration value indicating the version of the
		/// object for which the URL should be generated. Possible values: Thumbnail, Optimized, Original.
		/// An exception is thrown if any other enumeration is passed.</param>
		/// <returns>Returns the URL to the thumbnail, optimized, or original version of the requested media object.</returns>
		public static string GetMediaObjectUrl(IGalleryObject galleryObject, DisplayObjectType displayType)
		{
			string filenamePhysicalPath = null;

			switch (displayType)
			{
				case DisplayObjectType.Thumbnail: filenamePhysicalPath = galleryObject.Thumbnail.FileNamePhysicalPath; break;
				case DisplayObjectType.Optimized: filenamePhysicalPath = galleryObject.Optimized.FileNamePhysicalPath; break;
				case DisplayObjectType.Original: filenamePhysicalPath = galleryObject.Original.FileNamePhysicalPath; break;
				default: throw new ArgumentOutOfRangeException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.GspPage_GetMediaObjectUrl_Ex_Msg1, displayType.ToString()));
			}
			return MediaObjectHtmlBuilder.GenerateUrl(galleryObject.Id, galleryObject.Parent.Id, galleryObject.MimeType, filenamePhysicalPath, displayType, galleryObject.IsPrivate);
		}

		/// <summary>
		/// Remove all HTML tags from the specified string and HTML-encodes the result.
		/// </summary>
		/// <param name="textWithHtml">The string containing HTML tags to remove.</param>
		/// <returns>Returns a string with all HTML tags removed, including the brackets.</returns>
		/// <returns>Returns an HTML-encoded string with all HTML tags removed.</returns>
		public string RemoveHtmlTags(string textWithHtml)
		{
			// Return the text with all HTML removed.
			return Server.HtmlEncode(WebsiteController.RemoveHtmlTags(textWithHtml));
		}

		/// <summary>
		/// Check to ensure user has permission to perform the specified security action. Throw a 
		/// GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException if the 
		/// permission isn't granted to the logged on user. Un-authenticated users (anonymous users) are always considered 
		/// NOT authorized (that is, this method returns false) except when the requested security action is ViewAlbumOrMediaObject 
		/// or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access but it does 
		/// not allow anonymous editing of any kind. This method behaves similarly to IsUserAuthorized() except that it throws an
		/// exception instead of returning false when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException">Thrown when the logged on user 
		/// does not belong to a role that authorizes the specified security action, or if an anonymous user is requesting any permission 
		/// other than a viewing-related permission (i.e., SecurityActions.ViewAlbumOrMediaObject or SecurityActions.ViewOriginalImage).</exception>
		public void CheckUserSecurity(SecurityActions securityActions)
		{
			if (!WebsiteController.IsUserAuthorized(securityActions, GetGalleryServerRolesForUser(), this.AlbumId, this.GetAlbum().IsPrivate))
			{
				if (this.IsAnonymousUser)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "Anonymous user does not have permission '{0}' for album ID {1}.", securityActions.ToString(), this.AlbumId));
				}
				else
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "User '{0}' does not have permission '{1}' for album ID {2}.", HttpContext.Current.User.Identity.Name, securityActions.ToString(), this.AlbumId));
				}
			}
		}

		/// <summary>
		/// Check to ensure user has permission to perform the specified security action. Throw a 
		/// GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException if the 
		/// permission isn't granted to the logged on user. Un-authenticated users (anonymous users) are always considered 
		/// NOT authorized (that is, this method returns false) except when the requested security action is ViewAlbumOrMediaObject 
		/// or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access but it does 
		/// not allow anonymous editing of any kind. This method behaves similarly to IsUserAuthorized() except that it throws an
		/// exception instead of returning false when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <param name="album">The album for which the security check is to be applied.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException">Thrown when the logged on user 
		/// does not belong to a role that authorizes the specified security action, or if an anonymous user is requesting any permission 
		/// other than a viewing-related permission (i.e., SecurityActions.ViewAlbumOrMediaObject or SecurityActions.ViewOriginalImage).</exception>
		public void CheckUserSecurity(SecurityActions securityActions, IAlbum album)
		{
			if (!WebsiteController.IsUserAuthorized(securityActions, GetGalleryServerRolesForUser(), album.Id, album.IsPrivate))
			{
				if (this.IsAnonymousUser)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "Anonymous user does not have permission '{0}' for album ID {1}.", securityActions.ToString(), album.Id));
				}
				else
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "User '{0}' does not have permission '{1}' for album ID {2}.", HttpContext.Current.User.Identity.Name, securityActions.ToString(), album.Id));
				}
			}
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action against the specified album. If no album 
		/// is specified, then the current album (as returned by GetAlbum()) is used. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityAction">The security action represents the action being carried out by the web page.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action; otherwise returns false.</returns>
		public bool IsUserAuthorized(SecurityActions securityAction)
		{
			return WebsiteController.IsUserAuthorized(securityAction, GetGalleryServerRolesForUser(), this.AlbumId, this.GetAlbum().IsPrivate);
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action against the specified album. If no album 
		/// is specified, then the current album (as returned by GetAlbum()) is used. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityAction">The security action represents the action being carried out by the web page.</param>
		/// <param name="album">The album for which the security check is to be applied.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action; otherwise returns false.</returns>
		public bool IsUserAuthorized(SecurityActions securityAction, IAlbum album)
		{
			return WebsiteController.IsUserAuthorized(securityAction, GetGalleryServerRolesForUser(), album.Id, album.IsPrivate);
		}

		/// <summary>
		/// Gets Gallery Server roles representing the roles for the currently logged-on user. Returns an 
		/// empty collection if no user is logged in or the user is logged in but not assigned to any roles (Count = 0).
		/// </summary>
		/// <returns>Returns a collection of Gallery Server roles representing the roles for the currently logged-on user. 
		/// Returns an empty collection if no user is logged in or the user is logged in but not assigned to any roles (Count = 0).</returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public IGalleryServerRoleCollection GetGalleryServerRolesForUser()
		{
			if (this._roles == null)
			{
				this._roles = WebsiteController.GetRolesForUser();
			}

			return this._roles;
		}

		/// <summary>
		/// Retrieve a collection of Gallery Server roles that match the specified parameters. If no parameters are specified, then
		/// all roles for the current gallery are returned.
		/// </summary>
		/// <returns>Returns the Gallery Server roles that match the specified parameters.</returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryServerRoleCollection GetGalleryServerRoles()
		{
			return Factory.LoadGalleryServerRoles();
		}

		/// <summary>
		/// Redirect the user to the previous page he or she was on. If a query string name/pair value is specified, append that 
		/// to the URL. The previous page is retrieved from a session variable that was stored during 
		/// the Page_Init event. If the original query string contains a "msg" parameter, it is removed so that the message 
		/// is not shown again to the user. If no previous page URL is available - perhaps because the user navigated directly to
		/// the page or has just logged in - the user is redirected to the application root.
		/// </summary>
		public void RedirectToReferringPage()
		{
			RedirectToReferringPage(String.Empty, String.Empty);
		}

		/// <summary>
		/// Redirect the user to the previous page he or she was on. If a query string name/value pair value is specified, append that 
		/// to the URL. The previous page is retrieved from a session variable that was stored during 
		/// the Page_Init event. If the original query string contains a "msg" parameter, it is removed so that the message 
		/// is not shown again to the user. If no previous page URL is available - perhaps because the user navigated directly to
		/// the page or has just logged in - the user is redirected to the application root.
		/// </summary>
		/// <param name="queryStringName">A query string name to append to the URL. Ex: to append "&msg=17", specify "msg" for
		/// the queryStringName parameter and "17" for the queryStringValue parameter. This parameter requires the queryStringValue
		/// parameter to also be specified.</param>
		/// <param name="queryStringValue">A query string value to append to the URL. Ex: to append "&msg=17", specify "msg" for
		/// the queryStringName parameter and "17" for the queryStringValue parameter. This parameter requires the queryStringName
		/// parameter to also be specified.</param>
		public void RedirectToReferringPage(string queryStringName, string queryStringValue)
		{
			#region Validation

			if (!String.IsNullOrEmpty(queryStringName) && String.IsNullOrEmpty(queryStringValue))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringValue parameter is required when the queryStringName parameter is specified. (queryStringName='{0}', queryStringValue='{1}')", queryStringName, queryStringValue));

			if (!String.IsNullOrEmpty(queryStringValue) && String.IsNullOrEmpty(queryStringName))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringName parameter is required when the queryStringValue parameter is specified. (queryStringName='{0}', queryStringValue='{1}')", queryStringName, queryStringValue));

			#endregion

			string url = GetReferringPageUrl(queryStringName, queryStringValue);

			Page.Response.Redirect(url, true);
			//Page.Response.Redirect(url, false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		/// <summary>
		/// Get the URL to the page the user was on before navigating to the current page. If a query string name/value pair is
		/// specified, append that to the referring URL. This can be useful for specifying a message to be displayed.
		/// If the original query string contains a "msg" parameter, it is removed so that the message 
		/// is not shown again to the user. If no previous page URL is available - perhaps because the user navigated directly to
		/// the page or has just logged in - the user is redirected to the application root.
		/// </summary>
		/// <returns>Returns the URL to the page the user was on before navigating to the current page, with a query string name/value
		/// pair optionally appended.</returns>
		public string GetReferringPageUrl()
		{
			return GetReferringPageUrl(String.Empty, String.Empty);
		}

		/// <summary>
		/// Get the URL to the page the user was on before navigating to the current page. If a query string name/value pair is
		/// specified, append that to the referring URL. This can be useful for specifying a message to be displayed.
		/// If the original query string contains a "msg" parameter, it is removed so that the message 
		/// is not shown again to the user. If no previous page URL is available - perhaps because the user navigated directly to
		/// the page or has just logged in - the user is redirected to the application root.
		/// </summary>
		/// <param name="queryStringName">A query string name to append to the URL. Ex: to append "&msg=17", specify "msg" for
		/// the queryStringName parameter and "17" for the queryStringValue parameter. This parameter requires the queryStringValue
		/// parameter to also be specified.</param>
		/// <param name="queryStringValue">A query string value to append to the URL. Ex: to append "&msg=17", specify "msg" for
		/// the queryStringName parameter and "17" for the queryStringValue parameter. This parameter requires the queryStringName
		/// parameter to also be specified.</param>
		/// <returns>Returns the URL to the page the user was on before navigating to the current page, with a query string name/value
		/// pair optionally appended.</returns>
		public string GetReferringPageUrl(string queryStringName, string queryStringValue)
		{
			string url;
			Uri backURL;

			// If we have a url and it's not the login page, remove any message
			// query string parms and set it as the url we want to navigate to. We exclude the login 
			// page  because the  user is likely logged in at this point so there is no sense in 
			// going to it.
			if ((HttpContext.Current.Session != null) && ((backURL = (Uri)HttpContext.Current.Session["ReferringUrl"]) != null) && (backURL.AbsolutePath.IndexOf("login.aspx", StringComparison.OrdinalIgnoreCase) < 0))
			{
				url = backURL.PathAndQuery;
				if (backURL.Query.Length > 0)
				{
					// Strip out the message ID if one is contained in the URL, since we don't want to 
					// show the original message again to the user.
					url = WebsiteController.RemoveQueryStringParameter(url, "msg");
				}
				HttpContext.Current.Session["ReferringUrl"] = null;
			}
			else
			{
				// No previous url is available. Default to the application root.
				url = WebsiteController.GetAppRootUrl();
			}

			if (!String.IsNullOrEmpty(queryStringName))
				url = WebsiteController.AddQueryStringParameter(url, String.Format(CultureInfo.CurrentCulture, "{0}={1}", queryStringName, queryStringValue));
			return url;
		}

		/// <overloads>Redirects to album view page.</overloads>
		/// <summary>
		/// Redirects to album view page.
		/// </summary>
		public void RedirectToAlbumViewPage()
		{
			RedirectToAlbumViewPage(String.Empty, String.Empty);
		}

		/// <summary>
		/// Redirects to the album summary page for the current album. The specified query string value is appended to the redirect URL.
		/// </summary>
		/// <param name="queryStringName">Name of the query string to append to the redirect URL.</param>
		/// <param name="queryStringValue">The query string value to append to the redirect URL.</param>
		public void RedirectToAlbumViewPage(string queryStringName, string queryStringValue)
		{
			#region Validation

			if (!String.IsNullOrEmpty(queryStringName) && String.IsNullOrEmpty(queryStringValue))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringValue parameter is required when the queryStringName parameter is specified. (queryStringName='{0}', queryStringValue='{1}')", queryStringName, queryStringValue));

			if (!String.IsNullOrEmpty(queryStringValue) && String.IsNullOrEmpty(queryStringName))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringName parameter is required when the queryStringValue parameter is specified. (queryStringName='{0}', queryStringValue='{1}')", queryStringName, queryStringValue));

			#endregion

			bool processedQueryString = false;

			string albumId = GetAlbum().Id.ToString(CultureInfo.InvariantCulture);

			if (String.Equals(queryStringName, "aid", StringComparison.OrdinalIgnoreCase))
			{
				albumId = queryStringValue;
				processedQueryString = true;
			}

			string url = WebsiteController.AddQueryStringParameter(WebsiteController.GetAppRootUrl(), String.Format(CultureInfo.CurrentCulture, "aid={0}", albumId));

			if (!processedQueryString && !String.IsNullOrEmpty(queryStringName))
				url = WebsiteController.AddQueryStringParameter(url, String.Format(CultureInfo.CurrentCulture, "{0}={1}", queryStringName, queryStringValue));

			Page.Response.Redirect(url, true);
			//Page.Response.Redirect(url, false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		/// <overloads>Redirects the user to the gallery home page.</overloads>
		/// <summary>
		/// Redirects the user to the gallery home page.
		/// </summary>
		public void RedirectToHomePage()
		{
			RedirectToHomePage(String.Empty, String.Empty);
		}

		/// <summary>
		/// Redirects to home page. The specified query string value is appended to the redirect URL.
		/// </summary>
		/// <param name="queryStringName">Name of the query string to append to the redirect URL.</param>
		/// <param name="queryStringValue">The query string value to append to the redirect URL.</param>
		public void RedirectToHomePage(string queryStringName, string queryStringValue)
		{
			RedirectToHomePage(new string[] { queryStringName }, new string[] { queryStringValue });
		}

		/// <summary>
		/// Redirects to home page. The specified query string values are appended to the redirect URL.
		/// </summary>
		/// <param name="queryStringNames">The query string names to append to the redirect URL.</param>
		/// <param name="queryStringValues">The query string values to append to the redirect URL.</param>
		public void RedirectToHomePage(string[] queryStringNames, string[] queryStringValues)
		{
			#region Validation

			if (queryStringNames == null)
				throw new ArgumentNullException("queryStringNames");

			if (queryStringValues == null)
				throw new ArgumentNullException("queryStringValues");

			if (queryStringNames.Length != queryStringValues.Length)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringValues parameter must have the same number of items as the queryStringNames parameter is specified. (queryStringNames.length='{0}', queryStringValues.length='{1}')", queryStringNames.Length, queryStringValues.Length));

			#endregion

			string url = WebsiteController.GetAppRootUrl();

			if (queryStringNames.Length > 0)
			{
				for (int index = 0; index < queryStringNames.Length; index++)
				{
					if (!String.IsNullOrEmpty(queryStringNames[index]))
						url = WebsiteController.AddQueryStringParameter(url, String.Format(CultureInfo.CurrentCulture, "{0}={1}", queryStringNames[index], queryStringValues[index]));
				}
			}

			Page.Response.Redirect(url, true);
			//Page.Response.Redirect(url, false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		/// <summary>
		/// Recursively iterate through the children of the specified containing control, searching for a child control with
		/// the specified server ID. If the control is found, return it; otherwise return null. This method is useful for finding
		/// child controls of composite controls like GridView and ComponentArt's controls.
		/// </summary>
		/// <param name="containingControl">The containing control whose child controls should be searched.</param>
		/// <param name="id">The server ID of the child control to search for.</param>
		/// <returns>Returns a Control matching the specified server id, or null if no matching control is found.</returns>
		public System.Web.UI.Control FindControlRecursive(System.Web.UI.Control containingControl, string id)
		{
			foreach (System.Web.UI.Control ctrl in containingControl.Controls)
			{
				if (ctrl.ID == id)
					return ctrl;

				if (ctrl.HasControls())
				{
					System.Web.UI.Control foundCtrl = FindControlRecursive(ctrl, id);
					if (foundCtrl != null)
						return foundCtrl;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the ID for the media object before the current one, as sorted by the Sequence property. Albums are
		/// not counted as media objects. If the current media object is the first one in the album, zero is returned.
		/// </summary>
		/// <returns>Returns the ID for the media object after the current one, as sorted by the Sequence property,
		/// or zero if the current media object is the first one in the album.</returns>
		public int GetPreviousMediaObjectId()
		{
			IGalleryObjectCollection siblings = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.MediaObject, true);
			int mediaObjectIndex = siblings.IndexOf(this.GetMediaObject());

			return GetPreviousMediaObjectId(mediaObjectIndex, siblings);
		}

		/// <summary>
		/// Gets the ID for the media object after the current one, as sorted by the Sequence property. Albums are
		/// not counted as media objects. If the current media object is the last one in the album, zero is returned.
		/// </summary>
		/// <returns>Returns the ID for the media object after the current one, as sorted by the Sequence property,
		/// or zero if the current media object is the last one in the album.</returns>
		public int GetNextMediaObjectId()
		{
			IGalleryObjectCollection siblings = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.MediaObject, true);
			int mediaObjectIndex = siblings.IndexOf(this.GetMediaObject());

			return GetNextMediaObjectId(mediaObjectIndex, siblings);
		}

		/// <overloads>Set a page level CSS style defining the width and height of the containers of the thumbnail image of each gallery object.</overloads>
		/// <summary>
		/// Set a page level CSS style defining the width and height of the containers of the thumbnail image of each gallery object.
		/// The width and height is based on the widest thumbnail image and the height of the tallest thumbnail image in the collection of
		/// specified gallery objects, plus a predefined buffer width and height specified by the configuration settings ThumbnailWidthBuffer
		/// and ThumbnailHeightBuffer. The page level style is named "thmb" and is hard-coded to apply to a div tag. The style written to 
		/// the page may look like this:
		/// &lt;style type="text/css"&gt;&lt;!-- div.thmb {width:145px;height:180px;} --&gt;&lt;/style&gt;
		/// </summary>
		/// <param name="galleryObjects">A collection of gallery objects from which the width and height is to be calculated.</param>
		/// <remarks>If the thumbnail images were always the same dimension, the width and height for the thumbnail image container
		/// could be hardcoded in the global style sheet. But since it is variable, we need to programmatically set it.</remarks>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="galleryObjects"/> is null.</exception>
		public void SetThumbnailCssStyle(IGalleryObjectCollection galleryObjects)
		{
			SetThumbnailCssStyle(galleryObjects, 0, 0, new string[] { });
		}

		/// <summary>
		/// Set a page level CSS style defining the width and height of the containers of the thumbnail image of each gallery object.
		/// The width and height is based on the widest thumbnail image and the height of the tallest thumbnail image in the collection of
		/// specified gallery objects, plus a predefined buffer width and height specified by the configuration settings ThumbnailWidthBuffer
		/// and ThumbnailHeightBuffer. The page level style is applied to <paramref name="thumbnailCssClass"/> and is hard-coded to apply
		/// to a div tag. The style written to the page may look like this:
		/// &lt;style type="text/css"&gt;&lt;!-- div.thmb {width:145px;height:180px;} --&gt;&lt;/style&gt;
		/// </summary>
		/// <param name="galleryObjects">A collection of gallery objects from which the width and height is to be calculated.</param>
		/// <param name="thumbnailCssClass">A string representing a CSS class. The calculated width and height will be applied to this
		/// class and written to the page header as a page level style. If not specified (null), this parameter defaults to "thmb".</param>
		/// <remarks>If the thumbnail images were always the same dimension, the width and height for the thumbnail image container
		/// could be hardcoded in the global style sheet. But since it is variable, we need to programmatically set it.</remarks>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="galleryObjects"/> is null.</exception>
		public void SetThumbnailCssStyle(IGalleryObjectCollection galleryObjects, string thumbnailCssClass)
		{
			SetThumbnailCssStyle(galleryObjects, 0, 0, new string[] { thumbnailCssClass });
		}

		/// <summary>
		/// Set a page level CSS style defining the width and height of the containers of the thumbnail image of each gallery object.
		/// The width and height is based on the widest thumbnail image and the height of the tallest thumbnail image in the collection of
		/// specified gallery objects, plus a predefined buffer width and height specified by the configuration settings ThumbnailWidthBuffer
		/// and ThumbnailHeightBuffer. The values in <paramref name="widthBuffer"/> and <paramref name="heightBuffer"/> are added to the calculated
		/// width and height. This is useful when extra space is needed; for example, to make room the textbox on the edit captions page
		/// or the rotate icons on the rotate images page. If no thumbnail CSS class is specified, the page level style defaults to 
		/// setting the width and height on a class named "thmb". The style written to the page may look like this:
		/// &lt;style type="text/css"&gt;&lt;!-- div.thmb {width:145px;height:180px;} --&gt;&lt;/style&gt;
		/// </summary>
		/// <param name="galleryObjects">A collection of gallery objects from which the width and height is to be calculated.</param>
		/// <param name="widthBuffer">A value indicating extra horizontal padding for the thumbnail image container. An integer larger
		/// than zero increases the width; less than zero causes the width to decrease from its calculated value. This parameter is 
		/// typically specified when extra space is needed to make room for elements within the thumbnail image container, such as
		/// the textbox on the edit captions page or the rotate icons on the rotate images page.</param>
		/// <param name="heightBuffer">A value indicating extra vertical padding for the thumbnail image container. An integer larger
		/// than zero increases the height; less than zero causes the height to decrease from its calculated value. This parameter is 
		/// typically specified when extra space is needed to make room for elements within the thumbnail image container, such as
		/// the textbox on the edit captions page or the rotate icons on the rotate images page.</param>
		/// <remarks>If the thumbnail images were always the same dimension, the width and height for the thumbnail image container
		/// could be hardcoded in the global style sheet. But since it is variable, we need to programmatically set it.</remarks>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="galleryObjects"/> is null.</exception>
		public void SetThumbnailCssStyle(IGalleryObjectCollection galleryObjects, int widthBuffer, int heightBuffer)
		{
			SetThumbnailCssStyle(galleryObjects, widthBuffer, heightBuffer, new string[] { });
		}

		/// <summary>
		/// Set a page level CSS style defining the width and height of the containers of the thumbnail image of each gallery object.
		/// The width and height is based on the widest thumbnail image and the height of the tallest thumbnail image in the collection of
		/// specified gallery objects, plus a predefined buffer width and height specified by the configuration settings ThumbnailWidthBuffer
		/// and ThumbnailHeightBuffer. The values in <paramref name="widthBuffer"/> and <paramref name="heightBuffer"/> are added to the calculated
		/// width and height. This is useful when extra space is needed; for example, to make room the textbox on the edit captions page
		/// or the rotate icons on the rotate images page. The page level style is applied to <paramref name="thumbnailCssClass"/> and is 
		/// hard-coded to apply to a div tag. The style written to the page may look like this:
		/// &lt;style type="text/css"&gt;&lt;!-- div.thmb {width:145px;height:180px;} --&gt;&lt;/style&gt;
		/// </summary>
		/// <param name="galleryObjects">A collection of gallery objects from which the width and height is to be calculated.</param>
		/// <param name="widthBuffer">A value indicating extra horizontal padding for the thumbnail image container. An integer larger
		/// than zero increases the width; less than zero causes the width to decrease from its calculated value. This parameter is 
		/// typically specified when extra space is needed to make room for elements within the thumbnail image container, such as
		/// the textbox on the edit captions page or the rotate icons on the rotate images page.</param>
		/// <param name="heightBuffer">A value indicating extra vertical padding for the thumbnail image container. An integer larger
		/// than zero increases the height; less than zero causes the height to decrease from its calculated value. This parameter is 
		/// typically specified when extra space is needed to make room for elements within the thumbnail image container, such as
		/// the textbox on the edit captions page or the rotate icons on the rotate images page.</param>
		/// <param name="thumbnailCssClass">A string representing a CSS class. The calculated width and height will be applied to this
		/// class and written to the page header as a page level style. If not specified (null), this parameter defaults to "thmb".</param>
		/// <remarks>If the thumbnail images were always the same dimension, the width and height for the thumbnail image container
		/// could be hardcoded in the global style sheet. But since it is variable, we need to programmatically set it.</remarks>
		/// <exception cref="System.ArgumentNullException">Thrown when the galleryObjects parameter is null.</exception>
		public void SetThumbnailCssStyle(IGalleryObjectCollection galleryObjects, int widthBuffer, int heightBuffer, string thumbnailCssClass)
		{
			SetThumbnailCssStyle(galleryObjects, widthBuffer, heightBuffer, new string[] { thumbnailCssClass });
		}

		/// <summary>
		/// Set a page level CSS style defining the width and height of the containers of the thumbnail image of each gallery object.
		/// The width and height is based on the widest thumbnail image and the height of the tallest thumbnail image in the collection of
		/// specified gallery objects, plus a predefined buffer width and height specified by the configuration settings ThumbnailWidthBuffer
		/// and ThumbnailHeightBuffer. The values in <paramref name="widthBuffer"/> and <paramref name="heightBuffer"/> are added to the calculated
		/// width and height. This is useful when extra space is needed; for example, to make room the textbox on the edit captions page
		/// or the rotate icons on the rotate images page. The page level style is applied to each of the strings in
		/// <paramref name="thumbnailCssClasses"/> and is hard-coded to apply to a div tag. The style written to the page may look like this:
		/// &lt;style type="text/css"&gt;&lt;!-- div.thmb {width:145px;height:180px;} --&gt;&lt;/style&gt;
		/// </summary>
		/// <param name="galleryObjects">A collection of gallery objects from which the width and height is to be calculated.</param>
		/// <param name="widthBuffer">A value indicating extra horizontal padding for the thumbnail image container. An integer larger
		/// than zero increases the width; less than zero causes the width to decrease from its calculated value. This parameter is 
		/// typically specified when extra space is needed to make room for elements within the thumbnail image container, such as
		/// the textbox on the edit captions page or the rotate icons on the rotate images page.</param>
		/// <param name="heightBuffer">A value indicating extra vertical padding for the thumbnail image container. An integer larger
		/// than zero increases the height; less than zero causes the height to decrease from its calculated value. This parameter is 
		/// typically specified when extra space is needed to make room for elements within the thumbnail image container, such as
		/// the textbox on the edit captions page or the rotate icons on the rotate images page.</param>
		/// <param name="thumbnailCssClasses">A string array of CSS classes. The calculated width and height will be applied to these
		/// classes and written to the page header as a page level style. If not specified (null) or it has a length of zero, this 
		/// parameter defaults to a single string "thmb".</param>
		/// <remarks>If the thumbnail images were always the same dimension, the width and height for the thumbnail image container
		/// could be hardcoded in the global style sheet. But since it is variable, we need to programmatically set it.</remarks>
		/// <exception cref="System.ArgumentNullException">Thrown when the galleryObjects parameter is null.</exception>
		public void SetThumbnailCssStyle(IGalleryObjectCollection galleryObjects, int widthBuffer, int heightBuffer, string[] thumbnailCssClasses)
		{
			if (galleryObjects == null)
				throw new ArgumentNullException("galleryObjects");

			if ((thumbnailCssClasses == null) || (thumbnailCssClasses.Length == 0))
			{
				thumbnailCssClasses = new string[] { "thmb" };
			}

			// Calculate the width of the widest thumbnail image and the height of the tallest thumbnail 
			// image in this album. 
			int maxMoWidth = 0;
			int maxMoHeight = 0;

			foreach (IGalleryObject mo in galleryObjects)
			{
				if (mo.Thumbnail.Width > maxMoWidth)
					maxMoWidth = mo.Thumbnail.Width;

				if (mo.Thumbnail.Height > maxMoHeight)
					maxMoHeight = mo.Thumbnail.Height;
			}

			// If no width or height have been set, set to the default thumbnail width and height so
			// that we have reasonable minimum values.
			GalleryServerPro.Configuration.Core coreConfig = WebsiteController.GetGalleryServerProConfigSection().Core;
			if ((maxMoWidth == 0) || (maxMoHeight == 0))
			{
				int maxLength = coreConfig.MaxThumbnailLength;
				float ratio = coreConfig.EmptyAlbumThumbnailWidthToHeightRatio;
				if (ratio > 1) // Landscape (width is greater than height)
				{
					maxMoWidth = maxLength;
					maxMoHeight = Convert.ToInt32((float)maxLength / ratio);
				}
				else // Portrait (width is less than height)
				{
					maxMoHeight = maxLength;
					maxMoWidth = Convert.ToInt32((float)maxLength * ratio);
				}
			}

			int maxWidth = maxMoWidth + coreConfig.ThumbnailWidthBuffer + widthBuffer;
			int maxHeight = maxMoHeight + coreConfig.ThumbnailHeightBuffer + heightBuffer;

			string pageStyle = "\n<style type=\"text/css\"><!-- ";
			foreach (string cssClass in thumbnailCssClasses)
			{
				pageStyle += String.Format(CultureInfo.CurrentCulture, "div.{0} {{width:{1}px;height:{2}px;}} ", cssClass, maxWidth, maxHeight);
			}
			pageStyle += "--></style>\n";

			this.Page.Header.Controls.Add(new System.Web.UI.LiteralControl(pageStyle));
		}

		public void ClearIsAnony()
		{
			this._isAnonymousUser = null;
		}

		#endregion

		#region Public Static Methods (WebMethods)

		/// <summary>
		/// Get information for the specified media object, including its previous and next media object.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object.</param>
		/// <param name="displayType">The type of display object to receive (thumbnail, optimized, original).</param>
		/// <returns>Returns an instance of MediaObjectWebEntity containing information for the specified media object,
		/// including its previous and next media object.</returns>
		[System.Web.Services.WebMethod]
		public static MediaObjectWebEntity GetMediaObjectHtml(int mediaObjectId, DisplayObjectType displayType)
		{
			return GetMediaObjectHtml(Factory.LoadMediaObjectInstance(mediaObjectId), displayType, true);
		}

		/// <summary>
		/// Permanently deletes the specified media object from the file system and data store. No action is taken if the
		/// user does not have delete permission.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object to be deleted.</param>
		[System.Web.Services.WebMethod]
		public static void DeleteMediaObject(int mediaObjectId)
		{
			IGalleryObject mo = Factory.LoadMediaObjectInstance(mediaObjectId);
			if (GspPage.IsUserAuthorized(SecurityActions.DeleteMediaObject, mo.Parent.Id))
			{
				mo.Delete();
				HelperFunctions.PurgeCache();
			}
		}

		/// <summary>
		/// Update the media object with the specified title and persist to the data store. The title is validated before
		/// saving, and may be altered to conform to business rules, such as removing HTML tags. The validated title is returned.
		/// If the user is not authorized to edit the title, no action is taken and the original title as stored in the data
		/// store is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object whose title is to be updated.</param>
		/// <param name="title">The title to be saved to the media object.</param>
		/// <returns>Returns the validated title.</returns>
		[System.Web.Services.WebMethod]
		public static string UpdateMediaObjectTitle(int mediaObjectId, string title)
		{
			IGalleryObject mo = Factory.LoadMediaObjectInstance(mediaObjectId);
			if (GspPage.IsUserAuthorized(SecurityActions.EditMediaObject, mo.Parent.Id))
			{
				string previousTitle = mo.Title;
				mo.Title = WebsiteController.CleanHtmlTags(title);

				if (mo.Title != previousTitle)
					WebsiteController.SaveGalleryObject(mo);

				HelperFunctions.PurgeCache();
			}

			return mo.Title;
		}

		/// <summary>
		/// Update the album with the specified properties in the albumEntity parameter. The title is validated before
		/// saving, and may be altered to conform to business rules, such as removing HTML tags. After the object is persisted
		/// to the data store, the albumEntity parameter is updated with the latest properties from the album object and returned.
		/// If the user is not authorized to edit the album, no action is taken.
		/// </summary>
		/// <param name="albumEntity">An AlbumWebEntity instance containing data to be persisted to the data store.</param>
		/// <returns>Returns an AlbumWebEntity instance containing the data as persisted to the data store. Some properties,
		/// such as the Title property, may be slightly altered to conform to validation rules.</returns>
		[System.Web.Services.WebMethod]
		public static AlbumWebEntity UpdateAlbumInfo(AlbumWebEntity albumEntity)
		{
			IAlbum album = Factory.LoadAlbumInstance(albumEntity.Id, false);
			if (GspPage.IsUserAuthorized(SecurityActions.EditAlbum, album.Id))
			{
				if (album.Title != albumEntity.Title)
				{
					album.Title = WebsiteController.CleanHtmlTags(albumEntity.Title);
					if ((!album.IsRootAlbum) && (WebsiteController.GetGalleryServerProConfigSection().Core.SynchAlbumTitleAndDirectoryName))
					{
						// Root albums do not have a directory name that reflects the album's title, so only update this property for non-root albums.
						album.DirectoryName = HelperFunctions.ValidateDirectoryName(album.Parent.FullPhysicalPath, album.Title);
					}
				}
				album.Summary = WebsiteController.CleanHtmlTags(albumEntity.Summary);
				album.DateStart = albumEntity.DateStart.Date;
				album.DateEnd = albumEntity.DateEnd.Date;
				if (albumEntity.IsPrivate != album.IsPrivate)
				{
					if (!albumEntity.IsPrivate && album.Parent.IsPrivate)
					{
						throw new NotSupportedException("Cannot make album public: It is invalid to make an album public when it's parent album is private.");
					}
					album.IsPrivate = albumEntity.IsPrivate;
					SynchIsPrivatePropertyOnChildGalleryObjects(album);
				}
				WebsiteController.SaveGalleryObject(album);
				HelperFunctions.PurgeCache();

				// Refresh the entity object with the data from the album object, in case something changed. For example,
				// some javascript or HTML may have been removed from the Title or Summary fields.
				albumEntity.Title = album.Title;
				albumEntity.Summary = album.Summary;
				albumEntity.DateStart = album.DateStart;
				albumEntity.DateEnd = album.DateEnd;
				albumEntity.IsPrivate = album.IsPrivate;
			}

			return albumEntity;
		}

		/// <summary>
		/// Retrieve album information for the specified album ID. Returns an object with empty properties if the user
		/// does not have permission to view the specified album.
		/// </summary>
		/// <param name="albumId">The album ID for which to retrieve information.</param>
		/// <returns>Returns AlbumWebEntity object containing information about the requested album.</returns>
		[System.Web.Services.WebMethod]
		public static AlbumWebEntity GetAlbumInfo(int albumId)
		{
			AlbumWebEntity albumEntity = new AlbumWebEntity();

			if (GspPage.IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, albumId))
			{
				IAlbum album = Factory.LoadAlbumInstance(albumId, false);
				albumEntity.Title = album.Title;
				albumEntity.Summary = album.Summary;
				albumEntity.DateStart = album.DateStart;
				albumEntity.DateEnd = album.DateEnd;
				albumEntity.IsPrivate = album.IsPrivate;
			}

			return albumEntity;
		}

		/// <summary>
		/// Get a list of metadata items corresponding to the specified mediaObjectId. Guaranteed to not return null.
		/// </summary>
		/// <param name="mediaObjectId">The ID of the media object for which to return its metadata items.</param>
		/// <returns>Returns a generic list of MetadataItemWebEntity objects that contain the metadata for the 
		/// specified media object.</returns>
		[System.Web.Services.WebMethod]
		public static System.Collections.Generic.List<MetadataItemWebEntity> GetMetadataItems(int mediaObjectId)
		{
			System.Collections.Generic.List<MetadataItemWebEntity> metadataItems = new System.Collections.Generic.List<MetadataItemWebEntity>();

			bool isUserAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
			IGalleryObject mediaObject = Factory.LoadMediaObjectInstance(mediaObjectId);
			if (SecurityManager.IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, WebsiteController.GetRolesForUser(), mediaObject.Parent.Id, isUserAuthenticated, ((IAlbum)mediaObject.Parent).IsPrivate))
			{
				foreach (IGalleryObjectMetadataItem metadata in mediaObject.MetadataItems)
				{
					metadataItems.Add(new MetadataItemWebEntity(metadata.Description, metadata.Value));
				}
			}

			return metadataItems;
		}

		#endregion

		#region Internal Static Methods

		/// <summary>
		/// Get information for the specified media object, including its previous and next media object.
		/// </summary>
		/// <param name="mediaObject">The media object.</param>
		/// <param name="displayType">The type of display object to receive (thumbnail, optimized, original).</param>
		/// <param name="isCallBack">Indicates whether the current invocation is caused by an AJAX callback.</param>
		/// <returns>
		/// Returns an instance of MediaObjectWebEntity containing information for the specified media object,
		/// including its previous and next media object.
		/// </returns>
		internal static MediaObjectWebEntity GetMediaObjectHtml(IGalleryObject mediaObject, DisplayObjectType displayType, bool isCallBack)
		{
			// Get the information about the specified media object, its previous one, next one, and the next one in a slide show.

			System.Collections.ArrayList browsers = HttpContext.Current.Request.Browser.Browsers;
			bool isUserAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
			MediaObjectWebEntity mo = new MediaObjectWebEntity();

			if ((displayType == DisplayObjectType.Original) && (!SecurityManager.IsUserAuthorized(SecurityActions.ViewOriginalImage, WebsiteController.GetRolesForUser(), mediaObject.Parent.Id, isUserAuthenticated, ((IAlbum)mediaObject.Parent).IsPrivate)))
			{
				displayType = DisplayObjectType.Optimized;
			}

			#region Step 1: Process current media object

			if (mediaObject.Id > 0)
			{
				// This section is enclosed in the above if statement to force all declared variables within it to be local so they are
				// not accidentally re-used in steps 2 or 3. In reality, mediaObject.Id should ALWAYS be greater than 0.
				IDisplayObject displayObject = GetDisplayObject(mediaObject, displayType);

				string htmlOutput = String.Empty;
				string scriptOutput = String.Empty;
				if (!String.IsNullOrEmpty(mediaObject.Original.ExternalHtmlSource))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(mediaObject.Original.ExternalHtmlSource);
					htmlOutput = moBuilder.GenerateHtml();
				}
				else if ((displayObject.Width > 0) && (displayObject.Height > 0))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(mediaObject.Id, mediaObject.Parent.Id, displayObject.MimeType, displayObject.FileNamePhysicalPath, displayObject.Width, displayObject.Height, mediaObject.Title, browsers, displayType, mediaObject.IsPrivate);
					htmlOutput = moBuilder.GenerateHtml();
					scriptOutput = moBuilder.GenerateScript();
				}

				if (String.IsNullOrEmpty(htmlOutput))
				{
					// We'll get here when the user is trying to view a media object that cannot be displayed in the browser or the
					// config file does not have a definition for this MIME type. Default to a standard message noting that the user
					// can download the object via one of the toolbar commands.
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Get the siblings of this media object and the index that specifies its position within its siblings.
				bool excludePrivateObjects = !isUserAuthenticated;

				///TODO: This technique for identifying the index is very expensive when there are a lot of objects in the album.
				IGalleryObjectCollection siblings = ((IAlbum)mediaObject.Parent).GetChildGalleryObjects(GalleryObjectType.MediaObject, true, excludePrivateObjects);
				int mediaObjectIndex = siblings.IndexOf(mediaObject);

				// Build up the entity object we'll be sending to the client.
				bool moIsImage = (mediaObject is GalleryServerPro.Business.Image);
				bool moIsExternalObject = (mediaObject is GalleryServerPro.Business.ExternalMediaObject);
				mo.Id = mediaObject.Id;
				mo.Index = mediaObjectIndex;
				mo.NumObjectsInAlbum = siblings.Count;
				mo.Title = mediaObject.Title;
				mo.PrevId = GetPreviousMediaObjectId(mediaObjectIndex, siblings); ;
				mo.NextId = GetNextMediaObjectId(mediaObjectIndex, siblings);
				mo.NextSSId = GetNextMediaObjectIdForSlideshow(mediaObjectIndex, siblings);
				mo.HtmlOutput = htmlOutput;
				mo.ScriptOutput = scriptOutput;
				mo.PermalinkUrl = GetPermalinkUrl(mediaObject.Id, displayType, moIsImage);
				mo.Width = displayObject.Width;
				mo.Height = displayObject.Height;
				mo.HiResAvailable = (moIsImage && (!String.IsNullOrEmpty(mediaObject.Optimized.FileName)) && (mediaObject.Original.FileName != mediaObject.Optimized.FileName));
				mo.IsDownloadable = !moIsExternalObject;
			}

			#endregion

			#region Step 2: Process previous media object

			if (mo.PrevId > 0)
			{
				IGalleryObject prevMO = Factory.LoadMediaObjectInstance(mo.PrevId);

				IDisplayObject displayObject = GetDisplayObject(prevMO, displayType);

				string htmlOutput = String.Empty;
				string scriptOutput = String.Empty;
				if (!String.IsNullOrEmpty(prevMO.Original.ExternalHtmlSource))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(prevMO.Original.ExternalHtmlSource);
					htmlOutput = moBuilder.GenerateHtml();
				}
				else if ((displayObject.Width > 0) && (displayObject.Height > 0))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(prevMO.Id, prevMO.Parent.Id, displayObject.MimeType, displayObject.FileNamePhysicalPath, displayObject.Width, displayObject.Height, prevMO.Title, browsers, displayType, prevMO.IsPrivate);
					htmlOutput = moBuilder.GenerateHtml();
					scriptOutput = moBuilder.GenerateScript();
				}
				
				if (String.IsNullOrEmpty(htmlOutput))
				{
					// We'll get here when the user is trying to view a media object that cannot be displayed in the browser or the
					// config file does not have a definition for this MIME type. Default to a standard message noting that the user
					// can download the object via one of the toolbar commands.
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Build up the entity object we'll be sending to the client.
				bool prevMoIsImage = (prevMO is GalleryServerPro.Business.Image);
				bool prevMoIsExternalObject = (prevMO is GalleryServerPro.Business.ExternalMediaObject);
				mo.PrevTitle = prevMO.Title;
				mo.PrevHtmlOutput = htmlOutput;
				mo.PrevScriptOutput = scriptOutput;
				mo.PrevPermalinkUrl = GetPermalinkUrl(prevMO.Id, displayType, prevMoIsImage);
				mo.PrevWidth = displayObject.Width;
				mo.PrevHeight = displayObject.Height;
				mo.PrevHiResAvailable = (prevMoIsImage && (!String.IsNullOrEmpty(prevMO.Optimized.FileName)) && (prevMO.Original.FileName != prevMO.Optimized.FileName));
				mo.PrevIsDownloadable = !prevMoIsExternalObject;
			}

			#endregion

			#region Step 3: Process next media object

			if (mo.NextId > 0)
			{
				IGalleryObject nextMO = Factory.LoadMediaObjectInstance(mo.NextId);

				IDisplayObject displayObject = GetDisplayObject(nextMO, displayType);

				string htmlOutput = String.Empty;
				string scriptOutput = String.Empty;
				if (!String.IsNullOrEmpty(nextMO.Original.ExternalHtmlSource))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(nextMO.Original.ExternalHtmlSource);
					htmlOutput = moBuilder.GenerateHtml();
				}
				else if ((displayObject.Width > 0) && (displayObject.Height > 0))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(nextMO.Id, nextMO.Parent.Id, displayObject.MimeType, displayObject.FileNamePhysicalPath, displayObject.Width, displayObject.Height, nextMO.Title, browsers, displayType, nextMO.IsPrivate);
					htmlOutput = moBuilder.GenerateHtml();
					scriptOutput = moBuilder.GenerateScript();
				}

				if (String.IsNullOrEmpty(htmlOutput))
				{
					// We'll get here when the user is trying to view a media object that cannot be displayed in the browser or the
					// config file does not have a definition for this MIME type. Default to a standard message noting that the user
					// can download the object via one of the toolbar commands.
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Build up the entity object we'll be sending to the client.
				bool nextMoIsImage = (nextMO is GalleryServerPro.Business.Image);
				bool nextMoIsExternalObject = (nextMO is GalleryServerPro.Business.ExternalMediaObject);
				mo.NextTitle = nextMO.Title;
				mo.NextHtmlOutput = htmlOutput;
				mo.NextScriptOutput = scriptOutput;
				mo.NextPermalinkUrl = GetPermalinkUrl(nextMO.Id, displayType, nextMoIsImage);
				mo.NextWidth = displayObject.Width;
				mo.NextHeight = displayObject.Height;
				mo.NextHiResAvailable = (nextMoIsImage && (!String.IsNullOrEmpty(nextMO.Optimized.FileName)) && (nextMO.Original.FileName != nextMO.Optimized.FileName));
				mo.NextIsDownloadable = !nextMoIsExternalObject;
			}

			#endregion

			#region Step 4: Process next slide show media object

			if (mo.NextSSId > 0)
			{
				IGalleryObject nextSSMO = Factory.LoadMediaObjectInstance(mo.NextSSId);

				IDisplayObject displayObject = GetDisplayObject(nextSSMO, displayType);

				string htmlOutput = String.Empty;
				string scriptOutput = String.Empty;
				string url = String.Empty;
				if (!String.IsNullOrEmpty(nextSSMO.Original.ExternalHtmlSource))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(nextSSMO.Original.ExternalHtmlSource);
					htmlOutput = moBuilder.GenerateHtml();
				}
				else if ((displayObject.Width > 0) && (displayObject.Height > 0))
				{
					IMediaObjectHtmlBuilder moBuilder = new MediaObjectHtmlBuilder(nextSSMO.Id, nextSSMO.Parent.Id, displayObject.MimeType, displayObject.FileNamePhysicalPath, displayObject.Width, displayObject.Height, nextSSMO.Title, browsers, displayType, nextSSMO.IsPrivate);
					htmlOutput = moBuilder.GenerateHtml();
					scriptOutput = moBuilder.GenerateScript();
					url = moBuilder.GenerateUrl();
				}

				if (String.IsNullOrEmpty(htmlOutput))
				{
					// We'll get here when the user is trying to view a media object that cannot be displayed in the browser or the
					// config file does not have a definition for this MIME type. Default to a standard message noting that the user
					// can download the object via one of the toolbar commands.
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Get the siblings of this media object and the index that specifies its position within its siblings.
				bool excludePrivateObjects = !isUserAuthenticated;

				///TODO: This technique for identifying the index is very expensive when there are a lot of objects in the album.
				IGalleryObjectCollection siblings = ((IAlbum)nextSSMO.Parent).GetChildGalleryObjects(GalleryObjectType.MediaObject, true, excludePrivateObjects);
				int mediaObjectIndex = siblings.IndexOf(nextSSMO);

				// Build up the entity object we'll be sending to the client.
				bool nextSSMoIsImage = (nextSSMO is GalleryServerPro.Business.Image);
				mo.NextSSIndex = mediaObjectIndex;
				mo.NextSSTitle = nextSSMO.Title;
				mo.NextSSUrl = url;
				mo.NextSSHtmlOutput = htmlOutput;
				mo.NextSSScriptOutput = scriptOutput;
				mo.NextSSPermalinkUrl = GetPermalinkUrl(nextSSMO.Id, displayType, nextSSMoIsImage);
				mo.NextSSWidth = displayObject.Width;
				mo.NextSSHeight = displayObject.Height;
				mo.NextSSHiResAvailable = (nextSSMoIsImage && (!String.IsNullOrEmpty(nextSSMO.Optimized.FileName)) && (nextSSMO.Original.FileName != nextSSMO.Optimized.FileName));
				mo.NextSSIsDownloadable = true; // Slide show objects are always locally stored images and are therefore always downloadable
			}

			#endregion

			#region Step 5: Update ReferringUrl session variable

			if (HttpContext.Current.Session != null)
			{
				Uri backURL = (Uri)HttpContext.Current.Session["ReferringUrl"];
				if (isCallBack && (backURL != null))
				{
					// We are in a callback. Even though the page hasn't changed, the user is probably viewing a different media object,
					// so update the moid query string parameter so that the referring url points to the current media object.
					backURL = UpdateReferringUrlBeforeSaving(backURL, mediaObject.Id);
				}
				else
				{
					backURL = HttpContext.Current.Request.Url;
				}
				HttpContext.Current.Session["ReferringUrl"] = backURL;
			}

			#endregion

			return mo;
		}

		internal static int GetPreviousMediaObjectId(int mediaObjectIndex, IGalleryObjectCollection siblings)
		{
			int previousMediaObjectId = 0;
			if (mediaObjectIndex > 0)
			{
				previousMediaObjectId = siblings[mediaObjectIndex - 1].Id;
			}

			return previousMediaObjectId;
		}

		internal static int GetNextMediaObjectId(int mediaObjectIndex, IGalleryObjectCollection siblings)
		{
			int nextMediaObjectId = 0;
			if (mediaObjectIndex < (siblings.Count - 1))
			{
				nextMediaObjectId = siblings[mediaObjectIndex + 1].Id;
			}

			return nextMediaObjectId;
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method will continue to work correctly if the webmaster configures
		/// Gallery Server to require users to log in in order to view objects, since at that point there will be no such thing as 
		/// un-authenticated users, and the standard gallery server role functionality applies. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <param name="albumId">The album ID to which the security action applies.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action against the specified album;
		/// otherwise returns false.</returns>
		internal static bool IsUserAuthorized(SecurityActions securityActions, int albumId)
		{
			if (((securityActions == SecurityActions.ViewAlbumOrMediaObject) || (securityActions == SecurityActions.ViewOriginalImage))
			    && (!HttpContext.Current.User.Identity.IsAuthenticated))
				throw new System.NotSupportedException("Wrong method call: You must call the overload of GspPage.IsUserAuthorized that has the isPrivate parameter when the security action is ViewAlbumOrMediaObject or ViewOriginalImage and the user is anonymous (not logged on).");

			return IsUserAuthorized(securityActions, WebsiteController.GetRolesForUser(), albumId, false);
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method will continue to work correctly if the webmaster configures
		/// Gallery Server to require users to log in in order to view objects, since at that point there will be no such thing as 
		/// un-authenticated users, and the standard gallery server role functionality applies. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <param name="albumId">The album ID to which the security action applies.</param>
		/// <param name="isPrivate">Indicates whether the specified album is private (hidden from anonymous users). The parameter
		/// is ignored for logged on users.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action against the specified album;
		/// otherwise returns false.</returns>
		internal static bool IsUserAuthorized(SecurityActions securityActions, int albumId, bool isPrivate)
		{
			return IsUserAuthorized(securityActions, WebsiteController.GetRolesForUser(), albumId, isPrivate);
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method will continue to work correctly if the webmaster configures
		/// Gallery Server to require users to log in in order to view objects, since at that point there will be no such thing as 
		/// un-authenticated users, and the standard gallery server role functionality applies. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <param name="roles">A collection of Gallery Server roles to which the currently logged-on user belongs.</param>
		/// <param name="albumId">The album ID to which the security action applies.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action against the specified album;
		/// otherwise returns false.</returns>
		internal static bool IsUserAuthorized(SecurityActions securityActions, IGalleryServerRoleCollection roles, int albumId)
		{
			if (((securityActions == SecurityActions.ViewAlbumOrMediaObject) || (securityActions == SecurityActions.ViewOriginalImage))
			    && (!HttpContext.Current.User.Identity.IsAuthenticated))
				throw new System.NotSupportedException("Wrong method call: You must call the overload of GspPage.IsUserAuthorized that has the isPrivate parameter when the security action is ViewAlbumOrMediaObject or ViewOriginalImage and the user is anonymous (not logged on).");

			return IsUserAuthorized(securityActions, roles, albumId, false);
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method will continue to work correctly if the webmaster configures
		/// Gallery Server to require users to log in in order to view objects, since at that point there will be no such thing as 
		/// un-authenticated users, and the standard gallery server role functionality applies. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <param name="roles">A collection of Gallery Server roles to which the currently logged-on user belongs.</param>
		/// <param name="albumId">The album ID to which the security action applies.</param>
		/// <param name="isPrivate">Indicates whether the specified album is private (hidden from anonymous users). The parameter
		/// is ignored for logged on users.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action against the specified album;
		/// otherwise returns false.</returns>
		internal static bool IsUserAuthorized(SecurityActions securityActions, IGalleryServerRoleCollection roles, int albumId, bool isPrivate)
		{
			return WebsiteController.IsUserAuthorized(securityActions, roles, albumId, isPrivate);
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Return the requested display object from the specified media object. If Unknown is passed in the 
		/// displayType parameter, and the object is an image, return the optimized object. If an optimized 
		/// version does not exist, return the original object. If Unknown is passed in the displayType parameter, 
		/// and the object is NOT an image, return the original object. If a thumbnail is requested, always 
		/// return a thumbnail object.
		/// </summary>
		/// <param name="mediaObject">The media object containing the display object to return.</param>
		/// <param name="displayType">One of the DisplayObjectType enumeration values indicating which object to return.</param>
		/// <returns>Returns the requested display object from the specified media object.</returns>
		private static IDisplayObject GetDisplayObject(IGalleryObject mediaObject, DisplayObjectType displayType)
		{
			IDisplayObject displayObject = null;

			if (displayType == DisplayObjectType.Thumbnail)
			{
				displayObject = mediaObject.Thumbnail;
			}
			else if (mediaObject is GalleryServerPro.Business.Image)
			{
				displayObject = GetDisplayObjectForImage(mediaObject, displayType);
			}
			else
			{
				displayObject = mediaObject.Original;
			}

			return displayObject;
		}

		/// <summary>
		/// Return the requested display object from the specified image object. If Unknown is passed in the 
		/// displayType parameter, return the optimized object. If an optimized version does not exist, return 
		/// the original object. If a thumbnail is requested, always return a thumbnail object.
		/// </summary>
		/// <param name="mediaObject">The media object containing the display object to return.</param>
		/// <param name="displayType">One of the DisplayObjectType enumeration values indicating which object to return.</param>
		/// <returns>Returns the requested display object from the specified image object.</returns>
		/// <exception cref="System.ArgumentException">Thrown when the parameter mediaObject is not of type 
		/// GalleryServerPro.Business.Image.</exception>
		private static IDisplayObject GetDisplayObjectForImage(IGalleryObject mediaObject, DisplayObjectType displayType)
		{
			if (!(mediaObject is GalleryServerPro.Business.Image))
			{
				throw new ArgumentException("The parameter 'mediaObject' in function GspPage.GetDisplayObjectForImage must be of type GalleryServerPro.Business.Image, but it was not.");
			}

			IDisplayObject displayObject;
			switch (displayType)
			{
				case DisplayObjectType.Thumbnail:
					{
						displayObject = mediaObject.Thumbnail;
						break;
					}
				case DisplayObjectType.Unknown:
				case DisplayObjectType.Optimized:
					{
						if (mediaObject.Optimized.FileName == mediaObject.Original.FileName)
						{
							// No optimized version is available
							displayObject = mediaObject.Original;
						}
						else
						{
							displayObject = mediaObject.Optimized;
						}
						break;
					}
				case DisplayObjectType.Original:
					{
						displayObject = mediaObject.Original;
						break;
					}
				default:
					{
						displayObject = mediaObject.Optimized;
						break;
					}
			}

			return displayObject;
		}

		private static int GetNextMediaObjectIdForSlideshow(int mediaObjectIndex, IGalleryObjectCollection siblings)
		{
			int nextMediaObjectId = 0;
			while (mediaObjectIndex < (siblings.Count - 1))
			{
				IGalleryObject nextMediaObject = siblings[mediaObjectIndex + 1];
				if (nextMediaObject is GalleryServerPro.Business.Image)
				{
					nextMediaObjectId = nextMediaObject.Id;
					break;
				}

				mediaObjectIndex += 1;
			}

			return nextMediaObjectId;
		}

		/// <summary>
		/// Calculate an URL that can be used by the user to navigate directly to the specified media object. This
		/// functionality is required because the user may be navigating through objects using AJAX callbacks which
		/// does not update the web browser's address bar.
		/// </summary>
		/// <param name="mediaObjectId">An integer that uniquely identifies the media object.</param>
		/// <param name="displayType">A DisplayObjectType enumeration value indicating the version of the
		/// object for which the URL should be generated. Possible values: Optimized, Original.
		/// An exception is thrown if any other enumeration is passed.</param>
		/// <param name="isImage">A value indicating whether the media object is the type GalleryServerPro.Business.Image.
		/// If true, and if the user is requesting a link to the original image (displayType = DisplayObjectType.Original),
		/// then "&amp;hr=1" is appended to the query string.
		/// </param>
		/// <returns>Returns an URL that can be used by the user to navigate directly to the specified media object.</returns>
		private static string GetPermalinkUrl(int mediaObjectId, DisplayObjectType displayType, bool isImage)
		{
			if ((displayType != DisplayObjectType.Optimized) && (displayType != DisplayObjectType.Original))
				throw new ArgumentOutOfRangeException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.GspPage_GetPermalinkUrl_Ex_Msg1, displayType.ToString()));

			string permalinkUrl = String.Format(CultureInfo.CurrentCulture, "{0}?moid={1}", WebsiteController.GetAppRootAbsoluteUrl(), mediaObjectId);

			if ((isImage) && (displayType == DisplayObjectType.Original))
			{
				permalinkUrl += "&hr=1";
			}

			return permalinkUrl;
		}

		/// <summary>
		/// Set the IsPrivate property of all child albums and media objects of the specified album to have the same value
		/// as the specified album.
		/// </summary>
		/// <param name="album">The album whose child objects are to be updated to have the same IsPrivate value.</param>
		private static void SynchIsPrivatePropertyOnChildGalleryObjects(IAlbum album)
		{
			album.Inflate(true);
			foreach (IAlbum childAlbum in album.GetChildGalleryObjects(GalleryObjectType.Album))
			{
				childAlbum.Inflate(true); // The above Inflate() does not inflate child albums, so we need to explicitly inflate it.
				childAlbum.IsPrivate = album.IsPrivate;
				WebsiteController.SaveGalleryObject(childAlbum);
				SynchIsPrivatePropertyOnChildGalleryObjects(childAlbum);
			}

			foreach (IGalleryObject childGalleryObject in album.GetChildGalleryObjects(GalleryObjectType.MediaObject))
			{
				childGalleryObject.IsPrivate = album.IsPrivate;
				WebsiteController.SaveGalleryObject(childGalleryObject);
			}
		}

		#endregion

		#region Private Methods

		private static string GetEncryptedAlbumId(int albumId)
		{
			string encryptedAlbumId;
			if (!_encryptedAlbumIds.TryGetValue(albumId, out encryptedAlbumId))
			{
				encryptedAlbumId = HelperFunctions.Encrypt(albumId.ToString(CultureInfo.InvariantCulture));
				_encryptedAlbumIds.Add(albumId, encryptedAlbumId);
			}

			return encryptedAlbumId;
		}

		/// <summary>
		/// Stores the URI of the page the user navigated from in session so that it is available after subsequent postbacks.
		/// After this method is called the web page may call RedirectToReferringPage() to redirect the user to the referring
		/// web page. Note: If the referring page is the login page (login.aspx), no value is stored in session because we do
		/// not want to navigate back to the login page. In this case, any previous value in the session variable is preserved,
		/// assuming the session has not timed out (which it probably has, since the user had to log in). If session state is
		/// disabled (as it is on task/synchronize.aspx), this method does nothing.
		/// </summary>
		private void StoreReferringPage()
		{
			if (HttpContext.Current.Session == null)
				return; // Session is disabled for this page.

			Uri backURL = Page.Request.UrlReferrer;
			if (ShouldStoreReferringPage(backURL))
			{
				// User navigated to a new page. Update the session variable with the new previous page.
				HttpContext.Current.Session["ReferringUrl"] = backURL;
			}
		}

		/// <summary>
		/// Modify the URI if needed to reflect the desired page to navigate back to. This method removes the "msg" 
		/// query string parameter if it is present.
		/// </summary>
		/// <param name="uri">The referring URI to update.</param>
		/// <param name="currentMediaObjectId">The ID of the current media object.</param>
		/// <returns>Returns a modified URI if changes are needed; othewise returns the original URI.</returns>
		private static Uri UpdateReferringUrlBeforeSaving(Uri uri, int currentMediaObjectId)
		{
			// The "moid" query string parameter is not updated when the user navigates between media objects in an album via
			// AJAX. If the current media object ID is different than the value in the query string, then update the query
			// string value.
			string queryString = uri.Query;
			if (queryString.Contains("?moid=") || queryString.Contains("&moid="))
			{
				if (WebsiteController.GetQueryStringParameterInt32(uri, "moid") != currentMediaObjectId)
				{
					// The URI has a "moid" query string parm and it is different than the current media object ID. Update the URI.
					queryString = WebsiteController.RemoveQueryStringParameter(queryString, "moid");
					queryString = WebsiteController.AddQueryStringParameter(queryString, String.Format(CultureInfo.CurrentCulture, "moid={0}", currentMediaObjectId));

					UriBuilder uriBuilder = new UriBuilder(uri);
					uriBuilder.Query = queryString.TrimStart(new char[] { '?' });
					uri = uriBuilder.Uri;
				}
			}
			return uri;
		}

		/// <summary>
		/// Determine if the specified URI should be stored in the user's session for possible navigation back to. We only want to
		/// store the URI if the user navigated to a new page. Also, don't store the login page since the user probably does not
		/// want to return there.
		/// </summary>
		/// <param name="uri">The referring URI to the current page.</param>
		/// <returns>Returns true if the URI should be stored; otherwise returns false.</returns>
		private bool ShouldStoreReferringPage(Uri uri)
		{
			bool rv = false;
			if (uri != null)
			{
				bool isDefaultPage = (Page.Request.Url.AbsolutePath == WebsiteController.GetAppRootUrl());
				bool isLoginPage = (uri.AbsolutePath.IndexOf("login.aspx", StringComparison.OrdinalIgnoreCase) >= 0);

				if (!(isDefaultPage || isLoginPage))
				{
					if ((uri.AbsolutePath != Page.Request.Url.AbsolutePath) || ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["ReferringUrl"] == null)))
					{
						// Don't save if the referring url is the default page and it has the moid query string parm, because that
						// means the user was viewing a media object. Since AJAX callbacks can cause the displayed media object to
						// be different than the moid in the query string, we store the referring url in the GetMediaObjectHtml
						// callback method instead of doing it here.
						bool isReferrerDefaultPage = (uri.AbsolutePath == WebsiteController.GetAppRootUrl());
						bool doesReferrerHaveMoidQueryStringParm = (WebsiteController.GetQueryStringParameterInt32(uri, "moid") > int.MinValue);

						if (!(isReferrerDefaultPage && doesReferrerHaveMoidQueryStringParm))
						{
							rv = true; // Referring page WAS NOT the default page with a moid query string parm. Ok to save.
						}
					}
				}
			}
			return rv;
		}

		/// <summary>
		/// Set the public properties on this class related to user permissions. This method is called as needed from
		/// within the property getters.
		/// </summary>
		private void EvaluateUserPermissions()
		{
			bool isPhysicalAlbum = !this.GetAlbum().IsVirtualAlbum;

			// We need include isPhysicalAlbum in the expressions below because the IsUserAuthorized function uses
			// the album ID of the current album to evaluate the user's ability to perform the action. In the case
			// of a virtual album, the album ID is int.MinValue and the method is therefore not able to evaluate the permission.

			this.UserCanCreateAlbum = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.AddChildAlbum);
			this.UserCanEditAlbum = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.EditAlbum);
			this.UserCanAddMediaObject = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.AddMediaObject);
			this.UserCanEditMediaObject = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.EditMediaObject);
			this.UserCanAdministerSite = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.AdministerSite);
			this.UserCanDeleteCurrentAlbum = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.DeleteAlbum);
			this.UserCanDeleteMediaObject = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.DeleteMediaObject);
			this.UserCanSynchronize = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.Synchronize);
			this.UserCanViewHiResImage = isPhysicalAlbum && this.IsUserAuthorized(SecurityActions.ViewOriginalImage);

			foreach (IGalleryServerRole role in GetGalleryServerRolesForUser())
			{
				if (role.AllowAdministerSite)
				{
					this.UserCanAddMediaObjectToAtLeastOneAlbum = true;
					this.UserCanAddAlbumToAtLeastOneAlbum = true;
					break;
				}

				if (role.AllowAddMediaObject)
					this.UserCanAddMediaObjectToAtLeastOneAlbum = true;

				if (role.AllowAddChildAlbum)
					this.UserCanAddAlbumToAtLeastOneAlbum = true;
			}

			// If UserCanAddAlbumToAtLeastOneAlbum or UserCanAddMediaObjectToAtLeastOneAlbum havn't been set to
			// true by now, then user does not have permission to do those things. Set to false.
			if (!this._userCanAddAlbumToAtLeastOneAlbum.HasValue)
			{
				this.UserCanAddAlbumToAtLeastOneAlbum = false;
			}

			if (!this._userCanAddMediaObjectToAtLeastOneAlbum.HasValue)
			{
				this.UserCanAddMediaObjectToAtLeastOneAlbum = false;
			}
		}

		private string GetAlbumThumbnailUrl(IGalleryObject galleryObject)
		{
			// Get a reference to the path to the thumbnail. If the user is anonymous and the thumbnail is from a private
			// media object or album, then change the thumbnail to the DefaultFilename variable. This will be interpreted
			// by the image handler to generate a default, empty thumbnail image.
			string thumbnailPath = galleryObject.Thumbnail.FileNamePhysicalPath;
			if (this.IsAnonymousUser && (galleryObject.Thumbnail.MediaObjectId > 0))
			{
				try
				{
					IGalleryObject mediaObject = Factory.LoadMediaObjectInstance(galleryObject.Thumbnail.MediaObjectId);
					if (mediaObject.Parent.IsPrivate || mediaObject.IsPrivate)
					{
						thumbnailPath = GlobalConstants.DefaultFileName;
					}
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException)
				{
					// We'll get here if the ID for the thumbnail doesn't represent an existing media object.
					thumbnailPath = GlobalConstants.DefaultFileName;
				}
			}

			return MediaObjectHtmlBuilder.GenerateUrl(int.MinValue, galleryObject.Id, galleryObject.MimeType, thumbnailPath, DisplayObjectType.Thumbnail, galleryObject.IsPrivate);
		}

		#endregion
	}

}
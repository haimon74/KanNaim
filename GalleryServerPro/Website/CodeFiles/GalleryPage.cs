using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Controls;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Pages
{
	/// <summary>
	/// The base class user control used in Gallery Server Pro to represent page-like functionality.
	/// </summary>
	public abstract class GalleryPage : UserControl
	{
		#region Private Fields

		private Core _core;
		private readonly Util _util;
		private IAlbum _album;
		private Message _message = Message.None;
		private IGalleryServerRoleCollection _roles;
		private string _pageTitle = String.Empty;
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
		private Gallery _galleryControl;
		private Controls.albummenu _albumMenu;
		private Controls.galleryheader _galleryHeader;
		private int _currentPage;
		private bool? _isComponentArtCallback;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="GalleryPage"/> class.
		/// </summary>
		static GalleryPage()
		{
			if (!AppSetting.Instance.IsInitialized)
			{
				Util.InitializeApplication();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GalleryPage"/> class.
		/// </summary>
		protected GalleryPage()
		{
			// Ensure the app is initialized. This should have been done in the static constructor, but if anything went wrong
			// there, it may not be initialized, so we check again.
			if (!AppSetting.Instance.IsInitialized)
			{
				Util.InitializeApplication();
			}

			this._util = new Util();

			this.Init += GalleryPage_Init;
			//this.Load += this.GalleryPage_Load;
			//this.Unload += this.GalleryPage_Unload;
			//this.Error += this.GalleryPage_Error;
			this.PreRender += (GalleryPage_PreRender);
		}

		#endregion

		#region Protected Events

		/// <summary>
		/// Handles the Init event of the GalleryPage control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void GalleryPage_Init(object sender, System.EventArgs e)
		{
			if (RequiresLogin())
				Util.Redirect(PageId.login, true, "ReturnUrl={0}", Util.UrlEncode(Request.RawUrl));

			// Add a ScriptManager if the page doesn't already have one.
			ScriptManager sm = ScriptManager.GetCurrent(this.Page);
			if (sm == null)
			{
				sm = new ScriptManager();
				sm.EnableScriptGlobalization = true;
				this.Controls.AddAt(0, sm);
			}

			this.StoreCurrentPageUri();

			if (IsPostBack)
			{
				// Postback: Since the user may have been navigating several media objects in this album through AJAX calls, we need to check
				// a hidden field to discover the current media object. Assign this object's ID to our base user control. The base 
				// control is smart enough to retrieve the new media object if it is different than what was previously set.
				object formFieldMoid = Request.Form["moid"];
				int moid;
				if ((formFieldMoid != null) && (Int32.TryParse(formFieldMoid.ToString(), out moid)))
				{
					this.MediaObjectId = moid;
				}
			}

			if (!IsPostBack)
			{
				RegisterHiddenFields();
			}

			sm.Services.Add(new ServiceReference(Util.GetUrl("/services/Gallery.asmx")));

			// Add user controls to the page, such as the header and album breadcrumb menu.
			this.AddUserControls();
		}

		/// <summary>
		/// Handles the PreRender event of the GalleryPage control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void GalleryPage_PreRender(object sender, EventArgs e)
		{
			HtmlHead header = this.Page.Header;
			if (header == null)
				throw new WebException(Resources.GalleryServerPro.Error_Head_Tag_Missing_Server_Attribute_Ex_Msg);

			SetupHeadControl(header);
		}

		/// <summary>
		/// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			// Wrap HTML in an enclosing <div id="gsp_ns"> tag. This is used as a pseudo namespace that is used to limit the
			// influence CSS has to only the Gallery Server code, thus preventing the CSS from affecting HTML that may 
			// exist in the master page or other areas outside the user control.
			writer.AddAttribute("class", "gsp_ns"); // gsp_ns stands for Gallery Server Pro namespace
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// Write out the HTML for this control.
			base.Render(writer);

			// Add the GSP logo to the end
			AddGspLogo(writer);

			// Close out the <div> tag.
			writer.RenderEndTag();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a reference to the &lt;core...&gt; section of the galleryServerPro custom configuration section in galleryserverpro.config.
		/// A read-only instance is returned.
		/// </summary>
		/// <value>The &lt;core...&gt; section of the galleryServerPro custom configuration section in galleryserverpro.config.</value>
		public Core Core
		{
			get
			{
				if (this._core == null)
				{
					this._core = ConfigManager.GetGalleryServerProConfigSection().Core;
				}

				return this._core;
			}
		}

		/// <summary>
		/// Gets the album ID corresponding to the current album. Guaranteed to return a valid album ID. Returns the album ID
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
		/// exist, the value is retrieved from <see cref="Util.MediaObjectId" />, which attempts to retrieve it from a query string 
		/// parameter "moid". Returns int.MinValue if no valid media object ID is found. Setting this value stores it in a ViewState
		/// object named "moid" and assigns it to <see cref="Util.MediaObjectId" />, which causes the Util's private media 
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
					if (moid != this._util.MediaObjectId)
					{
						this._util.MediaObjectId = moid;
					}
					return moid;
				}
				else
				{
					return this._util.MediaObjectId;
				}
			}
			set
			{
				ViewState["moid"] = value;
				this._util.MediaObjectId = value;
			}
		}

		public int CurrentPage
		{
			get
			{
				if (this._currentPage == 0)
				{
					int page = Util.GetQueryStringParameterInt32("page");

					this._currentPage = (page > 0 ? page : 1);
				}

				return this._currentPage;
			}
			set
			{
				this._currentPage = value;

				if (HttpContext.Current.Session != null)
				{
					Uri backURL = this.PreviousUri;
					if (backURL != null)
					{
						// Update the page query string parameter so that the referring url points to the current page index.
						backURL = UpdateUriQueryString(backURL, "page", this._currentPage.ToString());
					}
					else
					{
						backURL = UpdateUriQueryString(HttpContext.Current.Request.Url, "page", this._currentPage.ToString());
					}
					this.PreviousUri = backURL;
				}

			}
		}

		/// <summary>
		/// Gets a value indicating whether the current user is anonymous. If the user has authenticated with a user name/password, 
		/// this property is false.
		/// </summary>
		public bool IsAnonymousUser
		{
			// Note: Do not store in a private field that lasts the lifetime of the page request, as this may give the wrong
			// value after logon and logoff events.
			get
			{
				return !Util.IsAuthenticated;
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
		/// and rearranging the order of objects within the album.
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
					int msgId = Util.GetQueryStringParameterInt32("msg");
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

		private PageId _pageId;
		/// <summary>
		/// Gets the value that identifies the type of gallery page that is currently being displayed. This value is 
		/// retrieved from the "g" query string parameter. If the parameter is not present, the query string is searched
		/// for a "moid" or "aid" parameter. If "moid" is found, the page is <see cref="Web.PageId.mediaobject"/>. If
		/// "aid" is found, the page is <see cref="Web.PageId.album"/>. If both "aid" and "moid" are present, the page
		/// is <see cref="Web.PageId.mediaobject"/>.
		/// </summary>
		/// <value>The value that identifies the type of gallery page that is currently being displayed.</value>
		public PageId PageId
		{
			get
			{
				if (this._pageId == 0)
					this._pageId = Util.GetPage();
				
				return this._pageId;
			}
		}

		/// <summary>
		/// Gets or sets the instance of the user control that created this user control.
		/// </summary>
		/// <value>The user control that created this user control.</value>
		public Gallery GalleryControl
		{
			get
			{
				if (_galleryControl != null)
					return _galleryControl;

				System.Web.UI.Control ctl = Parent;
				while (ctl.GetType() != typeof(Gallery))
					ctl = ctl.Parent;

				_galleryControl = (Gallery)ctl;
				return _galleryControl;
			}
			set
			{
				_galleryControl = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that can be used in the title tag in the HTML page header. If this property is not set by the user
		/// control, the current album's title is used.
		/// </summary>
		/// <value>A value that can be used in the title tag in the HTML page header.</value>
		public virtual string PageTitle
		{
			get
			{
				if (String.IsNullOrEmpty(_pageTitle))
				{
					// Get an HTML-cleaned version of the current album's title, limited to the first 50 characters.
					string title = Util.RemoveHtmlTags(GetAlbum().Title);
					title = title.Substring(0, title.Length < 50 ? title.Length : 50);

					return String.Concat(Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text, " ", title);
				}
				else
					return _pageTitle;
			}
			set
			{
				this._pageTitle = value;
			}
		}

		/// <summary>
		/// Gets a reference to the <see cref="albummenu"/> control on the page.
		/// </summary>
		/// <value>The <see cref="albummenu"/> control on the page.</value>
		public Controls.albummenu AlbumMenu
		{
			get
			{
				return this._albumMenu;
			}
		}

		/// <summary>
		/// Gets a reference to the <see cref="galleryheader"/> control on the page.
		/// </summary>
		/// <value>The <see cref="galleryheader"/> control on the page.</value>
		public Controls.galleryheader GalleryHeader
		{
			get
			{
				return this._galleryHeader;
			}
		}

		/// <summary>
		/// Gets or sets the URI of the previous page the user was viewing. The value is stored in the user's session, and 
		/// can be used after a user has completed a task to return to the original page. If the Session object is not available,
		/// no value is saved in the setter and a null is returned in the getter.
		/// </summary>
		/// <value>The URI of the previous page the user was viewing.</value>
		public Uri PreviousUri
		{
			get
			{
				return Util.PreviousUri;
			}
			set
			{
				Util.PreviousUri = value;
			}
		}

		/// <summary>
		/// Gets the URL of the previous page the user was viewing. The value is based on the PreviousUri property. If that
		/// property is null, such as when the Session object is not available or it has never been assigned, return 
		/// String.Empty. Remove the query string parameter "msg" if present.
		/// </summary>
		/// <value>The URL of the previous page the user was viewing.</value>
		public string PreviousUrl
		{
			get
			{
				if (PreviousUri != null)
					return Util.RemoveQueryStringParameter(PreviousUri.PathAndQuery, "msg");
				else
					return String.Empty;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the page request is a result of a callback. This is determined by checking 
		/// <see cref="Page.IsCallback"/> and <see cref="IsCallbackCausedByComponentArt"/>. If either is true, then return
		/// true; otherwise return false.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the page request is a result of a callback; otherwise, <c>false</c>.
		/// </value>
		public bool IsCallback
		{
			get
			{
				return (base.Page.IsCallback || IsCallbackCausedByComponentArt);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the page request is a result of a callback initiated by the ComponentArt
		/// Callback control. This property is required because the native <see cref="Page.IsCallback"/> does
		/// not return true during a ComponentArt callback.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the page request is a result of a callback initiated by the ComponentArt Callback control; 
		/// otherwise, <c>false</c>.
		/// </value>
		public bool IsCallbackCausedByComponentArt
		{
			get
			{
				if (!this._isComponentArtCallback.HasValue)
				{
					this._isComponentArtCallback = false;

					for (int i = 0; i < Request.Form.Count; i++)
					{
						if (!String.IsNullOrEmpty(Request.Form.Keys[i]) && Request.Form.Keys[i].IndexOf("_Callback_Param") > -1)
						{
							this._isComponentArtCallback = true;
							break;
						}
					}
				}

				return this._isComponentArtCallback.Value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is a new page load. That is, the page is not a post back and not 
		/// involved in any AJAX callbacks.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is a new page load; otherwise, <c>false</c>.
		/// </value>
		public bool IsNewPageLoad
		{
			get
			{
				return (!(IsPostBack) && (!this.Page.IsCallback) && !(IsCallbackCausedByComponentArt));
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current user is an administrator.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the current user is an administrator; otherwise, <c>false</c>.
		/// </value>
		public static bool UserIsAdministrator
		{
			get
			{
				foreach (IGalleryServerRole role in RoleController.GetGalleryServerRolesForUser())
				{
					if (role.AllowAdministerSite)
						return true;
				}
				return false;
			}
		}

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs just before the gallery header and album breadcrumb menu controls are added to the control collection. This event is an
		/// opportunity for inheritors to insert controls of their own at the zero position using the Controls.AddAt(0, myControl) method.
		/// Viewstate is lost if inheritors add controls at any index other than 0, so the way to deal with this is to use this 
		/// event handler to add controls. For example, the Site Settings admin menu is added in the event handler in the <see cref="AdminPage"/> class.
		/// </summary>
		protected event EventHandler BeforeHeaderControlsAdded;

		#endregion

		#region Public Methods

		/// <summary>
		/// Create a fully inflated album instance for the album containing the media object ID or album ID specified 
		/// in the query string (moid or aid). If the requested album doesn't exist, the user is redirected to the album
		/// view page, which will show the top level album that the user can view. If the requested album exists, but 
		/// the user doesn't have permission to view it, get the top level album which the user does have access to view.
		/// It may be a virtual album. Album properties are retrieved from the data store. If this album contains child 
		/// objects, they are added but not inflated. An automatic security check is performed to make sure the user has view
		/// permission for the specified album. Guaranteed to never return null.
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
					tempAlbum = this._util.GetAlbum();
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidAlbumException)
				{
					// The 'aid' query string parm refers to an album that doesn't exist. Redirect to home page and pass along
					// a message to inform the user.
					Util.Redirect(Util.AddQueryStringParameter(Util.GetCurrentPageUrl(), "msg=" + (int)Message.AlbumDoesNotExist));
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
						// User does not have permission to view the album. Redirect to home page.
						Util.Redirect(PageId.album);

						// Use the following instead if you want to pass along a message to inform the user why the requested album is unavailable.
						//string redirectUrl = Util.AddQueryStringParameter(Util.GetCurrentPageUrl(), "msg=" + (int)Message.InsufficientPermissionCannotViewAlbum);
						//System.Web.HttpContext.Current.Response.Redirect(redirectUrl, true);  // Use 'true' to force immediate end to page processing
					}
				}

				// Step 3: If we got to this point, no album or media object is specified on the query string. Load the top
				// level albums for which the current user has permission to view.
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
						// User is logged on. Get list of top level albums user has permission to view.
						this._album = Factory.LoadRootAlbum(SecurityActions.ViewAlbumOrMediaObject | SecurityActions.ViewOriginalImage, this.GetGalleryServerRolesForUser());

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
				if (moid != this._util.MediaObjectId)
				{
					this._util.MediaObjectId = moid;
				}
			}

			return this._util.GetMediaObject();
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
				default: throw new ArgumentOutOfRangeException(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Error_GetMediaObjectUrl_Ex_Msg1, displayType.ToString()));
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
			return Util.HtmlEncode(Util.RemoveHtmlTags(textWithHtml));
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
			if (!Util.IsUserAuthorized(securityActions, GetGalleryServerRolesForUser(), this.AlbumId, this.GetAlbum().IsPrivate))
			{
				if (this.IsAnonymousUser)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "Anonymous user does not have permission '{0}' for album ID {1}.", securityActions.ToString(), this.AlbumId));
				}
				else
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "User '{0}' does not have permission '{1}' for album ID {2}.", Util.UserName, securityActions.ToString(), this.AlbumId));
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
			if (!Util.IsUserAuthorized(securityActions, GetGalleryServerRolesForUser(), album.Id, album.IsPrivate))
			{
				if (this.IsAnonymousUser)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "Anonymous user does not have permission '{0}' for album ID {1}.", securityActions.ToString(), album.Id));
				}
				else
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "User '{0}' does not have permission '{1}' for album ID {2}.", Util.UserName, securityActions.ToString(), album.Id));
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
			return Util.IsUserAuthorized(securityAction, GetGalleryServerRolesForUser(), this.AlbumId, this.GetAlbum().IsPrivate);
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
			return Util.IsUserAuthorized(securityAction, GetGalleryServerRolesForUser(), album.Id, album.IsPrivate);
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
				this._roles = RoleController.GetGalleryServerRolesForUser();
			}

			return this._roles;
		}

		/// <summary>
		/// Redirect the user to the previous page he or she was on. If a query string name/pair value is specified, append that 
		/// to the URL. The previous page is retrieved from a session variable that was stored during 
		/// the Page_Init event. If the original query string contains a "msg" parameter, it is removed so that the message 
		/// is not shown again to the user. If no previous page URL is available - perhaps because the user navigated directly to
		/// the page or has just logged in - the user is redirected to the application root.
		/// </summary>
		public void RedirectToPreviousPage()
		{
			RedirectToPreviousPage(String.Empty, String.Empty);
		}

		public void RedirectToPreviousPage(string queryStringName, string queryStringValue)
		{
			#region Validation

			if (!String.IsNullOrEmpty(queryStringName) && String.IsNullOrEmpty(queryStringValue))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringValue parameter is required when the queryStringName parameter is specified. (queryStringName='{0}', queryStringValue='{1}')", queryStringName, queryStringValue));

			if (!String.IsNullOrEmpty(queryStringValue) && String.IsNullOrEmpty(queryStringName))
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The queryStringName parameter is required when the queryStringValue parameter is specified. (queryStringName='{0}', queryStringValue='{1}')", queryStringName, queryStringValue));

			#endregion

			string url = this.PreviousUrl;

			if (String.IsNullOrEmpty(url))
				url = Util.GetCurrentPageUrl(); // No previous url is available. Default to the current page.
			
			if (!String.IsNullOrEmpty(queryStringName))
				url = Util.AddQueryStringParameter(url, String.Concat(queryStringName, "=", queryStringValue));

			this.PreviousUri = null;

			Page.Response.Redirect(url, true);
		}

		/// <overloads>Redirects to album view page of the current album.</overloads>
		/// <summary>
		/// Redirects to album view page of the current album.
		/// </summary>
		public void RedirectToAlbumViewPage()
		{
			Util.Redirect(PageId.album, "aid={0}", AlbumId);
		}

		/// <summary>
		/// Redirects to album view page of the current album and with the specified <paramref name="args"/> appended as query string 
		/// parameters. Example: If the current page is /dev/gs/gallery.aspx, the user is viewing album 218, <paramref name="format"/> 
		/// is "msg={0}", and <paramref name="args"/> is "23", this function redirects to /dev/gs/gallery.aspx?g=album&amp;aid=218&amp;msg=23.
		/// </summary>
		/// <param name="format">A format string whose placeholders are replaced by values in <paramref name="args"/>. Do not use a '?'
		/// or '&amp;' at the beginning of the format string. Example: "msg={0}".</param>
		/// <param name="args">The values to be inserted into the <paramref name="format"/> string.</param>
		public void RedirectToAlbumViewPage(string format, params object[] args)
		{
			if (format.StartsWith("?"))
				format.Remove(0, 1); // Remove leading '?' if present

			string queryString = String.Format(format, args);
			if (!queryString.StartsWith("&"))
				queryString = String.Concat("&", queryString); // Append leading '&' if not present

			Util.Redirect(PageId.album, String.Concat("aid={0}", queryString), AlbumId);
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
			if (containingControl == null)
				throw new ArgumentNullException("containingControl");

			if (String.IsNullOrEmpty(id))
				throw new ArgumentException("The parameter 'id' is null or empty.");

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
			if ((maxMoWidth == 0) || (maxMoHeight == 0))
			{
				int maxLength = Core.MaxThumbnailLength;
				float ratio = Core.EmptyAlbumThumbnailWidthToHeightRatio;
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

			int maxWidth = maxMoWidth + Core.ThumbnailWidthBuffer + widthBuffer;
			int maxHeight = maxMoHeight + Core.ThumbnailHeightBuffer + heightBuffer;

			string pageStyle = "\n<style type=\"text/css\"><!-- ";
			foreach (string cssClass in thumbnailCssClasses)
			{
				pageStyle += String.Format(CultureInfo.CurrentCulture, "div.{0} {{width:{1}px;height:{2}px;}} ", cssClass, maxWidth, maxHeight);
			}
			pageStyle += "--></style>\n";

			this.Page.Header.Controls.Add(new System.Web.UI.LiteralControl(pageStyle));
		}

		/// <summary>
		/// Gets an instance of he usermessage.ascx user control that is formatted and pre-configured with a message for the user.
		/// The message is based on the <see cref="Message"/> property. The control can be added to the control collection of the
		/// page, typically a PlaceHolder contro.
		/// </summary>
		/// <returns>Returns an instance of he usermessage.ascx user control that is formatted and pre-configured with a message 
		/// for the user.</returns>
		public usermessage GetMessageControl()
		{
			const string resourcePrefix = "Msg_";
			const string headerSuffix = "_Hdr";
			const string detailSuffix = "_Dtl";

			string headerMsg = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), headerSuffix));
			string detailMsg = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), detailSuffix));

			switch (this.Message)
			{
				case Message.CaptionExceededMaxLength:
				case Message.OneOrMoreCaptionsExceededMaxLength:
					{
						int maxLength = Config.GetDataStore().MediaObjectTitleLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxLength);
						break;
					}
				case Message.AlbumSummaryExceededMaxLength:
					{
						int maxLength = Config.GetDataStore().AlbumSummaryLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxLength);
						break;
					}
				case Message.AlbumNameExceededMaxLength:
					{
						int maxLength = Config.GetDataStore().AlbumTitleLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxLength);
						break;
					}
				case Message.AlbumNameAndSummaryExceededMaxLength:
					{
						int maxTitleLength = Config.GetDataStore().AlbumTitleLength;
						int maxSummaryLength = Config.GetDataStore().AlbumSummaryLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxTitleLength, maxSummaryLength);
						break;
					}
				case Message.ObjectsSkippedDuringUpload:
					{
						List<KeyValuePair<string, string>> skippedFiles = Session[GlobalConstants.SkippedFilesDuringUploadSessionKey] as List<KeyValuePair<string, string>>;

						detailMsg = string.Empty;
						if (skippedFiles != null)
						{
							// This message is unique in that we need to choose one of two detail messages from the resource file. One is for when a single
							// file has been skipped; the other is when multiple files have been skipped.
							if (skippedFiles.Count == 1)
							{
								string detailMsgTemplate = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), "Single", detailSuffix));
								detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsgTemplate, skippedFiles[0].Key, skippedFiles[0].Value);
							}
							else if (skippedFiles.Count > 1)
							{
								string detailMsgTemplate = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), "Multiple", detailSuffix));
								detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsgTemplate, ConvertListToHtmlBullets(skippedFiles));
							}
						}
						break;
					}
			}
			GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
			msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Information;
			msgBox.MessageTitle = headerMsg;
			msgBox.MessageDetail = detailMsg;
			msgBox.CssClass = "um2ContainerCss";
			msgBox.HeaderCssClass = "um2HeaderCss";
			msgBox.DetailCssClass = "um2DetailCss";

			return msgBox;
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

			Array browsers = HttpContext.Current.Request.Browser.Browsers.ToArray();

			// Perf improvement: The code that uses the browsers variable needs it ordered such that the most specific 
			// browser id is first and the most general (id="Default") is last. Reverse order if needed. For robustness the code 
			// that requires this still does the check (GalleryServerPro.Configuration.BrowserCollection.FindClosestMatchById),
			// but since we do it once it here it doesn't have to do it several times (once for most of the steps below).
			lock (browsers)
			{
				if ((browsers.Length > 0) && (browsers.GetValue(0).ToString().Equals("default", StringComparison.OrdinalIgnoreCase)))
				{
					Array.Reverse(browsers);
				}
			}

			if ((displayType == DisplayObjectType.Original) && (!Util.IsUserAuthorized(SecurityActions.ViewOriginalImage, RoleController.GetGalleryServerRolesForUser(), mediaObject.Parent.Id, ((IAlbum)mediaObject.Parent).IsPrivate)))
			{
				displayType = DisplayObjectType.Optimized;
			}

			bool excludePrivateObjects = !Util.IsAuthenticated;

			MediaObjectWebEntity mo = new MediaObjectWebEntity();

			#region Step 1: Process current media object

			if (mediaObject.Id > 0)
			{
				// This section is enclosed in the above if statement to force all declared variables within it to be local so they are
				// not accidentally re-used in steps 2 or 3. In reality, mediaObject.Id should ALWAYS be greater than 0.
				IDisplayObject displayObject = GalleryObjectController.GetDisplayObject(mediaObject, displayType);

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
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Get the siblings of this media object and the index that specifies its position within its siblings.

				//TODO: This technique for identifying the index is very expensive when there are a lot of objects in the album.
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

				IDisplayObject displayObject = GalleryObjectController.GetDisplayObject(prevMO, displayType);

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
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Build up the entity object we'll be sending to the client.
				bool prevMoIsImage = (prevMO is GalleryServerPro.Business.Image);
				bool prevMoIsExternalObject = (prevMO is GalleryServerPro.Business.ExternalMediaObject);
				mo.PrevTitle = prevMO.Title;
				mo.PrevHtmlOutput = htmlOutput;
				mo.PrevScriptOutput = scriptOutput;
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

				IDisplayObject displayObject = GalleryObjectController.GetDisplayObject(nextMO, displayType);

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
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Build up the entity object we'll be sending to the client.
				bool nextMoIsImage = (nextMO is GalleryServerPro.Business.Image);
				bool nextMoIsExternalObject = (nextMO is GalleryServerPro.Business.ExternalMediaObject);
				mo.NextTitle = nextMO.Title;
				mo.NextHtmlOutput = htmlOutput;
				mo.NextScriptOutput = scriptOutput;
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

				IDisplayObject displayObject = GalleryObjectController.GetDisplayObject(nextSSMO, displayType);

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
					htmlOutput = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_MediaObjectView_Browser_Cannot_Display_Media_Object_Text);
				}

				// Get the siblings of this media object and the index that specifies its position within its siblings.

				//TODO: This technique for identifying the index is very expensive when there are a lot of objects in the album.
				IGalleryObjectCollection siblings = ((IAlbum)nextSSMO.Parent).GetChildGalleryObjects(GalleryObjectType.MediaObject, true, excludePrivateObjects);
				int mediaObjectIndex = siblings.IndexOf(nextSSMO);

				// Build up the entity object we'll be sending to the client.
				bool nextSSMoIsImage = (nextSSMO is GalleryServerPro.Business.Image);
				mo.NextSSIndex = mediaObjectIndex;
				mo.NextSSTitle = nextSSMO.Title;
				mo.NextSSUrl = url;
				mo.NextSSHtmlOutput = htmlOutput;
				mo.NextSSScriptOutput = scriptOutput;
				mo.NextSSWidth = displayObject.Width;
				mo.NextSSHeight = displayObject.Height;
				mo.NextSSHiResAvailable = (nextSSMoIsImage && (!String.IsNullOrEmpty(nextSSMO.Optimized.FileName)) && (nextSSMO.Original.FileName != nextSSMO.Optimized.FileName));
				mo.NextSSIsDownloadable = true; // Slide show objects are always locally stored images and are therefore always downloadable
			}

			#endregion

			#region Step 5: Update Previous Uri variable

			if (HttpContext.Current.Session != null)
			{
				Uri backURL = Util.PreviousUri;
				if (isCallBack && (backURL != null))
				{
					// We are in a callback. Even though the page hasn't changed, the user is probably viewing a different media object,
					// so update the moid query string parameter so that the referring url points to the current media object.
					backURL = UpdateUriQueryString(backURL, "moid", mediaObject.Id.ToString());
				}
				else
				{
					backURL = HttpContext.Current.Request.Url;
				}

				Util.PreviousUri = backURL;
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
			    && (!Util.IsAuthenticated))
				throw new System.NotSupportedException("Wrong method call: You must call the overload of GspPage.IsUserAuthorized that has the isPrivate parameter when the security action is ViewAlbumOrMediaObject or ViewOriginalImage and the user is anonymous (not logged on).");

			return IsUserAuthorized(securityActions, RoleController.GetGalleryServerRolesForUser(), albumId, false);
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
			return IsUserAuthorized(securityActions, RoleController.GetGalleryServerRolesForUser(), albumId, isPrivate);
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
			    && (!Util.IsAuthenticated))
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
			return Util.IsUserAuthorized(securityActions, roles, albumId, isPrivate);
		}

		#endregion

		#region Private Static Methods

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

		private static HtmlLink MakeStyleSheetControl(string href)
		{
			HtmlLink stylesheet = new HtmlLink();
			stylesheet.Href = href;
			stylesheet.Attributes.Add("rel", "stylesheet");
			stylesheet.Attributes.Add("type", "text/css");

			return stylesheet;
		}

		/// <summary>
		/// Updates the query string parameter in the <paramref name="uri"/> with the specified value. If the 
		/// <paramref name="queryStringName"/> is not present, it is added. The modified URI is returned. The <paramref name="uri"/>
		/// is not modified.
		/// </summary>
		/// <param name="uri">The URI that is to receive the updated or added query string <paramref name="queryStringName">name</paramref>
		/// and <paramref name="queryStringValue">value</paramref>. This object is not modified; rather, a new URI is created
		/// and returned.</param>
		/// <param name="queryStringName">Name of the query string to include in the URI.</param>
		/// <param name="queryStringValue">The query string value to include in the URI.</param>
		/// <returns>Returns the uri with the specified query string name and value updated or added.</returns>
		private static Uri UpdateUriQueryString(Uri uri, string queryStringName, string queryStringValue)
		{
			Uri updatedUri = null;
			string newQueryString = uri.Query;

			if (Util.IsQueryStringParameterPresent(uri, queryStringName))
			{
				if (Util.GetQueryStringParameterString(uri, queryStringName) != queryStringValue)
				{
					// The URI has the query string parm and it is different than the value. Update the URI.
					newQueryString = Util.RemoveQueryStringParameter(newQueryString, queryStringName);
					newQueryString = Util.AddQueryStringParameter(newQueryString, String.Format(CultureInfo.CurrentCulture, "{0}={1}", queryStringName, queryStringValue));

					UriBuilder uriBuilder = new UriBuilder(uri);
					uriBuilder.Query = newQueryString.TrimStart(new char[] { '?' });
					updatedUri = uriBuilder.Uri;
				}
				//else {} // Query string is present and already has the requested value. Do nothing.
			}
			else
			{
				// Query string parm not present. Add it.
				newQueryString = Util.AddQueryStringParameter(newQueryString, String.Format(CultureInfo.CurrentCulture, "{0}={1}", queryStringName, queryStringValue));

				UriBuilder uriBuilder = new UriBuilder(uri);
				uriBuilder.Query = newQueryString.TrimStart(new char[] { '?' });
				updatedUri = uriBuilder.Uri;
			}
			return updatedUri ?? uri;
		}

		private static string ConvertListToHtmlBullets(IEnumerable<KeyValuePair<string, string>> skippedFiles)
		{
			string html = "<ul>";
			foreach (KeyValuePair<string, string> kvp in skippedFiles)
			{
				html += String.Format(CultureInfo.CurrentCulture, "<li>{0}: {1}</li>", kvp.Key, kvp.Value);
			}
			html += "</ul>";

			return html;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Determines whether the current user must be logged in to access the requested page.
		/// </summary>
		/// <returns>Returns <c>true</c> if the user must be logged in to access the requested page; otherwise
		/// returns <c>false</c>.</returns>
		private bool RequiresLogin()
		{
			if ((this.PageId == PageId.login) || (this.PageId == PageId.createaccount) || (this.PageId == PageId.recoverpassword))
				return false; // The login, create account, & recover password pages never require one to be logged in

			if (!this.IsAnonymousUser)
				return false; // Already logged in

			if (!Core.AllowAnonymousBrowsing)
				return true; // Not logged in, anonymous browsing disabled

			// Some pages allow anonymous browsing. If it is one of those, return false; otherwise return true;
			switch (this.PageId)
			{
				//case PageId.createaccount:
				//case PageId.login:
				//case PageId.recoverpassword: // These 3 are redundent because we already handle them above
				case PageId.album:
				case PageId.mediaobject:
				case PageId.search:
				case PageId.task_downloadobjects:
					return false;
				default:
					return true;
			}
		}

		private void AddAlbumMenu()
		{
			Controls.albummenu albumMenu = (Controls.albummenu)LoadControl(Util.GetUrl("/controls/albummenu.ascx"));
			this._albumMenu = albumMenu;
			this.Controls.AddAt(0, albumMenu);
		}

		private void AddGalleryHeader()
		{
			Controls.galleryheader header = (Controls.galleryheader)LoadControl(Util.GetUrl("/controls/galleryheader.ascx"));
			this._galleryHeader = header;
			this.Controls.AddAt(0, header);
		}

		/// <summary>
		/// Stores the URI of the current album or media object page so that we can return to it later, if desired. This
		/// method store the current URI only for fresh page loads (no postbacks or callbacks) and when the current page
		/// is displaying an album view or media object. Other pages, such as task or admin pages, 
		/// are not stored since we do not want to return to them. This method assigns the current URI to the 
		/// <see cref="PreviousUri"/> property. After assigning this property, one can use 
		/// <see cref="RedirectToPreviousPage()"/> to navigate to the page. If session state is disabled, this method does nothing.
		/// </summary>		
		private void StoreCurrentPageUri()
		{
			if (IsNewPageLoad)
			{
				if ((this.PageId == PageId.album) || (this.PageId == PageId.mediaobject))
					this.PreviousUri = this.Page.Request.Url;
			}
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

		private void RegisterHiddenFields()
		{
			ScriptManager.RegisterHiddenField(this, "moid", this.MediaObjectId.ToString(CultureInfo.InvariantCulture));
			ScriptManager.RegisterHiddenField(this, "aid", this.AlbumId.ToString(CultureInfo.InvariantCulture));
		}

		private void AddUserControls()
		{
			// If any inheritors subscribed to the event, fire it.
			if (BeforeHeaderControlsAdded != null)
			{
				BeforeHeaderControlsAdded(this, new EventArgs());
			}

			if (PageId != PageId.login)
				this.AddAlbumMenu();

			this.AddGalleryHeader();
		}

		/// <summary>
		/// Write out the Gallery Server Pro logo to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		private void AddGspLogo(HtmlTextWriter writer)
		{
			// This function writes out HTML like this:
			// <div class="gsp_addtopmargin5 gsp_footer">
			//  <a href="http://www.galleryserverpro.com" title="Powered by Gallery Server Pro v2.1.3222">
			//   <img src="/images/gsp_ftr_logo_170x46.png" alt="Powered by Gallery Server Pro v2.1.3222" style="width:170px;height:46px;" />
			//  </a>
			// </div>
			string tooltip = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Logo_Tooltip, Util.GetGalleryServerVersion());
			//string url = Page.ClientScript.GetWebResourceUrl(typeof(footer), "GalleryServerPro.Web.gs.images.gsp_ftr_logo_170x46.png");

			// Create <div> tag that wraps the <a> and <img> tags.<div id="gs_footer">
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "gsp_addtopmargin5 gsp_footer");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// Create <a> tag that wraps <img> tag.
			writer.AddAttribute(HtmlTextWriterAttribute.Title, tooltip);
			writer.AddAttribute(HtmlTextWriterAttribute.Href, "http://www.galleryserverpro.com");
			writer.RenderBeginTag(HtmlTextWriterTag.A);

			// Create <img> tag.
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "170px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "46px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "middle");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ClientScript.GetWebResourceUrl(this.GetType().BaseType, "GalleryServerPro.Web.gs.images.gsp_ftr_logo_170x46.png"));
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, tooltip);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			// Close out the <a> tag.
			writer.RenderEndTag();

			// Close out the <div> tag.
			writer.RenderEndTag();
		}

		private void SetupHeadControl(HtmlHead head)
		{
			if (String.IsNullOrEmpty(head.Title))
				head.Title = PageTitle;

			// Add CSS links to the header.
			head.Controls.Add(MakeStyleSheetControl(Util.GetUrl("/styles/gallery.css")));
			head.Controls.Add(MakeStyleSheetControl(Util.GetUrl("/styles/ca_styles.css")));
		}

		#endregion

	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using PagerPosition = GalleryServerPro.Business.PagerPosition;

namespace GalleryServerPro.Web.Controls
{
	public partial class thumbnailview : GalleryUserControl
	{
		#region Private Fields

		private int? _pageSize;
		private bool? _viewOriginal;
		private bool? _pagingEnabled;
		private IGalleryObjectCollection _galleryObjectsDataSource;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets data that can be retrieved during a callback by the CallBack control. The value is HTML encoded and
		/// stored in a hidden field in the browser.
		/// </summary>
		/// <value>A string that can be retrieved during a callback by the CallBack control.</value>
		public string CallBackData
		{
			get
			{
				return Util.HtmlDecode(hfCallbackData.Value);
			}
			set
			{
				hfCallbackData.Value = Util.HtmlEncode(value);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the application is configured to display the original image by default.
		/// That is, thumbnailClickShowsOriginal="true" in galleryserverpro.config.
		/// </summary>
		/// <value><c>true</c> if the original image is to be displayed by default; otherwise, <c>false</c>.</value>
		public bool ViewOriginal
		{
			get
			{
				if (!this._viewOriginal.HasValue)
				{
					this._viewOriginal = this.GalleryPage.Core.ThumbnailClickShowsOriginal;
				}

				return this._viewOriginal.Value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether paging is enabled and the current number of objects is greater than page size.
		/// </summary>
		/// <value><c>true</c> if paging is enabled and the current number of objects is greater than page size;
		/// otherwise, <c>false</c>.</value>
		public bool PagingEnabled
		{
			get
			{
				if (!this._pagingEnabled.HasValue)
				{
					int pageSize = this.GalleryPage.Core.PageSize;
					int objectCount = (this.GalleryObjectsDataSource ?? this.GalleryPage.GetAlbum().GetChildGalleryObjects(false, this.GalleryPage.IsAnonymousUser)).Count;

					this._pagingEnabled = ((pageSize > 0) && (objectCount > pageSize));
				}

				return this._pagingEnabled.Value;
			}
		}

		/// <summary>
		/// Gets the page size as specified in the configuration file.
		/// </summary>
		/// <value>The size of the page.</value>
		public int PageSize
		{
			get
			{
				if (!this._pageSize.HasValue)
				{
					this._pageSize = this.GalleryPage.Core.PageSize;
				}

				return this._pageSize.Value;
			}
		}

		/// <summary>
		/// Gets or sets the media objects to be displayed as thumbnail items. When this property is not specified, 
		/// the gallery items within the current album are displayed.
		/// </summary>
		/// <value>The gallery objects data source.</value>
		public IGalleryObjectCollection GalleryObjectsDataSource
		{
			get
			{
				return this._galleryObjectsDataSource;
			}
			set
			{
				this._galleryObjectsDataSource = value;
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			//if (this.GalleryPage.IsNewPageLoad)
			//{
			//  RegisterJavascript();
			//}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (!cbThumbnailView.IsCallback)
				BindData();
		}

		protected void cbThumbnailView_Callback(object sender, CallBackEventArgs e)
		{
			this.GalleryPage.CurrentPage = Int32.Parse(e.Parameter);

			BindData();

			phPagerTop.RenderControl(e.Output);
			rptr.RenderControl(e.Output);
			phPagerBtm.RenderControl(e.Output);
			hfCallbackData.RenderControl(e.Output);
		}

		#endregion

		#region Protected Methods

		protected static string GetThumbnailCssClass(Type galleryObjectType)
		{
			// If it's an album then specify the appropriate CSS class so that the "Album"
			// header appears over the thumbnail. This is to indicate to the user that the
			// thumbnail represents an album.
			if (galleryObjectType == typeof(Album))
				return "thmb album";
			else
				return "thmb";
		}

		/// <summary>Return a string representing the title of the album. It is truncated and purged of
		/// HTML tags if necessary.  Returns an empty string if the gallery object is not an album 
		/// (<paramref name="galleryObjectType"/> != typeof(<see cref="Album"/>))
		/// </summary>
		/// <param name="title">The title of the album as stored in the data store.</param>
		/// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
		/// <returns>Returns a string representing the title of the album. It is truncated (if necessary) 
		/// and purged of HTML tags.</returns>
		protected static string GetAlbumText(string title, Type galleryObjectType)
		{
			if (galleryObjectType != typeof(Album))
				return String.Empty;

			int maxLength = Config.GetCore().MaxAlbumThumbnailTitleDisplayLength;
			string truncatedText = Util.TruncateTextForWeb(title, maxLength);
			
			if (truncatedText.Length != title.Length)
				return string.Format(CultureInfo.CurrentCulture, "<p class=\"albumtitle\"><b>{0}</b> {1}...</p>", Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text, truncatedText);
			else
				return string.Format(CultureInfo.CurrentCulture, "<p class=\"albumtitle\"><b>{0}</b> {1}</p>", Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text, truncatedText);
		}

		/// <summary>
		/// Return a string representing the title of the media object. It is truncated and purged of HTML tags
		/// if necessary. Returns an empty string if the gallery object is an album (<paramref name="galleryObjectType"/>
		/// == typeof(<see cref="Album"/>))
		/// </summary>
		/// <param name="title">The title of the media object as stored in the data store.</param>
		/// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
		/// <returns>Returns a string representing the title of the media object. It is truncated and purged of HTML tags
		/// if necessary.</returns>
		protected static string GetGalleryObjectText(string title, Type galleryObjectType)
		{
			// If this is an album, return an empty string. Otherwise, return the title, truncated and purged of HTML
			// tags if necessary. If the title is truncated, add an ellipses to the text.
			if (galleryObjectType == typeof(Album))
				return String.Empty;

			int maxLength = Config.GetCore().MaxMediaObjectThumbnailTitleDisplayLength;
			string truncatedText = Util.TruncateTextForWeb(title, maxLength);
			
			if (truncatedText.Length != title.Length)
				return string.Format(CultureInfo.CurrentCulture, "<p>{0}...</p>", truncatedText);
			else
				return string.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", truncatedText);
		}

		protected string GetHovertip(IGalleryObject galleryObject)
		{
			// Return the text to be used as the hovertip in standards compliant browsers. This is the
			// summary text for albums, and the title text for objects.
			string hoverTip = galleryObject.Title;

			IAlbum album = galleryObject as IAlbum;
			if (album != null)
			{
				if (album.Summary.Trim().Length > 0)
					hoverTip = album.Summary;
			}

			string hoverTipClean = Util.HtmlEncode(Util.RemoveHtmlTags(hoverTip));

			return hoverTipClean;
		}

		protected string GenerateUrl(IGalleryObject galleryObject)
		{
			string rv;

			if (galleryObject is Album)
			{
				// We have an album.
				rv = String.Concat(Util.GetUrl(PageId.album, "aid={0}", galleryObject.Id));
			}
			else if (galleryObject is GalleryServerPro.Business.Image)
			{
				// We have an image.
				if (ViewOriginal)
					rv = String.Concat(Util.GetUrl(PageId.mediaobject, "moid={0}&hr=1", galleryObject.Id));
				else
					rv = String.Concat(Util.GetUrl(PageId.mediaobject, "moid={0}", galleryObject.Id));
			}
			else
			{
				rv = String.Concat(Util.GetUrl(PageId.mediaobject, "moid={0}", galleryObject.Id));
			}

			if (String.IsNullOrEmpty(rv))
				throw new WebException("Unsupported media object type: " + galleryObject.GetType());

			return rv;
		}

		protected string GetThumbnailUrl(IGalleryObject galleryObject)
		{
			return this.GalleryPage.GetThumbnailUrl(galleryObject);
		}

		#endregion

		#region Private Methods

		private void BindData()
		{
			//Get the data associated with the album and display
			if (this.GalleryObjectsDataSource == null)
				DisplayThumbnails(this.GalleryPage.GetAlbum().GetChildGalleryObjects(true, this.GalleryPage.IsAnonymousUser), true);
			else
				DisplayThumbnails(this.GalleryObjectsDataSource, false);
		}

		/// <summary>
		/// Displays thumbnail versions of the specified <paramref name="galleryObjects"/>.
		/// </summary>
		/// <param name="galleryObjects">The gallery objects to display.</param>
		/// <param name="showAddObjectsLink">If set to <c>true</c> show a message and a link allowing the user to add objects to the 
		/// current album as specified in the query string. Set to false when displaying objects that may belong to more than one
		/// album.</param>
		private void DisplayThumbnails(IGalleryObjectCollection galleryObjects, bool showAddObjectsLink)
		{
			string msg;
			if (galleryObjects.Count > 0)
			{
				// At least one album or media object in album.
				//msg = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_addtopmargin2'>{0}</p>", Resources.GalleryServerPro.UC_ThumbnailView_Intro_Text_With_Objects);
				//phMsg.Controls.Add(new LiteralControl(msg));
			}
			else if ((showAddObjectsLink) && (this.GalleryPage.UserCanAddMediaObject) && (!this.GalleryPage.Core.MediaObjectPathIsReadOnly))
			{
				// We have no objects to display. The user is authorized to add objects to this album and the gallery is writeable, so show 
				// message and link to add objects page.
				string innerMsg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_ThumbnailView_Intro_Text_No_Objects_User_Has_Add_MediaObject_Permission, Util.GetUrl(PageId.task_addobjects, "aid={0}", this.GalleryPage.AlbumId));
				msg = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_addtopmargin2 gsp_msgfriendly'>{0}</p>", innerMsg);
				phMsg.Controls.Add(new LiteralControl(msg));
			}
			else
			{
				// No objects and/or user doesn't have permission to add media objects.
				msg = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_addtopmargin2 gsp_msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_ThumbnailView_Intro_Text_No_Objects);
				phMsg.Controls.Add(new LiteralControl(msg));
			}

			this.GalleryPage.SetThumbnailCssStyle(galleryObjects);

			if (PagingEnabled)
			{
				rptr.DataSource = CreatePaging(galleryObjects);

				RegisterPagingScript();
			}
			else
				rptr.DataSource = galleryObjects;

			rptr.DataBind();
		}

		private void RegisterPagingScript()
		{
			const string script = @"function goPage(page) { cbThumbnailView.Callback(page); }";
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "albumHeaderScript", script, true);
		}

		private PagedDataSource CreatePaging(ICollection<IGalleryObject> galleryObjects)
		{
			PagedDataSource pds = new PagedDataSource();
			pds.DataSource = galleryObjects;
			pds.AllowPaging = true;
			pds.PageSize = PageSize;
			pds.CurrentPageIndex = this.GalleryPage.CurrentPage - 1; // Index is 0-based, CurrentPage is 1-based, so subtract one

			PagerPosition pagerPosition = PagerPositionEnumHelper.ParsePagerPosition(this.GalleryPage.Core.PagerLocation);

			if ((pagerPosition == PagerPosition.Top) || (pagerPosition == PagerPosition.TopAndBottom))
			{
				// Configure the top pager.
				pager topPager = (pager)Page.LoadControl(Util.GetUrl("/controls/pager.ascx"));
				topPager.DataSource = pds;
				topPager.CurrentPage = GalleryPage.CurrentPage;
				phPagerTop.Controls.Add(topPager);
			}

			if ((pagerPosition == PagerPosition.Bottom) || (pagerPosition == PagerPosition.TopAndBottom))
			{
				// Configure the bottom pager.
				pager bottomPager = (pager)Page.LoadControl(Util.GetUrl("/controls/pager.ascx"));
				bottomPager.DataSource = pds;
				bottomPager.CurrentPage = GalleryPage.CurrentPage;
				phPagerBtm.Controls.Add(bottomPager);
			}

			return pds;
		}

		#region NOT USED: Animation script

		// See the note in GetAnimationScript for why this is commented out.
		//private void RegisterJavascript()
		//{
		//  string script = GetAnimationScript();

		//  if (!String.IsNullOrEmpty(script))
		//    ScriptManager.RegisterStartupScript(this, this.GetType(), "thumbnailViewScript", script, true);
		//}

//    /// <summary>
//    /// NOT USED: Fading the thumbnails was very slow, so this method is considered unusable for the time being.
//    /// Generate the javascript required for thumbnail animation effects. If transition animation is disabled
//    /// (mediaObjectTransitionType="None" in the config file), then return an empty string.
//    /// </summary>
//    /// <returns>Returns the javascript required for media object animation effects, or an empty string if no
//    /// transition effect is configured.</returns>
//    private static string GetAnimationScript()
//    {
//      string animationScript = String.Empty;
//      MediaObjectTransitionType transitionType = (MediaObjectTransitionType)Enum.Parse(typeof(MediaObjectTransitionType), GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectTransitionType);
//      switch (transitionType)
//      {
//        case MediaObjectTransitionType.Fade:
//          {
//            animationScript = String.Format(CultureInfo.InvariantCulture, @"
//		_fadeInAlbumAnimation = new AjaxControlToolkit.Animation.FadeInAnimation($get('thmbCtnr'), {0}, 3, 0, 1, true);
//		_fadeOutAlbumAnimation = new AjaxControlToolkit.Animation.FadeOutAnimation($get('thmbCtnr'), {0}, 3, 0, 1, true);
//",
//            GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectTransitionDuration);
//            break;
//          }
//        case MediaObjectTransitionType.None: break;
//        default: throw new WebException(String.Format(CultureInfo.CurrentCulture, "The function GetAnimationScript() in user control thumbnailview.ascx encountered the MediaObjectTransitionType \"{0}\", which it was not designed to handle. The developer must update this method to process this enum item.", transitionType.ToString()));
//      }
//      return animationScript;
//    }

		// To enable animation, add the following script to the ascx page:

//  <script type="text/javascript">
//  var _fadeInAlbumAnimation;
//  var _fadeOutAlbumAnimation;
	
//  function cbThumbnailView_onBeforeCallback(sender, eventArgs)
//  {
//    if (_fadeOutAlbumAnimation)
//    {
//      _fadeOutAlbumAnimation.set_target($get('thmbCtnr'));
//      _fadeOutAlbumAnimation.play();
//    }
//  }

//  function cbThumbnailView_onCallbackComplete(sender, eventArgs)
//  {
//    if (_fadeInAlbumAnimation)
//    {
//      _fadeInAlbumAnimation.set_target($get('thmbCtnr'));
//      _fadeInAlbumAnimation.play();
//    }
//  }
//</script>

		#endregion

		#endregion

	}
}
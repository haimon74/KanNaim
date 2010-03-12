using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.uc
{
	public partial class mediaobjectview : System.Web.UI.UserControl
	{
		#region Private Fields

		private int _numMediaObjectsInAlbum = int.MinValue;
		private int _currentMediaObjectIndex = int.MinValue;
		private int _mediaObjectContainerWidth = int.MinValue;
		private MediaObjectWebEntity _mediaObjectEntity;

		private const string SHOW_METADATA_PROFILE_NAME = "ShowMediaObjectMetadata";

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!cbDummy.IsCallback && !IsPostBack)
			{
				// First time page is loading.
				RenderMediaObject();

				ConfigureToolbar();

				ConfigureGrid();

				ShowMediaObjectMetadata();

				ConfigureSecurityRelatedControls();

				RegisterJavascript();
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (!cbDummy.IsCallback)
			{
				ConfigureControls();
			}

			base.OnPreRender(e);
		}

		protected void tbMediaObjectActions_ItemCommand(object sender, ToolBarItemEventArgs e)
		{
			ToolBarItem tbi = e.Item;
			switch (tbi.ID)
			{
				case "tbiDownload": { DownloadMediaObjectToUser(); break; }
				case "tbiRotate": { RedirectToRotatePage(); break; }
				case "tbiMove": { RedirectToMovePage(); break; }
				case "tbiCopy": { RedirectToCopyPage(); break; }
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		public GspPage PageBase
		{
			get { return (GspPage)this.Page; }
		}

		public int NumMediaObjectsInAlbum
		{
			get
			{
				if (_numMediaObjectsInAlbum == int.MinValue)
					InitializeVariables();

				return _numMediaObjectsInAlbum;
			}
			set { _numMediaObjectsInAlbum = value; }
		}

		public int CurrentMediaObjectIndex
		{
			get
			{
				if (_currentMediaObjectIndex == int.MinValue)
					InitializeVariables();

				return _currentMediaObjectIndex;
			}
			set { _currentMediaObjectIndex = value; }
		}


		public bool ViewHiResImage
		{
			get
			{
				// Get from hidden form field first. If not there, look at query string. If not there, look at config file.
				bool viewHiRes;
				object formFieldHiRes = Request.Form["hr"];
				if ((formFieldHiRes == null) || (!Boolean.TryParse(formFieldHiRes.ToString(), out viewHiRes)))
				{
					bool? configValue = WebsiteController.GetQueryStringParameterBoolean("hr");

					viewHiRes = (configValue.HasValue ? configValue.Value : GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailClickShowsOriginal);
				}

				if (viewHiRes && !this.PageBase.UserCanViewHiResImage)
				{
					// User is not authorized to view the original, so deny it even though it is being requested.
					viewHiRes = false;
				}

				return viewHiRes;
			}
		}

		public MediaObjectWebEntity MediaObjectEntity
		{
			get { return _mediaObjectEntity; }
			set { _mediaObjectEntity = value; }
		}

		public int MediaObjectContainerWidth
		{
			get
			{
				if (this._mediaObjectContainerWidth == int.MinValue)
				{
					// Calculate the width of the widest optimized image in this album.
					int maxWidth = int.MinValue;
					bool excludePrivateObjects = !System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
					foreach (IGalleryObject mo in this.PageBase.GetAlbum().GetChildGalleryObjects(false, excludePrivateObjects))
					{
						if (mo is GalleryServerPro.Business.Image)
						{
							if (mo.Optimized.Width > maxWidth)
								maxWidth = mo.Optimized.Width;
						}
						else if (!(mo is GenericMediaObject))
						{
							if (mo.Original.Width > maxWidth)
								maxWidth = mo.Original.Width;
						}
					}

					// Set the width of the previous and next links
					int opt_width = maxWidth;

					if (opt_width < 400) opt_width = 400; //Minimum 400px width

					int paddingBuffer = 40; // Buffer to allow for margins, padding, and border on media media object.
					_mediaObjectContainerWidth = opt_width + paddingBuffer;
				}

				return _mediaObjectContainerWidth;
			}
		}

		/// <summary>
		/// Return the client object ID for the metadata grid inside the dialog control. Used to build javascript in the rendered page.
		/// </summary>
		//public string gdMediaObjectMetadataClientObjectId
		//{
		//  get
		//  {
		//    if (String.IsNullOrEmpty(_gdMediaObjectMetadataClientObjectId))
		//    {
		//      this._gdMediaObjectMetadataClientObjectId = ((ComponentArt.Web.UI.Grid)PageBase.FindControlRecursive(dgMediaObjectInfo, "gdMediaObjectMetadata")).ClientObjectId;
		//    }

		//    return this._gdMediaObjectMetadataClientObjectId;
		//  }
		//}

		//public string dgMediaObjectInfoClientObjectId
		//{
		//  get
		//  {
		//    if (String.IsNullOrEmpty(_dgMediaObjectInfoClientObjectId))
		//    {
		//      this._dgMediaObjectInfoClientObjectId = dgMediaObjectInfo.ClientObjectId;
		//    }

		//    return this._dgMediaObjectInfoClientObjectId;
		//  }
		//}

		#endregion

		#region Private Methods

		private void BindGrid()
		{
			IGalleryObjectMetadataItemCollection metadata = this.PageBase.GetMediaObject().MetadataItems;
			Grid gd = ((Grid)this.PageBase.FindControlRecursive(dgMediaObjectInfo, "gdmeta"));
			gd.DataSource = metadata;
			gd.DataBind();
		}

		private void InitializeVariables()
		{
			IGalleryObject mediaObject = this.PageBase.GetMediaObject();
			IAlbum album = (IAlbum)mediaObject.Parent;

			System.Diagnostics.Debug.Assert(mediaObject.Parent.Equals(this.PageBase.GetAlbum()), "WebController.Album is not the same object in memory as WebController.MediaObject.GetParent().");

			bool excludePrivateObjects = !System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
			IGalleryObjectCollection siblings = album.GetChildGalleryObjects(GalleryObjectType.MediaObject, true, excludePrivateObjects);

			this.NumMediaObjectsInAlbum = siblings.Count;
			this.CurrentMediaObjectIndex = siblings.IndexOf(mediaObject);
		}

		private void ConfigureControls()
		{
			ScriptManager.RegisterHiddenField(this, "hr", this.ViewHiResImage.ToString());

			imgNext.ImageUrl = String.Format(CultureInfo.CurrentCulture, "{0}/images/right_arrow.png", this.PageBase.ThemePath);
			imgPrevious.ImageUrl = String.Format(CultureInfo.CurrentCulture, "{0}/images/left_arrow.png", this.PageBase.ThemePath);

			// Set the position text
			string positionText = string.Format(CultureInfo.CurrentCulture, "(<span id='lblMoPosition'>{0}</span> {1} <span id='lblMoCount'>{2}</span>)", this.CurrentMediaObjectIndex + 1, Resources.GalleryServerPro.UC_MediaObjectView_Position_Separator_Text, this.NumMediaObjectsInAlbum);
			phPosition.Controls.Add(new System.Web.UI.LiteralControl(positionText));
		}

		private void ConfigureGrid()
		{
			// The EmptyGridText property cannot be declaratively styled or assigned to a resource string, so we'll do it programmatically.
			string emptyGridText = String.Format(System.Globalization.CultureInfo.CurrentCulture, "<span class=\"msgfriendly gdInfoEmptyGridText\">{0}</span>", Resources.GalleryServerPro.UC_MediaObjectView_Info_Empty_Grid_Text);
			((Grid)this.PageBase.FindControlRecursive(dgMediaObjectInfo, "gdmeta")).EmptyGridText = emptyGridText;
		}

		private void RenderMediaObject()
		{
			IGalleryObject mediaObject = this.PageBase.GetMediaObject();

			DisplayObjectType displayType = DisplayObjectType.Original;

			if (mediaObject is GalleryServerPro.Business.Image)
			{
				displayType = (this.ViewHiResImage ? DisplayObjectType.Original : DisplayObjectType.Optimized);
			}
			MediaObjectWebEntity mo = GspPage.GetMediaObjectHtml(mediaObject, displayType, false);

			pnlMediaObject.Controls.Add(new LiteralControl(mo.HtmlOutput));

			// If there is an javascript that needs to be sent, register it now.
			if (!String.IsNullOrEmpty(mo.ScriptOutput))
			{
				ScriptManager.RegisterStartupScript(this, typeof(GspPage), "mediaObjectStartupScript", mo.ScriptOutput, true);
			}

			this.MediaObjectEntity = mo;

			moTitle.InnerHtml = mo.Title;
		}

		private void DownloadMediaObjectToUser()
		{
			IGalleryObject mediaObject = this.PageBase.GetMediaObject();
			IDisplayObject displayObject = mediaObject.Original;
			bool isMediaObjectImage = (mediaObject is GalleryServerPro.Business.Image);

			if ((isMediaObjectImage) && (!this.ViewHiResImage) && (!String.IsNullOrEmpty(mediaObject.Optimized.FileNamePhysicalPath)))
			{
				// Grab optimized version if the object is an image, the user is requesting the optimized version, and
				// the optimized version has a valid filepath. (There may not be a valid filepath for those images that Gallery Server
				// can't create an optimized version for; corrupted images, for example.)
				displayObject = mediaObject.Optimized;
			}

			Response.Clear();
			IMimeType mimeType = mediaObject.MimeType;
			if ((!String.IsNullOrEmpty(mimeType.MajorType)) && (!String.IsNullOrEmpty(mimeType.Subtype)))
				Response.ContentType = mediaObject.MimeType.FullType;
			else
				Response.ContentType = String.Format(CultureInfo.CurrentCulture, "Unknown/{0}", System.IO.Path.GetExtension(displayObject.FileName).Replace(".", String.Empty));

			Response.AppendHeader("Content-Disposition", "attachment;filename=" + displayObject.FileName.Replace(" ", String.Empty)); // must remove spaces

			bool applyWatermark = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ApplyWatermark;

			if (AppSetting.Instance.IsInReducedFunctionalityMode || 
				(applyWatermark && (mediaObject is GalleryServerPro.Business.Image) && (!this.PageBase.IsUserAuthorized(SecurityActions.HideWatermark))))
				TransmitMediaObjectWithWatermark(displayObject.FileNamePhysicalPath); // This is an image and user does not have the 'hide watermark' permission
			else
				Response.TransmitFile(displayObject.FileNamePhysicalPath); // User has permission to have watermark applied or this is not an image

			//if (!isMediaObjectImage)
			//{
			// Don't send Response.End if the current media object is an image. It causes metadata popup to give "The data could not be loaded" error 
			// when navigating media objects. However, we need it in order for ZIP files to be sent to the client (without it they should up empty).
			// Since the user probably has the metadata popup only when viewing images, it is a reasonable workaround to send End() for non-images.
			Response.End();
			//}
		}

		private void TransmitMediaObjectWithWatermark(string filepath)
		{
			// Send the specified file to the client with the watermark overlayed on top.
			System.Drawing.Image watermarkedImage = null;
			try
			{
				watermarkedImage = ImageHelper.AddWatermark(filepath);
				watermarkedImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			catch
			{
				// Can't apply watermark to image. Abort mission and display error message.
				string redirectUrl = WebsiteController.AddQueryStringParameter(Request.RawUrl, String.Format(CultureInfo.CurrentCulture, "msg={0}", (int)Message.CannotOverlayWatermarkOnImage));
				Response.Redirect(redirectUrl, false);
				System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
		}

		private void ShowMediaObjectMetadata()
		{
			if (ConfigManager.GetGalleryServerProConfigSection().Core.EnableImageMetadata)
			{
				bool showMetadata = false;
				if ((this.Context.Profile != null) && (this.Context.Profile[SHOW_METADATA_PROFILE_NAME] != null))
					showMetadata = Convert.ToBoolean(this.Context.Profile[SHOW_METADATA_PROFILE_NAME], CultureInfo.InvariantCulture);

				FindToolBarItem("tbiInfo").Checked = showMetadata;

				// Bind the grid to the data. This should not be necessary because we have javascript code run during the client grid load 
				// event that displays the metadata if the toolbar icon is checked (from the previous line of code). However, unless
				// we bind the grid on the server, the column widths are not correctly rendered, and no amount of tinkering seems to 
				// do the trick. In subsequent releases of the CA grid control, we can try commenting out this line. (If you do that,
				// replace the line gdmeta.render(); with refreshMetadata($get('moid').value); in the javascript function gdmeta_onLoad.
				BindGrid();
			}
		}

		private ToolBarItem FindToolBarItem(string id)
		{
			foreach (ToolBarItem tbItem in tbMediaObjectActions.Items)
			{
				if (tbItem.ID == id)
					return tbItem;
			}
			throw new WebException(string.Format(CultureInfo.CurrentCulture, "The toolbar tbMediaObjectActions does not contain a ToolBarItem with ID {0}.", id));
		}

		private void RegisterJavascript()
		{
			string script = String.Format(CultureInfo.InvariantCulture, @"
	var _mo = '{0}';
	var _moTitle = '{1}';
	var _optimizedMediaObjectContainerWidth = {2};
	var _ssDelay = {3};
	var _viewHiRes = {4};
	var _tbIsVisible = {5};
	var _moid = {6};

	function redirectToHomePage()
	{{
		window.location = '{7}';
	}}

	function togglePermalink(toolbarItem)
	{{
		var showPermalink = toolbarItem.get_checked();
		var permalinkContainer = $get('divPermalink');
		var permalinkUrlTag = $get('permaLinkUrlTag');
		
		var linkUrl = ""<a href='"" + _moInfo.PermalinkUrl + ""' + title='{8}'>"" + _moInfo.PermalinkUrl + ""</a>"";
		permalinkUrlTag.innerHTML = linkUrl;
		if (showPermalink)
		{{
			Sys.UI.DomElement.removeCssClass(permalinkContainer, 'invisible');
			Sys.UI.DomElement.addCssClass(permalinkContainer, 'visible');
		}}
		else
		{{
			Sys.UI.DomElement.removeCssClass(permalinkContainer, 'visible');
			Sys.UI.DomElement.addCssClass(permalinkContainer, 'invisible');
		}}
	}}

	{9}

	{10}

	{11}
	
	{12}	

	{13}		

			",
			pnlMediaObject.ClientID, // 0
			moTitle.ClientID, // 1
			this.MediaObjectContainerWidth, // 2
			GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.SlideshowInterval, // 3
			this.ViewHiResImage.ToString().ToLowerInvariant(), // 4
			tbMediaObjectActions.Visible.ToString().ToLowerInvariant(), // 5
			this.MediaObjectEntity.Id, // 6
			ResolveClientUrl("~/default.aspx?aid=" + this.PageBase.AlbumId), // 7
			Resources.GalleryServerPro.UC_MediaObjectView_Permalink_Url_Tooltip, // 8
			GetToggleHiResScript(), // 9
			GetDeleteMediaObjectScript(), // 10
			GetPageLoadScript(), // 11
			GetEditMediaObjectScript(), // 12
			GetEditAlbumScript() // 13
			);

			ScriptManager.RegisterClientScriptBlock(this, typeof(GspPage), "mediaObjectViewScript", script, true);
		}

		/// <summary>
		/// Generate the javascript to show or hide the high resolution version of an image.
		/// </summary>
		/// <returns>Returns the javascript to show or hide the high resolution version of an image.</returns>
		private string GetToggleHiResScript()
		{
			string script = String.Empty;

			if (this.PageBase.UserCanViewHiResImage)
			{
				script = @"
	function toggleHiRes(toolbarItem)
	{{
		if (toolbarItem.get_checked())
		{{
			if (_moInfo.HiResAvailable)
			{{
				_viewHiRes = true;
				$get('hr').value = _viewHiRes;
				showMediaObject(_moInfo.Id);
			}}
			else
			{{
				_viewHiRes = false;
				toolbarItem.set_checked(false);
			}}
		}}
		else
		{{
			_viewHiRes = false;
			$get('hr').value = _viewHiRes;
			$get('divMoView').style.width = _optimizedMediaObjectContainerWidth + 'px';
			showMediaObject(_moInfo.Id);
		}}
	}}
";
			}

			return script;
		}

		/// <summary>
		/// Generate the javascript to support deleting the current media object. Returns an empty string 
		/// if the logged on user does not have this permission.
		/// </summary>
		/// <returns>Returns the javascript to support deleting the current media object,
		/// or an empty string if the logged on user does not have this permission.</returns>
		private string GetDeleteMediaObjectScript()
		{
			string script = String.Empty;

			if (this.PageBase.UserCanEditMediaObject)
			{
				script = String.Format(CultureInfo.InvariantCulture, @"
	function deleteObject(toolbarItem)
	{{
		var question = '{0}';
		if (confirm(question))
		{{
			PageMethods.DeleteMediaObject(_moInfo.Id, getDeleteMediaObjectCompleted);
		}}
	}}
	
	function getDeleteMediaObjectCompleted(results, context, methodName)
	{{
		_moInfo.NumObjectsInAlbum = _moInfo.NumObjectsInAlbum - 1;
		_moInfo.Index = _moInfo.Index - 1;
		showNextMediaObject();
	}}
", Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Delete_Confirmation_Msg);
			}

			return script;
		}

		/// <summary>
		/// Generate the javascript to support the scenario where the logged on user has edit media object permission,
		/// Returns an empty string if the logged on user does not have this permission.
		/// </summary>
		/// <returns>Returns the javascript to support the scenario where the logged on user has edit media object permission,
		/// or an empty string if the logged on user does not have this permission.</returns>
		private string GetEditMediaObjectScript()
		{
			string script = String.Empty;

			if (this.PageBase.UserCanEditMediaObject)
			{
				script = String.Format(CultureInfo.InvariantCulture, @"
	function editCaption()
	{{
		var moTitle = $get('{0}');
		var dgHeight = moTitle.clientHeight < 110? 110 : moTitle.clientHeight;
		setDialogSize(dgEditCaption, moTitle.clientWidth, dgHeight);
		dgEditCaption.show();
		$get('taCaption').value = $get(_moTitle).innerHTML;
		$get('taCaption').focus();
	}}

	function saveCaption(title)
	{{
		document.body.style.cursor = 'wait';
		PageMethods.UpdateMediaObjectTitle(_moInfo.Id, title, updateMediaObjectTitleCompleted);
	}}
	
	function updateMediaObjectTitleCompleted(results, context, methodName)
	{{
		$get(_moTitle).innerHTML = results;
		dgEditCaption.close();
		document.body.style.cursor = 'default';
	}}
", pnlMediaObjectTitle.ClientID);
			}

			return script;
		}

		/// <summary>
		/// Generate the javascript to support the scenario where the logged on user has edit album permission,
		/// Returns an empty string if the logged on user does not have this permission.
		/// </summary>
		/// <returns>Returns the javascript to support the scenario where the logged on user has edit album permission,
		/// or an empty string if the logged on user does not have this permission.</returns>
		private string GetEditAlbumScript()
		{
			string script = String.Empty;

			if (this.PageBase.UserCanEditAlbum)
			{
				IAlbum album = this.PageBase.GetAlbum();
				script = String.Format(CultureInfo.InvariantCulture, @"

  var _dateFormatString = 'd'; // Short date format
	var _albumId = {0};

	function editAlbumInfo()
	{{
		dgEditAlbum.show();
		document.body.style.cursor = 'wait';
		PageMethods.GetAlbumInfo(_albumId, getAlbumInfoCompleted);
	}}
	
	function getAlbumInfoCompleted(results, context, methodName)
	{{
		$get(_txtTitleId).focus();
		dgEditAlbum.set_title('{1}: ' + results.Title);
		$get(_txtTitleId).value = results.Title;

		$get('albumSummary').value = results.Summary;
		$get('private').checked = results.IsPrivate;
		$get('private').disabled = {2};

		if (results.DateStart > new Date(1,1,1))
    {{
		  $get('beginDate').value = results.DateStart.localeFormat(_dateFormatString);
			var cdrDateStart = $find(_cdrBeginDateId);
		  cdrDateStart.setSelectedDate(results.DateStart);		
    }}

    if (results.DateEnd > new Date(1,1,1))
    {{
		  $get('endDate').value = results.DateEnd.localeFormat(_dateFormatString);
		  var cdrDateEnd = $find(_cdrEndDateId);
		  cdrDateEnd.setSelectedDate(results.DateEnd);		
    }}
		
		document.body.style.cursor = 'default';
	}}

	function saveAlbumInfo()
	{{
		document.body.style.cursor = 'wait';
		var albumEntity = new Gsp.AlbumWebEntity();
		albumEntity.Id = _albumId;
		albumEntity.Title = $get(_txtTitleId).value;
		albumEntity.Summary = $get('albumSummary').value;
		albumEntity.IsPrivate = $get('private').checked;
		
    var beginDate = Date.parseLocale($get('beginDate').value, _dateFormatString);
    if (beginDate != null)
    {{
      _beginDate = beginDate;
		  albumEntity.DateStart = beginDate;
		}}
    else
    {{
      _beginDate = null;
    }}

    var endDate = Date.parseLocale($get('endDate').value, _dateFormatString);
    if (endDate != null)
    {{
      _endDate = endDate;
		  albumEntity.DateEnd = endDate;
    }}
    else
    {{
      _endDate = null;
    }}

		PageMethods.UpdateAlbumInfo(albumEntity, updateAlbumInfoCompleted);
		dgEditAlbum.close();
	}}

	function updateAlbumInfoCompleted(results, context, methodName)
	{{
		setText($get('currentAlbumLink'), results.Title);
		document.body.style.cursor = 'default';
	}}

	function setText(node, newText)
	{{
		var childNodes = node.childNodes;
		for (var i=0; i < childNodes.length; i++)
		{{
			node.removeChild(childNodes[i]);
		}}
		if ((newText != null) && (newText.length > 0))
			node.appendChild(document.createTextNode(newText));
	}}
",
				album.Id, // 0
				Resources.GalleryServerPro.UC_Album_Header_Dialog_Title_Edit_Album, // 1
				album.Parent.IsPrivate.ToString().ToLowerInvariant() // 2
				);

				// Add script reference to the javascript file that defines the Gsp.AlbumWebEntity class. This is needed to allow 
				// javascript to be able to instantiate the class.
				smp.Scripts.Add(new ScriptReference("~/script/entityobjects.js"));
			}

			return script;
		}

		/// <summary>
		/// Generate the javascript function that will run when the page loads. It includes support for media object 
		/// animation effects and the keydown handler for navigating between media objects. This function is registered
		/// to run automatically when the page loads with: Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(moViewPageLoad);
		/// This registration occurs in the javascript in mediaobjectview.ascx.
		/// </summary>
		/// <returns>Returns the javascript function to be added to the page output.</returns>
		private static string GetPageLoadScript()
		{
			// Note: Even though we programatically set the hidden field using ScriptManager.RegisterHiddenField in the page load
			// event of default.aspx, we need to explicitly assign it here using $get('moid').value = _moid;
			// This is because Firefox caches the previous value during a page reload (F5), but we need the moid that is in
			// the query string to be the new value. This function is the only place we use the _moid variable. (IE does not have this issue.)
			string pageLoadScript = String.Format(CultureInfo.InvariantCulture, @"	
	function moViewPageLoad(sender, args)
	{{
		$addHandler($get('html'), 'keydown', html_onkeydown);
		$get('moid').value = _moid;

		_inPrefetch = true;
		PageMethods.GetMediaObjectHtml(_moid, getDisplayType(), getMediaObjectHtmlCompleted);
		{0}
	}}

	function html_onkeydown(e)
	{{
		var tag = e.target.tagName.toLowerCase();
		if ((e.keyCode === Sys.UI.Key.enter) && ((tag == 'html') || (tag == 'body'))) // Firefox = html; IE = body
		{{
			if (e.shiftKey)
				showPrevMediaObject();
			else
				showNextMediaObject();
		}}
	}}
",
				GetAnimationScript());

			return pageLoadScript;
		}

		/// <summary>
		/// Generate the javascript required for media object animation effects. If transition animation is disabled
		/// (mediaObjectTransitionType="None" in the config file), then return an empty string.
		/// </summary>
		/// <returns>Returns the javascript required for media object animation effects, or an empty string if no
		/// transition effect is configured.</returns>
		private static string GetAnimationScript()
		{
			string animationScript = String.Empty;
			MediaObjectTransitionType transitionType = (MediaObjectTransitionType)Enum.Parse(typeof(MediaObjectTransitionType), GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectTransitionType);
			switch (transitionType)
			{
				case MediaObjectTransitionType.Fade:
					{
						animationScript = String.Format(CultureInfo.InvariantCulture, @"
		_fadeInMoAnimation = new AjaxControlToolkit.Animation.FadeInAnimation($get(_mo), {0}, 20, 0, 1, true);
",
						GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectTransitionDuration);
						break;
					}
				case MediaObjectTransitionType.None: break;
				default: throw new WebException(String.Format(CultureInfo.CurrentCulture, "The function GetAnimationScript() in user control mediaobjectview.ascx encountered the MediaObjectTransitionType \"{0}\", which it was not designed to handle. The developer must update this method to process this enum item.", transitionType.ToString()));
			}
			return animationScript;
		}

		/// <summary>
		/// Generate the javascript that preloads all optimized or original images in the album.
		/// </summary>
		/// <returns>Returns the javascript that preloads all optimized or original images in the album.</returns>
		//private string GetImagePreloadScript()
		//{
		//  string preloadScript = String.Empty;
		//  string imgPath = String.Empty;
		//  bool viewHiRes = this.ViewHiResImage;
		//  int moIterator = 1;
		//  IGalleryObjectCollection imageObjects = this.PageBase.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Image, true);

		//  if (imageObjects.Count > 6)
		//  {
		//    // More than 6 objects - use a StringBuilder
		//    System.Text.StringBuilder sbPreloadScript = new System.Text.StringBuilder(imageObjects.Count);
		//    foreach (IGalleryObject mo in imageObjects)
		//    {
		//      imgPath = (this.ViewHiResImage ? this.PageBase.GetOriginalUrl(mo) : this.PageBase.GetOptimizedUrl(mo));
		//      sbPreloadScript.Append(String.Format(CultureInfo.CurrentCulture, "var img{0} = new Image(); img{0}.src = \"{1}\";\n", moIterator, imgPath));
		//      moIterator++;
		//    }
		//    preloadScript = sbPreloadScript.ToString();
		//  }
		//  else
		//  {
		//    // Six or less objects - just append a string.
		//    foreach (IGalleryObject mo in imageObjects)
		//    {
		//      imgPath = (this.ViewHiResImage ? this.PageBase.GetOriginalUrl(mo) : this.PageBase.GetOptimizedUrl(mo));
		//      preloadScript += String.Format(CultureInfo.CurrentCulture, "var img{0} = new Image(); img{0}.src = \"{1}\";\n", moIterator, imgPath);
		//      moIterator++;
		//    }
		//  }

		//  return preloadScript;
		//}

		//private void DeleteMediaObject()
		//{
		//  int nextMoid = this.PageBase.GetNextMediaObjectId();

		//  IGalleryObject mo = this.PageBase.GetMediaObject();
		//  mo.Delete();
		//  HelperFunctions.PurgeCache();

		//  if (nextMoid > 0)
		//  {
		//    Response.Redirect(ResolveClientUrl("~/default.aspx?moid=" + nextMoid), false);
		//    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		//  }
		//  else
		//  {
		//    Response.Redirect(ResolveClientUrl("~/default.aspx?aid=" + this.PageBase.AlbumId), false);
		//    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		//  }
		//}

		private void RedirectToRotatePage()
		{
			Response.Redirect(String.Format(CultureInfo.CurrentCulture, "~/task/rotateimage.aspx?moid={0}", this.PageBase.GetMediaObject().Id), false);
			System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		private void RedirectToMovePage()
		{
			Response.Redirect(String.Format(CultureInfo.CurrentCulture, "~/task/transferobject.aspx?moid={0}&tt=move&skipstep1=true", this.PageBase.GetMediaObject().Id), false);
			System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		private void RedirectToCopyPage()
		{
			Response.Redirect(String.Format(CultureInfo.CurrentCulture, "~/task/transferobject.aspx?moid={0}&tt=copy&skipstep1=true", this.PageBase.GetMediaObject().Id), false);
			System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		private void ConfigureToolbar()
		{
			ToolBarItem tbItem;

			Core core = ConfigManager.GetGalleryServerProConfigSection().Core;

			//<CA:ToolBarItem ID="tbiInfo" runat="server" ImageUrl="info.png" ItemType="ToggleCheck"
			//ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Info_Tooltip %>" />
			if (core.EnableImageMetadata)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiInfo";
				tbItem.ImageUrl = "info.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Info_Text;
				tbItem.ItemType = ToolBarItemType.ToggleCheck;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Info_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);

				// Separator
				tbMediaObjectActions.Items.Add(GetToolBarSeparator());
			}

			//<CA:ToolBarItem ID="tbiDownload" runat="server" AutoPostBackOnSelect="true" ImageUrl="download.png"
			//Text="Download" ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Download_Tooltip %>" />
			if (core.EnableMediaObjectDownload)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiDownload";
				tbItem.ImageUrl = "download.png";
				tbItem.Visible = this.MediaObjectEntity.IsDownloadable;
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Download_Text;
				tbItem.AutoPostBackOnSelect = true;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Download_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			//<CA:ToolBarItem ID="tbiViewHiRes" runat="server" ItemType="ToggleCheck" ImageUrl="hires.png"
			//Text="View hi-res" ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_HiRes_Tooltip %>" />
			if (this.PageBase.UserCanViewHiResImage)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiViewHiRes";
				tbItem.ImageUrl = "hires.png";
				tbItem.Visible = this.MediaObjectEntity.HiResAvailable;
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_HiRes_Text;
				tbItem.ItemType = ToolBarItemType.ToggleCheck;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_HiRes_Tooltip;
				tbItem.Checked = this.ViewHiResImage;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			//<CA:ToolBarItem ID="tbiPermalink" runat="server" ItemType="ToggleCheck" ImageUrl="hyperlink.png"
			//Text="Permalink" ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Permalink_Tooltip %>" />
			if (core.EnablePermalink)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiPermalink";
				tbItem.ImageUrl = "hyperlink.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Permalink_Text;
				tbItem.ItemType = ToolBarItemType.ToggleCheck;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Permalink_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			// Separator
			InsertToolBarSeparator();

			//<CA:ToolBarItem ID="tbiSlideshow" runat="server" ImageUrl="play.png" Text="Slideshow"
			//ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Slideshow_Tooltip %>" />
			if (core.EnableSlideShow)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiSlideshow";
				tbItem.ImageUrl = "play.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Slideshow_Text;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Slideshow_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);

				// Separator
				tbMediaObjectActions.Items.Add(GetToolBarSeparator());
			}

			//<CA:ToolBarItem ID="tbiMove" runat="server" AutoPostBackOnSelect="true" ImageUrl="move.png"
			//Text="Transfer" ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Transfer_Tooltip %>" />
			if (this.PageBase.UserCanDeleteMediaObject)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiMove";
				tbItem.ImageUrl = "move.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Transfer_Text;
				tbItem.AutoPostBackOnSelect = true;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Transfer_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			//<CA:ToolBarItem ID="tbiCopy" runat="server" AutoPostBackOnSelect="true" ImageUrl="copy.png"
			//Text="Copy" ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Copy_Tooltip %>" />
			if (CanUserCopyCurrentMediaObject())
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiCopy";
				tbItem.ImageUrl = "copy.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Copy_Text;
				tbItem.AutoPostBackOnSelect = true;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Copy_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			//<CA:ToolBarItem ID="tbiRotate" runat="server" AutoPostBackOnSelect="true" ImageUrl="rotate.png"
			//Text="Rotate" ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Rotate_Tooltip %>" />
			if (this.PageBase.UserCanEditMediaObject)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiRotate";
				tbItem.ImageUrl = "rotate.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Rotate_Text;
				tbItem.AutoPostBackOnSelect = true;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Rotate_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			//<CA:ToolBarItem ID="tbiDelete" runat="server" ImageUrl="delete.png" Text="Delete"
			//ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_ToolBar_Delete_Tooltip %>" />
			if (this.PageBase.UserCanDeleteMediaObject)
			{
				tbItem = new ToolBarItem();
				tbItem.ID = "tbiDelete";
				tbItem.ImageUrl = "delete.png";
				tbItem.Text = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Delete_Text;
				tbItem.ToolTip = Resources.GalleryServerPro.UC_MediaObjectView_ToolBar_Delete_Tooltip;
				tbMediaObjectActions.Items.Add(tbItem);
			}

			if (tbMediaObjectActions.Items.Count == 0)
			{
				tbMediaObjectActions.Visible = false;
			}
		}

		/// <summary>
		/// Insert a separator bar at the end of the toolbar items, but only if the last one isn't already 
		/// a separator bar.
		/// </summary>
		private void InsertToolBarSeparator()
		{
			if (tbMediaObjectActions.Items.Count > 0)
			{
				ToolBarItem tbi = tbMediaObjectActions.Items[tbMediaObjectActions.Items.Count - 1];
				if (tbi.ItemType != ToolBarItemType.Separator)
				{
					tbMediaObjectActions.Items.Add(GetToolBarSeparator());
				}
			}
		}

		/// <summary>
		/// Determine if the current user can copy the current media object into another album. This requires
		/// AllowAddMediaObject permission for at least one album that is NOT the current album. Permission is
		/// not granted if no user is logged in.
		/// </summary>
		/// <returns>Returns true if there is at least one album where the logged on user can copy the current 
		/// media object to; returns false if permission does not exist or current user is not logged in.</returns>
		private bool CanUserCopyCurrentMediaObject()
		{
			bool userCanCopy = false;
			if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
			{
				IGalleryServerRoleCollection roles = this.PageBase.GetGalleryServerRolesForUser();
				foreach (IGalleryServerRole role in roles)
				{
					if (role.AllowAddMediaObject)
					{
						if ((role.AllAlbumIds.Count > 0)
							|| ((role.AllAlbumIds.Count == 1) && (!role.AllAlbumIds.Contains(this.PageBase.GetAlbum().Id))))
						{
							userCanCopy = true;
							break;
						}
					}
				}
			}
			return userCanCopy;
		}

		private static ToolBarItem GetToolBarSeparator()
		{
			//<CA:ToolBarItem ItemType="Separator" ImageUrl="break.gif" ImageHeight="16" ImageWidth="2" />
			ToolBarItem tbItem = new ToolBarItem();
			tbItem.ItemType = ToolBarItemType.Separator;
			tbItem.ImageUrl = "break.gif";
			tbItem.ImageHeight = new Unit(16);
			tbItem.ImageWidth = new Unit(2);
			return tbItem;
		}

		private void ConfigureSecurityRelatedControls()
		{
			if (this.PageBase.UserCanEditAlbum)
			{
				AddEditAlbumInfoDialog();
			}

			if (this.PageBase.UserCanEditMediaObject)
			{
				AddEditMediaObjectCaptionDialog();
			}
		}

		private void AddEditAlbumInfoDialog()
		{
			Dialog dgEditAlbum = new Dialog();

			dgEditAlbum.ContentTemplate = Page.LoadTemplate("~/uc/albumedittemplate.ascx");

			#region Set Dialog Properties

			dgEditAlbum.ID = "dgEditAlbum";
			dgEditAlbum.AnimationDirectionElement = "currentAlbumLink";
			dgEditAlbum.CloseTransition = TransitionType.Fade;
			dgEditAlbum.ShowTransition = TransitionType.Fade;
			dgEditAlbum.AnimationSlide = SlideType.Linear;
			dgEditAlbum.AnimationType = DialogAnimationType.Live;
			dgEditAlbum.AnimationPath = SlidePath.Direct;
			dgEditAlbum.AnimationDuration = 400;
			dgEditAlbum.TransitionDuration = 400;
			dgEditAlbum.Icon = "pencil.gif";
			dgEditAlbum.Alignment = DialogAlignType.MiddleCentre;
			dgEditAlbum.AllowResize = true;
			dgEditAlbum.ContentCssClass = "dg0ContentCss";
			dgEditAlbum.HeaderCssClass = "dg0HeaderCss";
			dgEditAlbum.CssClass = "dg0DialogCss";
			dgEditAlbum.FooterCssClass = "dg0FooterCss";

			dgEditAlbum.HeaderClientTemplateId = "dgEditAlbumHeaderTemplate";

			#endregion

			#region Header Template

			ClientTemplate ctHeader = new ClientTemplate();
			ctHeader.ID = "dgEditAlbumHeaderTemplate";

			ctHeader.Text = @"
		<div onmousedown='dgEditAlbum.StartDrag(event);'>
			<img id='dg0DialogCloseImage' onclick=""dgEditAlbum.Close('cancelled');"" src='images/componentart/dialog/close.gif' /><img
				id='dg0DialogIconImage' src='images/componentart/dialog/pencil.gif' style='width:27px;height:30px;' />
				## Parent.Title ##
		</div>";

			dgEditAlbum.ClientTemplates.Add(ctHeader);

			#endregion

			phDialogContainer.Controls.Add(dgEditAlbum);
		}

		private void AddEditMediaObjectCaptionDialog()
		{
			// If the user has permission to edit the media object, configure the caption so that when it is double-clicked,
			// a dialog window appears that lets the user edit and save the caption. Note that this code is dependent on the
			// saveCaption javascript function, which is added in the RegisterJavascript method.
			if (this.PageBase.UserCanEditMediaObject)
			{
				pnlMediaObjectTitle.ToolTip = Resources.GalleryServerPro.Site_Editable_Content_Tooltip;
				pnlMediaObjectTitle.CssClass = "editableContentOff";
				pnlMediaObjectTitle.Attributes.Add("onmouseover", "this.className='editableContentOn';");
				pnlMediaObjectTitle.Attributes.Add("onmouseout", "this.className='editableContentOff';");
				pnlMediaObjectTitle.Attributes.Add("ondblclick", "editCaption()");

				Dialog dgEditCaption = new Dialog();
				dgEditCaption.ID = "dgEditCaption";
				dgEditCaption.AlignmentElement = pnlMediaObjectTitle.ClientID;
				dgEditCaption.CssClass = "dg3DialogCss";
				dgEditCaption.ContentCssClass = "dg3ContentCss";
				dgEditCaption.ContentClientTemplateId = "dgEditCaptionContentTemplate";

				ClientTemplate ct = new ClientTemplate();
				ct.ID = "dgEditCaptionContentTemplate";

				ct.Text = String.Format(CultureInfo.InvariantCulture, @"
<textarea id='taCaption' rows='4' cols='75' class='mediaObjectTitleTextArea'>{0}</textarea>
<div class='okCancelContainer'>
	<input type='button' value='{1}' onclick=""saveCaption($get('taCaption').value)"" />&nbsp;<input type='button' value='{2}' onclick='dgEditCaption.close()' />
</div>",
					this.PageBase.GetMediaObject().Title,
					Resources.GalleryServerPro.Default_Task_Ok_Button_Text,
					Resources.GalleryServerPro.Default_Task_Cancel_Button_Text
					);

				dgEditCaption.ClientTemplates.Add(ct);

				phDialogContainer.Controls.Add(dgEditCaption);
			}
		}

		#endregion

	}
}
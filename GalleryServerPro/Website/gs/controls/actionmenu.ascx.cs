using System;

namespace GalleryServerPro.Web.Controls
{
	public partial class actionmenu : GalleryUserControl
	{
		#region Private Fields

		int _albumId = int.MinValue;
		int _mediaObjectId = int.MinValue;

		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.GalleryPage.IsCallback)
			{
				ConfigureMenu();
			}
		}

		private void Menu1_ItemSelected(object sender, ComponentArt.Web.UI.MenuItemEventArgs e)
		{
			if (e.Item.ID == "mnuLogout")
			{
				LogOffUser();

				ReloadPage();
			}
		}

		#endregion

		#region Public Properties

		public int AlbumId
		{
			get
			{
				if (this._albumId == int.MinValue)
				{
					this._albumId = this.GalleryPage.AlbumId;
				}

				return this._albumId;
			}
		}

		public int MediaObjectId
		{
			get
			{
				if (this._mediaObjectId == int.MinValue)
				{
					this._mediaObjectId = this.GalleryPage.MediaObjectId;
				}

				return this._mediaObjectId;
			}
		}

		public string DisabledDueToInsufficientPermissionText
		{
			get
			{
				return Resources.GalleryServerPro.UC_ActionMenu_Disabled_Insufficient_Permission_Tooltip;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureMenu()
		{
			Menu1.ItemSelected += this.Menu1_ItemSelected;

			SetMenuProperties();

			SetMenuItemsText();

			SetUrlTooltips();

			SetNavigationUrls();

			EnableOrDisableMenuItemsBasedOnUserPermission();

			EnableOrDisableMenuItemsBasedOnWriteAccess();
		}

		private void SetMenuProperties()
		{
			Menu1.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/menu/");
		}

		private void SetMenuItemsText()
		{
			mnuCreateAlbum.Text = Resources.GalleryServerPro.UC_ActionMenu_Create_Album_Text;
			mnuEditAlbum.Text = Resources.GalleryServerPro.UC_ActionMenu_Edit_Album_Text;
			mnuAddObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Add_Objects_Text;
			mnuMoveObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Transfer_Objects_Text;
			mnuCopyObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Copy_Objects_Text;
			mnuMoveAlbum.Text = Resources.GalleryServerPro.UC_ActionMenu_Move_Album_Text;
			mnuCopyAlbum.Text = Resources.GalleryServerPro.UC_ActionMenu_Copy_Album_Text;
			mnuDownloadObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Download_Objects_Text;
			mnuEditCaptions.Text = Resources.GalleryServerPro.UC_ActionMenu_Edit_Captions_Text;
			mnuAssignThumbnail.Text = Resources.GalleryServerPro.UC_ActionMenu_Assign_Thumbnail_Text;
			mnuRearrangeObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Rearrange_Text;
			mnuRotate.Text = Resources.GalleryServerPro.UC_ActionMenu_Rotate_Text;
			mnuDeleteObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Delete_Objects_Text;
			mnuDeleteHiRes.Text = Resources.GalleryServerPro.UC_ActionMenu_Delete_HiRes_Text;
			mnuDeleteAlbum.Text = Resources.GalleryServerPro.UC_ActionMenu_Delete_Album_Text;
			mnuSynch.Text = Resources.GalleryServerPro.UC_ActionMenu_Synchronize_Text;
			mnuSiteSettings.Text = Resources.GalleryServerPro.UC_ActionMenu_Admin_Console_Text;
			mnuLogout.Text = Resources.GalleryServerPro.UC_ActionMenu_Logout_Text;
		}

		private void SetUrlTooltips()
		{
			mnuCreateAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Create_Album_Tooltip;
			mnuEditAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Edit_Album_Tooltip;
			mnuAddObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Add_Objects_Tooltip;
			mnuMoveObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Transfer_Objects_Tooltip;
			mnuCopyObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Copy_Objects_Tooltip;
			mnuMoveAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Move_Album_Tooltip;
			mnuCopyAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Copy_Album_Tooltip;
			mnuDownloadObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Download_Objects_Tooltip;
			mnuEditCaptions.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Edit_Captions_Tooltip;
			mnuAssignThumbnail.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Assign_Thumbnail_Tooltip;
			mnuRearrangeObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Rearrange_Tooltip;
			mnuRotate.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Rotate_Tooltip;
			mnuDeleteObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Delete_Objects_Tooltip;
			mnuDeleteHiRes.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Delete_HiRes_Tooltip;
			mnuDeleteAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Delete_Album_Tooltip;
			mnuSynch.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Synchronize_Tooltip;
			mnuSiteSettings.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Admin_Console_Tooltip;
			mnuLogout.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Logout_Tooltip;
		}

		private void SetNavigationUrls()
		{
			mnuCreateAlbum.NavigateUrl = Util.GetUrl(PageId.task_createalbum, "aid={0}", this.AlbumId);
			mnuEditAlbum.ClientSideCommand = "if (window.editAlbumInfo) editAlbumInfo();";
			mnuAddObjects.NavigateUrl = Util.GetUrl(PageId.task_addobjects, "aid={0}", this.AlbumId);
			mnuMoveObjects.NavigateUrl = Util.GetUrl(PageId.task_transferobject, "aid={0}&tt=move&skipstep1=false", this.AlbumId);
			mnuCopyObjects.NavigateUrl = Util.GetUrl(PageId.task_transferobject, "aid={0}&tt=copy&skipstep1=false", this.AlbumId);
			mnuMoveAlbum.NavigateUrl = Util.GetUrl(PageId.task_transferobject, "aid={0}&tt=move&skipstep1=true", this.AlbumId);
			mnuCopyAlbum.NavigateUrl = Util.GetUrl(PageId.task_transferobject, "aid={0}&tt=copy&skipstep1=true", this.AlbumId);
			mnuDownloadObjects.NavigateUrl = Util.GetUrl(PageId.task_downloadobjects, "aid={0}", this.AlbumId);
			mnuEditCaptions.NavigateUrl = Util.GetUrl(PageId.task_editcaptions, "aid={0}", this.AlbumId);
			mnuAssignThumbnail.NavigateUrl = Util.GetUrl(PageId.task_assignthumbnail, "aid={0}", this.AlbumId);
			mnuRearrangeObjects.NavigateUrl = Util.GetUrl(PageId.task_rearrange, "aid={0}", this.AlbumId);
			mnuRotate.NavigateUrl = Util.GetUrl(PageId.task_rotateimages, "aid={0}", this.AlbumId);
			mnuDeleteObjects.NavigateUrl = Util.GetUrl(PageId.task_deleteobjects, "aid={0}", this.AlbumId);
			mnuDeleteHiRes.NavigateUrl = Util.GetUrl(PageId.task_deletehires, "aid={0}", this.AlbumId);
			mnuDeleteAlbum.NavigateUrl = Util.GetUrl(PageId.task_deletealbum, "aid={0}", this.AlbumId);
			mnuSynch.NavigateUrl = Util.GetUrl(PageId.task_synchronize, "aid={0}", this.AlbumId);
			mnuSiteSettings.NavigateUrl = Util.GetUrl(PageId.admin_general, "aid={0}", this.AlbumId);
			//mnuLogout.NavigateUrl = // No navigation URL. Instead we have an event handler (Menu1_ItemSelected)
		}

		private void EnableOrDisableMenuItemsBasedOnUserPermission()
		{
			mnuCreateAlbum.Enabled = this.GalleryPage.UserCanAddAlbumToAtLeastOneAlbum;
			//mnuEditAlbum.Enabled = XXX // We set this in javascript during page load based on whether the function editAlbumInfo exists
			mnuAddObjects.Enabled = this.GalleryPage.UserCanAddMediaObject;
			mnuMoveObjects.Enabled = this.GalleryPage.UserCanDeleteMediaObject;

			if (this.GalleryPage.Core.AllowCopyingReadOnlyObjects)
			{
				mnuCopyObjects.Enabled = (this.GalleryPage.UserCanAddAlbumToAtLeastOneAlbum || this.GalleryPage.UserCanAddMediaObjectToAtLeastOneAlbum);
				mnuCopyAlbum.Enabled = (!this.GalleryPage.GetAlbum().IsRootAlbum && this.GalleryPage.UserCanAddAlbumToAtLeastOneAlbum);
			}
			else
			{
				mnuCopyObjects.Enabled = (this.GalleryPage.UserCanAddMediaObject);
				mnuCopyAlbum.Enabled = (!this.GalleryPage.GetAlbum().IsRootAlbum && this.GalleryPage.UserCanCreateAlbum);
			}

			mnuMoveAlbum.Enabled = (!this.GalleryPage.GetAlbum().IsRootAlbum && this.GalleryPage.UserCanDeleteCurrentAlbum);
			mnuDownloadObjects.Enabled = this.GalleryPage.Core.EnableMediaObjectZipDownload;
			mnuEditCaptions.Enabled = this.GalleryPage.UserCanEditMediaObject;
			mnuAssignThumbnail.Enabled = this.GalleryPage.UserCanEditAlbum;
			mnuRearrangeObjects.Enabled = this.GalleryPage.UserCanEditAlbum;
			mnuRotate.Enabled = this.GalleryPage.UserCanEditMediaObject;
			mnuDeleteObjects.Enabled = this.GalleryPage.UserCanDeleteMediaObject;
			mnuDeleteHiRes.Enabled = this.GalleryPage.UserCanEditMediaObject;
			mnuDeleteAlbum.Enabled = this.GalleryPage.UserCanDeleteCurrentAlbum;
			mnuSynch.Enabled = this.GalleryPage.UserCanSynchronize;
			mnuSiteSettings.Enabled = this.GalleryPage.UserCanAdministerSite;

			string noPermissionTooltip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_Insufficient_Permission_Tooltip;

			if (!mnuCreateAlbum.Enabled)
				mnuCreateAlbum.ToolTip = noPermissionTooltip;

			if (!mnuAddObjects.Enabled)
				mnuAddObjects.ToolTip = noPermissionTooltip;

			if (!mnuMoveObjects.Enabled)
				mnuMoveObjects.ToolTip = noPermissionTooltip;

			if (!mnuCopyObjects.Enabled)
				mnuCopyObjects.ToolTip = noPermissionTooltip;

			if (!mnuMoveAlbum.Enabled)
				mnuMoveAlbum.ToolTip = noPermissionTooltip;

			if (!mnuCopyAlbum.Enabled)
				mnuCopyAlbum.ToolTip = noPermissionTooltip;

			if (!mnuEditCaptions.Enabled)
				mnuEditCaptions.ToolTip = noPermissionTooltip;
	
			if (!mnuAssignThumbnail.Enabled)
				mnuAssignThumbnail.ToolTip = noPermissionTooltip;

			if (!mnuRearrangeObjects.Enabled)
				mnuRearrangeObjects.ToolTip = noPermissionTooltip;

			if (!mnuRotate.Enabled)
				mnuRotate.ToolTip = noPermissionTooltip;

			if (!mnuDeleteObjects.Enabled)
				mnuDeleteObjects.ToolTip = noPermissionTooltip;

			if (!mnuDeleteHiRes.Enabled)
				mnuDeleteHiRes.ToolTip = noPermissionTooltip;
	
			if (!mnuDeleteAlbum.Enabled)
				mnuDeleteAlbum.ToolTip = noPermissionTooltip;

			if (!mnuSynch.Enabled)
				mnuSynch.ToolTip = noPermissionTooltip;

			if (!mnuSiteSettings.Enabled)
				mnuSiteSettings.ToolTip = noPermissionTooltip;
	}

		private void EnableOrDisableMenuItemsBasedOnWriteAccess()
		{
			bool isReadOnly = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectPathIsReadOnly;

			if (isReadOnly)
			{
				mnuCreateAlbum.Enabled = false;
				mnuAddObjects.Enabled = false;
				mnuMoveObjects.Enabled = false;
				mnuCopyObjects.Enabled = false;
				mnuMoveAlbum.Enabled = false;
				mnuCopyAlbum.Enabled = false;
				mnuRotate.Enabled = false;
				mnuDeleteObjects.Enabled = false;
				mnuDeleteHiRes.Enabled = false;
				mnuDeleteAlbum.Enabled = false;

				mnuCreateAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuAddObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuMoveObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuCopyObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuMoveAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuCopyAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuRotate.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuDeleteObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuDeleteHiRes.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
				mnuDeleteAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Disabled_ReadOnly_Tooltip;
			}
		}

		private static void LogOffUser()
		{
			System.Web.Security.FormsAuthentication.SignOut();

			Controller.UserController.UserLoggedOff();
		}

		private void ReloadPage()
		{
			// If currently looking at a media object, reload page with same query string parameters (but any
			// messages stripped out).
			if (this.GalleryPage.MediaObjectId > int.MinValue)
			{
				string url = Request.Url.PathAndQuery;

				url = Util.RemoveQueryStringParameter(url, "msg"); // Remove any messages
				url = Util.RemoveQueryStringParameter(Request.Url.PathAndQuery, "moid");
				url = Util.AddQueryStringParameter(url, String.Concat("moid=", this.GalleryPage.MediaObjectId));
				Util.Redirect(url);
			}

			// If not looking at a media object, redirect to current album.
			Util.Redirect(PageId.album, "aid={0}", this.GalleryPage.AlbumId);
		}

		#endregion
	}
}
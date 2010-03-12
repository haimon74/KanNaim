using System;

using System.Globalization;

namespace GalleryServerPro.Web.uc
{
	public partial class actionmenu : System.Web.UI.UserControl
	{
		#region Private Fields

		int _albumId = int.MinValue;
		int _mediaObjectId = int.MinValue;
		
		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!cbDummy.IsCallback)
			{
				ConfigureMenu();
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

		public int AlbumId
		{
			get
			{
				if (this._albumId == int.MinValue)
				{
					this._albumId = this.PageBase.AlbumId;
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
					this._mediaObjectId = this.PageBase.MediaObjectId;
				}

				return this._mediaObjectId;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureMenu()
		{
			SetMenuItemsText();

			SetUrlTooltips();

			SetNavigationUrls();

			EnableOrDisableMenuItemsBasedOnUserPermission();
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
			mnuEditCaptions.Text = Resources.GalleryServerPro.UC_ActionMenu_Edit_Captions_Text;
			mnuAssignThumbnail.Text = Resources.GalleryServerPro.UC_ActionMenu_Assign_Thumbnail_Text;
			mnuRearrangeObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Rearrange_Text;
			mnuRotate.Text = Resources.GalleryServerPro.UC_ActionMenu_Rotate_Text;
			mnuDeleteObjects.Text = Resources.GalleryServerPro.UC_ActionMenu_Delete_Objects_Text;
			mnuDeleteHiRes.Text = Resources.GalleryServerPro.UC_ActionMenu_Delete_HiRes_Text;
			mnuDeleteAlbum.Text = Resources.GalleryServerPro.UC_ActionMenu_Delete_Album_Text;
			mnuSynch.Text = Resources.GalleryServerPro.UC_ActionMenu_Synchronize_Text;
			mnuSiteSettings.Text = Resources.GalleryServerPro.UC_ActionMenu_Admin_Console_Text;
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
			mnuEditCaptions.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Edit_Captions_Tooltip;
			mnuAssignThumbnail.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Assign_Thumbnail_Tooltip;
			mnuRearrangeObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Rearrange_Tooltip;
			mnuRotate.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Rotate_Tooltip;
			mnuDeleteObjects.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Delete_Objects_Tooltip;
			mnuDeleteHiRes.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Delete_HiRes_Tooltip;
			mnuDeleteAlbum.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Delete_Album_Tooltip;
			mnuSynch.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Synchronize_Tooltip;
			mnuSiteSettings.ToolTip = Resources.GalleryServerPro.UC_ActionMenu_Admin_Console_Tooltip;
		}

		private void SetNavigationUrls()
		{
			mnuCreateAlbum.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/createalbum.aspx?aid={0}", this.AlbumId);
			mnuEditAlbum.ClientSideCommand = "if (window.editAlbumInfo) editAlbumInfo();";
			mnuAddObjects.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/addobjects.aspx?aid={0}", this.AlbumId);
			mnuMoveObjects.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/transferobject.aspx?aid={0}&tt=move&skipstep1=false", this.AlbumId);
			mnuCopyObjects.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/transferobject.aspx?aid={0}&tt=copy&skipstep1=false", this.AlbumId);
			mnuMoveAlbum.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/transferobject.aspx?aid={0}&tt=move&skipstep1=true", this.AlbumId);
			mnuCopyAlbum.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/transferobject.aspx?aid={0}&tt=copy&skipstep1=true", this.AlbumId);
			mnuEditCaptions.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/editcaptions.aspx?aid={0}", this.AlbumId);
			mnuAssignThumbnail.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/assignthumbnail.aspx?aid={0}", this.AlbumId);
			mnuRearrangeObjects.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/rearrange.aspx?aid={0}", this.AlbumId);
			mnuRotate.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/rotateimages.aspx?aid={0}", this.AlbumId);
			mnuDeleteObjects.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/deleteobjects.aspx?aid={0}", this.AlbumId);
			mnuDeleteHiRes.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/deletehires.aspx?aid={0}", this.AlbumId);
			mnuDeleteAlbum.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/deletealbum.aspx?aid={0}", this.AlbumId);
			mnuSynch.NavigateUrl = String.Format(CultureInfo.CurrentCulture, "~/task/synchronize.aspx?aid={0}", this.AlbumId);
			mnuSiteSettings.NavigateUrl = "~/admin/default.aspx";
		}

		private void EnableOrDisableMenuItemsBasedOnUserPermission()
		{
			mnuCreateAlbum.Enabled = this.PageBase.UserCanAddAlbumToAtLeastOneAlbum;
			//mnuEditAlbum.Enabled = XXX // We set this in javascript during page load based on whether the function editAlbumInfo exists
			mnuAddObjects.Enabled = this.PageBase.UserCanAddMediaObject;
			mnuMoveObjects.Enabled = this.PageBase.UserCanDeleteMediaObject;
			mnuCopyObjects.Enabled = this.PageBase.UserCanAddAlbumToAtLeastOneAlbum || this.PageBase.UserCanAddMediaObjectToAtLeastOneAlbum;
			mnuMoveAlbum.Enabled = (!this.PageBase.GetAlbum().IsRootAlbum && this.PageBase.UserCanDeleteCurrentAlbum);
      mnuCopyAlbum.Enabled = (!this.PageBase.GetAlbum().IsRootAlbum && this.PageBase.UserCanAddAlbumToAtLeastOneAlbum);
			mnuEditCaptions.Enabled = this.PageBase.UserCanEditMediaObject;
			mnuAssignThumbnail.Enabled = this.PageBase.UserCanEditAlbum;
			mnuRearrangeObjects.Enabled = this.PageBase.UserCanEditAlbum;
			mnuRotate.Enabled = this.PageBase.UserCanEditMediaObject;
			mnuDeleteObjects.Enabled = this.PageBase.UserCanDeleteMediaObject;
			mnuDeleteHiRes.Enabled = this.PageBase.UserCanEditMediaObject;
			mnuDeleteAlbum.Enabled = this.PageBase.UserCanDeleteCurrentAlbum;
			mnuSynch.Enabled = this.PageBase.UserCanSynchronize;
			mnuSiteSettings.Enabled = this.PageBase.UserCanAdministerSite;
		}

		#endregion
	}
}
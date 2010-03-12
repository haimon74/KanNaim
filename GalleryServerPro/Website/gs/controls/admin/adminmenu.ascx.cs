using System;

namespace GalleryServerPro.Web.Controls.admin
{
	public partial class adminmenu : GalleryUserControl
	{
		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControlsEveryTime();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			nbAdminMenu.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/navbar/");

			nbiGeneral.NavigateUrl = Util.GetUrl(PageId.admin_general, "aid={0}", GalleryPage.AlbumId);
			nbiEmail.NavigateUrl = Util.GetUrl(PageId.admin_email, "aid={0}", GalleryPage.AlbumId);
			nbiBackupRestore.NavigateUrl = Util.GetUrl(PageId.admin_backuprestore, "aid={0}", GalleryPage.AlbumId);
			nbiErrorLog.NavigateUrl = Util.GetUrl(PageId.admin_errorlog, "aid={0}", GalleryPage.AlbumId);
			nbiUserSettings.NavigateUrl = Util.GetUrl(PageId.admin_usersettings, "aid={0}", GalleryPage.AlbumId);
			nbiManageUsers.NavigateUrl = Util.GetUrl(PageId.admin_manageusers, "aid={0}", GalleryPage.AlbumId);
			nbiManageRoles.NavigateUrl = Util.GetUrl(PageId.admin_manageroles, "aid={0}", GalleryPage.AlbumId);
			nbiAlbums.NavigateUrl = Util.GetUrl(PageId.admin_albums, "aid={0}", GalleryPage.AlbumId);
			nbiMediaObjects.NavigateUrl = Util.GetUrl(PageId.admin_mediaobjects, "aid={0}", GalleryPage.AlbumId);
			nbiMediaObjectTypes.NavigateUrl = Util.GetUrl(PageId.admin_mediaobjecttypes, "aid={0}", GalleryPage.AlbumId);
			nbiImages.NavigateUrl = Util.GetUrl(PageId.admin_images, "aid={0}", GalleryPage.AlbumId);
			nbiVideoAudioOther.NavigateUrl = Util.GetUrl(PageId.admin_videoaudioother, "aid={0}", GalleryPage.AlbumId);
		}

		#endregion
	}
}
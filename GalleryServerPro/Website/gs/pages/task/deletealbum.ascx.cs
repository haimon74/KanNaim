using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.task
{
	public partial class deletealbum : Pages.TaskPage
	{
		#region Event Handlers

		protected void Page_Init(object sender, EventArgs e)
		{
			this.TaskHeaderPlaceHolder = phTaskHeader;
			this.TaskFooterPlaceHolder = phTaskFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Core.MediaObjectPathIsReadOnly)
				RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotEditGalleryIsReadOnly).ToString(CultureInfo.InvariantCulture));

			this.CheckUserSecurity(SecurityActions.DeleteAlbum);

			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				int parentAlbumId = this.GetAlbum().Parent.Id;

				if (btnOkClicked())
					Util.Redirect(PageId.album, "aid={0}", parentAlbumId);
			}

			return true;
		}

		#endregion

		#region Public Properties

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			this.TaskHeaderText = Resources.GalleryServerPro.Task_Delete_Album_Header_Text;
			this.TaskBodyText = Resources.GalleryServerPro.Task_Delete_Album_Body_Text;
			this.OkButtonText = Resources.GalleryServerPro.Task_Delete_Album_Ok_Button_Text;
			this.OkButtonToolTip = Resources.GalleryServerPro.Task_Delete_Album_Ok_Button_Tooltip;

			this.PageTitle = Resources.GalleryServerPro.Task_Delete_Album_Page_Title;
		}

		private bool btnOkClicked()
		{
			//User clicked 'Delete album'.
			try
			{
				AlbumController.DeleteAlbum(this.GetAlbum());

				HelperFunctions.PurgeCache();

				return true;
			}
			catch (ErrorHandler.CustomExceptions.CannotDeleteAlbumException ex)
			{
				ucUserMessage.MessageTitle = Resources.GalleryServerPro.Task_Delete_Album_Cannot_Delete_Contains_User_Album_Parent_Hdr;
				ucUserMessage.MessageDetail = ex.Message;
				ucUserMessage.Visible = true;

				return false;
			}
		}

		#endregion
	}
}
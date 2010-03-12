using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;

namespace GalleryServerPro.Web.task
{
	public partial class deletealbum : GspPage
	{
		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
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

				btnOkClicked();

				this.RedirectToAlbumViewPage("aid", parentAlbumId.ToString(CultureInfo.InvariantCulture));
			}

			return true;
		}

		#endregion

		#region Public Properties

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Delete_Album_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Delete_Album_Body_Text;
			Master.OkButtonText = Resources.GalleryServerPro.Task_Delete_Album_Ok_Button_Text;
			Master.OkButtonToolTip = Resources.GalleryServerPro.Task_Delete_Album_Ok_Button_Tooltip;
		}

		private void btnOkClicked()
		{
			//User clicked 'Delete album'.
			this.GetAlbum().Delete();
			
			HelperFunctions.PurgeCache();
		}

		#endregion
	}
}

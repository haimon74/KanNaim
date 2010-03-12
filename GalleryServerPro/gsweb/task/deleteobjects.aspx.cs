using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.task
{
	public partial class deleteobjects : GspPage
	{
		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.DeleteMediaObject);

			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from a control has bubbled up.  If it's the Ok button, then run the
			//code to synchronize; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				if (btnOkClicked())
				{
					this.RedirectToAlbumViewPage("msg", ((int)Message.ObjectsSuccessfullyDeleted).ToString(CultureInfo.InvariantCulture));
				}
			}

			return true;
		}

		#endregion

		#region Protected Methods

		protected static string GetTitle(string title)
		{
			// Truncate the Title if it is too long
			int maxLength = WebsiteController.GetGalleryServerProConfigSection().Core.MaxMediaObjectThumbnailTitleDisplayLength;
			string truncatedText = WebsiteController.TruncateTextForWeb(title, maxLength);
			string titleText;
			if (truncatedText.Length != title.Length)
				titleText = string.Format(CultureInfo.CurrentCulture, "{0}...", truncatedText);
			else
				titleText = truncatedText;

			return titleText;
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Delete_Objects_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Delete_Objects_Body_Text;
			Master.OkButtonText = Resources.GalleryServerPro.Task_Delete_Objects_Ok_Button_Text;
			Master.OkButtonToolTip = Resources.GalleryServerPro.Task_Delete_Objects_Ok_Button_Tooltip;

			IGalleryObjectCollection albumChildren = this.WebController.GetAlbum().GetChildGalleryObjects(GalleryObjectType.MediaObject, true);

			if (albumChildren.Count > 0)
			{
				SetThumbnailCssStyle(albumChildren);

				rptr.DataSource = albumChildren;
				rptr.DataBind();
			}
			else
			{
				this.RedirectToAlbumViewPage("msg", ((int)Message.CannotDeleteObjectsNoObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
			}
		}

		private bool btnOkClicked()
		{
			//User clicked 'Delete images'.  Delete the selected images.
			List<int> selectedItems = retrieveUserSelections();
			if (selectedItems.Count == 0)
			{
				// No images were selected. Inform user and exit function.
				string msg = String.Format(CultureInfo.CurrentCulture, "<p class='msgwarning'><span class='bold'>{0} </span>{1}</p>", Resources.GalleryServerPro.Task_Delete_Objects_No_Objects_Selected_Hdr, Resources.GalleryServerPro.Task_Delete_Objects_No_Objects_Selected_Dtl);
				phMsg.Controls.Clear();
				phMsg.Controls.Add(new System.Web.UI.LiteralControl(msg));

				return false;
			}

			try
			{
				HelperFunctions.BeginTransaction();
				foreach (int moid in selectedItems)
				{
					IGalleryObject mo = Factory.LoadMediaObjectInstance(moid);
					mo.Delete();
				}
				HelperFunctions.CommitTransaction();
			}
			catch
			{
				HelperFunctions.RollbackTransaction();
				throw;
			}

			HelperFunctions.PurgeCache();

			return true;
		}

		private List<int> retrieveUserSelections()
		{
			// Iterate through all the checkboxes, saving checked ones to an array.
			// The media object IDs are stored in a hidden input tag.
			CheckBox chkbx;
			HtmlInputHidden gc;
			List<int> moids = new List<int>();

			// Loop through each item in the repeater control. If an item is checked, extract the ID.
			foreach (RepeaterItem rptrItem in rptr.Items)
			{
				chkbx = (CheckBox)rptrItem.Controls[1]; // The <INPUT TYPE="CHECKBOX"> tag
				if (chkbx.Checked)
				{
					// Checkbox is checked. Save media object ID to array.
					gc = (HtmlInputHidden)rptrItem.Controls[3]; // The hidden <input> tag
					moids.Add(Convert.ToInt32(gc.Value, CultureInfo.InvariantCulture));
				}
			}

			return moids;
		}

		#endregion
	}
}

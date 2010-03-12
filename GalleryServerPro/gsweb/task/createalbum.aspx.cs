using System;
using System.Globalization;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.task
{
	public partial class createalbum : GspPage
	{
		#region Private Fields

		private int _msgId;
		private int _currentAlbumId = int.MinValue;

		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the album ID for the current album.
		/// </summary>
		public int CurrentAlbumId
		{
			get
			{
				if (this._currentAlbumId == int.MinValue)
					this._currentAlbumId = this.GetAlbum().Id;

				return this._currentAlbumId;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Create_Album_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Create_Album_Body_Text;
			Master.OkButtonText = Resources.GalleryServerPro.Task_Create_Album_Ok_Button_Text;
			Master.OkButtonToolTip = Resources.GalleryServerPro.Task_Create_Album_Ok_Button_Tooltip;

			txtTitle.MaxLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;

			lblMaxTitleLengthInfo.Text = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Create_Album_Title_Max_Length_Text,
				GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength.ToString(CultureInfo.InvariantCulture));

			tvUC.RequiredSecurityPermissions = SecurityActions.AddChildAlbum;
			tvUC.DataBind(this.GetAlbum());

			this.Page.Form.DefaultFocus = txtTitle.ClientID;
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				if (Page.IsValid)
				{
					int newAlbumID = btnOkClicked();

					redirectToNewAlbumPage(newAlbumID); // Redirect to the page for the newly created album.
				}
			}

			return true;
		}

		private int btnOkClicked()
		{
			//User clicked 'Create album'. Create the new album and return the new album ID.
			TreeViewNode selectedNode = tvUC.SelectedNode;
			int parentAlbumID = Int32.Parse(selectedNode.Value, CultureInfo.InvariantCulture);
			IAlbum parentAlbum = Factory.LoadAlbumInstance(parentAlbumID, false);

			this.CheckUserSecurity(SecurityActions.AddChildAlbum, parentAlbum);
			
			int newAlbumID;

			if (parentAlbumID > 0)
			{
				IAlbum newAlbum = Factory.CreateAlbumInstance();
				newAlbum.Title = GetAlbumTitle();
				//newAlbum.ThumbnailMediaObjectId = 0; // not needed
				newAlbum.Parent = parentAlbum;
				newAlbum.IsPrivate = parentAlbum.IsPrivate;
				WebsiteController.SaveGalleryObject(newAlbum);
				newAlbumID = newAlbum.Id;

				HelperFunctions.PurgeCache();
			}
			else
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.InvalidAlbumException(parentAlbumID);

			return newAlbumID;
		}

		private string GetAlbumTitle()
		{
			// Get the title the user entered for this album. If the length exceeds our maximum, set the messageId
			// variable so that the receiving page is notified of the situation.
			string title = WebsiteController.CleanHtmlTags(txtTitle.Text.Trim());

			int maxTitleLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;

			if (title.Length > maxTitleLength)
			{
				title = title.Substring(0, maxTitleLength).Trim();
				_msgId = (int) GalleryServerPro.Web.Message.AlbumNameExceededMaxLength;
			}

			return title;
		}

		private void redirectToNewAlbumPage(int newAlbumID)
		{
			string url = "../default.aspx?aid=" + newAlbumID;

			// If we have a message to show the user (msgID > 0), then add it to the query string.
			url = (_msgId > 0) ? WebsiteController.AddQueryStringParameter(url, "msg=" + _msgId) : url;

			Response.Redirect(url, false);
			System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		#endregion
	}
}

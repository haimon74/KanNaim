using System;
using System.Globalization;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.task
{
	public partial class createalbum : Pages.TaskPage
	{
		#region Private Fields

		private int _msgId;
		private int _currentAlbumId = int.MinValue;

		#endregion

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
				if (Page.IsValid)
				{
					int newAlbumID = btnOkClicked();

					redirectToNewAlbumPage(newAlbumID); // Redirect to the page for the newly created album.
				}
			}

			return true;
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
					this._currentAlbumId = this.AlbumId;

				return this._currentAlbumId;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			this.TaskHeaderText = Resources.GalleryServerPro.Task_Create_Album_Header_Text;
			this.TaskBodyText = Resources.GalleryServerPro.Task_Create_Album_Body_Text;
			this.OkButtonText = Resources.GalleryServerPro.Task_Create_Album_Ok_Button_Text;
			this.OkButtonToolTip = Resources.GalleryServerPro.Task_Create_Album_Ok_Button_Tooltip;

			this.PageTitle = Resources.GalleryServerPro.Task_Create_Album_Page_Title;

			txtTitle.MaxLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;

			lblMaxTitleLengthInfo.Text = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Create_Album_Title_Max_Length_Text,
				GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength.ToString(CultureInfo.InvariantCulture));

			tvUC.RequiredSecurityPermissions = SecurityActions.AddChildAlbum;

			if (this.GetAlbum().IsPrivate)
			{
				chkIsPrivate.Checked = true;
				chkIsPrivate.Enabled = false;
				lblPrivateAlbumIsInherited.Text = Resources.GalleryServerPro.Task_Create_Album_Is_Private_Disabled_Text;
			}

			IAlbum albumToSelect = this.GetAlbum();
			if (!IsUserAuthorized(SecurityActions.AddChildAlbum, albumToSelect))
			{
				albumToSelect = AlbumController.GetHighestLevelAlbumWithCreatePermission();
			}

			if (albumToSelect == null)
				tvUC.BindTreeView();
			else
				tvUC.BindTreeView(albumToSelect);

			this.Page.Form.DefaultFocus = txtTitle.ClientID;
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
				newAlbum.IsPrivate = (parentAlbum.IsPrivate ? true : chkIsPrivate.Checked);
				GalleryObjectController.SaveGalleryObject(newAlbum);
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
			string title = Util.CleanHtmlTags(txtTitle.Text.Trim());

			int maxTitleLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;

			if (title.Length > maxTitleLength)
			{
				title = title.Substring(0, maxTitleLength).Trim();
				_msgId = (int)GalleryServerPro.Web.Message.AlbumNameExceededMaxLength;
			}

			return title;
		}

		private void redirectToNewAlbumPage(int newAlbumID)
		{
			if (_msgId > 0)
				Util.Redirect(PageId.album, "aid={0}&msg={1}", newAlbumID, _msgId);
			else
				Util.Redirect(PageId.album, "aid={0}", newAlbumID);
		}

		#endregion
	}
}
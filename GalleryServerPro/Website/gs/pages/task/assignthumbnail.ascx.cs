using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.task
{
	public partial class assignthumbnail : Pages.TaskPage
	{
		#region Private Fields

		private int _thumbnailMediaObjectId;

		#endregion
		
		#region Event Handlers

		protected void Page_Init(object sender, EventArgs e)
		{
			this.TaskHeaderPlaceHolder = phTaskHeader;
			this.TaskFooterPlaceHolder = phTaskFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.EditAlbum);

			if (!IsPostBack)
			{
				ConfigureControls();
			}

			if ((this.GetAlbum() != null) && (this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.MediaObject).Count == 0))
			{
				this.RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotAssignThumbnailNoObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
			}

			// Import javascript to help the radio button work properly. See KB 316495 at microsoft.com
			// for more info on the bug. The javascript work-around was posted by a MS employee on an aspnet
			// newsgroup and can be seen here: http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&selm=w5qH8dXEEHA.616%40cpmsftngxa06.phx.gbl&rnum=2
			// Add reference to entityobjects.js.
			string scriptUrl = Util.GetUrl("/script/radiobuttonworkaround.js");
			ScriptManager sm = ScriptManager.GetCurrent(this.Page);
			if (sm != null)
				sm.Scripts.Add(new ScriptReference(scriptUrl));
			else
				throw new WebException("Gallery Server Pro requires a ScriptManager on the page.");
		}

		#endregion

		#region Protected Methods

		protected static string GetTitle(string title)
		{
			// Truncate the Title if it is too long
			int maxLength = Config.GetCore().MaxMediaObjectThumbnailTitleDisplayLength;
			string truncatedText = Util.TruncateTextForWeb(title, maxLength);
			string titleText;
			if (truncatedText.Length != title.Length)
				titleText = string.Format(CultureInfo.CurrentCulture, "{0}...", truncatedText);
			else
				titleText = truncatedText;

			return titleText;
		}

		/// <summary>
		/// Determines whether the specified media object ID is the currently assigned thumbnail for this album.
		/// </summary>
		/// <param name="mediaObjectId">A media object ID for which to determine whether it matches the currently assigned thumbnail
		/// for this album.</param>
		/// <returns>Returns true if the specified media object ID is the currently assigned thumbnail for this album; 
		/// otherwise returns false.</returns>
		protected bool IsAlbumThumbnail(int mediaObjectId)
		{
			return (this._thumbnailMediaObjectId == mediaObjectId);
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				btnOkClicked();

				this.RedirectToAlbumViewPage("msg={0}", ((int)Message.ThumbnailSuccessfullyAssigned).ToString(CultureInfo.InvariantCulture));
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			this.TaskHeaderText = Resources.GalleryServerPro.Task_Assign_Thumbnail_Header_Text;
			this.TaskBodyText = Resources.GalleryServerPro.Task_Assign_Thumbnail_Body_Text;
			this.OkButtonText = Resources.GalleryServerPro.Task_AssignThumbnail_Ok_Button_Text;
			this.OkButtonToolTip = Resources.GalleryServerPro.Task_AssignThumbnail_Ok_Button_Tooltip;

			this.PageTitle = Resources.GalleryServerPro.Task_AssignThumbnail_Page_Title;

			this._thumbnailMediaObjectId = this.GetAlbum().Thumbnail.MediaObjectId;

			IGalleryObjectCollection albumChildren = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.MediaObject, true);

			SetThumbnailCssStyle(albumChildren);

			rptr.DataSource = albumChildren;
			rptr.DataBind();
		}

		private void btnOkClicked()
		{
			//User clicked 'Assign thumbnail'.  Assign the specified thumbnail to this album.
			int moid = getSelectedMediaObjectID();

			IAlbum album = this.GetAlbum();
			album.ThumbnailMediaObjectId = moid;
			GalleryObjectController.SaveGalleryObject(album);

			HelperFunctions.PurgeCache();
		}

		private int getSelectedMediaObjectID()
		{
			// Return the media object ID for the object the user selected. Do this by
			// iterating through all the radio buttons until we get to a checked one.
			// The media object IDs are stored in a hidden input tag.
			RadioButton rb;
			HtmlInputHidden gc;
			int rv = 0;

			// Loop through each item in the repeater control. If an item is checked, extract the ID.
			foreach (RepeaterItem rptrItem in rptr.Items)
			{
				rb = (RadioButton)rptrItem.Controls[1]; // The <INPUT TYPE="RADIO"> tag
				if (rb.Checked)
				{
					// RadioButton is checked. Get media object ID.
					gc = (HtmlInputHidden)rptrItem.Controls[2]; // The hidden <input> tag
					rv = Convert.ToInt32(gc.Value, CultureInfo.InvariantCulture);
					break;
				}
			}

			return rv;
		}

		#endregion
	}
}
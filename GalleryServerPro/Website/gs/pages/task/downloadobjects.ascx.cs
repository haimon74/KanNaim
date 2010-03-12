using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.gs.pages.task
{
	public partial class downloadobjects : Pages.TaskPage
	{		
		#region Private Fields

		private IGalleryObjectCollection _galleryObjects;

		private const DisplayObjectType DEFAULT_IMAGE_SIZE = DisplayObjectType.Optimized;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the gallery objects that are candidates for deleting.
		/// </summary>
		/// <value>The gallery objects that are candidates for deleting.</value>
		public IGalleryObjectCollection GalleryObjects
		{
			get
			{
				if (this._galleryObjects == null)
				{
					this._galleryObjects = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.All, true);
				}

				return this._galleryObjects;
			}
		}

		#endregion

		#region Event Handlers

		protected void Page_Init(object sender, EventArgs e)
		{
			this.TaskHeaderPlaceHolder = phTaskHeader;
			this.TaskFooterPlaceHolder = phTaskFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// We do not need to verify user can view objects because the GalleryPage.GetAlbum method will return ONLY
			// those objects user can view.
			//this.CheckUserSecurity(SecurityActions.ViewAlbumOrMediaObject);

			if (!Core.EnableMediaObjectZipDownload)
				RedirectToAlbumViewPage();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}

			ConfigureControlsEveryPageLoad();
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from a control has bubbled up.  If it's the Ok button, then run the
			//code to synchronize; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				btnOkClicked();
			}

			return true;
		}

		private void btnOkClicked()
		{
			// User clicked the download button. Gather selected items, build a ZIP file, and send to user.
			List<int> albumIds, mediaObjectIds;
			RetrieveUserSelections(out albumIds, out mediaObjectIds);

			if ((albumIds.Count == 0) && (mediaObjectIds.Count == 0))
			{
				// No objects were selected. Inform user and exit function.
				string msg = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_msgwarning'><span class='gsp_bold'>{0} </span>{1}</p>", Resources.GalleryServerPro.Task_No_Objects_Selected_Hdr, Resources.GalleryServerPro.Task_No_Objects_Selected_Dtl);
				phMsg.Controls.Clear();
				phMsg.Controls.Add(new System.Web.UI.LiteralControl(msg));

				return;
			}

			BuildAndSendZipFile(albumIds, mediaObjectIds);
		}

		private void BuildAndSendZipFile(List<int> albumIds, List<int> mediaObjectIds) 
		{
			IMimeType mimeType = MimeType.LoadInstanceByFilePath("dummy.zip");
			string zipFilename = Util.UrlEncode("Media Files".Replace(" ", "_"));

			ZipUtility zip = new ZipUtility(Util.UserName, Controller.RoleController.GetGalleryServerRolesForUser());

			int bufferSize = Core.MediaObjectDownloadBufferSize;
			byte[] buffer = new byte[bufferSize];

			Stream stream = null;
			try
			{
				// Create an in-memory ZIP file.
				stream = zip.CreateZipStream(this.AlbumId, albumIds, mediaObjectIds, GetImageSize());

				// Send to user.
				Response.AddHeader("Content-Disposition", "attachment; filename=" + zipFilename + ".zip");

				Response.Clear();
				Response.ContentType = (mimeType != null ? mimeType.FullType : "application/octet-stream");
				Response.Buffer = false;

				stream.Position = 0;
				int byteCount;
				while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
				{
					if (Response.IsClientConnected)
					{
						Response.OutputStream.Write(buffer, 0, byteCount);
						Response.Flush();
					}
					else
					{
						return;
					}
				}
			}
			finally
			{
				if (stream != null)
					stream.Close();

				Response.End();
			}
		}

		private DisplayObjectType GetImageSize() 
		{
			DisplayObjectType displayType = DEFAULT_IMAGE_SIZE;

			try
			{
				displayType = (DisplayObjectType)Convert.ToInt32(this.ddlImageSize.SelectedValue);
			}
			catch(FormatException) { } // Suppress any parse errors
			catch(OverflowException) { } // Suppress any parse errors
			catch(ArgumentOutOfRangeException) { } // Suppress any parse errors
			
			if (!DisplayObjectTypeEnumHelper.IsValidDisplayObjectType(displayType))
			{
				displayType = DEFAULT_IMAGE_SIZE;
			}

			if ((displayType == DisplayObjectType.Original) && (!this.IsUserAuthorized(SecurityActions.ViewOriginalImage)))
			{
				displayType = DEFAULT_IMAGE_SIZE;
			}

			return displayType;
		}

		protected static string GetThumbnailCssClass(Type galleryObjectType)
		{
			// If it's an album then specify the appropriate CSS class so that the "Album"
			// header appears over the thumbnail. This is to indicate to the user that the
			// thumbnail represents an album.
			if (galleryObjectType == typeof(Album))
				return "thmb album";
			else
				return "thmb";
		}

		/// <summary>
		/// Return an HTML formatted string representing the title of the gallery object. It is truncated and purged of HTML tags
		/// if necessary.
		/// </summary>
		/// <param name="title">The title of the gallery object as stored in the data store.</param>
		/// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
		/// <returns>Returns a string representing the title of the media object. It is truncated and purged of HTML tags
		/// if necessary.</returns>
		protected static string GetGalleryObjectText(string title, Type galleryObjectType)
		{
			// If this is an album, return an empty string. Otherwise, return the title, truncated and purged of HTML
			// tags if necessary. If the title is truncated, add an ellipses to the text.
			//<asp:Label ID="lblAlbumPrefix" runat="server" CssClass="gsp_bold" Text="<%$ Resources:GalleryServerPro, UC_ThumbnailView_Album_Title_Prefix_Text %>" />&nbsp;<%# GetGalleryObjectText(Eval("Title").ToString(), Container.DataItem.GetType())%>

			int maxLength = Config.GetCore().MaxMediaObjectThumbnailTitleDisplayLength;
			string titlePrefix = String.Empty;

			if (galleryObjectType == typeof(Album))
			{
				// Album titles need a prefix, so assign that now.
				titlePrefix = String.Format(CultureInfo.CurrentCulture, "<span class='gsp_bold'>{0} </span>", Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text);

				// Override the previous max length with the value that is appropriate for albums.
				maxLength = Config.GetCore().MaxAlbumThumbnailTitleDisplayLength;
			}

			string truncatedText = Util.TruncateTextForWeb(title, maxLength);

			if (truncatedText.Length != title.Length)
				return String.Concat(titlePrefix, truncatedText, "...");
			else
				return String.Concat(titlePrefix, truncatedText);
		}

		protected static string GetId(IGalleryObject galleryObject)
		{
			// Prepend an 'a' (for album) or 'm' (for media object) to the ID to indicate whether it is
			// an album ID or media object ID.
			if (galleryObject is Album)
				return "a" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
			else
				return "m" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			this.TaskHeaderText = Resources.GalleryServerPro.Task_Download_Objects_Header_Text;
			this.TaskBodyText = Resources.GalleryServerPro.Task_Download_Objects_Body_Text;
			this.OkButtonText = Resources.GalleryServerPro.Task_Download_Objects_Ok_Button_Text;
			this.OkButtonToolTip = Resources.GalleryServerPro.Task_Download_Objects_Ok_Button_Tooltip;

			this.PageTitle = Resources.GalleryServerPro.Task_Download_Objects_Page_Title;

			if (GalleryObjects.Count > 0)
			{
				rptr.DataSource = GalleryObjects;
				rptr.DataBind();
			}
			else
			{
				this.RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotDownloadObjectsNoObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
			}

			ConfigureImageSizeDropDown();
		}

		private void ConfigureImageSizeDropDown() 
		{
			// Add options to the image size dropdown box.
			this.ddlImageSize.Items.Add(new ListItem(Resources.GalleryServerPro.Task_Download_Objects_Select_Image_Size_Thumbnail_Option, ((int)DisplayObjectType.Thumbnail).ToString()));
			this.ddlImageSize.Items.Add(new ListItem(Resources.GalleryServerPro.Task_Download_Objects_Select_Image_Size_Compressed_Option, ((int)DisplayObjectType.Optimized).ToString()));

			if (this.IsUserAuthorized(SecurityActions.ViewOriginalImage))
			{
				this.ddlImageSize.Items.Add(new ListItem(Resources.GalleryServerPro.Task_Download_Objects_Select_Image_Size_Original_Option, ((int)DisplayObjectType.Original).ToString()));
			}

			// Pre-select the compressed image size. Subtract one from the enum value because it starts at 1 and the index is zero-based.
			this.ddlImageSize.SelectedIndex = (int)DEFAULT_IMAGE_SIZE - 1;
		}

		private void ConfigureControlsEveryPageLoad()
		{
			SetThumbnailCssStyle(GalleryObjects);
		}

		private void RetrieveUserSelections(out List<int> albumIds, out List<int> mediaObjectIds)
		{
			// Iterate through all the checkboxes, saving checked ones to an array. The gallery object IDs are stored 
			// in a hidden input tag. Albums have an 'a' prefix; images have a 'm' prefix (e.g. "a322", "m999")
			CheckBox chkbx;
			HiddenField gc;
			albumIds = new List<int>();
			mediaObjectIds = new List<int>();

			// Loop through each item in the repeater control. If an item is checked, extract the ID.
			foreach (RepeaterItem rptrItem in rptr.Items)
			{
				// Each item will have one checkbox named either chkMO (for a media object) or chkAlbum (for an album).
				chkbx = rptrItem.FindControl("chkMO") as CheckBox;

				if ((chkbx == null) || (chkbx.Visible == false))
					chkbx = rptrItem.FindControl("chkAlbum") as CheckBox;

				if ((chkbx == null) || (chkbx.Visible == false))
					throw new WebException("Cannot find a checkbox named chkMO or chkAlbum or it has been made invisible");

				if (chkbx.Checked)
				{
					// Checkbox is checked. Find ID and save to appropriate List.
					gc = (HiddenField)rptrItem.FindControl("hdn");

					if ((gc == null) || (gc.Value.Length < 2))
						continue;

					int id = Convert.ToInt32(gc.Value.Substring(1), CultureInfo.InvariantCulture);
					char idType = Convert.ToChar(gc.Value.Substring(0, 1), CultureInfo.InvariantCulture); // 'a' or 'm'

					if (idType == 'm')
					{
						mediaObjectIds.Add(id);
					}
					else if (idType == 'a')
					{
						albumIds.Add(id);
					}
				}
			}
		}

		#endregion
	}
}
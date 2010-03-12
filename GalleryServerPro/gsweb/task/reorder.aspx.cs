using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.task
{
	public partial class reorder : GspPage
	{
		#region Private Fields

		private int _mediaObjectIndex;
		private string[] _snapDockIds;
		private string[] _galleryObjectIds;
		private string _snapDockIdsString;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a comma-delimited string of the IDs of the div tags used as the snap docking containers. This property is assigned
		/// to the DockingContainers property of the Snap control as each one is data bound.
		/// </summary>
		public string SnapDockIds
		{
			get 
			{
				if (String.IsNullOrEmpty(this._snapDockIdsString))
				{
					if ((this._snapDockIds == null) || (this._snapDockIds.Length == 0))
						throw new GalleryServerPro.ErrorHandler.CustomExceptions.WebException("The string array _snapDockIds must be assigned before accessing the SnapDockIds property. Instead, this property was null or empty.");

					this._snapDockIdsString = String.Join(",", this._snapDockIds);
				}
				return this._snapDockIdsString; 
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.EditAlbum);

			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		#endregion

		#region Protected Methods

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				btnOkClicked();

				this.RedirectToAlbumViewPage("msg", ((int)Message.ObjectsSuccessfullyRearranged).ToString(CultureInfo.InvariantCulture));
			}

			return true;
		}

		protected static string GetThumbnailCssClass(Type galleryObjectType)
		{
			// If it's an album then specify the appropriate CSS class so that the "Album"
			// header appears over the thumbnail. This is to indicate to the user that the
			// thumbnail represents an album.
			if (galleryObjectType == typeof(Album))
				return "thmb album2";
			else
				return "thmb2";
		}

		/// <summary>Return a string representing the title of the album. It is truncated and purged of
		/// HTML tags if necessary. </summary>
		/// <param name="title">The title of the album as stored in the data store.</param>
		/// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
		/// <returns>Returns a string representing the title of the album. It is truncated (if necessary) 
		/// and purged of HTML tags.</returns>
		protected static string GetAlbumText(string title, Type galleryObjectType)
		{
			if (galleryObjectType != typeof(Album))
				return String.Empty;

			int maxLength = WebsiteController.GetGalleryServerProConfigSection().Core.MaxAlbumThumbnailTitleDisplayLength;
			string truncatedText = WebsiteController.TruncateTextForWeb(title, maxLength);
			string titleText;
			if (truncatedText.Length != title.Length)
				titleText = string.Format(CultureInfo.CurrentCulture, "<p class=\"albumtitle\"><b>{0}</b> {1}...</p>", Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text, truncatedText);
			else
				titleText = string.Format(CultureInfo.CurrentCulture, "<p class=\"albumtitle\"><b>{0}</b> {1}</p>", Resources.GalleryServerPro.UC_ThumbnailView_Album_Title_Prefix_Text, truncatedText);

			return titleText;
		}

		/// <summary>
		/// Return a string representing the title of the media object. It is truncated and purged of HTML tags
		/// if necessary.
		/// </summary>
		/// <param name="title">The title of the media object as stored in the data store.</param>
		/// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
		/// <returns>Returns a string representing the title of the media object. It is truncated and purged of HTML tags
		/// if necessary.</returns>
		protected static string GetMediaObjectText(string title, Type galleryObjectType)
		{
			// If this is an album, return an empty string. Otherwise, we need to truncate the title while trying to 
			// preserve embedded HTML. Do this by checking the length of the title after it has been purged of HTML
			// codes. If the purged length is less than our maximum allowed, return the entire string with HTML.
			// If not, truncate the 
			if (galleryObjectType == typeof(Album))
				return String.Empty;

			int maxLength = WebsiteController.GetGalleryServerProConfigSection().Core.MaxMediaObjectThumbnailTitleDisplayLength;
			string truncatedText = WebsiteController.TruncateTextForWeb(title, maxLength);
			string titleText;
			if (truncatedText.Length != title.Length)
				titleText = string.Format(CultureInfo.CurrentCulture, "<p>{0}...</p>", truncatedText);
			else
				titleText = string.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", truncatedText);

			return titleText;
		}

		protected string GetGalleryObjectId(IGalleryObject galleryObject)
		{
			// Prepend an 'a' (for album) or 'm' (for media object) to the ID to indicate whether it is
			// an album ID or media object ID.
			string id;
			if (galleryObject is Album)
				id = "a" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
			else
				id = "m" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);

			// Add to member variable. Later we'll send these to the client in javascript and a hidden tag 
			// to help manage the rearranging.
			this._galleryObjectIds[this._mediaObjectIndex] = id;

			return id;
		}

		protected string GetSnapDockId()
		{
			this._snapDockIds[this._mediaObjectIndex] = String.Concat("sd", this._mediaObjectIndex.ToString());
			string snapDockId = this._snapDockIds[this._mediaObjectIndex];
			this._mediaObjectIndex++;
			return snapDockId;
		}

		protected string GetThumbnailImageTag(IGalleryObject galleryObject)
		{
			//<img src="<%# GetThumbnailUrl((IGalleryObject)((RepeaterItem)Container.Parent).DataItem) %>"
			//alt="<%# GetHovertip((IGalleryObject) ((RepeaterItem)Container.Parent).DataItem) %>"
			//title="<%# GetHovertip((IGalleryObject)((RepeaterItem)Container.Parent).DataItem) %>"
			//style="width: <%# DataBinder.Eval(((RepeaterItem)Container.Parent).DataItem, "Thumbnail.Width").ToString() %>px;
			//height: <%# DataBinder.Eval(((RepeaterItem)Container.Parent).DataItem, "Thumbnail.Height").ToString() %>px;" />
			string hoverTip = GetHovertip(galleryObject);

			string thumbImgTag = String.Format(@"<img src='{0}' alt='{1}' title='{1}' style='width:{2}px;height:{3}px;' />",
				GetThumbnailUrl(galleryObject), // 0
				GetHovertip(galleryObject), // 1
				galleryObject.Thumbnail.Width, // 2
				galleryObject.Thumbnail.Height
				);

			return thumbImgTag;
		}

		//protected string GetSnapId(object container)
		//{
		//  //"sp0"
		//  //return String.Concat("sp", this._mediaObjectIndex.ToString());
		//  return ((Snap)container).ClientID;
		//  //return "ctl00_ctl00_ctl00_c1_c2_c3_rprSnap_ctl00_sp";
		//}

		protected string GetSnapDockingContainerId()
		{
			//"sd0"
			return String.Concat("sd", this._mediaObjectIndex.ToString());
		}

		protected void rprSnap_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			this._mediaObjectIndex++;
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Rearrange_Objects_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Rearrange_Objects_Body_Text;

			IGalleryObjectCollection albumChildren = this.GetAlbum().GetChildGalleryObjects(true);


			SetThumbnailCssStyle(albumChildren, 0, 0, new string[] { "thmb2", "snapDock" });

			// Bind the docking containers (really, just div tags) that will hold each snap control.
			this._snapDockIds = new string[albumChildren.Count];
			rprDock.DataSource = albumChildren;
			rprDock.DataBind(); // _snapDockIds is updated within GetSnapDockId() during data binding (it is invoked from aspx markup)

			// Reset media object index to 0 and bind the snaps to the page. Each snap is assigned to one of the snap docks that
			// were bound in the rprDock.DataBind method.
			this._galleryObjectIds = new string[albumChildren.Count];
			this._mediaObjectIndex = 0;
			rprSnap.DataSource = albumChildren;
			rprSnap.DataBind();

			ClientScript.RegisterArrayDeclaration("_snapDockIds", String.Concat("'", String.Join("','", this._snapDockIds), "'"));
			ClientScript.RegisterArrayDeclaration("_goIds", String.Concat("'", String.Join("','", this._galleryObjectIds), "'"));
			//Snap1.DockingContainers = String.Join(",", this._snapDockIds);
		}

		private string GetHovertip(IGalleryObject galleryObject)
		{
			// Return the text to be used as the hovertip in standards compliant browsers. This is the
			// summary text for albums, and the title text for objects.
			string hoverTip = galleryObject.Title;

			IAlbum album = galleryObject as IAlbum;
			if (album != null)
			{
				if (album.Summary.Trim().Length > 0)
					hoverTip = album.Summary;
			}

			string hoverTipClean = Server.HtmlEncode(WebsiteController.RemoveHtmlTags(hoverTip)); ;

			return hoverTipClean;
		}

		private void btnOkClicked()
		{
			HtmlInputHidden gc;
			foreach (RepeaterItem rptrItem in rprDock.Items)
			{
				// Checkbox is checked. Save media object ID to array.
				int numControls = rptrItem.Controls.Count;
				//gc = (HtmlInputHidden)rptrItem.Controls[3]; // The hidden <input> tag
				//ids.Add(gc.Value.ToString());
			}

		}

		#endregion


	}
}

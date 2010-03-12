using System;
using System.Globalization;
using System.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.uc
{
	public partial class thumbnailview : System.Web.UI.UserControl
	{
		#region Private Fields

		private bool? _viewOriginal;

		#endregion

		#region Protected events

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		#endregion

		#region Public Methods

		public void DisplayThumbnails(IAlbum album)
		{
			string msg = string.Empty;

			//Get the data associated with the album and display
			IGalleryObjectCollection albumObjects;
			if (this.PageBase.IsAnonymousUser)
			{
				albumObjects = album.GetChildGalleryObjects(true, true);
			}
			else
			{
				albumObjects = album.GetChildGalleryObjects(true);
			}

			if (albumObjects.Count > 0)
			{
				// At least one album or media object in album.
				msg = String.Format(CultureInfo.CurrentCulture, "<p class='addtopmargin2'>{0}</p>", Resources.GalleryServerPro.UC_ThumbnailView_Intro_Text_With_Objects);
				phMsg.Controls.Add(new LiteralControl(msg));
			}
			else if (this.PageBase.UserCanAddMediaObject)
			{
				// No objects, user has permission to add media objects.
				string innerMsg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_ThumbnailView_Intro_Text_No_Objects_User_Has_Add_MediaObject_Permission, album.Id);
				msg = String.Format(CultureInfo.CurrentCulture, "<p class='addtopmargin2 msgfriendly'>{0}</p>", innerMsg);
				phMsg.Controls.Add(new LiteralControl(msg));
			}
			else
			{
				// No objects, user doesn't have permission to add media objects.
				msg = String.Format(CultureInfo.CurrentCulture, "<p class='addtopmargin2 msgfriendly'>{0}</p>", Resources.GalleryServerPro.UC_ThumbnailView_Intro_Text_No_Objects);
				phMsg.Controls.Add(new LiteralControl(msg));
			}

			this.PageBase.SetThumbnailCssStyle(albumObjects);

			rptr.DataSource = albumObjects;
			rptr.DataBind();
		}

		public void DisplayThumbnails(IGalleryObjectCollection galleryObjects)
		{
			this.PageBase.SetThumbnailCssStyle(galleryObjects);

			rptr.DataSource = galleryObjects;
			rptr.DataBind();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		public GspPage PageBase
		{
			get { return (GspPage) this.Page; }
		}

		public bool ViewOriginal
		{
			get 
			{
				if (!this._viewOriginal.HasValue)
				{
					this._viewOriginal = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ThumbnailClickShowsOriginal;
				}
				return this._viewOriginal.Value; 
			}
		}

		#endregion

		#region Private Methods

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
		protected static string GetGalleryObjectText(string title, Type galleryObjectType)
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

		protected string GetHovertip(IGalleryObject galleryObject)
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

		protected string GenerateUrl(IGalleryObject galleryObject)
		{
			string rv = string.Empty;

			if (galleryObject is Album)
			{
				// We have an album.
				rv = "default.aspx?aid=" + galleryObject.Id;
			}
			else if (galleryObject is GalleryServerPro.Business.Image)
			{
				// We have an image.
				if (ViewOriginal)
					rv = "default.aspx?moid=" + galleryObject.Id + "&hr=1";
				else
					rv = "default.aspx?moid=" + galleryObject.Id;
			}
			else
			{
				rv = "default.aspx?moid=" + galleryObject.Id;
			}

			if (String.IsNullOrEmpty(rv))
				throw new WebException("Unsupported media object type: " + galleryObject.GetType().ToString());

			return rv;
		}

		protected string GetThumbnailUrl(IGalleryObject galleryObject)
		{
			return this.PageBase.GetThumbnailUrl(galleryObject);
		}

		#endregion
	}
}
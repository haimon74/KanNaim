using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using GalleryServerPro.Business;

namespace GalleryServerPro.Web
{
	public partial class _default : GspPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
			{
				// Postback: Since the user may have been navigating several media objects in this album through AJAX calls, we need to check
				// a hidden field to discover the current media object. Assign this object's ID to our base page. The base 
				// page is smart enough to retrieve the new media object if it is different than what was previously set.
				object formFieldMoid = Request.Form["moid"];
				int moid;
				if ((formFieldMoid != null) && (Int32.TryParse(formFieldMoid.ToString(), out moid)))
				{
					this.MediaObjectId = moid;
				}
			}

			ConfigureControls();

			if ((!IsPostBack) && (!cbDummy.IsCallback))
			{
				RegisterHiddenFields();
			}
		}

		#region Public Properties


		#endregion

		#region Private Methods

		private void RegisterHiddenFields()
		{
			ScriptManager.RegisterHiddenField(this, "moid", this.MediaObjectId.ToString(CultureInfo.InvariantCulture));
			ScriptManager.RegisterHiddenField(this, "aid", this.AlbumId.ToString(CultureInfo.InvariantCulture));
		}

		private void ConfigureControls()
		{
			// If a media object ID has been specified, show the media object. Otherwise show the album
			// (defaulting to the root album it no album ID is specified).
			bool showMediaObject = (base.MediaObjectId > int.MinValue);

			if (showMediaObject)
			{
				phMediaObjectContainer.Controls.Add(Page.LoadControl("~/uc/mediaobjectview.ascx"));
			}
			else
			{
				// Add album header
				uc.albumheader albumHeader = (uc.albumheader)Page.LoadControl("~/uc/albumheader.ascx");
				albumHeader.EnableInlineEditing = true;
				phAlbumHeaderContainer.Controls.Clear();
				phAlbumHeaderContainer.Controls.Add(albumHeader);

				// Add thumbnail view
				uc.thumbnailview thumbnailView = (uc.thumbnailview)Page.LoadControl("~/uc/thumbnailview.ascx");
				thumbnailView.DisplayThumbnails(this.GetAlbum());
				phMediaObjectContainer.Controls.Clear();
				phMediaObjectContainer.Controls.Add(thumbnailView);
			}

			ShowMessage();
		}

		/// <summary>
		/// If the base page's Message property has been assigned a value, display a message to the user.
		/// </summary>
		private void ShowMessage()
		{
			if (this.Message == Message.None)
				return;

			const string resourcePrefix = "Msg_";
			const string headerSuffix = "_Hdr";
			const string detailSuffix = "_Dtl";

			string headerMsg = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), headerSuffix));
			string detailMsg = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), detailSuffix));

			switch (this.Message)
			{
				case Message.CaptionExceededMaxLength:
				case Message.OneOrMoreCaptionsExceededMaxLength:
					{
						int maxLength = WebsiteController.GetGalleryServerProConfigSection().DataStore.MediaObjectTitleLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxLength);
						break;
					}
				case Message.AlbumSummaryExceededMaxLength:
					{
						int maxLength = WebsiteController.GetGalleryServerProConfigSection().DataStore.AlbumSummaryLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxLength);
						break;
					}
				case Message.AlbumNameExceededMaxLength:
					{
						int maxLength = WebsiteController.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxLength);
						break;
					}
				case Message.AlbumNameAndSummaryExceededMaxLength:
					{
						int maxTitleLength = WebsiteController.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;
						int maxSummaryLength = WebsiteController.GetGalleryServerProConfigSection().DataStore.AlbumSummaryLength;
						detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsg, maxTitleLength, maxSummaryLength);
						break;
					}
				case Message.ObjectsSkippedDuringUpload:
					{
						List<KeyValuePair<string, string>> skippedFiles = Session[GlobalConstants.SkippedFilesDuringUploadSessionKey] as List<KeyValuePair<string, string>>;

						detailMsg = string.Empty;
						if (skippedFiles != null)
						{
							// This message is unique in that we need to choose one of two detail messages from the resource file. One is for when a single
							// file has been skipped; the other is when multiple files have been skipped.
							if (skippedFiles.Count == 1)
							{
								string detailMsgTemplate = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), "Single", detailSuffix));
								detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsgTemplate, skippedFiles[0].Key, skippedFiles[0].Value);
							}
							else if (skippedFiles.Count > 1)
							{
								string detailMsgTemplate = Resources.GalleryServerPro.ResourceManager.GetString(String.Concat(resourcePrefix, this.Message.ToString(), "Multiple", detailSuffix));
								detailMsg = String.Format(CultureInfo.CurrentCulture, detailMsgTemplate, ConvertListToHtmlBullets(skippedFiles));
							}
						}
						break;
					}
			}
			GalleryServerPro.Web.uc.usermessage msgBox = (GalleryServerPro.Web.uc.usermessage)LoadControl("~/uc/usermessage.ascx");
			msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Information;
			msgBox.MessageTitle = headerMsg;
			msgBox.MessageDetail = detailMsg;
			msgBox.CssClass = "um2ContainerCss";
			msgBox.HeaderCssClass = "um2HeaderCss";
			msgBox.DetailCssClass = "um2DetailCss";
			phMessage.Controls.Add(msgBox);
		}

		private static string ConvertListToHtmlBullets(IEnumerable<KeyValuePair<string, string>> skippedFiles)
		{
			string html = "<ul>";
			foreach (KeyValuePair<string, string> kvp in skippedFiles)
			{
				html += String.Format(CultureInfo.CurrentCulture, "<li>{0}: {1}</li>", kvp.Key, kvp.Value);
			}
			html += "</ul>";

			return html;
		}

		#endregion
	}
}

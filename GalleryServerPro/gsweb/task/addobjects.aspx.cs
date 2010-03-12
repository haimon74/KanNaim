using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.WebControls;

namespace GalleryServerPro.Web.task
{
	public partial class addobjects : GspPage
	{
		#region Private Fields

		private readonly List<KeyValuePair<string, string>> _skippedFiles = new List<KeyValuePair<string, string>>();

		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AddMediaObject);

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}

			ConfigureControlsEveryPageLoad();
		}

		protected void btnUploadMediaObjects_Click(object sender, EventArgs e)
		{
			AddUploadedFilesLessThanFullTrust();

			RedirectToAlbumPage();
		}

		protected void Upload1_Uploaded(object sender, UploadUploadedEventArgs e)
		{
			//string[] importOptions = Upload1.CallbackParameter.Split(new char[] { '|' });
			//bool importMembership = Convert.ToBoolean(importOptions[0]);
			//bool importGalleryData = Convert.ToBoolean(importOptions[1]);

			AddUploadedFilesForFullTrust(e.UploadedFiles);

			RedirectToAlbumPage();
		}

		protected void btnAddExternalHtmlSource_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				AddExternalHtmlContent();

				RedirectToAlbumPage();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.RedirectToReferringPage();
		}

		#endregion

		#region Public Properties

		#endregion


		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			if (AppSetting.Instance.AppTrustLevel == ApplicationTrustLevel.Full)
			{
				btnUploadMediaObjects.Visible = false;
			}

			Master.OkButtonIsVisible = false; // Instead, we'll use our own buttons inside the tab control.
			Master.CancelButtonIsVisible = false;
			Master.TaskHeader = Resources.GalleryServerPro.Task_Add_Objects_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Add_Objects_Body_Text;

			ConfigureTabStrip();

			if (!HelperFunctions.IsFileAuthorizedForAddingToGallery("dummy.zip"))
			{
				chkDoNotExtractZipFile.Enabled = false;
				chkDoNotExtractZipFile.CssClass = "disabledtext";
			}
		}

		private void ConfigureTabStrip()
		{
			// By default both tabs are invisible. Check config settings to see which ones are enabled, and set
			// visibility as needed.
			bool allowLocalContent = WebsiteController.GetGalleryServerProConfigSection().Core.AllowAddLocalContent;
			bool allowExternalContent = WebsiteController.GetGalleryServerProConfigSection().Core.AllowAddExternalContent;

			if (allowLocalContent)
			{
				tabLocalMedia.Visible = true;
				mpAddObjects.SelectPageById(pvAddLocal.ID);
			}

			if (allowExternalContent)
			{
				tabExternal.Visible = true;
				if (!allowLocalContent)
					mpAddObjects.SelectPageById(pvAddExternal.ID);
			}

			if ((!allowLocalContent) && (!allowExternalContent))
			{
				// Both settings are disabled, which means no objects can be added. This is probably a mis-configuration,
				// so give a friendly message to help point the administrator in the right direction for changing it.
				mpAddObjects.Visible = false;
				wwMessage.ShowMessage(Resources.GalleryServerPro.Task_Add_Objects_All_Adding_Types_Disabled_Msg);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
			}
		}

		private void ConfigureControlsEveryPageLoad()
		{
			if (AppSetting.Instance.AppTrustLevel == ApplicationTrustLevel.Full)
			{
				// Use ComponentArt's Upload control.
				AddUploadControlForFullTrust();
			}
			else
			{
				// Use ASP.NET's FileUpload control.
				AddUploadControlForLessThanFullTrust();
			}

			AddPopupInfoItems();
		}

		/// <summary>
		/// Add any PopupInfoItem controls that cannot be declaratively created in the aspx page because one or more
		/// properties are computed.
		/// </summary>
		private void AddPopupInfoItems()
		{
			// Create the popup for the local media tab's body text in the overview section, just below the text
			// "Add media files to your gallery". If it wasn't for the string formatting of the DialogBody property, 
			// we could have declared it in the aspx page like this:

			//<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="lblLocalMediaOverview" DialogTitle="<%$ Resources:GalleryServerPro, Task_Add_Objects_Local_Media_Overview_Hdr %>"
			//DialogBody="<% =GetLocalMediaPopupBodyText();%>" />
			int maxUploadSize = WebsiteController.GetGalleryServerProConfigSection().Core.MaxUploadSize;

			PopupInfoItem popupInfoItem = new PopupInfoItem();
			popupInfoItem.ID = "poi1";
			popupInfoItem.ControlId = "lblLocalMediaOverview";
			popupInfoItem.DialogTitle = Resources.GalleryServerPro.Task_Add_Objects_Local_Media_Overview_Hdr;
			popupInfoItem.DialogBody = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Add_Objects_Local_Media_Overview_Bdy, maxUploadSize);

			PopupInfo1.PopupInfoItems.Add(popupInfoItem);
		}

		/// <summary>
		/// Adds the upload control that is used when the application is running under full trust.
		/// </summary>
		private void AddUploadControlForFullTrust()
		{
			const string htmlBeforeUpload = @"<div class=""sel"">";
			string htmlAfterUpload = String.Format(@"
</div>
<div class='actions'>
	 <a href='javascript:void(0);' onclick='add_file(Upload1,this);this.blur();return false;'
		 class='add' id='btn-add'></a><a href='javascript:void(0);' onclick='init_upload(Upload1);this.blur();return false;'
			class='upload-d' id='btn-upload' title='{0}'></a>
</div>
", Resources.GalleryServerPro.Task_Add_Objects_Upload_Upload_Button_Tooltip);

			Upload upload = new Upload();

			upload.ID = "Upload1";
			upload.MaximumFileCount = 10;
			upload.AutoPostBack = true;
			upload.TempFileFolder = AppSetting.Instance.TempUploadDirectory;
			upload.MaximumUploadSize = WebsiteController.GetGalleryServerProConfigSection().Core.MaxUploadSize;
			upload.FileInputClientTemplateId = "FileInputTemplate";
			upload.UploadCompleteClientTemplateId = "CompletedTemplate";
			upload.FileInputImageUrl = "../images/componentart/upload/_browse.png";
			upload.FileInputHoverImageUrl = "../images/componentart/upload/_browse-h.png";
			upload.ProgressClientTemplateId = "ProgressTemplate";
			upload.ProgressDomElementId = "upload-progress";
			upload.ClientEvents.FileChange = new ClientEvent("file_change");
			upload.ClientEvents.UploadBegin = new ClientEvent("upload_begin");
			upload.ClientEvents.UploadEnd = new ClientEvent("upload_end");

			upload.Uploaded += Upload1_Uploaded;

			phUpload.Controls.Add(new LiteralControl(htmlBeforeUpload));
			phUpload.Controls.Add(upload);
			phUpload.Controls.Add(new LiteralControl(htmlAfterUpload));

			AddUploadClientTemplates(upload);

			AddUploadDialogClientTemplate();
		}

		/// <summary>
		/// Adds ten upload controls that are used when the application is running at less than full trust.
		/// </summary>
		private void AddUploadControlForLessThanFullTrust()
		{
			const string htmlBeforeUpload = @"<p>";
			const string htmlAfterUpload = @"</p>";

			for (int i = 0; i < 5; i++)
			{
				FileUpload upload = new FileUpload();
				upload.ID = "fuUpload" + i;
				upload.Attributes.Add("title", Resources.GalleryServerPro.Task_Add_Objects_Input_Tooltip);
				upload.Attributes.Add("size", "65");

				phUpload.Controls.Add(new LiteralControl(htmlBeforeUpload));
				phUpload.Controls.Add(upload);
				phUpload.Controls.Add(new LiteralControl(htmlAfterUpload));
			}
		}

		/// <summary>
		/// Adds client templates for the Upload control.
		/// </summary>
		private static void AddUploadClientTemplates(Upload upload)
		{
			#region File input client template

			// Add client template for the file input section (includes input tag and browse and upload buttons).
			ClientTemplate uploadFileInputClientTemplate = new ClientTemplate();

			uploadFileInputClientTemplate.ID = "FileInputTemplate";

			uploadFileInputClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<div class=""file"">
	<div class=""## DataItem.FileName ? 'filename' : 'filename empty'; ##"">
		<input value=""## DataItem.FileName ? DataItem.FileName : ""{0}""; ##""
			onfocus=""this.blur();"" /></div>
	<a href=""javascript:void(0);"" onclick=""this.blur();return false;"" class=""browse""
		title=""{1}"">#$FileInputImage</a> <a href=""javascript:void(0);"" onclick=""remove_file(## Parent.Id ##,## DataItem.FileIndex ##);return false;""
			class=""remove"" title=""{2}""></a>
</div>
",
			Resources.GalleryServerPro.Task_Add_Objects_Upload_File_Input_Text,
			Resources.GalleryServerPro.Task_Add_Objects_Upload_File_Browse_Button_Tooltip,
			Resources.GalleryServerPro.Task_Add_Objects_Remove_Button_Tooltip);

			upload.ClientTemplates.Add(uploadFileInputClientTemplate);

			#endregion

			#region Upload progress client template

			// Add client template for the progress section.
			ClientTemplate uploadProgressClientTemplate = new ClientTemplate();

			uploadProgressClientTemplate.ID = "ProgressTemplate";

			uploadProgressClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<!-- Dialogue contents -->
<div class=""con"">
	<div class=""stat"">
		<h3 rel=""total"">
			{0}</h3>
		<div class=""prog"">
			<div class=""con"">
				<div class=""bar"" style=""width: ## get_percentage(DataItem.Progress) ##%;"">
				</div>
			</div>
		</div>
		<div class=""lbl"" style=""text-align: right;"">
			<strong>## format_file_size(DataItem.ReceivedBytes) ##</strong> {1} <strong>## format_file_size(DataItem.TotalBytes)
				##</strong> (## get_percentage(DataItem.Progress) ##%) {2}</div>
	</div>
	<div class=""list"">
		<h3>
			{3} <span style=""font-size: 11px;"">(<strong>## get_file_position(Parent,DataItem.CurrentFile)
				##</strong> {1} <strong>## Parent.GetFiles().length ##</strong>):</span></h3>
		<div class=""files"">
			## generate_file_list(Parent,DataItem.CurrentFile); ##</div>
	</div>
</div>
<!-- /Dialogue contents -->
<!-- Dialogue footer -->
<div class=""ftr"">
	<div class=""ftr-l"">
	</div>
	<div class=""ftr-m"">
		<div class=""info"">
			<span>{4} <strong>## format_time(DataItem.ElapsedTime); ##</strong></span>
			<span style=""padding-left: 8px;"">{5} <strong>## format_time(DataItem.ElapsedTime
				+ DataItem.RemainingTime); ##</strong></span> <span style=""padding-left: 8px;"">{6}
					<strong>## DataItem.Speed.toFixed(2) ## {7}</strong></span>
		</div>
		<div class=""btns"">
			<a onclick=""Upload1.abort();UploadDialog.close();this.blur();return false;""
				href=""javascript:void(0);"" rel=""cancel""><span class=""l""></span><span class=""m"" id=""btn1"">{8}</span>
				<span class=""r""></span></a>
		</div>
	</div>
	<div class=""ftr-r"">
	</div>
</div>
<!-- /Dialogue footer -->
",
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Progress_Header, // 0
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Progress_Separator_Text, // 1
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Bytes_Uploaded_Suffix, // 2
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Filename_Label, // 3
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Elapsed_Time_Label, // 4
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Estimated_Time_Label, // 5
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Speed_Label, // 6
					Resources.GalleryServerPro.Site_KiloBytes_Per_Second_Abbreviation, // 7
					Resources.GalleryServerPro.Task_Add_Objects_Upload_Cancel_Upload_Text // 8
				);

			upload.ClientTemplates.Add(uploadProgressClientTemplate);

			#endregion

			#region Upload complete client template

			// Add client template for the upload complete section.
			ClientTemplate uploadCompleteClientTemplate = new ClientTemplate();

			uploadCompleteClientTemplate.ID = "CompletedTemplate";

			uploadCompleteClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<!-- Dialogue contents -->
<div class=""con"">
	<div class=""stat"">
		<h3 class=""red"">
			<img src=""../images/spinner.gif"" style=""width:16px;height:16px;"" alt="""" /> {0}</h3>
		<div class=""prog"">
			<div class=""con"">
				<div class=""bar"" style=""width: ## get_percentage(DataItem.Progress) ##%;"">
				</div>
			</div>
		</div>
		<div class=""lbl"" style=""text-align: right;"">
			<strong>## format_file_size(DataItem.ReceivedBytes) ##</strong> {1} <strong>## format_file_size(DataItem.TotalBytes)
				##</strong> (## get_percentage(DataItem.Progress) ##%) {2}</div>
	</div>
	<div class=""list"">
		<h3>
			<strong>## Parent.GetFiles().length ##</strong> ## (Parent.GetFiles().length > 1)
			? ""{3}"" : ""{4}"" ## <strong>## format_time(DataItem.ElapsedTime,true);
				##</strong>:</h3>
		<div class=""files"">
			## generate_file_list(Parent,DataItem.CurrentFile); ##</div>
	</div>
</div>
<!-- /Dialogue contents -->
<!-- Dialogue footer -->
<div class=""ftr"">
	<div class=""ftr-l"">
	</div>
	<div class=""ftr-m"">
	</div>
	<div class=""ftr-r"">
	</div>
</div>
<!-- /Dialogue footer -->
",
			Resources.GalleryServerPro.Task_Add_Objects_Upload_Complete_Header, // 0
			Resources.GalleryServerPro.Task_Add_Objects_Upload_Progress_Separator_Text, // 1
			Resources.GalleryServerPro.Task_Add_Objects_Upload_Bytes_Uploaded_Suffix, // 2
			Resources.GalleryServerPro.Task_Add_Objects_Upload_Complete_Files_Separator_Text_Plural, // 3
			Resources.GalleryServerPro.Task_Add_Objects_Upload_Complete_Files_Separator_Text_Singular // 4
			);

			upload.ClientTemplates.Add(uploadCompleteClientTemplate);

			#endregion
		}

		private void AddUploadDialogClientTemplate()
		{
			// Add loading panel client template
			ComponentArt.Web.UI.ClientTemplate uploadDialogClientTemplate = new ComponentArt.Web.UI.ClientTemplate();

			uploadDialogClientTemplate.ID = "UploadContent";

			uploadDialogClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<div class=""ttl"" onmousedown=""UploadDialog.StartDrag(event);"">
	<div class=""ttlt"">
		<div class=""ttlt-l"">
		</div>
		<div class=""ttlt-m"">
			<a class=""close"" href=""javascript:void(0);"" onclick=""UploadDialog.close();this.blur();return false;"">
			</a><span>{0}</span>
		</div>
		<div class=""ttlt-r"">
		</div>
	</div>
	<div class=""ttlb"">
		<div class=""ttlb-l"">
		</div>
		<div class=""ttlb-m"">
		</div>
		<div class=""ttlb-r"">
		</div>
	</div>
</div>
<!-- for contents & footer, see upload progress client template -->
<div id=""upload-progress"">
</div>
",
				Resources.GalleryServerPro.Task_Add_Objects_Upload_Dialog_Uploading_Text);

			UploadDialog.ClientTemplates.Add(uploadDialogClientTemplate);
		}

		private void RedirectToAlbumPage()
		{
			if (this._skippedFiles.Count > 0)
			{
				Session[GlobalConstants.SkippedFilesDuringUploadSessionKey] = this._skippedFiles;

				this.RedirectToAlbumViewPage("msg", ((int)Message.ObjectsSkippedDuringUpload).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				this.RedirectToAlbumViewPage();
			}
		}

		/// <summary>
		/// Adds the uploaded files to the gallery. This method is called when the application is operating at lesss than full trust. In this case,
		/// the ASP.NET FileUpload control is used. The logic is nearly identical to that in AddUploadedFilesForFullTrust - the only
		/// differences are syntax differences arising from the different file upload control.
		/// </summary>
		private void AddUploadedFilesLessThanFullTrust()
		{
			// Clear the list of hash keys so we're starting with a fresh load from the data store.
			try
			{
				MediaObjectHashKeys.Clear();

				string albumPhysicalPath = this.GetAlbum().FullPhysicalPathOnDisk;

				HelperFunctions.BeginTransaction();

				for (int i = 0; i < 5; i++)
				{
					FileUpload file = (FileUpload)phUpload.FindControl("fuUpload" + i);

					if (!file.HasFile)
						continue;

					if ((System.IO.Path.GetExtension(file.FileName).ToUpperInvariant() == ".ZIP") && (!chkDoNotExtractZipFile.Checked))
					{
						#region Extract the files from the zipped file.

						// Extract the files from the zipped file.
						string userName = (String.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
						using (ZipUtility zip = new ZipUtility(userName, WebsiteController.GetRolesForUser(), User.Identity.IsAuthenticated))
						{
							this._skippedFiles.AddRange(zip.ExtractZipFile(file.FileContent, this.GetAlbum()));
						}

						#endregion
					}
					else
					{
						#region Add the file

						string filename = HelperFunctions.ValidateFileName(albumPhysicalPath, file.FileName);
						string filepath = Path.Combine(albumPhysicalPath, filename);

#if DEBUG
						TechInfoSystems.TracingTools.Tools.MarkSpot(string.Format(CultureInfo.CurrentCulture, "Attempting to move file {0} to {1}...", file.FileName, filepath));
#endif
						file.SaveAs(filepath);

						try
						{
							IGalleryObject go = Factory.CreateMediaObjectInstance(filepath, this.GetAlbum());
							WebsiteController.SaveGalleryObject(go);
						}
						catch (UnsupportedMediaObjectTypeException ex)
						{
							File.Delete(filepath);
							this._skippedFiles.Add(new KeyValuePair<string, string>(filename, ex.Message));
						}

						#endregion
					}
				}

				HelperFunctions.CommitTransaction();
			}
			catch
			{
				HelperFunctions.RollbackTransaction();
				throw;
			}
			finally
			{
				// Clear the list of hash keys to free up memory.
				MediaObjectHashKeys.Clear();

				HelperFunctions.PurgeCache();
			}
		}

		/// <summary>
		/// Adds the uploaded files to the gallery. This method is called when the application is operating under full trust. In this case,
		/// the ComponentArt Upload control is used. The logic is nearly identical to that in AddUploadedFilesLessThanFullTrust - the only
		/// differences are syntax differences arising from the different file upload control.
		/// </summary>
		/// <param name="files">The files to add to the gallery.</param>
		private void AddUploadedFilesForFullTrust(UploadedFileInfoCollection files)
		{
			// Clear the list of hash keys so we're starting with a fresh load from the data store.
			try
			{
				MediaObjectHashKeys.Clear();

				string albumPhysicalPath = this.GetAlbum().FullPhysicalPathOnDisk;

				HelperFunctions.BeginTransaction();

				UploadedFileInfo[] fileInfos = new UploadedFileInfo[files.Count];
				files.CopyTo(fileInfos, 0);
				Array.Reverse(fileInfos);

				foreach (UploadedFileInfo file in fileInfos)
				{
					if (String.IsNullOrEmpty(file.FileName))
						continue;

					if ((System.IO.Path.GetExtension(file.FileName).ToUpperInvariant() == ".ZIP") && (!chkDoNotExtractZipFile.Checked))
					{
						#region Extract the files from the zipped file.

						lock (file)
						{
							if (File.Exists(file.TempFileName))
							{
								string userName = (String.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
								using (ZipUtility zip = new ZipUtility(userName, WebsiteController.GetRolesForUser(), User.Identity.IsAuthenticated))
								{
									this._skippedFiles.AddRange(zip.ExtractZipFile(file.GetStream(), this.GetAlbum()));
								}
							}
							else
							{
								// When one of the files causes an OutOfMemoryException, this can cause the other files to disappear from the
								// temp upload directory. This seems to be an issue with the ComponentArt Upload control, since this does not
								// seem to happen with the ASP.NET FileUpload control. If the file doesn't exist, make a note of it and move on 
								// to the next one.
								this._skippedFiles.Add(new KeyValuePair<string, string>(file.FileName, Resources.GalleryServerPro.Task_Add_Objects_Uploaded_File_Does_Not_Exist_Msg));
								continue; // Skip to the next file.
							}
						}

						#endregion
					}
					else
					{
						#region Add the file

						string filename = HelperFunctions.ValidateFileName(albumPhysicalPath, file.FileName);
						string filepath = Path.Combine(albumPhysicalPath, filename);

#if DEBUG
						TechInfoSystems.TracingTools.Tools.MarkSpot(string.Format(CultureInfo.CurrentCulture, "Attempting to move file {0} to {1}...", file.FileName, filepath));
#endif
						lock (file)
						{
							if (File.Exists(file.TempFileName))
							{
								file.SaveAs(filepath);
							}
							else
							{
								// When one of the files causes an OutOfMemoryException, this can cause the other files to disappear from the
								// temp upload directory. This seems to be an issue with the ComponentArt Upload control, since this does not
								// seem to happen with the ASP.NET FileUpload control. If the file doesn't exist, make a note of it and move on 
								// to the next one.
								this._skippedFiles.Add(new KeyValuePair<string, string>(file.FileName, Resources.GalleryServerPro.Task_Add_Objects_Uploaded_File_Does_Not_Exist_Msg));
								continue; // Skip to the next file.
							}
						}

						try
						{
							IGalleryObject go = Factory.CreateMediaObjectInstance(filepath, this.GetAlbum());
							WebsiteController.SaveGalleryObject(go);
						}
						catch (UnsupportedMediaObjectTypeException ex)
						{
							File.Delete(filepath);
							this._skippedFiles.Add(new KeyValuePair<string, string>(filename, ex.Message));
						}


						#endregion
					}
				}

				HelperFunctions.CommitTransaction();
			}
			catch
			{
				HelperFunctions.RollbackTransaction();
				throw;
			}
			finally
			{
				// Delete the uploaded temporary files, as by this time they have been saved to the destination directory.
				foreach (UploadedFileInfo file in files)
				{
					System.IO.File.Delete(file.TempFileName);
				}

				// Clear the list of hash keys to free up memory.
				MediaObjectHashKeys.Clear();

				HelperFunctions.PurgeCache();
			}
		}

		private void AddExternalHtmlContent()
		{
			string externalHtmlSource = txtExternalHtmlSource.Text.Trim();

			MimeTypeCategory mimeTypeCategory = MimeTypeCategory.Other;
			string mimeTypeCategoryString = ddlMediaTypes.SelectedValue;
			try
			{
				mimeTypeCategory = (MimeTypeCategory)Enum.Parse(typeof(MimeTypeCategory), mimeTypeCategoryString, true);
			}
			catch { } // Suppress any parse errors so that category remains the default value 'Other'.

			string title = txtTitle.Text.Trim();
			if (String.IsNullOrEmpty(title))
			{
				// If user didn't enter a title, use the media category (e.g. Video, Audio, Image, Other).
				title = mimeTypeCategory.ToString();
			}

			IGalleryObject mediaObject = Factory.CreateExternalMediaObjectInstance(externalHtmlSource, mimeTypeCategory, this.GetAlbum());
			mediaObject.Title = title;
			WebsiteController.SaveGalleryObject(mediaObject);

			HelperFunctions.PurgeCache();
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.admin
{
	public partial class backup_restore : GspPage
	{
		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			string test = Resources.GalleryServerPro.Admin_Backup_Restore_Backup_Tab_Title;

			this.CheckUserSecurity(SecurityActions.AdministerSite);

			Page.MaintainScrollPositionOnPostBack = true;

			if (!IsPostBack)
			{
				ConfigureControlsFirstPageLoad();
			}

			ConfigureControlsEveryPageLoad();

			///TODO: Enforce this rule: The application name for all four providers must be equal if both checkboxes are selected.
			/// If only GS data is requested, do not enforce. If only non-GS data is requested (membership, roles, & profile), verify
			/// that those three app names are equal. The following code may be helpful.
			//string membershipProvider = ((System.Web.Security.SqlMembershipProvider)System.Web.Security.Membership.Provider).ApplicationName;
			//string roleProvider = System.Web.Security.Roles.Provider.ApplicationName;
			//string profileProvider = System.Web.Profile.ProfileManager.Provider.ApplicationName;
			//string galleryProvider = Factory.GetDataProvider().ApplicationName;
		}

		protected void btnExportData_Click(object sender, EventArgs e)
		{
			const string backupFileExtension = ".xml";
			string backupFilename = "GalleryServerBackup_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + backupFileExtension;
			string filePath = Path.Combine(AppSetting.Instance.TempUploadDirectory, backupFilename);
			HelperFunctions.ExportGalleryData(filePath, chkExportMembership.Checked, chkExportGalleryData.Checked);

			Response.Clear();

			// It would be pretty safe to just hard code the ContentType to text/xml, but let's use the value from the config file if it's there.
			// If not, then we'll default to text.xml.
			IMimeType xmlMimeType = MimeType.LoadInstance(backupFileExtension);
			if ((xmlMimeType != null) && (!String.IsNullOrEmpty(xmlMimeType.MajorType)) && (!String.IsNullOrEmpty(xmlMimeType.Subtype)))
				Response.ContentType = xmlMimeType.FullType;
			else
				Response.ContentType = "text/xml";

			Response.AppendHeader("Content-Disposition", "attachment;filename=" + backupFilename.Replace(" ", String.Empty)); // must remove spaces

			Response.TransmitFile(filePath);
			Response.End();
			//File.Delete(filePath); // Can't delete file because TransmitFile locks it and runs asynchronously, so we'll just let the cleanup code that runs on app startup delete it.
		}

		protected void lbRemoveRestoreFile_Click(object sender, EventArgs e)
		{
			DeletePreviouslyUploadedFile();

			ConfigureBackupFileInfo(null);
		}

		protected void Upload1_Uploaded(object sender, UploadUploadedEventArgs e)
		{
			//string[] importOptions = Upload1.CallbackParameter.Split(new char[] { '|' });
			//bool importMembership = Convert.ToBoolean(importOptions[0]);
			//bool importGalleryData = Convert.ToBoolean(importOptions[1]);

			DeletePreviouslyUploadedFile();

			string filePath = SaveFileToTempDirectory(e.UploadedFiles[0]);

			IBackupFile backupFile = new BackupFile(filePath);
			ValidateRestoreFile(backupFile);

			ConfigureBackupFileInfo(backupFile);

			if (!backupFile.IsValid)
				File.Delete(filePath);
		}

		protected void btnRestore_Click(object sender, EventArgs e)
		{
			string filePath = ViewState["FilePath"].ToString();
			string msg = String.Empty;
			Page.MaintainScrollPositionOnPostBack = false;

			try
			{
				if (File.Exists(filePath))
				{
					HelperFunctions.ImportGalleryData(filePath, chkImportMembership.Checked, chkImportGalleryData.Checked);

					this.wwMessage.CssClass = "wwErrorSuccess msgfriendly bold";
					msg = Resources.GalleryServerPro.Admin_Backup_Restore_Db_Successfully_Restored_Msg;
					wwMessage.ShowMessage(msg);
				}
				else
				{
					wwMessage.CssClass = "wwErrorFailure msgwarning";
					msg = String.Format(Resources.GalleryServerPro.Admin_Backup_Restore_Cannot_Restore_File_File_Not_Found_Msg, filePath);
					wwMessage.ShowError(msg);
				}
			}
			catch (Exception ex)
			{
				this.wwMessage.CssClass = "wwErrorFailure msgwarning";
				msg = String.Concat(Resources.GalleryServerPro.Admin_Backup_Restore_Cannot_Restore_File_Label, ex.Message);
				wwMessage.ShowError(msg);
			}
			finally
			{
				DeletePreviouslyUploadedFile();

				ConfigureBackupFileInfo(null);

				HelperFunctions.PurgeCache();

				// Reset the application name for the Membership, Roles, and Profile providers. In the case of SQLite, this has
				// the effect of reloading the internal _applicationId field in those classes from the data store. Without this,
				// the ID remains the old ID that was initialized when the application first started.
				System.Web.Security.Membership.Provider.ApplicationName = AppSetting.Instance.ApplicationName;
				System.Web.Security.Roles.Provider.ApplicationName = AppSetting.Instance.ApplicationName;
				System.Web.Profile.ProfileManager.Provider.ApplicationName = AppSetting.Instance.ApplicationName;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstPageLoad()
		{
			Master.OkButtonIsVisible = false;
			Master.CancelButtonIsVisible = false;
			Master.AdminPageTitle = Resources.GalleryServerPro.Admin_Backup_Restore_Page_Header;
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

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
				btnExportData.Enabled = false;
				btnRestore.Enabled = false;
			}

		}

		private void AddUploadControlForFullTrust()
		{
			const string htmlBeforeUpload = @"<div class='sel addtopmargin5'>";
			const string htmlAfterUpload = @"</div>";

			Upload upload = new Upload();

			upload.ID = "Upload1";
			upload.MaximumFileCount = 1;
			upload.AutoPostBack = true;
			upload.TempFileFolder = AppSetting.Instance.TempUploadDirectory;
			upload.MaximumUploadSize = 2097151;
			upload.FileInputClientTemplateId = "FileInputTemplate";
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

		private void AddUploadControlForLessThanFullTrust()
		{
			const string htmlBeforeUpload = @"<p>";
			const string htmlAfterUpload = @"</p>";

			FileUpload upload = new FileUpload();
			upload.ID = "fuUpload1";
			upload.Attributes.Add("title", Resources.GalleryServerPro.Task_Add_Objects_Input_Tooltip);
			upload.Attributes.Add("size", "45");

			Button uploadButton = new Button();
			uploadButton.ID = "btnUpload";
			uploadButton.Text = "Upload and validate";
			uploadButton.Click += new EventHandler(uploadButton_Click);

			phUpload.Controls.Add(new LiteralControl(htmlBeforeUpload));
			phUpload.Controls.Add(upload);
			phUpload.Controls.Add(new LiteralControl("&nbsp;"));
			phUpload.Controls.Add(uploadButton);
			phUpload.Controls.Add(new LiteralControl(htmlAfterUpload));
		}

		protected void uploadButton_Click(object sender, EventArgs e)
		{
			DeletePreviouslyUploadedFile();
			FileUpload uploadedFile = (FileUpload)phUpload.FindControl("fuUpload1");
			string filePath = SaveFileToTempDirectory(uploadedFile);

			IBackupFile backupFile = new BackupFile(filePath);
			ValidateRestoreFile(backupFile);

			ConfigureBackupFileInfo(backupFile);

			if (!backupFile.IsValid)
				File.Delete(filePath);
		}

		private static string SaveFileToTempDirectory(UploadedFileInfo fileToRestore)
		{
			// Save file to temp directory, ensuring that we are not overwriting an existing file.
			string newFileName = fileToRestore.FileName;
			int uniqueId = 0;

			while (File.Exists(Path.Combine(AppSetting.Instance.TempUploadDirectory, newFileName)))
			{
				if (fileToRestore.Extension != String.Empty)
				{
					newFileName = fileToRestore.FileName.Replace("." + fileToRestore.Extension, "(" + uniqueId + ")." + fileToRestore.Extension);
				}
				else
				{
					newFileName = fileToRestore.FileName + "(" + uniqueId + ")";
				}

				uniqueId++;
			}

			string filePath = Path.Combine(AppSetting.Instance.TempUploadDirectory, newFileName);
			fileToRestore.SaveAs(filePath);
			return filePath;
		}

		private static string SaveFileToTempDirectory(FileUpload fileToRestore)
		{
			// Save file to temp directory, ensuring that we are not overwriting an existing file.
			string newFileName = fileToRestore.FileName;
			int uniqueId = 0;

			while (File.Exists(Path.Combine(AppSetting.Instance.TempUploadDirectory, newFileName)))
			{
				string extension = Path.GetExtension(fileToRestore.FileName);

				if (!String.IsNullOrEmpty(extension))
				{
					newFileName = fileToRestore.FileName.Replace(extension, "(" + uniqueId + ")" + extension);
				}
				else
				{
					newFileName = fileToRestore.FileName + "(" + uniqueId + ")";
				}

				uniqueId++;
			}

			string filePath = Path.Combine(AppSetting.Instance.TempUploadDirectory, newFileName);
			fileToRestore.SaveAs(filePath);
			return filePath;
		}

		private static void ValidateRestoreFile(IBackupFile backupFile)
		{
			if (Path.GetExtension(backupFile.FilePath).ToLowerInvariant() == ".xml")
				HelperFunctions.ValidateBackupFile(backupFile);
		}

		private void ConfigureBackupFileInfo(IBackupFile backupFile)
		{
			if (backupFile == null)
			{
				lblRestoreFilename.Text = Resources.GalleryServerPro.Admin_Backup_Restore_File_Not_Uploaded_Msg;
				lblRestoreFilename.CssClass = "msgwarning";
				lblNumApps.Text = String.Empty;
				lblNumProfiles.Text = String.Empty;
				lblNumRoles.Text = String.Empty;
				lblNumUsers.Text = String.Empty;
				lblNumUsersInRoles.Text = String.Empty;
				lblNumGalleries.Text = String.Empty;
				lblNumAlbums.Text = String.Empty;
				lblNumMediaObjects.Text = String.Empty;
				lblNumMetadata.Text = String.Empty;
				lblNumRoleAlbums.Text = String.Empty;
				lblNumGalleryRoles.Text = String.Empty;

				btnRestore.Enabled = false;
				imgValidationResult.Visible = false;
				lblValidationResult.Text = String.Empty;
				lblValidationResult.CssClass = String.Empty;
				lbRemoveRestoreFile.Visible = false;

				return;
			}

			lblRestoreFilename.Text = Path.GetFileName(backupFile.FilePath);

			string[] tableNames = new string[] { "aspnet_Applications", "aspnet_Profile", "aspnet_Roles", "aspnet_Users", "aspnet_UsersInRoles", "gs_Gallery", "gs_Album", "gs_MediaObject", "gs_MediaObjectMetadata", "gs_Role_Album", "gs_Role" };

			Dictionary<string, int> dataRecords = backupFile.DataTableRecordCount;

			foreach (string tableName in tableNames)
			{
				switch (tableName)
				{
					case "aspnet_Applications":
						lblNumApps.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "aspnet_Profile":
						lblNumProfiles.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "aspnet_Roles":
						lblNumRoles.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "aspnet_Users":
						lblNumUsers.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "aspnet_UsersInRoles":
						lblNumUsersInRoles.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "gs_Gallery":
						lblNumGalleries.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "gs_Album":
						lblNumAlbums.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "gs_MediaObject":
						lblNumMediaObjects.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "gs_MediaObjectMetadata":
						lblNumMetadata.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "gs_Role_Album":
						lblNumRoleAlbums.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
					case "gs_Role":
						lblNumGalleryRoles.Text = (dataRecords.ContainsKey(tableName) ? backupFile.DataTableRecordCount[tableName].ToString() : String.Empty);
						break;
				}
			}

			if (backupFile.IsValid)
			{
				btnRestore.Enabled = true && !AppSetting.Instance.IsInReducedFunctionalityMode;
				imgValidationResult.ImageUrl = "~/images/info.gif";
				imgValidationResult.Visible = true;
				lblValidationResult.Text = Resources.GalleryServerPro.Admin_Backup_Restore_File_Valid_Msg;
				lblValidationResult.CssClass = "msgsuccess";
				lblRestoreFilename.CssClass = "msgattention";
				lbRemoveRestoreFile.Visible = true;
				lblSchemaVersion.Text = backupFile.SchemaVersion;

				ViewState["FilePath"] = backupFile.FilePath;
			}
			else
			{
				btnRestore.Enabled = false;
				imgValidationResult.ImageUrl = "~/images/warning.gif";
				imgValidationResult.Visible = true;
				lblValidationResult.Text = Resources.GalleryServerPro.Admin_Backup_Restore_File_Not_Valid_Msg;
				lblValidationResult.CssClass = "msgfailure";
				lblRestoreFilename.CssClass = "msgattention";
				lbRemoveRestoreFile.Visible = false;
				lblSchemaVersion.Text = String.Empty;
			}
		}

		private void DeletePreviouslyUploadedFile()
		{
			string filePath = ViewState["FilePath"] as string;

			if (!String.IsNullOrEmpty(filePath))
			{
				File.Delete(filePath);

				ViewState["FilePath"] = null;
			}
		}

		/// <summary>
		/// Adds client templates for the specified Upload control.
		/// </summary>
		private static void AddUploadClientTemplates(Upload upload)
		{
			#region File input client template

			// Add client template for the file input section (includes input tag and browse and upload buttons).
			ClientTemplate uploadFileInputClientTemplate = new ClientTemplate();

			uploadFileInputClientTemplate.ID = "FileInputTemplate";

			uploadFileInputClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<div class=""file"">
	<div class='## DataItem.FileName ? ""filename"" : ""filename empty""; ##'>
		<input value='## DataItem.FileName ? DataItem.FileName : ""{0}""; ##'
			onfocus=""this.blur();"" /></div>
	<a href=""javascript:void(0);"" onclick=""this.blur();return false;"" class=""browse""
		title=""{1}"">#$FileInputImage</a> <a href=""javascript:void(0);"" onclick=""init_upload(Upload1);this.blur();return false;""
			class=""upload"" id=""btn-upload""></a>
</div>
",
			Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_File_Input_Text,
			Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_File_Browse_Button_Tooltip);

			upload.ClientTemplates.Add(uploadFileInputClientTemplate);

			#endregion

			#region Upload progress client template

			// Add client template for the progress section (includes input tag and browse and upload buttons).
			ClientTemplate uploadProgressClientTemplate = new ClientTemplate();

			uploadProgressClientTemplate.ID = "ProgressTemplate";

			uploadProgressClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<!-- Dialogue contents -->
<div class=""con"">
	<div class=""stat"">
		<h3 rel=""total"">
			{0} <span class=""red"">## DataItem.CurrentFile; ##</span></h3>
		<div class=""prog"">
			<div class=""con"">
				<div class=""bar"" style=""width: ## get_percentage(DataItem.Progress) ##%;"">
				</div>
			</div>
		</div>
		<div class=""lbl"">
			<strong>## format_file_size(DataItem.ReceivedBytes) ##</strong> (## get_percentage(DataItem.Progress)
			##%) {1}</div>
	</div>
</div>
<!-- /Dialogue contents -->
<!-- Dialogue footer -->
<div class=""ftr"">
	<div class=""ftr-l"">
	</div>
	<div class=""ftr-m"">
		<div class=""info"" id=""info1"">
			<span>{2} <strong>## format_time(DataItem.ElapsedTime); ##</strong></span>
			<span style=""padding-left: 8px;"">{3} <strong>## format_time(DataItem.ElapsedTime
				+ DataItem.RemainingTime); ##</strong></span> <span style=""padding-left: 8px;"">{4}
					<strong>## DataItem.Speed.toFixed(2) ## {5}</strong></span>
		</div>
		<div class=""btns"">
			<a onclick=""UploadDialog.close();this.blur();return false;"" href=""javascript:void(0);""
				rel=""cancel""><span class=""l""></span><span class=""m"" id=""btn1"">{6}</span>
				<span class=""r""></span></a>
		</div>
	</div>
	<div class=""ftr-r"">
	</div>
</div>
<!-- /Dialogue footer -->
",
					Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Filename_Label, // 0
					Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Bytes_Uploaded_Suffix, // 1
					Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Elapsed_Time_Label, // 2
					Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Estimated_Time_Label, // 3
					Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Speed_Label, // 4
					Resources.GalleryServerPro.Site_KiloBytes_Per_Second_Abbreviation, // 5
					Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Cancel_Upload_Text // 6
				);

			upload.ClientTemplates.Add(uploadProgressClientTemplate);

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
				Resources.GalleryServerPro.Admin_Backup_Restore_Restore_Tab_Upload_Dialog_Uploading_Text);

			UploadDialog.ClientTemplates.Add(uploadDialogClientTemplate);
		}

		#endregion
	}
}

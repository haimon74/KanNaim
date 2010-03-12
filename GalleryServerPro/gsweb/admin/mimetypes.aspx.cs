using System;
using System.Globalization;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Web.admin
{
	public partial class mimetypes : GspPage
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

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
				SaveSettings();
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			ConfigureGrid();

			Master.AdminPageTitle = Resources.GalleryServerPro.Admin_Media_Objects_Mime_Types_Page_Header;

			if (!Master.HasEditConfigPermission)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
				Master.OkButtonBottom.Enabled = false;
				Master.OkButtonTop.Enabled = false;
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
				Master.OkButtonBottom.Enabled = false;
				Master.OkButtonTop.Enabled = false;
			}

			this.wwDataBinder.DataBind();
		}

		private void ConfigureGrid()
		{
			AddClientTemplates();

			gdMimeTypes.DataSource = GalleryServerPro.Business.MimeType.LoadInstances();
			gdMimeTypes.DataBind();
		}

		private void AddClientTemplates()
		{
			// Add table header client template that contains the Check/uncheck all checkbox.
			ComponentArt.Web.UI.ClientTemplate tableHdrClientTemplate = new ComponentArt.Web.UI.ClientTemplate();
			tableHdrClientTemplate.ID = "enabledHeader";

			tableHdrClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"<input id=""chkAll"" type=""checkbox"" onclick=""javascript:setGridCheck(0, this.checked);"" ## chkAllCheckboxIsChecked ? ""checked='checked'"" : """" ## /> <label for=""chkAll"">{0}</label>",
				Resources.GalleryServerPro.Admin_MimeTypes_ToggleCheckAll_Label);

			gdMimeTypes.ClientTemplates.Add(tableHdrClientTemplate);
		}

		private void SaveSettings()
		{
			bool hasChanges = false;

			bool previousAllowAllValue = Master.CoreConfig.AllowUnspecifiedMimeTypes;
			this.wwDataBinder.Unbind(this);
			
			if (Master.CoreConfig.AllowUnspecifiedMimeTypes != previousAllowAllValue)
				hasChanges = true;

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			// Loop through each record in the grid. For each file extension, find the matching MIME types from the 
			// configuration file. If the value has changed, update it. When done looping, save config file.
			MimeTypeCollection mimeTypes = Master.GspConfig.GalleryObject.MimeTypes;

			foreach (GridItem row in gdMimeTypes.Items)
			{
				object[] rowValues = (object[]) row.DataItem;
				bool enabled = Convert.ToBoolean(rowValues[0], CultureInfo.InvariantCulture);
				string fileExt = rowValues[1].ToString();

				foreach (GalleryServerPro.Configuration.MimeType mimeType in mimeTypes.FindAllByFileExtension(fileExt))
				{
					if (mimeType.AllowAddToGallery != enabled)
					{
						hasChanges = true;
						mimeType.AllowAddToGallery = enabled;
					}
				}
			}

			if (hasChanges)
			{
				#region Save with error trapping

				string msgResult = string.Empty;
				try
				{
					Master.Config.Save(System.Configuration.ConfigurationSaveMode.Minimal);

					msgResult = Resources.GalleryServerPro.Admin_Save_Success_Text;
					this.wwMessage.CssClass = "wwErrorSuccess msgfriendly bold";
				}
				catch (System.Configuration.ConfigurationErrorsException ex)
				{
					if ((ex.InnerException != null) && (ex.InnerException is UnauthorizedAccessException))
					{
						// We get here under IIS7/Vista when the IIS account doesn't have permission to galleryserverpro.config.
						string configDir = System.IO.Path.GetDirectoryName(ex.Filename);
						msgResult = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Save_ConfigurationErrorsException_Msg, configDir, System.IO.Path.GetFileName(ex.Filename));
					}
					else
					{
						msgResult = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_Error_Hdr, ex.Message);
					}
					this.wwMessage.CssClass = "wwErrorFailure msgwarning";
				}
				catch (UnauthorizedAccessException ex)
				{
					// We get here under IIS6/Win2003 when the IIS account doesn't have permission to galleryserverpro.config.
					msgResult = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_UnauthorizedAccessException_Msg, ex.Message);
					this.wwMessage.CssClass = "wwErrorFailure msgwarning";
				}
				finally
				{
					this.wwMessage.ShowMessage(msgResult);
				}

				#endregion
			}
		}

		#endregion
	}
}

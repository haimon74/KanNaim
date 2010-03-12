using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using MimeType = GalleryServerPro.Business.MimeType;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class mediaobjecttypes : Pages.AdminPage
	{
		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			ConfigureControlsEveryTime();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
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

		private void ConfigureControlsFirstTime()
		{
			ConfigureGrid();

			AdminPageTitle = Resources.GalleryServerPro.Admin_Media_Objects_Mime_Types_Page_Header;

			if (!HasEditConfigPermission)
			{
				wwMessage.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}

			this.wwDataBinder.DataBind();
		}

		private void ConfigureGrid()
		{
			gdMimeTypes.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/grid/");
			gdMimeTypes.DataSource = GalleryServerPro.Business.MimeType.LoadInstances();
			gdMimeTypes.DataBind();
		}

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Media_Objects_Mime_Types_Page_Header;
			
			AddClientTemplatesToGrid();
		}

		private void AddClientTemplatesToGrid()
		{
			// Add table header client template that contains the Check/uncheck all checkbox.
			ClientTemplate tableHdrClientTemplate = new ClientTemplate();
			tableHdrClientTemplate.ID = "enabledHeader";

			tableHdrClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"<input id=""chkAll"" type=""checkbox"" onclick=""javascript:setGridCheck(0, this.checked);"" ## chkAllCheckboxIsChecked ? ""checked='checked'"" : """" ## /> <label for=""chkAll"">{0}</label>",
				Resources.GalleryServerPro.Admin_MimeTypes_ToggleCheckAll_Label);

			gdMimeTypes.ClientTemplates.Add(tableHdrClientTemplate);
		}

		private void SaveSettings()
		{
			bool previousAllowAllValue = CoreConfig.allowUnspecifiedMimeTypes;
			this.wwDataBinder.Unbind(this);

			if (CoreConfig.allowUnspecifiedMimeTypes != previousAllowAllValue)
			{
				GspConfigController.SaveCore(this.CoreConfig);
			}

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			// Loop through each record in the grid. For each file extension, find the matching MIME types from the 
			// configuration file. If the value has changed, update it. When done looping, save config file.
			Dictionary<string, bool> mimeTypesToUpdate = new Dictionary<string, bool>();

			foreach (GridItem row in gdMimeTypes.Items)
			{
				object[] rowValues = (object[])row.DataItem;
				bool enabled = Convert.ToBoolean(rowValues[0], CultureInfo.InvariantCulture);
				string fileExt = rowValues[1].ToString();

				IMimeType mimeType = MimeType.LoadInstanceByFilePath(fileExt);
				if (mimeType.AllowAddToGallery != enabled)
				{
					mimeTypesToUpdate.Add(mimeType.Extension, enabled);
				}
			}

			if (mimeTypesToUpdate.Count > 0)
			{
				GspConfigController.SaveMimeTypes(mimeTypesToUpdate);
			}

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Save_Success_Text);
		}

		#endregion

	}
}
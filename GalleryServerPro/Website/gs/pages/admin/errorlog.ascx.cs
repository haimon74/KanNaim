using System;
using System.Globalization;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class errorlog : Pages.AdminPage
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

			if (!IsPostBack)
			{
				// Remove errors if needed to ensure log does not exceed max log size. Normally the log size is validated each time an error
				// occurs, but we run it here in case the user just reduced the log size setting.
				ErrorHandler.Error.ValidateLogSize();

				ConfigureControlsFirstPageLoad();

				BindData();
			}

			ConfigureControlsEveryPageLoad();
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			// Persist new config setting to data store.
			SaveSettings();

			// When auto trim is disabled, we store "0" in the config file, but we want to display an empty string
			// in the max # of items textbox. The event wwDataBinder_BeforeUnbindControl may have put a "0" in the 
			// textbox, so undo that now.
			if (txtMaxErrorItems.Text == "0")
				txtMaxErrorItems.Text = String.Empty;

			// Bind the grid. No need to rebind the config controls.
			BindGrid();
		}

		protected void btnClearLog_Click(object sender, EventArgs e)
		{
			ErrorHandler.Error.ClearErrorLog();

			HelperFunctions.PurgeCache();

			BindData();
		}

		protected bool wwDataBinder_BeforeUnbindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// When auto trim is disabled, we store "0" in the config file.
			if (!chkAutoTrimLog.Checked)
				txtMaxErrorItems.Text = "0";

			return true;
		}

		protected void wwDataBinder_AfterBindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// 
			if (item.ControlInstance == txtMaxErrorItems)
			{
				int maxErrorItems = Convert.ToInt32(this.txtMaxErrorItems.Text);
				if (maxErrorItems == 0)
				{
					// Disable the checkbox because feature is turned off (a "0" indicates it is off). Set textbox to
					// an empty string because we don't want to display 0.
					chkAutoTrimLog.Checked = false;
					txtMaxErrorItems.Text = String.Empty;
				}
				else if (maxErrorItems > 0)
					chkAutoTrimLog.Checked = true; // Select the checkbox when max # of items is > 0
				else
				{
					// We'll never get here because the config definition uses an IntegerValidator to force the number
					// to be greater than 0.
				}
			}
		}

		protected bool wwDataBinder_ValidateControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			if (item.ControlInstance == txtMaxErrorItems)
			{
				if ((chkAutoTrimLog.Checked) && (Convert.ToInt32(txtMaxErrorItems.Text) <= 0))
				{
					item.BindingErrorMessage = Resources.GalleryServerPro.Admin_Error_Invalid_MaxNumberErrorItems_Msg;
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstPageLoad()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_Site_Settings_Error_Log_Page_Header;
			OkButtonIsVisible = false;
			CancelButtonIsVisible = false;

			if (!HasEditConfigPermission)
			{
				wwMsgOptions.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
				wwMsgOptions.CssClass = "wwErrorSuccess gsp_msgwarning";
				btnSave.Enabled = false;
				chkAutoTrimLog.Enabled = false;
				chkAutoTrimLog.CssClass = "gsp_disabledtext";
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				btnSave.Enabled = false;
				btnClearLog.Enabled = false;
			}

			btnClearLog.OnClientClick = String.Concat("return confirm('", Resources.GalleryServerPro.Admin_Error_ClearLog_Confirm_Txt, "')");
			cpe.ExpandedImage = Util.GetUrl("/images/collapse.jpg");
			cpe.CollapsedImage = Util.GetUrl("/images/expand.jpg");
			imgExpCol.ImageUrl = Util.GetUrl("/images/expand.jpg");

			ConfigureGrid();
		}

		private void ConfigureControlsEveryPageLoad()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Site_Settings_Error_Log_Page_Header;

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}
			else
			{
				AddEditColumnClientTemplate();
			}

			AddGridClientTemplates();
		}

		private void ConfigureGrid()
		{
			gd.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/grid/");
			gd.TreeLineImagesFolderUrl = String.Concat(Util.GalleryRoot, "/images/componentart/grid/lines/");

			string emptyGridText = String.Format(CultureInfo.CurrentCulture, "<span class=\"gsp_msgfriendly gdInfoEmptyGridText\">{0}</span>", Resources.GalleryServerPro.Admin_Error_Grid_Empty_Text);
			gd.EmptyGridText = emptyGridText;
		}

		private void AddEditColumnClientTemplate()
		{
			//Add the client template containing the editing controls.
			ClientTemplate optionsClientTemplate = new ClientTemplate();
			optionsClientTemplate.ID = "ctOptions";

			optionsClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<a href=""javascript:deleteRow('## DataItem.ClientId ##')"" title='{0}' class=""gsp_addleftmargin1"">
				<img src='{1}' alt='{0}' /></a>
					",
				Resources.GalleryServerPro.Admin_Error_Grid_Ex_Delete_Tooltip,
				Util.GetUrl("images/componentart/grid/delete.png")
				);

			gd.ClientTemplates.Add(optionsClientTemplate);
		}

		private void AddGridClientTemplates()
		{
			ClientTemplate ctErrDetails = new ClientTemplate();
			ctErrDetails.ID = "ctErrDetails";
			ctErrDetails.Text = Resources.GalleryServerPro.Admin_Error_Error_Details_Hdr;

			gd.ClientTemplates.Add(ctErrDetails);
		}

		private void BindData()
		{
			BindGrid();

			BindConfigOptions();
		}

		private void BindGrid()
		{
			gd.DataSource = Controller.AppErrorController.GetAppErrorsDataSet();
			gd.DataBind();
		}

		private void BindConfigOptions()
		{
			wwDataBinder.DataBind();
		}

		private void SaveSettings()
		{
			this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			GspConfigController.SaveCore(this.CoreConfig);

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Save_Success_With_Refresh_Note_Text);
		}

		#endregion
	}
}
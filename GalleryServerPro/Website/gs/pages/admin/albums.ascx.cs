using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Configuration;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class albums : Pages.AdminPage
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

				// When paging is disabled, we store "0" in the config file, but we want to display an empty string
				// in the page size textbox. The event wwDataBinder_BeforeUnbindControl may have put a "0" in the 
				// textbox, so undo that now.
				if (txtPageSize.Text == "0")
					txtPageSize.Text = String.Empty;
			}

			return true;
		}

		protected void cvColor_ServerValidate(object sender, ServerValidateEventArgs args)
		{
			try
			{
				HelperFunctions.GetColor(args.Value.Trim());
				args.IsValid = true;
			}
			catch (ArgumentNullException) { args.IsValid = false; }
			catch (ArgumentOutOfRangeException) { args.IsValid = false; }
		}

		protected void wwDataBinder_AfterBindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			if (item.ControlInstance == txtPageSize)
			{
				int pageSize = Convert.ToInt32(this.txtPageSize.Text);
				if (pageSize == 0)
				{
					// Disable the checkbox because feature is turned off (a "0" indicates it is off). Set textbox to
					// an empty string because we don't want to display 0.
					chkEnablePaging.Checked = false;
					txtPageSize.Text = String.Empty;
				}
				else if (pageSize > 0)
					chkEnablePaging.Checked = true; // Select the checkbox when max # of items is > 0
				else
				{
					// We'll never get here because the config definition uses an IntegerValidator to force the number
					// to be greater than 0.
				}
			}

		}

		protected bool wwDataBinder_BeforeUnbindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			if (!this.chkEnablePaging.Checked)
			{
				// When paging is disabled, we store "0" in the config file.
				if (item.ControlId == this.txtPageSize.ID)
				{
					txtPageSize.Text = "0";
					return true; // true indicates that we want to save this setting to the config file
				}

				// When paging is unchecked, several child items are disabled via javascript. Disabled HTML items are not
				// posted during a postback, so we don't have accurate information about their states. For these controls 
				// (except the page size as configured above) don't save anything by returning false. Furthermore, to 
				// prevent these child controls from incorrectly reverting to an empty or unchecked state in the UI, 
				// assign their properties to their config setting. 
				if (item.ControlId == this.ddlPagerLocation.ID)
				{
					this.ddlPagerLocation.SelectedValue = Core.PagerLocation;
					return false;
				}
			}

			return true;
		}

		protected bool wwDataBinder_ValidateControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			if (item.ControlInstance == txtPageSize)
			{
				if ((chkEnablePaging.Checked) && (Convert.ToInt32(txtPageSize.Text) <= 0))
				{
					item.BindingErrorMessage = Resources.GalleryServerPro.Admin_Error_Invalid_PageSize_Msg;
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Albums_General_Page_Header;
		}

		private void ConfigureControlsFirstTime()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_Albums_General_Page_Header;

			if (!HasEditConfigPermission)
			{
				wwMessage.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
				chkEnablePaging.Enabled = false;

				foreach (System.Web.UI.Control ctl in this.Controls)
				{
					if ((ctl is CheckBox) || (ctl is TextBox) || (ctl is DropDownList))
					{
						((WebControl)ctl).Enabled = false;
					}
				}
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}

			this.wwDataBinder.DataBind();

			ddlPagerLocation.DataSource = Enum.GetValues(typeof(GalleryServerPro.Business.PagerPosition));
			ddlPagerLocation.DataBind();
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
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Save_Success_Text);

		}

		#endregion


	}
}
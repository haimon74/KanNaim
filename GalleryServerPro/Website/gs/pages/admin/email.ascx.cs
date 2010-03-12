using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class email : Pages.AdminPage
	{
		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			CheckUserSecurity(SecurityActions.AdministerSite);

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

		protected void btnEmailTest_Click(object sender, EventArgs e)
		{
			SendTestEmail();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Site_Settings_Email_Page_Header;
		}

		private void ConfigureControlsFirstTime()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_Site_Settings_Email_Page_Header;

			if (!HasEditConfigPermission)
			{
				wwMessage.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;

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

		private void SendTestEmail()
		{
			string subject = Resources.GalleryServerPro.Admin_Email_Test_Email_Subject;
			string body = Resources.GalleryServerPro.Admin_Email_Test_Email_Body;
			string msgResult = String.Empty;
			bool emailSent = false;
			try
			{
				EmailController.SendEmail(subject, body, false);
				emailSent = true;
			}
			catch (Exception ex)
			{
				string errorMsg = Util.GetExceptionDetails(ex);

				msgResult = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Email_Test_Email_Failure_Text, errorMsg);
			}

			if (emailSent)
			{
				this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly";
				msgResult = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Email_Test_Email_Success_Text, Web.Config.GetCore().EmailToName, Web.Config.GetCore().EmailToAddress);

			}
			else
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
			}

			this.wwMessage.ShowMessage(msgResult);
		}

		#endregion
	}
}
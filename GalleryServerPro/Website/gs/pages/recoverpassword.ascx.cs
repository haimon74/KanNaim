using System;
using System.Globalization;
using System.Web.Security;
using GalleryServerPro.Business;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class recoverpassword : Pages.GalleryPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControls();
		}

		private void ConfigureControls()
		{
			txtUserName.Focus();

			string msgTitle = String.Empty;
			string msgDetail = String.Empty;

			if (UserController.MembershipGsp.RequiresQuestionAndAnswer)
			{
				msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Header;
				msgDetail = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Because_Question_Answer_Is_Enabled_Body;
			}

			if (!UserController.MembershipGsp.EnablePasswordRetrieval)
			{
				msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Header;
				msgDetail = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Because_Password_Retrieval_Is_Disabled_Body;
			}

			if (!String.IsNullOrEmpty(msgTitle))
			{
				pnlMsgContainer.Controls.Clear();
				GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage) Page.LoadControl(Util.GetUrl("/controls/usermessage.ascx"));

				msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Warning;
				msgBox.MessageTitle = msgTitle;
				msgBox.MessageDetail = msgDetail;
				msgBox.CssClass = "um3ContainerCss gsp_rounded10 gsp_floatcontainer";
				msgBox.HeaderCssClass = "um1HeaderCss";
				msgBox.DetailCssClass = "um1DetailCss";
				pnlMsgContainer.Controls.Add(msgBox);

				pnlPwdRecoverContainer.Visible = false;
			}
		}

		protected void btnRetrievePassword_Click(object sender, EventArgs e)
		{
			RecoverPassword();
		}

		private void RecoverPassword()
		{
			GalleryServerPro.Web.MessageStyle iconStyle = GalleryServerPro.Web.MessageStyle.Warning;
			string msgTitle;
			string msgDetail;

			UserEntity user = UserController.GetUser(txtUserName.Text, true);

			if (user == null)
			{
				msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Header;
				msgDetail = String.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", Resources.GalleryServerPro.Anon_Pwd_Recovery_GetUser_Throws_Exception_Message);
			}
			else
			{
				if (HelperFunctions.IsValidEmail(user.Email))
				{
					// Send user's password in an email.
					try
					{
						EmailController.SendNotificationEmail(user, Entity.EmailTemplateForm.UserNotificationPasswordRecovery, false);
						
						msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Confirmation_Title;
						string url = String.Format(CultureInfo.CurrentCulture, " <a href='{0}'>{1}</a>", Web.Util.GetUrl(PageId.login), Resources.GalleryServerPro.Login_Button_Text);
						msgDetail = String.Concat(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Anon_Pwd_Recovery_Confirmation_Body, user.Email), url);
						iconStyle = GalleryServerPro.Web.MessageStyle.Information;
						pnlPwdRecoverContainer.Visible = false;
					}
					catch (Exception ex)
					{
						string errorMsg;
						if (Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ShowErrorDetails)
							errorMsg = Util.GetExceptionDetails(ex);
						else
							errorMsg = Resources.GalleryServerPro.Error_Generic_Msg;

						msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Header;
						msgDetail = "<p>" + String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Generic_Error, errorMsg) + "</p>";
					}
				}
				else // User is valid but does not have a valid email
				{
					msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Header;
					msgDetail = String.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Invalid_Email);
				}
			}

			#region Render Confirmation Message

			pnlMsgContainer.Controls.Clear();
			GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)Page.LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
			msgBox.IconStyle = iconStyle;
			msgBox.MessageTitle = msgTitle;
			msgBox.MessageDetail = msgDetail;
			msgBox.CssClass = "um3ContainerCss";
			msgBox.HeaderCssClass = "um1HeaderCss";
			msgBox.DetailCssClass = "um1DetailCss";
			pnlMsgContainer.Controls.Add(msgBox);

			#endregion
		}
	}
}
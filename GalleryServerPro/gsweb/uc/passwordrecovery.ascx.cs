using System;
using System.Globalization;
using System.Web.Security;
using GalleryServerPro.Business;

namespace GalleryServerPro.Web.uc
{
	public partial class passwordrecovery : System.Web.UI.UserControl
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
			
			if (Membership.RequiresQuestionAndAnswer)
			{
				msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Header;
				msgDetail = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Because_Question_Answer_Is_Enabled_Body;
			}

			if (!Membership.EnablePasswordRetrieval)
			{
				msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Header;
				msgDetail = Resources.GalleryServerPro.Anon_Pwd_Recovery_Disabled_Because_Password_Retrieval_Is_Disabled_Body;
			}

			if (!String.IsNullOrEmpty(msgTitle))
			{
				pnlMsgContainer.Controls.Clear();
				GalleryServerPro.Web.uc.usermessage msgBox = (GalleryServerPro.Web.uc.usermessage)LoadControl("~/uc/usermessage.ascx");
				msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Warning;
				msgBox.MessageTitle = msgTitle;
				msgBox.MessageDetail = msgDetail;
				msgBox.CssClass = "um3ContainerCss";
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
			string msgTitle = String.Empty;
			string msgDetail = String.Empty;

			MembershipUser user = null;
			try
			{
				user = Membership.GetUser(txtUserName.Text);
			}
			catch
			{
				msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Header;
				msgDetail = Resources.GalleryServerPro.Anon_Pwd_Recovery_GetUser_Throws_Exception_Message;
			}

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
					string pwd = user.GetPassword();

					string subject = Resources.GalleryServerPro.Anon_Pwd_Recovery_Email_Subject;
					string body = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Anon_Pwd_Recovery_Email_Body, user.UserName, pwd);
					try
					{
						WebsiteController.SendEmail(new System.Net.Mail.MailAddress(user.Email), subject, body, true);
						msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Confirmation_Title;
						msgDetail = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Anon_Pwd_Recovery_Confirmation_Body, user.Email);
						iconStyle = GalleryServerPro.Web.MessageStyle.Information;
						pnlPwdRecoverContainer.Visible = false;
					}
					catch (Exception ex)
					{
						string exMsg = ex.Message;
						Exception innerException = ex.InnerException;
						while (innerException != null)
						{
							exMsg += " " + innerException.Message;
							innerException = innerException.InnerException;
						}
						msgTitle = Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Header;
						msgDetail = "<p>" + String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Anon_Pwd_Recovery_Cannot_Retrieve_Pwd_Generic_Error, exMsg) + "</p>";
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
			GalleryServerPro.Web.uc.usermessage msgBox = (GalleryServerPro.Web.uc.usermessage)LoadControl("~/uc/usermessage.ascx");
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
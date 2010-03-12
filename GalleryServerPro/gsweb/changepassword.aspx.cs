using System;
using System.Net.Mail;

namespace GalleryServerPro.Web
{
	public partial class changepassword : GspPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsAnonymousUser)
				this.RedirectToHomePage();
			else
				ConfigureControlsFirstTime();

			ChangePassword1.Focus();
		}

		private void ConfigureControlsFirstTime()
		{
			GalleryServerPro.Configuration.Core coreConfig = WebsiteController.GetGalleryServerProConfigSection().Core;
			ChangePassword1.MailDefinition.From = coreConfig.EmailFromAddress;

			ChangePassword1.CancelDestinationPageUrl = this.GetReferringPageUrl();
			ChangePassword1.ContinueDestinationPageUrl ="myaccount.aspx";
		}

		protected void ChangePassword1_SendingMail(object sender, System.Web.UI.WebControls.MailMessageEventArgs e)
		{
			// By default the ChangePassword control will use the mail settings in web.config. But since Gallery Server allows setting the 
			// SMTP server and port in galleryserverpro.config, the user might not have configured web.config, resulting in a failed e-mail.
			// To prevent this, we'll grab the e-mail and send it ourselves, then cancel the original e-mail.
			MailMessage msg = e.Message;
			string body = Resources.GalleryServerPro.Change_Pwd_Email_Body;
			WebsiteController.SendEmail(msg.To, msg.Subject, body, msg.IsBodyHtml);
			e.Cancel = true;
		}
	}
}

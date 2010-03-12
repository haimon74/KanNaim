using System;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class changepassword : Pages.GalleryPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsAnonymousUser)
				Util.Redirect(Web.PageId.album);
			else
				ConfigureControlsFirstTime();

			cp1.Focus();
		}

		private void ConfigureControlsFirstTime()
		{
			cp1.MembershipProvider = UserController.MembershipGsp.Name;
			cp1.CancelDestinationPageUrl = this.PreviousUrl;
			cp1.ContinueDestinationPageUrl = Util.GetUrl(Web.PageId.myaccount);
		}

		protected void cp1_SendingMail(object sender, System.Web.UI.WebControls.MailMessageEventArgs e)
		{
			// By default the ChangePassword control will use the mail settings in web.config. But since Gallery Server allows setting the 
			// SMTP server and port in galleryserverpro.config, the user might not have configured web.config, resulting in a failed e-mail.
			// To prevent this, we'll send our own e-mail, then cancel the original one.
			EmailController.SendNotificationEmail(UserController.GetUser(), Entity.EmailTemplateForm.UserNotificationPasswordChanged);

			e.Cancel = true;
		}

		protected void cp1_CancelButtonClick(object sender, EventArgs e)
		{
			this.RedirectToPreviousPage();
		}
	}
}
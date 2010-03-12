using System;
using System.Web.Security;

namespace GalleryServerPro.Web
{
	public partial class myaccount : GspPage
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsAnonymousUser)
				this.RedirectToHomePage();

			if (!IsPostBack)
				ConfigureControlsFirstTime();
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SaveUserInfo();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToReferringPage();
		}
		
		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			MembershipUser user = Membership.GetUser();
			litUserName.Text = user.UserName;
			txtEmail.Text = user.Email;
		}

		private void SaveUserInfo()
		{
			MembershipUser user = Membership.GetUser();
			user.Email = txtEmail.Text;
			Membership.UpdateUser(user);

			string msgResult = Resources.GalleryServerPro.MyAccount_Save_Success_Text;
			this.wwMessage.CssClass = "wwErrorSuccess msgfriendly bold";
			this.wwMessage.ShowMessage(msgResult);
		}
		
		#endregion
	}
}

using System;

namespace GalleryServerPro.Web
{
	public partial class login : GspPage
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}

			Login1.Focus();
		}
		
		#endregion
		
		
		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			this.Master.ShowLoginAndSearch = false;

			if (this.Message == Message.UserNameOrPasswordIncorrect)
			{
				string msg = Resources.GalleryServerPro.Login_FailureText;
				pnlInvalidLogin.Controls.Add(new System.Web.UI.LiteralControl(msg));
				pnlInvalidLogin.Visible = true;
			}
		}
 
		#endregion
	}
}

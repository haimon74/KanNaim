using System;
using System.Web.Security;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;
using System.Globalization;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class myaccount : Pages.GalleryPage
	{
		#region Private Fields

		private UserEntity _user;
		private ProfileEntity _currentProfile;

		#endregion

		#region Public Properties

		public UserEntity CurrentUser
		{
			get
			{
				if (this._user == null)
					_user = UserController.GetUser();

				return this._user;
			}
		}

		public ProfileEntity CurrentProfile
		{
			get
			{
				if (this._currentProfile == null)
				{
					this._currentProfile = ProfileController.GetProfile();
				}

				return this._currentProfile;
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsAnonymousUser)
				Util.Redirect(Web.PageId.album);

			if (!Core.AllowManageOwnAccount)
				Util.Redirect(Web.PageId.album);

			if (!IsPostBack)
				ConfigureControlsFirstTime();

			RegisterJavascript();
		}

		private void RegisterJavascript()
		{
			// Register some startup script that will make the user album warning invisible.
			string script = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"
var msgbox = $get('{0}');
if (msgbox != null)
	msgbox.style.display = 'none';",
	wwUserAlbumWarning.ClientID);

			System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "myAccountStartupScript", script, true);

			// Register some script to handle the click event of the user album checkbox.
			script = String.Format(System.Globalization.CultureInfo.InvariantCulture, @"
function toggleWarning(chk)
	{{
		if (chk.checked)
			$get('{0}').style.display = 'none';
		else
			$get('{0}').style.display = '';
	}}
",
 wwUserAlbumWarning.ClientID);

			System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myAccountScript", script, true);

		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToPreviousPage();
		}

		protected void lbDeleteAccount_Click(object sender, EventArgs e)
		{
			ProcessAccountDeletion();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			hlChangePwd.NavigateUrl = Web.Util.GetUrl(Web.PageId.changepassword);

			lbDeleteAccount.OnClientClick = String.Format(CultureInfo.CurrentCulture, "return confirm('{0}')", Resources.GalleryServerPro.MyAccount_Delete_Account_Confirmation);

			if (Core.EnableUserAlbum)
			{
				litDeleteAccountWarning.Text = Resources.GalleryServerPro.MyAccount_Delete_Account_With_User_Albums_Warning;
			}
			else
			{
				litDeleteAccountWarning.Text = Resources.GalleryServerPro.MyAccount_Delete_Account_Warning;
				pnlUserAlbum.Visible = false;
			}

			if (Core.AllowDeleteOwnAccount)
			{
				pnlDeleteAccount.Visible = true;
			}

			this.wwDataBinder.DataBind();
		}

		private void SaveSettings()
		{
			bool success = this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			UserController.UpdateUser(this.CurrentUser);

			SaveProfile(this.CurrentProfile);

			if (this.CurrentProfile.EnableUserAlbum)
			{
				UserController.ValidateUserAlbum(this.CurrentProfile.UserName);
			}

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.MyAccount_Save_Success_Text);
		}

		private static void SaveProfile(ProfileEntity userProfile)
		{
			// Get reference to user's album. We need to do this *before* saving the profile, because if the user disabled their user album,
			// this method will return null after saving the profile.
			IAlbum album = UserController.GetUserAlbum();

			if (!userProfile.EnableUserAlbum)
			{
				userProfile.UserAlbumId = 0;
			}

			ProfileController.SaveProfile(userProfile);

			if (!userProfile.EnableUserAlbum)
			{
				AlbumController.DeleteAlbum(album);
			}
		}

		private void ProcessAccountDeletion()
		{
			try
			{
				UserController.DeleteGalleryServerProUser(this.CurrentUser.UserName, false);
			}
			catch (WebException ex)
			{
				int errorId = AppErrorController.LogError(ex);
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.ShowMessage(String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.MyAccount_Delete_Account_Err_Msg, errorId, ex.GetType()));

				return;
			}

			FormsAuthentication.SignOut();

			UserController.UserLoggedOff();

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.MyAccount_Delete_Account_Success_Text);
		}

		#endregion
	}
}
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace GalleryServerPro.Web.Controls
{
	public partial class login : GalleryUserControl
	{
		#region Private Fields

		private Login _login1;
		private LoginStatus _loginStatus1;
		private LoginName _loginName1;

		#endregion

		#region Properties

		/// <summary>
		/// Gets a reference to the Login control on the page. This property recursively searches the Dialog control
		/// to find it.
		/// </summary>
		/// <value>The Login control on the page.</value>
		protected Login Login1
		{
			get
			{
				if (_login1 == null)
				{
					Dialog dg = (Dialog)this.GalleryPage.FindControlRecursive(lv, "dgLogin");
					this._login1 = (Login)this.GalleryPage.FindControlRecursive(dg, "Login1");
				}

				return this._login1;
			}
		}

		/// <summary>
		/// Gets a reference to the LoginStatus control on the page. This property recursively searches the LoginView control
		/// to find it. Returns null when no user is logged on.
		/// </summary>
		/// <value>The LoginStatus control on the page.</value>
		protected LoginStatus LoginStatus1
		{
			get
			{
				if (_loginStatus1 == null)
				{
					this._loginStatus1 = (LoginStatus)this.GalleryPage.FindControlRecursive(lv, "LoginStatus1");
				}

				return this._loginStatus1;
			}
		}

		/// <summary>
		/// Gets a reference to the LoginName control on the page. This property recursively searches the LoginView control
		/// to find it. Returns null when no user is logged on.
		/// </summary>
		/// <value>The LoginName control on the page.</value>
		protected LoginName LoginName1
		{
			get
			{
				if (_loginName1 == null)
				{
					this._loginName1 = (LoginName)this.GalleryPage.FindControlRecursive(lv, "LoginName1");
				}

				return this._loginName1;
			}
		}

		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControls();

			RegisterJavascript();
		}

		protected void Login1_LoginError(object sender, EventArgs e)
		{
			// The user has entered an invalid user name and/or error. Redirect to login page and append message.
			Util.Redirect(PageId.login, "msg={0}&ReturnUrl={1}", ((int)Message.UserNameOrPasswordIncorrect).ToString(CultureInfo.InvariantCulture), Util.UrlEncode(Request.RawUrl));
		}

		protected void Login1_LoggedIn(object sender, EventArgs e)
		{
			Controller.UserController.UserLoggedOn(Login1.UserName);

			if (this.GalleryPage.Core.EnableUserAlbum && this.GalleryPage.Core.RedirectToUserAlbumAfterLogin)
			{
				Util.Redirect(Util.GetUrl(PageId.album, "aid={0}", Controller.UserController.GetUserAlbumId(Login1.UserName)));
			}

			ReloadPage();
		}

		protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
		{
			Controller.UserController.UserLoggedOff();

			ReloadPage();
		}

		protected string GetAccountLinkText()
		{
			return String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Login_My_Account_Link_Text, Util.UserName);
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			if (this.GalleryPage.GalleryControl.ShowLogin)
			{
				ConfigureLogin();
			}
		}

		private void ConfigureLogin()
		{
			if (String.IsNullOrEmpty(Resources.GalleryServerPro.Login_Logged_On_Msg))
			{
				LoginName1.Visible = false;
			}

			ConfigureLoginDialog();
		}

		private void ConfigureLoginDialog()
		{
			if (this.GalleryPage.IsAnonymousUser)
			{
				Login1.MembershipProvider = Controller.UserController.MembershipGsp.Name;
				Login1.PasswordRecoveryUrl = Util.GetUrl(Web.PageId.recoverpassword);

				if (this.GalleryPage.Core.EnableSelfRegistration)
				{
					Login1.CreateUserText = Resources.GalleryServerPro.Login_Create_Account_Text;
					Login1.CreateUserUrl = Util.GetUrl(Web.PageId.createaccount);
				}
			}
			else
			{
				if (LoginStatus1 != null)
				{
					// The LoginStatus control should really never be null for logged on users, but in reality it sometimes is.
					// Not sure why - may be a bug in the control. We'll get around it by checking for null. Not a big deal since
					// all that happens is there is text instead of an image for the logout link.
					LoginStatus1.LogoutImageUrl = Util.GetUrl("/images/logoff.png");
				}
			}
		}

		private void RegisterJavascript()
		{
			if (this.GalleryPage.IsAnonymousUser && this.GalleryPage.GalleryControl.ShowLogin)
			{
				// Get reference to the UserName textbox.
				TextBox tb = (TextBox)this.GalleryPage.FindControlRecursive(Login1, "UserName");

				string script = String.Format(@"
function toggleLogin()
{{
	if (typeof(dgSearch) !== 'undefined')
	 dgSearch.close();

	if (dgLogin.get_isShowing())
		dgLogin.close();
	else
		dgLogin.show();
}}

function dgLogin_OnShow()
{{
	$get('{0}').focus();
}}", tb.ClientID);

				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "loginFocusScript", script, true);
			}
		}

		private void ReloadPage()
		{
			// If currently looking at a media object or album, update query string to point to current media object or
			// album page (if album paging is enabled) and redirect. Otherwise just navigate to current album.
			PageId pageId = this.GalleryPage.PageId;
			if ((pageId == PageId.album) || (pageId == PageId.mediaobject))
			{
				string url = Request.Url.PathAndQuery;

				url = Util.RemoveQueryStringParameter(url, "msg"); // Remove any messages

				if (this.GalleryPage.MediaObjectId > int.MinValue)
				{
					url = Util.RemoveQueryStringParameter(url, "moid");
					url = Util.AddQueryStringParameter(url, String.Concat("moid=", this.GalleryPage.MediaObjectId));
				}

				int page = Util.GetQueryStringParameterInt32(this.GalleryPage.PreviousUri, "page");
				if (page > int.MinValue)
				{
					url = Util.RemoveQueryStringParameter(url, "page");
					url = Util.AddQueryStringParameter(url, String.Concat("page=", page));
				}

				Util.Redirect(url);
			}
			else
			{
				Util.Redirect(PageId.album, "aid={0}", this.GalleryPage.AlbumId);
			}
		}

		#endregion

	}
}
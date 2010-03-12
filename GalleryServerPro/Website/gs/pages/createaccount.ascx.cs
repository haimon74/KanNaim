using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class createaccount : Pages.GalleryPage
	{

		#region Private Fields

		private bool? _enableUserAlbum;
		private bool? _enableEmailVerification;
		private bool? _requireAdminApproval;
		private bool? _useEmailForAccountName;

		#endregion

		#region Public Properties

		public bool EnableUserAlbum
		{
			get
			{
				if (!this._enableUserAlbum.HasValue)
					this._enableUserAlbum = Core.EnableUserAlbum;

				return this._enableUserAlbum.Value;
			}
		}

		public bool EnableEmailVerification
		{
			get
			{
				if (!this._enableEmailVerification.HasValue)
					this._enableEmailVerification = Core.RequireEmailValidationForSelfRegisteredUser;

				return this._enableEmailVerification.Value;
			}
		}

		public bool RequireAdminApproval
		{
			get
			{
				if (!this._requireAdminApproval.HasValue)
					this._requireAdminApproval = Core.RequireApprovalForSelfRegisteredUser;

				return this._requireAdminApproval.Value;
			}
		}

		public bool UseEmailForAccountName
		{
			get
			{
				if (!this._useEmailForAccountName.HasValue)
					this._useEmailForAccountName = Core.UseEmailForAccountName;

				return this._useEmailForAccountName.Value;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Util.IsQueryStringParameterPresent("verify"))
				ValidateUser();

			if (!IsAnonymousUser)
				Util.Redirect(Web.PageId.album);

			if (!Core.EnableSelfRegistration)
				Util.Redirect(Web.PageId.album);

			ConfigureControls();
		}

		protected void cbValidateNewUserName_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
		{
			// The user just typed in a user name in the Add New User wizard. Let's check to see if it already
			// exists and let the user know the result.
			string requestedUsername = e.Parameter;

			if (String.IsNullOrEmpty(requestedUsername))
			{
				lblUserNameValidationResult.Text = String.Empty;
			}
			else
			{
				if (UseEmailForAccountName && (!HelperFunctions.IsValidEmail(requestedUsername)))
				{
					// App is configured to use an e-mail address as the account name, but the name is not a valid
					// e-mail.
					lblUserNameValidationResult.Text = Resources.GalleryServerPro.CreateAccount_Verification_Username_Not_Valid_Email_Text;
					lblUserNameValidationResult.CssClass = "gsp_msgwarning";
				}
				else if (Util.RemoveHtmlTags(requestedUsername).Length != requestedUsername.Length)
				{
					// The user name has HTML tags, which are not allowed.
					lblUserNameValidationResult.Text = Resources.GalleryServerPro.Site_Invalid_Text;
					lblUserNameValidationResult.CssClass = "gsp_msgwarning";
				}
				else
				{
					// We passed the first test above. Now verify that the requested user name is not already taken.
					UserEntity user = UserController.GetUser(requestedUsername, false);

					bool userNameIsInUse = (user != null);

					if (userNameIsInUse)
					{
						lblUserNameValidationResult.Text = Resources.GalleryServerPro.Admin_Manage_Users_Username_Already_In_Use_Msg;
						lblUserNameValidationResult.CssClass = "gsp_msgwarning";
					}
					else
					{
						lblUserNameValidationResult.Text = Resources.GalleryServerPro.Admin_Manage_Users_Username_Already_Is_Valid_Msg;
						lblUserNameValidationResult.CssClass = "gsp_msgfriendly";
					}
				}
			}

			lblUserNameValidationResult.RenderControl(e.Output);
		}

		private void ConfigureControls()
		{
			txtNewUserUserName.Focus();

			if (UseEmailForAccountName)
			{
				trEmail.Visible = false;
				l2.Text = Resources.GalleryServerPro.CreateAccount_Email_Header_Text;
			}

			if (this.EnableEmailVerification)
			{
				lblEmailReqd.Visible = true;
				rfvEmail.Enabled = true;
			}

			RegisterJavaScript();
		}

		private void RegisterJavaScript()
		{
			string script = String.Format(CultureInfo.InvariantCulture, @"

			function validateNewUserName(userNameTextbox)
			{{
				var newUserName = userNameTextbox.value;
				cbValidateNewUserName.callback(newUserName);
			}}

");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "createUserFunctions", script, true);
		}

		protected void btnCreateAccount_Click(object sender, EventArgs e)
		{
			CreateAccount();
		}

		private void CreateAccount()
		{
			try
			{
				UserEntity user = this.AddUser();

				ReportSuccess(user);
			}
			catch (MembershipCreateUserException ex)
			{
				// Just in case we created the user and the exception occured at a later step, like adding the roles, delete the user
				// and - if it exists - the user album, but only if the user exists AND the error wasn't 'DuplicateUserName'.
				if ((ex.StatusCode != MembershipCreateStatus.DuplicateUserName) && (UserController.GetUser(this.txtNewUserUserName.Text, false) != null))
				{
					DeleteUserAlbum();

					UserController.DeleteUser(this.txtNewUserEmail.Text);
				}

				this.DisplayErrorMessage(Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Create_User_Msg, UserController.GetAddUserErrorMessage(ex.StatusCode));
			}
			catch (Exception ex)
			{
				// Just in case we created the user and the exception occured at a later step, like adding the roles, delete the user
				// and - if it exists - the user album.
				DeleteUserAlbum();

				if (UserController.GetUser(this.txtNewUserUserName.Text, false) != null)
				{
					UserController.DeleteUser(this.txtNewUserUserName.Text);
				}

				this.DisplayErrorMessage(Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Create_User_Msg, ex.Message);
			}
		}

		private void DeleteUserAlbum()
		{
			if (String.IsNullOrEmpty(this.txtNewUserUserName.Text))
				return;

			if (Core.EnableUserAlbum)
			{
				IAlbum album;

				try
				{
					album = Factory.LoadAlbumInstance(ProfileController.GetProfile(this.txtNewUserUserName.Text).UserAlbumId, false);
				}
				catch (InvalidAlbumException) { return; }

				if (album != null)
				{
					AlbumController.DeleteAlbum(album);
				}
			}
		}

		private void ReportSuccess(UserEntity user)
		{
			string title = Resources.GalleryServerPro.CreateAccount_Success_Header_Text;

			string detailPendingNotification = String.Concat("<p>", Resources.GalleryServerPro.CreateAccount_Success_Detail1_Text, "</p>");
			detailPendingNotification += String.Concat(@"<p>", String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.CreateAccount_Success_Pending_Notification_Detail2_Text, user.Email), "</p>");
			detailPendingNotification += String.Concat(@"<p>", Resources.GalleryServerPro.CreateAccount_Success_Pending_Notification_Detail3_Text, "</p>");

			string detailPendingApproval = String.Concat("<p>", Resources.GalleryServerPro.CreateAccount_Success_Detail1_Text, "</p>");
			detailPendingApproval += String.Concat(@"<p>", String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.CreateAccount_Success_Pending_Approval_Detail2_Text), "</p>");
			detailPendingApproval += String.Concat(@"<p>", Resources.GalleryServerPro.CreateAccount_Success_Pending_Approval_Detail3_Text, "</p>");

			string detailActivated = string.Format(@"<p>{0}</p><p><a href=""{1}"">{2}</a></p>",
			                                       Resources.GalleryServerPro.CreateAccount_Success_Detail1_Text,
			                                       Util.GetCurrentPageUrl(),
			                                       Resources.GalleryServerPro.CreateAccount_Gallery_Link_Text);

			if (EnableEmailVerification)
			{
				DisplaySuccessMessage(title, detailPendingNotification);
			}
			else if (RequireAdminApproval)
			{
				DisplaySuccessMessage(title, detailPendingApproval);
			}
			else
			{
				UserController.LogOnUser(user.UserName);

				if (EnableUserAlbum && (UserController.GetUserAlbumId(user.UserName) > int.MinValue))
				{
					detailActivated += String.Format(@"<p><a href=""{0}"">{1}</a></p>",
																					 Util.GetUrl(PageId.album, "aid={0}", UserController.GetUserAlbumId(user.UserName)),
																					 Resources.GalleryServerPro.CreateAccount_User_Album_Link_Text);
				}

				DisplaySuccessMessage(title, detailActivated);
			}

			pnlCreateUser.Visible = false;
		}

		private UserEntity AddUser()
		{
			string newUserName = txtNewUserUserName.Text;
			string newUserPassword1 = txtNewUserPassword1.Text;
			string newUserPassword2 = txtNewUserPassword2.Text;

			if (newUserPassword1 != newUserPassword2)
				throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Passwords_Not_Matching_Error);

			string[] roles = Core.DefaultRolesForSelfRegisteredUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			return UserController.CreateUser(newUserName, newUserPassword1, txtNewUserEmail.Text, roles, true);
		}

		private void DisplayErrorMessage(string title, string detail)
		{
			DisplayMessage(title, detail, MessageStyle.Error);
		}

		private void DisplaySuccessMessage(string title, string detail)
		{
			DisplayMessage(title, detail, MessageStyle.Information);
		}

		private void DisplayMessage(string title, string detail, MessageStyle iconStyle)
		{
			pnlMsgContainer.Controls.Clear();
			GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)Page.LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
			msgBox.IconStyle = iconStyle;
			msgBox.MessageTitle = title;
			msgBox.MessageDetail = detail;
			msgBox.CssClass = "um3ContainerCss gsp_rounded10 gsp_floatcontainer";
			msgBox.HeaderCssClass = "um1HeaderCss";
			msgBox.DetailCssClass = "um1DetailCss";
			pnlMsgContainer.Controls.Add(msgBox);
			pnlMsgContainer.Visible = true;
		}

		/// <summary>
		/// Update the user account to indicate the e-mail address has been validated. If admin approval is required, send an e-mail
		/// to the administrators. If not required, activate the account. Display results to user.
		/// </summary>
		private void ValidateUser()
		{
			pnlCreateUser.Visible = false;

			try
			{
				string userName = HelperFunctions.Decrypt(Util.GetQueryStringParameterString("verify"));

				UserController.UserEmailValidatedAfterCreation(userName);

				string title = Resources.GalleryServerPro.CreateAccount_Verification_Success_Header_Text;

				string detail = GetEmailValidatedUserMessageDetail(userName);

				DisplaySuccessMessage(title, detail);
			}
			catch (Exception ex)
			{
				AppErrorController.LogError(ex);

				string failDetailText = String.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", Resources.GalleryServerPro.CreateAccount_Verification_Fail_Detail_Text);

				DisplayErrorMessage(Resources.GalleryServerPro.CreateAccount_Verification_Fail_Header_Text, failDetailText);
			}
		}

		/// <summary>
		/// Gets the message to display to the user after she validated the account by clicking on the link in the verification
		/// e-mail.
		/// </summary>
		/// <param name="userName">The username whose account has been validated.</param>
		/// <returns>Returns an HTML-formatted string to display to the user.</returns>
		private static string GetEmailValidatedUserMessageDetail(string userName) 
		{
			if (Config.GetCore().RequireApprovalForSelfRegisteredUser)
			{
				return string.Format(@"<p>{0}</p>", Resources.GalleryServerPro.CreateAccount_Verification_Success_Needs_Admin_Approval_Detail_Text);
			}

			string detail = string.Format(@"<p>{0}</p><p><a href=""{1}"">{2}</a></p>",
			                              Resources.GalleryServerPro.CreateAccount_Verification_Success_Detail_Text,
			                              Util.GetCurrentPageUrl(),
			                              Resources.GalleryServerPro.CreateAccount_Gallery_Link_Text);

			if (Config.GetCore().EnableUserAlbum && (UserController.GetUserAlbumId(userName) > int.MinValue))
			{
				detail += String.Format(@"<p><a href=""{0}"">{1}</a></p>",
				                        Util.GetUrl(PageId.album, "aid={0}", UserController.GetUserAlbumId(userName)),
				                        Resources.GalleryServerPro.CreateAccount_User_Album_Link_Text);
			}

			return detail;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Web.Profile;
using System.Web.Security;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using System.Web;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality related to user management.
	/// </summary>
	public static class UserController
	{
		#region Private Fields

		private static MembershipProvider _membershipProvider;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Membership provider used by Gallery Server Pro.
		/// </summary>
		/// <value>The Membership provider used by Gallery Server Pro.</value>
		internal static MembershipProvider MembershipGsp
		{
			get
			{
				if (_membershipProvider == null)
				{
					_membershipProvider = GetMembershipProvider();
				}

				return _membershipProvider;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured to require the user to answer a password 
		/// question for password reset and retrieval. 
		/// </summary>
		/// <value>
		/// 	<c>true</c> if a password answer is required for password reset and retrieval; otherwise, <c>false</c>. The default is true.
		/// </value>
		public static bool RequiresQuestionAndAnswer
		{
			get
			{
				return MembershipGsp.RequiresQuestionAndAnswer;
			}
		}

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to reset their passwords. 
		/// </summary>
		/// <value><c>true</c> if the membership provider supports password reset; otherwise, <c>false</c>. The default is true.</value>
		public static bool EnablePasswordReset
		{
			get
			{
				return MembershipGsp.EnablePasswordReset;
			}
		}

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to retrieve their passwords. 
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the membership provider is configured to support password retrieval; otherwise, <c>false</c>. The default is false.
		/// </value>
		public static bool EnablePasswordRetrieval
		{
			get
			{
				return MembershipGsp.EnablePasswordRetrieval;
			}
		}

		/// <summary>
		/// Gets the minimum length required for a password. 
		/// </summary>
		/// <value>The minimum length required for a password. </value>
		public static int MinRequiredPasswordLength
		{
			get
			{
				return MembershipGsp.MinRequiredPasswordLength;
			}
		}

		/// <summary>
		/// Gets the minimum number of non alphanumeric characters that must be present in a password. 
		/// </summary>
		/// <value>The minimum number of non alphanumeric characters that must be present in a password.</value>
		public static int MinRequiredNonAlphanumericCharacters
		{
			get
			{
				return MembershipGsp.MinRequiredNonAlphanumericCharacters;
			}
		}

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Gets a collection of all the users in the database.
		/// </summary>
		/// <returns>Returns a collection of all the users in the database.</returns>
		public static List<UserEntity> GetAllUsers()
		{
			List<UserEntity> users = new List<UserEntity>();

			int totalRecords;
			foreach (MembershipUser user in MembershipGsp.GetAllUsers(0, 0x7fffffff, out totalRecords))
			{
				users.Add(ToUserEntity(user));
			}

			return users;
		}

		/// <overloads>
		/// Gets information from the data source for a user.
		/// </overloads>
		/// <summary>
		/// Gets information from the data source for the current logged-on membership user.
		/// </summary>
		/// <returns>A <see cref="UserEntity"/> representing the current logged-on membership user.</returns>
		public static UserEntity GetUser()
		{
			return ToUserEntity(MembershipGsp.GetUser(Util.UserName, false));
		}

		/// <summary>
		/// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user. 
		/// </summary>
		/// <param name="userName">The name of the user to get information for.</param>
		/// <param name="userIsOnline"><c>true</c> to update the last-activity date/time stamp for the user; <c>false</c> to return user 
		/// information without updating the last-activity date/time stamp for the user.</param>
		/// <returns>A <see cref="UserEntity"/> object populated with the specified user's information from the data source.</returns>
		public static UserEntity GetUser(string userName, bool userIsOnline)
		{
			return ToUserEntity(MembershipGsp.GetUser(userName, userIsOnline));
		}

		/// <summary>
		/// Gets the password for the specified user name from the data source. 
		/// </summary>
		/// <param name="userName">The user to retrieve the password for. </param>
		/// <returns>The password for the specified user name.</returns>
		public static String GetPassword(string userName)
		{
			return MembershipGsp.GetPassword(userName, null);
		}

		/// <summary>
		/// Resets a user's password to a new, automatically generated password.
		/// </summary>
		/// <param name="userName">The user to reset the password for. </param>
		/// <returns>The new password for the specified user.</returns>
		public static String ResetPassword(string userName)
		{
			return MembershipGsp.ResetPassword(userName, null);
		}

		/// <summary>
		/// Processes a request to update the password for a membership user.
		/// </summary>
		/// <param name="userName">The user to update the password for.</param>
		/// <param name="oldPassword">The current password for the specified user.</param>
		/// <param name="newPassword">The new password for the specified user.</param>
		/// <returns><c>true</c> if the password was updated successfully; otherwise, <c>false</c>.</returns>
		public static bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			return MembershipGsp.ChangePassword(userName, oldPassword, newPassword);
		}

		/// <summary>
		/// Clears a lock so that the membership user can be validated.
		/// </summary>
		/// <param name="userName">The membership user whose lock status you want to clear.</param>
		/// <returns><c>true</c> if the membership user was successfully unlocked; otherwise, <c>false</c>.</returns>
		public static bool UnlockUser(string userName)
		{
			return MembershipGsp.UnlockUser(userName);
		}

		/// <summary>
		/// Adds a new user with the specified e-mail address to the data store.
		/// </summary>
		/// <param name="userName">The user name for the new user.</param>
		/// <param name="password">The password for the new user.</param>
		/// <param name="email">The email for the new user.</param>
		/// <returns>Returns a new user with the specified e-mail address to the data store.</returns>
		private static UserEntity CreateUser(string userName, string password, string email)
		{
			// This function is a re-implementation of the System.Web.Security.Membership.CreateUser method. We can't call it directly
			// because it uses the default provider, and we might be using a named provider.
			MembershipCreateStatus status;
			MembershipUser user = MembershipGsp.CreateUser(userName, password, email, null, null, true, null, out status);
			if (user == null)
			{
				throw new MembershipCreateUserException(status);
			}

			return ToUserEntity(user);
		}

		/// <summary>
		/// Updates information about a user in the data source.
		/// </summary>
		/// <param name="user">A <see cref="UserEntity"/> object that represents the user to update and the updated information for the user.</param>
		public static void UpdateUser(UserEntity user)
		{
			MembershipGsp.UpdateUser(ToMembershipUser(user));
		}

		/// <summary>
		/// Removes a user from the membership data source.
		/// </summary>
		/// <param name="userName">The name of the user to delete.</param>
		/// <returns><c>true</c> if the user was successfully deleted; otherwise, <c>false</c>.</returns>
		public static bool DeleteUser(string userName)
		{
			return MembershipGsp.DeleteUser(userName, true);
		}

		/// <summary>
		/// Contains functionality that must execute after a user has logged on. Specifically, roles are cleared from the cache
		/// and, if user albums are enabled, the user's personal album is validated. Developers integrating Gallery Server into
		/// their applications should call this method after they have authenticated a user. User must be logged on by the
		/// time this method is called. For example, one can call this method in the LoggedIn event of the ASP.NET Login control.
		/// </summary>
		/// <param name="userName">Name of the user that has logged on.</param>
		public static void UserLoggedOn(string userName)
		{
			// Store the user name and the fact that user is authenticated. Ideally we would not do this and just use
			// User.Identity.Name and User.Identity.IsAuthenticated, but those won't be assigned by ASP.NET until the 
			// next page load.
			Util.IsAuthenticated = true;
			Util.UserName = userName;

			RemoveRolesFromCache(userName);

			ValidateUserAlbum(userName);
		}

		/// <summary>
		/// Contains functionality that must execute after a user has logged off. Specifically, roles are cleared from the cache.
		/// Developers integrating Gallery Server into their applications should call this method after a user has signed out. 
		/// User must be already be logged off by the time this method is called. For example, one can call this method in the 
		/// LoggedOut event of the ASP.NET LoginStatus control.
		/// </summary>
		public static void UserLoggedOff()
		{
			string userName = Util.UserName;

			// Clear the user name and the fact that user is not authenticated. Ideally we would not do this and just use
			// User.Identity.Name and User.Identity.IsAuthenticated, but those won't be assigned by ASP.NET until the 
			// next page load.
			Util.IsAuthenticated = false;
			Util.UserName = String.Empty;

			RemoveRolesFromCache(userName);
		}

		/// <summary>
		/// Creates a new account in the membership system with the specified <paramref name="userName"/>, <paramref name="password"/>,
		/// <paramref name="email"/>, and belonging to the specified <paramref name="roles"/>. If required, it sends a verification 
		/// e-mail to the user, sends an e-mail notification to admins, and creates a user album. The account will be disabled when
		/// <paramref name="isSelfRegistration"/> is <c>true</c> and either the system option RequireEmailValidationForSelfRegisteredUser
		/// or RequireApprovalForSelfRegisteredUser is enabled.
		/// </summary>
		/// <param name="userName">Account name of the user. Cannot be null or empty.</param>
		/// <param name="password">The password for the user. Cannot be null or empty.</param>
		/// <param name="email">The email associated with the user. Required when <paramref name="isSelfRegistration"/> is true 
		/// and email verification is enabled.</param>
		/// <param name="roles">The names of the roles to assign to the user. The roles must already exist. If null or empty, no
		/// roles are assigned to the user.</param>
		/// <param name="isSelfRegistration">Indicates when the user is creating his or her own account. Set to false when an
		/// administrator creates an account.</param>
		/// <returns>Returns the newly created user.</returns>
		/// <exception cref="MembershipCreateUserException">Thrown when an error occurs during account creation. Check the StatusCode 
		/// property for a MembershipCreateStatus value.</exception>
		public static UserEntity CreateUser(string userName, string password, string email, string[] roles, bool isSelfRegistration)
		{
			#region Validation

			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("userName");

			if (String.IsNullOrEmpty(password))
				throw new ArgumentException("password");

			if ((String.IsNullOrEmpty(email)) && (HelperFunctions.IsValidEmail(userName)))
			{
				// No email address was specified, but the user name happens to be in the form of an email address,
				// so let's set the email property to the user name.
				email = userName;
			}

			#endregion

			// Step 1: Create the user. Any number of exceptions may occur; we'll let the caller deal with them.
			UserEntity user = CreateUser(userName, password, email);

			// Step 2: If this is a self-registered account and email verification is enabled or admin approval is required,
			// disable it. It will be approved when the user validates the email or the admin gives approval.
			if (isSelfRegistration)
			{
				if (Config.GetCore().RequireEmailValidationForSelfRegisteredUser || Config.GetCore().RequireApprovalForSelfRegisteredUser)
				{
					user.IsApproved = false;
					UpdateUser(user);
				}
			}

			// Step 3: Add user to roles.
			if ((roles != null) && (roles.Length > 0))
			{
				foreach (string role in roles)
				{
					RoleController.AddUserToRole(userName, role);
				}
			}

			// Step 4: Notify admins that an account was created.
			NotifyAdminsOfNewlyCreatedAccount(user, isSelfRegistration, false);

			// Step 5: Send user a welcome message or a verification link.
			if (HelperFunctions.IsValidEmail(user.Email))
			{
				NotifyUserOfNewlyCreatedAccount(user);
			}
			else if (isSelfRegistration && Config.GetCore().RequireEmailValidationForSelfRegisteredUser)
			{
				// Invalid email, but we need one to send the email verification. Throw error.
				throw new MembershipCreateUserException(MembershipCreateStatus.InvalidEmail);
			}

			HelperFunctions.PurgeCache();
			
			return user;
		}

		/// <summary>
		/// Delete the user from the membership system. In addition, remove the user from any roles. If a role is an ownership role,
		/// then delete it if the user is the only member. Remove the user from ownership of any albums, and delete the user's
		/// personal album, if user albums are enabled.
		/// </summary>
		/// <param name="userName">Name of the user to be deleted.</param>
		/// <param name="preventDeletingLoggedOnUser">If set to <c>true</c>, throw a <see cref="WebException"/> if attempting 
		/// to delete the currently logged on user.</param>
		/// <exception cref="WebException">Thrown when the user cannot be deleted because doing so violates one of the business rules.</exception>
		public static void DeleteGalleryServerProUser(string userName, bool preventDeletingLoggedOnUser)
		{
			if (String.IsNullOrEmpty(userName))
				return;

			ValidateDeleteUser(userName, preventDeletingLoggedOnUser, true);

			DeleteUserAlbum(userName);

			UpdateRolesAndOwnershipForDeletedUser(userName);

			DeleteUser(userName);

			HelperFunctions.PurgeCache();
		}

		/// <summary>
		/// Gets a <see cref="System.Data.DataTable"/> named Users with a single string column named UserName that contains the user 
		/// names of all the members as returned by GetAllUsers(). Data may be returned from cache.
		/// </summary>
		/// <returns>Returns a <see cref="System.Data.DataTable"/> containing the user names of all the current users.</returns>
		public static System.Data.DataTable GetUserNames()
		{
			System.Data.DataTable usersCache = (System.Data.DataTable)HelperFunctions.GetCache(CacheItem.UserNames);

			if (usersCache == null)
			{
				usersCache = new System.Data.DataTable("Users");
				usersCache.Columns.Add(new System.Data.DataColumn("UserName", typeof(String)));
				foreach (UserEntity user in GetAllUsers())
				{
					System.Data.DataRow dr = usersCache.NewRow();
					dr[0] = user.UserName;
					usersCache.Rows.Add(dr);
				}

				HelperFunctions.SetCache(CacheItem.UserNames, usersCache);
			}

			return usersCache;
		}

		/// <overloads>
		/// Gets the personal album for a user.
		/// </overloads>
		/// <summary>
		/// Gets the album for the current user's personal album (that is, get the album that was created when the
		/// user's account was created). The album is created if it does not exist. If user albums are disabled or the user
		/// has disabled their own album, this function returns null. It also returns null if the UserAlbumId property 
		/// is not found in the profile (this should not typically occur).
		/// </summary>
		/// <returns>Returns the album for the current user's personal album.</returns>
		public static IAlbum GetUserAlbum()
		{
			return GetUserAlbum(Util.UserName);
		}

		/// <summary>
		/// Gets the personal album for the specified <paramref name="userName"/> (that is, get the album that was created when the
		/// user's account was created). The album is created if it does not exist. If user albums are disabled or the user
		/// has disabled their own album, this function returns null. It also returns null if the UserAlbumId property
		/// is not found in the profile (this should not typically occur).
		/// </summary>
		/// <param name="userName">The account name for the user.</param>
		/// <returns>
		/// Returns the personal album for the specified <paramref name="userName"/>.
		/// </returns>
		public static IAlbum GetUserAlbum(string userName)
		{
			return ValidateUserAlbum(userName);
		}

		///// <summary>
		///// Gets the ID of the album for the current user's personal album (that is, this is the album that was created when the
		///// user's account was created). If user albums are disabled, this function returns int.MinValue. It also returns int.MinValue
		///// if the profile is null or the UserAlbumId property is not found  (this should not typically occur).
		///// </summary>
		///// <returns>Returns the ID of the album for the current user's personal album.</returns>
		//public static int GetUserAlbumId()
		//{
		//  int albumId = int.MinValue;

		//  if (!Config.Core.EnableUserAlbum)
		//    return albumId;

		//  if ((System.Web.HttpContext.Current.Profile != null) && (System.Web.HttpContext.Current.Profile[Constants.USER_ALBUM_ID_PROFILE_NAME] != null))
		//  {
		//    int tmpAlbumId = Convert.ToInt32(System.Web.HttpContext.Current.Profile[Constants.USER_ALBUM_ID_PROFILE_NAME], CultureInfo.InvariantCulture);

		//    albumId = (tmpAlbumId > 0 ? tmpAlbumId : albumId);
		//  }

		//  return albumId;
		//}

		/// <summary>
		/// Gets the ID of the album for the current user's personal album (that is, this is the album that was created when the
		/// user's account was created). If user albums are disabled or the UserAlbumId property is not found in the profile, 
		/// this function returns int.MinValue. This function executes faster than <see cref="GetUserAlbum"/> but it does not
		/// validate that the album exists.
		/// </summary>
		/// <returns>Returns the ID of the album for the current user's personal album.</returns>
		public static int GetUserAlbumId(string userName)
		{
			int albumId = Int32.MinValue;

			if (!Config.GetCore().EnableUserAlbum)
				return albumId;

			int tmpAlbumId = ProfileController.GetProfile(userName).UserAlbumId;
			albumId = (tmpAlbumId > 0 ? tmpAlbumId : albumId);

			return albumId;
		}

		/// <summary>
		/// Verifies the user album for the specified <paramref name="userName">user</paramref> exists and returns a reference
		/// to it. If it does not exist, or if the UserAlbumId property is not found in the profile, the profile and/or
		/// user album is created. This function returns null if user albums are disabled, if the user has disabled their own 
		/// album, or the userAlbumParentAlbumId in galleryserverpro.config does not match an existing album.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <returns>Returns a reference to the user album for the specified <paramref name="userName">user</paramref>, or null
		/// if user albums are disabled or the userAlbumParentAlbumId in galleryserverpro.config does not match an existing album.</returns>
		public static IAlbum ValidateUserAlbum(string userName)
		{
			if (!Config.GetCore().EnableUserAlbum)
				return null;

			if (!ProfileController.GetProfile(userName).EnableUserAlbum)
				return null;

			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("userName");

			int albumId = GetUserAlbumId(userName);

			IAlbum album;

			if (albumId > Int32.MinValue)
			{
				try
				{
					// Load the album. It will throw an InvalidAlbumException if it doesn't exist. If that happens, we'll create a new
					// user album.
					album = Factory.LoadAlbumInstance(albumId, false);
				}
				catch (InvalidAlbumException)
				{
					album = AlbumController.CreateUserAlbum(userName);
				}
			}
			else
			{
				// Profile doesn't store a valid album ID. Create user album. This will also create the profile entry.
				album = AlbumController.CreateUserAlbum(userName);
			}

			return album;
		}

		/// <summary>
		/// Delete all anonymous accounts older than today. This is typically called during application startup to clean up the 
		/// profiles data store.
		/// </summary>
		public static void DeleteAnonymousUsers()
		{
			HelperFunctions.BeginTransaction();

			try
			{
				foreach (ProfileInfo profile in ProfileManager.GetAllInactiveProfiles(ProfileAuthenticationOption.Anonymous, DateTime.Today))
				{
					DeleteUser(profile.UserName);
				}
				HelperFunctions.CommitTransaction();
			}
			catch
			{
				HelperFunctions.RollbackTransaction();
				throw;
			}

			// Don't use the following technique. It only deletes records in the profile table. Records in the users table are not deleted.
			//System.Web.Profile.ProfileManager.DeleteInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption.Anonymous, DateTime.Today);
		}

		/// <summary>
		/// Validate the integrity of the membership, roles, and profile configuration.
		/// </summary>
		public static void ValidateMembership()
		{
			ProcessInstallerFile();

			RoleController.ValidateRoles();
		}

		/// <summary>
		/// Activates the account for the specified <paramref name="userName"/> and automatically logs on the user. If the
		/// admin approval system setting is enabled (RequireApprovalForSelfRegisteredUser=<c>true</c>), then record the
		/// validation in the user's comment field but do not activate the account. Instead, send the administrator(s) an
		/// e-mail notifying them of a pending account. This method is typically called after a user clicks the confirmation
		/// link in the verification e-mail after creating a new account.
		/// </summary>
		/// <param name="userName">Name of the user who has just validated his or her e-mail address.</param>
		public static void UserEmailValidatedAfterCreation(string userName)
		{
			UserEntity user = GetUser(userName, true);

			if (Config.GetCore().RequireApprovalForSelfRegisteredUser)
			{
				NotifyAdminsOfNewlyCreatedAccount(user, true, true);
			}
			else
			{
				user.IsApproved = true;

				LogOffUser();
				LogOnUser(userName);
			}

			user.Comment = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.CreateAccount_Verification_Comment_Text, user.Email, DateTime.Now);

			UpdateUser(user);
		}

		/// <summary>
		/// Logs off the current user.
		/// </summary>
		public static void LogOffUser()
		{
			FormsAuthentication.SignOut();

			UserLoggedOff();
		}

		/// <summary>
		/// Logs on the specified <paramref name="userName"/>.
		/// </summary>
		/// <param name="userName">The username for the user to log on.</param>
		public static void LogOnUser(string userName)
		{
			FormsAuthentication.SetAuthCookie(userName, false);

			UserLoggedOn(userName);
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Gets the Membership provider used by Gallery Server Pro.
		/// </summary>
		/// <returns>The Membership provider used by Gallery Server Pro.</returns>
		private static MembershipProvider GetMembershipProvider()
		{
			if (String.IsNullOrEmpty(Config.GetCore().MembershipProviderName))
			{
				return Membership.Provider;
			}
			else
			{
				return Membership.Providers[Config.GetCore().MembershipProviderName];
			}
		}

		private static void RemoveRolesFromCache(string userName)
		{
			Dictionary<string, IGalleryServerRoleCollection> rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)HelperFunctions.GetCache(CacheItem.GalleryServerRoles);

			if ((rolesCache != null) && (HttpContext.Current.Session != null))
			{
				string key = String.Concat(HttpContext.Current.Session.SessionID, userName);

				rolesCache.Remove(key);
			}
		}

		/// <summary>
		/// Send an e-mail to the users that are subscribed to new account notifications. These are specified in the
		/// usersToNotifyWhenAccountIsCreated configuration setting. If RequireEmailValidationForSelfRegisteredUser
		/// is enabled, do not send an e-mail at this time. Instead, it is sent when the user clicks the confirmation
		/// link in the e-mail.
		/// </summary>
		/// <param name="user">An instance of <see cref="UserEntity"/> that represents the newly created account.</param>
		/// <param name="isSelfRegistration">Indicates when the user is creating his or her own account. Set to false when an
		/// administrator creates an account.</param>
		private static void NotifyAdminsOfNewlyCreatedAccount(UserEntity user, bool isSelfRegistration, bool isEmailVerified)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			if (isSelfRegistration && !isEmailVerified && Config.GetCore().RequireEmailValidationForSelfRegisteredUser)
			{
				return;
			}

			EmailTemplate emailTemplate;
			if (isSelfRegistration && Config.GetCore().RequireApprovalForSelfRegisteredUser)
			{
				emailTemplate = EmailController.GetEmailTemplate(EmailTemplateForm.AdminNotificationAccountCreatedRequiresApproval, user);
			}
			else
			{
				emailTemplate = EmailController.GetEmailTemplate(EmailTemplateForm.AdminNotificationAccountCreated, user);
			}

			foreach (string accountName in Config.GetCore().UsersToNotifyWhenAccountIsCreated.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				UserEntity account = GetUser(accountName, true);

				if (!String.IsNullOrEmpty(account.Email))
				{
					MailAddress admin = new MailAddress(account.Email, account.UserName);
					try
					{
						EmailController.SendEmail(admin, emailTemplate.Subject, emailTemplate.Body);
					}
					catch (WebException ex)
					{
						AppErrorController.LogError(ex);
					}
					catch (SmtpException ex)
					{
						AppErrorController.LogError(ex);
					}
				}
			}
		}

		/// <summary>
		/// Send an e-mail to the user associated with the new account. This will be a verification e-mail if e-mail verification
		/// is enabled; otherwise it is a welcome message. The calling method should ensure that the <paramref name="user"/>
		/// has a valid e-mail configured before invoking this function.
		/// </summary>
		/// <param name="user">An instance of <see cref="UserEntity"/> that represents the newly created account.</param>
		private static void NotifyUserOfNewlyCreatedAccount(UserEntity user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			bool enableEmailVerification = Config.GetCore().RequireEmailValidationForSelfRegisteredUser;
			bool requireAdminApproval = Config.GetCore().RequireApprovalForSelfRegisteredUser;

			if (enableEmailVerification)
			{
				EmailController.SendNotificationEmail(user, EmailTemplateForm.UserNotificationAccountCreatedNeedsVerification);
			}
			else if (requireAdminApproval)
			{
				EmailController.SendNotificationEmail(user, EmailTemplateForm.UserNotificationAccountCreatedNeedsApproval);
			}
			else
			{
				EmailController.SendNotificationEmail(user, EmailTemplateForm.UserNotificationAccountCreated);
			}
		}

		/// <summary>
		/// Throws an exception if the user cannot be deleted, such as when trying to delete his or her own account, or when deleting
		/// the only account with admin permission.
		/// </summary>
		/// <param name="userName">Name of the user to delete.</param>
		/// <param name="preventDeletingLoggedOnUser">If set to <c>true</c>, throw a <see cref="WebException"/> if attempting
		/// to delete the currently logged on user.</param>
		/// <param name="preventDeletingLastAdminAccount">If set to <c>true</c> throw a <see cref="WebException"/> if attempting
		/// to delete the last user with AllowSiteAdminister permission. When false, do not perform this check. It does not matter
		/// whether the user to delete is actually an administrator.</param>
		/// <exception cref="WebException">Thrown when the user cannot be deleted because doing so violates one of the business rules.</exception>
		private static void ValidateDeleteUser(string userName, bool preventDeletingLoggedOnUser, bool preventDeletingLastAdminAccount)
		{
			if (preventDeletingLoggedOnUser)
			{
				// Don't let user delete their own account.
				if (userName.Equals(Util.UserName, StringComparison.OrdinalIgnoreCase))
				{
					throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Delete_User_Msg);
				}
			}

			if (preventDeletingLastAdminAccount)
			{
				foreach (IGalleryServerRole role in RoleController.GetGalleryServerRoles())
				{
					if (!role.AllowAdministerSite)
						continue;

					bool atLeastOneOtherAdminExists = false;
					foreach (string userInAdminRole in RoleController.GetUsersInRole(role.RoleName))
					{
						if (userInAdminRole != userName)
						{
							atLeastOneOtherAdminExists = true;
							break;
						}
					}

					if (!atLeastOneOtherAdminExists)
					{
						throw new WebException("You are attempting to delete the only user with Administer site permission. If you want to delete this account, first assign another account to a role with Administer site permission.");
					}
				}
				// Make sure at least one other user has admin permission.
				//bool atLeastOneOtherAdminExists = false;
				//foreach (MembershipUser user in Membership.GetAllUsers())
				//{
				//  if (user.UserName == userName)
				//    continue;

				//  foreach (string role in RoleController.Role.GetRolesForUser(user.UserName))
				//  {
				//    IGalleryServerRole galleryRole = Factory.LoadGalleryServerRole(role);
				//    if (galleryRole.AllowAdministerSite)
				//    {
				//      atLeastOneOtherAdminExists = true;
				//      break;
				//    }
				//  }
				//  if (atLeastOneOtherAdminExists)
				//    break;
				//}

				//if (!atLeastOneOtherAdminExists)
				//{
				//  throw new Exception("You are attempting to delete the only user with Administer site permission. If you want to delete this account, first assign another account to a role with Administer site permission.");
				//}

			}
		}

		/// <summary>
		/// In certain cases, the web-based installer creates a text file in the App Data directory that is meant as a signal to this
		/// code that additional setup steps are required. If this file is found, carry out the additional actions. This file is
		/// created in the SetFlagForMembershipConfiguration() method of pages\install.ascx.cs.
		/// </summary>
		private static void ProcessInstallerFile()
		{
			string filePath = Path.Combine(AppSetting.Instance.PhysicalApplicationPath, Path.Combine(GlobalConstants.AppDataDirectory, GlobalConstants.InstallerFileName));

			if (!File.Exists(filePath))
				return;

			string adminUserName;
			string adminPwd;
			string adminEmail;
			using (StreamReader sw = File.OpenText(filePath))
			{
				adminUserName = sw.ReadLine();
				adminPwd = sw.ReadLine();
				adminEmail = sw.ReadLine();
			}

			HelperFunctions.BeginTransaction();

			#region Create the Sys Admin role.

			// Create the Sys Admin role. If it already exists, make sure it has AllowAdministerSite permission.
			string sysAdminRoleName = Resources.GalleryServerPro.Installer_Sys_Admin_Role_Name;
			if (!RoleController.RoleExists(sysAdminRoleName))
				RoleController.CreateRole(sysAdminRoleName);

			IGalleryServerRole role = Factory.LoadGalleryServerRole(sysAdminRoleName);
			if (role == null)
			{
				role = Factory.CreateGalleryServerRoleInstance(sysAdminRoleName, true, true, true, true, true, true, true, true, true, true, false);
				role.RootAlbumIds.Add(Factory.LoadRootAlbumInstance().Id);
				role.Save();
			}
			else
			{
				// Role already exists. Make sure it has Sys Admin permission.
				if (!role.AllowAdministerSite)
				{
					role.AllowAdministerSite = true;
					role.Save();
				}
			}

			#endregion

			#region Create the Sys Admin user account.

			// Create the Sys Admin user account. Will throw an exception if the name is already in use.
			try
			{
				CreateUser(adminUserName, adminPwd, adminEmail);
			}
			catch (MembershipCreateUserException ex)
			{
				if (ex.StatusCode == MembershipCreateStatus.DuplicateUserName)
				{
					// The user already exists. Update the password and email address to our values.
					UserEntity user = GetUser(adminUserName, true);
					ChangePassword(user.UserName, GetPassword(user.UserName), adminPwd);
					user.Email = adminEmail;
					UpdateUser(user);
				}
			}

			// Add the Sys Admin user to the Sys Admin role.
			if (!RoleController.IsUserInRole(adminUserName, sysAdminRoleName))
				RoleController.AddUserToRole(adminUserName, sysAdminRoleName);

			#endregion

			#region Create sample album and image

			if (!Config.GetCore().MediaObjectPathIsReadOnly)
			{
				DateTime currentTimestamp = DateTime.Now;
				IAlbum sampleAlbum = null;
				foreach (IAlbum album in Factory.LoadRootAlbumInstance().GetChildGalleryObjects(GalleryObjectType.Album))
				{
					if (album.DirectoryName == "Samples")
					{
						sampleAlbum = album;
						break;
					}
				}
				if (sampleAlbum == null)
				{
					// Create sample album.
					sampleAlbum = Factory.CreateAlbumInstance();
					sampleAlbum.Parent = Factory.LoadRootAlbumInstance();
					sampleAlbum.Title = "Samples";
					sampleAlbum.DirectoryName = "Samples";
					sampleAlbum.Summary = "Welcome to Gallery Server Pro!";
					sampleAlbum.CreatedByUserName = adminUserName;
					sampleAlbum.DateAdded = currentTimestamp;
					sampleAlbum.LastModifiedByUserName = adminUserName;
					sampleAlbum.DateLastModified = currentTimestamp;
					sampleAlbum.Save();
				}

				// Look for sample image in sample album.
				IGalleryObject sampleImage = null;
				foreach (IGalleryObject image in sampleAlbum.GetChildGalleryObjects(GalleryObjectType.Image))
				{
					if (image.Original.FileName == Constants.SAMPLE_IMAGE_FILENAME)
					{
						sampleImage = image;
						break;
					}
				}

				if (sampleImage == null)
				{
					// Sample image not found. Pull image from assembly, save to disk, and create a media object from it.
					string sampleDirPath = Path.Combine(AppSetting.Instance.MediaObjectPhysicalPath, sampleAlbum.DirectoryName);
					string sampleImageFilename = HelperFunctions.ValidateFileName(sampleDirPath, Constants.SAMPLE_IMAGE_FILENAME);
					string sampleImageFilepath = Path.Combine(sampleDirPath, sampleImageFilename);

					System.Reflection.Assembly asm = Assembly.GetExecutingAssembly();
					using (Stream stream = asm.GetManifestResourceStream(String.Concat("GalleryServerPro.Web.gs.images.", Constants.SAMPLE_IMAGE_FILENAME)))
					{
						if (stream != null)
						{
							BinaryWriter bw = new BinaryWriter(File.Create(sampleImageFilepath));
							byte[] buffer = new byte[stream.Length];
							stream.Read(buffer, 0, (int)stream.Length);
							bw.Write(buffer);
							bw.Flush();
							bw.Close();
						}
					}

					if (File.Exists(sampleImageFilepath))
					{
						IGalleryObject image = Factory.CreateImageInstance(new FileInfo(sampleImageFilepath), sampleAlbum);
						image.Title = "Margaret, Skyler and Roger Martin (December 2008)";
						image.CreatedByUserName = adminUserName;
						image.DateAdded = currentTimestamp;
						image.LastModifiedByUserName = adminUserName;
						image.DateLastModified = currentTimestamp;
						image.Save();
					}
				}
			}

			#endregion

			HelperFunctions.CommitTransaction();
			HelperFunctions.PurgeCache();

			File.Delete(filePath);
		}

		private static void DeleteUserAlbum(string userName)
		{
			IAlbum album = GetUserAlbum(userName);

			if (album != null)
				AlbumController.DeleteAlbum(album);
		}

		/// <summary>
		/// Remove the user from any roles. If a role is an ownership role, then delete it if the user is the only member.
		/// Remove the user from ownership of any albums.
		/// </summary>
		/// <param name="userName">Name of the user to be deleted.</param>
		/// <remarks>The user will be specified as an owner only for those albums that belong in ownership roles, so 
		/// to find all albums the user owns, we need only to loop through the user's roles and inspect the ones
		/// where the names begin with the album owner role name prefix variable.</remarks>
		private static void UpdateRolesAndOwnershipForDeletedUser(string userName)
		{
			List<string> rolesToDelete = new List<string>();

			string[] userRoles = RoleController.GetRolesForUser(userName);
			foreach (string roleName in userRoles)
			{
				if (RoleController.IsRoleAnAlbumOwnerRole(roleName))
				{
					// This is a role that was automatically created to provide ownership permission to an album. Check each
					// album and empty out the OwnerUserName field if the listed owner matches our user.
					IGalleryServerRole role = Factory.LoadGalleryServerRole(roleName);
					foreach (int albumId in role.RootAlbumIds)
					{
						IAlbum album = Factory.LoadAlbumInstance(albumId, false);
						if (album.OwnerUserName == userName)
						{
							album.OwnerUserName = String.Empty;
							GalleryObjectController.SaveGalleryObject(album);
						}
					}

					if (RoleController.GetUsersInRole(roleName).Length <= 1)
					{
						// The user we are deleting is the only user in the owner role. Mark for deletion.
						rolesToDelete.Add(roleName);
					}
				}

			}

			if (userRoles.Length > 0)
			{
				foreach (string role in userRoles)
				{
					RoleController.RemoveUserFromRole(userName, role);
				}
			}

			foreach (string roleName in rolesToDelete)
			{
				RoleController.DeleteGalleryServerProRole(roleName);
			}
		}

		private static UserEntity ToUserEntity(MembershipUser u)
		{
			if (u == null)
				return null;

			return new UserEntity(u.Comment, u.CreationDate, u.Email, u.IsApproved, u.IsLockedOut, u.IsOnline, u.LastActivityDate, u.LastLockoutDate, u.LastLoginDate, u.LastPasswordChangedDate, u.PasswordQuestion, u.ProviderName, u.ProviderUserKey, u.UserName);
		}

		private static MembershipUser ToMembershipUser(UserEntity u)
		{
			if (String.IsNullOrEmpty(u.UserName))
			{
				throw new ArgumentException("UserEntity.UserName cannot be empty.");
			}

			MembershipUser user = MembershipGsp.GetUser(u.UserName, false);

			user.Comment = u.Comment;
			user.Email = u.Email;
			user.IsApproved = u.IsApproved;
			user.LastActivityDate = u.LastActivityDate;
			user.LastLoginDate = u.LastLoginDate;

			return user;
		}

		#endregion

		public static string GetAddUserErrorMessage(MembershipCreateStatus status)
		{
			switch (status)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_DuplicateUserName;

				case MembershipCreateStatus.DuplicateEmail:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_DuplicateEmail;

				case MembershipCreateStatus.InvalidPassword:
					return String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_InvalidPassword, MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);

				case MembershipCreateStatus.InvalidEmail:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_InvalidEmail;

				case MembershipCreateStatus.InvalidAnswer:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_InvalidAnswer;

				case MembershipCreateStatus.InvalidQuestion:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_InvalidQuestion;

				case MembershipCreateStatus.InvalidUserName:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_InvalidUserName;

				case MembershipCreateStatus.ProviderError:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_ProviderError;

				case MembershipCreateStatus.UserRejected:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_UserRejected;

				default:
					return Resources.GalleryServerPro.Admin_Manage_Users_Create_User_Error_Generic;
			}
		}
	}
}

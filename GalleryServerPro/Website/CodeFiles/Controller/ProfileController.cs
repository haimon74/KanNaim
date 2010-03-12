using System;
using System.Web;
using System.Web.Profile;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality related to managing the user profile.
	/// </summary>
	public static class ProfileController
	{
		#region Private Fields

		//private static readonly ProfileProvider _profileProvider = GetProfileProvider();

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Profile provider used by Gallery Server Pro.
		/// </summary>
		/// <value>The Profile provider used by Gallery Server Pro.</value>
		//public static ProfileProvider ProfileGsp
		//{
		//  get
		//  {
		//    return _profileProvider;
		//  }
		//}

		#endregion

		#region Public Methods

		/// <overloads>
		/// Gets a user's profile settings.
		/// </overloads>
		/// <summary>
		/// Gets the profile for the currently logged on user.
		/// </summary>
		/// <returns>Gets the profile for the currently logged on user.</returns>
		public static ProfileEntity GetProfile()
		{
			string userName = (Util.IsAuthenticated ? Util.UserName : HttpContext.Current.Request.AnonymousID);
			return GetProfile(userName);
		}

		/// <summary>
		/// Gets the profile for the specified user.
		/// </summary>
		/// <param name="userName">The account name for the user whose profile settings are to be retrieved.</param>
		/// <returns>Gets the profile for the specified user.</returns>
		public static ProfileEntity GetProfile(string userName)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentNullException("userName");

			ProfileBase p = ProfileBase.Create(userName, Util.IsAuthenticated);

			ProfileEntity pe = new ProfileEntity();
			pe.UserName = userName;

			bool showMediaObjectMetadata;
			if (Boolean.TryParse(p.GetPropertyValue(Constants.SHOW_METADATA_PROFILE_NAME).ToString(), out showMediaObjectMetadata))
			{
				pe.ShowMediaObjectMetadata = showMediaObjectMetadata;
			}

			int userAlbumId;
			if (Int32.TryParse(p.GetPropertyValue(Constants.USER_ALBUM_ID_PROFILE_NAME).ToString(), out userAlbumId))
			{
				pe.UserAlbumId = userAlbumId;
			}

			bool enableUserAlbum;
			if (Boolean.TryParse(p.GetPropertyValue(Constants.ENABLE_USER_ALBUM_PROFILE_NAME).ToString(), out enableUserAlbum))
			{
				pe.EnableUserAlbum = enableUserAlbum;
			}

			return pe;
		}

		/// <summary>
		/// Saves the profile to the data store for the currently logged on user.
		/// </summary>
		/// <param name="userProfile">The user profile to save to the data store.</param>
		public static void SaveProfile(ProfileEntity userProfile)
		{
			if (userProfile == null)
				throw new ArgumentNullException("userProfile");

			if (String.IsNullOrEmpty(userProfile.UserName))
				throw new ArgumentNullException("userProfile.UserName", "The UserName property of the userProfile parameter is null or empty.");

			ProfileBase p = ProfileBase.Create(userProfile.UserName, Util.IsAuthenticated);

			// All users - authenticated and anonymous - have the show metadata profile property
			p.SetPropertyValue(Constants.SHOW_METADATA_PROFILE_NAME, userProfile.ShowMediaObjectMetadata.ToString());

			if (!p.IsAnonymous)
			{
				// Only save these for logged-on users.
				p.SetPropertyValue(Constants.USER_ALBUM_ID_PROFILE_NAME, userProfile.UserAlbumId);

				p.SetPropertyValue(Constants.ENABLE_USER_ALBUM_PROFILE_NAME, userProfile.EnableUserAlbum.ToString());
			}
			
			p.Save();
		}

		#endregion

		#region Private Functions

		///// <summary>
		///// Gets the Profile provider for the current application.
		///// </summary>
		///// <returns>The Profile provider for the current application</returns>
		//private static ProfileProvider GetProfileProvider()
		//{
		//  if (String.IsNullOrEmpty(Config.GetCore().ProfileProviderName))
		//  {
		//    return ProfileManager.Provider;
		//  }
		//  else
		//  {
		//    return ProfileManager.Providers[Config.GetCore().ProfileProviderName];
		//  }
		//}

		#endregion

	}
}

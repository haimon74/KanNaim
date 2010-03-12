using System;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using System.Globalization;
using System.Collections.Generic;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Contains security-related functionality.
	/// </summary>
	public static class SecurityManager
	{
		#region Public Static Methods

		/// <summary>
		/// Throws a <see cref="GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException" /> if the user does not have the appropriate 
		/// permission for the specified album.
		/// </summary>
		/// <param name="securityRequest">The requested permissions.</param>
		/// <param name="roles">A collection of Gallery Server roles to which the currently logged-on user belongs. This parameter is ignored
		/// for anonymous users (i.e. <paramref name="isAuthenticated"/>=false). The parameter may be null.</param>
		/// <param name="albumId">The album for which the requested permission applies.</param>
		/// <param name="isAuthenticated">A value indicating whether the current user is logged in. If true, the
		/// <paramref name="roles"/> parameter must be given the names of the roles for the current user. If 
		/// <paramref name="isAuthenticated"/>=true and the <paramref name="roles"/> parameter
		/// is either null or an empty collection, this method thows a <see cref="GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException" /> exception.</param>
		/// <param name="isPrivateAlbum">A value indicating whether the album is hidden from anonymous users. This parameter is ignored for
		/// logged-on users.</param>
		/// <remarks>This method handles both anonymous and logged on users. Note that when <paramref name="isAuthenticated"/>=true, the <paramref name="isPrivateAlbum"/> parameter is
		/// ignored. When it is false, the <paramref name="roles"/> parameter is ignored.</remarks>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException">Throw when the user does not have 
		/// permission to the requested album.</exception>
		public static void ThrowIfUserNotAuthorized(SecurityActions securityRequest, IGalleryServerRoleCollection roles, int albumId, bool isAuthenticated, bool isPrivateAlbum)
		{
			if (!(IsUserAuthorized(securityRequest, roles, albumId, isAuthenticated, isPrivateAlbum)))
			{
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException(String.Format(CultureInfo.CurrentCulture, "You do not have permission '{0}' for album ID {1}.", securityRequest.ToString(), albumId));
			}
		}

		/// <summary>
		/// Determine if the user has the appropriate permission for the specified album. The user may be anonymous or logged on. 
		/// When the the user is logged on (i.e. <paramref name="isAuthenticated"/> = true), this method determines whether the user is authorized by 
		/// validating that at least one role has the requested permission to the specified album. When the user is anonymous, 
		/// the <paramref name="roles"/> parameter is ignored and instead the <paramref name="isPrivateAlbum"/> parameter is used. 
		/// Anonymous users do not have any access to private albums. When the the user is logged on (i.e. <paramref name="isAuthenticated"/> = true),
		/// the <paramref name="roles"/> parameter must contain the roles belonging to the user.
		/// </summary>
		/// <param name="securityRequests">The requested permissions.</param>
		/// <param name="roles">A collection of Gallery Server roles to which the currently logged-on user belongs. This parameter is ignored
		/// for anonymous users (i.e. <paramref name="isAuthenticated"/>=false). The parameter may be null.</param>
		/// <param name="albumId">The album for which the requested permission applies.</param>
		/// <param name="isAuthenticated">A value indicating whether the current user is logged on. If true, the
		/// <paramref name="roles"/> parameter should contain the names of the roles for the current user. If <paramref name="isAuthenticated"/>=true 
		/// and the <paramref name="roles"/> parameter is either null or an empty collection, this method returns false.</param>
		/// <param name="isPrivateAlbum">A value indicating whether the album is hidden from anonymous users. This parameter is ignored for
		/// logged-on users.</param>
		/// <returns>Returns true if the user has the requested permission; returns false if not.</returns>
		/// <remarks>This method handles both anonymous and logged on users. Note that when <paramref name="isAuthenticated"/>=true, the 
		/// <paramref name="isPrivateAlbum"/> parameter is ignored. When it is false, the roles parameter is ignored.</remarks>
		public static bool IsUserAuthorized(SecurityActions securityRequests, IGalleryServerRoleCollection roles, int albumId, bool isAuthenticated, bool isPrivateAlbum)
		{
			#region Validation

			if (isAuthenticated && ((roles == null) || (roles.Count == 0)))
				return false;

			#endregion

			// Handle anonymous users.
			if (!isAuthenticated)
			{
				return IsAnonymousUserAuthorized(securityRequests, isPrivateAlbum);
			}

			// If we get here we are dealing with an authenticated (logged on) user. Authorization for authenticated users is
			// given if the user is a member of at least one role that provides permission.
			if (SecurityActionEnumHelper.IsSingleSecurityAction(securityRequests))
			{
				// Iterate through each GalleryServerRole. If at least one allows the action, return true. Note that the
				// AdministerSite security action, if granted, applies to all albums and allows all actions (except HideWatermark).
				foreach (IGalleryServerRole role in roles)
				{
					if (IsAuthenticatedUserAuthorized(securityRequests, role, albumId))
						return true;
				}
				return false;
			}
			else
			{
				// There are multiple security actions in securityRequest enum. User is authorized only if EVERY security request
				// is authorized.
				foreach (SecurityActions securityAction in SecurityActionEnumHelper.ParseSecurityAction(securityRequests))
				{
					// Iterate through each GalleryServerRole. If at least role allows the action, return true. Note that the
					// AdministerSite security action, if granted, applies to all albums and allows all actions (except HideWatermark).
					bool securityActionIsAuthorized = false;
					foreach (IGalleryServerRole role in roles)
					{
						if (IsAuthenticatedUserAuthorized(securityAction, role, albumId))
						{
							securityActionIsAuthorized = true;
							break;
						}
					}
					if (!securityActionIsAuthorized)
						return false;
				}
				// If we get to this point then every security request has been authorized, so we can authorize the entire request.
				return true;
			}
		}

		#endregion

		#region Private Static Methods

		private static bool IsAnonymousUserAuthorized(SecurityActions securityAction, bool isPrivateAlbum)
		{
			// Anonymous user. Return true for viewing-related permission requests on PUBLIC albums; return false for all others.
			if (SecurityActionEnumHelper.IsSingleSecurityAction(securityAction))
			{
				if ((securityAction == SecurityActions.ViewAlbumOrMediaObject) && !isPrivateAlbum)
					return true;

				if ((securityAction == SecurityActions.ViewOriginalImage) && !isPrivateAlbum && ConfigManager.GetGalleryServerProConfigSection().Core.AllowAnonymousHiResViewing)
					return true;
			}
			else
			{
				// There are multiple security actions in securityAction enum. User is authorized only if EVERY security request
				// is authorized.
				bool isAuthorized = true;
				foreach (SecurityActions securityRequest in SecurityActionEnumHelper.ParseSecurityAction(securityAction))
				{
					bool securityActionIsAuthorized = false;

					if ((securityRequest == SecurityActions.ViewAlbumOrMediaObject) && !isPrivateAlbum)
						securityActionIsAuthorized = true;

					if ((securityRequest == SecurityActions.ViewOriginalImage) && !isPrivateAlbum && ConfigManager.GetGalleryServerProConfigSection().Core.AllowAnonymousHiResViewing)
						securityActionIsAuthorized = true;

					if (!securityActionIsAuthorized)
					{
						isAuthorized = false;
						break;
					}
				}
				return isAuthorized;
			}

			return false;
		}

		private static bool IsAuthenticatedUserAuthorized(SecurityActions securityRequest, IGalleryServerRole role, int albumId)
		{
			if ((role.AllowAdministerSite) && (securityRequest != SecurityActions.HideWatermark))
			{
				// Administer permissions imply permissions to carry out all other actions, except for hide watermark, which is more of 
				// a preference assigned to the user.
				return true;
			}

			switch (securityRequest)
			{
				case SecurityActions.AdministerSite: if (role.AllowAdministerSite) return true; break;
				case SecurityActions.ViewAlbumOrMediaObject: if (role.AllowViewAlbumOrMediaObject && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.ViewOriginalImage: if (role.AllowViewOriginalImage && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.AddChildAlbum: if (role.AllowAddChildAlbum && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.AddMediaObject: if (role.AllowAddMediaObject && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.DeleteAlbum:
					{
						// It is OK to delete the album if the AllowDeleteChildAlbum permission is true and one of the following is true:
						// 1. The album is the root album and its ID is in the list of targeted albums (Note that we never actually delete the root album.
						//    Instead, we delete all objects within the album. But the idea of deleting the top level album to clear out all objects in the
						//    gallery is useful to the user.)
						// 2. The album is not the root album and its parent album's ID is in the list of targeted albums.
						if (role.AllowDeleteChildAlbum)
						{
							IAlbum album = Factory.LoadAlbumInstance(albumId, false);
							if (album.IsRootAlbum)
							{
								if (role.AllAlbumIds.Contains(album.Id)) return true; break;
							}
							else
							{
								if (role.AllAlbumIds.Contains(album.Parent.Id)) return true; break;
							}
						}
						break;
					}
				case SecurityActions.DeleteChildAlbum: if (role.AllowDeleteChildAlbum && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.DeleteMediaObject: if (role.AllowDeleteMediaObject && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.EditAlbum: if (role.AllowEditAlbum && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.EditMediaObject: if (role.AllowEditMediaObject && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.HideWatermark: if (role.HideWatermark && role.AllAlbumIds.Contains(albumId)) return true; break;
				case SecurityActions.Synchronize: if (role.AllowSynchronize && role.AllAlbumIds.Contains(albumId)) return true; break;
				default: throw new GalleryServerPro.ErrorHandler.CustomExceptions.BusinessException(string.Format(CultureInfo.CurrentCulture, "The IsUserAuthorized function is not designed to handle the {0} SecurityActions. It must be updated by a developer.", securityRequest.ToString()));
			}
			return false;
		}

		#endregion
	}
}

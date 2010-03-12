using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for managing roles.
	/// </summary>
	public static class RoleController
	{
		#region Private Fields

		private static RoleProvider _roleProvider;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the role provider used by Gallery Server Pro.
		/// </summary>
		/// <value>The role provider used by Gallery Server Pro.</value>
		internal static RoleProvider RoleGsp
		{
			get
			{
				if (_roleProvider == null)
				{
					_roleProvider = GetRoleProvider();
				}

				return _roleProvider;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Add the specified user to the specified role.
		/// </summary>
		/// <param name="userName">The user name to add to the specified role.</param>
		/// <param name="roleName">The role to add the specified user name to.</param>
		public static void AddUserToRole(string userName, string roleName)
		{
			RoleGsp.AddUsersToRoles(new string[] { userName }, new string[] { roleName } );	
		}

		/// <summary>
		/// Removes the specified user from the specified role.
		/// </summary>
		/// <param name="userName">The user to remove from the specified role.</param>
		/// <param name="roleName">The role to remove the specified user from.</param>
		public static void RemoveUserFromRole(string userName, string roleName)
		{
			RoleGsp.RemoveUsersFromRoles(new string[] { userName }, new string[] { roleName });	
		}

		/// <summary>
		/// Gets a list of all the roles for the current application.
		/// </summary>
		/// <returns>A list of all the roles for the current application.</returns>
		public static string[] GetAllRoles()
		{
			return RoleGsp.GetAllRoles();
		}

		/// <summary>
		/// Gets a list of the roles that a specified user is in for the current application.
		/// </summary>
		/// <param name="userName">The user name.</param>
		/// <returns>A list of the roles that a specified user is in for the current application.</returns>
		public static string[] GetRolesForUser(string userName)
		{
			return RoleGsp.GetRolesForUser(userName);
		}

		/// <summary>
		/// Gets a list of users in the specified role for the current application.
		/// </summary>
		/// <param name="roleName">The name of the role.</param>
		/// <returns>A list of users in the specified role for the current application.</returns>
		public static string[] GetUsersInRole(string roleName)
		{
			return RoleGsp.GetUsersInRole(roleName);
		}

		/// <summary>
		/// Adds a role to the data source for the current application.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		public static void CreateRole(string roleName)
		{
			RoleGsp.CreateRole(roleName);
		}

		/// <summary>
		/// Removes a role from the data source for the current application.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		private static void DeleteRole(string roleName)
		{
			RoleGsp.DeleteRole(roleName, false);
		}

		/// <summary>
		/// Gets a value indicating whether the specified role name already exists in the data source for the current application.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		/// <returns><c>true</c> if the role exists; otherwise <c>false</c>.</returns>
		public static bool RoleExists(string roleName)
		{
			return RoleGsp.RoleExists(roleName);
		}

		/// <summary>
		/// Gets a value indicating whether the specified user is in the specified role for the current application.
		/// </summary>
		/// <param name="userName">The user name to search for.</param>
		/// <param name="roleName">The role to search in.</param>
		/// <returns>
		/// 	<c>true</c> if the specified user is in the specified role for the configured applicationName; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsUserInRole(string userName, string roleName)
		{
			return RoleGsp.IsUserInRole(userName, roleName);
		}

		/// <overloads>Retrieve a collection of Gallery Server roles.  The roles may be returned from a cache.</overloads>
		/// <summary>
		/// Retrieve all roles for the current gallery. Album owner roles are included in the collection. This is the same
		/// as calling GetGalleryServerRoles with the includeOwnerRoles parameter set to <c>true</c>.  The roles may be 
		/// returned from a cache.
		/// </summary>
		/// <returns>Returns all roles for the current gallery.</returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryServerRoleCollection GetGalleryServerRoles()
		{
			return GetGalleryServerRoles(true);
		}

		/// <summary>
		/// Retrieve Gallery Server roles for the current gallery, optionally excluding roles that were programmatically
		/// created to assist with the album ownership and user album functions. Excluding the owner roles may be useful
		/// in reducing the clutter when an administrator is viewing the list of roles, as it hides those not specifically created
		/// by the administrator. The roles may be returned from a cache.
		/// </summary>
		/// <param name="includeOwnerRoles">If set to <c>true</c> include all roles that serve as an album owner role.
		/// When <c>false</c>, exclude owner roles from the result set.</param>
		/// <returns>
		/// Returns the Gallery Server roles for the current gallery, optionally excluding owner roles.
		/// </returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryServerRoleCollection GetGalleryServerRoles(bool includeOwnerRoles)
		{
			if (includeOwnerRoles)
			{
				return Factory.LoadGalleryServerRoles();
			}			
			else
			{
				IGalleryServerRoleCollection roles = new GalleryServerRoleCollection();

				foreach (IGalleryServerRole role in Factory.LoadGalleryServerRoles())
				{
					if (!IsRoleAnAlbumOwnerRole(role.RoleName))
					{
						roles.Add(role);
					}
				}

				return roles;
			}
		}

		/// <summary>
		/// Gets Gallery Server roles representing the roles for the currently logged-on user. Returns an 
		/// empty collection if no user is logged in or the user is logged in but not assigned to any roles (Count = 0).
		/// </summary>
		/// <returns>Returns a collection of Gallery Server roles representing the roles for the currently logged-on user. 
		/// Returns an empty collection if no user is logged in or the user is logged in but not assigned to any roles (Count = 0).</returns>
		public static IGalleryServerRoleCollection GetGalleryServerRolesForUser()
		{
			if (!Util.IsAuthenticated)
				return new GalleryServerRoleCollection();

			// Get cached dictionary entry matching logged on user. If not found, retrieve from business layer and add to cache.
			Dictionary<string, IGalleryServerRoleCollection> rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)HelperFunctions.GetCache(CacheItem.GalleryServerRoles);

			IGalleryServerRoleCollection roles;

			if (HttpContext.Current.Session != null)
			{
				string key = String.Concat(HttpContext.Current.Session.SessionID, Util.UserName);
				if ((rolesCache != null) && (rolesCache.TryGetValue(key, out roles)))
				{
					return roles;
				}
			}

			// No roles in the cache, so get from business layer and add to cache.
			roles = Factory.LoadGalleryServerRoles(GetRolesForUser(Util.UserName));
			
			if (rolesCache == null)
			{
				// The factory method should have created a cache item, so try again.
				rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)HelperFunctions.GetCache(CacheItem.GalleryServerRoles);
				if (rolesCache == null)
				{
					AppErrorController.LogError(new WebException("The method Factory.LoadGalleryServerRoles() should have created a cache entry, but none was found. This is not an issue if it occurs occasionally, but should be addressed if it is frequent."));
					return roles;
				}
			}

			// Add to the cache, but only if we have access to the session ID. Some pages, such as synchronize.aspx, turn off session 
			// so that asynchronous page methods can work.
			if (HttpContext.Current.Session != null)
			{
				lock (rolesCache)
				{
					string key = String.Concat(HttpContext.Current.Session.SessionID, Util.UserName);
					if (!rolesCache.ContainsKey(key))
					{
						rolesCache.Add(key, roles);
					}
				}
				HelperFunctions.SetCache(CacheItem.GalleryServerRoles, rolesCache);
			}

			return roles;
		}

		/// <summary>
		/// Create a Gallery Server Pro role corresponding to the specified parameters. Also creates the corresponding ASP.NET role.
		/// Throws an exception if a role with the specified name already exists in the data store. The role is persisted to the data store.
		/// </summary>
		/// <param name="roleName">A string that uniquely identifies the role.</param>
		/// <param name="allowViewAlbumOrMediaObject">A value indicating whether the user assigned to this role has permission to view albums
		/// and media objects.</param>
		/// <param name="allowViewOriginalImage">A value indicating whether the user assigned to this role has permission to view the original,
		/// high resolution version of an image. This setting applies only to images. It has no effect if there are no
		/// high resolution images in the album or albums to which this role applies.</param>
		/// <param name="allowAddMediaObject">A value indicating whether the user assigned to this role has permission to add media objects to an album.</param>
		/// <param name="allowAddChildAlbum">A value indicating whether the user assigned to this role has permission to create child albums.</param>
		/// <param name="allowEditMediaObject">A value indicating whether the user assigned to this role has permission to edit a media object.</param>
		/// <param name="allowEditAlbum">A value indicating whether the user assigned to this role has permission to edit an album.</param>
		/// <param name="allowDeleteMediaObject">A value indicating whether the user assigned to this role has permission to delete media objects within an album.</param>
		/// <param name="allowDeleteChildAlbum">A value indicating whether the user assigned to this role has permission to delete child albums.</param>
		/// <param name="allowSynchronize">A value indicating whether the user assigned to this role has permission to synchronize an album.</param>
		/// <param name="allowAdministerSite">A value indicating whether the user has administrative permission for all albums. This permission
		/// automatically applies to all albums; it cannot be selectively applied.</param>
		/// <param name="hideWatermark">A value indicating whether the user assigned to this role has a watermark applied to images.
		/// This setting has no effect if watermarks are not used. A true value means the user does not see the watermark;
		/// a false value means the watermark is applied.</param>
		/// <param name="topLevelCheckedAlbumIds">The top level checked album ids. May be null.</param>
		/// <returns>Returns an <see cref="IGalleryServerRole" /> object corresponding to the specified parameters.</returns>
		/// <exception cref="InvalidGalleryServerRoleException">Thrown when a role with the specified role name already exists in the data store.</exception>
		public static IGalleryServerRole CreateRole(string roleName, bool allowViewAlbumOrMediaObject, bool allowViewOriginalImage, bool allowAddMediaObject, bool allowAddChildAlbum, bool allowEditMediaObject, bool allowEditAlbum, bool allowDeleteMediaObject, bool allowDeleteChildAlbum, bool allowSynchronize, bool allowAdministerSite, bool hideWatermark, IIntegerCollection topLevelCheckedAlbumIds)
		{
			CreateRole(roleName);

			IGalleryServerRole role = Factory.CreateGalleryServerRoleInstance(roleName, allowViewAlbumOrMediaObject, allowViewOriginalImage, allowAddMediaObject, allowAddChildAlbum, allowEditMediaObject, allowEditAlbum, allowDeleteMediaObject, allowDeleteChildAlbum, allowSynchronize, allowAdministerSite, hideWatermark);

			UpdateRoleAlbumRelationships(role, topLevelCheckedAlbumIds);

			role.Save();

			return role;
		}

		/// <summary>
		/// Create a role to manage the ownership permissions for the <paramref name="album"/> and user specified in the OwnerUserName
		/// property of the album. The permissions of the new role are copied from the album owner role template. The new role
		/// is persisted to the data store and the user specified as the album owner is added as its sole member. The album is updated
		/// so that the OwnerRoleName property contains the role's name, but the album is not persisted to the data store.
		/// </summary>
		/// <param name="album">The album for which a role to represent owner permissions is to be created.</param>
		/// <returns>Returns the name of the role that is created.</returns>
		public static string CreateAlbumOwnerRole(IAlbum album)
		{
			// Create a role modeled after the template owner role, attach it to the album, then add the specified user as its member.
			// Role name: Album Owner - rdmartin - rdmartin's album (album 193)
			if (album == null)
				throw new ArgumentNullException("album");

			if (album.IsNew)
				throw new ArgumentException("Album must be persisted to data store before calling Util.CreateAlbumOwnerRole.");

			// Get the album title, shortened if necessary
			const int maxAlbumTitleLengthToUseInAlbumOwnerRole = 100;
			string albumTitle = album.Title;
			if (albumTitle.Length > maxAlbumTitleLengthToUseInAlbumOwnerRole)
				albumTitle = albumTitle.Substring(0, maxAlbumTitleLengthToUseInAlbumOwnerRole);

			// Get the user name, shortened if necessary
			const int maxUsernameLengthToUseInAlbumOwnerRole = 100;
			string userName = album.OwnerUserName;
			if (userName.Length > maxUsernameLengthToUseInAlbumOwnerRole)
				userName = userName.Substring(0, maxUsernameLengthToUseInAlbumOwnerRole);

			string roleName = String.Format("{0} - {1} - {2} (album {3})", GlobalConstants.AlbumOwnerRoleNamePrefix, userName, albumTitle, album.Id);

			if (!RoleExists(roleName))
				CreateRole(roleName);

			if (!IsUserInRole(album.OwnerUserName, roleName))
				AddUserToRole(album.OwnerUserName, roleName);

			// Remove the roles from the cache. We do this because may may have just created a user album (that is, 
			// AlbumController.CreateUserAlbum() is in the call stack) and we want to make sure the AllAlbumIds property
			// of the album owner template role has the latest list of albums, including potentially the new album 
			// (which will be the case if the administrator has selected a parent album of the user album in the template
			// role).
			HelperFunctions.RemoveCache(CacheItem.GalleryServerRoles);

			IGalleryServerRole role = Factory.LoadGalleryServerRole(roleName);
			if (role == null)
			{
				IGalleryServerRole roleSource = Factory.LoadGalleryServerRole(GlobalConstants.AlbumOwnerRoleTemplateName);
				
				if (roleSource == null)
					roleSource = CreateAlbumOwnerRoleTemplate();

				role = roleSource.Copy();
				role.RoleName = roleName;
			}

			if (!role.AllAlbumIds.Contains(album.Id))
				role.RootAlbumIds.Add(album.Id);

			role.Save();

			return roleName;
		}

		/// <summary>
		/// Delete the specified role. Both components of the role are deleted: the IGalleryServerRole and ASP.NET role.
		/// </summary>
		/// <param name="roleName">Name of the role. Must match an existing <see cref="IGalleryServerRole.RoleName"/>. If no match
		/// if found, no action is taken.</param>
		public static void DeleteGalleryServerProRole(string roleName)
		{
			ValidateDeleteRole(roleName);

			try
			{
				DeleteGalleryServerRole(roleName);
			}
			finally
			{
				try
				{
					DeleteAspnetRole(roleName);
				}
				finally
				{
					HelperFunctions.PurgeCache();
				}
			}
		}

		/// <summary>
		/// Make sure the list of ASP.NET roles is synchronized with the Gallery Server roles. If any are missing from 
		/// either, add it.
		/// </summary>
		public static void ValidateRoles()
		{
			List<IGalleryServerRole> validatedRoles = new List<IGalleryServerRole>();
			IGalleryServerRoleCollection galleryRoles = Factory.LoadGalleryServerRoles();
			bool needToPurgeCache = false;

			foreach (string roleName in GetAllRoles())
			{
				IGalleryServerRole galleryRole = galleryRoles.GetRoleByRoleName(roleName);
				if (galleryRole == null)
				{
					// This is an ASP.NET role that doesn't exist in our list of gallery server roles. Add it with minimum permissions
					// applied to zero albums.
					IGalleryServerRole newRole = Factory.CreateGalleryServerRoleInstance(roleName, false, false, false, false, false, false, false, false, false, false, false);
					newRole.Save();
					needToPurgeCache = true;
				}
				validatedRoles.Add(galleryRole);
			}

			// Now check to see if there are gallery roles that are not ASP.NET roles. Add if necessary.
			foreach (IGalleryServerRole galleryRole in galleryRoles)
			{
				if (!validatedRoles.Contains(galleryRole))
				{
					// Need to create an ASP.NET role for this gallery role.
					CreateRole(galleryRole.RoleName);
					needToPurgeCache = true;
				}
			}

			if (needToPurgeCache)
			{
				HelperFunctions.PurgeCache();
			}
		}

		/// <summary>
		/// Verify that any role needed for album ownership exists and is properly configured. If an album owner
		/// is specified and the album is new (IsNew == true), the album is persisted to the data store. This is 
		/// required because the ID is not assigned until it is saved, and a valid ID is required to configure the
		/// role.
		/// </summary>
		/// <param name="album">The album to validate for album ownership. If a null value is passed, the function
		/// returns without error or taking any action.</param>
		public static void ValidateRoleExistsForAlbumOwner(IAlbum album)
		{
			// For albums, verify that any needed roles for album ownership are present. Create/update as needed.
			if (album == null)
				return;

			if (String.IsNullOrEmpty(album.OwnerUserName))
			{
				// If owner role is specified, delete it.
				if (!String.IsNullOrEmpty(album.OwnerRoleName))
				{
					DeleteGalleryServerProRole(album.OwnerRoleName);
					album.OwnerRoleName = String.Empty;
				}
			}
			else
			{
				// If this is a new album, save it before proceeding. We will need its album ID to configure the role, 
				// and it is not assigned until it is saved.
				if (album.IsNew)
					album.Save();

				// Verify that a role exists that corresponds to the owner.
				IGalleryServerRole role = Factory.LoadGalleryServerRoles().GetRoleByRoleName(album.OwnerRoleName);
				if (role == null)
				{
					// No role exists. Create it.
					album.OwnerRoleName = CreateAlbumOwnerRole(album);
				}
				else
				{
					// Role exists. Make sure album is assigned to role and owner is a member.
					if (!role.RootAlbumIds.Contains(album.Id))
					{
						// Current album is not a member. This should not typically occur, but just in case
						// it does let's add the current album to it and save it.
						role.RootAlbumIds.Add(album.Id);
						role.Save();
					}

					string[] rolesForUser = GetRolesForUser(album.OwnerUserName);
					if (Array.IndexOf<string>(rolesForUser, role.RoleName) < 0)
					{
						// Owner is not a member. Add.
						AddUserToRole(album.OwnerUserName, role.RoleName);
					}
				}
			}
		}

		/// <summary>
		/// Replace the list of root album IDs for the <paramref name="role"/> with the album ID's specified in
		/// <paramref name="topLevelCheckedAlbumIds"/>. Note that this function will cause the AllAlbumIds property 
		/// to be cleared out (Count = 0). The property can be repopulated by calling <see cref="IGalleryServerRole.Save"/>.
		/// </summary>
		/// <param name="role">The role whose root album/role relationships should be updated. When editing
		/// an existing role, specify this.GalleryRole. For new roles, pass the newly created role before
		/// saving it.</param>
		/// <param name="topLevelCheckedAlbumIds">The top level checked album ids. May be null.</param>
		public static void UpdateRoleAlbumRelationships(IGalleryServerRole role, IIntegerCollection topLevelCheckedAlbumIds)
		{
			if (role == null)
				throw new ArgumentNullException("role");

			if (topLevelCheckedAlbumIds == null)
				topLevelCheckedAlbumIds = new IntegerCollection();

			int[] rootAlbumIdsOld = new int[role.RootAlbumIds.Count];
			role.RootAlbumIds.CopyTo(rootAlbumIdsOld, 0);

			role.RootAlbumIds.Clear();

			if (role.AllowAdministerSite)
			{
				// Administer site permission automatically applies to all albums, so all we need to do is get
				// a reference to the root album ID.
				role.RootAlbumIds.Add(Factory.LoadRootAlbumInstance().Id);
			}
			else
			{
				role.RootAlbumIds.AddRange(topLevelCheckedAlbumIds);
			}

			if (IsRoleAnAlbumOwnerRole(role.RoleName))
				ValidateAlbumOwnerRoles(role.RoleName, rootAlbumIdsOld, role.RootAlbumIds);
		}

		/// <summary>
		/// Determines whether the <paramref name="roleName"/> is a role that serves as an album owner role. Returns <c>true</c> if the
		/// <paramref name="roleName"/> starts with the same string as the global constant <see cref="GlobalConstants.AlbumOwnerRoleNamePrefix"/>.
		/// Album owner roles are roles that are programmatically created to provide the security context used for the album ownership
		/// and user album features.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		/// <returns>
		/// 	<c>true</c> if <paramref name="roleName"/> is a role that serves as an album owner role; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsRoleAnAlbumOwnerRole(string roleName)
		{
			return roleName.StartsWith(GlobalConstants.AlbumOwnerRoleNamePrefix);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the Role provider used by Gallery Server Pro.
		/// </summary>
		/// <returns>The Role provider used by Gallery Server Pro.</returns>
		private static RoleProvider GetRoleProvider()
		{
			if (String.IsNullOrEmpty(Config.GetCore().RoleProviderName))
			{
				return Roles.Provider;
			}
			else
			{
				return Roles.Providers[Config.GetCore().RoleProviderName];
			}
		}

		private static void ValidateDeleteRole(string roleName)
		{
			// Make sure the loggod-on person isn't doing anything stupid, like deleting the only role with Administer
			// site permission.
			IGalleryServerRole roleToDelete = Factory.LoadGalleryServerRole(roleName);

			if (roleToDelete == null)
				return;

			if (roleToDelete.AllowAdministerSite)
			{
				// User is trying to delete a role with administer site permission. Make sure
				// at least one other role has this permission, and that the role has at least one member.
				bool atLeastOneOtherRoleHasAdminSitePermission = false;
				foreach (IGalleryServerRole role in Factory.LoadGalleryServerRoles())
				{
					if ((!role.RoleName.Equals(roleToDelete.RoleName, StringComparison.OrdinalIgnoreCase) && role.AllowAdministerSite))
					{
						if (GetUsersInRole(role.RoleName).Length > 0)
						{
							atLeastOneOtherRoleHasAdminSitePermission = true;
							break;
						}
					}
				}

				if (!atLeastOneOtherRoleHasAdminSitePermission)
				{
					throw new WebException(Resources.GalleryServerPro.Admin_Manage_Roles_Cannot_Delete_Role_Msg);
				}
			}

		}

		private static void DeleteAspnetRole(string roleName)
		{
			if (String.IsNullOrEmpty(roleName))
				return;

			if (RoleExists(roleName))
				DeleteRole(roleName); // This also deletes any user/role relationships
		}

		private static void DeleteGalleryServerRole(string roleName)
		{
			IGalleryServerRole role = Factory.LoadGalleryServerRole(roleName);

			if (role != null)
			{
				UpdateAlbumOwnerBeforeRoleDelete(role);
				role.Delete();
			}
		}

		/// <summary>
		/// For roles that provide album ownership functionality, remove users belonging to this role from the OwnedBy 
		/// property of any albums this role is assigned to. Since we are deleting the role that provides the ownership
		/// functionality, it is necessary to clear the owner field of all affected albums.
		/// </summary>
		/// <param name="role">Name of the role to be deleted.</param>
		private static void UpdateAlbumOwnerBeforeRoleDelete(IGalleryServerRole role)
		{
			// Proceed only when dealing with an album ownership role.
			if (!IsRoleAnAlbumOwnerRole(role.RoleName))
				return;

			// Loop through each album assigned to this role. If this role is assigned as the owner role,
			// clear the OwnerUserName property.
			foreach (int albumId in role.RootAlbumIds)
			{
				IAlbum album = Factory.LoadAlbumInstance(albumId, false);
				if (album.OwnerRoleName == role.RoleName)
				{
					album.OwnerUserName = String.Empty;
					GalleryObjectController.SaveGalleryObject(album);
				}
			}
		}

		/// <summary>
		/// Creates the album owner role template. This is the role that is used as the template for roles that define 
		/// a user's permission level when the user is assigned as an album owner. Call this method when the role does
		/// not exist. It is set up with all permissions except Administer Site. The HideWatermark permission is not applied,
		/// so this role allows its members to view watermarks if that functionality is enabled.
		/// </summary>
		/// <returns>Returns an <see cref="IGalleryServerRole"/> that can be used as a template for all album owner roles.</returns>
		private static IGalleryServerRole CreateAlbumOwnerRoleTemplate()
		{
			return CreateRole(GlobalConstants.AlbumOwnerRoleTemplateName, true, true, true, true, true, true, true, true, true, false, false, null);
		}

		/// <summary>
		/// Validates the album owner. If an album is being removed from the <paramref name="roleName"/> and that album is
		/// using this role for album ownership, remove the ownership setting from the album.
		/// </summary>
		/// <param name="roleName">Name of the role that is being modified.</param>
		/// <param name="rootAlbumIdsOld">The list of album ID's that were previously assigned to the role. If an album ID exists
		/// in this object and not in <paramref name="rootAlbumIdsNew"/>, that means the album is being removed from the role.</param>
		/// <param name="rootAlbumIdsNew">The list of album ID's that are now assigned to the role. If an album ID exists
		/// in this object and not in <paramref name="rootAlbumIdsOld"/>, that means it is a newly added album.</param>
		private static void ValidateAlbumOwnerRoles(string roleName, IEnumerable<int> rootAlbumIdsOld, ICollection<int> rootAlbumIdsNew)
		{
			foreach (int albumId in rootAlbumIdsOld)
			{
				if (!rootAlbumIdsNew.Contains(albumId))
				{
					// Album has been removed from role. Remove owner from the album if the album owner role matches the one we are dealing with.
					IAlbum album = Factory.LoadAlbumInstance(albumId, false);
					if (album.OwnerRoleName == roleName)
					{
						album.OwnerUserName = String.Empty;
						GalleryObjectController.SaveGalleryObject(album);
					}
				}
			}
		}

		#endregion

	}
}

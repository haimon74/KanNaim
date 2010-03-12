using System;
using System.Collections.Generic;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.Business;
using GalleryServerPro.Web.Entity;
using GalleryServerPro.Web.Pages;
using System.Globalization;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for interacting with albums. Typically web pages directly call the appropriate business layer objects,
	/// but when a task involves multiple steps or the functionality does not exist in the business layer, the methods here are
	/// used.
	/// </summary>
	public static class AlbumController
	{
		#region Public Static Methods

		/// <summary>
		/// Creates an album, assigns the user name as the owner, saves it, and returns the newly created album. 
		/// A profile entry is created containing the album ID. Returns null if the ID specified in the config file
		/// for the parent album does not represent an existing album. That is, returns null if the userAlbumParentAlbumId
		/// in galleryserverpro.config does not match an existing album.
		/// </summary>
		/// <param name="userName">The user name representing the user who is the owner of the album.</param>
		/// <returns>Returns the newly created user album. It has already been persisted to the database.
		/// Returns null if the ID specified in the config file for the parent album does not represent an existing album.
		/// That is, returns null if the userAlbumParentAlbumId in galleryserverpro.config does not match an existing album.</returns>
		public static IAlbum CreateUserAlbum(string userName)
		{
			Core core = Config.GetCore();
			string albumNameTemplate = core.UserAlbumNameTemplate;

			IAlbum parentAlbum;
			try
			{
				parentAlbum = Factory.LoadAlbumInstance(core.UserAlbumParentAlbumId, false);
			}
			catch (ErrorHandler.CustomExceptions.InvalidAlbumException ex)
			{
				// The parent album does not exist. Record the error and return null.
				string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Error_User_Album_Parent_Invalid_Ex_Msg, core.UserAlbumParentAlbumId);
				AppErrorController.LogError(new ErrorHandler.CustomExceptions.WebException(msg, ex));
				return null;
			}

			IAlbum album = Factory.CreateAlbumInstance();

			album.Title = albumNameTemplate.Replace("{UserName}", userName);
			album.Summary = core.UserAlbumSummaryTemplate;
			album.OwnerUserName = userName;
			//newAlbum.ThumbnailMediaObjectId = 0; // not needed
			album.Parent = parentAlbum;
			album.IsPrivate = parentAlbum.IsPrivate;
			GalleryObjectController.SaveGalleryObject(album, userName);

			SaveAlbumIdToProfile(album.Id, userName);

			HelperFunctions.PurgeCache();

			return album;
		}

		/// <summary>
		/// Get a reference to the highest level album the current user has permission to add albums to. Returns null if no album
		/// meets this criteria.
		/// </summary>
		/// <returns>Returns a reference to the highest level album the user has permission to add albums to.</returns>
		public static IAlbum GetHighestLevelAlbumWithCreatePermission()
		{
			// Step 1: Loop through the roles and compile a list of album IDs where the role has create album permission.
			List<int> rootAlbumIdsWithCreatePermission = new List<int>();
			foreach (IGalleryServerRole role in RoleController.GetGalleryServerRolesForUser())
			{
				if (role.AllowAddChildAlbum)
				{
					foreach (int albumId in role.RootAlbumIds)
					{
						if (!rootAlbumIdsWithCreatePermission.Contains(albumId))
							rootAlbumIdsWithCreatePermission.Add(albumId);
					}
				}
			}

			// Step 2: Loop through our list of album IDs. If any album has an ancestor that is also in the list, then remove it. 
			// We only want a list of top level albums.
			List<int> albumIdsToRemove = new List<int>();
			foreach (int albumIdWithCreatePermission in rootAlbumIdsWithCreatePermission)
			{
				IGalleryObject album = Factory.LoadAlbumInstance(albumIdWithCreatePermission, false);
				while (true)
				{
					album = album.Parent as IAlbum;
					if (album == null)
						break;

					if (rootAlbumIdsWithCreatePermission.Contains(album.Id))
					{
						albumIdsToRemove.Add(albumIdWithCreatePermission);
						break;
					}
				}
			}

			foreach (int albumId in albumIdsToRemove)
			{
				rootAlbumIdsWithCreatePermission.Remove(albumId);
			}

			// Step 3: Starting with the root album, start iterating through the child albums. When we get to
			// one in our list, we can conclude that is the highest level album for which the user has create album permission.
			return FindFirstMatchingAlbumRecursive(Factory.LoadRootAlbumInstance(), rootAlbumIdsWithCreatePermission);
		}

		/// <summary>
		/// Get a reference to the highest level album the current user has permission to add albums and/or media objects to. Returns
		/// null if no album meets this criteria.
		/// </summary>
		/// <param name="verifyAddAlbumPermissionExists">Specifies whether the current user must have permission to add child albums
		/// to the album.</param>
		/// <param name="verifyAddMediaObjectPermissionExists">Specifies whether the current user must have permission to add media objects
		/// to the album.</param>
		/// <returns>
		/// Returns a reference to the highest level album the user has permission to add albums and/or media objects to.
		/// </returns>
		public static IAlbum GetHighestLevelAlbumWithAddPermission(bool verifyAddAlbumPermissionExists, bool verifyAddMediaObjectPermissionExists)
		{
			// Step 1: Loop through the roles and compile a list of album IDs where the role has the required permission.
			// If the verifyAddAlbumPermissionExists parameter is true, then the user must have permission to add child albums.
			// If the verifyAddMediaObjectPermissionExists parameter is true, then the user must have permission to add media objects.
			// If either parameter is false, then the absense of that permission does not disqualify an album.
			List<int> rootAlbumIdsWithPermission = new List<int>();
			foreach (IGalleryServerRole role in RoleController.GetGalleryServerRolesForUser())
			{
				bool albumPermGranted = (verifyAddAlbumPermissionExists ? role.AllowAddChildAlbum : true);
				bool mediaObjectPermGranted = (verifyAddMediaObjectPermissionExists ? role.AllowAddMediaObject : true);

				if (albumPermGranted && mediaObjectPermGranted)
				{
					// This role satisfies the requirements, so add each album to the list.
					foreach (int albumId in role.RootAlbumIds)
					{
						if (!rootAlbumIdsWithPermission.Contains(albumId))
							rootAlbumIdsWithPermission.Add(albumId);
					}
				}
			}

			// Step 2: Loop through our list of album IDs. If any album has an ancestor that is also in the list, then remove it. 
			// We only want a list of top level albums.
			List<int> albumIdsToRemove = new List<int>();
			foreach (int albumIdWithPermission in rootAlbumIdsWithPermission)
			{
				IGalleryObject album = Factory.LoadAlbumInstance(albumIdWithPermission, false);
				while (true)
				{
					album = album.Parent as IAlbum;
					if (album == null)
						break;

					if (rootAlbumIdsWithPermission.Contains(album.Id))
					{
						albumIdsToRemove.Add(albumIdWithPermission);
						break;
					}
				}
			}

			foreach (int albumId in albumIdsToRemove)
			{
				rootAlbumIdsWithPermission.Remove(albumId);
			}

			// Step 3: Starting with the root album, start iterating through the child albums. When we get to
			// one in our list, we can conclude that is the highest level album for which the user has create album permission.
			return FindFirstMatchingAlbumRecursive(Factory.LoadRootAlbumInstance(), rootAlbumIdsWithPermission);
		}

		/// <summary>
		/// Update the album with the specified properties in the albumEntity parameter. The title is validated before
		/// saving, and may be altered to conform to business rules, such as removing HTML tags. After the object is persisted
		/// to the data store, the albumEntity parameter is updated with the latest properties from the album object and returned.
		/// If the user is not authorized to edit the album, no action is taken.
		/// </summary>
		/// <param name="albumEntity">An AlbumWebEntity instance containing data to be persisted to the data store.</param>
		/// <returns>Returns an AlbumWebEntity instance containing the data as persisted to the data store. Some properties,
		/// such as the Title property, may be slightly altered to conform to validation rules.</returns>
		public static AlbumWebEntity UpdateAlbumInfo(AlbumWebEntity albumEntity)
		{
			if (albumEntity.Owner == Resources.GalleryServerPro.UC_Album_Header_Edit_Album_No_Owner_Text)
			{
				albumEntity.Owner = String.Empty;
			}

			IAlbum album = Factory.LoadAlbumInstance(albumEntity.Id, false);

			// Update remaining properties if user has edit album permission.
			if (GalleryPage.IsUserAuthorized(SecurityActions.EditAlbum, album.Id))
			{
				if (album.Title != albumEntity.Title)
				{
					album.Title = Util.CleanHtmlTags(albumEntity.Title);
					if ((!album.IsRootAlbum) && (Config.GetCore().SynchAlbumTitleAndDirectoryName))
					{
						// Root albums do not have a directory name that reflects the album's title, so only update this property for non-root albums.
						album.DirectoryName = HelperFunctions.ValidateDirectoryName(album.Parent.FullPhysicalPath, album.Title);
					}
				}
				album.Summary = Util.CleanHtmlTags(albumEntity.Summary);
				album.DateStart = albumEntity.DateStart.Date;
				album.DateEnd = albumEntity.DateEnd.Date;
				if (albumEntity.IsPrivate != album.IsPrivate)
				{
					if (!albumEntity.IsPrivate && album.Parent.IsPrivate)
					{
						throw new NotSupportedException("Cannot make album public: It is invalid to make an album public when it's parent album is private.");
					}
					album.IsPrivate = albumEntity.IsPrivate;
					SynchIsPrivatePropertyOnChildGalleryObjects(album);
				}

				// If the owner has changed, update it, but only if the user is administrator.
				if (albumEntity.Owner != album.OwnerUserName)
				{
					if (GalleryPage.IsUserAuthorized(SecurityActions.AdministerSite, album.Id))
					{
						if (!String.IsNullOrEmpty(album.OwnerUserName))
						{
							// Another user was previously assigned as owner. Delete role since this person will no longer be the owner.
							RoleController.DeleteGalleryServerProRole(album.OwnerRoleName);
						}
						// Util.SaveGalleryObject will make sure there is a role created for this user.
						album.OwnerUserName = albumEntity.Owner;
					}
				}

				GalleryObjectController.SaveGalleryObject(album);
				HelperFunctions.PurgeCache();

				// Refresh the entity object with the data from the album object, in case something changed. For example,
				// some javascript or HTML may have been removed from the Title or Summary fields.
				albumEntity.Title = album.Title;
				albumEntity.Summary = album.Summary;
				albumEntity.DateStart = album.DateStart;
				albumEntity.DateEnd = album.DateEnd;
				albumEntity.IsPrivate = album.IsPrivate;
				albumEntity.Owner = album.OwnerUserName;
			}

			return albumEntity;
		}

		/// <summary>
		/// Permanently delete this album from the data store and disk. Validation is performed prior to deletion to ensure
		/// album can be safely deleted. The validation is contained in the method <see cref="ValidateBeforeAlbumDelete"/>
		/// and may be invoked separately if desired. No security checks are performed; the caller must ensure the user
		/// has permission to delete an album prior to invoking this method.
		/// </summary>
		/// <param name="album">The album to delete. If null, the function returns without taking any action.</param>
		/// <exception cref="ErrorHandler.CustomExceptions.CannotDeleteAlbumException">Thrown when the album does not meet the 
		/// requirements for safe deletion. At this time this exception is thrown only when the album is or contains the user album 
		/// parent album and user albums are enabled.</exception>
		public static void DeleteAlbum(IAlbum album)
		{
			if (album == null)
				return;

			ValidateBeforeAlbumDelete(album);

			OnBeforeAlbumDelete(album);

			album.Delete();

			HelperFunctions.PurgeCache();
		}

		/// <summary>
		/// Verifies that the album meets the prerequisites to be safely deleted but does not actually delete the album. Throws a
		/// CannotDeleteAlbumException when it cannot be deleted. Specifically, the function checks to see if user albums are enabled, and
		/// if they are, it checks if the album to delete contains the user album parent album. If it does, the validation fails and an
		/// exception is thrown.
		/// </summary>
		/// <param name="albumToDelete">The album to delete.</param>
		/// <remarks>This function is automatically called when using the <see cref="DeleteAlbum"/> method, so it is not necessary to 
		/// invoke when using that method. Typically you will call this method when there are several items to delete and you want to 
		/// check all of them before deleting any of them, such as we have on the Delete Objects page.</remarks>
		/// <exception cref="ErrorHandler.CustomExceptions.CannotDeleteAlbumException">Thrown when the album does not meet the 
		/// requirements for safe deletion. At this time this exception is thrown only when the album is or contains the user album 
		/// parent album and user albums are enabled.</exception>
		public static void ValidateBeforeAlbumDelete(IAlbum albumToDelete)
		{
			if (!Config.GetCore().EnableUserAlbum)
				return;

			IGalleryObject userAlbumParent;
			try
			{
				userAlbumParent = Factory.LoadAlbumInstance(Config.GetCore().UserAlbumParentAlbumId, false);
			}
			catch (ErrorHandler.CustomExceptions.InvalidAlbumException ex)
			{
				// User album doesn't exist. Record the error and then return because there is no problem
				// with deleting the current album.
				string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Error_User_Album_Parent_Invalid_Ex_Msg, albumToDelete.Id);
				AppErrorController.LogError(new ErrorHandler.CustomExceptions.WebException(msg, ex));
				return;
			}

			// Test #1: Are we trying to delete the album that is specified as the user album parent album?
			bool albumToDeleteIsUserAlbumContainer = (userAlbumParent.Id == albumToDelete.Id);

			// Test #2: Does the user album parent album exist somewhere below the album we want to delete?
			bool userAlbumExistsInAlbumToDelete = false;
			IGalleryObject albumParent = userAlbumParent.Parent;
			while (!(albumParent is GalleryServerPro.Business.NullObjects.NullGalleryObject))
			{
				if (albumParent.Id == albumToDelete.Id)
				{
					userAlbumExistsInAlbumToDelete = true;
					break;
				}
				albumParent = albumParent.Parent;
			}

			bool okToDelete = (!(albumToDeleteIsUserAlbumContainer || userAlbumExistsInAlbumToDelete));
			if (!okToDelete)
			{
				// Album is or contains the user album parent album and user albums are enabled. Throw exception.
				string albumTitle = String.Concat("'", albumToDelete.Title, "' (ID# ", albumToDelete.Id, ")");
				string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Delete_Album_Cannot_Delete_Contains_User_Album_Parent_Ex_Msg, albumTitle);
				throw new ErrorHandler.CustomExceptions.CannotDeleteAlbumException(msg);
			}
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Performs any necessary actions that must occur before an album is deleted. Specifically, it deletes the owner role 
		/// if one exists for the album, but only when this album is the only one assigned to the role.
		/// </summary>
		/// <param name="album">The album to be deleted.</param>
		private static void OnBeforeAlbumDelete(IAlbum album)
		{
			// If there is an owner role associated with this album, and the role is not assigned to any other albums, delete it.
			if (!String.IsNullOrEmpty(album.OwnerRoleName))
			{
				IGalleryServerRole role = RoleController.GetGalleryServerRoles().GetRoleByRoleName(album.OwnerRoleName);

				if ((role != null) && (role.AllAlbumIds.Count == 1) && role.AllAlbumIds.Contains(album.Id))
				{
					RoleController.DeleteGalleryServerProRole(role.RoleName);
				}
			}
		}

		/// <summary>
		/// Finds the first album within the heirarchy of the specified <paramref name="album"/> whose ID is in 
		/// <paramref name="albumIds"/>. Acts recursively in an across-first, then-down search pattern, resulting 
		/// in the highest level matching album to be returned. Returns null if there are no matching albums.
		/// </summary>
		/// <param name="album">The album to be searched to see if it, or any of its children, matches one of the IDs
		/// in <paramref name="albumIds"/>.</param>
		/// <param name="albumIds">Contains the IDs of the albums to search for.</param>
		/// <returns>Returns the first album within the heirarchy of the specified <paramref name="album"/> whose ID is in 
		/// <paramref name="albumIds"/>.</returns>
		private static IAlbum FindFirstMatchingAlbumRecursive(IAlbum album, ICollection<int> albumIds)
		{
			// Is the current album in the list?
			if (albumIds.Contains(album.Id))
				return album;

			// Nope, so look at the child albums of this album.
			IAlbum albumToSelect = null;
			IGalleryObjectCollection childAlbums = album.GetChildGalleryObjects(GalleryObjectType.Album, true);

			foreach (IGalleryObject childAlbum in childAlbums)
			{
				if (albumIds.Contains(childAlbum.Id))
				{
					albumToSelect = (IAlbum)childAlbum;
					break;
				}
			}

			// Not the child albums either, so iterate through the children of the child albums. Act recursively.
			if (albumToSelect == null)
			{
				foreach (IGalleryObject childAlbum in childAlbums)
				{
					albumToSelect = (IAlbum)FindFirstMatchingAlbumRecursive((IAlbum)childAlbum, albumIds);

					if (albumToSelect != null)
						break;
				}
			}

			return albumToSelect; // Returns null if no matching album is found
		}

		private static void SaveAlbumIdToProfile(int albumId, string userName)
		{
			ProfileEntity profileEntity = ProfileController.GetProfile(userName);
			
			profileEntity.UserAlbumId = albumId;

			ProfileController.SaveProfile(profileEntity);
		}

		/// <summary>
		/// Set the IsPrivate property of all child albums and media objects of the specified album to have the same value
		/// as the specified album.
		/// </summary>
		/// <param name="album">The album whose child objects are to be updated to have the same IsPrivate value.</param>
		private static void SynchIsPrivatePropertyOnChildGalleryObjects(IAlbum album)
		{
			album.Inflate(true);
			foreach (IAlbum childAlbum in album.GetChildGalleryObjects(GalleryObjectType.Album))
			{
				childAlbum.Inflate(true); // The above Inflate() does not inflate child albums, so we need to explicitly inflate it.
				childAlbum.IsPrivate = album.IsPrivate;
				GalleryObjectController.SaveGalleryObject(childAlbum);
				SynchIsPrivatePropertyOnChildGalleryObjects(childAlbum);
			}

			foreach (IGalleryObject childGalleryObject in album.GetChildGalleryObjects(GalleryObjectType.MediaObject))
			{
				childGalleryObject.IsPrivate = album.IsPrivate;
				GalleryObjectController.SaveGalleryObject(childGalleryObject);
			}
		}

		#endregion
	}
}

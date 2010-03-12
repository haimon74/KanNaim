using System;
using System.Collections.ObjectModel;
using GalleryServerPro.Business.Interfaces;
using System.Globalization;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// A collection of <see cref="IGalleryServerRole" /> objects.
	/// </summary>
	public class GalleryServerRoleCollection : Collection<IGalleryServerRole>, IGalleryServerRoleCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GalleryServerRoleCollection"/> class.
		/// </summary>
		public GalleryServerRoleCollection()
			: base(new System.Collections.Generic.List<IGalleryServerRole>())
		{
		}

		/// <summary>
		/// Sort the objects in this collection based on the <see cref="IGalleryServerRole.RoleName"/> property.
		/// </summary>
		public void Sort()
		{
			// We know galleryServerRoles is actually a List<IGalleryServerRole> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryServerRole> galleryServerRoles = (System.Collections.Generic.List<IGalleryServerRole>) Items;

			galleryServerRoles.Sort();
		}

		/// <summary>
		/// Return the role that matches the specified <paramref name="roleName"/>. It is not case sensitive, so that "ReadAll" matches "readall".
		/// Returns null if no match is found.
		/// </summary>
		/// <param name="roleName">The name of the role to return.</param>
		/// <returns>
		/// Returns the role that matches the specified role name. Returns null if no match is found.
		/// </returns>
		public IGalleryServerRole GetRoleByRoleName(string roleName)
		{
			// We know galleryServerRoles is actually a List<IGalleryServerRole> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryServerRole> galleryServerRoles = (System.Collections.Generic.List<IGalleryServerRole>) Items;

			return galleryServerRoles.Find(delegate(IGalleryServerRole galleryServerRole)
			{
				return (String.Compare(galleryServerRole.RoleName, roleName, true) == 0);
			});
		}

		/// <summary>
		/// Gets the Gallery Server roles that match the specified role names. The count of the returned collection will
		/// match the length of the roleNames array. It is not case sensitive, so that "ReadAll" matches "readall".
		/// </summary>
		/// <param name="roleNames">The name of the roles to return.</param>
		/// <returns>
		/// Returns the Gallery Server roles that match the specified role names.
		/// </returns>
		public IGalleryServerRoleCollection GetRolesByRoleNames(string[] roleNames)
		{
			// We know galleryServerRoles is actually a List<IGalleryServerRole> because we passed it to the constructor.
			System.Collections.Generic.List<IGalleryServerRole> galleryServerRoles = (System.Collections.Generic.List<IGalleryServerRole>) Items;

			IGalleryServerRoleCollection roles = new GalleryServerRoleCollection();
			foreach (string roleName in roleNames)
			{
				IGalleryServerRole role = galleryServerRoles.Find(delegate(IGalleryServerRole galleryServerRole)
				{
					return (String.Compare(galleryServerRole.RoleName, roleName, true) == 0);
				});

				if (role == null)
				{
					// Purge the cache so if the user manually updates the table, we will notice it the next time Gallery Server requests the roles.
					HelperFunctions.PurgeCache();
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Could not find a Gallery Server role named '{0}'. Verify the data table contains a record for this role and the current gallery, and that the cache is being properly managed.", roleName));
				}
				else
				{
					roles.Add(role);
				}
			}

			return roles;
		}

		//public int IndexOf(IGalleryObject galleryObject)
		//{
		//  // We know galleryObjects is actually a List<IGalleryObject> because we passed it to the constructor.
		//  System.Collections.Generic.List<IGalleryObject> galleryObjects = (System.Collections.Generic.List<IGalleryObject>)Items;

		//  return galleryObjects.IndexOf(galleryObject);
		//}

	}
}

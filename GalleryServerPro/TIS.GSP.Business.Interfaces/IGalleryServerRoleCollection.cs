using System;

namespace GalleryServerPro.Business.Interfaces
{
	/// <summary>
	/// A collection of <see cref="IGalleryServerRole" /> objects.
	/// </summary>
	public interface IGalleryServerRoleCollection : System.Collections.Generic.ICollection<IGalleryServerRole>
	{
		/// <summary>
		/// Sort the objects in this collection based on the <see cref="IGalleryServerRole.RoleName" /> property.
		/// </summary>
		void Sort();

		/// <summary>
		/// Return the role that matches the specified <paramref name="roleName"/>. It is not case sensitive, so that "ReadAll" matches "readall".
		/// Returns null if no match is found.
		/// </summary>
		/// <param name="roleName">The name of the role to return.</param>
		/// <returns>
		/// Returns the role that matches the specified role name. Returns null if no match is found.
		/// </returns>
		IGalleryServerRole GetRoleByRoleName(string roleName);

		/// <summary>
		/// Gets the Gallery Server roles that match the specified role names. The count of the returned collection will
		/// match the length of the roleNames array. It is not case sensitive, so that "ReadAll" matches "readall".
		/// </summary>
		/// <param name="roleNames">The name of the roles to return.</param>
		/// <returns>
		/// Returns the Gallery Server roles that match the specified role names.
		/// </returns>
		IGalleryServerRoleCollection GetRolesByRoleNames(string[] roleNames);

		/// <summary>
		/// Gets a reference to the IGalleryServerRole object at the specified index position.
		/// </summary>
		/// <param name="indexPosition">An integer specifying the position of the object within this collection to
		/// return. Zero returns the first item.</param>
		/// <returns>Returns a reference to the IGalleryServerRole object at the specified index position.</returns>
		IGalleryServerRole this[Int32 indexPosition]
		{
			get;
			set;
		}

		/// <summary>
		/// Searches for the specified object and returns the zero-based index of the first occurrence within the collection.  
		/// </summary>
		/// <param name="galleryServerRole">The gallery server role to locate in the collection. The value can be a null 
		/// reference (Nothing in Visual Basic).</param>
		/// <returns>The zero-based index of the first occurrence of galleryServerRole within the collection, if found; 
		/// otherwise, –1. </returns>
		Int32 IndexOf(IGalleryServerRole galleryServerRole);

	}
}

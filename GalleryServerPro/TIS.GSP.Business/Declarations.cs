namespace GalleryServerPro.Business
{
	/// <summary>
	/// Defines a list that uniquely identifies cache items stored in the cache.
	/// </summary>
	public enum CacheItem
	{
		/// <summary>
		/// A System.Collections.Generic.Dictionary&lt;<see cref="int" />, <see cref="GalleryServerPro.Business.Interfaces.IGalleryServerRoleCollection" />&gt;
		/// stored in cache. The key is a concatenation of the user's session ID and user name. The corresponding value stores the roles that 
		/// user belongs to. The first item in the dictionary will have a key = "AllRoles", and its dictionary entry holds all 
		/// roles used in the current gallery.
		/// </summary>
		GalleryServerRoles,
		/// <summary>
		/// A <see cref="System.Data.DataTable"/> named "Users" with a single string column named "UserName". Contains a list of all user
		/// names as reported by the membership provider (Membership.GetAllUsers()).
		/// </summary>
		UserNames,
		/// <summary>
		/// A System.Collections.Generic.Dictionary&lt;<see cref="int" />, <see cref="GalleryServerPro.Business.Interfaces.IAlbum" />&gt; 
		/// stored in cache. The key specifies the ID of the album stored in the dictionary entry.
		/// </summary>
		Albums,
		/// <summary>
		/// A System.Collections.Generic.Dictionary&lt;<see cref="int" />, <see cref="GalleryServerPro.Business.Interfaces.IGalleryObject" />&gt; 
		/// stored in cache. The key specifies the ID of the media object stored in the dictionary entry.
		/// </summary>
		MediaObjects,
		/// <summary>
		/// An IAppErrorCollection stored in cache.
		/// </summary>
		AppErrors
	}
}

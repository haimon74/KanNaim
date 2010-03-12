namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains user profile information.
	/// </summary>
	public class ProfileEntity
	{
		/// <summary>
		/// The account name of the user these profile settings belong to.
		/// </summary>
		public string UserName;

		/// <summary>
		/// A value indicating whether the user wants the metadata popup window to be displayed.
		/// </summary>
		public bool ShowMediaObjectMetadata;

		/// <summary>
		/// The ID for the user's personal album (aka user album).
		/// </summary>
		public int UserAlbumId;

		/// <summary>
		/// Indicates whether the user has enabled or disabled her personal album (aka user album).
		/// </summary>
		public bool EnableUserAlbum;
	}
}

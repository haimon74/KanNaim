namespace GalleryServerPro.Business.Interfaces
{
	/// <summary>
	/// Specifies the type of the gallery object.
	/// </summary>
	public enum GalleryObjectType
	{
		/// <summary>
		/// Specifies that no gallery object type has been assigned.
		/// </summary>
		None = 0,
		/// <summary>
		/// Gets all possible gallery object types.
		/// </summary>
		All = 0x0001,
		/// <summary>
		/// Gets all gallery object types except the Album type.
		/// </summary>
		MediaObject = 0x0002,
		/// <summary>
		/// Gets the Album gallery object type.
		/// </summary>
		Album = 0x0004,
		/// <summary>
		/// Gets the Image gallery object type.
		/// </summary>
		Image = 0x0008,
		/// <summary>
		/// Gets the Audio gallery object type.
		/// </summary>
		Audio = 0x0010,
		/// <summary>
		/// Gets the Video gallery object type.
		/// </summary>
		Video = 0x0020,
		/// <summary>
		/// Gets the Generic gallery object type.
		/// </summary>
		Generic = 0x0040,
		/// <summary>
		/// Gets the External gallery object type.
		/// </summary>
		External = 0x0080,
		/// <summary>
		/// Gets the Unknown gallery object type.
		/// </summary>
		Unknown = 0x0100
	}
}

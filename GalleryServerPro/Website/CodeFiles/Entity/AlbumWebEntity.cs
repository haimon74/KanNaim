using System;

namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains album information. This class is used to pass information between the browser and the web server
	/// via AJAX callbacks.
	/// </summary>
	public class AlbumWebEntity
	{
		/// <summary>
		/// The album ID.
		/// </summary>
		public int Id;
		/// <summary>
		/// The album title.
		/// </summary>
		public string Title;
		/// <summary>
		/// The album summary.
		/// </summary>
		public string Summary;
		/// <summary>
		/// The album owner.
		/// </summary>
		public string Owner;
		/// <summary>
		/// The starting date of this album.
		/// </summary>
		public DateTime DateStart;
		/// <summary>
		/// The ending date of this album.
		/// </summary>
		public DateTime DateEnd;
		/// <summary>
		/// Indicates whether this album is hidden from anonymous users.
		/// </summary>
		public bool IsPrivate;
	}
}


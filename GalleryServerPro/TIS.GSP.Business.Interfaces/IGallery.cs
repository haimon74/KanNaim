using System;

namespace GalleryServerPro.Business.Interfaces
{
	/// <summary>
	/// Represents a gallery within Gallery Server Pro.
	/// </summary>
	public interface IGallery
	{
		/// <summary>
		/// Gets or sets the unique identifier for this gallery.
		/// </summary>
		/// <value>The unique identifier for this gallery.</value>
		int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the description for this gallery.
		/// </summary>
		/// <value>The description for this gallery.</value>
		string Description
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the date this gallery was created.
		/// </summary>
		/// <value>The date this gallery was created.</value>
		DateTime CreationDate
		{
			get;
			set;
		}
	}
}

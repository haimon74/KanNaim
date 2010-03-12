using System;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Represents a gallery within Gallery Server Pro.
	/// </summary>
	public class Gallery : IGallery
	{
		private int _id;
		private string _description;
		private DateTime _creationDate;

		/// <summary>
		/// Gets or sets the unique identifier for this gallery.
		/// </summary>
		/// <value>The unique identifier for this gallery.</value>
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Gets or sets the description for this gallery.
		/// </summary>
		/// <value>The description for this gallery.</value>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		/// Gets or sets the date this gallery was created.
		/// </summary>
		/// <value>The date this gallery was created.</value>
		public DateTime CreationDate
		{
			get { return _creationDate; }
			set { _creationDate = value; }
		}
	}
}

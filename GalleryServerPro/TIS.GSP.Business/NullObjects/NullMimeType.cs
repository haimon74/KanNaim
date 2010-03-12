using System;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business.NullObjects
{
	/// <summary>
	/// Represents a <see cref="IMimeType" /> that is equivalent to null. This class is used instead of null to prevent 
	/// <see cref="NullReferenceException" /> errors if the calling code accesses a property or executes a method.
	/// </summary>
	public class NullMimeType : IMimeType
  {
    #region IMimeType Members

    public string Extension
    {
      get { return string.Empty; }
    }

    public string FullType
    {
      get { return string.Empty; }
		}

		public string MajorType
		{
			get { return string.Empty; }
		}

		public string Subtype
    {
      get { return string.Empty; }
    }

    public MimeTypeCategory TypeCategory
    {
      get { return MimeTypeCategory.Other; }
    }

    public string GetFullTypeForBrowser(Array browserIds)
    {
      return string.Empty;
    }

    #endregion


		public bool AllowAddToGallery
		{
			get { return false; }
		}
	}
}

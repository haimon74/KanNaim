using System;

namespace GalleryServerPro.Business.Interfaces
{
	/// <summary>
	/// Represents a mime type associated with a file's extension.
	/// </summary>
  public interface IMimeType
  {
		/// <summary>
		/// Gets the file extension this mime type is associated with.
		/// </summary>
		/// <value>The file extension this mime type is associated with.</value>
    string Extension { get; }

		/// <summary>
		/// Gets the full mime type. This is the <see cref="MajorType" /> concatenated with the <see cref="Subtype" />, with a '/' between them
		/// (e.g. image/jpeg, video/quicktime).
		/// </summary>
		/// <value>The full mime type.</value>
		string FullType { get; }

		/// <summary>
		/// Gets the subtype this mime type is associated with (e.g. jpeg, quicktime).
		/// </summary>
		/// <value>The subtype this mime type is associated with (e.g. jpeg, quicktime).</value>
		string Subtype { get; }

		/// <summary>
		/// Gets the major type this mime type is associated with (e.g. image, video).
		/// </summary>
		/// <value>The major type this mime type is associated with (e.g. image, video).</value>
		string MajorType { get; }

		/// <summary>
		/// Gets the type category this mime type is associated with (e.g. image, video, other).
		/// </summary>
		/// <value>The type category this mime type is associated with (e.g. image, video, other).</value>
		MimeTypeCategory TypeCategory { get; }

		/// <summary>
		/// Gets a value indicating whether objects of this MIME type can be added to Gallery Server Pro.
		/// </summary>
		/// <value><c>true</c> if objects of this MIME type can be added to Gallery Server Pro; otherwise, <c>false</c>.</value>
		bool AllowAddToGallery { get; }
    
		/// <summary>
		/// Gets the most specific MIME type that matches one of the browser ids. The MIME type will be a value that
		/// can be understood by the specified browser, and will often correspond to the <see cref="FullType" />, <see cref="Subtype" />, and 
		/// <see cref="MajorType" /> properties. But it will be different if the user specified an alternate or browser-specific 
		/// MIME type in the configuration file. 
		/// </summary>
		/// <param name="browserIds">An <see cref="System.Array"/> of browser ids for the current browser. This is a list of strings
		/// that represent the various categories of browsers the current browser belongs to. This is typically populated by
		/// calling ToArray() on the Request.Browser.Browsers property. </param>
		/// <returns>Returns a string that represents a browser-friendly MIME type. This value may be used when 
		/// generating HTML content that requires a MIME type, especially the type attribute of the object tag.</returns>
		/// <example>The MIME type for a wave file is "audio/wav". Since some browsers do not recognize
		/// "&lt;object type='audio/wav' ...", we can specify browserMimeType = "application/x-mplayer2" in the 
		/// configuration file (within the //galleryServerPro/galleryObject/mimeTypes/mimeType element).
		/// This method will then return "application/x-mplayer2", which will allow downstream code to generate HTML 
		/// like this: "&lt;object type='application/x-mplayer2' ...", which is a value the browser understands.
		/// </example>
		string GetFullTypeForBrowser(Array browserIds);
  }
}

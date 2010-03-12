using System;

namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains media object information. This class is used to pass information between the browser and the web server
	/// via AJAX callbacks.
	/// </summary>
	public class MediaObjectWebEntity
	{
		// Stats about the current media object
		/// <summary>
		/// The media object ID.
		/// </summary>
		public int Id;
		/// <summary>
		/// Specifies the zero-based index of this media object among the others in the containing album.
		/// The first media object in an album has index = 0.
		/// </summary>
		public int Index;
		/// <summary>
		/// The number of media objects in this album.
		/// </summary>
		public int NumObjectsInAlbum;
		/// <summary>
		/// The media object title.
		/// </summary>
		public string Title;
		/// <summary>
		/// The HTML fragment that renders this media object.
		/// </summary>
		public string HtmlOutput;
		/// <summary>
		/// The ECMA script fragment that renders this media object.
		/// </summary>
		public string ScriptOutput;
		/// <summary>
		/// The width, in pixels, of this media object.
		/// </summary>
		public int Width;
		/// <summary>
		/// The height, in pixels, of this media object.
		/// </summary>
		public int Height;
		/// <summary>
		/// Indicates whether a high resolution version of this image exists and is available for viewing.
		/// </summary>
		public bool HiResAvailable;
		/// <summary>
		/// Indicates whether a downloadable version of this media object exists and can be downloaded. External media objects
		/// cannot be downloaded.
		/// </summary>
		public bool IsDownloadable;

		// Stats about the previous media object
		/// <summary>
		/// The ID of the previous media object. Specify zero if the current media object is the first item in this album.
		/// </summary>
		public int PrevId;
		/// <summary>
		/// The title of the previous media object.
		/// </summary>
		public string PrevTitle;
		/// <summary>
		/// The HTML fragment that renders the previous media object.
		/// </summary>
		public string PrevHtmlOutput;
		/// <summary>
		/// The ECMA script fragment that renders the previous media object.
		/// </summary>
		public string PrevScriptOutput;
		/// <summary>
		/// The width, in pixels, of the previous media object.
		/// </summary>
		public int PrevWidth;
		/// <summary>
		/// The height, in pixels, of the previous media object.
		/// </summary>
		public int PrevHeight;
		/// <summary>
		/// Indicates whether a high resolution version of the previous media object exists and is available for viewing.
		/// </summary>
		public bool PrevHiResAvailable;
		/// <summary>
		/// Indicates whether a downloadable version of the previous media object exists and can be downloaded. External media objects
		/// cannot be downloaded.
		/// </summary>
		public bool PrevIsDownloadable;

		// Stats about the next media object
		/// <summary>
		/// The ID of the next media object. Specify zero if the current media object is the last item in this album.
		/// </summary>
		public int NextId;
		/// <summary>
		/// The title of the next media object.
		/// </summary>
		public string NextTitle;
		/// <summary>
		/// The HTML fragment that renders the next media object.
		/// </summary>
		public string NextHtmlOutput;
		/// <summary>
		/// The ECMA script fragment that renders the next media object.
		/// </summary>
		public string NextScriptOutput;
		/// <summary>
		/// The width, in pixels, of the next media object.
		/// </summary>
		public int NextWidth;
		/// <summary>
		/// The height, in pixels, of the next media object.
		/// </summary>
		public int NextHeight;
		/// <summary>
		/// Indicates whether a high resolution version of the next media object exists and is available for viewing.
		/// </summary>
		public bool NextHiResAvailable;
		/// <summary>
		/// Indicates whether a downloadable version of the next media object exists and can be downloaded. External media objects
		/// cannot be downloaded.
		/// </summary>
		public bool NextIsDownloadable;

		// Stats about the next media object in a slide show. Slide shows skip over non-image objects, so the
		// next media object in a slide show may or may not be the same as the next media object.
		/// <summary>
		/// The ID of the next media object in a slide show.
		/// </summary>
		public int NextSSId;
		/// <summary>
		/// Specifies the zero-based index of the next media object that appears in a slide show.
		/// </summary>
		public int NextSSIndex;
		/// <summary>
		/// The title of the next media object in a slide show.
		/// </summary>
		public string NextSSTitle;
		/// <summary>
		/// The URL that points directly to the next media object in a slide show. For example, for images this 
		/// URL can be assigned to the src attribute of an img tag.
		/// </summary>
		public string NextSSUrl;
		/// <summary>
		/// The HTML fragment that renders the next media object in a slide show.
		/// </summary>
		public string NextSSHtmlOutput;
		/// <summary>
		/// The ECMA script fragment that renders the next media object in a slide show.
		/// </summary>
		public string NextSSScriptOutput;
		/// <summary>
		/// The width, in pixels, of the next media object in a slide show.
		/// </summary>
		public int NextSSWidth;
		/// <summary>
		/// The height, in pixels, of the next media object in a slide show.
		/// </summary>
		public int NextSSHeight;
		/// <summary>
		/// Indicates whether a high resolution version of the next media object in the slide show exists and is available for viewing.
		/// </summary>
		public bool NextSSHiResAvailable;
		/// <summary>
		/// Indicates whether a downloadable version of the next media object in a slide show exists and can be downloaded. 
		/// External media objects cannot be downloaded, but images, which are the only media objects allowed in a slide show,
		/// are always downloadable. Therefore this property is always true, but I added it anyway for consistency in this
		/// object and to allow for the possibility of future enhancement that might modify this behavior.
		/// </summary>
		public bool NextSSIsDownloadable;
	}
}

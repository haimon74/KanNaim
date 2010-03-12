using System.Web;

namespace GalleryServerPro.Web
{
	/// <summary>
	/// Represents an object that is capable of creating URL's.
	/// </summary>
	public interface IUrlBuilder
	{
		/// <summary>
		/// Builds the URL by concatenating the current page with the <paramref name="queryString"/>. Example: If the current
		/// page is /dev/gs/gallery.aspx and the queryString is "g=album", this function returns "/dev/gs/gallery.aspx?g=album"..
		/// Any existing query string on the current page is discarded.
		///  </summary>
		/// <param name="queryString">The query string to append to the current page.</param>
		/// <returns>Returns an URL pointing to the current page with the specified <paramref name="queryString"/>.</returns>
		string BuildUrl(string queryString);
	}

	/// <summary>
	/// Represents an object that is capable of creating URL's.
	/// </summary>
	public class UrlBuilder : IUrlBuilder
	{
		/// <summary>
		/// Builds the URL by concatenating the current page with the <paramref name="queryString"/>. Example: If the current
		/// page is /dev/gs/gallery.aspx and the queryString is "g=album", this function returns "/dev/gs/gallery.aspx?g=album"..
		/// Any existing query string on the current page is discarded.
		///  </summary>
		/// <param name="queryString">The query string to append to the current page.</param>
		/// <returns>Returns an URL pointing to the current page with the specified <paramref name="queryString"/>.</returns>
		public string BuildUrl(string queryString)
		{
			return string.Concat(Util.GetCurrentPageUrl(), "?", queryString);
		}
	}
}
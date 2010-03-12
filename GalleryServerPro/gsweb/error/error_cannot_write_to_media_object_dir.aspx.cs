using System;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.error
{
	public partial class error_cannot_write_to_media_object_dir : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControls();
		}

		private void ConfigureControls()
		{
			// The path we can't write to has been passed as a query string parameter. Replicate the original exception and
			// output the message.
			string mediaObjectPath = Server.UrlDecode(WebsiteController.GetQueryStringParameterString("path"));

			CannotWriteToDirectoryException ex = new CannotWriteToDirectoryException(mediaObjectPath);

			litErrorInfo.Text = Server.HtmlEncode(ex.Message);
		}
	}
}

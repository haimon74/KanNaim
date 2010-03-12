using System;
using System.Web.UI;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.gs.pages.error
{
	public partial class generic : UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.ConfigureControls();
		}

		private void ConfigureControls()
		{
			imgGspLogo.ImageUrl = Util.GetUrl("/images/gsp_logo_313x75.png");
			hlSiteAdmin.NavigateUrl = Util.GetUrl(PageId.admin_general);
			hlHome.NavigateUrl = Util.GetCurrentPageUrl();

			// The global error handler in Gallery.cs should have, just prior to transferring to this page, placed the original
			// error in the HttpContext Items bag. If the Show Error Details setting is enabled, grab this info and display it.
			if (Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ShowErrorDetails)
			{
				pErrorDtl2.Visible = false;
				lblSeparator.Visible = false;
				hlSiteAdmin.Visible = false;

				IAppError error = System.Web.HttpContext.Current.Items["CurrentAppError"] as IAppError;
				if (error != null)
				{
					this.Page.Header.Controls.Add(new LiteralControl(error.CssStyles));

					litErrorDetails.Text = error.ToHtml();
				}
				else
				{
					litErrorDetails.Text = "<p class='gsp_msgwarning'>Error information missing from HttpContext Items bag. Please submit bug report to the <a href='http://www.galleryserverpro.com/forum/'>Gallery Server Pro forum</a>.</p>";
				}

			}
		}
	}
}
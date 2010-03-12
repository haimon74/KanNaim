using System;
using System.Web.UI.HtmlControls;

using GalleryServerPro.Business;
using System.Web.UI.WebControls;
using System.Globalization;

namespace GalleryServerPro.Web.uc
{
	public partial class footer : System.Web.UI.UserControl
	{
		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControls();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		public GspPage PageBase
		{
			get { return (GspPage) this.Page; }
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			string galleryServerVersion = WebsiteController.GetGalleryServerVersion();

			System.Web.UI.WebControls.Image logo = new System.Web.UI.WebControls.Image();
			logo.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(footer), "GalleryServerPro.Web.images.gsp_ftr_logo_170x46.png");
			logo.AlternateText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Logo_Tooltip, galleryServerVersion);
			logo.Width = new Unit(170);
			logo.Height = new Unit(46);

			HyperLink hl = new HyperLink();
			hl.NavigateUrl = "http://www.galleryserverpro.com";
			hl.ToolTip = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Logo_Tooltip, galleryServerVersion);
			
			hl.Controls.Add(logo);

			pnlFooter.Controls.Add(hl);
		}

		#endregion
	}
}
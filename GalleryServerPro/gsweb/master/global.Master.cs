using System;

namespace GalleryServerPro.Web.Master
{
	public partial class global : System.Web.UI.MasterPage
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControls();
		}
		
		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			GalleryServerPro.Configuration.Core configSection = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core;
			this.litPageTitle.Text = configSection.WebsiteTitle;
		}

		#endregion
	}
}

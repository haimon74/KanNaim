using System;

namespace GalleryServerPro.Web.anon
{
	public partial class recoverpassword : GspPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}
		}

		private void ConfigureControlsFirstTime()
		{
			this.Master.ShowLoginAndSearch = false;
		}
	}
}

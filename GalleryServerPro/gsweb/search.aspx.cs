using System;
using System.Globalization;
using System.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web
{
	public partial class search : GspPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				ConfigureControlsFirstTime();
      
			SearchGallery();
		}

		private void SearchGallery()
		{
			IGalleryObjectCollection galleryObjects = null;
			string searchText;

      if (IsPostBack)
		  {
		    searchText = txtSearch.Text.Trim();
		  }
		  else
		  {
        searchText = Server.UrlDecode(WebsiteController.GetQueryStringParameterString("search"));
		  }

			if (!String.IsNullOrEmpty(searchText))
			{
				// Search gallery and display results.
				galleryObjects = HelperFunctions.SearchGallery(searchText, this.GetGalleryServerRolesForUser(), User.Identity.IsAuthenticated);

				if (galleryObjects != null)
				{
					searchResultTitle.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Search_Results_Text, galleryObjects.Count);
					uc.thumbnailview thumbnailView = (uc.thumbnailview)Page.LoadControl("~/uc/thumbnailview.ascx");
					thumbnailView.DisplayThumbnails(galleryObjects);
					phMediaObjectContainer.Controls.Clear();
					phMediaObjectContainer.Controls.Add(thumbnailView);
				}
			}
			else // No search text found
			{
				searchResultTitle.InnerText = Resources.GalleryServerPro.Search_Instructions;
			}

		}

		private void ConfigureControlsFirstTime()
		{
			this.Master.ShowSearch = false;
		}
	}
}

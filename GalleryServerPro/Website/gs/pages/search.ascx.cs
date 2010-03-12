using System;
using System.Globalization;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class search : Pages.GalleryPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.Page.Form.DefaultButton = btnSearch.UniqueID;
			imgSearchIcon.ImageUrl = String.Concat(Util.GalleryRoot, "/images/search_48x48.png");

			SearchGallery(GetSearchText());
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
		}

		private string GetSearchText()
		{
			string searchText = String.Empty;

			if (IsNewPageLoad)
			{
				searchText = Server.UrlDecode(Util.GetQueryStringParameterString("search"));
				this.tv.CallBackData = searchText;
			}
			else if (IsCallback)
			{
				searchText = this.tv.CallBackData;
			}
			else if (IsPostBack) // Note: IsPostBack="true" even when IsCallback is true, so check this last
			{
				searchText = this.txtSearch.Text.Trim();
				this.tv.CallBackData = searchText;
			}

			return searchText;
		}

		private void SearchGallery(string searchText)
		{
			IGalleryObjectCollection galleryObjects = null;

			if (!String.IsNullOrEmpty(searchText))
			{
				// Search gallery and display results.
				galleryObjects = HelperFunctions.SearchGallery(searchText, this.GetGalleryServerRolesForUser(), Util.IsAuthenticated);

				if (galleryObjects != null)
				{
					tv.GalleryObjectsDataSource = galleryObjects;
					searchResultTitle.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Search_Results_Text, galleryObjects.Count);
				}
				else
				{
					tv.Visible = false;
				}
			}
			else // No search text found
			{
				searchResultTitle.InnerText = Resources.GalleryServerPro.Search_Instructions;
				tv.Visible = false;
			}

		}
	}
}
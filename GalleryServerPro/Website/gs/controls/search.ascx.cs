using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GalleryServerPro.Web.Controls
{
	public partial class search : GalleryUserControl
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			RegisterJavascript();
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			// Get reference to the Search textbox.
			TextBox txtSearch = (TextBox)this.GalleryPage.FindControlRecursive(dgSearch, "txtSearch");

			Util.Redirect(PageId.search, String.Format("aid={0}&search={1}", this.GalleryPage.GetAlbum().Id, Util.UrlEncode(txtSearch.Text)));
		}

		#endregion

		#region Private Methods

		private void RegisterJavascript()
		{
			if (this.GalleryPage.GalleryControl.ShowSearch)
			{
				// Get reference to the UserName textbox.
				TextBox txtSearch = (TextBox)this.GalleryPage.FindControlRecursive(dgSearch, "txtSearch");

				string script = String.Format(@"
function toggleSearch()
{{
	if (typeof(dgLogin) !== 'undefined')
		dgLogin.close();

	if (dgSearch.get_isShowing())
		dgSearch.close();
	else
		dgSearch.show();
}}

function dgSearch_OnShow()
{{
	$get('{0}').focus();
}}", txtSearch.ClientID);

				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "searchFocusScript", script, true);
			}
		}

		#endregion

	}
}
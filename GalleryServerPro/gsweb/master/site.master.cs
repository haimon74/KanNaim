using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.Master
{
	public partial class site : System.Web.UI.MasterPage
	{
		#region Private Fields


		#endregion

		#region Constructors

		public site()
		{
		}

		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControls();
		}

		protected void Login1_LoginError(object sender, EventArgs e)
		{
			// The user has entered an invalid user name and/or error. Redirect to login page and append message.
			string redirectUrl = WebsiteController.AddQueryStringParameter("~/login.aspx", String.Format(CultureInfo.CurrentCulture, "msg={0}", ((int)Message.UserNameOrPasswordIncorrect).ToString(CultureInfo.InvariantCulture)));

			Response.Redirect(redirectUrl, true);
			//Response.Redirect(redirectUrl, false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		protected void Login1_LoggedIn(object sender, EventArgs e)
		{
			RemoveRolesFromCache();

			// Reload page.
			Response.Redirect(GetRedirectUrlAfterLoginOrLogout());

			// Note: For reasons I don't quite understand we cannot use the following pattern that is used elsewhere. It has 
			// something to do with the fact that we are in a postback. When we try to use it, the page finishes the postback
			// without doing the redirect. That would be OK except we need to clear out any msg parameters in
			// the query string. The easiest way to do that is with a redirect.
			//Response.Redirect(GetRedirectUrlAfterLoginOrLogout(), false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
		{
			RemoveRolesFromCache();

			//this.PageBase.IsAnonymousUser = true;

			// Reload page.
			Response.Redirect(GetRedirectUrlAfterLoginOrLogout(), true);

			// Note: For reasons I don't quite understand we cannot use the following pattern that is used elsewhere. It has 
			// something to do with the fact that we are in a postback. When we try to use it, the page finishes the postback
			// without doing the redirect. That would be OK except we need to clear out any msg parameters in
			// the query string. The easiest way to do that is with a redirect.
			//Response.Redirect(GetRedirectUrlAfterLoginOrLogout(), false);
			//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
      Response.Redirect("~/search.aspx?search=" + Server.UrlEncode(txtSearch.Text));
    }

		#endregion

		#region Public Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		protected GspPage PageBase
		{
			get { return (GspPage)this.Page; }
		}

		public bool ShowLoginAndSearch
		{
			get { return pnlheaderloginandsearch.Visible; }
			set { pnlheaderloginandsearch.Visible = value; }
		}

		public bool ShowSearch
		{
			get { return pnlSearch.Visible; }
			set { pnlSearch.Visible = value; }
		}

		public HtmlGenericControl ContentDiv
		{
			get 
			{
				return content; 
			}
		}
	
	
		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			GalleryServerPro.Configuration.Core configSection = WebsiteController.GetGalleryServerProConfigSection().Core;
			
			if (configSection.PageHeaderTextUrl.Trim().Length > 0)
			{
				HyperLink hlHeader = new HyperLink();
				hlHeader.Text = configSection.PageHeaderText;

				string headerTextUrl = configSection.PageHeaderTextUrl.Trim();

				switch (headerTextUrl)
				{
					case "/":
						{
							// Create a link to the root of the web site.
							hlHeader.NavigateUrl = headerTextUrl;
							hlHeader.ToolTip = Resources.GalleryServerPro.Master_Site_PageHeaderTextUrlToolTipWebRoot;
							break;
						}
					case "~/":
						{
							// Create a link to the root of the web application.
							hlHeader.NavigateUrl = WebsiteController.GetAppRootUrl();
							hlHeader.ToolTip = Resources.GalleryServerPro.Master_Site_PageHeaderTextUrlToolTipAppRoot;
							break;
						}
					default:
						{
							// Craete a link to the specified URL.
							hlHeader.NavigateUrl = headerTextUrl;
							hlHeader.ToolTip = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Master_Site_PageHeaderTextUrlToolTip, headerTextUrl);
							break;
						}
				}

				phHeaderTitle.Controls.Add(hlHeader);
			}
			else
			{
				phHeaderTitle.Controls.Add(new System.Web.UI.LiteralControl(configSection.PageHeaderText));
			}
						
		}

		private void RemoveRolesFromCache()
		{
			Dictionary<string, IGalleryServerRoleCollection> rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)HelperFunctions.CacheManager.GetData(CacheItem.GalleryServerRoles.ToString());

			if ((rolesCache != null) && (this.Context.Session != null))
			{
				rolesCache.Remove(Session.SessionID);
			}
		}

		private string GetRedirectUrlAfterLoginOrLogout()
		{
			string url = Request.Url.PathAndQuery;

			// Remove any messages
			url = WebsiteController.RemoveQueryStringParameter(url, "msg");

			// If the user was viewing a media object, update the query string with its ID. (Since the user may have been 
			// navigating through objects using AJAX, the "moid" query string parm does not necessarrily represent the current
			// media object, but the property this.PageBase.MediaObjectId does.
			if (this.PageBase.MediaObjectId > int.MinValue)
			{
				int moid = this.PageBase.MediaObjectId;
				url = WebsiteController.RemoveQueryStringParameter(Request.Url.PathAndQuery, "moid");
				url = WebsiteController.AddQueryStringParameter(url, String.Format(CultureInfo.CurrentCulture, "moid={0}", moid));
			}
			return url;
		}

		#endregion

	}
}

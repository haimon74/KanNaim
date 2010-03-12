using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SideTreeMenu runat=server></{0}:SideTreeMenu>")]
    public class SideTreeMenu : TreeView
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return (s ?? String.Empty);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            base.RenderContents(output);
            //output.Write(Text);
        }


        private int _menuId;
        private string _rootPath;
        private string _siblingPath;
        private string _siblingFileName;
        private bool _createSitemap = true;
        private SiteMapDataSource _siteMapDataSource1;

        [DefaultValue("1")]
        public int MenuID
        {
            get { return _menuId; }
            set {   _menuId = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            int enabled = 0;
            if (Context.Request.QueryString["en"] != null)
            {
                int.TryParse(Context.Request.QueryString["en"], out enabled);
            }

            this.Visible = true;//(enabled == 1);

            if (Page.IsPostBack)
                return;

            Page.Session["SideMenuId"] = MenuID;

            if (_createSitemap)
            {
                _rootPath = Context.Server.MapPath(String.Format("~/Sitemaps/RightSideMenu{0}.sitemap", MenuID));
                _siblingFileName = String.Format("~/Sitemaps/RightSideMenu{0}SiblingsOf", MenuID);
                _siblingPath = Context.Server.MapPath(_siblingFileName);

                XmlSiteMapGenerator.GenerateSiteMapFiles(MenuID, _rootPath, _siblingPath, _siblingFileName);
            }
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

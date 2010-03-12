using System;
using UserControlsLibrary;

public partial class SiteMap : System.Web.UI.UserControl
{
    private int _menuId;
    private string _rootPath;
    private string _siblingPath;
    private string _siblingFileName;
    private static bool _createSitemap = true;

    public int MenuID
    {
        get { return _menuId; }
        set { _menuId = value; }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        _rootPath = Server.MapPath(String.Format("~/Sitemaps/SiteMapPath{0}.sitemap", MenuID));
        _siblingFileName = String.Format("~/Sitemaps/SiteMapPath{0}SiblingsOf", MenuID);
        _siblingPath = Server.MapPath(_siblingFileName);

        if (_createSitemap)
            XmlSiteMapGenerator.GenerateSiteMapFiles(MenuID, _rootPath, _siblingPath, _siblingFileName);

        SiteMapPath1.SiteMapProvider = String.Format("SiteMapPath{0}", MenuID);
    }
}

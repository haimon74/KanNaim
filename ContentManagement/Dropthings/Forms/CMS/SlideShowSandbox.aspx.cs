using System;

public partial class Forms_CMS_SlideShowSandbox : System.Web.UI.Page
{
    static private string _path;
    static private string _contextKey = "../../images/";

    public static string SlidesPath
    {
        get { return _path; }
        set { _path = value; }
    }

    public static string ContextKey
    {
        get { return _contextKey; }
        set { _contextKey = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SlidesPath = Server.MapPath(ContextKey);
    }
}

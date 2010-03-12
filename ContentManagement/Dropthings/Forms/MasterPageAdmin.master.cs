using System;
using UserControlsLibrary;

public partial class MasterPageDemo : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "Theme1";
    }

}

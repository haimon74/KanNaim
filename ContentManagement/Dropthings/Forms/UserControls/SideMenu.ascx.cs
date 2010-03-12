using System;
using UserControlsLibrary;


public partial class SideMenu : System.Web.UI.UserControl
{
    private int _menuId;
    
    public int MenuID
    {
        get { return _menuId; }
        set { _menuId = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SideTreeMenu1.MenuID = MenuID;
    }
}

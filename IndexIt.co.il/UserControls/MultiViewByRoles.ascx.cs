using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_MultiViewByRoles : System.Web.UI.UserControl
{
    public enum ViewEnum
    {
        User = 0,
        ShopOwner = 1,
        Admin = 2
    } ;
    public ViewEnum View
    {
        set { MultiView1.ActiveViewIndex = (int) value; }
        get { return (ViewEnum) MultiView1.ActiveViewIndex; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}

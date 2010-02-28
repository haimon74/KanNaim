using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginRolesTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        CreateUserWizard1.Visible = true;
        ChangePassword1.Visible = false;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ChangePassword1.Visible = true;
        CreateUserWizard1.Visible = false;
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Server.Transfer("~/LoginRolesTest.aspx?en=1");
    }
}

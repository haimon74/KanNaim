using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_SandboxForm_InputDate : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (TextBox1.Text == "")
        {
            DateTime now = DateTime.Now;
            TextBox1.Text = String.Format("{0}/{1}/{2}", now.Day, now.Month, now.Year);
        }
    }
}

using System;

public partial class UserControls_SandboxForm_InputPasswordX2 : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TextBox1.Text = InputRegExprPassword1.Text;
        TextBox2.Text = InputRegExprPassword2.Text;
    }
}

using System;

public partial class UserControls_StyleManagementForm_InputLeftOrRight : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SelectedValue
    {
        get { return RadioButtonListLeftRight.SelectedValue; }
    }
}

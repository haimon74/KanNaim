using System;

public partial class UserControls_StyleManagementForm_InputFontSize : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SelectedValue
    {
        get { return DropDownListFontSize.SelectedValue; }
    }
}

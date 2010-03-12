using System;

public partial class UserControls_StyleManagementForm_InputFontFamily : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string  SelectedValue
    {
        get { return FontFamilyDropDownList1.SelectedValue; }
    }
}

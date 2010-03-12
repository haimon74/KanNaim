using System;

public partial class UserControls_StyleManagementForm_InputPositionType : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SelectedValue
    {
        get { return PositionDropDownList1.SelectedValue; }
    }
}

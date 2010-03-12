using System;

public partial class UserControls_StyleManagementForm_InputLetterSpacing : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SelectedValue
    {
        get { return LetterSpacingDropDownList1.SelectedValue;}
    }
}

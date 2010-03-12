using System;

public partial class UserControls_StyleManagementForm_InputAlignment : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public string BgImageVAlignSelectedValue 
    {
        get { return DropDownListBgImageVAlign.SelectedValue; }
    }
    public string BgImageHAlignSelectedValue
    {
        get { return DropDownListBgImageHAlign.SelectedValue; }
    }
}

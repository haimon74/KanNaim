using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_StyleManagementForm_InputBackgroundColor : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //FileUpload1.Enabled = _enable;
        //TextBoxBgImageHeight.Enabled = _enable;
        //TextBoxBgImageWidth.Enabled = _enable;
        //DropDownListBgImageHAlign.Enabled = _enable;
        //DropDownListBgImageVAlign.Enabled = _enable;
    }

    private bool _enable;
    public bool Enable
    {
        get { return _enable; }
        set { _enable = value; }
    }

    //protected void SetBackgroundColor(object sender, EventArgs e)
    //{
    //    TextBoxBackgroundColor.Style.Add("background-color", TextBoxBackgroundColor_ColorPickerExtender.SelectedColor);
    //}
}

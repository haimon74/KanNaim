using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CMS_StyleManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["BackGroundImageEnabled"] = false;
        }
        EnableBackgroundImage((bool)ViewState["BackGroundImageEnabled"]);
    }
    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["BackGroundImageEnabled"] = (RadioButtonList2.SelectedIndex == 1);
        EnableBackgroundImage((bool)ViewState["BackGroundImageEnabled"]);
    }

    private void EnableBackgroundImage(bool enable)
    {
        //InputBackgroundImage1.Enable = enable;
    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        ButtonSave.Text = "clicked";
    }
}

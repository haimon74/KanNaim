using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_StyleManagementForm_InputColorPicker : System.Web.UI.UserControl
{
    private string _labelText = "";

    public string LabelText
    {
        get { return _labelText; }
        set { _labelText = value; }
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        Label3.Text = LabelText;
    }
}

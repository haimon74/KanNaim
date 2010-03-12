using System;

public partial class UserControls_SandboxForm_InputName : System.Web.UI.UserControl
{
    private string _displayText = "שם";

    public string DisplayText
    {
        get { return _displayText; }
        set { _displayText = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InputRegExprFullName.LabelText = DisplayText;
    }
}

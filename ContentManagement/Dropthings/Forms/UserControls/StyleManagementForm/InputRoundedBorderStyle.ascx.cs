using System;
using System.Web.UI.WebControls;

public partial class Forms_UserControls_StyleManagementForm_InputRoundedBorderStyle : System.Web.UI.UserControl
{

    public string SelectedStyle
    {
        get { return PanelPreviewSelectedBorder.Attributes["style"]; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void IndexChanged(object sender, EventArgs e)
    {
        string name = RadioButtonListRoundedBorders.SelectedValue;

        foreach (var control in Controls)
        {
            if (control is Panel)
            {
                if (((Panel)control).ID.EndsWith(name))
                    PanelPreviewSelectedBorder.Attributes["style"] = ((Panel)control).Attributes["style"];
            }
        }
        
    }
}

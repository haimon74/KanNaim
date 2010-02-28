using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:IsBoldFontCheckBox runat=server></{0}:IsBoldFontCheckBox>")]
    public class IsBoldFontCheckBox : CheckBox
    {
        protected override void OnLoad(System.EventArgs e)
        {
            Text = "פונט מודגש";
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

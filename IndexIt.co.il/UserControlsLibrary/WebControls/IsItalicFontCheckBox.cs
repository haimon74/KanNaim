using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:IsItalicFontCheckBox runat=server></{0}:IsItalicFontCheckBox >")]
    public class IsItalicFontCheckBox : CheckBox
    {
        protected override void OnLoad(System.EventArgs e)
        {
            Text = "פונט אלכסוני";
            Font.Italic = true;
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

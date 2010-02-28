using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:VerticalAlignmentDropDownList runat=server></{0}:VerticalAlignmentDropDownList>")]
    public class VerticalAlignmentDropDownList : DropDownList
    {
        private void AddItem(string textValue)
        {
            this.Items.Add(new ListItem(textValue));
        }
        private void AddItem(string text, string value)
        {
            this.Items.Add(new ListItem(text,value));
        }
        private void AddItem(string text, string value, bool enable)
        {
            this.Items.Add(new ListItem(text, value, enable));
        }

        public VerticalAlignmentDropDownList()
        {
            AddItem("למעלה", "top");
            AddItem("מרכז", "center");
            AddItem("למטה", "bottom");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            SelectedIndex = 0;
            base.OnLoad(e);
        }
    }
}

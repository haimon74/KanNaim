using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:HorizontalAlignmentDropDownList runat=server></{0}:HorizontalAlignmentDropDownList>")]
    public class HorizontalAlignmentDropDownList : DropDownList
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

        public HorizontalAlignmentDropDownList()
        {
            AddItem("ימינה", "right");
            AddItem("מרכז", "center");
            AddItem("שמאלה", "right");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            SelectedIndex = 0;
            base.OnLoad(e);
        }
    }
}

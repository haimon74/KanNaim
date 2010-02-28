using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:FloatDropDownList ID=\"FloatDropDownList\"runat=\"server\"></{0}:FloatDropDownList>")]
    public class FloatDropDownList: DropDownList
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

        public FloatDropDownList()
        {
            AddItem("אוביקט מימין", "float:right;");
            AddItem("אוביקט משמאל", "float:left;");
            AddItem("אוביקטים מימין ומשמאל", "float:both;");
            AddItem("ללא אוביקט מימין", "clear:right;");
            AddItem("ללא אוביקט משמאל", "clear:left;");
            AddItem("ללא אוביקטים מימין ומשמאל", "clear:both;");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            SelectedIndex = 0;
            base.OnLoad(e);
        }
    }
}

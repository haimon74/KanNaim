using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:TextDirectionDropDownList runat=server></{0}:TextDirectionDropDownList>")]
    public class TextDirectionDropDownList : DropDownList
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

        public TextDirectionDropDownList()
        {
            AddItem("מימין לשמאל", "rtl");
            AddItem("משמאל לימין", "ltr");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            //SelectedIndex = 0;
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

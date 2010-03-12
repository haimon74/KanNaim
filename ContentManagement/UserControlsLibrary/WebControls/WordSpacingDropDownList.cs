using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:WordSpacingDropDownList ID=\"WordSpacingDropDownList\" runat=\"server\"></{0}:WordSpacingDropDownList>")]
    public class WordSpacingDropDownList : DropDownList
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

        public WordSpacingDropDownList()
        {
            AddItem("0px", "normal");
            AddItem("-3px");
            AddItem("-2px");
            AddItem("-1px");
            AddItem("1px");
            AddItem("2px");
            AddItem("3px");
            AddItem("4px");
            AddItem("5px");
            AddItem("6px");
            AddItem("7px");
            AddItem("8px");
            AddItem("9px");
            AddItem("10px");
            AddItem("15px");
            AddItem("20px");
            AddItem("30px");
            
        }
        protected override void OnLoad(System.EventArgs e)
        {
            //SelectedIndex = 0; //normal
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

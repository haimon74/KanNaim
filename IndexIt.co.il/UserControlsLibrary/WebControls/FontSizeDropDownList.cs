using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:FontSizeDropDownList runat=server></{0}:FontSizeDropDownList>")]
    public class FontSizeDropDownList : DropDownList
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

        public  FontSizeDropDownList()
        {
            AddItem("xx-small");
            AddItem("x-small");
            AddItem("smaller");
            AddItem("small");
            AddItem("medium");
            AddItem("large");
            AddItem("larger");
            AddItem("x-large");
            AddItem("xx-large");
            AddItem("6pt");
            AddItem("7pt");
            AddItem("8pt");
            AddItem("9pt");
            AddItem("10pt");
            AddItem("11pt");
            AddItem("12pt");
            AddItem("13pt");
            AddItem("14pt");
            AddItem("16pt");
            AddItem("18pt");
            AddItem("20pt");
            AddItem("24pt");
            AddItem("30pt");
            AddItem("36pt");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            SelectedIndex = 3; //small size
            base.OnLoad(e);
        }
    }
}

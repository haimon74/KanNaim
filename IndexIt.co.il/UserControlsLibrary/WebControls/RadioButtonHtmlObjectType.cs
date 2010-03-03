using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:RadioButtonHtmlObjectType ID=\"RadioButtonHtmlObjectType\"runat=\"server\"></{0}:RadioButtonHtmlObjectType>")]
    public class RadioButtonHtmlObjectType: RadioButtonList
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

        public RadioButtonHtmlObjectType()
        {
            AddItem("טקסט", "p");
            AddItem("לינק", "a");
            AddItem("תמונה", "img");
            AddItem("אובייקט אחר מושתל", "embed");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            RepeatDirection = RepeatDirection.Horizontal;
            SelectedIndex = 0;
            base.OnLoad(e);
        }
    }
}

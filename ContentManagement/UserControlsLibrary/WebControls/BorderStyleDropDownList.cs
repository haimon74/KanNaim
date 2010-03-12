using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:BorderStyleDropDownList ID=\"BorderStyleDropDownList\"runat=\"server\"></{0}:BorderStyleDropDownList>")]
    public class BorderStyleDropDownList: DropDownList
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

        public BorderStyleDropDownList()
        {
            AddItem("ללא", "none");
            AddItem("מוסתר", "hidden");
            AddItem("נקודות","dotted");
            AddItem("מקווקו", "dashed");
            AddItem("קו יחיד", "solid");
            AddItem("כפול", "double");
            AddItem("חריץ", "groove");
            AddItem("תבליט", "ridge");
            AddItem("שקוע", "inset");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            //SelectedIndex = 4; //solid 
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

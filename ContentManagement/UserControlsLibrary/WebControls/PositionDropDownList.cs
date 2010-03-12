using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:PositionDropDownList ID=\"PositionDropDownList\"runat=\"server\"></{0}:PositionDropDownList>")]
    public class PositionDropDownList: DropDownList
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

        public PositionDropDownList()
        {
            AddItem("מוחלט", "absolute");
            AddItem("קבוע", "fixed");
            AddItem("יחסי", "relative");
            AddItem("סטטי", "static");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            //SelectedIndex = 0; // absolute position
            EnableViewState = true;
            ToolTip = "מוחלט = במיקום (x,y) יחסי לאובייקט המכיל אותו\r\n" +
                      "קבוע = במיקום (x,y) ביחס לחלון הכללי\r\n" +
                      "יחסי = בהזזה מהאובייקט הקרוב אליו\r\n" +
                      "סטטי = תמיד במיקום קבוע בחלון הכללי ";

            base.OnLoad(e);
        }
    }
}

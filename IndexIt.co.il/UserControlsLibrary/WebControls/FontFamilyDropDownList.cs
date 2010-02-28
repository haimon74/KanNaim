using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Language")]
    [ToolboxData("<{0}:FontFamilyDropDownList runat=server></{0}:FontFamilyDropDownList>")]
    public class FontFamilyDropDownList : DropDownList
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

        public FontFamilyDropDownList()
        {
            AddItem("Aharoni");
            AddItem("Arial");
            AddItem("Courier New");
            AddItem("Microsoft Sans Serif");
            AddItem("Miriam");
            AddItem("Tahoma");
            AddItem("Verdana");

            if (_fontLanguage == FontLanguageEnum.English)
            {
                AddItem("Book Antiqua");
                AddItem("Brush Script MT");
                AddItem("Comic Sans MS");
                AddItem("David");
                AddItem("Georgia");
                AddItem("Lucida Console");
                AddItem("Times New Roman");
            }
        }
        protected override void OnLoad(System.EventArgs e)
        {
            SelectedIndex = 1;
            base.OnLoad(e);
        }
        public enum FontLanguageEnum
        {
            Hebrew,
            English
        } ;

        private FontLanguageEnum _fontLanguage;

        [Bindable(false)]
        [ThemeableAttribute(false)]
        public FontLanguageEnum Language
        {
            get
            {
                return _fontLanguage;
            }

            set
            {
                _fontLanguage = value;
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ListBoxInt32 runat=server></{0}:ListBoxInt32>")]
    public class ListBoxInt32 : ListBox
    {
        [Bindable(true)]
        [ThemeableAttribute(false)]
        public Int32 IntValue
        {
            get
            {
                Int32 i = (Int32)(ViewState["IntValue"] ?? -1);
                return i;
            }

            set
            {
                ViewState["IntValue"] = value;
            }
        }
    }
}

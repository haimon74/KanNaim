using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SessionLogin runat=server></{0}:SessionLogin>")]
    public class SessionLogin : Login
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return (s ?? String.Empty);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        
        }
}

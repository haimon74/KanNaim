using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:PollOptionsSandbox runat=server></{0}:PollOptionsSandbox>")]
    [ParseChildren(true, "Options")]
    public class PollOptionsSandbox : Control, INamingContainer
    {
        private ArrayList options = new ArrayList();

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ArrayList Options
        {
            get { return options; }
        }
    }


    public class Option
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:LabelToSlideToolTip runat=server></{0}:LabelToSlideToolTip>")]
    public class LabelToSlideToolTip : Label
    {
        private LabelToSlideToolTip()
        {
            
        }

        private ImageButton _image;

        public LabelToSlideToolTip(ref ImageButton image)
        {
            _image = image;
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                _image.ToolTip = s;
                _image.AlternateText = s;
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
                _image.ToolTip = value;
                _image.AlternateText = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            base.OnLoad(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(Text);
        }
    }
}

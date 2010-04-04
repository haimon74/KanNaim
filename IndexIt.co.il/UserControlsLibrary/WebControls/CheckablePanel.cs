using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:CheckablePanel runat=server></{0}:CheckablePanel>")]
    public class CheckablePanel : Panel
    {
        private PlaceHolder _placeHolder = new PlaceHolder();
        private UpdatePanel _updatePanel = new UpdatePanel();
        
        protected override void OnLoad(EventArgs e)
        {
            _updatePanel.ID = this.ID + "UpdatePanel";
            _placeHolder.ID = this.ID + "PlaceHolder";
            _updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
            this.Controls.Add(_updatePanel);
            EnableViewState = true;
            base.OnLoad(e);
        }

        public void Update(bool toEnable)
        {
            Enabled = toEnable;
            
            var en = Controls.GetEnumerator();
            var cc = new Collection<string>();
            int i = 0;
            while (en.MoveNext())
            {
                if (en.Current is WebControl)
                    cc.Add(((WebControl)en.Current).ID);
            }
            foreach (string id in cc)
            {
                WebControl control = ((WebControl) FindControl(id));
                Controls.Remove(control);
                _updatePanel.ContentTemplateContainer.Controls.Add(control);
                control.BackColor = (Enabled) ? Color.Empty : Color.Gray;
            }
            _updatePanel.Update();
        }
    }
}

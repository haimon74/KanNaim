using System;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:EnableControlCheckBox runat=server></{0}:EnableControlCheckBox>")]
    public class EnableControlCheckBox : CheckBox
    {
        private CheckablePanel _panel;
        private string _panelName = "";
        private bool _isEnabled = false;
        private bool _isVisible = true;
        private bool _isLoaded = false;
        
        protected override void OnLoad(System.EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                Text = "סמן לאפשר עריכה";
            AutoPostBack = true;
            CheckedChanged += PrivateOnCheckedChanged;
            BindedPanel = _panelName;
            UpdatePanel();
            EnableViewState = true;
            _isLoaded = true;
            base.OnLoad(e);
        }

        private void PrivateOnCheckedChanged(object sender, EventArgs e)
        {
            BindedPanel = _panelName;
            UpdatePanel();
        }

        private void UpdatePanel()
        {
            if ((_panel != null) && (_isLoaded))
            {
                _panel.Visible = _isVisible;
                _panel.Update(this.Checked);
            }
        }

        public string BindedPanel
        {
            set
            {
                _panelName = value;
                
                if ((this.Parent == null) || (this.Parent.Controls == null))
                    return;

                //ControlCollection cc = Parent.Controls;
                try
                {
                    object control = Parent.FindControl(value);
                    if (control is CheckablePanel)
                        _panel = (CheckablePanel)control;
                }
                catch (Exception exception)
                {
                    _panel = null;
                }
            }
        }
        public bool BindedVisible
        {
            set
            {
                _isVisible = value;
            }
        }
    }
}

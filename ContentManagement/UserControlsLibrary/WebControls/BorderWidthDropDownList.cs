﻿using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [ToolboxData("<{0}:BorderWidthDropDownList ID=\"BorderWidthDropDownList\"runat=\"server\"></{0}:BorderWidthDropDownList>")]
    public class BorderWidthDropDownList: DropDownList
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

        public BorderWidthDropDownList()
        {
            AddItem("0", "collapse");
            AddItem("1px");
            AddItem("2px");
            AddItem("3px");
            AddItem("4px");
            AddItem("5px");
            AddItem("6px");
            AddItem("7px");
            AddItem("8px");
            AddItem("9px");
            AddItem("10px");
        }
        protected override void OnLoad(System.EventArgs e)
        {
            //SelectedIndex = 0; //without borders
            EnableViewState = true;
            base.OnLoad(e);
        }
    }
}

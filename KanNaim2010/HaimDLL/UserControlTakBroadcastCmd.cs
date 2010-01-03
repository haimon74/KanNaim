using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HaimDLL
{
    public partial class UserControlTakBroadcastCmd : UserControl
    {
        public UserControlTakBroadcastCmd()
        {
            InitializeComponent();
        }

        private void checkBoxTakStart_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = this.checkBoxTakStart.Checked;
            
            this.datePickerTakStart.Enabled = isEnabled;
            this.timePickerTakStart.Enabled = isEnabled;

            isEnabled |= this.checkBoxTakRecursive.Checked;

            this.checkBoxTakBroadcastAllCatSelector.Enabled = isEnabled;
            this.treeViewTakBroadcastCatSelector.Enabled = isEnabled;
        }

        private void checkBoxTakRecursive_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = this.checkBoxTakRecursive.Checked;

            this.groupBoxRecursiveCmd.Enabled = isEnabled;

            isEnabled |= this.checkBoxTakStart.Checked;

            this.checkBoxTakBroadcastAllCatSelector.Enabled = isEnabled;
            this.treeViewTakBroadcastCatSelector.Enabled = isEnabled;
        }
    }
}

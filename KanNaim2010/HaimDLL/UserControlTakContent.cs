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
    public partial class UserControlTakContent : UserControl
    {
        public UserControlTakContent()
        {
            InitializeComponent();
        }

        private void checkBoxTakPhoto_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBoxTakPhoto.Enabled = this.checkBoxTakPhoto.Checked;
        }

        private void checkBoxEnableContent_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTakContent.Enabled = !textBoxTakContent.Enabled;
        }
    }
}

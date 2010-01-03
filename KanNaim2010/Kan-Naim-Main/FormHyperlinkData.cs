using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormHyperlinkData : Form
    {
        private int _collectionNum = 0;
        private int _anchorNum = 0;

        public FormHyperlinkData(int colNum, int aNum)
        {
            _collectionNum = colNum;
            _anchorNum = aNum;
            InitializeComponent();
        }

        private void FormHyperlinkData_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // valitation
            bool validated = ((textBoxHref.Text != String.Empty) && (textBoxUrlName.Text != String.Empty));
            if (validated)
            {
                // if validated update anchor
                string anchorTag = String.Format("<a{0}>", _anchorNum);
                string target = "";
                if (checkBoxTarget.Checked)
                    target = " target='_self'";
                FormEditArtical.HyperlinksCollection[_collectionNum][anchorTag] =
                    String.Format("<a id='{0}' href='{1}' name='{2}'{3}>", anchorTag, textBoxHref.Text, textBoxUrlName.Text, target);
                this.Close();
            }
            else
            {
                MessageBox.Show("עדיין לא מולאו כל השדות", "לא הושלם");
            }
        }

        private void buttonDelUrl_Click(object sender, EventArgs e)
        {
            string anchorTag = String.Format("<a{0}>", _anchorNum);
            FormEditArtical.HyperlinksCollection[_collectionNum][anchorTag] = "";                    
            this.Close();
        }
    }
}

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
    public partial class FormManageBottomPageLinks : Form
    {
        public FormManageBottomPageLinks()
        {
            InitializeComponent();
        }

        private void FormManageBottomPageLinks_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSet.Table_LinksPageBottom' table. You can move, or remove it, as needed.
            this.table_LinksPageBottomTableAdapter.Fill(this._10infoDataSet.Table_LinksPageBottom);

        }
    }
}

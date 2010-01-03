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
    public partial class FormManagePreferedLinks : Form
    {
        public FormManagePreferedLinks()
        {
            InitializeComponent();
        }

        private void FormManagePreferedLinks_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetPreferedLinks.DataTablePreferedLinks' table. You can move, or remove it, as needed.
            this.dataTablePreferedLinksTableAdapter.FillPreferedLinks(this._10infoDataSetPreferedLinks.DataTablePreferedLinks);
            // TODO: This line of code loads data into the '_10infoDataSetPreferedLinks.DataTablePreferedLinks' table. You can move, or remove it, as needed.
            this.dataTablePreferedLinksTableAdapter.FillPreferedLinks(this._10infoDataSetPreferedLinks.DataTablePreferedLinks);
            // TODO: This line of code loads data into the '_10infoDataSetPreferedLinks.Table_LinksPrefered' table. You can move, or remove it, as needed.
            //this.table_LinksPreferedTableAdapter.Fill(this._10infoDataSetPreferedLinks.Table_LinksPrefered);

        }

        private void infoDataSetPreferedLinksBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}

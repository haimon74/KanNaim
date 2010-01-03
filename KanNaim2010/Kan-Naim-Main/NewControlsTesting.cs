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
    public partial class NewControlsTesting : Form
    {
        public NewControlsTesting()
        {
            InitializeComponent();
        }

        private void NewControlsTesting_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_Kan_NaimDataSetCategories.Table_LookupCategories' table. You can move, or remove it, as needed.
            this.table_LookupCategoriesTableAdapter.Fill(this._Kan_NaimDataSetCategories.Table_LookupCategories);
            // TODO: This line of code loads data into the '_Kan_NaimDataSet1.Table_PhotosArchive' table. You can move, or remove it, as needed.
            this.table_PhotosArchiveTableAdapter.Fill(this._Kan_NaimDataSet1.Table_PhotosArchive);

            this.Update();
        }
    }
}

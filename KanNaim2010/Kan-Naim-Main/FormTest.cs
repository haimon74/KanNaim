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
    public partial class FormTest : Form
    {
        
        public FormTest(string cat)
        {
            InitializeComponent();

            this.table_LookupCategoriesTableAdapter.Fill(this._Kan_NaimDataSetCategories.Table_LookupCategories);

            this.comboBox1.SelectedIndex = this.comboBox1.FindString(cat);

        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            
        }
    }
}

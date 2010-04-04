using System;
using System.Data;
using System.Windows.Forms;
using Kan_Naim_Main.DataAccess.DataSet.dsLookupTableAdapters;

namespace Kan_Naim_Main
{
    public partial class Sandbox : Form
    {
        public Sandbox()
        {
            InitializeComponent();
            this.customDataGridView1.Columns.Add(customDataGridView1.CatIdColumn);
        }
        //CustomDataGridView grid = new CustomDataGridView();
        private void Sandbox_Load(object sender, EventArgs e)
        {
            //foreach (ICloneable column in this.customDataGridView1.Columns)
            //{
                
            //}
            //this.customDataGridView1.Columns.Add(customDataGridView1.CatIdColumn);

            //this.customDataGridView1.Columns.Add(new IdDataGridViewColumn());
            var  myDataSet = new DataAccess.DataSet.dsLookup ();
            var adapter = new Table_LookupCategoriesTableAdapter();
            adapter.Fill(myDataSet.Table_LookupCategories);
            this.customDataGridView1.DataMember = "Table_LookupCategories";
            this.customDataGridView1.DataSource = myDataSet;
        }
    }
}

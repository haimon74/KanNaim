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
    public partial class FormManageIndex : Form
    {
        public FormManageIndex(int indexTypeId)
        {
            InitializeComponent();
            typeIdToolStripTextBox.Text = indexTypeId.ToString();
        }

        private void FormManageIndex_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetLookupIndex.Table_LookupIndexes' table. You can move, or remove it, as needed.
            this.table_LookupIndexesTableAdapter.Fill(this._10infoDataSetLookupIndex.Table_LookupIndexes);
            // TODO: This line of code loads data into the '_10infoDataSetIndexes.Table_Indexes' table. You can move, or remove it, as needed.
            this.table_IndexesTableAdapter.Fill(this._10infoDataSetIndexes.Table_Indexes);

        }

        private void fillByIndexTypeToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_IndexesTableAdapter.FillByIndexType(this._10infoDataSetIndexes.Table_Indexes, ((int)(System.Convert.ChangeType(typeIdToolStripTextBox.Text, typeof(int)))));
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

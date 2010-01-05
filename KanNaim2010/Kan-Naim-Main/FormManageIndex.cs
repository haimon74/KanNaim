using System;
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
            table_LookupIndexesTableAdapter.Fill(_10infoDataSetLookupIndex.Table_LookupIndexes);
            // TODO: This line of code loads data into the '_10infoDataSetIndexes.Table_Indexes' table. You can move, or remove it, as needed.
            table_IndexesTableAdapter.Fill(_10infoDataSetIndexes.Table_Indexes);

        }

        private void fillByIndexTypeToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                table_IndexesTableAdapter.FillByIndexType(_10infoDataSetIndexes.Table_Indexes, ((int)(Convert.ChangeType(typeIdToolStripTextBox.Text, typeof(int)))));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                row.Cells[8].Value = int.Parse(typeIdToolStripTextBox.Text);
                object value = row.Cells[1].Value;
                string catIdStr = value.ToString();
                row.Cells[9].Value = int.Parse(catIdStr);
            }
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            //MessageBox.Show("You chose button cell on row " + e.RowIndex + " column number " + e.ColumnIndex); 

            //object value = row.Cells[1].Value;

            //if (value != null)
            //{
            //    string catIdStr = value.ToString();
            //    int indexTypeId = int.Parse(typeIdToolStripTextBox.Text);
            //    int catIdInt = int.Parse(catIdStr);

                try
                {
                    table_IndexesTableAdapter.Insert(
                        row.Cells[0].Value.ToString(),
                        row.Cells[2].Value.ToString(),
                        row.Cells[3].Value.ToString(),
                        row.Cells[4].Value.ToString(),
                        row.Cells[5].Value.ToString(),
                        row.Cells[6].Value.ToString(),
                        row.Cells[7].Value.ToString(),
                        (int) row.Cells[8].Value,
                        (int) row.Cells[9].Value);
                        //indexTypeId,
                        //catIdInt);

                    table_IndexesTableAdapter.Fill(_10infoDataSetIndexes.Table_Indexes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            //}
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //MessageBox.Show("Row Removed " + e.RowIndex);
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //DataRow = dataGridView1.CurrentRow
            string name = dataGridView1.Columns[0].Name;
            table_IndexesTableAdapter.Delete(name);
            
            //MessageBox.Show("Row Removed " + e.Row.Index + " | " + result);
            table_IndexesTableAdapter.Fill(_10infoDataSetIndexes.Table_Indexes);
        }

        //private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (row.IsNewRow)
            {
                //row.Cells[0].Value = dataGridView1.Columns[0].Name + e.RowIndex;

                for (int i = 2; i < 8; i++)
                {
                    //row.Cells[i].Value = "testing";
                }
            }
        }

        
    }
}

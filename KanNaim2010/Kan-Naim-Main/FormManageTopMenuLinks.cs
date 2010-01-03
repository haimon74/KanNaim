using System;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormManageTopMenuLinks : Form
    {
        public FormManageTopMenuLinks()
        {
            InitializeComponent();
        }

        private void FormManageTopMenuLinks_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetMenuTop.Table_MenuTop' table. You can move, or remove it, as needed.
            table_MenuTopTableAdapter.Fill(_10infoDataSetMenuTop.Table_MenuTop);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

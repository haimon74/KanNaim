using System;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormManageRightSideMenuLinks : Form
    {
        public FormManageRightSideMenuLinks()
        {
            InitializeComponent();
        }

        private void FormManageRightSideMenuLinks_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetMenuRightSide.Table_MenuRightSide' table. You can move, or remove it, as needed.
            table_MenuRightSideTableAdapter.Fill(_10infoDataSetMenuRightSide.Table_MenuRightSide);
            // TODO: This line of code loads data into the '_10infoDataSetMenuTop.Table_MenuTop' table. You can move, or remove it, as needed.
            this.table_MenuRightSideTableAdapter.Fill(this._10infoDataSetMenuRightSide.Table_MenuRightSide);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

using System;
using System.Drawing;
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
            PopulateTable();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void PopulateTable()
        {
            table_MenuRightSideTableAdapter.Fill(_10infoDataSetMenuRightSide.Table_MenuRightSide);
        }

        private static void OpenEditForm(int id)
        {
            var form = new Form
            {
                Width = 450,
                Height = 300,
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                MaximizeBox = false,
                MinimizeBox = false,
                MinimumSize = new Size(450, 300),
                Name = "הזנת פרטי לינק בתפריט צד ימין",
            };
            var userControl = new HaimDLL.UserControlEditBottomPageLink(id);
            userControl.Dock = DockStyle.Fill;
            form.Controls.Add(userControl);
            form.Show();

        }
        private void ToolStripMenuItemAddNewRecord_Click(object sender, EventArgs e)
        {
            OpenEditForm(-1);
        }

        private void ToolStripMenuItemEditRecord_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[5].Value;
            OpenEditForm(id);
        }

        private void ToolStripMenuItemDeleteSelected_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[5].Value;
            table_MenuRightSideTableAdapter.Delete(id);
            PopulateTable();
        }

        private void ToolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            PopulateTable();
        }
    }
}

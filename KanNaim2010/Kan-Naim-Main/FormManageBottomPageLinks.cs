using System;
using System.Drawing;
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
            PopulateTable();
        }

        private void PopulateTable()
        {
            table_LinksPageBottomTableAdapter.Fill(_10infoDataSetPageBottomLinks.Table_LinksPageBottom);
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
                               Name = "הזנת פרטי לינק בתחתית העמוד",
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
            int id = (int) dataGridView1.CurrentRow.Cells[6].Value;
            OpenEditForm(id);
        }

        private void ToolStripMenuItemDeleteSelected_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[6].Value;
            table_LinksPageBottomTableAdapter.Delete(id);
            PopulateTable();
        }

        private void ToolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            PopulateTable();
        }
    }
}

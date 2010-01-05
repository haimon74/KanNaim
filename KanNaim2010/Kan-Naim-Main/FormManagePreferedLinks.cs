using System;
using System.Drawing;
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
            PopulateTable();

        }

        private void PopulateTable()
        {
            dataTablePreferedLinksTableAdapter.FillPreferedLinks(_10infoDataSetPreferedLinks.DataTablePreferedLinks);
        }

        private static void OpenEditForm(int id)
        {
            var form = new Form
            {
                Width = 500,
                Height = 300,
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                MaximizeBox = false,
                MinimizeBox = false,
                MinimumSize = new Size(500, 300),
                Name = "הזנת פרטי לינקים מועדפים",
            };
            var userControl = new HaimDLL.UserControlEditPreferedLinks(id);
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
            int id = (int)dataGridView1.CurrentRow.Cells[6].Value;
            OpenEditForm(id);
        }

        private void ToolStripMenuItemDeleteSelected_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[6].Value;
            //dataTablePreferedLinksTableAdapter.Delete(id);
            var link = DataAccess.Filter.GetPreferedLinkByLinkId(id);
            DataAccess.Lookup.Db.Table_LinksPrefereds.DeleteOnSubmit(link);
            DataAccess.Lookup.Db.SubmitChanges();
            PopulateTable();
        }

        private void ToolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            PopulateTable();
        }
    }
}

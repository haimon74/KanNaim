using System;
using System.Windows.Forms;
using HaimDLL;

namespace Kan_Naim_Main
{
    public partial class FormAdministrator : Form
    {
        public FormAdministrator()
        {
            InitializeComponent();
        }

        private void Form_Administrator_Load(object sender, EventArgs e)
        {
            ToolStripMenuItemAddArticle_Click(sender, e);
        }

        private void כתבותToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxPhoto2x_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void tabPageArticle_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void EditPublicArticleCallback(int articleId)
        {
            FormEditArtical newArt = FormEditArtical.GetFormEditArtical(articleId);
            newArt.Show();
            newArt.Focus();
        }

        private void ToolStripMenuItemAddArticle_Click(object sender, EventArgs e)
        {
            if (userControlTreeView1.SelectedNode == null)
                return;

            string category = userControlTreeView1.SelectedNode.Text;
            FormEditArtical newArt = FormEditArtical.GetFormEditNewArtical(category, "משה נעים");
            newArt.Show();
            newArt.Focus();
        }

        private void ToolStripMenuItemActiveCategories_Click(object sender, EventArgs e)
        {
            var categoryManager = new Form_CategoriesManager();
            categoryManager.Show();
            categoryManager.Focus();
        }

        private void ToolStripMenuItemEditUserDetails_Click(object sender, EventArgs e)
        {
            var form1 = new FormEditUserDetails();
            form1.Show();
        }

        private void ToolStripMenuItemPasswordReminder_Click(object sender, EventArgs e)
        {
            var form1 = new FormRemindPasswordByUserNameOrPhone();
            form1.Show();
        }

        private void ToolStripMenuItemBottomPageLinks_Click(object sender, EventArgs e)
        {
            var form1 = new FormManageBottomPageLinks();
            form1.Show();
        }

        private void ToolStripMenuItemAddPreferedLink_Click(object sender, EventArgs e)
        {
            var form1 = new FormAddNewPreferedLink();
            form1.Show();
        }

        private void ToolStripMenuItemPreferedLinksList_Click(object sender, EventArgs e)
        {
            var form1 = new FormManagePreferedLinks();
            form1.Show();
        }

        private void ToolStripMenuItemTopMenu_Click(object sender, EventArgs e)
        {
            var form1 = new FormManageTopMenuLinks();
            form1.Show();
        }

        private void ToolStripMenuItemRightMenu_Click(object sender, EventArgs e)
        {
            var form1 = new FormManageRightSideMenuLinks();
            form1.Show();
        }

        private void מודיעינעיםToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form1 = new FormManageIndex(1);
            form1.Show();
        }

        private void עסקיםToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form1 = new FormManageIndex(2);
            form1.Show();
        }

        private void בילוינעיםToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form1 = new FormManageIndex(3);
            form1.Show();
        }

        private void ToolStripMenuItemAddNewVideo_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemAddNewPhoto_Click(object sender, EventArgs e)
        {
            var form1 = new FormAddNewPhotos();
            form1.Show();
        }

        private void ToolStripMenuItemAddNewBanner_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemCategories_Click(object sender, EventArgs e)
        {
            var form1 = new Form_CategoriesManager();
            form1.Show();
            form1.Focus();
        }
        private void ShowForm(Form form)
        {
            form.Show();
            form.Focus();
        }
        private void ShowUserControl(UserControl userControl)
        {
            var form = new Form
            {
                Width = userControl.Width,
                Height = userControl.Height,
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                MaximizeBox = false,
                MinimizeBox = false,
                Name = "הזנת פרטי לינקים מועדפים",
                SizeGripStyle = SizeGripStyle.Hide
            };
            userControl.Dock = DockStyle.Fill;
            form.Controls.Add(userControl);
            form.Show();
            form.Focus();
        }
        private void buttonShowResults_Click(object sender, EventArgs e)
        {
            switch (comboBoxDataType.SelectedIndex)
            {
                //case 0: // articles
                //    ShowUserControl(new UserControlManageArticle());
                //    break;
                case 1: // taktzirim
                    ShowUserControl(new UserControlManageTaktzirim(EditPublicArticleCallback));
                    break;
                //case 2: // images
                //    ShowUserControl(new UserControlManageImages());
                //    break;
                //case 3: // videos
                //    ShowUserControl(new UserControlManageVideos());
                //    break;
                //case 4: //rss
                //    ShowUserControl(new UserControlManageRSS());
                //    break;
                //case 5: //banners
                //    ShowUserControl(new UserControlManageBanners());
                //    break;
                default:
                    break;
                
            }
        }

        private void ToolStripMenuItemEditPublicArticle_Click(object sender, EventArgs e)
        {
            
        }
    }
}

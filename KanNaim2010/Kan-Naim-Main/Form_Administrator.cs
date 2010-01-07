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

        private void groupBox1_Enter(object sender, EventArgs e)
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
        private void ShowUserControl(UserControl userControl, string titleName)
        {
            var form = new Form
            {
                Width = userControl.Width,
                Height = userControl.Height,
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true,
                MaximizeBox = false,
                MinimizeBox = false,
                Name = titleName,
                SizeGripStyle = SizeGripStyle.Hide
            };
            userControl.Dock = DockStyle.Fill;
            form.Controls.Add(userControl);
            form.Show();
            form.Focus();
        }

        private void EditOriginalImage(int imageId)
        {
            var uc = new UserControlEditPhoto(imageId);
            ShowUserControl(uc, "עריכת תמונה");
        }
        private void ViewImageDetails(int imageId)
        {
            var uc = new UserControlViewImageDetails(imageId);
            ShowUserControl(uc, "פרטי תמונות");
        }
        private void AddNewImage()
        {
            var uc = new UserControlUploadPhoto();
            ShowUserControl(uc, "הוספת תמונות לארכיון");
        }


        private void buttonShowResults_Click(object sender, EventArgs e)
        {
            switch (comboBoxDataType.SelectedIndex)
            {
                case 0: // articles
                    var uc0 = new UserControlManageArticles(EditPublicArticleCallback);
                    ShowUserControl(uc0, "ניהול כתבות");
                    break;
                //case 1: // taktzirim
                //    ShowUserControl(new UserControlManageArticles(EditPublicArticleCallback));
                //    break;
                case 2: // images
                    var uc2 = new UserControlManageImages(EditOriginalImage, AddNewImage, ViewImageDetails);
                    ShowUserControl(uc2, "ניהול תמונות");
                    break;
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

        private void checkBoxSelectCategory_CheckedChanged(object sender, EventArgs e)
        {
            userControlTreeView1.PopulateRootLevel("Table_LookupCategories", "ParentCatId");
            userControlTreeView1.Enabled = checkBoxSelectCategory.Checked;
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormAddNewPreferedLink : Form
    {
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public FormAddNewPreferedLink()
        {
            InitializeComponent();
        }

        private void buttonSearchPhoto_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
                                     {
                                         Filter = "Image Files(*.PNG;*.JPG;*.GIF)|*.PNG;*.JPG;*.GIF",
                                         InitialDirectory = "D:\\Pictures\\",
                                         DefaultExt = "jpg",
                                         CheckFileExists = true,
                                         Multiselect = false
                                     };
            
            openFileDialog.ShowDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                int length = filename.IndexOf("_size");
                if (length > 0)
                {
                    string name = filename.Substring(0, length);

                    Table_OriginalPhotosArchive photo = (from c in Db.Table_OriginalPhotosArchives
                                                         where c.Name.StartsWith(name)
                                                         select c).Single();

                    int originalPhotoId = photo.PhotoId;

                    int photoId = (from c in Db.Table_PhotosArchives
                                   where c.OriginalPhotoId == originalPhotoId &&
                                         c.PhotoTypeId == 3 // TODO - confirm size
                                   select c.Id).Single();

                    textBoxPhotoId.Text = photoId.ToString();
                }
            }

        }

        private void SetArticleIdValue(int articleId)
        {
            textBoxArticleId.Text = articleId.ToString();
        }
        private void buttonSearchArticle_Click(object sender, EventArgs e)
        {
            var form1 = new FormPreferedLinkArticleSelector(SetArticleIdValue, (int)comboBox1.SelectedValue);
            form1.Show();
        }

        private void FormAddNewPreferedLink_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_Kan_NaimDataSetCategories.Table_LookupCategories' table. You can move, or remove it, as needed.
            this.table_LookupCategoriesTableAdapter.Fill(this._Kan_NaimDataSetCategories.Table_LookupCategories);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSearchArticle.Enabled = true;
        }

        private void buttonClearForm_Click(object sender, EventArgs e)
        {
            textBoxUrl.Text = "";
            textBoxPhotoId.Text = "";
            textBoxOrderPlace.Text = "";
            textBoxArticleId.Text = "";
            textBoxAlternativeText.Text = "";
            buttonSearchArticle.Enabled = false;
        }

        private void buttonOpenTableView_Click(object sender, EventArgs e)
        {
            var form1 = new FormManagePreferedLinks();
            form1.Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int artId, photoId, orderPlace;
            if(!int.TryParse(textBoxArticleId.Text, out artId))
                artId = -1;
            if(!int.TryParse(textBoxPhotoId.Text, out photoId))
                photoId = -1;
            if(!int.TryParse(textBoxOrderPlace.Text, out orderPlace))
                orderPlace = -1;
            var link = new Table_LinksPrefered()
                           {
                               AltText = textBoxAlternativeText.Text,
                               ArticleId = artId,
                               OrderPlace = orderPlace,
                               PhotoId = photoId,
                               Url = textBoxUrl.Text
                           };
            GlobalValiables.Db.Table_LinksPrefereds.InsertOnSubmit(link);
            GlobalValiables.Db.SubmitChanges();
        }
    }
}

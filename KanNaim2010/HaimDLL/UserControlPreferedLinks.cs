using System;
using System.Linq;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlEditPreferedLinks : UserControl
    {
        private int _linkId;
        private bool _isNewLink;

        public UserControlEditPreferedLinks(int linkId)
        {
            InitializeComponent();
            _linkId = linkId;
            PopulateFormByLinqData();
        }

        private void PopulateFormByLinqData()
        {
            var link = Filter.GetPreferedLinkByLinkId(_linkId);

            try
            {
                textBoxUrl.Text = link.Url;
                textBoxAlternativeText.Text = link.AltText;
                textBoxOrderPlace.Text = String.Format("{0}",link.OrderPlace);
                textBoxArticleId.Text = String.Format("{0}",link.ArticleId);
                textBoxPhotoId.Text = String.Format("{0}",link.PhotoId);
                _isNewLink = false;
            }
            catch
            {
                _isNewLink = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Table_LinksPrefered link;

                if (_isNewLink)
                {
                    link = new Table_LinksPrefered();
                    Lookup.Db.Table_LinksPrefereds.InsertOnSubmit(link);
                }
                else
                {
                    link = Filter.GetPreferedLinkByLinkId(_linkId);
                }

                link.Url = textBoxUrl.Text;
                link.AltText = textBoxAlternativeText.Text;
                link.OrderPlace = Conversions.FromStringToNint(textBoxOrderPlace.Text);
                link.ArticleId = Conversions.FromStringToNint(textBoxArticleId.Text);
                link.PhotoId = Conversions.FromStringToNint(textBoxPhotoId.Text);
                link.Url = textBoxUrl.Text;
                
                Lookup.Db.SubmitChanges();
            }
            catch
            {
                
            }
        }

        private void buttonClearForm_Click(object sender, EventArgs e)
        {
            textBoxUrl.Text = "";
            textBoxAlternativeText.Text = "";
            textBoxOrderPlace.Text = "";
            textBoxArticleId.Text = "";
            textBoxPhotoId.Text = "";
            _isNewLink = true;
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

                    Table_OriginalPhotosArchive photo = (from c in Lookup.Db.Table_OriginalPhotosArchives
                                                         where c.Name.StartsWith(name)
                                                         select c).Single();

                    int originalPhotoId = photo.PhotoId;

                    int photoId = (from c in Lookup.Db.Table_PhotosArchives
                                   where c.OriginalPhotoId == originalPhotoId &&
                                         c.PhotoTypeId == 3 // TODO - confirm size
                                   select c.Id).Single();

                    textBoxPhotoId.Text = photoId.ToString();
                }
            }
        }
    }
}

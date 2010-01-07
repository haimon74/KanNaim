using System.Linq;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlViewImageDetails : UserControl
    {
        public UserControlViewImageDetails(int photoId)
        {
            InitializeComponent();

            var photo = Filter.GetOriginalPhotoFromId(photoId);

            if (photo == null)
                ((Form)this.Parent).Close();

            textBoxWidth.Text = photo.Width.ToString();
            textBoxHeight.Text = photo.Height.ToString();
            textBoxPhotoName.Text = photo.Name;
            textBoxDescription.Text = photo.Description;
            textBoxCaption.Text = photo.Caption;
            textBoxCategoryName.Text = Lookup.GetLookupCategoryNameFromId(photo.CategoryId);

            var photosArchive = Filter.GetPhotosArchiveByOriginalPhotoId(photoId);
            string url = (from c in photosArchive
                          where c.PhotoTypeId == 6     /* 80 x 75 */
                          select c.ImageUrl).SingleOrDefault();

            if (url != null)
            {
                pictureBox1.ImageLocation = url;
                pictureBox1.Load();
            }
        }
    }
}

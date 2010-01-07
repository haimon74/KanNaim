using System;
using System.Linq;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlManageImages : UserControl
    {
        public delegate void EditImageCallbackFunction(int imageId);
        private EditImageCallbackFunction _editImageCallback;

        public delegate void UploadImageCallbackFunction();
        private UploadImageCallbackFunction _uploadImageCallback;

        public delegate void ImageDetailViewCallbackFunction(int imageId);
        private ImageDetailViewCallbackFunction _viewImageCallback;

        public UserControlManageImages(
            EditImageCallbackFunction editImageCallback, 
            UploadImageCallbackFunction uploadImageCallback, 
            ImageDetailViewCallbackFunction viewImageCallback)
        {
            InitializeComponent();
            PopulateGridView(1); //TODO - get from caller
            _editImageCallback = editImageCallback;
            _viewImageCallback = viewImageCallback;
            _uploadImageCallback = uploadImageCallback;
        }

        private void PopulateGridView(int categoryId)
        {
            var queryResult = (categoryId > 0)
                                  ?
                                      Filter.GetOriginalPhotosByCategoryId(categoryId).OrderByDescending(x => x.Date)
                                  :
                                      Filter.GetAllOriginalPhotos().OrderByDescending(x => x.Date);

            dataGridView1.DataSource = queryResult;
        }

        private void ToolStripMenuItemEditImage_Click(object sender, EventArgs e)
        {
            var dataSource = (IQueryable<Table_OriginalPhotosArchive>)dataGridView1.DataSource;
            int imageId = dataSource.ToArray()[dataGridView1.CurrentRow.Index].PhotoId; 
            
            _editImageCallback(imageId);
        }

        private void ToolStripMenuItemDeleteImage_Click(object sender, EventArgs e)
        {
            var dataSource = (IQueryable<Table_OriginalPhotosArchive>)dataGridView1.DataSource;
            int imageId = dataSource.ToArray()[dataGridView1.CurrentRow.Index].PhotoId;

            Delete.TableOriginalImages(imageId);

            int delIdx = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(delIdx);
        }


        private void ToolStripMenuItemNewImage_Click(object sender, EventArgs e)
        {
            _uploadImageCallback();
        }

        private void toolStripMenuItemDetailView_Click(object sender, EventArgs e)
        {
            var dataSource = (IQueryable<Table_OriginalPhotosArchive>)dataGridView1.DataSource;
            int imageId = dataSource.ToArray()[dataGridView1.CurrentRow.Index].PhotoId;

            _viewImageCallback(imageId);
        }

    }
}

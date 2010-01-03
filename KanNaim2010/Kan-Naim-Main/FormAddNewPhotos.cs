using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HaimDLL;
using Kan_Naim_Main.DataAccess;


namespace Kan_Naim_Main
{
    public partial class FormAddNewPhotos : Form
    {
        public FormAddNewPhotos()
        {
            InitializeComponent();
        }

        private static Table_PhotosArchive _tblPhotos = new Table_PhotosArchive();
        private static Table_OriginalPhotosArchive _tblOriginalPhotos = new Table_OriginalPhotosArchive();


        private void UpLoadImageFileAndSaveToDatabase(FileInfo info)
        {
            try
            {
                FillTablesOriginalPhotosRecord(info);
                GlobalValiables.Db.Table_OriginalPhotosArchives.InsertOnSubmit(_tblOriginalPhotos);
                GlobalValiables.Db.Table_PhotosArchives.InsertOnSubmit(_tblPhotos);
                GlobalValiables.Db.SubmitChanges();
                MessageBox.Show("תמונה מקורית נשמרה בבסיס הנתונים בהצלחה", "הודעת - הצלחה", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                MessageBox.Show(ex.Message, "הודעת אזהרה - כשלון בשמירת קובץ לבסיס הנתונים", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        private void FillTablesOriginalPhotosRecord(FileInfo info)
        { // use only with original photos

            byte[] imageData = UserControlUploadPhoto.GetFileContentFromInfo(info);

            // original photos table
            _tblOriginalPhotos = new Table_OriginalPhotosArchive
            {
                AlternateText = ucUploadPhoto1.textBoxPhotoDescription.Text,
                Caption = ucUploadPhoto1.textBoxPhotoCaption.Text,
                CategoryId = Lookup.GetLookupCategoryIdFromName(comboBoxCategory.SelectedText),
                Date = DateTime.Now,
                Description = ucUploadPhoto1.textBoxPhotoDescription.Text
            };
            var pb = new PictureBox
            {
                Image = UserControlUploadPhoto.GetImageFromFileInfo(info)
            };
            _tblOriginalPhotos.ImageData = imageData;
            string[] pathParts = ucUploadPhoto1.textBoxPhotoPath.Text.Split('\\', '/', '.');
            _tblOriginalPhotos.Name = pathParts[pathParts.Length - 2].Trim('\\', '/', '.', ' ');
            _tblOriginalPhotos.Width = pb.Image.Width;
            _tblOriginalPhotos.Height = pb.Image.Height;

            // photos archive table
            _tblPhotos = new Table_PhotosArchive
            {
                LastTakId = 0,
                LastArticleId = 0,
                CssClass = "OriginalPhotoView",
                Date = DateTime.Now,
                ImageUrl =
                    String.Format("~\\Photos\\Originals\\{0}\\{1}.jpg", _tblOriginalPhotos.CategoryId,
                                  ucUploadPhoto1.textBoxPhotoCaption.Text),
                OriginalPhotoId = _tblOriginalPhotos.PhotoId,
                GalleryId = null,
                PhotoTypeId = 1
            };
            _tblPhotos.UrlLink = _tblPhotos.ImageUrl;
            _tblPhotos.Width = _tblOriginalPhotos.Width;
            _tblPhotos.Height = _tblOriginalPhotos.Height;
        }
        
        // saving in both (localy and archive)
        private void SaveNewPhotosClick(object sender, EventArgs e)
        {
            ucUploadPhoto1.SavePhotosLocally(); // saving locally

            if (!ucUploadPhoto1.IsStateEqual(UserControlUploadPhoto.UploadState.SavedLocalyOk))
                return;

            if (ucUploadPhoto1.radioButtonSavePhotosToArchive.Checked)
            {
                // saving in SQL Server Table_Photos
                var imageInfo = new FileInfo(ucUploadPhoto1._photoPath);
                // var myBitmap = new Bitmap(_ucUploadPhoto1._photoPath);
                // var rd = new StreamReader(imageInfo.FullName);
                // string content = rd.ReadToEnd();
                UpLoadImageFileAndSaveToDatabase(imageInfo);

                // getting the file from SQL , browse it (--> saves small copies)
                var wb = new WebBrowser();
                //string filePath = FormEditArtical._photoPath;
                const int fileId = 2;
                string url = String.Format("http://www.kan-naim.co.il/SavingPhoto.aspx?id={0}", fileId);
                wb.Navigate(url);
                wb.Show(); wb.Visible = true;
                //wb.Document.Write("Please Wait...");

                //reseting the form
                ucUploadPhoto1.Clear();
            }
        }

        private void userControlUploadPhoto1_Load(object sender, EventArgs e)
        {
            ucUploadPhoto1.SetSaveButtonCallbackFunction(SaveNewPhotosClick);
        }

        private void FormAddNewPhotos_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetLookupCategories.Table_LookupCategories' table. You can move, or remove it, as needed.
            this.table_LookupCategoriesTableAdapter.Fill(this._10infoDataSetLookupCategories.Table_LookupCategories);

        }
    }
}

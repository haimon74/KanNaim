using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlUploadPhoto : UserControl
    {
        public delegate void EventFunctionCallback(object sender, EventArgs e);

        private EventFunctionCallback _buttonSaveClickCallback;

        public enum UploadState
        {
            FileNotSelected = 0,
            FormatNotSupported = 1,
            SrcFileConfirmed = 2,
            SavingLocalyFailed = 3,
            SavedLocalyOk = 4,
            SavingOnlineFailed = 5,
            SavedOnlineOk = 6
        };

        static readonly string[] PhotoFormMsgs = {
                                            "עדיין לא נבחר קובץ מקור", // state 0
                                            "קובץ שנבחר אינו נתמך על ידי התוכנה", //state 1
                                            "בחר גדלי תמונות ולחץ לשמירה", // state 2
                                            "נכשל - קבצים לא נשמרו מקומית", //state 3
                                            "קבצים נשמרו במחשב מקומי", // state4
                                            "נכשל - קבצים לא נשמרו באתר אינטרנט", //state5
                                            "קבצים נשמרו באתר אינטרנט" //state6
                                        };

        public string _photoPath = null;

        private UploadState _uploadState = UploadState.FileNotSelected;
        private static readonly int[,] Sizes = { { 230, 217 }, { 208, 196 }, { 165, 155 }, { 125, 117 }, { 100, 94 }, { 80, 75 } };

        public UserControlUploadPhoto()
        {
            InitializeComponent();

            SetPhotoFormState(UploadState.FileNotSelected);
        }

        public void Clear()
        {
            textBoxPhotoWidth.Text = "";
            textBoxPhotoPath.Text = "";
            textBoxPhotoHeight.Text = "";
            textBoxPhotoDescription.Text = "";
            textBoxPhotoCaption.Text = "";

            SetPhotoFormState(UploadState.FileNotSelected);
        }
        public void SetSaveButtonCallbackFunction(EventFunctionCallback eventSaveCallback)
        {
            _buttonSaveClickCallback = eventSaveCallback;
        }

        public void PopulateByOriginalPhotoId(int photoId)
        {
            try
            {
                var originalPhoto = (from c in Lookup.Db.Table_OriginalPhotosArchives
                                     where c.PhotoId == photoId
                                     select c).Single();
                _uploadState = UploadState.SavedOnlineOk;
                textBoxPhotoCaption.Text = originalPhoto.Caption;
                textBoxPhotoDescription.Text = originalPhoto.Description;
                textBoxPhotoHeight.Text = originalPhoto.Height.ToString();
                textBoxPhotoWidth.Text = originalPhoto.Width.ToString();
                textBoxPhotoPath.Text = "מארכיון";
                Image img = Conversions.ByteArrayToImage(originalPhoto.ImageData.ToArray()); // TODO: need to confirm retrieving well...
                pictureBoxPreview.Image = img.GetThumbnailImage(Sizes[2, 0], Sizes[2, 1], ThumbnailCallback, IntPtr.Zero);
            }
            catch
            {
            }
        }
        
        private void SetPhotoFormState(UploadState theState)
        {
            _uploadState = theState;
            labelResultMsg.Text = PhotoFormMsgs[(int)_uploadState];
        }

        public bool IsStateEqual(UploadState theState)
        { 
            return _uploadState == theState;
        }

        private static Image GetThumbnail(string path, int width, int height, string description)
        {
            Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;

            try
            {
                var myBitmap = new Bitmap(path);
                var pathParts = path.Split('.', '/', '\\');
                var newfilename = pathParts[pathParts.Length - 2].Trim('.') + String.Format("_{0}_{1}x{2}_.jpg", description, width, height);
                var newPath = path.Substring(0, path.Length - pathParts[pathParts.Length - 2].Length - 4) + newfilename;
                var myThumbnail = myBitmap.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                myThumbnail.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                return myThumbnail;
            }
            catch
            {
                return null;
            }
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Insert an image ",
                InitialDirectory = "c:",
                FileName = "",
                Filter = "JPEG Image|*.jpg|GIF Image|*.gif|PNG Image|*.png"
            };
            //string chosen_file = "";


            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                //MessageBox.Show("Operation cancelled !");
            }
            else if (!String.IsNullOrEmpty(openFileDialog.FileName))
            {
                var chosenFile = openFileDialog.FileName;

                //if (!Directory.Exists(@"d:\Pictures\"))
                //    Directory.CreateDirectory(@"d:\Pictures\");

                Image.FromFile(chosenFile);
                var fileName = Path.GetFileNameWithoutExtension(chosenFile);

                if (fileName != null)
                {
                    SetPhotoFormState(UploadState.SrcFileConfirmed);

                    var img2 = Image.FromFile(chosenFile);
                    _photoPath = "d:\\Pictures\\" + fileName + ".jpg";

                    img2.Save(_photoPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    pictureBoxPreview.Image = img2.GetThumbnailImage(Sizes[2, 0], Sizes[2, 1], ThumbnailCallback, IntPtr.Zero);
                    textBoxPhotoWidth.Text = img2.Width.ToString();
                    textBoxPhotoHeight.Text = img2.Height.ToString();
                    textBoxPhotoPath.Text = _photoPath;
                }
                else
                {
                    SetPhotoFormState(UploadState.FormatNotSupported);
                }
            }
        }

        // saving photos only localy 
        public void SavePhotosLocally()
        { // saving localy only //TODO
            string filePath = _photoPath;

            try
            {
                if (checkBoxSize230w.Checked)
                {
                }
                if (checkBoxSize208w.Checked)
                {
                }
                if (checkBoxSize165w.Checked)
                {
                }
                if (checkBoxSize125w.Checked)
                {
                }
                if (checkBoxSize100w.Checked)
                {
                }
                if (checkBoxSize80w.Checked)
                {
                }

                SetPhotoFormState(UploadState.SavedLocalyOk);

            }
            catch
            {
                SetPhotoFormState(UploadState.SavingLocalyFailed);
            }
        }

        public static byte[] GetFileContentFromInfo(FileInfo info)
        {
            var content = new byte[info.Length];
            FileStream imagestream = info.OpenRead();
            imagestream.Read(content, 0, content.Length);
            imagestream.Close();
            return content;
        }
        public static Image GetImageFromByteArray(byte[] imageData)
        {
            //Initialize image variable
            Image newImage = null;
            try
            {
                //Read image data into a memory stream
                using (var ms = new MemoryStream(imageData, 0, imageData.Length))
                {
                    ms.Write(imageData, 0, imageData.Length);

                    //Set image variable value using memory stream.
                    newImage = Image.FromStream(ms, true);
                }
            }
            catch (Exception exception)
            {
                Messages.ExceptionMessage(exception);
            }
            return newImage;
        }
        public static Image GetImageFromFileInfo(FileInfo info)
        {
            //Initialize image variable
            Image newImage = null;

            try
            {
                byte[] imageData = GetFileContentFromInfo(info);

                newImage = GetImageFromByteArray(imageData);
            }
            catch (Exception exception)
            {
                Messages.ExceptionMessage(exception);
            }
            return newImage;
        }

        private void UserControlUploadPhoto_Load(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            _buttonSaveClickCallback(sender, e);
        }

        private void buttonSelectCategory_Click(object sender, EventArgs e)
        {

        }

        private void SelectPhotoDoubleClick(object sender, EventArgs e)
        {
            try
            {
                int photoId = int.Parse(((ListView)sender).SelectedItems[0].SubItems[0].Text);

                var photo = Filter.GetOriginalPhotoFromId(photoId);

                textBoxPhotoWidth.Text = photo.Width.ToString();
                textBoxPhotoPath.Text = "מארכיון";
                textBoxPhotoPath.Enabled = false;
                textBoxPhotoHeight.Text = photo.Height.ToString();
                textBoxPhotoDescription.Text = photo.Description;
                textBoxPhotoCaption.Text = photo.Caption;
                checkBoxSize80w.Checked = false;
                checkBoxSize80w.Enabled = false;
                checkBoxSize100w.Checked = false;
                checkBoxSize100w.Enabled = false;
                checkBoxSize125w.Checked = false;
                checkBoxSize125w.Enabled = false;
                checkBoxSize165w.Checked = false;
                checkBoxSize165w.Enabled = false;
                checkBoxSize208w.Checked = false;
                checkBoxSize208w.Enabled = false;
                checkBoxSize230w.Checked = false;
                checkBoxSize230w.Enabled = false;
                button15.Enabled = false;
                radioButtonSavePhotosToArchive.Enabled = false;
                radioButtonSavePhotosLocally.Enabled = false;
                
            }
            catch
            {

            }
        }
        private void buttonLoadFromArchive_Click(object sender, EventArgs e)
        {
            var form1 = new Form()
                            {
                                RightToLeft = RightToLeft.Yes,
                                RightToLeftLayout = true,
                                Height = 500,
                                Width = 700,
                                Name = String.Format(comboBox1.SelectedText + "רשימת תמונות מקטגוריה")
            };
            var listView = new ListView
            {
                View = View.Details,
                GridLines = true,
                FullRowSelect = true,
                CheckBoxes = false,
                AllowColumnReorder = true,
                LabelEdit = false,
                RightToLeftLayout = true,
                MultiSelect = false,
                Dock = DockStyle.Fill,
                Visible = true
            };

            listView.DoubleClick += SelectPhotoDoubleClick;

            listView.Columns.Add("מספר", 50, HorizontalAlignment.Center);
            listView.Columns.Add("שם התמונה", 650, HorizontalAlignment.Center);
            //listView.Columns.Add("שם הווידאו", 600, HorizontalAlignment.Center);

            var itemCollection = new ListView.ListViewItemCollection(listView);

            var photos = from c in Lookup.Db.Table_OriginalPhotosArchives
                         select c;
            foreach (Table_OriginalPhotosArchive photo in photos)
            {
                var item = new ListViewItem();
                item.SubItems.Add(photo.PhotoId.ToString());
                item.SubItems.Add(photo.Caption);
                itemCollection.Add(item);
            }
            
            form1.Controls.Add(listView);
            listView.Refresh();

            form1.Show();

        }

        private void buttonClearForm_Click(object sender, EventArgs e)
        {
            checkBoxSize80w.Checked = true;
            checkBoxSize80w.Enabled = true;
            checkBoxSize100w.Checked = true;
            checkBoxSize100w.Enabled = true;
            checkBoxSize125w.Checked = true;
            checkBoxSize125w.Enabled = true;
            checkBoxSize165w.Checked = true;
            checkBoxSize165w.Enabled = true;
            checkBoxSize208w.Checked = true;
            checkBoxSize208w.Enabled = true;
            checkBoxSize230w.Checked = true;
            checkBoxSize230w.Enabled = true;
            textBoxPhotoCaption.Enabled = true;
            textBoxPhotoCaption.Text = "";
            textBoxPhotoDescription.Enabled = true;
            textBoxPhotoDescription.Text = "";
            textBoxPhotoHeight.Text = "";
            textBoxPhotoWidth.Text = "";
            textBoxPhotoPath.Enabled = true;
            textBoxPhotoPath.Text = "";
            button15.Enabled = true;
        }


    }
}

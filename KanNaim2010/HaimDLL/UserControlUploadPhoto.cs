using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}

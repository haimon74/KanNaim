using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HaimDLL
{
    public partial class UserControlUploadPhotos : UserControl
    {
        enum PhotoFormStates
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
        static string _photoPath = null;
        private PhotoFormStates _photoFormState = PhotoFormStates.FileNotSelected;
        private static readonly int[,] Sizes = { { 230, 217 }, { 208, 196 }, { 165, 155 }, { 125, 117 }, { 100, 94 }, { 80, 75 } };
        
        public UserControlUploadPhotos()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void SetPhotoFormState(PhotoFormStates theState)
        {
            _photoFormState = theState;
            labelResultMsg.Text = PhotoFormMsgs[(int)_photoFormState];
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
                    SetPhotoFormState(PhotoFormStates.SrcFileConfirmed);

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
                    SetPhotoFormState(PhotoFormStates.FormatNotSupported);
                }
            }
        }
    }
}

using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlEditPhoto : UserControl
    {
        public delegate void EventFunctionCallback(object sender, EventArgs e);

        private EventFunctionCallback _buttonSaveClickCallback;

        public  readonly IDictionary<string, CheckBox> SizeSelectCollection = new Dictionary<string, CheckBox>();

        private static readonly int[,] Sizes = { { 230, 217 }, { 208, 196 }, { 165, 155 }, { 125, 117 }, { 100, 94 }, { 80, 75 } };

        public UserControlEditPhoto(int photoId)
        {
            InitializeComponent();

            //SetPhotoFormState(UploadState.FileNotSelected);
            PopulateByOriginalPhotoId(photoId);

            SizeSelectCollection.Clear();
            SizeSelectCollection.Add("W230", checkBoxSize230w);
            SizeSelectCollection.Add("W208", checkBoxSize208w);
            SizeSelectCollection.Add("W165", checkBoxSize165w);
            SizeSelectCollection.Add("W125", checkBoxSize125w);
            SizeSelectCollection.Add("100", checkBoxSize100w);
            SizeSelectCollection.Add("W80", checkBoxSize80w);
        }

        public void Clear()
        {
            textBoxPhotoWidth.Text = "";
            textBoxPhotoHeight.Text = "";
            textBoxPhotoDescription.Text = "";
            textBoxPhotoCaption.Text = "";
        }
        public void SetSaveButtonCallbackFunction(EventFunctionCallback eventSaveCallback)
        {
            _buttonSaveClickCallback = eventSaveCallback;
        }

        public void PopulateByOriginalPhotoId(int photoId)
        {
            try
            {
                var originalPhoto = Filter.GetOriginalPhotoFromId(photoId);

                textBoxPhotoCaption.Text = originalPhoto.Caption;
                textBoxPhotoDescription.Text = originalPhoto.Description;
                textBoxPhotoHeight.Text = originalPhoto.Height.ToString();
                textBoxPhotoWidth.Text = originalPhoto.Width.ToString();

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

                // TODO: change to retrieve frome small photo url in archive table
                Image img = Conversions.ByteArrayToImage(originalPhoto.ImageData.ToArray()); // TODO: need to confirm retrieving well...
                pictureBoxPreview.Image = img.GetThumbnailImage(Sizes[2, 0], Sizes[2, 1], ThumbnailCallback, IntPtr.Zero);
            }
            catch
            {
            }
        }


        private static bool ThumbnailCallback()
        {
            return false;
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
    }
}

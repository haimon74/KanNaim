using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Kan_Naim_Main
{
    public partial class FormEditArtical : Form
    {
        enum PhotoFormStates
        {
            FILE_NOT_SELECTED = 0,
            FORMAT_NOT_SUPPORTED = 1, 
            SRC_FILE_CONFIRMED = 2, 
            SAVING_LOCALY_FAILED=3,
            SAVED_LOCALY_OK = 4, 
            SAVING_ONLINE_FAILED = 5,
            SAVED_ONLINE_OK = 6
        };
        static string[] _photoFormMsgs = {
                                            "עדיין לא נבחר קובץ מקור", // state 0
                                            "קובץ שנבחר אינו נתמך על ידי התוכנה", //state 1
                                            "בחר גדלי תמונות ולחץ לשמירה", // state 2
                                            "נכשל - קבצים לא נשמרו מקומית", //state 3
                                            "קבצים נשמרו במחשב מקומי", // state4
                                            "נכשל - קבצים לא נשמרו באתר אינטרנט", //state5
                                            "קבצים נשמרו באתר אינטרנט" //state6
                                        };
        static string _photoPath = null;

        private PhotoFormStates _photoFormState = PhotoFormStates.FILE_NOT_SELECTED;

        private int[,] sizes = { { 230, 217 }, { 208, 196 }, { 165, 155 }, { 125, 117 }, { 100, 94 }, { 80, 75 } };

        public FormEditArtical()
        {
            InitializeComponent();

            SetPhotoFormState(PhotoFormStates.FILE_NOT_SELECTED);
            // UpLoadImageFile(new FileInfo("foo"));
        }
        private bool ThumbnailCallback()
        {
            return false;
        }

        private Image GetThumbnail(string path, int width, int height, string description)
        { 
            System.Drawing.Image.GetThumbnailImageAbort myCallback =
                new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

            try
            {
                System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(path);
                string[] pathParts = path.Split('.', '/', '\\');
                string newfilename = pathParts[pathParts.Length - 2].Trim('.') + String.Format("_{0}_{1}x{2}_.jpg", description, width, height);
                string newPath = path.Substring(0, path.Length - pathParts[pathParts.Length - 2].Length - 4) + newfilename;
                System.Drawing.Image myThumbnail = myBitmap.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                myThumbnail.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                return myThumbnail;
            }
            catch
            {
                return null;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void tabPagePhotos_Click(object sender, EventArgs e)
        {

        }

        private void SetPhotoFormState(PhotoFormStates theState)
        {
            this._photoFormState = theState;
            this.labelResultMsg.Text = FormEditArtical._photoFormMsgs[(int)_photoFormState];
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            //string chosen_file = "";

            openFD.Title = "Insert an image ";
            openFD.InitialDirectory = "c:";
            openFD.FileName = "";
            openFD.Filter = "JPEG Image|*.jpg|GIF Image|*.gif|PNG Image|*.png";


            if (openFD.ShowDialog() == DialogResult.Cancel)
            {
                //MessageBox.Show("Operation cancelled !");
            }
            else if (!String.IsNullOrEmpty(openFD.FileName))
            {
                string chosen_file = openFD.FileName;
             
                //if (!Directory.Exists(@"d:\Pictures\"))
                //    Directory.CreateDirectory(@"d:\Pictures\");

                Image img1 = Image.FromFile(chosen_file);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(chosen_file);
                
                if ((img1 != null) && (fileName != null))
                {
                    SetPhotoFormState(PhotoFormStates.SRC_FILE_CONFIRMED);
                    
                    Image img2 = Image.FromFile(chosen_file);
                    FormEditArtical._photoPath = "d:\\Pictures\\" + fileName + ".jpg";

                    img2.Save(FormEditArtical._photoPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    
                    button16.ImageAlign = ContentAlignment.MiddleRight;
                    button16.Image = img2;

                    this.textBoxPhotoPath.Text = FormEditArtical._photoPath;
                }
                else
                {
                    SetPhotoFormState(PhotoFormStates.FORMAT_NOT_SUPPORTED);                    
                }                
            }
        }

        private void button16_Click(object sender, EventArgs e)
        { // saving localy only
            string filePath = FormEditArtical._photoPath;
            
            Image img2 = null;

            try
            {
                if (this.checkBoxSize230w.Checked)
                {
                    img2 = GetThumbnail(filePath, sizes[0, 0], sizes[0, 1], "size1");
                }
                if (this.checkBoxSize208w.Checked)
                {
                    img2 = GetThumbnail(filePath, sizes[1, 0], sizes[1, 1], "size2");
                }
                if (this.checkBoxSize165w.Checked)
                {
                    img2 = GetThumbnail(filePath, sizes[2, 0], sizes[2, 1], "size3");
                }
                if (this.checkBoxSize125w.Checked)
                {
                    img2 = GetThumbnail(filePath, sizes[3, 0], sizes[3, 1], "size4");
                }
                if (this.checkBoxSize100w.Checked)
                {
                    img2 = GetThumbnail(filePath, sizes[4, 0], sizes[4, 1], "size5");
                }
                if (this.checkBoxSize80w.Checked)
                {
                    img2 = GetThumbnail(filePath, sizes[5, 0], sizes[5, 1], "size6");
                }

                button16.Image = img2;

                SetPhotoFormState(PhotoFormStates.SAVED_LOCALY_OK);

            }
            catch
            {
                SetPhotoFormState(PhotoFormStates.SAVING_LOCALY_FAILED);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                string[] pathParts = this.textBoxPhotoPath.Text.Split('.', '\\', '/');
                this.textBoxPhotoName.Text = pathParts[pathParts.Length - 2].Trim('.', '\\', '/');
            }
            else
            {
                this.textBoxPhotoName.Text = "";
            }
        }

        private string GetConnectionString()
        {
            return @"Provider=SQLOLEDB;Data Source=PRIVATE-DCB1672;Integrated Security=SSPI;Initial Catalog=KanNaimOld";
        }

        private void UpLoadImageFile(FileInfo info)
        {
            OleDbConnection objConn = null;
            OleDbCommand objCom = null;
            try
            {
                byte[] content = new byte[info.Length];
                FileStream imagestream = info.OpenRead();
                imagestream.Read(content, 0, content.Length);
                imagestream.Close();

                objConn = new OleDbConnection(GetConnectionString());
                string qry = "insert into Table_Photos (PhotoId, Width, Height, ContentType, Date, Name, CategoryId, FileSize, FileData)" +
                            "VALUES (? , ? , ? , ? , ? , ? , ? , ? , ?)";
                            //"VALUES (@PhotoId, @Width, @Height, @ContentType, @Date, @Name, @CategoryId, @FileSize, @FileData)";
                            //"VALUES ([PhotoId], [Width], [Height], [ContentType], [Date], [Name], [CategoryId], [FileSize], [FileData])";
                objCom = new OleDbCommand(qry, objConn);

                OleDbParameter photoId = new OleDbParameter("@PhotoId", OleDbType.Integer);
                photoId.Value = (int)2;
                objCom.Parameters.Add(photoId);

                OleDbParameter widthParam = new OleDbParameter("@Width", OleDbType.Integer);
                widthParam.Value = (int)111;
                objCom.Parameters.Add(widthParam);

                OleDbParameter heightParam = new OleDbParameter("@Height", OleDbType.Integer);
                heightParam.Value = (int)222;
                objCom.Parameters.Add(heightParam);

                OleDbParameter contentTypeParam = new OleDbParameter("@ContentType", OleDbType.VarChar);
                contentTypeParam.Value = "image/jpeg";
                objCom.Parameters.Add(contentTypeParam);

                OleDbParameter dateParam = new OleDbParameter("@Date", OleDbType.Date);
                dateParam.Value = DateTime.Now.ToShortDateString();
                objCom.Parameters.Add(dateParam);

                OleDbParameter nameParam = new OleDbParameter("@Name", OleDbType.VarChar);
                nameParam.Value = "test photo";
                objCom.Parameters.Add(nameParam);

                OleDbParameter categoryIdParam = new OleDbParameter("@CategoryId", OleDbType.Integer);
                categoryIdParam.Value = (int)777;
                objCom.Parameters.Add(categoryIdParam);

                OleDbParameter fileSizeParam = new OleDbParameter("@FileSize", OleDbType.Integer);
                fileSizeParam.Value = (int)1000000;
                objCom.Parameters.Add(fileSizeParam);

                string base64String =
                    System.Convert.ToBase64String(content, //binaryData,
                                                  0,
                                                  content.Length);
                
                OleDbParameter dataParam = new OleDbParameter("@FileData", OleDbType.VarChar);
                dataParam.Value = base64String;
                objCom.Parameters.Add(dataParam);

                objConn.Open();
                objCom.ExecuteNonQuery();
                objConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objConn.Close();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        { // saving in both (localy and archive)

            button16_Click(sender, e);

            if (this._photoFormState != PhotoFormStates.SAVED_LOCALY_OK)
                return;

            // saving in SQL Server Table_Photos
            FileInfo imageInfo = new FileInfo(FormEditArtical._photoPath);
            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(FormEditArtical._photoPath);
            StreamReader rd = new StreamReader(imageInfo.FullName);
            string content = rd.ReadToEnd();
            UpLoadImageFile(imageInfo);
            //string qry = "INSERT INTO Table_Photos (PhotoId, Width, Height, ContentType, Date, Name, CategoryId, FileSize, FileData) " +
            //             "VALUES (1, 400,300,'image/jpeg', '27/11/2009' , 'testPhoto', 777, 11111" +
            //             String.Format(@"'{0}'", content);
            //OleDbConnection dbConn = new OleDbConnection(GetConnectionString());
            //OleDbCommand dbComm = new OleDbCommand(qry, dbConn);
            //dbConn.Open();
            //dbComm.ExecuteNonQuery();
            //dbConn.Close();
            
            // getting the file from SQL , browse it (--> saves small copies)
            WebBrowser wb = new WebBrowser();
            //string filePath = FormEditArtical._photoPath;
            int fileId = 2;
            string url = String.Format("http://www.kan-naim.co.il/SavingPhoto.aspx?id={0}", fileId) ;
            wb.Navigate(url);
            wb.Show(); wb.Visible = true;
            //wb.Document.Write("Please Wait...");
        }
    }
}

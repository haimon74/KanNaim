using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.OleDb;
using System.Data.SqlClient;
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

        private static DataClassesEditArticleDataContext _db = new DataClassesEditArticleDataContext();
        private static Table_PhotosArchive _tblPhotos = new Table_PhotosArchive();
        private static Table_OriginalPhotosArchive _tblOriginalPhotos = new Table_OriginalPhotosArchive();
        private static Table_Article _tblArticle = new Table_Article();
        private static Table_LookupArticleStatus _tblLookupArticleStatuses = new Table_LookupArticleStatus();
        private static Table_LookupCategory _tblLookupCategories = new Table_LookupCategory();
        private static Table_LookupPhotoType _tblLookupPhotoTypes = new Table_LookupPhotoType();
        private static Table_LookupReporter _tblLookupReporters = new Table_LookupReporter();
        private static Table_LookupRole _tblLookupRoles = new Table_LookupRole();
        private static Table_Taktzirim _tblTaktzirim = new Table_Taktzirim();
        private static Table_VideosArchive _tblVideosArchive = new Table_VideosArchive();
        
        private PhotoFormStates _photoFormState = PhotoFormStates.FILE_NOT_SELECTED;

        private static int[,] sizes = { { 230, 217 }, { 208, 196 }, { 165, 155 }, { 125, 117 }, { 100, 94 }, { 80, 75 } };

        private static FormEditArtical singleFormEditArtical = new FormEditArtical();
        private FormEditArtical()
        {            
        }

        private static void InitializeForm(string category, string username)
        {
            singleFormEditArtical.InitializeComponent();
            singleFormEditArtical.SetPhotoFormState(PhotoFormStates.FILE_NOT_SELECTED);
            singleFormEditArtical.table_LookupCategoriesTableAdapter.Fill(singleFormEditArtical._Kan_NaimDataSetCategories.Table_LookupCategories);
            singleFormEditArtical.comboBoxArticleCategory.SelectedIndex = singleFormEditArtical.comboBoxArticleCategory.FindString(category);
            singleFormEditArtical.table_LookupReportersTableAdapter1.Fill(singleFormEditArtical._Kan_NaimDataSetReportersNames.Table_LookupReporters);
            singleFormEditArtical.comboBoxEditor.SelectedIndex = singleFormEditArtical.comboBoxEditor.FindString(username);
                
        }
        private static int GetCetegoryIdFromName(string hebrewName)
        {
            try
            {
                int catId = (from c in _db.Table_LookupCategories
                             where c.CatHebrewName.Trim() == hebrewName.Trim()
                             select c.CatId).Single();
                return catId;
            }
            catch 
            {
                return -1;
            }
        }
        private static string CategoryNameFromId(int id)
        {
            try
            {
                string name = (from c in FormEditArtical._db.Table_LookupCategories
                               where c.CatId == FormEditArtical._tblArticle.CategoryId
                               select c.CatHebrewName).Single();
                return name;
            }
            catch
            {
                return "";
            }
        }
        public static FormEditArtical GetFormEditNewArtical(string category, string username)
        {
            InitializeForm(category, username);

            FormEditArtical._tblArticle = CreateNewArticleInArchive();

            return FormEditArtical.singleFormEditArtical;
        }

        private static string ReporterPublishNameFromId(int id)
        {
            try
            {
                string name = (from c in FormEditArtical._db.Table_LookupReporters
                               where c.UserId == FormEditArtical._tblArticle.UpdatedBy
                               select c.PublishNameShort).Single();
                return name;
            }
            catch
            {
                return "";
            }
        }
        
        private static void FillComboBoxPhotosFromOriginId(int imgId)
        {
            var img = (from c in _db.Table_PhotosArchives
                       where c.Id == imgId
                       select c).Single();
            
            int? originId = img.OriginalPhotoId;
            
            var relatedPhotos = from c in _db.Table_PhotosArchives
                                where c.OriginalPhotoId == originId
                                select c;

            foreach (Table_PhotosArchive item in relatedPhotos)
            {
                singleFormEditArtical.comboBoxArticlePhoto.Items.Add(item);    
            }
        }

        private static void InitializeForm()
        {
            singleFormEditArtical.InitializeComponent();
            singleFormEditArtical.SetPhotoFormState(PhotoFormStates.FILE_NOT_SELECTED);
            singleFormEditArtical.table_LookupCategoriesTableAdapter.Fill(singleFormEditArtical._Kan_NaimDataSetCategories.Table_LookupCategories);
            string categoryName = CategoryNameFromId(FormEditArtical._tblArticle.CategoryId);
            singleFormEditArtical.comboBoxArticleCategory.SelectedIndex = singleFormEditArtical.comboBoxArticleCategory.FindString(categoryName);
            singleFormEditArtical.table_LookupReportersTableAdapter1.Fill(singleFormEditArtical._Kan_NaimDataSetReportersNames.Table_LookupReporters);
            string reporter = ReporterPublishNameFromId(FormEditArtical._tblArticle.UpdatedBy);
            singleFormEditArtical.comboBoxEditor.SelectedIndex = singleFormEditArtical.comboBoxEditor.FindString(reporter);
            FillComboBoxPhotosFromOriginId(_tblArticle.ImageId);
        }
        
        public static FormEditArtical GetFormEditArtical(int articleId)
        {
            FormEditArtical._tblArticle = (from c in FormEditArtical._db.Table_Articles
                                           where c.ArticleId == articleId
                                           select c).Single();

            return FormEditArtical.singleFormEditArtical;
        }

        private static Table_Article CreateNewArticleInArchive()
        {
            FormEditArtical._tblArticle = new Table_Article();
            
            // default value
            FormEditArtical._tblArticle.CategoryId = 0;
            FormEditArtical._tblArticle.ArticleContent = "Empty Content";
            FormEditArtical._tblArticle.CountComments = 0;
            FormEditArtical._tblArticle.CountViews = 0;
            FormEditArtical._tblArticle.CreatedBy = 0;
            FormEditArtical._tblArticle.EmbedObjId = 0;
            FormEditArtical._tblArticle.FlagActiveMivzak = true;
            FormEditArtical._tblArticle.FlagActiveRSS = true;
            FormEditArtical._tblArticle.FlagShowDateTime = true;
            FormEditArtical._tblArticle.FlagTak1ColPic = false;
            FormEditArtical._tblArticle.FlagTak1ColPicTxt = false;
            FormEditArtical._tblArticle.FlagTak1ColTxt = false;
            FormEditArtical._tblArticle.FlagTak2ColPic = false;
            FormEditArtical._tblArticle.FlagTak2ColPicTxt = false;
            FormEditArtical._tblArticle.FlagTak2ColTxt = false;
            FormEditArtical._tblArticle.FlagTak3ColPic = false;
            FormEditArtical._tblArticle.FlagTak3ColPicTxt = false;
            FormEditArtical._tblArticle.FlagTak3ColTxt = false;
            FormEditArtical._tblArticle.FlagTakLineFeeds = false;
            FormEditArtical._tblArticle.FlagTakSmallPic = false;
            FormEditArtical._tblArticle.FlagTakSmallPicTxt = false;
            FormEditArtical._tblArticle.FlagTakSmallTxt = false;
            FormEditArtical._tblArticle.ImageId = 0;
            FormEditArtical._tblArticle.IsPublished = false;
            FormEditArtical._tblArticle.KeysAssociated = "";
            FormEditArtical._tblArticle.KeysLookup = "";
            FormEditArtical._tblArticle.MetaTags = "";
            FormEditArtical._tblArticle.StatusId = 0;
            FormEditArtical._tblArticle.SubTitle = "Empty Subtitle";
            FormEditArtical._tblArticle.Summery = "";
            FormEditArtical._tblArticle.Title = "Empty Title";
            FormEditArtical._tblArticle.UpdatedBy = 0;
            // specific values
            DateTime now = DateTime.Now;
            FormEditArtical._tblArticle.CreateDate = now;
            FormEditArtical._tblArticle.UpdateDate = now;
            //FormEditArtical._tblArticle.CategoryId = (int)singleFormEditArtical.comboBoxArticleCategory.SelectedValue;
            FormEditArtical._db.Table_Articles.InsertOnSubmit(FormEditArtical._tblArticle);
            FormEditArtical._db.SubmitChanges();
            var newArticle = (from c in FormEditArtical._db.Table_Articles
                             where (c.CreateDate == now)
                             select c).Single();
            return newArticle;
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

        // saving photos only localy 
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
        
        private string GetConnectionString()
        {
            // MySql
            //return @"Provider=SQLOLEDB;Data Source=PRIVATE-DCB1672;Integrated Security=SSPI;Initial Catalog=KanNaimOld";
            // MS-SQL
            return @"Data Source=PRIVATE-DCB1672;Initial Catalog=Kan-Naim;Integrated Security=True";
        }
        private byte[] GetFileContentFromInfo(FileInfo info)
        {
            byte[] content = new byte[info.Length];
            FileStream imagestream = info.OpenRead();
            imagestream.Read(content, 0, content.Length);
            imagestream.Close();
            return content;
        }
        private System.Drawing.Image GetImageFromByteArray(byte[] imageData)
        {
            //Initialize image variable
            Image newImage = null;
            try
            {
                //Read image data into a memory stream
                using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                {
                    ms.Write(imageData, 0, imageData.Length);

                    //Set image variable value using memory stream.
                    newImage = Image.FromStream(ms, true);
                }
            }
            catch
            {
            }
            return newImage;
        }
        private System.Drawing.Image GetImageFromFileInfo(FileInfo info)
        {
            //Initialize image variable
            Image newImage = null;
                
            try
            {
                byte[] imageData = GetFileContentFromInfo(info);

                newImage = GetImageFromByteArray(imageData);                
            }
            catch
            {
            }
            return newImage;
        }

        private void FillTablesOriginalPhotosRecord(FileInfo info)
        { // use only with original photos

            byte[] imageData = GetFileContentFromInfo(info);
            
            // original photos table
            FormEditArtical._tblOriginalPhotos = new Table_OriginalPhotosArchive();
            FormEditArtical._tblOriginalPhotos.AlternateText = textBoxPhotoDescription.Text;
            FormEditArtical._tblOriginalPhotos.Caption = textBoxPhotoCaption.Text;
            FormEditArtical._tblOriginalPhotos.CategoryId = GetCetegoryIdFromName(comboBoxArticleCategory.SelectedText);
            FormEditArtical._tblOriginalPhotos.Date = DateTime.Now;
            FormEditArtical._tblOriginalPhotos.Description = textBoxPhotoDescription.Text;
            PictureBox pb = new PictureBox();
            pb.Image = GetImageFromFileInfo(info);
            FormEditArtical._tblOriginalPhotos.ImageData = imageData;
            string[] pathParts = textBoxPhotoPath.Text.Split('\\', '/', '.');
            FormEditArtical._tblOriginalPhotos.Name = pathParts[pathParts.Length - 2].Trim('\\', '/', '.', ' ');
            FormEditArtical._tblOriginalPhotos.Width = pb.Image.Width;
            FormEditArtical._tblOriginalPhotos.Height = pb.Image.Height; 
            
            // photos archive table
            FormEditArtical._tblPhotos = new Table_PhotosArchive();
            FormEditArtical._tblPhotos.LastTakId = 0; //  - no use for original image
            FormEditArtical._tblPhotos.LastArticleId = 0; //  - no use for original image
            FormEditArtical._tblPhotos.CssClass = "OriginalPhotoView"; //TBD
            FormEditArtical._tblPhotos.Date = DateTime.Now;
            FormEditArtical._tblPhotos.ImageUrl = String.Format("~\\Photos\\Originals\\{0}\\{1}.jpg", FormEditArtical._tblOriginalPhotos.CategoryId, this.textBoxPhotoCaption.Text);
            FormEditArtical._tblPhotos.OriginalPhotoId = FormEditArtical._tblOriginalPhotos.PhotoId;
            FormEditArtical._tblPhotos.GalleryId = null;
            FormEditArtical._tblPhotos.PhotoTypeId = 1; // 1 means original size
            FormEditArtical._tblPhotos.UrlLink = FormEditArtical._tblPhotos.ImageUrl;
            FormEditArtical._tblPhotos.Width = FormEditArtical._tblOriginalPhotos.Width;
            FormEditArtical._tblPhotos.Height = FormEditArtical._tblOriginalPhotos.Height;
        }
        
        private void UpLoadImageFile(FileInfo info)
        {
            try
            {
                FillTablesOriginalPhotosRecord(info);
                _db.Table_OriginalPhotosArchives.InsertOnSubmit(_tblOriginalPhotos);
                _db.Table_PhotosArchives.InsertOnSubmit(_tblPhotos);
                _db.SubmitChanges();
                MessageBox.Show("תמונה מקורית נשמרה בבסיס הנתונים בהצלחה", "הודעת - הצלחה", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //string base64String =
                //    System.Convert.ToBase64String(imageData, //binaryData,
                //                                  0,
                //                                  imageData.Length);

                //SqlParameter dataParam = new SqlParameter("@FileData", SqlDbType.VarChar);
                //dataParam.Value = base64String;
                //objCom.Parameters.Add(dataParam);                
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                MessageBox.Show(ex.Message, "הודעת אזהרה - כשלון בשמירת קובץ לבסיס הנתונים", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                //objConn.Close();
            }
        }

        // saving in both (localy and archive)
        private void button15_Click(object sender, EventArgs e)
        {     
            button16_Click(sender, e); // saving locally

            if (this._photoFormState != PhotoFormStates.SAVED_LOCALY_OK)
                return;

            // saving in SQL Server Table_Photos
            FileInfo imageInfo = new FileInfo(FormEditArtical._photoPath);
            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(FormEditArtical._photoPath);
            StreamReader rd = new StreamReader(imageInfo.FullName);
            string content = rd.ReadToEnd();
            UpLoadImageFile(imageInfo);
            
            // getting the file from SQL , browse it (--> saves small copies)
            WebBrowser wb = new WebBrowser();
            //string filePath = FormEditArtical._photoPath;
            int fileId = 2;
            string url = String.Format("http://www.kan-naim.co.il/SavingPhoto.aspx?id={0}", fileId) ;
            wb.Navigate(url);
            wb.Show(); wb.Visible = true;
            //wb.Document.Write("Please Wait...");
        }

        private void FormEditArtical_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_Kan_NaimDataSet1.Table_PhotosArchive' table. You can move, or remove it, as needed.
            this.table_PhotosArchiveTableAdapter.Fill(this._Kan_NaimDataSet1.Table_PhotosArchive);
            
            // TODO: This line of code loads data into the '_Kan_NaimDataSet.Table_LookupArticleStatus' table. You can move, or remove it, as needed.
            this.table_LookupArticleStatusTableAdapter.Fill(this._Kan_NaimDataSet.Table_LookupArticleStatus);

        }

        private void buttonClearCategoriesList_Click(object sender, EventArgs e)
        {
            this.listBoxSelectedCategories.Items.Clear();
        }
        private void buttonManageCategories_Cilck(object sender, EventArgs e)
        {
            Form_CategoriesManager form1 = new Form_CategoriesManager();
            form1.Show();
            form1.Focus();
        }

        private void buttonRemoveSelectedCategory_Click(object sender, EventArgs e)
        {
            int length = this.listBoxSelectedCategories.Items.Count;

            for (int i = length-1 ; i >= 0; i--)
			{
			    string catItem = (string)this.listBoxSelectedCategories.Items[i];
                //string catName = catItem.Split('#')[0].Trim('#', ' ');
                if (listBoxSelectedCategories.SelectedItems.Contains(catItem))
                {
                    this.listBoxSelectedCategories.Items.RemoveAt(i);
                }		        
	        } 
        }

        private void buttonAddSelectedCategories_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                if (node.Checked)
                    this.listBoxSelectedCategories.Items.Add(String.Format("{0} #{1}",node.Name, node.ToolTipText));
                    //this.listBoxSelectedCategories.Items.Add(new CategoryItem(node.Name, 0/*int.Parse(node.ToolTipText)*/));
            }
            //this.listBoxSelectedCategories.Refresh();
      
        }
        private void RemoveCategoryForever(TreeNode theNode, bool toRemoveSiblings)
        {
            if (toRemoveSiblings == true)
            {
                // remove recursivly - must do it recursivly for updating the DB ???
                foreach (TreeNode node in theNode.Nodes)
                {
                    RemoveCategoryForever(node, true);
                }
            }
            else
            {
                TreeNode parent = theNode.Parent;
                // updating siblings father
                foreach (TreeNode node in theNode.Nodes)
                {
                    parent.Nodes.Add((TreeNode)node.Clone());
                }
            }
            // remove from treeview
            theNode.Remove();
            // remove from DB
            //_tblLookupCategories = (from c in _db.Table_LookupCategories
            //                        where c.CatHebrewName.Contains(theNode.Text)
            //                        select c).Single();
            //_db.Table_LookupCategories.DeleteOnSubmit(_tblLookupCategories);
            //_db.SubmitChanges();
        }

        private void ToolStripMenuItemAddCategory_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView1.SelectedNode;

            if (node == null)
                return;

            // creating new category in the DB
            int pId = int.Parse(node.ToolTipText);
            _tblLookupCategories = new Table_LookupCategory();
            _tblLookupCategories.CatHebrewName = "קטגוריה חדשה";
            _tblLookupCategories.CatEnglishName = "New Category";
            _tblLookupCategories.ParentCatId = pId;
            string ticksId = DateTime.Now.Ticks.ToString();
            _tblLookupCategories.Tags = ticksId;
            _db.Table_LookupCategories.InsertOnSubmit(_tblLookupCategories);
            _db.SubmitChanges();
            
            // getting the new category ID
            _tblLookupCategories = (from c in  _db.Table_LookupCategories
                                    where c.Tags.Trim() == ticksId
                                    select c).Single();

            node.Nodes.Add(_tblLookupCategories.CatHebrewName);
            node.Nodes[_tblLookupCategories.CatHebrewName].ToolTipText = _tblLookupCategories.CatId.ToString();            
        }

        private void ToolStripMenuItemDeleteCategory_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView1.SelectedNode;

            if (node == null)
                return;

            if (node.Nodes == null)
            {
                node.Remove();
                return;
            }
            // else
            DialogResult result = MessageBox.Show("האם ברצונך למחוק גם תתי קטגוריות ?",
                                        "בקשת אישור למחיקת תת קטגוריות מרובות",
                                        MessageBoxButtons.YesNoCancel,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Cancel)
                return;
            else if (result == DialogResult.No) // keep siblings
                RemoveCategoryForever(node, false);
            else  // removing category and all its siblings
                RemoveCategoryForever(node, true);
        }

        private void ToolStripMenuItemUpdateCategory_Click(object sender, EventArgs e)
        {

        }
     
    }
}

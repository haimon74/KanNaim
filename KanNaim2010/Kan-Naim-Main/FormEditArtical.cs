using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using HaimDLL;


namespace Kan_Naim_Main
{
    public partial class FormEditArtical : Form
    {
        private static bool _isNewArticle = true;
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();
        private static Table_PhotosArchive _tblPhotos = new Table_PhotosArchive();
        private static Table_OriginalPhotosArchive _tblOriginalPhotos = new Table_OriginalPhotosArchive();
        private static Table_Article _tblArticle = new Table_Article();
        private static Table_LookupCategory _tblLookupCategories = new Table_LookupCategory();

        private static readonly IDictionary<string, UserControlTakFill> UcTakFillCollection =
            new Dictionary<string, UserControlTakFill>();

        public static readonly Dictionary<string, string>[] StylesCollection = new Dictionary<string, string>[6];
        public static readonly Dictionary<string, string>[] HyperlinksCollection = new Dictionary<string, string>[6];

        private static FormEditArtical _singleton;// = new FormEditArtical();

        private FormEditArtical()
        {
            //this.ucUploadPhoto1 = ucUploadPhoto1;
        }

        private static void InitializeForm(string category, string username)
        {
            if (_singleton == null)
                _singleton = new FormEditArtical();
            
            _singleton.InitializeComponent();

            _singleton._tableLookupCategoriesTableAdapter.Fill(_singleton._kanNaimDataSetCategories.Table_LookupCategories);
            _singleton._comboBoxArticleCategory.SelectedIndex = _singleton._comboBoxArticleCategory.FindString(category);
            _singleton._tableLookupReportersTableAdapter1.Fill(_singleton._kanNaimDataSetReportersNames.Table_LookupReporters);
            _singleton._comboBoxEditor.SelectedIndex = _singleton._comboBoxEditor.FindString(username);

            _singleton._userControlTakFillSizeX3.TakType = 1;
            _singleton._userControlTakFillSizeX2.TakType = 2;
            _singleton._userControlTakFillSizeX1.TakType = 3;
            _singleton._userControlTakFillSizeMedium.TakType = 4;
            _singleton._userControlTakFillSizeSmall.TakType = 5;
            
            UcTakFillCollection.Clear();
            UcTakFillCollection.Add("takMedium",_singleton._userControlTakFillSizeMedium);
            UcTakFillCollection.Add("takSmall",_singleton._userControlTakFillSizeSmall);
            UcTakFillCollection.Add("takX1",_singleton._userControlTakFillSizeX1);
            UcTakFillCollection.Add("takX2",_singleton._userControlTakFillSizeX2);
            UcTakFillCollection.Add("takX3",_singleton._userControlTakFillSizeX3);
        }

        public static FormEditArtical GetFormEditNewArtical(string category, string username)
        {
            InitializeForm(category, username);

            _tblArticle = CreateNewArticleInArchive();

            return _singleton;
        }
        

/*
        private static void FillComboBoxPhotosFromOriginId(int imgId)
        {
            var img = (from c in Db.Table_PhotosArchives
                       where c.Id == imgId
                       select c).Single();
            
            int? originId = img.OriginalPhotoId;
            
            var relatedPhotos = from c in Db.Table_PhotosArchives
                                where c.OriginalPhotoId == originId
                                select c;

            foreach (Table_PhotosArchive item in relatedPhotos)
            {
                _singl._comboBoxArticlePhoto.Items.Add(item);    
            }
        }
*/

        public static FormEditArtical GetFormEditArtical(int articleId)
        {
            if (_singleton == null)
                _singleton = new FormEditArtical();

            try
            {
                _tblArticle = (Db.Table_Articles
                    .Where(c => c.ArticleId == articleId))
                    .Single();

                PopulateFromArticle();
                _isNewArticle = false;
                return _singleton;
            }
            catch
            {
                _isNewArticle = true;
                return GetFormEditNewArtical("חדשות", "משה נעים");
            }
        }



        private static Table_Article CreateNewArticleInArchive()
        {
            _tblArticle = new Table_Article
                              {
                                  CategoryId = 0,
                                  ArticleContent = "Empty Content",
                                  CountComments = 0,
                                  CountViews = 0,
                                  CreatedBy = 0,
                                  EmbedObjId = 0,
                                  FlagActiveMivzak = true,
                                  FlagActiveRSS = true,
                                  FlagShowDateTime = true,
                                  FlagTak1ColPic = false,
                                  FlagTak1ColPicTxt = false,
                                  FlagTak1ColTxt = false,
                                  FlagTak2ColPic = false,
                                  FlagTak2ColPicTxt = false,
                                  FlagTak2ColTxt = false,
                                  FlagTak3ColPic = false,
                                  FlagTak3ColPicTxt = false,
                                  FlagTak3ColTxt = false,
                                  FlagTakLineFeeds = false,
                                  FlagTakSmallPic = false,
                                  FlagTakSmallPicTxt = false,
                                  FlagTakSmallTxt = false,
                                  ImageId = 0,
                                  IsPublished = false,
                                  KeysAssociated = "",
                                  KeysLookup = "",
                                  MetaTags = "",
                                  StatusId = 0,
                                  SubTitle = "Empty Subtitle",
                                  Summery = "",
                                  Title = "Empty Title",
                                  UpdatedBy = 0
                              };
            
            // default value
            // specific values
            DateTime now = DateTime.Now;
            _tblArticle.CreateDate = now;
            _tblArticle.UpdateDate = now;
            //FormEditArtical._tblArticle.CategoryId = (int)singleFormEditArtical.comboBoxArticleCategory.SelectedValue;
            //Db.SubmitChanges();
            //var newArticle = (from c in Db.Table_Articles
            //                 where (c.CreateDate == now)
            //                 select c).Single();
            return _tblArticle;
        }

        private static void PopulateFromArticle()
        {
            _singleton._comboBoxArticleCategory.SelectedText =
                DataAccess.Lookup.GetLookupCategoryNameFromId(_tblArticle.CategoryId);
            _singleton._richTextBoxArticleContent.Rtf = _tblArticle.ArticleContent;
            _singleton._comboBoxEditor.SelectedText =
                DataAccess.Lookup.GetLookupReporterFromUserId(_tblArticle.UpdatedBy).PublishNameShort;
            _singleton._ucUploadVideo1.PopulateByVideoId(_tblArticle.EmbedObjId);
            _singleton._ucUploadPhoto1.PopulateByOriginalPhotoId(_tblArticle.ImageId);
            _singleton._checkBoxPublish.Checked = _tblArticle.IsPublished;
            _singleton._textBoxKeyWords.Text = _tblArticle.KeysAssociated;
            _singleton._textBoxTags.Text = _tblArticle.MetaTags;
            _singleton._textBoxArticleTitle.Text = _tblArticle.Title;
            _singleton._textBoxArticleSubtitle.Text = _tblArticle.SubTitle;
            char[] splitChars = {'|'};
            string[] keysLookup = _tblArticle.KeysLookup.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            _singleton._listBoxSelectedCategories.ResetText();
            foreach (string key in keysLookup)
            {
                _singleton._listBoxSelectedCategories.Items.Add(key.Trim());
            }
            // change date to 'Now' only when saving 
            //DateTime now = DateTime.Now;
            //_tblArticle.CreateDate = now;
            //_tblArticle.UpdateDate = now;
            ////Singleton._comboBoxArticlePhoto = FillComboBoxWithPhotosArchive(Singleton._comboBoxArticlePhoto);
            _tblOriginalPhotos.Name = DataAccess.Filter.GetOriginalPhotoNameFromPhotoId(_tblArticle.ImageId);
            PopulateArticlePhotosComboBox(_tblOriginalPhotos.Name);
            _singleton.comboBoxArticlePhoto_SelectedIndexChanged(_singleton._comboBoxArticleCategory, new EventArgs());
        }

        private static bool FillTablesOriginalPhotosRecord(FileInfo info)
        { // use only with original photos
            
            byte[] imageData = UserControlUploadPhoto.GetFileContentFromInfo(info);
            
            // original photos table
            _tblOriginalPhotos = new Table_OriginalPhotosArchive
                                     {
                                         AlternateText = _singleton._ucUploadPhoto1.textBoxPhotoDescription.Text,
                                         Caption = _singleton._ucUploadPhoto1.textBoxPhotoCaption.Text,
                                         CategoryId = _tblArticle.CategoryId,
                                         Date = DateTime.Now,
                                         Description = _singleton._ucUploadPhoto1.textBoxPhotoDescription.Text
                                     };
            var pb = new PictureBox
                         {
                             Image = UserControlUploadPhoto.GetImageFromFileInfo(info)
                         };
            _tblOriginalPhotos.ImageData = imageData;
            string[] pathParts = _singleton._ucUploadPhoto1.textBoxPhotoPath.Text.Split('\\', '/', '.');
            _tblOriginalPhotos.Name = pathParts[pathParts.Length - 2].Trim('\\', '/', '.', ' ');
            _tblOriginalPhotos.Width = pb.Image.Width;
            _tblOriginalPhotos.Height = pb.Image.Height; 
            
            // photos archive table
            _tblPhotos = new Table_PhotosArchive
                             {
                                 LastTakId = 0,
                                 LastArticleId = 0,
                                 CssClass = "ArchivePhotoView",
                                 Date = DateTime.Now,
                                 ImageUrl = String.Format("~\\Photos\\Originals\\{0}\\{1}.jpg", 
                                                    _tblOriginalPhotos.CategoryId,
                                                   _tblOriginalPhotos.Name),
                                 OriginalPhotoId = _tblOriginalPhotos.PhotoId,
                                 GalleryId = null,
                                 PhotoTypeId = 1
                             };
            _tblPhotos.UrlLink = _tblPhotos.ImageUrl;
            _tblPhotos.Width = _tblOriginalPhotos.Width;
            _tblPhotos.Height = _tblOriginalPhotos.Height;

            bool returnValue = 
                (_tblOriginalPhotos.Name != "") &&
                (_tblOriginalPhotos.ImageData.Length > 0) &&
                (_tblOriginalPhotos.Height > 0) &&
                (_tblOriginalPhotos.Width > 0) &&
                (_tblOriginalPhotos.CategoryId > 0) &&
                (_tblPhotos.ImageUrl.Length > 20);

            return returnValue;
        }
        
        private static void UpLoadImageFileAndSaveToDatabase(FileInfo info)
        {
            if (FillTablesOriginalPhotosRecord(info) == false)
            {
                Messages.ExceptionMessage(new Exception(), " נתונים שגויים");
                return;
            }

            try
            {
                Db.Table_OriginalPhotosArchives.InsertOnSubmit(_tblOriginalPhotos);
                Db.SubmitChanges();
                Db.Table_PhotosArchives.InsertOnSubmit(_tblPhotos);
                Db.SubmitChanges();
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
        }

        private static void PopulateArticlePhotosComboBox(string photoName)
        {
            var photosByCategory =
                        DataAccess.Filter.GetOriginalPhotosNamesByCategoryId(_tblOriginalPhotos.CategoryId);
            _singleton._comboBoxArticlePhoto.ResetText();
            _singleton._comboBoxArticlePhoto.DataSource = photosByCategory;
            
            if (photoName != String.Empty)
                _singleton._comboBoxArticlePhoto.SelectedIndex = _singleton._comboBoxArticlePhoto.FindString(photoName);
        }

        // saving in both (localy and archive)
        private void SaveNewPhotosClick(object sender, EventArgs e)
        {
            if (_singleton._ucUploadPhoto1._photoPath == null)
                return;

            if (_singleton._ucUploadPhoto1._photoPath.Length < 5)
                return;

            _singleton._ucUploadPhoto1.SavePhotosLocally(); // saving locally

            if (!_singleton._ucUploadPhoto1.IsStateEqual(UserControlUploadPhoto.UploadState.SavedLocalyOk))
                return;

            if (_singleton._ucUploadPhoto1.radioButtonSavePhotosToArchive.Checked)
            {
                // saving in SQL Server Table_Photos

                var imageInfo = new FileInfo(_singleton._ucUploadPhoto1._photoPath);
                // var myBitmap = new Bitmap(_ucUploadPhoto1._photoPath);
                // var rd = new StreamReader(imageInfo.FullName);
                // string content = rd.ReadToEnd();
                
                UpLoadImageFileAndSaveToDatabase(imageInfo);

                // updating the images combobox 
                //string photoName = (from c in photosByCategory
                //                    where Singleton._ucUploadPhoto1.textBoxPhotoPath.Text.Contains(c.Name.Trim())
                //                    orderby c.PhotoId descending
                //                    select c.Name).First();
                //Singleton._comboBoxArticlePhoto.Items.Insert(0, photoName);
                //Singleton._comboBoxArticlePhoto.SelectedIndex = 0;

                string[] pathParts = _singleton._ucUploadPhoto1.textBoxPhotoPath.Text.Split('\\', '/', '.');
                string photoName = pathParts[pathParts.Length - 2].Trim('\\', '/', '.', ' ');
                
                try
                {
                    PopulateArticlePhotosComboBox(photoName);

                    // SavePhotosCopiesAtDomainDirectory();

                    foreach (CheckBox checkBox in _singleton._ucUploadPhoto1.SizeSelectCollection.Values)
                    {
                        if (checkBox.Checked)
                        {
                            SaveSmallPhotoCopyToPhotoArchive(checkBox.Text);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Messages.ExceptionMessage(ex);
                }

                /*
                var photosByCategory = DataAccess.Filter.GetOriginalPhotosByCategoryName("");
                var photosNames = (from c in photosByCategory
                                   orderby c.Date descending
                                   select c.Name);
                string toSelect = "";
                foreach (string s in photosNames)
                {
                    comboBoxArticlePhoto.Items.Add(s);
                    if (ucUploadPhoto1.textBoxPhotoPath.Text.Contains(s))
                        toSelect = s;
                }
            
                comboBoxArticlePhoto.SelectedText = toSelect;
                */
            }
        }

        private static bool GetThumbnailCallback()
        {
            return false;
        }

        
        private static void SaveSmallPhotoCopyToPhotoArchive(string photoSizeAsString)
        {
            string[] splitStrings = {"x","גודל "};
            string[] sizes = photoSizeAsString.Split(splitStrings, StringSplitOptions.RemoveEmptyEntries);

            int width = int.Parse(sizes[0]);
            int height = int.Parse(sizes[1]);

            //MessageBox.Show("width = " + width + "  height = " + height);
            
            DateTime now = DateTime.Now;
            int typeId = DataAccess.Lookup.GetLookupPhotoTypeIdFromPhotoWidth(width);
            string url = String.Format("~\\Photos\\Thumbnail\\{0}\\{1}_{2}x{3}_KanNaim_{4}-{5}-{6}_{7}.jpg",
                                       _tblOriginalPhotos.CategoryId, _tblOriginalPhotos.Name,
                                       width, height, now.Day, now.Month, now.Year, typeId);
            var copyData = _tblPhotos;

            _tblPhotos = new Table_PhotosArchive
                             {
                                 PhotoTypeId = typeId,
                                 OriginalPhotoId = copyData.OriginalPhotoId,
                                 ImageUrl = url,
                                 Width = width,
                                 Height = height,
                                 CssClass = copyData.CssClass,
                                 Date = DateTime.Now,
                                 GalleryId=null,
                                 LastArticleId = null
                             };
            try
            {
                Db.Table_PhotosArchives.InsertOnSubmit(_tblPhotos);
                Db.SubmitChanges();
            }
            catch(Exception exception)
            {
                Messages.ExceptionMessage(exception);
            }
        }

        private static void SavePhotosCopiesAtDomainDirectory()
        {
            var wb = new WebBrowser();
            //string filePath = FormEditArtical._photoPath;
            const int fileId = 2;
            string url = String.Format("http://www.kan-naim.co.il/SavingPhoto.aspx?id={0}", fileId);
            //byte[] imageData = _tblOriginalPhotos.ImageData.ToArray();
            //Image image = Conversions.ByteArrayToImage(imageData).GetThumbnailImage(width, height, GetThumbnailCallback, IntPtr.Zero);
            //imageData = Conversions.ImageToByteArray(image);
            wb.Navigate(url);
            wb.Show(); wb.Visible = true;
        }

        private void FormEditArtical_Load(object sender, EventArgs e)
        {
            _singleton._tablePhotosArchiveTableAdapter.Fill(_singleton._kanNaimDataSet1.Table_PhotosArchive);
            _singleton._tableLookupArticleStatusTableAdapter.Fill(_singleton._kanNaimDataSet.Table_LookupArticleStatus);
            //_comboBoxArticleCategory.DataSource = DataAccess.Filter.Get

            //_singleton._userControlTreeView1.PopulateRootLevel("Table_LookupCategories", "ParentCatId");
            buttonReloadCategoryTree_Click(null, null);
        }

        private void buttonClearCategoriesList_Click(object sender, EventArgs e)
        {
            for (int i = _singleton._listBoxSelectedCategories.Items.Count; i > 0; i--)
            {
                RemoveItemTakCatTreeSelectorAt(i - 1);
            }
            _singleton._listBoxSelectedCategories.ResetText();
        }
        private void buttonManageCategories_Cilck(object sender, EventArgs e)
        {
            var form1 = new Form_CategoriesManager();
            form1.Show();
            form1.Focus();
        }

        private static void RemoveItemTakCatTreeSelectorAt(int idx)
        {
            foreach (UserControlTakFill userControlTakFill in UcTakFillCollection.Values)
            {
                userControlTakFill.ucTakBroadcastCmd1.treeViewTakBroadcastCatSelector.Nodes.RemoveAt(idx);
            }
        }

        private void buttonRemoveSelectedCategory_Click(object sender, EventArgs e)
        {
            int length = _singleton._listBoxSelectedCategories.Items.Count;

            for (int i = length-1 ; i >= 0; i--)
			{
			    var catItem = (string)_singleton._listBoxSelectedCategories.Items[i];
                //string catName = catItem.Split('#')[0].Trim('#', ' ');
                if (_singleton._listBoxSelectedCategories.SelectedItems.Contains(catItem))
                {
                    _singleton._listBoxSelectedCategories.Items.RemoveAt(i);
                    RemoveItemTakCatTreeSelectorAt(i);
                    return;
                }		        
	        } 
        }

        private void buttonAddSelectedCategories_Click(object sender, EventArgs e)
        {
            foreach (TreeNode siblingNode in _singleton._userControlTreeView1.Nodes)
            {
                AddSelectedTreeNodesToList(false, siblingNode);
            }
        }
        private static void RemoveCategoryForever(TreeNode theNode, bool toRemoveSiblings)
        {
            if (toRemoveSiblings)
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
            TreeNode node = _singleton._userControlTreeView1.SelectedNode;

            if (node == null)
                return;

            // creating new category in the DB
            int pId = int.Parse(node.ToolTipText);
            _tblLookupCategories = new Table_LookupCategory
                                       {
                                           CatHebrewName = "קטגוריה חדשה",
                                           CatEnglishName = "New Category",
                                           ParentCatId = pId
                                       };
            string ticksId = DateTime.Now.Ticks.ToString();
            _tblLookupCategories.Tags = ticksId;
            Db.Table_LookupCategories.InsertOnSubmit(_tblLookupCategories);
            Db.SubmitChanges();
            
            // getting the new category ID
            _tblLookupCategories = (from c in  Db.Table_LookupCategories
                                    where c.Tags.Trim() == ticksId
                                    select c).Single();

            node.Nodes.Add(_tblLookupCategories.CatHebrewName);
            node.Nodes[_tblLookupCategories.CatHebrewName].ToolTipText = _tblLookupCategories.CatId.ToString();            
        }

        private void ToolStripMenuItemDeleteCategory_Click(object sender, EventArgs e)
        {
            TreeNode node = _singleton._userControlTreeView1.SelectedNode;

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

            RemoveCategoryForever(node, result != DialogResult.No);
        }

        private static string GetSelectedCategoryHebrewName()
        {
            //return (string)((DataRowView)_singleton._comboBoxArticleCategory.SelectedItem)["CatHebrewName"];
            return _singleton._comboBoxArticleCategory.Text;
        }
        private static void RefreshCategoryComboByValue(string value)
        {
            _singleton._tableLookupCategoriesTableAdapter.Fill(_singleton._kanNaimDataSetCategories.Table_LookupCategories);
            _singleton._comboBoxArticleCategory.SelectedIndex = _singleton._comboBoxArticleCategory.FindString(value);
        }

        private void buttonReloadCategoryTree_Click(object sender, EventArgs e)
        {
            _singleton._userControlTreeView1.PopulateRootLevel("Table_LookupCategories", "ParentCatId");

            string categoryName = GetSelectedCategoryHebrewName();
            RefreshCategoryComboByValue(categoryName);
        }

        private static void AddItemToTakCatTreeSelector(string newItem)
        {
            foreach (UserControlTakFill userControlTakFill in UcTakFillCollection.Values)
            {
                userControlTakFill.ucTakBroadcastCmd1.treeViewTakBroadcastCatSelector.Nodes.Add(newItem);
            }
        }

        private static void AddSelectedTreeNodesToList(bool isNotConditional, TreeNode node)
        {
                if (node.Checked || isNotConditional)
                {
                    node.ToolTipText = (from c in Db.Table_LookupCategories
                                        where c.CatHebrewName == node.Text
                                        select c.CatId).Single().ToString();
                    
                    string newItem = String.Format("{1} # {0}", node.Text, node.ToolTipText);
                    _singleton._listBoxSelectedCategories.Items.Add(newItem);
                    AddItemToTakCatTreeSelector(newItem);                    
                }

                foreach (TreeNode siblingNode in node.Nodes)
                {
                    AddSelectedTreeNodesToList(isNotConditional, siblingNode);
                }
        }
        private void buttonAddAllCategories_Click(object sender, EventArgs e)
        {
            foreach (TreeNode siblingNode in _singleton._userControlTreeView1.Nodes)
            {
                AddSelectedTreeNodesToList(true, siblingNode);
            }
        }

        private void comboBoxArticleCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string categoryName = _singleton._comboBoxArticleCategory.Text.Trim();
            _tblArticle.CategoryId = DataAccess.Lookup.GetLookupCategoryIdFromName(categoryName);
            _tblOriginalPhotos.CategoryId = _tblArticle.CategoryId;

            PopulateArticlePhotosComboBox(_tblOriginalPhotos.Name);
            //PopulateArticleVideosComboBox(Singleton._ucUploadVideo1._textBoxVideoCaption.Text);
        }

        private static void PopulateArticleVideosComboBox(string videoName)
        {
            var videosByCategory =
                        DataAccess.Filter.GetVideosNamesByCategoryId((_tblOriginalPhotos.CategoryId));
            _singleton._comboBoxArticleVideo.ResetText();
            _singleton._comboBoxArticleVideo.DataSource = videosByCategory;

            if (videoName != String.Empty)
                _singleton._comboBoxArticleVideo.SelectedIndex = _singleton._comboBoxArticleVideo.FindString(videoName);
        }

        private static void PopulateTaksPhotosComboBoxes()
        {
            //var photos = DataAccess.Filter.GetArchivePhotosNamesByOriginalPhotoId(_tblOriginalPhotos.PhotoId);
            var photos = DataAccess.Filter.GetPhotosArchiveByOriginalPhotoId(_tblOriginalPhotos.PhotoId);
            foreach (UserControlTakFill uc in UcTakFillCollection.Values)
            {
                uc.ucTakContent1.comboBoxTakPhoto.ResetText();
                uc.ucTakContent1.comboBoxTakPhoto.DataSource = photos;
                uc.ucTakContent1.comboBoxTakPhoto.ValueMember = "Id";
                uc.ucTakContent1.comboBoxTakPhoto.DisplayMember = "ImageUrl";
            }

        }
        /*
                private static ComboBox FillComboBoxWithPhotosArchive(ComboBox comboBox)
                {
                    int originalPhotoId;

                    try
                    {
                        originalPhotoId =
                            DataAccess.Filter.
                            GetOriginalPhotoIdByPhotoName(_singleton._comboBoxArticlePhoto.SelectedText);
                    }
                    catch
                    {
                        return comboBox;
                    }

                    var relatedPhotosArchive =
                        DataAccess.Filter.GetPhotosArchiveByOriginalPhotoId(originalPhotoId);

                    //ComboBox combo = userControlTakFillSizeX3.ucTakContent1.comboBoxTakPhoto;
                    comboBox.ResetText();

                    var types = DataAccess.Lookup.GetLookupPhotoTypes();

                    foreach (Table_PhotosArchive tablePhotosArchive in relatedPhotosArchive)
                    {
                        int photoTypeId = tablePhotosArchive.PhotoTypeId;
                        string photoTypeDescription = (from c in types
                                                       where c.PhotoTypeId == photoTypeId
                                                       select c.TypeDescription).Single();
                        string itemText = String.Format("{0} - {1}", _singleton._comboBoxArticlePhoto.SelectedText, photoTypeDescription);
                        comboBox.Items.Add(itemText);
                    }
                    return comboBox;
                }
        */

        private void comboBoxArticlePhoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateTaksPhotosComboBoxes();
        }

        public static void UpdateHtmlTextIntoArticleContentTextBox(string rtfContent)
        {
            _singleton._richTextBoxArticleContent.Rtf = rtfContent;
        }

        private void buttonOpenEditor_Click(object sender, EventArgs e)
        {
            //_singleton._richTextBoxArticleContent.Enabled = !_singleton._richTextBoxArticleContent.Enabled;

            var updateFunctionCallback = new FormArticleRichTextBoxEditor.ReturnHtmlTextCallbackType(UpdateHtmlTextIntoArticleContentTextBox);

            var articleEditorForm = new FormArticleRichTextBoxEditor(_singleton._richTextBoxArticleContent.Rtf, updateFunctionCallback);

            articleEditorForm.Show();

            //var artBrowser = new WebBrowser
            //                            {
            //                                DocumentText =
            //                                    "<html lang=\"he\">\n<head>\n<basefont color=\"black\" size=\"12\" font=\"Arial\" />\n" +
            //                                    "<base target=\"_blank\" />\n</head>\n<body>\n<bdo dir=\"rtl\">\n<pre>\n\n\n\n" +
            //                                    textBoxArticleContent.Text +
            //                                    "</pre>\n</bdo>\n</body>\n</html>\n\n\n"
            //                            };

            //var htmlDoc = artBrowser.Document;

        }
        
        /* 
        // all the following related to previous implementation for editing article content RTF 
        
        private bool BeforeSelectedTextIsEqual(string cmpStr)
        {
            string content = textBoxArticleContent.Text;
            int idx = textBoxArticleContent.SelectionStart;

            int len = cmpStr.Length;
            bool isEqual = (content.Substring(idx - len, len) == cmpStr);

            return isEqual;
        }

        private void AddTagToSelectedText(string tagBefore, string tagAfter)
        {
            int idx = textBoxArticleContent.SelectionStart;
            string content = textBoxArticleContent.Text;

            string contentBeforeTxt = content.Substring(0, idx);
            string contentAfterTxt = content.Substring(idx + textBoxArticleContent.SelectionLength);
            textBoxArticleContent.Text =
                contentBeforeTxt + tagBefore +
                textBoxArticleContent.SelectedText + tagAfter +
                contentAfterTxt;
        }
        private void RemoveTagFromSelectedText(string tagBefore, string tagAfter)
        {
            int idx = textBoxArticleContent.SelectionStart;
            string content = textBoxArticleContent.Text;

            string contentBeforeTxt = content.Substring(0, idx);
            string contentAfterTxt = content.Substring(idx + textBoxArticleContent.SelectionLength);
            int idx1 = contentAfterTxt.IndexOf(tagAfter);
            textBoxArticleContent.Text =
                contentBeforeTxt.Substring(0, contentBeforeTxt.Length - tagBefore.Length) +
                textBoxArticleContent.SelectedText +
                contentAfterTxt.Substring(0, idx1) +
                contentAfterTxt.Substring(idx1 + tagAfter.Length); 
        }

        private void buttonTextTag_Click(object sender, EventArgs e)
        {
            int idx = textBoxArticleContent.SelectionStart;
            string content = textBoxArticleContent.Text;
            //string originalSelectedText = textBoxArticleContent.SelectedText;
            int originalSelectedStart = textBoxArticleContent.SelectionStart;
            int originalSelectedLength = textBoxArticleContent.SelectionLength;

            string[] tagsBeforeBoldUnderlinedItalic = { "<B>", "<U>", "<I>" };
            string tagName = ((Button)sender).Text;
            string tagBefore = String.Format("<{0}>", tagName);
            string tagAfter = String.Format("</{0}>", tagName);
            int i = 0;
            int len = tagsBeforeBoldUnderlinedItalic.Length;
            while (tagsBeforeBoldUnderlinedItalic[i] != tagBefore)
            {
                if ((BeforeSelectedTextIsEqual(tagsBeforeBoldUnderlinedItalic[i])) && (i < len))
                {
                    idx -= tagsBeforeBoldUnderlinedItalic[i].Length;
                    textBoxArticleContent.SelectionStart = idx;
                    textBoxArticleContent.SelectionLength += tagsBeforeBoldUnderlinedItalic[i].Length;                    
                }
                i++;
            }

            int offsetStart = 0;
            int offsetEnd = 0;

            if (BeforeSelectedTextIsEqual(tagBefore))
            { // un-style the selected text
                RemoveTagFromSelectedText(tagBefore, tagAfter);
                offsetStart = -tagBefore.Length;
            }
            else
            { // style the selected text
                AddTagToSelectedText(tagBefore, tagAfter);
                offsetStart = tagBefore.Length;
            }

            textBoxArticleContent.SelectionStart = originalSelectedStart + offsetStart;
            textBoxArticleContent.SelectionLength = originalSelectedLength + offsetEnd;
        }

        private void ReselectFullLines()
        {
            int idxStart = textBoxArticleContent.SelectionStart;
            int idxEnd = idxStart + textBoxArticleContent.SelectionLength;
            string content = textBoxArticleContent.Text;

            int lineStart = textBoxArticleContent.GetLineFromCharIndex(idxStart);
            int lineEnd = textBoxArticleContent.GetLineFromCharIndex(idxEnd);

            textBoxArticleContent.SelectionStart = textBoxArticleContent.GetFirstCharIndexFromLine(lineStart);
            textBoxArticleContent.SelectionLength = 0;
            for (int i = lineStart; i <= lineEnd; i++)
            {
                textBoxArticleContent.SelectionLength += textBoxArticleContent.Lines[i].Length + 1;
            }
        }

        private string ReplaceInsideLine(int lineNum, string oldStr, string newStr)
        {
            string content = textBoxArticleContent.Text;
            int idxStartLine = textBoxArticleContent.GetFirstCharIndexFromLine(lineNum);
            string contentBeforeLine = content.Substring(0, idxStartLine);
            int lineLen = textBoxArticleContent.Lines[lineNum].Length;
            string contentSelectedLine = content.Substring(idxStartLine, lineLen); 
            string contentAfterLine = content.Substring(idxStartLine + lineLen);

            string newLineContent = contentSelectedLine.Replace(oldStr, newStr);

            return (contentBeforeLine + newLineContent + contentAfterLine);
        }

        private void buttonOrderedUnorderedList_Click(object sender, EventArgs e)
        {
            ReselectFullLines();

            int idxStart = textBoxArticleContent.SelectionStart;
            int idxEnd = idxStart + textBoxArticleContent.SelectionLength;
            int lineStart = textBoxArticleContent.GetLineFromCharIndex(idxStart);
            int lineEnd = textBoxArticleContent.GetLineFromCharIndex(idxEnd);

            string tagName = ((Button)sender).Tag.ToString();
            string tagBefore = String.Format("<{0}>", tagName);
            const string tagListItemBefore = "<li>";
            const string tagListItemAfter = "</li>";
            string tagAfter = String.Format("</{0}>", tagName);

            //string testStr = tagBefore + tagListItemBefore;
            if (textBoxArticleContent.SelectedText.Contains(tagBefore))
            { // un-list the selected text
                idxStart = textBoxArticleContent.Text.IndexOf(tagBefore, textBoxArticleContent.SelectionStart);
                idxEnd = textBoxArticleContent.Text.IndexOf(tagAfter, idxStart);
                lineStart = textBoxArticleContent.GetLineFromCharIndex(idxStart);
                lineEnd = textBoxArticleContent.GetLineFromCharIndex(idxEnd);
                
                textBoxArticleContent.SelectionStart = idxStart;
                textBoxArticleContent.SelectionLength = idxEnd - idxStart;
                ReselectFullLines();

                string content = textBoxArticleContent.Text;
                textBoxArticleContent.Text = ReplaceInsideLine(lineEnd, tagAfter, "");
                textBoxArticleContent.Text = ReplaceInsideLine(lineStart, tagBefore, "");

                textBoxArticleContent.SelectionLength = 0;
                
                for (int i = lineStart; i <= lineEnd; i++)
                {
                    textBoxArticleContent.Text = ReplaceInsideLine(i, tagListItemAfter, "");
                    textBoxArticleContent.Text = ReplaceInsideLine(i, tagListItemBefore, "");
                    textBoxArticleContent.SelectionLength += textBoxArticleContent.Lines[i].Length;
                }
                textBoxArticleContent.SelectionStart = textBoxArticleContent.GetFirstCharIndexFromLine(lineStart);                
            }
            else if (textBoxArticleContent.SelectedText.Contains(tagListItemBefore))
            { // didn't find starting tag but found list items - doesn't support any logic
                return;
            }
            else
            { // new listing the selected text lines
                
                int lengthSum = 0;
                for (int i = lineEnd; i >= lineStart; i--)
                {
                    textBoxArticleContent.SelectionStart = textBoxArticleContent.GetFirstCharIndexFromLine(i);
                    textBoxArticleContent.SelectionLength = textBoxArticleContent.Lines[i].Length;
                    
                    AddTagToSelectedText(tagListItemBefore, tagListItemAfter);

                    lengthSum += textBoxArticleContent.Lines[i].Length+1;

                    //string oldLine = textBoxArticleContent.Lines[i];
                    //string newLine = String.Concat(tagListItemBefore, oldLine, tagListItemAfter);
                    //textBoxArticleContent.Lines[i] = newLine.ToString();
                    //textBoxArticleContent.SelectionLength += newLine.Length;
                }
                textBoxArticleContent.SelectionStart = textBoxArticleContent.GetFirstCharIndexFromLine(lineStart);
                textBoxArticleContent.SelectionLength = lengthSum;
                AddTagToSelectedText(tagBefore, tagAfter);
                textBoxArticleContent.SelectionLength += tagBefore.Length + tagAfter.Length;
                textBoxArticleContent.SelectionStart = textBoxArticleContent.GetFirstCharIndexFromLine(lineStart);
            }
        }

        private void buttonIncreaseIdent_Click(object sender, EventArgs e)
        {
            ReselectFullLines();
            int idxStart = textBoxArticleContent.SelectionStart;
            int idxEnd = idxStart + textBoxArticleContent.SelectionLength;
            int lineStart = textBoxArticleContent.GetLineFromCharIndex(idxStart);
            int lineEnd = textBoxArticleContent.GetLineFromCharIndex(idxEnd);

            const string tagBefore = "<dl><dd>";
            const string tagAfter = "</dl></dd>";
            string oldLine = textBoxArticleContent.Lines[lineStart];
            string newLine = oldLine.Insert(0, tagBefore);
            textBoxArticleContent.Lines[lineStart] = newLine;
            int len = idxEnd - idxStart;
            textBoxArticleContent.SelectionStart = idxStart + tagBefore.Length;
            oldLine = textBoxArticleContent.Lines[lineEnd];
            newLine = String.Concat(textBoxArticleContent.Lines[lineEnd], tagAfter);
            textBoxArticleContent.Lines[lineEnd] = newLine;
            textBoxArticleContent.SelectionLength = len;
        }

        private void comboBoxSymbols_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxArticleContent.Text.Insert(textBoxArticleContent.SelectionStart, comboBoxSymbols.SelectedItem.ToString());
        }

        private void buttonHyperlink_Click(object sender, EventArgs e)
        {
            if (textBoxArticleContent.SelectionLength == 0)
                return;

            ShiftSelectedTextOverTagsBoldUnderlinedItalic();
            
            int aTestIdx = textBoxArticleContent.SelectionStart - 4;
            string aTestStr = textBoxArticleContent.Text.Substring(aTestIdx, 4);
            
            if (aTestStr.StartsWith("<a"))
            { // replacing the hyperlink string
                int anchorNum = int.Parse(aTestStr.Substring(2,1));
                var editLinkForm = new FormHyperlinkData(0, anchorNum);
                // filling from current link instance
                string anchorStr = HyperlinksCollection[0][aTestStr.Substring(0, 4)];
                string attribStr = "href='";
                int idx1 = anchorStr.IndexOf(attribStr) + attribStr.Length;
                int idx2 = anchorStr.IndexOf('\'', idx1);
                editLinkForm.textBoxHref.Text = anchorStr.Substring(idx1, idx2 - idx1);
                attribStr = "name='";
                idx1 = anchorStr.IndexOf(attribStr) + attribStr.Length;
                idx2 = anchorStr.IndexOf('\'', idx1);
                editLinkForm.textBoxUrlName.Text = anchorStr.Substring(idx1, idx2 - idx1);
                attribStr = "target='";
                idx1 = anchorStr.IndexOf(attribStr);
                editLinkForm.checkBoxTarget.Checked = (idx1 >0);
                editLinkForm.textBoxHyperlinkText.Text = textBoxArticleContent.SelectedText;
            }
            else
            { // adding new hyperlink
                int anchorNum = HyperlinksCollection[0].Count;
                var editLinkForm = new FormHyperlinkData(0, anchorNum)
                                       {
                                           textBoxHyperlinkText = {Text = textBoxArticleContent.SelectedText}
                                       };
                string tagBefore = String.Format("<a{0}>", anchorNum);
                string tagAfter = String.Format("</a{0}>", anchorNum);
                AddTagToSelectedText(tagBefore, tagAfter);
            }
        }

        private void buttonDecreaseIdent_Click(object sender, EventArgs e)
        {
            ReselectFullLines();
            int idxStart = textBoxArticleContent.SelectionStart;
            int idxEnd = idxStart + textBoxArticleContent.SelectionLength;
            int lineStart = textBoxArticleContent.GetLineFromCharIndex(idxStart);
            int lineEnd = textBoxArticleContent.GetLineFromCharIndex(idxEnd);

            const string tagBefore = "<dl><dd>";
            const string tagAfter = "</dl></dd>";

            if (textBoxArticleContent.Lines[lineStart].Contains(tagBefore))
            {
                // removing starting tags
                int idx = textBoxArticleContent.Lines[lineStart].IndexOf(tagBefore);
                string oldLine = textBoxArticleContent.Lines[lineStart];
                string newLine = oldLine.Substring(0, idx + 1) + oldLine.Substring(idx + tagBefore.Length);
                textBoxArticleContent.Lines[lineStart] = newLine;
                // removing ending tags
                idx = textBoxArticleContent.Text.IndexOf(tagAfter, idxStart + idx);
                int lineNum = textBoxArticleContent.GetLineFromCharIndex(idx);
                idx = textBoxArticleContent.Lines[lineNum].IndexOf(tagAfter);
                oldLine = textBoxArticleContent.Lines[lineNum];
                newLine = oldLine.Substring(0, idx + 1) + oldLine.Substring(idx + tagAfter.Length);
                textBoxArticleContent.Lines[lineNum] = newLine;            
            }

            int len = idxEnd - idxStart;
            textBoxArticleContent.SelectionStart = idxStart - tagBefore.Length;
            textBoxArticleContent.SelectionLength = len;
        }

        private void comboBoxFontStyle_SelectedIndexChanged(object sender, EventArgs e)
        {   
            int originalSelectedStart = textBoxArticleContent.SelectionStart;
            int originalSelectedLength = textBoxArticleContent.SelectionLength;

            ShiftSelectedTextOverTagsBoldUnderlinedItalic();

            int styleNum = StylesCollection[0].Count;
            string value = String.Format("<span id='s{0}' style='{1}:{2};'>", styleNum, ((ComboBox)sender).Tag, ((ComboBox)sender).SelectedItem);
            string spanStyleTagBefore = String.Format("<s{0}>", styleNum);
            string spanTagAfter = String.Format("</s{0}>", styleNum);
            StylesCollection[0].Add(spanStyleTagBefore, value);
            int startIdx = textBoxArticleContent.SelectionStart + textBoxArticleContent.SelectionLength;
            string oldText = textBoxArticleContent.Text;
            string newText = oldText.Insert(startIdx, spanTagAfter);
            startIdx = textBoxArticleContent.SelectionStart;
            textBoxArticleContent.Text = newText.Insert(startIdx, spanStyleTagBefore);
            textBoxArticleContent.SelectionStart = originalSelectedStart + spanStyleTagBefore.Length;
            textBoxArticleContent.SelectionLength = originalSelectedLength;
        }

        private void ShiftSelectedTextOverTagsBoldUnderlinedItalic()
        {
            string[] tagsBeforeBoldUnderlinedItalic = { "<B>", "<U>", "<I>" };
            int len = tagsBeforeBoldUnderlinedItalic.Length;
            for (int i = 0; i < len; i++)
            {
                if (BeforeSelectedTextIsEqual(tagsBeforeBoldUnderlinedItalic[i]))
                {
                    textBoxArticleContent.SelectionStart -= tagsBeforeBoldUnderlinedItalic[i].Length;
                    textBoxArticleContent.SelectionLength += 1 + 2 * tagsBeforeBoldUnderlinedItalic[i].Length;
                    //textBoxArticleContent.SelectedText = tagsBeforeBUI[i] + textBoxArticleContent.SelectedText;                   
                }
            }
        }
        */

        private void buttonTitlesH1andH2_Click(object sender, EventArgs e)
        {
            int selectedStart = _singleton._richTextBoxArticleContent.SelectionStart;
            int selectedLength = _singleton._richTextBoxArticleContent.SelectionLength;

            string tagName = ((Button)sender).Text;
            string tagBefore = String.Format("\\{0}", tagName);
            string tagAfter = String.Format("\\{0}0", tagName);
            const string strikeTag = "\\strike";

            /*
            // removing old H1 or H2 selections (important to remove first the tagAfter and after that tag Before
            _richTextBoxArticleContent.Rtf = _richTextBoxArticleContent.Rtf.Replace(tagAfter, "");
            _richTextBoxArticleContent.Rtf = _richTextBoxArticleContent.Rtf.Replace(tagBefore, "");
            _richTextBoxArticleContent.Rtf = _richTextBoxArticleContent.Rtf.Replace(strikeTag, "");
            _richTextBoxArticleContent.Rtf = _richTextBoxArticleContent.Rtf.Replace("\\fs34", "\\fs20");

            _richTextBoxArticleContent.SelectionStart = selectedStart;
            _richTextBoxArticleContent.SelectionLength = selectedLength;
            */
            // updating the title textbox
            if (tagName == "H1") // h1 --> title
                _singleton._textBoxArticleTitle.Text = _singleton._richTextBoxArticleContent.SelectedText;
            else // h2 --> subtitle
                _singleton._textBoxArticleSubtitle.Text = _singleton._richTextBoxArticleContent.SelectedText;
            /*
            _richTextBoxArticleContent.SelectionColor = Color.Black;
            _richTextBoxArticleContent.SelectionAlignment = HorizontalAlignment.Right;
            _richTextBoxArticleContent.SelectionFont = new Font("Arial", 17, FontStyle.Strikeout, GraphicsUnit.Point);
            // adding new H1 or H2 selection
            int idx = _richTextBoxArticleContent.Rtf.IndexOf("\\pard");
            int tempIdx = _richTextBoxArticleContent.Rtf.IndexOf(strikeTag);
            if (idx < tempIdx)
                idx = tempIdx;
            tempIdx = _richTextBoxArticleContent.Rtf.IndexOf("\\fs34");
            if (idx < tempIdx)
                idx = tempIdx;
            _richTextBoxArticleContent.Rtf = _richTextBoxArticleContent.Rtf.Insert(idx, tagBefore);
            idx = _richTextBoxArticleContent.Rtf.IndexOf(_richTextBoxArticleContent.SelectedText, idx);
            int len = _richTextBoxArticleContent.SelectionLength;
            idx += len;
            _richTextBoxArticleContent.Rtf = _richTextBoxArticleContent.Rtf.Insert(idx, tagAfter);
             */
        }

        private void buttonArticlePreview_Click(object sender, EventArgs e)
        {
            string winpath = Environment.GetEnvironmentVariable("windir");
            string path = System.IO.Path.GetDirectoryName(
                              System.Windows.Forms.Application.ExecutablePath);

            string absPath = "C:\\KanNaim\\";
            string rtfFileName = "temp.rtf";
            string tmp = "Temp\\";
            var fileStream = new FileStream( 
                                absPath + tmp + rtfFileName, 
                                FileMode.OpenOrCreate,
                                FileAccess.Write);
            char[] chars = (@_richTextBoxArticleContent.Rtf).ToCharArray();
            byte[] buffer = new byte[chars.Length];
            int idx = 0;
            foreach (var c in chars)
            {
                buffer[idx] = Convert.ToByte(c);
                idx++;
            }
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
            fileStream.Close();

            string tag = "";// "/o";
            string exeFileName = "Rtf2Html.exe";
            //Documents and Settings\Haim\Desktop\Kan-Naim\Release\KN20100106\";
            Process p = new Process();
            p.StartInfo.WorkingDirectory = absPath;
            p.StartInfo.FileName = exeFileName;
            p.StartInfo.Arguments = @String.Format("{0}{1}{2} {3}{4} {5}",
                                        absPath, tmp, rtfFileName, absPath, tmp, tag);
            p.Start();
            p.WaitForExit();

            string htmlFileName = "temp.html";
            fileStream = new FileStream(
                absPath + tmp + htmlFileName,
                FileMode.Open,
                FileAccess.Read);
            long length = fileStream.Length;
            buffer = new byte[length];
            fileStream.Read(buffer, 0, (int)length);
            fileStream.Close();

            var browserAsForm = new FormPreviewArticleAndTaksOnBrowser
            {
                Text = "תצוגת דפדפן",
                RightToLeftLayout = true,
                RightToLeft = RightToLeft.Yes
            };
            
            var browser = browserAsForm.WebBrowser1;
            
            string result = "";
            
            foreach (var b in buffer)
            {
                result = String.Format("{0}{1}",result, Convert.ToChar(b));
            }
            browser.DocumentText = result;
            browserAsForm.Show();
        }

        private void comboBoxArticleVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO - same as photos logic (complete the video form)
        }

        private void buttonSaveArticle_Click(object sender, EventArgs e)
        {
            if ( ! ValidateFormBeforeSavingArticle())
                return;

            // saving photos in both (localy and archive)
            SaveNewPhotosClick(sender, e);

            // saving the artical
            _tblArticle.CategoryId = DataAccess.Lookup.GetLookupCategoryIdFromName(_singleton._comboBoxArticleCategory.SelectedText.Trim());
            _tblArticle.ArticleContent = _singleton._richTextBoxArticleContent.Rtf;
            _tblArticle.CreatedBy = DataAccess.Lookup.GetLookupReporterIdFromName(_singleton._comboBoxEditor.SelectedText.Trim());
            //_tblArticle.EmbedObjId = -1;
            _tblArticle.FlagActiveMivzak = _singleton._checkBoxMivzak.Checked;
            _tblArticle.FlagActiveRSS = _singleton._checkBoxRss.Checked;
            _tblArticle.FlagShowDateTime = _singleton._checkBoxDateTime.Checked;
            _tblArticle.FlagTak1ColPic = _singleton._userControlTakFillSizeX1.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTak1ColPicTxt = false;
            _tblArticle.FlagTak1ColTxt = false;
            _tblArticle.FlagTak2ColPic = _singleton._userControlTakFillSizeX2.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTak2ColPicTxt = false;
            _tblArticle.FlagTak2ColTxt = false;
            _tblArticle.FlagTak3ColPic = _singleton._userControlTakFillSizeX3.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTak3ColPicTxt = false;
            _tblArticle.FlagTak3ColTxt = false;
            _tblArticle.FlagTakLineFeeds = _singleton._checkBoxMivzak.Checked; //TODO - maybe other aditional checkbox
            _tblArticle.FlagTakSmallPic = _singleton._userControlTakFillSizeSmall.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTakSmallPicTxt = false;
            _tblArticle.FlagTakSmallTxt = false;
            _tblArticle.ImageId = DataAccess.Filter.GetOriginalPhotoIdByPhotoName(_singleton._comboBoxArticlePhoto.SelectedText.Trim());
            _tblArticle.IsPublished = _singleton._checkBoxPublish.Checked;
            _tblArticle.KeysAssociated = "";
            _tblArticle.KeysLookup = _singleton._textBoxKeyWords.Text;
            foreach (object obj in _singleton._listBoxSelectedCategories.Items)
            {
                _tblArticle.KeysAssociated += String.Format(" | {0}", (string) obj);
            }
            _tblArticle.KeysLookup += _tblArticle.KeysAssociated;
            _tblArticle.MetaTags = _singleton._textBoxTags.Text;
            _tblArticle.StatusId = 0; //TODO - TBD...
            _tblArticle.SubTitle = _singleton._textBoxArticleSubtitle.Text;
            _tblArticle.Summery = "";
            _tblArticle.Title = _singleton._textBoxArticleTitle.Text;
            _tblArticle.UpdatedBy = DataAccess.Lookup.GetLookupReporterIdFromName(_singleton._comboBoxEditor.SelectedText.Trim());
            // specific values
            DateTime now = DateTime.Now;
            _tblArticle.UpdateDate = now;

            try
            {
                _tblArticle.ArticleId = DataAccess.Insert.TableArticles(_tblArticle);
                if (_tblArticle.ArticleId > 0)
                    _isNewArticle = false;
            }
            catch(Exception exception)
            {
                Messages.ExceptionMessage(exception, "שמירת מאמר נכשלה");
            }    
            
            try
            {
                // saving taktzirim
                foreach (UserControlTakFill userControlTakFill in UcTakFillCollection.Values)
                {
                    if (userControlTakFill.IsEnabled() &&
                        userControlTakFill.ValidateValuesBeforeSave())
                    {
                        int newTakId = userControlTakFill.SaveToDatabase(_tblArticle.ArticleId, -1);
                        
                        if (newTakId > 0)
                            MessageBox.Show("נשמר תקציר מספר פנימי " + newTakId, "הצלחה");
                    }
                }
            }
            catch(Exception exception)
            {
                Messages.ExceptionMessage(exception, "שמירת תקציר נכשלה");
            }
        }

        private bool ValidateFormBeforeSavingArticle()
        {
            if (_singleton._comboBoxArticleCategory.SelectedIndex < 0)
            {
                MessageBox.Show("לא נבחרה קטגוריה !!!");
                return false;
            }
            if (_singleton._comboBoxArticlePhoto.SelectedIndex < 0)
            {
                MessageBox.Show("לא נבחרה תמונה לכתבה !!!");
                return false;
            }
            if (_singleton._textBoxArticleTitle.TextLength < 5)
            {
                MessageBox.Show("כותרת הכתבה קצרה מדי !!!");
                return false;
            }
            if (_singleton._textBoxArticleSubtitle.TextLength < 5)
            {
                MessageBox.Show("כותרת משנית של הכתבה קצרה מדי !!!");
                return false;
            }
            if (_singleton._richTextBoxArticleContent.TextLength < 10)
            {
                MessageBox.Show("תוכן הכתבה קצרה מדי !!!");
                return false;
            } 
            if (_singleton._comboBoxEditor.SelectedIndex < 0)
            {
                MessageBox.Show("לא נבחר כתב !!!");
                return false;
            }

            foreach (var userControlTakFill in UcTakFillCollection.Values)
            {
                if (userControlTakFill.IsEnabled() &&
                    ( ! userControlTakFill.ValidateValuesBeforeSave()))
                {
                    MessageBox.Show("תקציר לא מולא כראוי !!!");
                    return false;
                }
            }

            return true;
        }

        private void ButtonSaveVideoToArchiveClick(object sender, EventArgs e)
        {
            var newRow = new Table_VideosArchive
                             {
                                 AlternateText = _singleton._ucUploadVideo1._textBoxVideoAlternateText.Text,
                                 Caption = _singleton._ucUploadVideo1._textBoxVideoCaption.Text,
                                 Description = _singleton._ucUploadVideo1._textBoxVideoDescription.Text,
                                 EmbedUrl = _singleton._ucUploadVideo1._textBoxVideoEmbedUrl.Text,
                                 CategoryId =
                                     DataAccess.Lookup.GetLookupCategoryIdFromName(
                                     _singleton._comboBoxArticleCategory.SelectedText.Trim()),
                                 Date = DateTime.Now,
                                 UrlLink = "",
                                 CssClass = "EmbedVideo"
                             };

            Db.Table_VideosArchives.InsertOnSubmit(newRow);
            Db.SubmitChanges();

        }

        private void userControlUploadVideo1_Load(object sender, EventArgs e)
        {
            _singleton._ucUploadVideo1.SetCallbackFunction(ButtonSaveVideoToArchiveClick);
        }

        private void tabPageCategories_Enter(object sender, EventArgs e)
        {
            buttonReloadCategoryTree_Click(_singleton._buttonReloadCategoryTree, e);
        }

        private void buttonCopyTitleToTak_Click(object sender, EventArgs e)
        {
            foreach (var userControlTakFill in UcTakFillCollection.Values)
            {
                userControlTakFill.SetTitleText(_textBoxArticleTitle.Text);
            }
        }

        private void buttonCopySubtitelToTak_Click(object sender, EventArgs e)
        {
            foreach (var userControlTakFill in UcTakFillCollection.Values)
            {
                userControlTakFill.SetContentText(_textBoxArticleSubtitle.Text);
            }
        }

        private void FormEditArtical_FormClosing(object sender, FormClosingEventArgs e)
        {
            _singleton = null;
        }
    }
}

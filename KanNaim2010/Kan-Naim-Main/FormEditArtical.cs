using System;
using System.Collections.Generic;
using System.Data;
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
        
        private static readonly FormEditArtical Singleton = new FormEditArtical();

        private FormEditArtical()
        {
            //this.ucUploadPhoto1 = ucUploadPhoto1;
        }

        private static void InitializeForm(string category, string username)
        {
            Singleton.InitializeComponent();
            Singleton._tableLookupCategoriesTableAdapter.Fill(Singleton._kanNaimDataSetCategories.Table_LookupCategories);
            Singleton._comboBoxArticleCategory.SelectedIndex = Singleton._comboBoxArticleCategory.FindString(category);
            Singleton._tableLookupReportersTableAdapter1.Fill(Singleton._kanNaimDataSetReportersNames.Table_LookupReporters);
            Singleton._comboBoxEditor.SelectedIndex = Singleton._comboBoxEditor.FindString(username);

            UcTakFillCollection.Add("takMedium",Singleton._userControlTakFillSizeMedium);
            UcTakFillCollection.Add("takSmall",Singleton._userControlTakFillSizeSmall);
            UcTakFillCollection.Add("takX1",Singleton._userControlTakFillSizeX1);
            UcTakFillCollection.Add("takX2",Singleton._userControlTakFillSizeX2);
            UcTakFillCollection.Add("takX3",Singleton._userControlTakFillSizeX3);
        }
        private static int GetCetegoryIdFromName(string hebrewName)
        {
            try
            {
                int catId = (from c in Db.Table_LookupCategories
                             where c.CatHebrewName.Trim() == hebrewName.Trim()
                             select c.CatId).Single();
                return catId;
            }
            catch 
            {
                return -1;
            }
        }

        public static FormEditArtical GetFormEditNewArtical(string category, string username)
        {
            InitializeForm(category, username);

            _tblArticle = CreateNewArticleInArchive();

            return Singleton;
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
            try
            {
                _tblArticle = (Db.Table_Articles
                    .Where(c => c.ArticleId == articleId))
                    .Single();

                PopulateFromArticle();
                _isNewArticle = false;
                return Singleton;
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
            Singleton._comboBoxArticleCategory.SelectedText =
                DataAccess.Lookup.GetLookupCategoryNameFromId(_tblArticle.CategoryId);
            Singleton._richTextBoxArticleContent.Rtf = _tblArticle.ArticleContent;
            Singleton._comboBoxEditor.SelectedText =
                DataAccess.Lookup.GetLookupReporterFromUserId(_tblArticle.UpdatedBy).PublishNameShort;
            Singleton._ucUploadVideo1.PopulateByVideoId(_tblArticle.EmbedObjId);
            Singleton._ucUploadPhoto1.PopulateByOriginalPhotoId(_tblArticle.ImageId);
            Singleton._checkBoxPublish.Checked = _tblArticle.IsPublished;
            Singleton._textBoxKeyWords.Text = _tblArticle.KeysAssociated;
            Singleton._textBoxTags.Text = _tblArticle.MetaTags;
            Singleton._textBoxArticleTitle.Text = _tblArticle.Title;
            Singleton._textBoxArticleSubtitle.Text = _tblArticle.SubTitle;
            char[] splitChars = {'|'};
            string[] keysLookup = _tblArticle.KeysLookup.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            Singleton._listBoxSelectedCategories.Items.Clear();
            foreach (string key in keysLookup)
            {
                Singleton._listBoxSelectedCategories.Items.Add(key.Trim());
            }
            // change date to 'Now' only when saving 
            //DateTime now = DateTime.Now;
            //_tblArticle.CreateDate = now;
            //_tblArticle.UpdateDate = now;
            Singleton._comboBoxArticlePhoto = FillComboBoxWithPhotosArchive(Singleton._comboBoxArticlePhoto);
            comboBoxArticlePhoto_SelectedIndexChanged(Singleton._comboBoxArticleCategory, new EventArgs());
        }

        private static void FillTablesOriginalPhotosRecord(FileInfo info)
        { // use only with original photos

            byte[] imageData = UserControlUploadPhoto.GetFileContentFromInfo(info);
            
            // original photos table
            _tblOriginalPhotos = new Table_OriginalPhotosArchive
                                     {
                                         AlternateText = Singleton._ucUploadPhoto1.textBoxPhotoDescription.Text,
                                         Caption = Singleton._ucUploadPhoto1.textBoxPhotoCaption.Text,
                                         CategoryId = GetCetegoryIdFromName(Singleton._comboBoxArticleCategory.SelectedText),
                                         Date = DateTime.Now,
                                         Description = Singleton._ucUploadPhoto1.textBoxPhotoDescription.Text
                                     };
            var pb = new PictureBox
                         {
                             Image = UserControlUploadPhoto.GetImageFromFileInfo(info)
                         };
            _tblOriginalPhotos.ImageData = imageData;
            string[] pathParts = Singleton._ucUploadPhoto1.textBoxPhotoPath.Text.Split('\\', '/', '.');
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
                                                   Singleton._ucUploadPhoto1.textBoxPhotoCaption.Text),
                                 OriginalPhotoId = _tblOriginalPhotos.PhotoId,
                                 GalleryId = null,
                                 PhotoTypeId = 1
                             };
            _tblPhotos.UrlLink = _tblPhotos.ImageUrl;
            _tblPhotos.Width = _tblOriginalPhotos.Width;
            _tblPhotos.Height = _tblOriginalPhotos.Height;
        }
        
        private static void UpLoadImageFileAndSaveToDatabase(FileInfo info)
        {
            try
            {
                FillTablesOriginalPhotosRecord(info);
                Db.Table_OriginalPhotosArchives.InsertOnSubmit(_tblOriginalPhotos);
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

        // saving in both (localy and archive)
        private static void SaveNewPhotosClick(object sender, EventArgs e)
        {     
            Singleton._ucUploadPhoto1.SavePhotosLocally(); // saving locally

            if ( ! Singleton._ucUploadPhoto1.IsStateEqual(UserControlUploadPhoto.UploadState.SavedLocalyOk))
                return;

            if (Singleton._ucUploadPhoto1.radioButtonSavePhotosToArchive.Checked)
            {
                // saving in SQL Server Table_Photos
                var imageInfo = new FileInfo(Singleton._ucUploadPhoto1._photoPath);
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

                // updating the images combobox 
                var photosByCategory = DataAccess.Filter.GetOriginalPhotosByCategoryName(Singleton._comboBoxArticleCategory.SelectedText.Trim());
                var photosNames = (from c in photosByCategory
                                   where Singleton._ucUploadPhoto1.textBoxPhotoPath.Text.Contains(c.Name)
                                   orderby c.Date descending
                                   select c.Name);
                Singleton._comboBoxArticlePhoto.Items.Insert(0, photosNames.First());
                Singleton._comboBoxArticlePhoto.SelectedIndex = 0;
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

        private static void FormEditArtical_Load(object sender, EventArgs e)
        {
            Singleton._tablePhotosArchiveTableAdapter.Fill(Singleton._kanNaimDataSet1.Table_PhotosArchive);
            Singleton._tableLookupArticleStatusTableAdapter.Fill(Singleton._kanNaimDataSet.Table_LookupArticleStatus);
            //_comboBoxArticleCategory.DataSource = DataAccess.Filter.Get

        }

        private static void buttonClearCategoriesList_Click(object sender, EventArgs e)
        {
            for (int i = Singleton._listBoxSelectedCategories.Items.Count; i > 0; i--)
            {
                RemoveItemTakCatTreeSelectorAt(i - 1);
            }
            Singleton._listBoxSelectedCategories.Items.Clear();
        }
        private static void buttonManageCategories_Cilck(object sender, EventArgs e)
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

        private static void buttonRemoveSelectedCategory_Click(object sender, EventArgs e)
        {
            int length = Singleton._listBoxSelectedCategories.Items.Count;

            for (int i = length-1 ; i >= 0; i--)
			{
			    var catItem = (string)Singleton._listBoxSelectedCategories.Items[i];
                //string catName = catItem.Split('#')[0].Trim('#', ' ');
                if (Singleton._listBoxSelectedCategories.SelectedItems.Contains(catItem))
                {
                    Singleton._listBoxSelectedCategories.Items.RemoveAt(i);
                    RemoveItemTakCatTreeSelectorAt(i);
                    return;
                }		        
	        } 
        }

        private static void buttonAddSelectedCategories_Click(object sender, EventArgs e)
        {
            foreach (TreeNode siblingNode in Singleton._userControlTreeView1.Nodes)
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

        private static void ToolStripMenuItemAddCategory_Click(object sender, EventArgs e)
        {
            TreeNode node = Singleton._userControlTreeView1.SelectedNode;

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

        private static void ToolStripMenuItemDeleteCategory_Click(object sender, EventArgs e)
        {
            TreeNode node = Singleton._userControlTreeView1.SelectedNode;

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
            return (string)((DataRowView)Singleton._comboBoxArticleCategory.SelectedItem)["CatHebrewName"];
        }
        private static void RefreshCategoryComboByValue(string value)
        {
            Singleton._tableLookupCategoriesTableAdapter.Fill(Singleton._kanNaimDataSetCategories.Table_LookupCategories);
            Singleton._comboBoxArticleCategory.SelectedIndex = Singleton._comboBoxArticleCategory.FindString(value);
        }

        private static void buttonReloadCategoryTree_Click(object sender, EventArgs e)
        {
            Singleton._userControlTreeView1.PopulateRootLevel("Table_LookupCategories", "ParentCatId");

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
                    Singleton._listBoxSelectedCategories.Items.Add(newItem);
                    AddItemToTakCatTreeSelector(newItem);                    
                }

                foreach (TreeNode siblingNode in node.Nodes)
                {
                    AddSelectedTreeNodesToList(isNotConditional, siblingNode);
                }
        }
        private static void buttonAddAllCategories_Click(object sender, EventArgs e)
        {
            foreach (TreeNode siblingNode in Singleton._userControlTreeView1.Nodes)
            {
                AddSelectedTreeNodesToList(true, siblingNode);
            }
        }

        private static void comboBoxArticleCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: bind videos list in drop down list

            var photos = DataAccess.Filter.GetOriginalPhotosByCategoryName(Singleton._comboBoxArticleCategory.SelectedText);

            // TODO - bind photos in drop down list
            //_comboBoxArticlePhoto.Items.Clear();

            //foreach (Table_OriginalPhotosArchive tableOriginalPhotosArchive in photos)
            //{
            //    _comboBoxArticlePhoto.Items.Add(tableOriginalPhotosArchive.Name);
            //}
        }

        private static ComboBox FillComboBoxWithPhotosArchive(ComboBox comboBox)
        {
            int originalPhotoId;

            try
            {
                originalPhotoId =
                    DataAccess.Filter.
                    GetOriginalPhotoIdByPhotoName(Singleton._comboBoxArticlePhoto.SelectedText);
            }
            catch
            {
                return comboBox;
            }

            var relatedPhotosArchive =
                DataAccess.Filter.GetPhotosArchiveByOriginalPhotoId(originalPhotoId);

            //ComboBox combo = userControlTakFillSizeX3.ucTakContent1.comboBoxTakPhoto;
            comboBox.Items.Clear();

            var types = DataAccess.Lookup.GetLookupPhotoTypes();

            foreach (Table_PhotosArchive tablePhotosArchive in relatedPhotosArchive)
            {
                int photoTypeId = tablePhotosArchive.PhotoTypeId;
                string photoTypeDescription = (from c in types
                                               where c.PhotoTypeId == photoTypeId
                                               select c.TypeDescription).Single();
                string itemText = String.Format("{0} - {1}", Singleton._comboBoxArticlePhoto.SelectedText, photoTypeDescription);
                comboBox.Items.Add(itemText);
            }
            return comboBox;
        }

        private static void comboBoxArticlePhoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (UserControlTakFill userControlTakFill in UcTakFillCollection.Values)
            {
                ComboBox combo = userControlTakFill.ucTakContent1.comboBoxTakPhoto;
                userControlTakFill.ucTakContent1.comboBoxTakPhoto = FillComboBoxWithPhotosArchive(combo);
            }
        }

        public static void UpdateHtmlTextIntoArticleContentTextBox(string rtfContent)
        {
            Singleton._richTextBoxArticleContent.Rtf = rtfContent;
        }

        private static void buttonOpenEditor_Click(object sender, EventArgs e)
        {
            Singleton._richTextBoxArticleContent.Enabled = !Singleton._richTextBoxArticleContent.Enabled;
            var updateFunctionCallback = new FormArticleRichTextBoxEditor.ReturnHtmlTextCallbackType(UpdateHtmlTextIntoArticleContentTextBox);

            var articleEditorForm = new FormArticleRichTextBoxEditor(Singleton._richTextBoxArticleContent.Rtf, updateFunctionCallback);

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

        private static void buttonTitlesH1andH2_Click(object sender, EventArgs e)
        {
            int selectedStart = Singleton._richTextBoxArticleContent.SelectionStart;
            int selectedLength = Singleton._richTextBoxArticleContent.SelectionLength;

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
                Singleton._textBoxArticleTitle.Text = Singleton._richTextBoxArticleContent.SelectedText;
            else // h2 --> subtitle
                Singleton._textBoxArticleSubtitle.Text = Singleton._richTextBoxArticleContent.SelectedText;
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

        private static void buttonArticlePreview_Click(object sender, EventArgs e)
        {
            var browserAsForm = new FormPreviewArticleAndTaksOnBrowser();
            var browser = browserAsForm.WebBrowser1;

            browser.DocumentText = Singleton._richTextBoxArticleContent.Text;
            browserAsForm.Show();
        }

        private static void comboBoxArticleVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO - same as photos logic (complete the video form)
        }

        private static void buttonSaveArticle_Click(object sender, EventArgs e)
        {
            // saving photos in both (localy and archive)
            SaveNewPhotosClick(sender, e);

            // saving the artical
            _tblArticle.CategoryId = DataAccess.Lookup.GetLookupCategoryIdFromName(Singleton._comboBoxArticleCategory.SelectedText.Trim());
            _tblArticle.ArticleContent = Singleton._richTextBoxArticleContent.Rtf;
            _tblArticle.CreatedBy = DataAccess.Lookup.GetLookupReporterIdFromName(Singleton._comboBoxEditor.SelectedText.Trim());
            //_tblArticle.EmbedObjId = -1;
            _tblArticle.FlagActiveMivzak = Singleton._checkBoxMivzak.Checked;
            _tblArticle.FlagActiveRSS = Singleton._checkBoxRss.Checked;
            _tblArticle.FlagShowDateTime = Singleton._checkBoxDateTime.Checked;
            _tblArticle.FlagTak1ColPic = Singleton._userControlTakFillSizeX1.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTak1ColPicTxt = false;
            _tblArticle.FlagTak1ColTxt = false;
            _tblArticle.FlagTak2ColPic = Singleton._userControlTakFillSizeX2.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTak2ColPicTxt = false;
            _tblArticle.FlagTak2ColTxt = false;
            _tblArticle.FlagTak3ColPic = Singleton._userControlTakFillSizeX3.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTak3ColPicTxt = false;
            _tblArticle.FlagTak3ColTxt = false;
            _tblArticle.FlagTakLineFeeds = Singleton._checkBoxMivzak.Checked; //TODO - maybe other aditional checkbox
            _tblArticle.FlagTakSmallPic = Singleton._userControlTakFillSizeSmall.ucTakContent1.checkBoxTakPhoto.Checked; 
            //_tblArticle.FlagTakSmallPicTxt = false;
            _tblArticle.FlagTakSmallTxt = false;
            _tblArticle.ImageId = DataAccess.Filter.GetOriginalPhotoIdByPhotoName(Singleton._comboBoxArticlePhoto.SelectedText.Trim());
            _tblArticle.IsPublished = Singleton._checkBoxPublish.Checked;
            _tblArticle.KeysAssociated = "";
            _tblArticle.KeysLookup = Singleton._textBoxKeyWords.Text;
            foreach (object obj in Singleton._listBoxSelectedCategories.Items)
            {
                _tblArticle.KeysLookup += String.Format(" | {0}", (string) obj);
            }
            _tblArticle.MetaTags = Singleton._textBoxTags.Text;
            _tblArticle.StatusId = 0; //TODO - TBD...
            _tblArticle.SubTitle = Singleton._textBoxArticleSubtitle.Text;
            _tblArticle.Summery = "";
            _tblArticle.Title = Singleton._textBoxArticleTitle.Text;
            _tblArticle.UpdatedBy = DataAccess.Lookup.GetLookupReporterIdFromName(Singleton._comboBoxEditor.SelectedText.Trim());
            // specific values
            DateTime now = DateTime.Now;
            _tblArticle.UpdateDate = now;

            try
            {
                if (_isNewArticle)
                {
                    Db.Table_Articles.InsertOnSubmit(_tblArticle);
                }
                Db.SubmitChanges();
                _tblArticle = (from c in Db.Table_Articles
                               where c.CreateDate == now
                               select c).Single();
                _isNewArticle = false;

                // saving taktzirim
                // TODO !
            }
            catch
            {
                
            }
        }

        private static void ButtonSaveVideoToArchiveClick(object sender, EventArgs e)
        {
            var newRow = new Table_VideosArchive
                             {
                                 AlternateText = Singleton._ucUploadVideo1._textBoxVideoAlternateText.Text,
                                 Caption = Singleton._ucUploadVideo1._textBoxVideoCaption.Text,
                                 Description = Singleton._ucUploadVideo1._textBoxVideoDescription.Text,
                                 EmbedUrl = Singleton._ucUploadVideo1._textBoxVideoEmbedUrl.Text,
                                 CategoryId =
                                     DataAccess.Lookup.GetLookupCategoryIdFromName(
                                     Singleton._comboBoxArticleCategory.SelectedText.Trim()),
                                 Date = DateTime.Now,
                                 UrlLink = "",
                                 CssClass = "EmbedVideo"
                             };

            Db.Table_VideosArchives.InsertOnSubmit(newRow);
            Db.SubmitChanges();

        }

        private static void userControlUploadVideo1_Load(object sender, EventArgs e)
        {
            Singleton._ucUploadVideo1.SetCallbackFunction(ButtonSaveVideoToArchiveClick);
        }
    }
}

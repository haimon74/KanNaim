using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Kan_Naim_Main
{
    public partial class Form_CategoriesManager : Form
    {
        private enum CategoryEditorStateEnum { VIEW = 0, ADD = 1, EDIT = 2 }
        
        //private SqlConnection objConn = new SqlConnection("Data Source=PRIVATE-DCB1672;Initial Catalog=Kan-Naim;Integrated Security=True");
        private SqlConnection objConn = new SqlConnection("Data Source=sql02.intervision.co.il;Initial Catalog=10info;Persist Security Info=True;User ID=10info;Password=4a7RxLszj");

        private static DataClassesKanNaimDataContext _db = new DataClassesKanNaimDataContext();
        private static Table_LookupCategory _linqLookupCategoryTableRow = new Table_LookupCategory();
        private static TreeNode selectedTreeNode = new TreeNode();
        private static TreeNode newNode = new TreeNode();
        private static CategoryEditorStateEnum _formState = CategoryEditorStateEnum.VIEW;

        private static Table_LookupCategory GetCategoryDataObjectById(int catId)
        {
            return (from c in _db.Table_LookupCategories
                    where c.CatId == catId
                    select c).Single();
        }
        private static void InsertNewCategoryDataObject(Table_LookupCategory catLinqObj)
        {
            _db.Table_LookupCategories.InsertOnSubmit(catLinqObj);
            _db.SubmitChanges();
        }
        private static void ChangeCategoryParentToGrandParent(int catId)
        {
            Table_LookupCategory catLinqObj = GetCategoryDataObjectById(catId);
            Table_LookupCategory parentLinqObj = GetCategoryDataObjectById(catLinqObj.ParentCatId);
            catLinqObj.ParentCatId = parentLinqObj.ParentCatId;
            _db.SubmitChanges();
        }
        private DataTable GetTableFromQuery(string qry)
        {
            SqlCommand objCommand = new SqlCommand(qry, objConn);
            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            return dt;
        }
        private TreeView PopulateRootLevel()
        {
            DataTable dt = GetTableFromQuery("select * FROM Table_LookupCategories WHERE ParentCatId='-1'");
            TreeView tv = this.treeViewCategories;
            if ((dt == null) || (dt.Rows.Count == 0))
            {
                // create main category
                _linqLookupCategoryTableRow = new Table_LookupCategory();
                _linqLookupCategoryTableRow.CatHebrewName = "עמוד ראשי";
                _linqLookupCategoryTableRow.CatEnglishName = "Main|Home Page";
                _linqLookupCategoryTableRow.ParentCatId = -1;
                _db.Table_LookupCategories.InsertOnSubmit(_linqLookupCategoryTableRow);
                _db.SubmitChanges();
                dt = GetTableFromQuery("select * FROM Table_LookupCategories WHERE ParentCatId='-1'");
            }
            PopulateNodes(dt, tv.Nodes);
            return tv;
        }
        private void PopulateNodes(DataTable dt, TreeNodeCollection nodes)
        {
            if ((dt == null) || (dt.Rows.Count == 0))
                return;

            if (nodes == null)
            {
                TreeNode tn = new TreeNode();
                tn.Text = "אין קטגוריות";
                tn.ToolTipText = "-1";
                nodes.Add(tn);
            }

            foreach (DataRow dr in dt.Rows)
            {
                string name = dr["CatHebrewName"].ToString().Trim();
                string id = dr["CatId"].ToString().Trim();
                TreeNode tn = new TreeNode();
                tn.Text = name;
                tn.ToolTipText = id;
                try
                {
                    nodes.Add(tn);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                DataTable childDT = GetTableFromQuery(String.Format("select * FROM Table_LookupCategories WHERE ParentCatId='{0}'", id));
                PopulateNodes(childDT, tn.Nodes);
            }
        }
        private void PopulateSubLevel(int pId, ref TreeNode pNode)
        {
        }
        

        public Form_CategoriesManager()
        {
            InitializeComponent();

            ResetForm(true);
            

            ////this.buttonSaveChanges.Enabled = false;
            
            ////this.checkBoxEnableDelete.Enabled = false;
            ////this.buttonDeleteCategory.Enabled = false;

            ////this.buttonSaveChanges.Enabled = false;
            ////this.buttonAddRelated.Enabled = true;
            //this.textBoxKeys.Enabled = true;
            //this.textBoxTags.Enabled = true;
            //this.listBoxRelatedCategories.Enabled = true;
            //this.textBoxCategoryNameEnglish.Enabled = true;
            //this.textBoxCategoryNameHebrew.Enabled = true;
            
            //upload the current categories from the DB
            //DataClassesCategoriesDataContext _db = GlobalValiables.Db;
            //var categories =  from c in Db.Table_LookupCategories
            //                    select c;

            //var root = from c in categories
            //           where c.ParentCatId == null
            //           select c;
            
            //foreach (Table_LookupCategory cat in categories)
            //{
            //    TreeNode tn = new TreeNode(cat.CatHebrewName + " | " + cat.CatEnglishName + " :: " + cat.CatId.ToString());
            //    tn.ToolTipText = String.Format("[{}] [{1}] [{2}]", cat.MetaTags, cat.RelatedCatIds, cat.Tags);
            //}
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private TreeNode AddSibling(string parentName, string siblingName)
        {
            TreeNode parent = this.treeViewCategories.SelectedNode;
            TreeNode newNode = parent.Nodes.Insert(parent.Nodes.Count, siblingName);
            //parent.TreeView.ExpandAll();

            return newNode;
        }
        
        //private void ToolStripMenuItemEditCategory_Click(object sender, EventArgs e)
        //{
        //    selectedTreeNode = treeViewCategories.Nodes[0];

        //    if (selectedTreeNode.ToolTipText == "-1") 
        //        return;

        //    _formState = CategoryEditorStateEnum.EDIT;

        //    try
        //    {
        //        _linqLookupCategoryTableRow = (from c in _db.Table_LookupCategories
        //                                       where c.CatId == int.Parse(selectedTreeNode.ToolTipText)
        //                                       select c).Single();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    this.buttonSaveChanges.Enabled = true;
            
        //    // fetch KEYS
        //    this.textBoxKeys.Text = _linqLookupCategoryTableRow.Description;
        //    // fetch TAGS
        //    this.textBoxTags.Text = _linqLookupCategoryTableRow.MetaTags;
        //    // fetch RELATED CATEGORIES
            
        //    listBoxRelatedCategories.Items.Clear();
        //    if (_linqLookupCategoryTableRow.RelatedCatIds != null)
        //    {
        //        string[] relatedCategoriesIds = _linqLookupCategoryTableRow.RelatedCatIds.Split('#');

        //        foreach (string item in relatedCategoriesIds)
        //        {
        //            try
        //            {
        //                string relatedCatStr = (from c in _db.Table_LookupCategories
        //                                        where c.CatId == int.Parse(item.Trim('#',' '))
        //                                        select c.CatHebrewName).Single();

        //                listBoxRelatedCategories.Items.Add(relatedCatStr);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    this.textBoxCategoryNameEnglish.Text = _linqLookupCategoryTableRow.CatEnglishName;
        //    this.textBoxCategoryNameHebrew.Text = _linqLookupCategoryTableRow.CatHebrewName; 

        //    this.buttonSaveChanges.Enabled = true;

        //    // only for delete need explicitly enabled
        //    this.buttonDeleteCategory.Enabled = false;
        //}

        private void buttonAddRelated_Click(object sender, EventArgs e)
        {
            int itemsCount = this.listBoxRelatedCategories.Items.Count;
            bool smallList = (itemsCount < 6);
            if (smallList)
            {
                try
                {
                    if (treeViewCategories.SelectedNode == null)
                        return;

                    string newItemName = treeViewCategories.SelectedNode.Text;
                    this.listBoxRelatedCategories.Items.Insert(itemsCount, newItemName);
                    this.listBoxRelatedCategories.Refresh();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return;
        }
        private bool NodeNameExistInSiblings(TreeNode parent)
        {
            // checking category's Hebrew name doesn't exist in siblings
            if (parent == null)
                return false;
            if (parent.Nodes == null)
                return false;

            foreach (TreeNode sibling in parent.Nodes)
            {
                if (parent.Nodes.Contains(new TreeNode(textBoxCategoryNameHebrew.Text)))
                {
                    MessageBox.Show("category already exist as sibling");
                    return true;
                }
            }
            return false;
        }
        
        private void FillFromFormAndSaveToDB(int parentCatId)
        {
            if (_formState == CategoryEditorStateEnum.EDIT)
            {  // editing is on the "parent" (=selected) node
                // fetch record from DB
                _linqLookupCategoryTableRow = GetCategoryDataObjectById(parentCatId); //(from c in _db.Table_LookupCategories
                                                                                       //where c.CatId == parentCatId
                                                                                       //select c).Single();
            }
            else if (_formState == CategoryEditorStateEnum.ADD)
            {
                _linqLookupCategoryTableRow = new Table_LookupCategory();
            }

            //filling data from form
            _linqLookupCategoryTableRow.CatHebrewName = textBoxCategoryNameHebrew.Text;
            _linqLookupCategoryTableRow.CatEnglishName = textBoxCategoryNameEnglish.Text;
            _linqLookupCategoryTableRow.MetaTags = textBoxTags.Text;
            _linqLookupCategoryTableRow.RelatedCatIds = "";
            // fetching related cats from listbox
            foreach (object item in listBoxRelatedCategories.Items)
            {
                int idx = this.treeViewCategories.Nodes.IndexOfKey((string)item);
                int relatedCatId = int.Parse(this.treeViewCategories.Nodes[idx].ToolTipText);
                _linqLookupCategoryTableRow.RelatedCatIds += String.Format("{0}#", relatedCatId);
            }

            _linqLookupCategoryTableRow.RssId = _linqLookupCategoryTableRow.ParentCatId; // TBD - implement RSS support
            _linqLookupCategoryTableRow.Description = textBoxKeys.Text;

            try
            {
                if (_formState == CategoryEditorStateEnum.ADD)
                {
                    _linqLookupCategoryTableRow.ParentCatId = parentCatId;
                    // inserting the new category into the DB
                    _db.Table_LookupCategories.InsertOnSubmit(_linqLookupCategoryTableRow);
                }
                
                _db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }
        private void AddTreeNodeSiblingFromForm(TreeNode node)
        {
            if (NodeNameExistInSiblings(node))
                return;

            // ELSE:

            buttonSaveChanges.Enabled = false;

            try
            {
                int parentId = int.Parse(node.ToolTipText);

                if (_formState == CategoryEditorStateEnum.ADD)
                {
                    // add the node to tree
                    Form_CategoriesManager.newNode = node.Nodes.Add(textBoxCategoryNameHebrew.Text);

                    FillFromFormAndSaveToDB(parentId);

                    // fetching the new Category from DB to get its CatId
                    _linqLookupCategoryTableRow = (from c in _db.Table_LookupCategories
                                                   where c.CatHebrewName.Contains(textBoxCategoryNameHebrew.Text)
                                                   select c).Single();

                    newNode.ToolTipText = _linqLookupCategoryTableRow.CatId.ToString();
                    
                    _formState = CategoryEditorStateEnum.EDIT;
                }
                else if (_formState == CategoryEditorStateEnum.EDIT)
                {
                    FillFromFormAndSaveToDB(parentId);
                    selectedTreeNode.Text = textBoxCategoryNameHebrew.Text;
                }
            }
            catch (Exception ex)
            {
                buttonSaveChanges.Enabled = true;
                MessageBox.Show(ex.Message ,  "חלה שגיאה בהוספת קטגוריה" , MessageBoxButtons.OK , MessageBoxIcon.Error);
            }            
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            TreeView tv = GlobalValiables._gCategoriesTreeView;

            // selectedTreeNode = this.treeViewCategories.SelectedNode;
            TreeNode node = selectedTreeNode;

            // difrent logic for add and edit states is inside func AddTreeNodeSiblingFromForm(...)
            AddTreeNodeSiblingFromForm(node);
            
            //node.Nodes.[textBoxCategoryNameHebrew.Text].Text =  (TreeNode)treeViewCategories.Nodes[0].Clone();
            
            tv = this.treeViewCategories;
            
            ResetForm(true);
        }

        private void treeViewCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_formState == CategoryEditorStateEnum.VIEW)
            {
                selectedTreeNode = treeViewCategories.SelectedNode;
                selectedTreeNode.BackColor = Color.Pink;
            }

            if ((_formState == CategoryEditorStateEnum.EDIT))
            // EDIT (- means update or delete)
            {
                FillFormWithSelectedNodeData();
            }                
        }

        //private void ToolStripMenuItemAddCategory_Click(object sender, EventArgs e)
        //{
        //    _formState = CategoryEditorStateEnum.ADD;

        //    this.buttonSaveChanges.Enabled = true;
            
        //    // clear KEYS
        //    this.textBoxKeys.Text = "";
        //    // clear TAGS
        //    this.textBoxTags.Text = "";
        //    // clear RELATED CATEGORIES
        //    this.listBoxRelatedCategories.Items.Clear();

        //    this.textBoxCategoryNameEnglish.Text = "";
        //    this.textBoxCategoryNameHebrew.Text = "";

        //    //enabled only explicitly
        //    this.buttonDeleteCategory.Enabled = false;            
        //}

        private void DeleteCategoryFromDB(DialogResult dr, TreeNode parentNode)
        {
            if (parentNode == null)
                return;

            if (DialogResult.Yes == dr)
                foreach (TreeNode node in parentNode.Nodes)
                {
                    DeleteCategoryFromDB(dr, node);
                }
            
            try
            {
                // delete from DB
                int catId = int.Parse(selectedTreeNode.ToolTipText);

                _linqLookupCategoryTableRow = GetCategoryDataObjectById(catId); //(from c in _db.Table_LookupCategories
                                                                                 //where c.CatId == catId
                                                                                 //select c).Single();

                _db.Table_LookupCategories.DeleteOnSubmit(_linqLookupCategoryTableRow);
                _db.SubmitChanges();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }          

        }
        private void ChangeSiblingParentToGrandParent(TreeNode selectedNode)
        {
            if ((selectedNode == null) || (selectedNode == treeViewCategories.Nodes[0]))
                return;

            foreach (TreeNode node in selectedNode.Nodes)
            {
                ChangeCategoryParentToGrandParent(int.Parse(node.ToolTipText));
                //selectedNode.Parent.Nodes.Add((TreeNode)node.Clone());                
                //node.Remove();
            }
        }

        private void ClearForm()
        {
            textBoxKeys.Text = "";
            textBoxTags.Text = "";
            textBoxCategoryNameHebrew.Text = "";
            textBoxCategoryNameEnglish.Text = "";
            listBoxRelatedCategories.Items.Clear();            
        }

        private void ResetForm(bool rebuildTreeView)
        {
            if (rebuildTreeView)
            {
                this.treeViewCategories.Nodes.Clear();

                this.treeViewCategories = PopulateRootLevel();

                if (treeViewCategories.Nodes.Count == 0)
                {
                    // this should never occur !!
                    this.treeViewCategories.Nodes.Add("אין קטגוריות");
                }

                Form_CategoriesManager.selectedTreeNode = this.treeViewCategories.Nodes[0];

                this.treeViewCategories.Refresh();
            }

            ClearForm();
            buttonDeleteCategory.Enabled = false;
            buttonSaveChanges.Enabled = false;

            radioButtonAddNewCat.Checked = false;
            radioButtonDelCat.Checked = false;
            radioButtonEditCat.Checked = false;
            comboBoxActionSelector.SelectedIndex = 0;
            _formState = CategoryEditorStateEnum.VIEW;
            selectedTreeNode = treeViewCategories.Nodes[0];
            selectedTreeNode.Expand();
        }

        private void buttonDeleteCategory_Click(object sender, EventArgs e)
        {
            DialogResult dr ;

            if (selectedTreeNode == null)
                return;

            if (selectedTreeNode == treeViewCategories.Nodes[0])
            {
                dr = MessageBox.Show("לא ניתן למחוק בסיס עץ הקטגוריות",
                    "נכשל - ניסיון למחוק קטגוריה ראשית",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            dr = MessageBox.Show("האם למחוק גם את כל תתי הקטגוריות ?",
                    "אישור מחיקת קטגוריות ",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);

            if (dr == DialogResult.Cancel)
                return;

            if ((selectedTreeNode.Nodes == null) || (selectedTreeNode.Nodes.Count == 0))
            {
                dr = DialogResult.No;
            }
            
            // TBD: this place must be a transaction start
            
            if (dr == DialogResult.No)
            {
                ChangeSiblingParentToGrandParent(selectedTreeNode);
            }

            TreeNode prevNode = selectedTreeNode.PrevVisibleNode;

            //selectedTreeNode.Remove();

            try
            {            
                DeleteCategoryFromDB(dr, selectedTreeNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            ResetForm(true);
            selectedTreeNode = prevNode;
            selectedTreeNode.Expand();
            
            // TBD: this place is transaction end
            
        }

        private bool StringIsNull(string tst)
        {
            return (tst == null);
        }
        private bool StringIsEmpty(string tst)
        {
            return (tst == "");
        }
        private bool StringIsNullOrEmpty(string tst)
        {
            return (StringIsNull(tst) || StringIsEmpty(tst));
        }
        private string ConvertRelatedCategories_ListToString()
        {
            string retStr = null;

            foreach (object item in listBoxRelatedCategories.Items)
            {
                int idx = treeViewCategories.Nodes.IndexOfKey((string)item);
                string catIdStr = treeViewCategories.Nodes[idx].ToolTipText;
                retStr += String.Format("# {0} ", catIdStr);
            }

            return retStr;
        }
        private void ConvertRelatedCategories_StringToList(string srcString)
        {
            if ((srcString == "") || (srcString == null))
                return;

            listBoxRelatedCategories.Items.Clear();
            string[] strParts = srcString.Split('#');
            int[] catIds = new int[strParts.Length];

            for (int i=0 ; i<strParts.Length; i++)
            {
                catIds[i] = int.Parse(strParts[i].Trim('#', ' '));
                var category = (from c in _db.Table_LookupCategories
                                where c.CatId == catIds[i]
                                select c).Single();

                listBoxRelatedCategories.Items.Insert(i, (object)category.CatHebrewName);
            }
        }
        private void FillFormWithSelectedNodeData()
        {
            if (selectedTreeNode == null)
                return;

            if (StringIsNullOrEmpty(selectedTreeNode.ToolTipText))
                return;

            int catId = int.Parse(selectedTreeNode.ToolTipText);

            _linqLookupCategoryTableRow = GetCategoryDataObjectById(catId); //(from c in _db.Table_LookupCategories
                                                                             //where catId == c.CatId
                                                                             //select c).Single();
            textBoxCategoryNameEnglish.Text = _linqLookupCategoryTableRow.CatEnglishName;
            textBoxCategoryNameHebrew.Text = _linqLookupCategoryTableRow.CatHebrewName;
            textBoxKeys.Text = _linqLookupCategoryTableRow.Description;
            textBoxTags.Text = _linqLookupCategoryTableRow.MetaTags;
            ConvertRelatedCategories_StringToList(_linqLookupCategoryTableRow.RelatedCatIds);
        }

        private void treeViewCategories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        private void treeViewCategories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // this solving case of double clicking on the '+'
            if (treeViewCategories.SelectedNode == null)
                return;

            string addedCatStr = String.Format(" | {0} ", treeViewCategories.SelectedNode.Text.Trim());
            if ((buttonDeleteCategory.Enabled == false) &&
                (_formState != CategoryEditorStateEnum.VIEW))
            {
                // don't add in delete or view state
                textBoxKeys.Text += addedCatStr;
                textBoxTags.Text += addedCatStr;
            }
            else
            { // in delete or view state - update the selection on double click
                selectedTreeNode = treeViewCategories.SelectedNode;
            }
        }

        private void buttonRemoveRelatedCategory_Click(object sender, EventArgs e)
        {
            if (listBoxRelatedCategories.SelectedIndex != -1)
                listBoxRelatedCategories.Items.RemoveAt(listBoxRelatedCategories.SelectedIndex);
        }

        //private void radioButtonAddNewCategory_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool isChecked = radioButtonAddNewCategory.Checked;

        //    //if (selectedTreeNode == null)
        //    //    selectedTreeNode = treeViewCategories.Nodes[0];
            
        //    if (isChecked)
        //    {
        //        _formState = CategoryEditorStateEnum.ADD;                
        //    }
        //    buttonDeleteCategory.Enabled = !isChecked;
        //    ActivateInputFields(isChecked);
        //    buttonSaveChanges.Enabled = isChecked;
        //    ClearForm();
        //}

        private void radioButtonEditCategory_CheckedChanged(object sender, EventArgs e)
        {
            ////if (selectedTreeNode == null)
            ////{
            ////    radioButtonEditCategory.Checked = false;
            ////    return;
            ////}

            //bool isChecked = radioButtonEditCategory.Checked;

            //if (isChecked)
            //{   
            //    _formState = CategoryEditorStateEnum.EDIT;
            //    FillFormWithSelectedNodeData();
            //}
            //buttonDeleteCategory.Enabled = !isChecked;
            //buttonSaveChanges.Enabled = isChecked;
            //ActivateInputFields(isChecked);            
        }

        private void ActivateInputFields(bool toEnable)
        {
            textBoxCategoryNameEnglish.Enabled = toEnable;
            textBoxCategoryNameHebrew.Enabled = toEnable;
            textBoxTags.Enabled = toEnable;
            textBoxKeys.Enabled = toEnable;
            buttonAddRelated.Enabled = toEnable;
            buttonRemoveRelatedCategory.Enabled = toEnable;
            listBoxRelatedCategories.Enabled = toEnable;
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        { // delete radio button
            //if (selectedTreeNode == null)
            //{
            //    radioButton1.Checked = false;
            //    return;
            //}
            
            //bool isChecked = radioButton1.Checked;

            //if (isChecked)
            //{
            //    _formState = CategoryEditorStateEnum.EDIT;
            //    FillFormWithSelectedNodeData();
            //}
            //buttonDeleteCategory.Enabled = isChecked ;
            //buttonSaveChanges.Enabled = !isChecked ;
            //ActivateInputFields(!isChecked);
        }

        private void treeViewCategories_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_formState == CategoryEditorStateEnum.VIEW)
            {
                selectedTreeNode.BackColor = Color.Transparent;
            }
        }
        
        private void AddCatNameToLists(bool addToTags, bool addToKeys, bool addToRelated, bool addHeb, bool addEng, object sender, EventArgs e)
        {
            if (treeViewCategories.SelectedNode == null)
                return;

            Table_LookupCategory linqCatRow = (from c in _db.Table_LookupCategories 
                                               where c.CatId == int.Parse(treeViewCategories.SelectedNode.ToolTipText)
                                               select c).Single();
            string hebCatName = String.Format(" | {0} ", linqCatRow.CatHebrewName.Trim());
            string engCatName = String.Format(" | {0} ", linqCatRow.CatEnglishName.Trim());
            
            string strToAdd = "";
            
            if (addEng)
                strToAdd += engCatName;
            if (addHeb)
                strToAdd += hebCatName;

            if ((buttonDeleteCategory.Enabled == false) &&
                (_formState != CategoryEditorStateEnum.VIEW))
            {
                // don't add in delete or view state
                if (addToKeys)
                    textBoxKeys.Text += strToAdd;
                if (addToTags)
                    textBoxTags.Text += strToAdd;

                if (addToRelated)
                    buttonAddRelated_Click(sender, e);
            }
            else
            { // in delete or view state - update the selection on double click
                selectedTreeNode = treeViewCategories.SelectedNode;
            }

        }

        private void ToolStripMenuItemAddCatToTagsList_Click(object sender, EventArgs e)
        {
            ToolStripMenuItemTagsHebEng_Click(sender, e);
        }

        private void ToolStripMenuItemTagsHeb_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, false, false, true, false, sender, e);
        }

        private void ToolStripMenuItemTagsEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, false, false, false, true, sender, e);
        }

        private void ToolStripMenuItemTagsHebEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, false, false, true, true, sender, e);
        }

        private void ToolStripMenuItem1KeysHeb_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(false, true, false, true, false, sender, e);
        }

        private void ToolStripMenuItem1KeysEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(false, true, false, false, true, sender, e);
        }

        private void ToolStripMenuItemKeysHebEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(false, true, false, true, true, sender, e);
        }

        private void ToolStripMenuItem2KeysTagsHeb_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, true, false, true, false, sender, e);
        }

        private void ToolStripMenuItem2KeysTagsEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, true, false, false, true, sender, e);
        }

        private void ToolStripMenuItem2KeysTagsHebEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, true, false, true, true, sender, e);
        }

        private void ToolStripMenuItemAddCatToRelatedList_Click(object sender, EventArgs e)
        {
            buttonAddRelated_Click(sender, e);
        }

        private void ToolStripMenuItem3AllHeb_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, true, true, true, false, sender, e);
        }

        private void ToolStripMenuItem3AllEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, true, true, false, true, sender, e);
        }

        private void ToolStripMenuItem3AllHebEng_Click(object sender, EventArgs e)
        {
            AddCatNameToLists(true, true, true, true, true, sender, e);
        }

        private void comboBoxActionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeViewCategories.SelectedNode == null)
                return;

            selectedTreeNode.BackColor = Color.Transparent;
            selectedTreeNode = treeViewCategories.SelectedNode;

            switch (comboBoxActionSelector.SelectedIndex)
            {
                case 0: // NOT SELECTED ACTION --> view state
                    _formState = CategoryEditorStateEnum.VIEW;
                    buttonDeleteCategory.Enabled = false;
                    buttonSaveChanges.Enabled = false;
                    ActivateInputFields(false);
                    ClearForm();
                    break;
                case 1: // ADD NEW CATEGORY
                    _formState = CategoryEditorStateEnum.ADD;
                    buttonDeleteCategory.Enabled = false;
                    ActivateInputFields(true);
                    buttonSaveChanges.Enabled = true;
                    ClearForm();
                    break;
                case 2: // EDIT category
                    _formState = CategoryEditorStateEnum.EDIT;
                    FillFormWithSelectedNodeData();            
                    buttonDeleteCategory.Enabled = false;
                    buttonSaveChanges.Enabled = true;
                    ActivateInputFields(true);       
                    break;
                case 3: // DELETE category
                    _formState = CategoryEditorStateEnum.EDIT; // delete is also category's edit state
                    FillFormWithSelectedNodeData();
                    buttonDeleteCategory.Enabled = true;
                    buttonSaveChanges.Enabled = false;
                    ActivateInputFields(false);
                    break;
            }

            if (_formState != CategoryEditorStateEnum.VIEW)
                selectedTreeNode.BackColor = Color.OrangeRed;
            
            return;
        }

        private void buttonResetForm_Click(object sender, EventArgs e)
        {
            ResetForm(true);
        }

        private void Form_CategoriesManager_Load(object sender, EventArgs e)
        {

        }

        private void radioButtonDelCat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDelCat.Checked)
            {
                comboBoxActionSelector.SelectedIndex = 3;
                comboBoxActionSelector_SelectedIndexChanged(sender, e);
                buttonDeleteCategory_Click(sender, e);
            }
        }

        private void radioButtonEditCat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEditCat.Checked)
            {
                comboBoxActionSelector.SelectedIndex = 2;
                comboBoxActionSelector_SelectedIndexChanged(sender, e);
            }
        }

        private void radioButtonAddNewCat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAddNewCat.Checked)
            {
                comboBoxActionSelector.SelectedIndex = 1;
                comboBoxActionSelector_SelectedIndexChanged(sender, e);            
            }
        }
        
    }
}

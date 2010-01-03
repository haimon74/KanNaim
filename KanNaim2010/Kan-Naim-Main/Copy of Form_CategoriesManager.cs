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

        private SqlConnection objConn = new SqlConnection("Data Source=PRIVATE-DCB1672;Initial Catalog=Kan-Naim;Integrated Security=True");

        private static DataClassesEditArticleDataContext _db = new DataClassesEditArticleDataContext();
        private static Table_LookupCategory _linqLookupCategoryTableRow = new Table_LookupCategory();
        private static TreeNode selectedTreeNode = new TreeNode();
        private static TreeNode newNode = new TreeNode();
        private static CategoryEditorStateEnum _formState = CategoryEditorStateEnum.VIEW;
        
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
            PopulateNodes(dt, tv.Nodes);
            return tv;
        }
        private void PopulateNodes(DataTable dt, TreeNodeCollection nodes)
        {
            TreeNode tn = new TreeNode();

            if (dt == null)
                return;

            if (nodes == null)
            {
                tn.Text = "אין קטגוריות";
                tn.ToolTipText = "-1";
                nodes.Add(tn);
            }

            foreach (DataRow dr in dt.Rows)
            {
                string name = dr["CatHebrewName"].ToString();
                string id = dr["CatId"].ToString();
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
            //DataClassesCategoriesDataContext _db = GlobalValiables._gDB_cat;
            //var categories =  from c in _gDB_cat.Table_LookupCategories
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
        
        private void ToolStripMenuItemEditCategory_Click(object sender, EventArgs e)
        {
            selectedTreeNode = treeViewCategories.Nodes[0];

            if (selectedTreeNode.ToolTipText == "-1") 
                return;

            _formState = CategoryEditorStateEnum.EDIT;

            try
            {
                _linqLookupCategoryTableRow = (from c in _db.Table_LookupCategories
                                               where c.CatId == int.Parse(selectedTreeNode.ToolTipText)
                                               select c).Single();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            this.buttonSaveChanges.Enabled = true;
            
            // fetch KEYS
            this.textBoxKeys.Text = _linqLookupCategoryTableRow.Description;
            // fetch TAGS
            this.textBoxTags.Text = _linqLookupCategoryTableRow.MetaTags;
            // fetch RELATED CATEGORIES
            
            listBoxRelatedCategories.Items.Clear();
            if (_linqLookupCategoryTableRow.RelatedCatIds != null)
            {
                string[] relatedCategoriesIds = _linqLookupCategoryTableRow.RelatedCatIds.Split('#');

                foreach (string item in relatedCategoriesIds)
                {
                    try
                    {
                        string relatedCatStr = (from c in _db.Table_LookupCategories
                                                where c.CatId == int.Parse(item.Trim('#',' '))
                                                select c.CatHebrewName).Single();

                        listBoxRelatedCategories.Items.Add(relatedCatStr);
                    }
                    catch
                    {
                    }
                }
            }
            this.textBoxCategoryNameEnglish.Text = _linqLookupCategoryTableRow.CatEnglishName;
            this.textBoxCategoryNameHebrew.Text = _linqLookupCategoryTableRow.CatHebrewName; 

            this.buttonSaveChanges.Enabled = true;

            // only for delete need explicitly enabled
            this.checkBoxEnableDelete.Enabled = true;
            this.checkBoxEnableDelete.Checked = false;
            this.buttonDeleteCategory.Enabled = false;
        }

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
                _linqLookupCategoryTableRow = (from c in _db.Table_LookupCategories
                                               where c.CatId == parentCatId
                                               select c).Single();
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

        }

        private void ToolStripMenuItemAddCategory_Click(object sender, EventArgs e)
        {
            _formState = CategoryEditorStateEnum.ADD;

            this.buttonSaveChanges.Enabled = true;
            
            // clear KEYS
            this.textBoxKeys.Text = "";
            // clear TAGS
            this.textBoxTags.Text = "";
            // clear RELATED CATEGORIES
            this.listBoxRelatedCategories.Items.Clear();

            this.textBoxCategoryNameEnglish.Text = "";
            this.textBoxCategoryNameHebrew.Text = "";

            //enabled only explicitly
            this.checkBoxEnableDelete.Enabled = false;
            this.buttonDeleteCategory.Enabled = false;            
        }

        private void checkBoxEnableDelete_CheckedChanged(object sender, EventArgs e)
        {
            buttonDeleteCategory.Enabled = checkBoxEnableDelete.Checked;            
        }

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

                _linqLookupCategoryTableRow = (from c in _db.Table_LookupCategories
                                               where c.CatId == catId
                                               select c).Single();

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
            if (selectedNode.Parent == null)
                return;

            foreach (TreeNode node in selectedNode.Nodes)
            {
                selectedNode.Parent.Nodes.Add((TreeNode)node.Clone());
                
                node.Remove();
            }
        }

        private void ResetForm(bool rebuildTreeView)
        {
            if (rebuildTreeView)
            {
                this.treeViewCategories.Nodes.Clear();

                this.treeViewCategories = PopulateRootLevel();

                if (treeViewCategories.Nodes.Count == 0)
                    this.treeViewCategories.Nodes.Add("אין קטגוריות");

                Form_CategoriesManager.selectedTreeNode = this.treeViewCategories.Nodes[0];

                this.treeViewCategories.Refresh();
            }

            textBoxKeys.Text = "";
            textBoxTags.Text = "";
            textBoxCategoryNameHebrew.Text = "";
            textBoxCategoryNameEnglish.Text = "";
            listBoxRelatedCategories.Items.Clear();
            checkBoxEnableDelete.Checked = false;
            checkBoxEnableDelete.Enabled = false;
            buttonDeleteCategory.Enabled = false;
            buttonSaveChanges.Enabled = false;
            _formState = CategoryEditorStateEnum.VIEW;
        }

        private void buttonDeleteCategory_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("האם למחוק גם את כל תתי הקטגוריות ?",
                                                "אישור מחיקת קטגוריות מרובות",
                                                MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Exclamation,
                                                MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Abort)
                return;
            if (selectedTreeNode == null)
                return;

            try
            {
                // TBD: this place must be a transaction start
                DeleteCategoryFromDB(dr, selectedTreeNode);

                if (dr == DialogResult.No)
                {
                    ChangeSiblingParentToGrandParent(selectedTreeNode);
                }

                selectedTreeNode.Remove();
                ResetForm(true);

                // TBD: this place is transaction end
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void treeViewCategories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (buttonSaveChanges.Enabled == false)
                selectedTreeNode = treeViewCategories.SelectedNode;
        }

        private void treeViewCategories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewCategories.SelectedNode == null)
                return;

            if (buttonSaveChanges.Enabled == false)
                selectedTreeNode = treeViewCategories.SelectedNode;

            string addedCatStr = String.Format(" | {0} ", treeViewCategories.SelectedNode.Text).Trim();
            textBoxKeys.Text += addedCatStr;
            textBoxTags.Text += addedCatStr;
        }

        private void buttonRemoveRelatedCategory_Click(object sender, EventArgs e)
        {
            if (listBoxRelatedCategories.SelectedIndex != -1)
                listBoxRelatedCategories.Items.RemoveAt(listBoxRelatedCategories.SelectedIndex);
        }
        
    }
}

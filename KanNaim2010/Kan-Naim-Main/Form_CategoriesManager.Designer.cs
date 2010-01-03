namespace Kan_Naim_Main
{
    partial class Form_CategoriesManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemAddCatToTagsList = new System.Windows.Forms.ToolStripMenuItem();
            this.עoolStripMenuItemTagsHebEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTagsHeb = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTagsEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddCatToKeysList = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemKeysHebEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1KeysHeb = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1KeysEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddCatToTagsAndKeysList = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2KeysTagsHebEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2KeysTagsHeb = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2KeysTagsEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddCatToRelatedList = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddCatToTagsKeysRelatedLists = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3AllHebEng = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3AllHeb = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3AllEng = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxCategoryNameHebrew = new System.Windows.Forms.TextBox();
            this.textBoxCategoryNameEnglish = new System.Windows.Forms.TextBox();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            this.buttonDeleteCategory = new System.Windows.Forms.Button();
            this.labelTags = new System.Windows.Forms.Label();
            this.textBoxTags = new System.Windows.Forms.TextBox();
            this.labelKeys = new System.Windows.Forms.Label();
            this.textBoxKeys = new System.Windows.Forms.TextBox();
            this.listBoxRelatedCategories = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAddRelated = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRemoveRelatedCategory = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageTreeView = new System.Windows.Forms.TabPage();
            this.treeViewCategories = new System.Windows.Forms.TreeView();
            this.comboBoxActionSelector = new System.Windows.Forms.ComboBox();
            this.buttonResetForm = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDelCat = new System.Windows.Forms.RadioButton();
            this.radioButtonEditCat = new System.Windows.Forms.RadioButton();
            this.radioButtonAddNewCat = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageTreeView.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddCatToTagsList,
            this.ToolStripMenuItemAddCatToKeysList,
            this.ToolStripMenuItemAddCatToTagsAndKeysList,
            this.ToolStripMenuItemAddCatToRelatedList,
            this.ToolStripMenuItemAddCatToTagsKeysRelatedLists});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(219, 114);
            // 
            // ToolStripMenuItemAddCatToTagsList
            // 
            this.ToolStripMenuItemAddCatToTagsList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.עoolStripMenuItemTagsHebEng,
            this.ToolStripMenuItemTagsHeb,
            this.ToolStripMenuItemTagsEng});
            this.ToolStripMenuItemAddCatToTagsList.Name = "ToolStripMenuItemAddCatToTagsList";
            this.ToolStripMenuItemAddCatToTagsList.Size = new System.Drawing.Size(218, 22);
            this.ToolStripMenuItemAddCatToTagsList.Text = "הוסף לתגיות";
            this.ToolStripMenuItemAddCatToTagsList.Click += new System.EventHandler(this.ToolStripMenuItemAddCatToTagsList_Click);
            // 
            // עoolStripMenuItemTagsHebEng
            // 
            this.עoolStripMenuItemTagsHebEng.Name = "עoolStripMenuItemTagsHebEng";
            this.עoolStripMenuItemTagsHebEng.Size = new System.Drawing.Size(159, 22);
            this.עoolStripMenuItemTagsHebEng.Text = "עברית + אנגלית";
            this.עoolStripMenuItemTagsHebEng.Click += new System.EventHandler(this.ToolStripMenuItemTagsHebEng_Click);
            // 
            // ToolStripMenuItemTagsHeb
            // 
            this.ToolStripMenuItemTagsHeb.Name = "ToolStripMenuItemTagsHeb";
            this.ToolStripMenuItemTagsHeb.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItemTagsHeb.Text = "עברית";
            this.ToolStripMenuItemTagsHeb.Click += new System.EventHandler(this.ToolStripMenuItemTagsHeb_Click);
            // 
            // ToolStripMenuItemTagsEng
            // 
            this.ToolStripMenuItemTagsEng.Name = "ToolStripMenuItemTagsEng";
            this.ToolStripMenuItemTagsEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItemTagsEng.Text = "אנגלית";
            this.ToolStripMenuItemTagsEng.Click += new System.EventHandler(this.ToolStripMenuItemTagsEng_Click);
            // 
            // ToolStripMenuItemAddCatToKeysList
            // 
            this.ToolStripMenuItemAddCatToKeysList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemKeysHebEng,
            this.ToolStripMenuItem1KeysHeb,
            this.ToolStripMenuItem1KeysEng});
            this.ToolStripMenuItemAddCatToKeysList.Name = "ToolStripMenuItemAddCatToKeysList";
            this.ToolStripMenuItemAddCatToKeysList.Size = new System.Drawing.Size(218, 22);
            this.ToolStripMenuItemAddCatToKeysList.Text = "הוסף למילות חיפוש";
            // 
            // ToolStripMenuItemKeysHebEng
            // 
            this.ToolStripMenuItemKeysHebEng.Name = "ToolStripMenuItemKeysHebEng";
            this.ToolStripMenuItemKeysHebEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItemKeysHebEng.Text = "עברית + אנגלית";
            this.ToolStripMenuItemKeysHebEng.Click += new System.EventHandler(this.ToolStripMenuItemKeysHebEng_Click);
            // 
            // ToolStripMenuItem1KeysHeb
            // 
            this.ToolStripMenuItem1KeysHeb.Name = "ToolStripMenuItem1KeysHeb";
            this.ToolStripMenuItem1KeysHeb.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem1KeysHeb.Text = "עברית";
            this.ToolStripMenuItem1KeysHeb.Click += new System.EventHandler(this.ToolStripMenuItem1KeysHeb_Click);
            // 
            // ToolStripMenuItem1KeysEng
            // 
            this.ToolStripMenuItem1KeysEng.Name = "ToolStripMenuItem1KeysEng";
            this.ToolStripMenuItem1KeysEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem1KeysEng.Text = "אנגלית";
            this.ToolStripMenuItem1KeysEng.Click += new System.EventHandler(this.ToolStripMenuItem1KeysEng_Click);
            // 
            // ToolStripMenuItemAddCatToTagsAndKeysList
            // 
            this.ToolStripMenuItemAddCatToTagsAndKeysList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem2KeysTagsHebEng,
            this.ToolStripMenuItem2KeysTagsHeb,
            this.ToolStripMenuItem2KeysTagsEng});
            this.ToolStripMenuItemAddCatToTagsAndKeysList.Name = "ToolStripMenuItemAddCatToTagsAndKeysList";
            this.ToolStripMenuItemAddCatToTagsAndKeysList.Size = new System.Drawing.Size(218, 22);
            this.ToolStripMenuItemAddCatToTagsAndKeysList.Text = "הוסף לתגיות + מילות חיפוש";
            // 
            // ToolStripMenuItem2KeysTagsHebEng
            // 
            this.ToolStripMenuItem2KeysTagsHebEng.Name = "ToolStripMenuItem2KeysTagsHebEng";
            this.ToolStripMenuItem2KeysTagsHebEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem2KeysTagsHebEng.Text = "עברית + אנגלית";
            this.ToolStripMenuItem2KeysTagsHebEng.Click += new System.EventHandler(this.ToolStripMenuItem2KeysTagsHebEng_Click);
            // 
            // ToolStripMenuItem2KeysTagsHeb
            // 
            this.ToolStripMenuItem2KeysTagsHeb.Name = "ToolStripMenuItem2KeysTagsHeb";
            this.ToolStripMenuItem2KeysTagsHeb.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem2KeysTagsHeb.Text = "עברית";
            this.ToolStripMenuItem2KeysTagsHeb.Click += new System.EventHandler(this.ToolStripMenuItem2KeysTagsHeb_Click);
            // 
            // ToolStripMenuItem2KeysTagsEng
            // 
            this.ToolStripMenuItem2KeysTagsEng.Name = "ToolStripMenuItem2KeysTagsEng";
            this.ToolStripMenuItem2KeysTagsEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem2KeysTagsEng.Text = "אנגלית";
            this.ToolStripMenuItem2KeysTagsEng.Click += new System.EventHandler(this.ToolStripMenuItem2KeysTagsEng_Click);
            // 
            // ToolStripMenuItemAddCatToRelatedList
            // 
            this.ToolStripMenuItemAddCatToRelatedList.Name = "ToolStripMenuItemAddCatToRelatedList";
            this.ToolStripMenuItemAddCatToRelatedList.Size = new System.Drawing.Size(218, 22);
            this.ToolStripMenuItemAddCatToRelatedList.Text = "הוסף לקטגוריות קשורות";
            this.ToolStripMenuItemAddCatToRelatedList.Click += new System.EventHandler(this.ToolStripMenuItemAddCatToRelatedList_Click);
            // 
            // ToolStripMenuItemAddCatToTagsKeysRelatedLists
            // 
            this.ToolStripMenuItemAddCatToTagsKeysRelatedLists.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem3AllHebEng,
            this.ToolStripMenuItem3AllHeb,
            this.ToolStripMenuItem3AllEng});
            this.ToolStripMenuItemAddCatToTagsKeysRelatedLists.Name = "ToolStripMenuItemAddCatToTagsKeysRelatedLists";
            this.ToolStripMenuItemAddCatToTagsKeysRelatedLists.Size = new System.Drawing.Size(218, 22);
            this.ToolStripMenuItemAddCatToTagsKeysRelatedLists.Text = "הוסף לכל הרשימות";
            // 
            // ToolStripMenuItem3AllHebEng
            // 
            this.ToolStripMenuItem3AllHebEng.Name = "ToolStripMenuItem3AllHebEng";
            this.ToolStripMenuItem3AllHebEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem3AllHebEng.Text = "עברית + אנגלית";
            this.ToolStripMenuItem3AllHebEng.Click += new System.EventHandler(this.ToolStripMenuItem3AllHebEng_Click);
            // 
            // ToolStripMenuItem3AllHeb
            // 
            this.ToolStripMenuItem3AllHeb.Name = "ToolStripMenuItem3AllHeb";
            this.ToolStripMenuItem3AllHeb.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem3AllHeb.Text = "עברית";
            this.ToolStripMenuItem3AllHeb.Click += new System.EventHandler(this.ToolStripMenuItem3AllHeb_Click);
            // 
            // ToolStripMenuItem3AllEng
            // 
            this.ToolStripMenuItem3AllEng.Name = "ToolStripMenuItem3AllEng";
            this.ToolStripMenuItem3AllEng.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItem3AllEng.Text = "אנגלית";
            this.ToolStripMenuItem3AllEng.Click += new System.EventHandler(this.ToolStripMenuItem3AllEng_Click);
            // 
            // textBoxCategoryNameHebrew
            // 
            this.textBoxCategoryNameHebrew.Location = new System.Drawing.Point(12, 107);
            this.textBoxCategoryNameHebrew.Name = "textBoxCategoryNameHebrew";
            this.textBoxCategoryNameHebrew.Size = new System.Drawing.Size(342, 20);
            this.textBoxCategoryNameHebrew.TabIndex = 1;
            // 
            // textBoxCategoryNameEnglish
            // 
            this.textBoxCategoryNameEnglish.Location = new System.Drawing.Point(12, 155);
            this.textBoxCategoryNameEnglish.Name = "textBoxCategoryNameEnglish";
            this.textBoxCategoryNameEnglish.Size = new System.Drawing.Size(342, 20);
            this.textBoxCategoryNameEnglish.TabIndex = 3;
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Enabled = false;
            this.buttonSaveChanges.Location = new System.Drawing.Point(12, 537);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(341, 37);
            this.buttonSaveChanges.TabIndex = 5;
            this.buttonSaveChanges.Text = "שמור שינוי";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // buttonDeleteCategory
            // 
            this.buttonDeleteCategory.Enabled = false;
            this.buttonDeleteCategory.Location = new System.Drawing.Point(163, 5);
            this.buttonDeleteCategory.Name = "buttonDeleteCategory";
            this.buttonDeleteCategory.Size = new System.Drawing.Size(43, 23);
            this.buttonDeleteCategory.TabIndex = 6;
            this.buttonDeleteCategory.Text = "מחק קטגוריה";
            this.buttonDeleteCategory.UseVisualStyleBackColor = true;
            this.buttonDeleteCategory.Visible = false;
            this.buttonDeleteCategory.Click += new System.EventHandler(this.buttonDeleteCategory_Click);
            // 
            // labelTags
            // 
            this.labelTags.AutoSize = true;
            this.labelTags.Location = new System.Drawing.Point(12, 187);
            this.labelTags.Name = "labelTags";
            this.labelTags.Size = new System.Drawing.Size(37, 13);
            this.labelTags.TabIndex = 7;
            this.labelTags.Text = "תגיות";
            // 
            // textBoxTags
            // 
            this.textBoxTags.Location = new System.Drawing.Point(12, 203);
            this.textBoxTags.Multiline = true;
            this.textBoxTags.Name = "textBoxTags";
            this.textBoxTags.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTags.Size = new System.Drawing.Size(341, 88);
            this.textBoxTags.TabIndex = 8;
            // 
            // labelKeys
            // 
            this.labelKeys.AutoSize = true;
            this.labelKeys.Location = new System.Drawing.Point(12, 311);
            this.labelKeys.Name = "labelKeys";
            this.labelKeys.Size = new System.Drawing.Size(74, 13);
            this.labelKeys.TabIndex = 9;
            this.labelKeys.Text = "מילות חיפוש";
            // 
            // textBoxKeys
            // 
            this.textBoxKeys.Location = new System.Drawing.Point(12, 327);
            this.textBoxKeys.Multiline = true;
            this.textBoxKeys.Name = "textBoxKeys";
            this.textBoxKeys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxKeys.Size = new System.Drawing.Size(342, 90);
            this.textBoxKeys.TabIndex = 10;
            // 
            // listBoxRelatedCategories
            // 
            this.listBoxRelatedCategories.FormattingEnabled = true;
            this.listBoxRelatedCategories.Location = new System.Drawing.Point(112, 436);
            this.listBoxRelatedCategories.MultiColumn = true;
            this.listBoxRelatedCategories.Name = "listBoxRelatedCategories";
            this.listBoxRelatedCategories.Size = new System.Drawing.Size(241, 82);
            this.listBoxRelatedCategories.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(723, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "קטגוריות קשורות";
            // 
            // buttonAddRelated
            // 
            this.buttonAddRelated.Location = new System.Drawing.Point(32, 459);
            this.buttonAddRelated.Name = "buttonAddRelated";
            this.buttonAddRelated.Size = new System.Drawing.Size(71, 26);
            this.buttonAddRelated.TabIndex = 13;
            this.buttonAddRelated.Text = "הוסף";
            this.buttonAddRelated.UseVisualStyleBackColor = true;
            this.buttonAddRelated.Click += new System.EventHandler(this.buttonAddRelated_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "שם קטגוריה בעברית";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "שם קטגוריה באנגלית";
            // 
            // buttonRemoveRelatedCategory
            // 
            this.buttonRemoveRelatedCategory.Location = new System.Drawing.Point(32, 491);
            this.buttonRemoveRelatedCategory.Name = "buttonRemoveRelatedCategory";
            this.buttonRemoveRelatedCategory.Size = new System.Drawing.Size(71, 27);
            this.buttonRemoveRelatedCategory.TabIndex = 16;
            this.buttonRemoveRelatedCategory.Text = "מחק";
            this.buttonRemoveRelatedCategory.UseVisualStyleBackColor = true;
            this.buttonRemoveRelatedCategory.Click += new System.EventHandler(this.buttonRemoveRelatedCategory_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageTreeView);
            this.tabControl1.Location = new System.Drawing.Point(362, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(502, 573);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageTreeView
            // 
            this.tabPageTreeView.Controls.Add(this.treeViewCategories);
            this.tabPageTreeView.Location = new System.Drawing.Point(4, 22);
            this.tabPageTreeView.Name = "tabPageTreeView";
            this.tabPageTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTreeView.Size = new System.Drawing.Size(494, 547);
            this.tabPageTreeView.TabIndex = 0;
            this.tabPageTreeView.Text = "עץ קטגוריות";
            this.tabPageTreeView.UseVisualStyleBackColor = true;
            // 
            // treeViewCategories
            // 
            this.treeViewCategories.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewCategories.FullRowSelect = true;
            this.treeViewCategories.LabelEdit = true;
            this.treeViewCategories.Location = new System.Drawing.Point(6, 6);
            this.treeViewCategories.Name = "treeViewCategories";
            this.treeViewCategories.RightToLeftLayout = true;
            this.treeViewCategories.ShowNodeToolTips = true;
            this.treeViewCategories.Size = new System.Drawing.Size(478, 534);
            this.treeViewCategories.TabIndex = 1;
            this.treeViewCategories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategories_AfterSelect);
            this.treeViewCategories.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCategories_NodeMouseClick);
            this.treeViewCategories.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCategories_BeforeSelect);
            // 
            // comboBoxActionSelector
            // 
            this.comboBoxActionSelector.Enabled = false;
            this.comboBoxActionSelector.FormattingEnabled = true;
            this.comboBoxActionSelector.Items.AddRange(new object[] {
            "--- בחר פעולה ---",
            "הוספת קטגוריה",
            "עריכת קטגוריה",
            "מחיקת קטגוריה"});
            this.comboBoxActionSelector.Location = new System.Drawing.Point(212, 7);
            this.comboBoxActionSelector.Name = "comboBoxActionSelector";
            this.comboBoxActionSelector.Size = new System.Drawing.Size(23, 21);
            this.comboBoxActionSelector.TabIndex = 17;
            this.comboBoxActionSelector.Visible = false;
            this.comboBoxActionSelector.SelectedIndexChanged += new System.EventHandler(this.comboBoxActionSelector_SelectedIndexChanged);
            // 
            // buttonResetForm
            // 
            this.buttonResetForm.Location = new System.Drawing.Point(12, 7);
            this.buttonResetForm.Name = "buttonResetForm";
            this.buttonResetForm.Size = new System.Drawing.Size(108, 26);
            this.buttonResetForm.TabIndex = 18;
            this.buttonResetForm.Text = "איתחול טופס ";
            this.buttonResetForm.UseVisualStyleBackColor = true;
            this.buttonResetForm.Click += new System.EventHandler(this.buttonResetForm_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDelCat);
            this.groupBox1.Controls.Add(this.radioButtonEditCat);
            this.groupBox1.Controls.Add(this.radioButtonAddNewCat);
            this.groupBox1.Location = new System.Drawing.Point(12, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 44);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "בחר פעולה לביצוע";
            // 
            // radioButtonDelCat
            // 
            this.radioButtonDelCat.AutoSize = true;
            this.radioButtonDelCat.Location = new System.Drawing.Point(50, 19);
            this.radioButtonDelCat.Name = "radioButtonDelCat";
            this.radioButtonDelCat.Size = new System.Drawing.Size(58, 17);
            this.radioButtonDelCat.TabIndex = 22;
            this.radioButtonDelCat.TabStop = true;
            this.radioButtonDelCat.Text = "מחיקה";
            this.radioButtonDelCat.UseVisualStyleBackColor = true;
            this.radioButtonDelCat.CheckedChanged += new System.EventHandler(this.radioButtonDelCat_CheckedChanged);
            // 
            // radioButtonEditCat
            // 
            this.radioButtonEditCat.AutoSize = true;
            this.radioButtonEditCat.Location = new System.Drawing.Point(151, 19);
            this.radioButtonEditCat.Name = "radioButtonEditCat";
            this.radioButtonEditCat.Size = new System.Drawing.Size(57, 17);
            this.radioButtonEditCat.TabIndex = 21;
            this.radioButtonEditCat.TabStop = true;
            this.radioButtonEditCat.Text = "עריכה";
            this.radioButtonEditCat.UseVisualStyleBackColor = true;
            this.radioButtonEditCat.CheckedChanged += new System.EventHandler(this.radioButtonEditCat_CheckedChanged);
            // 
            // radioButtonAddNewCat
            // 
            this.radioButtonAddNewCat.AutoSize = true;
            this.radioButtonAddNewCat.Location = new System.Drawing.Point(250, 19);
            this.radioButtonAddNewCat.Name = "radioButtonAddNewCat";
            this.radioButtonAddNewCat.Size = new System.Drawing.Size(58, 17);
            this.radioButtonAddNewCat.TabIndex = 20;
            this.radioButtonAddNewCat.TabStop = true;
            this.radioButtonAddNewCat.Text = "הוספה";
            this.radioButtonAddNewCat.UseVisualStyleBackColor = true;
            this.radioButtonAddNewCat.CheckedChanged += new System.EventHandler(this.radioButtonAddNewCat_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 436);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "קטגוריות קשורות";
            // 
            // Form_CategoriesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 599);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBoxRelatedCategories);
            this.Controls.Add(this.buttonResetForm);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxActionSelector);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRemoveRelatedCategory);
            this.Controls.Add(this.textBoxCategoryNameHebrew);
            this.Controls.Add(this.labelKeys);
            this.Controls.Add(this.buttonAddRelated);
            this.Controls.Add(this.textBoxTags);
            this.Controls.Add(this.labelTags);
            this.Controls.Add(this.textBoxCategoryNameEnglish);
            this.Controls.Add(this.buttonDeleteCategory);
            this.Controls.Add(this.textBoxKeys);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_CategoriesManager";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ניהול קטגוריות";
            this.Load += new System.EventHandler(this.Form_CategoriesManager_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageTreeView.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCatToTagsList;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCatToKeysList;
        private System.Windows.Forms.TextBox textBoxCategoryNameHebrew;
        private System.Windows.Forms.TextBox textBoxCategoryNameEnglish;
        private System.Windows.Forms.Button buttonSaveChanges;
        private System.Windows.Forms.Button buttonDeleteCategory;
        private System.Windows.Forms.Label labelTags;
        private System.Windows.Forms.TextBox textBoxTags;
        private System.Windows.Forms.Label labelKeys;
        private System.Windows.Forms.TextBox textBoxKeys;
        private System.Windows.Forms.ListBox listBoxRelatedCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAddRelated;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonRemoveRelatedCategory;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageTreeView;
        private System.Windows.Forms.TreeView treeViewCategories;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCatToTagsAndKeysList;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCatToRelatedList;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCatToTagsKeysRelatedLists;
        private System.Windows.Forms.ToolStripMenuItem עoolStripMenuItemTagsHebEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTagsHeb;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTagsEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemKeysHebEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1KeysHeb;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1KeysEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2KeysTagsHebEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2KeysTagsHeb;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2KeysTagsEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3AllHebEng;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3AllHeb;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3AllEng;
        private System.Windows.Forms.ComboBox comboBoxActionSelector;
        private System.Windows.Forms.Button buttonResetForm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDelCat;
        private System.Windows.Forms.RadioButton radioButtonEditCat;
        private System.Windows.Forms.RadioButton radioButtonAddNewCat;
        private System.Windows.Forms.Label label4;
    }
}
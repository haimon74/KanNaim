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
            this.ToolStripMenuItemAddCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemEditCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxCategoryNameHebrew = new System.Windows.Forms.TextBox();
            this.textBoxCategoryNameEnglish = new System.Windows.Forms.TextBox();
            this.checkBoxEnableDelete = new System.Windows.Forms.CheckBox();
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
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddCategory,
            this.ToolStripMenuItemEditCategory});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 48);
            // 
            // ToolStripMenuItemAddCategory
            // 
            this.ToolStripMenuItemAddCategory.Name = "ToolStripMenuItemAddCategory";
            this.ToolStripMenuItemAddCategory.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuItemAddCategory.Text = "הוסף תת קטגוריה";
            this.ToolStripMenuItemAddCategory.Click += new System.EventHandler(this.ToolStripMenuItemAddCategory_Click);
            // 
            // ToolStripMenuItemEditCategory
            // 
            this.ToolStripMenuItemEditCategory.Name = "ToolStripMenuItemEditCategory";
            this.ToolStripMenuItemEditCategory.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuItemEditCategory.Text = "ערוך קטגוריה";
            this.ToolStripMenuItemEditCategory.Click += new System.EventHandler(this.ToolStripMenuItemEditCategory_Click);
            // 
            // textBoxCategoryNameHebrew
            // 
            this.textBoxCategoryNameHebrew.Location = new System.Drawing.Point(12, 85);
            this.textBoxCategoryNameHebrew.Name = "textBoxCategoryNameHebrew";
            this.textBoxCategoryNameHebrew.Size = new System.Drawing.Size(342, 20);
            this.textBoxCategoryNameHebrew.TabIndex = 1;
            // 
            // textBoxCategoryNameEnglish
            // 
            this.textBoxCategoryNameEnglish.Location = new System.Drawing.Point(12, 135);
            this.textBoxCategoryNameEnglish.Name = "textBoxCategoryNameEnglish";
            this.textBoxCategoryNameEnglish.Size = new System.Drawing.Size(342, 20);
            this.textBoxCategoryNameEnglish.TabIndex = 3;
            // 
            // checkBoxEnableDelete
            // 
            this.checkBoxEnableDelete.AutoSize = true;
            this.checkBoxEnableDelete.Enabled = false;
            this.checkBoxEnableDelete.Location = new System.Drawing.Point(27, 12);
            this.checkBoxEnableDelete.Name = "checkBoxEnableDelete";
            this.checkBoxEnableDelete.Size = new System.Drawing.Size(92, 17);
            this.checkBoxEnableDelete.TabIndex = 4;
            this.checkBoxEnableDelete.Text = "אפשר מחיקה";
            this.checkBoxEnableDelete.UseVisualStyleBackColor = true;
            this.checkBoxEnableDelete.CheckedChanged += new System.EventHandler(this.checkBoxEnableDelete_CheckedChanged);
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Enabled = false;
            this.buttonSaveChanges.Location = new System.Drawing.Point(70, 537);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(147, 37);
            this.buttonSaveChanges.TabIndex = 5;
            this.buttonSaveChanges.Text = "שמור שינוי";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // buttonDeleteCategory
            // 
            this.buttonDeleteCategory.Enabled = false;
            this.buttonDeleteCategory.Location = new System.Drawing.Point(12, 30);
            this.buttonDeleteCategory.Name = "buttonDeleteCategory";
            this.buttonDeleteCategory.Size = new System.Drawing.Size(107, 34);
            this.buttonDeleteCategory.TabIndex = 6;
            this.buttonDeleteCategory.Text = "מחק קטגוריה";
            this.buttonDeleteCategory.UseVisualStyleBackColor = true;
            this.buttonDeleteCategory.Click += new System.EventHandler(this.buttonDeleteCategory_Click);
            // 
            // labelTags
            // 
            this.labelTags.AutoSize = true;
            this.labelTags.Location = new System.Drawing.Point(319, 174);
            this.labelTags.Name = "labelTags";
            this.labelTags.Size = new System.Drawing.Size(37, 13);
            this.labelTags.TabIndex = 7;
            this.labelTags.Text = "תגיות";
            // 
            // textBoxTags
            // 
            this.textBoxTags.Location = new System.Drawing.Point(12, 190);
            this.textBoxTags.Multiline = true;
            this.textBoxTags.Name = "textBoxTags";
            this.textBoxTags.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTags.Size = new System.Drawing.Size(341, 88);
            this.textBoxTags.TabIndex = 8;
            // 
            // labelKeys
            // 
            this.labelKeys.AutoSize = true;
            this.labelKeys.Location = new System.Drawing.Point(282, 294);
            this.labelKeys.Name = "labelKeys";
            this.labelKeys.Size = new System.Drawing.Size(74, 13);
            this.labelKeys.TabIndex = 9;
            this.labelKeys.Text = "מילות חיפוש";
            // 
            // textBoxKeys
            // 
            this.textBoxKeys.Location = new System.Drawing.Point(12, 310);
            this.textBoxKeys.Multiline = true;
            this.textBoxKeys.Name = "textBoxKeys";
            this.textBoxKeys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxKeys.Size = new System.Drawing.Size(342, 90);
            this.textBoxKeys.TabIndex = 10;
            // 
            // listBoxRelatedCategories
            // 
            this.listBoxRelatedCategories.FormattingEnabled = true;
            this.listBoxRelatedCategories.Location = new System.Drawing.Point(12, 423);
            this.listBoxRelatedCategories.MultiColumn = true;
            this.listBoxRelatedCategories.Name = "listBoxRelatedCategories";
            this.listBoxRelatedCategories.Size = new System.Drawing.Size(242, 95);
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
            this.buttonAddRelated.Location = new System.Drawing.Point(282, 436);
            this.buttonAddRelated.Name = "buttonAddRelated";
            this.buttonAddRelated.Size = new System.Drawing.Size(46, 27);
            this.buttonAddRelated.TabIndex = 13;
            this.buttonAddRelated.Text = "הוסף";
            this.buttonAddRelated.UseVisualStyleBackColor = true;
            this.buttonAddRelated.Click += new System.EventHandler(this.buttonAddRelated_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "שם קטגוריה בעברית";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "שם קטגוריה באנגלית";
            // 
            // buttonRemoveRelatedCategory
            // 
            this.buttonRemoveRelatedCategory.Location = new System.Drawing.Point(282, 478);
            this.buttonRemoveRelatedCategory.Name = "buttonRemoveRelatedCategory";
            this.buttonRemoveRelatedCategory.Size = new System.Drawing.Size(46, 23);
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
            this.treeViewCategories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCategories_NodeMouseDoubleClick);
            this.treeViewCategories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategories_AfterSelect);
            this.treeViewCategories.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCategories_NodeMouseClick);
            // 
            // Form_CategoriesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 583);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.listBoxRelatedCategories);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonRemoveRelatedCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAddRelated);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxKeys);
            this.Controls.Add(this.labelKeys);
            this.Controls.Add(this.checkBoxEnableDelete);
            this.Controls.Add(this.buttonDeleteCategory);
            this.Controls.Add(this.textBoxCategoryNameHebrew);
            this.Controls.Add(this.textBoxTags);
            this.Controls.Add(this.labelTags);
            this.Controls.Add(this.textBoxCategoryNameEnglish);
            this.Name = "Form_CategoriesManager";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "ניהול קטגוריות";
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageTreeView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddCategory;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEditCategory;
        private System.Windows.Forms.TextBox textBoxCategoryNameHebrew;
        private System.Windows.Forms.TextBox textBoxCategoryNameEnglish;
        private System.Windows.Forms.CheckBox checkBoxEnableDelete;
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
    }
}
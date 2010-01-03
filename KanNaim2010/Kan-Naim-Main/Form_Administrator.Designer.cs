namespace Kan_Naim_Main
{
    partial class FormAdministrator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBoxObjectStatus = new System.Windows.Forms.GroupBox();
            this.radioButtonArchive = new System.Windows.Forms.RadioButton();
            this.radioButtonBroadcast = new System.Windows.Forms.RadioButton();
            this.radioButtonActive = new System.Windows.Forms.RadioButton();
            this.buttonShowResults = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker21 = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.כתבותToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemEditUserDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPasswordReminder = new System.Windows.Forms.ToolStripMenuItem();
            this.כתבותToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.הוסףחדשהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.עריכתכתבהציבוריתToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.עריכתכתבהפרטיתToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.אינדקסיםToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.מודיעינעיםToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.עסקיםToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.בילוינעיםToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.לינקיםToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemBottomPageLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddPreferedLink = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPreferedLinksList = new System.Windows.Forms.ToolStripMenuItem();
            this.אחרToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTopMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRightMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.מולטימדיהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddNewPhoto = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddNewVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddNewBanner = new System.Windows.Forms.ToolStripMenuItem();
            this.userControlTreeView1 = new HaimDLL.UserControlTreeView();
            this.groupBox1.SuspendLayout();
            this.groupBoxObjectStatus.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelCategory);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.groupBoxObjectStatus);
            this.groupBox1.Controls.Add(this.userControlTreeView1);
            this.groupBox1.Controls.Add(this.buttonShowResults);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker21);
            this.groupBox1.Location = new System.Drawing.Point(12, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 462);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "סינן רשימות לפי תאריכים וקטגוריות";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Location = new System.Drawing.Point(257, 23);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(110, 13);
            this.labelCategory.TabIndex = 44;
            this.labelCategory.Text = "בחר קטגוריה לסינון";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(625, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "סוג רשימה";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "כתבות",
            "תקצירים",
            "תמונות",
            "ווידאו",
            "RSS"});
            this.comboBox1.Location = new System.Drawing.Point(418, 242);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(188, 21);
            this.comboBox1.TabIndex = 42;
            // 
            // groupBoxObjectStatus
            // 
            this.groupBoxObjectStatus.Controls.Add(this.radioButtonArchive);
            this.groupBoxObjectStatus.Controls.Add(this.radioButtonBroadcast);
            this.groupBoxObjectStatus.Controls.Add(this.radioButtonActive);
            this.groupBoxObjectStatus.Location = new System.Drawing.Point(409, 147);
            this.groupBoxObjectStatus.Name = "groupBoxObjectStatus";
            this.groupBoxObjectStatus.Size = new System.Drawing.Size(275, 55);
            this.groupBoxObjectStatus.TabIndex = 8;
            this.groupBoxObjectStatus.TabStop = false;
            this.groupBoxObjectStatus.Text = "בחר סטטוס לסינון";
            // 
            // radioButtonArchive
            // 
            this.radioButtonArchive.AutoSize = true;
            this.radioButtonArchive.Location = new System.Drawing.Point(24, 19);
            this.radioButtonArchive.Name = "radioButtonArchive";
            this.radioButtonArchive.Size = new System.Drawing.Size(61, 17);
            this.radioButtonArchive.TabIndex = 2;
            this.radioButtonArchive.Text = "ארכיון";
            this.radioButtonArchive.UseVisualStyleBackColor = true;
            // 
            // radioButtonBroadcast
            // 
            this.radioButtonBroadcast.AutoSize = true;
            this.radioButtonBroadcast.Checked = true;
            this.radioButtonBroadcast.Location = new System.Drawing.Point(189, 19);
            this.radioButtonBroadcast.Name = "radioButtonBroadcast";
            this.radioButtonBroadcast.Size = new System.Drawing.Size(71, 17);
            this.radioButtonBroadcast.TabIndex = 1;
            this.radioButtonBroadcast.TabStop = true;
            this.radioButtonBroadcast.Text = "משודרים";
            this.radioButtonBroadcast.UseVisualStyleBackColor = true;
            // 
            // radioButtonActive
            // 
            this.radioButtonActive.AutoSize = true;
            this.radioButtonActive.Location = new System.Drawing.Point(109, 19);
            this.radioButtonActive.Name = "radioButtonActive";
            this.radioButtonActive.Size = new System.Drawing.Size(63, 17);
            this.radioButtonActive.TabIndex = 0;
            this.radioButtonActive.Text = "פעילים";
            this.radioButtonActive.UseVisualStyleBackColor = true;
            // 
            // buttonShowResults
            // 
            this.buttonShowResults.Location = new System.Drawing.Point(409, 326);
            this.buttonShowResults.Name = "buttonShowResults";
            this.buttonShowResults.Size = new System.Drawing.Size(275, 48);
            this.buttonShowResults.TabIndex = 41;
            this.buttonShowResults.Text = "לחץ כאן להצגת תוצאות חיפוש";
            this.buttonShowResults.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(630, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "עד תאריך";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(640, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "מתאריך";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(518, 96);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dateTimePicker2.RightToLeftLayout = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(88, 20);
            this.dateTimePicker2.TabIndex = 37;
            // 
            // dateTimePicker21
            // 
            this.dateTimePicker21.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker21.Location = new System.Drawing.Point(518, 50);
            this.dateTimePicker21.Name = "dateTimePicker21";
            this.dateTimePicker21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dateTimePicker21.RightToLeftLayout = true;
            this.dateTimePicker21.Size = new System.Drawing.Size(88, 20);
            this.dateTimePicker21.TabIndex = 35;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.כתבותToolStripMenuItem,
            this.כתבותToolStripMenuItem1,
            this.אינדקסיםToolStripMenuItem,
            this.לינקיםToolStripMenuItem,
            this.אחרToolStripMenuItem,
            this.מולטימדיהToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(728, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // כתבותToolStripMenuItem
            // 
            this.כתבותToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemEditUserDetails,
            this.ToolStripMenuItemPasswordReminder});
            this.כתבותToolStripMenuItem.Name = "כתבותToolStripMenuItem";
            this.כתבותToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.כתבותToolStripMenuItem.Text = "הרשאות";
            // 
            // ToolStripMenuItemEditUserDetails
            // 
            this.ToolStripMenuItemEditUserDetails.Name = "ToolStripMenuItemEditUserDetails";
            this.ToolStripMenuItemEditUserDetails.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemEditUserDetails.Text = "עריכה";
            this.ToolStripMenuItemEditUserDetails.Click += new System.EventHandler(this.ToolStripMenuItemEditUserDetails_Click);
            // 
            // ToolStripMenuItemPasswordReminder
            // 
            this.ToolStripMenuItemPasswordReminder.Name = "ToolStripMenuItemPasswordReminder";
            this.ToolStripMenuItemPasswordReminder.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemPasswordReminder.Text = "שיחזור סיסמא";
            this.ToolStripMenuItemPasswordReminder.Click += new System.EventHandler(this.ToolStripMenuItemPasswordReminder_Click);
            // 
            // כתבותToolStripMenuItem1
            // 
            this.כתבותToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.הוסףחדשהToolStripMenuItem,
            this.עריכתכתבהציבוריתToolStripMenuItem,
            this.עריכתכתבהפרטיתToolStripMenuItem});
            this.כתבותToolStripMenuItem1.Name = "כתבותToolStripMenuItem1";
            this.כתבותToolStripMenuItem1.Size = new System.Drawing.Size(55, 20);
            this.כתבותToolStripMenuItem1.Text = "כתבות";
            // 
            // הוסףחדשהToolStripMenuItem
            // 
            this.הוסףחדשהToolStripMenuItem.Name = "הוסףחדשהToolStripMenuItem";
            this.הוסףחדשהToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.הוסףחדשהToolStripMenuItem.Text = "הוסף חדשה";
            this.הוסףחדשהToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItemAddArticle_Click);
            // 
            // עריכתכתבהציבוריתToolStripMenuItem
            // 
            this.עריכתכתבהציבוריתToolStripMenuItem.Name = "עריכתכתבהציבוריתToolStripMenuItem";
            this.עריכתכתבהציבוריתToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.עריכתכתבהציבוריתToolStripMenuItem.Text = "עריכת כתבה ציבורית";
            // 
            // עריכתכתבהפרטיתToolStripMenuItem
            // 
            this.עריכתכתבהפרטיתToolStripMenuItem.Name = "עריכתכתבהפרטיתToolStripMenuItem";
            this.עריכתכתבהפרטיתToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.עריכתכתבהפרטיתToolStripMenuItem.Text = "עריכת כתבה פרטית";
            // 
            // אינדקסיםToolStripMenuItem
            // 
            this.אינדקסיםToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.מודיעינעיםToolStripMenuItem,
            this.עסקיםToolStripMenuItem,
            this.בילוינעיםToolStripMenuItem});
            this.אינדקסיםToolStripMenuItem.Name = "אינדקסיםToolStripMenuItem";
            this.אינדקסיםToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.אינדקסיםToolStripMenuItem.Text = "אינדקסים";
            // 
            // מודיעינעיםToolStripMenuItem
            // 
            this.מודיעינעיםToolStripMenuItem.Name = "מודיעינעיםToolStripMenuItem";
            this.מודיעינעיםToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.מודיעינעיםToolStripMenuItem.Text = "מודיעינעים";
            this.מודיעינעיםToolStripMenuItem.Click += new System.EventHandler(this.מודיעינעיםToolStripMenuItem_Click);
            // 
            // עסקיםToolStripMenuItem
            // 
            this.עסקיםToolStripMenuItem.Name = "עסקיםToolStripMenuItem";
            this.עסקיםToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.עסקיםToolStripMenuItem.Text = "עסקים";
            this.עסקיםToolStripMenuItem.Click += new System.EventHandler(this.עסקיםToolStripMenuItem_Click);
            // 
            // בילוינעיםToolStripMenuItem
            // 
            this.בילוינעיםToolStripMenuItem.Name = "בילוינעיםToolStripMenuItem";
            this.בילוינעיםToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.בילוינעיםToolStripMenuItem.Text = "בילוי נעים";
            this.בילוינעיםToolStripMenuItem.Click += new System.EventHandler(this.בילוינעיםToolStripMenuItem_Click);
            // 
            // לינקיםToolStripMenuItem
            // 
            this.לינקיםToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemBottomPageLinks,
            this.ToolStripMenuItemAddPreferedLink,
            this.ToolStripMenuItemPreferedLinksList});
            this.לינקיםToolStripMenuItem.Name = "לינקיםToolStripMenuItem";
            this.לינקיםToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.לינקיםToolStripMenuItem.Text = "לינקים";
            // 
            // ToolStripMenuItemBottomPageLinks
            // 
            this.ToolStripMenuItemBottomPageLinks.Name = "ToolStripMenuItemBottomPageLinks";
            this.ToolStripMenuItemBottomPageLinks.Size = new System.Drawing.Size(212, 22);
            this.ToolStripMenuItemBottomPageLinks.Text = "ניהול לינקים בתחתית עמוד";
            this.ToolStripMenuItemBottomPageLinks.Click += new System.EventHandler(this.ToolStripMenuItemBottomPageLinks_Click);
            // 
            // ToolStripMenuItemAddPreferedLink
            // 
            this.ToolStripMenuItemAddPreferedLink.Name = "ToolStripMenuItemAddPreferedLink";
            this.ToolStripMenuItemAddPreferedLink.Size = new System.Drawing.Size(212, 22);
            this.ToolStripMenuItemAddPreferedLink.Text = "הוסף לינק למועדפים";
            this.ToolStripMenuItemAddPreferedLink.Click += new System.EventHandler(this.ToolStripMenuItemAddPreferedLink_Click);
            // 
            // ToolStripMenuItemPreferedLinksList
            // 
            this.ToolStripMenuItemPreferedLinksList.Name = "ToolStripMenuItemPreferedLinksList";
            this.ToolStripMenuItemPreferedLinksList.Size = new System.Drawing.Size(212, 22);
            this.ToolStripMenuItemPreferedLinksList.Text = "רשימת לינקים מועדפים";
            this.ToolStripMenuItemPreferedLinksList.Click += new System.EventHandler(this.ToolStripMenuItemPreferedLinksList_Click);
            // 
            // אחרToolStripMenuItem
            // 
            this.אחרToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTopMenu,
            this.ToolStripMenuItemRightMenu});
            this.אחרToolStripMenuItem.Name = "אחרToolStripMenuItem";
            this.אחרToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.אחרToolStripMenuItem.Text = "תפריטים";
            // 
            // ToolStripMenuItemTopMenu
            // 
            this.ToolStripMenuItemTopMenu.Name = "ToolStripMenuItemTopMenu";
            this.ToolStripMenuItemTopMenu.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemTopMenu.Text = "עליון";
            this.ToolStripMenuItemTopMenu.Click += new System.EventHandler(this.ToolStripMenuItemTopMenu_Click);
            // 
            // ToolStripMenuItemRightMenu
            // 
            this.ToolStripMenuItemRightMenu.Name = "ToolStripMenuItemRightMenu";
            this.ToolStripMenuItemRightMenu.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemRightMenu.Text = "ימין";
            this.ToolStripMenuItemRightMenu.Click += new System.EventHandler(this.ToolStripMenuItemRightMenu_Click);
            // 
            // מולטימדיהToolStripMenuItem
            // 
            this.מולטימדיהToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddNewPhoto,
            this.ToolStripMenuItemAddNewVideo,
            this.ToolStripMenuItemAddNewBanner});
            this.מולטימדיהToolStripMenuItem.Name = "מולטימדיהToolStripMenuItem";
            this.מולטימדיהToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.מולטימדיהToolStripMenuItem.Text = "מולטימדיה";
            // 
            // ToolStripMenuItemAddNewPhoto
            // 
            this.ToolStripMenuItemAddNewPhoto.Name = "ToolStripMenuItemAddNewPhoto";
            this.ToolStripMenuItemAddNewPhoto.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemAddNewPhoto.Text = "הוספת תמונות";
            this.ToolStripMenuItemAddNewPhoto.Click += new System.EventHandler(this.ToolStripMenuItemAddNewPhoto_Click);
            // 
            // ToolStripMenuItemAddNewVideo
            // 
            this.ToolStripMenuItemAddNewVideo.Name = "ToolStripMenuItemAddNewVideo";
            this.ToolStripMenuItemAddNewVideo.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemAddNewVideo.Text = "הוספת וידאו";
            this.ToolStripMenuItemAddNewVideo.Click += new System.EventHandler(this.ToolStripMenuItemAddNewVideo_Click);
            // 
            // ToolStripMenuItemAddNewBanner
            // 
            this.ToolStripMenuItemAddNewBanner.Name = "ToolStripMenuItemAddNewBanner";
            this.ToolStripMenuItemAddNewBanner.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemAddNewBanner.Text = "הוספת באנרים";
            this.ToolStripMenuItemAddNewBanner.Click += new System.EventHandler(this.ToolStripMenuItemAddNewBanner_Click);
            // 
            // userControlTreeView1
            // 
            this.userControlTreeView1.IdColumnName = "CatId";
            this.userControlTreeView1.Location = new System.Drawing.Point(37, 39);
            this.userControlTreeView1.LookupTableName = null;
            this.userControlTreeView1.MyQry = "select * FROM Table_LookupCategories WHERE ParentCatId=\'-1\'";
            this.userControlTreeView1.Name = "userControlTreeView1";
            this.userControlTreeView1.ParentIdColumnName = null;
            this.userControlTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.userControlTreeView1.RootNodeId = "1";
            this.userControlTreeView1.RootNodeName = "עמוד ראשי";
            this.userControlTreeView1.Size = new System.Drawing.Size(337, 397);
            this.userControlTreeView1.TabIndex = 8;
            this.userControlTreeView1.TextColumnName = "CatHebrewName";
            // 
            // FormAdministrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(728, 512);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAdministrator";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "מנהל אתר - כאן נעים";
            this.Load += new System.EventHandler(this.Form_Administrator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxObjectStatus.ResumeLayout(false);
            this.groupBoxObjectStatus.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem כתבותToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem כתבותToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem אחרToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker21;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTopMenu;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRightMenu;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEditUserDetails;
        private System.Windows.Forms.ToolStripMenuItem הוסףחדשהToolStripMenuItem;
        private System.Windows.Forms.Button buttonShowResults;
        private HaimDLL.UserControlTreeView userControlTreeView1;
        private System.Windows.Forms.GroupBox groupBoxObjectStatus;
        private System.Windows.Forms.RadioButton radioButtonBroadcast;
        private System.Windows.Forms.RadioButton radioButtonActive;
        private System.Windows.Forms.RadioButton radioButtonArchive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.ToolStripMenuItem עריכתכתבהציבוריתToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem עריכתכתבהפרטיתToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem אינדקסיםToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem מודיעינעיםToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem עסקיםToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem בילוינעיםToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem לינקיםToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemBottomPageLinks;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddPreferedLink;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPasswordReminder;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPreferedLinksList;
        private System.Windows.Forms.ToolStripMenuItem מולטימדיהToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddNewPhoto;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddNewVideo;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddNewBanner;
    }
}
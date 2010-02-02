using HaimDLL;

namespace Kan_Naim_Main
{
    partial class FormEditArtical
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer _components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
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
            this._tabControl1 = new System.Windows.Forms.TabControl();
            this._tabPageArticle = new System.Windows.Forms.TabPage();
            this._groupBox8 = new System.Windows.Forms.GroupBox();
            this.buttonCopySubtitelToTak = new System.Windows.Forms.Button();
            this.buttonCopyTitleToTak = new System.Windows.Forms.Button();
            this._richTextBoxArticleContent = new System.Windows.Forms.RichTextBox();
            this._comboBoxVideoPos = new System.Windows.Forms.ComboBox();
            this._comboBoxImgPos = new System.Windows.Forms.ComboBox();
            this._buttonOpenEditor = new System.Windows.Forms.Button();
            this._labelOriginPhotoId = new System.Windows.Forms.Label();
            this._comboBoxArticleCategory = new System.Windows.Forms.ComboBox();
            this._tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSetCategories = new Kan_Naim_Main._Kan_NaimDataSetCategories();
            this._label22 = new System.Windows.Forms.Label();
            this._buttonSearchVideosArchive = new System.Windows.Forms.Button();
            this._buttonSearchPhotosArchive = new System.Windows.Forms.Button();
            this._comboBoxArticlePhoto = new System.Windows.Forms.ComboBox();
            this._tablePhotosArchiveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSet1 = new Kan_Naim_Main._Kan_NaimDataSet1();
            this._comboBoxArticleVideo = new System.Windows.Forms.ComboBox();
            this._dateTimePicker22 = new System.Windows.Forms.DateTimePicker();
            this._labelArtical = new System.Windows.Forms.Label();
            this._textBoxKeyWords = new System.Windows.Forms.TextBox();
            this._labelKeyWords = new System.Windows.Forms.Label();
            this._textBoxTags = new System.Windows.Forms.TextBox();
            this._labelTags = new System.Windows.Forms.Label();
            this._labelEditor = new System.Windows.Forms.Label();
            this._textBoxArticleTitle = new System.Windows.Forms.TextBox();
            this._comboBoxEditor = new System.Windows.Forms.ComboBox();
            this._tableLookupReportersBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSetReportersNames = new Kan_Naim_Main._Kan_NaimDataSetReportersNames();
            this._dateTimePicker21 = new System.Windows.Forms.DateTimePicker();
            this._checkBoxMivzak = new System.Windows.Forms.CheckBox();
            this._checkBoxDateTime = new System.Windows.Forms.CheckBox();
            this._checkBoxRss = new System.Windows.Forms.CheckBox();
            this._checkBoxPublish = new System.Windows.Forms.CheckBox();
            this._labelTitle = new System.Windows.Forms.Label();
            this._labelSubtitle = new System.Windows.Forms.Label();
            this._textBoxArticleSubtitle = new System.Windows.Forms.TextBox();
            this._buttonTitleH1 = new System.Windows.Forms.Button();
            this._buttonSubTitleH2 = new System.Windows.Forms.Button();
            this._tabPageTak3X = new System.Windows.Forms.TabPage();
            this._label5 = new System.Windows.Forms.Label();
            this._userControlTakFillSizeX3 = new HaimDLL.UserControlTakFill();
            this._tabPageTak2X = new System.Windows.Forms.TabPage();
            this._label4 = new System.Windows.Forms.Label();
            this._userControlTakFillSizeX2 = new HaimDLL.UserControlTakFill();
            this._tabPageTak1X = new System.Windows.Forms.TabPage();
            this._label3 = new System.Windows.Forms.Label();
            this._userControlTakFillSizeX1 = new HaimDLL.UserControlTakFill();
            this._tabPageTakMedium = new System.Windows.Forms.TabPage();
            this._label2 = new System.Windows.Forms.Label();
            this._userControlTakFillSizeMedium = new HaimDLL.UserControlTakFill();
            this._tabPageTakSmall = new System.Windows.Forms.TabPage();
            this._label1 = new System.Windows.Forms.Label();
            this._userControlTakFillSizeSmall = new HaimDLL.UserControlTakFill();
            this._tabPageCategories = new System.Windows.Forms.TabPage();
            this._groupBox7 = new System.Windows.Forms.GroupBox();
            this._userControlTreeView1 = new HaimDLL.UserControlTreeView();
            this._buttonManageCategories = new System.Windows.Forms.Button();
            this._buttonReloadCategoryTree = new System.Windows.Forms.Button();
            this._buttonAddAllCategories = new System.Windows.Forms.Button();
            this._buttonClearCategoriesList = new System.Windows.Forms.Button();
            this._listBoxSelectedCategories = new System.Windows.Forms.ListBox();
            this._buttonAddSelectedCategories = new System.Windows.Forms.Button();
            this._buttonRemoveSelectedCategory = new System.Windows.Forms.Button();
            this._tabPagePhotos = new System.Windows.Forms.TabPage();
            this._ucUploadPhoto1 = new HaimDLL.UserControlUploadPhoto();
            this._tabPageVideo = new System.Windows.Forms.TabPage();
            this._ucUploadVideo1 = new HaimDLL.UserControlUploadVideo();
            this._tabPageAutoPublish = new System.Windows.Forms.TabPage();
            this._buttonArticlePreview = new System.Windows.Forms.Button();
            this._contextMenuStripTreeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._toolStripMenuItemAddCategory = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripMenuItemDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripMenuItemUpdateCategory = new System.Windows.Forms.ToolStripMenuItem();
            this._tableLookupReportersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSetReporters = new Kan_Naim_Main._Kan_NaimDataSetReporters();
            this._buttonSaveArticle = new System.Windows.Forms.Button();
            this._kanNaimDataSet = new Kan_Naim_Main._Kan_NaimDataSet();
            this._tableLookupArticleStatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._tableLookupArticleStatusTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetTableAdapters.Table_LookupArticleStatusTableAdapter();
            this._tableLookupReportersTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetReportersTableAdapters.Table_LookupReportersTableAdapter();
            this._tableLookupCategoriesTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            this._tableLookupReportersTableAdapter1 = new Kan_Naim_Main._Kan_NaimDataSetReportersNamesTableAdapters.Table_LookupReportersTableAdapter();
            this._tablePhotosArchiveTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet1TableAdapters.Table_PhotosArchiveTableAdapter();
            this._kanNaimDataSet2 = new Kan_Naim_Main._Kan_NaimDataSet2();
            this._spGetAllPhotosByOriginIdBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._spGetAllPhotosByOriginIdTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet2TableAdapters.sp_GetAllPhotosByOriginIdTableAdapter();
            this._groupBox1 = new System.Windows.Forms.GroupBox();
            this._radioButtonSaveAsPrivate = new System.Windows.Forms.RadioButton();
            this._radioButtonSaveAsPublic = new System.Windows.Forms.RadioButton();
            this._tabControl1.SuspendLayout();
            this._tabPageArticle.SuspendLayout();
            this._groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tablePhotosArchiveBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReportersNames)).BeginInit();
            this._tabPageTak3X.SuspendLayout();
            this._tabPageTak2X.SuspendLayout();
            this._tabPageTak1X.SuspendLayout();
            this._tabPageTakMedium.SuspendLayout();
            this._tabPageTakSmall.SuspendLayout();
            this._tabPageCategories.SuspendLayout();
            this._groupBox7.SuspendLayout();
            this._tabPagePhotos.SuspendLayout();
            this._tabPageVideo.SuspendLayout();
            this._contextMenuStripTreeNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReporters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupArticleStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spGetAllPhotosByOriginIdBindingSource)).BeginInit();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl1
            // 
            this._tabControl1.Controls.Add(this._tabPageArticle);
            this._tabControl1.Controls.Add(this._tabPageTak3X);
            this._tabControl1.Controls.Add(this._tabPageTak2X);
            this._tabControl1.Controls.Add(this._tabPageTak1X);
            this._tabControl1.Controls.Add(this._tabPageTakMedium);
            this._tabControl1.Controls.Add(this._tabPageTakSmall);
            this._tabControl1.Controls.Add(this._tabPageCategories);
            this._tabControl1.Controls.Add(this._tabPagePhotos);
            this._tabControl1.Controls.Add(this._tabPageVideo);
            this._tabControl1.Controls.Add(this._tabPageAutoPublish);
            this._tabControl1.HotTrack = true;
            this._tabControl1.Location = new System.Drawing.Point(12, 27);
            this._tabControl1.Name = "_tabControl1";
            this._tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._tabControl1.RightToLeftLayout = true;
            this._tabControl1.SelectedIndex = 0;
            this._tabControl1.Size = new System.Drawing.Size(716, 485);
            this._tabControl1.TabIndex = 4;
            // 
            // _tabPageArticle
            // 
            this._tabPageArticle.Controls.Add(this._groupBox8);
            this._tabPageArticle.Location = new System.Drawing.Point(4, 22);
            this._tabPageArticle.Name = "_tabPageArticle";
            this._tabPageArticle.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageArticle.Size = new System.Drawing.Size(708, 459);
            this._tabPageArticle.TabIndex = 0;
            this._tabPageArticle.Text = "כתבה";
            this._tabPageArticle.UseVisualStyleBackColor = true;
            // 
            // _groupBox8
            // 
            this._groupBox8.Controls.Add(this.buttonCopySubtitelToTak);
            this._groupBox8.Controls.Add(this.buttonCopyTitleToTak);
            this._groupBox8.Controls.Add(this._richTextBoxArticleContent);
            this._groupBox8.Controls.Add(this._comboBoxVideoPos);
            this._groupBox8.Controls.Add(this._comboBoxImgPos);
            this._groupBox8.Controls.Add(this._buttonOpenEditor);
            this._groupBox8.Controls.Add(this._labelOriginPhotoId);
            this._groupBox8.Controls.Add(this._comboBoxArticleCategory);
            this._groupBox8.Controls.Add(this._label22);
            this._groupBox8.Controls.Add(this._buttonSearchVideosArchive);
            this._groupBox8.Controls.Add(this._buttonSearchPhotosArchive);
            this._groupBox8.Controls.Add(this._comboBoxArticlePhoto);
            this._groupBox8.Controls.Add(this._comboBoxArticleVideo);
            this._groupBox8.Controls.Add(this._dateTimePicker22);
            this._groupBox8.Controls.Add(this._labelArtical);
            this._groupBox8.Controls.Add(this._textBoxKeyWords);
            this._groupBox8.Controls.Add(this._labelKeyWords);
            this._groupBox8.Controls.Add(this._textBoxTags);
            this._groupBox8.Controls.Add(this._labelTags);
            this._groupBox8.Controls.Add(this._labelEditor);
            this._groupBox8.Controls.Add(this._textBoxArticleTitle);
            this._groupBox8.Controls.Add(this._comboBoxEditor);
            this._groupBox8.Controls.Add(this._dateTimePicker21);
            this._groupBox8.Controls.Add(this._checkBoxMivzak);
            this._groupBox8.Controls.Add(this._checkBoxDateTime);
            this._groupBox8.Controls.Add(this._checkBoxRss);
            this._groupBox8.Controls.Add(this._checkBoxPublish);
            this._groupBox8.Controls.Add(this._labelTitle);
            this._groupBox8.Controls.Add(this._labelSubtitle);
            this._groupBox8.Controls.Add(this._textBoxArticleSubtitle);
            this._groupBox8.Controls.Add(this._buttonTitleH1);
            this._groupBox8.Controls.Add(this._buttonSubTitleH2);
            this._groupBox8.Location = new System.Drawing.Point(15, 6);
            this._groupBox8.Name = "_groupBox8";
            this._groupBox8.Size = new System.Drawing.Size(675, 447);
            this._groupBox8.TabIndex = 4;
            this._groupBox8.TabStop = false;
            this._groupBox8.Text = "הזנת תוכן ומאפייני הכתבה";
            // 
            // buttonCopySubtitelToTak
            // 
            this.buttonCopySubtitelToTak.Location = new System.Drawing.Point(568, 100);
            this.buttonCopySubtitelToTak.Name = "buttonCopySubtitelToTak";
            this.buttonCopySubtitelToTak.Size = new System.Drawing.Size(75, 23);
            this.buttonCopySubtitelToTak.TabIndex = 87;
            this.buttonCopySubtitelToTak.Text = "לתקצירים";
            this.buttonCopySubtitelToTak.UseVisualStyleBackColor = true;
            this.buttonCopySubtitelToTak.Click += new System.EventHandler(this.buttonCopySubtitelToTak_Click);
            // 
            // buttonCopyTitleToTak
            // 
            this.buttonCopyTitleToTak.Location = new System.Drawing.Point(565, 35);
            this.buttonCopyTitleToTak.Name = "buttonCopyTitleToTak";
            this.buttonCopyTitleToTak.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyTitleToTak.TabIndex = 86;
            this.buttonCopyTitleToTak.Text = "לתקצירים";
            this.buttonCopyTitleToTak.UseVisualStyleBackColor = true;
            this.buttonCopyTitleToTak.Click += new System.EventHandler(this.buttonCopyTitleToTak_Click);
            // 
            // _richTextBoxArticleContent
            // 
            this._richTextBoxArticleContent.Enabled = false;
            this._richTextBoxArticleContent.Location = new System.Drawing.Point(19, 306);
            this._richTextBoxArticleContent.MaxLength = 10000;
            this._richTextBoxArticleContent.Name = "_richTextBoxArticleContent";
            this._richTextBoxArticleContent.Size = new System.Drawing.Size(541, 123);
            this._richTextBoxArticleContent.TabIndex = 85;
            this._richTextBoxArticleContent.Text = "";
            // 
            // _comboBoxVideoPos
            // 
            this._comboBoxVideoPos.Enabled = false;
            this._comboBoxVideoPos.FormattingEnabled = true;
            this._comboBoxVideoPos.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "110",
            "120",
            "130",
            "140",
            "150",
            "160",
            "170",
            "180",
            "190",
            "200"});
            this._comboBoxVideoPos.Location = new System.Drawing.Point(19, 226);
            this._comboBoxVideoPos.Name = "_comboBoxVideoPos";
            this._comboBoxVideoPos.Size = new System.Drawing.Size(70, 21);
            this._comboBoxVideoPos.TabIndex = 84;
            this._comboBoxVideoPos.Text = "  מלמעלה";
            this._comboBoxVideoPos.Visible = false;
            // 
            // _comboBoxImgPos
            // 
            this._comboBoxImgPos.Enabled = false;
            this._comboBoxImgPos.FormattingEnabled = true;
            this._comboBoxImgPos.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "110",
            "120",
            "130",
            "140",
            "150",
            "160",
            "170",
            "180",
            "190",
            "200"});
            this._comboBoxImgPos.Location = new System.Drawing.Point(19, 199);
            this._comboBoxImgPos.Name = "_comboBoxImgPos";
            this._comboBoxImgPos.Size = new System.Drawing.Size(70, 21);
            this._comboBoxImgPos.TabIndex = 83;
            this._comboBoxImgPos.Text = "  מלמעלה";
            this._comboBoxImgPos.Visible = false;
            // 
            // _buttonOpenEditor
            // 
            this._buttonOpenEditor.Location = new System.Drawing.Point(573, 333);
            this._buttonOpenEditor.Name = "_buttonOpenEditor";
            this._buttonOpenEditor.Size = new System.Drawing.Size(59, 23);
            this._buttonOpenEditor.TabIndex = 76;
            this._buttonOpenEditor.Text = "עריכה";
            this._buttonOpenEditor.UseVisualStyleBackColor = true;
            this._buttonOpenEditor.Click += new System.EventHandler(this.buttonOpenEditor_Click);
            // 
            // _labelOriginPhotoId
            // 
            this._labelOriginPhotoId.AutoSize = true;
            this._labelOriginPhotoId.Location = new System.Drawing.Point(642, 321);
            this._labelOriginPhotoId.Name = "_labelOriginPhotoId";
            this._labelOriginPhotoId.Size = new System.Drawing.Size(13, 13);
            this._labelOriginPhotoId.TabIndex = 62;
            this._labelOriginPhotoId.Text = "0";
            this._labelOriginPhotoId.Visible = false;
            // 
            // _comboBoxArticleCategory
            // 
            this._comboBoxArticleCategory.DataSource = this._tableLookupCategoriesBindingSource;
            this._comboBoxArticleCategory.DisplayMember = "CatHebrewName";
            this._comboBoxArticleCategory.FormattingEnabled = true;
            this._comboBoxArticleCategory.Location = new System.Drawing.Point(19, 172);
            this._comboBoxArticleCategory.Name = "_comboBoxArticleCategory";
            this._comboBoxArticleCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._comboBoxArticleCategory.Size = new System.Drawing.Size(193, 21);
            this._comboBoxArticleCategory.TabIndex = 61;
            this._comboBoxArticleCategory.ValueMember = "CatId";
            this._comboBoxArticleCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxArticleCategory_SelectedIndexChanged);
            // 
            // _tableLookupCategoriesBindingSource
            // 
            this._tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            this._tableLookupCategoriesBindingSource.DataSource = this._kanNaimDataSetCategories;
            // 
            // _kanNaimDataSetCategories
            // 
            this._kanNaimDataSetCategories.DataSetName = "_Kan_NaimDataSetCategories";
            this._kanNaimDataSetCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _label22
            // 
            this._label22.AutoSize = true;
            this._label22.Location = new System.Drawing.Point(218, 175);
            this._label22.Name = "_label22";
            this._label22.Size = new System.Drawing.Size(50, 13);
            this._label22.TabIndex = 60;
            this._label22.Text = "קטגוריה";
            // 
            // _buttonSearchVideosArchive
            // 
            this._buttonSearchVideosArchive.Enabled = false;
            this._buttonSearchVideosArchive.Location = new System.Drawing.Point(568, 224);
            this._buttonSearchVideosArchive.Name = "_buttonSearchVideosArchive";
            this._buttonSearchVideosArchive.Size = new System.Drawing.Size(75, 23);
            this._buttonSearchVideosArchive.TabIndex = 59;
            this._buttonSearchVideosArchive.Text = "וידאו..";
            this._buttonSearchVideosArchive.UseVisualStyleBackColor = true;
            // 
            // _buttonSearchPhotosArchive
            // 
            this._buttonSearchPhotosArchive.Enabled = false;
            this._buttonSearchPhotosArchive.Location = new System.Drawing.Point(568, 197);
            this._buttonSearchPhotosArchive.Name = "_buttonSearchPhotosArchive";
            this._buttonSearchPhotosArchive.Size = new System.Drawing.Size(75, 23);
            this._buttonSearchPhotosArchive.TabIndex = 58;
            this._buttonSearchPhotosArchive.Text = "תמונה..";
            this._buttonSearchPhotosArchive.UseVisualStyleBackColor = true;
            // 
            // _comboBoxArticlePhoto
            // 
            this._comboBoxArticlePhoto.DataSource = this._tablePhotosArchiveBindingSource;
            this._comboBoxArticlePhoto.DisplayMember = "ImageUrl";
            this._comboBoxArticlePhoto.FormattingEnabled = true;
            this._comboBoxArticlePhoto.Location = new System.Drawing.Point(95, 199);
            this._comboBoxArticlePhoto.Name = "_comboBoxArticlePhoto";
            this._comboBoxArticlePhoto.Size = new System.Drawing.Size(465, 21);
            this._comboBoxArticlePhoto.TabIndex = 57;
            this._comboBoxArticlePhoto.ValueMember = "Id";
            this._comboBoxArticlePhoto.SelectedIndexChanged += new System.EventHandler(this.comboBoxArticlePhoto_SelectedIndexChanged);
            // 
            // _tablePhotosArchiveBindingSource
            // 
            this._tablePhotosArchiveBindingSource.DataMember = "Table_PhotosArchive";
            this._tablePhotosArchiveBindingSource.DataSource = this._kanNaimDataSet1;
            // 
            // _kanNaimDataSet1
            // 
            this._kanNaimDataSet1.DataSetName = "_Kan_NaimDataSet1";
            this._kanNaimDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _comboBoxArticleVideo
            // 
            this._comboBoxArticleVideo.Enabled = false;
            this._comboBoxArticleVideo.FormattingEnabled = true;
            this._comboBoxArticleVideo.Location = new System.Drawing.Point(95, 226);
            this._comboBoxArticleVideo.Name = "_comboBoxArticleVideo";
            this._comboBoxArticleVideo.Size = new System.Drawing.Size(465, 21);
            this._comboBoxArticleVideo.TabIndex = 56;
            this._comboBoxArticleVideo.SelectedIndexChanged += new System.EventHandler(this.comboBoxArticleVideo_SelectedIndexChanged);
            // 
            // _dateTimePicker22
            // 
            this._dateTimePicker22.Enabled = false;
            this._dateTimePicker22.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._dateTimePicker22.Location = new System.Drawing.Point(19, 146);
            this._dateTimePicker22.Name = "_dateTimePicker22";
            this._dateTimePicker22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._dateTimePicker22.RightToLeftLayout = true;
            this._dateTimePicker22.Size = new System.Drawing.Size(90, 20);
            this._dateTimePicker22.TabIndex = 34;
            // 
            // _labelArtical
            // 
            this._labelArtical.AutoSize = true;
            this._labelArtical.Location = new System.Drawing.Point(573, 306);
            this._labelArtical.Name = "_labelArtical";
            this._labelArtical.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._labelArtical.Size = new System.Drawing.Size(62, 13);
            this._labelArtical.TabIndex = 24;
            this._labelArtical.Text = "תוכן כתבה";
            // 
            // _textBoxKeyWords
            // 
            this._textBoxKeyWords.Location = new System.Drawing.Point(19, 280);
            this._textBoxKeyWords.MaxLength = 200;
            this._textBoxKeyWords.Name = "_textBoxKeyWords";
            this._textBoxKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxKeyWords.Size = new System.Drawing.Size(541, 20);
            this._textBoxKeyWords.TabIndex = 23;
            // 
            // _labelKeyWords
            // 
            this._labelKeyWords.AutoSize = true;
            this._labelKeyWords.Location = new System.Drawing.Point(573, 283);
            this._labelKeyWords.Name = "_labelKeyWords";
            this._labelKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._labelKeyWords.Size = new System.Drawing.Size(74, 13);
            this._labelKeyWords.TabIndex = 22;
            this._labelKeyWords.Text = "מילות חיפוש";
            // 
            // _textBoxTags
            // 
            this._textBoxTags.Location = new System.Drawing.Point(19, 253);
            this._textBoxTags.MaxLength = 200;
            this._textBoxTags.Name = "_textBoxTags";
            this._textBoxTags.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxTags.Size = new System.Drawing.Size(541, 20);
            this._textBoxTags.TabIndex = 21;
            // 
            // _labelTags
            // 
            this._labelTags.AutoSize = true;
            this._labelTags.Location = new System.Drawing.Point(573, 260);
            this._labelTags.Name = "_labelTags";
            this._labelTags.Size = new System.Drawing.Size(37, 13);
            this._labelTags.TabIndex = 20;
            this._labelTags.Text = "תגיות";
            // 
            // _labelEditor
            // 
            this._labelEditor.AutoSize = true;
            this._labelEditor.Location = new System.Drawing.Point(573, 175);
            this._labelEditor.Name = "_labelEditor";
            this._labelEditor.Size = new System.Drawing.Size(32, 13);
            this._labelEditor.TabIndex = 18;
            this._labelEditor.Text = "עורך";
            // 
            // _textBoxArticleTitle
            // 
            this._textBoxArticleTitle.Location = new System.Drawing.Point(17, 20);
            this._textBoxArticleTitle.MaxLength = 150;
            this._textBoxArticleTitle.Multiline = true;
            this._textBoxArticleTitle.Name = "_textBoxArticleTitle";
            this._textBoxArticleTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxArticleTitle.Size = new System.Drawing.Size(541, 42);
            this._textBoxArticleTitle.TabIndex = 15;
            // 
            // _comboBoxEditor
            // 
            this._comboBoxEditor.DataSource = this._tableLookupReportersBindingSource1;
            this._comboBoxEditor.DisplayMember = "PublishNameShort";
            this._comboBoxEditor.FormattingEnabled = true;
            this._comboBoxEditor.Location = new System.Drawing.Point(291, 172);
            this._comboBoxEditor.Name = "_comboBoxEditor";
            this._comboBoxEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._comboBoxEditor.Size = new System.Drawing.Size(269, 21);
            this._comboBoxEditor.TabIndex = 10;
            this._comboBoxEditor.ValueMember = "UserId";
            // 
            // _tableLookupReportersBindingSource1
            // 
            this._tableLookupReportersBindingSource1.DataMember = "Table_LookupReporters";
            this._tableLookupReportersBindingSource1.DataSource = this._kanNaimDataSetReportersNames;
            // 
            // _kanNaimDataSetReportersNames
            // 
            this._kanNaimDataSetReportersNames.DataSetName = "_Kan_NaimDataSetReportersNames";
            this._kanNaimDataSetReportersNames.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _dateTimePicker21
            // 
            this._dateTimePicker21.Enabled = false;
            this._dateTimePicker21.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dateTimePicker21.Location = new System.Drawing.Point(125, 146);
            this._dateTimePicker21.MaxDate = new System.DateTime(2015, 12, 31, 0, 0, 0, 0);
            this._dateTimePicker21.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this._dateTimePicker21.Name = "_dateTimePicker21";
            this._dateTimePicker21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._dateTimePicker21.RightToLeftLayout = true;
            this._dateTimePicker21.Size = new System.Drawing.Size(87, 20);
            this._dateTimePicker21.TabIndex = 9;
            this._dateTimePicker21.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // _checkBoxMivzak
            // 
            this._checkBoxMivzak.AutoSize = true;
            this._checkBoxMivzak.Checked = true;
            this._checkBoxMivzak.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxMivzak.Location = new System.Drawing.Point(422, 149);
            this._checkBoxMivzak.Name = "_checkBoxMivzak";
            this._checkBoxMivzak.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxMivzak.Size = new System.Drawing.Size(52, 17);
            this._checkBoxMivzak.TabIndex = 7;
            this._checkBoxMivzak.Tag = "";
            this._checkBoxMivzak.Text = "מבזק";
            this._checkBoxMivzak.UseVisualStyleBackColor = true;
            // 
            // _checkBoxDateTime
            // 
            this._checkBoxDateTime.AutoSize = true;
            this._checkBoxDateTime.Checked = true;
            this._checkBoxDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxDateTime.Location = new System.Drawing.Point(218, 149);
            this._checkBoxDateTime.Name = "_checkBoxDateTime";
            this._checkBoxDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxDateTime.Size = new System.Drawing.Size(90, 17);
            this._checkBoxDateTime.TabIndex = 6;
            this._checkBoxDateTime.Tag = "";
            this._checkBoxDateTime.Text = "תאריך ושעה";
            this._checkBoxDateTime.UseVisualStyleBackColor = true;
            // 
            // _checkBoxRss
            // 
            this._checkBoxRss.AutoSize = true;
            this._checkBoxRss.Checked = true;
            this._checkBoxRss.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxRss.Location = new System.Drawing.Point(348, 149);
            this._checkBoxRss.Name = "_checkBoxRss";
            this._checkBoxRss.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxRss.Size = new System.Drawing.Size(48, 17);
            this._checkBoxRss.TabIndex = 5;
            this._checkBoxRss.Tag = "";
            this._checkBoxRss.Text = "RSS";
            this._checkBoxRss.UseVisualStyleBackColor = true;
            // 
            // _checkBoxPublish
            // 
            this._checkBoxPublish.AutoSize = true;
            this._checkBoxPublish.Checked = true;
            this._checkBoxPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxPublish.Location = new System.Drawing.Point(494, 149);
            this._checkBoxPublish.Name = "_checkBoxPublish";
            this._checkBoxPublish.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxPublish.Size = new System.Drawing.Size(53, 17);
            this._checkBoxPublish.TabIndex = 4;
            this._checkBoxPublish.Tag = "";
            this._checkBoxPublish.Text = "פרסם";
            this._checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // _labelTitle
            // 
            this._labelTitle.AutoSize = true;
            this._labelTitle.Location = new System.Drawing.Point(564, 19);
            this._labelTitle.Name = "_labelTitle";
            this._labelTitle.Size = new System.Drawing.Size(77, 13);
            this._labelTitle.TabIndex = 3;
            this._labelTitle.Text = "כותרת ראשית";
            // 
            // _labelSubtitle
            // 
            this._labelSubtitle.AutoSize = true;
            this._labelSubtitle.Location = new System.Drawing.Point(568, 84);
            this._labelSubtitle.Name = "_labelSubtitle";
            this._labelSubtitle.Size = new System.Drawing.Size(75, 13);
            this._labelSubtitle.TabIndex = 0;
            this._labelSubtitle.Text = "כותרת משנית";
            // 
            // _textBoxArticleSubtitle
            // 
            this._textBoxArticleSubtitle.Location = new System.Drawing.Point(17, 68);
            this._textBoxArticleSubtitle.MaxLength = 300;
            this._textBoxArticleSubtitle.Multiline = true;
            this._textBoxArticleSubtitle.Name = "_textBoxArticleSubtitle";
            this._textBoxArticleSubtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxArticleSubtitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxArticleSubtitle.Size = new System.Drawing.Size(541, 72);
            this._textBoxArticleSubtitle.TabIndex = 14;
            // 
            // _buttonTitleH1
            // 
            this._buttonTitleH1.Enabled = false;
            this._buttonTitleH1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonTitleH1.Location = new System.Drawing.Point(573, 380);
            this._buttonTitleH1.Name = "_buttonTitleH1";
            this._buttonTitleH1.Size = new System.Drawing.Size(59, 20);
            this._buttonTitleH1.TabIndex = 78;
            this._buttonTitleH1.Text = "H1";
            this._buttonTitleH1.UseVisualStyleBackColor = true;
            this._buttonTitleH1.Visible = false;
            this._buttonTitleH1.Click += new System.EventHandler(this.buttonTitlesH1andH2_Click);
            // 
            // _buttonSubTitleH2
            // 
            this._buttonSubTitleH2.Enabled = false;
            this._buttonSubTitleH2.Location = new System.Drawing.Point(573, 406);
            this._buttonSubTitleH2.Name = "_buttonSubTitleH2";
            this._buttonSubTitleH2.Size = new System.Drawing.Size(59, 23);
            this._buttonSubTitleH2.TabIndex = 79;
            this._buttonSubTitleH2.Text = "H2";
            this._buttonSubTitleH2.UseVisualStyleBackColor = true;
            this._buttonSubTitleH2.Visible = false;
            this._buttonSubTitleH2.Click += new System.EventHandler(this.buttonTitlesH1andH2_Click);
            // 
            // _tabPageTak3X
            // 
            this._tabPageTak3X.Controls.Add(this._label5);
            this._tabPageTak3X.Controls.Add(this._userControlTakFillSizeX3);
            this._tabPageTak3X.Location = new System.Drawing.Point(4, 22);
            this._tabPageTak3X.Name = "_tabPageTak3X";
            this._tabPageTak3X.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTak3X.Size = new System.Drawing.Size(708, 459);
            this._tabPageTak3X.TabIndex = 1;
            this._tabPageTak3X.Text = "תקציר גדול X3";
            this._tabPageTak3X.UseVisualStyleBackColor = true;
            // 
            // _label5
            // 
            this._label5.AutoSize = true;
            this._label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label5.Location = new System.Drawing.Point(644, 28);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(55, 13);
            this._label5.TabIndex = 3;
            this._label5.Text = "בגודל X3";
            // 
            // _userControlTakFillSizeX3
            // 
            this._userControlTakFillSizeX3.Location = new System.Drawing.Point(6, 6);
            this._userControlTakFillSizeX3.Name = "_userControlTakFillSizeX3";
            this._userControlTakFillSizeX3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeX3.Size = new System.Drawing.Size(640, 445);
            this._userControlTakFillSizeX3.TabIndex = 0;
            // 
            // _tabPageTak2X
            // 
            this._tabPageTak2X.Controls.Add(this._label4);
            this._tabPageTak2X.Controls.Add(this._userControlTakFillSizeX2);
            this._tabPageTak2X.Location = new System.Drawing.Point(4, 22);
            this._tabPageTak2X.Name = "_tabPageTak2X";
            this._tabPageTak2X.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTak2X.Size = new System.Drawing.Size(708, 459);
            this._tabPageTak2X.TabIndex = 2;
            this._tabPageTak2X.Text = "תקציר גדול X2";
            this._tabPageTak2X.UseVisualStyleBackColor = true;
            // 
            // _label4
            // 
            this._label4.AutoSize = true;
            this._label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label4.Location = new System.Drawing.Point(647, 25);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(55, 13);
            this._label4.TabIndex = 3;
            this._label4.Text = "בגודל X2";
            // 
            // _userControlTakFillSizeX2
            // 
            this._userControlTakFillSizeX2.Location = new System.Drawing.Point(6, 8);
            this._userControlTakFillSizeX2.Name = "_userControlTakFillSizeX2";
            this._userControlTakFillSizeX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeX2.Size = new System.Drawing.Size(635, 445);
            this._userControlTakFillSizeX2.TabIndex = 0;
            // 
            // _tabPageTak1X
            // 
            this._tabPageTak1X.Controls.Add(this._label3);
            this._tabPageTak1X.Controls.Add(this._userControlTakFillSizeX1);
            this._tabPageTak1X.Location = new System.Drawing.Point(4, 22);
            this._tabPageTak1X.Name = "_tabPageTak1X";
            this._tabPageTak1X.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTak1X.Size = new System.Drawing.Size(708, 459);
            this._tabPageTak1X.TabIndex = 3;
            this._tabPageTak1X.Text = "תקציר גדול X1";
            this._tabPageTak1X.UseVisualStyleBackColor = true;
            // 
            // _label3
            // 
            this._label3.AutoSize = true;
            this._label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label3.Location = new System.Drawing.Point(644, 18);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(55, 13);
            this._label3.TabIndex = 2;
            this._label3.Text = "בגודל X1";
            // 
            // _userControlTakFillSizeX1
            // 
            this._userControlTakFillSizeX1.Location = new System.Drawing.Point(6, 3);
            this._userControlTakFillSizeX1.Name = "_userControlTakFillSizeX1";
            this._userControlTakFillSizeX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeX1.Size = new System.Drawing.Size(635, 445);
            this._userControlTakFillSizeX1.TabIndex = 0;
            // 
            // _tabPageTakMedium
            // 
            this._tabPageTakMedium.Controls.Add(this._label2);
            this._tabPageTakMedium.Controls.Add(this._userControlTakFillSizeMedium);
            this._tabPageTakMedium.Location = new System.Drawing.Point(4, 22);
            this._tabPageTakMedium.Name = "_tabPageTakMedium";
            this._tabPageTakMedium.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTakMedium.Size = new System.Drawing.Size(708, 459);
            this._tabPageTakMedium.TabIndex = 4;
            this._tabPageTakMedium.Text = "תקציר בינוני";
            this._tabPageTakMedium.UseVisualStyleBackColor = true;
            // 
            // _label2
            // 
            this._label2.AutoSize = true;
            this._label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label2.Location = new System.Drawing.Point(638, 21);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(74, 13);
            this._label2.TabIndex = 2;
            this._label2.Text = "בגודל בינוני";
            // 
            // _userControlTakFillSizeMedium
            // 
            this._userControlTakFillSizeMedium.Location = new System.Drawing.Point(6, 6);
            this._userControlTakFillSizeMedium.Name = "_userControlTakFillSizeMedium";
            this._userControlTakFillSizeMedium.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeMedium.Size = new System.Drawing.Size(635, 445);
            this._userControlTakFillSizeMedium.TabIndex = 0;
            // 
            // _tabPageTakSmall
            // 
            this._tabPageTakSmall.Controls.Add(this._label1);
            this._tabPageTakSmall.Controls.Add(this._userControlTakFillSizeSmall);
            this._tabPageTakSmall.Location = new System.Drawing.Point(4, 22);
            this._tabPageTakSmall.Name = "_tabPageTakSmall";
            this._tabPageTakSmall.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTakSmall.Size = new System.Drawing.Size(708, 459);
            this._tabPageTakSmall.TabIndex = 5;
            this._tabPageTakSmall.Text = "תקציר קטן";
            this._tabPageTakSmall.UseVisualStyleBackColor = true;
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label1.Location = new System.Drawing.Point(641, 25);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(61, 13);
            this._label1.TabIndex = 1;
            this._label1.Text = "בגודל קטן";
            // 
            // _userControlTakFillSizeSmall
            // 
            this._userControlTakFillSizeSmall.Location = new System.Drawing.Point(6, 8);
            this._userControlTakFillSizeSmall.Name = "_userControlTakFillSizeSmall";
            this._userControlTakFillSizeSmall.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeSmall.Size = new System.Drawing.Size(635, 445);
            this._userControlTakFillSizeSmall.TabIndex = 0;
            // 
            // _tabPageCategories
            // 
            this._tabPageCategories.Controls.Add(this._groupBox7);
            this._tabPageCategories.Location = new System.Drawing.Point(4, 22);
            this._tabPageCategories.Name = "_tabPageCategories";
            this._tabPageCategories.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageCategories.Size = new System.Drawing.Size(708, 459);
            this._tabPageCategories.TabIndex = 6;
            this._tabPageCategories.Text = "קטגוריות";
            this._tabPageCategories.UseVisualStyleBackColor = true;
            this._tabPageCategories.Enter += new System.EventHandler(this.tabPageCategories_Enter);
            // 
            // _groupBox7
            // 
            this._groupBox7.Controls.Add(this._userControlTreeView1);
            this._groupBox7.Controls.Add(this._buttonManageCategories);
            this._groupBox7.Controls.Add(this._buttonReloadCategoryTree);
            this._groupBox7.Controls.Add(this._buttonAddAllCategories);
            this._groupBox7.Controls.Add(this._buttonClearCategoriesList);
            this._groupBox7.Controls.Add(this._listBoxSelectedCategories);
            this._groupBox7.Controls.Add(this._buttonAddSelectedCategories);
            this._groupBox7.Controls.Add(this._buttonRemoveSelectedCategory);
            this._groupBox7.Location = new System.Drawing.Point(30, 19);
            this._groupBox7.Name = "_groupBox7";
            this._groupBox7.Size = new System.Drawing.Size(647, 434);
            this._groupBox7.TabIndex = 39;
            this._groupBox7.TabStop = false;
            this._groupBox7.Text = "בחירה מרובה של קטגוריות הקשורות לכתבה";
            // 
            // _userControlTreeView1
            // 
            this._userControlTreeView1.IdColumnName = "CatId";
            this._userControlTreeView1.Location = new System.Drawing.Point(299, 19);
            this._userControlTreeView1.LookupTableName = null;
            this._userControlTreeView1.MyQry = "select * FROM Table_LookupCategories WHERE ParentCatId=\'-1\'";
            this._userControlTreeView1.Name = "_userControlTreeView1";
            this._userControlTreeView1.ParentIdColumnName = null;
            this._userControlTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTreeView1.RootNodeId = "1";
            this._userControlTreeView1.RootNodeName = "עמוד ראשי";
            this._userControlTreeView1.Size = new System.Drawing.Size(342, 407);
            this._userControlTreeView1.TabIndex = 38;
            this._userControlTreeView1.TextColumnName = "CatHebrewName";
            // 
            // _buttonManageCategories
            // 
            this._buttonManageCategories.Location = new System.Drawing.Point(195, 61);
            this._buttonManageCategories.Name = "_buttonManageCategories";
            this._buttonManageCategories.Size = new System.Drawing.Size(66, 40);
            this._buttonManageCategories.TabIndex = 37;
            this._buttonManageCategories.Text = "ניהול קטגוריות";
            this._buttonManageCategories.UseVisualStyleBackColor = true;
            this._buttonManageCategories.Click += new System.EventHandler(this.buttonManageCategories_Cilck);
            // 
            // _buttonReloadCategoryTree
            // 
            this._buttonReloadCategoryTree.Location = new System.Drawing.Point(195, 32);
            this._buttonReloadCategoryTree.Name = "_buttonReloadCategoryTree";
            this._buttonReloadCategoryTree.Size = new System.Drawing.Size(66, 23);
            this._buttonReloadCategoryTree.TabIndex = 36;
            this._buttonReloadCategoryTree.Text = "עדכן עץ קטגוריות";
            this._buttonReloadCategoryTree.UseVisualStyleBackColor = true;
            this._buttonReloadCategoryTree.Click += new System.EventHandler(this.buttonReloadCategoryTree_Click);
            // 
            // _buttonAddAllCategories
            // 
            this._buttonAddAllCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonAddAllCategories.Location = new System.Drawing.Point(195, 359);
            this._buttonAddAllCategories.Name = "_buttonAddAllCategories";
            this._buttonAddAllCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonAddAllCategories.Size = new System.Drawing.Size(57, 42);
            this._buttonAddAllCategories.TabIndex = 34;
            this._buttonAddAllCategories.Text = "<< <<";
            this._buttonAddAllCategories.UseVisualStyleBackColor = true;
            this._buttonAddAllCategories.Click += new System.EventHandler(this.buttonAddAllCategories_Click);
            // 
            // _buttonClearCategoriesList
            // 
            this._buttonClearCategoriesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonClearCategoriesList.Location = new System.Drawing.Point(195, 154);
            this._buttonClearCategoriesList.Name = "_buttonClearCategoriesList";
            this._buttonClearCategoriesList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonClearCategoriesList.Size = new System.Drawing.Size(57, 35);
            this._buttonClearCategoriesList.TabIndex = 33;
            this._buttonClearCategoriesList.Text = ">> >>";
            this._buttonClearCategoriesList.UseVisualStyleBackColor = true;
            this._buttonClearCategoriesList.Click += new System.EventHandler(this.buttonClearCategoriesList_Click);
            // 
            // _listBoxSelectedCategories
            // 
            this._listBoxSelectedCategories.FormattingEnabled = true;
            this._listBoxSelectedCategories.Location = new System.Drawing.Point(6, 19);
            this._listBoxSelectedCategories.Name = "_listBoxSelectedCategories";
            this._listBoxSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._listBoxSelectedCategories.ScrollAlwaysVisible = true;
            this._listBoxSelectedCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._listBoxSelectedCategories.Size = new System.Drawing.Size(155, 407);
            this._listBoxSelectedCategories.TabIndex = 30;
            // 
            // _buttonAddSelectedCategories
            // 
            this._buttonAddSelectedCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonAddSelectedCategories.Location = new System.Drawing.Point(195, 304);
            this._buttonAddSelectedCategories.Name = "_buttonAddSelectedCategories";
            this._buttonAddSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonAddSelectedCategories.Size = new System.Drawing.Size(57, 40);
            this._buttonAddSelectedCategories.TabIndex = 32;
            this._buttonAddSelectedCategories.Text = "<< +";
            this._buttonAddSelectedCategories.UseVisualStyleBackColor = true;
            this._buttonAddSelectedCategories.Click += new System.EventHandler(this.buttonAddSelectedCategories_Click);
            // 
            // _buttonRemoveSelectedCategory
            // 
            this._buttonRemoveSelectedCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonRemoveSelectedCategory.Location = new System.Drawing.Point(195, 204);
            this._buttonRemoveSelectedCategory.Name = "_buttonRemoveSelectedCategory";
            this._buttonRemoveSelectedCategory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonRemoveSelectedCategory.Size = new System.Drawing.Size(57, 38);
            this._buttonRemoveSelectedCategory.TabIndex = 31;
            this._buttonRemoveSelectedCategory.Text = "-- >>";
            this._buttonRemoveSelectedCategory.UseVisualStyleBackColor = true;
            this._buttonRemoveSelectedCategory.Click += new System.EventHandler(this.buttonRemoveSelectedCategory_Click);
            // 
            // _tabPagePhotos
            // 
            this._tabPagePhotos.Controls.Add(this._ucUploadPhoto1);
            this._tabPagePhotos.Location = new System.Drawing.Point(4, 22);
            this._tabPagePhotos.Name = "_tabPagePhotos";
            this._tabPagePhotos.Padding = new System.Windows.Forms.Padding(3);
            this._tabPagePhotos.Size = new System.Drawing.Size(708, 459);
            this._tabPagePhotos.TabIndex = 7;
            this._tabPagePhotos.Text = "תמונות";
            this._tabPagePhotos.UseVisualStyleBackColor = true;
            // 
            // _ucUploadPhoto1
            // 
            this._ucUploadPhoto1.Location = new System.Drawing.Point(32, 15);
            this._ucUploadPhoto1.Name = "_ucUploadPhoto1";
            this._ucUploadPhoto1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._ucUploadPhoto1.Size = new System.Drawing.Size(640, 429);
            this._ucUploadPhoto1.TabIndex = 0;
            // 
            // _tabPageVideo
            // 
            this._tabPageVideo.Controls.Add(this._ucUploadVideo1);
            this._tabPageVideo.Location = new System.Drawing.Point(4, 22);
            this._tabPageVideo.Name = "_tabPageVideo";
            this._tabPageVideo.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageVideo.Size = new System.Drawing.Size(708, 459);
            this._tabPageVideo.TabIndex = 8;
            this._tabPageVideo.Text = "ווידאו";
            this._tabPageVideo.UseVisualStyleBackColor = true;
            // 
            // _ucUploadVideo1
            // 
            this._ucUploadVideo1.Location = new System.Drawing.Point(52, 29);
            this._ucUploadVideo1.Name = "_ucUploadVideo1";
            this._ucUploadVideo1.Size = new System.Drawing.Size(601, 398);
            this._ucUploadVideo1.TabIndex = 0;
            this._ucUploadVideo1.Load += new System.EventHandler(this.userControlUploadVideo1_Load);
            // 
            // _tabPageAutoPublish
            // 
            this._tabPageAutoPublish.Location = new System.Drawing.Point(4, 22);
            this._tabPageAutoPublish.Name = "_tabPageAutoPublish";
            this._tabPageAutoPublish.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageAutoPublish.Size = new System.Drawing.Size(708, 459);
            this._tabPageAutoPublish.TabIndex = 9;
            this._tabPageAutoPublish.Text = "שידורים אוטו\'";
            this._tabPageAutoPublish.UseVisualStyleBackColor = true;
            // 
            // _buttonArticlePreview
            // 
            this._buttonArticlePreview.Location = new System.Drawing.Point(159, 531);
            this._buttonArticlePreview.Name = "_buttonArticlePreview";
            this._buttonArticlePreview.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._buttonArticlePreview.Size = new System.Drawing.Size(80, 44);
            this._buttonArticlePreview.TabIndex = 26;
            this._buttonArticlePreview.Text = "הצג בדפדפן";
            this._buttonArticlePreview.UseVisualStyleBackColor = true;
            this._buttonArticlePreview.Click += new System.EventHandler(this.buttonArticlePreview_Click);
            // 
            // _contextMenuStripTreeNode
            // 
            this._contextMenuStripTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripMenuItemAddCategory,
            this._toolStripMenuItemDeleteCategory,
            this._toolStripMenuItemUpdateCategory});
            this._contextMenuStripTreeNode.Name = "contextMenuStripTreeNode";
            this._contextMenuStripTreeNode.Size = new System.Drawing.Size(147, 70);
            // 
            // _toolStripMenuItemAddCategory
            // 
            this._toolStripMenuItemAddCategory.Name = "_toolStripMenuItemAddCategory";
            this._toolStripMenuItemAddCategory.Size = new System.Drawing.Size(146, 22);
            this._toolStripMenuItemAddCategory.Text = "הוסף קטגוריה";
            this._toolStripMenuItemAddCategory.Click += new System.EventHandler(this.ToolStripMenuItemAddCategory_Click);
            // 
            // _toolStripMenuItemDeleteCategory
            // 
            this._toolStripMenuItemDeleteCategory.Name = "_toolStripMenuItemDeleteCategory";
            this._toolStripMenuItemDeleteCategory.Size = new System.Drawing.Size(146, 22);
            this._toolStripMenuItemDeleteCategory.Text = "מחק קטגוריה";
            this._toolStripMenuItemDeleteCategory.Click += new System.EventHandler(this.ToolStripMenuItemDeleteCategory_Click);
            // 
            // _toolStripMenuItemUpdateCategory
            // 
            this._toolStripMenuItemUpdateCategory.Name = "_toolStripMenuItemUpdateCategory";
            this._toolStripMenuItemUpdateCategory.Size = new System.Drawing.Size(146, 22);
            this._toolStripMenuItemUpdateCategory.Text = "עדכן טקסט";
            // 
            // _tableLookupReportersBindingSource
            // 
            this._tableLookupReportersBindingSource.DataMember = "Table_LookupReporters";
            this._tableLookupReportersBindingSource.DataSource = this._kanNaimDataSetReporters;
            // 
            // _kanNaimDataSetReporters
            // 
            this._kanNaimDataSetReporters.DataSetName = "_Kan_NaimDataSetReporters";
            this._kanNaimDataSetReporters.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _buttonSaveArticle
            // 
            this._buttonSaveArticle.Location = new System.Drawing.Point(310, 531);
            this._buttonSaveArticle.Name = "_buttonSaveArticle";
            this._buttonSaveArticle.Size = new System.Drawing.Size(132, 44);
            this._buttonSaveArticle.TabIndex = 7;
            this._buttonSaveArticle.Text = "שמור כתבה";
            this._buttonSaveArticle.UseVisualStyleBackColor = true;
            this._buttonSaveArticle.Click += new System.EventHandler(this.buttonSaveArticle_Click);
            // 
            // _kanNaimDataSet
            // 
            this._kanNaimDataSet.DataSetName = "_Kan_NaimDataSet";
            this._kanNaimDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _tableLookupArticleStatusBindingSource
            // 
            this._tableLookupArticleStatusBindingSource.DataMember = "Table_LookupArticleStatus";
            this._tableLookupArticleStatusBindingSource.DataSource = this._kanNaimDataSet;
            // 
            // _tableLookupArticleStatusTableAdapter
            // 
            this._tableLookupArticleStatusTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupReportersTableAdapter
            // 
            this._tableLookupReportersTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupCategoriesTableAdapter
            // 
            this._tableLookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupReportersTableAdapter1
            // 
            this._tableLookupReportersTableAdapter1.ClearBeforeFill = true;
            // 
            // _tablePhotosArchiveTableAdapter
            // 
            this._tablePhotosArchiveTableAdapter.ClearBeforeFill = true;
            // 
            // _kanNaimDataSet2
            // 
            this._kanNaimDataSet2.DataSetName = "_Kan_NaimDataSet2";
            this._kanNaimDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _spGetAllPhotosByOriginIdBindingSource
            // 
            this._spGetAllPhotosByOriginIdBindingSource.DataMember = "sp_GetAllPhotosByOriginId";
            this._spGetAllPhotosByOriginIdBindingSource.DataSource = this._kanNaimDataSet2;
            // 
            // _spGetAllPhotosByOriginIdTableAdapter
            // 
            this._spGetAllPhotosByOriginIdTableAdapter.ClearBeforeFill = true;
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this._radioButtonSaveAsPrivate);
            this._groupBox1.Controls.Add(this._radioButtonSaveAsPublic);
            this._groupBox1.Enabled = false;
            this._groupBox1.Location = new System.Drawing.Point(457, 525);
            this._groupBox1.Name = "_groupBox1";
            this._groupBox1.Size = new System.Drawing.Size(249, 50);
            this._groupBox1.TabIndex = 8;
            this._groupBox1.TabStop = false;
            this._groupBox1.Text = "בחר שמירה לארכיון ציבורי או פרטי לעריכה";
            this._groupBox1.Visible = false;
            // 
            // _radioButtonSaveAsPrivate
            // 
            this._radioButtonSaveAsPrivate.AutoSize = true;
            this._radioButtonSaveAsPrivate.Location = new System.Drawing.Point(53, 19);
            this._radioButtonSaveAsPrivate.Name = "_radioButtonSaveAsPrivate";
            this._radioButtonSaveAsPrivate.Size = new System.Drawing.Size(50, 17);
            this._radioButtonSaveAsPrivate.TabIndex = 1;
            this._radioButtonSaveAsPrivate.Text = "פרטי";
            this._radioButtonSaveAsPrivate.UseVisualStyleBackColor = true;
            // 
            // _radioButtonSaveAsPublic
            // 
            this._radioButtonSaveAsPublic.AutoSize = true;
            this._radioButtonSaveAsPublic.Checked = true;
            this._radioButtonSaveAsPublic.Location = new System.Drawing.Point(133, 19);
            this._radioButtonSaveAsPublic.Name = "_radioButtonSaveAsPublic";
            this._radioButtonSaveAsPublic.Size = new System.Drawing.Size(60, 17);
            this._radioButtonSaveAsPublic.TabIndex = 0;
            this._radioButtonSaveAsPublic.TabStop = true;
            this._radioButtonSaveAsPublic.Text = "ציבורי";
            this._radioButtonSaveAsPublic.UseVisualStyleBackColor = true;
            // 
            // FormEditArtical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(781, 604);
            this.Controls.Add(this._groupBox1);
            this.Controls.Add(this._buttonSaveArticle);
            this.Controls.Add(this._tabControl1);
            this.Controls.Add(this._buttonArticlePreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormEditArtical";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "עריכת כתבה";
            this.Load += new System.EventHandler(this.FormEditArtical_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditArtical_FormClosing);
            this._tabControl1.ResumeLayout(false);
            this._tabPageArticle.ResumeLayout(false);
            this._groupBox8.ResumeLayout(false);
            this._groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tablePhotosArchiveBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReportersNames)).EndInit();
            this._tabPageTak3X.ResumeLayout(false);
            this._tabPageTak3X.PerformLayout();
            this._tabPageTak2X.ResumeLayout(false);
            this._tabPageTak2X.PerformLayout();
            this._tabPageTak1X.ResumeLayout(false);
            this._tabPageTak1X.PerformLayout();
            this._tabPageTakMedium.ResumeLayout(false);
            this._tabPageTakMedium.PerformLayout();
            this._tabPageTakSmall.ResumeLayout(false);
            this._tabPageTakSmall.PerformLayout();
            this._tabPageCategories.ResumeLayout(false);
            this._groupBox7.ResumeLayout(false);
            this._tabPagePhotos.ResumeLayout(false);
            this._tabPageVideo.ResumeLayout(false);
            this._contextMenuStripTreeNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReporters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupArticleStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spGetAllPhotosByOriginIdBindingSource)).EndInit();
            this._groupBox1.ResumeLayout(false);
            this._groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tabControl1;
        private System.Windows.Forms.TabPage _tabPageArticle;
        private System.Windows.Forms.GroupBox _groupBox8;
        private System.Windows.Forms.ComboBox _comboBoxArticlePhoto;
        private System.Windows.Forms.ComboBox _comboBoxArticleVideo;
        private System.Windows.Forms.DateTimePicker _dateTimePicker22;
        private System.Windows.Forms.Button _buttonArticlePreview;
        private System.Windows.Forms.Label _labelArtical;
        private System.Windows.Forms.TextBox _textBoxKeyWords;
        private System.Windows.Forms.Label _labelKeyWords;
        private System.Windows.Forms.TextBox _textBoxTags;
        private System.Windows.Forms.Label _labelTags;
        private System.Windows.Forms.Label _labelEditor;
        private System.Windows.Forms.TextBox _textBoxArticleTitle;
        private System.Windows.Forms.TextBox _textBoxArticleSubtitle;
        private System.Windows.Forms.ComboBox _comboBoxEditor;
        private System.Windows.Forms.DateTimePicker _dateTimePicker21;
        private System.Windows.Forms.CheckBox _checkBoxMivzak;
        private System.Windows.Forms.CheckBox _checkBoxDateTime;
        private System.Windows.Forms.CheckBox _checkBoxRss;
        private System.Windows.Forms.CheckBox _checkBoxPublish;
        private System.Windows.Forms.Label _labelTitle;
        private System.Windows.Forms.Label _labelSubtitle;
        private System.Windows.Forms.TabPage _tabPageTak3X;
        private System.Windows.Forms.TabPage _tabPageTak2X;
        private System.Windows.Forms.TabPage _tabPageTak1X;
        private System.Windows.Forms.TabPage _tabPageTakMedium;
        private System.Windows.Forms.TabPage _tabPageTakSmall;
        private System.Windows.Forms.TabPage _tabPageCategories;
        private System.Windows.Forms.GroupBox _groupBox7;
        private System.Windows.Forms.Button _buttonAddAllCategories;
        private System.Windows.Forms.Button _buttonClearCategoriesList;
        private System.Windows.Forms.ListBox _listBoxSelectedCategories;
        private System.Windows.Forms.Button _buttonAddSelectedCategories;
        private System.Windows.Forms.Button _buttonRemoveSelectedCategory;
        private System.Windows.Forms.TabPage _tabPagePhotos;
        private System.Windows.Forms.TabPage _tabPageVideo;
        private System.Windows.Forms.TabPage _tabPageAutoPublish;
        private System.Windows.Forms.Button _buttonSaveArticle;
        private _Kan_NaimDataSet _kanNaimDataSet;
        private System.Windows.Forms.BindingSource _tableLookupArticleStatusBindingSource;
        private _Kan_NaimDataSetTableAdapters.Table_LookupArticleStatusTableAdapter _tableLookupArticleStatusTableAdapter;
        private _Kan_NaimDataSetReporters _kanNaimDataSetReporters;
        private System.Windows.Forms.BindingSource _tableLookupReportersBindingSource;
        private _Kan_NaimDataSetReportersTableAdapters.Table_LookupReportersTableAdapter _tableLookupReportersTableAdapter;
        private System.Windows.Forms.Button _buttonSearchVideosArchive;
        private System.Windows.Forms.Button _buttonSearchPhotosArchive;
        private System.Windows.Forms.ComboBox _comboBoxArticleCategory;
        private System.Windows.Forms.Label _label22;
        private _Kan_NaimDataSetCategories _kanNaimDataSetCategories;
        private System.Windows.Forms.BindingSource _tableLookupCategoriesBindingSource;
        private _Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter _tableLookupCategoriesTableAdapter;
        private _Kan_NaimDataSetReportersNames _kanNaimDataSetReportersNames;
        private System.Windows.Forms.BindingSource _tableLookupReportersBindingSource1;
        private _Kan_NaimDataSetReportersNamesTableAdapters.Table_LookupReportersTableAdapter _tableLookupReportersTableAdapter1;
        private _Kan_NaimDataSet1 _kanNaimDataSet1;
        private System.Windows.Forms.BindingSource _tablePhotosArchiveBindingSource;
        private _Kan_NaimDataSet1TableAdapters.Table_PhotosArchiveTableAdapter _tablePhotosArchiveTableAdapter;
        private System.Windows.Forms.Label _labelOriginPhotoId;
        private System.Windows.Forms.BindingSource _spGetAllPhotosByOriginIdBindingSource;
        private _Kan_NaimDataSet2 _kanNaimDataSet2;
        private _Kan_NaimDataSet2TableAdapters.sp_GetAllPhotosByOriginIdTableAdapter _spGetAllPhotosByOriginIdTableAdapter;
        private System.Windows.Forms.Button _buttonReloadCategoryTree;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStripTreeNode;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItemAddCategory;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItemDeleteCategory;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItemUpdateCategory;
        private System.Windows.Forms.Button _buttonManageCategories;
        private System.Windows.Forms.GroupBox _groupBox1;
        private System.Windows.Forms.RadioButton _radioButtonSaveAsPrivate;
        private System.Windows.Forms.RadioButton _radioButtonSaveAsPublic;
        private UserControlTreeView _userControlTreeView1;
        private UserControlTakFill _userControlTakFillSizeX3;
        private UserControlTakFill _userControlTakFillSizeX2;
        private UserControlTakFill _userControlTakFillSizeX1;
        private UserControlTakFill _userControlTakFillSizeMedium;
        private UserControlTakFill _userControlTakFillSizeSmall;
        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.Label _label5;
        private System.Windows.Forms.Label _label4;
        private System.Windows.Forms.Label _label3;
        private System.Windows.Forms.Label _label2;
        private System.Windows.Forms.Button _buttonOpenEditor;
        private System.Windows.Forms.Button _buttonTitleH1;
        private System.Windows.Forms.Button _buttonSubTitleH2;
        private System.Windows.Forms.ComboBox _comboBoxVideoPos;
        private System.Windows.Forms.ComboBox _comboBoxImgPos;
        private System.Windows.Forms.RichTextBox _richTextBoxArticleContent;
        private UserControlUploadPhoto _ucUploadPhoto1;
        private UserControlUploadVideo _ucUploadVideo1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button buttonCopySubtitelToTak;
        private System.Windows.Forms.Button buttonCopyTitleToTak;
    }
}
/*
using HaimDLL;

namespace Kan_Naim_Main
{
    partial class FormEditArtical
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer _components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
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
            this._tabControl1 = new System.Windows.Forms.TabControl();
            this._tabPageArticle = new System.Windows.Forms.TabPage();
            this._groupBox8 = new System.Windows.Forms.GroupBox();
            this._richTextBoxArticleContent = new System.Windows.Forms.RichTextBox();
            this._comboBoxVideoPos = new System.Windows.Forms.ComboBox();
            this._comboBoxImgPos = new System.Windows.Forms.ComboBox();
            this._buttonSubTitleH2 = new System.Windows.Forms.Button();
            this._buttonTitleH1 = new System.Windows.Forms.Button();
            this._buttonOpenEditor = new System.Windows.Forms.Button();
            this._labelOriginPhotoId = new System.Windows.Forms.Label();
            this._comboBoxArticleCategory = new System.Windows.Forms.ComboBox();
            this._tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSetCategories = new Kan_Naim_Main._Kan_NaimDataSetCategories();
            this._label22 = new System.Windows.Forms.Label();
            this._buttonSearchVideosArchive = new System.Windows.Forms.Button();
            this._buttonSearchPhotosArchive = new System.Windows.Forms.Button();
            this._comboBoxArticlePhoto = new System.Windows.Forms.ComboBox();
            this._tablePhotosArchiveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSet1 = new Kan_Naim_Main._Kan_NaimDataSet1();
            this._comboBoxArticleVideo = new System.Windows.Forms.ComboBox();
            this._dateTimePicker22 = new System.Windows.Forms.DateTimePicker();
            this._labelArtical = new System.Windows.Forms.Label();
            this._textBoxKeyWords = new System.Windows.Forms.TextBox();
            this._labelKeyWords = new System.Windows.Forms.Label();
            this._textBoxTags = new System.Windows.Forms.TextBox();
            this._labelTags = new System.Windows.Forms.Label();
            this._labelEditor = new System.Windows.Forms.Label();
            this._textBoxArticleTitle = new System.Windows.Forms.TextBox();
            this._comboBoxEditor = new System.Windows.Forms.ComboBox();
            this._tableLookupReportersBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSetReportersNames = new Kan_Naim_Main._Kan_NaimDataSetReportersNames();
            this._dateTimePicker21 = new System.Windows.Forms.DateTimePicker();
            this._checkBoxMivzak = new System.Windows.Forms.CheckBox();
            this._checkBoxDateTime = new System.Windows.Forms.CheckBox();
            this._checkBoxRss = new System.Windows.Forms.CheckBox();
            this._checkBoxPublish = new System.Windows.Forms.CheckBox();
            this._labelTitle = new System.Windows.Forms.Label();
            this._labelSubtitle = new System.Windows.Forms.Label();
            this._textBoxArticleSubtitle = new System.Windows.Forms.TextBox();
            this._tabPageTak3X = new System.Windows.Forms.TabPage();
            this._label5 = new System.Windows.Forms.Label();
            this._tabPageTak2X = new System.Windows.Forms.TabPage();
            this._label4 = new System.Windows.Forms.Label();
            this._tabPageTak1X = new System.Windows.Forms.TabPage();
            this._label3 = new System.Windows.Forms.Label();
            this._tabPageTakMedium = new System.Windows.Forms.TabPage();
            this._label2 = new System.Windows.Forms.Label();
            this._tabPageTakSmall = new System.Windows.Forms.TabPage();
            this._label1 = new System.Windows.Forms.Label();
            this._tabPageCategories = new System.Windows.Forms.TabPage();
            this._groupBox7 = new System.Windows.Forms.GroupBox();
            this._userControlTreeView1 = new HaimDLL.UserControlTreeView();
            this._userControlTakFillSizeX3 = new UserControlTakFill(1);
            this._userControlTakFillSizeX2 = new UserControlTakFill(2);
            this._userControlTakFillSizeX1 = new UserControlTakFill(3);
            this._userControlTakFillSizeMedium = new UserControlTakFill(5);
            this._userControlTakFillSizeSmall = new UserControlTakFill(6);
            this._buttonManageCategories = new System.Windows.Forms.Button();
            this._buttonReloadCategoryTree = new System.Windows.Forms.Button();
            this._buttonAddAllCategories = new System.Windows.Forms.Button();
            this._buttonClearCategoriesList = new System.Windows.Forms.Button();
            this._listBoxSelectedCategories = new System.Windows.Forms.ListBox();
            this._buttonAddSelectedCategories = new System.Windows.Forms.Button();
            this._buttonRemoveSelectedCategory = new System.Windows.Forms.Button();
            this._tabPagePhotos = new System.Windows.Forms.TabPage();
            this._ucUploadPhoto1 = new HaimDLL.UserControlUploadPhoto();
            this._tabPageVideo = new System.Windows.Forms.TabPage();
            this._ucUploadVideo1 = new HaimDLL.UserControlUploadVideo();
            this._tabPageAutoPublish = new System.Windows.Forms.TabPage();
            this._buttonArticlePreview = new System.Windows.Forms.Button();
            this._contextMenuStripTreeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._toolStripMenuItemAddCategory = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripMenuItemDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripMenuItemUpdateCategory = new System.Windows.Forms.ToolStripMenuItem();
            this._tableLookupReportersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._kanNaimDataSetReporters = new Kan_Naim_Main._Kan_NaimDataSetReporters();
            this._buttonSaveArticle = new System.Windows.Forms.Button();
            this._kanNaimDataSet = new Kan_Naim_Main._Kan_NaimDataSet();
            this._tableLookupArticleStatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._tableLookupArticleStatusTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetTableAdapters.Table_LookupArticleStatusTableAdapter();
            this._tableLookupReportersTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetReportersTableAdapters.Table_LookupReportersTableAdapter();
            this._tableLookupCategoriesTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            this._tableLookupReportersTableAdapter1 = new Kan_Naim_Main._Kan_NaimDataSetReportersNamesTableAdapters.Table_LookupReportersTableAdapter();
            this._tablePhotosArchiveTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet1TableAdapters.Table_PhotosArchiveTableAdapter();
            this._kanNaimDataSet2 = new Kan_Naim_Main._Kan_NaimDataSet2();
            this._spGetAllPhotosByOriginIdBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._spGetAllPhotosByOriginIdTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet2TableAdapters.sp_GetAllPhotosByOriginIdTableAdapter();
            this._groupBox1 = new System.Windows.Forms.GroupBox();
            this._radioButtonSaveAsPrivate = new System.Windows.Forms.RadioButton();
            this._radioButtonSaveAsPublic = new System.Windows.Forms.RadioButton();
            this._tabControl1.SuspendLayout();
            this._tabPageArticle.SuspendLayout();
            this._groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tablePhotosArchiveBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReportersNames)).BeginInit();
            this._tabPageTak3X.SuspendLayout();
            this._tabPageTak2X.SuspendLayout();
            this._tabPageTak1X.SuspendLayout();
            this._tabPageTakMedium.SuspendLayout();
            this._tabPageTakSmall.SuspendLayout();
            this._tabPageCategories.SuspendLayout();
            this._groupBox7.SuspendLayout();
            this._tabPagePhotos.SuspendLayout();
            this._tabPageVideo.SuspendLayout();
            this._contextMenuStripTreeNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReporters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupArticleStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spGetAllPhotosByOriginIdBindingSource)).BeginInit();
            this._groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl1
            // 
            this._tabControl1.Controls.Add(this._tabPageArticle);
            this._tabControl1.Controls.Add(this._tabPageTak3X);
            this._tabControl1.Controls.Add(this._tabPageTak1X);
            this._tabControl1.Controls.Add(this._tabPageTakMedium);
            this._tabControl1.Controls.Add(this._tabPageTakSmall);
            this._tabControl1.Controls.Add(this._tabPageCategories);
            this._tabControl1.Controls.Add(this._tabPagePhotos);
            this._tabControl1.Controls.Add(this._tabPageVideo);
            this._tabControl1.Controls.Add(this._tabPageAutoPublish);
            this._tabControl1.Controls.Add(this._tabPageTak2X);
            this._tabControl1.HotTrack = true;
            this._tabControl1.Location = new System.Drawing.Point(12, 27);
            this._tabControl1.Name = "_tabControl1";
            this._tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._tabControl1.RightToLeftLayout = true;
            this._tabControl1.SelectedIndex = 0;
            this._tabControl1.Size = new System.Drawing.Size(713, 485);
            this._tabControl1.TabIndex = 4;
            // 
            // _tabPageArticle
            // 
            this._tabPageArticle.Controls.Add(this._groupBox8);
            this._tabPageArticle.Location = new System.Drawing.Point(4, 22);
            this._tabPageArticle.Name = "_tabPageArticle";
            this._tabPageArticle.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageArticle.Size = new System.Drawing.Size(705, 459);
            this._tabPageArticle.TabIndex = 0;
            this._tabPageArticle.Text = "כתבה";
            this._tabPageArticle.UseVisualStyleBackColor = true;
            // 
            // _groupBox8
            // 
            this._groupBox8.Controls.Add(this._richTextBoxArticleContent);
            this._groupBox8.Controls.Add(this._comboBoxVideoPos);
            this._groupBox8.Controls.Add(this._comboBoxImgPos);
            this._groupBox8.Controls.Add(this._buttonSubTitleH2);
            this._groupBox8.Controls.Add(this._buttonTitleH1);
            this._groupBox8.Controls.Add(this._buttonOpenEditor);
            this._groupBox8.Controls.Add(this._labelOriginPhotoId);
            this._groupBox8.Controls.Add(this._comboBoxArticleCategory);
            this._groupBox8.Controls.Add(this._label22);
            this._groupBox8.Controls.Add(this._buttonSearchVideosArchive);
            this._groupBox8.Controls.Add(this._buttonSearchPhotosArchive);
            this._groupBox8.Controls.Add(this._comboBoxArticlePhoto);
            this._groupBox8.Controls.Add(this._comboBoxArticleVideo);
            this._groupBox8.Controls.Add(this._dateTimePicker22);
            this._groupBox8.Controls.Add(this._labelArtical);
            this._groupBox8.Controls.Add(this._textBoxKeyWords);
            this._groupBox8.Controls.Add(this._labelKeyWords);
            this._groupBox8.Controls.Add(this._textBoxTags);
            this._groupBox8.Controls.Add(this._labelTags);
            this._groupBox8.Controls.Add(this._labelEditor);
            this._groupBox8.Controls.Add(this._textBoxArticleTitle);
            this._groupBox8.Controls.Add(this._comboBoxEditor);
            this._groupBox8.Controls.Add(this._dateTimePicker21);
            this._groupBox8.Controls.Add(this._checkBoxMivzak);
            this._groupBox8.Controls.Add(this._checkBoxDateTime);
            this._groupBox8.Controls.Add(this._checkBoxRss);
            this._groupBox8.Controls.Add(this._checkBoxPublish);
            this._groupBox8.Controls.Add(this._labelTitle);
            this._groupBox8.Controls.Add(this._labelSubtitle);
            this._groupBox8.Controls.Add(this._textBoxArticleSubtitle);
            this._groupBox8.Location = new System.Drawing.Point(15, 6);
            this._groupBox8.Name = "_groupBox8";
            this._groupBox8.Size = new System.Drawing.Size(675, 447);
            this._groupBox8.TabIndex = 4;
            this._groupBox8.TabStop = false;
            this._groupBox8.Text = "הזנת תוכן ומאפייני הכתבה";
            // 
            // _richTextBoxArticleContent
            // 
            this._richTextBoxArticleContent.Enabled = false;
            this._richTextBoxArticleContent.Location = new System.Drawing.Point(19, 264);
            this._richTextBoxArticleContent.MaxLength = 10000;
            this._richTextBoxArticleContent.Name = "_richTextBoxArticleContent";
            this._richTextBoxArticleContent.Size = new System.Drawing.Size(541, 165);
            this._richTextBoxArticleContent.TabIndex = 85;
            this._richTextBoxArticleContent.Text = "";
            // 
            // _comboBoxVideoPos
            // 
            this._comboBoxVideoPos.Enabled = false;
            this._comboBoxVideoPos.FormattingEnabled = true;
            this._comboBoxVideoPos.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "110",
            "120",
            "130",
            "140",
            "150",
            "160",
            "170",
            "180",
            "190",
            "200"});
            this._comboBoxVideoPos.Location = new System.Drawing.Point(19, 171);
            this._comboBoxVideoPos.Name = "_comboBoxVideoPos";
            this._comboBoxVideoPos.Size = new System.Drawing.Size(70, 21);
            this._comboBoxVideoPos.TabIndex = 84;
            this._comboBoxVideoPos.Text = "  מלמעלה";
            this._comboBoxVideoPos.Visible = false;
            // 
            // _comboBoxImgPos
            // 
            this._comboBoxImgPos.Enabled = false;
            this._comboBoxImgPos.FormattingEnabled = true;
            this._comboBoxImgPos.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "110",
            "120",
            "130",
            "140",
            "150",
            "160",
            "170",
            "180",
            "190",
            "200"});
            this._comboBoxImgPos.Location = new System.Drawing.Point(19, 144);
            this._comboBoxImgPos.Name = "_comboBoxImgPos";
            this._comboBoxImgPos.Size = new System.Drawing.Size(70, 21);
            this._comboBoxImgPos.TabIndex = 83;
            this._comboBoxImgPos.Text = "  מלמעלה";
            this._comboBoxImgPos.Visible = false;
            // 
            // _buttonSubTitleH2
            // 
            this._buttonSubTitleH2.Location = new System.Drawing.Point(573, 344);
            this._buttonSubTitleH2.Name = "_buttonSubTitleH2";
            this._buttonSubTitleH2.Size = new System.Drawing.Size(59, 23);
            this._buttonSubTitleH2.TabIndex = 79;
            this._buttonSubTitleH2.Text = "H2";
            this._buttonSubTitleH2.UseVisualStyleBackColor = true;
            this._buttonSubTitleH2.Click += new System.EventHandler(buttonTitlesH1andH2_Click);
            // 
            // _buttonTitleH1
            // 
            this._buttonTitleH1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonTitleH1.Location = new System.Drawing.Point(573, 315);
            this._buttonTitleH1.Name = "_buttonTitleH1";
            this._buttonTitleH1.Size = new System.Drawing.Size(59, 23);
            this._buttonTitleH1.TabIndex = 78;
            this._buttonTitleH1.Text = "H1";
            this._buttonTitleH1.UseVisualStyleBackColor = true;
            this._buttonTitleH1.Click += new System.EventHandler(buttonTitlesH1andH2_Click);
            // 
            // _buttonOpenEditor
            // 
            this._buttonOpenEditor.Location = new System.Drawing.Point(573, 286);
            this._buttonOpenEditor.Name = "_buttonOpenEditor";
            this._buttonOpenEditor.Size = new System.Drawing.Size(59, 23);
            this._buttonOpenEditor.TabIndex = 76;
            this._buttonOpenEditor.Text = "עריכה";
            this._buttonOpenEditor.UseVisualStyleBackColor = true;
            _buttonOpenEditor.Click += new System.EventHandler(buttonOpenEditor_Click);
            // 
            // _labelOriginPhotoId
            // 
            this._labelOriginPhotoId.AutoSize = true;
            this._labelOriginPhotoId.Location = new System.Drawing.Point(642, 279);
            this._labelOriginPhotoId.Name = "_labelOriginPhotoId";
            this._labelOriginPhotoId.Size = new System.Drawing.Size(13, 13);
            this._labelOriginPhotoId.TabIndex = 62;
            this._labelOriginPhotoId.Text = "0";
            this._labelOriginPhotoId.Visible = false;
            // 
            // _comboBoxArticleCategory
            // 
            this._comboBoxArticleCategory.DataSource = this._tableLookupCategoriesBindingSource;
            this._comboBoxArticleCategory.DisplayMember = "CatHebrewName";
            this._comboBoxArticleCategory.FormattingEnabled = true;
            this._comboBoxArticleCategory.Location = new System.Drawing.Point(19, 117);
            this._comboBoxArticleCategory.Name = "_comboBoxArticleCategory";
            this._comboBoxArticleCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._comboBoxArticleCategory.Size = new System.Drawing.Size(193, 21);
            this._comboBoxArticleCategory.TabIndex = 61;
            this._comboBoxArticleCategory.ValueMember = "CatId";
            // 
            // _tableLookupCategoriesBindingSource
            // 
            this._tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            this._tableLookupCategoriesBindingSource.DataSource = this._kanNaimDataSetCategories;
            // 
            // _kanNaimDataSetCategories
            // 
            this._kanNaimDataSetCategories.DataSetName = "_Kan_NaimDataSetCategories";
            this._kanNaimDataSetCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _label22
            // 
            this._label22.AutoSize = true;
            this._label22.Location = new System.Drawing.Point(218, 121);
            this._label22.Name = "_label22";
            this._label22.Size = new System.Drawing.Size(50, 13);
            this._label22.TabIndex = 60;
            this._label22.Text = "קטגוריה";
            // 
            // _buttonSearchVideosArchive
            // 
            this._buttonSearchVideosArchive.Enabled = false;
            this._buttonSearchVideosArchive.Location = new System.Drawing.Point(577, 169);
            this._buttonSearchVideosArchive.Name = "_buttonSearchVideosArchive";
            this._buttonSearchVideosArchive.Size = new System.Drawing.Size(75, 23);
            this._buttonSearchVideosArchive.TabIndex = 59;
            this._buttonSearchVideosArchive.Text = "וידאו..";
            this._buttonSearchVideosArchive.UseVisualStyleBackColor = true;
            // 
            // _buttonSearchPhotosArchive
            // 
            this._buttonSearchPhotosArchive.Enabled = false;
            this._buttonSearchPhotosArchive.Location = new System.Drawing.Point(577, 142);
            this._buttonSearchPhotosArchive.Name = "_buttonSearchPhotosArchive";
            this._buttonSearchPhotosArchive.Size = new System.Drawing.Size(75, 23);
            this._buttonSearchPhotosArchive.TabIndex = 58;
            this._buttonSearchPhotosArchive.Text = "תמונה..";
            this._buttonSearchPhotosArchive.UseVisualStyleBackColor = true;
            // 
            // _comboBoxArticlePhoto
            // 
            this._comboBoxArticlePhoto.DataSource = this._tablePhotosArchiveBindingSource;
            this._comboBoxArticlePhoto.DisplayMember = "ImageUrl";
            this._comboBoxArticlePhoto.FormattingEnabled = true;
            this._comboBoxArticlePhoto.Location = new System.Drawing.Point(95, 144);
            this._comboBoxArticlePhoto.Name = "_comboBoxArticlePhoto";
            this._comboBoxArticlePhoto.Size = new System.Drawing.Size(465, 21);
            this._comboBoxArticlePhoto.TabIndex = 57;
            this._comboBoxArticlePhoto.ValueMember = "Id";
            // 
            // _tablePhotosArchiveBindingSource
            // 
            this._tablePhotosArchiveBindingSource.DataMember = "Table_PhotosArchive";
            this._tablePhotosArchiveBindingSource.DataSource = this._kanNaimDataSet1;
            // 
            // _kanNaimDataSet1
            // 
            this._kanNaimDataSet1.DataSetName = "_Kan_NaimDataSet1";
            this._kanNaimDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _comboBoxArticleVideo
            // 
            this._comboBoxArticleVideo.Enabled = false;
            this._comboBoxArticleVideo.FormattingEnabled = true;
            this._comboBoxArticleVideo.Location = new System.Drawing.Point(95, 171);
            this._comboBoxArticleVideo.Name = "_comboBoxArticleVideo";
            this._comboBoxArticleVideo.Size = new System.Drawing.Size(465, 21);
            this._comboBoxArticleVideo.TabIndex = 56;
            // 
            // _dateTimePicker22
            // 
            this._dateTimePicker22.Enabled = false;
            this._dateTimePicker22.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._dateTimePicker22.Location = new System.Drawing.Point(19, 91);
            this._dateTimePicker22.Name = "_dateTimePicker22";
            this._dateTimePicker22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._dateTimePicker22.RightToLeftLayout = true;
            this._dateTimePicker22.ShowUpDown = true;
            this._dateTimePicker22.Size = new System.Drawing.Size(90, 20);
            this._dateTimePicker22.TabIndex = 34;
            // 
            // _labelArtical
            // 
            this._labelArtical.AutoSize = true;
            this._labelArtical.Location = new System.Drawing.Point(571, 264);
            this._labelArtical.Name = "_labelArtical";
            this._labelArtical.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._labelArtical.Size = new System.Drawing.Size(62, 13);
            this._labelArtical.TabIndex = 24;
            this._labelArtical.Text = "תוכן כתבה";
            // 
            // _textBoxKeyWords
            // 
            this._textBoxKeyWords.Location = new System.Drawing.Point(19, 225);
            this._textBoxKeyWords.MaxLength = 200;
            this._textBoxKeyWords.Name = "_textBoxKeyWords";
            this._textBoxKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxKeyWords.Size = new System.Drawing.Size(541, 20);
            this._textBoxKeyWords.TabIndex = 23;
            this._textBoxKeyWords.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _labelKeyWords
            // 
            this._labelKeyWords.AutoSize = true;
            this._labelKeyWords.Location = new System.Drawing.Point(577, 228);
            this._labelKeyWords.Name = "_labelKeyWords";
            this._labelKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._labelKeyWords.Size = new System.Drawing.Size(74, 13);
            this._labelKeyWords.TabIndex = 22;
            this._labelKeyWords.Text = "מילות חיפוש";
            // 
            // _textBoxTags
            // 
            this._textBoxTags.Location = new System.Drawing.Point(19, 199);
            this._textBoxTags.MaxLength = 200;
            this._textBoxTags.Name = "_textBoxTags";
            this._textBoxTags.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxTags.Size = new System.Drawing.Size(541, 20);
            this._textBoxTags.TabIndex = 21;
            this._textBoxTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _labelTags
            // 
            this._labelTags.AutoSize = true;
            this._labelTags.Location = new System.Drawing.Point(577, 202);
            this._labelTags.Name = "_labelTags";
            this._labelTags.Size = new System.Drawing.Size(37, 13);
            this._labelTags.TabIndex = 20;
            this._labelTags.Text = "תגיות";
            // 
            // _labelEditor
            // 
            this._labelEditor.AutoSize = true;
            this._labelEditor.Location = new System.Drawing.Point(577, 121);
            this._labelEditor.Name = "_labelEditor";
            this._labelEditor.Size = new System.Drawing.Size(32, 13);
            this._labelEditor.TabIndex = 18;
            this._labelEditor.Text = "עורך";
            // 
            // _textBoxArticleTitle
            // 
            this._textBoxArticleTitle.Location = new System.Drawing.Point(17, 16);
            this._textBoxArticleTitle.MaxLength = 150;
            this._textBoxArticleTitle.Name = "_textBoxArticleTitle";
            this._textBoxArticleTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxArticleTitle.Size = new System.Drawing.Size(541, 20);
            this._textBoxArticleTitle.TabIndex = 15;
            this._textBoxArticleTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _comboBoxEditor
            // 
            this._comboBoxEditor.DataSource = this._tableLookupReportersBindingSource1;
            this._comboBoxEditor.DisplayMember = "PublishNameShort";
            this._comboBoxEditor.FormattingEnabled = true;
            this._comboBoxEditor.Location = new System.Drawing.Point(291, 118);
            this._comboBoxEditor.Name = "_comboBoxEditor";
            this._comboBoxEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._comboBoxEditor.Size = new System.Drawing.Size(269, 21);
            this._comboBoxEditor.TabIndex = 10;
            this._comboBoxEditor.ValueMember = "UserId";
            // 
            // _tableLookupReportersBindingSource1
            // 
            this._tableLookupReportersBindingSource1.DataMember = "Table_LookupReporters";
            this._tableLookupReportersBindingSource1.DataSource = this._kanNaimDataSetReportersNames;
            // 
            // _kanNaimDataSetReportersNames
            // 
            this._kanNaimDataSetReportersNames.DataSetName = "_Kan_NaimDataSetReportersNames";
            this._kanNaimDataSetReportersNames.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _dateTimePicker21
            // 
            this._dateTimePicker21.Enabled = false;
            this._dateTimePicker21.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dateTimePicker21.Location = new System.Drawing.Point(125, 91);
            this._dateTimePicker21.MaxDate = new System.DateTime(2015, 12, 31, 0, 0, 0, 0);
            this._dateTimePicker21.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this._dateTimePicker21.Name = "_dateTimePicker21";
            this._dateTimePicker21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._dateTimePicker21.RightToLeftLayout = true;
            this._dateTimePicker21.ShowUpDown = true;
            this._dateTimePicker21.Size = new System.Drawing.Size(87, 20);
            this._dateTimePicker21.TabIndex = 9;
            this._dateTimePicker21.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // _checkBoxMivzak
            // 
            this._checkBoxMivzak.AutoSize = true;
            this._checkBoxMivzak.Checked = true;
            this._checkBoxMivzak.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxMivzak.Location = new System.Drawing.Point(428, 94);
            this._checkBoxMivzak.Name = "_checkBoxMivzak";
            this._checkBoxMivzak.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxMivzak.Size = new System.Drawing.Size(52, 17);
            this._checkBoxMivzak.TabIndex = 7;
            this._checkBoxMivzak.Tag = "";
            this._checkBoxMivzak.Text = "מבזק";
            this._checkBoxMivzak.UseVisualStyleBackColor = true;
            // 
            // _checkBoxDateTime
            // 
            this._checkBoxDateTime.AutoSize = true;
            this._checkBoxDateTime.Checked = true;
            this._checkBoxDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxDateTime.Location = new System.Drawing.Point(231, 94);
            this._checkBoxDateTime.Name = "_checkBoxDateTime";
            this._checkBoxDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxDateTime.Size = new System.Drawing.Size(90, 17);
            this._checkBoxDateTime.TabIndex = 6;
            this._checkBoxDateTime.Tag = "";
            this._checkBoxDateTime.Text = "תאריך ושעה";
            this._checkBoxDateTime.UseVisualStyleBackColor = true;
            // 
            // _checkBoxRss
            // 
            this._checkBoxRss.AutoSize = true;
            this._checkBoxRss.Checked = true;
            this._checkBoxRss.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxRss.Location = new System.Drawing.Point(348, 94);
            this._checkBoxRss.Name = "_checkBoxRss";
            this._checkBoxRss.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxRss.Size = new System.Drawing.Size(48, 17);
            this._checkBoxRss.TabIndex = 5;
            this._checkBoxRss.Tag = "";
            this._checkBoxRss.Text = "RSS";
            this._checkBoxRss.UseVisualStyleBackColor = true;
            // 
            // _checkBoxPublish
            // 
            this._checkBoxPublish.AutoSize = true;
            this._checkBoxPublish.Checked = true;
            this._checkBoxPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkBoxPublish.Location = new System.Drawing.Point(505, 95);
            this._checkBoxPublish.Name = "_checkBoxPublish";
            this._checkBoxPublish.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._checkBoxPublish.Size = new System.Drawing.Size(53, 17);
            this._checkBoxPublish.TabIndex = 4;
            this._checkBoxPublish.Tag = "";
            this._checkBoxPublish.Text = "פרסם";
            this._checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // _labelTitle
            // 
            this._labelTitle.AutoSize = true;
            this._labelTitle.Location = new System.Drawing.Point(571, 19);
            this._labelTitle.Name = "_labelTitle";
            this._labelTitle.Size = new System.Drawing.Size(77, 13);
            this._labelTitle.TabIndex = 3;
            this._labelTitle.Text = "כותרת ראשית";
            // 
            // _labelSubtitle
            // 
            this._labelSubtitle.AutoSize = true;
            this._labelSubtitle.Location = new System.Drawing.Point(571, 52);
            this._labelSubtitle.Name = "_labelSubtitle";
            this._labelSubtitle.Size = new System.Drawing.Size(75, 13);
            this._labelSubtitle.TabIndex = 0;
            this._labelSubtitle.Text = "כותרת משנית";
            // 
            // _textBoxArticleSubtitle
            // 
            this._textBoxArticleSubtitle.Location = new System.Drawing.Point(17, 49);
            this._textBoxArticleSubtitle.MaxLength = 300;
            this._textBoxArticleSubtitle.Multiline = true;
            this._textBoxArticleSubtitle.Name = "_textBoxArticleSubtitle";
            this._textBoxArticleSubtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxArticleSubtitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxArticleSubtitle.Size = new System.Drawing.Size(541, 36);
            this._textBoxArticleSubtitle.TabIndex = 14;
            this._textBoxArticleSubtitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _tabPageTak3X
            // 
            this._tabPageTak3X.Controls.Add(this._label5);
            this._tabPageTak3X.Controls.Add(this._userControlTakFillSizeX3);
            this._tabPageTak3X.Location = new System.Drawing.Point(4, 22);
            this._tabPageTak3X.Name = "_tabPageTak3X";
            this._tabPageTak3X.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTak3X.Size = new System.Drawing.Size(705, 459);
            this._tabPageTak3X.TabIndex = 1;
            this._tabPageTak3X.Text = "תקציר גדול X3";
            this._tabPageTak3X.UseVisualStyleBackColor = true;
            // 
            // _label5
            // 
            this._label5.AutoSize = true;
            this._label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label5.Location = new System.Drawing.Point(644, 28);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(55, 13);
            this._label5.TabIndex = 3;
            this._label5.Text = "בגודל X3";
            // 
            // _userControlTakFillSizeX3
            // 
            this._userControlTakFillSizeX3.Location = new System.Drawing.Point(6, 6);
            this._userControlTakFillSizeX3.Name = "_userControlTakFillSizeX3";
            this._userControlTakFillSizeX3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeX3.Size = new System.Drawing.Size(640, 445);
            this._userControlTakFillSizeX3.TabIndex = 0;
            // 
            // _tabPageTak2X
            // 
            this._tabPageTak2X.Controls.Add(this._label4);
            this._tabPageTak2X.Location = new System.Drawing.Point(4, 22);
            this._tabPageTak2X.Name = "_tabPageTak2X";
            this._tabPageTak2X.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTak2X.Size = new System.Drawing.Size(705, 459);
            this._tabPageTak2X.TabIndex = 2;
            this._tabPageTak2X.Text = "תקציר גדול X2";
            this._tabPageTak2X.UseVisualStyleBackColor = true;
            // 
            // _label4
            // 
            this._label4.AutoSize = true;
            this._label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label4.Location = new System.Drawing.Point(647, 25);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(55, 13);
            this._label4.TabIndex = 3;
            this._label4.Text = "בגודל X2";
            // 
            // _tabPageTak1X
            // 
            this._tabPageTak1X.Controls.Add(this._label3);
            this._tabPageTak1X.Location = new System.Drawing.Point(4, 22);
            this._tabPageTak1X.Name = "_tabPageTak1X";
            this._tabPageTak1X.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTak1X.Size = new System.Drawing.Size(705, 459);
            this._tabPageTak1X.TabIndex = 3;
            this._tabPageTak1X.Text = "תקציר גדול X1";
            this._tabPageTak1X.UseVisualStyleBackColor = true;
            // 
            // _label3
            // 
            this._label3.AutoSize = true;
            this._label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label3.Location = new System.Drawing.Point(644, 18);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(55, 13);
            this._label3.TabIndex = 2;
            this._label3.Text = "בגודל X1";
            // 
            // _tabPageTakMedium
            // 
            this._tabPageTakMedium.Controls.Add(this._label2);
            this._tabPageTakMedium.Controls.Add(this._userControlTakFillSizeMedium);
            this._tabPageTakMedium.Location = new System.Drawing.Point(4, 22);
            this._tabPageTakMedium.Name = "_tabPageTakMedium";
            this._tabPageTakMedium.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTakMedium.Size = new System.Drawing.Size(705, 459);
            this._tabPageTakMedium.TabIndex = 4;
            this._tabPageTakMedium.Text = "תקציר בינוני";
            this._tabPageTakMedium.UseVisualStyleBackColor = true;
            // 
            // _label2
            // 
            this._label2.AutoSize = true;
            this._label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label2.Location = new System.Drawing.Point(638, 21);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(74, 13);
            this._label2.TabIndex = 2;
            this._label2.Text = "בגודל בינוני";
            // 
            // _userControlTakFillSizeMedium
            // 
            this._userControlTakFillSizeMedium.Location = new System.Drawing.Point(6, 6);
            this._userControlTakFillSizeMedium.Name = "_userControlTakFillSizeMedium";
            this._userControlTakFillSizeMedium.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeMedium.Size = new System.Drawing.Size(635, 445);
            this._userControlTakFillSizeMedium.TabIndex = 0;
            // 
            // _tabPageTakSmall
            // 
            this._tabPageTakSmall.Controls.Add(this._label1);
            this._tabPageTakSmall.Controls.Add(this._userControlTakFillSizeSmall);
            this._tabPageTakSmall.Location = new System.Drawing.Point(4, 22);
            this._tabPageTakSmall.Name = "_tabPageTakSmall";
            this._tabPageTakSmall.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageTakSmall.Size = new System.Drawing.Size(705, 459);
            this._tabPageTakSmall.TabIndex = 5;
            this._tabPageTakSmall.Text = "תקציר קטן";
            this._tabPageTakSmall.UseVisualStyleBackColor = true;
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this._label1.Location = new System.Drawing.Point(641, 25);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(61, 13);
            this._label1.TabIndex = 1;
            this._label1.Text = "בגודל קטן";
            // 
            // _userControlTakFillSizeSmall
            // 
            this._userControlTakFillSizeSmall.Location = new System.Drawing.Point(6, 8);
            this._userControlTakFillSizeSmall.Name = "_userControlTakFillSizeSmall";
            this._userControlTakFillSizeSmall.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTakFillSizeSmall.Size = new System.Drawing.Size(635, 445);
            this._userControlTakFillSizeSmall.TabIndex = 0;
            // 
            // _tabPageCategories
            // 
            this._tabPageCategories.Controls.Add(this._groupBox7);
            this._tabPageCategories.Location = new System.Drawing.Point(4, 22);
            this._tabPageCategories.Name = "_tabPageCategories";
            this._tabPageCategories.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageCategories.Size = new System.Drawing.Size(705, 459);
            this._tabPageCategories.TabIndex = 6;
            this._tabPageCategories.Text = "קטגוריות";
            this._tabPageCategories.UseVisualStyleBackColor = true;
            // 
            // _groupBox7
            // 
            this._groupBox7.Controls.Add(this._userControlTreeView1);
            this._groupBox7.Controls.Add(this._buttonManageCategories);
            this._groupBox7.Controls.Add(this._buttonReloadCategoryTree);
            this._groupBox7.Controls.Add(this._buttonAddAllCategories);
            this._groupBox7.Controls.Add(this._buttonClearCategoriesList);
            this._groupBox7.Controls.Add(this._listBoxSelectedCategories);
            this._groupBox7.Controls.Add(this._buttonAddSelectedCategories);
            this._groupBox7.Controls.Add(this._buttonRemoveSelectedCategory);
            this._groupBox7.Location = new System.Drawing.Point(30, 19);
            this._groupBox7.Name = "_groupBox7";
            this._groupBox7.Size = new System.Drawing.Size(647, 434);
            this._groupBox7.TabIndex = 39;
            this._groupBox7.TabStop = false;
            this._groupBox7.Text = "בחירה מרובה של קטגוריות הקשורות לכתבה";
            // 
            // _userControlTreeView1
            // 
            this._userControlTreeView1.IdColumnName = "CatId";
            this._userControlTreeView1.Location = new System.Drawing.Point(299, 19);
            this._userControlTreeView1.LookupTableName = null;
            this._userControlTreeView1.MyQry = "select * FROM Table_LookupCategories WHERE ParentCatId=\'-1\'";
            this._userControlTreeView1.Name = "_userControlTreeView1";
            this._userControlTreeView1.ParentIdColumnName = null;
            this._userControlTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._userControlTreeView1.RootNodeId = "1";
            this._userControlTreeView1.RootNodeName = "עמוד ראשי";
            this._userControlTreeView1.Size = new System.Drawing.Size(342, 407);
            this._userControlTreeView1.TabIndex = 38;
            this._userControlTreeView1.TextColumnName = "CatHebrewName";
            // 
            // _buttonManageCategories
            // 
            this._buttonManageCategories.Location = new System.Drawing.Point(195, 61);
            this._buttonManageCategories.Name = "_buttonManageCategories";
            this._buttonManageCategories.Size = new System.Drawing.Size(66, 40);
            this._buttonManageCategories.TabIndex = 37;
            this._buttonManageCategories.Text = "ניהול קטגוריות";
            this._buttonManageCategories.UseVisualStyleBackColor = true;
            // 
            // _buttonReloadCategoryTree
            // 
            this._buttonReloadCategoryTree.Location = new System.Drawing.Point(195, 32);
            this._buttonReloadCategoryTree.Name = "_buttonReloadCategoryTree";
            this._buttonReloadCategoryTree.Size = new System.Drawing.Size(66, 23);
            this._buttonReloadCategoryTree.TabIndex = 36;
            this._buttonReloadCategoryTree.Text = "עדכן עץ קטגוריות";
            this._buttonReloadCategoryTree.UseVisualStyleBackColor = true;
            //this._buttonReloadCategoryTree.Click += new System.EventHandler(this._buttonReloadCategoryTree_Click);
            // 
            // _buttonAddAllCategories
            // 
            this._buttonAddAllCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonAddAllCategories.Location = new System.Drawing.Point(195, 359);
            this._buttonAddAllCategories.Name = "_buttonAddAllCategories";
            this._buttonAddAllCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonAddAllCategories.Size = new System.Drawing.Size(57, 42);
            this._buttonAddAllCategories.TabIndex = 34;
            this._buttonAddAllCategories.Text = "<< <<";
            this._buttonAddAllCategories.UseVisualStyleBackColor = true;
            // 
            // _buttonClearCategoriesList
            // 
            this._buttonClearCategoriesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonClearCategoriesList.Location = new System.Drawing.Point(195, 154);
            this._buttonClearCategoriesList.Name = "_buttonClearCategoriesList";
            this._buttonClearCategoriesList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonClearCategoriesList.Size = new System.Drawing.Size(57, 35);
            this._buttonClearCategoriesList.TabIndex = 33;
            this._buttonClearCategoriesList.Text = ">> >>";
            this._buttonClearCategoriesList.UseVisualStyleBackColor = true;
            // 
            // _listBoxSelectedCategories
            // 
            this._listBoxSelectedCategories.FormattingEnabled = true;
            this._listBoxSelectedCategories.Location = new System.Drawing.Point(6, 19);
            this._listBoxSelectedCategories.Name = "_listBoxSelectedCategories";
            this._listBoxSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._listBoxSelectedCategories.ScrollAlwaysVisible = true;
            this._listBoxSelectedCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._listBoxSelectedCategories.Size = new System.Drawing.Size(155, 407);
            this._listBoxSelectedCategories.TabIndex = 30;
            // 
            // _buttonAddSelectedCategories
            // 
            this._buttonAddSelectedCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonAddSelectedCategories.Location = new System.Drawing.Point(195, 304);
            this._buttonAddSelectedCategories.Name = "_buttonAddSelectedCategories";
            this._buttonAddSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonAddSelectedCategories.Size = new System.Drawing.Size(57, 40);
            this._buttonAddSelectedCategories.TabIndex = 32;
            this._buttonAddSelectedCategories.Text = "<< +";
            this._buttonAddSelectedCategories.UseVisualStyleBackColor = true;
            // 
            // _buttonRemoveSelectedCategory
            // 
            this._buttonRemoveSelectedCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonRemoveSelectedCategory.Location = new System.Drawing.Point(195, 204);
            this._buttonRemoveSelectedCategory.Name = "_buttonRemoveSelectedCategory";
            this._buttonRemoveSelectedCategory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._buttonRemoveSelectedCategory.Size = new System.Drawing.Size(57, 38);
            this._buttonRemoveSelectedCategory.TabIndex = 31;
            this._buttonRemoveSelectedCategory.Text = "-- >>";
            this._buttonRemoveSelectedCategory.UseVisualStyleBackColor = true;
            // 
            // _tabPagePhotos
            // 
            this._tabPagePhotos.Controls.Add(this._ucUploadPhoto1);
            this._tabPagePhotos.Location = new System.Drawing.Point(4, 22);
            this._tabPagePhotos.Name = "_tabPagePhotos";
            this._tabPagePhotos.Padding = new System.Windows.Forms.Padding(3);
            this._tabPagePhotos.Size = new System.Drawing.Size(705, 459);
            this._tabPagePhotos.TabIndex = 7;
            this._tabPagePhotos.Text = "תמונות";
            this._tabPagePhotos.UseVisualStyleBackColor = true;
            // 
            // _ucUploadPhoto1
            // 
            this._ucUploadPhoto1.Location = new System.Drawing.Point(32, 15);
            this._ucUploadPhoto1.Name = "_ucUploadPhoto1";
            this._ucUploadPhoto1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._ucUploadPhoto1.Size = new System.Drawing.Size(640, 429);
            this._ucUploadPhoto1.TabIndex = 0;
            // 
            // _tabPageVideo
            // 
            this._tabPageVideo.Controls.Add(this._ucUploadVideo1);
            this._tabPageVideo.Location = new System.Drawing.Point(4, 22);
            this._tabPageVideo.Name = "_tabPageVideo";
            this._tabPageVideo.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageVideo.Size = new System.Drawing.Size(705, 459);
            this._tabPageVideo.TabIndex = 8;
            this._tabPageVideo.Text = "ווידאו";
            this._tabPageVideo.UseVisualStyleBackColor = true;
            // 
            // _ucUploadVideo1
            // 
            this._ucUploadVideo1.Location = new System.Drawing.Point(52, 29);
            this._ucUploadVideo1.Name = "_ucUploadVideo1";
            this._ucUploadVideo1.Size = new System.Drawing.Size(601, 398);
            this._ucUploadVideo1.TabIndex = 0;
            // 
            // _tabPageAutoPublish
            // 
            this._tabPageAutoPublish.Location = new System.Drawing.Point(4, 22);
            this._tabPageAutoPublish.Name = "_tabPageAutoPublish";
            this._tabPageAutoPublish.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageAutoPublish.Size = new System.Drawing.Size(705, 459);
            this._tabPageAutoPublish.TabIndex = 9;
            this._tabPageAutoPublish.Text = "שידורים אוטו\'";
            this._tabPageAutoPublish.UseVisualStyleBackColor = true;
            // 
            // _buttonArticlePreview
            // 
            this._buttonArticlePreview.Location = new System.Drawing.Point(159, 531);
            this._buttonArticlePreview.Name = "_buttonArticlePreview";
            this._buttonArticlePreview.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._buttonArticlePreview.Size = new System.Drawing.Size(80, 44);
            this._buttonArticlePreview.TabIndex = 26;
            this._buttonArticlePreview.Text = "הצג בדפדפן";
            this._buttonArticlePreview.UseVisualStyleBackColor = true;
            // 
            // _contextMenuStripTreeNode
            // 
            this._contextMenuStripTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripMenuItemAddCategory,
            this._toolStripMenuItemDeleteCategory,
            this._toolStripMenuItemUpdateCategory});
            this._contextMenuStripTreeNode.Name = "contextMenuStripTreeNode";
            this._contextMenuStripTreeNode.Size = new System.Drawing.Size(147, 70);
            // 
            // _toolStripMenuItemAddCategory
            // 
            this._toolStripMenuItemAddCategory.Name = "_toolStripMenuItemAddCategory";
            this._toolStripMenuItemAddCategory.Size = new System.Drawing.Size(146, 22);
            this._toolStripMenuItemAddCategory.Text = "הוסף קטגוריה";
            // 
            // _toolStripMenuItemDeleteCategory
            // 
            this._toolStripMenuItemDeleteCategory.Name = "_toolStripMenuItemDeleteCategory";
            this._toolStripMenuItemDeleteCategory.Size = new System.Drawing.Size(146, 22);
            this._toolStripMenuItemDeleteCategory.Text = "מחק קטגוריה";
            // 
            // _toolStripMenuItemUpdateCategory
            // 
            this._toolStripMenuItemUpdateCategory.Name = "_toolStripMenuItemUpdateCategory";
            this._toolStripMenuItemUpdateCategory.Size = new System.Drawing.Size(146, 22);
            this._toolStripMenuItemUpdateCategory.Text = "עדכן טקסט";
            // 
            // _tableLookupReportersBindingSource
            // 
            this._tableLookupReportersBindingSource.DataMember = "Table_LookupReporters";
            this._tableLookupReportersBindingSource.DataSource = this._kanNaimDataSetReporters;
            // 
            // _kanNaimDataSetReporters
            // 
            this._kanNaimDataSetReporters.DataSetName = "_Kan_NaimDataSetReporters";
            this._kanNaimDataSetReporters.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _buttonSaveArticle
            // 
            this._buttonSaveArticle.Location = new System.Drawing.Point(310, 531);
            this._buttonSaveArticle.Name = "_buttonSaveArticle";
            this._buttonSaveArticle.Size = new System.Drawing.Size(132, 44);
            this._buttonSaveArticle.TabIndex = 7;
            this._buttonSaveArticle.Text = "שמור כתבה";
            this._buttonSaveArticle.UseVisualStyleBackColor = true;
            // 
            // _kanNaimDataSet
            // 
            this._kanNaimDataSet.DataSetName = "_Kan_NaimDataSet";
            this._kanNaimDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _tableLookupArticleStatusBindingSource
            // 
            this._tableLookupArticleStatusBindingSource.DataMember = "Table_LookupArticleStatus";
            this._tableLookupArticleStatusBindingSource.DataSource = this._kanNaimDataSet;
            // 
            // _tableLookupArticleStatusTableAdapter
            // 
            this._tableLookupArticleStatusTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupReportersTableAdapter
            // 
            this._tableLookupReportersTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupCategoriesTableAdapter
            // 
            this._tableLookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupReportersTableAdapter1
            // 
            this._tableLookupReportersTableAdapter1.ClearBeforeFill = true;
            // 
            // _tablePhotosArchiveTableAdapter
            // 
            this._tablePhotosArchiveTableAdapter.ClearBeforeFill = true;
            // 
            // _kanNaimDataSet2
            // 
            this._kanNaimDataSet2.DataSetName = "_Kan_NaimDataSet2";
            this._kanNaimDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _spGetAllPhotosByOriginIdBindingSource
            // 
            this._spGetAllPhotosByOriginIdBindingSource.DataMember = "sp_GetAllPhotosByOriginId";
            this._spGetAllPhotosByOriginIdBindingSource.DataSource = this._kanNaimDataSet2;
            // 
            // _spGetAllPhotosByOriginIdTableAdapter
            // 
            this._spGetAllPhotosByOriginIdTableAdapter.ClearBeforeFill = true;
            // 
            // _groupBox1
            // 
            this._groupBox1.Controls.Add(this._radioButtonSaveAsPrivate);
            this._groupBox1.Controls.Add(this._radioButtonSaveAsPublic);
            this._groupBox1.Enabled = false;
            this._groupBox1.Location = new System.Drawing.Point(457, 525);
            this._groupBox1.Name = "_groupBox1";
            this._groupBox1.Size = new System.Drawing.Size(249, 50);
            this._groupBox1.TabIndex = 8;
            this._groupBox1.TabStop = false;
            this._groupBox1.Text = "בחר שמירה לארכיון ציבורי או פרטי לעריכה";
            this._groupBox1.Visible = false;
            // 
            // _radioButtonSaveAsPrivate
            // 
            this._radioButtonSaveAsPrivate.AutoSize = true;
            this._radioButtonSaveAsPrivate.Location = new System.Drawing.Point(53, 19);
            this._radioButtonSaveAsPrivate.Name = "_radioButtonSaveAsPrivate";
            this._radioButtonSaveAsPrivate.Size = new System.Drawing.Size(50, 17);
            this._radioButtonSaveAsPrivate.TabIndex = 1;
            this._radioButtonSaveAsPrivate.Text = "פרטי";
            this._radioButtonSaveAsPrivate.UseVisualStyleBackColor = true;
            // 
            // _radioButtonSaveAsPublic
            // 
            this._radioButtonSaveAsPublic.AutoSize = true;
            this._radioButtonSaveAsPublic.Checked = true;
            this._radioButtonSaveAsPublic.Location = new System.Drawing.Point(133, 19);
            this._radioButtonSaveAsPublic.Name = "_radioButtonSaveAsPublic";
            this._radioButtonSaveAsPublic.Size = new System.Drawing.Size(60, 17);
            this._radioButtonSaveAsPublic.TabIndex = 0;
            this._radioButtonSaveAsPublic.TabStop = true;
            this._radioButtonSaveAsPublic.Text = "ציבורי";
            this._radioButtonSaveAsPublic.UseVisualStyleBackColor = true;
            // 
            // FormEditArtical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(781, 604);
            this.Controls.Add(this._groupBox1);
            this.Controls.Add(this._buttonSaveArticle);
            this.Controls.Add(this._tabControl1);
            this.Controls.Add(this._buttonArticlePreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormEditArtical";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "עריכת כתבה";
            this._tabControl1.ResumeLayout(false);
            this._tabPageArticle.ResumeLayout(false);
            this._groupBox8.ResumeLayout(false);
            this._groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tablePhotosArchiveBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReportersNames)).EndInit();
            this._tabPageTak3X.ResumeLayout(false);
            this._tabPageTak3X.PerformLayout();
            this._tabPageTak2X.ResumeLayout(false);
            this._tabPageTak2X.PerformLayout();
            this._tabPageTak1X.ResumeLayout(false);
            this._tabPageTak1X.PerformLayout();
            this._tabPageTakMedium.ResumeLayout(false);
            this._tabPageTakMedium.PerformLayout();
            this._tabPageTakSmall.ResumeLayout(false);
            this._tabPageTakSmall.PerformLayout();
            this._tabPageCategories.ResumeLayout(false);
            this._groupBox7.ResumeLayout(false);
            this._tabPagePhotos.ResumeLayout(false);
            this._tabPageVideo.ResumeLayout(false);
            this._contextMenuStripTreeNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupReportersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSetReporters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tableLookupArticleStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._kanNaimDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spGetAllPhotosByOriginIdBindingSource)).EndInit();
            this._groupBox1.ResumeLayout(false);
            this._groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tabControl1;
        private System.Windows.Forms.TabPage _tabPageArticle;
        private System.Windows.Forms.GroupBox _groupBox8;
        private System.Windows.Forms.ComboBox _comboBoxArticlePhoto;
        private System.Windows.Forms.ComboBox _comboBoxArticleVideo;
        private System.Windows.Forms.DateTimePicker _dateTimePicker22;
        private System.Windows.Forms.Button _buttonArticlePreview;
        private System.Windows.Forms.Label _labelArtical;
        private System.Windows.Forms.TextBox _textBoxKeyWords;
        private System.Windows.Forms.Label _labelKeyWords;
        private System.Windows.Forms.TextBox _textBoxTags;
        private System.Windows.Forms.Label _labelTags;
        private System.Windows.Forms.Label _labelEditor;
        private System.Windows.Forms.TextBox _textBoxArticleTitle;
        private System.Windows.Forms.TextBox _textBoxArticleSubtitle;
        private System.Windows.Forms.ComboBox _comboBoxEditor;
        private System.Windows.Forms.DateTimePicker _dateTimePicker21;
        private System.Windows.Forms.CheckBox _checkBoxMivzak;
        private System.Windows.Forms.CheckBox _checkBoxDateTime;
        private System.Windows.Forms.CheckBox _checkBoxRss;
        private System.Windows.Forms.CheckBox _checkBoxPublish;
        private System.Windows.Forms.Label _labelTitle;
        private System.Windows.Forms.Label _labelSubtitle;
        private System.Windows.Forms.TabPage _tabPageTak3X;
        private System.Windows.Forms.TabPage _tabPageTak2X;
        private System.Windows.Forms.TabPage _tabPageTak1X;
        private System.Windows.Forms.TabPage _tabPageTakMedium;
        private System.Windows.Forms.TabPage _tabPageTakSmall;
        private System.Windows.Forms.TabPage _tabPageCategories;
        private System.Windows.Forms.GroupBox _groupBox7;
        private System.Windows.Forms.Button _buttonAddAllCategories;
        private System.Windows.Forms.Button _buttonClearCategoriesList;
        private System.Windows.Forms.ListBox _listBoxSelectedCategories;
        private System.Windows.Forms.Button _buttonAddSelectedCategories;
        private System.Windows.Forms.Button _buttonRemoveSelectedCategory;
        private System.Windows.Forms.TabPage _tabPagePhotos;
        private System.Windows.Forms.TabPage _tabPageVideo;
        private System.Windows.Forms.TabPage _tabPageAutoPublish;
        private System.Windows.Forms.Button _buttonSaveArticle;
        private _Kan_NaimDataSet _kanNaimDataSet;
        private System.Windows.Forms.BindingSource _tableLookupArticleStatusBindingSource;
        private _Kan_NaimDataSetTableAdapters.Table_LookupArticleStatusTableAdapter _tableLookupArticleStatusTableAdapter;
        private _Kan_NaimDataSetReporters _kanNaimDataSetReporters;
        private System.Windows.Forms.BindingSource _tableLookupReportersBindingSource;
        private _Kan_NaimDataSetReportersTableAdapters.Table_LookupReportersTableAdapter _tableLookupReportersTableAdapter;
        private System.Windows.Forms.Button _buttonSearchVideosArchive;
        private System.Windows.Forms.Button _buttonSearchPhotosArchive;
        private System.Windows.Forms.ComboBox _comboBoxArticleCategory;
        private System.Windows.Forms.Label _label22;
        private _Kan_NaimDataSetCategories _kanNaimDataSetCategories;
        private System.Windows.Forms.BindingSource _tableLookupCategoriesBindingSource;
        private _Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter _tableLookupCategoriesTableAdapter;
        private _Kan_NaimDataSetReportersNames _kanNaimDataSetReportersNames;
        private System.Windows.Forms.BindingSource _tableLookupReportersBindingSource1;
        private _Kan_NaimDataSetReportersNamesTableAdapters.Table_LookupReportersTableAdapter _tableLookupReportersTableAdapter1;
        private _Kan_NaimDataSet1 _kanNaimDataSet1;
        private System.Windows.Forms.BindingSource _tablePhotosArchiveBindingSource;
        private _Kan_NaimDataSet1TableAdapters.Table_PhotosArchiveTableAdapter _tablePhotosArchiveTableAdapter;
        private System.Windows.Forms.Label _labelOriginPhotoId;
        private System.Windows.Forms.BindingSource _spGetAllPhotosByOriginIdBindingSource;
        private _Kan_NaimDataSet2 _kanNaimDataSet2;
        private _Kan_NaimDataSet2TableAdapters.sp_GetAllPhotosByOriginIdTableAdapter _spGetAllPhotosByOriginIdTableAdapter;
        private System.Windows.Forms.Button _buttonReloadCategoryTree;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStripTreeNode;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItemAddCategory;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItemDeleteCategory;
        private System.Windows.Forms.ToolStripMenuItem _toolStripMenuItemUpdateCategory;
        private System.Windows.Forms.Button _buttonManageCategories;
        private System.Windows.Forms.GroupBox _groupBox1;
        private System.Windows.Forms.RadioButton _radioButtonSaveAsPrivate;
        private System.Windows.Forms.RadioButton _radioButtonSaveAsPublic;
        private UserControlTreeView _userControlTreeView1;
        private UserControlTakFill _userControlTakFillSizeX3;
        private UserControlTakFill _userControlTakFillSizeX2;
        private UserControlTakFill _userControlTakFillSizeX1;
        private UserControlTakFill _userControlTakFillSizeMedium;
        private UserControlTakFill _userControlTakFillSizeSmall;
        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.Label _label5;
        private System.Windows.Forms.Label _label4;
        private System.Windows.Forms.Label _label3;
        private System.Windows.Forms.Label _label2;
        private System.Windows.Forms.Button _buttonOpenEditor;
        private System.Windows.Forms.Button _buttonTitleH1;
        private System.Windows.Forms.Button _buttonSubTitleH2;
        private System.Windows.Forms.ComboBox _comboBoxVideoPos;
        private System.Windows.Forms.ComboBox _comboBoxImgPos;
        private System.Windows.Forms.RichTextBox _richTextBoxArticleContent;
        private UserControlUploadPhoto _ucUploadPhoto1;
        private UserControlUploadVideo _ucUploadVideo1;
        private System.ComponentModel.IContainer components;
    }
}
*/
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
            _components = new System.ComponentModel.Container();
            _tabControl1 = new System.Windows.Forms.TabControl();
            _tabPageArticle = new System.Windows.Forms.TabPage();
            _groupBox8 = new System.Windows.Forms.GroupBox();
            _richTextBoxArticleContent = new System.Windows.Forms.RichTextBox();
            _comboBoxVideoPos = new System.Windows.Forms.ComboBox();
            _comboBoxImgPos = new System.Windows.Forms.ComboBox();
            _buttonSubTitleH2 = new System.Windows.Forms.Button();
            _buttonTitleH1 = new System.Windows.Forms.Button();
            _buttonOpenEditor = new System.Windows.Forms.Button();
            _labelOriginPhotoId = new System.Windows.Forms.Label();
            _comboBoxArticleCategory = new System.Windows.Forms.ComboBox();
            _tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(_components);
            _kanNaimDataSetCategories = new Kan_Naim_Main._Kan_NaimDataSetCategories();
            _label22 = new System.Windows.Forms.Label();
            _buttonSearchVideosArchive = new System.Windows.Forms.Button();
            _buttonSearchPhotosArchive = new System.Windows.Forms.Button();
            _comboBoxArticlePhoto = new System.Windows.Forms.ComboBox();
            _tablePhotosArchiveBindingSource = new System.Windows.Forms.BindingSource(_components);
            _kanNaimDataSet1 = new Kan_Naim_Main._Kan_NaimDataSet1();
            _comboBoxArticleVideo = new System.Windows.Forms.ComboBox();
            _dateTimePicker22 = new System.Windows.Forms.DateTimePicker();
            _labelArtical = new System.Windows.Forms.Label();
            _textBoxKeyWords = new System.Windows.Forms.TextBox();
            _labelKeyWords = new System.Windows.Forms.Label();
            _textBoxTags = new System.Windows.Forms.TextBox();
            _labelTags = new System.Windows.Forms.Label();
            _labelEditor = new System.Windows.Forms.Label();
            _textBoxArticleTitle = new System.Windows.Forms.TextBox();
            _comboBoxEditor = new System.Windows.Forms.ComboBox();
            _tableLookupReportersBindingSource1 = new System.Windows.Forms.BindingSource(_components);
            _kanNaimDataSetReportersNames = new Kan_Naim_Main._Kan_NaimDataSetReportersNames();
            _dateTimePicker21 = new System.Windows.Forms.DateTimePicker();
            _checkBoxMivzak = new System.Windows.Forms.CheckBox();
            _checkBoxDateTime = new System.Windows.Forms.CheckBox();
            _checkBoxRss = new System.Windows.Forms.CheckBox();
            _checkBoxPublish = new System.Windows.Forms.CheckBox();
            _labelTitle = new System.Windows.Forms.Label();
            _labelSubtitle = new System.Windows.Forms.Label();
            _textBoxArticleSubtitle = new System.Windows.Forms.TextBox();
            _tabPageTak3X = new System.Windows.Forms.TabPage();
            _label5 = new System.Windows.Forms.Label();
            _userControlTakFillSizeX3 = new HaimDLL.UserControlTakFill();
            _tabPageTak2X = new System.Windows.Forms.TabPage();
            _label4 = new System.Windows.Forms.Label();
            _userControlTakFillSizeX2 = new HaimDLL.UserControlTakFill();
            _tabPageTak1X = new System.Windows.Forms.TabPage();
            _label3 = new System.Windows.Forms.Label();
            _userControlTakFillSizeX1 = new HaimDLL.UserControlTakFill();
            _tabPageTakMedium = new System.Windows.Forms.TabPage();
            _label2 = new System.Windows.Forms.Label();
            _userControlTakFillSizeMedium = new HaimDLL.UserControlTakFill();
            _tabPageTakSmall = new System.Windows.Forms.TabPage();
            _label1 = new System.Windows.Forms.Label();
            _userControlTakFillSizeSmall = new HaimDLL.UserControlTakFill();
            _tabPageCategories = new System.Windows.Forms.TabPage();
            _groupBox7 = new System.Windows.Forms.GroupBox();
            _userControlTreeView1 = new HaimDLL.UserControlTreeView();
            _buttonManageCategories = new System.Windows.Forms.Button();
            _buttonReloadCategoryTree = new System.Windows.Forms.Button();
            _buttonAddAllCategories = new System.Windows.Forms.Button();
            _buttonClearCategoriesList = new System.Windows.Forms.Button();
            _listBoxSelectedCategories = new System.Windows.Forms.ListBox();
            _buttonAddSelectedCategories = new System.Windows.Forms.Button();
            _buttonRemoveSelectedCategory = new System.Windows.Forms.Button();
            _tabPagePhotos = new System.Windows.Forms.TabPage();
            _ucUploadPhoto1 = new HaimDLL.UserControlUploadPhoto();
            _ucUploadPhoto1.SetSaveButtonCallbackFunction(SaveNewPhotosClick);
            _tabPageVideo = new System.Windows.Forms.TabPage();
            _ucUploadVideo1 = new HaimDLL.UserControlUploadVideo();
            _tabPageAutoPublish = new System.Windows.Forms.TabPage();
            _buttonArticlePreview = new System.Windows.Forms.Button();
            _contextMenuStripTreeNode = new System.Windows.Forms.ContextMenuStrip(_components);
            _toolStripMenuItemAddCategory = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripMenuItemDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripMenuItemUpdateCategory = new System.Windows.Forms.ToolStripMenuItem();
            _tableLookupReportersBindingSource = new System.Windows.Forms.BindingSource(_components);
            _kanNaimDataSetReporters = new Kan_Naim_Main._Kan_NaimDataSetReporters();
            _buttonSaveArticle = new System.Windows.Forms.Button();
            _kanNaimDataSet = new Kan_Naim_Main._Kan_NaimDataSet();
            _tableLookupArticleStatusBindingSource = new System.Windows.Forms.BindingSource(_components);
            _tableLookupArticleStatusTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetTableAdapters.Table_LookupArticleStatusTableAdapter();
            _tableLookupReportersTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetReportersTableAdapters.Table_LookupReportersTableAdapter();
            _tableLookupCategoriesTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            _tableLookupReportersTableAdapter1 = new Kan_Naim_Main._Kan_NaimDataSetReportersNamesTableAdapters.Table_LookupReportersTableAdapter();
            _tablePhotosArchiveTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet1TableAdapters.Table_PhotosArchiveTableAdapter();
            _kanNaimDataSet2 = new Kan_Naim_Main._Kan_NaimDataSet2();
            _spGetAllPhotosByOriginIdBindingSource = new System.Windows.Forms.BindingSource(_components);
            _spGetAllPhotosByOriginIdTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet2TableAdapters.sp_GetAllPhotosByOriginIdTableAdapter();
            _groupBox1 = new System.Windows.Forms.GroupBox();
            _radioButtonSaveAsPrivate = new System.Windows.Forms.RadioButton();
            _radioButtonSaveAsPublic = new System.Windows.Forms.RadioButton();
            _tabControl1.SuspendLayout();
            _tabPageArticle.SuspendLayout();
            _groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSetCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_tablePhotosArchiveBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupReportersBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSetReportersNames)).BeginInit();
            _tabPageTak3X.SuspendLayout();
            _tabPageTak2X.SuspendLayout();
            _tabPageTak1X.SuspendLayout();
            _tabPageTakMedium.SuspendLayout();
            _tabPageTakSmall.SuspendLayout();
            _tabPageCategories.SuspendLayout();
            _groupBox7.SuspendLayout();
            _tabPagePhotos.SuspendLayout();
            _tabPageVideo.SuspendLayout();
            _contextMenuStripTreeNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupReportersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSetReporters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupArticleStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_spGetAllPhotosByOriginIdBindingSource)).BeginInit();
            _groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // _tabControl1
            // 
            _tabControl1.Controls.Add(_tabPageArticle);
            _tabControl1.Controls.Add(_tabPageTak3X);
            _tabControl1.Controls.Add(_tabPageTak2X);
            _tabControl1.Controls.Add(_tabPageTak1X);
            _tabControl1.Controls.Add(_tabPageTakMedium);
            _tabControl1.Controls.Add(_tabPageTakSmall);
            _tabControl1.Controls.Add(_tabPageCategories);
            _tabControl1.Controls.Add(_tabPagePhotos);
            _tabControl1.Controls.Add(_tabPageVideo);
            _tabControl1.Controls.Add(_tabPageAutoPublish);
            _tabControl1.HotTrack = true;
            _tabControl1.Location = new System.Drawing.Point(12, 27);
            _tabControl1.Name = "_tabControl1";
            _tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _tabControl1.RightToLeftLayout = true;
            _tabControl1.SelectedIndex = 0;
            _tabControl1.Size = new System.Drawing.Size(713, 485);
            _tabControl1.TabIndex = 4;
            // 
            // _tabPageArticle
            // 
            _tabPageArticle.Controls.Add(_groupBox8);
            _tabPageArticle.Location = new System.Drawing.Point(4, 22);
            _tabPageArticle.Name = "_tabPageArticle";
            _tabPageArticle.Padding = new System.Windows.Forms.Padding(3);
            _tabPageArticle.Size = new System.Drawing.Size(705, 459);
            _tabPageArticle.TabIndex = 0;
            _tabPageArticle.Text = "כתבה";
            _tabPageArticle.UseVisualStyleBackColor = true;
            // 
            // _groupBox8
            // 
            _groupBox8.Controls.Add(_richTextBoxArticleContent);
            _groupBox8.Controls.Add(_comboBoxVideoPos);
            _groupBox8.Controls.Add(_comboBoxImgPos);
            _groupBox8.Controls.Add(_buttonSubTitleH2);
            _groupBox8.Controls.Add(_buttonTitleH1);
            _groupBox8.Controls.Add(_buttonOpenEditor);
            _groupBox8.Controls.Add(_labelOriginPhotoId);
            _groupBox8.Controls.Add(_comboBoxArticleCategory);
            _groupBox8.Controls.Add(_label22);
            _groupBox8.Controls.Add(_buttonSearchVideosArchive);
            _groupBox8.Controls.Add(_buttonSearchPhotosArchive);
            _groupBox8.Controls.Add(_comboBoxArticlePhoto);
            _groupBox8.Controls.Add(_comboBoxArticleVideo);
            _groupBox8.Controls.Add(_dateTimePicker22);
            _groupBox8.Controls.Add(_labelArtical);
            _groupBox8.Controls.Add(_textBoxKeyWords);
            _groupBox8.Controls.Add(_labelKeyWords);
            _groupBox8.Controls.Add(_textBoxTags);
            _groupBox8.Controls.Add(_labelTags);
            _groupBox8.Controls.Add(_labelEditor);
            _groupBox8.Controls.Add(_textBoxArticleTitle);
            _groupBox8.Controls.Add(_comboBoxEditor);
            _groupBox8.Controls.Add(_dateTimePicker21);
            _groupBox8.Controls.Add(_checkBoxMivzak);
            _groupBox8.Controls.Add(_checkBoxDateTime);
            _groupBox8.Controls.Add(_checkBoxRss);
            _groupBox8.Controls.Add(_checkBoxPublish);
            _groupBox8.Controls.Add(_labelTitle);
            _groupBox8.Controls.Add(_labelSubtitle);
            _groupBox8.Controls.Add(_textBoxArticleSubtitle);
            _groupBox8.Location = new System.Drawing.Point(15, 6);
            _groupBox8.Name = "_groupBox8";
            _groupBox8.Size = new System.Drawing.Size(675, 447);
            _groupBox8.TabIndex = 4;
            _groupBox8.TabStop = false;
            _groupBox8.Text = "הזנת תוכן ומאפייני הכתבה";
            // 
            // _richTextBoxArticleContent
            // 
            _richTextBoxArticleContent.Enabled = false;
            _richTextBoxArticleContent.Location = new System.Drawing.Point(19, 264);
            _richTextBoxArticleContent.MaxLength = 10000;
            _richTextBoxArticleContent.Name = "_richTextBoxArticleContent";
            _richTextBoxArticleContent.Size = new System.Drawing.Size(541, 165);
            _richTextBoxArticleContent.TabIndex = 85;
            _richTextBoxArticleContent.Text = "";
            // 
            // _comboBoxVideoPos
            // 
            _comboBoxVideoPos.Enabled = false;
            _comboBoxVideoPos.FormattingEnabled = true;
            _comboBoxVideoPos.Items.AddRange(new object[] {
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
            _comboBoxVideoPos.Location = new System.Drawing.Point(19, 171);
            _comboBoxVideoPos.Name = "_comboBoxVideoPos";
            _comboBoxVideoPos.Size = new System.Drawing.Size(70, 21);
            _comboBoxVideoPos.TabIndex = 84;
            _comboBoxVideoPos.Text = "  מלמעלה";
            _comboBoxVideoPos.Visible = false;
            // 
            // _comboBoxImgPos
            // 
            _comboBoxImgPos.Enabled = false;
            _comboBoxImgPos.FormattingEnabled = true;
            _comboBoxImgPos.Items.AddRange(new object[] {
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
            _comboBoxImgPos.Location = new System.Drawing.Point(19, 144);
            _comboBoxImgPos.Name = "_comboBoxImgPos";
            _comboBoxImgPos.Size = new System.Drawing.Size(70, 21);
            _comboBoxImgPos.TabIndex = 83;
            _comboBoxImgPos.Text = "  מלמעלה";
            _comboBoxImgPos.Visible = false;
            // 
            // _buttonSubTitleH2
            // 
            _buttonSubTitleH2.Location = new System.Drawing.Point(573, 344);
            _buttonSubTitleH2.Name = "_buttonSubTitleH2";
            _buttonSubTitleH2.Size = new System.Drawing.Size(59, 23);
            _buttonSubTitleH2.TabIndex = 79;
            _buttonSubTitleH2.Text = "H2";
            _buttonSubTitleH2.UseVisualStyleBackColor = true;
            _buttonSubTitleH2.Click += new System.EventHandler(buttonTitlesH1andH2_Click);
            // 
            // _buttonTitleH1
            // 
            _buttonTitleH1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _buttonTitleH1.Location = new System.Drawing.Point(573, 315);
            _buttonTitleH1.Name = "_buttonTitleH1";
            _buttonTitleH1.Size = new System.Drawing.Size(59, 23);
            _buttonTitleH1.TabIndex = 78;
            _buttonTitleH1.Text = "H1";
            _buttonTitleH1.UseVisualStyleBackColor = true;
            _buttonTitleH1.Click += new System.EventHandler(buttonTitlesH1andH2_Click);
            // 
            // _buttonOpenEditor
            // 
            _buttonOpenEditor.Location = new System.Drawing.Point(573, 286);
            _buttonOpenEditor.Name = "_buttonOpenEditor";
            _buttonOpenEditor.Size = new System.Drawing.Size(59, 23);
            _buttonOpenEditor.TabIndex = 76;
            _buttonOpenEditor.Text = "עריכה";
            _buttonOpenEditor.UseVisualStyleBackColor = true;
            _buttonOpenEditor.Click += new System.EventHandler(buttonOpenEditor_Click);
            // 
            // _labelOriginPhotoId
            // 
            _labelOriginPhotoId.AutoSize = true;
            _labelOriginPhotoId.Location = new System.Drawing.Point(642, 279);
            _labelOriginPhotoId.Name = "_labelOriginPhotoId";
            _labelOriginPhotoId.Size = new System.Drawing.Size(13, 13);
            _labelOriginPhotoId.TabIndex = 62;
            _labelOriginPhotoId.Text = "0";
            _labelOriginPhotoId.Visible = false;
            // 
            // _comboBoxArticleCategory
            // 
            _comboBoxArticleCategory.DataSource = _tableLookupCategoriesBindingSource;
            _comboBoxArticleCategory.DisplayMember = "CatHebrewName";
            _comboBoxArticleCategory.FormattingEnabled = true;
            _comboBoxArticleCategory.Location = new System.Drawing.Point(19, 117);
            _comboBoxArticleCategory.Name = "_comboBoxArticleCategory";
            _comboBoxArticleCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _comboBoxArticleCategory.Size = new System.Drawing.Size(193, 21);
            _comboBoxArticleCategory.TabIndex = 61;
            _comboBoxArticleCategory.ValueMember = "CatId";
            _comboBoxArticleCategory.SelectedIndexChanged += new System.EventHandler(comboBoxArticleCategory_SelectedIndexChanged);
            // 
            // _tableLookupCategoriesBindingSource
            // 
            _tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            _tableLookupCategoriesBindingSource.DataSource = _kanNaimDataSetCategories;
            // 
            // _kanNaimDataSetCategories
            // 
            _kanNaimDataSetCategories.DataSetName = "_Kan_NaimDataSetCategories";
            _kanNaimDataSetCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _label22
            // 
            _label22.AutoSize = true;
            _label22.Location = new System.Drawing.Point(218, 121);
            _label22.Name = "_label22";
            _label22.Size = new System.Drawing.Size(50, 13);
            _label22.TabIndex = 60;
            _label22.Text = "קטגוריה";
            // 
            // _buttonSearchVideosArchive
            // 
            _buttonSearchVideosArchive.Enabled = false;
            _buttonSearchVideosArchive.Location = new System.Drawing.Point(577, 169);
            _buttonSearchVideosArchive.Name = "_buttonSearchVideosArchive";
            _buttonSearchVideosArchive.Size = new System.Drawing.Size(75, 23);
            _buttonSearchVideosArchive.TabIndex = 59;
            _buttonSearchVideosArchive.Text = "וידאו..";
            _buttonSearchVideosArchive.UseVisualStyleBackColor = true;
            // 
            // _buttonSearchPhotosArchive
            // 
            _buttonSearchPhotosArchive.Enabled = false;
            _buttonSearchPhotosArchive.Location = new System.Drawing.Point(577, 142);
            _buttonSearchPhotosArchive.Name = "_buttonSearchPhotosArchive";
            _buttonSearchPhotosArchive.Size = new System.Drawing.Size(75, 23);
            _buttonSearchPhotosArchive.TabIndex = 58;
            _buttonSearchPhotosArchive.Text = "תמונה..";
            _buttonSearchPhotosArchive.UseVisualStyleBackColor = true;
            // 
            // _comboBoxArticlePhoto
            // 
            _comboBoxArticlePhoto.DataSource = _tablePhotosArchiveBindingSource;
            _comboBoxArticlePhoto.DisplayMember = "ImageUrl";
            _comboBoxArticlePhoto.FormattingEnabled = true;
            _comboBoxArticlePhoto.Location = new System.Drawing.Point(95, 144);
            _comboBoxArticlePhoto.Name = "_comboBoxArticlePhoto";
            _comboBoxArticlePhoto.Size = new System.Drawing.Size(465, 21);
            _comboBoxArticlePhoto.TabIndex = 57;
            _comboBoxArticlePhoto.ValueMember = "Id";
            _comboBoxArticlePhoto.SelectedIndexChanged += new System.EventHandler(comboBoxArticlePhoto_SelectedIndexChanged);
            // 
            // _tablePhotosArchiveBindingSource
            // 
            _tablePhotosArchiveBindingSource.DataMember = "Table_PhotosArchive";
            _tablePhotosArchiveBindingSource.DataSource = _kanNaimDataSet1;
            // 
            // _kanNaimDataSet1
            // 
            _kanNaimDataSet1.DataSetName = "_Kan_NaimDataSet1";
            _kanNaimDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _comboBoxArticleVideo
            // 
            _comboBoxArticleVideo.Enabled = false;
            _comboBoxArticleVideo.FormattingEnabled = true;
            _comboBoxArticleVideo.Location = new System.Drawing.Point(95, 171);
            _comboBoxArticleVideo.Name = "_comboBoxArticleVideo";
            _comboBoxArticleVideo.Size = new System.Drawing.Size(465, 21);
            _comboBoxArticleVideo.TabIndex = 56;
            _comboBoxArticleVideo.SelectedIndexChanged += new System.EventHandler(comboBoxArticleVideo_SelectedIndexChanged);
            // 
            // _dateTimePicker22
            // 
            _dateTimePicker22.Enabled = false;
            _dateTimePicker22.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            _dateTimePicker22.Location = new System.Drawing.Point(19, 91);
            _dateTimePicker22.Name = "_dateTimePicker22";
            _dateTimePicker22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _dateTimePicker22.RightToLeftLayout = true;
            _dateTimePicker22.Size = new System.Drawing.Size(90, 20);
            _dateTimePicker22.TabIndex = 34;
            // 
            // _labelArtical
            // 
            _labelArtical.AutoSize = true;
            _labelArtical.Location = new System.Drawing.Point(571, 264);
            _labelArtical.Name = "_labelArtical";
            _labelArtical.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _labelArtical.Size = new System.Drawing.Size(62, 13);
            _labelArtical.TabIndex = 24;
            _labelArtical.Text = "תוכן כתבה";
            // 
            // _textBoxKeyWords
            // 
            _textBoxKeyWords.Location = new System.Drawing.Point(19, 225);
            _textBoxKeyWords.MaxLength = 200;
            _textBoxKeyWords.Name = "_textBoxKeyWords";
            _textBoxKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _textBoxKeyWords.Size = new System.Drawing.Size(541, 20);
            _textBoxKeyWords.TabIndex = 23;
            _textBoxKeyWords.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _labelKeyWords
            // 
            _labelKeyWords.AutoSize = true;
            _labelKeyWords.Location = new System.Drawing.Point(577, 228);
            _labelKeyWords.Name = "_labelKeyWords";
            _labelKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _labelKeyWords.Size = new System.Drawing.Size(74, 13);
            _labelKeyWords.TabIndex = 22;
            _labelKeyWords.Text = "מילות חיפוש";
            // 
            // _textBoxTags
            // 
            _textBoxTags.Location = new System.Drawing.Point(19, 199);
            _textBoxTags.MaxLength = 200;
            _textBoxTags.Name = "_textBoxTags";
            _textBoxTags.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _textBoxTags.Size = new System.Drawing.Size(541, 20);
            _textBoxTags.TabIndex = 21;
            _textBoxTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _labelTags
            // 
            _labelTags.AutoSize = true;
            _labelTags.Location = new System.Drawing.Point(577, 202);
            _labelTags.Name = "_labelTags";
            _labelTags.Size = new System.Drawing.Size(37, 13);
            _labelTags.TabIndex = 20;
            _labelTags.Text = "תגיות";
            // 
            // _labelEditor
            // 
            _labelEditor.AutoSize = true;
            _labelEditor.Location = new System.Drawing.Point(577, 121);
            _labelEditor.Name = "_labelEditor";
            _labelEditor.Size = new System.Drawing.Size(32, 13);
            _labelEditor.TabIndex = 18;
            _labelEditor.Text = "עורך";
            // 
            // _textBoxArticleTitle
            // 
            _textBoxArticleTitle.Location = new System.Drawing.Point(17, 16);
            _textBoxArticleTitle.MaxLength = 150;
            _textBoxArticleTitle.Name = "_textBoxArticleTitle";
            _textBoxArticleTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _textBoxArticleTitle.Size = new System.Drawing.Size(541, 20);
            _textBoxArticleTitle.TabIndex = 15;
            _textBoxArticleTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _comboBoxEditor
            // 
            _comboBoxEditor.DataSource = _tableLookupReportersBindingSource1;
            _comboBoxEditor.DisplayMember = "PublishNameShort";
            _comboBoxEditor.FormattingEnabled = true;
            _comboBoxEditor.Location = new System.Drawing.Point(291, 118);
            _comboBoxEditor.Name = "_comboBoxEditor";
            _comboBoxEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _comboBoxEditor.Size = new System.Drawing.Size(269, 21);
            _comboBoxEditor.TabIndex = 10;
            _comboBoxEditor.ValueMember = "UserId";
            // 
            // _tableLookupReportersBindingSource1
            // 
            _tableLookupReportersBindingSource1.DataMember = "Table_LookupReporters";
            _tableLookupReportersBindingSource1.DataSource = _kanNaimDataSetReportersNames;
            // 
            // _kanNaimDataSetReportersNames
            // 
            _kanNaimDataSetReportersNames.DataSetName = "_Kan_NaimDataSetReportersNames";
            _kanNaimDataSetReportersNames.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _dateTimePicker21
            // 
            _dateTimePicker21.Enabled = false;
            _dateTimePicker21.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            _dateTimePicker21.Location = new System.Drawing.Point(125, 91);
            _dateTimePicker21.MaxDate = new System.DateTime(2015, 12, 31, 0, 0, 0, 0);
            _dateTimePicker21.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            _dateTimePicker21.Name = "_dateTimePicker21";
            _dateTimePicker21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _dateTimePicker21.RightToLeftLayout = true;
            _dateTimePicker21.Size = new System.Drawing.Size(87, 20);
            _dateTimePicker21.TabIndex = 9;
            _dateTimePicker21.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // _checkBoxMivzak
            // 
            _checkBoxMivzak.AutoSize = true;
            _checkBoxMivzak.Checked = true;
            _checkBoxMivzak.CheckState = System.Windows.Forms.CheckState.Checked;
            _checkBoxMivzak.Location = new System.Drawing.Point(428, 94);
            _checkBoxMivzak.Name = "_checkBoxMivzak";
            _checkBoxMivzak.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _checkBoxMivzak.Size = new System.Drawing.Size(52, 17);
            _checkBoxMivzak.TabIndex = 7;
            _checkBoxMivzak.Tag = "";
            _checkBoxMivzak.Text = "מבזק";
            _checkBoxMivzak.UseVisualStyleBackColor = true;
            // 
            // _checkBoxDateTime
            // 
            _checkBoxDateTime.AutoSize = true;
            _checkBoxDateTime.Checked = true;
            _checkBoxDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            _checkBoxDateTime.Location = new System.Drawing.Point(231, 94);
            _checkBoxDateTime.Name = "_checkBoxDateTime";
            _checkBoxDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _checkBoxDateTime.Size = new System.Drawing.Size(90, 17);
            _checkBoxDateTime.TabIndex = 6;
            _checkBoxDateTime.Tag = "";
            _checkBoxDateTime.Text = "תאריך ושעה";
            _checkBoxDateTime.UseVisualStyleBackColor = true;
            // 
            // _checkBoxRss
            // 
            _checkBoxRss.AutoSize = true;
            _checkBoxRss.Checked = true;
            _checkBoxRss.CheckState = System.Windows.Forms.CheckState.Checked;
            _checkBoxRss.Location = new System.Drawing.Point(348, 94);
            _checkBoxRss.Name = "_checkBoxRss";
            _checkBoxRss.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _checkBoxRss.Size = new System.Drawing.Size(48, 17);
            _checkBoxRss.TabIndex = 5;
            _checkBoxRss.Tag = "";
            _checkBoxRss.Text = "RSS";
            _checkBoxRss.UseVisualStyleBackColor = true;
            // 
            // _checkBoxPublish
            // 
            _checkBoxPublish.AutoSize = true;
            _checkBoxPublish.Checked = true;
            _checkBoxPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            _checkBoxPublish.Location = new System.Drawing.Point(505, 95);
            _checkBoxPublish.Name = "_checkBoxPublish";
            _checkBoxPublish.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _checkBoxPublish.Size = new System.Drawing.Size(53, 17);
            _checkBoxPublish.TabIndex = 4;
            _checkBoxPublish.Tag = "";
            _checkBoxPublish.Text = "פרסם";
            _checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // _labelTitle
            // 
            _labelTitle.AutoSize = true;
            _labelTitle.Location = new System.Drawing.Point(571, 19);
            _labelTitle.Name = "_labelTitle";
            _labelTitle.Size = new System.Drawing.Size(77, 13);
            _labelTitle.TabIndex = 3;
            _labelTitle.Text = "כותרת ראשית";
            // 
            // _labelSubtitle
            // 
            _labelSubtitle.AutoSize = true;
            _labelSubtitle.Location = new System.Drawing.Point(571, 52);
            _labelSubtitle.Name = "_labelSubtitle";
            _labelSubtitle.Size = new System.Drawing.Size(75, 13);
            _labelSubtitle.TabIndex = 0;
            _labelSubtitle.Text = "כותרת משנית";
            // 
            // _textBoxArticleSubtitle
            // 
            _textBoxArticleSubtitle.Location = new System.Drawing.Point(17, 49);
            _textBoxArticleSubtitle.MaxLength = 300;
            _textBoxArticleSubtitle.Multiline = true;
            _textBoxArticleSubtitle.Name = "_textBoxArticleSubtitle";
            _textBoxArticleSubtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _textBoxArticleSubtitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _textBoxArticleSubtitle.Size = new System.Drawing.Size(541, 36);
            _textBoxArticleSubtitle.TabIndex = 14;
            _textBoxArticleSubtitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _tabPageTak3X
            // 
            _tabPageTak3X.Controls.Add(_label5);
            _tabPageTak3X.Controls.Add(_userControlTakFillSizeX3);
            _tabPageTak3X.Location = new System.Drawing.Point(4, 22);
            _tabPageTak3X.Name = "_tabPageTak3X";
            _tabPageTak3X.Padding = new System.Windows.Forms.Padding(3);
            _tabPageTak3X.Size = new System.Drawing.Size(705, 459);
            _tabPageTak3X.TabIndex = 1;
            _tabPageTak3X.Text = "תקציר גדול X3";
            _tabPageTak3X.UseVisualStyleBackColor = true;
            // 
            // _label5
            // 
            _label5.AutoSize = true;
            _label5.ForeColor = System.Drawing.SystemColors.Highlight;
            _label5.Location = new System.Drawing.Point(644, 28);
            _label5.Name = "_label5";
            _label5.Size = new System.Drawing.Size(55, 13);
            _label5.TabIndex = 3;
            _label5.Text = "בגודל X3";
            // 
            // _userControlTakFillSizeX3
            // 
            _userControlTakFillSizeX3.Location = new System.Drawing.Point(6, 6);
            _userControlTakFillSizeX3.Name = "_userControlTakFillSizeX3";
            _userControlTakFillSizeX3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _userControlTakFillSizeX3.Size = new System.Drawing.Size(640, 445);
            _userControlTakFillSizeX3.TabIndex = 0;
            // 
            // _tabPageTak2X
            // 
            _tabPageTak2X.Controls.Add(_label4);
            _tabPageTak2X.Controls.Add(_userControlTakFillSizeX2);
            _tabPageTak2X.Location = new System.Drawing.Point(4, 22);
            _tabPageTak2X.Name = "_tabPageTak2X";
            _tabPageTak2X.Padding = new System.Windows.Forms.Padding(3);
            _tabPageTak2X.Size = new System.Drawing.Size(705, 459);
            _tabPageTak2X.TabIndex = 2;
            _tabPageTak2X.Text = "תקציר גדול X2";
            _tabPageTak2X.UseVisualStyleBackColor = true;
            // 
            // _label4
            // 
            _label4.AutoSize = true;
            _label4.ForeColor = System.Drawing.SystemColors.Highlight;
            _label4.Location = new System.Drawing.Point(647, 25);
            _label4.Name = "_label4";
            _label4.Size = new System.Drawing.Size(55, 13);
            _label4.TabIndex = 3;
            _label4.Text = "בגודל X2";
            // 
            // _userControlTakFillSizeX2
            // 
            _userControlTakFillSizeX2.Location = new System.Drawing.Point(6, 8);
            _userControlTakFillSizeX2.Name = "_userControlTakFillSizeX2";
            _userControlTakFillSizeX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _userControlTakFillSizeX2.Size = new System.Drawing.Size(635, 445);
            _userControlTakFillSizeX2.TabIndex = 0;
            // 
            // _tabPageTak1X
            // 
            _tabPageTak1X.Controls.Add(_label3);
            _tabPageTak1X.Controls.Add(_userControlTakFillSizeX1);
            _tabPageTak1X.Location = new System.Drawing.Point(4, 22);
            _tabPageTak1X.Name = "_tabPageTak1X";
            _tabPageTak1X.Padding = new System.Windows.Forms.Padding(3);
            _tabPageTak1X.Size = new System.Drawing.Size(705, 459);
            _tabPageTak1X.TabIndex = 3;
            _tabPageTak1X.Text = "תקציר גדול X1";
            _tabPageTak1X.UseVisualStyleBackColor = true;
            // 
            // _label3
            // 
            _label3.AutoSize = true;
            _label3.ForeColor = System.Drawing.SystemColors.Highlight;
            _label3.Location = new System.Drawing.Point(644, 18);
            _label3.Name = "_label3";
            _label3.Size = new System.Drawing.Size(55, 13);
            _label3.TabIndex = 2;
            _label3.Text = "בגודל X1";
            // 
            // _userControlTakFillSizeX1
            // 
            _userControlTakFillSizeX1.Location = new System.Drawing.Point(6, 3);
            _userControlTakFillSizeX1.Name = "_userControlTakFillSizeX1";
            _userControlTakFillSizeX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _userControlTakFillSizeX1.Size = new System.Drawing.Size(635, 445);
            _userControlTakFillSizeX1.TabIndex = 0;
            // 
            // _tabPageTakMedium
            // 
            _tabPageTakMedium.Controls.Add(_label2);
            _tabPageTakMedium.Controls.Add(_userControlTakFillSizeMedium);
            _tabPageTakMedium.Location = new System.Drawing.Point(4, 22);
            _tabPageTakMedium.Name = "_tabPageTakMedium";
            _tabPageTakMedium.Padding = new System.Windows.Forms.Padding(3);
            _tabPageTakMedium.Size = new System.Drawing.Size(705, 459);
            _tabPageTakMedium.TabIndex = 4;
            _tabPageTakMedium.Text = "תקציר בינוני";
            _tabPageTakMedium.UseVisualStyleBackColor = true;
            // 
            // _label2
            // 
            _label2.AutoSize = true;
            _label2.ForeColor = System.Drawing.SystemColors.Highlight;
            _label2.Location = new System.Drawing.Point(638, 21);
            _label2.Name = "_label2";
            _label2.Size = new System.Drawing.Size(74, 13);
            _label2.TabIndex = 2;
            _label2.Text = "בגודל בינוני";
            // 
            // _userControlTakFillSizeMedium
            // 
            _userControlTakFillSizeMedium.Location = new System.Drawing.Point(6, 6);
            _userControlTakFillSizeMedium.Name = "_userControlTakFillSizeMedium";
            _userControlTakFillSizeMedium.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _userControlTakFillSizeMedium.Size = new System.Drawing.Size(635, 445);
            _userControlTakFillSizeMedium.TabIndex = 0;
            // 
            // _tabPageTakSmall
            // 
            _tabPageTakSmall.Controls.Add(_label1);
            _tabPageTakSmall.Controls.Add(_userControlTakFillSizeSmall);
            _tabPageTakSmall.Location = new System.Drawing.Point(4, 22);
            _tabPageTakSmall.Name = "_tabPageTakSmall";
            _tabPageTakSmall.Padding = new System.Windows.Forms.Padding(3);
            _tabPageTakSmall.Size = new System.Drawing.Size(705, 459);
            _tabPageTakSmall.TabIndex = 5;
            _tabPageTakSmall.Text = "תקציר קטן";
            _tabPageTakSmall.UseVisualStyleBackColor = true;
            // 
            // _label1
            // 
            _label1.AutoSize = true;
            _label1.ForeColor = System.Drawing.SystemColors.Highlight;
            _label1.Location = new System.Drawing.Point(641, 25);
            _label1.Name = "_label1";
            _label1.Size = new System.Drawing.Size(61, 13);
            _label1.TabIndex = 1;
            _label1.Text = "בגודל קטן";
            // 
            // _userControlTakFillSizeSmall
            // 
            _userControlTakFillSizeSmall.Location = new System.Drawing.Point(6, 8);
            _userControlTakFillSizeSmall.Name = "_userControlTakFillSizeSmall";
            _userControlTakFillSizeSmall.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _userControlTakFillSizeSmall.Size = new System.Drawing.Size(635, 445);
            _userControlTakFillSizeSmall.TabIndex = 0;
            // 
            // _tabPageCategories
            // 
            _tabPageCategories.Controls.Add(_groupBox7);
            _tabPageCategories.Location = new System.Drawing.Point(4, 22);
            _tabPageCategories.Name = "_tabPageCategories";
            _tabPageCategories.Padding = new System.Windows.Forms.Padding(3);
            _tabPageCategories.Size = new System.Drawing.Size(705, 459);
            _tabPageCategories.TabIndex = 6;
            _tabPageCategories.Text = "קטגוריות";
            _tabPageCategories.UseVisualStyleBackColor = true;
            // 
            // _groupBox7
            // 
            _groupBox7.Controls.Add(_userControlTreeView1);
            _groupBox7.Controls.Add(_buttonManageCategories);
            _groupBox7.Controls.Add(_buttonReloadCategoryTree);
            _groupBox7.Controls.Add(_buttonAddAllCategories);
            _groupBox7.Controls.Add(_buttonClearCategoriesList);
            _groupBox7.Controls.Add(_listBoxSelectedCategories);
            _groupBox7.Controls.Add(_buttonAddSelectedCategories);
            _groupBox7.Controls.Add(_buttonRemoveSelectedCategory);
            _groupBox7.Location = new System.Drawing.Point(30, 19);
            _groupBox7.Name = "_groupBox7";
            _groupBox7.Size = new System.Drawing.Size(647, 434);
            _groupBox7.TabIndex = 39;
            _groupBox7.TabStop = false;
            _groupBox7.Text = "בחירה מרובה של קטגוריות הקשורות לכתבה";
            // 
            // _userControlTreeView1
            // 
            _userControlTreeView1.IdColumnName = "CatId";
            _userControlTreeView1.Location = new System.Drawing.Point(299, 19);
            _userControlTreeView1.LookupTableName = null;
            _userControlTreeView1.MyQry = "select * FROM Table_LookupCategories WHERE ParentCatId=\'-1\'";
            _userControlTreeView1.Name = "_userControlTreeView1";
            _userControlTreeView1.ParentIdColumnName = null;
            _userControlTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _userControlTreeView1.RootNodeId = "1";
            _userControlTreeView1.RootNodeName = "עמוד ראשי";
            _userControlTreeView1.Size = new System.Drawing.Size(342, 407);
            _userControlTreeView1.TabIndex = 38;
            _userControlTreeView1.TextColumnName = "CatHebrewName";
            // 
            // _buttonManageCategories
            // 
            _buttonManageCategories.Location = new System.Drawing.Point(195, 61);
            _buttonManageCategories.Name = "_buttonManageCategories";
            _buttonManageCategories.Size = new System.Drawing.Size(66, 40);
            _buttonManageCategories.TabIndex = 37;
            _buttonManageCategories.Text = "ניהול קטגוריות";
            _buttonManageCategories.UseVisualStyleBackColor = true;
            _buttonManageCategories.Click += new System.EventHandler(buttonManageCategories_Cilck);
            // 
            // _buttonReloadCategoryTree
            // 
            _buttonReloadCategoryTree.Location = new System.Drawing.Point(195, 32);
            _buttonReloadCategoryTree.Name = "_buttonReloadCategoryTree";
            _buttonReloadCategoryTree.Size = new System.Drawing.Size(66, 23);
            _buttonReloadCategoryTree.TabIndex = 36;
            _buttonReloadCategoryTree.Text = "עדכן עץ קטגוריות";
            _buttonReloadCategoryTree.UseVisualStyleBackColor = true;
            _buttonReloadCategoryTree.Click += new System.EventHandler(buttonReloadCategoryTree_Click);
            // 
            // _buttonAddAllCategories
            // 
            _buttonAddAllCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _buttonAddAllCategories.Location = new System.Drawing.Point(195, 359);
            _buttonAddAllCategories.Name = "_buttonAddAllCategories";
            _buttonAddAllCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _buttonAddAllCategories.Size = new System.Drawing.Size(57, 42);
            _buttonAddAllCategories.TabIndex = 34;
            _buttonAddAllCategories.Text = "<< <<";
            _buttonAddAllCategories.UseVisualStyleBackColor = true;
            _buttonAddAllCategories.Click += new System.EventHandler(buttonAddAllCategories_Click);
            // 
            // _buttonClearCategoriesList
            // 
            _buttonClearCategoriesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _buttonClearCategoriesList.Location = new System.Drawing.Point(195, 154);
            _buttonClearCategoriesList.Name = "_buttonClearCategoriesList";
            _buttonClearCategoriesList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _buttonClearCategoriesList.Size = new System.Drawing.Size(57, 35);
            _buttonClearCategoriesList.TabIndex = 33;
            _buttonClearCategoriesList.Text = ">> >>";
            _buttonClearCategoriesList.UseVisualStyleBackColor = true;
            _buttonClearCategoriesList.Click += new System.EventHandler(buttonClearCategoriesList_Click);
            // 
            // _listBoxSelectedCategories
            // 
            _listBoxSelectedCategories.FormattingEnabled = true;
            _listBoxSelectedCategories.Location = new System.Drawing.Point(6, 19);
            _listBoxSelectedCategories.Name = "_listBoxSelectedCategories";
            _listBoxSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _listBoxSelectedCategories.ScrollAlwaysVisible = true;
            _listBoxSelectedCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            _listBoxSelectedCategories.Size = new System.Drawing.Size(155, 407);
            _listBoxSelectedCategories.TabIndex = 30;
            // 
            // _buttonAddSelectedCategories
            // 
            _buttonAddSelectedCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _buttonAddSelectedCategories.Location = new System.Drawing.Point(195, 304);
            _buttonAddSelectedCategories.Name = "_buttonAddSelectedCategories";
            _buttonAddSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _buttonAddSelectedCategories.Size = new System.Drawing.Size(57, 40);
            _buttonAddSelectedCategories.TabIndex = 32;
            _buttonAddSelectedCategories.Text = "<< +";
            _buttonAddSelectedCategories.UseVisualStyleBackColor = true;
            _buttonAddSelectedCategories.Click += new System.EventHandler(buttonAddSelectedCategories_Click);
            // 
            // _buttonRemoveSelectedCategory
            // 
            _buttonRemoveSelectedCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _buttonRemoveSelectedCategory.Location = new System.Drawing.Point(195, 204);
            _buttonRemoveSelectedCategory.Name = "_buttonRemoveSelectedCategory";
            _buttonRemoveSelectedCategory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _buttonRemoveSelectedCategory.Size = new System.Drawing.Size(57, 38);
            _buttonRemoveSelectedCategory.TabIndex = 31;
            _buttonRemoveSelectedCategory.Text = "-- >>";
            _buttonRemoveSelectedCategory.UseVisualStyleBackColor = true;
            _buttonRemoveSelectedCategory.Click += new System.EventHandler(buttonRemoveSelectedCategory_Click);
            // 
            // _tabPagePhotos
            // 
            _tabPagePhotos.Controls.Add(_ucUploadPhoto1);
            _tabPagePhotos.Location = new System.Drawing.Point(4, 22);
            _tabPagePhotos.Name = "_tabPagePhotos";
            _tabPagePhotos.Padding = new System.Windows.Forms.Padding(3);
            _tabPagePhotos.Size = new System.Drawing.Size(705, 459);
            _tabPagePhotos.TabIndex = 7;
            _tabPagePhotos.Text = "תמונות";
            _tabPagePhotos.UseVisualStyleBackColor = true;
            // 
            // _ucUploadPhoto1
            // 
            _ucUploadPhoto1.Location = new System.Drawing.Point(32, 15);
            _ucUploadPhoto1.Name = "_ucUploadPhoto1";
            _ucUploadPhoto1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _ucUploadPhoto1.Size = new System.Drawing.Size(640, 429);
            _ucUploadPhoto1.TabIndex = 0;
            // 
            // _tabPageVideo
            // 
            _tabPageVideo.Controls.Add(_ucUploadVideo1);
            _tabPageVideo.Location = new System.Drawing.Point(4, 22);
            _tabPageVideo.Name = "_tabPageVideo";
            _tabPageVideo.Padding = new System.Windows.Forms.Padding(3);
            _tabPageVideo.Size = new System.Drawing.Size(705, 459);
            _tabPageVideo.TabIndex = 8;
            _tabPageVideo.Text = "ווידאו";
            _tabPageVideo.UseVisualStyleBackColor = true;
            // 
            // ucUploadVideo1
            // 
            _ucUploadVideo1.Location = new System.Drawing.Point(52, 29);
            _ucUploadVideo1.Name = "ucUploadVideo1";
            _ucUploadVideo1.Size = new System.Drawing.Size(601, 398);
            _ucUploadVideo1.TabIndex = 0;
            _ucUploadVideo1.Load += new System.EventHandler(userControlUploadVideo1_Load);
            // 
            // _tabPageAutoPublish
            // 
            _tabPageAutoPublish.Location = new System.Drawing.Point(4, 22);
            _tabPageAutoPublish.Name = "_tabPageAutoPublish";
            _tabPageAutoPublish.Padding = new System.Windows.Forms.Padding(3);
            _tabPageAutoPublish.Size = new System.Drawing.Size(705, 459);
            _tabPageAutoPublish.TabIndex = 9;
            _tabPageAutoPublish.Text = "שידורים אוטו\'";
            _tabPageAutoPublish.UseVisualStyleBackColor = true;
            // 
            // _buttonArticlePreview
            // 
            _buttonArticlePreview.Location = new System.Drawing.Point(159, 531);
            _buttonArticlePreview.Name = "_buttonArticlePreview";
            _buttonArticlePreview.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            _buttonArticlePreview.Size = new System.Drawing.Size(80, 44);
            _buttonArticlePreview.TabIndex = 26;
            _buttonArticlePreview.Text = "הצג בדפדפן";
            _buttonArticlePreview.UseVisualStyleBackColor = true;
            _buttonArticlePreview.Click += new System.EventHandler(buttonArticlePreview_Click);
            // 
            // _contextMenuStripTreeNode
            // 
            _contextMenuStripTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _toolStripMenuItemAddCategory,
            _toolStripMenuItemDeleteCategory,
            _toolStripMenuItemUpdateCategory});
            _contextMenuStripTreeNode.Name = "contextMenuStripTreeNode";
            _contextMenuStripTreeNode.Size = new System.Drawing.Size(147, 70);
            // 
            // _toolStripMenuItemAddCategory
            // 
            _toolStripMenuItemAddCategory.Name = "_toolStripMenuItemAddCategory";
            _toolStripMenuItemAddCategory.Size = new System.Drawing.Size(146, 22);
            _toolStripMenuItemAddCategory.Text = "הוסף קטגוריה";
            _toolStripMenuItemAddCategory.Click += new System.EventHandler(ToolStripMenuItemAddCategory_Click);
            // 
            // _toolStripMenuItemDeleteCategory
            // 
            _toolStripMenuItemDeleteCategory.Name = "_toolStripMenuItemDeleteCategory";
            _toolStripMenuItemDeleteCategory.Size = new System.Drawing.Size(146, 22);
            _toolStripMenuItemDeleteCategory.Text = "מחק קטגוריה";
            _toolStripMenuItemDeleteCategory.Click += new System.EventHandler(ToolStripMenuItemDeleteCategory_Click);
            // 
            // _toolStripMenuItemUpdateCategory
            // 
            _toolStripMenuItemUpdateCategory.Name = "_toolStripMenuItemUpdateCategory";
            _toolStripMenuItemUpdateCategory.Size = new System.Drawing.Size(146, 22);
            _toolStripMenuItemUpdateCategory.Text = "עדכן טקסט";
            // 
            // _tableLookupReportersBindingSource
            // 
            _tableLookupReportersBindingSource.DataMember = "Table_LookupReporters";
            _tableLookupReportersBindingSource.DataSource = _kanNaimDataSetReporters;
            // 
            // _kanNaimDataSetReporters
            // 
            _kanNaimDataSetReporters.DataSetName = "_Kan_NaimDataSetReporters";
            _kanNaimDataSetReporters.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _buttonSaveArticle
            // 
            _buttonSaveArticle.Location = new System.Drawing.Point(310, 531);
            _buttonSaveArticle.Name = "_buttonSaveArticle";
            _buttonSaveArticle.Size = new System.Drawing.Size(132, 44);
            _buttonSaveArticle.TabIndex = 7;
            _buttonSaveArticle.Text = "שמור כתבה";
            _buttonSaveArticle.UseVisualStyleBackColor = true;
            _buttonSaveArticle.Click += new System.EventHandler(buttonSaveArticle_Click);
            // 
            // _kanNaimDataSet
            // 
            _kanNaimDataSet.DataSetName = "_Kan_NaimDataSet";
            _kanNaimDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _tableLookupArticleStatusBindingSource
            // 
            _tableLookupArticleStatusBindingSource.DataMember = "Table_LookupArticleStatus";
            _tableLookupArticleStatusBindingSource.DataSource = _kanNaimDataSet;
            // 
            // _tableLookupArticleStatusTableAdapter
            // 
            _tableLookupArticleStatusTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupReportersTableAdapter
            // 
            _tableLookupReportersTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupCategoriesTableAdapter
            // 
            _tableLookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // _tableLookupReportersTableAdapter1
            // 
            _tableLookupReportersTableAdapter1.ClearBeforeFill = true;
            // 
            // _tablePhotosArchiveTableAdapter
            // 
            _tablePhotosArchiveTableAdapter.ClearBeforeFill = true;
            // 
            // _kanNaimDataSet2
            // 
            _kanNaimDataSet2.DataSetName = "_Kan_NaimDataSet2";
            _kanNaimDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // _spGetAllPhotosByOriginIdBindingSource
            // 
            _spGetAllPhotosByOriginIdBindingSource.DataMember = "sp_GetAllPhotosByOriginId";
            _spGetAllPhotosByOriginIdBindingSource.DataSource = _kanNaimDataSet2;
            // 
            // _spGetAllPhotosByOriginIdTableAdapter
            // 
            _spGetAllPhotosByOriginIdTableAdapter.ClearBeforeFill = true;
            // 
            // _groupBox1
            // 
            _groupBox1.Controls.Add(_radioButtonSaveAsPrivate);
            _groupBox1.Controls.Add(_radioButtonSaveAsPublic);
            _groupBox1.Enabled = false;
            _groupBox1.Location = new System.Drawing.Point(457, 525);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new System.Drawing.Size(249, 50);
            _groupBox1.TabIndex = 8;
            _groupBox1.TabStop = false;
            _groupBox1.Text = "בחר שמירה לארכיון ציבורי או פרטי לעריכה";
            _groupBox1.Visible = false;
            // 
            // _radioButtonSaveAsPrivate
            // 
            _radioButtonSaveAsPrivate.AutoSize = true;
            _radioButtonSaveAsPrivate.Location = new System.Drawing.Point(53, 19);
            _radioButtonSaveAsPrivate.Name = "_radioButtonSaveAsPrivate";
            _radioButtonSaveAsPrivate.Size = new System.Drawing.Size(50, 17);
            _radioButtonSaveAsPrivate.TabIndex = 1;
            _radioButtonSaveAsPrivate.Text = "פרטי";
            _radioButtonSaveAsPrivate.UseVisualStyleBackColor = true;
            // 
            // _radioButtonSaveAsPublic
            // 
            _radioButtonSaveAsPublic.AutoSize = true;
            _radioButtonSaveAsPublic.Checked = true;
            _radioButtonSaveAsPublic.Location = new System.Drawing.Point(133, 19);
            _radioButtonSaveAsPublic.Name = "_radioButtonSaveAsPublic";
            _radioButtonSaveAsPublic.Size = new System.Drawing.Size(60, 17);
            _radioButtonSaveAsPublic.TabIndex = 0;
            _radioButtonSaveAsPublic.TabStop = true;
            _radioButtonSaveAsPublic.Text = "ציבורי";
            _radioButtonSaveAsPublic.UseVisualStyleBackColor = true;
            // 
            // FormEditArtical
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(781, 604);
            Controls.Add(_groupBox1);
            Controls.Add(_buttonSaveArticle);
            Controls.Add(_tabControl1);
            Controls.Add(_buttonArticlePreview);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormEditArtical";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "עריכת כתבה";
            Load += new System.EventHandler(FormEditArtical_Load);
            _tabControl1.ResumeLayout(false);
            _tabPageArticle.ResumeLayout(false);
            _groupBox8.ResumeLayout(false);
            _groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSetCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_tablePhotosArchiveBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupReportersBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSetReportersNames)).EndInit();
            _tabPageTak3X.ResumeLayout(false);
            _tabPageTak3X.PerformLayout();
            _tabPageTak2X.ResumeLayout(false);
            _tabPageTak2X.PerformLayout();
            _tabPageTak1X.ResumeLayout(false);
            _tabPageTak1X.PerformLayout();
            _tabPageTakMedium.ResumeLayout(false);
            _tabPageTakMedium.PerformLayout();
            _tabPageTakSmall.ResumeLayout(false);
            _tabPageTakSmall.PerformLayout();
            _tabPageCategories.ResumeLayout(false);
            _groupBox7.ResumeLayout(false);
            _tabPagePhotos.ResumeLayout(false);
            _tabPageVideo.ResumeLayout(false);
            _contextMenuStripTreeNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(_tableLookupReportersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSetReporters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_tableLookupArticleStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_kanNaimDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_spGetAllPhotosByOriginIdBindingSource)).EndInit();
            _groupBox1.ResumeLayout(false);
            _groupBox1.PerformLayout();
            ResumeLayout(false);

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
    }
}
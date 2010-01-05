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
            if (disposing && (Singleton._components != null))
            {
                Singleton._components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private static void InitializeComponent()
        {
            Singleton._components = new System.ComponentModel.Container();
            Singleton._tabControl1 = new System.Windows.Forms.TabControl();
            Singleton._tabPageArticle = new System.Windows.Forms.TabPage();
            Singleton._groupBox8 = new System.Windows.Forms.GroupBox();
            Singleton._richTextBoxArticleContent = new System.Windows.Forms.RichTextBox();
            Singleton._comboBoxVideoPos = new System.Windows.Forms.ComboBox();
            Singleton._comboBoxImgPos = new System.Windows.Forms.ComboBox();
            Singleton._buttonSubTitleH2 = new System.Windows.Forms.Button();
            Singleton._buttonTitleH1 = new System.Windows.Forms.Button();
            Singleton._buttonOpenEditor = new System.Windows.Forms.Button();
            Singleton._labelOriginPhotoId = new System.Windows.Forms.Label();
            Singleton._comboBoxArticleCategory = new System.Windows.Forms.ComboBox();
            Singleton._tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(Singleton._components);
            Singleton._kanNaimDataSetCategories = new Kan_Naim_Main._Kan_NaimDataSetCategories();
            Singleton._label22 = new System.Windows.Forms.Label();
            Singleton._buttonSearchVideosArchive = new System.Windows.Forms.Button();
            Singleton._buttonSearchPhotosArchive = new System.Windows.Forms.Button();
            Singleton._comboBoxArticlePhoto = new System.Windows.Forms.ComboBox();
            Singleton._tablePhotosArchiveBindingSource = new System.Windows.Forms.BindingSource(Singleton._components);
            Singleton._kanNaimDataSet1 = new Kan_Naim_Main._Kan_NaimDataSet1();
            Singleton._comboBoxArticleVideo = new System.Windows.Forms.ComboBox();
            Singleton._dateTimePicker22 = new System.Windows.Forms.DateTimePicker();
            Singleton._labelArtical = new System.Windows.Forms.Label();
            Singleton._textBoxKeyWords = new System.Windows.Forms.TextBox();
            Singleton._labelKeyWords = new System.Windows.Forms.Label();
            Singleton._textBoxTags = new System.Windows.Forms.TextBox();
            Singleton._labelTags = new System.Windows.Forms.Label();
            Singleton._labelEditor = new System.Windows.Forms.Label();
            Singleton._textBoxArticleTitle = new System.Windows.Forms.TextBox();
            Singleton._comboBoxEditor = new System.Windows.Forms.ComboBox();
            Singleton._tableLookupReportersBindingSource1 = new System.Windows.Forms.BindingSource(Singleton._components);
            Singleton._kanNaimDataSetReportersNames = new Kan_Naim_Main._Kan_NaimDataSetReportersNames();
            Singleton._dateTimePicker21 = new System.Windows.Forms.DateTimePicker();
            Singleton._checkBoxMivzak = new System.Windows.Forms.CheckBox();
            Singleton._checkBoxDateTime = new System.Windows.Forms.CheckBox();
            Singleton._checkBoxRss = new System.Windows.Forms.CheckBox();
            Singleton._checkBoxPublish = new System.Windows.Forms.CheckBox();
            Singleton._labelTitle = new System.Windows.Forms.Label();
            Singleton._labelSubtitle = new System.Windows.Forms.Label();
            Singleton._textBoxArticleSubtitle = new System.Windows.Forms.TextBox();
            Singleton._tabPageTak3X = new System.Windows.Forms.TabPage();
            Singleton._label5 = new System.Windows.Forms.Label();
            Singleton._userControlTakFillSizeX3 = new HaimDLL.UserControlTakFill();
            Singleton._tabPageTak2X = new System.Windows.Forms.TabPage();
            Singleton._label4 = new System.Windows.Forms.Label();
            Singleton._userControlTakFillSizeX2 = new HaimDLL.UserControlTakFill();
            Singleton._tabPageTak1X = new System.Windows.Forms.TabPage();
            Singleton._label3 = new System.Windows.Forms.Label();
            Singleton._userControlTakFillSizeX1 = new HaimDLL.UserControlTakFill();
            Singleton._tabPageTakMedium = new System.Windows.Forms.TabPage();
            Singleton._label2 = new System.Windows.Forms.Label();
            Singleton._userControlTakFillSizeMedium = new HaimDLL.UserControlTakFill();
            Singleton._tabPageTakSmall = new System.Windows.Forms.TabPage();
            Singleton._label1 = new System.Windows.Forms.Label();
            Singleton._userControlTakFillSizeSmall = new HaimDLL.UserControlTakFill();
            Singleton._tabPageCategories = new System.Windows.Forms.TabPage();
            Singleton._groupBox7 = new System.Windows.Forms.GroupBox();
            Singleton._userControlTreeView1 = new HaimDLL.UserControlTreeView();
            Singleton._buttonManageCategories = new System.Windows.Forms.Button();
            Singleton._buttonReloadCategoryTree = new System.Windows.Forms.Button();
            Singleton._buttonAddAllCategories = new System.Windows.Forms.Button();
            Singleton._buttonClearCategoriesList = new System.Windows.Forms.Button();
            Singleton._listBoxSelectedCategories = new System.Windows.Forms.ListBox();
            Singleton._buttonAddSelectedCategories = new System.Windows.Forms.Button();
            Singleton._buttonRemoveSelectedCategory = new System.Windows.Forms.Button();
            Singleton._tabPagePhotos = new System.Windows.Forms.TabPage();
            Singleton._ucUploadPhoto1 = new HaimDLL.UserControlUploadPhoto();
            Singleton._tabPageVideo = new System.Windows.Forms.TabPage();
            Singleton._ucUploadVideo1 = new HaimDLL.UserControlUploadVideo();
            Singleton._tabPageAutoPublish = new System.Windows.Forms.TabPage();
            Singleton._buttonArticlePreview = new System.Windows.Forms.Button();
            Singleton._contextMenuStripTreeNode = new System.Windows.Forms.ContextMenuStrip(Singleton._components);
            Singleton._toolStripMenuItemAddCategory = new System.Windows.Forms.ToolStripMenuItem();
            Singleton._toolStripMenuItemDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
            Singleton._toolStripMenuItemUpdateCategory = new System.Windows.Forms.ToolStripMenuItem();
            Singleton._tableLookupReportersBindingSource = new System.Windows.Forms.BindingSource(Singleton._components);
            Singleton._kanNaimDataSetReporters = new Kan_Naim_Main._Kan_NaimDataSetReporters();
            Singleton._buttonSaveArticle = new System.Windows.Forms.Button();
            Singleton._kanNaimDataSet = new Kan_Naim_Main._Kan_NaimDataSet();
            Singleton._tableLookupArticleStatusBindingSource = new System.Windows.Forms.BindingSource(Singleton._components);
            Singleton._tableLookupArticleStatusTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetTableAdapters.Table_LookupArticleStatusTableAdapter();
            Singleton._tableLookupReportersTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetReportersTableAdapters.Table_LookupReportersTableAdapter();
            Singleton._tableLookupCategoriesTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            Singleton._tableLookupReportersTableAdapter1 = new Kan_Naim_Main._Kan_NaimDataSetReportersNamesTableAdapters.Table_LookupReportersTableAdapter();
            Singleton._tablePhotosArchiveTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet1TableAdapters.Table_PhotosArchiveTableAdapter();
            Singleton._kanNaimDataSet2 = new Kan_Naim_Main._Kan_NaimDataSet2();
            Singleton._spGetAllPhotosByOriginIdBindingSource = new System.Windows.Forms.BindingSource(Singleton._components);
            Singleton._spGetAllPhotosByOriginIdTableAdapter = new Kan_Naim_Main._Kan_NaimDataSet2TableAdapters.sp_GetAllPhotosByOriginIdTableAdapter();
            Singleton._groupBox1 = new System.Windows.Forms.GroupBox();
            Singleton._radioButtonSaveAsPrivate = new System.Windows.Forms.RadioButton();
            Singleton._radioButtonSaveAsPublic = new System.Windows.Forms.RadioButton();
            Singleton._tabControl1.SuspendLayout();
            Singleton._tabPageArticle.SuspendLayout();
            Singleton._groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSetCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tablePhotosArchiveBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupReportersBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSetReportersNames)).BeginInit();
            Singleton._tabPageTak3X.SuspendLayout();
            Singleton._tabPageTak2X.SuspendLayout();
            Singleton._tabPageTak1X.SuspendLayout();
            Singleton._tabPageTakMedium.SuspendLayout();
            Singleton._tabPageTakSmall.SuspendLayout();
            Singleton._tabPageCategories.SuspendLayout();
            Singleton._groupBox7.SuspendLayout();
            Singleton._tabPagePhotos.SuspendLayout();
            Singleton._tabPageVideo.SuspendLayout();
            Singleton._contextMenuStripTreeNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupReportersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSetReporters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupArticleStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._spGetAllPhotosByOriginIdBindingSource)).BeginInit();
            Singleton._groupBox1.SuspendLayout();
            Singleton.SuspendLayout();
            // 
            // Singleton._tabControl1
            // 
            Singleton._tabControl1.Controls.Add(Singleton._tabPageArticle);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageTak3X);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageTak2X);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageTak1X);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageTakMedium);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageTakSmall);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageCategories);
            Singleton._tabControl1.Controls.Add(Singleton._tabPagePhotos);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageVideo);
            Singleton._tabControl1.Controls.Add(Singleton._tabPageAutoPublish);
            Singleton._tabControl1.HotTrack = true;
            Singleton._tabControl1.Location = new System.Drawing.Point(12, 27);
            Singleton._tabControl1.Name = "Singleton._tabControl1";
            Singleton._tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._tabControl1.RightToLeftLayout = true;
            Singleton._tabControl1.SelectedIndex = 0;
            Singleton._tabControl1.Size = new System.Drawing.Size(713, 485);
            Singleton._tabControl1.TabIndex = 4;
            // 
            // Singleton._tabPageArticle
            // 
            Singleton._tabPageArticle.Controls.Add(Singleton._groupBox8);
            Singleton._tabPageArticle.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageArticle.Name = "Singleton._tabPageArticle";
            Singleton._tabPageArticle.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageArticle.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageArticle.TabIndex = 0;
            Singleton._tabPageArticle.Text = "כתבה";
            Singleton._tabPageArticle.UseVisualStyleBackColor = true;
            // 
            // Singleton._groupBox8
            // 
            Singleton._groupBox8.Controls.Add(Singleton._richTextBoxArticleContent);
            Singleton._groupBox8.Controls.Add(Singleton._comboBoxVideoPos);
            Singleton._groupBox8.Controls.Add(Singleton._comboBoxImgPos);
            Singleton._groupBox8.Controls.Add(Singleton._buttonSubTitleH2);
            Singleton._groupBox8.Controls.Add(Singleton._buttonTitleH1);
            Singleton._groupBox8.Controls.Add(Singleton._buttonOpenEditor);
            Singleton._groupBox8.Controls.Add(Singleton._labelOriginPhotoId);
            Singleton._groupBox8.Controls.Add(Singleton._comboBoxArticleCategory);
            Singleton._groupBox8.Controls.Add(Singleton._label22);
            Singleton._groupBox8.Controls.Add(Singleton._buttonSearchVideosArchive);
            Singleton._groupBox8.Controls.Add(Singleton._buttonSearchPhotosArchive);
            Singleton._groupBox8.Controls.Add(Singleton._comboBoxArticlePhoto);
            Singleton._groupBox8.Controls.Add(Singleton._comboBoxArticleVideo);
            Singleton._groupBox8.Controls.Add(Singleton._dateTimePicker22);
            Singleton._groupBox8.Controls.Add(Singleton._labelArtical);
            Singleton._groupBox8.Controls.Add(Singleton._textBoxKeyWords);
            Singleton._groupBox8.Controls.Add(Singleton._labelKeyWords);
            Singleton._groupBox8.Controls.Add(Singleton._textBoxTags);
            Singleton._groupBox8.Controls.Add(Singleton._labelTags);
            Singleton._groupBox8.Controls.Add(Singleton._labelEditor);
            Singleton._groupBox8.Controls.Add(Singleton._textBoxArticleTitle);
            Singleton._groupBox8.Controls.Add(Singleton._comboBoxEditor);
            Singleton._groupBox8.Controls.Add(Singleton._dateTimePicker21);
            Singleton._groupBox8.Controls.Add(Singleton._checkBoxMivzak);
            Singleton._groupBox8.Controls.Add(Singleton._checkBoxDateTime);
            Singleton._groupBox8.Controls.Add(Singleton._checkBoxRss);
            Singleton._groupBox8.Controls.Add(Singleton._checkBoxPublish);
            Singleton._groupBox8.Controls.Add(Singleton._labelTitle);
            Singleton._groupBox8.Controls.Add(Singleton._labelSubtitle);
            Singleton._groupBox8.Controls.Add(Singleton._textBoxArticleSubtitle);
            Singleton._groupBox8.Location = new System.Drawing.Point(15, 6);
            Singleton._groupBox8.Name = "Singleton._groupBox8";
            Singleton._groupBox8.Size = new System.Drawing.Size(675, 447);
            Singleton._groupBox8.TabIndex = 4;
            Singleton._groupBox8.TabStop = false;
            Singleton._groupBox8.Text = "הזנת תוכן ומאפייני הכתבה";
            // 
            // Singleton._richTextBoxArticleContent
            // 
            Singleton._richTextBoxArticleContent.Enabled = false;
            Singleton._richTextBoxArticleContent.Location = new System.Drawing.Point(19, 264);
            Singleton._richTextBoxArticleContent.MaxLength = 10000;
            Singleton._richTextBoxArticleContent.Name = "Singleton._richTextBoxArticleContent";
            Singleton._richTextBoxArticleContent.Size = new System.Drawing.Size(541, 165);
            Singleton._richTextBoxArticleContent.TabIndex = 85;
            Singleton._richTextBoxArticleContent.Text = "";
            // 
            // Singleton._comboBoxVideoPos
            // 
            Singleton._comboBoxVideoPos.Enabled = false;
            Singleton._comboBoxVideoPos.FormattingEnabled = true;
            Singleton._comboBoxVideoPos.Items.AddRange(new object[] {
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
            Singleton._comboBoxVideoPos.Location = new System.Drawing.Point(19, 171);
            Singleton._comboBoxVideoPos.Name = "Singleton._comboBoxVideoPos";
            Singleton._comboBoxVideoPos.Size = new System.Drawing.Size(70, 21);
            Singleton._comboBoxVideoPos.TabIndex = 84;
            Singleton._comboBoxVideoPos.Text = "  מלמעלה";
            Singleton._comboBoxVideoPos.Visible = false;
            // 
            // Singleton._comboBoxImgPos
            // 
            Singleton._comboBoxImgPos.Enabled = false;
            Singleton._comboBoxImgPos.FormattingEnabled = true;
            Singleton._comboBoxImgPos.Items.AddRange(new object[] {
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
            Singleton._comboBoxImgPos.Location = new System.Drawing.Point(19, 144);
            Singleton._comboBoxImgPos.Name = "Singleton._comboBoxImgPos";
            Singleton._comboBoxImgPos.Size = new System.Drawing.Size(70, 21);
            Singleton._comboBoxImgPos.TabIndex = 83;
            Singleton._comboBoxImgPos.Text = "  מלמעלה";
            Singleton._comboBoxImgPos.Visible = false;
            // 
            // Singleton._buttonSubTitleH2
            // 
            Singleton._buttonSubTitleH2.Location = new System.Drawing.Point(573, 344);
            Singleton._buttonSubTitleH2.Name = "Singleton._buttonSubTitleH2";
            Singleton._buttonSubTitleH2.Size = new System.Drawing.Size(59, 23);
            Singleton._buttonSubTitleH2.TabIndex = 79;
            Singleton._buttonSubTitleH2.Text = "H2";
            Singleton._buttonSubTitleH2.UseVisualStyleBackColor = true;
            Singleton._buttonSubTitleH2.Click += new System.EventHandler(buttonTitlesH1andH2_Click);
            // 
            // Singleton._buttonTitleH1
            // 
            Singleton._buttonTitleH1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Singleton._buttonTitleH1.Location = new System.Drawing.Point(573, 315);
            Singleton._buttonTitleH1.Name = "Singleton._buttonTitleH1";
            Singleton._buttonTitleH1.Size = new System.Drawing.Size(59, 23);
            Singleton._buttonTitleH1.TabIndex = 78;
            Singleton._buttonTitleH1.Text = "H1";
            Singleton._buttonTitleH1.UseVisualStyleBackColor = true;
            Singleton._buttonTitleH1.Click += new System.EventHandler(buttonTitlesH1andH2_Click);
            // 
            // Singleton._buttonOpenEditor
            // 
            Singleton._buttonOpenEditor.Location = new System.Drawing.Point(573, 286);
            Singleton._buttonOpenEditor.Name = "Singleton._buttonOpenEditor";
            Singleton._buttonOpenEditor.Size = new System.Drawing.Size(59, 23);
            Singleton._buttonOpenEditor.TabIndex = 76;
            Singleton._buttonOpenEditor.Text = "עריכה";
            Singleton._buttonOpenEditor.UseVisualStyleBackColor = true;
            Singleton._buttonOpenEditor.Click += new System.EventHandler(buttonOpenEditor_Click);
            // 
            // Singleton._labelOriginPhotoId
            // 
            Singleton._labelOriginPhotoId.AutoSize = true;
            Singleton._labelOriginPhotoId.Location = new System.Drawing.Point(642, 279);
            Singleton._labelOriginPhotoId.Name = "Singleton._labelOriginPhotoId";
            Singleton._labelOriginPhotoId.Size = new System.Drawing.Size(13, 13);
            Singleton._labelOriginPhotoId.TabIndex = 62;
            Singleton._labelOriginPhotoId.Text = "0";
            Singleton._labelOriginPhotoId.Visible = false;
            // 
            // Singleton._comboBoxArticleCategory
            // 
            Singleton._comboBoxArticleCategory.DataSource = Singleton._tableLookupCategoriesBindingSource;
            Singleton._comboBoxArticleCategory.DisplayMember = "CatHebrewName";
            Singleton._comboBoxArticleCategory.FormattingEnabled = true;
            Singleton._comboBoxArticleCategory.Location = new System.Drawing.Point(19, 117);
            Singleton._comboBoxArticleCategory.Name = "Singleton._comboBoxArticleCategory";
            Singleton._comboBoxArticleCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._comboBoxArticleCategory.Size = new System.Drawing.Size(193, 21);
            Singleton._comboBoxArticleCategory.TabIndex = 61;
            Singleton._comboBoxArticleCategory.ValueMember = "CatId";
            Singleton._comboBoxArticleCategory.SelectedIndexChanged += new System.EventHandler(comboBoxArticleCategory_SelectedIndexChanged);
            // 
            // Singleton._tableLookupCategoriesBindingSource
            // 
            Singleton._tableLookupCategoriesBindingSource.DataMember = "TableSingleton._LookupCategories";
            Singleton._tableLookupCategoriesBindingSource.DataSource = Singleton._kanNaimDataSetCategories;
            // 
            // Singleton._kanNaimDataSetCategories
            // 
            Singleton._kanNaimDataSetCategories.DataSetName = "_Kan_NaimDataSetCategories";
            Singleton._kanNaimDataSetCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Singleton._label22
            // 
            Singleton._label22.AutoSize = true;
            Singleton._label22.Location = new System.Drawing.Point(218, 121);
            Singleton._label22.Name = "Singleton._label22";
            Singleton._label22.Size = new System.Drawing.Size(50, 13);
            Singleton._label22.TabIndex = 60;
            Singleton._label22.Text = "קטגוריה";
            // 
            // Singleton._buttonSearchVideosArchive
            // 
            Singleton._buttonSearchVideosArchive.Enabled = false;
            Singleton._buttonSearchVideosArchive.Location = new System.Drawing.Point(577, 169);
            Singleton._buttonSearchVideosArchive.Name = "Singleton._buttonSearchVideosArchive";
            Singleton._buttonSearchVideosArchive.Size = new System.Drawing.Size(75, 23);
            Singleton._buttonSearchVideosArchive.TabIndex = 59;
            Singleton._buttonSearchVideosArchive.Text = "וידאו..";
            Singleton._buttonSearchVideosArchive.UseVisualStyleBackColor = true;
            // 
            // Singleton._buttonSearchPhotosArchive
            // 
            Singleton._buttonSearchPhotosArchive.Enabled = false;
            Singleton._buttonSearchPhotosArchive.Location = new System.Drawing.Point(577, 142);
            Singleton._buttonSearchPhotosArchive.Name = "Singleton._buttonSearchPhotosArchive";
            Singleton._buttonSearchPhotosArchive.Size = new System.Drawing.Size(75, 23);
            Singleton._buttonSearchPhotosArchive.TabIndex = 58;
            Singleton._buttonSearchPhotosArchive.Text = "תמונה..";
            Singleton._buttonSearchPhotosArchive.UseVisualStyleBackColor = true;
            // 
            // Singleton._comboBoxArticlePhoto
            // 
            Singleton._comboBoxArticlePhoto.DataSource = Singleton._tablePhotosArchiveBindingSource;
            Singleton._comboBoxArticlePhoto.DisplayMember = "ImageUrl";
            Singleton._comboBoxArticlePhoto.FormattingEnabled = true;
            Singleton._comboBoxArticlePhoto.Location = new System.Drawing.Point(95, 144);
            Singleton._comboBoxArticlePhoto.Name = "Singleton._comboBoxArticlePhoto";
            Singleton._comboBoxArticlePhoto.Size = new System.Drawing.Size(465, 21);
            Singleton._comboBoxArticlePhoto.TabIndex = 57;
            Singleton._comboBoxArticlePhoto.ValueMember = "Id";
            Singleton._comboBoxArticlePhoto.SelectedIndexChanged += new System.EventHandler(comboBoxArticlePhoto_SelectedIndexChanged);
            // 
            // Singleton._tablePhotosArchiveBindingSource
            // 
            Singleton._tablePhotosArchiveBindingSource.DataMember = "TableSingleton._PhotosArchive";
            Singleton._tablePhotosArchiveBindingSource.DataSource = Singleton._kanNaimDataSet1;
            // 
            // Singleton._kanNaimDataSet1
            // 
            Singleton._kanNaimDataSet1.DataSetName = "_Kan_NaimDataSet1";
            Singleton._kanNaimDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Singleton._comboBoxArticleVideo
            // 
            Singleton._comboBoxArticleVideo.Enabled = false;
            Singleton._comboBoxArticleVideo.FormattingEnabled = true;
            Singleton._comboBoxArticleVideo.Location = new System.Drawing.Point(95, 171);
            Singleton._comboBoxArticleVideo.Name = "Singleton._comboBoxArticleVideo";
            Singleton._comboBoxArticleVideo.Size = new System.Drawing.Size(465, 21);
            Singleton._comboBoxArticleVideo.TabIndex = 56;
            Singleton._comboBoxArticleVideo.SelectedIndexChanged += new System.EventHandler(comboBoxArticleVideo_SelectedIndexChanged);
            // 
            // Singleton._dateTimePicker22
            // 
            Singleton._dateTimePicker22.Enabled = false;
            Singleton._dateTimePicker22.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            Singleton._dateTimePicker22.Location = new System.Drawing.Point(19, 91);
            Singleton._dateTimePicker22.Name = "Singleton._dateTimePicker22";
            Singleton._dateTimePicker22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._dateTimePicker22.RightToLeftLayout = true;
            Singleton._dateTimePicker22.Size = new System.Drawing.Size(90, 20);
            Singleton._dateTimePicker22.TabIndex = 34;
            // 
            // Singleton._labelArtical
            // 
            Singleton._labelArtical.AutoSize = true;
            Singleton._labelArtical.Location = new System.Drawing.Point(571, 264);
            Singleton._labelArtical.Name = "Singleton._labelArtical";
            Singleton._labelArtical.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._labelArtical.Size = new System.Drawing.Size(62, 13);
            Singleton._labelArtical.TabIndex = 24;
            Singleton._labelArtical.Text = "תוכן כתבה";
            // 
            // Singleton._textBoxKeyWords
            // 
            Singleton._textBoxKeyWords.Location = new System.Drawing.Point(19, 225);
            Singleton._textBoxKeyWords.MaxLength = 200;
            Singleton._textBoxKeyWords.Name = "Singleton._textBoxKeyWords";
            Singleton._textBoxKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._textBoxKeyWords.Size = new System.Drawing.Size(541, 20);
            Singleton._textBoxKeyWords.TabIndex = 23;
            Singleton._textBoxKeyWords.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Singleton._labelKeyWords
            // 
            Singleton._labelKeyWords.AutoSize = true;
            Singleton._labelKeyWords.Location = new System.Drawing.Point(577, 228);
            Singleton._labelKeyWords.Name = "Singleton._labelKeyWords";
            Singleton._labelKeyWords.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._labelKeyWords.Size = new System.Drawing.Size(74, 13);
            Singleton._labelKeyWords.TabIndex = 22;
            Singleton._labelKeyWords.Text = "מילות חיפוש";
            // 
            // Singleton._textBoxTags
            // 
            Singleton._textBoxTags.Location = new System.Drawing.Point(19, 199);
            Singleton._textBoxTags.MaxLength = 200;
            Singleton._textBoxTags.Name = "Singleton._textBoxTags";
            Singleton._textBoxTags.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._textBoxTags.Size = new System.Drawing.Size(541, 20);
            Singleton._textBoxTags.TabIndex = 21;
            Singleton._textBoxTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Singleton._labelTags
            // 
            Singleton._labelTags.AutoSize = true;
            Singleton._labelTags.Location = new System.Drawing.Point(577, 202);
            Singleton._labelTags.Name = "Singleton._labelTags";
            Singleton._labelTags.Size = new System.Drawing.Size(37, 13);
            Singleton._labelTags.TabIndex = 20;
            Singleton._labelTags.Text = "תגיות";
            // 
            // Singleton._labelEditor
            // 
            Singleton._labelEditor.AutoSize = true;
            Singleton._labelEditor.Location = new System.Drawing.Point(577, 121);
            Singleton._labelEditor.Name = "Singleton._labelEditor";
            Singleton._labelEditor.Size = new System.Drawing.Size(32, 13);
            Singleton._labelEditor.TabIndex = 18;
            Singleton._labelEditor.Text = "עורך";
            // 
            // Singleton._textBoxArticleTitle
            // 
            Singleton._textBoxArticleTitle.Location = new System.Drawing.Point(17, 16);
            Singleton._textBoxArticleTitle.MaxLength = 150;
            Singleton._textBoxArticleTitle.Name = "Singleton._textBoxArticleTitle";
            Singleton._textBoxArticleTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._textBoxArticleTitle.Size = new System.Drawing.Size(541, 20);
            Singleton._textBoxArticleTitle.TabIndex = 15;
            Singleton._textBoxArticleTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Singleton._comboBoxEditor
            // 
            Singleton._comboBoxEditor.DataSource = Singleton._tableLookupReportersBindingSource1;
            Singleton._comboBoxEditor.DisplayMember = "PublishNameShort";
            Singleton._comboBoxEditor.FormattingEnabled = true;
            Singleton._comboBoxEditor.Location = new System.Drawing.Point(291, 118);
            Singleton._comboBoxEditor.Name = "Singleton._comboBoxEditor";
            Singleton._comboBoxEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._comboBoxEditor.Size = new System.Drawing.Size(269, 21);
            Singleton._comboBoxEditor.TabIndex = 10;
            Singleton._comboBoxEditor.ValueMember = "UserId";
            // 
            // Singleton._tableLookupReportersBindingSource1
            // 
            Singleton._tableLookupReportersBindingSource1.DataMember = "TableSingleton._LookupReporters";
            Singleton._tableLookupReportersBindingSource1.DataSource = Singleton._kanNaimDataSetReportersNames;
            // 
            // Singleton._kanNaimDataSetReportersNames
            // 
            Singleton._kanNaimDataSetReportersNames.DataSetName = "_Kan_NaimDataSetReportersNames";
            Singleton._kanNaimDataSetReportersNames.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Singleton._dateTimePicker21
            // 
            Singleton._dateTimePicker21.Enabled = false;
            Singleton._dateTimePicker21.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            Singleton._dateTimePicker21.Location = new System.Drawing.Point(125, 91);
            Singleton._dateTimePicker21.MaxDate = new System.DateTime(2015, 12, 31, 0, 0, 0, 0);
            Singleton._dateTimePicker21.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            Singleton._dateTimePicker21.Name = "Singleton._dateTimePicker21";
            Singleton._dateTimePicker21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._dateTimePicker21.RightToLeftLayout = true;
            Singleton._dateTimePicker21.Size = new System.Drawing.Size(87, 20);
            Singleton._dateTimePicker21.TabIndex = 9;
            Singleton._dateTimePicker21.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // Singleton._checkBoxMivzak
            // 
            Singleton._checkBoxMivzak.AutoSize = true;
            Singleton._checkBoxMivzak.Checked = true;
            Singleton._checkBoxMivzak.CheckState = System.Windows.Forms.CheckState.Checked;
            Singleton._checkBoxMivzak.Location = new System.Drawing.Point(428, 94);
            Singleton._checkBoxMivzak.Name = "Singleton._checkBoxMivzak";
            Singleton._checkBoxMivzak.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._checkBoxMivzak.Size = new System.Drawing.Size(52, 17);
            Singleton._checkBoxMivzak.TabIndex = 7;
            Singleton._checkBoxMivzak.Tag = "";
            Singleton._checkBoxMivzak.Text = "מבזק";
            Singleton._checkBoxMivzak.UseVisualStyleBackColor = true;
            // 
            // Singleton._checkBoxDateTime
            // 
            Singleton._checkBoxDateTime.AutoSize = true;
            Singleton._checkBoxDateTime.Checked = true;
            Singleton._checkBoxDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            Singleton._checkBoxDateTime.Location = new System.Drawing.Point(231, 94);
            Singleton._checkBoxDateTime.Name = "Singleton._checkBoxDateTime";
            Singleton._checkBoxDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._checkBoxDateTime.Size = new System.Drawing.Size(90, 17);
            Singleton._checkBoxDateTime.TabIndex = 6;
            Singleton._checkBoxDateTime.Tag = "";
            Singleton._checkBoxDateTime.Text = "תאריך ושעה";
            Singleton._checkBoxDateTime.UseVisualStyleBackColor = true;
            // 
            // Singleton._checkBoxRss
            // 
            Singleton._checkBoxRss.AutoSize = true;
            Singleton._checkBoxRss.Checked = true;
            Singleton._checkBoxRss.CheckState = System.Windows.Forms.CheckState.Checked;
            Singleton._checkBoxRss.Location = new System.Drawing.Point(348, 94);
            Singleton._checkBoxRss.Name = "Singleton._checkBoxRss";
            Singleton._checkBoxRss.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._checkBoxRss.Size = new System.Drawing.Size(48, 17);
            Singleton._checkBoxRss.TabIndex = 5;
            Singleton._checkBoxRss.Tag = "";
            Singleton._checkBoxRss.Text = "RSS";
            Singleton._checkBoxRss.UseVisualStyleBackColor = true;
            // 
            // Singleton._checkBoxPublish
            // 
            Singleton._checkBoxPublish.AutoSize = true;
            Singleton._checkBoxPublish.Checked = true;
            Singleton._checkBoxPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            Singleton._checkBoxPublish.Location = new System.Drawing.Point(505, 95);
            Singleton._checkBoxPublish.Name = "Singleton._checkBoxPublish";
            Singleton._checkBoxPublish.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._checkBoxPublish.Size = new System.Drawing.Size(53, 17);
            Singleton._checkBoxPublish.TabIndex = 4;
            Singleton._checkBoxPublish.Tag = "";
            Singleton._checkBoxPublish.Text = "פרסם";
            Singleton._checkBoxPublish.UseVisualStyleBackColor = true;
            // 
            // Singleton._labelTitle
            // 
            Singleton._labelTitle.AutoSize = true;
            Singleton._labelTitle.Location = new System.Drawing.Point(571, 19);
            Singleton._labelTitle.Name = "Singleton._labelTitle";
            Singleton._labelTitle.Size = new System.Drawing.Size(77, 13);
            Singleton._labelTitle.TabIndex = 3;
            Singleton._labelTitle.Text = "כותרת ראשית";
            // 
            // Singleton._labelSubtitle
            // 
            Singleton._labelSubtitle.AutoSize = true;
            Singleton._labelSubtitle.Location = new System.Drawing.Point(571, 52);
            Singleton._labelSubtitle.Name = "Singleton._labelSubtitle";
            Singleton._labelSubtitle.Size = new System.Drawing.Size(75, 13);
            Singleton._labelSubtitle.TabIndex = 0;
            Singleton._labelSubtitle.Text = "כותרת משנית";
            // 
            // Singleton._textBoxArticleSubtitle
            // 
            Singleton._textBoxArticleSubtitle.Location = new System.Drawing.Point(17, 49);
            Singleton._textBoxArticleSubtitle.MaxLength = 300;
            Singleton._textBoxArticleSubtitle.Multiline = true;
            Singleton._textBoxArticleSubtitle.Name = "Singleton._textBoxArticleSubtitle";
            Singleton._textBoxArticleSubtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._textBoxArticleSubtitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            Singleton._textBoxArticleSubtitle.Size = new System.Drawing.Size(541, 36);
            Singleton._textBoxArticleSubtitle.TabIndex = 14;
            Singleton._textBoxArticleSubtitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Singleton._tabPageTak3X
            // 
            Singleton._tabPageTak3X.Controls.Add(Singleton._label5);
            Singleton._tabPageTak3X.Controls.Add(Singleton._userControlTakFillSizeX3);
            Singleton._tabPageTak3X.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageTak3X.Name = "Singleton._tabPageTak3X";
            Singleton._tabPageTak3X.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageTak3X.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageTak3X.TabIndex = 1;
            Singleton._tabPageTak3X.Text = "תקציר גדול X3";
            Singleton._tabPageTak3X.UseVisualStyleBackColor = true;
            // 
            // Singleton._label5
            // 
            Singleton._label5.AutoSize = true;
            Singleton._label5.ForeColor = System.Drawing.SystemColors.Highlight;
            Singleton._label5.Location = new System.Drawing.Point(644, 28);
            Singleton._label5.Name = "Singleton._label5";
            Singleton._label5.Size = new System.Drawing.Size(55, 13);
            Singleton._label5.TabIndex = 3;
            Singleton._label5.Text = "בגודל X3";
            // 
            // Singleton._userControlTakFillSizeX3
            // 
            Singleton._userControlTakFillSizeX3.Location = new System.Drawing.Point(6, 6);
            Singleton._userControlTakFillSizeX3.Name = "Singleton._userControlTakFillSizeX3";
            Singleton._userControlTakFillSizeX3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._userControlTakFillSizeX3.Size = new System.Drawing.Size(640, 445);
            Singleton._userControlTakFillSizeX3.TabIndex = 0;
            // 
            // Singleton._tabPageTak2X
            // 
            Singleton._tabPageTak2X.Controls.Add(Singleton._label4);
            Singleton._tabPageTak2X.Controls.Add(Singleton._userControlTakFillSizeX2);
            Singleton._tabPageTak2X.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageTak2X.Name = "Singleton._tabPageTak2X";
            Singleton._tabPageTak2X.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageTak2X.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageTak2X.TabIndex = 2;
            Singleton._tabPageTak2X.Text = "תקציר גדול X2";
            Singleton._tabPageTak2X.UseVisualStyleBackColor = true;
            // 
            // Singleton._label4
            // 
            Singleton._label4.AutoSize = true;
            Singleton._label4.ForeColor = System.Drawing.SystemColors.Highlight;
            Singleton._label4.Location = new System.Drawing.Point(647, 25);
            Singleton._label4.Name = "Singleton._label4";
            Singleton._label4.Size = new System.Drawing.Size(55, 13);
            Singleton._label4.TabIndex = 3;
            Singleton._label4.Text = "בגודל X2";
            // 
            // Singleton._userControlTakFillSizeX2
            // 
            Singleton._userControlTakFillSizeX2.Location = new System.Drawing.Point(6, 8);
            Singleton._userControlTakFillSizeX2.Name = "Singleton._userControlTakFillSizeX2";
            Singleton._userControlTakFillSizeX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._userControlTakFillSizeX2.Size = new System.Drawing.Size(635, 445);
            Singleton._userControlTakFillSizeX2.TabIndex = 0;
            // 
            // Singleton._tabPageTak1X
            // 
            Singleton._tabPageTak1X.Controls.Add(Singleton._label3);
            Singleton._tabPageTak1X.Controls.Add(Singleton._userControlTakFillSizeX1);
            Singleton._tabPageTak1X.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageTak1X.Name = "Singleton._tabPageTak1X";
            Singleton._tabPageTak1X.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageTak1X.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageTak1X.TabIndex = 3;
            Singleton._tabPageTak1X.Text = "תקציר גדול X1";
            Singleton._tabPageTak1X.UseVisualStyleBackColor = true;
            // 
            // Singleton._label3
            // 
            Singleton._label3.AutoSize = true;
            Singleton._label3.ForeColor = System.Drawing.SystemColors.Highlight;
            Singleton._label3.Location = new System.Drawing.Point(644, 18);
            Singleton._label3.Name = "Singleton._label3";
            Singleton._label3.Size = new System.Drawing.Size(55, 13);
            Singleton._label3.TabIndex = 2;
            Singleton._label3.Text = "בגודל X1";
            // 
            // Singleton._userControlTakFillSizeX1
            // 
            Singleton._userControlTakFillSizeX1.Location = new System.Drawing.Point(6, 3);
            Singleton._userControlTakFillSizeX1.Name = "Singleton._userControlTakFillSizeX1";
            Singleton._userControlTakFillSizeX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._userControlTakFillSizeX1.Size = new System.Drawing.Size(635, 445);
            Singleton._userControlTakFillSizeX1.TabIndex = 0;
            // 
            // Singleton._tabPageTakMedium
            // 
            Singleton._tabPageTakMedium.Controls.Add(Singleton._label2);
            Singleton._tabPageTakMedium.Controls.Add(Singleton._userControlTakFillSizeMedium);
            Singleton._tabPageTakMedium.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageTakMedium.Name = "Singleton._tabPageTakMedium";
            Singleton._tabPageTakMedium.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageTakMedium.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageTakMedium.TabIndex = 4;
            Singleton._tabPageTakMedium.Text = "תקציר בינוני";
            Singleton._tabPageTakMedium.UseVisualStyleBackColor = true;
            // 
            // Singleton._label2
            // 
            Singleton._label2.AutoSize = true;
            Singleton._label2.ForeColor = System.Drawing.SystemColors.Highlight;
            Singleton._label2.Location = new System.Drawing.Point(638, 21);
            Singleton._label2.Name = "Singleton._label2";
            Singleton._label2.Size = new System.Drawing.Size(74, 13);
            Singleton._label2.TabIndex = 2;
            Singleton._label2.Text = "בגודל בינוני";
            // 
            // Singleton._userControlTakFillSizeMedium
            // 
            Singleton._userControlTakFillSizeMedium.Location = new System.Drawing.Point(6, 6);
            Singleton._userControlTakFillSizeMedium.Name = "Singleton._userControlTakFillSizeMedium";
            Singleton._userControlTakFillSizeMedium.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._userControlTakFillSizeMedium.Size = new System.Drawing.Size(635, 445);
            Singleton._userControlTakFillSizeMedium.TabIndex = 0;
            // 
            // Singleton._tabPageTakSmall
            // 
            Singleton._tabPageTakSmall.Controls.Add(Singleton._label1);
            Singleton._tabPageTakSmall.Controls.Add(Singleton._userControlTakFillSizeSmall);
            Singleton._tabPageTakSmall.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageTakSmall.Name = "Singleton._tabPageTakSmall";
            Singleton._tabPageTakSmall.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageTakSmall.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageTakSmall.TabIndex = 5;
            Singleton._tabPageTakSmall.Text = "תקציר קטן";
            Singleton._tabPageTakSmall.UseVisualStyleBackColor = true;
            // 
            // Singleton._label1
            // 
            Singleton._label1.AutoSize = true;
            Singleton._label1.ForeColor = System.Drawing.SystemColors.Highlight;
            Singleton._label1.Location = new System.Drawing.Point(641, 25);
            Singleton._label1.Name = "Singleton._label1";
            Singleton._label1.Size = new System.Drawing.Size(61, 13);
            Singleton._label1.TabIndex = 1;
            Singleton._label1.Text = "בגודל קטן";
            // 
            // Singleton._userControlTakFillSizeSmall
            // 
            Singleton._userControlTakFillSizeSmall.Location = new System.Drawing.Point(6, 8);
            Singleton._userControlTakFillSizeSmall.Name = "Singleton._userControlTakFillSizeSmall";
            Singleton._userControlTakFillSizeSmall.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._userControlTakFillSizeSmall.Size = new System.Drawing.Size(635, 445);
            Singleton._userControlTakFillSizeSmall.TabIndex = 0;
            // 
            // Singleton._tabPageCategories
            // 
            Singleton._tabPageCategories.Controls.Add(Singleton._groupBox7);
            Singleton._tabPageCategories.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageCategories.Name = "Singleton._tabPageCategories";
            Singleton._tabPageCategories.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageCategories.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageCategories.TabIndex = 6;
            Singleton._tabPageCategories.Text = "קטגוריות";
            Singleton._tabPageCategories.UseVisualStyleBackColor = true;
            // 
            // Singleton._groupBox7
            // 
            Singleton._groupBox7.Controls.Add(Singleton._userControlTreeView1);
            Singleton._groupBox7.Controls.Add(Singleton._buttonManageCategories);
            Singleton._groupBox7.Controls.Add(Singleton._buttonReloadCategoryTree);
            Singleton._groupBox7.Controls.Add(Singleton._buttonAddAllCategories);
            Singleton._groupBox7.Controls.Add(Singleton._buttonClearCategoriesList);
            Singleton._groupBox7.Controls.Add(Singleton._listBoxSelectedCategories);
            Singleton._groupBox7.Controls.Add(Singleton._buttonAddSelectedCategories);
            Singleton._groupBox7.Controls.Add(Singleton._buttonRemoveSelectedCategory);
            Singleton._groupBox7.Location = new System.Drawing.Point(30, 19);
            Singleton._groupBox7.Name = "Singleton._groupBox7";
            Singleton._groupBox7.Size = new System.Drawing.Size(647, 434);
            Singleton._groupBox7.TabIndex = 39;
            Singleton._groupBox7.TabStop = false;
            Singleton._groupBox7.Text = "בחירה מרובה של קטגוריות הקשורות לכתבה";
            // 
            // Singleton._userControlTreeView1
            // 
            Singleton._userControlTreeView1.IdColumnName = "CatId";
            Singleton._userControlTreeView1.Location = new System.Drawing.Point(299, 19);
            Singleton._userControlTreeView1.LookupTableName = null;
            Singleton._userControlTreeView1.MyQry = "select * FROM TableSingleton._LookupCategories WHERE ParentCatId=\'-1\'";
            Singleton._userControlTreeView1.Name = "Singleton._userControlTreeView1";
            Singleton._userControlTreeView1.ParentIdColumnName = null;
            Singleton._userControlTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._userControlTreeView1.RootNodeId = "1";
            Singleton._userControlTreeView1.RootNodeName = "עמוד ראשי";
            Singleton._userControlTreeView1.Size = new System.Drawing.Size(342, 407);
            Singleton._userControlTreeView1.TabIndex = 38;
            Singleton._userControlTreeView1.TextColumnName = "CatHebrewName";
            // 
            // Singleton._buttonManageCategories
            // 
            Singleton._buttonManageCategories.Location = new System.Drawing.Point(195, 61);
            Singleton._buttonManageCategories.Name = "Singleton._buttonManageCategories";
            Singleton._buttonManageCategories.Size = new System.Drawing.Size(66, 40);
            Singleton._buttonManageCategories.TabIndex = 37;
            Singleton._buttonManageCategories.Text = "ניהול קטגוריות";
            Singleton._buttonManageCategories.UseVisualStyleBackColor = true;
            Singleton._buttonManageCategories.Click += new System.EventHandler(buttonManageCategories_Cilck);
            // 
            // Singleton._buttonReloadCategoryTree
            // 
            Singleton._buttonReloadCategoryTree.Location = new System.Drawing.Point(195, 32);
            Singleton._buttonReloadCategoryTree.Name = "Singleton._buttonReloadCategoryTree";
            Singleton._buttonReloadCategoryTree.Size = new System.Drawing.Size(66, 23);
            Singleton._buttonReloadCategoryTree.TabIndex = 36;
            Singleton._buttonReloadCategoryTree.Text = "עדכן עץ קטגוריות";
            Singleton._buttonReloadCategoryTree.UseVisualStyleBackColor = true;
            Singleton._buttonReloadCategoryTree.Click += new System.EventHandler(buttonReloadCategoryTree_Click);
            // 
            // Singleton._buttonAddAllCategories
            // 
            Singleton._buttonAddAllCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Singleton._buttonAddAllCategories.Location = new System.Drawing.Point(195, 359);
            Singleton._buttonAddAllCategories.Name = "Singleton._buttonAddAllCategories";
            Singleton._buttonAddAllCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            Singleton._buttonAddAllCategories.Size = new System.Drawing.Size(57, 42);
            Singleton._buttonAddAllCategories.TabIndex = 34;
            Singleton._buttonAddAllCategories.Text = "<< <<";
            Singleton._buttonAddAllCategories.UseVisualStyleBackColor = true;
            Singleton._buttonAddAllCategories.Click += new System.EventHandler(buttonAddAllCategories_Click);
            // 
            // Singleton._buttonClearCategoriesList
            // 
            Singleton._buttonClearCategoriesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Singleton._buttonClearCategoriesList.Location = new System.Drawing.Point(195, 154);
            Singleton._buttonClearCategoriesList.Name = "Singleton._buttonClearCategoriesList";
            Singleton._buttonClearCategoriesList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            Singleton._buttonClearCategoriesList.Size = new System.Drawing.Size(57, 35);
            Singleton._buttonClearCategoriesList.TabIndex = 33;
            Singleton._buttonClearCategoriesList.Text = ">> >>";
            Singleton._buttonClearCategoriesList.UseVisualStyleBackColor = true;
            Singleton._buttonClearCategoriesList.Click += new System.EventHandler(buttonClearCategoriesList_Click);
            // 
            // Singleton._listBoxSelectedCategories
            // 
            Singleton._listBoxSelectedCategories.FormattingEnabled = true;
            Singleton._listBoxSelectedCategories.Location = new System.Drawing.Point(6, 19);
            Singleton._listBoxSelectedCategories.Name = "Singleton._listBoxSelectedCategories";
            Singleton._listBoxSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._listBoxSelectedCategories.ScrollAlwaysVisible = true;
            Singleton._listBoxSelectedCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            Singleton._listBoxSelectedCategories.Size = new System.Drawing.Size(155, 407);
            Singleton._listBoxSelectedCategories.TabIndex = 30;
            // 
            // Singleton._buttonAddSelectedCategories
            // 
            Singleton._buttonAddSelectedCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Singleton._buttonAddSelectedCategories.Location = new System.Drawing.Point(195, 304);
            Singleton._buttonAddSelectedCategories.Name = "Singleton._buttonAddSelectedCategories";
            Singleton._buttonAddSelectedCategories.RightToLeft = System.Windows.Forms.RightToLeft.No;
            Singleton._buttonAddSelectedCategories.Size = new System.Drawing.Size(57, 40);
            Singleton._buttonAddSelectedCategories.TabIndex = 32;
            Singleton._buttonAddSelectedCategories.Text = "<< +";
            Singleton._buttonAddSelectedCategories.UseVisualStyleBackColor = true;
            Singleton._buttonAddSelectedCategories.Click += new System.EventHandler(buttonAddSelectedCategories_Click);
            // 
            // Singleton._buttonRemoveSelectedCategory
            // 
            Singleton._buttonRemoveSelectedCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Singleton._buttonRemoveSelectedCategory.Location = new System.Drawing.Point(195, 204);
            Singleton._buttonRemoveSelectedCategory.Name = "Singleton._buttonRemoveSelectedCategory";
            Singleton._buttonRemoveSelectedCategory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            Singleton._buttonRemoveSelectedCategory.Size = new System.Drawing.Size(57, 38);
            Singleton._buttonRemoveSelectedCategory.TabIndex = 31;
            Singleton._buttonRemoveSelectedCategory.Text = "-- >>";
            Singleton._buttonRemoveSelectedCategory.UseVisualStyleBackColor = true;
            Singleton._buttonRemoveSelectedCategory.Click += new System.EventHandler(buttonRemoveSelectedCategory_Click);
            // 
            // Singleton._tabPagePhotos
            // 
            Singleton._tabPagePhotos.Controls.Add(Singleton._ucUploadPhoto1);
            Singleton._tabPagePhotos.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPagePhotos.Name = "Singleton._tabPagePhotos";
            Singleton._tabPagePhotos.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPagePhotos.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPagePhotos.TabIndex = 7;
            Singleton._tabPagePhotos.Text = "תמונות";
            Singleton._tabPagePhotos.UseVisualStyleBackColor = true;
            // 
            // Singleton._ucUploadPhoto1
            // 
            Singleton._ucUploadPhoto1.Location = new System.Drawing.Point(32, 15);
            Singleton._ucUploadPhoto1.Name = "Singleton._ucUploadPhoto1";
            Singleton._ucUploadPhoto1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._ucUploadPhoto1.Size = new System.Drawing.Size(640, 429);
            Singleton._ucUploadPhoto1.TabIndex = 0;
            // 
            // Singleton._tabPageVideo
            // 
            Singleton._tabPageVideo.Controls.Add(Singleton._ucUploadVideo1);
            Singleton._tabPageVideo.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageVideo.Name = "Singleton._tabPageVideo";
            Singleton._tabPageVideo.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageVideo.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageVideo.TabIndex = 8;
            Singleton._tabPageVideo.Text = "ווידאו";
            Singleton._tabPageVideo.UseVisualStyleBackColor = true;
            // 
            // ucUploadVideo1
            // 
            Singleton._ucUploadVideo1.Location = new System.Drawing.Point(52, 29);
            Singleton._ucUploadVideo1.Name = "ucUploadVideo1";
            Singleton._ucUploadVideo1.Size = new System.Drawing.Size(601, 398);
            Singleton._ucUploadVideo1.TabIndex = 0;
            Singleton._ucUploadVideo1.Load += new System.EventHandler(userControlUploadVideo1_Load);
            // 
            // Singleton._tabPageAutoPublish
            // 
            Singleton._tabPageAutoPublish.Location = new System.Drawing.Point(4, 22);
            Singleton._tabPageAutoPublish.Name = "Singleton._tabPageAutoPublish";
            Singleton._tabPageAutoPublish.Padding = new System.Windows.Forms.Padding(3);
            Singleton._tabPageAutoPublish.Size = new System.Drawing.Size(705, 459);
            Singleton._tabPageAutoPublish.TabIndex = 9;
            Singleton._tabPageAutoPublish.Text = "שידורים אוטו\'";
            Singleton._tabPageAutoPublish.UseVisualStyleBackColor = true;
            // 
            // Singleton._buttonArticlePreview
            // 
            Singleton._buttonArticlePreview.Location = new System.Drawing.Point(159, 531);
            Singleton._buttonArticlePreview.Name = "Singleton._buttonArticlePreview";
            Singleton._buttonArticlePreview.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton._buttonArticlePreview.Size = new System.Drawing.Size(80, 44);
            Singleton._buttonArticlePreview.TabIndex = 26;
            Singleton._buttonArticlePreview.Text = "הצג בדפדפן";
            Singleton._buttonArticlePreview.UseVisualStyleBackColor = true;
            Singleton._buttonArticlePreview.Click += new System.EventHandler(buttonArticlePreview_Click);
            // 
            // Singleton._contextMenuStripTreeNode
            // 
            Singleton._contextMenuStripTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            Singleton._toolStripMenuItemAddCategory,
            Singleton._toolStripMenuItemDeleteCategory,
            Singleton._toolStripMenuItemUpdateCategory});
            Singleton._contextMenuStripTreeNode.Name = "contextMenuStripTreeNode";
            Singleton._contextMenuStripTreeNode.Size = new System.Drawing.Size(147, 70);
            // 
            // Singleton._toolStripMenuItemAddCategory
            // 
            Singleton._toolStripMenuItemAddCategory.Name = "Singleton._toolStripMenuItemAddCategory";
            Singleton._toolStripMenuItemAddCategory.Size = new System.Drawing.Size(146, 22);
            Singleton._toolStripMenuItemAddCategory.Text = "הוסף קטגוריה";
            Singleton._toolStripMenuItemAddCategory.Click += new System.EventHandler(ToolStripMenuItemAddCategory_Click);
            // 
            // Singleton._toolStripMenuItemDeleteCategory
            // 
            Singleton._toolStripMenuItemDeleteCategory.Name = "Singleton._toolStripMenuItemDeleteCategory";
            Singleton._toolStripMenuItemDeleteCategory.Size = new System.Drawing.Size(146, 22);
            Singleton._toolStripMenuItemDeleteCategory.Text = "מחק קטגוריה";
            Singleton._toolStripMenuItemDeleteCategory.Click += new System.EventHandler(ToolStripMenuItemDeleteCategory_Click);
            // 
            // Singleton._toolStripMenuItemUpdateCategory
            // 
            Singleton._toolStripMenuItemUpdateCategory.Name = "Singleton._toolStripMenuItemUpdateCategory";
            Singleton._toolStripMenuItemUpdateCategory.Size = new System.Drawing.Size(146, 22);
            Singleton._toolStripMenuItemUpdateCategory.Text = "עדכן טקסט";
            // 
            // Singleton._tableLookupReportersBindingSource
            // 
            Singleton._tableLookupReportersBindingSource.DataMember = "TableSingleton._LookupReporters";
            Singleton._tableLookupReportersBindingSource.DataSource = Singleton._kanNaimDataSetReporters;
            // 
            // Singleton._kanNaimDataSetReporters
            // 
            Singleton._kanNaimDataSetReporters.DataSetName = "_Kan_NaimDataSetReporters";
            Singleton._kanNaimDataSetReporters.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Singleton._buttonSaveArticle
            // 
            Singleton._buttonSaveArticle.Location = new System.Drawing.Point(310, 531);
            Singleton._buttonSaveArticle.Name = "Singleton._buttonSaveArticle";
            Singleton._buttonSaveArticle.Size = new System.Drawing.Size(132, 44);
            Singleton._buttonSaveArticle.TabIndex = 7;
            Singleton._buttonSaveArticle.Text = "שמור כתבה";
            Singleton._buttonSaveArticle.UseVisualStyleBackColor = true;
            Singleton._buttonSaveArticle.Click += new System.EventHandler(buttonSaveArticle_Click);
            // 
            // Singleton._kanNaimDataSet
            // 
            Singleton._kanNaimDataSet.DataSetName = "_Kan_NaimDataSet";
            Singleton._kanNaimDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Singleton._tableLookupArticleStatusBindingSource
            // 
            Singleton._tableLookupArticleStatusBindingSource.DataMember = "TableSingleton._LookupArticleStatus";
            Singleton._tableLookupArticleStatusBindingSource.DataSource = Singleton._kanNaimDataSet;
            // 
            // Singleton._tableLookupArticleStatusTableAdapter
            // 
            Singleton._tableLookupArticleStatusTableAdapter.ClearBeforeFill = true;
            // 
            // Singleton._tableLookupReportersTableAdapter
            // 
            Singleton._tableLookupReportersTableAdapter.ClearBeforeFill = true;
            // 
            // Singleton._tableLookupCategoriesTableAdapter
            // 
            Singleton._tableLookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // Singleton._tableLookupReportersTableAdapter1
            // 
            Singleton._tableLookupReportersTableAdapter1.ClearBeforeFill = true;
            // 
            // Singleton._tablePhotosArchiveTableAdapter
            // 
            Singleton._tablePhotosArchiveTableAdapter.ClearBeforeFill = true;
            // 
            // Singleton._kanNaimDataSet2
            // 
            Singleton._kanNaimDataSet2.DataSetName = "_Kan_NaimDataSet2";
            Singleton._kanNaimDataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Singleton._spGetAllPhotosByOriginIdBindingSource
            // 
            Singleton._spGetAllPhotosByOriginIdBindingSource.DataMember = "spSingleton._GetAllPhotosByOriginId";
            Singleton._spGetAllPhotosByOriginIdBindingSource.DataSource = Singleton._kanNaimDataSet2;
            // 
            // Singleton._spGetAllPhotosByOriginIdTableAdapter
            // 
            Singleton._spGetAllPhotosByOriginIdTableAdapter.ClearBeforeFill = true;
            // 
            // Singleton._groupBox1
            // 
            Singleton._groupBox1.Controls.Add(Singleton._radioButtonSaveAsPrivate);
            Singleton._groupBox1.Controls.Add(Singleton._radioButtonSaveAsPublic);
            Singleton._groupBox1.Enabled = false;
            Singleton._groupBox1.Location = new System.Drawing.Point(457, 525);
            Singleton._groupBox1.Name = "Singleton._groupBox1";
            Singleton._groupBox1.Size = new System.Drawing.Size(249, 50);
            Singleton._groupBox1.TabIndex = 8;
            Singleton._groupBox1.TabStop = false;
            Singleton._groupBox1.Text = "בחר שמירה לארכיון ציבורי או פרטי לעריכה";
            Singleton._groupBox1.Visible = false;
            // 
            // Singleton._radioButtonSaveAsPrivate
            // 
            Singleton._radioButtonSaveAsPrivate.AutoSize = true;
            Singleton._radioButtonSaveAsPrivate.Location = new System.Drawing.Point(53, 19);
            Singleton._radioButtonSaveAsPrivate.Name = "Singleton._radioButtonSaveAsPrivate";
            Singleton._radioButtonSaveAsPrivate.Size = new System.Drawing.Size(50, 17);
            Singleton._radioButtonSaveAsPrivate.TabIndex = 1;
            Singleton._radioButtonSaveAsPrivate.Text = "פרטי";
            Singleton._radioButtonSaveAsPrivate.UseVisualStyleBackColor = true;
            // 
            // Singleton._radioButtonSaveAsPublic
            // 
            Singleton._radioButtonSaveAsPublic.AutoSize = true;
            Singleton._radioButtonSaveAsPublic.Checked = true;
            Singleton._radioButtonSaveAsPublic.Location = new System.Drawing.Point(133, 19);
            Singleton._radioButtonSaveAsPublic.Name = "Singleton._radioButtonSaveAsPublic";
            Singleton._radioButtonSaveAsPublic.Size = new System.Drawing.Size(60, 17);
            Singleton._radioButtonSaveAsPublic.TabIndex = 0;
            Singleton._radioButtonSaveAsPublic.TabStop = true;
            Singleton._radioButtonSaveAsPublic.Text = "ציבורי";
            Singleton._radioButtonSaveAsPublic.UseVisualStyleBackColor = true;
            // 
            // FormEditArtical
            // 
            Singleton.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            Singleton.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Singleton.AutoSize = true;
            Singleton.ClientSize = new System.Drawing.Size(781, 604);
            Singleton.Controls.Add(Singleton._groupBox1);
            Singleton.Controls.Add(Singleton._buttonSaveArticle);
            Singleton.Controls.Add(Singleton._tabControl1);
            Singleton.Controls.Add(Singleton._buttonArticlePreview);
            Singleton.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Singleton.MaximizeBox = false;
            Singleton.Name = "FormEditArtical";
            Singleton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            Singleton.RightToLeftLayout = true;
            Singleton.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Singleton.Text = "עריכת כתבה";
            Singleton.Load += new System.EventHandler(FormEditArtical_Load);
            Singleton._tabControl1.ResumeLayout(false);
            Singleton._tabPageArticle.ResumeLayout(false);
            Singleton._groupBox8.ResumeLayout(false);
            Singleton._groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSetCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tablePhotosArchiveBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupReportersBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSetReportersNames)).EndInit();
            Singleton._tabPageTak3X.ResumeLayout(false);
            Singleton._tabPageTak3X.PerformLayout();
            Singleton._tabPageTak2X.ResumeLayout(false);
            Singleton._tabPageTak2X.PerformLayout();
            Singleton._tabPageTak1X.ResumeLayout(false);
            Singleton._tabPageTak1X.PerformLayout();
            Singleton._tabPageTakMedium.ResumeLayout(false);
            Singleton._tabPageTakMedium.PerformLayout();
            Singleton._tabPageTakSmall.ResumeLayout(false);
            Singleton._tabPageTakSmall.PerformLayout();
            Singleton._tabPageCategories.ResumeLayout(false);
            Singleton._groupBox7.ResumeLayout(false);
            Singleton._tabPagePhotos.ResumeLayout(false);
            Singleton._tabPageVideo.ResumeLayout(false);
            Singleton._contextMenuStripTreeNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupReportersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSetReporters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._tableLookupArticleStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._kanNaimDataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(Singleton._spGetAllPhotosByOriginIdBindingSource)).EndInit();
            Singleton._groupBox1.ResumeLayout(false);
            Singleton._groupBox1.PerformLayout();
            Singleton.ResumeLayout(false);

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
namespace Kan_Naim_Main
{
    partial class FormAddNewPreferedLink
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
            this.labelPhotoId = new System.Windows.Forms.Label();
            this.textBoxPhotoId = new System.Windows.Forms.TextBox();
            this.buttonSearchPhoto = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.textBoxArticleId = new System.Windows.Forms.TextBox();
            this.buttonSearchArticle = new System.Windows.Forms.Button();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.textBoxOrderPlace = new System.Windows.Forms.TextBox();
            this.labelOrderPlace = new System.Windows.Forms.Label();
            this.textBoxAlternativeText = new System.Windows.Forms.TextBox();
            this.labelAlternativeText = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClearForm = new System.Windows.Forms.Button();
            this.buttonOpenTableView = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.kanNaimDataSetCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._Kan_NaimDataSetCategories = new Kan_Naim_Main._Kan_NaimDataSetCategories();
            this.radioButtonUrl = new System.Windows.Forms.RadioButton();
            this.radioButtonArticleId = new System.Windows.Forms.RadioButton();
            this.table_LookupCategoriesTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kanNaimDataSetCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._Kan_NaimDataSetCategories)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPhotoId
            // 
            this.labelPhotoId.AutoSize = true;
            this.labelPhotoId.Location = new System.Drawing.Point(36, 37);
            this.labelPhotoId.Name = "labelPhotoId";
            this.labelPhotoId.Size = new System.Drawing.Size(68, 13);
            this.labelPhotoId.TabIndex = 0;
            this.labelPhotoId.Text = "מספר תמונה";
            // 
            // textBoxPhotoId
            // 
            this.textBoxPhotoId.Location = new System.Drawing.Point(122, 34);
            this.textBoxPhotoId.Name = "textBoxPhotoId";
            this.textBoxPhotoId.Size = new System.Drawing.Size(100, 20);
            this.textBoxPhotoId.TabIndex = 1;
            // 
            // buttonSearchPhoto
            // 
            this.buttonSearchPhoto.Location = new System.Drawing.Point(238, 32);
            this.buttonSearchPhoto.Name = "buttonSearchPhoto";
            this.buttonSearchPhoto.Size = new System.Drawing.Size(94, 23);
            this.buttonSearchPhoto.TabIndex = 2;
            this.buttonSearchPhoto.Text = "חפש תמונה...";
            this.buttonSearchPhoto.UseVisualStyleBackColor = true;
            this.buttonSearchPhoto.Click += new System.EventHandler(this.buttonSearchPhoto_Click);
            // 
            // fontDialog1
            // 
            this.fontDialog1.AllowVerticalFonts = false;
            this.fontDialog1.FontMustExist = true;
            this.fontDialog1.ShowApply = true;
            this.fontDialog1.ShowColor = true;
            // 
            // textBoxArticleId
            // 
            this.textBoxArticleId.Location = new System.Drawing.Point(340, 18);
            this.textBoxArticleId.Name = "textBoxArticleId";
            this.textBoxArticleId.Size = new System.Drawing.Size(100, 20);
            this.textBoxArticleId.TabIndex = 4;
            // 
            // buttonSearchArticle
            // 
            this.buttonSearchArticle.Enabled = false;
            this.buttonSearchArticle.Location = new System.Drawing.Point(190, 16);
            this.buttonSearchArticle.Name = "buttonSearchArticle";
            this.buttonSearchArticle.Size = new System.Drawing.Size(137, 23);
            this.buttonSearchArticle.TabIndex = 5;
            this.buttonSearchArticle.Text = "חפש מאמר בקטגוריה...";
            this.buttonSearchArticle.UseVisualStyleBackColor = true;
            this.buttonSearchArticle.Click += new System.EventHandler(this.buttonSearchArticle_Click);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Enabled = false;
            this.textBoxUrl.Location = new System.Drawing.Point(13, 44);
            this.textBoxUrl.Multiline = true;
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(430, 61);
            this.textBoxUrl.TabIndex = 7;
            // 
            // textBoxOrderPlace
            // 
            this.textBoxOrderPlace.Location = new System.Drawing.Point(125, 183);
            this.textBoxOrderPlace.Name = "textBoxOrderPlace";
            this.textBoxOrderPlace.Size = new System.Drawing.Size(100, 20);
            this.textBoxOrderPlace.TabIndex = 9;
            // 
            // labelOrderPlace
            // 
            this.labelOrderPlace.AutoSize = true;
            this.labelOrderPlace.Location = new System.Drawing.Point(36, 186);
            this.labelOrderPlace.Name = "labelOrderPlace";
            this.labelOrderPlace.Size = new System.Drawing.Size(68, 13);
            this.labelOrderPlace.TabIndex = 8;
            this.labelOrderPlace.Text = "מספר מיקום";
            // 
            // textBoxAlternativeText
            // 
            this.textBoxAlternativeText.Location = new System.Drawing.Point(125, 218);
            this.textBoxAlternativeText.Name = "textBoxAlternativeText";
            this.textBoxAlternativeText.Size = new System.Drawing.Size(427, 20);
            this.textBoxAlternativeText.TabIndex = 11;
            this.textBoxAlternativeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelAlternativeText
            // 
            this.labelAlternativeText.AutoSize = true;
            this.labelAlternativeText.Location = new System.Drawing.Point(36, 221);
            this.labelAlternativeText.Name = "labelAlternativeText";
            this.labelAlternativeText.Size = new System.Drawing.Size(69, 13);
            this.labelAlternativeText.TabIndex = 10;
            this.labelAlternativeText.Text = "טקסט חלופי";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(122, 282);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(94, 23);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "שמירה";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClearForm
            // 
            this.buttonClearForm.Location = new System.Drawing.Point(263, 282);
            this.buttonClearForm.Name = "buttonClearForm";
            this.buttonClearForm.Size = new System.Drawing.Size(94, 23);
            this.buttonClearForm.TabIndex = 13;
            this.buttonClearForm.Text = "נקה טופס";
            this.buttonClearForm.UseVisualStyleBackColor = true;
            this.buttonClearForm.Click += new System.EventHandler(this.buttonClearForm_Click);
            // 
            // buttonOpenTableView
            // 
            this.buttonOpenTableView.Location = new System.Drawing.Point(413, 282);
            this.buttonOpenTableView.Name = "buttonOpenTableView";
            this.buttonOpenTableView.Size = new System.Drawing.Size(94, 23);
            this.buttonOpenTableView.TabIndex = 14;
            this.buttonOpenTableView.Text = "לרשימה";
            this.buttonOpenTableView.UseVisualStyleBackColor = true;
            this.buttonOpenTableView.Click += new System.EventHandler(this.buttonOpenTableView_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.radioButtonUrl);
            this.groupBox1.Controls.Add(this.radioButtonArticleId);
            this.groupBox1.Controls.Add(this.buttonSearchArticle);
            this.groupBox1.Controls.Add(this.textBoxArticleId);
            this.groupBox1.Controls.Add(this.textBoxUrl);
            this.groupBox1.Location = new System.Drawing.Point(12, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 114);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "קישור מטרה";
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.tableLookupCategoriesBindingSource;
            this.comboBox1.DisplayMember = "CatHebrewName";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(58, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.ValueMember = "CatId";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tableLookupCategoriesBindingSource
            // 
            this.tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            this.tableLookupCategoriesBindingSource.DataSource = this.kanNaimDataSetCategoriesBindingSource;
            // 
            // kanNaimDataSetCategoriesBindingSource
            // 
            this.kanNaimDataSetCategoriesBindingSource.DataSource = this._Kan_NaimDataSetCategories;
            this.kanNaimDataSetCategoriesBindingSource.Position = 0;
            // 
            // _Kan_NaimDataSetCategories
            // 
            this._Kan_NaimDataSetCategories.DataSetName = "_Kan_NaimDataSetCategories";
            this._Kan_NaimDataSetCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // radioButtonUrl
            // 
            this.radioButtonUrl.AutoSize = true;
            this.radioButtonUrl.Location = new System.Drawing.Point(451, 44);
            this.radioButtonUrl.Name = "radioButtonUrl";
            this.radioButtonUrl.Size = new System.Drawing.Size(92, 17);
            this.radioButtonUrl.TabIndex = 9;
            this.radioButtonUrl.Text = "קישור ל URL";
            this.radioButtonUrl.UseVisualStyleBackColor = true;
            // 
            // radioButtonArticleId
            // 
            this.radioButtonArticleId.AutoSize = true;
            this.radioButtonArticleId.Checked = true;
            this.radioButtonArticleId.Location = new System.Drawing.Point(460, 19);
            this.radioButtonArticleId.Name = "radioButtonArticleId";
            this.radioButtonArticleId.Size = new System.Drawing.Size(83, 17);
            this.radioButtonArticleId.TabIndex = 8;
            this.radioButtonArticleId.TabStop = true;
            this.radioButtonArticleId.Text = "מספר מאמר";
            this.radioButtonArticleId.UseVisualStyleBackColor = true;
            // 
            // table_LookupCategoriesTableAdapter
            // 
            this.table_LookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // FormAddNewPreferedLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 327);
            this.Controls.Add(this.buttonOpenTableView);
            this.Controls.Add(this.buttonClearForm);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxAlternativeText);
            this.Controls.Add(this.labelAlternativeText);
            this.Controls.Add(this.textBoxOrderPlace);
            this.Controls.Add(this.labelOrderPlace);
            this.Controls.Add(this.buttonSearchPhoto);
            this.Controls.Add(this.textBoxPhotoId);
            this.Controls.Add(this.labelPhotoId);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddNewPreferedLink";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "הוספת לינק מועדף";
            this.Load += new System.EventHandler(this.FormAddNewPreferedLink_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kanNaimDataSetCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._Kan_NaimDataSetCategories)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPhotoId;
        private System.Windows.Forms.TextBox textBoxPhotoId;
        private System.Windows.Forms.Button buttonSearchPhoto;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox textBoxArticleId;
        private System.Windows.Forms.Button buttonSearchArticle;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.TextBox textBoxOrderPlace;
        private System.Windows.Forms.Label labelOrderPlace;
        private System.Windows.Forms.TextBox textBoxAlternativeText;
        private System.Windows.Forms.Label labelAlternativeText;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClearForm;
        private System.Windows.Forms.Button buttonOpenTableView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonArticleId;
        private System.Windows.Forms.RadioButton radioButtonUrl;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.BindingSource kanNaimDataSetCategoriesBindingSource;
        private _Kan_NaimDataSetCategories _Kan_NaimDataSetCategories;
        private System.Windows.Forms.BindingSource tableLookupCategoriesBindingSource;
        private Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter table_LookupCategoriesTableAdapter;
    }
}
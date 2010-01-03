namespace Kan_Naim_Main
{
    partial class FormAddNewPhotos
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
            this.ucUploadPhoto1 = new HaimDLL.UserControlUploadPhoto();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this._10infoDataSetLookupCategories = new Kan_Naim_Main._10infoDataSetLookupCategories();
            this.tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_LookupCategoriesTableAdapter = new Kan_Naim_Main._10infoDataSetLookupCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetLookupCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ucUploadPhoto1
            // 
            this.ucUploadPhoto1.Location = new System.Drawing.Point(12, 12);
            this.ucUploadPhoto1.Name = "ucUploadPhoto1";
            this.ucUploadPhoto1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ucUploadPhoto1.Size = new System.Drawing.Size(640, 429);
            this.ucUploadPhoto1.TabIndex = 0;
            this.ucUploadPhoto1.Load += new System.EventHandler(this.userControlUploadPhoto1_Load);
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DataSource = this.tableLookupCategoriesBindingSource;
            this.comboBoxCategory.DisplayMember = "CatHebrewName";
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(442, 12);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(210, 21);
            this.comboBoxCategory.TabIndex = 1;
            this.comboBoxCategory.ValueMember = "CatId";
            // 
            // _10infoDataSetLookupCategories
            // 
            this._10infoDataSetLookupCategories.DataSetName = "_10infoDataSetLookupCategories";
            this._10infoDataSetLookupCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableLookupCategoriesBindingSource
            // 
            this.tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            this.tableLookupCategoriesBindingSource.DataSource = this._10infoDataSetLookupCategories;
            // 
            // table_LookupCategoriesTableAdapter
            // 
            this.table_LookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "בחר קטגוריה לשיוך התמונה";
            // 
            // FormAddNewPhotos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 462);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.ucUploadPhoto1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddNewPhotos";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "הוספת תמונות חדשות לארכיון";
            this.Load += new System.EventHandler(this.FormAddNewPhotos_Load);
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetLookupCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HaimDLL.UserControlUploadPhoto ucUploadPhoto1;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private _10infoDataSetLookupCategories _10infoDataSetLookupCategories;
        private System.Windows.Forms.BindingSource tableLookupCategoriesBindingSource;
        private Kan_Naim_Main._10infoDataSetLookupCategoriesTableAdapters.Table_LookupCategoriesTableAdapter table_LookupCategoriesTableAdapter;
        private System.Windows.Forms.Label label1;
    }
}
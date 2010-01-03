namespace Kan_Naim_Main
{
    partial class FormAddNewVideo
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
            this.ucUploadVideo1 = new HaimDLL.UserControlUploadVideo();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.infoDataSetLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetLookupCategories = new Kan_Naim_Main._10infoDataSetLookupCategories();
            this.table_LookupCategoriesTableAdapter = new Kan_Naim_Main._10infoDataSetLookupCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataSetLookupCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetLookupCategories)).BeginInit();
            this.SuspendLayout();
            // 
            // ucUploadVideo1
            // 
            this.ucUploadVideo1.Location = new System.Drawing.Point(12, 12);
            this.ucUploadVideo1.Name = "ucUploadVideo1";
            this.ucUploadVideo1.Size = new System.Drawing.Size(601, 398);
            this.ucUploadVideo1.TabIndex = 0;
            this.ucUploadVideo1.Load += new System.EventHandler(this.userControlUploadVideo1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "בחר קטגוריה לשיוך התמונה";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DataSource = this.tableLookupCategoriesBindingSource;
            this.comboBoxCategory.DisplayMember = "CatHebrewName";
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(400, 9);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(210, 21);
            this.comboBoxCategory.TabIndex = 3;
            this.comboBoxCategory.ValueMember = "CatId";
            // 
            // tableLookupCategoriesBindingSource
            // 
            this.tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            this.tableLookupCategoriesBindingSource.DataSource = this.infoDataSetLookupCategoriesBindingSource;
            // 
            // infoDataSetLookupCategoriesBindingSource
            // 
            this.infoDataSetLookupCategoriesBindingSource.DataSource = this._10infoDataSetLookupCategories;
            this.infoDataSetLookupCategoriesBindingSource.Position = 0;
            // 
            // _10infoDataSetLookupCategories
            // 
            this._10infoDataSetLookupCategories.DataSetName = "_10infoDataSetLookupCategories";
            this._10infoDataSetLookupCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_LookupCategoriesTableAdapter
            // 
            this.table_LookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // FormAddNewVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 409);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.ucUploadVideo1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddNewVideo";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "הוספת וידאו חדש";
            this.Load += new System.EventHandler(this.FormAddNewVideo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataSetLookupCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetLookupCategories)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HaimDLL.UserControlUploadVideo ucUploadVideo1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.BindingSource infoDataSetLookupCategoriesBindingSource;
        private _10infoDataSetLookupCategories _10infoDataSetLookupCategories;
        private System.Windows.Forms.BindingSource tableLookupCategoriesBindingSource;
        private Kan_Naim_Main._10infoDataSetLookupCategoriesTableAdapters.Table_LookupCategoriesTableAdapter table_LookupCategoriesTableAdapter;
    }
}
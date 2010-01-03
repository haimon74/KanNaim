namespace Kan_Naim_Main
{
    partial class FormTest
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this._Kan_NaimDataSetCategories = new Kan_Naim_Main._Kan_NaimDataSetCategories();
            this.kanNaimDataSetCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLookupCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_LookupCategoriesTableAdapter = new Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this._Kan_NaimDataSetCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kanNaimDataSetCategoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.tableLookupCategoriesBindingSource;
            this.comboBox1.DisplayMember = "CatHebrewName";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(81, 66);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.ValueMember = "CatId";
            // 
            // _Kan_NaimDataSetCategories
            // 
            this._Kan_NaimDataSetCategories.DataSetName = "_Kan_NaimDataSetCategories";
            this._Kan_NaimDataSetCategories.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // kanNaimDataSetCategoriesBindingSource
            // 
            this.kanNaimDataSetCategoriesBindingSource.DataSource = this._Kan_NaimDataSetCategories;
            this.kanNaimDataSetCategoriesBindingSource.Position = 0;
            // 
            // tableLookupCategoriesBindingSource
            // 
            this.tableLookupCategoriesBindingSource.DataMember = "Table_LookupCategories";
            this.tableLookupCategoriesBindingSource.DataSource = this.kanNaimDataSetCategoriesBindingSource;
            // 
            // table_LookupCategoriesTableAdapter
            // 
            this.table_LookupCategoriesTableAdapter.ClearBeforeFill = true;
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.comboBox1);
            this.Name = "FormTest";
            this.Text = "FormTest";
            this.Load += new System.EventHandler(this.FormTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this._Kan_NaimDataSetCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kanNaimDataSetCategoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupCategoriesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.BindingSource kanNaimDataSetCategoriesBindingSource;
        private _Kan_NaimDataSetCategories _Kan_NaimDataSetCategories;
        private System.Windows.Forms.BindingSource tableLookupCategoriesBindingSource;
        private Kan_Naim_Main._Kan_NaimDataSetCategoriesTableAdapters.Table_LookupCategoriesTableAdapter table_LookupCategoriesTableAdapter;
    }
}
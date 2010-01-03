namespace Kan_Naim_Main
{
    partial class FormManagePreferedLinks
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.imageUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photoIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.articleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderPlaceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.altTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataTablePreferedLinksBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetPreferedLinks = new Kan_Naim_Main._10infoDataSetPreferedLinks();
            this.dataTablePreferedLinksTableAdapter = new Kan_Naim_Main._10infoDataSetPreferedLinksTableAdapters.DataTablePreferedLinksTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTablePreferedLinksBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetPreferedLinks)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageUrlDataGridViewTextBoxColumn,
            this.photoIdDataGridViewTextBoxColumn,
            this.articleIdDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.orderPlaceDataGridViewTextBoxColumn,
            this.altTextDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.dataTablePreferedLinksBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(946, 399);
            this.dataGridView1.TabIndex = 0;
            // 
            // imageUrlDataGridViewTextBoxColumn
            // 
            this.imageUrlDataGridViewTextBoxColumn.DataPropertyName = "ImageUrl";
            this.imageUrlDataGridViewTextBoxColumn.HeaderText = "תמונה";
            this.imageUrlDataGridViewTextBoxColumn.Name = "imageUrlDataGridViewTextBoxColumn";
            // 
            // photoIdDataGridViewTextBoxColumn
            // 
            this.photoIdDataGridViewTextBoxColumn.DataPropertyName = "PhotoId";
            this.photoIdDataGridViewTextBoxColumn.HeaderText = "מספר תמונה";
            this.photoIdDataGridViewTextBoxColumn.Name = "photoIdDataGridViewTextBoxColumn";
            // 
            // articleIdDataGridViewTextBoxColumn
            // 
            this.articleIdDataGridViewTextBoxColumn.DataPropertyName = "ArticleId";
            this.articleIdDataGridViewTextBoxColumn.HeaderText = "מספר כתבה";
            this.articleIdDataGridViewTextBoxColumn.Name = "articleIdDataGridViewTextBoxColumn";
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url - קישור";
            this.urlDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.Width = 300;
            // 
            // orderPlaceDataGridViewTextBoxColumn
            // 
            this.orderPlaceDataGridViewTextBoxColumn.DataPropertyName = "OrderPlace";
            this.orderPlaceDataGridViewTextBoxColumn.HeaderText = "מיקום";
            this.orderPlaceDataGridViewTextBoxColumn.Name = "orderPlaceDataGridViewTextBoxColumn";
            // 
            // altTextDataGridViewTextBoxColumn
            // 
            this.altTextDataGridViewTextBoxColumn.DataPropertyName = "AltText";
            this.altTextDataGridViewTextBoxColumn.HeaderText = "טקסט חלופי";
            this.altTextDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.altTextDataGridViewTextBoxColumn.Name = "altTextDataGridViewTextBoxColumn";
            this.altTextDataGridViewTextBoxColumn.Width = 200;
            // 
            // dataTablePreferedLinksBindingSource
            // 
            this.dataTablePreferedLinksBindingSource.DataMember = "DataTablePreferedLinks";
            this.dataTablePreferedLinksBindingSource.DataSource = this._10infoDataSetPreferedLinks;
            // 
            // _10infoDataSetPreferedLinks
            // 
            this._10infoDataSetPreferedLinks.DataSetName = "_10infoDataSetPreferedLinks";
            this._10infoDataSetPreferedLinks.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataTablePreferedLinksTableAdapter
            // 
            this.dataTablePreferedLinksTableAdapter.ClearBeforeFill = true;
            // 
            // FormManagePreferedLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 399);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormManagePreferedLinks";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "עריכת קישורים מועדפים";
            this.Load += new System.EventHandler(this.FormManagePreferedLinks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTablePreferedLinksBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetPreferedLinks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSetPreferedLinks _10infoDataSetPreferedLinks;
        private System.Windows.Forms.BindingSource dataTablePreferedLinksBindingSource;
        private Kan_Naim_Main._10infoDataSetPreferedLinksTableAdapters.DataTablePreferedLinksTableAdapter dataTablePreferedLinksTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn articleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderPlaceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altTextDataGridViewTextBoxColumn;

    }
}
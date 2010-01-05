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
            this.LinkId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemAddNewRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemEditRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTablePreferedLinksBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetPreferedLinks = new Kan_Naim_Main._10infoDataSetPreferedLinks();
            this.dataTablePreferedLinksTableAdapter = new Kan_Naim_Main._10infoDataSetPreferedLinksTableAdapters.DataTablePreferedLinksTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataTablePreferedLinksBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetPreferedLinks)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageUrlDataGridViewTextBoxColumn,
            this.photoIdDataGridViewTextBoxColumn,
            this.articleIdDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.orderPlaceDataGridViewTextBoxColumn,
            this.altTextDataGridViewTextBoxColumn,
            this.LinkId});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.dataTablePreferedLinksBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(946, 399);
            this.dataGridView1.TabIndex = 0;
            // 
            // imageUrlDataGridViewTextBoxColumn
            // 
            this.imageUrlDataGridViewTextBoxColumn.DataPropertyName = "ImageUrl";
            this.imageUrlDataGridViewTextBoxColumn.HeaderText = "תמונה";
            this.imageUrlDataGridViewTextBoxColumn.Name = "imageUrlDataGridViewTextBoxColumn";
            this.imageUrlDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // photoIdDataGridViewTextBoxColumn
            // 
            this.photoIdDataGridViewTextBoxColumn.DataPropertyName = "PhotoId";
            this.photoIdDataGridViewTextBoxColumn.HeaderText = "מספר תמונה";
            this.photoIdDataGridViewTextBoxColumn.Name = "photoIdDataGridViewTextBoxColumn";
            this.photoIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // articleIdDataGridViewTextBoxColumn
            // 
            this.articleIdDataGridViewTextBoxColumn.DataPropertyName = "ArticleId";
            this.articleIdDataGridViewTextBoxColumn.HeaderText = "מספר כתבה";
            this.articleIdDataGridViewTextBoxColumn.Name = "articleIdDataGridViewTextBoxColumn";
            this.articleIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url - קישור";
            this.urlDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.ReadOnly = true;
            this.urlDataGridViewTextBoxColumn.Width = 300;
            // 
            // orderPlaceDataGridViewTextBoxColumn
            // 
            this.orderPlaceDataGridViewTextBoxColumn.DataPropertyName = "OrderPlace";
            this.orderPlaceDataGridViewTextBoxColumn.HeaderText = "מיקום";
            this.orderPlaceDataGridViewTextBoxColumn.Name = "orderPlaceDataGridViewTextBoxColumn";
            this.orderPlaceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // altTextDataGridViewTextBoxColumn
            // 
            this.altTextDataGridViewTextBoxColumn.DataPropertyName = "AltText";
            this.altTextDataGridViewTextBoxColumn.HeaderText = "טקסט חלופי";
            this.altTextDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.altTextDataGridViewTextBoxColumn.Name = "altTextDataGridViewTextBoxColumn";
            this.altTextDataGridViewTextBoxColumn.ReadOnly = true;
            this.altTextDataGridViewTextBoxColumn.Width = 200;
            // 
            // LinkId
            // 
            this.LinkId.DataPropertyName = "LinkId";
            this.LinkId.HeaderText = "LinkId";
            this.LinkId.Name = "LinkId";
            this.LinkId.ReadOnly = true;
            this.LinkId.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddNewRecord,
            this.ToolStripMenuItemEditRecord,
            this.ToolStripMenuItemDeleteSelected,
            this.ToolStripMenuItemRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 114);
            // 
            // ToolStripMenuItemAddNewRecord
            // 
            this.ToolStripMenuItemAddNewRecord.Name = "ToolStripMenuItemAddNewRecord";
            this.ToolStripMenuItemAddNewRecord.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemAddNewRecord.Text = "הוסף חדש";
            this.ToolStripMenuItemAddNewRecord.Click += new System.EventHandler(this.ToolStripMenuItemAddNewRecord_Click);
            // 
            // ToolStripMenuItemEditRecord
            // 
            this.ToolStripMenuItemEditRecord.Name = "ToolStripMenuItemEditRecord";
            this.ToolStripMenuItemEditRecord.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemEditRecord.Text = "עריכה";
            this.ToolStripMenuItemEditRecord.Click += new System.EventHandler(this.ToolStripMenuItemEditRecord_Click);
            // 
            // ToolStripMenuItemDeleteSelected
            // 
            this.ToolStripMenuItemDeleteSelected.Name = "ToolStripMenuItemDeleteSelected";
            this.ToolStripMenuItemDeleteSelected.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemDeleteSelected.Text = "מחיקה";
            this.ToolStripMenuItemDeleteSelected.Click += new System.EventHandler(this.ToolStripMenuItemDeleteSelected_Click);
            // 
            // ToolStripMenuItemRefresh
            // 
            this.ToolStripMenuItemRefresh.Name = "ToolStripMenuItemRefresh";
            this.ToolStripMenuItemRefresh.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemRefresh.Text = "רענן טבלה";
            this.ToolStripMenuItemRefresh.Click += new System.EventHandler(this.ToolStripMenuItemRefresh_Click);
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
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataTablePreferedLinksBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetPreferedLinks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSetPreferedLinks _10infoDataSetPreferedLinks;
        private System.Windows.Forms.BindingSource dataTablePreferedLinksBindingSource;
        private Kan_Naim_Main._10infoDataSetPreferedLinksTableAdapters.DataTablePreferedLinksTableAdapter dataTablePreferedLinksTableAdapter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddNewRecord;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEditRecord;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDeleteSelected;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn articleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderPlaceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkId;

    }
}
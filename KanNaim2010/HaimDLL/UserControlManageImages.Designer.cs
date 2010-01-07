namespace HaimDLL
{
    partial class UserControlManageImages
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.photoIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.captionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alternateTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.widthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.heightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableOriginalPhotosArchiveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemEditImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDeleteImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemNewImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDetailView = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableOriginalPhotosArchiveBindingSource)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.photoIdDataGridViewTextBoxColumn,
            this.categoryIdDataGridViewTextBoxColumn,
            this.captionDataGridViewTextBoxColumn,
            this.dateDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.alternateTextDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.widthDataGridViewTextBoxColumn,
            this.heightDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tableOriginalPhotosArchiveBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.Size = new System.Drawing.Size(1144, 662);
            this.dataGridView1.TabIndex = 0;
            // 
            // photoIdDataGridViewTextBoxColumn
            // 
            this.photoIdDataGridViewTextBoxColumn.DataPropertyName = "PhotoId";
            this.photoIdDataGridViewTextBoxColumn.HeaderText = "PhotoId";
            this.photoIdDataGridViewTextBoxColumn.Name = "photoIdDataGridViewTextBoxColumn";
            this.photoIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.photoIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // categoryIdDataGridViewTextBoxColumn
            // 
            this.categoryIdDataGridViewTextBoxColumn.DataPropertyName = "CategoryId";
            this.categoryIdDataGridViewTextBoxColumn.HeaderText = "CategoryId";
            this.categoryIdDataGridViewTextBoxColumn.Name = "categoryIdDataGridViewTextBoxColumn";
            this.categoryIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.categoryIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // captionDataGridViewTextBoxColumn
            // 
            this.captionDataGridViewTextBoxColumn.DataPropertyName = "Caption";
            this.captionDataGridViewTextBoxColumn.HeaderText = "כותרת התמונה";
            this.captionDataGridViewTextBoxColumn.MaxInputLength = 150;
            this.captionDataGridViewTextBoxColumn.MinimumWidth = 300;
            this.captionDataGridViewTextBoxColumn.Name = "captionDataGridViewTextBoxColumn";
            this.captionDataGridViewTextBoxColumn.ReadOnly = true;
            this.captionDataGridViewTextBoxColumn.Width = 300;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            this.dateDataGridViewTextBoxColumn.HeaderText = "תאריך";
            this.dateDataGridViewTextBoxColumn.MaxInputLength = 20;
            this.dateDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "שם התמונה";
            this.nameDataGridViewTextBoxColumn.MaxInputLength = 100;
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 300;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 300;
            // 
            // alternateTextDataGridViewTextBoxColumn
            // 
            this.alternateTextDataGridViewTextBoxColumn.DataPropertyName = "AlternateText";
            this.alternateTextDataGridViewTextBoxColumn.HeaderText = "טקסט חלופי";
            this.alternateTextDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.alternateTextDataGridViewTextBoxColumn.Name = "alternateTextDataGridViewTextBoxColumn";
            this.alternateTextDataGridViewTextBoxColumn.ReadOnly = true;
            this.alternateTextDataGridViewTextBoxColumn.Width = 200;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "תאור נוסף";
            this.descriptionDataGridViewTextBoxColumn.MaxInputLength = 250;
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 200;
            // 
            // widthDataGridViewTextBoxColumn
            // 
            this.widthDataGridViewTextBoxColumn.DataPropertyName = "Width";
            this.widthDataGridViewTextBoxColumn.HeaderText = "רוחב";
            this.widthDataGridViewTextBoxColumn.MaxInputLength = 5;
            this.widthDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.widthDataGridViewTextBoxColumn.Name = "widthDataGridViewTextBoxColumn";
            this.widthDataGridViewTextBoxColumn.ReadOnly = true;
            this.widthDataGridViewTextBoxColumn.Visible = false;
            // 
            // heightDataGridViewTextBoxColumn
            // 
            this.heightDataGridViewTextBoxColumn.DataPropertyName = "Height";
            this.heightDataGridViewTextBoxColumn.HeaderText = "גובה";
            this.heightDataGridViewTextBoxColumn.MaxInputLength = 5;
            this.heightDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.heightDataGridViewTextBoxColumn.Name = "heightDataGridViewTextBoxColumn";
            this.heightDataGridViewTextBoxColumn.ReadOnly = true;
            this.heightDataGridViewTextBoxColumn.Visible = false;
            // 
            // tableOriginalPhotosArchiveBindingSource
            // 
            this.tableOriginalPhotosArchiveBindingSource.DataSource = typeof(DbSql.Table_OriginalPhotosArchive);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemEditImage,
            this.ToolStripMenuItemDeleteImage,
            this.ToolStripMenuItemNewImage,
            this.toolStripMenuItemDetailView});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 114);
            // 
            // ToolStripMenuItemEditImage
            // 
            this.ToolStripMenuItemEditImage.Name = "ToolStripMenuItemEditImage";
            this.ToolStripMenuItemEditImage.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemEditImage.Text = "עריכה";
            this.ToolStripMenuItemEditImage.Click += new System.EventHandler(this.ToolStripMenuItemEditImage_Click);
            // 
            // ToolStripMenuItemDeleteImage
            // 
            this.ToolStripMenuItemDeleteImage.Name = "ToolStripMenuItemDeleteImage";
            this.ToolStripMenuItemDeleteImage.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemDeleteImage.Text = "מחיקה";
            this.ToolStripMenuItemDeleteImage.Click += new System.EventHandler(this.ToolStripMenuItemDeleteImage_Click);
            // 
            // ToolStripMenuItemNewImage
            // 
            this.ToolStripMenuItemNewImage.Name = "ToolStripMenuItemNewImage";
            this.ToolStripMenuItemNewImage.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemNewImage.Text = "הוספה";
            this.ToolStripMenuItemNewImage.Click += new System.EventHandler(this.ToolStripMenuItemNewImage_Click);
            // 
            // toolStripMenuItemDetailView
            // 
            this.toolStripMenuItemDetailView.Name = "toolStripMenuItemDetailView";
            this.toolStripMenuItemDetailView.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemDetailView.Text = "הצג פרטים";
            this.toolStripMenuItemDetailView.Click += new System.EventHandler(this.toolStripMenuItemDetailView_Click);
            // 
            // UserControlManageImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "UserControlManageImages";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1144, 662);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableOriginalPhotosArchiveBindingSource)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource tableOriginalPhotosArchiveBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn captionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn alternateTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn widthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEditImage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDeleteImage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemNewImage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDetailView;
    }
}

namespace HaimDLL
{
    partial class UserControlManageArticles
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemEditTak = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDeleteTak = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemFilterBroadcast = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemNewArticle = new System.Windows.Forms.ToolStripMenuItem();
            this.tableArticleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.articleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableArticleBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.articleIdDataGridViewTextBoxColumn,
            this.CategoryId,
            this.UpdateDate,
            this.UpdatedBy,
            this.Title,
            this.SubTitle});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.tableArticleBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1076, 661);
            this.dataGridView1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemEditTak,
            this.ToolStripMenuItemDeleteTak,
            this.ToolStripMenuItemFilterBroadcast,
            this.ToolStripMenuItemNewArticle});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(215, 92);
            // 
            // ToolStripMenuItemEditTak
            // 
            this.ToolStripMenuItemEditTak.Name = "ToolStripMenuItemEditTak";
            this.ToolStripMenuItemEditTak.Size = new System.Drawing.Size(214, 22);
            this.ToolStripMenuItemEditTak.Text = "עריכה";
            this.ToolStripMenuItemEditTak.Click += new System.EventHandler(this.ToolStripMenuItemEditTak_Click);
            // 
            // ToolStripMenuItemDeleteTak
            // 
            this.ToolStripMenuItemDeleteTak.Name = "ToolStripMenuItemDeleteTak";
            this.ToolStripMenuItemDeleteTak.Size = new System.Drawing.Size(214, 22);
            this.ToolStripMenuItemDeleteTak.Text = "מחיקה";
            this.ToolStripMenuItemDeleteTak.Click += new System.EventHandler(this.ToolStripMenuItemDeleteTak_Click);
            // 
            // ToolStripMenuItemFilterBroadcast
            // 
            this.ToolStripMenuItemFilterBroadcast.CheckOnClick = true;
            this.ToolStripMenuItemFilterBroadcast.Name = "ToolStripMenuItemFilterBroadcast";
            this.ToolStripMenuItemFilterBroadcast.Size = new System.Drawing.Size(214, 22);
            this.ToolStripMenuItemFilterBroadcast.Text = "סינון לפי שיגורים אוטומטיים";
            this.ToolStripMenuItemFilterBroadcast.Click += new System.EventHandler(this.ToolStripMenuItemFilterBroadcast_Click);
            // 
            // ToolStripMenuItemNewArticle
            // 
            this.ToolStripMenuItemNewArticle.Name = "ToolStripMenuItemNewArticle";
            this.ToolStripMenuItemNewArticle.Size = new System.Drawing.Size(214, 22);
            this.ToolStripMenuItemNewArticle.Text = "כתבה חדשה";
            this.ToolStripMenuItemNewArticle.Click += new System.EventHandler(this.ToolStripMenuItemNewArticle_Click);
            // 
            // tableArticleBindingSource
            // 
            this.tableArticleBindingSource.DataSource = typeof(DbSql.Table_Article);
            // 
            // articleIdDataGridViewTextBoxColumn
            // 
            this.articleIdDataGridViewTextBoxColumn.DataPropertyName = "ArticleId";
            this.articleIdDataGridViewTextBoxColumn.HeaderText = "ArticleId";
            this.articleIdDataGridViewTextBoxColumn.MaxInputLength = 10;
            this.articleIdDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.articleIdDataGridViewTextBoxColumn.Name = "articleIdDataGridViewTextBoxColumn";
            this.articleIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.articleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // CategoryId
            // 
            this.CategoryId.DataPropertyName = "CategoryId";
            this.CategoryId.HeaderText = "CategoryId";
            this.CategoryId.MaxInputLength = 5;
            this.CategoryId.Name = "CategoryId";
            this.CategoryId.ReadOnly = true;
            this.CategoryId.Visible = false;
            // 
            // UpdateDate
            // 
            this.UpdateDate.ContextMenuStrip = this.contextMenuStrip1;
            this.UpdateDate.DataPropertyName = "UpdateDate";
            this.UpdateDate.HeaderText = "תאריך עדכון";
            this.UpdateDate.MaxInputLength = 30;
            this.UpdateDate.MinimumWidth = 100;
            this.UpdateDate.Name = "UpdateDate";
            this.UpdateDate.ReadOnly = true;
            // 
            // UpdatedBy
            // 
            this.UpdatedBy.DataPropertyName = "UpdatedBy";
            this.UpdatedBy.HeaderText = "עורך";
            this.UpdatedBy.MaxInputLength = 30;
            this.UpdatedBy.MinimumWidth = 150;
            this.UpdatedBy.Name = "UpdatedBy";
            this.UpdatedBy.ReadOnly = true;
            this.UpdatedBy.Width = 150;
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            this.Title.HeaderText = "כותרת ראשית";
            this.Title.MaxInputLength = 250;
            this.Title.MinimumWidth = 300;
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 300;
            // 
            // SubTitle
            // 
            this.SubTitle.DataPropertyName = "SubTitle";
            this.SubTitle.HeaderText = "כותרת משנית";
            this.SubTitle.MaxInputLength = 500;
            this.SubTitle.MinimumWidth = 500;
            this.SubTitle.Name = "SubTitle";
            this.SubTitle.ReadOnly = true;
            this.SubTitle.Width = 500;
            // 
            // UserControlManageArticles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dataGridView1);
            this.Name = "UserControlManageArticles";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1076, 661);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableArticleBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource tableArticleBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEditTak;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDeleteTak;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFilterBroadcast;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemNewArticle;
        private System.Windows.Forms.DataGridViewTextBoxColumn articleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubTitle;
    }
}

namespace Kan_Naim_Main
{
    partial class FormManageBottomPageLinks
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
            this.displeyTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flagVisibleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.altTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bottomUrlCatIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderPlaceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLinksPageBottomBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetPageBottomLinks = new Kan_Naim_Main._10infoDataSet();
            this.table_LinksPageBottomTableAdapter = new Kan_Naim_Main._10infoDataSetTableAdapters.Table_LinksPageBottomTableAdapter();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemAddNewRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemEditRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLinksPageBottomBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetPageBottomLinks)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.displeyTextDataGridViewTextBoxColumn,
            this.flagVisibleDataGridViewCheckBoxColumn,
            this.altTextDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.bottomUrlCatIdDataGridViewTextBoxColumn,
            this.orderPlaceDataGridViewTextBoxColumn,
            this.LinkId});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.tableLinksPageBottomBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(945, 600);
            this.dataGridView1.TabIndex = 0;
            // 
            // displeyTextDataGridViewTextBoxColumn
            // 
            this.displeyTextDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.displeyTextDataGridViewTextBoxColumn.DataPropertyName = "DispleyText";
            this.displeyTextDataGridViewTextBoxColumn.HeaderText = "טקסט להצגה";
            this.displeyTextDataGridViewTextBoxColumn.MaxInputLength = 50;
            this.displeyTextDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.displeyTextDataGridViewTextBoxColumn.Name = "displeyTextDataGridViewTextBoxColumn";
            this.displeyTextDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // flagVisibleDataGridViewCheckBoxColumn
            // 
            this.flagVisibleDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.flagVisibleDataGridViewCheckBoxColumn.DataPropertyName = "FlagVisible";
            this.flagVisibleDataGridViewCheckBoxColumn.HeaderText = "פעיל";
            this.flagVisibleDataGridViewCheckBoxColumn.Name = "flagVisibleDataGridViewCheckBoxColumn";
            this.flagVisibleDataGridViewCheckBoxColumn.ReadOnly = true;
            this.flagVisibleDataGridViewCheckBoxColumn.Width = 39;
            // 
            // altTextDataGridViewTextBoxColumn
            // 
            this.altTextDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.altTextDataGridViewTextBoxColumn.DataPropertyName = "AltText";
            this.altTextDataGridViewTextBoxColumn.HeaderText = "טקסט חלופי";
            this.altTextDataGridViewTextBoxColumn.MaxInputLength = 100;
            this.altTextDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.altTextDataGridViewTextBoxColumn.Name = "altTextDataGridViewTextBoxColumn";
            this.altTextDataGridViewTextBoxColumn.ReadOnly = true;
            this.altTextDataGridViewTextBoxColumn.Width = 200;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url טקסט הקישור ";
            this.urlDataGridViewTextBoxColumn.MaxInputLength = 250;
            this.urlDataGridViewTextBoxColumn.MinimumWidth = 300;
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.ReadOnly = true;
            this.urlDataGridViewTextBoxColumn.Width = 500;
            // 
            // bottomUrlCatIdDataGridViewTextBoxColumn
            // 
            this.bottomUrlCatIdDataGridViewTextBoxColumn.DataPropertyName = "BottomUrlCatId";
            this.bottomUrlCatIdDataGridViewTextBoxColumn.HeaderText = "מספר קטגוריה";
            this.bottomUrlCatIdDataGridViewTextBoxColumn.MaxInputLength = 3;
            this.bottomUrlCatIdDataGridViewTextBoxColumn.Name = "bottomUrlCatIdDataGridViewTextBoxColumn";
            this.bottomUrlCatIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.bottomUrlCatIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // orderPlaceDataGridViewTextBoxColumn
            // 
            this.orderPlaceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.orderPlaceDataGridViewTextBoxColumn.DataPropertyName = "OrderPlace";
            this.orderPlaceDataGridViewTextBoxColumn.HeaderText = "מיקום";
            this.orderPlaceDataGridViewTextBoxColumn.MaxInputLength = 3;
            this.orderPlaceDataGridViewTextBoxColumn.Name = "orderPlaceDataGridViewTextBoxColumn";
            this.orderPlaceDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderPlaceDataGridViewTextBoxColumn.Width = 63;
            // 
            // LinkId
            // 
            this.LinkId.DataPropertyName = "LinkId";
            this.LinkId.HeaderText = "LinkId";
            this.LinkId.Name = "LinkId";
            this.LinkId.ReadOnly = true;
            this.LinkId.Visible = false;
            // 
            // tableLinksPageBottomBindingSource
            // 
            this.tableLinksPageBottomBindingSource.DataMember = "Table_LinksPageBottom";
            this.tableLinksPageBottomBindingSource.DataSource = this._10infoDataSetPageBottomLinks;
            // 
            // _10infoDataSetPageBottomLinks
            // 
            this._10infoDataSetPageBottomLinks.DataSetName = "_10infoDataSetPageBottomLinks";
            this._10infoDataSetPageBottomLinks.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_LinksPageBottomTableAdapter
            // 
            this.table_LinksPageBottomTableAdapter.ClearBeforeFill = true;
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 92);
            // 
            // ToolStripMenuItemAddNewRecord
            // 
            this.ToolStripMenuItemAddNewRecord.Name = "ToolStripMenuItemAddNewRecord";
            this.ToolStripMenuItemAddNewRecord.Size = new System.Drawing.Size(130, 22);
            this.ToolStripMenuItemAddNewRecord.Text = "הוסף חדש";
            this.ToolStripMenuItemAddNewRecord.Click += new System.EventHandler(this.ToolStripMenuItemAddNewRecord_Click);
            // 
            // ToolStripMenuItemEditRecord
            // 
            this.ToolStripMenuItemEditRecord.Name = "ToolStripMenuItemEditRecord";
            this.ToolStripMenuItemEditRecord.Size = new System.Drawing.Size(130, 22);
            this.ToolStripMenuItemEditRecord.Text = "עריכה";
            this.ToolStripMenuItemEditRecord.Click += new System.EventHandler(this.ToolStripMenuItemEditRecord_Click);
            // 
            // ToolStripMenuItemDeleteSelected
            // 
            this.ToolStripMenuItemDeleteSelected.Name = "ToolStripMenuItemDeleteSelected";
            this.ToolStripMenuItemDeleteSelected.Size = new System.Drawing.Size(130, 22);
            this.ToolStripMenuItemDeleteSelected.Text = "מחיקה";
            this.ToolStripMenuItemDeleteSelected.Click += new System.EventHandler(this.ToolStripMenuItemDeleteSelected_Click);
            // 
            // ToolStripMenuItemRefresh
            // 
            this.ToolStripMenuItemRefresh.Name = "ToolStripMenuItemRefresh";
            this.ToolStripMenuItemRefresh.Size = new System.Drawing.Size(130, 22);
            this.ToolStripMenuItemRefresh.Text = "רענן טבלה";
            this.ToolStripMenuItemRefresh.Click += new System.EventHandler(this.ToolStripMenuItemRefresh_Click);
            // 
            // FormManageBottomPageLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 600);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormManageBottomPageLinks";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ניהול לינקים בתחתית העמוד";
            this.Load += new System.EventHandler(this.FormManageBottomPageLinks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLinksPageBottomBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetPageBottomLinks)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSet _10infoDataSetPageBottomLinks;
        private System.Windows.Forms.BindingSource tableLinksPageBottomBindingSource;
        private Kan_Naim_Main._10infoDataSetTableAdapters.Table_LinksPageBottomTableAdapter table_LinksPageBottomTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn displeyTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn flagVisibleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bottomUrlCatIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderPlaceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkId;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddNewRecord;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemEditRecord;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDeleteSelected;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRefresh;
    }
}
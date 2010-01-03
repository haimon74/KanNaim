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
            this.tableLinksPageBottomBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSet = new Kan_Naim_Main._10infoDataSet();
            this.table_LinksPageBottomTableAdapter = new Kan_Naim_Main._10infoDataSetTableAdapters.Table_LinksPageBottomTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLinksPageBottomBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.displeyTextDataGridViewTextBoxColumn,
            this.flagVisibleDataGridViewCheckBoxColumn,
            this.altTextDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.bottomUrlCatIdDataGridViewTextBoxColumn,
            this.orderPlaceDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tableLinksPageBottomBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(945, 600);
            this.dataGridView1.TabIndex = 0;
            // 
            // displeyTextDataGridViewTextBoxColumn
            // 
            this.displeyTextDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.displeyTextDataGridViewTextBoxColumn.DataPropertyName = "DispleyText";
            this.displeyTextDataGridViewTextBoxColumn.HeaderText = "טקסט להצגה";
            this.displeyTextDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.displeyTextDataGridViewTextBoxColumn.Name = "displeyTextDataGridViewTextBoxColumn";
            // 
            // flagVisibleDataGridViewCheckBoxColumn
            // 
            this.flagVisibleDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.flagVisibleDataGridViewCheckBoxColumn.DataPropertyName = "FlagVisible";
            this.flagVisibleDataGridViewCheckBoxColumn.HeaderText = "פעיל";
            this.flagVisibleDataGridViewCheckBoxColumn.Name = "flagVisibleDataGridViewCheckBoxColumn";
            this.flagVisibleDataGridViewCheckBoxColumn.Width = 39;
            // 
            // altTextDataGridViewTextBoxColumn
            // 
            this.altTextDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.altTextDataGridViewTextBoxColumn.DataPropertyName = "AltText";
            this.altTextDataGridViewTextBoxColumn.HeaderText = "טקסט חלופי";
            this.altTextDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.altTextDataGridViewTextBoxColumn.Name = "altTextDataGridViewTextBoxColumn";
            this.altTextDataGridViewTextBoxColumn.Width = 200;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url טקסט הקישור ";
            this.urlDataGridViewTextBoxColumn.MinimumWidth = 300;
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.Width = 500;
            // 
            // bottomUrlCatIdDataGridViewTextBoxColumn
            // 
            this.bottomUrlCatIdDataGridViewTextBoxColumn.DataPropertyName = "BottomUrlCatId";
            this.bottomUrlCatIdDataGridViewTextBoxColumn.HeaderText = "מספר קטגוריה";
            this.bottomUrlCatIdDataGridViewTextBoxColumn.Name = "bottomUrlCatIdDataGridViewTextBoxColumn";
            this.bottomUrlCatIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // orderPlaceDataGridViewTextBoxColumn
            // 
            this.orderPlaceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.orderPlaceDataGridViewTextBoxColumn.DataPropertyName = "OrderPlace";
            this.orderPlaceDataGridViewTextBoxColumn.HeaderText = "מיקום";
            this.orderPlaceDataGridViewTextBoxColumn.Name = "orderPlaceDataGridViewTextBoxColumn";
            this.orderPlaceDataGridViewTextBoxColumn.Width = 63;
            // 
            // tableLinksPageBottomBindingSource
            // 
            this.tableLinksPageBottomBindingSource.DataMember = "Table_LinksPageBottom";
            this.tableLinksPageBottomBindingSource.DataSource = this._10infoDataSet;
            // 
            // _10infoDataSet
            // 
            this._10infoDataSet.DataSetName = "_10infoDataSet";
            this._10infoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_LinksPageBottomTableAdapter
            // 
            this.table_LinksPageBottomTableAdapter.ClearBeforeFill = true;
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
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSet _10infoDataSet;
        private System.Windows.Forms.BindingSource tableLinksPageBottomBindingSource;
        private Kan_Naim_Main._10infoDataSetTableAdapters.Table_LinksPageBottomTableAdapter table_LinksPageBottomTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn displeyTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn flagVisibleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bottomUrlCatIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderPlaceDataGridViewTextBoxColumn;
    }
}
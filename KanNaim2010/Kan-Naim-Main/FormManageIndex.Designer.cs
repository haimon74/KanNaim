namespace Kan_Naim_Main
{
    partial class FormManageIndex
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
            this.tableLookupIndexesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetLookupIndex = new Kan_Naim_Main._10infoDataSetLookupIndex();
            this.tableIndexesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetIndexes = new Kan_Naim_Main._10infoDataSetIndexes();
            this.fillByIndexTypeToolStrip = new System.Windows.Forms.ToolStrip();
            this.typeIdToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.typeIdToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.fillByIndexTypeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.table_LookupIndexesTableAdapter = new Kan_Naim_Main._10infoDataSetLookupIndexTableAdapters.Table_LookupIndexesTableAdapter();
            this.table_IndexesTableAdapter = new Kan_Naim_Main._10infoDataSetIndexesTableAdapters.Table_IndexesTableAdapter();
            this.businessNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.homePageUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobilePhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otherPhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.faxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndexType = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupIndexesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetLookupIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableIndexesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetIndexes)).BeginInit();
            this.fillByIndexTypeToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.businessNameDataGridViewTextBoxColumn,
            this.GroupName,
            this.homePageUrlDataGridViewTextBoxColumn,
            this.mobilePhoneDataGridViewTextBoxColumn,
            this.otherPhoneDataGridViewTextBoxColumn,
            this.faxDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.IndexType,
            this.CategoryId});
            this.dataGridView1.DataSource = this.tableIndexesBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1104, 442);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            this.dataGridView1.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowLeave);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // tableLookupIndexesBindingSource
            // 
            this.tableLookupIndexesBindingSource.DataMember = "Table_LookupIndexes";
            this.tableLookupIndexesBindingSource.DataSource = this._10infoDataSetLookupIndex;
            // 
            // _10infoDataSetLookupIndex
            // 
            this._10infoDataSetLookupIndex.DataSetName = "_10infoDataSetLookupIndex";
            this._10infoDataSetLookupIndex.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableIndexesBindingSource
            // 
            this.tableIndexesBindingSource.DataMember = "Table_Indexes";
            this.tableIndexesBindingSource.DataSource = this._10infoDataSetIndexes;
            // 
            // _10infoDataSetIndexes
            // 
            this._10infoDataSetIndexes.DataSetName = "_10infoDataSetIndexes";
            this._10infoDataSetIndexes.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fillByIndexTypeToolStrip
            // 
            this.fillByIndexTypeToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.typeIdToolStripLabel,
            this.typeIdToolStripTextBox,
            this.fillByIndexTypeToolStripButton});
            this.fillByIndexTypeToolStrip.Location = new System.Drawing.Point(0, 0);
            this.fillByIndexTypeToolStrip.Name = "fillByIndexTypeToolStrip";
            this.fillByIndexTypeToolStrip.Size = new System.Drawing.Size(1104, 25);
            this.fillByIndexTypeToolStrip.TabIndex = 1;
            this.fillByIndexTypeToolStrip.Text = "fillByIndexTypeToolStrip";
            // 
            // typeIdToolStripLabel
            // 
            this.typeIdToolStripLabel.Name = "typeIdToolStripLabel";
            this.typeIdToolStripLabel.Size = new System.Drawing.Size(276, 22);
            this.typeIdToolStripLabel.Text = "מספר אינדקס: (1-עסקים, 2-מודיעינעים, 3-בילוי נעים)";
            // 
            // typeIdToolStripTextBox
            // 
            this.typeIdToolStripTextBox.Name = "typeIdToolStripTextBox";
            this.typeIdToolStripTextBox.Size = new System.Drawing.Size(100, 25);
            // 
            // fillByIndexTypeToolStripButton
            // 
            this.fillByIndexTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fillByIndexTypeToolStripButton.Name = "fillByIndexTypeToolStripButton";
            this.fillByIndexTypeToolStripButton.Size = new System.Drawing.Size(71, 22);
            this.fillByIndexTypeToolStripButton.Text = "לחץ להצגה";
            this.fillByIndexTypeToolStripButton.Click += new System.EventHandler(this.fillByIndexTypeToolStripButton_Click);
            // 
            // table_LookupIndexesTableAdapter
            // 
            this.table_LookupIndexesTableAdapter.ClearBeforeFill = true;
            // 
            // table_IndexesTableAdapter
            // 
            this.table_IndexesTableAdapter.ClearBeforeFill = true;
            // 
            // businessNameDataGridViewTextBoxColumn
            // 
            this.businessNameDataGridViewTextBoxColumn.DataPropertyName = "BusinessName";
            this.businessNameDataGridViewTextBoxColumn.Frozen = true;
            this.businessNameDataGridViewTextBoxColumn.HeaderText = "שם העסק";
            this.businessNameDataGridViewTextBoxColumn.Name = "businessNameDataGridViewTextBoxColumn";
            // 
            // GroupName
            // 
            this.GroupName.DataSource = this.tableLookupIndexesBindingSource;
            this.GroupName.DisplayMember = "IndexName";
            this.GroupName.HeaderText = "שם קטגוריה";
            this.GroupName.MaxDropDownItems = 20;
            this.GroupName.MinimumWidth = 100;
            this.GroupName.Name = "GroupName";
            this.GroupName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GroupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.GroupName.ValueMember = "Id";
            this.GroupName.Width = 150;
            // 
            // homePageUrlDataGridViewTextBoxColumn
            // 
            this.homePageUrlDataGridViewTextBoxColumn.DataPropertyName = "HomePageUrl";
            this.homePageUrlDataGridViewTextBoxColumn.HeaderText = "כתובת האתר אינטרנט";
            this.homePageUrlDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.homePageUrlDataGridViewTextBoxColumn.Name = "homePageUrlDataGridViewTextBoxColumn";
            this.homePageUrlDataGridViewTextBoxColumn.Width = 150;
            // 
            // mobilePhoneDataGridViewTextBoxColumn
            // 
            this.mobilePhoneDataGridViewTextBoxColumn.DataPropertyName = "MobilePhone";
            this.mobilePhoneDataGridViewTextBoxColumn.HeaderText = "טלפון סלולרי";
            this.mobilePhoneDataGridViewTextBoxColumn.Name = "mobilePhoneDataGridViewTextBoxColumn";
            // 
            // otherPhoneDataGridViewTextBoxColumn
            // 
            this.otherPhoneDataGridViewTextBoxColumn.DataPropertyName = "OtherPhone";
            this.otherPhoneDataGridViewTextBoxColumn.HeaderText = "טלפון אחר";
            this.otherPhoneDataGridViewTextBoxColumn.Name = "otherPhoneDataGridViewTextBoxColumn";
            // 
            // faxDataGridViewTextBoxColumn
            // 
            this.faxDataGridViewTextBoxColumn.DataPropertyName = "Fax";
            this.faxDataGridViewTextBoxColumn.HeaderText = "פקס";
            this.faxDataGridViewTextBoxColumn.Name = "faxDataGridViewTextBoxColumn";
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "אימייל";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "תאור";
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 200;
            // 
            // IndexType
            // 
            this.IndexType.DataPropertyName = "IndexType";
            this.IndexType.HeaderText = "לחץ לאישור";
            this.IndexType.MinimumWidth = 50;
            this.IndexType.Name = "IndexType";
            this.IndexType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.IndexType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IndexType.Width = 50;
            // 
            // CategoryId
            // 
            this.CategoryId.DataPropertyName = "CategoryId";
            this.CategoryId.HeaderText = "CategoryId";
            this.CategoryId.Name = "CategoryId";
            this.CategoryId.Visible = false;
            // 
            // FormManageIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 470);
            this.Controls.Add(this.fillByIndexTypeToolStrip);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManageIndex";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ניהול רשימת אינדקסים";
            this.Load += new System.EventHandler(this.FormManageIndex_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupIndexesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetLookupIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableIndexesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetIndexes)).EndInit();
            this.fillByIndexTypeToolStrip.ResumeLayout(false);
            this.fillByIndexTypeToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSetIndexes _10infoDataSetIndexes;
        private System.Windows.Forms.BindingSource tableIndexesBindingSource;
        private Kan_Naim_Main._10infoDataSetIndexesTableAdapters.Table_IndexesTableAdapter table_IndexesTableAdapter;
        private System.Windows.Forms.ToolStrip fillByIndexTypeToolStrip;
        private System.Windows.Forms.ToolStripLabel typeIdToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox typeIdToolStripTextBox;
        private System.Windows.Forms.ToolStripButton fillByIndexTypeToolStripButton;
        private _10infoDataSetLookupIndex _10infoDataSetLookupIndex;
        private System.Windows.Forms.BindingSource tableLookupIndexesBindingSource;
        private Kan_Naim_Main._10infoDataSetLookupIndexTableAdapters.Table_LookupIndexesTableAdapter table_LookupIndexesTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn businessNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn homePageUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobilePhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn otherPhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn faxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn IndexType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryId;
    }
}
namespace Kan_Naim_Main
{
    partial class FormManageTopMenuLinks
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
            this.displayTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTipDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationFromTheRightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableMenuTopBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetMenuTop = new Kan_Naim_Main._10infoDataSetMenuTop();
            this.table_MenuTopTableAdapter = new Kan_Naim_Main._10infoDataSetMenuTopTableAdapters.Table_MenuTopTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableMenuTopBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetMenuTop)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.displayTextDataGridViewTextBoxColumn,
            this.imageUrlDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.toolTipDataGridViewTextBoxColumn,
            this.locationFromTheRightDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tableMenuTopBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(994, 369);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // displayTextDataGridViewTextBoxColumn
            // 
            this.displayTextDataGridViewTextBoxColumn.DataPropertyName = "DisplayText";
            this.displayTextDataGridViewTextBoxColumn.HeaderText = "טקסט מוצג";
            this.displayTextDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.displayTextDataGridViewTextBoxColumn.Name = "displayTextDataGridViewTextBoxColumn";
            // 
            // imageUrlDataGridViewTextBoxColumn
            // 
            this.imageUrlDataGridViewTextBoxColumn.DataPropertyName = "ImageUrl";
            this.imageUrlDataGridViewTextBoxColumn.HeaderText = "קישור לתמונה";
            this.imageUrlDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.imageUrlDataGridViewTextBoxColumn.Name = "imageUrlDataGridViewTextBoxColumn";
            this.imageUrlDataGridViewTextBoxColumn.Width = 300;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url - קישור יעד";
            this.urlDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.Width = 300;
            // 
            // toolTipDataGridViewTextBoxColumn
            // 
            this.toolTipDataGridViewTextBoxColumn.DataPropertyName = "ToolTip";
            this.toolTipDataGridViewTextBoxColumn.HeaderText = "טקסט תיאור נוסף";
            this.toolTipDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.toolTipDataGridViewTextBoxColumn.Name = "toolTipDataGridViewTextBoxColumn";
            this.toolTipDataGridViewTextBoxColumn.Width = 200;
            // 
            // locationFromTheRightDataGridViewTextBoxColumn
            // 
            this.locationFromTheRightDataGridViewTextBoxColumn.DataPropertyName = "LocationFromTheRight";
            this.locationFromTheRightDataGridViewTextBoxColumn.HeaderText = "מיקום מספר מימין";
            this.locationFromTheRightDataGridViewTextBoxColumn.MinimumWidth = 20;
            this.locationFromTheRightDataGridViewTextBoxColumn.Name = "locationFromTheRightDataGridViewTextBoxColumn";
            this.locationFromTheRightDataGridViewTextBoxColumn.Width = 50;
            // 
            // tableMenuTopBindingSource
            // 
            this.tableMenuTopBindingSource.DataMember = "Table_MenuTop";
            this.tableMenuTopBindingSource.DataSource = this._10infoDataSetMenuTop;
            // 
            // _10infoDataSetMenuTop
            // 
            this._10infoDataSetMenuTop.DataSetName = "_10infoDataSetMenuTop";
            this._10infoDataSetMenuTop.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_MenuTopTableAdapter
            // 
            this.table_MenuTopTableAdapter.ClearBeforeFill = true;
            // 
            // FormManageTopMenuLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 369);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManageTopMenuLinks";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ניהול תפריט עליון";
            this.Load += new System.EventHandler(this.FormManageTopMenuLinks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableMenuTopBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetMenuTop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSetMenuTop _10infoDataSetMenuTop;
        private System.Windows.Forms.BindingSource tableMenuTopBindingSource;
        private Kan_Naim_Main._10infoDataSetMenuTopTableAdapters.Table_MenuTopTableAdapter table_MenuTopTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolTipDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationFromTheRightDataGridViewTextBoxColumn;
    }
}
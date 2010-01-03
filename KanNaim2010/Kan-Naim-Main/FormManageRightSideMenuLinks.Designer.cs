﻿namespace Kan_Naim_Main
{
    partial class FormManageRightSideMenuLinks
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
            this.LocationFromTheTop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableMenuRightSideBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetMenuRightSide = new Kan_Naim_Main._10infoDataSetMenuRightSide();
            this.table_MenuRightSideTableAdapter = new Kan_Naim_Main._10infoDataSetMenuRightSideTableAdapters.Table_MenuRightSideTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableMenuRightSideBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetMenuRightSide)).BeginInit();
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
            this.LocationFromTheTop});
            this.dataGridView1.DataSource = this.tableMenuRightSideBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1045, 369);
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
            // LocationFromTheTop
            // 
            this.LocationFromTheTop.DataPropertyName = "LocationFromTheTop";
            this.LocationFromTheTop.HeaderText = "מיקום מלמעלה";
            this.LocationFromTheTop.MinimumWidth = 50;
            this.LocationFromTheTop.Name = "LocationFromTheTop";
            // 
            // tableMenuRightSideBindingSource
            // 
            this.tableMenuRightSideBindingSource.DataMember = "Table_MenuRightSide";
            this.tableMenuRightSideBindingSource.DataSource = this._10infoDataSetMenuRightSide;
            // 
            // _10infoDataSetMenuRightSide
            // 
            this._10infoDataSetMenuRightSide.DataSetName = "_10infoDataSetMenuRightSide";
            this._10infoDataSetMenuRightSide.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_MenuRightSideTableAdapter
            // 
            this.table_MenuRightSideTableAdapter.ClearBeforeFill = true;
            // 
            // FormManageRightSideMenuLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 369);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManageRightSideMenuLinks";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ניהול תפריט צד ימין";
            this.Load += new System.EventHandler(this.FormManageRightSideMenuLinks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableMenuRightSideBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetMenuRightSide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private _10infoDataSetMenuRightSide _10infoDataSetMenuRightSide;
        private System.Windows.Forms.BindingSource tableMenuRightSideBindingSource;
        private _10infoDataSetMenuRightSideTableAdapters.Table_MenuRightSideTableAdapter table_MenuRightSideTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolTipDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationFromTheTop;
    }
}
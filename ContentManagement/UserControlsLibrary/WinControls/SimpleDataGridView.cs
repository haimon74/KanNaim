using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    class SimpleDataGridView : DataGridView
    {
        public delegate void EventDelegate(object sender, EventArgs e);

        //private SimpleListDataGridView _rowListGridView;

        public SimpleDataGridView(int width, int height, int fromTop, int fromLeft, Font font,
            Color foreColor, Color bgColor, Color alternateBgColor, Color selectBgColor,
            DataSet dataSet, string tableName, string detailsTableName,
            bool enableDelete, bool enableEditing,
            EventDelegate onCellDoubleClickEventFunction, 
            EventDelegate onTextChangedEventFunction,
            SimpleDataGridViewColumn[] columns
            )
        {
            this.RowTemplate = new DataGridViewRow();
            
            this.StandardTab = false;
            
            this.Columns.Clear();
            foreach (SimpleDataGridViewColumn column in columns)
            {
                this.Columns.Add(column);
            }
            
            
            
        }
        public SimpleDataGridView(int width, int height, int fromTop, int fromLeft, 
            DataSet dataSet, string tableName, string detailsTableName,
            EventDelegate onCellDoubleClickEventFunction
            )
        {
            //this._rowListGridView = new SimpleDataGridView(width, height, fromTop, fromLeft);
            this.AllowUserToAddRows = true;
            this.AllowUserToDeleteRows = true;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToResizeColumns = false;
            this.AllowUserToResizeRows = true;
            var font = new Font("Arial", 1, FontStyle.Regular);
            this.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                                                       {
                                                           Alignment = DataGridViewContentAlignment.MiddleRight,
                                                           BackColor = Color.Cornsilk,
                                                           Font = font,
                                                           //DataSourceNullValue = " ",
                                                           //ForeColor = Color.Black,
                                                           //Format = "{0}",
                                                           //NullValue = " ",
                                                           Padding = new Padding(1, 2, 3, 1),
                                                           SelectionBackColor = Color.Beige
                                                       };
            this.DefaultCellStyle = this.AlternatingRowsDefaultCellStyle;
            this.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.AutoGenerateColumns = true;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            this.CausesValidation = true;
            this.ClientSize = new Size(width, height);
            this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            //this.ColumnCount = 30;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            var headerFont = new Font("Arial", 1, FontStyle.Bold);
            this.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                                                     {
                                                         Alignment = DataGridViewContentAlignment.MiddleRight,
                                                         BackColor = Color.LightSalmon,
                                                         ForeColor = Color.Blue,
                                                         Font = headerFont,
                                                         Padding = new Padding(2, 3, 4, 3),
                                                         SelectionBackColor = Color.LightPink
                                                     };
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColumnHeadersVisible = true;
            this.DataSource = dataSet.Tables[tableName].DefaultView;
            this.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.Enabled = true;
            this.FontHeight = 12;
            this.GridColor = Color.Honeydew;
            this.IsAccessible = true;
            this.Location = new Point(fromTop , fromLeft);
            this.Margin = new Padding(10);
            this.MultiSelect = false;
            this.Name = String.Format("dataGridViewDs{0}Dt{1}", dataSet, tableName);
            this.Padding = new Padding(2);
            this.ReadOnly = false;
            //this.RowCount = 1000;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.ShowCellErrors = true;
            this.ShowCellToolTips = false;
            this.ShowEditingIcon = true;
            this.ShowRowErrors = true;
            this.UseWaitCursor = false;
            this.Visible = true;
        }

        #region Properties
        
        #endregion
    }
}

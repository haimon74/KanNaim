using System;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    public class IdDataGridViewColumn : SimpleDataGridViewColumn
    {
        public IdDataGridViewColumn()
        {
            this.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.DataPropertyName = "Id";
            this.DividerWidth = 1;
            this.DisplayIndex = 0;
            this.HeaderCell = new DataGridViewColumnHeaderCell();
            this.HeaderText = "#";
            this.MinimumWidth = 10;
            this.Width = 30;
            this.FillWeight = this.Width;
            this.Name = "Id";
            this.SortMode = DataGridViewColumnSortMode.Automatic;
            this.DefaultHeaderCellType = Type.GetType("int");
            this.ValueType = Type.GetType("int");

            InitializeVirtualProperties(null, null, null);
        }

        public void InitializeVirtualProperties(
            DataGridViewCellStyle defaultCellStyle, 
            bool? frozen, bool? visible)
        {
            this.Frozen = frozen ?? true;
            this.DefaultCellStyle = defaultCellStyle ?? new SimpleDataGridViewCellStyle();
            this.CellTemplate = new SimpleDataGridViewTextBoxCell(new SimpleDataGridViewCellStyle());
            this.Resizable = DataGridViewTriState.False;
            this.ReadOnly = true;
            this.Visible = visible ?? false;
        }
    }
}

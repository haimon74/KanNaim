using System;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    public class SimpleDataGridViewColumn : DataGridViewColumn
    {
#region static private members

        private static int _defaultDisplayIndex = 0;
        
#endregion

#region Properties

        public int TableWidth { get; set;}
        public int DisplayedColumns { get; set;}
        
#endregion

#region Constructors

        public SimpleDataGridViewColumn()
        {
            InitializeNonVirtualProperties(
                null, null, null, null, null, null,
                null, null, null, null, null, null, null);
            InitializeVirtualProperties(null, true, null, null, null);
        }
        public SimpleDataGridViewColumn(
            DataGridViewAutoSizeColumnMode? autoSizeColumnMode,
            DataGridViewColumnHeaderCell headerCell,
            DataGridViewColumnSortMode? sortMode,
            Type defaultHeaderCellType, Type valueType,
            string dataPropertyName, string headerText, string name,
            int? dividerWidth, int? displayIndex, float? fillWeight, int? minWidth, int? width,
            DataGridViewCellStyle defaultCellStyle,
            bool resizable, bool? frozen, bool? readOnly, bool? visible)
        {
            InitializeNonVirtualProperties(
                autoSizeColumnMode, headerCell, sortMode,
                defaultHeaderCellType, valueType, dataPropertyName, headerText, name,
                dividerWidth, displayIndex, fillWeight, minWidth, width);

            InitializeVirtualProperties(defaultCellStyle, resizable, frozen, readOnly, visible);
        }

        public void InitializeNonVirtualProperties(
            DataGridViewAutoSizeColumnMode? autoSizeColumnMode,
            DataGridViewColumnHeaderCell headerCell, 
            DataGridViewColumnSortMode? sortMode,
            Type defaultHeaderCellType, Type valueType, 
            string dataPropertyName, string headerText, string name,
            int? dividerWidth, int? displayIndex, float? fillWeight, int? minWidth, int? width)
        {
            this.AutoSizeMode = autoSizeColumnMode ?? DataGridViewAutoSizeColumnMode.AllCells;
            this.DataPropertyName = dataPropertyName ?? "Text";
            this.DividerWidth = dividerWidth ?? 2;
            this.DisplayIndex = displayIndex ?? _defaultDisplayIndex++;
            this.FillWeight = fillWeight ?? 10;
            this.HeaderCell = headerCell ?? new DataGridViewColumnHeaderCell();
            this.HeaderText = headerText;
            this.MinimumWidth = minWidth ?? 50;
            this.Width = width ?? 100;
            this.Name = name;
            this.SortMode = sortMode ?? DataGridViewColumnSortMode.Programmatic;
            this.DefaultHeaderCellType = defaultHeaderCellType ?? Type.GetType("string");
            this.ValueType = valueType ?? Type.GetType("string");
            
        }

        public void InitializeVirtualProperties(
            DataGridViewCellStyle defaultCellStyle,
            bool resizable, bool? frozen, bool? readOnly, bool? visible)
        {
            this.Frozen = frozen ?? false;
            this.DefaultCellStyle = defaultCellStyle ?? new DataGridViewCellStyle();
            this.Resizable = (resizable) ?  
                DataGridViewTriState.True : 
                DataGridViewTriState.False;
            this.ReadOnly = readOnly ?? true;
            this.Visible = visible ?? true;
        }
        public void InitializeVirtualProperties()
        {
            InitializeVirtualProperties(null, true, null, null, null);
        }
        
#endregion        
        

#region public Methods

        public void SetSizes(int? minWidth, int? width, bool isFillMode, 
            bool resizable, bool? frozen, float? fillWeight)
        {
            this.Resizable = (resizable) ?
                DataGridViewTriState.True :
                DataGridViewTriState.False;
            this.Frozen = frozen ?? false;
            this.FillWeight = fillWeight ?? TableWidth / DisplayedColumns;
            this.MinimumWidth = minWidth ?? (int)this.FillWeight / 2;
            this.Width = width ?? (int)this.FillWeight;
            if (isFillMode)
                this.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

#endregion
    }
}

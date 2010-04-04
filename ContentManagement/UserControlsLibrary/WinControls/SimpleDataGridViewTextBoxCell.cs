using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    class SimpleDataGridViewTextBoxCell : DataGridViewTextBoxCell
    {
        public SimpleDataGridViewTextBoxCell(DataGridViewCellStyle cellStyle)
        {
            //this.ContextMenuStrip = null; // virtual
            this.ErrorText = "ERR";
            //this.ReadOnly = true; // virtual
            this.Style = cellStyle ?? new SimpleDataGridViewCellStyle();
            //this.ValueType = Type.GetType("string"); // virtual
            this.Value = null;
        }
    }
}

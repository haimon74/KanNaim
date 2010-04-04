using System.Drawing;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    class SimpleDataGridViewCellStyle : DataGridViewCellStyle
    {
        public SimpleDataGridViewCellStyle()
        {
            InitializeNonVirtualProperties(true, null, null);
            SetFontColors(null, null, null, null, null);
        }
        public SimpleDataGridViewCellStyle(bool isHebrew, object dsNullValue, object nullValue )
        {
            InitializeNonVirtualProperties(isHebrew, dsNullValue, nullValue);
            SetFontColors(null, null, null, null, null);
        }

        private void InitializeNonVirtualProperties(bool isHebrew, object dsNullValue, object nullValue)
        {
            this.Alignment = (isHebrew)
                                 ? DataGridViewContentAlignment.TopRight
                                 : DataGridViewContentAlignment.TopLeft;

            this.DataSourceNullValue = dsNullValue;
            this.NullValue = nullValue;

            
        }
        public void SetFontColors(Font font, Color? bgColor, Color? foreColor, Color? bgSelection, Color? colorSelection)
        {
            this.Font = font ?? new Font("Arial", 1 , FontStyle.Regular);
            this.BackColor = bgColor ?? Color.WhiteSmoke;
            this.ForeColor = foreColor ?? Color.Black;
            this.SelectionBackColor = bgSelection ?? Color.LightPink;
            this.SelectionForeColor = colorSelection ?? Color.Blue;
        }
    }
}

using System;
using System.Windows.Forms;

namespace HaimDLL
{
    public partial class UserControlTakFill : UserControl
    {
        private int _takType;

        public UserControlTakFill()
        {
            InitializeComponent();
            TakType = 0;
        }

        public int TakType
        {
            set { _takType = value; }
        }

        private void ucTakContent1_Load(object sender, EventArgs e)
        {

        }

        public int SaveToDatabase(int articleId, int embedObjId)
        {
            int returnValue = ucTakContent1.SaveToDatabase(articleId, embedObjId, _takType);

            ucTakBroadcastCmd1.SaveToDatabase(returnValue);

            return returnValue;
        }

        public bool ValidateValuesBeforeSave()
        {
            return ucTakContent1.ValidateValuesBeforeSave();
        }

        public bool IsEnabled()
        {
            return ucTakContent1.IsEnabled();
        }
    }
}

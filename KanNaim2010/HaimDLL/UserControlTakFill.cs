using System;
using System.Windows.Forms;

namespace HaimDLL
{
    public partial class UserControlTakFill : UserControl
    {
        private static readonly int[,] TextBoxesLength = new int[7, 2] { { 5, 10 }, { 5, 10 }, { 5, 10 }, { 5, 10 }, { 5, 10 }, { 5, 10 }, { 5, 10 }};
        
        private int _takType;

        public UserControlTakFill()
        {
            InitializeComponent();
            TakType = 0;
        }

        public int TakType
        {
            set
            {
                _takType = value;
                ucTakContent1.MaxTitleLength = TextBoxesLength[_takType, 0];
                ucTakContent1.MaxContentLength = TextBoxesLength[_takType, 1];
            }
        }

        private void ucTakContent1_Load(object sender, EventArgs e)
        {

        }

        public int SaveToDatabase(int articleId, int embedObjId)
        {
            int returnValue = ucTakContent1.SaveToDatabase(articleId, embedObjId, _takType);

            ucTakBroadcastCmd1.SaveToDatabase(returnValue, _takType);

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

        public void SetTitleText(string title)
        {
            ucTakContent1.Title = title;
        }
        public void SetContentText(string content)
        {
            ucTakContent1.Content = content;
        }
    }
}

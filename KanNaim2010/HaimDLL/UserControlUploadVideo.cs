using System;
using System.Windows.Forms;

namespace HaimDLL
{
    public partial class UserControlUploadVideo : UserControl
    {
        public delegate void SaveVideoCallBackFunction(object sender, EventArgs e);

        private SaveVideoCallBackFunction _saveVideoCallbackFunctionName;

        public UserControlUploadVideo()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            _textBoxVideoAlternateText.Text = "";
            _textBoxVideoEmbedUrl.Text = "";
            _textBoxVideoDescription.Text = "";
            _textBoxVideoCaption.Text = "";
        }

        public void SetCallbackFunction(SaveVideoCallBackFunction callBackFunction)
        {
            _saveVideoCallbackFunctionName = callBackFunction;
        }

        private void _buttonSaveVideoToArchive_Click(object sender, EventArgs e)
        {
            _saveVideoCallbackFunctionName(sender, e);
        }
    }
}

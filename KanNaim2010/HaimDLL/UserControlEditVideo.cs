using System;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlEditVideo : UserControl
    {
        public delegate void SaveVideoCallBackFunction(object sender, EventArgs e);

        private int _videoId;
        private bool _isNewVideo;

        public UserControlEditVideo(int videoId)
        {
            InitializeComponent();
            _videoId = videoId;
            PopulateFormById();
        }

        public void Clear()
        {
            _textBoxVideoAlternateText.Text = "";
            _textBoxVideoEmbedUrl.Text = "";
            _textBoxVideoDescription.Text = "";
            _textBoxVideoCaption.Text = "";
        }

        private void PopulateFormById()
        {
            var video = Filter.GetVideoFromId(_videoId);

            try
            {
                _textBoxVideoEmbedUrl.Text = video.EmbedUrl;
                _textBoxVideoAlternateText.Text = video.AlternateText;
                _textBoxVideoCaption.Text = video.Caption;
                _textBoxVideoDescription.Text = video.Description;
                _isNewVideo = false;
            }
            catch
            {
                _isNewVideo = true;
            }
        }

        private void _buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Table_VideosArchive video;

                if (_isNewVideo)
                {
                    video = new Table_VideosArchive();
                    Lookup.Db.Table_VideosArchives.InsertOnSubmit(video);
                }
                else
                {
                    video = Filter.GetVideoFromId(_videoId);
                }
                video.EmbedUrl = _textBoxVideoEmbedUrl.Text;
                video.AlternateText = _textBoxVideoAlternateText.Text;
                video.Description = _textBoxVideoDescription.Text;
                video.Caption = _textBoxVideoCaption.Text;

                Lookup.Db.SubmitChanges();
            }
            catch
            {
            }
        }
        
        private void buttonClearForm_Click(object sender, EventArgs e)
        {
            _buttonSave.Enabled = true;

            _textBoxVideoEmbedUrl.Text = "";
            _textBoxVideoDescription.Text = "";
            _textBoxVideoCaption.Text = "";
            _textBoxVideoAlternateText.Text = "";

            _isNewVideo = true;
        }
    }
}

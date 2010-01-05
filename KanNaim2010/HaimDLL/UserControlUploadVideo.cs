using System;
using System.Linq;
using System.Windows.Forms;
using DbSql;

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

        public void PopulateByVideoId(int videoId)
        {
            try
            {
                var video = (from c in DbSql.Lookup.Db.Table_VideosArchives
                             where c.Id == videoId
                             select c).Single();
                _textBoxVideoEmbedUrl.Text = video.EmbedUrl;
                _textBoxVideoDescription.Text = video.Description;
                _textBoxVideoCaption.Text = video.Caption;
                _textBoxVideoAlternateText.Text = video.AlternateText;
            }
            catch
            {
                
            }
        }
        private void _buttonSaveVideoToArchive_Click(object sender, EventArgs e)
        {
            _saveVideoCallbackFunctionName(sender, e);
        }

        private void SelectVideoDoubleClick(object sender, EventArgs e)
        {
            try
            {
                int videoId = int.Parse(((ListView) sender).SelectedItems[0].SubItems[0].Text);

                var video = Filter.GetVideoFromId(videoId);

                _textBoxVideoEmbedUrl.Text = video.EmbedUrl;
                _textBoxVideoDescription.Text = video.Description;
                _textBoxVideoCaption.Text = video.Caption;
                _textBoxVideoAlternateText.Text = video.AlternateText;
            }
            catch
            {

            }
        }

        private void buttonLoadFromArchive_Click(object sender, EventArgs e)
        {
            var form1 = new Form()
                            {
                                RightToLeft = RightToLeft.Yes,
                                RightToLeftLayout = true,
                                Height = 500,
                                Width = 800
                            };
            var listView = new ListView
                               {
                                   View = View.Details,
                                   GridLines = true,
                                   FullRowSelect = true,
                                   CheckBoxes = false,
                                   AllowColumnReorder = true,
                                   LabelEdit = false,
                                   RightToLeftLayout = true,
                                   MultiSelect = false,
                                   Dock = DockStyle.Fill,
                                   Visible = true
                               };
            
            listView.DoubleClick += SelectVideoDoubleClick;

            listView.Columns.Add("מספר", 50, HorizontalAlignment.Center);
            listView.Columns.Add("קטגוריה", 150, HorizontalAlignment.Center);
            listView.Columns.Add("שם הווידאו", 600, HorizontalAlignment.Center);

            var itemCollection = new ListView.ListViewItemCollection(listView);
            
            var videos = from c in Lookup.Db.Table_VideosArchives
                         select c;
            foreach (Table_VideosArchive video in videos)
            {
                var item = new ListViewItem();
                item.SubItems.Add(video.Id.ToString());
                string catName = Lookup.GetLookupCategoryNameFromId(video.CategoryId);
                item.SubItems.Add(catName);
                item.SubItems.Add(video.Caption);
                //item.SubItems.Add(video.Date.ToShortDateString());
                //item.SubItems.Add(video.Description);
                itemCollection.Add(item);
            }
            /* - for testing
            string[] strings1 = {"1", "text", "text2"};
            var item1 = new ListViewItem(strings1);
            itemCollection.Add(item1);
            string[] strings2 = { "2", "text", "text2" };
            var item2 = new ListViewItem(strings2);
            itemCollection.Add(item2);
            */

            form1.Controls.Add(listView);
            listView.Refresh();

            form1.Show();

            _buttonSaveVideoToArchive.Enabled = false;
        }

        private void buttonClearForm_Click(object sender, EventArgs e)
        {
            _buttonSaveVideoToArchive.Enabled = true;

            _textBoxVideoEmbedUrl.Text = "";
            _textBoxVideoDescription.Text = "";
            _textBoxVideoCaption.Text = "";
            _textBoxVideoAlternateText.Text = "";
        }
    }
}

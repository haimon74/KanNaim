using System;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlTakContent : UserControl
    {
        private int _maxTitleLength;
        private int _maxContentLength;

        public int MaxTitleLength
        {
            set { _maxTitleLength = value; }
        }
        public int MaxContentLength
        {
            set { _maxContentLength = value; }
        }

        public string Title
        {
            set { textBoxTakTitle.Text = value; }
        }

        public string Content
        {
            set { textBoxTakContent.Text = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserControlTakContent()
        {
            InitializeComponent();
        }

        private void checkBoxTakPhoto_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxTakPhoto.Enabled = checkBoxTakPhoto.Checked;
        }

        private void checkBoxEnableContent_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTakContent.Enabled = checkBoxEnableContent.Checked;
        }

        public int SaveToDatabase(int articleId, int embedObjId, int takType)
        {
            var tak = new Table_Taktzirim
            {
                PhotoId = (int)comboBoxTakPhoto.SelectedValue,
                ArticleId = articleId,
                EmbedObjId = embedObjId,
                ScheduleId = -1, // TODO: need to insert the schedule id when schedule is activated
                TakContent = textBoxTakContent.Text,
                TakTitle = textBoxTakTitle.Text,
                TakTypeId = takType
            };
            
            tak = Insert.TableTaktzirim(tak);
            
            return (tak == null) ? -1 : tak.TakId;
        }

        public bool ValidateValuesBeforeSave()
        {

            bool contentOK = (textBoxTakContent.TextLength > 5);
            bool titleOK = (textBoxTakTitle.TextLength > 5);
            if ((( ! contentOK) || ( ! titleOK)) && checkBoxEnableContent.Checked)
                return false;

            return true;
        }

        public bool IsEnabled()
        {
            return (checkBoxTakPhoto.Checked || checkBoxEnableContent.Checked);
        }

        private void textBoxTakTitle_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTakTitle.TextLength > _maxTitleLength)
                textBoxTakTitle.BackColor = System.Drawing.Color.Pink;
            else
                textBoxTakTitle.BackColor = System.Drawing.Color.White;
        }

        private void textBoxTakContent_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTakContent.TextLength > _maxContentLength)
                textBoxTakContent.BackColor = System.Drawing.Color.Pink;
            else
                textBoxTakContent.BackColor = System.Drawing.Color.White;
        }
    }
}

using System;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlEditBottomPageLink : UserControl
    {
        private int _linkId;
        private bool _isNewLink;

        public UserControlEditBottomPageLink(int linkId)
        {
            InitializeComponent();
            _linkId = linkId;
            PopulateFormByLinqData();
        }

        private void PopulateFormByLinqData()
        {
            var link = DbSql.Filter.GetPageBottomLinkByLinkId(_linkId);

            try
            {
                textBoxURL.Text = link.Url;
                textBoxOrder.Text = String.Format("{0}",link.OrderPlace);
                textBoxAlternativeText.Text = link.AltText;
                textBoxDisplayText.Text = link.DispleyText;
                checkBoxLinkStatus.Checked = link.FlagVisible;
                _isNewLink = false;
            }
            catch
            {
                _isNewLink = true;
            }
        }

        private void UserControlEditBottomPageLink_Load(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Table_LinksPageBottom link;

                if (_isNewLink)
                {
                    link = new Table_LinksPageBottom();
                    Lookup.Db.Table_LinksPageBottoms.InsertOnSubmit(link);
                }
                else
                {
                    link = Filter.GetPageBottomLinkByLinkId(_linkId);
                }

                int order;
                bool parsedOK = int.TryParse(textBoxOrder.Text, out order);
                order = (parsedOK) ? order : -1;

                link.Url = textBoxURL.Text;
                link.AltText = textBoxAlternativeText.Text;
                link.BottomUrlCatId = null;
                link.DispleyText = textBoxDisplayText.Text;
                link.FlagVisible = checkBoxLinkStatus.Checked;
                link.OrderPlace = order;

                Lookup.Db.SubmitChanges();
            }
            catch
            {
                
            }
        }
    }
}

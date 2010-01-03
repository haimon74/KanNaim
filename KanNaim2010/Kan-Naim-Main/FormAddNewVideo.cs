using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormAddNewVideo : Form
    {
        public FormAddNewVideo()
        {
            InitializeComponent();
        }

        private void ButtonSaveVideoToArchiveClick(object sender, EventArgs e)
        {
            var newRow = new Table_VideosArchive
            {
                AlternateText = ucUploadVideo1._textBoxVideoAlternateText.Text,
                Caption = ucUploadVideo1._textBoxVideoCaption.Text,
                Description = ucUploadVideo1._textBoxVideoDescription.Text,
                EmbedUrl = ucUploadVideo1._textBoxVideoEmbedUrl.Text,
                CategoryId =
                    DataAccess.Lookup.GetLookupCategoryIdFromName(
                        comboBoxCategory.SelectedText.Trim()),
                        Date = DateTime.Now,
                        UrlLink = "",
                        CssClass = "EmbedVideo"
            };

            GlobalValiables.Db.Table_VideosArchives.InsertOnSubmit(newRow);
            GlobalValiables.Db.SubmitChanges();

            ucUploadVideo1.Clear();
        }

        private void userControlUploadVideo1_Load(object sender, EventArgs e)
        {
            ucUploadVideo1.SetCallbackFunction(ButtonSaveVideoToArchiveClick);
        }

        private void FormAddNewVideo_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetLookupCategories.Table_LookupCategories' table. You can move, or remove it, as needed.
            this.table_LookupCategoriesTableAdapter.Fill(this._10infoDataSetLookupCategories.Table_LookupCategories);

        }
    }
}

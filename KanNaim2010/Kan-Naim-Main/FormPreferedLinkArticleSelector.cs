using System;
using System.Linq;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormPreferedLinkArticleSelector : Form
    {
        public delegate void ArticleSelectedCallback(int articleId);

        private readonly ArticleSelectedCallback _callbackFunction;

        private readonly int _categoryId;

        public FormPreferedLinkArticleSelector(ArticleSelectedCallback callbackFunction, int categoryId)
        {
            InitializeComponent();

            _callbackFunction = callbackFunction;
            _categoryId = categoryId;
        }

        private void FormPreferedLinkPhotoSelector_Load(object sender, EventArgs e)
        {
            // Set the view to show details.
            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = false;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;
            // Display grid lines.
            listView1.GridLines = true;
            // Sort the items in the list in ascending order.
            listView1.Sorting = SortOrder.Ascending;
            listView1.MultiSelect = false;
            listView1.RightToLeftLayout = true;

            // Create columns for the items and subitems.
            listView1.Columns.Add("מספר המאמר", -2, HorizontalAlignment.Center);
            listView1.Columns.Add("כותרת המאמר", -2, HorizontalAlignment.Center);
            listView1.Columns.Add("תאריך עדכון אחרון", -2, HorizontalAlignment.Center);
            listView1.Columns.Add("מספר צפיות", -2, HorizontalAlignment.Center);

            PopulateListView();

            //Add the items to the ListView.
            //listView1.Items.AddRange(itemCollection);

            //// Create two ImageList objects.
            //ImageList imageListSmall = new ImageList();
            //ImageList imageListLarge = new ImageList();

            //// Initialize the ImageList objects with bitmaps.
            //imageListSmall.Images.Add(Image.FromFile("D:\\Pictures\\alon_hatavor_ez_size4_125x117_.jpg"));
            //imageListSmall.Images.Add(Image.FromFile("D:\\Pictures\\n681337191_209344_3937_size5_100x94_.jpg"));
            //imageListLarge.Images.Add(Image.FromFile("D:\\Pictures\\alon_hatavor_ez_size1_230x217_.jpg"));
            //imageListLarge.Images.Add(Image.FromFile("D:\\Pictures\\n681337191_209344_3937_size3_165x155_.jpg"));

            ////Assign the ImageList objects to the ListView.
            //listView1.LargeImageList = imageListLarge;
            //listView1.SmallImageList = imageListSmall;

            //// Add the ListView to the control collection.
            ////this.Controls.Add(listView1);

        }

        private void PopulateListView()
        {
            var itemCollection = new ListView.ListViewItemCollection(listView1);

            var articles = from c in GlobalValiables.Db.Table_Articles
                           where c.CategoryId == _categoryId
                           select c;

            foreach (Table_Article article in articles)
            {
                var item = new ListViewItem();
                item.SubItems.Add(article.ArticleId.ToString());
                item.SubItems.Add(article.Title);
                item.SubItems.Add(article.UpdateDate.ToShortDateString());
                item.SubItems.Add(article.CountViews.ToString());
                itemCollection.Add(item);
            }
            listView1.Refresh();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            _callbackFunction(int.Parse(listView1.SelectedItems[0].Text));
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            listView1_DoubleClick(sender,e);
        }
    }
}

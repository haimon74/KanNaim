using System;
using System.Linq;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlManageArticles : UserControl
    {
        public delegate void EditArticleCallbackFunction(int articleId);

        private EditArticleCallbackFunction _editArticleCallback;

        public UserControlManageArticles(EditArticleCallbackFunction editArticleCallback)
        {
            InitializeComponent();
            PopulateGridView();
            _editArticleCallback = editArticleCallback;
        }

        private void PopulateGridView()
        {
            var queryResult = Filter.GetAllArticles().OrderByDescending(x => x.UpdateDate);
            dataGridView1.DataSource = queryResult;
        }

        private void ToolStripMenuItemEditTak_Click(object sender, EventArgs e)
        {
            var dataSource = (IQueryable<Table_Article>) dataGridView1.DataSource;
            int articleId = dataSource.ToArray()[dataGridView1.CurrentRow.Index].ArticleId;
            
            _editArticleCallback(articleId);
        }

        private void ToolStripMenuItemDeleteTak_Click(object sender, EventArgs e)
        {
            var dataSource = (IQueryable<Table_Article>)dataGridView1.DataSource;
            int articleId = dataSource.ToArray()[dataGridView1.CurrentRow.Index].ArticleId; 
            
            Delete.TableArticle(articleId); 
            
            int delIdx = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(delIdx);
        }

/*
        private void ToolStripMenuItemFilterSizeClick(object sender, EventArgs e)
        {
            string start = "toolStripMenuItemFilterSize";
            string end = ((ToolStripMenuItem) sender).Name.Substring(start.Length).Trim();
            int size;
            bool parsedOK = int.TryParse(end, out size);

            if (parsedOK)
            {
                dataGridView1.DataSource = Filter.GetTaktzirimByTakTypeId(size);
            }
            else
            {
                dataGridView1.DataSource = Filter.GetAllTaktzirim();
            }
        }
*/

        private void ToolStripMenuItemNewArticle_Click(object sender, EventArgs e)
        {
            _editArticleCallback(-1);
        }

        private void ToolStripMenuItemFilterBroadcast_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Filter.GetTaktzirimByBroadcast();
        }
    }
}

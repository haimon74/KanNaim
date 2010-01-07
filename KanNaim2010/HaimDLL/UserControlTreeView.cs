using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HaimDLL
{
    public partial class UserControlTreeView : UserControl
    {
        public UserControlTreeView()
        {
            InitializeComponent();
            _nodes = treeView1.Nodes;
        }

        private void UserControlTreeView_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
        }

        private static string _conStr = Constants.My10InfoConnectionString;

        private SqlConnection _objConn = new SqlConnection(_conStr);
        public SqlConnection ObjConn
        {
            get { return _objConn; }
            set { _objConn = value; }
        }

        private string _myQry = "select * FROM Table_LookupCategories WHERE ParentCatId='-1'";
        public string MyQry
        {
            get { return _myQry; }
            set { _myQry = value; }
        }

        private string _rootNodeName = "עמוד ראשי";
        public string RootNodeName
        {
            get { return _rootNodeName; }
            set { _rootNodeName = value; }
        }

        private string _rootNodeId = "1";
        public string RootNodeId
        {
            get { return _rootNodeId; }
            set { _rootNodeId = value; }
        }

        private const string RootParentId = "-1";

        private string _textColumnName = "CatHebrewName";
        public string TextColumnName
        {
            get { return _textColumnName; }
            set { _textColumnName = value; }
        }

        private string _idColumnName = "CatId";
        public string IdColumnName
        {
            get { return _idColumnName; }
            set { _idColumnName = value; }
        }

        private string _lookupTableName;
        public string LookupTableName
        {
            get { return _lookupTableName; }
            set { _lookupTableName = value; }
        }

        private string _parentIdColumnName;
        public string ParentIdColumnName
        {
            get { return _parentIdColumnName; }
            set { _parentIdColumnName = value; }
        }

        private TreeNodeCollection _nodes = null;
        public TreeNodeCollection Nodes
        {
            get { return _nodes; }
            //set { _nodes = value; }
        }

        private TreeNode _selectedNode;
        public TreeNode SelectedNode
        {
            get { return _selectedNode; }
            //set { _selectedNode = value; }
        }

        public DataTable GetTableFromQuery(string qry)
        {
            SqlCommand objCommand = new SqlCommand(qry, _objConn);
            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public TreeView PopulateRootLevel(string lookupTableName, string parentIdColumnName)
        {
            treeView1.Nodes.Clear();
            return PopulateRootLevel(lookupTableName, parentIdColumnName, TextColumnName, IdColumnName);
        }

        public TreeView PopulateRootLevel(string lookupTableName, string parentIdColumnName, string textColumnName, string idColumnName)
        {
            TextColumnName = textColumnName;
            IdColumnName = idColumnName;
            LookupTableName = lookupTableName;
            ParentIdColumnName = parentIdColumnName;
            MyQry = String.Format("select * FROM {0} WHERE {1}='{2}'", LookupTableName, ParentIdColumnName, RootParentId);
            DataTable dt = GetTableFromQuery(MyQry);

            TreeView tv = treeView1;
            if ((dt == null) || (dt.Rows.Count == 0))
            {
                //// create main category - assuming not relevant for now
                //_linqLookupCategoryTableRow = new Table_LookupCategory();
                //_linqLookupCategoryTableRow.CatHebrewName = "עמוד ראשי";
                //_linqLookupCategoryTableRow.CatEnglishName = "Main|Home Page";
                //_linqLookupCategoryTableRow.ParentCatId = -1;
                //_db.Table_LookupCategories.InsertOnSubmit(_linqLookupCategoryTableRow);
                //_db.SubmitChanges();
                //string siblingQuery = String.Format(MyQry);
                //dt = GetTableFromQuery(siblingQuery);
            }
            PopulateNodes(dt, tv.Nodes);
            return tv;
        }

        private void PopulateNodes(DataTable dt, TreeNodeCollection nodes)
        {
            if ((dt == null) || (dt.Rows.Count == 0))
                return;

            if (nodes == null)
            {
                TreeNode tn = new TreeNode();
                tn.Text = "אין קטגוריות";
                tn.ToolTipText = "-1";
                nodes.Add(tn);
            }

            foreach (DataRow dr in dt.Rows)
            {
                string name = dr[TextColumnName].ToString().Trim();
                string id = dr[IdColumnName].ToString().Trim();
                TreeNode tn = new TreeNode();
                tn.Text = name;
                tn.ToolTipText = id;
                try
                {
                    nodes.Add(tn);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                MyQry = String.Format("select * FROM {0} WHERE {1}='{2}'", LookupTableName, ParentIdColumnName, id);
                DataTable childDT = GetTableFromQuery(MyQry);
                PopulateNodes(childDT, tn.Nodes);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _selectedNode = treeView1.SelectedNode;
        }

    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace UserControlsLibrary.WinControls
{
    [DesignerAttribute(typeof(CustomDataGridViewDesigner))]
    public partial class CustomDataGridView : DataGridView
    {
        private static Dictionary<ColumnEnum, DataGridViewColumn> _columnsDictionary = new Dictionary<ColumnEnum, DataGridViewColumn>();
        private static int _columnIndex = 0;

        public CustomDataGridView()
        {
            InitializeComponent();
            BuildStaticData();
        }

        private void BuildStaticData()
        {
            _columnsDictionary[ColumnEnum.Text10Chars] = Text10ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text15Chars] = Text15ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text20Chars] = Text20ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text25Chars] = Text25ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text30Chars] = Text30ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text50Chars] = Text50ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text100Chars] = Text100ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text250Chars] = Text250ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text500Chars] = Text500ColumnTemplate;
            _columnsDictionary[ColumnEnum.Text1000Chars] = Text1000ColumnTemplate;
            _columnsDictionary[ColumnEnum.CheckBox] = CheckBoxColumnTemplate;
            _columnsDictionary[ColumnEnum.ComboBox] = ComboBoxReadColumnTemplate;
            _columnsDictionary[ColumnEnum.Link] = LinkColumnTemplate;
            _columnsDictionary[ColumnEnum.Number] = NumberColumnTemplate;
            //TODO below
            //_columnsDictionary[ColumnEnum.Real] = RealColumnTemplate;
            //_columnsDictionary[ColumnEnum.Money] = MoneyColumnTemplate;
            _columnsDictionary[ColumnEnum.SmallImage] = SmallImageColumnTemplate;
            //TODO below
            //_columnsDictionary[ColumnEnum.MediumImage] = MediumImageColumnTemplate;
            //_columnsDictionary[ColumnEnum.LargeImage] = LargemageColumnTemplate;
            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


#region Public Section

        public enum ColumnEnum
        {
            Text10Chars,
            Text15Chars,
            Text20Chars,
            Text25Chars,
            Text30Chars,
            Text50Chars,
            Text100Chars,
            Text150Chars,
            Text200Chars,
            Text250Chars,
            Text500Chars,
            Text1000Chars,
            Number,
            Money,
            Real,
            Date,
            ComboBox,
            CheckBox,
            Link,
            SmallImage,
            MediumImage,
            LargeImage
        }
        public struct ColumnProperties
        {
            //public int DisplayIndex;
            //public ColumnEnum TemplateId;
            //public string HeaderText;
            //public string DataPropertyName;
            //public string Name;

            private int _displayIndex;
            private ColumnEnum _templateId;
            private string _headerText;
            private string _dataPropertyName;
            private string _name;

            public ColumnProperties(int idx, string headerText, string dataPropertyName, string columnName, ColumnEnum templateId)
            {
                _displayIndex = idx;
                _templateId = templateId;
                _headerText = headerText;
                _dataPropertyName = dataPropertyName;
                _name = columnName;
            }

            public ColumnProperties(int idx, string headerText, string dataPropertyName, ColumnEnum templateId)
            {
                _displayIndex = idx;
                _templateId = templateId;
                _headerText = headerText;
                _dataPropertyName = dataPropertyName;
                _name = dataPropertyName;
            }

            public int DisplayIndex
            {
                get { return _displayIndex; }
                set { _displayIndex = value; }
            }
            public ColumnEnum TemplateID
            {
                get { return _templateId; }
                set { _templateId = value; }
            }
            public string HeaderText
            {
                get { return _headerText; }
                set { _headerText = value; }
            }
            public string DataPropertyName
            {
                get { return _dataPropertyName; }
                set { _dataPropertyName = value; }
            }
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
        }

        private DataGridViewColumn GetNewColumnTemplate(ColumnProperties cp)
        {
            DataGridViewColumn c = (DataGridViewColumn)
                (_columnsDictionary[cp.TemplateID]).Clone();

            c.DisplayIndex = cp.DisplayIndex;
            c.DataPropertyName = cp.DataPropertyName;
            c.Name = cp.Name ?? cp.DataPropertyName;
            c.HeaderText = cp.HeaderText;
            c.DefaultCellStyle = DefaultCellStyle;
            c.HeaderCell.Style = ColumnHeadersDefaultCellStyle;
            return c;
        }
        private DataGridViewColumn AddNewColumnTemplate(ColumnProperties cp)
        {
            DataGridViewColumn newColumn = GetNewColumnTemplate(cp);
            Columns.Add(newColumn);
            return newColumn;
        }

        public void SetColumnProperties(ColumnProperties[] columnProperties)
        {
            foreach (DataGridViewColumn column in Columns)
            {
                column.Visible = false;
            }
            
            foreach (ColumnProperties cp in columnProperties)
            {

                DataGridViewColumn column = AddNewColumnTemplate(cp);
                
                if (Columns[cp.DataPropertyName].IsDataBound)
                    column.ValueType = Columns[cp.DataPropertyName].ValueType;

                column.Visible = true;
            }
        }

        public void ClearNonVisibleColumns()
        {
            for (int i = Columns.Count-1 ; i >= 0 ; i--)
            {
                if (! Columns[i].Visible)
                    Columns.RemoveAt(i);
            }
        }

        public void SetDataSource(DataSet ds, string tableName, ColumnProperties[] columnProperties)
        {
            DataSource = ds;
            DataMember = tableName;

            SetColumnProperties(columnProperties);
        }
#endregion


#region Non Browsable Properties

        //[BrowsableAttribute(false)]
        //new public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        //{
        //    get { return base.AutoSizeColumnsMode; }
        //    set { base.AutoSizeColumnsMode = value; }
        //}

        //[BrowsableAttribute(false)]
        //new public Color BackgroundColor
        //{
        //    get { return base.BackgroundColor; }
        //    set { base.BackgroundColor = value; }
        //}

        //[BrowsableAttribute(false)]
        //new public DataGridViewCellStyle DefaultCellStyle
        //{
        //    get { return base.DefaultCellStyle; }
        //    set { base.DefaultCellStyle = value; }
        //}

        //[BrowsableAttribute(false)]
        //new public DockStyle Dock
        //{
        //    get { return base.Dock; }
        //    set { base.Dock = value; }
        //}

        //[Browsable(false)]
        //new public Color GridColor
        //{
        //    get { return base.GridColor; }
        //    set { base.GridColor = value; }
        //}

        //[Browsable(false)]
        //new public Point Location
        //{
        //    get { return base.Location; }
        //    set { base.Location = value; }
        //}

        //[Browsable(false)]
        //new public Size MinimumSize
        //{
        //    get { return base.MinimumSize; }
        //    set { base.MinimumSize = value; }
        //}

        //[Browsable(false)]
        //new public bool MultiSelect
        //{
        //    get { return base.MultiSelect; }
        //    set { base.MultiSelect = value; }
        //}

        ////[Browsable(false)]
        ////new public RightToLeft RightToLeft
        ////{
        ////    get { return base.RightToLeft; }
        ////    set { base.RightToLeft = value; }
        ////}

        //[Browsable(false)]
        //new public DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode
        //{
        //    get { return base.RowHeadersWidthSizeMode; }
        //    set { base.RowHeadersWidthSizeMode = value; }
        //}

        //[Browsable(false)]
        //new public DataGridViewSelectionMode SelectionMode
        //{
        //    get { return base.SelectionMode; }
        //    set { base.SelectionMode = value; }
        //}

        //[Browsable(false)]
        //new public bool ShowCellToolTips
        //{
        //    get { return base.ShowCellToolTips; }
        //    set { base.ShowCellToolTips = value; }
        //}

        //[Browsable(false)]
        //new public Size Size
        //{
        //    get { return base.Size; }
        //    set { base.Size = value; }
        //}

        //[Browsable(false)]
        //new public DataGridViewAutoSizeRowsMode AutoSizeRowsMode
        //{
        //    get { return base.AutoSizeRowsMode; }
        //    set { base.AutoSizeRowsMode = value; }
        //}

        //[Browsable(false)]
        //new public BorderStyle BorderStyle
        //{
        //    get { return base.BorderStyle; }
        //    set { base.BorderStyle = value; }
        //}

        //[Browsable(false)]
        //new public DataGridViewCellBorderStyle CellBorderStyle
        //{
        //    get { return base.CellBorderStyle; }
        //    set { base.CellBorderStyle = value; }
        //}

        //[Browsable(false)]
        //new public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
        //{
        //    get { return base.ColumnHeadersHeightSizeMode; }
        //    set { base.ColumnHeadersHeightSizeMode = value; }
        //}

        //[Browsable(false)]
        //new public DataGridViewClipboardCopyMode ClipboardCopyMode
        //{
        //    get { return base.ClipboardCopyMode; }
        //    set { base.ClipboardCopyMode = value; }
        //}

        //[Browsable(false)]
        //new public DataGridViewColumnCollection Columns
        //{
        //    get { return base.Columns; }
        //}
        //[Browsable(false)]
        //new public AnchorStyles Anchor
        //{
        //    get { return base.Anchor; }
        //    set { base.Anchor = value; }
        //}
        //[Browsable(false)]
        //new public object DataSource
        //{
        //    get { return base.DataSource; }
        //    set { base.DataSource = value; }
        //}
        //[Browsable(false)]
        //new public string DataMember
        //{
        //    set { base.DataMember = value; }
        //    get { return base.DataMember; }
        //}

#endregion

        
#region Browsable Properties
#endregion
    }
}

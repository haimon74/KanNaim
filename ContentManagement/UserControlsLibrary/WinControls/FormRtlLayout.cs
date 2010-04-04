using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserControlsLibrary.WinControls
{
    //[DesignerAttribute(typeof(FormRtlLayoutDesigner))]
    public partial class FormRtlLayout : Form
    {
        private Size _minSize = new Size(200,200);
        private Size _maxSize = new Size(0, 0);
        private Color _bgColor = Color.LightGoldenrodYellow;
        private string _text = "טופס";

        public FormRtlLayout()
        {
            InitializeComponent();
        }

        public FormRtlLayout(FormBorderStyle borderStyle, Size minSize,
            Icon icon, Color bgColor, string text)
        {
            InitializeComponent();

            FormBorderStyle = borderStyle;
            _minSize = minSize;
            //_maxSize = maxSize;
            Icon = icon;
            _bgColor = bgColor;
            _text = text;
            //RightToLeft = RightToLeft.Yes;
            //RightToLeftLayout = true;
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void FormRtlLayout_Load(object sender, EventArgs e)
        {
            Text = Text ?? _text;
            MinimumSize = _minSize;
            MaximumSize = _maxSize;
            BackColor = _bgColor;
        }

        [Browsable(false)]
        new public RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }
        [Browsable(false)]
        new public bool RightToLeftLayout
        {
            get { return base.RightToLeftLayout; }
            set { base.RightToLeftLayout = value; }
        }

        [Browsable(false)]
        new public IButtonControl AcceptButton
        {
            get { return base.AcceptButton; }
            set { base.AcceptButton = value; }
        }

        [Browsable(false)]
        new public string AccessibleDescription
        {
            get { return base.AccessibleDescription; }
            set { base.AccessibleDescription = value; }
        }

        [Browsable(false)]
        new public string AccessibleName
        {
            get { return base.AccessibleName; }
            set { base.AccessibleName = value; }
        }

        [Browsable(false)]
        new public AccessibleRole AccessibleRole
        {
            get { return base.AccessibleRole; }
            set { base.AccessibleRole = value; }
        }

        [Browsable(false)]
        new public AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [Browsable(false)]
        new public bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Browsable(false)]
        new public bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = value; }
        }

        [Browsable(false)]
        new public bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = value; }
        }

        [Browsable(false)]
        new public Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { base.AutoScrollMargin = value; }
        }

        [Browsable(false)]
        new public Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { base.AutoScrollMinSize = value; }
        }

        [Browsable(false)]
        new public AutoValidate AutoValidate
        {
            get { return base.AutoValidate; }
            set { base.AutoValidate = value; }
        }

        [Browsable(false)]
        new public IButtonControl CancelButton
        {
            get { return base.CancelButton; }
            set { base.CancelButton = value; }
        }

        [Browsable(false)]
        new public bool CausesValidation
        {
            get { return base.CausesValidation; }
            set { base.CausesValidation = value; }
        }

        [Browsable(false)]
        new public Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        [Browsable(false)]
        new public bool IsMdiContainer
        {
            get { return base.IsMdiContainer; }
            set { base.IsMdiContainer = value; }
        }

        [Browsable(false)]
        new public bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        [Browsable(false)]
        new public ImeMode ImeMode
        {
            get { return base.ImeMode; }
            set { base.ImeMode = value; }
        }

        [Browsable(false)]
        new public Point Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }

        [Browsable(false)]
        public bool Locked
        {
            get { return Locked; }
            set { Locked = value; }
        }

        [Browsable(false)]
        new public Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [Browsable(false)]
        new public Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        [Browsable(false)]
        new public bool ShowIcon
        {
            get { return base.ShowIcon; }
            set { base.ShowIcon = value; }
        }

        [Browsable(false)]
        new public object Tag
        {
            get { return base.Tag; }
            set { base.Tag = value; }
        }

        [Browsable(false)]
        new public bool HelpButton
        {
            get { return base.HelpButton; }
            set { base.HelpButton = value; }
        }

        [Browsable(false)]
        new public bool UseWaitCursor
        {
            get { return base.UseWaitCursor; }
            set { base.UseWaitCursor = value; }
        }

        [Browsable(false)]
        new public bool TopMost
        {
            get { return base.TopMost; }
            set { base.TopMost = value; }
        }

        [Browsable(false)]
        new public bool KeyPreview
        {
            get { return base.KeyPreview; }
            set { base.KeyPreview = value; }
        }

        [Browsable(false)]
        new public Color TransparencyKey
        {
            get { return base.TransparencyKey; }
            set { base.TransparencyKey = value; }
        }

        [Browsable(false)]
        new public FormWindowState WindowState
        {
            get { return base.WindowState; }
            set { base.WindowState = value; }
        }
    }
}

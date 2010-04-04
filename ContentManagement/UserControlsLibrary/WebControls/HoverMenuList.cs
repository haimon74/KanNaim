using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;


[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    ////[ParseChildren(true, "Items")]
    //[ParseChildren(true, "HoverPanels")]
    //[DefaultProperty("Text")]
    //[ToolboxData("<{0}:HoverMenuList runat=server></{0}:HoverMenuList>")]
    //public class HoverMenuList : ListBox
    //{
    //    private Collection<HoverMenuPanel> _hoverPanels;

    //    [PersistenceMode(PersistenceMode.InnerProperty)]
    //    public Collection<HoverMenuPanel> HoverPanels
    //    {
    //        get { return _hoverPanels; }
    //        set { _hoverPanels = value; }
    //    }

    //    private ListItemCollection _items;

    //    //[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    //    public virtual ListItemCollection Items
    //    {
    //        get { return this._items; }
    //    } 
        
    //    private string _menuItemCss;
    //    private string _panelItemCss;
    //    //private string _hoverCss;
    //    private string _markHoverScriptName;
    //    private string _cancelHoverScriptName;

    //    [Bindable(true)]
    //    [Category("Appearance")]
    //    [DefaultValue("")]
    //    [Localizable(true)]
    //    public string Text
    //    {
    //        get
    //        {
    //            String s = (String)ViewState["Text"];
    //            return ((s == null) ? String.Empty : s);
    //        }

    //        set
    //        {
    //            ViewState["Text"] = value;
    //        }
    //    }
        
    //    public string MenuItemCss
    //    {
    //        get { return _menuItemCss; }
    //        set { _menuItemCss = value; }
    //    }

    //    public string PanelItemCss
    //    {
    //        get { return _panelItemCss; }
    //        set { _panelItemCss = value; }
    //    }

    //    //public string HoverCss
    //    //{
    //    //    get { return _hoverCss; }
    //    //    set { _hoverCss = value; }
    //    //}

    //    public string MarkHoverScriptName
    //    {
    //        get { return _markHoverScriptName; }
    //        set { _markHoverScriptName = value; }
    //    }

    //    public string CancelHoverScriptName
    //    {
    //        get { return _cancelHoverScriptName; }
    //        set { _cancelHoverScriptName = value; }
    //    }

    //    protected void Page_Init(object sender, EventArgs e)
    //    {
    //        foreach (var item in Items)
    //        {
    //            if (item is Button)
    //            {
    //                Button btn = item as Button;
    //                btn.CssClass = MenuItemCss;
    //                btn.Attributes.Add("onmouseover", MarkHoverScriptName);
    //                btn.Attributes.Add("onmouseout", CancelHoverScriptName);
    //            }
    //        }
    //        foreach (HoverMenuPanel panel in HoverPanels)
    //        {
    //            if (panel != null)
    //            {
    //                foreach (Button btn in panel.PanelItems)
    //                {
    //                    if (btn != null)
    //                    {
    //                        btn.CssClass = PanelItemCss;
    //                        btn.Attributes.Add("onmouseover", MarkHoverScriptName);
    //                        btn.Attributes.Add("onmouseout", CancelHoverScriptName);
    //                    }
    //                }
    //            }
    //        }
    //    } 
    //    protected override void RenderContents(HtmlTextWriter output)
    //    {
    //        output.Write(Text);
    //    }
    //}

    //[ParseChildren(true, "PanelItems")]
    //public class HoverMenuPanel : Panel
    //{
    //    private Collection<Button> _panelItems = new Collection<Button>();

    //    //[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    //    public Collection<Button> PanelItems
    //    {
    //        get { return _panelItems; }
    //        set { _panelItems = value; }
    //    }

    //    private string _url;
    //    protected void Page_Init(object sender, EventArgs e)
    //    {
    //        PanelItems.Clear();

    //        foreach (Button newItem in PanelItems)
    //        {
    //            newItem.ToolTip = newItem.Text;
    //            newItem.Attributes.Add("OnClick", newItem.ToolTip);
    //            this.Controls.Add(newItem);
    //        }
    //    }
    //}


    [ControlBuilder(typeof(QueryControlBuilder))]
    public class HoverMenu : Control 
    {
        private ArrayList _menuSettings = new ArrayList();
        private ArrayList _hoverPanels = new ArrayList();
        //
        
        protected override void AddParsedSubObject(Object obj) 
        {
            
            if (obj is HoverPanel)
                _hoverPanels.Add((HoverPanel) obj);

            if (obj is MenuSettings)
                _menuSettings.Add((MenuSettings)obj);

            //if (obj is HoverItem)
              //  _hoverItems.Add((HoverItem) obj);

        }
    }

    public class MenuSettings : Control, IAttributeAccessor
    {
        private string _staticItemCss;
        private string _panelItemCss;
        private string _panelMenuCss;
        private string _markHoverFunction;
        private string _restoreHoverFunction;

        public string StaticItemCss
        {
            get { return _staticItemCss; }
            set { _staticItemCss = value; }
        }

        public string PanelItemCss
        {
            get { return _panelItemCss; }
            set { _panelItemCss = value; }
        }

        public string PanelMenuCss
        {
            get { return _panelMenuCss; }
            set { _panelMenuCss = value; }
        }

        public string MarkHoverFunction
        {
            get { return _markHoverFunction; }
            set { _markHoverFunction = value; }
        }

        public string RestoreHoverFunction
        {
            get { return _restoreHoverFunction; }
            set { _restoreHoverFunction = value; }
        }

        private StringDictionary _otherAttributes = new StringDictionary();
        public StringDictionary Attributes
        {
            get { return _otherAttributes; }
        }

        public string GetAttribute(string key)
        {
            return _otherAttributes[key];
        }

        public void SetAttribute(string key, string value)
        {
            _otherAttributes[key] = value;
        }
    }
    public class HoverPanel : Control, IAttributeAccessor
    {
        private string _name;
        private ArrayList _hoverItems = new ArrayList();
        
        public string Name {
            get {return _name;}
            set {_name = value;}
        }

        protected override void AddParsedSubObject(Object obj)
        {
            if (obj is Button)
              _hoverItems.Add((Button) obj);
        }
        
        private StringDictionary _otherAttributes = new StringDictionary();
        public StringDictionary Attributes
        {
            get { return _otherAttributes; }
        }

        public string GetAttribute(string key)
        {
            return _otherAttributes[key];
        }

        public void SetAttribute(string key, string value)
        {
            _otherAttributes[key] = value;
        }
    }

    
    public class QueryControlBuilder : ControlBuilder
    {
        public override Type GetChildControlType(String tagName,
                                         IDictionary attributes)
        {
            if (String.Compare(tagName, "hoverPanel", true) == 0) 
                return typeof(HoverPanel);
            
            if (String.Compare(tagName, "button", true) == 0) 
                return typeof(Button);


            // if any other tag name is used, throw an exception

            throw new Exception(
                "The <HoverMenu> tag should only contain <HoverPanel>, <HoverItem> tags.");
        }     
    }



}

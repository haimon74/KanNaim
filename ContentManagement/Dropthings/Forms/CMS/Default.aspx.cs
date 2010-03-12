
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using DashboardBusiness;
using DashboardDataAccess;
using Page = DashboardDataAccess.Page;
using Dropthings.Web.Util;


public partial class _Default : BasePage
{
    private const string WIDGET_CONTAINER = "WidgetContainer.ascx";
    private string[] updatePanelIDs = new string[] { "LeftUpdatePanel", "MiddleUpdatePanel", "RightUpdatePanel" };

    private UserPageSetup _Setup 
    { 
        get { return Context.Items[typeof(UserPageSetup)] as UserPageSetup; } 
        set { Context.Items[typeof(UserPageSetup)] = value; }
    }

    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if( val == null ) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Check if revisit is valid or not
        if( !base.IsPostBack ) 
        {
            // Block cookie less visit attempts
            if( Profile.IsFirstVisit )
            {
                if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.FirstVisit))  Response.End();
            }
            else
            {
                if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Revisit) )  Response.End();
            }
        }
        else
        {
            // Limit number of postbacks
            if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Postback) ) Response.End();
        }
    }

    protected override void CreateChildControls()
    {
        base.CreateChildControls();        
        
        this.LoadUserPageSetup(false);
        
        this.LoadAddStuff();
            
        this.WidgetPanelsLayout.SetLayout(_Setup.CurrentPage.LayoutType);
        
        // First visit, non postback            
        this.SetupWidgets(wi => !ScriptManager.GetCurrent(Page).IsInAsyncPostBack);
        this.SetupTabs();                    
    }


    private void LoadUserPageSetup(bool noCache)
    {
        // If URL has the page title, load that page by default
        string pageTitle = (Request.Url.Query ?? string.Empty).TrimStart('?');

        if( Profile.IsAnonymous )
        {
            if( Profile.IsFirstVisit )
            {
                // First visit
                Profile.IsFirstVisit = false;
                Profile.Save();
                
                _Setup = new DashboardFacade(Profile.UserName).NewUserVisit();
            }
            else
            {
                // OMAR Apr 5, 2008: Turning off cache because sometimes page setups get out of sync with the cached item
                //_Setup = Cache[Profile.UserName] as UserPageSetup;
                //if( noCache || null == _Setup )
                    _Setup = new DashboardFacade(Profile.UserName).LoadUserSetup(pageTitle);
            }
        }
        else
        {
            // OMAR Apr 5, 2008: Turning off cache because sometimes page setups get out of sync with the cached item
            //_Setup = Cache[Profile.UserName] as UserPageSetup;
            //if( noCache || null == _Setup )
                _Setup = new DashboardFacade(Profile.UserName).LoadUserSetup(pageTitle);
        }

        // Cache the user setup in order to avoid repeated loading during postback
        //Cache[Profile.UserName] = _Setup;
    }

    
    private void SetupTabs()
    {
        tabList.Controls.Clear();

        var setup = _Setup;
        var currentPage = setup.CurrentPage;

        foreach( Page page in setup.Pages )
        {
            var li = new HtmlGenericControl("li");
            li.ID = "Tab" + page.ID.ToString();
            li.Attributes["class"] = "tab " + (page.ID == currentPage.ID ? "activetab" : "inactivetab");

            var liWrapper = new HtmlGenericControl("div");
            li.Controls.Add(liWrapper);
            liWrapper.Attributes["class"] = "tab_wrapper";

            if (page.ID == currentPage.ID)
            {
                /*
                var linkButton = new LinkButtonWithLayout();
                linkButton.ID = page.ID.ToString();
                linkButton.Text = page.Title;
                linkButton.CommandName = "ChangePage";
                linkButton.CommandArgument = page.ID.ToString();
            
                linkButton.Click += new EventHandler(PageTitleEditMode_Click);
                liWrapper.Controls.Add(linkButton);                
                */
                var tabTextDiv = new HtmlGenericControl("span");
                tabTextDiv.InnerText = page.Title;
                liWrapper.Controls.Add(tabTextDiv);
            }
            else
            {
                var tabLink = new HyperLink { Text = page.Title, NavigateUrl = "/?" + page.TabName() };
                liWrapper.Controls.Add(tabLink);
            }                        
            tabList.Controls.Add(li);
        }

        var addNewTabLinkButton = new LinkButton();
        addNewTabLinkButton.ID = "AddNewPage";
        addNewTabLinkButton.Text = "new tab";
        addNewTabLinkButton.Click += new EventHandler(addNewTabLinkButton_Click);
        var li2 = new HtmlGenericControl("li");
        li2.Attributes["class"] = "newtab";
        li2.Controls.Add(addNewTabLinkButton);
        tabList.Controls.Add(li2);

    }

    private void RedirectToTab(Page page)
    {
        Response.Redirect('?' + page.TabName());
    }

    void addNewTabLinkButton_Click(object sender, EventArgs e)
    {
        var page = new DashboardFacade(Profile.UserName).AddNewPage("1");
        RedirectToTab(page);
    }
    private void SetupWidgets(Func<WidgetInstance, bool> isWidgetFirstLoad)
    {
        var setup = _Setup;

        var columnPanels = new Panel[] { 
            this.WidgetPanelsLayout.FindControl("LeftPanel") as Panel, 
            this.WidgetPanelsLayout.FindControl("MiddlePanel") as Panel, 
            this.WidgetPanelsLayout.FindControl("RightPanel") as Panel };

        // Clear existing widgets if any
        foreach( Panel panel in columnPanels )
        {
            List<WidgetContainer> widgets = panel.Controls.OfType<WidgetContainer>().ToList();
            foreach( var widget in widgets ) panel.Controls.Remove( widget );
        }

        foreach( WidgetInstance instance in setup.WidgetInstances )
        {
            var panel = columnPanels[instance.ColumnNo];
            
            var widget = LoadControl(WIDGET_CONTAINER) as WidgetContainer;
            widget.ID = "WidgetContainer" + instance.Id.ToString();
            widget.IsFirstLoad = isWidgetFirstLoad(instance);
            widget.WidgetInstance = instance;
            
            widget.Deleted += new Action<WidgetInstance>(widget_Deleted);

            try
            {

                panel.Controls.Add(widget);
            }
            catch { }
        }

    }

    void widget_Deleted(WidgetInstance obj)
    {
        new DashboardFacade(Profile.UserName).DeleteWidgetInstance(obj);
        
        this.ReloadPage(wi => false);

        this.RefreshColumn(obj.ColumnNo);
    }

    private void ReloadPage(Func<WidgetInstance, bool> isWidgetFirstLoad)
    {
        this.LoadUserPageSetup(true);
        this.SetupTabs();        
        
        this.SetupWidgets(isWidgetFirstLoad);
    }

    private void RefreshAllColumns()
    {
        this.RefreshColumn(0);
        this.RefreshColumn(1);
        this.RefreshColumn(2);
    }

    private void RefreshColumn(int columnNo)
    {
        var updatePanel = this.WidgetPanelsLayout.FindControl(this.updatePanelIDs[columnNo]) as UpdatePanel;
        updatePanel.Update();
    }

    protected void ShowAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = true;
        this.HideAddContentPanel.Visible = true;
        this.ShowAddContentPanel.Visible = false;

        this.LoadAddStuff();
    }
    
    protected void HideAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = false;
        this.HideAddContentPanel.Visible = false;
        this.ShowAddContentPanel.Visible = true;
    }

    private List<Widget> WidgetList
    {
        get
        {
            List<Widget> widgets = Cache["Widgets"] as List<Widget>;
            if( null == widgets )
            {
                widgets = new DashboardFacade(Profile.UserName).GetWidgetList();
                Cache["Widgets"] = widgets;
            }
        
            return widgets;
        }
    }

    private void LoadAddStuff()
    {
        if (this.AddContentPanel.Visible)
        {
            this.WidgetDataList.ItemCommand += new DataListCommandEventHandler(WidgetDataList_ItemCommand);

            var itemsToShow = WidgetList.Skip(AddStuffPageIndex * 30).Take(30);
            this.WidgetDataList.DataSource = itemsToShow;
            this.WidgetDataList.DataBind();

            this.WidgetListPreviousLinkButton.Visible = AddStuffPageIndex > 0;
            this.WidgetListNextButton.Visible = AddStuffPageIndex * 30 + 30 < WidgetList.Count;
        }
    }

    void WidgetDataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.AddNewWidget) ) return;

        int widgetId = int.Parse( e.CommandArgument.ToString() );

        DashboardFacade facade = new DashboardFacade(Profile.UserName);
        WidgetInstance newWidget = facade.AddWidget( widgetId );

        /// User added a new widget. The new widget is loaded for the first time. So, it's not 
        /// a postback experience for the widget. But for rest of the widgets, it is a postback experience.
        this.ReloadPage(wi => wi.Id == newWidget.Id);
        this.RefreshColumn(newWidget.ColumnNo); // Refresh the middle column where the new widget is added
    }

    void WidgetDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
        {
            Widget widget = e.Item.DataItem as Widget;

            LinkButton link = e.Item.Controls.OfType<LinkButton>().Single();

            link.Text = widget.Name;

            link.CommandName = "AddWidget";
            link.CommandArgument = widget.ID.ToString();
        }
    }

    void AddWidgetLink_Click(object sender, EventArgs e)
    {
        
    }
    protected void WidgetListPreviousLinkButton_Click(object sender, EventArgs e)
    {
        if( 0 == this.AddStuffPageIndex ) 
            return;
        
        this.AddStuffPageIndex --;

        this.LoadAddStuff();
    }
    protected void WidgetListNextButton_Click(object sender, EventArgs e)
    {
        this.AddStuffPageIndex ++;
        this.LoadAddStuff();
    }

    protected void ChangeTabSettingsLinkButton_Clicked(object sender, EventArgs e)
    {
        if (this.ChangePageSettingsPanel.Visible)
            this.HideChangeSettingsPanel();
        else
            this.ShowChangeSettingsPanel();
    }

    protected void SaveNewTitleButton_Clicked(object sender, EventArgs e)
    {
        var newTitle = this.NewTitleTextBox.Text.Trim();

        if (newTitle != _Setup.CurrentPage.Title)
        {
            new DashboardFacade(Profile.UserName).ChangePageName(newTitle);

            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
        
    }

    protected void DeleteTabLinkButton_Clicked(object sender, EventArgs e)
    {
        var currentPage = new DashboardFacade(Profile.UserName).DeleteCurrentPage(_Setup.CurrentPage.ID);
        Context.Cache.Remove(Profile.UserName);

        RedirectToTab(currentPage);
    }

    private void ShowChangeSettingsPanel()
    {
        this.ChangePageSettingsPanel.Visible = true;
        this.ChangePageTitleLinkButton.Text = "Hide Settings »";

        this.NewTitleTextBox.Text = _Setup.CurrentPage.Title;
    }

    private void HideChangeSettingsPanel()
    {
        this.ChangePageSettingsPanel.Visible = false;
        this.ChangePageTitleLinkButton.Text = "Change Settings";
    }
}

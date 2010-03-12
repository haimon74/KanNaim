
using System;
using System.Web.UI;
using DashboardDataAccess;
using Dropthings.Widget.Framework;

public partial class WidgetContainer : System.Web.UI.UserControl, IWidgetHost
{
    public event Action<WidgetInstance> Deleted;

    public bool SettingsOpen
    {
        get 
        {
            object val = ViewState[this.ClientID + "_SettingsOpen"] ?? false;
            return (bool)val;
        }
        set { ViewState[this.ClientID + "_SettingsOpen"] = value; }
    }

    private WidgetInstance _WidgetInstance;

    public WidgetInstance WidgetInstance
    {
        get { return _WidgetInstance; }
        set { _WidgetInstance = value; }
    }

    public Widget WidgetDef { get; set; }
	
    private IWidget _WidgetRef;

    private bool _IsFirstLoad;

    public bool IsFirstLoad
    {
        get { return _IsFirstLoad; }
        set { _IsFirstLoad = value; }
    }
	

    protected void Page_Load(object sender, EventArgs e)
    {
        WidgetTitle.Text = this.WidgetInstance.Title;
        this.SetExpandCollapseButtons();

        //this.CloseWidget.OnClientClick = "DeleteWarning.show( function() { __doPostBack('" + this.CloseWidget.UniqueID+ "','') }, Function.emptyFunction ); return false; ";
        this.CloseWidget.OnClientClick = "DeleteWarning.show( function() { DropthingsUI.Actions.deleteWidget('" + this.WidgetInstance.Id + "')}, Function.emptyFunction ); return false; ";
        this.CollapseWidget.OnClientClick = "DropthingsUI.Actions.minimizeWidget('" + this.WidgetBodyPanel.ClientID + "')";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var widget = LoadControl(this.WidgetInstance.Widget.Url);        
        widget.ID = "Widget" + this.WidgetInstance.Id.ToString();
        
        //WidgetBodyUpdatePanel.ContentTemplateContainer.Controls.Add(widget);
        WidgetBodyPanel.Controls.Add(widget);
        this._WidgetRef = widget as IWidget;
        this._WidgetRef.Init(this);
    }

    private void SetExpandCollapseButtons()
    {
        if (!this.WidgetInstance.Expanded)
        {
            ExpandWidget.Visible = true;
            CollapseWidget.Visible = false;
            WidgetBodyPanel.Visible = false;            
        }
        else
        {
            ExpandWidget.Visible = false;
            CollapseWidget.Visible = true;
            WidgetBodyPanel.Visible = true;
        }
    }

    protected void EditWidget_Click(object sender, EventArgs e)
    {
        if( this.SettingsOpen )
        {
            (this as IWidgetHost).HideSettings();
        }
        else
        {
            (this as IWidgetHost).ShowSettings();
        }

        WidgetBodyUpdatePanel.Update();
    }

    protected void CollapseWidget_Click(object sender, EventArgs e)
    {
        (this as IWidgetHost).Minimize();
    }

    protected void ExpandWidget_Click(object sender, EventArgs e)
    {
        (this as IWidgetHost).Maximize();
    }

    protected void CloseWidget_Click(object sender, EventArgs e)
    {
        this._WidgetRef.Closed();
        (this as IWidgetHost).Close();        
    }

    protected void SaveWidgetTitle_Click(object sender, EventArgs e)
    {
        WidgetTitleTextBox.Visible = SaveWidgetTitle.Visible = false;
        WidgetTitle.Visible = true;
        WidgetTitle.Text = WidgetTitleTextBox.Text;

        DetachAssociation(new Action(delegate()
        {
            DatabaseHelper.Update<WidgetInstance>(this.WidgetInstance, delegate(WidgetInstance wi)
            {
                wi.Title = WidgetTitleTextBox.Text;
            });
        }));
    }

    protected void WidgetTitle_Click(object sender, EventArgs e)
    {
        WidgetTitleTextBox.Text = this.WidgetInstance.Title;
        WidgetTitleTextBox.Visible = true;
        SaveWidgetTitle.Visible = true;
        WidgetTitle.Visible = false;
    }

    protected void CancelEditWidget_Click(object sender, EventArgs e)
    {

    }

    int IWidgetHost.ID
    {
        get
        {
            return this.WidgetInstance.Id;
        }
    }

    void IWidgetHost.Maximize()
    {
        DetachAssociation(new Action(delegate()
        {
            DatabaseHelper.Update<WidgetInstance>(this.WidgetInstance, delegate(WidgetInstance i)
            {
                i.Expanded = true;
            });
        }));


        this.SetExpandCollapseButtons();
        this._WidgetRef.Maximized();

        WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();
    }

    void IWidgetHost.Minimize()
    {
        DetachAssociation(new Action(delegate()
        {
            DatabaseHelper.Update<WidgetInstance>(this.WidgetInstance, delegate(WidgetInstance i)
                {
                    i.Expanded = false;
                });
        }));
        
        this.SetExpandCollapseButtons();
        this._WidgetRef.Minimized();

        WidgetBodyUpdatePanel.Update();        
        WidgetHeaderUpdatePanel.Update();
    }

    void IWidgetHost.Close()
    {
        Deleted(this.WidgetInstance);        
    }

    public override void RenderControl(HtmlTextWriter writer)
    {
        writer.AddAttribute("InstanceId", this.WidgetInstance.Id.ToString());
        base.RenderControl(writer);
    }

    void IWidgetHost.SaveState(string state)
    {
        DetachAssociation(new Action(delegate()
        {
            DatabaseHelper.Update<WidgetInstance>(this.WidgetInstance, delegate(WidgetInstance i)
            {
                i.State = state;
            });
        }));

        // Invalidate cache because widget's state is stored in cache
        Cache.Remove(Profile.UserName);
    }

    /// <summary>
    /// Detach associated objects from WidgetInstance so that
    /// they do not get inserted again
    /// </summary>
    /// <param name="a"></param>
    private void DetachAssociation(Action a)
    {
        var pageRef = this.WidgetInstance.Page;
        var widgetRef = this.WidgetInstance.Widget;
        
        this.WidgetInstance.Detach();

        a.Invoke();

        this.WidgetInstance.Detach();

        this.WidgetInstance.Page = pageRef;
        this.WidgetInstance.Widget = widgetRef;
    }

    string IWidgetHost.GetState()
    {
        return this.WidgetInstance.State;
    }

    bool IWidgetHost.IsFirstLoad
    {
        get
        {
            return this.IsFirstLoad;
        }
    }

    void IWidgetHost.ShowSettings()
    {
        this.SettingsOpen = true;
        this._WidgetRef.ShowSettings();
        (this as IWidgetHost).Maximize();
        EditWidget.Visible = false;
        CancelEditWidget.Visible = true;
        this.WidgetHeaderUpdatePanel.Update();
    }

    void IWidgetHost.HideSettings()
    {
        this.SettingsOpen = false;
        this._WidgetRef.HideSettings();
        EditWidget.Visible = true;
        CancelEditWidget.Visible = false;
        this.WidgetHeaderUpdatePanel.Update();
    }

}

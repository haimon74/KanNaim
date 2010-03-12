
namespace DashboardBusiness
{
	partial class AddWidgetWorkflow
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.AddWidgetOnCurrentPage = new DashboardBusiness.Activities.AddWidgetOnPage();
            this.PushDownColumn = new DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity();
            this.GetThePageToAddWidget = new DashboardBusiness.Activities.GetUserSettingActivity();
            this.GetUserGuid = new DashboardBusiness.Activities.GetUserGuidActivity();
            // 
            // AddWidgetOnCurrentPage
            // 
            this.AddWidgetOnCurrentPage.Name = "AddWidgetOnCurrentPage";
            activitybind1.Name = "AddWidgetWorkflow";
            activitybind1.Path = "NewWidget";
            activitybind2.Name = "GetThePageToAddWidget";
            activitybind2.Path = "UserSetting.CurrentPageId";
            activitybind3.Name = "AddWidgetWorkflow";
            activitybind3.Path = "WidgetId";
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.AddWidgetOnPage)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetId", typeof(DashboardBusiness.Activities.AddWidgetOnPage)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewWidget", typeof(DashboardBusiness.Activities.AddWidgetOnPage)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // PushDownColumn
            // 
            this.PushDownColumn.ColumnNo = 0;
            this.PushDownColumn.Name = "PushDownColumn";
            activitybind4.Name = "GetThePageToAddWidget";
            activitybind4.Path = "UserSetting.CurrentPageId";
            this.PushDownColumn.Position = 0;
            this.PushDownColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // GetThePageToAddWidget
            // 
            this.GetThePageToAddWidget.CurrentPage = null;
            this.GetThePageToAddWidget.Name = "GetThePageToAddWidget";
            activitybind5.Name = "GetUserGuid";
            activitybind5.Path = "UserGuid";
            this.GetThePageToAddWidget.UserSetting = null;
            this.GetThePageToAddWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind6.Name = "AddWidgetWorkflow";
            activitybind6.Path = "UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // AddWidgetWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetThePageToAddWidget);
            this.Activities.Add(this.PushDownColumn);
            this.Activities.Add(this.AddWidgetOnCurrentPage);
            this.Name = "AddWidgetWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity PushDownColumn;
        private DashboardBusiness.Activities.AddWidgetOnPage AddWidgetOnCurrentPage;
        private DashboardBusiness.Activities.GetUserSettingActivity GetThePageToAddWidget;
        private DashboardBusiness.Activities.GetUserGuidActivity GetUserGuid;












    }
}

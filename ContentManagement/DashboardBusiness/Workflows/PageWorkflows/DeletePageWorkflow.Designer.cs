using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace DashboardBusiness
{
	partial class DeletePageWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            this.DeleteWidgetInstance = new DashboardBusiness.Activities.DeleteWidgetInstanceActivity();
            this.SetCurrentPage = new DashboardBusiness.Activities.SetCurrentPageActivity();
            this.DecideCurrentPage = new DashboardBusiness.Activities.DecideCurrentPageActivity();
            this.DeletePage = new DashboardBusiness.Activities.DeletePageActivity();
            this.DeleteEachWidgetInstance = new ForEachActivity.ForEach();
            this.LoadWidgetsOfPage = new DashboardBusiness.Activities.GetWidgetsInPageActivity();
            this.GetUserGUID = new DashboardBusiness.Activities.GetUserGuidActivity();
            // 
            // DeleteWidgetInstance
            // 
            this.DeleteWidgetInstance.Name = "DeleteWidgetInstance";
            this.DeleteWidgetInstance.WidgetInstance = null;
            // 
            // SetCurrentPage
            // 
            this.SetCurrentPage.Description = "Set the current page after deleting the current page";
            this.SetCurrentPage.Name = "SetCurrentPage";
            activitybind1.Name = "DeletePageWorkflow";
            activitybind1.Path = "CurrentPageId";
            activitybind2.Name = "GetUserGUID";
            activitybind2.Path = "UserGuid";
            this.SetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // DecideCurrentPage
            // 
            activitybind3.Name = "DeletePageWorkflow";
            activitybind3.Path = "NewCurrentPage";
            activitybind4.Name = "DeletePageWorkflow";
            activitybind4.Path = "CurrentPageId";
            this.DecideCurrentPage.Name = "DecideCurrentPage";
            this.DecideCurrentPage.PageName = null;
            activitybind5.Name = "GetUserGUID";
            activitybind5.Path = "UserGuid";
            this.DecideCurrentPage.SetBinding(DashboardBusiness.Activities.DecideCurrentPageActivity.CurrentPageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.DecideCurrentPage.SetBinding(DashboardBusiness.Activities.DecideCurrentPageActivity.CurrentPageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.DecideCurrentPage.SetBinding(DashboardBusiness.Activities.DecideCurrentPageActivity.UserGuidProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // DeletePage
            // 
            this.DeletePage.Name = "DeletePage";
            activitybind6.Name = "DeletePageWorkflow";
            activitybind6.Path = "PageID";
            activitybind7.Name = "GetUserGUID";
            activitybind7.Path = "UserGuid";
            this.DeletePage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.DeletePageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.DeletePage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.DeletePageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            activitybind8.Name = "DeletePageWorkflow";
            activitybind8.Path = "WidgetInstances";
            // 
            // DeleteEachWidgetInstance
            // 
            this.DeleteEachWidgetInstance.Activities.Add(this.DeleteWidgetInstance);
            this.DeleteEachWidgetInstance.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.DeleteEachWidgetInstance.Name = "DeleteEachWidgetInstance";
            this.DeleteEachWidgetInstance.Iterating += new System.EventHandler(this.OnForEach);
            this.DeleteEachWidgetInstance.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // LoadWidgetsOfPage
            // 
            this.LoadWidgetsOfPage.Name = "LoadWidgetsOfPage";
            activitybind9.Name = "DeletePageWorkflow";
            activitybind9.Path = "PageID";
            this.LoadWidgetsOfPage.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind10.Name = "DeletePageWorkflow";
            activitybind10.Path = "WidgetInstances";
            this.LoadWidgetsOfPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.GetWidgetsInPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.LoadWidgetsOfPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstances", typeof(DashboardBusiness.Activities.GetWidgetsInPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // GetUserGUID
            // 
            this.GetUserGUID.Name = "GetUserGUID";
            this.GetUserGUID.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind11.Name = "DeletePageWorkflow";
            activitybind11.Path = "UserName";
            this.GetUserGUID.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // DeletePageWorkflow
            // 
            this.Activities.Add(this.GetUserGUID);
            this.Activities.Add(this.LoadWidgetsOfPage);
            this.Activities.Add(this.DeleteEachWidgetInstance);
            this.Activities.Add(this.DeletePage);
            this.Activities.Add(this.DecideCurrentPage);
            this.Activities.Add(this.SetCurrentPage);
            this.Name = "DeletePageWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.DeleteWidgetInstanceActivity DeleteWidgetInstance;
        private ForEachActivity.ForEach DeleteEachWidgetInstance;
        private DashboardBusiness.Activities.GetWidgetsInPageActivity LoadWidgetsOfPage;
        private DashboardBusiness.Activities.SetCurrentPageActivity SetCurrentPage;
        private DashboardBusiness.Activities.DecideCurrentPageActivity DecideCurrentPage;
        private DashboardBusiness.Activities.DeletePageActivity DeletePage;
        private DashboardBusiness.Activities.GetUserGuidActivity GetUserGUID;






















































    }
}

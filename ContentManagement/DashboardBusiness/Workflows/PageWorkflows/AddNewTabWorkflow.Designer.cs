// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

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
	partial class AddNewTabWorkflow
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
            this.SetNewPageAsCurrent = new DashboardBusiness.Activities.SetCurrentPageActivity();
            this.CreateNewPage = new DashboardBusiness.Activities.CreateNewPageActivity();
            this.GetUserGuid = new DashboardBusiness.Activities.GetUserGuidActivity();
            // 
            // SetNewPageAsCurrent
            // 
            this.SetNewPageAsCurrent.Name = "SetNewPageAsCurrent";
            activitybind1.Name = "CreateNewPage";
            activitybind1.Path = "NewPageId";
            activitybind2.Name = "GetUserGuid";
            activitybind2.Path = "UserGuid";
            this.SetNewPageAsCurrent.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SetNewPageAsCurrent.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // CreateNewPage
            // 
            activitybind3.Name = "AddNewTabWorkflow";
            activitybind3.Path = "LayoutType";
            this.CreateNewPage.Name = "CreateNewPage";
            activitybind4.Name = "AddNewTabWorkflow";
            activitybind4.Path = "NewPage";
            this.CreateNewPage.NewPageId = 0;
            this.CreateNewPage.Title = "New Page";
            activitybind5.Name = "GetUserGuid";
            activitybind5.Path = "UserGuid";
            this.CreateNewPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(DashboardBusiness.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.CreateNewPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(DashboardBusiness.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.CreateNewPage.SetBinding(DashboardBusiness.Activities.CreateNewPageActivity.NewPageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind6.Name = "AddNewTabWorkflow";
            activitybind6.Path = "UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // AddNewTabWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.CreateNewPage);
            this.Activities.Add(this.SetNewPageAsCurrent);
            this.Name = "AddNewTabWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.SetCurrentPageActivity SetNewPageAsCurrent;
        private DashboardBusiness.Activities.CreateNewPageActivity CreateNewPage;
        private DashboardBusiness.Activities.GetUserGuidActivity GetUserGuid;








    }
}

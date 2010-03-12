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
	partial class DeleteWidgetInstanceWorkflow
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
            this.reorderWidgetInstanceOnColumnActivity1 = new DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity();
            this.deleteWidgetInstanceActivity1 = new DashboardBusiness.Activities.DeleteWidgetInstanceActivity();
            this.EnsureWidgetOwner = new DashboardBusiness.Activities.EnsureOwnerActivity();
            // 
            // reorderWidgetInstanceOnColumnActivity1
            // 
            activitybind1.Name = "DeleteWidgetInstanceWorkflow";
            activitybind1.Path = "WidgetInstance.ColumnNo";
            this.reorderWidgetInstanceOnColumnActivity1.Name = "reorderWidgetInstanceOnColumnActivity1";
            activitybind2.Name = "DeleteWidgetInstanceWorkflow";
            activitybind2.Path = "WidgetInstance.PageId";
            this.reorderWidgetInstanceOnColumnActivity1.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.reorderWidgetInstanceOnColumnActivity1.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("ColumnNo", typeof(DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // deleteWidgetInstanceActivity1
            // 
            this.deleteWidgetInstanceActivity1.Name = "deleteWidgetInstanceActivity1";
            activitybind3.Name = "DeleteWidgetInstanceWorkflow";
            activitybind3.Path = "WidgetInstance";
            this.deleteWidgetInstanceActivity1.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstance", typeof(DashboardBusiness.Activities.DeleteWidgetInstanceActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // EnsureWidgetOwner
            // 
            this.EnsureWidgetOwner.Name = "EnsureWidgetOwner";
            activitybind4.Name = "DeleteWidgetInstanceWorkflow";
            activitybind4.Path = "WidgetInstance.PageId";
            activitybind5.Name = "DeleteWidgetInstanceWorkflow";
            activitybind5.Path = "UserName";
            activitybind6.Name = "DeleteWidgetInstanceWorkflow";
            activitybind6.Path = "WidgetInstance.Id";
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(DashboardBusiness.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // DeleteWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetOwner);
            this.Activities.Add(this.deleteWidgetInstanceActivity1);
            this.Activities.Add(this.reorderWidgetInstanceOnColumnActivity1);
            this.Name = "DeleteWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.DeleteWidgetInstanceActivity deleteWidgetInstanceActivity1;
        private DashboardBusiness.Activities.EnsureOwnerActivity EnsureWidgetOwner;
        private DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity reorderWidgetInstanceOnColumnActivity1;















    }
}

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
	partial class MoveWidgetInstanceWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            this.PullWidgetsUpInOldColumn = new DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity();
            this.ChangeColumnAndRowOfWidget = new DashboardBusiness.Activities.ChangeWidgetInstancePositionActivity();
            this.PushWidgetsDownInNewColumn = new DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity();
            this.PutWidgetInstanceInWorkflow = new DashboardBusiness.Activities.LoadWidgetActivity();
            this.EnsureWidgetOwner = new DashboardBusiness.Activities.EnsureOwnerActivity();
            // 
            // PullWidgetsUpInOldColumn
            // 
            activitybind1.Name = "PutWidgetInstanceInWorkflow";
            activitybind1.Path = "WidgetInstance.ColumnNo";
            this.PullWidgetsUpInOldColumn.Name = "PullWidgetsUpInOldColumn";
            activitybind2.Name = "PutWidgetInstanceInWorkflow";
            activitybind2.Path = "WidgetInstance.PageId";
            this.PullWidgetsUpInOldColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("ColumnNo", typeof(DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.PullWidgetsUpInOldColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // ChangeColumnAndRowOfWidget
            // 
            activitybind3.Name = "MoveWidgetInstanceWorkflow";
            activitybind3.Path = "ColumnNo";
            this.ChangeColumnAndRowOfWidget.Name = "ChangeColumnAndRowOfWidget";
            activitybind4.Name = "MoveWidgetInstanceWorkflow";
            activitybind4.Path = "RowNo";
            activitybind5.Name = "MoveWidgetInstanceWorkflow";
            activitybind5.Path = "WidgetInstanceId";
            this.ChangeColumnAndRowOfWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("ColumnNo", typeof(DashboardBusiness.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.ChangeColumnAndRowOfWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RowNo", typeof(DashboardBusiness.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.ChangeColumnAndRowOfWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(DashboardBusiness.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // PushWidgetsDownInNewColumn
            // 
            activitybind6.Name = "MoveWidgetInstanceWorkflow";
            activitybind6.Path = "ColumnNo";
            this.PushWidgetsDownInNewColumn.Name = "PushWidgetsDownInNewColumn";
            activitybind7.Name = "PutWidgetInstanceInWorkflow";
            activitybind7.Path = "WidgetInstance.PageId";
            activitybind8.Name = "MoveWidgetInstanceWorkflow";
            activitybind8.Path = "RowNo";
            this.PushWidgetsDownInNewColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.PushWidgetsDownInNewColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("ColumnNo", typeof(DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.PushWidgetsDownInNewColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Position", typeof(DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // PutWidgetInstanceInWorkflow
            // 
            this.PutWidgetInstanceInWorkflow.Name = "PutWidgetInstanceInWorkflow";
            this.PutWidgetInstanceInWorkflow.WidgetInstance = null;
            activitybind9.Name = "MoveWidgetInstanceWorkflow";
            activitybind9.Path = "WidgetInstanceId";
            this.PutWidgetInstanceInWorkflow.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(DashboardBusiness.Activities.LoadWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // EnsureWidgetOwner
            // 
            this.EnsureWidgetOwner.Name = "EnsureWidgetOwner";
            activitybind11.Name = "MoveWidgetInstanceWorkflow";
            activitybind11.Path = "UserName";
            activitybind12.Name = "MoveWidgetInstanceWorkflow";
            activitybind12.Path = "WidgetInstanceId";
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(DashboardBusiness.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // MoveWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetOwner);
            this.Activities.Add(this.PutWidgetInstanceInWorkflow);
            this.Activities.Add(this.PushWidgetsDownInNewColumn);
            this.Activities.Add(this.ChangeColumnAndRowOfWidget);
            this.Activities.Add(this.PullWidgetsUpInOldColumn);
            this.Name = "MoveWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.EnsureOwnerActivity EnsureWidgetOwner;
        private DashboardBusiness.Activities.ReorderWidgetInstanceOnColumnActivity PullWidgetsUpInOldColumn;
        private DashboardBusiness.Activities.ChangeWidgetInstancePositionActivity ChangeColumnAndRowOfWidget;
        private DashboardBusiness.Activities.LoadWidgetActivity PutWidgetInstanceInWorkflow;
        private DashboardBusiness.Activities.PushDownWidgetsOnColumnActivity PushWidgetsDownInNewColumn;





















    }
}

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
	partial class FirstVisitWorkflow
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
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding1 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding2 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference4 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.SetException2 = new System.Workflow.Activities.CodeActivity();
            this.CallWorkflow = new DashboardBusiness.Activities.CallWorkflowActivity();
            this.SecondPageFailed = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfSecondPageCreated = new System.Workflow.Activities.IfElseBranchActivity();
            this.SetException = new System.Workflow.Activities.CodeActivity();
            this.SecondPageCheck = new System.Workflow.Activities.IfElseActivity();
            this.CreateSecondPage = new DashboardBusiness.Activities.CreateNewPageActivity();
            this.CreateDefaultWidgets = new DashboardBusiness.Activities.CreateDeafultWidgetsOnPageActivity();
            this.FirstPageFailed = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfCreated = new System.Workflow.Activities.IfElseBranchActivity();
            this.FirstPageCreateCheck = new System.Workflow.Activities.IfElseActivity();
            this.CreateFirstTab = new DashboardBusiness.Activities.CreateNewPageActivity();
            this.GetUserGUID = new DashboardBusiness.Activities.GetUserGuidActivity();
            // 
            // SetException2
            // 
            this.SetException2.Name = "SetException2";
            this.SetException2.ExecuteCode += new System.EventHandler(this.SetException_ExecuteCode);
            // 
            // CallWorkflow
            // 
            this.CallWorkflow.Name = "CallWorkflow";
            activitybind1.Name = "FirstVisitWorkflow";
            activitybind1.Path = "UserName";
            workflowparameterbinding1.ParameterName = "UserName";
            workflowparameterbinding1.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            activitybind2.Name = "FirstVisitWorkflow";
            activitybind2.Path = "UserPageSetup";
            workflowparameterbinding2.ParameterName = "UserPageSetup";
            workflowparameterbinding2.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.CallWorkflow.Parameters.Add(workflowparameterbinding1);
            this.CallWorkflow.Parameters.Add(workflowparameterbinding2);
            this.CallWorkflow.Type = typeof(DashboardBusiness.UserVisitWorkflow);
            // 
            // SecondPageFailed
            // 
            this.SecondPageFailed.Activities.Add(this.SetException2);
            ruleconditionreference1.ConditionName = "SecondPageIDZeroOrLess";
            this.SecondPageFailed.Condition = ruleconditionreference1;
            this.SecondPageFailed.Name = "SecondPageFailed";
            // 
            // IfSecondPageCreated
            // 
            this.IfSecondPageCreated.Activities.Add(this.CallWorkflow);
            ruleconditionreference2.ConditionName = "SecondPageIDNonZero";
            this.IfSecondPageCreated.Condition = ruleconditionreference2;
            this.IfSecondPageCreated.Name = "IfSecondPageCreated";
            // 
            // SetException
            // 
            this.SetException.Name = "SetException";
            this.SetException.ExecuteCode += new System.EventHandler(this.SetException_ExecuteCode);
            // 
            // SecondPageCheck
            // 
            this.SecondPageCheck.Activities.Add(this.IfSecondPageCreated);
            this.SecondPageCheck.Activities.Add(this.SecondPageFailed);
            this.SecondPageCheck.Name = "SecondPageCheck";
            // 
            // CreateSecondPage
            // 
            this.CreateSecondPage.Name = "CreateSecondPage";
            this.CreateSecondPage.NewPageId = 0;
            this.CreateSecondPage.Title = "Second Page";
            activitybind3.Name = "GetUserGUID";
            activitybind3.Path = "UserGuid";
            this.CreateSecondPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(DashboardBusiness.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // CreateDefaultWidgets
            // 
            this.CreateDefaultWidgets.Name = "CreateDefaultWidgets";
            activitybind4.Name = "CreateFirstTab";
            activitybind4.Path = "NewPageId";
            this.CreateDefaultWidgets.SetBinding(DashboardBusiness.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // FirstPageFailed
            // 
            this.FirstPageFailed.Activities.Add(this.SetException);
            ruleconditionreference3.ConditionName = "FirstPageIDZeroOrLess";
            this.FirstPageFailed.Condition = ruleconditionreference3;
            this.FirstPageFailed.Name = "FirstPageFailed";
            // 
            // IfCreated
            // 
            this.IfCreated.Activities.Add(this.CreateDefaultWidgets);
            this.IfCreated.Activities.Add(this.CreateSecondPage);
            this.IfCreated.Activities.Add(this.SecondPageCheck);
            ruleconditionreference4.ConditionName = "FirstPageIDNonZero";
            this.IfCreated.Condition = ruleconditionreference4;
            this.IfCreated.Name = "IfCreated";
            // 
            // FirstPageCreateCheck
            // 
            this.FirstPageCreateCheck.Activities.Add(this.IfCreated);
            this.FirstPageCreateCheck.Activities.Add(this.FirstPageFailed);
            this.FirstPageCreateCheck.Name = "FirstPageCreateCheck";
            // 
            // CreateFirstTab
            // 
            this.CreateFirstTab.Description = "Create the first default tab ";
            this.CreateFirstTab.Name = "CreateFirstTab";
            this.CreateFirstTab.NewPageId = 0;
            this.CreateFirstTab.Title = "First Page";
            activitybind5.Name = "GetUserGUID";
            activitybind5.Path = "UserGuid";
            this.CreateFirstTab.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(DashboardBusiness.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // GetUserGUID
            // 
            this.GetUserGUID.Description = "Get user GUID from user name";
            this.GetUserGUID.Name = "GetUserGUID";
            this.GetUserGUID.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind6.Name = "FirstVisitWorkflow";
            activitybind6.Path = "UserName";
            this.GetUserGUID.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // FirstVisitWorkflow
            // 
            this.Activities.Add(this.GetUserGUID);
            this.Activities.Add(this.CreateFirstTab);
            this.Activities.Add(this.FirstPageCreateCheck);
            this.Name = "FirstVisitWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.CreateDeafultWidgetsOnPageActivity CreateDefaultWidgets;
        private IfElseBranchActivity FirstPageFailed;
        private IfElseBranchActivity IfCreated;
        private IfElseActivity FirstPageCreateCheck;
        private DashboardBusiness.Activities.CreateNewPageActivity CreateSecondPage;
        private DashboardBusiness.Activities.CallWorkflowActivity CallWorkflow;
        private IfElseBranchActivity SecondPageFailed;
        private IfElseBranchActivity IfSecondPageCreated;
        private IfElseActivity SecondPageCheck;
        private CodeActivity SetException2;
        private CodeActivity SetException;
        private DashboardBusiness.Activities.CreateNewPageActivity CreateFirstTab;
        private DashboardBusiness.Activities.GetUserGuidActivity GetUserGUID;




















    }
}

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
	partial class UserVisitWorkflow
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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            this.ReloadUserSetting = new DashboardBusiness.Activities.GetUserSettingActivity();
            this.ChangeCurrentPage = new DashboardBusiness.Activities.SetCurrentPageActivity();
            this.IfCurrentPageIsDifferentThanUserSetting = new System.Workflow.Activities.IfElseBranchActivity();
            this.ReturnUserPageSetup = new System.Workflow.Activities.CodeActivity();
            this.CheckIfCurrentPageHasChanged = new System.Workflow.Activities.IfElseActivity();
            this.GetWidgetsInCurrentPage = new DashboardBusiness.Activities.GetWidgetsInPageActivity();
            this.DecideCurrentPage = new DashboardBusiness.Activities.DecideCurrentPageActivity();
            this.GetUserSetting = new DashboardBusiness.Activities.GetUserSettingActivity();
            this.GetUserPages = new DashboardBusiness.Activities.GetUserPagesActivity();
            this.GetUserGuid = new DashboardBusiness.Activities.GetUserGuidActivity();
            // 
            // ReloadUserSetting
            // 
            activitybind1.Name = "UserVisitWorkflow";
            activitybind1.Path = "CurrentPage";
            this.ReloadUserSetting.Name = "ReloadUserSetting";
            activitybind2.Name = "GetUserGuid";
            activitybind2.Path = "UserGuid";
            activitybind3.Name = "UserVisitWorkflow";
            activitybind3.Path = "UserSetting";
            this.ReloadUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ReloadUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserSetting", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.ReloadUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // ChangeCurrentPage
            // 
            this.ChangeCurrentPage.Name = "ChangeCurrentPage";
            activitybind4.Name = "UserVisitWorkflow";
            activitybind4.Path = "CurrentPage.ID";
            activitybind5.Name = "UserVisitWorkflow";
            activitybind5.Path = "UserSetting.UserId";
            this.ChangeCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.ChangeCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // IfCurrentPageIsDifferentThanUserSetting
            // 
            this.IfCurrentPageIsDifferentThanUserSetting.Activities.Add(this.ChangeCurrentPage);
            this.IfCurrentPageIsDifferentThanUserSetting.Activities.Add(this.ReloadUserSetting);
            ruleconditionreference1.ConditionName = "CurrentPageHasChanged";
            this.IfCurrentPageIsDifferentThanUserSetting.Condition = ruleconditionreference1;
            this.IfCurrentPageIsDifferentThanUserSetting.Name = "IfCurrentPageIsDifferentThanUserSetting";
            // 
            // ReturnUserPageSetup
            // 
            this.ReturnUserPageSetup.Name = "ReturnUserPageSetup";
            this.ReturnUserPageSetup.ExecuteCode += new System.EventHandler(this.ReturnUserPageSetup_ExecuteCode);
            // 
            // CheckIfCurrentPageHasChanged
            // 
            this.CheckIfCurrentPageHasChanged.Activities.Add(this.IfCurrentPageIsDifferentThanUserSetting);
            this.CheckIfCurrentPageHasChanged.Name = "CheckIfCurrentPageHasChanged";
            // 
            // GetWidgetsInCurrentPage
            // 
            this.GetWidgetsInCurrentPage.Name = "GetWidgetsInCurrentPage";
            activitybind6.Name = "UserVisitWorkflow";
            activitybind6.Path = "CurrentPage.ID";
            activitybind7.Name = "GetUserGuid";
            activitybind7.Path = "UserGuid";
            this.GetWidgetsInCurrentPage.WidgetInstances = null;
            this.GetWidgetsInCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.GetWidgetsInPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.GetWidgetsInCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.GetWidgetsInPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // DecideCurrentPage
            // 
            activitybind8.Name = "UserVisitWorkflow";
            activitybind8.Path = "CurrentPage";
            activitybind9.Name = "UserVisitWorkflow";
            activitybind9.Path = "CurrentPage.ID";
            this.DecideCurrentPage.Name = "DecideCurrentPage";
            activitybind10.Name = "UserVisitWorkflow";
            activitybind10.Path = "PageName";
            activitybind11.Name = "GetUserGuid";
            activitybind11.Path = "UserGuid";
            this.DecideCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageName", typeof(DashboardBusiness.Activities.DecideCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.DecideCurrentPage.SetBinding(DashboardBusiness.Activities.DecideCurrentPageActivity.UserGuidProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.DecideCurrentPage.SetBinding(DashboardBusiness.Activities.DecideCurrentPageActivity.CurrentPageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.DecideCurrentPage.SetBinding(DashboardBusiness.Activities.DecideCurrentPageActivity.CurrentPageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // GetUserSetting
            // 
            activitybind12.Name = "UserVisitWorkflow";
            activitybind12.Path = "CurrentPage";
            this.GetUserSetting.Name = "GetUserSetting";
            activitybind13.Name = "GetUserGuid";
            activitybind13.Path = "UserGuid";
            activitybind14.Name = "UserVisitWorkflow";
            activitybind14.Path = "UserSetting";
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserSetting", typeof(DashboardBusiness.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            // 
            // GetUserPages
            // 
            this.GetUserPages.Name = "GetUserPages";
            activitybind15.Name = "UserVisitWorkflow";
            activitybind15.Path = "UserPages";
            activitybind16.Name = "GetUserGuid";
            activitybind16.Path = "UserGuid";
            this.GetUserPages.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(DashboardBusiness.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.GetUserPages.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Pages", typeof(DashboardBusiness.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Description = "name";
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind17.Name = "UserVisitWorkflow";
            activitybind17.Path = "UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            // 
            // UserVisitWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetUserPages);
            this.Activities.Add(this.GetUserSetting);
            this.Activities.Add(this.DecideCurrentPage);
            this.Activities.Add(this.GetWidgetsInCurrentPage);
            this.Activities.Add(this.CheckIfCurrentPageHasChanged);
            this.Activities.Add(this.ReturnUserPageSetup);
            this.Name = "UserVisitWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.SetCurrentPageActivity ChangeCurrentPage;
        private DashboardBusiness.Activities.GetUserSettingActivity ReloadUserSetting;
        private IfElseBranchActivity IfCurrentPageIsDifferentThanUserSetting;
        private IfElseActivity CheckIfCurrentPageHasChanged;
        private DashboardBusiness.Activities.DecideCurrentPageActivity DecideCurrentPage;
        private CodeActivity ReturnUserPageSetup;
        private DashboardBusiness.Activities.GetUserSettingActivity GetUserSetting;
        private DashboardBusiness.Activities.GetWidgetsInPageActivity GetWidgetsInCurrentPage;
        private DashboardBusiness.Activities.GetUserPagesActivity GetUserPages;
        private DashboardBusiness.Activities.GetUserGuidActivity GetUserGuid;














































































    }
}

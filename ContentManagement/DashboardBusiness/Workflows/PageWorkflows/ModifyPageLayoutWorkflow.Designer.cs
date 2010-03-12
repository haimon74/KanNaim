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
	partial class ModifyPageLayoutWorkflow
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
            this.ChangePageLayout = new DashboardBusiness.Activities.ChangePageLayoutActivity();
            this.GetUserGuid = new DashboardBusiness.Activities.GetUserGuidActivity();
            // 
            // ChangePageLayout
            // 
            activitybind1.Name = "ModifyPageLayoutWorkflow";
            activitybind1.Path = "LayoutType";
            this.ChangePageLayout.Name = "ChangePageLayout";
            activitybind2.Name = "ModifyPageLayoutWorkflow";
            activitybind2.Path = "PageID";
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(DashboardBusiness.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(DashboardBusiness.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind3.Name = "ModifyPageLayoutWorkflow";
            activitybind3.Path = "UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(DashboardBusiness.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // ModifyPageLayoutWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.ChangePageLayout);
            this.Name = "ModifyPageLayoutWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private DashboardBusiness.Activities.ChangePageLayoutActivity ChangePageLayout;
        private DashboardBusiness.Activities.GetUserGuidActivity GetUserGuid;












    }
}

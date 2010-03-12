// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using DashboardDataAccess;

namespace DashboardBusiness
{
	public sealed partial class UserVisitWorkflow: SequentialWorkflowActivity
	{
		public UserVisitWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.UserVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.UserVisitWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.UserVisitWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty UserPageSetupProperty = DependencyProperty.Register("UserPageSetup", typeof(UserPageSetup), typeof(DashboardBusiness.UserVisitWorkflow));

        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public UserPageSetup UserPageSetup
        {
            get
            {
                return ((UserPageSetup)(base.GetValue(DashboardBusiness.UserVisitWorkflow.UserPageSetupProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.UserVisitWorkflow.UserPageSetupProperty, value);
            }
        }

        public static DependencyProperty UserPagesProperty = DependencyProperty.Register("UserPages", typeof(System.Collections.Generic.List<Page>), typeof(DashboardBusiness.UserVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public System.Collections.Generic.List<Page> UserPages
        {
            get
            {
                return ((System.Collections.Generic.List<Page>)(base.GetValue(DashboardBusiness.UserVisitWorkflow.UserPagesProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.UserVisitWorkflow.UserPagesProperty, value);
            }
        }

        private void UserHasPageCode_ExecuteCode(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("User has pages");
        }

        public static DependencyProperty ReturnPages_MethodInvoking1Event = DependencyProperty.Register("ReturnPages_MethodInvoking1", typeof(System.EventHandler), typeof(DashboardBusiness.UserVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Handlers")]
        public event EventHandler ReturnPages_MethodInvoking1
        {
            add
            {
                base.AddHandler(ReturnPages_MethodInvoking1Event, value);
            }
            remove
            {
                base.RemoveHandler(ReturnPages_MethodInvoking1Event, value);
            }
        }


        private void ReturnUserPageSetup_ExecuteCode(object sender, EventArgs e)
        {
            this.UserPageSetup.Pages = this.GetUserPages.Pages;
            this.UserPageSetup.UserSetting = this.GetUserSetting.UserSetting;
            this.UserPageSetup.WidgetInstances = this.GetWidgetsInCurrentPage.WidgetInstances;
        }

        public static DependencyProperty CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(DashboardDataAccess.Page), typeof(DashboardBusiness.UserVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Page CurrentPage
        {
            get
            {
                return ((DashboardDataAccess.Page)(base.GetValue(DashboardBusiness.UserVisitWorkflow.CurrentPageProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.UserVisitWorkflow.CurrentPageProperty, value);
            }
        }

        public static DependencyProperty PageNameProperty = DependencyProperty.Register("PageName", typeof(System.String), typeof(DashboardBusiness.UserVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String PageName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.UserVisitWorkflow.PageNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.UserVisitWorkflow.PageNameProperty, value);
            }
        }

        public static DependencyProperty UserSettingProperty = DependencyProperty.Register("UserSetting", typeof(DashboardDataAccess.UserSetting), typeof(DashboardBusiness.UserVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public UserSetting UserSetting
        {
            get
            {
                return ((DashboardDataAccess.UserSetting)(base.GetValue(DashboardBusiness.UserVisitWorkflow.UserSettingProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.UserVisitWorkflow.UserSettingProperty, value);
            }
        }

        
	}

}

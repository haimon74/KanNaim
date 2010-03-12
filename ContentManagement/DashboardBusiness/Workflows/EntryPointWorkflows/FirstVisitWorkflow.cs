// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace DashboardBusiness
{
    public sealed partial class FirstVisitWorkflow : SequentialWorkflowActivity
    {
        public FirstVisitWorkflow()
        {
            InitializeComponent();
        }

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.FirstVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.FirstVisitWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.FirstVisitWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty UserPageSetupProperty = DependencyProperty.Register("UserPageSetup", typeof(DashboardBusiness.UserPageSetup), typeof(DashboardBusiness.FirstVisitWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Parameters")]
        public UserPageSetup UserPageSetup
        {
            get
            {
                return ((DashboardBusiness.UserPageSetup)(base.GetValue(DashboardBusiness.FirstVisitWorkflow.UserPageSetupProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.FirstVisitWorkflow.UserPageSetupProperty, value);
            }
        }

        private void SetException_ExecuteCode(object sender, EventArgs e)
        {
            throw new ApplicationException("First page creation failed");
        }
    }
}

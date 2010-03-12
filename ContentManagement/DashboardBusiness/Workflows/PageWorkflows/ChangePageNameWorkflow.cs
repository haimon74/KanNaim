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
	public sealed partial class ChangePageNameWorkflow: SequentialWorkflowActivity
	{
		public ChangePageNameWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.ChangePageNameWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.ChangePageNameWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.ChangePageNameWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty PageNameProperty = DependencyProperty.Register("PageName", typeof(System.String), typeof(DashboardBusiness.ChangePageNameWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String PageName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.ChangePageNameWorkflow.PageNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.ChangePageNameWorkflow.PageNameProperty, value);
            }
        }
	}

}

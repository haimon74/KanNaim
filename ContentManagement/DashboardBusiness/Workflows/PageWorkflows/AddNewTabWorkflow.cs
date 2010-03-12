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
	public sealed partial class AddNewTabWorkflow: SequentialWorkflowActivity
	{
		public AddNewTabWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.AddNewTabWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.AddNewTabWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.AddNewTabWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(System.String), typeof(DashboardBusiness.AddNewTabWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String LayoutType
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.AddNewTabWorkflow.LayoutTypeProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.AddNewTabWorkflow.LayoutTypeProperty, value);
            }
        }

        public static DependencyProperty NewPageProperty = DependencyProperty.Register("NewPage", typeof(DashboardDataAccess.Page), typeof(DashboardBusiness.AddNewTabWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public DashboardDataAccess.Page NewPage
        {
            get
            {
                return ((DashboardDataAccess.Page)(base.GetValue(DashboardBusiness.AddNewTabWorkflow.NewPageProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.AddNewTabWorkflow.NewPageProperty, value);
            }
        }
	}

}

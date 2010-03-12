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
	public sealed partial class AddWidgetWorkflow: SequentialWorkflowActivity
	{
		public AddWidgetWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.AddWidgetWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.AddWidgetWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.AddWidgetWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty WidgetIdProperty = DependencyProperty.Register("WidgetId", typeof(System.Int32), typeof(DashboardBusiness.AddWidgetWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 WidgetId
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.AddWidgetWorkflow.WidgetIdProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.AddWidgetWorkflow.WidgetIdProperty, value);
            }
        }

        public static DependencyProperty NewWidgetProperty = DependencyProperty.Register("NewWidget", typeof(DashboardDataAccess.WidgetInstance), typeof(DashboardBusiness.AddWidgetWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public DashboardDataAccess.WidgetInstance NewWidget
        {
            get
            {
                return ((DashboardDataAccess.WidgetInstance)(base.GetValue(DashboardBusiness.AddWidgetWorkflow.NewWidgetProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.AddWidgetWorkflow.NewWidgetProperty, value);
            }
        }
	}

}

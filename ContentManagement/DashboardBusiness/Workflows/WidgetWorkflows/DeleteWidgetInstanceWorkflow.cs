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
using DashboardDataAccess;

namespace DashboardBusiness
{
	public sealed partial class DeleteWidgetInstanceWorkflow: SequentialWorkflowActivity
	{
		public DeleteWidgetInstanceWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty WidgetInstanceProperty = DependencyProperty.Register("WidgetInstance", typeof(WidgetInstance), typeof(DashboardBusiness.DeleteWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public WidgetInstance WidgetInstance
        {
            get
            {
                return ((WidgetInstance)(base.GetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.WidgetInstanceProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.WidgetInstanceProperty, value);
            }
        }

        /*
        public static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(System.Int32), typeof(DashboardBusiness.DeleteWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 PageId
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.PageIdProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.PageIdProperty, value);
            }
        }

        public static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(System.Int32), typeof(DashboardBusiness.DeleteWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 ColumnNo
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.ColumnNoProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.ColumnNoProperty, value);
            }
        }
        */

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.DeleteWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.UserNameProperty, value);
            }
        }
	}

}

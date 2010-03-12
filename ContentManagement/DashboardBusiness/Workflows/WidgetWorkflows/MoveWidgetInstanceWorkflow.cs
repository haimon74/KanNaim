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
	public sealed partial class MoveWidgetInstanceWorkflow: SequentialWorkflowActivity
	{
		public MoveWidgetInstanceWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(System.Int32), typeof(DashboardBusiness.MoveWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 WidgetInstanceId
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.WidgetInstanceIdProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.WidgetInstanceIdProperty, value);
            }
        }

        public static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(System.Int32), typeof(DashboardBusiness.MoveWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 ColumnNo
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.ColumnNoProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.ColumnNoProperty, value);
            }
        }

        public static DependencyProperty RowNoProperty = DependencyProperty.Register("RowNo", typeof(System.Int32), typeof(DashboardBusiness.MoveWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 RowNo
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.RowNoProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.RowNoProperty, value);
            }
        }

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.MoveWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.MoveWidgetInstanceWorkflow.UserNameProperty, value);
            }
        }
	}

}

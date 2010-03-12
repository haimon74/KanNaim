// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using System.Linq;
using System.Data.Linq;

using DashboardDataAccess;

namespace DashboardBusiness.Activities
{
    public partial class ChangeWidgetInstancePositionActivity : System.Workflow.ComponentModel.Activity
	{
		public ChangeWidgetInstancePositionActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(int), typeof(ChangeWidgetInstancePositionActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int ColumnNo
        {
            get { return (int)base.GetValue(ColumnNoProperty); }
            set { base.SetValue(ColumnNoProperty, value); }
        }

        private static DependencyProperty RowNoProperty = DependencyProperty.Register("RowNo", typeof(int), typeof(ChangeWidgetInstancePositionActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int RowNo
        {
            get { return (int)base.GetValue(RowNoProperty); }
            set { base.SetValue(RowNoProperty, value); }
        }

        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(ChangeWidgetInstancePositionActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {
                WidgetInstance widgetInstance = db.WidgetInstances.Single(wi => wi.Id == WidgetInstanceId);
                widgetInstance.Detach();

                DatabaseHelper.Update<WidgetInstance>(widgetInstance, delegate(WidgetInstance wi)
                {
                    wi.ColumnNo = ColumnNo;
                    wi.OrderNo = RowNo;
                });
            }

            return ActivityExecutionStatus.Closed;
        }
	}
}

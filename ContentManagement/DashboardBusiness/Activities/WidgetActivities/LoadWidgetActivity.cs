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

using System.Data.Linq;
using System.Linq;

using DashboardDataAccess;

namespace DashboardBusiness.Activities
{
    public partial class LoadWidgetActivity : System.Workflow.ComponentModel.Activity
	{
		public LoadWidgetActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(LoadWidgetActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        [ReadOnly(true)]
        private static DependencyProperty WidgetInstanceProperty = DependencyProperty.Register("WidgetInstance", typeof(WidgetInstance), typeof(LoadWidgetActivity));
        public WidgetInstance WidgetInstance
        {
            get { return (WidgetInstance)base.GetValue(WidgetInstanceProperty); }
            set { base.SetValue(WidgetInstanceProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {
                WidgetInstance = db.WidgetInstances.Single(wi => wi.Id == WidgetInstanceId);
            }
            return ActivityExecutionStatus.Closed;
        }

	}
}

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

using DashboardDataAccess;

namespace DashboardBusiness.Activities
{
    public partial class DeleteWidgetInstanceActivity : System.Workflow.ComponentModel.Activity
	{
		public DeleteWidgetInstanceActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty WidgetInstanceProperty = DependencyProperty.Register("WidgetInstance", typeof(WidgetInstance), typeof(DeleteWidgetInstanceActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public WidgetInstance WidgetInstance
        {
            get { return (WidgetInstance)base.GetValue(WidgetInstanceProperty); }
            set { base.SetValue(WidgetInstanceProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.Delete<WidgetInstance>(this.WidgetInstance);
            return ActivityExecutionStatus.Closed;
        }
	}
}

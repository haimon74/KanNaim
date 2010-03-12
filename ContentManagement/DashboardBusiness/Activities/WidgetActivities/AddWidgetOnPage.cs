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
    public partial class AddWidgetOnPage : System.Workflow.ComponentModel.Activity
	{
		public AddWidgetOnPage()
		{
			InitializeComponent();
		}

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(AddWidgetOnPage));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        private static DependencyProperty WidgetIdProperty = DependencyProperty.Register("WidgetId", typeof(int), typeof(AddWidgetOnPage));

        public int WidgetId
        {
            get { return (int)base.GetValue(WidgetIdProperty); }
            set { base.SetValue(WidgetIdProperty, value); }
        }

        private static DependencyProperty NewWidgetProperty = DependencyProperty.Register("NewWidget", typeof(WidgetInstance), typeof(AddWidgetOnPage));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public WidgetInstance NewWidget
        {
            get { return (WidgetInstance)base.GetValue(NewWidgetProperty); }
            set { base.SetValue(NewWidgetProperty, value); }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {
                Widget w = db.Widgets.Single(a => a.ID == WidgetId);

                WidgetInstance wi = new WidgetInstance();
                wi.Title = w.Name;
                wi.PageId = PageId;
                wi.CreatedDate = wi.LastUpdate = DateTime.Now;
                wi.VersionNo = 1;
                wi.State = string.Empty;
                wi.WidgetId = w.ID;
                wi.Expanded = true;
                wi.State = w.DefaultState;

                DatabaseHelper.Insert<WidgetInstance>(wi);

                this.NewWidget = wi;
            }

            return ActivityExecutionStatus.Closed;
        }
	}
}

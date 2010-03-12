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
    public partial class ChangePageNameActivity : System.Workflow.ComponentModel.Activity
	{
        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(ChangePageNameActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        private static DependencyProperty PageNameProperty = DependencyProperty.Register("PageName", typeof(string), typeof(ChangePageNameActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string PageName
        {
            get { return (string)base.GetValue(PageNameProperty); }
            set { base.SetValue(PageNameProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {

                var page = db.Pages.Single(p => p.ID == PageId);
                page.Title = PageName;
                db.SubmitChanges();
            }
            
            return ActivityExecutionStatus.Closed;
        }


		public ChangePageNameActivity()
		{
			InitializeComponent();
		}
	}
}

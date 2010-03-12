// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
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
    public partial class PushDownWidgetsOnColumnActivity : System.Workflow.ComponentModel.Activity
	{
		public PushDownWidgetsOnColumnActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(PushDownWidgetsOnColumnActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        private static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(int), typeof(PushDownWidgetsOnColumnActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int ColumnNo
        {
            get { return (int)base.GetValue(ColumnNoProperty); }
            set { base.SetValue(ColumnNoProperty, value); }
        }

        private static DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(PushDownWidgetsOnColumnActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int Position
        {
            get { return (int)base.GetValue(PositionProperty); }
            set { base.SetValue(PositionProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {
                var query = from wi in db.WidgetInstances
                            where wi.PageId == PageId && wi.ColumnNo == ColumnNo && wi.OrderNo >= Position
                            orderby wi.OrderNo
                            select wi;
                List<WidgetInstance> list = query.ToList();

                int orderNo = Position + 1;
                foreach (WidgetInstance wi in list)
                {
                    wi.OrderNo = orderNo++;
                }

                db.SubmitChanges();
            }
            
            return ActivityExecutionStatus.Closed;
        }
	}
}

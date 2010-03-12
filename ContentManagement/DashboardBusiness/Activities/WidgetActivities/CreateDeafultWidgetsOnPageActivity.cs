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
    public partial class CreateDeafultWidgetsOnPageActivity : System.Workflow.ComponentModel.Activity
	{
		public CreateDeafultWidgetsOnPageActivity()
		{
			InitializeComponent();
		}

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {

                var defaultWidgets = db.Widgets.Where(w => w.IsDefault == true).OrderBy(w => w.OrderNo).ToList();
                var widgetsPerColumn = (int)Math.Ceiling((float)defaultWidgets.Count / 3.0);

                var row = 0;
                var col = 0;

                foreach (Widget w in defaultWidgets)
                {
                    var newWidget = new WidgetInstance();
                    newWidget.PageId = this.PageId;
                    newWidget.ColumnNo = col;
                    newWidget.OrderNo = row;
                    newWidget.CreatedDate = newWidget.LastUpdate = DateTime.Now;
                    newWidget.Expanded = true;
                    newWidget.Title = w.Name;
                    newWidget.VersionNo = 1;
                    newWidget.WidgetId = w.ID;
                    newWidget.State = w.DefaultState;

                    db.WidgetInstances.InsertOnSubmit(newWidget);

                    row++;
                    if (row >= widgetsPerColumn)
                    {
                        row = 0;
                        col++;
                    }
                }

                db.SubmitChanges();
            }
            return ActivityExecutionStatus.Closed;
        }

        public static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(System.Int32), typeof(DashboardBusiness.Activities.CreateDeafultWidgetsOnPageActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Int32 PageId
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty, value);
            }
        }

	
	}
}

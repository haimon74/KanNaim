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
    public partial class SetCurrentPageActivity : System.Workflow.ComponentModel.Activity
	{
		public SetCurrentPageActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(SetCurrentPageActivity));
        
        [ValidationOptionAttribute(ValidationOption.Required)]        
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(SetCurrentPageActivity));
        
        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {
                var userSetting = db.UserSettings.Single(u => u.UserId == UserGuid);

                userSetting.CurrentPageId = PageId;
                db.SubmitChanges();
            }
            return ActivityExecutionStatus.Closed;
        }

	}
}

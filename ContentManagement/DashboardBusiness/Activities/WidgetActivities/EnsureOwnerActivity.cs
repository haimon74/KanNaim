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
using System.Data.Linq;
using System.Linq;

namespace DashboardBusiness.Activities
{
    public partial class EnsureOwnerActivity : System.Workflow.ComponentModel.Activity
	{
		public EnsureOwnerActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(EnsureOwnerActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string UserName
        {
            get { return (string)base.GetValue(UserNameProperty); }
            set { base.SetValue(UserNameProperty, value); }
        }

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(EnsureOwnerActivity));
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(EnsureOwnerActivity));
        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {

                if (this.PageId == 0 && this.WidgetInstanceId == 0)
                {
                    throw new ApplicationException("No valid object specified to check");
                }

                if (this.WidgetInstanceId > 0)
                {
                    // Get the user who is the owner of the widget. Then see if the current user is the same
                    var ownerName = (from wi in db.WidgetInstances
                                     where wi.Id == this.WidgetInstanceId
                                     select wi.Page.aspnet_User.LoweredUserName).First();

                    if (!this.UserName.ToLower().Equals(ownerName))
                        throw new ApplicationException(string.Format("User {0} is not the owner of the widget instance {1}", this.UserName, this.WidgetInstanceId));
                }


                if (this.PageId > 0)
                {
                    // Get the user who is the owner of the page. Then see if the current user is the same
                    var ownerName = (from p in db.Pages
                                     where p.ID == this.PageId
                                     select p.aspnet_User.LoweredUserName).First();

                    if (!this.UserName.ToLower().Equals(ownerName))
                        throw new ApplicationException(string.Format("User {0} is not the owner of the page {1}", this.UserName, this.PageId));
                }
            }
            
            return ActivityExecutionStatus.Closed;
        }
	}
}

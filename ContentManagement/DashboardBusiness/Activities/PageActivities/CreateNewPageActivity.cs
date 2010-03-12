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
    public partial class CreateNewPageActivity : Activity
	{
        public CreateNewPageActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(String), typeof(CreateNewPageActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }
        private static DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(String), typeof(CreateNewPageActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string LayoutType
        {
            get { return (string)base.GetValue(LayoutTypeProperty); }
            set { base.SetValue(LayoutTypeProperty, value); }
        }
        private static DependencyProperty UserIdProperty = DependencyProperty.Register("UserId", typeof(Guid), typeof(CreateNewPageActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserId
        {
            get { return (Guid)base.GetValue(UserIdProperty); }
            set { base.SetValue(UserIdProperty, value); }
        }
	
        private static DependencyProperty NewPageIdProperty = DependencyProperty.Register("NewPageId", typeof(int), typeof(CreateNewPageActivity));

        public int NewPageId
        {
            get { return (int)base.GetValue(NewPageIdProperty); }
            set { base.SetValue(NewPageIdProperty, value); }
        }

        public Page NewPage
        {
            get { return (Page)GetValue(NewPageProperty); }
            set { SetValue(NewPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewPageProperty =
            DependencyProperty.Register("NewPage", typeof(Page), typeof(CreateNewPageActivity));



        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (var db = DatabaseHelper.GetDashboardData())
            {

                var newPage = new Page();
                newPage.UserId = UserId;
                newPage.Title = Title;
                newPage.CreatedDate = DateTime.Now;
                newPage.LastUpdate = DateTime.Now;
                newPage.LayoutType = Convert.ToInt32(LayoutType);
                DatabaseHelper.Insert<Page>(newPage);

                NewPageId = newPage.ID;

                var page = db.Pages.Single<Page>(p => p.ID == NewPageId);
                this.NewPage = page;
            }

            return ActivityExecutionStatus.Closed;
        }
	}
}

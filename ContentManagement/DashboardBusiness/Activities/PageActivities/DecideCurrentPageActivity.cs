using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using DashboardDataAccess;
using System.Collections.Generic;

namespace DashboardBusiness.Activities
{
	public partial class DecideCurrentPageActivity: Activity
	{
        public string PageName
        {
            get { return (string)base.GetValue(PageNameProperty); }
            set { base.SetValue(PageNameProperty, value); }
        }

        private static DependencyProperty PageNameProperty = 
            DependencyProperty.Register("PageName", typeof(string), typeof(DecideCurrentPageActivity));
        

        public int CurrentPageId
        {
            get { return (int)GetValue(CurrentPageIdProperty); }
            set { SetValue(CurrentPageIdProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageIdProperty = 
            DependencyProperty.Register("CurrentPageId", typeof(int), typeof(DecideCurrentPageActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)GetValue(UserGuidProperty); }
            set { SetValue(UserGuidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserGuid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserGuidProperty =
            DependencyProperty.Register("UserGuid", typeof(Guid), typeof(DecideCurrentPageActivity));



        public Page CurrentPage
        {
            get { return (Page)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(Page), typeof(DecideCurrentPageActivity));



        public DecideCurrentPageActivity()
		{
			InitializeComponent();
		}

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            string pageName = this.PageName;

            using (var db = DatabaseHelper.GetDashboardData())
            {

                List<Page> userPages = db.Pages.Where<Page>(p => p.UserId == UserGuid).ToList();

                // Find the page that has the specified Page Name and make it as current
                // page. This is needed to make a tab as current tab when the tab name is 
                // known
                if (!string.IsNullOrEmpty(pageName))
                {

                    foreach (Page page in userPages)
                    {
                        if (page.Title.Replace(' ', '_') == pageName)
                        {
                            this.CurrentPageId = page.ID;
                            this.CurrentPage = page;
                            break;
                        }
                    }
                }

                // If there's no such page, then the first page user has will be the current
                // page. This happens when a page is deleted.
                if (this.CurrentPageId == 0)
                {
                    this.CurrentPageId = userPages[0].ID;
                    this.CurrentPage = userPages[0];
                }
            }

            return ActivityExecutionStatus.Closed;
        }
	}
}

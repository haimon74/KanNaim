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
    public partial class GetUserPagesActivity : System.Workflow.ComponentModel.Activity
	{
		public GetUserPagesActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetUserPagesActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        private static DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(List<Page>), typeof(GetUserPagesActivity));

        public List<Page> Pages
        {
            get { return (List<Page>)base.GetValue(PagesProperty); }
            set { base.SetValue(PagesProperty, value); }
        }
        
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using( new TimedLog(UserGuid.ToString(), "Activity: Get User Pages") )
            {
                using (var db = DatabaseHelper.GetDashboardData())
                {

                    this.Pages = (from page in db.Pages
                                  where page.UserId == UserGuid
                                  select page).ToList();
                }
                return ActivityExecutionStatus.Closed;
            }
        }
	}
}

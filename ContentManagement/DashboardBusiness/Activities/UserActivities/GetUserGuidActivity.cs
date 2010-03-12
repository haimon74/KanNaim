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
    public partial class GetUserGuidActivity : System.Workflow.ComponentModel.Activity
	{
        public GetUserGuidActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(GetUserGuidActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string UserName
        {
            get { return (string)base.GetValue(UserNameProperty); }
            set { base.SetValue(UserNameProperty, value); }
        }

        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetUserGuidActivity));

        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using( new TimedLog(UserName, "Activity: Get User Guid") )
            {
                using (var db = DatabaseHelper.GetDashboardData())
                {

                    this.UserGuid = (from u in db.aspnet_Users
                                     where u.LoweredUserName == UserName && u.ApplicationId == DatabaseHelper.ApplicationGuid
                                     select u.UserId).Single();
                }
                
                return ActivityExecutionStatus.Closed;
            }
        }
	}
}

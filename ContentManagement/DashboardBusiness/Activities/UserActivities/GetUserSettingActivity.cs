using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
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
    public partial class GetUserSettingActivity : System.Workflow.ComponentModel.Activity
	{
		public GetUserSettingActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetUserSettingActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        
        private static DependencyProperty UserSettingProperty = DependencyProperty.Register("UserSetting", typeof(UserSetting), typeof(GetUserSettingActivity));
        public UserSetting UserSetting
        {
            get { return (UserSetting)base.GetValue(UserSettingProperty); }
            set { base.SetValue(UserSettingProperty, value); }
        }

        private static DependencyProperty CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(Page), typeof(GetUserSettingActivity));
        public Page CurrentPage
        {
            get { return (Page)base.GetValue(CurrentPageProperty); }
            set { base.SetValue(CurrentPageProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using( new TimedLog(UserGuid.ToString(), "Activity: Get User Setting") )
            {
                using (var db = DatabaseHelper.GetDashboardData())
                {
                    var query = from u in db.UserSettings
                                where u.UserId == UserGuid
                                select u;

                    IEnumerator<UserSetting> e = query.GetEnumerator();

                    if (e.MoveNext())
                    {
                        this.UserSetting = e.Current;
                    }
                    else
                    {
                        // No setting saved before. Create default setting
                        UserSetting newSetting = new UserSetting();
                        newSetting.UserId = UserGuid;
                        newSetting.CurrentPageId = (from page in db.Pages
                                                    where page.UserId == UserGuid
                                                    select page.ID).First();

                        db.UserSettings.InsertOnSubmit(newSetting);
                        db.SubmitChanges();

                        this.UserSetting = newSetting;
                    }

                    // Get users current page and if not available, get the first page
                    this.CurrentPage = db.Pages.SingleOrDefault(page => page.ID == this.UserSetting.CurrentPageId)
                        ?? db.Pages.First(page => page.UserId == this.UserGuid);
                }

                return ActivityExecutionStatus.Closed;
            }
        }

	}
}

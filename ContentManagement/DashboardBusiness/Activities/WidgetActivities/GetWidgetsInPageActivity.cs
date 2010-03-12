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

using System.Data.Linq;
using System.Linq;

using DashboardDataAccess;

namespace DashboardBusiness.Activities
{
    public partial class GetWidgetsInPageActivity : System.Workflow.ComponentModel.Activity
	{
		public GetWidgetsInPageActivity()
		{
			InitializeComponent();
		}

        private static DependencyProperty WidgetInstancesProperty = DependencyProperty.Register("WidgetInstances", typeof(List<WidgetInstance>), typeof(GetWidgetsInPageActivity));
        public List<WidgetInstance> WidgetInstances
        {
            get { return (List<WidgetInstance>)base.GetValue(WidgetInstancesProperty); }
            set { base.SetValue(WidgetInstancesProperty, value); }
        }

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(GetWidgetsInPageActivity));


        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetWidgetsInPageActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using( new TimedLog(UserGuid.ToString(), "Activity: Get Widgets in page: " + PageId) )
            {
                using (var db = DatabaseHelper.GetDashboardData())
                {
                    DataLoadOptions options = new DataLoadOptions();
                    options.LoadWith<WidgetInstance>(w => w.Widget);
                    options.LoadWith<WidgetInstance>(w => w.Page);
                    db.LoadOptions = options;
                    // Load widget instances along with the Widget definition
                    // for the specified page
                    this.WidgetInstances = (from widgetInstance in db.WidgetInstances
                                            where widgetInstance.PageId == this.PageId
                                            orderby widgetInstance.ColumnNo, widgetInstance.OrderNo
                                            select widgetInstance)
                                    .ToList();

                }                

                return ActivityExecutionStatus.Closed;
            }
        }
	}
}

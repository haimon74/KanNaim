using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using DashboardDataAccess;
using DashboardBusiness.Activities;

namespace DashboardBusiness
{
	public sealed partial class DeletePageWorkflow: SequentialWorkflowActivity
	{
		public DeletePageWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.DeletePageWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.DeletePageWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeletePageWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty PageIDProperty = DependencyProperty.Register("PageID", typeof(System.Int32), typeof(DashboardBusiness.DeletePageWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 PageID
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.DeletePageWorkflow.PageIDProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeletePageWorkflow.PageIDProperty, value);
            }
        }

        public static DependencyProperty CurrentPageIdProperty = DependencyProperty.Register("CurrentPageId", typeof(System.Int32), typeof(DashboardBusiness.DeletePageWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 CurrentPageId
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.DeletePageWorkflow.CurrentPageIdProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeletePageWorkflow.CurrentPageIdProperty, value);
            }
        }

        public static DependencyProperty WidgetInstancesProperty = DependencyProperty.Register("WidgetInstances", typeof(System.Collections.Generic.List<DashboardDataAccess.WidgetInstance>), typeof(DashboardBusiness.DeletePageWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public System.Collections.Generic.List<DashboardDataAccess.WidgetInstance> WidgetInstances
        {
            get
            {
                return ((System.Collections.Generic.List<DashboardDataAccess.WidgetInstance>)(base.GetValue(DashboardBusiness.DeletePageWorkflow.WidgetInstancesProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeletePageWorkflow.WidgetInstancesProperty, value);
            }
        }

        private void OnForEach(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var widgetInstanceToDelete = forEachActivity.Enumerator.Current as WidgetInstance;
            widgetInstanceToDelete.Detach();
            (forEachActivity.DynamicActivity as DeleteWidgetInstanceActivity).WidgetInstance = widgetInstanceToDelete;
        }

        public static DependencyProperty NewCurrentPageProperty = DependencyProperty.Register("NewCurrentPage", typeof(Page), typeof(DashboardBusiness.DeletePageWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Page NewCurrentPage
        {
            get
            {
                return ((DashboardDataAccess.Page)(base.GetValue(DashboardBusiness.DeletePageWorkflow.NewCurrentPageProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeletePageWorkflow.NewCurrentPageProperty, value);
            }
        }
	}

}

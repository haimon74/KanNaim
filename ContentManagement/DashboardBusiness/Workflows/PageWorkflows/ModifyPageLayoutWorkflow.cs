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

namespace DashboardBusiness
{
	public sealed partial class ModifyPageLayoutWorkflow: SequentialWorkflowActivity
	{
		public ModifyPageLayoutWorkflow()
		{
			InitializeComponent();
		}

        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(DashboardBusiness.ModifyPageLayoutWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(DashboardBusiness.ModifyPageLayoutWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.ModifyPageLayoutWorkflow.UserNameProperty, value);
            }
        }

        public static DependencyProperty PageIDProperty = DependencyProperty.Register("PageID", typeof(System.Int32), typeof(DashboardBusiness.ModifyPageLayoutWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 PageID
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.ModifyPageLayoutWorkflow.PageIDProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.ModifyPageLayoutWorkflow.PageIDProperty, value);
            }
        }

        public static DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(System.Int32), typeof(DashboardBusiness.ModifyPageLayoutWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 LayoutType
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.ModifyPageLayoutWorkflow.LayoutTypeProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.ModifyPageLayoutWorkflow.LayoutTypeProperty, value);
            }
        }


	}

}

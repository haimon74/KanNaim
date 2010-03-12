
using System;
using System.Data.Linq;

namespace DashboardDataAccess
{
    public partial class Page
    {
        public void Detach()
        {
            PropertyChanged = null;
            PropertyChanging = null;

            _WidgetInstances = new EntitySet<WidgetInstance>(new Action<WidgetInstance>(this.attach_WidgetInstances), new Action<WidgetInstance>(this.detach_WidgetInstances));
            _aspnet_User = default(EntityRef<aspnet_User>);            
        }
    }
}

using System.Data.Linq;

namespace DashboardDataAccess
{
    public partial class WidgetInstance
    {
        public void Detach()
        {
            PropertyChanged = null;
            PropertyChanging = null;
            
            _Page = default(EntityRef<Page>);
            _Widget = default(EntityRef<Widget>);
        }
    }
}

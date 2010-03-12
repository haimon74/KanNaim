using System.Data.Linq;

namespace DashboardDataAccess
{
    public partial class UserSetting
    {
        public void Detach()
        {
            PropertyChanged = null;
            PropertyChanging = null;

            _aspnet_User = default(EntityRef<aspnet_User>);
        }
    }
}

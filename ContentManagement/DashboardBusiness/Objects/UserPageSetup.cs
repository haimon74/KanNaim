
using System.Collections.Generic;
using System.Linq;
using DashboardDataAccess;

namespace DashboardBusiness
{    
    public class UserPageSetup
	{
        public UserPageSetup()
        {
        }

        private List<Page> _Pages;

        public List<Page> Pages
        {
            get { return _Pages; }
            set { _Pages = value; }
        }

        public Page CurrentPage
        {
            get { return (from page in this.Pages where page.ID == this.UserSetting.CurrentPageId select page).Single(); }
        }

        private UserSetting _UserSetting;

        public UserSetting UserSetting
        {
            get { return _UserSetting; }
            set { _UserSetting = value; }
        }

        private List<WidgetInstance> _WidgetInstances;

        public List<WidgetInstance> WidgetInstances
        {
            get { return _WidgetInstances; }
            set { _WidgetInstances = value; }
        }        
	}
}

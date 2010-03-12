using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;



[assembly: TagPrefix("UserControlsLibrary.WebControls", "wc")]
namespace UserControlsLibrary.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SideMenuDataSource runat=server></{0}:SideMenuDataSource>")]
    public class SideMenuDataSource : SiteMapDataSource
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        private string _sideMenuId;
        [DefaultValue("SideMenu")]
        public string SideMenuId
        {
            get { return _sideMenuId; }
            set { _sideMenuId = value;}
        }
        protected override void OnLoad(EventArgs e)
        {
            EnableViewState = true;
            //int menuId = ((SideMenu)Page.FindControl(SideMenuId)).MenuID;
            int menuId = (int)Page.Session["SideMenuId"];
            SiteMapProvider = String.Format("RightSideMenuSiteMap{0}", menuId);

            base.OnLoad(e);
        }
    }
}

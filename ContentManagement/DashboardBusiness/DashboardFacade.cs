
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Transactions;
using System.Linq;
using DashboardDataAccess;

namespace DashboardBusiness
{
    public class DashboardFacade
    {
        private string _UserName;

        public DashboardFacade(string userName)
        {
            this._UserName = userName;
        }

        public UserPageSetup NewUserVisit()
        {
            using (new TimedLog(this._UserName, "New user visit"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                var userSetup = new UserPageSetup();
                properties.Add("UserPageSetup", userSetup);

                WorkflowHelper.ExecuteWorkflow(typeof(FirstVisitWorkflow), properties);

                return userSetup;
            }
        }

        public UserPageSetup LoadUserSetup(string pageTitle)
        {
            using (new TimedLog(this._UserName, "Total: Existing user visit"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("PageName", pageTitle);
                var userSetup = new UserPageSetup();
                properties.Add("UserPageSetup", userSetup);

                WorkflowHelper.ExecuteWorkflow(typeof(UserVisitWorkflow), properties);

                return userSetup;
            }
        }

        public Page AddNewPage(string layoutType)
        {
            using (new TimedLog(this._UserName, "Add New Page"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("LayoutType", layoutType);

                // NewPage will be returned after workflow completes
                properties.Add("NewPage", null);

                WorkflowHelper.ExecuteWorkflow(typeof(AddNewTabWorkflow), properties);

                return properties["NewPage"] as Page;
            }
        }


        public void DeleteWidgetInstance(WidgetInstance instance)
        {
            using (new TimedLog(this._UserName, "Delete Widget:" + instance.Title))
            {
                // Detach it from all associations so that it can be safely deleted
                instance.Detach();

                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("WidgetInstance", instance);
                WorkflowHelper.ExecuteWorkflow(typeof(DeleteWidgetInstanceWorkflow), properties);
            }
        }
        public Page DeleteCurrentPage(int pageID)
        {
            using (new TimedLog(this._UserName, "DeletePage"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("PageID", pageID);

                // Workflow will return the current page
                properties.Add("NewCurrentPage", null);

                WorkflowHelper.ExecuteWorkflow(typeof(DeletePageWorkflow), properties);

                return properties["NewCurrentPage"] as Page;
            }

        }

        public void MoveWidgetInstance(int widgetInstanceId, int toColumn, int toRow)
        {
            using (new TimedLog(this._UserName, "Move Widget:" + widgetInstanceId))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("WidgetInstanceId", widgetInstanceId);
                properties.Add("ColumnNo", toColumn);
                properties.Add("RowNo", toRow);

                WorkflowHelper.ExecuteWorkflow(typeof(MoveWidgetInstanceWorkflow), properties);
            }
        }

        public List<Widget> GetWidgetList()
        {
            using (var db = DatabaseHelper.GetDashboardData())
                return db.Widgets.OrderBy(w => w.OrderNo).ToList();
        }

        public WidgetInstance AddWidget(int widgetId)
        {
            using (new TimedLog(this._UserName, "Add Widget" + widgetId))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("WidgetId", widgetId);

                // New Widget instance will be returned after the workflow completes
                properties.Add("NewWidget", null);

                WorkflowHelper.ExecuteWorkflow(typeof(AddWidgetWorkflow), properties);

                return properties["NewWidget"] as WidgetInstance;
            }
        }

        public void RegisterAs(string email)
        {
            using (new TimedLog(this._UserName, "Register As: " + email))
            {
                using (var db = DatabaseHelper.GetDashboardData())
                {
                    MembershipUser newUser = Membership.GetUser(email);

                    // Get the User Id for the anonymous user from the aspnet_users table
                    aspnet_User anonUser = db.aspnet_Users.Single(u => u.LoweredUserName == this._UserName && u.ApplicationId == DatabaseHelper.ApplicationGuid);

                    Guid oldGuid = anonUser.UserId;
                    Guid newGuid = (Guid)newUser.ProviderUserKey;

                    // Move page ownership
                    using (TransactionScope ts = new TransactionScope())
                    {
                        List<Page> pages = db.Pages.Where(p => p.UserId == oldGuid).ToList();
                        foreach (Page page in pages)
                            page.UserId = newGuid;

                        // Change setting ownership
                        UserSetting setting = db.UserSettings.Single(u => u.UserId == oldGuid);
                        db.UserSettings.DeleteOnSubmit(setting);

                        UserSetting newSetting = new UserSetting();
                        newSetting.UserId = newGuid;
                        newSetting.CurrentPageId = setting.CurrentPageId;
                        db.UserSettings.InsertOnSubmit(newSetting);
                        db.SubmitChanges();

                        ts.Complete();
                    }
                }
            }
        }

        public void ChangePageName(string newName)
        {
            using (new TimedLog(this._UserName, "Change Page Name"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                var userSetup = new UserPageSetup();
                properties.Add("PageName", newName);

                WorkflowHelper.ExecuteWorkflow(typeof(ChangePageNameWorkflow), properties);
            }
        }


        public void ModifyPageLayout(int pageID, int newLayoutType)
        {
            using (new TimedLog(this._UserName, "ModifyPageLayout"))
            {
                var properties = new Dictionary<string, object>();
                properties.Add("UserName", this._UserName);
                properties.Add("LayoutType", newLayoutType);
                properties.Add("PageID", pageID);
                WorkflowHelper.ExecuteWorkflow(typeof(ModifyPageLayoutWorkflow), properties);
            }
        }
    }
}


// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Web.Script.Services;

using DashboardBusiness;

namespace Dropthings.Web.Framework
{
    /// <summary>
    /// Summary description for WidgetService
    /// </summary>

    public class WidgetService : WebServiceBase
    {

        public WidgetService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveWidgetInstance(int widgetId, int toColumn, int toRow)
        {
            new DashboardFacade(Profile.UserName).MoveWidgetInstance(widgetId, toColumn, toRow);
            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void DeleteWidgetInstance(int widgetId)
        {
            DashboardDataAccess.WidgetInstance WI = new DashboardDataAccess.WidgetInstance();
            WI.Id = widgetId;
            WI.Detach();

            new DashboardFacade(Profile.UserName).DeleteWidgetInstance(WI);

            Context.Cache.Remove(Profile.UserName);
        }
    }

}
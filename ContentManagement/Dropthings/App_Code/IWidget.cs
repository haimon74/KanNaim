// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DashboardDataAccess;

/// <summary>
/// Summary description for IWidget
/// </summary>
namespace Dropthings.Widget.Framework
{
    public interface IWidget
    {
        void Init(IWidgetHost host);
        void ShowSettings();
        void HideSettings();
        void Minimized();
        void Maximized();
        void Closed();
    }


    public interface IWidgetHost
    {
        int ID { get; }
        void SaveState(string state);
        string GetState();
        void Maximize();
        void Minimize();
        void Close();
        bool IsFirstLoad { get; }

        void ShowSettings();
        void HideSettings();
    }
}
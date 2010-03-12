<%@ WebHandler Language="C#" Class="Logout" %>
// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar


using System;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;

public class Logout : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        /// Expire all the cookies so browser visits us as a brand new user
        List<string> cookiesToClear = new List<string>();
        foreach (string cookieName in context.Request.Cookies)
        {
            HttpCookie cookie = context.Request.Cookies[cookieName];
            cookiesToClear.Add(cookie.Name);
        }

        foreach (string name in cookiesToClear)
        {
            HttpCookie cookie = new HttpCookie(name, string.Empty);
            cookie.Expires = DateTime.Today.AddYears(-1);

            context.Response.Cookies.Set(cookie);
        }

        context.Response.Redirect("~/Default.aspx");        
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}
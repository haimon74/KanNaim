﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : Page
{
    private readonly static string CSS_PREFIX = ConfigurationManager.AppSettings["CssPrefix"];
    private readonly static string CSS_VERSION = ConfigurationManager.AppSettings["CssVersion"];
    public BasePage()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    protected override void Render(HtmlTextWriter writer)
    {
        this.ReplaceThemeCssLink();
        base.Render(writer);
    }

    private void ReplaceThemeCssLink()
    {
        string themeName = Page.Theme;
        if (string.IsNullOrEmpty(themeName)) return;

        // Find out all the Css links that are generated on the theme folder because
        // they will be replaced with one single link to CssHandler.ashx
        List<HtmlLink> linksToRemove = new List<HtmlLink>();
        foreach (Control c in Page.Header.Controls)
            if (c is HtmlLink)
                if ((c as HtmlLink).Href.Contains("App_Themes/" + themeName))
                    linksToRemove.Add(c as HtmlLink);

        // Remove all of them and create a comma delimited file name list as the 
        // CssHandler needs to know what files to combine on the theme folder
        string themeCssNames = "";
        linksToRemove.ForEach(new Action<HtmlLink>(delegate(HtmlLink link)
            {
                Page.Header.Controls.Remove(link);
                themeCssNames += VirtualPathUtility.GetFileName(link.Href) + ',';
            }));

        // Produce a new <link> tag that will hit hte CssHandler.ashx with the 
        // necessary theme information. Using Literal because HtmlLink encodes
        // the href attribute and screws up the URL
        Literal linkTag = new Literal();

        string cssPath = CSS_PREFIX + "CssHandler.ashx?t=" + themeName
            + "&f=" + HttpUtility.UrlEncode(themeCssNames.TrimEnd(','))
            + "&v=" + CSS_VERSION;

        linkTag.Text = string.Format(@"<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />", cssPath);        
        Page.Header.Controls.Add(linkTag);
    }
}

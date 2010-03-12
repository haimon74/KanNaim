// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DashboardBusiness;

public partial class LoginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LoginButton_Click( object sender, EventArgs e)
    {
        if( Membership.ValidateUser( Email.Text, Password.Text ) )
        {
            FormsAuthentication.RedirectFromLoginPage( Email.Text, RememberMeCheckbox.Checked );
        }
        else
        {
            InvalidLoginLabel.Visible = true;
        }
    }

    protected void RegisterButton_Click( object sender, EventArgs e)
    {
        try
        {
            MembershipUser user = Membership.CreateUser( Email.Text, Password.Text, Email.Text );
            
            if( user != null )
            {
                new DashboardFacade(Profile.UserName).RegisterAs( Email.Text );
                FormsAuthentication.RedirectFromLoginPage( Email.Text, RememberMeCheckbox.Checked );
                Response.Redirect( "~/Default.aspx" );
            }
            else
            {
                InvalidLoginLabel.Visible = true;
            }
        }
        catch(Exception x )
        {
            Debug.WriteLine(x);
            InvalidLoginLabel.Visible = true;
            InvalidLoginLabel.Text = x.Message;
        }
    }
}

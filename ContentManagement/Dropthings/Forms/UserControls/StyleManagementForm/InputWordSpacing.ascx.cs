﻿using System;

public partial class UserControls_InputWordSpacing : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SelectedValue
    {
        get{return WordSpacingDropDownList.SelectedValue;}
    }
}

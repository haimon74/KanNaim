﻿using System;

public partial class UserControls_StyleManagementForm_InputBorderStyle : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string SelectedValue
    {
        get { return BorderStyleDropDownList1.SelectedValue; }
    }
}

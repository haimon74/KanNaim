﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputTextIdentationInPixels.ascx.cs" Inherits="UserControls_StyleManagementForm_InputTextIdentationInPixels" %>


<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="TextIdentationInPixels" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="הזזה מהתחלת פיסקה"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox runat="server" ID="TextBox1"></asp:TextBox>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>

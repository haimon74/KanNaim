﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputBackgroundColor.ascx.cs" Inherits="UserControls_StyleManagementForm_InputBackgroundColor" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputBackgroundColor" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="צבע רקע"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxBackgroundColor" runat="server"></asp:TextBox>
        <cc1:ColorPickerExtender ID="TextBoxBackgroundColor_ColorPickerExtender"  
            runat="server" Enabled="True" TargetControlID="TextBoxBackgroundColor">
        </cc1:ColorPickerExtender>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="true"  
                TargetControlID="TextBoxBackgroundColor" WatermarkText="RRGGBB" WatermarkCssClass="watermark">
        </cc1:TextBoxWatermarkExtender>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

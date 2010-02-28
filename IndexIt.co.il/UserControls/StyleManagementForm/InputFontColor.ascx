<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputFontColor.ascx.cs" Inherits="UserControls_StyleManagementForm_InputFontColor" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputFontColor" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="Label3" runat="server" Text="צבע פנים"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxColor" runat="server"></asp:TextBox>
        <cc1:ColorPickerExtender ID="TextBoxColor_ColorPickerExtender" runat="server" 
            Enabled="True" TargetControlID="TextBoxColor">
        </cc1:ColorPickerExtender>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>

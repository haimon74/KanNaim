<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputFontSize.ascx.cs" Inherits="UserControls_StyleManagementForm_InputFontSize" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputFontSize" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="Label2" runat="server" Text="גודל גופן" Font-Names="Arial"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:FontSizeDropDownList ID="DropDownListFontSize" runat="server">
        </wc:FontSizeDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>
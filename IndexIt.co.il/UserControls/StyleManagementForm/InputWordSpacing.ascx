<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputWordSpacing.ascx.cs" Inherits="UserControls_InputWordSpacing" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputWordSpacing" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="מרווח בין מילים"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:WordSpacingDropDownList ID="WordSpacingDropDownList" runat="server">
        </wc:WordSpacingDropDownList>
f    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>


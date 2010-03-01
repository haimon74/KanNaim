<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputTextDirection.ascx.cs" Inherits="UserControls_StyleManagementForm_InputTextDirection" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputTextDirection"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="כיוון טקסט"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:TextDirectionDropDownList runat="server" ID="TextDirectionDropDownList"></wc:TextDirectionDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>
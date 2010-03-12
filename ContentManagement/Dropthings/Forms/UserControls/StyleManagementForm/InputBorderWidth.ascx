<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputBorderWidth.ascx.cs" Inherits="UserControls_StyleManagementForm_InputBorderWidth" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputBorderWidth"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="עובי גבול"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:BorderWidthDropDownList ID="BorderWidthDropDownList" runat="server">
        </wc:BorderWidthDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

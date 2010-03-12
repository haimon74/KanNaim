<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputBorderStyle.ascx.cs" Inherits="UserControls_StyleManagementForm_InputBorderStyle" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputBorderStyle"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="סוג גבול"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:BorderStyleDropDownList ID="BorderStyleDropDownList1"  EnableViewState="true" runat="server">
        </wc:BorderStyleDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputPositionType.ascx.cs" Inherits="UserControls_StyleManagementForm_InputPositionType" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputPositionType"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="סוג מיקום"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:PositionDropDownList ID="PositionDropDownList1"  runat="server">
        </wc:PositionDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />


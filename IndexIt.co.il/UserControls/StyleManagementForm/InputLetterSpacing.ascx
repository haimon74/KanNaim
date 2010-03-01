<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputLetterSpacing.ascx.cs" Inherits="UserControls_StyleManagementForm_InputLetterSpacing" %>


<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputLetterSpacing"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="מרווח בין אותיות"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:LetterSpacingDropDownList ID="LetterSpacingDropDownList1" runat="server">
        </wc:LetterSpacingDropDownList>        
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputFontFamily.ascx.cs" Inherits="UserControls_StyleManagementForm_InputFontFamily" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputFontFamily"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="סוג גופן"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:FontFamilyDropDownList ID="FontFamilyDropDownList1" runat="server" Language="Hebrew">
        </wc:FontFamilyDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

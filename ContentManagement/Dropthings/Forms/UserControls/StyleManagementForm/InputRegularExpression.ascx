<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputRegularExpression.ascx.cs" Inherits="UserControls_StyleManagementForm_InputRegularExpression" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputRegularExpression" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1"  EnableViewState="true" runat="server" Enabled="true"  TargetControlID="TextBox1">
        </cc1:TextBoxWatermarkExtender>
    </div>
    <div class="InputValidation">
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"  runat="server" 
             ControlToValidate="TextBox1" Display="Dynamic" Text=" * ">
        </asp:RegularExpressionValidator>
    </div>    
</div>
<br />

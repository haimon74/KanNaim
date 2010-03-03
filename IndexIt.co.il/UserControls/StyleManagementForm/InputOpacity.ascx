<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputOpacity.ascx.cs" Inherits="UserControls_StyleManagementForm_InputOpacity" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<<div id="InputOpacity" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label2" runat="server" Text="שקיפות התמונה" ToolTip="note: '0' is full ocpacity - transparent"></asp:Label>
        <asp:Label ID="Label3" runat="server" Font-Size="Smaller" ForeColor="Red" ToolTip="note: '0' is full ocpacity - transparent"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxOpacity" runat="server"></asp:TextBox>
        <cc1:SliderExtender ID="TextBoxOpacity_SliderExtender" TooltipText="Value={0}"
            Minimum="0" Maximum="100" Length="200" Steps="100" runat="server" Enabled="True"
            TargetControlID="TextBoxOpacity" BoundControlID="Label3">
        </cc1:SliderExtender>
    </div>
    <div class="InputValidation">
    </div>
</div>
<br />

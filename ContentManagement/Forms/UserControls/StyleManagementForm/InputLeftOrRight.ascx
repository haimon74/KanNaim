<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputLeftOrRight.ascx.cs" Inherits="UserControls_StyleManagementForm_InputLeftOrRight" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputLeftOrRight" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="LabelV" runat="server" Text="אופקי"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:RadioButtonList ID="RadioButtonListLeftRight" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Selected="True" Value="right">מימין</asp:ListItem>
            <asp:ListItem Value="left">משמאל</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="InputValidation">        
    </div>    
</div>
<br />


                        
                        
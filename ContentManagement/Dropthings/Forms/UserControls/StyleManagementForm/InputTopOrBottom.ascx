<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputTopOrBottom.ascx.cs" Inherits="UserControls_StyleManagementForm_InputTopOrBottom" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputTopOrBottom" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="LabelV" runat="server" Text="אנכי"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:RadioButtonList ID="RadioButtonListTopBottom"  EnableViewState="true" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Selected="True" Value="top">מלמעלה</asp:ListItem>
            <asp:ListItem Value="bottom">מלמטה</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="InputValidation">        
    </div>    
</div>
<br />

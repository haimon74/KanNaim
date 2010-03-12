<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputRoundedBorderStyle.ascx.cs" Inherits="Forms_UserControls_StyleManagementForm_InputRoundedBorderStyle" %>


<div id="InputRoundedBorderStyle" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="LabelRoundedBorders" runat="server" Text="צורת גבול"></asp:Label><br />
        <asp:Panel ID="Square" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 0em 0em 0em 0em ;-webkit-border-radius: 0em 0em 0em 0em; " />
        <asp:Panel ID="PanelTop" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 100% 100% 0em 0em ;-webkit-border-radius: 100% 100% 0em 0em; " />
        <asp:Panel ID="PanelTopRight" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 250% 100% 250% 250%  ;-webkit-border-radius: 250% 100% 250% 250% ; " />
        <asp:Panel ID="PanelTopLeft" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 100% 250% 250% 250%  ;-webkit-border-radius: 100% 250% 250% 250%  " />
        <asp:Panel ID="PanelBottom" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 0em 0em 100% 100%  ;-webkit-border-radius: 0em 0em 100% 100% ; " />
        <asp:Panel ID="PanelBottomRight" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 250% 250% 100% 250% ;-webkit-border-radius: 250% 250% 100% 250%; " />
        <asp:Panel ID="PanelBottomLeft" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 250% 250% 250% 100%  ;-webkit-border-radius: 250% 250% 250% 100% ; " />
        <asp:Panel ID="PanelTopBottomSym" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 100% 100% 100% 100% ;-webkit-border-radius: 100% 100% 100% 100% ; " />
        <asp:Panel ID="PanelTopBottom1" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 200% 50% 200% 50% ;-webkit-border-radius: 200% 50% 200% 50% ; " />
        <asp:Panel ID="PanelTopBottom2" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 50% 200% 50% 200% ;-webkit-border-radius:  50% 200% 50% 200% ; " />
        <asp:Panel ID="PanelCircle" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 300% 300% 300% 300%  ;-webkit-border-radius: 300% 300% 300% 300% ; " />
        <asp:Panel ID="PanelObeliqueTop" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 200% 150% 200% 150%  ;-webkit-border-radius: 200% 150% 200% 150% " />
        <asp:Panel ID="PanelObeliqueBottom" runat="server" Width="30" Height="15" Style="background: #dddddd; margin: 1px; border: solid 3px #0000dd; -moz-border-radius: 150% 200% 150% 200%  ;-webkit-border-radius: 150% 200% 150% 200% ; " />
    </div>
    <div class="InputContent">
        <asp:RadioButtonList ID="RadioButtonListRoundedBorders" AutoPostBack="true" OnSelectedIndexChanged="IndexChanged" 
                                    EnableViewState="true" runat="server" RepeatDirection="Vertical">
            <asp:ListItem Selected="True" Value="Square">מלבני</asp:ListItem>
            <asp:ListItem Value="Top">מעוגל למעלה</asp:ListItem>
            <asp:ListItem Value="TopRight">מלמעלה ימין</asp:ListItem>
            <asp:ListItem Value="TopLaft">מלמעלה שמאל</asp:ListItem>
            <asp:ListItem Value="Bottom">מעוגל למטה</asp:ListItem>
            <asp:ListItem Value="BottomRight">מלמטה ימין</asp:ListItem>
            <asp:ListItem Value="BottomLeft">מלמטה שמאל</asp:ListItem>
            <asp:ListItem Value="TopBottomSym">למעלה ולמטה</asp:ListItem>
            <asp:ListItem Value="TopBottom1">אלכסוני 1</asp:ListItem>
            <asp:ListItem Value="TopBottom2">אלכסוני 2</asp:ListItem>
            <asp:ListItem Value="Circle">עיגול/אליפסה סימטרית</asp:ListItem>
            <asp:ListItem Value="ObeliqueTop">אליפסה אסימטרית 1</asp:ListItem>            
            <asp:ListItem Value="ObeliqueBottom">אליפסה אסימטרית 2</asp:ListItem>
        </asp:RadioButtonList>
        
        
        
        
    </div>
    <div class="InputValidation">        
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel runat="server" ID="Up1">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="RadioButtonListRoundedBorders" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="PanelPreviewSelectedBorder" runat="server"  Width="200" Height="100" 
                    Style="background: #dddddd; border: solid 3px #0000dd; -moz-border-radius: 0em 0em 0em 0em ;-webkit-border-radius: 0em 0em 0em 0em ; " />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</div>
<br />

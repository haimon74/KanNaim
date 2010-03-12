<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SlideShowControl.ascx.cs" Inherits="Forms_UserControls_SlideShowControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="System.IO"%>
<%@ Import Namespace="System.Collections.Generic"%>


<script runat="Server" type="text/C#">
       
    
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ButtonPrev" />
        <asp:AsyncPostBackTrigger ControlID="ButtonPlay" />
        <asp:AsyncPostBackTrigger ControlID="ButtonNext" />
    </Triggers>
    <ContentTemplate>
        <asp:Label ID="lblTitle" runat="server"></asp:Label>
        <br />
        <asp:Panel ID="PanelSlidShow" runat="server">
            
        </asp:Panel> 
        <br />
        <asp:Label ID="lblDescription" runat="server"></asp:Label><br />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Button ID="ButtonPrev" Text="Prev" runat="server" />
<asp:Button ID="ButtonPlay" Text="Play" runat="server" />
<asp:Button ID="ButtonNext" Text="Next" runat="server" /> 
<p>
    &nbsp;</p>
<asp:Button ID="ButtonTest" runat="server" onclick="ButtonTest_Click" 
    Text="Button" />
 
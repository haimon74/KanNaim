<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetContainer.ascx.cs" Inherits="WidgetContainer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CustomDragDrop" Namespace="CustomDragDrop" TagPrefix="cdd" %>
<asp:Panel ID="Widget" CssClass="widget" runat="server" onmouseover="this.className='widget widget_hover'" onmouseout="this.className='widget'">        
    <asp:Panel id="WidgetHeader" CssClass="widget_header" runat="server">
        <asp:UpdatePanel ID="WidgetHeaderUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>        
            <table class="widget_header_table" cellspacing="0" cellpadding="0">
            <tbody>
            <tr>
            <td class="widget_title"><asp:LinkButton ID="WidgetTitle" runat="Server" Text="Widget Title" OnClick="WidgetTitle_Click" /><asp:TextBox ID="WidgetTitleTextBox" runat="Server" Visible="False" /><asp:Button ID="SaveWidgetTitle" runat="Server" OnClick="SaveWidgetTitle_Click" Visible="False" Text="OK" /></td>
            <td class="widget_edit"><asp:LinkButton ID="EditWidget" runat="Server" Text="edit" OnClick="EditWidget_Click" /><asp:LinkButton ID="CancelEditWidget" runat="Server" Text="close edit" OnClick="EditWidget_Click" Visible="false" /></td>
            <td class="widget_button"><asp:LinkButton ID="CollapseWidget" runat="Server" Text="" OnClick="CollapseWidget_Click" CssClass="widget_min widget_box" /><asp:LinkButton ID="ExpandWidget" runat="Server" Text="" CssClass="widget_max widget_box" OnClick="ExpandWidget_Click" /></td>
            <td class="widget_button"><asp:LinkButton ID="CloseWidget" runat="Server" Text="" CssClass="widget_close widget_box" OnClick="CloseWidget_Click" /></td>
            </tr>
            </tbody>
            </table>            
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="WidgetHeaderUpdatePanel" >
    <ProgressTemplate><center>Working...</center></ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="WidgetBodyUpdatePanel" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
        <asp:Panel ID="WidgetBodyPanel" runat="Server" CssClass="widget_body"></asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="WidgetBodyUpdatePanel" >
    <ProgressTemplate><center>Working...</center></ProgressTemplate>
    </asp:UpdateProgress>
</asp:Panel>
<% /*<cdd:CustomFloatingBehaviorExtender ID="WidgetFloatingBehavior" DragHandleID="WidgetHeader" TargetControlID="Widget" runat="server" />*/ %>
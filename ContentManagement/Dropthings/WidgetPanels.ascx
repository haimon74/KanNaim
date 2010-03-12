<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetPanels.ascx.cs" Inherits="WidgetPanels" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CustomDragDrop" Namespace="CustomDragDrop" TagPrefix="cdd" %>
<%@ Register Src="WidgetContainer.ascx" TagName="WidgetContainer" TagPrefix="widget" %>

<table width="98%" cellspacing="5" border="0" align="left" class="table_fixed">
    <tbody>
        <tr>
            <td style="width:<%=LeftPanelSize%>;vertical-align:top" >
                <asp:UpdatePanel ID="LeftUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Panel ID="LeftPanel" runat="server"  class="widget_holder" columnNo="0">
                    <div id="DropCue1" class="widget_dropcue">
                    </div>
                </asp:Panel>
                
                <cdd:CustomDragDropExtender ID="CustomDragDropExtender1" runat="server" TargetControlID="LeftPanel"
                    DragItemClass="widget" DragItemHandleClass="widget_header" DropCueID="DropCue1"
                    OnClientDrop="DropthingsUI.Actions.onDrop" />
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="visibility:<%=MiddlePanelVisible%>" class="column_divider">&nbsp;</td>
            <td style="width:<%=MiddlePanelSize%>;vertical-align:top;visibility:<%=MiddlePanelVisible%>" >
                <asp:UpdatePanel ID="MiddleUpdatePanel" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                <asp:Panel ID="MiddlePanel" runat="server" class="widget_holder" columnNo="1">
                    <div id="DropCue2" class="widget_dropcue">
                    </div>
                </asp:Panel>
                
                <cdd:CustomDragDropExtender ID="CustomDragDropExtender2" runat="server" TargetControlID="MiddlePanel"
                    DragItemClass="widget" DragItemHandleClass="widget_header" DropCueID="DropCue2" 
                    OnClientDrop="DropthingsUI.Actions.onDrop" />

                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="visibility:<%=RightPanelVisible%>">&nbsp;</td>
            <td style="width:<%=RightPanelSize%>;vertical-align:top;visibility:<%=RightPanelVisible%>" >

            <asp:UpdatePanel ID="RightUpdatePanel" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                <asp:Panel ID="RightPanel" runat="server" class="widget_holder" columnNo="2" >
                    <div id="DropCue3" class="widget_dropcue">
                    </div>
                </asp:Panel>
                    <cdd:CustomDragDropExtender ID="CustomDragDropExtender3"  runat="server"  TargetControlID="RightPanel"
                    DragItemClass="widget" DragItemHandleClass="widget_header" DropCueID="DropCue3" 
                    OnClientDrop="DropthingsUI.Actions.onDrop" />       
                </ContentTemplate>
                </asp:UpdatePanel>
                </td>
        </tr>
    </tbody>
</table>   


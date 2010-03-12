<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SideMenu.ascx.cs" Inherits="SideMenu" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<wc:SideTreeMenu ID="SideTreeMenu1" runat="server"  MenuID="1" Visible="true" DataSourceID="SideMenuDataSource1"
                ImageSet="Arrows"/>
    <ParentNodeStyle Font-Bold="False" />
    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD"
        HorizontalPadding="0px" VerticalPadding="0px" />
    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />

<wc:SideMenuDataSource ID="SideMenuDataSource1" runat="server" SideMenuId="SideTreeMenu1"/>
















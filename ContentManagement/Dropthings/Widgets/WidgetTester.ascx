<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetTester.ascx.cs" Inherits="Widgets_WidgetTester" %>
<asp:Panel ID="SettingsPanel" runat="server">
This is the settings panel. <asp:Button ID="SettingsButton" runat="server" OnClick="SettingsButton_Click" Text="Settings" />
<asp:Button ID="CloseSettings" runat="Server" OnClick="CloseSettings_Click" Text="Close" />
</asp:Panel>
<asp:Button ID="ExpandWidget" runat="Server" OnClick="ExpandWidget_Click" Text="Expand" />
<asp:Button ID="CollapseWidget" runat="Server" OnClick="CollapseWidget_Click" Text="Collapse" />
<asp:Button ID="CloseWidget" runat="Server" OnClick="CloseWidget_Click" Text="Close" />
<asp:Label ID="Message" EnableViewState="False" runat="server" />
<asp:TextBox ID="Data" runat="Server" />
<asp:Button ID="Save" EnableViewState="False" runat="server" OnClick="Save_Click" Text="Save" />
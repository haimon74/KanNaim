<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiViewByRoles.ascx.cs" Inherits="UserControls_MultiViewByRoles" %>
<asp:MultiView ID="MultiView1" runat="server">
    <asp:View ID="ViewUser" runat="server">
        user<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </asp:View>
    <asp:View ID="ViewShopOwner" runat="server">
        shop owner<asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
    </asp:View>
    <asp:View ID="ViewAdmin" runat="server">
        admin<asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
    </asp:View>
</asp:MultiView>

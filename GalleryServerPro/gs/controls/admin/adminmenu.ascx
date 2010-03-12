<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="adminmenu.ascx.cs" Inherits="GalleryServerPro.Web.Controls.admin.adminmenu" %>
<%@ Register namespace="ComponentArt.Web.UI" assembly="ComponentArt.Web.UI" tagPrefix="ComponentArt" %>
<div class="gsp_navigation">
	<ComponentArt:NavBar ID="nbAdminMenu" runat="server" Height="500px" Width="200px"
		ExpandSinglePath="false" CssClass="nb0NavBar"
		DefaultItemLookId="TopItemLook" DefaultSelectedItemLookId="Level2SelectedItemLook">
		<ItemLooks>
			<ComponentArt:ItemLook LookId="TopItemLook" CssClass="nb0TopItem" HoverCssClass="nb0TopItemHover"
				ActiveCssClass="nb0TopItemActive" RightIconUrl="arrow.gif" ExpandedRightIconUrl="arrow_expanded.gif" />
			<ComponentArt:ItemLook LookId="Level2ItemLook" CssClass="nb0Level2Item" HoverCssClass="nb0Level2ItemHover" />
			<ComponentArt:ItemLook LookId="Level2SelectedItemLook" CssClass="nb0Level2ItemSelected"
				HoverCssClass="nb0Level2ItemSelected" />
		</ItemLooks>
		<Items>
			<ComponentArt:NavBarItem ID="NavBarItem1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Site_Settings_Hdr_Text %>" SubGroupCssClass="nb0Level2Group"
				DefaultSubItemLookId="Level2ItemLook" SelectedLookId="TopItemLook" Expanded="true" Selectable="false">
				<ComponentArt:NavBarItem ID="nbiGeneral" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Site_Settings_General_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiEmail" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Site_Settings_Email_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiBackupRestore" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Site_Settings_Backup_Restore_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiErrorLog" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Site_Settings_Error_Log_Link_Text %>" />
			</ComponentArt:NavBarItem>
			<ComponentArt:NavBarItem ID="NavBarItem5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Membership_Hdr_Text %>" SubGroupCssClass="nb0Level2Group"
				DefaultSubItemLookId="Level2ItemLook" SelectedLookId="TopItemLook" Expanded="true" Selectable="false">
				<ComponentArt:NavBarItem ID="nbiUserSettings" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Membership_User_Settings_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiManageUsers" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Membership_Manage_Users_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiManageRoles" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Membership_Manage_Roles_Link_Text %>" />
			</ComponentArt:NavBarItem>
			<ComponentArt:NavBarItem ID="NavBarItem8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Hdr_Text %>" SubGroupCssClass="nb0Level2Group"
				DefaultSubItemLookId="Level2ItemLook" SelectedLookId="TopItemLook" Expanded="true" Selectable="false">
				<ComponentArt:NavBarItem ID="nbiAlbums" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_General_Link_Text %>" />
			</ComponentArt:NavBarItem>
			<ComponentArt:NavBarItem ID="NavBarItem10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Media_Objects_Hdr_Text %>" SubGroupCssClass="nb0Level2Group"
				DefaultSubItemLookId="Level2ItemLook" SelectedLookId="TopItemLook" Expanded="true" Selectable="false">
				<ComponentArt:NavBarItem ID="nbiMediaObjects" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Media_Objects_General_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiMediaObjectTypes" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Media_Objects_Types_Link_Text %>" />
				<ComponentArt:NavBarItem ID="nbiImages" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Media_Objects_Types_Images_Text %>" />
				<ComponentArt:NavBarItem ID="nbiVideoAudioOther" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Media_Objects_Video_Audio_Other_Link_Text %>" />
			</ComponentArt:NavBarItem>
		</Items>
	</ComponentArt:NavBar>
</div>
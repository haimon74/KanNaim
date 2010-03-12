<%@ Control Language="C#" AutoEventWireup="true" Codebehind="actionmenu.ascx.cs" Inherits="GalleryServerPro.Web.uc.actionmenu" %>
<script type="text/javascript">
<!-- 
	Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(actionMenuPageLoad);
	
	function actionMenuPageLoad(sender, args)
	{
		Menu1.findItemById("mnuEditAlbum").set_enabled(window.editAlbumInfo != null);
	}
-->
</script>

<CA:Menu ID="Menu1" CssClass="mnu0TopGroup" DefaultGroupCssClass="mnu0MenuGroup" DefaultItemLookId="DefaultItemLook" DefaultGroupItemSpacing="1px"
	DefaultDisabledItemLookId="DefaultDisabledItemLook" ExpandDelay="100" EnableViewState="False" ImagesBaseUrl="~/images/componentart/menu/"
	runat="server" ScrollDownLookId="DefaultItemLook" ScrollUpLookId="DefaultItemLook" TopGroupItemSpacing="1px" OverlayWindowedElements="true">
	<ItemLooks>
		<CA:ItemLook LookId="TopItemLook" CssClass="mnu0TopMenuItem" HoverCssClass="mnu0TopMenuItemHover" ExpandedCssClass="mnu0TopMenuItemHover"
			LeftIconUrl="doubledownarrow.png" LeftIconWidth="16px" LeftIconHeight="16px" LabelPaddingLeft="3px" LabelPaddingRight="5px"
			LabelPaddingTop="2px" LabelPaddingBottom="3px" LeftIconVisibility="Always" />
		<CA:ItemLook LookId="DefaultItemLook" LabelPaddingBottom="3px" LabelPaddingLeft="7px" LabelPaddingRight="5px" LabelPaddingTop="2px"
			LeftIconHeight="16px" LeftIconWidth="16px" CssClass="mnu0MenuItem" ExpandedCssClass="mnu0MenuItemHover" HoverCssClass="mnu0MenuItemHover" />
		<CA:ItemLook LookId="DefaultDisabledItemLook" CssClass="mnu0DisabledMenuItem" HoverCssClass="mnu0DisabledMenuItemHover" LabelPaddingBottom="3px"
			LabelPaddingLeft="10px" LabelPaddingRight="5px" LabelPaddingTop="2px" LeftIconHeight="16px" LeftIconVisibility="Always"
			LeftIconWidth="16px" />
		<CA:ItemLook LookId="BreakItem" CssClass="mnu0MenuBreak" />
	</ItemLooks>
	<Items>
		<CA:MenuItem ID="mnuAction" runat="server" LookId="TopItemLook" ChildSelectedLook-LeftIconVisibility="Always" ChildSelectedLook-RightIconVisibility="Always"
			DefaultSubGroupCssClass="mnu0MenuGroup" DefaultSubGroupItemSpacing="1px" DisabledLook-LeftIconVisibility="Always" DisabledLook-RightIconVisibility="Always"
			Look-LeftIconVisibility="Always" Look-RightIconVisibility="Always" SelectedLook-LeftIconVisibility="Always" SelectedLook-RightIconVisibility="Always"
			SubGroupCssClass="mnu0MenuGroup" SubGroupItemSpacing="1px" Text="<%$ Resources:GalleryServerPro, UC_ActionMenu_Root_Text %>" Width="75">
			<CA:MenuItem ID="mnuCreateAlbum" runat="server" Look-LeftIconUrl="addalbum.png" />
			<CA:MenuItem ID="mnuEditAlbum" runat="server" Look-LeftIconUrl="addalbum.png" />
			<CA:MenuItem LookId="BreakItem" />
			<CA:MenuItem ID="mnuAddObjects" runat="server" Look-LeftIconUrl="addobject.png" />
			<CA:MenuItem LookId="BreakItem" />
			<CA:MenuItem ID="mnuMoveObjects" runat="server" Look-LeftIconUrl="moveobjects.png" />
			<CA:MenuItem ID="mnuCopyObjects" runat="server" Look-LeftIconUrl="copyobjects.png" />
			<CA:MenuItem ID="mnuMoveAlbum" runat="server" Look-LeftIconUrl="movefolder.png" />
			<CA:MenuItem ID="mnuCopyAlbum" runat="server" Look-LeftIconUrl="copyfolder.png" />
			<CA:MenuItem LookId="BreakItem" />
			<CA:MenuItem ID="mnuEditCaptions" runat="server" Look-LeftIconUrl="copyobjects.png" />
			<CA:MenuItem ID="mnuAssignThumbnail" runat="server" Look-LeftIconUrl="copyobjects.png" />
			<CA:MenuItem ID="mnuRearrangeObjects" runat="server" Look-LeftIconUrl="rearrange.png" />
			<CA:MenuItem ID="mnuRotate" runat="server" Look-LeftIconUrl="rotate.png" />
			<CA:MenuItem LookId="BreakItem" />
			<CA:MenuItem ID="mnuDeleteObjects" runat="server" Look-LeftIconUrl="delete.png" />
			<CA:MenuItem ID="mnuDeleteHiRes" runat="server" Look-LeftIconUrl="delete.png" />
			<CA:MenuItem ID="mnuDeleteAlbum" runat="server" Look-LeftIconUrl="deletealbum.png" />
			<CA:MenuItem LookId="BreakItem" />
			<CA:MenuItem ID="mnuSynch" runat="server" Look-LeftIconUrl="synchronize.png" />
			<CA:MenuItem LookId="BreakItem" />
			<CA:MenuItem ID="mnuSiteSettings" runat="server" Look-LeftIconUrl="sitesettings.png" />
		</CA:MenuItem>
	</Items>
</CA:Menu>
<CA:CallBack ID="cbDummy" runat="server" />

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="albumtreeview.ascx.cs"
	Inherits="GalleryServerPro.Web.uc.albumtreeview" %>

<script type="text/javascript">
	
	function ValidateAlbumIsSelected(ctl, args)
	{
		args.IsValid = (tv.get_selectedNode() != null);
	}

</script>

<asp:CustomValidator ID="cv" runat="server" ClientValidationFunction="ValidateAlbumIsSelected"
	OnServerValidate="cv_OnServerValidate" Display="Dynamic" CssClass="msgwarning"
	ForeColor="" Enabled="false"></asp:CustomValidator>
<CA:TreeView ID="tv" runat="server" ExpandNodeOnSelect="false" CollapseNodeOnSelect="false"
	CssClass="tv0TreeView" NodeCssClass="tv0TreeNode" SelectedNodeCssClass="tv0HoverTreeNode"
	HoverNodeCssClass="tv0HoverTreeNode" LineImageWidth="19" LineImageHeight="20" ShowLines="True"
	LineImagesFolderUrl="~/images/componentart/treeview/lines" KeyboardEnabled="false">
	<ClientEvents>
		<NodeSelect EventHandler="tv_onNodeSelect" />
		<NodeBeforeCheckChange EventHandler="tv_onNodeBeforeCheckChange" />
	</ClientEvents>
</CA:TreeView>

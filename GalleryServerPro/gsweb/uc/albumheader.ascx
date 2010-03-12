<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="albumheader.ascx.cs"
	Inherits="GalleryServerPro.Web.uc.albumheader" EnableViewState="false" %>
<asp:ScriptManagerProxy ID="smp" runat="server">
	<Scripts>
		<asp:ScriptReference Path="~/script/entityobjects.js" />
	</Scripts>
</asp:ScriptManagerProxy>
<asp:Panel ID="pnlAlbumHeader" runat="server" CssClass="albumHeaderReadOnly">
	<asp:Label ID="lblStats" runat="server" CssClass="albumHeaderStats" />
	<h1>
		<asp:Literal ID="litAlbum" runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Album_Title_Header_Text %>" />
		<img id="albumPrivateIcon" src="<%= this.PageBase.ThemePath %>/images/lock_24x24.png"
			style="width: 24px; height: 24px; margin-bottom: -5px;" class="<%= GetAlbumPrivateCssClass() %>"
			alt='<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Album_Is_Private_Icon_Tooltip %>" />'
			title='<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Album_Is_Private_Icon_Tooltip %>" />' />
		<asp:Label ID="lblTitle" runat="server" /></h1>
	<p class="minimargin">
		<asp:Label ID="lblSummaryLabel" runat="server" CssClass="bold" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Album_Summary_Header_Text %>" />
		<asp:Label ID="lblSummary" runat="server" /></p>
	<p id="dateContainer" runat="server">
		<asp:Label ID="lblDateLabel" runat="server" CssClass="bold" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Album_Date_Header_Text %>" />
		<asp:Label ID="lblDate" runat="server" /></p>
</asp:Panel>
<asp:PlaceHolder ID="phEditAlbumDialog" runat="server" />
<CA:CallBack ID="cbDummy" runat="server" />

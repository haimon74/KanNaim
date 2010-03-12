<%@ Control Language="C#" AutoEventWireup="true" Codebehind="albummenu.ascx.cs" Inherits="GalleryServerPro.Web.uc.albummenu" %>
<div class="albumMenuContainer">
	<asp:PlaceHolder ID="phActionMenu" runat="server" />
	<div class="<% =GetAlbumMenuClass() %>">
		<asp:PlaceHolder ID="phMenu" runat="server" />
	</div>
</div>
<CA:CallBack ID="cbDummy" runat="server" />

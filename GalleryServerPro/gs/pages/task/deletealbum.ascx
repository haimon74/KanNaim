<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="deletealbum.ascx.cs"
	Inherits="GalleryServerPro.Web.gs.pages.task.deletealbum" %>
<%@ Register Src="../../controls/albumheader.ascx" TagName="albumheader" TagPrefix="uc1" %>
<%@ Register Src="../../Controls/usermessage.ascx" TagName="usermessage" TagPrefix="uc1" %>
<div class="gsp_content">
	<asp:PlaceHolder ID="phTaskHeader" runat="server" />
	<uc1:usermessage ID="ucUserMessage" runat="server" Visible="false" CssClass="um0ContainerCss"
		IconStyle="Error" DetailCssClass="um1DetailCss" />
	<p class="gsp_textcol gsp_msgwarning">
		<asp:Literal ID="litId" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Delete_Album_Warning %>" /></p>
	<uc1:albumheader ID="ucAlbumHeader" runat="server" EnableAlbumDownload="false" />
	<asp:PlaceHolder ID="phTaskFooter" runat="server" />
</div>

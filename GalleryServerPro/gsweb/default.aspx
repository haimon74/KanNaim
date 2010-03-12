<%@ Page Language="C#" MasterPageFile="~/master/browse.master" AutoEventWireup="true" Codebehind="default.aspx.cs" Inherits="GalleryServerPro.Web._default"	Title="Gallery Server Pro" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.browse" %>
<%@ Register Src="uc/albumheader.ascx" TagName="albumheader" TagPrefix="uc1" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="Server" EnableViewState="false">
	<asp:PlaceHolder ID="phAlbumHeaderContainer" runat="server" />
	<asp:PlaceHolder ID="phMessage" runat="server" EnableViewState="False" />
	<div class="addleftpadding1">
		<asp:PlaceHolder ID="phMediaObjectContainer" runat="server" />
	</div>
	<CA:Callback ID="cbDummy" runat="server" />
</asp:Content>

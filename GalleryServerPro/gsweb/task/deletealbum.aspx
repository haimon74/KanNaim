<%@ Page Language="C#" MasterPageFile="~/master/taskMstr.master" AutoEventWireup="true"
	CodeBehind="deletealbum.aspx.cs" Inherits="GalleryServerPro.Web.task.deletealbum" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.taskMstr" %>
<%@ Register TagPrefix="uc1" TagName="albumheader" Src="../uc/albumheader.ascx" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="Server">
	<p class="textcol msgwarning">
		<asp:Literal ID="litId" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Delete_Album_Warning %>" /></p>
	<uc1:albumheader ID="ucAlbumHeader" runat="server" />
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="thumbnailview.ascx.cs" Inherits="GalleryServerPro.Web.uc.thumbnailview" EnableViewState="false" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<asp:PlaceHolder ID="phMsg" Runat="server" />
<asp:repeater id="rptr" runat="server">
	<itemtemplate>
		<div class="<%# GetThumbnailCssClass(Container.DataItem.GetType()) %>"><%# GetAlbumText(Eval("Title").ToString(), Container.DataItem.GetType()) %><div class="op0" style="width:<%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Thumbnail.Width")) + 15).ToString() %>px;height: <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Thumbnail.Height")) + 10).ToString() %>px;"><div class="op1"><div class="op2"><div class="sb"><div class="ib"><a href="<%# GenerateUrl((IGalleryObject) Container.DataItem) %>"
        title="<%# GetHovertip((IGalleryObject) Container.DataItem) %>"><img src="<%# GetThumbnailUrl((IGalleryObject)Container.DataItem) %>" alt="<%# GetHovertip((IGalleryObject) Container.DataItem) %>" style="width:<%# DataBinder.Eval(Container.DataItem, "Thumbnail.Width").ToString() %>px;height:<%# DataBinder.Eval(Container.DataItem, "Thumbnail.Height").ToString() %>px;" /></a></div></div></div></div></div>
			<%# GetGalleryObjectText(Eval("Title").ToString(), Container.DataItem.GetType())%>
		</div>
	</itemtemplate>
</asp:repeater>

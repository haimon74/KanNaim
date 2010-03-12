<%@ Page Language="C#" MasterPageFile="~/master/site.master" AutoEventWireup="true"
	CodeBehind="search.aspx.cs" Inherits="GalleryServerPro.Web.search" Title="Search Gallery Server" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.site" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<%@ Register Src="uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ PreviousPageType TypeName="GalleryServerPro.Web.GspPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c2" runat="server">
	<h1>
		<asp:Literal ID="lit1" runat="server" Text="<%$ Resources:GalleryServerPro, Search_Hdr %>" /></h1>
	<p>
		<asp:Literal ID="lit2" runat="server" Text="<%$ Resources:GalleryServerPro, Search_Box_Hdr %>" />
		<asp:TextBox ID="txtSearch" runat="server" Width="350px"></asp:TextBox>&nbsp;<asp:Button
			ID="btnSearch" runat="server" Text="<%$ Resources:GalleryServerPro, Search_Search_Button_Text %>" /><asp:Label ID="lblZipInfo" runat="server" /></p>
	<h2 id="searchResultTitle" runat="server">
	</h2>
	<asp:PlaceHolder ID="phMediaObjectContainer" runat="server" EnableViewState="False" />
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="pi1" runat="server" ControlId="lblZipInfo" DialogTitle="<%$ Resources:GalleryServerPro, Search_Popup_Help_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Search_Popup_Help_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

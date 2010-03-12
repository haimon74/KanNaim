<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="default.aspx.cs" Inherits="GalleryServerPro.Web.admin._default" Title="Gallery Server Pro - General Site Settings" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<div id="verContainer">
			<p class="verHdr">
				<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_GSP_Hdr %>" />
				<asp:Label ID="lblVersion" runat="server" CssClass="msgfriendly" /></p>
			<p class="nopadding">
				<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_GSP_Home_Page %>" />
			</p>
		</div>
		<p class="addtopmargin5">
			<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ProductKey_Label %>" />:
			<asp:TextBox ID="txtProductKey" runat="server" />&nbsp;<asp:Button ID="btnEnterProductKey" runat="server" Text="Enter product key"
				OnClick="btnEnterProductKey_Click" />
		</p>
		<p class="nopadding">
			<asp:Image ID="imgProductKeyValidation" runat="server" Width="16" Height="16" Visible="false" />&nbsp;<asp:Label ID="lblProductKeyValidationMsg" runat="server" />
		</p>
		<p class="addtopmargin5">
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_GalleryId_Label %>" />:
			<asp:TextBox ID="txtGalleryId" runat="server" />
		</p>
		<p class="addtopmargin5">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitle_Label %>" />:
		</p>
		<p class="nomargin nopadding">
			<asp:TextBox ID="txtWebsiteTitle" runat="server" CssClass="textbox" /></p>
		<p class="addtopmargin5">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitleUrl_Label %>" />:
		</p>
		<p class="nomargin nopadding">
			<asp:TextBox ID="txtWebsiteTitleUrl" runat="server" CssClass="textbox" /></p>
		<p class="addtopmargin5">
			<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitleForTitleTag_Label %>" />:
		</p>
		<p class="nomargin nopadding">
			<asp:TextBox ID="txtWebsiteTitleForTitleTag" runat="server" CssClass="textbox" />
		</p>
		<p class="addtopmargin5">
			<asp:CheckBox ID="chkAllowHtml" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_AllowHtml_Label %>" />
		</p>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="GalleryId" ControlId="txtGalleryId" IsRequired="True" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_GalleryId_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" ControlId="txtWebsiteTitle"
				BindingSource="Master.CoreConfig" BindingSourceMember="PageHeaderText" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitle_Label %>"
				IsRequired="True" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" ControlId="txtWebsiteTitleUrl"
				BindingSource="Master.CoreConfig" BindingSourceMember="PageHeaderTextUrl" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitleUrl_Label %>"
				IsRequired="True" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WebsiteTitle" ControlId="txtWebsiteTitleForTitleTag" IsRequired="True"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitleForTitleTag_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="AllowHtmlInTitlesAndCaptions" ControlId="chkAllowHtml" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_AllowHtml_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="txtProductKey" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_productKey_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_productKey_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="txtGalleryId" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_galleryId_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_galleryId_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="txtWebsiteTitle"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_pageHeaderText_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_pageHeaderText_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="txtWebsiteTitleUrl"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_pageHeaderTextUrl_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_pageHeaderTextUrl_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="txtWebsiteTitleForTitleTag"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_websiteTitle_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_websiteTitle_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="chkAllowHtml" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowHtmlInTitlesAndCaptions_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowHtmlInTitlesAndCaptions_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

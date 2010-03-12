<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="albums.aspx.cs" Inherits="GalleryServerPro.Web.admin.albums" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<h3>
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Thumbnail_Settings_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p>
				<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_TitleDisplayLength_Label %>" />
			</p>
			<p class="nomargin nopadding">
				<asp:TextBox ID="txtTitleDisplayLength" runat="server" /></p>
		</div>
		<h3>
			<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Empty_Album_Thumbnail_Settings_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p class="textcol">
				<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Empty_Album_Thumbnail_Settings_Dtl %>" />
			</p>
			<table class="standardTable">
				<tr>
					<td>
						<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Text_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtText" runat="server" /></td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_FontName_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtFontName" runat="server" /></td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_FontSize_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtFontSize" runat="server" /></td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_FontColor_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtFontColor" runat="server" />
						<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtFontColor"
							Display="None" ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>">
						</asp:RequiredFieldValidator>
						<cc1:ValidatorCalloutExtender runat="server" ID="rfv1E" TargetControlID="rfv1" HighlightCssClass="validatorCalloutHighlight" />
						<asp:CustomValidator ID="cvFontColor" runat="server" ControlToValidate="txtFontColor"
							Text="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							CssClass="msgwarning" OnServerValidate="cvColor_ServerValidate"></asp:CustomValidator>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_BackgroundColor_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtBackgroundColor" runat="server" />
						<asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtBackgroundColor"
							Display="None" ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>">
						</asp:RequiredFieldValidator>
						<cc1:ValidatorCalloutExtender runat="server" ID="rfv2E" TargetControlID="rfv2" HighlightCssClass="validatorCalloutHighlight" />
						<asp:CustomValidator ID="cvBackgroundColor" runat="server" ControlToValidate="txtBackgroundColor"
							Text="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							CssClass="msgwarning" OnServerValidate="cvColor_ServerValidate"></asp:CustomValidator></td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_AspectRatio_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtAspectRatio" runat="server" /></td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="MaxAlbumThumbnailTitleDisplayLength" ControlId="txtTitleDisplayLength"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_TitleDisplayLength_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailText" ControlId="txtText" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_Text_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailFontName" ControlId="txtFontName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_FontName_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailFontSize" ControlId="txtFontSize" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_FontSize_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailFontColor" ControlId="txtFontColor" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_FontColor_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailBackgroundColor" ControlId="txtBackgroundColor"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_BackgroundColor_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailWidthToHeightRatio" ControlId="txtAspectRatio"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_AspectRatio_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="txtTitleDisplayLength"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_maxAlbumThumbnailTitleDisplayLength_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_maxAlbumThumbnailTitleDisplayLength_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="txtText" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailText_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailText_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="txtFontName" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontName_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontName_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="txtFontSize" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontSize_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontSize_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="txtFontColor" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontColor_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontColor_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="txtBackgroundColor"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailBackgroundColor_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailBackgroundColor_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="txtAspectRatio"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailWidthToHeightRatio_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailWidthToHeightRatio_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="albums.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.admin.albums" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"	TagPrefix="tis" %>
<%@ Import Namespace="GalleryServerPro.Web" %>
<div id="d1" runat="server">

	<script type="text/javascript">

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(albumPageLoad);

		function albumPageLoad(sender, args)
		{
			configControls();
		}

		function chkEnablePaging_Click()
		{
			configControls();
		}

		function configControls()
		{
			var txtPageSize = $get('<%= txtPageSize.ClientID %>');
			var chkEnablePaging = $get('<%= chkEnablePaging.ClientID %>');
			var ddlPagerLocation = $get('<%= ddlPagerLocation.ClientID %>');

			txtPageSize.disabled = (!chkEnablePaging.checked || chkEnablePaging.disabled);
			ddlPagerLocation.disabled = txtPageSize.disabled;
		}

	</script>

</div>
<div class="gsp_indentedContent">
	<asp:PlaceHolder ID="phAdminHeader" runat="server" />
	<div class="gsp_addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<p class="admin_h3" style="margin-top: 0;">
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Thumbnail_Settings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p>
				<asp:CheckBox ID="chkEnablePaging" runat="server" onclick="chkEnablePaging_Click()"
					Text="<%$ Resources:GalleryServerPro, Admin_Albums_EnablePaging_Lbl %>" /></p>
			<table class="gsp_standardTable gsp_addleftmargin10">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblPageSize" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_PageSize_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtPageSize" runat="server" />&nbsp;<asp:RangeValidator ID="rvPageSize"
							runat="server" Display="Dynamic" ControlToValidate="txtPageSize" Type="Integer"
							MinimumValue="1" MaximumValue="2147483647" Text="<%$ Resources:GalleryServerPro, Validation_Positive_Int_Text %>" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblPagerLocation" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_PagerLocation_Label %>" />
					</td>
					<td>
						<asp:DropDownList ID="ddlPagerLocation" runat="server" />
					</td>
				</tr>
			</table>
			<p class="gsp_addtopmargin10">
				<asp:Label ID="lblTitleDisplayLength" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_TitleDisplayLength_Label %>" />&nbsp;<asp:TextBox
					ID="txtTitleDisplayLength" runat="server" />&nbsp;<asp:RangeValidator ID="rvTitleDisplayLength"
						runat="server" Display="Dynamic" ControlToValidate="txtTitleDisplayLength" Type="Integer"
						MinimumValue="1" MaximumValue="100" Text="<%$ Resources:GalleryServerPro, Validation_Int_1_To_100_Text %>" />
			</p>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Empty_Album_Thumbnail_Settings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p class="gsp_textcol">
				<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Empty_Album_Thumbnail_Settings_Dtl %>" />
			</p>
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblText" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_Text_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtText" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblFontName" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_FontName_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtFontName" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblFontSize" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_FontSize_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtFontSize" runat="server" />&nbsp;<asp:RangeValidator ID="rvFontSize"
							runat="server" Display="Dynamic" ControlToValidate="txtFontSize" Type="Integer"
							MinimumValue="6" MaximumValue="100" Text="<%$ Resources:GalleryServerPro, Validation_Font_Size_Text %>" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblFontColor" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_FontColor_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtFontColor" runat="server" />
						<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtFontColor"
							Display="Static" ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							ForeColor="" CssClass="gsp_msgfailure">
						</asp:RequiredFieldValidator>
						<asp:CustomValidator ID="cvFontColor" runat="server" ControlToValidate="txtFontColor"
							Text="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							CssClass="gsp_msgwarning" OnServerValidate="cvColor_ServerValidate"></asp:CustomValidator>
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblBackgroundColor" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_BackgroundColor_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtBackgroundColor" runat="server" />
						<asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtBackgroundColor"
							Display="Static" ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							ForeColor="" CssClass="gsp_msgfailure">
						</asp:RequiredFieldValidator>
						<asp:CustomValidator ID="cvBackgroundColor" runat="server" ControlToValidate="txtBackgroundColor"
							Text="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Albums_General_Invalid_Color_Text %>"
							CssClass="gsp_msgwarning" OnServerValidate="cvColor_ServerValidate"></asp:CustomValidator>
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblAspectRatio" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Albums_AspectRatio_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtAspectRatio" runat="server" />
					</td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server" OnAfterBindControl="wwDataBinder_AfterBindControl"
		OnBeforeUnbindControl="wwDataBinder_BeforeUnbindControl" OnValidateControl="wwDataBinder_ValidateControl">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="bi1" runat="server" BindingSource="CoreConfig" BindingSourceMember="PageSize"
				ControlId="txtPageSize" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_PageSize_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem14" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="PagerLocation" ControlId="ddlPagerLocation" BindingProperty="SelectedValue"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_PagerLocation_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="MaxAlbumThumbnailTitleDisplayLength" ControlId="txtTitleDisplayLength"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_TitleDisplayLength_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailText" ControlId="txtText" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_Text_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailFontName" ControlId="txtFontName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_FontName_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailFontSize" ControlId="txtFontSize" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_FontSize_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailFontColor" ControlId="txtFontColor" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_FontColor_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailBackgroundColor" ControlId="txtBackgroundColor"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_BackgroundColor_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmptyAlbumThumbnailWidthToHeightRatio" ControlId="txtAspectRatio"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Albums_AspectRatio_Label %>" />
			<tis:wwDataBindingItem runat="server" ControlId="chkEnablePaging">
			</tis:wwDataBindingItem>
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="chkEnablePaging"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_enabledPaging_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_enabledPaging_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem1b" runat="server" ControlId="lblPageSize" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_pageSize_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_pageSize_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem1c" runat="server" ControlId="lblPagerLocation"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_pagerLocation_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_pagerLocation_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem1d" runat="server" ControlId="lblTitleDisplayLength"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_maxAlbumThumbnailTitleDisplayLength_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_maxAlbumThumbnailTitleDisplayLength_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="lblText" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailText_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailText_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="lblFontName" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontName_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontName_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="lblFontSize" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontSize_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontSize_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="lblFontColor" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontColor_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailFontColor_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="lblBackgroundColor"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailBackgroundColor_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailBackgroundColor_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="lblAspectRatio"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailWidthToHeightRatio_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAlbumThumbnailWidthToHeightRatio_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:PlaceHolder ID="phAdminFooter" runat="server" />
</div>

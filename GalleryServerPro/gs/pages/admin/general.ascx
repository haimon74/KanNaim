<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="general.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.admin.general" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<div id="css" runat="server" visible="false">
	<link href="../../styles/gallery.css" rel="stylesheet" type="text/css" />
</div>
<div runat="server">

	<script type="text/javascript">

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(generalPageLoad);

		function generalPageLoad(sender, args)
		{
			updateDisplayDetailErrorCheckbox();
			updateAllowAnonymousHiResBrowsingCheckbox();
		}

		function updateDisplayDetailErrorCheckbox()
		{
			var chkEnableExceptionHandler = $get('<%= chkEnableExceptionHandler.ClientID %>');
			$get('<%= chkShowErrorDetails.ClientID %>').disabled = (!chkEnableExceptionHandler.checked || chkEnableExceptionHandler.disabled);
		}

		function updateAllowAnonymousHiResBrowsingCheckbox()
		{
			var chkAllowAnonymousBrowsing = $get('<%= chkAllowAnonymousBrowsing.ClientID %>');
			$get('<%= chkAllowAnonymousHiResBrowsing.ClientID %>').disabled = (!chkAllowAnonymousBrowsing.checked || chkAllowAnonymousBrowsing.disabled);
		}

	</script>

</div>
<div class="gsp_indentedContent">
	<asp:PlaceHolder ID="phAdminHeader" runat="server" />
	<div class="gsp_addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<div id="verContainer" class="gsp_rounded10">
			<p class="verHdr">
				<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_GSP_Hdr %>" />
				<asp:Label ID="lblVersion" runat="server" CssClass="gsp_msgfriendly" /></p>
			<p class="gsp_nopadding">
				<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_GSP_Home_Page %>" />
			</p>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ProductKey_Hdr %>" /></p>
		<div class="gsp_addleftpadding6">
			<p class="gsp_addtopmargin5">
				<asp:Label ID="lblProductKey" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ProductKey_Label %>" />
				<asp:TextBox ID="txtProductKey" runat="server" />&nbsp;<asp:Button ID="btnEnterProductKey"
					runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ProductKey_Btn_Lbl %>"
					OnClick="btnEnterProductKey_Click" />
			</p>
			<p class="gsp_nopadding">
				<asp:Image ID="imgProductKeyValidation" runat="server" Width="16" Height="16" Visible="false" />&nbsp;<asp:Label
					ID="lblProductKeyValidationMsg" runat="server" />
			</p>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_SystemSettings_Hdr %>" /></p>
		<div class="gsp_addleftpadding6">
			<p class="gsp_addtopmargin5">
				<asp:Label ID="lblWebsiteTitle" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitle_Label %>" />
			</p>
			<p class="gsp_collapse">
				<asp:TextBox ID="txtWebsiteTitle" runat="server" CssClass="gsp_textbox" /></p>
			<p class="gsp_addtopmargin5">
				<asp:Label ID="lblWebsiteTitleUrl" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitleUrl_Label %>" />
			</p>
			<p class="gsp_collapse">
				<asp:TextBox ID="txtWebsiteTitleUrl" runat="server" CssClass="gsp_textbox" /></p>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkAllowAnonymousBrowsing" runat="server" onclick="updateAllowAnonymousHiResBrowsingCheckbox()"
					Text="<%$ Resources:GalleryServerPro, Admin_Default_AllowAnonymousBrowsing_Label %>" />
			</p>
			<p class="gsp_addleftmargin5">
				<asp:CheckBox ID="chkAllowAnonymousHiResBrowsing" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_AllowAnonymousHiResBrowsing_Label %>" />
			</p>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkShowLogin" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ShowLogin_Label %>" />
			</p>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkShowSearch" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ShowSearch_Label %>" />
			</p>
			<p class="gsp_addtopmargin5">
				<asp:Label ID="lblGalleryId" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_GalleryId_Label %>" />
				<asp:TextBox ID="txtGalleryId" runat="server" />
			</p>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ErrorHandling_Hdr %>" /></p>
		<div class="gsp_addleftpadding6">
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkSendEmail" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_SendEmail_Label %>" />
			</p>
			<p class="gsp_addleftmargin10">
				<asp:Label ID="lblEmailAttn" runat="server" CssClass="gsp_msgattention" />
			</p>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkEnableExceptionHandler" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_EnableExceptionHandler_Label %>" />
			</p>
			<p class="gsp_addleftmargin5">
				<asp:CheckBox ID="chkShowErrorDetails" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_ShowErrorDetails_Label %>" />
			</p>
			<p>
				<asp:Button ID="btnThrowError" runat="server" Text="Generate sample error" OnClick="btnThrowError_Click" /></p>
		</div>
		<p class="admin_h3">
			<asp:Label ID="lblProviderLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Hdr %>" /></p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_addleftpadding6 gsp_standardTable">
				<tr>
					<td colspan="2">
						<p class="gsp_bold">
							<asp:Label ID="lblGalleryDataLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_GalleryData_Hdr %>" /></p>
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblGalleryDataProviderLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Description_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblGalleryDataProvider" runat="server" CssClass="gsp_msgfriendly" />
						-
						<asp:Label ID="lblGalleryDataProviderDesc" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblGalleryDataAppNameLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_AppName_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblGalleryDataAppName" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<p class="gsp_bold gsp_addtopmargin5">
							<asp:Label ID="lblMembershipLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Membership_Hdr %>" /></p>
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblMembershipDataProviderLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Description_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblMembershipDataProvider" runat="server" CssClass="gsp_msgfriendly" />
						-
						<asp:Label ID="lblMembershipDataProviderDesc" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblMembershipAppNameLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_AppName_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblMembershipAppName" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<p class="gsp_bold gsp_addtopmargin5">
							<asp:Label ID="lblRoleLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Roles_Hdr %>" /></p>
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblRoleDataProviderLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Description_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblRoleDataProvider" runat="server" CssClass="gsp_msgfriendly" />
						-
						<asp:Label ID="lblRoleDataProviderDesc" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblRoleAppNameLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_AppName_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblRoleAppName" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<p class="gsp_bold gsp_addtopmargin5">
							<asp:Label ID="lblProfileLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Profile_Hdr %>" /></p>
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblProfileDataProviderLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Description_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblProfileDataProvider" runat="server" CssClass="gsp_msgfriendly" />
						-
						<asp:Label ID="lblProfileDataProviderDesc" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblProfileAppNameLabel" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_AppName_Label %>" />
					</td>
					<td>
						<asp:Label ID="lblProfileAppName" runat="server" CssClass="gsp_msgfriendly" />
					</td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="wbi1" runat="server" BindingSource="CoreConfig" BindingSourceMember="GalleryId"
				ControlId="txtGalleryId" IsRequired="True" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_GalleryId_Label %>" />
			<tis:wwDataBindingItem ID="wbi2" runat="server" ControlId="txtWebsiteTitle" BindingSource="CoreConfig"
				BindingSourceMember="PageHeaderText" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitle_Label %>"
				IsRequired="False" />
			<tis:wwDataBindingItem ID="wbi3" runat="server" ControlId="txtWebsiteTitleUrl" BindingSource="CoreConfig"
				BindingSourceMember="PageHeaderTextUrl" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_WebsiteTitleUrl_Label %>"
				IsRequired="False" />
			<tis:wwDataBindingItem ID="wbi4" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowAnonymousBrowsing"
				ControlId="chkAllowAnonymousBrowsing" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_AllowAnonymousBrowsing_Label %>" />
			<tis:wwDataBindingItem ID="wbi5" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowAnonymousHiResViewing"
				ControlId="chkAllowAnonymousHiResBrowsing" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_AllowAnonymousBrowsing_Label %>" />
			<tis:wwDataBindingItem ID="wbi6" runat="server" BindingSource="CoreConfig" BindingSourceMember="ShowLogin"
				ControlId="chkShowLogin" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_ShowLogin_Label %>" />
			<tis:wwDataBindingItem ID="wbi7" runat="server" BindingSource="CoreConfig" BindingSourceMember="ShowSearch"
				ControlId="chkShowSearch" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_ShowSearch_Label %>" />
			<tis:wwDataBindingItem ID="wbi10" runat="server" BindingSource="CoreConfig" BindingSourceMember="SendEmailOnError"
				ControlId="chkSendEmail" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_SendEmail_Label %>" />
			<tis:wwDataBindingItem ID="wbi11" runat="server" BindingSource="CoreConfig" BindingSourceMember="EnableExceptionHandler"
				ControlId="chkEnableExceptionHandler" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_EnableExceptionHandler_Label %>" />
			<tis:wwDataBindingItem ID="wbi12" runat="server" BindingSource="CoreConfig" BindingSourceMember="ShowErrorDetails"
				ControlId="chkShowErrorDetails" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Default_ShowErrorDetails_Label %>" />
			<tis:wwDataBindingItem ID="wbi13" runat="server" BindingSource="GalleryDataProvider"
				BindingSourceMember="Name" ControlId="lblGalleryDataProvider" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi14" runat="server" BindingSource="GalleryDataProvider"
				BindingSourceMember="Description" ControlId="lblGalleryDataProviderDesc" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi15" runat="server" BindingSource="GalleryDataProvider"
				BindingSourceMember="ApplicationName" ControlId="lblGalleryDataAppName" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi16" runat="server" BindingSource="MembershipGsp"
				BindingSourceMember="Name" ControlId="lblMembershipDataProvider" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi17" runat="server" BindingSource="MembershipGsp"
				BindingSourceMember="Description" ControlId="lblMembershipDataProviderDesc" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi18" runat="server" BindingSource="MembershipGsp"
				BindingSourceMember="ApplicationName" ControlId="lblMembershipAppName" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi19" runat="server" BindingSource="RoleGsp" BindingSourceMember="Name"
				ControlId="lblRoleDataProvider" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi20" runat="server" BindingSource="RoleGsp" BindingSourceMember="Description"
				ControlId="lblRoleDataProviderDesc" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi21" runat="server" BindingSource="RoleGsp" BindingSourceMember="ApplicationName"
				ControlId="lblRoleAppName" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi22" runat="server" BindingSource="ProfileGsp"
				BindingSourceMember="Name" ControlId="lblProfileDataProvider" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi23" runat="server" BindingSource="ProfileGsp"
				BindingSourceMember="Description" ControlId="lblProfileDataProviderDesc" BindingMode="OneWay" />
			<tis:wwDataBindingItem ID="wbi24" runat="server" BindingSource="ProfileGsp"
				BindingSourceMember="ApplicationName" ControlId="lblProfileAppName" BindingMode="OneWay" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="poi1" runat="server" ControlId="lblProductKey" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_productKey_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_productKey_Bdy %>" />
			<tis:PopupInfoItem ID="poi2" runat="server" ControlId="lblGalleryId" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_galleryId_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_galleryId_Bdy %>" />
			<tis:PopupInfoItem ID="poi3" runat="server" ControlId="lblWebsiteTitle" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_pageHeaderText_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_pageHeaderText_Bdy %>" />
			<tis:PopupInfoItem ID="poi4" runat="server" ControlId="lblWebsiteTitleUrl" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_pageHeaderTextUrl_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_pageHeaderTextUrl_Bdy %>" />
			<tis:PopupInfoItem ID="poi5" runat="server" ControlId="chkAllowAnonymousBrowsing"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowAnonymousBrowsing_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowAnonymousBrowsing_Bdy %>" />
			<tis:PopupInfoItem ID="poi6" runat="server" ControlId="chkAllowAnonymousHiResBrowsing"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowAnonymousHiResBrowsing_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowAnonymousHiResBrowsing_Bdy %>" />
			<tis:PopupInfoItem ID="poi7" runat="server" ControlId="chkShowLogin" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_showLogin_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_showLogin_Bdy %>" />
			<tis:PopupInfoItem ID="poi8" runat="server" ControlId="chkShowSearch" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_showSearch_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_showSearch_Bdy %>" />
			<tis:PopupInfoItem ID="poi11" runat="server" ControlId="chkSendEmail" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_sendEmailOnError_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_sendEmailOnError_Bdy %>" />
			<tis:PopupInfoItem ID="poi12" runat="server" ControlId="chkEnableExceptionHandler"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_enableExceptionHandler_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_enableExceptionHandler_Bdy %>" />
			<tis:PopupInfoItem ID="poi13" runat="server" ControlId="chkShowErrorDetails" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_showErrorDetails_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_showErrorDetails_Bdy %>" />
			<tis:PopupInfoItem ID="poi14" runat="server" ControlId="lblGalleryDataLabel" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_GalleryData_Popup_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_GalleryData_Popup_Bdy %>" />
			<tis:PopupInfoItem ID="poi15" runat="server" ControlId="lblMembershipLabel" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Membership_Popup_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Membership_Popup_Bdy %>" />
			<tis:PopupInfoItem ID="poi16" runat="server" ControlId="lblRoleLabel" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Role_Popup_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Role_Popup_Bdy %>" />
			<tis:PopupInfoItem ID="poi17" runat="server" ControlId="lblProfileLabel" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Profile_Popup_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Profile_Popup_Bdy %>" />
			<tis:PopupInfoItem ID="poi18" runat="server" ControlId="lblProviderLabel" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Popup_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Default_DataProvider_Popup_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:PlaceHolder ID="phAdminFooter" runat="server" />
</div>

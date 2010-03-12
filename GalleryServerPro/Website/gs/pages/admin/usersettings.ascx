<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="usersettings.ascx.cs"
	Inherits="GalleryServerPro.Web.gs.pages.admin.usersettings" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<%@ Register Src="../../controls/albumtreeview.ascx" TagName="albumtreeview" TagPrefix="uc1" %>
<%@ Register Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Import Namespace="GalleryServerPro.Web" %>
<div id="css" runat="server" visible="false">
	<link href="../../styles/gallery.css" rel="stylesheet" type="text/css" />
</div>
<div id="d1" runat="server">

	<script type="text/javascript">

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(gsp_userSettingsPageLoad);

		function gsp_userSettingsPageLoad(sender, args)
		{
			gsp_configControlsPageLoad();

			if (!$get('<%= chkShowAllUsers.ClientID %>').checked)
				hideNonAdminUsers();

			if (!$get('<%= chkShowOwnerRoles.ClientID %>').checked)
				hideOwnerRoles();
		}

		function chkEnableSelfRegistration_Click()
		{
			gsp_configControls();
		}

		function chkEnableUserAlbums_Click()
		{
			gsp_configControls();
		}

		function chkAllowHtml_Click()
		{
			gsp_configControls();
		}

		function chkAllowManageAccount_Click()
		{
			gsp_configControls();
		}

		function gsp_configControlsPageLoad()
		{
			// If the combobox objects aren't ready, wait 1/10 second and check again. When ready call gsp_configControls().
			if ((typeof (cboUserRoles) === 'undefined') || (typeof (cboUserAlbumParent) === 'undefined'))
			{
				setTimeout('gsp_configControlsPageLoad()', 100);
			}
			else
				gsp_configControls();
		}

		function gsp_configControls()
		{
			// Self registration section
			var chkEnableSelfRegistration = $get('<%= chkEnableSelfRegistration.ClientID %>');
			var chkRequireEmailValidation = $get('<%= chkRequireEmailValidation.ClientID %>');
			var chkRequireAdminApproval = $get('<%= chkRequireAdminApproval.ClientID %>');
			var chkUseEmailForAccountName = $get('<%= chkUseEmailForAccountName.ClientID %>');

			chkRequireEmailValidation.disabled = (!chkEnableSelfRegistration.checked || chkEnableSelfRegistration.disabled);
			chkUseEmailForAccountName.disabled = chkRequireEmailValidation.disabled;
			chkRequireAdminApproval.disabled = chkRequireEmailValidation.disabled;
			if (chkRequireEmailValidation.disabled)
				cboUserRoles.disable();
			else
				cboUserRoles.enable();

			// User album section
			var chkEnableUserAlbums = $get('<%= chkEnableUserAlbums.ClientID %>');
			var chkRedirectAfterLogin = $get('<%= chkRedirectAfterLogin.ClientID %>');
			var txtAlbumNameTemplate = $get('<%= txtAlbumNameTemplate.ClientID %>');
			var txtAlbumSummaryTemplate = $get('<%= txtAlbumSummaryTemplate.ClientID %>');

			txtAlbumNameTemplate.disabled = (!chkEnableUserAlbums.checked || chkEnableUserAlbums.disabled);
			txtAlbumSummaryTemplate.disabled = txtAlbumNameTemplate.disabled;
			chkRedirectAfterLogin.disabled = txtAlbumNameTemplate.disabled;
			if (chkRedirectAfterLogin.disabled)
				cboUserAlbumParent.disable();
			else
				cboUserAlbumParent.enable();

			// User accounts section
			var chkAllowHtml = $get('<%= chkAllowHtml.ClientID %>');
			var txtAllowedHtmlTags = $get('<%= txtAllowedHtmlTags.ClientID %>');
			var txtAllowedHtmlAttributes = $get('<%= txtAllowedHtmlAttributes.ClientID %>');
			var chkAllowManageAccount = $get('<%= chkAllowManageAccount.ClientID %>');
			var chkAllowDeleteOwnAccount = $get('<%= chkAllowDeleteOwnAccount.ClientID %>');

			txtAllowedHtmlTags.disabled = (!chkAllowHtml.checked || chkAllowHtml.disabled);
			txtAllowedHtmlAttributes.disabled = txtAllowedHtmlTags.disabled;
			chkAllowDeleteOwnAccount.disabled = (!chkAllowManageAccount.checked || chkAllowManageAccount.disabled);
		}

		function gsp_handleUsersToNotifyComboBoxSelection(sender, e)
		{
			var userList = "";
			$('table#<%= cblU.ClientID %> input:checkbox:checked').next().each(function() { userList = userList + this.innerHTML + ', '; });
			userList = userList.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
			cboUsersToNotify.set_text(userList.slice(0, -2));
			cboUsersToNotify.collapse();
		}

		function gsp_handleRoleComboBoxSelection(sender, e)
		{
			var roleList = "";
			$('table#<%= cblR.ClientID %> input:checkbox:checked').next().each(function() { roleList = roleList + this.innerHTML + ', '; });
			roleList = roleList.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
			cboUserRoles.set_text(roleList.slice(0, -2));
			cboUserRoles.collapse();
		}

		function gsp_handleAlbumComboBoxSelection(sender, e)
		{
			cboUserAlbumParent.set_text(e.get_node().get_text());
			cboUserAlbumParent.collapse();
		}

		function hideOwnerRoles()
		{
			$("table.gsp_j_rolelist tr:has(td > span.gsp_j_albumownerrole)").hide();
		}

		function hideNonAdminUsers()
		{
			$("table.gsp_j_userlist tr:has(td > span.gsp_j_notadmin)").hide();
		}

		function toggleOwnerRoles(chk)
		{
			if (chk.checked)
				$("table.gsp_j_rolelist tr:has(td > span.gsp_j_albumownerrole)").fadeIn();
			else
				$("table.gsp_j_rolelist tr:has(td > span.gsp_j_albumownerrole)").fadeOut();
		}

		function toggleAdminUsers(chk)
		{
			if (chk.checked)
				$("table.gsp_j_userlist tr:has(td > span.gsp_j_notadmin)").fadeIn();
			else
				$("table.gsp_j_userlist tr:has(td > span.gsp_j_notadmin)").fadeOut();
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
			<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Self_Registration_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p>
				<asp:CheckBox ID="chkEnableSelfRegistration" runat="server" onclick="chkEnableSelfRegistration_Click()"
					Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_EnableSelfRegistration_Lbl %>" /></p>
			<div class="gsp_addleftmargin10">
				<p>
					<asp:CheckBox ID="chkRequireEmailValidation" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_RequireEmailValidation_Lbl %>" />
				</p>
				<p>
					<asp:CheckBox ID="chkRequireAdminApproval" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_RequireAdminApproval_Lbl %>" />
				</p>
				<p>
					<asp:CheckBox ID="chkUseEmailForAccountName" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_UseEmailForAccountName_Lbl %>" />
				</p>
				<p class="gsp_addtopmargin10">
					<asp:Label ID="lblUserRoles" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Default_Roles_Lbl %>" />
				</p>
				<ComponentArt:ComboBox ID="cboUserRoles" runat="Server" Width="575" Height="20" DropDownWidth="575"
					DropDownHeight="300" AutoHighlight="true" AutoComplete="true" AutoFilter="false"
					ItemCssClass="gsp_ddn-item" ItemHoverCssClass="gsp_ddn-item-hover" CssClass="gsp_cmb"
					TextBoxCssClass="gsp_txt" DropDownResizingMode="Corner" DropDownCssClass="gsp_ns gsp_ddn"
					DropDownContentCssClass="gsp_ddn-con" SelectedItemCssClass="gsp_ddn-item-hover"
					KeyboardEnabled="true" DropDownOffsetY="2">
					<DropDownHeader>
						<p>
							<asp:CheckBox ID="chkShowOwnerRoles" runat="server" CssClass="gsp_fs" onclick="toggleOwnerRoles(this);"
								Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Show_Album_Owner_Roles_Lbl %>" />
						</p>
						<p class="gsp_addleftpadding2">
							<a href="javascript:gsp_handleRoleComboBoxSelection();">
								<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Choose_Items_Lbl %>" /></a></p>
					</DropDownHeader>
					<DropDownContent>
						<asp:CheckBoxList ID="cblR" runat="server" SelectionMode="Multiple" CssClass="gsp_j_rolelist"
							EnableViewState="false" />
					</DropDownContent>
					<DropDownFooter>
						<div style="text-align: right;">
							<img src="<%= Util.GetUrl("/images/componentart/combobox/bottom_right.gif") %>" alt="" /></div>
					</DropDownFooter>
				</ComponentArt:ComboBox>
			</div>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_User_Albums_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p>
				<asp:CheckBox ID="chkEnableUserAlbums" runat="server" onclick="chkEnableUserAlbums_Click()"
					Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_EnableUserAlbums_Lbl %>" /></p>
			<div class="gsp_addleftmargin10">
				<p>
					<asp:CheckBox ID="chkRedirectAfterLogin" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_RedirectAfterLogin_Lbl %>" />
				</p>
				<p>
					<asp:Label ID="lblAlbumNameTemplate" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_User_Album_Name_Template_Label %>" />
					<asp:TextBox ID="txtAlbumNameTemplate" runat="server" CssClass="gsp_textbox" />
				</p>
				<p class="gsp_addtopmargin10">
					<asp:Label ID="lblAlbumSummaryTemplate" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_User_Album_Summary_Template_Label %>" />
				</p>
				<p>
					<asp:TextBox ID="txtAlbumSummaryTemplate" runat="server" CssClass="gsp_textarea3"
						TextMode="MultiLine" />
				</p>
				<p class="gsp_addtopmargin10">
					<asp:Label ID="lblUserAlbumParent" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_User_Album_Parent_Album_Lbl %>" /></p>
				<ComponentArt:ComboBox ID="cboUserAlbumParent" runat="Server" Width="575" Height="20"
					DropDownWidth="575" DropDownHeight="300" AutoHighlight="true" AutoComplete="true"
					AutoFilter="false" ItemCssClass="gsp_ddn-item" ItemHoverCssClass="gsp_ddn-item-hover"
					CssClass="gsp_cmb" TextBoxCssClass="gsp_txt" DropDownResizingMode="Corner" DropDownCssClass="gsp_ns gsp_ddn"
					DropDownContentCssClass="gsp_ddn-con" SelectedItemCssClass="gsp_ddn-item-hover"
					KeyboardEnabled="true" DropDownOffsetY="2">
					<DropDownContent>
						<uc1:albumtreeview ID="tvUC" runat="server" AllowMultiSelect="false" ClientOnTreeNodeSelectJavascriptFunctionName="gsp_handleAlbumComboBoxSelection"
							RequiredSecurityPermissions="AdministerSite" />
					</DropDownContent>
					<DropDownFooter>
						<div style="text-align: right;">
							<img src="<%= Util.GetUrl("/images/componentart/combobox/bottom_right.gif") %>" alt="" /></div>
					</DropDownFooter>
				</ComponentArt:ComboBox>
			</div>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Accounts_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p>
				<asp:Label ID="lblUsersToNotify" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Users_To_Notify_For_New_Accounts_Lbl %>" />
			</p>
			<ComponentArt:ComboBox ID="cboUsersToNotify" runat="Server" Width="575" Height="20"
				DropDownWidth="575" DropDownHeight="300" AutoHighlight="true" AutoComplete="true"
				AutoFilter="false" ItemCssClass="gsp_ddn-item" ItemHoverCssClass="gsp_ddn-item-hover"
				CssClass="gsp_cmb" TextBoxCssClass="gsp_txt" DropDownResizingMode="Corner" DropDownCssClass="gsp_ns gsp_ddn"
				DropDownContentCssClass="gsp_ddn-con" SelectedItemCssClass="gsp_ddn-item-hover"
				KeyboardEnabled="true" DropDownOffsetY="2">
				<DropDownHeader>
					<p>
						<asp:CheckBox ID="chkShowAllUsers" runat="server" CssClass="gsp_fs" onclick="toggleAdminUsers(this);"
							Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Show_All_Users_Lbl %>" />
					</p>
					<p class="gsp_addleftpadding2">
						<a href="javascript:gsp_handleUsersToNotifyComboBoxSelection();">
							<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Choose_Items_Lbl %>" /></a></p>
				</DropDownHeader>
				<DropDownContent>
					<asp:CheckBoxList ID="cblU" runat="server" SelectionMode="Multiple" CssClass="gsp_j_userlist"
						EnableViewState="false" />
				</DropDownContent>
				<DropDownFooter>
					<div style="text-align: right;">
						<img src="<%= Util.GetUrl("/images/componentart/combobox/bottom_right.gif") %>" alt="" /></div>
				</DropDownFooter>
			</ComponentArt:ComboBox>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkAllowHtml" runat="server" onclick="chkAllowHtml_Click()" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowHtml_Label %>" />
			</p>
			<div class="gsp_addleftmargin10">
				<p class="gsp_addtopmargin5">
					<asp:Label ID="lblAllowedHtmlTags" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Allowed_HTML_Tags_Label %>" />
				</p>
				<p>
					<asp:TextBox ID="txtAllowedHtmlTags" runat="server" CssClass="gsp_textarea3" TextMode="MultiLine" />
				</p>
				<p class="gsp_addtopmargin5">
					<asp:Label ID="lblAllowedHtmlAttributes" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_Allowed_HTML_Attributes_Label %>" />
				</p>
				<p>
					<asp:TextBox ID="txtAllowedHtmlAttributes" runat="server" CssClass="gsp_textarea3"
						TextMode="MultiLine" />
				</p>
			</div>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkAllowJavascript" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowJavascript_Label %>" />
			</p>
			<p class="gsp_addtopmargin5">
				<asp:CheckBox ID="chkAllowCopyingReadOnlyObjects" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowCopyingReadOnlyObjects_Label %>" />
			</p>
			<p>
				<asp:CheckBox ID="chkAllowManageAccount" runat="server" onclick="chkAllowManageAccount_Click()"
					Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowManageAccount_Lbl %>" /></p>
			<div class="gsp_addleftmargin10">
				<p>
					<asp:CheckBox ID="chkAllowDeleteOwnAccount" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowDeleteOwnAccount_Label %>" />
				</p>
			</div>
		</div>
		<tis:wwDataBinder ID="wwDataBinder" runat="server" OnAfterBindControl="wwDataBinder_AfterBindControl"
			OnBeforeUnbindControl="wwDataBinder_BeforeUnbindControl" OnValidateControl="wwDataBinder_ValidateControl">
			<DataBindingItems>
				<tis:wwDataBindingItem ID="wbi1" runat="server" BindingSource="CoreConfig" BindingSourceMember="EnableSelfRegistration"
					ControlId="chkEnableSelfRegistration" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_EnableSelfRegistration_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi2" runat="server" BindingSource="CoreConfig" BindingSourceMember="RequireEmailValidationForSelfRegisteredUser"
					ControlId="chkRequireEmailValidation" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_RequireEmailValidation_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi3" runat="server" BindingSource="CoreConfig" BindingSourceMember="RequireApprovalForSelfRegisteredUser"
					ControlId="chkRequireAdminApproval" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_RequireAdminApproval_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi4" runat="server" BindingSource="CoreConfig" BindingSourceMember="UseEmailForAccountName"
					ControlId="chkUseEmailForAccountName" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_UseEmailForAccountName_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi5" runat="server" BindingSource="CoreConfig" BindingSourceMember="DefaultRolesForSelfRegisteredUser"
					ControlId="cboUserRoles" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_Default_Roles_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi6" runat="server" BindingSource="CoreConfig" BindingSourceMember="EnableUserAlbum"
					ControlId="chkEnableUserAlbums" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_EnableUserAlbums_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi7" runat="server" BindingSource="CoreConfig" BindingSourceMember="UserAlbumNameTemplate"
					ControlId="txtAlbumNameTemplate" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_User_Album_Name_Template_Label %>" />
				<tis:wwDataBindingItem ID="wbi8" runat="server" BindingSource="CoreConfig" BindingSourceMember="UserAlbumSummaryTemplate"
					ControlId="txtAlbumSummaryTemplate" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_User_Album_Summary_Template_Label %>" />
				<tis:wwDataBindingItem ID="wbi9" runat="server" BindingSource="CoreConfig" BindingSourceMember="RedirectToUserAlbumAfterLogin"
					ControlId="chkRedirectAfterLogin" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_RedirectAfterLogin_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi10" runat="server" ControlId="cboUserAlbumParent" />
				<tis:wwDataBindingItem ID="wbi11" runat="server" BindingSource="CoreConfig" BindingSourceMember="UsersToNotifyWhenAccountIsCreated"
					ControlId="cboUsersToNotify" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_Users_To_Notify_For_New_Accounts_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi12" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowHtmlInTitlesAndCaptions"
					ControlId="chkAllowHtml" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowHtml_Label %>" />
				<tis:wwDataBindingItem ID="wbi13" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowedHtmlTags"
					ControlId="txtAllowedHtmlTags" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_Allowed_HTML_Tags_Label %>" />
				<tis:wwDataBindingItem ID="wbi14" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowedHtmlAttributes"
					ControlId="txtAllowedHtmlAttributes" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_Allowed_HTML_Attributes_Label %>" />
				<tis:wwDataBindingItem ID="wbi15" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowUserEnteredJavascript"
					ControlId="chkAllowJavascript" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowJavascript_Label %>" />
				<tis:wwDataBindingItem ID="wbi16" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowCopyingReadOnlyObjects"
					ControlId="chkAllowCopyingReadOnlyObjects" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowCopyingReadOnlyObjects_Label %>" />
				<tis:wwDataBindingItem ID="wbi17" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowManageOwnAccount"
					ControlId="chkAllowManageAccount" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowManageAccount_Lbl %>" />
				<tis:wwDataBindingItem ID="wbi18" runat="server" BindingSource="CoreConfig" BindingSourceMember="AllowDeleteOwnAccount"
					ControlId="chkAllowDeleteOwnAccount" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_User_Settings_AllowDeleteOwnAccount_Label %>" />
			</DataBindingItems>
		</tis:wwDataBinder>
		<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
			DefaultDialogBodyCss="dg5ContentBodyCss">
			<PopupInfoItems>
				<tis:PopupInfoItem ID="poi1" runat="server" ControlId="chkEnableSelfRegistration"
					DialogTitle="<%$ Resources:GalleryServerPro, Cfg_enableSelfRegistration_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_enableSelfRegistration_Bdy %>" />
				<tis:PopupInfoItem ID="poi2" runat="server" ControlId="chkRequireAdminApproval" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_requireApprovalForSelfRegisteredUser_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_requireApprovalForSelfRegisteredUser_Bdy %>" />
				<tis:PopupInfoItem ID="poi3" runat="server" ControlId="chkRequireEmailValidation"
					DialogTitle="<%$ Resources:GalleryServerPro, Cfg_requireEmailValidationForSelfRegisteredUser_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_requireEmailValidationForSelfRegisteredUser_Bdy %>" />
				<tis:PopupInfoItem ID="poi4" runat="server" ControlId="chkUseEmailForAccountName"
					DialogTitle="<%$ Resources:GalleryServerPro, Cfg_useEmailForAccountName_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_useEmailForAccountName_Bdy %>" />
				<tis:PopupInfoItem ID="poi5" runat="server" ControlId="lblUserRoles" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultRolesForSelfRegisteredUser_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultRolesForSelfRegisteredUser_Bdy %>" />
				<tis:PopupInfoItem ID="poi6" runat="server" ControlId="chkEnableUserAlbums" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_enableUserAlbum_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_enableUserAlbum_Bdy %>" />
				<tis:PopupInfoItem ID="poi7" runat="server" ControlId="lblAlbumNameTemplate" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_userAlbumNameTemplate_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_userAlbumNameTemplate_Bdy %>" />
				<tis:PopupInfoItem ID="poi8" runat="server" ControlId="lblAlbumSummaryTemplate" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_userAlbumSummaryTemplate_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_userAlbumSummaryTemplate_Bdy %>" />
				<tis:PopupInfoItem ID="poi9" runat="server" ControlId="chkRedirectAfterLogin" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_redirectToUserAlbumAfterLogin_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_redirectToUserAlbumAfterLogin_Bdy %>" />
				<tis:PopupInfoItem ID="poi10" runat="server" ControlId="lblUserAlbumParent" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_userAlbumParentAlbumId_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_userAlbumParentAlbumId_Bdy %>" />
				<tis:PopupInfoItem ID="poi11" runat="server" ControlId="lblUsersToNotify" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_usersToNotifyWhenAccountIsCreated_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_usersToNotifyWhenAccountIsCreated_Bdy %>" />
				<tis:PopupInfoItem ID="poi12" runat="server" ControlId="chkAllowHtml" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowHtmlInTitlesAndCaptions_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowHtmlInTitlesAndCaptions_Bdy %>" />
				<tis:PopupInfoItem ID="poi13" runat="server" ControlId="lblAllowedHtmlTags" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowedHtmlTags_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowedHtmlTags_Bdy %>" />
				<tis:PopupInfoItem ID="poi14" runat="server" ControlId="lblAllowedHtmlAttributes"
					DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowedHtmlAttributes_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowedHtmlAttributes_Bdy %>" />
				<tis:PopupInfoItem ID="poi15" runat="server" ControlId="chkAllowJavascript" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowUserEnteredJavascript_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowUserEnteredJavascript_Bdy %>" />
				<tis:PopupInfoItem ID="poi16" runat="server" ControlId="chkAllowCopyingReadOnlyObjects"
					DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowCopyingReadOnlyObjects_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowCopyingReadOnlyObjects_Bdy %>" />
				<tis:PopupInfoItem ID="poi17" runat="server" ControlId="chkAllowManageAccount" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowManageOwnAccount_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowManageOwnAccount_Bdy %>" />
				<tis:PopupInfoItem ID="poi18" runat="server" ControlId="chkAllowDeleteOwnAccount"
					DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowDeleteOwnAccount_Hdr %>"
					DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowDeleteOwnAccount_Bdy %>" />
			</PopupInfoItems>
		</tis:PopupInfo>
		<uc1:popup ID="ucPopupContainer" runat="server" />
		<asp:PlaceHolder ID="phAdminFooter" runat="server" />
	</div>

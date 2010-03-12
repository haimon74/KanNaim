<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="manageusers.ascx.cs"
	Inherits="GalleryServerPro.Web.gs.pages.admin.manageusers" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Src="../../Controls/albumtreeview.ascx" TagName="albumtreeview" TagPrefix="uc1" %>
<%@ Register Src="../../Controls/usermessage.ascx" TagName="usermessage" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<%@ Import Namespace="GalleryServerPro.Web" %>

<script type="text/javascript">

	$(function() { $("table.gsp_j_add_rolelist tr:has(td > span.gsp_j_aaor)").hide(); });

	function hideOwnerRoles()
	{
		$("table.gsp_j_edit_rolelist tr:has(td > span.gsp_j_eaor)").hide();
	}

	function toggleAddUserOwnerRoles(chk)
	{
		if (chk.checked)
			$("table.gsp_j_add_rolelist tr:has(td > span.gsp_j_aaor)").fadeIn();
		else
			$("table.gsp_j_add_rolelist tr:has(td > span.gsp_j_aaor)").fadeOut();
	}

	function toggleEditUserOwnerRoles(chk)
	{
		if (chk.checked)
			$("table.gsp_j_edit_rolelist tr:has(td > span.gsp_j_eaor)").fadeIn();
		else
			$("table.gsp_j_edit_rolelist tr:has(td > span.gsp_j_eaor)").fadeOut();
	}

	var gdUsersLoaded = false;
	function gdUsers_Load(sender, eventArgs)
	{
		gdUsersLoaded = true;
	}
</script>

<div class="gsp_indentedContent">
	<p class="admin_h2">
		<asp:Label ID="lblAdminPageHeader" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Page_Header %>" />
	</p>
	<div class="gsp_addpadding1">
		<uc1:usermessage ID="ucUserMessage" runat="server" CssClass="um0ContainerCss gsp_invisible"
			IconStyle="Error" MessageTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Users_User_Message_Hdr %>"
			MessageDetail="Placeholder text - do not delete" />
		<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False"
			Center="False" Width="500px" />
		<p id="pAddUser" runat="server">
			<a id="addnewuser" href="javascript:addUser();">
				<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Link_Text %>" /></a></p>
		<ComponentArt:Grid ID="gdUsers" runat="server" Width="70%" Height="400px" DataSourceID="odsGalleryServerUsers"
			PageSize="100" AutoCallBackOnDelete="true" OnDeleteCommand="gdUsers_DeleteCommand"
			CssClass="gd0Grid" ShowHeader="True" ShowSearchBox="True" SearchTextCssClass="gd0GridHeaderText"
			SearchOnKeyPress="True" SearchText="<%$ Resources:GalleryServerPro, Site_Grid_Search_Label %>"
			HeaderCssClass="gd0GridHeader" FooterCssClass="gd0GridFooter" GroupingNotificationText=""
			EnableViewState="false">
			<ClientEvents>
				<CallbackError EventHandler="gdUsers_CallbackError" />
				<Load EventHandler="gdUsers_Load" />
			</ClientEvents>
			<Levels>
				<ComponentArt:GridLevel DataKeyField="UserName" AllowGrouping="False" AllowReordering="false"
					RowCssClass="gd0Row" DataCellCssClass="gd0DataCell" HeadingCellCssClass="gd0HeadingCell"
					HeadingCellHoverCssClass="gd0HeadingCellHover" HeadingCellActiveCssClass="gd0HeadingCellActive"
					HeadingRowCssClass="gd0HeadingRow" HeadingTextCssClass="gd0HeadingCellText" SortAscendingImageUrl="asc.gif"
					SortDescendingImageUrl="desc.gif" SortImageWidth="10" SortImageHeight="19">
					<Columns>
						<ComponentArt:GridColumn DataCellClientTemplateId="userEditColumn" EditControlType="EditCommand"
							Width="100" AllowSorting="false" />
						<ComponentArt:GridColumn DataField="UserName" AllowHtmlContent="True" HeadingText="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Grid_UserName_Header %>" />
					</Columns>
				</ComponentArt:GridLevel>
			</Levels>
		</ComponentArt:Grid>
	</div>
	<ComponentArt:Dialog ID="dgEditUser" runat="server" CloseTransition="Fade" ShowTransition="Fade"
		AnimationSlide="Linear" AnimationType="Outline" AnimationPath="Direct" AnimationDuration="400"
		TransitionDuration="400" Icon="pencil.gif" Alignment="MiddleCentre" AllowResize="True"
		Height="500" Width="640" ContentCssClass="dg0ContentCss" HeaderCssClass="dg0HeaderCss"
		CssClass="gsp_dg0DialogCss gsp_ns" FooterCssClass="dg0FooterCss">
		<HeaderTemplate>
			<div onmousedown="dgEditUser.StartDrag(event);">
				<img id="dg0DialogCloseImage" onclick="dgEditUser.Close('cancelled');" src="<%= Util.GalleryRoot %>/images/componentart/dialog/close.gif"
					alt="<asp:Literal ID='litPageHeader' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>' />" /><img
						id="dg0DialogIconImage" src="<%= Util.GalleryRoot %>/images/componentart/dialog/pencil.gif"
						alt="" />
				<span id="editUserDialogHeader"></span>
			</div>
		</HeaderTemplate>
		<ContentTemplate>
			<ComponentArt:CallBack ID="cbEditUser" runat="server" OnCallback="cbEditUser_Callback"
				PostState="true">
				<ClientEvents>
					<BeforeCallback EventHandler="cbEditUser_OnBeforeCallback" />
					<CallbackComplete EventHandler="cbEditUser_OnCallbackComplete" />
				</ClientEvents>
				<Content>
					<asp:Panel ID="pnlEditUserDialogContent" runat="server" CssClass="editUserDialogContent">
						<asp:PlaceHolder ID="phEditUserMessage" runat="server" />
						<p class="gsp_fll gsp_bold">
							<asp:Literal ID="litUserName" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_User_Name %>"></asp:Literal>:&nbsp;<asp:Label
								ID="lblUserName" runat="server" CssClass="userNameReadOnly"></asp:Label></p>
						<ComponentArt:TabStrip ID="tsEditUser" MultiPageId="mpEditUser" TopGroupCssClass="ts0TopGroup"
							DefaultSelectedItemLookId="SelectedTab" DefaultItemLookId="DefaultTab" DefaultChildSelectedItemLookId="SelectedTab"
							TopGroupShowSeparators="true" DefaultGroupSeparatorWidth="22" DefaultGroupSeparatorHeight="22"
							DefaultGroupFirstSeparatorWidth="15" DefaultGroupFirstSeparatorHeight="22" DefaultGroupLastSeparatorWidth="22"
							DefaultGroupLastSeparatorHeight="22" Width="100%" runat="server">
							<ItemLooks>
								<ComponentArt:ItemLook LookId="DefaultTab" CssClass="ts0DefaultTab" HoverCssClass="ts0DefaultTabHover"
									LabelPaddingLeft="10" LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
								<ComponentArt:ItemLook LookId="SelectedTab" CssClass="ts0SelectedTab" LabelPaddingLeft="10"
									LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
							</ItemLooks>
							<Tabs>
								<ComponentArt:TabStripTab ID="tabGeneral" runat="server" Text="General" />
								<ComponentArt:TabStripTab ID="tabRoles" runat="server" Text="Roles" />
								<ComponentArt:TabStripTab ID="tabPassword" runat="server" Text="Password" />
							</Tabs>
						</ComponentArt:TabStrip>
						<ComponentArt:MultiPage ID="mpEditUser" CssClass="mp0MultiPage" runat="server">
							<ComponentArt:PageView ID="PageView1" CssClass="mp0MultiPageContent" runat="server">
								<table class="gsp_standardTable">
									<tr>
										<td class="gsp_bold gsp_aligntop">
											<asp:Label ID="lblComment" runat="server" AssociatedControlID="txtComment" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Description_Header_Text %>" />
										</td>
										<td>
											<asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" CssClass="gsp_textarea1"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="gsp_bold">
											<asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Email_Address_Header_Text %>" />
										</td>
										<td>
											<asp:TextBox ID="txtEmail" runat="server" CssClass="gsp_textbox"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="gsp_bold">
											<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Is_Approved_Header_Text %>" />
										</td>
										<td>
											<asp:RadioButton ID="rbApprovedYes" runat="server" GroupName="approved" Text="<%$ Resources:GalleryServerPro, Site_Yes %>" />&nbsp;<asp:RadioButton
												ID="rbApprovedNo" runat="server" GroupName="approved" Text="<%$ Resources:GalleryServerPro, Site_No %>" />
										</td>
									</tr>
									<tr id="trUserAlbumTableRow" runat="server">
										<td class="gsp_bold">
											<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Enable_User_Album_Header_Text %>" />
										</td>
										<td>
											<asp:RadioButton ID="rbUserAlbumYes" runat="server" GroupName="useralbum" Text="<%$ Resources:GalleryServerPro, Site_Yes %>" />&nbsp;<asp:RadioButton
												ID="rbUserAlbumNo" runat="server" GroupName="useralbum" Text="<%$ Resources:GalleryServerPro, Site_No %>" />
										</td>
									</tr>
									<tr>
										<td class="gsp_bold">
											<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Last_Activity_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblLastActivityDate" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td class="gsp_bold">
											<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Last_LogOn_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblLastLogOnDate" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td class="gsp_bold">
											<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Last_Password_Changed_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblLastPasswordChangedDate" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td class="gsp_bold">
											<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Account_Created_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblCreationDate" runat="server"></asp:Label>
										</td>
									</tr>
								</table>
							</ComponentArt:PageView>
							<ComponentArt:PageView ID="PageView2" CssClass="mp0MultiPageContent" runat="server"
								Height="300">
								<p>
									<asp:CheckBox ID="chkShowOwnerRoles" runat="server" CssClass="gsp_fs" onclick="toggleEditUserOwnerRoles(this);"
										Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Show_Album_Owner_Roles_Lbl %>" />
								</p>
								<p class="gsp_bold">
									<asp:Literal ID="litRoleHeader" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Role_Header %>" /></p>
								<div style="max-height: 330px; overflow: auto;">
									<asp:CheckBoxList ID="cblAvailableRolesForExistingUser" runat="server" DataSourceID="odsAllGalleryServerRoles"
										DataTextField="RoleName" DataValueField="RoleName" CssClass="gsp_j_edit_rolelist" OnDataBound="cblAvailableRolesForExistingUser_DataBound">
									</asp:CheckBoxList>
								</div>
							</ComponentArt:PageView>
							<ComponentArt:PageView ID="PageView3" runat="server" CssClass="mp0MultiPageContent">
								<asp:PlaceHolder ID="phDialogMessagePasswordTab" runat="server" />
								<p>
									<asp:RadioButton ID="rbResetPassword" runat="server" GroupName="PasswordOptions"
										Text="Reset Password" Checked="true" /></p>
								<p style="margin-left: 2em;">
									<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Reset_Password_Description %>" />
								</p>
								<p>
									<asp:RadioButton ID="rbChangePassword" runat="server" GroupName="PasswordOptions"
										Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Change_Password_Description %>" /></p>
								<div style="margin-left: 2em;">
									<table>
										<tr>
											<td>
												<asp:Label ID="lblPassword1" runat="server" AssociatedControlID="txtPassword1" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_New_Password_Header_Text %>">
												</asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtPassword1" runat="server" TextMode="Password" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblPassword2" runat="server" AssociatedControlID="txtPassword2" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_New_Password_Confirm_Header_Text %>" />
											</td>
											<td>
												<asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" />
												<asp:Label ID="lblNotMatchingPasswords" runat="server" Visible="false" ForeColor="red"
													Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Passwords_Not_Matching_Error %>" />
											</td>
										</tr>
									</table>
									<p style="margin-top: 2em;">
										<asp:CheckBox ID="chkEmailNewPasswordToUser" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Email_New_Password_To_User_Text %>" /></p>
									<p>
										<input id="btnUpdatePassword" runat="server" type="button" value="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Update_Password_Button_Text %>"
											title="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Update_Password_Button_Tooltip %>"
											onclick="updatePassword();" /></p>
								</div>
							</ComponentArt:PageView>
						</ComponentArt:MultiPage>
					</asp:Panel>
				</Content>
			</ComponentArt:CallBack>
		</ContentTemplate>
		<FooterTemplate>
			<div class="gsp_rightBottom">
				<input id="Button1" runat="server" type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>"
					title="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>" onclick="saveUser();" />
				<input id="Button2" runat="server" onclick="dgEditUser.Close('cancelled');" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
					type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
			</div>
		</FooterTemplate>
	</ComponentArt:Dialog>
	<ComponentArt:Dialog ID="dgAddUser" runat="server" Title="<%$ Resources:GalleryServerPro, Admin_Dialog_Title_Add_User %>"
		AnimationDirectionElement="addnewuser" CloseTransition="Fade" ShowTransition="Fade"
		AnimationSlide="Linear" AnimationType="Outline" AnimationPath="Direct" AnimationDuration="400"
		TransitionDuration="400" Icon="pencil.gif" Alignment="MiddleCentre" AllowResize="True"
		Height="500" Width="640" ContentCssClass="dg0ContentCss" HeaderCssClass="dg0HeaderCss"
		CssClass="gsp_dg0DialogCss gsp_ns">
		<HeaderTemplate>
			<div onmousedown="dgAddUser.StartDrag(event);">
				<img id="dg0DialogCloseImage" onclick="closeAddUserWizard();" src="<%= Util.GalleryRoot %>/images/componentart/dialog/close.gif"
					alt="<asp:Literal ID='litPageHeader' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>' />" /><img
						id="dg0DialogIconImage" src="<%= Util.GalleryRoot %>/images/componentart/dialog/pencil.gif"
						alt="" />
				<asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Dialog_Title_Text %>" />
			</div>
		</HeaderTemplate>
		<ContentTemplate>
			<asp:Panel ID="pnlAddUserDialogContent" runat="server" CssClass="addUserDialogContent">
				<ComponentArt:CallBack ID="cbAddUser" runat="server" OnCallback="cbAddUser_Callback"
					PostState="true">
					<ClientEvents>
						<BeforeCallback EventHandler="cbAddUser_BeforeCallback" />
						<CallbackComplete EventHandler="cbAddUser_OnCallbackComplete" />
					</ClientEvents>
					<Content>
						<asp:Panel ID="pnlAddUserMessage" runat="server" />
					</Content>
				</ComponentArt:CallBack>
				<ComponentArt:MultiPage ID="mpAddUser" CssClass="mp1MultiPage" runat="server">
					<ComponentArt:PageView ID="AddUser_GeneralInfo" CssClass="mp1MultiPageContent" runat="server">
						<p class="gsp_bold">
							<asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Step1_Header_Text %>" /></p>
						<table class="newUserWizStep1Table">
							<tr style="height: 60px;">
								<td style="width: 20%;">
									<asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Username_Header_Text %>" /><span
										class="gsp_fs gsp_msgwarning">*</span>
								</td>
								<td style="width: 80%;">
									<asp:TextBox ID="txtNewUserUserName" runat="server" onchange="validateNewUserName(this);"></asp:TextBox>
									<asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtNewUserUsername"
										ErrorMessage="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
									<ComponentArt:CallBack ID="cbValidateNewUserName" runat="server" CssClass="gsp_addtopmargin1"
										OnCallback="cbValidateNewUserName_Callback">
										<Content>
											<asp:Label ID="lblUserNameValidationResult" runat="server" /></Content>
									</ComponentArt:CallBack>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Email_Header_Text %>" />
								</td>
								<td>
									<asp:TextBox ID="txtNewUserEmail" runat="server" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Password_Header_Text %>" /><span
										class="gsp_fs gsp_msgwarning">*</span>
								</td>
								<td>
									<asp:TextBox ID="txtNewUserPassword1" runat="server" TextMode="Password"></asp:TextBox>
									<asp:RequiredFieldValidator ID="rfvPassword1" runat="server" ControlToValidate="txtNewUserPassword1"
										ErrorMessage="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Confirm_Password_Header_Text %>" /><span
										class="gsp_fs gsp_msgwarning">*</span>
								</td>
								<td>
									<asp:TextBox ID="txtNewUserPassword2" runat="server" TextMode="Password"></asp:TextBox>
									<asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtNewUserPassword2"
										Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>"></asp:RequiredFieldValidator>
									<asp:CompareValidator ID="cvPasswordsEqual" runat="server" ControlToCompare="txtNewUserPassword1"
										ControlToValidate="txtNewUserPassword2" Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Passwords_Not_Matching_Error %>"
										SetFocusOnError="True"></asp:CompareValidator>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<p class="gsp_fs gsp_msgwarning">
										<asp:Literal ID="Literal14" runat="server" Text="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" /></p>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<p class="gsp_fs">
										<asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Step1_Footer_Text %>" />
									</p>
								</td>
							</tr>
						</table>
						<div class="gsp_rightBottomAbsolute">
							<p class="gsp_collapse">
								<input id="btnAddUserNext" runat="server" onclick="addUserWizard_onCompleteStep1();"
									title="<%$ Resources:GalleryServerPro, Site_Wizard_Next_Button_Tooltip %>" type="button"
									value="<%$ Resources:GalleryServerPro, Site_Wizard_Next_Button_Text %>" />
								<input id="btnCancel1" runat="server" onclick="closeAddUserWizard();" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
						</div>
					</ComponentArt:PageView>
					<ComponentArt:PageView ID="AddUser_Roles" CssClass="mp1MultiPageContent" runat="server">
						<p>
							<asp:CheckBox ID="chkShowOwnerRoles2" runat="server" CssClass="gsp_fs" onclick="toggleAddUserOwnerRoles(this);"
								Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Show_Album_Owner_Roles_Lbl %>" />
						</p>
						<p class="gsp_bold">
							<asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Step2_Header_Text %>" /></p>
						<div style="max-height: 390px; overflow: auto;">
							<asp:CheckBoxList ID="cblAvailableRolesForNewUser" runat="server" DataSourceID="odsAllGalleryServerRoles"
								DataTextField="RoleName" DataValueField="RoleName" CssClass="gsp_j_add_rolelist" onclick="validateNewUser();" OnDataBound="cblAvailableRolesForNewUser_DataBound">
							</asp:CheckBoxList>
							&nbsp;</div>
						<div class="gsp_rightBottomAbsolute">
							<p class="gsp_collapse">
								<input id="btnAddUserPrevious" runat="server" onclick="addUserWizard_onPreviousClickStep2();"
									title="<%$ Resources:GalleryServerPro, Site_Wizard_Previous_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Site_Wizard_Previous_Button_Text %>" />
								<input id="btnAddUserComplete" runat="server" onclick="addUserWizard_onCompleteStep2();"
									title="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Complete_Wizard_Button_Text %>"
									type="button" value="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Complete_Wizard_Button_Tooltip %>" />
								<input id="btnCancel2" runat="server" onclick="closeAddUserWizard();" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
						</div>
					</ComponentArt:PageView>
					<ComponentArt:PageView ID="AddUser_Complete" CssClass="mp1MultiPageContent" runat="server">
						<p>
							<asp:Literal ID="Literal17" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Completed_Header_Text %>" />
							<a href="javascript:createAnotherUser();">
								<asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Create_Another_User_Hyperlink_Text %>" /></a></p>
						<p>
							<asp:Literal ID="Literal19" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Completed_Username_Header_Text %>" />
							<span id="newUsername" class="gsp_bold"></span>
						</p>
						<div class="gsp_rightBottomAbsolute">
							<p class="gsp_collapse">
								<input id="btnClose" runat="server" onclick="closeAddUserWizard();" title="<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>" />
						</div>
					</ComponentArt:PageView>
				</ComponentArt:MultiPage></asp:Panel>
		</ContentTemplate>
	</ComponentArt:Dialog>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem runat="server" ControlId="cblAvailableRolesForExistingUser" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:ObjectDataSource ID="odsGalleryServerUsers" runat="server" SelectMethod="GetAllUsers"
		DataObjectTypeName="GalleryServerPro.Web.Entity.UserEntity" TypeName="GalleryServerPro.Web.Controller.UserController">
	</asp:ObjectDataSource>
	<asp:ObjectDataSource ID="odsAllGalleryServerRoles" runat="server" SelectMethod="GetGalleryServerRoles"
		TypeName="GalleryServerPro.Web.Controller.RoleController"></asp:ObjectDataSource>
</div>

<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="manageusers.aspx.cs" Inherits="GalleryServerPro.Web.admin.manageusers" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../uc/usermessage.ascx" TagName="usermessage" TagPrefix="uc1" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<uc1:usermessage ID="ucUserMessage" runat="server" CssClass="um0ContainerCss invisible"
			IconStyle="Error" MessageTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Users_User_Message_Hdr %>"
			MessageDetail="Placeholder text - do not delete" />
		<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px" />
		<p id="pAddUser" runat="server">
			<a id="addnewuser" href="javascript:addUser();">
				<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Link_Text %>" /></a></p>
		<CA:Grid ID="gdUsers" runat="server" Width="70%" Height="400px" DataSourceID="odsGalleryServerUsers"
			ImagesBaseUrl="~/images/componentart/grid/" PagerImagesFolderUrl="~/images/componentart/grid/pager/"
			PageSize="100" AutoCallBackOnDelete="true" OnDeleteCommand="gdUsers_DeleteCommand"
			CssClass="gd0Grid" ShowHeader="True" ShowSearchBox="True" SearchTextCssClass="gd0GridHeaderText"
			SearchOnKeyPress="True" HeaderCssClass="gd0GridHeader" FooterCssClass="gd0GridFooter"
			GroupingNotificationText="" EnableViewState="false">
			<ClientEvents>
				<CallbackError EventHandler="gdUsers_CallbackError" />
			</ClientEvents>
			<Levels>
				<CA:GridLevel DataKeyField="UserName" AllowGrouping="False" AllowReordering="false"
					RowCssClass="gd0Row" DataCellCssClass="gd0DataCell" HeadingCellCssClass="gd0HeadingCell"
					HeadingCellHoverCssClass="gd0HeadingCellHover" HeadingCellActiveCssClass="gd0HeadingCellActive"
					HeadingRowCssClass="gd0HeadingRow" HeadingTextCssClass="gd0HeadingCellText" SortAscendingImageUrl="asc.gif"
					SortDescendingImageUrl="desc.gif" SortImageWidth="10" SortImageHeight="19">
					<Columns>
						<CA:GridColumn DataCellClientTemplateId="userEditColumn" EditControlType="EditCommand"
							Width="100" AllowSorting="false" />
						<CA:GridColumn DataField="UserName" HeadingText="UserName" />
					</Columns>
				</CA:GridLevel>
			</Levels>
		</CA:Grid>
	</div>
	<CA:Dialog ID="dgEditUser" runat="server" CloseTransition="Fade" ShowTransition="Fade"
		AnimationSlide="Linear" AnimationType="Live" AnimationPath="Direct" AnimationDuration="400"
		TransitionDuration="400" Icon="pencil.gif" Alignment="MiddleCentre" AllowResize="True"
		Height="500" Width="640" ContentCssClass="dg0ContentCss" HeaderCssClass="dg0HeaderCss"
		CssClass="dg0DialogCss" FooterCssClass="dg0FooterCss">
		<HeaderTemplate>
			<div onmousedown="dgEditUser.StartDrag(event);">
				<img id="dg0DialogCloseImage" onclick="dgEditUser.Close('cancelled');" src="../images/componentart/dialog/close.gif"
					alt="<asp:Literal ID='litPageHeader' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>' />" /><img
						id="dg0DialogIconImage" src="../images/componentart/dialog/pencil.gif" alt="" />
				<span id="editUserDialogHeader"></span>
			</div>
		</HeaderTemplate>
		<ContentTemplate>
			<CA:CallBack ID="cbEditUser" runat="server" OnCallback="cbEditUser_Callback" PostState="true">
				<ClientEvents>
					<CallbackComplete EventHandler="cbEditUser_OnCallbackComplete" />
				</ClientEvents>
				<Content>
					<asp:Panel ID="pnlEditUserDialogContent" runat="server" CssClass="editUserDialogContent">
						<asp:PlaceHolder ID="phEditUserMessage" runat="server" />
						<p class="fll bold">
							<asp:Literal ID="litUserName" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_User_Name %>"></asp:Literal>:&nbsp;<asp:Label
								ID="lblUserName" runat="server" CssClass="userNameReadOnly"></asp:Label></p>
						<CA:TabStrip ID="tsEditUser" MultiPageId="mpEditUser" TopGroupCssClass="ts0TopGroup"
							DefaultSelectedItemLookId="SelectedTab" DefaultItemLookId="DefaultTab" DefaultChildSelectedItemLookId="SelectedTab"
							TopGroupShowSeparators="true" DefaultGroupSeparatorWidth="22" DefaultGroupSeparatorHeight="22"
							DefaultGroupFirstSeparatorWidth="15" DefaultGroupFirstSeparatorHeight="22" DefaultGroupLastSeparatorWidth="22"
							DefaultGroupLastSeparatorHeight="22" ImagesBaseUrl="~/images/componentart/tabstrip/"
							TopGroupSeparatorImagesFolderUrl="~/images/componentart/tabstrip/" Width="100%"
							runat="server">
							<ItemLooks>
								<CA:ItemLook LookId="DefaultTab" CssClass="ts0DefaultTab" HoverCssClass="ts0DefaultTabHover"
									LabelPaddingLeft="10" LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
								<CA:ItemLook LookId="SelectedTab" CssClass="ts0SelectedTab" LabelPaddingLeft="10"
									LabelPaddingRight="17" LabelPaddingTop="2" LabelPaddingBottom="6" />
							</ItemLooks>
							<Tabs>
								<CA:TabStripTab ID="tabGeneral" runat="server" Text="General" />
								<CA:TabStripTab ID="tabRoles" runat="server" Text="Roles" />
								<CA:TabStripTab ID="tabPassword" runat="server" Text="Password" />
							</Tabs>
						</CA:TabStrip>
						<CA:MultiPage ID="mpEditUser" CssClass="mp0MultiPage" runat="server">
							<CA:PageView CssClass="mp0MultiPageContent" runat="server">
								<table class="standardTable">
									<tr>
										<td class="bold">
											<asp:Label ID="lblComment" runat="server" AssociatedControlID="txtComment" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Description_Header_Text %>" />
										</td>
										<td>
											<asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" CssClass="textarea1"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="bold">
											<asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Email_Address_Header_Text %>" />
										</td>
										<td>
											<asp:TextBox ID="txtEmail" runat="server" CssClass="textbox"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td class="bold">
											<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Is_Approved_Header_Text %>" />
										</td>
										<td>
											<asp:RadioButton ID="rbApprovedYes" runat="server" GroupName="approved" Text="Yes" />&nbsp;<asp:RadioButton
												ID="rbApprovedNo" runat="server" GroupName="approved" Text="No" />
										</td>
									</tr>
									<tr>
										<td class="bold">
											<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Last_Activity_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblLastActivityDate" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td class="bold">
											<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Last_LogOn_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblLastLogOnDate" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td class="bold">
											<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Last_Password_Changed_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblLastPasswordChangedDate" runat="server"></asp:Label>
										</td>
									</tr>
									<tr>
										<td class="bold">
											<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Account_Created_Date_Header_Text %>" />
										</td>
										<td>
											<asp:Label ID="lblCreationDate" runat="server"></asp:Label>
										</td>
									</tr>
								</table>
							</CA:PageView>
							<CA:PageView CssClass="mp0MultiPageContent" runat="server" Height="300">
								<p class="bold">
									<asp:Literal ID="litRoleHeader" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Role_Header %>" /></p>
								<div style="max-height: 330px; overflow: auto;">
									<asp:CheckBoxList ID="cblAvailableRolesForExistingUser" runat="server" DataSourceID="odsAllGalleryServerRoles"
										DataTextField="RoleName" DataValueField="RoleName">
									</asp:CheckBoxList>
								</div>
							</CA:PageView>
							<CA:PageView runat="server" CssClass="mp0MultiPageContent">
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
							</CA:PageView>
						</CA:MultiPage>
					</asp:Panel>
				</Content>
			</CA:CallBack>
		</ContentTemplate>
		<FooterTemplate>
			<div class="rightBottom">
				<input id="Button1" runat="server" type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>"
					title="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>" onclick="saveUser();" />
				<input id="Button2" runat="server" onclick="dgEditUser.Close('cancelled');" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
					type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
			</div>
		</FooterTemplate>
	</CA:Dialog>
	<CA:Dialog ID="dgAddUser" runat="server" Title="<%$ Resources:GalleryServerPro, Admin_Dialog_Title_Add_User %>"
		AnimationDirectionElement="addnewuser" CloseTransition="Fade" ShowTransition="Fade"
		AnimationSlide="Linear" AnimationType="Live" AnimationPath="Direct" AnimationDuration="400"
		TransitionDuration="400" Icon="pencil.gif" Alignment="MiddleCentre" AllowResize="True"
		Height="500" Width="640" ContentCssClass="dg0ContentCss" HeaderCssClass="dg0HeaderCss"
		CssClass="dg0DialogCss">
		<HeaderTemplate>
			<div onmousedown="dgAddUser.StartDrag(event);">
				<img id="dg0DialogCloseImage" onclick="closeAddUserWizard();" src="../images/componentart/dialog/close.gif"
					alt="<asp:Literal ID='litPageHeader' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>' />" /><img
						id="dg0DialogIconImage" src="../images/componentart/dialog/pencil.gif" alt="" />
				<asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Dialog_Title_Text %>" />
			</div>
		</HeaderTemplate>
		<ContentTemplate>
			<asp:Panel ID="pnlAddUserDialogContent" runat="server" CssClass="addUserDialogContent">
				<CA:CallBack ID="cbAddUser" runat="server" OnCallback="cbAddUser_Callback" PostState="true">
					<ClientEvents>
						<CallbackComplete EventHandler="cbAddUser_OnCallbackComplete" />
					</ClientEvents>
					<Content>
						<asp:Panel ID="pnlAddUserMessage" runat="server" />
					</Content>
				</CA:CallBack>
				<CA:MultiPage ID="mpAddUser" CssClass="mp1MultiPage" runat="server">
					<CA:PageView ID="AddUser_GeneralInfo" CssClass="mp1MultiPageContent" runat="server">
						<p>
							<asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Step1_Header_Text %>" /></p>
						<table class="newUserWizStep1Table">
							<tr style="height: 60px;">
								<td style="width: 20%;">
									<asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Username_Header_Text %>" /><span
										class="fs msgwarning">*</span>:
								</td>
								<td style="width: 80%;">
									<asp:TextBox ID="txtNewUserUserName" runat="server" onchange="validateNewUserName(this);"></asp:TextBox>
									<asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtNewUserUsername"
										ErrorMessage="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
									<CA:CallBack ID="cbValidateNewUserName" runat="server" CssClass="addtopmargin1" OnCallback="cbValidateNewUserName_Callback">
										<Content>
											<asp:Label ID="lblUserNameValidationResult" runat="server" /></Content>
									</CA:CallBack>
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
										class="fs msgwarning">*</span>:
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
										class="fs msgwarning">*</span>:
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
									<p class="fs msgwarning">
										<asp:Literal ID="Literal14" runat="server" Text="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" /></p>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<p class="fs">
										<asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Step1_Footer_Text %>" />
									</p>
								</td>
							</tr>
						</table>
						<div class="rightBottomAbsolute">
							<p class="nomargin nopadding">
								<input id="btnAddUserNext" runat="server" onclick="addUserWizard_onCompleteStep1();"
									title="<%$ Resources:GalleryServerPro, Site_Wizard_Next_Button_Tooltip %>" type="button"
									value="<%$ Resources:GalleryServerPro, Site_Wizard_Next_Button_Text %>" />
								<input id="btnCancel1" runat="server" onclick="closeAddUserWizard();" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
						</div>
					</CA:PageView>
					<CA:PageView ID="AddUser_Roles" CssClass="mp1MultiPageContent" runat="server">
						<p>
							<asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Step2_Header_Text %>" /></p>
						<div style="max-height: 390px; overflow: auto;">
							<asp:CheckBoxList ID="cblAvailableRolesForNewUser" runat="server" DataSourceID="odsAllGalleryServerRoles"
								DataTextField="RoleName" DataValueField="RoleName" onclick="validateNewUser();">
							</asp:CheckBoxList>
							&nbsp;</div>
						<div class="rightBottomAbsolute">
							<p class="nomargin nopadding">
								<input id="btnAddUserPrevious" runat="server" onclick="addUserWizard_onPreviousClickStep2();"
									title="<%$ Resources:GalleryServerPro, Site_Wizard_Previous_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Site_Wizard_Previous_Button_Text %>" />
								<input id="btnAddUserComplete" runat="server" onclick="addUserWizard_onCompleteStep2();"
									title="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Complete_Wizard_Button_Text %>"
									type="button" value="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Complete_Wizard_Button_Tooltip %>" />
								<input id="btnCancel2" runat="server" onclick="closeAddUserWizard();" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
						</div>
					</CA:PageView>
					<CA:PageView ID="AddUser_Complete" CssClass="mp1MultiPageContent" runat="server">
						<p>
							<asp:Literal ID="Literal17" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Completed_Header_Text %>" />
							<a href="javascript:createAnotherUser();">
								<asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Create_Another_User_Hyperlink_Text %>" /></a></p>
						<p>
							<asp:Literal ID="Literal19" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Add_User_Completed_Username_Header_Text %>" />
							<span id="newUsername" class="bold"></span>
						</p>
						<div class="rightBottomAbsolute">
							<p class="nomargin nopadding">
								<input id="btnClose" runat="server" onclick="closeAddUserWizard();" title="<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Tooltip %>"
									type="button" value="<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>" />
						</div>
					</CA:PageView>
				</CA:MultiPage></asp:Panel>
		</ContentTemplate>
	</CA:Dialog>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="lblAdminPageHeader"
				DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Overview_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Overview_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:ObjectDataSource ID="odsGalleryServerUsers" runat="server" SelectMethod="GetAllUsers"
		DataObjectTypeName="System.Web.Security.MembershipUser" TypeName="System.Web.Security.Membership">
	</asp:ObjectDataSource>
	<asp:ObjectDataSource ID="odsAllGalleryServerRoles" runat="server" SelectMethod="GetGalleryServerRoles"
		TypeName="GalleryServerPro.Web.GspPage"></asp:ObjectDataSource>
</asp:Content>

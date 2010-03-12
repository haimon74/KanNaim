<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true" CodeBehind="manageroles.aspx.cs"
	Inherits="GalleryServerPro.Web.admin.manageroles" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Src="~/uc/albumtreeview.ascx" TagName="albumtreeview" TagPrefix="uc1" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Src="../uc/usermessage.ascx" TagName="usermessage" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls" TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<uc1:usermessage ID="ucUserMessage" runat="server" CssClass="um0ContainerCss invisible" IconStyle="Error" MessageTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_User_Message_Hdr %>"
			MessageDetail="Placeholder text - do not delete" />
		<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px" />
		<p id="pAddRole" runat="server">
			<a id="addnewrole" href="javascript:addRole();">
				<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Add_Role_Link_Text %>" /></a>
		</p>
		<CA:Grid ID="gdRoles" runat="server" Width="70%" Height="400px" DataSourceID="odsGalleryServerRoles" ImagesBaseUrl="~/images/componentart/grid/"
			PagerImagesFolderUrl="~/images/componentart/grid/pager/" PageSize="100" AutoCallBackOnDelete="true" OnDeleteCommand="gdRoles_DeleteCommand"
			CssClass="gd0Grid" ShowHeader="True" ShowSearchBox="True" SearchTextCssClass="gd0GridHeaderText" SearchOnKeyPress="True"
			HeaderCssClass="gd0GridHeader" FooterCssClass="gd0GridFooter" GroupingNotificationText="" EnableViewState="false">
			<ClientEvents>
				<CallbackError EventHandler="gdRoles_CallbackError" />
			</ClientEvents>
			<Levels>
				<CA:GridLevel DataKeyField="RoleName" AllowGrouping="False" AllowReordering="false" RowCssClass="gd0Row" DataCellCssClass="gd0DataCell"
					HeadingCellCssClass="gd0HeadingCell" HeadingCellHoverCssClass="gd0HeadingCellHover" HeadingCellActiveCssClass="gd0HeadingCellActive"
					HeadingRowCssClass="gd0HeadingRow" HeadingTextCssClass="gd0HeadingCellText" SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif"
					SortImageWidth="10" SortImageHeight="19">
					<Columns>
						<CA:GridColumn DataCellClientTemplateId="roleEditColumn" EditControlType="EditCommand" Width="100" AllowSorting="false" />
						<CA:GridColumn DataField="RoleName" HeadingText="Name" />
					</Columns>
				</CA:GridLevel>
			</Levels>
		</CA:Grid>
	</div>
	<CA:Dialog ID="dgEditRole" runat="server" CloseTransition="Fade" ShowTransition="Fade" AnimationSlide="Linear" AnimationType="Live"
		AnimationPath="Direct" AnimationDuration="400" TransitionDuration="400" Icon="pencil.gif" Alignment="MiddleCentre" AllowResize="True"
		Height="500" Width="640" ContentCssClass="dg0ContentCss" HeaderCssClass="dg0HeaderCss" CssClass="dg0DialogCss" FooterCssClass="dg0FooterCss">
		<HeaderTemplate>
			<div onmousedown="dgEditRole.StartDrag(event);">
				<img id="dg0DialogCloseImage" onclick="dgEditRole.Close('cancelled');" src="../images/componentart/dialog/close.gif" alt="<asp:Literal ID='litPageHeader' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Wizard_Close_Button_Text %>' />" /><img
					id="dg0DialogIconImage" src="../images/componentart/dialog/pencil.gif" alt="" />
				<span id="dialogHeader"></span>
			</div>
		</HeaderTemplate>
		<ContentTemplate>
			<CA:CallBack ID="cbEditRole" runat="server" OnCallback="cbEditRole_Callback" PostState="true">
				<ClientEvents>
					<CallbackComplete EventHandler="cbEditRole_OnCallbackComplete" />
				</ClientEvents>
				<Content>
					<asp:Panel ID="pnlDialogContent" runat="server" CssClass="editRoleDialogContent">
						<asp:PlaceHolder ID="phMessage" runat="server" />
						<p class="fll bold nopadding nomargin">
							<asp:Literal ID="litRoleName" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Role_Name %>"></asp:Literal>:&nbsp;<asp:Label
								ID="lblRoleName" runat="server" CssClass="roleNameReadOnly"></asp:Label><asp:TextBox ID="txtRoleName" runat="server" Style="width: 75%">
								</asp:TextBox></p>
						<table>
							<tr>
								<td style="vertical-align: top; width: 50%;">
									<p class="bold" style="padding-right: 3em;">
										<asp:Literal ID="litRolePermissionsHeader" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Permission_Header %>"></asp:Literal></p>
									<p>
										<asp:CheckBox ID="chkAdministerSite" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Admin_Text %>"
											onclick="javascript:chkAdministerSite_CheckChanged(this);" />
										<asp:Label ID="lblAdministerSite" runat="server" /></p>
									<div class="addleftpadding6">
										<p>
											<asp:CheckBox ID="chkViewObject" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_View_Object_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblViewObject" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkViewHiResImage" runat="server" onclick="return !IsAdministerSiteChecked();" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_View_HiRes_Text %>" />
											<asp:Label ID="lblViewHiResImage" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkAddAlbum" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Add_Album_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblAddAlbum" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkAddMediaObject" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Add_Media_Object_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblAddMediaObject" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkEditAlbum" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Edit_Album_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblEditAlbum" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkEditMediaObject" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Edit_Media_Object_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblEditMediaObject" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkDeleteChildAlbum" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Delete_Album_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblDeleteChildAlbum" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkDeleteMediaObject" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Delete_Media_Object_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblDeleteMediaObject" runat="server" /></p>
										<p>
											<asp:CheckBox ID="chkSynchronize" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Synch_Text %>"
												onclick="return !IsAdministerSiteChecked();" />
											<asp:Label ID="lblSynchronize" runat="server" /></p>
									</div>
									<p>
										<asp:CheckBox ID="chkHideWatermark" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Hide_Watermark_Text %>" />
										<asp:Label ID="lblHideWatermark" runat="server" /></p>
								</td>
								<td style="vertical-align: top; width: 50%;">
									<p class="bold">
										<asp:Literal ID="litAlbumHeader" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Album_Header %>"></asp:Literal></p>
									<uc1:albumtreeview ID="tvUC" runat="server" AllowMultiSelect="true" Width="100%" Height="358" />
								</td>
							</tr>
						</table>
					</asp:Panel>
				</Content>
			</CA:CallBack>
		</ContentTemplate>
		<FooterTemplate>
			<div class="rightBottom">
				<input runat="server" type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>" title="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>"
					onclick="saveRole();" />
				<input runat="server" onclick="dgEditRole.Close('cancelled');" title="<%$ Resources:GalleryServerPro, Site_Dialog_Cancel_Button_Tooltip %>"
					type="button" value="<%$ Resources:GalleryServerPro, Default_Task_Close_Button_Text %>" />
			</div>
		</FooterTemplate>
	</CA:Dialog>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="poi1" runat="server" ControlId="lblAdminPageHeader" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Overview_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Overview_Bdy %>" />
			<tis:PopupInfoItem ID="poi2" runat="server" ControlId="lblAdministerSite" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_AdminSite_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_AdminSite_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="lblViewObject" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_ViewObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_ViewObject_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="lblViewHiResImage" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_ViewHiResImage_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_ViewHiResImage_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="lblAddAlbum" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_AddAlbum_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_AddAlbum_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="lblAddMediaObject" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_AddMediaObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_AddMediaObject_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="lblEditAlbum" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_EditAlbum_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_EditAlbum_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="lblEditMediaObject" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_EditMediaObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_EditMediaObject_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="lblDeleteChildAlbum" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_DeleteChildAlbum_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_DeleteChildAlbum_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem8" runat="server" ControlId="lblDeleteMediaObject" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_DeleteMediaObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_DeleteMediaObject_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem9" runat="server" ControlId="lblSynchronize" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Synchronize_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_Synchronize_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem10" runat="server" ControlId="lblHideWatermark" DialogTitle="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_HideWatermark_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Manage_Roles_HideWatermark_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:ObjectDataSource ID="odsGalleryServerRoles" runat="server" SelectMethod="GetGalleryServerRoles" TypeName="GalleryServerPro.Web.GspPage">
	</asp:ObjectDataSource>
</asp:Content>

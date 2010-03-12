<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.Master" AutoEventWireup="true" CodeBehind="backup-restore.aspx.cs"
	Inherits="GalleryServerPro.Web.admin.backup_restore" Title="Untitled Page" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls" TagPrefix="tis" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Src="../uc/usermessage.ascx" TagName="usermessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c3" runat="server">

	<script type="text/javascript">
function file_change(sender,args) { }

function add_file(ctrl,el) {
	ctrl.AddFile();
	if (ctrl.FileCount == ctrl.MaximumFileCount) el.className = "add-d";
}

function remove_files(ctrl) {
	if (ctrl.FileCount < ctrl.MaximumFileCount) el.className = "add";
}

function move_caret(el) {
	var i = el.value.length + 1;
	if (el.createTextRange) {
		var c = document.selection.createRange().duplicate();
		while (c.parentElement() == el && c.move("character",1) == 1) --i;
	}
}


function upload_begin(sender,args) { }

function upload_end(sender,args) {
}


//	File size formatting function
//	n = size in bytes
//	fmt = output format ("b","kb","mb","gb") [optional; if omitted it will return the best match]
function format_file_size(n,fmt) {
	if (!fmt) {		//	no formatting specified; automatically select the best format
		if (n < 1000) fmt = "b";
		else if (n < 1000000) fmt = "kb";
		else if (n < 1000000000) fmt = "mb";
		else fmt = "gb";
	}

	switch(fmt.toLowerCase()) {
		case "kb": return String((n * 0.001).toFixed(2)) + " <asp:Literal ID='l11' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Kilobyte_Abbreviation %>' />";break;
		case "mb": return String((n * 0.000001).toFixed(2)) + " <asp:Literal ID='l12' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Megabyte_Abbreviation %>' />";break;
		case "gb": return String((n * 0.000000001).toFixed(2)) + " <asp:Literal ID='l13' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Gigabyte_Abbreviation %>' />";break;
		default: return String(n.toFixed(2)) + " <asp:Literal ID='l14' runat='server' Text='<%$ Resources:GalleryServerPro, Site_Byte_Abbreviation %>' />";
	}
}

function get_percentage(n) { return String(Math.round(n * 100)); }

//	Time formatting function
//	t = time in seconds
//	returns a string formatted thusly: hh:mm:ss [hours is only present if needed]
function format_time(t) {
	var s = Math.floor(t);
	var m = Math.floor(s / 60);
	var h = Math.floor(m / 60);

	s = pad_time(s % 60);
	m = pad_time(m % 60) + ":";
	h = (h == 0) ? "" : pad_time(h % 60) + ":";

	return (h + m + s);
}

function pad_time(t) { return String(((t > 9) ? "" : "0") + t); }

function init_upload(ctrl) {
	if (ctrl.GetFiles().length > 0 && !ctrl.Uploading) {
		ctrl.Upload();
		UploadDialog.Show();
	}
}

//	Background image preloader
(new Image()).src = "../images/componentart/upload/vertical.png";
	</script>

	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<CA:TabStrip ID="tsBackupRestore" MultiPageId="mpBackupRestore" TopGroupCssClass="ts0TopGroup" DefaultSelectedItemLookId="SelectedTab"
			DefaultItemLookId="DefaultTab" DefaultChildSelectedItemLookId="SelectedTab" TopGroupShowSeparators="true" DefaultGroupSeparatorWidth="22"
			DefaultGroupSeparatorHeight="22" DefaultGroupFirstSeparatorWidth="15" DefaultGroupFirstSeparatorHeight="22" DefaultGroupLastSeparatorWidth="22"
			DefaultGroupLastSeparatorHeight="22" ImagesBaseUrl="~/images/componentart/tabstrip/" TopGroupSeparatorImagesFolderUrl="~/images/componentart/tabstrip/"
			Width="99%" runat="server">
			<ItemLooks>
				<CA:ItemLook LookId="DefaultTab" CssClass="ts0DefaultTab" HoverCssClass="ts0DefaultTabHover" LabelPaddingLeft="10" LabelPaddingRight="17"
					LabelPaddingTop="2" LabelPaddingBottom="6" />
				<CA:ItemLook LookId="SelectedTab" CssClass="ts0SelectedTab" LabelPaddingLeft="10" LabelPaddingRight="17" LabelPaddingTop="2"
					LabelPaddingBottom="6" />
			</ItemLooks>
			<Tabs>
				<CA:TabStripTab ID="tabGeneral" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Title %>" />
				<CA:TabStripTab ID="tabRoles" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Title %>" />
			</Tabs>
		</CA:TabStrip>
		<CA:MultiPage ID="mpBackupRestore" CssClass="mp2MultiPage" runat="server">
			<CA:PageView ID="PageView1" CssClass="mp2MultiPageContent" runat="server">
				<h3>
					<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Hdr %>" /></h3>
				<p class="addtopmargin5">
					<asp:Label ID="lblBackupDtl" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Dtl %>" /></p>
				<uc1:usermessage ID="ucUserMessageBackup" runat="server" CssClass="um0ContainerCss" IconStyle="Information" MessageTitle=""
					DetailCssClass="um1DetailCss" MessageDetail="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Note %>" />
				<p class="addtopmargin5">
					<asp:CheckBox ID="chkExportMembership" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Export_Users_Checkbox_Text %>"
						Checked="true" /></p>
				<p>
					<asp:CheckBox ID="chkExportGalleryData" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Export_Gallery_Data_Checkbox_Text %>"
						Checked="true" /></p>
				<p>
					<asp:Button ID="btnExportData" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Backup_Tab_Export_Btn_Text %>"
						OnClick="btnExportData_Click" />
				</p>
			</CA:PageView>
			<CA:PageView ID="PageView2" CssClass="mp2MultiPageContent" runat="server" Height="300">
				<h3>
					<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Hdr %>" />
				</h3>
				<p>
					<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Dtl %>" /></p>
				<uc1:usermessage ID="ucUserMessageRestore" runat="server" CssClass="um0ContainerCss" IconStyle="Information" MessageTitle=""
					DetailCssClass="um1DetailCss" MessageDetail="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Note %>" />
				<asp:PlaceHolder ID="phUpload" runat="server" />
				<asp:Panel ID="pnlRestoreFileInfo" runat="server" Visible="true">
					<table id="restoreFileContainer" cellpadding="0" cellspacing="0">
						<tr class="tableSummaryRow">
							<td colspan="3">
								<p>
									<asp:LinkButton ID="lbRemoveRestoreFile" runat="server" Text="Remove" CssClass="fs" Visible="false" OnClick="lbRemoveRestoreFile_Click" />
									<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Filename_Hdr %>" />
									<asp:Label ID="lblRestoreFilename" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Empty_Filename %>"
										CssClass="msgwarning" /></p>
								<p>
									<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Schema_Hdr %>" />&nbsp;<asp:Label
										ID="lblSchemaVersion" runat="server" CssClass="msgdark" /></p>
								<p>
									<asp:Image ID="imgValidationResult" runat="server" Width="16" Height="16" Style="vertical-align: middle;" Visible="false" />&nbsp;<asp:Label
										ID="lblValidationResult" runat="server" /></p>
							</td>
						</tr>
						<tr class="tableHeaderRow">
							<td class="topBorder bottomBorder">
								&nbsp;
							</td>
							<td class="topBorder bottomBorder">
								<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Table_Column_Hdr %>" />
							</td>
							<td class="numRecords topBorder bottomBorder">
								<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_NumRecords_Column_Hdr %>" />
							</td>
						</tr>
						<tr>
							<td rowspan="5" style="width: 150px;" class="bottomBorder">
								<asp:CheckBox ID="chkImportMembership" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Import_Users_Checkbox_Text %>"
									Checked="True" />
							</td>
							<td style="width: 250px;">
								aspnet_Applications
							</td>
							<td style="width: 100px;" class="numRecords">
								<asp:Label ID="lblNumApps" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								aspnet_Profile
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumProfiles" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								aspnet_Roles
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumRoles" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								aspnet_Users
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumUsers" runat="server"></asp:Label>
							</td>
						</tr>
						<tr class="bottomBorder">
							<td class="bottomBorder">
								aspnet_UsersInRoles
							</td>
							<td class="numRecords bottomBorder">
								<asp:Label ID="lblNumUsersInRoles" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td rowspan="6">
								<asp:CheckBox ID="chkImportGalleryData" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Import_Gallery_Data_Checkbox_Text %>"
									Checked="True" />
							</td>
							<td>
								gs_Gallery
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumGalleries" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								gs_Album
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumAlbums" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								gs_MediaObject
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumMediaObjects" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								gs_MediaObjectMetadata
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumMetadata" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								gs_Role_Album
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumRoleAlbums" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								gs_Role
							</td>
							<td class="numRecords">
								<asp:Label ID="lblNumGalleryRoles" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
					<div>
						<div style="height: 80px; float: left; margin-top: 10px; margin-right: 5px;">
							<asp:Button ID="btnRestore" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Restore_Btn_Text %>"
								Enabled="False" OnClick="btnRestore_Click" /></div>
						<p style="width: 400px;">
							<span class="fs msgwarning"><span class="bold">
								<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Restore_Warning_Hdr %>" /></span>&nbsp;<asp:Literal
									ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Backup_Restore_Restore_Tab_Restore_Warning_Dtl %>" /></span></p>
					</div>
				</asp:Panel>
			</CA:PageView>
		</CA:MultiPage>
		<CA:Dialog ID="UploadDialog" runat="server" AllowDrag="true" AllowResize="false" Modal="false" Alignment="MiddleCentre" Width="558"
			Height="347" ContentCssClass="dlg-up" ContentClientTemplateId="UploadContent" />
	</div>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="lblBackupDtl" DialogTitle="<%$ Resources:GalleryServerPro, Admin_BackupRestore_Backup_Overview_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_BackupRestore_Backup_Overview_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="chkExportMembership" DialogTitle="<%$ Resources:GalleryServerPro, Admin_BackupRestore_ExportUserAccounts_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_BackupRestore_ExportUserAccounts_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="chkExportGalleryData" DialogTitle="<%$ Resources:GalleryServerPro, Admin_BackupRestore_ExportGalleryData_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_BackupRestore_ExportGalleryData_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

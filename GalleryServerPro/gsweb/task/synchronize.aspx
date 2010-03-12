<%@ Page Language="C#" MasterPageFile="~/master/taskMstr.master" AutoEventWireup="true" CodeBehind="synchronize.aspx.cs"
	Inherits="GalleryServerPro.Web.task.synchronize" EnableSessionState="false" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.taskMstr" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls" TagPrefix="tis" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<asp:ScriptManagerProxy ID="smp" runat="server">
		<Scripts>
			<asp:ScriptReference Path="~/script/progress.js" />
		</Scripts>
	</asp:ScriptManagerProxy>

	<script type="text/javascript">
	<!--
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(synchPageLoad);
	--></script>

	<div class="addleftpadding1">
		<h3>
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Album_Title_Prefix_Text %>" />&nbsp;<asp:Label
				ID="lblAlbumTitle" runat="server" /></h3>
		<p class="textcol">
			<asp:Label ID="lblInstructions" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Body_Text %>" /></p>
		<div class="addleftpadding6">
			<p>
				<input id="chkIncludeChildAlbums" type="checkbox" /><label for="chkIncludeChildAlbums"><asp:Literal ID="lit1" runat="server"
					Text="<%$ Resources:GalleryServerPro, Task_Synch_IncludeChildAlbums_Text %>" /></label>
				<asp:Label ID="lblIncludeChildAlbums" runat="server" /></p>
			<p>
				<input id="chkOverwriteThumbnails" type="checkbox" /><label for="chkOverwriteThumbnails"><asp:Literal ID="lit2" runat="server"
					Text="<%$ Resources:GalleryServerPro, Task_Synch_OverwriteThumbnails_Text %>" /></label>
				<asp:Label ID="lblOverwriteThumbnails" runat="server" /></p>
			<p>
				<input id="chkOverwriteCompressed" type="checkbox" /><label for="chkOverwriteCompressed"><asp:Literal ID="lit3" runat="server"
					Text="<%$ Resources:GalleryServerPro, Task_Synch_OverwriteCompressed_Text %>" /></label>
				<asp:Label ID="lblOverwriteCompressed" runat="server" /></p>
			<p>
				<asp:CheckBox ID="chkRegenerateMetadata" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_RegenerateMetadata_Text %>" />
				<asp:Label ID="lblRegenerateMetadata" runat="server" />
			</p>
		</div>
	</div>
	<CA:Dialog ID="dgSynch" runat="server" Title="<%$ Resources:GalleryServerPro, Task_Synch_Progress_Title_Text %>" AnimationDirectionElement="btnStart"
		CloseTransition="Fade" ShowTransition="Fade" AnimationSlide="Linear" AnimationType="Live" AnimationPath="Direct" AnimationDuration="400"
		TransitionDuration="400" Icon="pencil.gif" Alignment="MiddleCentre" AllowResize="True" Height="350" Width="540" ContentCssClass="dg4ContentCss"
		HeaderCssClass="dg4HeaderCss" CssClass="dg4DialogCss">
		<HeaderTemplate>
			<div onmousedown="dgSynch.StartDrag(event);">
				<img id="dg0DialogIconImage" src="../images/componentart/dialog/pencil.gif" alt="" />
				<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Progress_Title_Text %>" />
			</div>
		</HeaderTemplate>
		<ContentTemplate>
			<div class="addpadding1">
				<h1 id="synchPopupHeader" style="margin-bottom: 0;">
					<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Progress_SynchInProgress_Hdr %>" /></h1>
				<img id="synchAnimation" src="<%= ThemePath %>/images/synch.gif" alt="" style="display: block; margin: 0 auto 1em auto; width: 272px;
					height: 40px;" />
				<div style="background-color: #cccccc; width: 100%; height: 20px;">
					<div id="progressbar" style="background-color: #336699; height: 20px; width: 1%">
					</div>
				</div>
				<table>
					<tr>
						<td class="bold">
							<asp:Literal ID="lit4" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Progress_ETL_Text %>" />
						</td>
						<td id="synchEtl">
						</td>
					</tr>
					<tr>
						<td class="bold">
							<asp:Literal ID="lit5" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Progress_SynchRate_Text %>" />
						</td>
						<td id="synchRate">
						</td>
					</tr>
					<tr>
						<td class="bold">
							<asp:Literal ID="lit6" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Synch_Progress_Status_Text %>" />
						</td>
						<td id="status">
						</td>
					</tr>
				</table>
				<div id="errorMessage" style="height: 100px; overflow: auto;">
				</div>
				<div class="rightBottomAbsolute">
					<p class="nomargin nopadding">
						<input id="btnCancel" onclick="cancelSynch()" title="Cancel synchronization" type="button" value="Cancel" />
						<input id="btnClose" onclick="closeSynchWindow()" disabled="disabled" title="Close" type="button" value="Close" />
				</div>
			</div>
		</ContentTemplate>
	</CA:Dialog>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="lblIncludeChildAlbums" DialogTitle="<%$ Resources:GalleryServerPro, Task_Synch_IncludeChildAlbums_Hlp_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Task_Synch_IncludeChildAlbums_Hlp_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="lblOverwriteThumbnails" DialogTitle="<%$ Resources:GalleryServerPro, Task_Synch_OverwriteThumbnails_Hlp_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Task_Synch_OverwriteThumbnails_Hlp_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="lblOverwriteCompressed" DialogTitle="<%$ Resources:GalleryServerPro, Task_Synch_OverwriteCompressed_Hlp_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Task_Synch_OverwriteCompressed_Hlp_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="lblRegenerateMetadata" DialogTitle="<%$ Resources:GalleryServerPro, Task_Synch_RegenerateMetadata_Hlp_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Task_Synch_RegenerateMetadata_Hlp_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="lblInstructions" DialogTitle="<%$ Resources:GalleryServerPro, Task_Synch_Options_Hlp_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Task_Synch_Options_Hlp_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

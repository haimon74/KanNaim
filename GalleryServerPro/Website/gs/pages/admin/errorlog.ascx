<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="errorlog.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.admin.errorlog" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Import Namespace="GalleryServerPro.Web" %>
<div id="Div1" runat="server">

	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(errorlogPageLoad);

		function errorlogPageLoad(sender, args)
		{
			configControls();
		}

		function configControls()
		{
			var txtMaxErrItems = $get('<%= txtMaxErrorItems.ClientID %>');
			var chkAutoTrimLog = $get('<%= chkAutoTrimLog.ClientID %>');

			txtMaxErrItems.disabled = (!chkAutoTrimLog.checked || chkAutoTrimLog.disabled);
		}

		function gd_onItemExpand(sender, e)
		{
			gd.select(e.get_item());
		}

		function deleteRow(rowId)
		{
			var row = gd.getItemFromClientId(rowId);
			var id = row.getMemberAt(0).Value;
			gd.deleteItem(row);
			Gsp.Gallery.DeleteAppError(id);
		}

		function chkAutoTrimLog_Click(chk)
		{
			configControls();
		}

	</script>

</div>
<div class="gsp_indentedContent">
	<asp:PlaceHolder ID="phAdminHeader" runat="server" />
	<div class="gsp_addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<asp:Panel ID="pnlOptionsHdr" runat="server">
			<div style="padding: 5px; cursor: pointer; vertical-align: middle;" title='<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_Options_Tooltip %>" />'>
				<asp:Image ID="imgExpCol" ImageUrl="~/gs/images/expand.jpg" runat="server" AlternateText="" />
				<span class="gsp_bold">
					<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_Options_Hdr %>" /></span>
				<asp:Label ID="lblExpCol" runat="server" CssClass="gsp_fs" />
			</div>
		</asp:Panel>
		<asp:Panel ID="pnlOptions" runat="server" Style="overflow: hidden; height: 0;" DefaultButton="btnSave">
			<tis:wwErrorDisplay ID="wwMsgOptions" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False"
				Center="False" Width="500px">
			</tis:wwErrorDisplay>
			<p>
				<asp:CheckBox ID="chkAutoTrimLog" runat="server" onclick="chkAutoTrimLog_Click(this)"
					Text="<%$ Resources:GalleryServerPro, Admin_Error_MaxNumberErrorItems_Lbl %>" /></p>
			<p class="gsp_addleftmargin10">
				<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_MaxNumberErrorItems_Label %>" />:
				<asp:TextBox ID="txtMaxErrorItems" runat="server" />&nbsp;<asp:Button ID="btnSave"
					runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_Options_Save_Lbl %>"
					OnClick="btnSave_Click" />&nbsp;<asp:RangeValidator ID="rvMaxErrorItems" runat="server"
						Display="Dynamic" ControlToValidate="txtMaxErrorItems" Type="Integer" MinimumValue="1"
						MaximumValue="2147483647" Text="<%$ Resources:GalleryServerPro, Validation_Positive_Int_Text %>" /></p>
		</asp:Panel>
		<p>
			<asp:Button ID="btnClearLog" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Error_ClearLog_Lbl %>"
				OnClick="btnClearLog_Click" /></p>
		<div style="width: 98%; height: 100%;">
			<ComponentArt:Grid ID="gd" runat="server" RunningMode="Client" AllowPaging="true"
				SearchText="<%$ Resources:GalleryServerPro, Site_Grid_Search_Label %>" PageSize="10"
				CssClass="gdErr0Grid" ShowHeader="True" ShowSearchBox="true" AutoFocusSearchBox="true"
				SearchTextCssClass="gdErr0GridHeaderText" SearchOnKeyPress="True" HeaderCssClass="gdErr0GridHeader"
				FooterCssClass="gdErr0GridFooter" GroupingNotificationText="" AllowHorizontalScrolling="false"
				FillContainer="true" AllowTextSelection="true" EditOnClickSelectedItem="false">
				<ClientEvents>
					<ItemExpand EventHandler="gd_onItemExpand" />
				</ClientEvents>
				<Levels>
					<ComponentArt:GridLevel DataKeyField="AppErrorId" DataMember="AppErrors" AllowGrouping="False"
						TableHeadingClientTemplateId="" AllowReordering="false" RowCssClass="gdErr0Row"
						DataCellCssClass="gdErr0DataCell" HeadingCellCssClass="gdErr0HeadingCell" TableHeadingCssClass="gdErr0TableHeader"
						ShowTableHeading="false" HeadingCellHoverCssClass="gdErr0HeadingCellHover" HeadingCellActiveCssClass="gdErr0HeadingCellActive"
						HeadingRowCssClass="gdErr0HeadingRow" HeadingTextCssClass="gdErr0HeadingCellText"
						SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
						SortImageHeight="19" ShowSelectorCells="false" SelectedRowCssClass="gdErr0SelectedRow">
						<Columns>
							<ComponentArt:GridColumn DataField="AppErrorId" Visible="false" />
							<ComponentArt:GridColumn DataCellClientTemplateId="ctOptions" DataCellCssClass="gdErr0DataCellOptions"
								Width="25" FixedWidth="true" />
							<ComponentArt:GridColumn DataField="TimeStamp" HeadingText="<%$ Resources:GalleryServerPro, Admin_Error_Grid_Timestamp_Hdr %>"
								Width="175" DataCellCssClass="gdErr0DataCellTimestamp" />
							<ComponentArt:GridColumn DataField="ExceptionType" HeadingText="<%$ Resources:GalleryServerPro, Admin_Error_Grid_Type_Hdr %>" />
							<ComponentArt:GridColumn DataField="Message" TextWrap="true" HeadingText="<%$ Resources:GalleryServerPro, Admin_Error_Grid_Message_Hdr %>" />
						</Columns>
					</ComponentArt:GridLevel>
					<ComponentArt:GridLevel DataKeyField="Name" DataMember="AppErrorItems" AllowGrouping="False"
						ShowHeadingCells="false" TableHeadingClientTemplateId="ctErrDetails" AllowReordering="false"
						RowCssClass="gdErr1Row" HeadingCellCssClass="gdErr1HeadingCell" TableHeadingCssClass="gdErr1TableHeader"
						ShowTableHeading="true" HeadingCellHoverCssClass="gdErr1HeadingCellHover" HeadingCellActiveCssClass="gdErr1HeadingCellActive"
						HeadingRowCssClass="gdErr1HeadingRow" HeadingTextCssClass="gdErr1HeadingCellText"
						SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
						SortImageHeight="19">
						<Columns>
							<ComponentArt:GridColumn DataField="Name" DataCellCssClass="gdErr1Column1" />
							<ComponentArt:GridColumn DataField="Value" DataCellCssClass="gdErr1Column2" />
						</Columns>
					</ComponentArt:GridLevel>
				</Levels>
			</ComponentArt:Grid>
		</div>
	</div>
	<asp:PlaceHolder ID="phAdminFooter" runat="server" />
	<tis:wwDataBinder ID="wwDataBinder" runat="server" OnBeforeUnbindControl="wwDataBinder_BeforeUnbindControl"
		OnAfterBindControl="wwDataBinder_AfterBindControl" OnValidateControl="wwDataBinder_ValidateControl">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="bi1" runat="server" BindingSource="CoreConfig" BindingSourceMember="MaxNumberErrorItems"
				ControlId="txtMaxErrorItems" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Error_MaxNumberErrorItems_Label %>" />
			<tis:wwDataBindingItem runat="server" ControlId="chkAutoTrimLog">
			</tis:wwDataBindingItem>
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="poi1" runat="server" ControlId="chkAutoTrimLog"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_maxNumberErrorItems_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_maxNumberErrorItems_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<cc1:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="pnlOptions"
		CollapsedSize="0" Collapsed="True" ExpandControlID="pnlOptionsHdr" CollapseControlID="pnlOptionsHdr"
		AutoCollapse="False" AutoExpand="False" ScrollContents="False" ImageControlID="imgExpCol"
		ExpandDirection="Vertical" TextLabelID="lblExpCol" ExpandedText="<%$ Resources:GalleryServerPro, Admin_Error_Options_ExpandedText_Label %>"
		CollapsedText="<%$ Resources:GalleryServerPro, Admin_Error_Options_CollapsedText_Label %>" />
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mediaobjectview.ascx.cs"
	Inherits="GalleryServerPro.Web.uc.mediaobjectview" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:ScriptManagerProxy ID="smp" runat="server">
	<Scripts>
		<asp:ScriptReference Path="~/script/simpletimer.js" />
		<asp:ScriptReference Path="~/script/mediaobjectview.js" />
		<asp:ScriptReference Path="~/script/SilverlightControl.js" />
		<asp:ScriptReference Path="~/script/SilverlightMedia.js" />
	</Scripts>
</asp:ScriptManagerProxy>

<script type="text/javascript">
<!-- 
	Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(moViewPageLoad);
-->
</script>

<div id="divMoView" style="width: <% =MediaObjectContainerWidth %>px;">
	<table id="toolbarContainer" style="width: <% = MediaObjectContainerWidth.ToString() %>px;">
		<tr>
			<td style="width: 42px;">
				<asp:Image ID="imgPrevious" runat="server" ImageUrl="~/images/componentart/toolbar/play.png"
					AlternateText="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Previous_Text %>"
					ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Previous_Tooltip %>"
					CssClass="navleft addpadding2" onclick="showPrevMediaObject();" /></td>
			<td id="toolbarCell" style="width: 100%;">
				<CA:ToolBar ID="tbMediaObjectActions" runat="server" EnableViewState="true" AutoPostBackOnSelect="false"
					AutoPostBackOnCheckChanged="false" ImagesBaseUrl="~/images/componentart/toolbar/"
					DefaultItemTextImageSpacing="2" DefaultItemTextImageRelation="ImageOnly" DefaultItemImageHeight="16"
					DefaultItemImageWidth="16" ItemSpacing="1" Orientation="Horizontal" UseFadeEffect="false"
					OnItemCommand="tbMediaObjectActions_ItemCommand" CssClass="toolbar" DefaultItemCssClass="item"
					DefaultItemHoverCssClass="itemHover" DefaultItemActiveCssClass="itemActive" DefaultItemCheckedCssClass="itemChecked"
					DefaultItemCheckedHoverCssClass="itemActive">
					<ClientEvents>
						<ItemSelect EventHandler="tbMediaObjectActions_onItemSelect" />
						<ItemCheckChange EventHandler="tbMediaObjectActions_onItemCheckChange" />
					</ClientEvents>
				</CA:ToolBar>
			</td>
			<td style="text-align: right; white-space: nowrap;">
				<asp:PlaceHolder ID="phPosition" runat="server" />
			</td>
			<td style="width: 42px;">
				<asp:Image ID="imgNext" runat="server" ImageUrl="~/images/componentart/toolbar/play.png"
					AlternateText="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Next_Text %>"
					ToolTip="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Next_Tooltip %>" CssClass="navright addpadding2"
					onclick="showNextMediaObject();" /></td>
		</tr>
	</table>
	<div id="divPermalink" class="permalinkContainer invisible">
		<p>
			<asp:Literal ID="litPermalinkHeaderText" runat="server" Text="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Permalink_Header_Text %>" /></p>
		<p id="permaLinkUrlTag" class="fs">
		</p>
	</div>
	<div id="mediaObjectInfoContainer">
	</div>
	<asp:Panel ID="pnlMediaObject" runat="server" CssClass="moContainer" />
	<asp:Panel ID="pnlMediaObjectTitle" runat="server" Style="clear: both;">
		<div id="moTitle" runat="server" class="mediaObjectTitle">
		</div>
	</asp:Panel>
</div>
<CA:Dialog ID="dgMediaObjectInfo" runat="server" Title="Details" RenderOverWindowedObjects="true"
	AnimationType="Live" AnimationDirectionElement="toolbarCell" AlignmentElement="mediaObjectInfoContainer"
	CloseTransition="Fade" ShowTransition="Fade" AnimationSlide="Linear" AnimationPath="Direct"
	AnimationDuration="400" TransitionDuration="400" Icon="info.png" AllowResize="True"
	ContentCssClass="dg2ContentCss" HeaderCssClass="dg2HeaderCss" CssClass="dg2DialogCss"
	OffsetX="5" Width="400" Height="480">
	<HeaderTemplate>
		<div onmousedown="dgMediaObjectInfo.StartDrag(event);">
			<img id="dg0DialogCloseImage" onclick="dgMediaObjectInfo.Close('cancelled');" src="images/componentart/dialog/close.gif"
				style="width: 28px; height: 15px;" alt="Close" /><img id="dg0DialogIconImage" src="images/componentart/dialog/info.png"
					style="width: 16px; height: 16px;" alt="" />
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Info_Dialog_Title_Text %>" />
		</div>
	</HeaderTemplate>
	<ContentTemplate>
		<CA:Grid ID="gdmeta" runat="server" RunningMode="Client" ImagesBaseUrl="~/images/componentart/grid/"
			FillContainer="true" ColumnResizeDistributeWidth="true" AllowPaging="false" ShowFooter="false"
			EmptyGridText="No data available" CssClass="gdInfoGrid" EnableViewState="false">
			<ClientEvents>
				<Load EventHandler="gdmeta_onLoad" />
			</ClientEvents>
			<Levels>
				<CA:GridLevel DataKeyField="MediaObjectMetadataId" AllowGrouping="False" AllowReordering="true"
					ColumnReorderIndicatorImageUrl="reorder.gif" HeadingCellCssClass="gdInfoHeadingCell"
					HeadingRowCssClass="gdInfoHeadingRow" DataCellCssClass="gdInfoDataCell" SortAscendingImageUrl="asc.gif"
					SortDescendingImageUrl="desc.gif" SortImageWidth="10" SortImageHeight="10">
					<Columns>
						<CA:GridColumn DataField="Description" HeadingCellCssClass="gdInfoFirstHeadingCell" HeadingText="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Info_Dialog_Description_Header_Text %>"
							DataCellCssClass="gdInfoFirstDataCell" TextWrap="true" AllowReordering="False" />
						<CA:GridColumn DataField="Value" TextWrap="true" AllowReordering="False" HeadingText="<%$ Resources:GalleryServerPro, UC_MediaObjectView_Info_Dialog_Value_Header_Text %>" />
					</Columns>
				</CA:GridLevel>
			</Levels>
		</CA:Grid>
	</ContentTemplate>
	<ClientEvents>
		<OnClose EventHandler="dgMediaObjectInfo_OnClose" />
	</ClientEvents>
</CA:Dialog>
<asp:PlaceHolder ID="phDialogContainer" runat="server" />
<cc1:AnimationExtender runat="server" ID="dummyAnimator" TargetControlID="pnlMediaObject" />
<CA:CallBack ID="cbDummy" runat="server" />
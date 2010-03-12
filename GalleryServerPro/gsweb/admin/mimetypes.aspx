<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="mimetypes.aspx.cs" Inherits="GalleryServerPro.Web.admin.mimetypes" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">

	<script type="text/javascript">
	<!--
	var chkAllCheckboxIsChecked = false;
	var sortDesc = false;
	
	function setGridCheck( columnNumber, checked )
	{
		var gridItem;
		var itemIndex = 0;
		chkAllCheckboxIsChecked = checked;
		while(gridItem = gdMimeTypes.Table.GetRow(itemIndex))
		{
			gridItem.SetValue( columnNumber, checked );
			itemIndex++;
		}

		gdMimeTypes.Render();
	} 
	--></script>

	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False"
			Center="False" Width="500px" />
		<p>
			<asp:CheckBox ID="chkAllowAll" runat="server" Text="Allow any file type, regardless of whether it is listed below" />
		</p>
		<div style="width: 98%;height:100%;">
			<CA:Grid ID="gdMimeTypes" runat="server" RunningMode="Client" ImagesBaseUrl="~/images/componentart/grid/"
				AllowPaging="true" SearchText="Search:" PageSize="500" CssClass="gd0Grid" ShowHeader="True"
				ShowSearchBox="True" SearchTextCssClass="gd0GridHeaderText" SearchOnKeyPress="True"
				HeaderCssClass="gd0GridHeader" FooterCssClass="gd0GridFooter" GroupingNotificationText=""
				AllowHorizontalScrolling="true" FillContainer="true">
				<Levels>
					<CA:GridLevel DataKeyField="FileExtension" DataMember="none" AllowGrouping="False"
						TableHeadingClientTemplateId="enabledHeader" AllowReordering="false" RowCssClass="gd0Row"
						DataCellCssClass="gd0DataCell" HeadingCellCssClass="gd0HeadingCell" TableHeadingCssClass="gd0TableHeader"
						ShowTableHeading="true" HeadingCellHoverCssClass="gd0HeadingCellHover" HeadingCellActiveCssClass="gd0HeadingCellActive"
						HeadingRowCssClass="gd0HeadingRow" HeadingTextCssClass="gd0HeadingCellText" SortAscendingImageUrl="asc.gif"
						SortDescendingImageUrl="desc.gif" SortImageWidth="10" SortImageHeight="19">
						<Columns>
							<CA:GridColumn DataField="AllowAddToGallery" ColumnType="checkBox" HeadingText="Enabled?"
								AllowEditing="true" Width="100" Align="center" />
							<CA:GridColumn DataField="Extension" HeadingText="File Extension" Width="175" />
							<CA:GridColumn DataField="FullType" HeadingText="MIME Type" Width="600" />
						</Columns>
					</CA:GridLevel>
				</Levels>
			</CA:Grid>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="AllowUnspecifiedMimeTypes" ControlId="chkAllowAll" BindingProperty="Checked"
				UserFieldName="Allow all file types" />
		</DataBindingItems>
	</tis:wwDataBinder>
		<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="poi1" runat="server" ControlId="lblAdminPageHeader"
				DialogTitle="<%$ Resources:GalleryServerPro, Admin_MimeTypes_Overview_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_MimeTypes_Overview_Bdy %>" />
			<tis:PopupInfoItem ID="poi2" runat="server" ControlId="chkAllowAll"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_allowUnspecifiedMimeTypes_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_allowUnspecifiedMimeTypes_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />

</asp:Content>

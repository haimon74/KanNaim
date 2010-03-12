<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="images.aspx.cs" Inherits="GalleryServerPro.Web.admin.images" Title="Gallery Server Pro - Image Settings" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<h3>
			<span id="compressedHdr" runat="server">
				<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_Compressed_Hdr %>" />
			</span>
		</h3>
		<div class="addleftpadding6">
			<p class="addtopmargin5">
				<asp:CheckBox ID="chkShowOriginal" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_ShowOriginal_Label %>" />
			</p>
			<table class="addtopmargin5 standardTable">
				<tr>
					<td>
						<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_OptTriggerSize_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtOptTriggerSize" runat="server" />
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_OptMaxLength_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtOptMaxLength" runat="server" />
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_OptJpegQuality_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtOptJpegQuality" runat="server" /></td>
					<td class="fs">
						<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_OptJpegQuality_Label2 %>" />:
					</td>
				</tr>
			</table>
		</div>
		<h3>
			<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_Original_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<table class="standardTable">
				<tr>
					<td>
						<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_OriginalJpegQuality_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtOriginalJpegQuality" runat="server" /></td>
					<td class="fs">
						<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_OriginalJpegQuality_Label2 %>" />
					</td>
				</tr>
			</table>
		</div>
		<h3>
			<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_Watermark_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p>
				<asp:CheckBox ID="chkApplyWatermark" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_ApplyWatermark_Label %>" />
			</p>
			<p class="addleftpadding6">
				<asp:CheckBox ID="chkApplyWmkToThumb" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_ApplyWmkToThumb_Label %>" />
			</p>
			<p class="bold addtopmargin5">
				<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_Watermark_Text_Hdr %>" />
			</p>
			<table class="addleftpadding6 standardTable">
				<tr>
					<td>
						<asp:Literal ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkText_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkText" runat="server" class="textbox" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l11" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkFontName_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkFontName" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l12" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkFontSize_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkFontSize" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l13" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkTextWidthPct_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkTextWidthPct" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l14" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkFontColor_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkFontColor" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l15" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkTextOpacity_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkTextOpacity" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l16" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkTextLocation_Label %>" />:
					</td>
					<td>
						<asp:DropDownList ID="ddlWmkTextLocation" runat="server" />
					</td>
				</tr>
			</table>
			<p class="bold addtopmargin5">
				<asp:Literal ID="l17" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_Watermark_Image_Hdr %>" />
			</p>
			<table class="addleftpadding6 standardTable">
				<tr>
					<td>
						<asp:Literal ID="l18" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkImagePath_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkImagePath" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l19" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkImageWidthPct_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkImageWidthPct" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l20" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkImageOpacity_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtWmkImageOpacity" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l21" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Images_WmkImageLocation_Label %>" />:
					</td>
					<td>
						<asp:DropDownList ID="ddlWmkImageLocation" runat="server" />
					</td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="ThumbnailClickShowsOriginal" ControlId="chkShowOriginal" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_ShowOriginal_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="OptimizedImageTriggerSizeKB" ControlId="txtOptTriggerSize"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_OptTriggerSize_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="MaxOptimizedLength" ControlId="txtOptMaxLength" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_OptMaxLength_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="OptimizedImageJpegQuality" ControlId="txtOptJpegQuality" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_OptJpegQuality_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="OriginalImageJpegQuality" ControlId="txtOriginalJpegQuality"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_OriginalJpegQuality_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="ApplyWatermark" ControlId="chkApplyWatermark" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_ApplyWatermark_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="ApplyWatermarkToThumbnails" ControlId="chkApplyWmkToThumb"
				BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_ApplyWmkToThumb_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem8" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkText" ControlId="txtWmkText" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkText_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem9" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkTextFontName" ControlId="txtWmkFontName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkFontName_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem10" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkTextFontSize" ControlId="txtWmkFontSize" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkFontSize_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem11" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkTextWidthPercent" ControlId="txtWmkTextWidthPct"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkTextWidthPct_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem12" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkTextColor" ControlId="txtWmkFontColor" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkFontColor_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem13" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkTextOpacityPercent" ControlId="txtWmkTextOpacity"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkTextOpacity_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem14" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkTextLocation" ControlId="ddlWmkTextLocation" BindingProperty="SelectedValue"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkTextLocation_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem15" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkImagePath" ControlId="txtWmkImagePath" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkImagePath_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem16" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkImageWidthPercent" ControlId="txtWmkImageWidthPct"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkImageWidthPct_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem17" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="WatermarkImageOpacityPercent" ControlId="txtWmkImageOpacity"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkImageOpacity_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem18" runat="server" BindingProperty="SelectedValue"
				BindingSource="Master.CoreConfig" BindingSourceMember="WatermarkImageLocation"
				ControlId="ddlWmkImageLocation" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Images_WmkImageLocation_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="chkShowOriginal"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_thumbnailClickShowsOriginal_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_thumbnailClickShowsOriginal_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="txtOptTriggerSize"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_optimizedImageTriggerSizeKB_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_optimizedImageTriggerSizeKB_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="txtOptMaxLength"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_maxOptimizedLength_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_maxOptimizedLength_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="txtOptJpegQuality"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_optimizedImageJpegQuality_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_optimizedImageJpegQuality_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="txtOriginalJpegQuality"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_originalImageJpegQuality_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_originalImageJpegQuality_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="chkApplyWatermark"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_applyWatermark_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_applyWatermark_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="chkApplyWmkToThumb"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_applyWatermarkToThumbnails_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_applyWatermarkToThumbnails_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem8" runat="server" ControlId="txtWmkText" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkText_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkText_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem9" runat="server" ControlId="txtWmkFontName"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkTextFontName_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkTextFontName_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem10" runat="server" ControlId="txtWmkFontSize"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkTextFontSize_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkTextFontSize_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem11" runat="server" ControlId="txtWmkTextWidthPct"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkTextWidthPercent_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkTextWidthPercent_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem12" runat="server" ControlId="txtWmkFontColor"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkTextColor_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkTextColor_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem13" runat="server" ControlId="txtWmkTextOpacity"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkTextOpacityPercent_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkTextOpacityPercent_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem14" runat="server" ControlId="ddlWmkTextLocation"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkTextLocation_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkTextLocation_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem15" runat="server" ControlId="txtWmkImagePath"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkImagePath_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkImagePath_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem16" runat="server" ControlId="txtWmkImageWidthPct"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkImageWidthPercent_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkImageWidthPercent_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem17" runat="server" ControlId="txtWmkImageOpacity"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkImageOpacityPercent_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkImageOpacityPercent_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem18" runat="server" ControlId="ddlWmkImageLocation"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_watermarkImageLocation_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_watermarkImageLocation_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem19" runat="server" ControlId="compressedHdr"
				DialogTitle="<%$ Resources:GalleryServerPro, Admin_Images_About_Compressed_Images_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Admin_Images_About_Compressed_Images_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true" CodeBehind="mediaobjects.aspx.cs"
	Inherits="GalleryServerPro.Web.admin.mediaobjects" Title="Gallery Server Pro - Media Object Settings" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls" TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<h3>
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_Storage_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p class="bold">
				<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_Original_Storage_Label %>" />
			</p>
			<div class="addleftpadding6">
				<p class="addtopmargin5">
					<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_MoPath_Label %>" />:
				</p>
				<p class="nomargin nopadding">
					<asp:TextBox ID="txtMoPath" runat="server" CssClass="textbox" />
				</p>
				<p class="addtopmargin5">
					<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_CurrentMoPath_Label %>" />:
				</p>
				<p class="addleftpadding6">
					<asp:Label ID="lblMoPath" runat="server" CssClass="msgfriendly" /></p>
			</div>
			<p class="bold">
				<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_Thumbnail_Storage_Label %>" />
			</p>
			<div class="addleftpadding6">
				<p>
					<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_ThumbnailCachePath_Label %>" />:
				</p>
				<p class="nomargin nopadding">
					<asp:TextBox ID="txtThumbnailCachePath" runat="server" CssClass="textbox" />
				</p>
				<p class="addtopmargin5">
					<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_CurrentThumbnailCachePath_Label %>" />:
				</p>
				<p class="addleftpadding6">
					<asp:Label ID="lblThumbnailCachePath" runat="server" CssClass="msgfriendly" /></p>
			</div>
			<p class="bold">
				<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_Compressed_Storage_Label %>" />
			</p>
			<div class="addleftpadding6">
				<p>
					<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_OptimizedCachePath_Label %>" />:
				</p>
				<p class="nomargin nopadding">
					<asp:TextBox ID="txtOptimizedCachePath" runat="server" CssClass="textbox" />
				</p>
				<p class="addtopmargin5">
					<asp:Literal ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_CurrentOptimizedCachePath_Label %>" />:
				</p>
				<p class="addleftpadding6">
					<asp:Label ID="lblOptimizedCachePath" runat="server" CssClass="msgfriendly" /></p>
			</div>
		</div>
		<h3 class="addtoppadding5">
			<asp:Literal ID="l11" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TransitionEffects_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p>
				<asp:Literal ID="l12" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TransType_Label %>" />:
				<asp:DropDownList ID="ddlTransType" runat="server" />
			</p>
			<p class="addtopmargin5">
				<asp:Literal ID="l13" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TransDuration_Label %>" />:
				<asp:TextBox ID="txtTransDuration" runat="server" />
			</p>
		</div>
		<h3 class="addtoppadding5">
			<asp:Literal ID="l17" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_Upload_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p class="addtopmargin5">
				<asp:CheckBox ID="chkAllowAddLocalContent" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_AllowAddLocalContent_Label %>" />
			</p>
			<p>
				<asp:CheckBox ID="chkAllowAddExternalContent" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_AllowAddExternalContent_Label %>" />
			</p>
			<p class="addtopmargin5">
				<asp:Literal ID="l18" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_MaxUploadSize_Label %>" />:
			</p>
			<p class="nomargin nopadding">
				<asp:TextBox ID="txtMaxUploadSize" runat="server" />
			</p>
		</div>
		<h3 class="addtoppadding5">
			<asp:Literal ID="l14" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_Thumbnail_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p>
				<asp:Literal ID="l15" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_MaxThumbnailLength_Label %>" />:
			</p>
			<p class="nomargin nopadding">
				<asp:TextBox ID="txtMaxThumbnailLength" runat="server" /></p>
			<p>
				<asp:Literal ID="l16" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TitleDisplayLength_Label %>" />:
			</p>
			<p class="nomargin nopadding">
				<asp:TextBox ID="txtTitleDisplayLength" runat="server" /></p>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server" OnValidateControl="wwDataBinder_ValidateControl">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="MediaObjectPath"
				ControlId="txtMoPath" IsRequired="True" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_MoPath_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="ThumbnailPath"
				ControlId="txtThumbnailCachePath" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_ThumbnailCachePath_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="OptimizedPath"
				ControlId="txtOptimizedCachePath" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_OptimizedCachePath_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="MediaObjectTransitionType"
				ControlId="ddlTransType" BindingProperty="SelectedValue" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TransType_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="MediaObjectTransitionDuration"
				ControlId="txtTransDuration" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TransDuration_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="MaxThumbnailLength"
				ControlId="txtMaxThumbnailLength" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_MaxThumbnailLength_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="MaxMediaObjectThumbnailTitleDisplayLength"
				ControlId="txtTitleDisplayLength" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_TitleDisplayLength_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem8" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="AllowAddLocalContent"
				ControlId="chkAllowAddLocalContent" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_AllowAddLocalContent_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem9" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="AllowAddExternalContent"
				ControlId="chkAllowAddExternalContent" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_AllowAddExternalContent_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem10" runat="server" BindingSource="Master.CoreConfig" BindingSourceMember="MaxUploadSize"
				ControlId="txtMaxUploadSize" UserFieldName="<%$ Resources:GalleryServerPro, Admin_MediaObjects_MaxUploadSize_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="txtMoPath" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_mediaObjectPath_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_mediaObjectPath_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="txtThumbnailCachePath" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_thumbnailPath_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_thumbnailPath_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="txtOptimizedCachePath" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_optimizedPath_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_optimizedPath_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="ddlTransType" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_mediaObjectTransitionType_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_mediaObjectTransitionType_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="txtTransDuration" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_mediaObjectTransitionDuration_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_mediaObjectTransitionDuration_Bdy %>" />
				
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="chkAllowAddLocalContent" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_AllowAddLocalContent_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_AllowAddLocalContent_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="chkAllowAddExternalContent" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_AllowAddExternalContent_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_AllowAddExternalContent_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem8" runat="server" ControlId="txtMaxUploadSize" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_MaxUploadSize_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_MaxUploadSize_Bdy %>" />

			<tis:PopupInfoItem ID="PopupInfoItem9" runat="server" ControlId="txtMaxThumbnailLength" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_maxThumbnailLength_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_maxThumbnailLength_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem10" runat="server" ControlId="txtTitleDisplayLength" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_maxMediaObjectThumbnailTitleDisplayLength_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_maxMediaObjectThumbnailTitleDisplayLength_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

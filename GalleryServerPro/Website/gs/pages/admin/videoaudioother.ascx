<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="videoaudioother.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.admin.videoaudioother" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"	TagPrefix="tis" %>
<div class="gsp_indentedContent">
	<asp:PlaceHolder ID="phAdminHeader" runat="server" />
		<div class="gsp_addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<p class="admin_h3" style="margin-top: 0;">
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_General_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<p>
				<asp:CheckBox ID="chkAutoStart" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AutoStart_Label %>" />
			</p>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoSettings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblVideoPlayerWidth" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerWidth_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtVideoPlayerWidth" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblVideoPlayerHeight" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerHeight_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtVideoPlayerHeight" runat="server" /></td>
				</tr>
			</table>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioSettings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblAudioPlayerWidth" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerWidth_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtAudioPlayerWidth" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblAudioPlayerHeight" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerHeight_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtAudioPlayerHeight" runat="server" /></td>
				</tr>
			</table>
		</div>
		<p class="admin_h3">
			<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_OtherSettings_Hdr %>" />
		</p>
		<div class="gsp_addleftpadding6">
			<table class="gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblGenericWidth" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericWidth_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtGenericWidth" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblGenericHeight" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericHeight_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtGenericHeight" runat="server" /></td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="AutoStartMediaObject" ControlId="chkAutoStart" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AutoStart_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="DefaultVideoPlayerWidth" ControlId="txtVideoPlayerWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerWidth_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="DefaultVideoPlayerHeight" ControlId="txtVideoPlayerHeight"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerHeight_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="DefaultAudioPlayerWidth" ControlId="txtAudioPlayerWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerWidth_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="DefaultAudioPlayerHeight" ControlId="txtAudioPlayerHeight"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerHeight_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="DefaultGenericObjectWidth" ControlId="txtGenericWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericWidth_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="DefaultGenericObjectHeight" ControlId="txtGenericHeight" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericHeight_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="chkAutoStart" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_autoStartMediaObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_autoStartMediaObject_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="lblVideoPlayerWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerWidth_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="lblVideoPlayerHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerHeight_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="lblAudioPlayerWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerWidth_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="lblAudioPlayerHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerHeight_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="lblGenericWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectWidth_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="lblGenericHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectHeight_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:PlaceHolder ID="phAdminFooter" runat="server" />
</div>
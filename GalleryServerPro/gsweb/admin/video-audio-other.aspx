<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="video-audio-other.aspx.cs" Inherits="GalleryServerPro.Web.admin.video_audio_other" %>

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
			<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_General_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<p>
				<asp:CheckBox ID="chkAutoStart" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AutoStart_Label %>" />
			</p>
		</div>
		<h3>
			<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoSettings_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<table class="standardTable">
				<tr>
					<td>
						<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerWidth_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtVideoPlayerWidth" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerHeight_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtVideoPlayerHeight" runat="server" /></td>
				</tr>
			</table>
		</div>
		<h3>
			<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioSettings_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<table class="standardTable">
				<tr>
					<td>
						<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerWidth_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtAudioPlayerWidth" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerHeight_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtAudioPlayerHeight" runat="server" /></td>
				</tr>
			</table>
		</div>
		<h3>
			<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_OtherSettings_Hdr %>" />
		</h3>
		<div class="addleftpadding6">
			<table class="standardTable">
				<tr>
					<td>
						<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericWidth_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtGenericWidth" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericHeight_Label %>" />:
					</td>
					<td>
						<asp:TextBox ID="txtGenericHeight" runat="server" /></td>
				</tr>
			</table>
		</div>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="AutoStartMediaObject" ControlId="chkAutoStart" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AutoStart_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="DefaultVideoPlayerWidth" ControlId="txtVideoPlayerWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerWidth_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="DefaultVideoPlayerHeight" ControlId="txtVideoPlayerHeight"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_VideoPlayerHeight_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="DefaultAudioPlayerWidth" ControlId="txtAudioPlayerWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerWidth_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="DefaultAudioPlayerHeight" ControlId="txtAudioPlayerHeight"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_AudioPlayerHeight_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="DefaultGenericObjectWidth" ControlId="txtGenericWidth" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericWidth_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="DefaultGenericObjectHeight" ControlId="txtGenericHeight" UserFieldName="<%$ Resources:GalleryServerPro, Admin_VidAudOther_GenericHeight_Label %>" />
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="chkAutoStart" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_autoStartMediaObject_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_autoStartMediaObject_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="txtVideoPlayerWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerWidth_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="txtVideoPlayerHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultVideoPlayerHeight_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="txtAudioPlayerWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerWidth_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="txtAudioPlayerHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultAudioPlayerHeight_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="txtGenericWidth"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectWidth_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectWidth_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="txtGenericHeight"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectHeight_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_defaultGenericObjectHeight_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

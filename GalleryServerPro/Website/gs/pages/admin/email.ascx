<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="email.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.admin.email" %>
<%@ Register Src="../../Controls/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<div class="gsp_indentedContent">
	<asp:PlaceHolder ID="phAdminHeader" runat="server" />
	<div class="gsp_addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<div class="gsp_addleftpadding6">
			<p class="gsp_bold">
				<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_To_Hdr %>" />
			</p>
			<table class="gsp_addleftpadding6 gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblEmailTo" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailToName" runat="server" CssClass="gsp_textbox" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblEmailToAddress" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailToAddress" runat="server" CssClass="gsp_textbox" />
					</td>
				</tr>
			</table>
			<p class="gsp_addtopmargin5 gsp_bold">
				<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_From_Hdr %>" />
			</p>
			<table class="gsp_addleftpadding6 gsp_standardTable">
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblEmailFromName" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailFromName" runat="server" CssClass="gsp_textbox" />
					</td>
				</tr>
				<tr>
					<td class="gsp_col1">
						<asp:Label ID="lblEmailFromAddress" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailFromAddress" runat="server" CssClass="gsp_textbox" />
					</td>
				</tr>
			</table>
		</div>
		<table class="gsp_addtopmargin5 gsp_standardTable">
			<tr>
				<td class="gsp_col1">
					<asp:Label ID="lblSmtpServer" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_SmtpServer_Label %>" />
				</td>
				<td>
					<asp:TextBox ID="txtSmtpServer" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="gsp_col1">
					<asp:Label ID="lblSmtpPort" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_SmtpPort_Label %>" />
				</td>
				<td>
					<asp:TextBox ID="txtSmtpPort" runat="server" />
				</td>
			</tr>
		</table>
		<p class="gsp_addtopmargin5">
			<asp:CheckBox ID="chkUseSsl" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_UseSsl_Label %>" />
		</p>
		<p class="gsp_addleftpadding6">
			<asp:Button ID="btnEmailTest" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_Sent_Test_Email_Text %>" OnClick="btnEmailTest_Click" /></p>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="wbi1" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmailToName" ControlId="txtEmailToName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
			<tis:wwDataBindingItem ID="wbi2" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmailToAddress" ControlId="txtEmailToAddress" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
			<tis:wwDataBindingItem ID="wbi3" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmailFromName" ControlId="txtEmailFromName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
			<tis:wwDataBindingItem ID="wbi4" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="EmailFromAddress" ControlId="txtEmailFromAddress" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
			<tis:wwDataBindingItem ID="wbi5" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="SmtpServer" ControlId="txtSmtpServer" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_SmtpServer_Label %>" />
			<tis:wwDataBindingItem ID="wbi6" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="SmtpServerPort" ControlId="txtSmtpPort" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_SmtpPort_Label %>" />
			<tis:wwDataBindingItem ID="wbi7" runat="server" BindingSource="CoreConfig"
				BindingSourceMember="SendEmailUsingSsl" ControlId="chkUseSsl" BindingProperty="Checked" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_UseSsl_Label %>" />
			<tis:wwDataBindingItem ID="wbi8" runat="server" ControlId="btnEmailTest">
			</tis:wwDataBindingItem>
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="poi1" runat="server" ControlId="lblEmailTo"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailToName_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailToName_Bdy %>" />
			<tis:PopupInfoItem ID="poi2" runat="server" ControlId="lblEmailToAddress"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailToAddress_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailToAddress_Bdy %>" />
			<tis:PopupInfoItem ID="poi3" runat="server" ControlId="lblEmailFromName"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailFromName_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailFromName_Bdy %>" />
			<tis:PopupInfoItem ID="poi4" runat="server" ControlId="lblEmailFromAddress"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailFromAddress_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailFromAddress_Bdy %>" />
			<tis:PopupInfoItem ID="poi5" runat="server" ControlId="lblSmtpServer" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_smtpServer_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_smtpServer_Bdy %>" />
			<tis:PopupInfoItem ID="poi6" runat="server" ControlId="lblSmtpPort" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_smtpPort_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_smtpPort_Bdy %>" />
			<tis:PopupInfoItem ID="poi7" runat="server" ControlId="chkUseSsl" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_sendEmailUsingSsl_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_sendEmailUsingSsl_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
	<asp:PlaceHolder ID="phAdminFooter" runat="server" />
</div>

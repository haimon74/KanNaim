<%@ Page Language="C#" MasterPageFile="~/master/adminMstr.master" AutoEventWireup="true"
	CodeBehind="email.aspx.cs" Inherits="GalleryServerPro.Web.admin.email" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.adminMstr" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<div class="addpadding1">
		<tis:wwErrorDisplay ID="wwMessage" runat="server" UserMessage="<%$ Resources:GalleryServerPro, Validation_Summary_Text %>"
			CellPadding="2" UseFixedHeightWhenHiding="False" Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<p>
			<asp:CheckBox ID="chkSendEmail" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_SendEmail_Label %>" />
		</p>
		<p class="addleftpadding6">
			<asp:Button ID="btnEmailTest" runat="server" Text="Send test email" OnClick="btnEmailTest_Click" /></p>
		<div class="addleftpadding6">
			<p class="addtopmargin5">
				<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_To_Hdr %>" />
			</p>
			<table class="addleftpadding6 standardTable">
				<tr>
					<td>
						<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailToName" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailToAddress" runat="server" CssClass="textbox" />
					</td>
				</tr>
			</table>
			<p class="addtopmargin5">
				<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_From_Hdr %>" />
			</p>
			<table class="addleftpadding6 standardTable">
				<tr>
					<td>
						<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailFromName" runat="server" CssClass="textbox" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
					</td>
					<td>
						<asp:TextBox ID="txtEmailFromAddress" runat="server" CssClass="textbox" />
					</td>
				</tr>
			</table>
		</div>
		<table class="addtopmargin5 standardTable">
			<tr>
				<td>
					<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_SmtpServer_Label %>" />
				</td>
				<td>
					<asp:TextBox ID="txtSmtpServer" runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Admin_Email_SmtpPort_Label %>" />
				</td>
				<td>
					<asp:TextBox ID="txtSmtpPort" runat="server" />
				</td>
			</tr>
		</table>
	</div>
	<tis:wwDataBinder ID="wwDataBinder" runat="server">
		<DataBindingItems>
			<tis:wwDataBindingItem ID="WwDataBindingItem1" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="SendEmailOnError" ControlId="chkSendEmail" BindingProperty="Checked"
				UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_SendEmail_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem2" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmailToName" ControlId="txtEmailToName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem3" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmailToAddress" ControlId="txtEmailToAddress" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem4" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmailFromName" ControlId="txtEmailFromName" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailName_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem5" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="EmailFromAddress" ControlId="txtEmailFromAddress" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_EmailAddress_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem6" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="SmtpServer" ControlId="txtSmtpServer" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_SmtpServer_Label %>" />
			<tis:wwDataBindingItem ID="WwDataBindingItem7" runat="server" BindingSource="Master.CoreConfig"
				BindingSourceMember="SmtpServerPort" ControlId="txtSmtpPort" UserFieldName="<%$ Resources:GalleryServerPro, Admin_Email_SmtpPort_Label %>" />
			<tis:wwDataBindingItem runat="server" ControlId="btnEmailTest">
			</tis:wwDataBindingItem>
		</DataBindingItems>
	</tis:wwDataBinder>
	<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
		DefaultDialogBodyCss="dg5ContentBodyCss">
		<PopupInfoItems>
			<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="chkSendEmail" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_sendEmailOnError_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_sendEmailOnError_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem2" runat="server" ControlId="txtEmailToName"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailToName_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailToName_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem3" runat="server" ControlId="txtEmailToAddress"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailToAddress_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailToAddress_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem4" runat="server" ControlId="txtEmailFromName"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailFromName_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailFromName_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem5" runat="server" ControlId="txtEmailFromAddress"
				DialogTitle="<%$ Resources:GalleryServerPro, Cfg_emailFromAddress_Hdr %>" DialogBody="<%$ Resources:GalleryServerPro, Cfg_emailFromAddress_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem6" runat="server" ControlId="txtSmtpServer" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_smtpServer_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_smtpServer_Bdy %>" />
			<tis:PopupInfoItem ID="PopupInfoItem7" runat="server" ControlId="txtSmtpPort" DialogTitle="<%$ Resources:GalleryServerPro, Cfg_smtpPort_Hdr %>"
				DialogBody="<%$ Resources:GalleryServerPro, Cfg_smtpPort_Bdy %>" />
		</PopupInfoItems>
	</tis:PopupInfo>
	<uc1:popup ID="ucPopupContainer" runat="server" />
</asp:Content>

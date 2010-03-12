<%@ Page Language="C#" MasterPageFile="~/master/site.master" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs" Inherits="GalleryServerPro.Web.changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c2" runat="server">
	<asp:ChangePassword ID="ChangePassword1" runat="server" CssClass="changepwd" Width="400px" CancelButtonText="<%$ Resources:GalleryServerPro, Change_Pwd_CancelButtonText %>"
		ChangePasswordButtonText="<%$ Resources:GalleryServerPro, Change_Pwd_ChangePasswordButtonText %>" ChangePasswordFailureText="<%$ Resources:GalleryServerPro, Change_Pwd_ChangePasswordFailureText %>"
		ChangePasswordTitleText="<%$ Resources:GalleryServerPro, Change_Pwd_ChangePasswordTitleText %>" ConfirmNewPasswordLabelText="<%$ Resources:GalleryServerPro, Change_Pwd_ConfirmNewPasswordLabelText %>"
		ConfirmPasswordCompareErrorMessage="<%$ Resources:GalleryServerPro, Change_Pwd_ConfirmPasswordCompareErrorMessage %>" ConfirmPasswordRequiredErrorMessage="<%$ Resources:GalleryServerPro, Change_Pwd_ConfirmPasswordRequiredErrorMessage %>"
		ContinueButtonText="<%$ Resources:GalleryServerPro, Change_Pwd_ContinueButtonText %>" NewPasswordLabelText="<%$ Resources:GalleryServerPro, Change_Pwd_NewPasswordLabelText %>"
		NewPasswordRequiredErrorMessage="<%$ Resources:GalleryServerPro, Change_Pwd_NewPasswordRequiredErrorMessage %>" PasswordLabelText="<%$ Resources:GalleryServerPro, Change_Pwd_PasswordLabelText %>"
		PasswordRequiredErrorMessage="<%$ Resources:GalleryServerPro, Change_Pwd_PasswordRequiredErrorMessage %>" SuccessText="<%$ Resources:GalleryServerPro, Change_Pwd_SuccessText %>"
		SuccessTitleText="<%$ Resources:GalleryServerPro, Change_Pwd_SuccessTitleText %>" UserNameLabelText="<%$ Resources:GalleryServerPro, Change_Pwd_UserNameLabelText %>"
		UserNameRequiredErrorMessage="<%$ Resources:GalleryServerPro, Change_Pwd_UserNameRequiredErrorMessage %>" OnSendingMail="ChangePassword1_SendingMail">
		<TitleTextStyle CssClass="changepwdTitle" />
		<FailureTextStyle CssClass="msgwarning" ForeColor="" />
		<MailDefinition BodyFileName="~/changepassword.aspx" Subject="<%$ Resources:GalleryServerPro, Change_Pwd_MailDefinitionSubject %>" />
	</asp:ChangePassword>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/master/site.master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="GalleryServerPro.Web.login" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c2" runat="Server">
	<asp:Panel ID="pnlInvalidLogin" runat="server" CssClass="invalidLoginMsg" EnableViewState="false" Visible="false" />
	<asp:Login ID="Login1" runat="server" CssClass="login" BorderPadding="0" PasswordRecoveryUrl="~/anon/recoverpassword.aspx"
		Width="300" FailureText="<%$ Resources:GalleryServerPro, Login_FailureText %>" PasswordLabelText="<%$ Resources:GalleryServerPro, Login_PasswordLabelText %>"
		PasswordRecoveryText="<%$ Resources:GalleryServerPro, Login_PasswordRecoveryText %>" PasswordRequiredErrorMessage="<%$ Resources:GalleryServerPro, Login_PasswordRequiredErrorMessage %>"
		RememberMeText="<%$ Resources:GalleryServerPro, Login_RememberMeText %>" TitleText="<%$ Resources:GalleryServerPro, Login_TitleText %>"
		UserNameLabelText="<%$ Resources:GalleryServerPro, Login_UserNameLabelText %>" UserNameRequiredErrorMessage="<%$ Resources:GalleryServerPro, Login_UserNameRequiredErrorMessage %>"
		LoginButtonText="<%$ Resources:GalleryServerPro, Login_LoginButtonText %>">
		<TitleTextStyle CssClass="loginTitle" />
		<FailureTextStyle CssClass="msgwarning" ForeColor="" />
		<ValidatorTextStyle CssClass="msgwarning" ForeColor="" />
	</asp:Login>
</asp:Content>

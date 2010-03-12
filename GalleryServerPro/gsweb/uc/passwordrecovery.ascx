<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="passwordrecovery.ascx.cs"
	Inherits="GalleryServerPro.Web.uc.passwordrecovery" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel ID="pnlMsgContainer" runat="server" />
<asp:Panel ID="pnlPwdRecoverContainer" runat="server" CssClass="pwdrecover">
	<p class="pwdrecoverTitle">
		<asp:Literal ID="lit1" runat="server" Text="<%$ Resources:GalleryServerPro, Anon_Pwd_Recovery_Header %>" /></p>
	<p>
		<asp:Literal ID="lit2" runat="server" Text="<%$ Resources:GalleryServerPro, Anon_Pwd_Recovery_Instructions %>" />
	</p>
	<p>
		<asp:Literal ID="lit3" runat="server" Text="<%$ Resources:GalleryServerPro, Anon_Pwd_Recovery_UserName_Label %>" />
		<asp:TextBox ID="txtUserName" runat="server" MaxLength="256" />
		<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtUserName"
			Display="None" ErrorMessage="<%$ Resources:GalleryServerPro, Anon_Pwd_Recovery_Username_Required_Text %>">
		</asp:RequiredFieldValidator>
		<cc1:ValidatorCalloutExtender runat="server" ID="rfv1E" TargetControlID="rfv1" HighlightCssClass="validatorCalloutHighlight" />
	</p>
	<p class="pwdRecoverRetrieve">
		<asp:Button ID="btnRetrievePassword" runat="server" Text="<%$ Resources:GalleryServerPro, Anon_Pwd_Recovery_Retrieve_Pwd_Button_Text %>" OnClick="btnRetrievePassword_Click" /></p>
</asp:Panel>

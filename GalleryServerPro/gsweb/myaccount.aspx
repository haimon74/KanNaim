<%@ Page Language="C#" MasterPageFile="~/master/site.master" AutoEventWireup="true"
	CodeBehind="myaccount.aspx.cs" Inherits="GalleryServerPro.Web.myaccount" Title="Untitled Page" %>

<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c2" runat="server">
	<div class="addpadding1">
		<h1>
			<asp:Literal ID="lit1" runat="server" Text="<%$ Resources:GalleryServerPro, MyAccount_Hdr %>" /></h1>
		<tis:wwErrorDisplay ID="wwMessage" runat="server" CellPadding="2" UseFixedHeightWhenHiding="False"
			Center="False" Width="500px">
		</tis:wwErrorDisplay>
		<p class="fll">
			<span class="bold">
				<asp:Literal ID="lit2" runat="server" Text="<%$ Resources:GalleryServerPro, MyAccount_UserName_Label %>" /></span>
			<asp:Literal ID="litUserName" runat="server" /></p>
		<p>
			<a href="changepassword.aspx">
				<asp:Literal ID="lit3" runat="server" Text="<%$ Resources:GalleryServerPro, MyAccount_Change_Pwd_Hyperlink_Text %>" /></a></p>
		<p>
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, MyAccount_Email_Address_Label %>" />
			<asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" /></p>
		<p class="textcol msgfriendly">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:GalleryServerPro, MyAccount_Email_Address_Note %>" /></p>
	</div>
	<div class="rightBottom">
		<p class="minimargin">
			<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources:GalleryServerPro, Default_Task_Ok_Button_Text %>" />
			<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
				Text="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Text %>" ToolTip="<%$ Resources:GalleryServerPro, Default_Task_Cancel_Button_Tooltip %>" />&nbsp;</p>
	</div>
</asp:Content>

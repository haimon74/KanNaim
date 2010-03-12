<%@ Page Language="C#" MasterPageFile="~/master/site.master" AutoEventWireup="true"
	CodeBehind="recoverpassword.aspx.cs" Inherits="GalleryServerPro.Web.anon.recoverpassword" %>

<%@ Register Src="~/uc/passwordrecovery.ascx" TagName="PwdRecovery" TagPrefix="uc1" %>
<%@ MasterType TypeName="GalleryServerPro.Web.Master.site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c2" runat="server">
	<uc1:PwdRecovery ID="pwdRecover" runat="server" />
</asp:Content>

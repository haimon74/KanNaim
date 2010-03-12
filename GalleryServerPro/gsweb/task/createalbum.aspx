<%@ Page Language="C#" MasterPageFile="~/master/taskMstr.master" AutoEventWireup="true"
	Codebehind="createalbum.aspx.cs" Inherits="GalleryServerPro.Web.task.createalbum" ValidateRequest="false" %>

<%@ Register Src="~/uc/albumtreeview.ascx" TagName="albumtreeview" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType TypeName="GalleryServerPro.Web.Master.taskMstr" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="Server">
	<h3>
		<asp:Literal ID="litStep1" runat="server" Text="<%$ Resources:GalleryServerPro, Task_All_Step_1_Text %>" /></h3>
	<p>
		<asp:Label ID="lblTitle" runat="server" AssociatedControlID="txtTitle" Text="<%$ Resources:GalleryServerPro, Task_Create_Album_Title_Label_Text %>" />
		<asp:TextBox ID="txtTitle" runat="server" Style="width: 450px;" />
		<asp:Label ID="lblMaxTitleLengthInfo" runat="server" AssociatedControlID="txtTitle"
			CssClass="fs" />
		<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtTitle"
			Display="None" ErrorMessage="<%$ Resources:GalleryServerPro, Task_Create_Album_Missing_Title_Text %>">
		</asp:RequiredFieldValidator>
		<cc1:ValidatorCalloutExtender runat="server" ID="rfv1E" TargetControlID="rfv1" HighlightCssClass="validatorCalloutHighlight" />
	</p>
	<p>
		&nbsp;</p>
	<h3>
		<asp:Literal ID="litStep2" runat="server" Text="<%$ Resources:GalleryServerPro, Task_All_Step_2_Text %>" /></h3>
	<p>
		<asp:Literal ID="litInstructions" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Create_Album_Instructions %>" /></p>
	<div>
		<uc1:albumtreeview ID="tvUC" runat="server" AllowMultiSelect="false" RequireAlbumSelection="true" />
	</div>
</asp:Content>

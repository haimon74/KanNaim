<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error_unauthorized.aspx.cs"
	Inherits="GalleryServerPro.Web.error.error_unauthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>
		<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Error_Unauthorized_Page_Title %>" /></title>
	<style type="text/css">
		<!
		-- body { font: 0.9em Verdana, Arial, Helvetica, sans-serif; background-color: #f5f5f5; color: #000; }
		img { border: none; }
		div#errContainer { margin: 25px auto auto auto; width: 600px; height: 400px; border: 3px solid #369; }
		div#errContainer div#errHeader { height: 75px; background-color: #ECF1F6; border-bottom: 1px solid #369; position: relative; }
		div#errContainer div#errHeader h1 { font-size: 2em; margin: 0; padding: 0; color: #369; position: absolute; top: auto; left: 0.4em; bottom: 0.4em; right: auto; }
		div#errContainer div#errContent { height: 215px; overflow: auto; background-color: #ECECD9; padding: 1em; }
		div#errContainer div#errContent h2 { margin: 0 0 1em 0; padding: 0; background-color: Transparent; }
		div#errContainer div#errFooter { width: 100%; height: 76px; background-color: #ECF1F6; position: relative; border-top: 1px solid #369; }
		-- ></style>
</head>
<body>
	<form id="form1" runat="server">
	<div id="errContainer">
		<div id="errHeader">
			<img src="../images/gsp_logo_313x75.png" style="width: 313px; height: 75px;" alt="Gallery Server Pro logo" />
		</div>
		<div id="errContent">
			<h2>
				<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Error_Unauthorized_Hdr %>" /></h2>
			<p>
				<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Error_Unauthorized_Dtl %>" /></p>
			<p style="margin-top: 3em;">
				<asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/default.aspx" ToolTip="<%$ Resources:GalleryServerPro, Error_Home_Page_Link_Text %>"
					Text="<%$ Resources:GalleryServerPro, Error_Home_Page_Link_Text %>" /></p>
			<p>
				<asp:HyperLink ID="hlGspHome" runat="server" NavigateUrl="http://www.galleryserverpro.com" ToolTip="<%$ Resources:GalleryServerPro, Error_Gsp_Home_Page_Link_Tooltip %>"
					Text="www.galleryserverpro.com" /></p>
		</div>
		<div id="errFooter">
			<asp:HyperLink ID="hlTisHome" runat="server" ImageUrl="~/images/tis_logo.gif" NavigateUrl="http://www.techinfosystems.com"
				ToolTip="<%$ Resources:GalleryServerPro, Error_Tis_Home_Page_Link_Tooltip %>" Text="www.galleryserverpro.com" />
		</div>
	</div>
	</form>
</body>
</html>

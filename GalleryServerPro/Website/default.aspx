<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Website._Default" %>

<%@ Register TagPrefix="gsp" Namespace="GalleryServerPro.Web" Assembly="GalleryServerPro.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<style type="text/css">
		html, body { margin:0;padding:2px; }
		body { background-color:#f5f5f5; }
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="sm" runat="server" EnableScriptGlobalization="true" />
		<gsp:Gallery ID="g" runat="server" />
	</form>
</body>
</html>

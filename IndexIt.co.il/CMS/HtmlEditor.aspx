<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HtmlEditor.aspx.cs" Inherits="CMS_HtmlEditor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <cc1:Editor ID="Editor1" runat="server" 
                Height="100%" 
                Width="100%"
                AutoFocus="true">
        </cc1:Editor>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sandbox.aspx.cs" Inherits="Sandbox" %>

<%@ Register src="UserControls/SandboxForm/InputDate.ascx" tagname="InputDate" tagprefix="uc1" %>

<%@ Register src="UserControls/SandboxForm/InputEmail.ascx" tagname="InputEmail" tagprefix="uc2" %>

<%@ Register src="UserControls/SandboxForm/InputZipCode.ascx" tagname="InputZipCode" tagprefix="uc3" %>
<%@ Register src="UserControls/SandboxForm/InputPhoneNumber.ascx" tagname="InputPhoneNumber" tagprefix="uc4" %>
<%@ Register src="UserControls/SandboxForm/InputIdNumber.ascx" tagname="InputIdNumber" tagprefix="uc5" %>
<%@ Register src="UserControls/SandboxForm/InputFullName.ascx" tagname="InputFullName" tagprefix="uc6" %>

<%@ Register src="UserControls/SandboxForm/InputName.ascx" tagname="InputName" tagprefix="uc7" %>

<%@ Register src="UserControls/SandboxForm/InputPassword.ascx" tagname="InputPassword" tagprefix="uc8" %>

<%@ Register src="UserControls/SandboxForm/InputPasswordX2.ascx" tagname="InputPasswordX2" tagprefix="uc9" %>

<%@ Register src="UserControls/StyleManagementForm/InputAsyncFileUpload/InputAsyncFileUpload.ascx" tagname="InputAsyncFileUpload" tagprefix="uc10" %>

<%@ Register src="UserControls/StyleManagementForm/InputImage.ascx" tagname="InputImage" tagprefix="uc11" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/Theme1/primary.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        
        window.onload = "Initialize()";

        function Initialize() {
            InitEvents();
            PreLoadImages();
        }
        function InitEvents() {
            window.onresize = "ResizeAllElements()";
            document.onmousemove = "GetMouseCoord()";
            document.oncontextmenu = "OpenCustomContextMenu()";
        }
        function PreLoadImages() {
            var a = new Image(); a.src = 'root.gif';
            var b = new Image(); b.src = 'hover.gif';
            var c = new Image(); c.src = 'active.gif';
            // <a href="whatever" onmouseover="document.images['s3a'].src='hover.gif';" 
            //          onmouseout="document.images['s3a'].src='root.gif';" 
            //          onmousedown="document.images['s3a'].src='active.gif';" 
            //          onmouseup="document.images['s3a'].src='hover.gif';">
            //      <img src="root.gif" name="s3a" height="50" width="50" alt="" border="0">
            // </a>
            // OR USE SIMPLER HTML (when the image is not a link!) !!! :
            // <input type="image" src="root.gif" hoversrc="hover.gif" activesrc="active.gif" height="50" width="50" alt="" border="0">
        }
        function ResizeAllElements() {
            return false;
        }
        function OpenCustomContextMenu() {
            return false;
        }
        function GetMouseCoord(e) {
            var xcoord, ycoord;
            if (!e) { e = window.event; }
            if (!e) { return; }
            if (typeof (e.pageX) == 'number') {
                xcoord = e.pageX;
                ycoord = e.pageY;
            } else if (typeof (e.clientX) == 'number') {
                xcoord = e.clientX;
                ycoord = e.clientY;
                if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
                    xcoord += document.body.scrollLeft;
                    ycoord += document.body.scrollTop;
                } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
                    xcoord += document.documentElement.scrollLeft;
                    ycoord += document.documentElement.scrollTop;
                }
            } else { return; }
            return [xcoord , ycoord];
        } 
        function GetInnerBrowserSize() {
            var myWidth = 0, myHeight = 0;
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
            window.alert('Width = ' + myWidth);
            window.alert('Height = ' + myHeight);
            return [myWidth, myHeight];
        }
        function GetScrollXY() {
            var scrOfX = 0, scrOfY = 0;
            if (typeof (window.pageYOffset) == 'number') {
                //Netscape compliant
                scrOfY = window.pageYOffset;
                scrOfX = window.pageXOffset;
            } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
                //DOM compliant
                scrOfY = document.body.scrollTop;
                scrOfX = document.body.scrollLeft;
            } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
                //IE6 standards compliant mode
                scrOfY = document.documentElement.scrollTop;
                scrOfX = document.documentElement.scrollLeft;
            }
            return [scrOfX, scrOfY];
        }
        function ResizeAllElements() {
            for (var elem in document.childNodes) {
                var x = window.innerWidth;
            }            
        }
        function clearClipboard() {
            clipboardData.clearData();
            alert('Sorry, Could not copy to clipboard');
        }

        function CancelCopyToClipboard(e) {
            if (!e) var e = window.event;
            if (e.type == 'copy')
                window.setTimeout(clearClipboard, 20);
        }

        //"clipboardData.setData('Text', '');";

        function OpenNewBrowser(path, winName, status, left, top, width, height,
						resizable, locationBar, menuBar, scrollBar, toolBal) {
            var str = 'left=' + left + ',top=' + top + ',width=' + width + ',height=' + height +
			          ',resizable=' + resizable + ',location=' + locationBar +
			          ',menubar=' + menuBar + ',scrollbar=' + scrollBar + ',status=' + status;
            window.open(path, winName, str);
        }
        function OpenNewLightBrowser(path, winName) {
            OpenNewBrowser(path, winName, 1, 100, 100, 600, 600, 0, 0, 0, 0, 0);
        }
        /*oncopy="clearClipboard();"*/        
    </script>
</head>

<body oncopy="return false;" oncontextmenu="return false;" ondoubleclick="return false;" onmousemove="return false;">
    <form id="Form1" runat="server">
    <input id="Button1" type="button" value="button" onclick="OpenNewLightBrowser('Default.aspx', 'testWin');" />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        
    <div>
        <uc1:InputDate ID="InputDate1" runat="server" />        
    </div>
    <div>        
        <uc2:InputEmail ID="InputEmail1" runat="server" />        
    </div>
    <div>
        <uc3:InputZipCode ID="InputZipCode1" runat="server" />
        <uc4:InputPhoneNumber ID="InputPhoneNumber1" runat="server" />
        <uc5:InputIdNumber ID="InputIdNumber1" runat="server" />
        <uc7:InputName ID="InputName2" runat="server" DisplayText="שם פרטי" />
        <uc7:InputName ID="InputName3" runat="server" DisplayText="שם משפחה" />
        <uc11:InputImage ID="InputImage1" runat="server" />
        <uc9:InputPasswordX2 ID="InputPasswordX2" runat="server" />        
    </div>
    <br /><br /><br />
    <div>    
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </div>
    
    </form>
</body>
</html>

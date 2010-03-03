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
</head>
<body>
    <form id="form1" runat="server">
    
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

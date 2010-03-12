<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputPassword.ascx.cs" Inherits="UserControls_SandboxForm_InputPassword" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprPassword" runat="server" 
        LabelText="סיסמא"
        Mode="Password"
        Language="English" 
        TextMaxLength="10"
        ErrorMsg="הזן סיסמא בין 5 ל 10 אותיות או מספרים בלבד"  
        RegularExpression="^[0-9a-zA-Zא-ת\s]{5,10}$"         
        WatermarkText="abcאבג123" 
        WatermarkCssClass="watermark"        
 />
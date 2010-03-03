<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputZipCode.ascx.cs" Inherits="UserControls_SandboxForm_InputZipCode" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprZip" runat="server" 
        LabelText="מיקוד" 
        Language="English" 
        TextMaxLength="5"
        ErrorMsg="מיקוד אינו תקני - הזן 5 ספרות בלבד"  
        RegularExpression="\d{5}"
        WatermarkText="12345" 
        WatermarkCssClass="watermark"        
 />
                     
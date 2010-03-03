<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputIdNumber.ascx.cs" Inherits="UserControls_SandboxForm_InputIdNumber" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprID" runat="server" 
        LabelText="תעודת זהות" 
        Language="English" 
        TextMaxLength="9"
        ErrorMsg=" מספר זהות אינו תקני - הזן 9 ספרות רצופות בלבד 123456789"  
        RegularExpression="\d{8,9}"
        WatermarkText="123456789" 
        WatermarkCssClass="watermark"        
 />
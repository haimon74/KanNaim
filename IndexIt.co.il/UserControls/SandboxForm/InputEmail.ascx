<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputEmail.ascx.cs" Inherits="UserControls_SandboxForm_InputEmail" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprEmail" runat="server" 
        LabelText="אימייל email" 
        Language="English" 
        TextMaxLength="50"
        ErrorMsg=" user@domain.com הזן אימייל מלא ותקני"  
        RegularExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"         
        WatermarkText="user@domain.com" 
        WatermarkCssClass="watermark"        
 />
                        
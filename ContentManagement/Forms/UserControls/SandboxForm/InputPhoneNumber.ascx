<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputPhoneNumber.ascx.cs" Inherits="UserControls_SandboxForm_InputPhoneNumber" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprPhone" runat="server" 
        LabelText="טלפון" 
        Language="English" 
        TextMaxLength="11"
        ErrorMsg="טלפון אינו תקני - הזן ספרות בפורמט הבא בלבד 050-1234567"  
        RegularExpression="\d{2,3}-?\d{6,7}"
        WatermarkText="050-1234567" 
        WatermarkCssClass="watermark"        
 />
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputFullNameHebrew.ascx.cs" Inherits="UserControls_SandboxForm_InputFullNameHebrew" %>

<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprFullNameHebrew" runat="server" 
        LabelText="שם מלא" 
        Language="Hebrew" 
        TextMaxLength="21"
        ErrorMsg=" הזן שם מלא באותיות עבריות בלבד"  
        RegularExpression="^[א-ת''-'\s]{2,10}( )+[א-ת''-'\s]{2,10}$"         
        WatermarkText="משה כהן" 
        WatermarkCssClass="watermark"        
 />
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputName.ascx.cs" Inherits="UserControls_SandboxForm_InputName" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprFullName" runat="server" 
        Language="Hebrew" 
        TextMaxLength="15"
        ErrorMsg=" הזן שם באותיות עבריות או אנליות בלבד"  
        RegularExpression="^[a-zA-Zא-ת''-'\s]{2,15}$"         
        WatermarkText="משה או Moshe" 
        WatermarkCssClass="watermark"        
 />
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputFullName.ascx.cs" Inherits="UserControls_SandboxForm_InputFullName" %>


<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprFullName" runat="server" 
        LabelText="שם מלא" 
        Language="English" 
        TextMaxLength="31"
        ErrorMsg=" הזן שם מלא באותיות אנגליות בלבד"  
        RegularExpression="^[a-zA-Z''-'\s]{2,15}( )+[a-zA-Z''-'\s]{2,15}$"         
        WatermarkText="Moshe Cohen" 
        WatermarkCssClass="watermark"        
 />
                        
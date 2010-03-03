<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputPasswordX2.ascx.cs" Inherits="UserControls_SandboxForm_InputPasswordX2" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprPassword1" runat="server" 
        LabelText="סיסמא"
        Mode="Password"
        Language="English" 
        TextMaxLength="10"
        ErrorMsg="הזן סיסמא בין 5 ל 10 אותיות או מספרים בלבד"  
        RegularExpression="^[0-9a-zA-Zא-ת\s]{5,10}$"         
        WatermarkText="abcאבג123" 
        WatermarkCssClass="watermark"        
 />
 
 <uc:InputRegularExpression ID="InputRegExprPassword2" runat="server" 
        LabelText="אימות סיסמא"
        Mode="Password"
        Language="English" 
        TextMaxLength="10"
        ErrorMsg="אימות סיסמא אינה זהה לסיסמא"  
        RegularExpression="^[0-9a-zA-Zא-ת\s]{5,10}$"         
        WatermarkText="abcאבג123" 
        WatermarkCssClass="watermark"      
 />
 
<asp:TextBox ID="TextBox1" Visible="false" runat="server"></asp:TextBox>
<asp:TextBox ID="TextBox2" Visible="false" runat="server"></asp:TextBox>
<asp:CompareValidator ID="CompareValidator1" runat="server" 
    ErrorMessage="סיסמא לא אומתה: שדות סיסמא ואימות סיסמא חייבים להיות זהים"
    ControlToValidate="TextBox2"
    ControlToCompare="TextBox1"
    EnableClientScript="True"     
    Operator="Equal"
    Type="String">
</asp:CompareValidator>
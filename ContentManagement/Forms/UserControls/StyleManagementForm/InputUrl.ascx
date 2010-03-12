<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputUrl.ascx.cs" Inherits="UserControls_StyleManagementForm_InputUrl" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputRegularExpression.ascx" TagPrefix="uc" TagName="InputRegularExpression" %>

<uc:InputRegularExpression ID="InputRegExprUrl" runat="server" 
        LabelText="לינק URL" 
        Language="English" 
        TextMaxLength="250"
        ErrorMsg="הזן לינק מלא ותקני"  
        RegularExpression="^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$" 
        WatermarkText="http://www.example.com" 
        WatermarkCssClass="watermark"
/>
                        
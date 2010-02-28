<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputBackgroundImage.ascx.cs" Inherits="UserControls_StyleManagementForm_InputBackgroundImage" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputBackgroundImage" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="תמונת רקע"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:FileUpload ID="FileUpload1" runat="server" Enabled="False" />
    </div>
    <div class="InputValidation">        
    </div>
    <br />
    
    <div class="InputLabel">
        <asp:Label ID="Label2" runat="server" Text="רוחב תמונה"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxBgImageWidth" runat="server"></asp:TextBox>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
    
    <div class="InputLabel">
        <asp:Label ID="Label3" runat="server" Text="גובה תמונה"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxBgImageHeight" runat="server"></asp:TextBox>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
    
    <div class="InputLabel">
        <asp:Label ID="Label6" runat="server" Text="הצמדת תמונה ציר Y"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:VerticalAlignmentDropDownList runat="server" ID="DropDownListBgImageVAlign"></wc:VerticalAlignmentDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
    
    <div class="InputLabel">
        <asp:Label ID="Label7" runat="server" Text="הצמדת תמונה ציר X"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:HorizontalAlignmentDropDownList runat="server" ID="DropDownListBgImageHAlign" ></wc:HorizontalAlignmentDropDownList>  
    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>

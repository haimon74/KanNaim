<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputAlignment.ascx.cs" Inherits="UserControls_StyleManagementForm_InputAlignment" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputAlignment" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="LabelV" runat="server" Text="יישור ורטיקלי"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:VerticalAlignmentDropDownList runat="server" ID="DropDownListBgImageVAlign"></wc:VerticalAlignmentDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
    <br />
    
    <div class="InputLabel">
        <asp:Label ID="LabelH" runat="server" Text="יישור הוריזונטלי"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:HorizontalAlignmentDropDownList runat="server" ID="DropDownListBgImageHAlign" ></wc:HorizontalAlignmentDropDownList>  
    </div>
    <div class="InputValidation">        
    </div>
    <br />
</div>
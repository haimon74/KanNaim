﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputImage.ascx.cs" Inherits="UserControls_StyleManagementForm_InputImage" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputNumber.ascx" TagPrefix="uc" TagName="InputNumber" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputOpacity.ascx" TagPrefix="uc" TagName="InputOpacity" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputAsyncFileUpload/InputAsyncFileUpload.ascx" TagPrefix="uc" TagName="InputAsyncFileUpload" %>


    <uc:InputAsyncFileUpload ID="InputAsyncFileUpload1" runat="server" />
    <uc:InputNumber ID="InputNumberWidth"  EnableViewState="true" LabelText="רוחב תמונה" runat="server"  RangeMin="0" RangeMax="800" />
    <uc:InputNumber ID="InputNumberHeight"  EnableViewState="true" LabelText="גובה תמונה" runat="server"  RangeMin="0" RangeMax="800" />
    <uc:InputOpacity ID="InputOpacity1" runat="server" />

<div id="Div3"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label6" runat="server" Text="הצמדת תמונה ציר Y"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:VerticalAlignmentDropDownList runat="server"  EnableViewState="true" ID="DropDownListBgImageVAlign"></wc:VerticalAlignmentDropDownList>
    </div>
    <div class="InputValidation">        
    </div>
</div>    
    <br />

<div id="Div4"  class="InputDiv">    
    <div class="InputLabel">
        <asp:Label ID="Label7" runat="server" Text="הצמדת תמונה ציר X"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:HorizontalAlignmentDropDownList runat="server" EnableViewState="true"  ID="DropDownListBgImageHAlign" ></wc:HorizontalAlignmentDropDownList>  
    </div>
    <div class="InputValidation">        
    </div>    
</div>
<br />


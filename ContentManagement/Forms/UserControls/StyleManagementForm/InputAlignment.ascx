<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputAlignment.ascx.cs" Inherits="UserControls_StyleManagementForm_InputAlignment" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputAlignmentVertical" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="LabelV" runat="server" Text="יישור אנכי"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:VerticalAlignmentDropDownList runat="server" ID="DropDownListBgImageVAlign"></wc:VerticalAlignmentDropDownList>
        <cc1:DropDownExtender ID="DropDownExtender2" TargetControlID="DropDownListBgImageVAlign"  
             DropArrowBackColor="Transparent" DropArrowWidth="50" HighlightBackColor="Bisque" 
             HighlightBorderColor="OrangeRed" runat="server" Enabled="true"></cc1:DropDownExtender>
    </div>
    <div class="InputValidation">        
    </div>    
</div>
<br />

<div id="InputAlignmentHorizontal" class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="LabelH" runat="server" Text="יישור אופקי"></asp:Label>
    </div>
    <div class="InputContent">
        <wc:HorizontalAlignmentDropDownList runat="server" ID="DropDownListBgImageHAlign" ></wc:HorizontalAlignmentDropDownList>  
        <cc1:DropDownExtender ID="DropDownExtender1" TargetControlID="DropDownListBgImageHAlign"  
             DropArrowBackColor="Transparent" DropArrowWidth="50" HighlightBackColor="Bisque" 
             HighlightBorderColor="OrangeRed" runat="server" Enabled="true"></cc1:DropDownExtender>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />
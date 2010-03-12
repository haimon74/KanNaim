<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputFontColor.ascx.cs" Inherits="UserControls_StyleManagementForm_InputFontColor" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputFontColor"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label3" runat="server" Text="צבע פנים"></asp:Label>
        <asp:Button ID="ButtonPreviewColor" runat="server" Width="20" Height="20" BorderStyle="None" />
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxColor" runat="server"></asp:TextBox>
        <cc1:ColorPickerExtender ID="TextBoxColor_ColorPickerExtender" runat="server" 
            Enabled="True" TargetControlID="TextBoxColor"  SampleControlID="ButtonPreviewColor">
        </cc1:ColorPickerExtender>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="true"  
                TargetControlID="TextBoxColor" WatermarkText="RRGGBB" WatermarkCssClass="watermark">
        </cc1:TextBoxWatermarkExtender>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputColorPicker.ascx.cs" Inherits="UserControls_StyleManagementForm_InputColorPicker" %>



<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<div id="InputColorPicker"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label3" runat="server"></asp:Label>
        <asp:Button ID="ButtonPreviewColor" runat="server" Width="20" Height="20" BorderStyle="None" />
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBoxColor" runat="server"></asp:TextBox>
        <cc1:ColorPickerExtender ID="TextBoxColor_ColorPickerExtender" runat="server"  PopupPosition="TopLeft"
            Enabled="True" TargetControlID="TextBoxColor" SampleControlID="ButtonPreviewColor">
        </cc1:ColorPickerExtender>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="true"  
                TargetControlID="TextBoxColor" WatermarkText="RRGGBB" WatermarkCssClass="watermark">
        </cc1:TextBoxWatermarkExtender>
    </div>
    <div class="InputValidation">        
    </div>
</div>
<br />

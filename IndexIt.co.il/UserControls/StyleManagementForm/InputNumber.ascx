<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputNumber.ascx.cs" Inherits="UserControls_StyleManagementForm_InputNumber" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputNumber"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" ></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox runat="server" ID="TextBox1" style="direction:ltr; text-align:center;"></asp:TextBox>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="true"  
                TargetControlID="TextBox1" WatermarkCssClass="watermark">
        </cc1:TextBoxWatermarkExtender>
    </div>
    <div class="InputValidation">
        <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" 
                ControlToValidate="TextBox1"  Text=" * ">
        </asp:RangeValidator>          
    </div>
</div>
<br />

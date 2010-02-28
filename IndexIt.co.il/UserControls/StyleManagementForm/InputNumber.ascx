<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputNumber.ascx.cs" Inherits="UserControls_StyleManagementForm_InputNumber" %>

<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls" TagPrefix="wc" %>

<div id="InputNumber" dir="rtl">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" ></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox runat="server" ID="TextBox1"></asp:TextBox>
    </div>
    <div class="InputValidation">
        <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" 
                ControlToValidate="TextBox1"  Text=" * ">
        </asp:RangeValidator>          
    </div>
    <br />
</div>

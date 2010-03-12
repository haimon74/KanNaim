<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputDate.ascx.cs" Inherits="UserControls_SandboxForm_InputDate" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<div id="InputDate"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="תאריך"></asp:Label>
    </div>
    <div class="InputContent">
        <asp:TextBox ID="TextBox1" runat="server" CssClass="EnglishText" Enabled="false" ToolTip="Click on the Image button to show calendar"></asp:TextBox>
        <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/img/Calendar_scheduleHS.png" ToolTip="Click here to show calendar" />
        <cc1:CalendarExtender runat="server"
            TargetControlID="TextBox1"
            Format="dd/MM/yyyy"
            FirstDayOfWeek="Sunday"
            PopupPosition="Left" 
            PopupButtonID="Button1" />  
    </div>
    <div class="InputValidation" style="width:auto;">
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                 ControlToValidate="TextBox1" Text=" * " 
                 ErrorMessage="Date not valid: DD/MM/YYYY" 
                 ToolTip="Input date in format: DD/MM/YYYY"                 
                 ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d" >
        </asp:RegularExpressionValidator>
    </div>
</div>
    <br />

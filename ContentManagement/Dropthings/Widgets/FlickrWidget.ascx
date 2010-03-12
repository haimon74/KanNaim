<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FlickrWidget.ascx.cs" Inherits="FlickrWidget" EnableViewState="True" %>
<style type="text/css">
div.previews { position: relative; }
div.preview { margin: 0 8px 8px 0; width:75px; height:75px; }
img.preview { position: absolute; z-index: 0; width: 75px; height: 75px; }
</style>
<asp:Panel ID="settingsPanel" runat="server" Visible="False">
    <asp:Label ID="LabelColumns" runat="server" Text="Columns" Width="100px" ToolTip="Photos per row"></asp:Label>
    <asp:DropDownList ID="DropDownListColumns" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListColumns_OnSelectedIndexChanged">
        <asp:ListItem>1</asp:ListItem>
        <asp:ListItem>2</asp:ListItem>
        <asp:ListItem>3</asp:ListItem>
        <asp:ListItem>4</asp:ListItem>
        <asp:ListItem>5</asp:ListItem>
        <asp:ListItem>6</asp:ListItem>
        <asp:ListItem>7</asp:ListItem>
        <asp:ListItem>8</asp:ListItem>
        <asp:ListItem>9</asp:ListItem>
        <asp:ListItem>10</asp:ListItem>
    </asp:DropDownList> 
    <br />
    <asp:Label ID="LabelRows" runat="server" Text="Rows" Width="100px" ToolTip="Photos per column"></asp:Label>
    <asp:DropDownList ID="DropDownListRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListRows_OnSelectedIndexChanged">
        <asp:ListItem>1</asp:ListItem>
        <asp:ListItem>2</asp:ListItem>
        <asp:ListItem>3</asp:ListItem>
        <asp:ListItem>4</asp:ListItem>
        <asp:ListItem>5</asp:ListItem>
        <asp:ListItem>6</asp:ListItem>
        <asp:ListItem>7</asp:ListItem>
        <asp:ListItem>8</asp:ListItem>
        <asp:ListItem>9</asp:ListItem>
        <asp:ListItem>10</asp:ListItem>
    </asp:DropDownList> 
    <br />
    <asp:RadioButton ID="mostInterestingRadioButton" runat="server" Width="100px" AutoPostBack="True"
        Checked="True" GroupName="FlickrPhoto" OnCheckedChanged="photoTypeRadio_CheckedChanged"
        Text="Most Interesting" />
        <br />
    <asp:RadioButton ID="mostRecentRadioButton" runat="server" Width="100px" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Most Recent" />
        <br />
    <asp:RadioButton ID="customTagRadioButton" runat="server" Width="100px" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Tag: " />
        <asp:TextBox ID="CustomTagTextBox" runat="server" Text="Pretty" /><asp:Button ID="ShowTagButton" runat="server" Text="Show" OnClick="ShowTagButton_Clicked" />
        <br />
    <asp:RadioButton ID="customSetNameRadioButton" runat="server" Width="100px" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Gallery Name: " />
        <asp:TextBox ID="CustomSetNameTextBox" runat="server" Text="72157623445479071" /><asp:Button ID="ShowUserNameButton" runat="server" Text="Show" OnClick="ShowUserNameButton_Clicked" />
        <br />
    <asp:RadioButton ID="customEmailTagRadioButton" runat="server" Width="100px" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Email: " />
        <asp:TextBox ID="CustomEmailTagTextBox" runat="server" Enabled="false" Text="IndexItID@example.com" /><asp:Button ID="ShowEmailButton" runat="server" Text="Show" OnClick="ShowEmailButton_Clicked" />
    
    <div style="visible:false;">
        <br />
    <asp:RadioButton ID="customFlickerIdRadioButton" runat="server" Width="100px" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Flicker User ID: "/>
        <asp:TextBox ID="CustomFlickerIdTextBox" runat="server" Text="" /><asp:Button ID="ShowUserIdButton" runat="server" Text="Show" OnClick="ShowUserIdButton_Clicked" />
    </div>
    
        <hr />
</asp:Panel>
<asp:MultiView ID="FlickrWidgetMultiview" runat="server" ActiveViewIndex="0">
<asp:View runat="server" ID="FlickrWidgetProgressView">
    <asp:Label runat="Server" ID="label1" Text="Loading..." Font-Size="smaller" ForeColor="DimGray" />
</asp:View>
<asp:View runat="server" ID="FlickrWidgetPhotoView">
    <asp:Panel ID="photoPanel" runat="server" CssClass="previews">

    </asp:Panel>
    <div style="overflow: auto; width:100%; white-space:nowrap; font-weight: bold; margin-top: 10px">
        <asp:LinkButton ID="ShowPrevious" runat="server" OnClick="ShowPrevious_Click" style="float:left">< Prev Photos</asp:LinkButton>
        <asp:LinkButton ID="ShowNext" runat="server" OnClick="ShowNext_Click" style="float:right">Next Photos ></asp:LinkButton>
    </div>
</asp:View>
</asp:MultiView>

<asp:Timer ID="FlickrWidgetTimer" Interval="100" OnTick="LoadPhotoView" runat="server" /> 
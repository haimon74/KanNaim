<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="StyleManagement.aspx.cs" Inherits="CMS_StyleManagement" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputFontColor.ascx" TagPrefix="uc" TagName="InpuFontColor" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputFontFamily.ascx" TagPrefix="uc" TagName="InpuFontFamily" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputFontSize.ascx" TagPrefix="uc" TagName="InpuFontSize" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputBackgroundColor.ascx" TagPrefix="uc" TagName="InputBackgroundColor" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputBackgroundImage.ascx" TagPrefix="uc" TagName="InputBackgroundImage" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputAlignment.ascx" TagPrefix="uc" TagName="InputAlignment" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputTextDirection.ascx" TagPrefix="uc" TagName="InputTextDirection" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputTextIdentationInPixels.ascx" TagPrefix="uc" TagName="InputTextIdentationInPixels" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputLetterSpacing.ascx" TagPrefix="uc" TagName="InputLetterSpacing" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputWordSpacing.ascx" TagPrefix="uc" TagName="InputWordSpacing" %>

<%@ Register Src="~/UserControls/StyleManagementForm/InputNumber.ascx" TagPrefix="uc" TagName="InputNumber" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputBorderStyle.ascx" TagPrefix="uc" TagName="InputBorderStyle" %>
<%@ Register Src="~/UserControls/StyleManagementForm/InputBorderWidth.ascx" TagPrefix="uc" TagName="InputBorderWidth" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="width:600px; height:600px;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <cc1:TabContainer ID="TabContainerStyle" runat="server" ActiveTabIndex="0"
            Width="600px" >
            <cc1:TabPanel ID="TabPanelFont" runat="server" HeaderText="Font">
                <ContentTemplate>
                    <div id="InputFontStyle" style="width:auto; direction:rtl;">
                        <uc:InpuFontFamily ID="InputFontFamily1" runat="server" />
                        <uc:InpuFontSize ID="InputFontSize1" runat="server" />
                        <wc:IsBoldFontCheckBox ID="IsBoldFontCheckBox1" runat="server" /> <br />
                        <wc:IsItalicFontCheckBox ID="IsItalicFontCheckBox1" runat="server" /> <br />
                        <uc:InpuFontColor ID="InputFontColor1" runat="server" />
                        
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="RadioButtonList2"/>
                            </Triggers>
                            <ContentTemplate>
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal"
                                    onselectedindexchanged="RadioButtonList2_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True">צבע רקע</asp:ListItem>
                                    <asp:ListItem>תמונת רקע</asp:ListItem>
                                </asp:RadioButtonList>
                                <uc:InputBackgroundColor ID="InputBackgroundColor1" runat="server" />
                                <uc:InputBackgroundImage ID="InputBackgroundImage1" runat="server" />             
                            </ContentTemplate>
                        </asp:UpdatePanel>                                                        
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            
            <cc1:TabPanel ID="TabPanelText" runat="server" HeaderText="Text">
                <ContentTemplate>
                    <div id="InputTextStyle" style="width:auto; direction:rtl;">
                        <uc:InputAlignment ID="InputAlignment1" runat="server" />
                        <uc:InputTextDirection ID="InputTextDirection1" runat="server" />
                        <uc:InputTextIdentationInPixels ID="InputTextIdentationInPixels1" runat="server" />
                        <uc:InputLetterSpacing ID="InputLetterSpacing1" runat="server" />
                        <uc:InputWordSpacing ID="InputWordSpacing1" runat="server" />
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            
            <cc1:TabPanel ID="TabPanelPositioning" runat="server" HeaderText="Positioning">
                <ContentTemplate>
                    <div  id="InputPositioningStyle" style="width:auto; direction:rtl;">
                        <uc:InputNumber ID="InputDivPadding" runat="server" LabelText="שוליים פנימיים" RangeMin="-10" RangeMax="200" />
                        <uc:InputNumber ID="InputDivMargin" runat="server" LabelText="שוליים חיצוניים" RangeMin="-10" RangeMax="200" />
                        <uc:InputNumber ID="InputDivWidth" runat="server" LabelText="רוחב" RangeMin="1" RangeMax="1200" />
                        <uc:InputNumber ID="InputDivHeight" runat="server" LabelText="גובה" RangeMin="1" RangeMax="1200" />
                        <uc:InputNumber ID="InputNumber1" runat="server" LabelText="מיקום X" RangeMin="1" RangeMax="1200" />
                        <uc:InputNumber ID="InputNumber2" runat="server" LabelText="מיקום Y" RangeMin="1" RangeMax="1200" />
                        <uc:InputBorderStyle ID="InputBorderStyle1" runat="server" />
                        <uc:InputBorderWidth ID="InputBorderWidth1" runat="server" />
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="TabPanelHyperlink" runat="server" HeaderText="Hyperlink">
                <ContentTemplate>
                    <div  id="InputHyperlinkStyle" style="width:auto; direction:rtl;">
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="TabPanelImage" runat="server" HeaderText="Image">
            </cc1:TabPanel>
        </cc1:TabContainer>
</div>
</asp:Content>


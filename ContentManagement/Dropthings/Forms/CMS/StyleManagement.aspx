<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="StyleManagement.aspx.cs" Inherits="CMS_StyleManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="UserControlsLibrary" Namespace="UserControlsLibrary.WebControls"
    TagPrefix="wc" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputFontColor.ascx" TagPrefix="uc"
    TagName="InputFontColor" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputFontFamily.ascx" TagPrefix="uc"
    TagName="InpuFontFamily" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputFontSize.ascx" TagPrefix="uc"
    TagName="InpuFontSize" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputBackgroundColor.ascx" TagPrefix="uc"
    TagName="InputBackgroundColor" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputImage.ascx" TagPrefix="uc"
    TagName="InputImage" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputAlignment.ascx" TagPrefix="uc"
    TagName="InputAlignment" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputTextDirection.ascx" TagPrefix="uc"
    TagName="InputTextDirection" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputTextIdentationInPixels.ascx"
    TagPrefix="uc" TagName="InputTextIdentationInPixels" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputLetterSpacing.ascx" TagPrefix="uc"
    TagName="InputLetterSpacing" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputWordSpacing.ascx" TagPrefix="uc"
    TagName="InputWordSpacing" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputNumber.ascx" TagPrefix="uc"
    TagName="InputNumber" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputBorderStyle.ascx" TagPrefix="uc"
    TagName="InputBorderStyle" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputBorderWidth.ascx" TagPrefix="uc"
    TagName="InputBorderWidth" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputPositionType.ascx" TagPrefix="uc"
    TagName="InputPositionType" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputLeftOrRight.ascx" TagPrefix="uc"
    TagName="InputLeftOrRight" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputTopOrBottom.ascx" TagPrefix="uc"
    TagName="InputTopOrBottom" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputColorPicker.ascx" TagPrefix="uc"
    TagName="InputColorPicker" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputUrl.ascx" TagPrefix="uc"
    TagName="InputUrl" %>
<%@ Register Src="~/Forms/UserControls/StyleManagementForm/InputRegularExpression.ascx"
    TagPrefix="uc" TagName="InputRegularExpression" %>
<%@ Register src="~/Forms/UserControls/StyleManagementForm/InputImage.ascx" tagname="InputImage" tagprefix="uc11" %>
<%@ Register src="~/Forms/UserControls/StyleManagementForm/InputRoundedBorderStyle.ascx" tagname="InputRoundedBorderStyle" tagprefix="uc12" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 600px; height: 600px;">
        <asp:Label ID="Label1" runat="server" Text="בחר סוג אובייקט"></asp:Label>
        <wc:RadioButtonHtmlObjectType ID="RadioButtonHtmlObjectType" runat="server">
        </wc:RadioButtonHtmlObjectType>
        <br />
        <uc:InputRegularExpression runat="server" ID="InputRegExpCssName" LabelText="שם הסטייל"
            RegularExpression="[a-zA-Z][a-z0-9A-Z]*" TextMaxLength="30" Language="English"
            WatermarkText="שם באנגלית" WatermarkCssClass="watermark" />
        <asp:Button ID="ButtonSave" runat="server" Text="לחץ לשמירה" OnClick="ButtonSave_Click" />
        <cc1:ConfirmButtonExtender ID="ButtonSave_ConfirmButtonExtender" runat="server" ConfirmText="פעולה זאת תשמור סטייל, האם להמשיך ?"
            Enabled="True" TargetControlID="ButtonSave">
        </cc1:ConfirmButtonExtender>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <asp:Button ID="ButtonPreviewStyle" runat="server" Text="הצג סטייל" 
            onclick="ButtonPreviewStyle_Click" />
            <br /> <br />
        <asp:UpdatePanel ID="UpdatePanelPreview" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonPreviewStyle" />
            </Triggers>
            <ContentTemplate>
                <!--div style=" -moz-border-radius: 10px;-webkit-border-radius: 10px;border: 3px solid #00ff00;" --/>
                <!--svg width="100%" height="100%" version="1.1" xmlns="http://www.w3.org/2000/svg">
                <rect x="20" y="20" rx="20" ry="20" width="250" height="100"
                    style="fill:red;stroke:black;stroke-width:5;opacity:0.5" >
                </rect>
                </svg-->
                <!--div style="-moz-border-radius: 10px;-webkit-border-radius: 10px;border: 3px solid #00ff00; behavior: url(../../styles/border-radius.htc);"-->
                <br />
                <asp:Panel ID="Panel1" runat="server" Style="width:auto;height:auto;">
                    אבגד הוזח טיכל מנסע פצקרשת <br />
                    abcdefghijklmnopqrstuvwxyz <br />
                    ABCDEFGHIJKLMNOPQRSTUVWXYZ <br />
                </asp:Panel>
                <cc1:DragPanelExtender ID="DragPanelExtender1" runat="server" TargetControlID="Panel1" EnableViewState="true" />
                <br />                
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <!-- kind of a BUG: On Page Load the active panel must be '0' which is the Input Image as there is a problem with the async file upload control -->
        <cc1:TabContainer ID="TabContainerStyle" runat="server" ActiveTabIndex="0" Width="700px" Style="height:auto;">
            
            <cc1:TabPanel ID="TabPanelImage" runat="server" HeaderText="Image">
                <ContentTemplate>
                    <div id="InputImage" style="width: auto; direction: rtl; text-align: right;">
                        <uc11:InputImage ID="InputImage1" runat="server" />        
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>            
            
            <cc1:TabPanel ID="TabPanelFont" runat="server" HeaderText="Font">
                <ContentTemplate>
                    <div id="InputFontStyle" style="width: auto; direction: rtl; text-align: right;">
                        <uc:InpuFontFamily ID="InputFontFamily1"  EnableViewState="true" runat="server" />
                        <uc:InpuFontSize ID="InputFontSize1" runat="server"  EnableViewState="true" />
                        <wc:IsBoldFontCheckBox ID="IsBoldFontCheckBox1" runat="server" Style="padding-right: 10px" />
                        <br />
                        <wc:IsItalicFontCheckBox ID="IsItalicFontCheckBox1" runat="server" Style="padding-right: 10px" />
                        <br /><br />                        
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RadioButtonList2" />
                                <asp:AsyncPostBackTrigger ControlID="InputFontColor1" />
                                <asp:AsyncPostBackTrigger ControlID="InputBackgroundColor1" />
                            </Triggers>
                            <ContentTemplate>
                                <uc:InputFontColor ID="InputFontColor1" runat="server" />
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True">צבע רקע</asp:ListItem>
                                    <asp:ListItem>תמונת רקע</asp:ListItem>
                                </asp:RadioButtonList>
                                <uc:InputBackgroundColor ID="InputBackgroundColor1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>                        
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            
            <cc1:TabPanel ID="TabPanelText" runat="server" HeaderText="Text">
                <ContentTemplate>
                    <div id="InputTextStyle" style="width: auto; direction: rtl; text-align: right;">
                        <uc:InputAlignment ID="InputAlignment1" runat="server" />
                        <uc:InputTextDirection ID="InputTextDirection1" runat="server" />
                        <uc:InputNumber ID="InputTextIdentationInPixels1" runat="server" LabelText="הזזה לפיסקה"
                            RangeMin="-20" RangeMax="100" />
                        <uc:InputLetterSpacing ID="InputLetterSpacing1" runat="server" />
                        <uc:InputWordSpacing ID="InputWordSpacing1" runat="server" />
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            
            <cc1:TabPanel ID="TabPanelPositioning" runat="server" HeaderText="Positioning">
                <ContentTemplate>
                    <div id="InputPositioningStyle" style="width: auto; height:auto; direction: rtl; text-align: right;">
                        <uc:InputNumber ID="InputDivPadding" runat="server" LabelText="שוליים פנימיים" RangeMin="-10"
                            RangeMax="200" />
                        <uc:InputNumber ID="InputDivMargin" runat="server" LabelText="שוליים חיצוניים" RangeMin="-10"
                            RangeMax="200" />
                        <uc:InputNumber ID="InputDivWidth" runat="server" LabelText="רוחב" RangeMin="1" RangeMax="1200" />
                        <uc:InputNumber ID="InputDivHeight" runat="server" LabelText="גובה" RangeMin="1"
                            RangeMax="1200" />
                        <uc:InputPositionType ID="InputPositionType1" runat="server" />
                        <uc:InputLeftOrRight ID="InputLeftOrRight1" runat="server" />
                        <uc:InputNumber ID="InputNumber1" runat="server" LabelText="מיקום X" RangeMin="1"
                            RangeMax="1200" />
                        <uc:InputTopOrBottom ID="InputTopOrBottom1" runat="server" />
                        <uc:InputNumber ID="InputNumber2" runat="server" LabelText="מיקום Y" RangeMin="1"
                            RangeMax="1200" />
                        <uc:InputBorderStyle ID="InputBorderStyle1" runat="server" />
                        <uc:InputBorderWidth ID="InputBorderWidth1" runat="server" />
                        <uc:InputColorPicker ID="InputColorPickerBorder" runat="server" LabelText="צבע גבול"/>
                        <uc12:InputRoundedBorderStyle ID="InputRoundedBorderStyle1" runat="server" />
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            
            <cc1:TabPanel ID="TabPanelHyperlink" runat="server" HeaderText="Hyperlink">
                <ContentTemplate>
                    <div id="InputHyperlinkStyle" style="width: auto; direction: rtl; text-align: right;">
                        <uc:InputUrl runat="server" ID="InputUrl1"></uc:InputUrl>
                        <uc:InputColorPicker runat="server" ID="InputColorPickerVisitedLink" LabelText="צבע לינק" />
                        <uc:InputColorPicker runat="server" ID="InputColorPicker1" LabelText="לינק היסטורי" />
                        <uc:InputColorPicker runat="server" ID="InputColorPicker2" LabelText="לינק מסומן" />
                        <uc:InputColorPicker runat="server" ID="InputColorPicker3" LabelText="לינק נבחר" />
                        <asp:CheckBox ID="CheckBoxIsImageUrl" Text="סמן לקישור כתמונה" Checked="false" runat="server" />
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
            
        </cc1:TabContainer>
    </div>
</asp:Content>

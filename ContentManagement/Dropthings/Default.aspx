<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Theme="GreenBlue" EnableSessionState="False" ValidateRequest="false"   %>
<%@ OutputCache Location="None" NoStore="true" %>
<%@ Register Src="Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CustomDragDrop" Namespace="CustomDragDrop" TagPrefix="cdd" %>
<%@ Register Src="WidgetContainer.ascx" TagName="WidgetContainer" TagPrefix="widget" %>

<%@ Register src="WidgetPanels.ascx" tagname="WidgetPanels" tagprefix="uc3" %>

    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>IndexIt - First popularity based Index</title>
    <meta name="Description" content="IndexIt is the first popularity based Indexing for local businesses in israel. ">
    <meta name="Keywords" content="IndexIt popularity index business">    
    <meta name="Page-topic" content="Start Page" />
</head>
<body>
<form id="default_form" runat="server">
<!-- A dummy panel to download Ajax Control Toolkit library for drag and drop that CustomDragDrop extender uses -->
<asp:Panel ID="DummyPanel" runat="server" ></asp:Panel>
<ajaxToolkit:DragPanelExtender ID="Dummy" DragHandleID="DummyPanel" TargetControlID="DummyPanel" runat="server"></ajaxToolkit:DragPanelExtender>

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" LoadScriptsBeforeUI="false" ScriptMode="Release">
    <Services>
        <asp:ServiceReference InlineScript="true" Path="PageService.asmx" />  
        <asp:ServiceReference InlineScript="true" Path="ProxyAsync.asmx" />                
        <asp:ServiceReference InlineScript="true" Path="WidgetService.asmx" />                
    </Services>
</asp:ScriptManager>            

<script src="Myframework.js" type="text/javascript"></script>        
<script type="text/javascript">if( typeof Proxy == "undefined" ) window.Proxy = Dropthings.Web.Framework.ProxyAsync;</script>

    <div id="container">
        <!-- Render header first so that user can start typing search criteria while the huge runtime and other scripts download -->
        <uc1:Header ID="Header1" runat="server" />

        <div id="tab_container">
            <asp:UpdatePanel ID="TabUpdatePanel" runat="server" UpdateMode="conditional">
                <ContentTemplate>
            
                <div id="tabs">
                    <ul class="tabs" runat="server" id="tabList">
                    <li class="tab inactivetab"><asp:LinkButton id="Page1Tab" runat="server" Text="Page 1"></asp:LinkButton></li>
                    <li class="tab activetab"><asp:LinkButton id="Page2Tab" runat="server" Text="Page 2"></asp:LinkButton>
                    </li>
                    </ul>            
                </div>        
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
        <div id="onpage_menu">
            <div id="onpage_menu_wrapper">
                <asp:UpdatePanel ID="OnPageMenuUpdatePanel" runat="server" >
                    <ContentTemplate>
                        <div id="onpage_menu_bar" onmouseover="this.className='onpage_menu_bar_hover'" onmouseout="this.className=''">
                            <asp:LinkButton CssClass="onpage_menu_action" ID="ShowAddContentPanel" runat="server" Text="Add stuff »" OnClick="ShowAddContentPanel_Click"/>
                            <asp:LinkButton CssClass="onpage_menu_action" ID="HideAddContentPanel" runat="server" Text="Hide Stuff »" OnClick="HideAddContentPanel_Click" Visible="false" />
                            
                            <asp:LinkButton ID="ChangePageTitleLinkButton" CssClass="onpage_menu_action" Text="Change Settings »" runat="server" OnClick="ChangeTabSettingsLinkButton_Clicked" />
                        </div>
                        <div id="onpage_menu_panels">
                            <asp:Panel ID="ChangePageSettingsPanel" runat="server" Visible="false" CssClass="onpage_menu_panel">
                                <div class="onpage_menu_panel_column">
                                    <h1>Change Tab Title</h1>
                                    <p>
                                        Title: <asp:TextBox ID="NewTitleTextBox" runat="server" />
                                        <asp:Button ID="SaveNewTitleButton" runat="server" OnClick="SaveNewTitleButton_Clicked" Text="Save" />
                                    </p>
                                </div>
                                
                                <div class="onpage_menu_panel_column">
                                    <h1>Delete Tab</h1>
                                    <p>
                                    Delete tab? <asp:Button ID="DeleteTabLinkButton" runat="server" OnClick="DeleteTabLinkButton_Clicked" Text="Yes" />
                                    </p>
                                </div>
                                
                                <div class="onpage_menu_panel_column">
                                    <h1>Change Columns</h1>
                                    
                                    <p>Please choose a column layout:<br />
                                    <input id="SelectLayoutPopup_Type1" type="image" value="1"  src="img/Layout1.jpg" onclick="DropthingsUI.Actions.changePageLayout(1)" /> 
                                    <input id="SelectLayoutPopup_Type2" type="image" value="2" src="img/Layout2.jpg" onclick="DropthingsUI.Actions.changePageLayout(2)" />         
                                    <input id="SelectLayoutPopup_Type3" type="image" value="3" src="img/Layout3.jpg" onclick="DropthingsUI.Actions.changePageLayout(3)" />      
                                    <input id="SelectLayoutPopup_Type4" type="image" value="4" src="img/Layout4.jpg" onclick="DropthingsUI.Actions.changePageLayout(4)" />
                                    </p>                                    
                                    
                                </div>
                            </asp:Panel>
                            
                            <asp:Panel ID="AddContentPanel" runat="Server" CssClass="onpage_menu_panel widget_showcase" Visible="false">
                                <p class="addcontent_message">Click on any of the item to add it to your page:</p>
                                <div class="addcontent_navigation"><asp:LinkButton ID="WidgetListPreviousLinkButton" runat="server" Visible="false" Text="&lt; Previous" OnClick="WidgetListPreviousLinkButton_Click" /> - <asp:LinkButton ID="WidgetListNextButton" runat="server" Visible="false" Text="Next &gt;" OnClick="WidgetListNextButton_Click" /></div>
                                
                                <asp:DataList ID="WidgetDataList" runat="server" RepeatDirection="Vertical" RepeatColumns="5" RepeatLayout="Table" CellPadding="3" CellSpacing="3" EnableViewState="False" ShowFooter="False" ShowHeader="False" Width="100%" >
                                    <ItemTemplate><asp:Image ID="Icon" ImageUrl='<%# Eval("Icon") %>' ImageAlign="AbsMiddle" runat="server" />&nbsp;<asp:LinkButton CommandArgument='<%# Eval("ID") %>' CommandName="AddWidget" ID="AddWidget" runat="server"><%# Eval("Name") %></asp:LinkButton></ItemTemplate>
                                </asp:DataList>                
                            </asp:Panel>            
                        </div>    
                            
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        
        <div id="contents">
            <div id="contents_wrapper">
                <div id="widget_area">
                    <div id="widget_area_wrapper">
                        <asp:UpdatePanel ID="UpdatePanelLayout" runat="server" UpdateMode="conditional">        
                            <ContentTemplate>
                                <uc3:WidgetPanels ID="WidgetPanelsLayout" runat="server" />    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
                        
        <uc2:Footer ID="Footer1" runat="server"></uc2:Footer>
                    
    </div>
    

    <!-- Fades the UI -->
    <div id="blockUI" style="display: none; background-color: black;
    width: 100%; height: 100px; position: absolute; left: 0px; top: 0px; z-index: 50000;     
    -moz-opacity:0.5;opacity:0.5;filter:alpha(opacity=50);"
    onclick="return false" onmousedown="return false" onmousemove="return false"
    onmouseup="return false" ondblclick="return false">&nbsp;</div>        
    
    <textarea id="TraceConsole" rows="10" cols="80" style="display:none"></textarea>
            
    <textarea id="DeleteConfirmPopupPlaceholder" style="display:none">
    &lt;div id="DeleteConfirmPopup"&gt;
        &lt;h1&gt;Delete a Widget&lt;/h1&gt;
        &lt;p&gt;Are you sure you want to delete the widget?&lt;/p&gt;
        &lt;input id="DeleteConfirmPopup_Yes" type="button" value="Yes" /&gt;&lt;input id="DeleteConfirmPopup_No" type="button" value="No" /&gt;
    &lt;/div&gt;    
    </textarea>    
    
     <textarea id="DeletePageConfirmPopupPlaceholder" style="display:none">
    &lt;div id="DeletePageConfirmPopup"&gt;
        &lt;h1&gt;Delete a Page&lt;/h1&gt;
        &lt;p&gt;Are you sure you want to delete the page?&lt;/p&gt;
        &lt;input id="DeletePageConfirmPopup_Yes" type="button" value="Yes" /&gt;&lt;input id="DeletePageConfirmPopup_No" type="button" value="No" /&gt;
    &lt;/div&gt;    
    </textarea>
    
    <textarea id="LayoutPickerPopupPlaceholder" style="display:none">
    
    </textarea>
</form>    
</body>
</html>

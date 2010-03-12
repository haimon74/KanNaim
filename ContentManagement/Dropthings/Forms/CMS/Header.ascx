<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>

<script type="text/javascript">
function doSearch()
{
    var searchQuery = encodeURI(document.getElementById('query').value);
    if( searchQuery.length > 0 ) document.location.href = "http://www.google.com/search?q=" + searchQuery;
    return false;
}
</script>
<div id="header">

    <h1><a href="/">IndexIt - Index your business in our popularity ranking Index</a></h1>

    <div id="login_panel">
        <asp:Label ID="UserNameLabel" runat="server" EnableViewState="false" Text="" Visible="false"></asp:Label><asp:HyperLink
            ID="LoginLinkButton" Text="Log in" runat="server" NavigateUrl="~/LoginPage.aspx" /> | 
        <asp:HyperLink ID="LogoutLinkButton" Text="Log out" runat="server" NavigateUrl="~/Logout.ashx" />
        <asp:HyperLink ID="StartOverButton" Text="Start Over" runat="server" NavigateUrl="~/Logout.ashx" /> | 
        <a id="HelpLink" href="javascript:void(0)" onclick="DropthingsUI.Actions.showHelp()">Help</a>       
    </div>
    
    <div id="search_bar">
        <div id="search_bar_wrapper">
            <div id="google_search">
                <img class="google_logo" src="img/google.jpg" alt="Google" />
                <input id="query" size="40" maxlength="2048" value="" type="text" onkeypress="if( event.keyCode == 13 ) return doSearch(); " />
                <input value="Search" type="button" onclick="return doSearch();" />
            </div>
            
            <div id="live_search">
                <img class="livesearch_logo" src="img/LiveSearch.jpg" alt="Live Search" />

                <!-- Windows Live Search -->
                <!-- Live Search -->
                <meta name="Search.WLSearchBox" content="1.1, en-US" />
                <div id="WLSearchBoxDiv">
                    <table cellpadding="0" cellspacing="0" style="width: 322px"><tr id="WLSearchBoxPlaceholder"><td style="width: 100%; border:solid 2px #4B7B9F;border-right-style: none;"><input id="WLSearchBoxInput" type="text" value="&#x4c;&#x6f;&#x61;&#x64;&#x69;&#x6e;&#x67;&#x2e;&#x2e;&#x2e;" disabled="disabled" style="padding:0;background-image: url(http://search.live.com//siteowner/s/siteowner/searchbox_background.png);background-position: right;background-repeat: no-repeat;height: 16px; width: 100%; border:none 0 Transparent" /></td><td style="border:solid 2px #4B7B9F;"><input id="WLSearchBoxButton" type="image" src="http://search.live.com//siteowner/s/siteowner/searchbutton_normal.png" align="absBottom" style="padding:0;border-style: none" /></td></tr></table>
	                    <script type="text/javascript" charset="utf-8">
	                    var WLSearchBoxConfiguration=
	                    {
		                    "global":{
			                    "serverDNS":"search.live.com",
			                    "market":"en-US"
		                    },
		                    "appearance":{
			                    "autoHideTopControl":false,
			                    "width":600,
			                    "height":400,
			                    "theme":"Green"
		                    },
		                    "scopes":[
			                    {
				                    "type":"web",
				                    "caption":"&#x57;&#x65;&#x62;",
				                    "searchParam":""
			                    }
		                    ]
	                    }
	                    </script>
                    <!-- Moved the search script to the end of the body for defered loading" -->
                </div>
            </div>   
        </div>
        
    </div>

    <div id="header_message">
        <div id="header_message_wrapper">
            
        </div>
    </div>

    <div id="Progress" >
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" DynamicLayout="false" >
        <ProgressTemplate><span><img src="indicator.gif" align="middle" alt="Loading..." /></span></ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <div id="HelpDiv"></div>
</div>


<script type="text/javascript">
document.getElementById('query').focus();
</script>
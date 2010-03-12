// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

var DropthingsUI = {
    Actions : {
        deleteWidget: function(instanceId)
        {
           Dropthings.Web.Framework.WidgetService.DeleteWidgetInstance(instanceId);
           var theDiv = document.getElementById("WidgetPanelsLayout_WidgetContainer" + instanceId + "_Widget");
           theDiv.style.display = "none";
        },

        deletePage: function(pageId)
        {
           Dropthings.Web.Framework.PageService.DeletePage(pageId,DropthingsUI.Actions._onDeletePageComplete);

              var tabButton = document.getElementById("Tab"+pageId);
              var tabList = document.getElementById("tabList");
                tabList.removeChild(tabButton);
        },

        _onDeletePageComplete: function(currentPageName)
        {
            document.location.href = '?' + encodeURI(currentPageName);
            //__doPostBack('UpdatePanelTabAndLayout','');
        },

        changePageLayout: function(newLayout)
        {
            Dropthings.Web.Framework.PageService.ChangePageLayout(newLayout,DropthingsUI.Actions._onChangePageLayoutComplete);
        },

        _onChangePageLayoutComplete: function(arg)
        {
	        document.location.reload();
        },

        newPage: function(newLayout)
        {
            Dropthings.Web.Framework.PageService.NewPage(newLayout, DropthingsUI.Actions._onNewPageComplete);
        },

        _onNewPageComplete: function(newPageName)
        {
            document.location.href = '?' + encodeURI(newPageName);
           //__doPostBack('UpdatePanelTabAndLayout','');
        },
        
        minimizeWidget : function(divId)
        {
            var div = $get(divId);
            div.style.display = "none";
        },

        renamePage: function(newLabel)
        {
            var newPageName = document.getElementById(newLabel).value;
            Dropthings.Web.Framework.PageService.RenamePage(newPageName, DropthingsUI.Actions._onRenamePageComplete);
        },
        
        _onRenamePageComplete: function()
        {
            __doPostBack('TabUpdatePanel','');
        },

        changePage: function(pageId, pageName)
        {
            //Dropthings.Web.Framework.PageService.ChangeCurrentPage(pageId, OnChangePageComplete);
            document.location.href = '?' + encodeURI(pageName);
        },

        _onChangePageComplete: function(arg)
        {
           __doPostBack('UpdatePanelTabAndLayout','');
        },

        onDrop: function( sender, e )
        {
            var container = e.get_container();
            var item = e.get_droppedItem();
            var position = e.get_position();
            
            //alert( String.format( "Container: {0}, Item: {1}, Position: {2}", container.id, item.id, position ) );
            
            var instanceId = parseInt(item.getAttribute("InstanceId"));
            var columnNo = parseInt(container.getAttribute("columnNo"));
            var row = position;
            Dropthings.Web.Framework.WidgetService.MoveWidgetInstance( instanceId, columnNo, row );
        },

        hide: function(id)
        {
            document.getElementById(id).style.display="none";
        },

        showHelp: function()
        {
            var request = new Sys.Net.WebRequest();
            request.set_httpVerb("GET");
            request.set_url('help.aspx');
            request.add_completed( function( executor )
            {
                if (executor.get_responseAvailable()) 
                {
                    var helpDiv = $get('HelpDiv');
                    var helpLink = $get('HelpLink');
                    
                    var helpLinkBounds = Sys.UI.DomElement.getBounds(helpLink);
                    
                    helpDiv.style.top = (helpLinkBounds.y + helpLinkBounds.height) + "px";
                    
                    var content = executor.get_responseData();
                    helpDiv.innerHTML = content;
                    helpDiv.style.display = "block";                       
                }
            });
            
            var executor = new Sys.Net.XMLHttpExecutor();
            request.set_executor(executor); 
            executor.executeRequest();
        }
    }
};

var Utility = 
{
    // change to display:none
    nodisplay : function(e) 
    { 
        if( typeof e == "object") e.style.display = "none"; else if( $get(e) != null ) $get(e).style.display = "none"; 
    },
    // change to display:block
    display : function (e,inline) 
    { 
        if( typeof e == "object") e.style.display = (inline?"inline":"block"); else if( $get(e) != null ) $get(e).style.display = (inline?"inline":"block"); 
    },
    getContentHeight : function()
        {
        if( document.body && document.body.offsetHeight ) {
            return document.body.offsetHeight;
        }
    },


    blockUI : function()
    {
        Utility.display('blockUI');
        var blockUI = $get('blockUI');
    
        if( blockUI != null ) // it will be null if called from CompactFramework
        blockUI.style.height = Math.max( Utility.getContentHeight(), 1000) + "px";    
    },

    unblockUI : function()
    {
        Utility.nodisplay('blockUI');
    }
};

var DeleteWarning =
{
    yesCallback : null,
    noCallback : null,
    _initialized : false,
    init : function()
    {
        if( DeleteWarning._initialized ) return;
        
        var hiddenHtmlTextArea = $get('DeleteConfirmPopupPlaceholder');
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);
        
        DeleteWarning._initialized = true;
    },
    show : function( yesCallback, noCallback )
    {
        DeleteWarning.init();
        
        Utility.blockUI();
        
        var popup = $get('DeleteConfirmPopup');
        Utility.display(popup);
        
        DeleteWarning.yesCallback = yesCallback;
        DeleteWarning.noCallback = noCallback;
        
        $addHandler( $get("DeleteConfirmPopup_Yes"), 'click', DeleteWarning._yesHandler );
        $addHandler( $get("DeleteConfirmPopup_No"), 'click', DeleteWarning._noHandler );
    },
    hide : function()
    {
        DeleteWarning.init();
        
        var popup = $get('DeleteConfirmPopup');
        Utility.nodisplay(popup);
        
        $clearHandlers( $get('DeleteConfirmPopup_Yes') );
        
        Utility.unblockUI();
        
    },
    _yesHandler : function()
    {
        DeleteWarning.hide();
        DeleteWarning.yesCallback();    
    },
    _noHandler : function()
    {
        DeleteWarning.hide();
        DeleteWarning.noCallback();
    }
};

var DeletePageWarning =
{
    yesCallback : null,
    noCallback : null,
    _initialized : false,
    init : function()
    {
        if( DeletePageWarning._initialized ) return;
        
        var hiddenHtmlTextArea = $get('DeletePageConfirmPopupPlaceholder');
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);
        
        DeletePageWarning._initialized = true;
    },
    show : function( yesCallback, noCallback )
    {
        DeletePageWarning.init();
        
        Utility.blockUI();
        
        var popup = $get('DeletePageConfirmPopup');
        Utility.display(popup);
        
        DeletePageWarning.yesCallback = yesCallback;
        DeletePageWarning.noCallback = noCallback;
        
        $addHandler( $get("DeletePageConfirmPopup_Yes"), 'click', DeletePageWarning._yesHandler );
        $addHandler( $get("DeletePageConfirmPopup_No"), 'click', DeletePageWarning._noHandler );
    },
    hide : function()
    {
        DeletePageWarning.init();
        
        var popup = $get('DeletePageConfirmPopup');
        Utility.nodisplay(popup);
        
        $clearHandlers( $get('DeletePageConfirmPopup_Yes') );
        
        Utility.unblockUI();
        
    },
    _yesHandler : function()
    {
        DeletePageWarning.hide();
        DeletePageWarning.yesCallback();    
    },
    _noHandler : function()
    {
        DeletePageWarning.hide();
        DeletePageWarning.noCallback();
    }
};

function winopen(url, w, h) 
{
  var left = (screen.width) ? (screen.width-w)/2 : 0;
  var top  = (screen.height) ? (screen.height-h)/2 : 0;

  window.open(url, "_blank", "width="+w+",height="+h+",left="+left+",top="+top+",resizable=yes,scrollbars=yes");
  
  return;
}

function winopen_withlocationbar(url) 
{
 var w = screen.width / 2;
 var h = screen.height /2;
  var left = (screen.width) ? (screen.width-w)/2 : 0;
  var top  = (screen.height) ? (screen.height-h)/2 : 0;

  window.open(url, "_blank");
  
  return;
}

function winopen2(url,target, w, h) 
{
  var left = (screen.width) ? (screen.width-w)/2 : 0;
  var top  = (screen.height) ? (screen.height-h)/2 : 0;

 if(popupWin_2[target] != null)
	if(!popupWin_2[target].closed)
		popupWin_2[target].focus();
	else
		popupWin_2[target] = window.open(url, target, "width="+w+",height="+h+",left="+left+",top="+top+",resizable=yes,scrollbars=yes");
  else
	popupWin_2[target] = window.open(url, target, "width="+w+",height="+h+",left="+left+",top="+top+",resizable=yes,scrollbars=yes");
  
  return;
}


var LayoutPicker =
{
    yesCallback : null,
    noCallback : null,
    type1Callback:null,
    type2Callback:null,
    type3Callback:null,
    type4Callback:null,
    _initialized : false,
    clientID :null,
    init : function()
    {
        if( LayoutPicker._initialized ) return;

        var hiddenHtmlTextArea = $get('LayoutPickerPopupPlaceholder');
        
        var html = hiddenHtmlTextArea.value;
        var div = document.createElement('div');
        div.innerHTML = html;
        document.body.appendChild(div);
        
        LayoutPicker._initialized = true;
    },
    show : function( Type1Callback,Type2Callback,Type3Callback,Type4Callback, noCallback, clientID )
    {   
        LayoutPicker.init();
        
        Utility.blockUI();
        
        var popup = $get('LayoutPickerPopup');
        Utility.display(popup);
        
        LayoutPicker.type1Callback= Type1Callback;
        LayoutPicker.type2Callback= Type2Callback;
        LayoutPicker.type3Callback= Type3Callback;
        LayoutPicker.type4Callback= Type4Callback;
        LayoutPicker.clientID = clientID;
        LayoutPicker.noCallback = noCallback;
        
        $addHandler( $get("SelectLayoutPopup_Cancel"), 'click', LayoutPicker._noHandler );
        $addHandler( $get("SelectLayoutPopup_Type1"), 'click', LayoutPicker._type1Handler );
        $addHandler( $get("SelectLayoutPopup_Type2"), 'click', LayoutPicker._type2Handler );
        $addHandler( $get("SelectLayoutPopup_Type3"), 'click', LayoutPicker._type3Handler );
        $addHandler( $get("SelectLayoutPopup_Type4"), 'click', LayoutPicker._type4Handler );
        
    },
    hide : function()
    {
        LayoutPicker.init();

        
        var popup = $get('LayoutPickerPopup');
        Utility.nodisplay(popup);
        //is there a cleaner way to clear the handlers?
        $clearHandlers( $get('SelectLayoutPopup_Type1') );
        $clearHandlers( $get('SelectLayoutPopup_Type2') );
        $clearHandlers( $get('SelectLayoutPopup_Type3') );
        $clearHandlers( $get('SelectLayoutPopup_Type4') );
        
        Utility.unblockUI();
        
    },

    _type1Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type1Callback();  
    },
    _type2Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type2Callback();    
    },
    _type3Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type3Callback();
    },
    _type4Handler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.type4Callback();
    },

    _noHandler : function()
    {
        LayoutPicker.hide();
        LayoutPicker.noCallback();
    }
};


function pageUnload()
{
    
}

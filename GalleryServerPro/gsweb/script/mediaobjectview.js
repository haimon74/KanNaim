var _timer = null; // Used in slideshow
var _ssInProgress = false; // Slideshow is running
var _moInfo; // Client version of server class MediaObjectWebEntity
var _moCacheImg = new Image(); // Prefetches next MO so it is already downloaded when user clicks next
var _inPrefetch = true; // Set to true during prefetch callback to server
var _nextMORequested = false; // Set to true when user requests next MO but callback hasn't been returned from prefetching the data
var _prevMORequested = false; // Set to true when user requests previous MO but callback hasn't been returned from prefetching the data
var _showCurrentMO = false; // Set to true when user requests the current MO to be re-rendered, possibly with new settings (like hi-res)
var _fadeInMoAnimation;
var _mediaObjectsToDispose = new Array(); // Contains AJAX components used to display a media object and should be disposed when moving to next/previous

function tbMediaObjectActions_onItemCheckChange(sender, eventArgs)
{
	var toolbarItem = eventArgs.get_item();
	switch(toolbarItem.get_id())
	{
		case "tbiInfo": toggleInfo(toolbarItem);	break;
		case "tbiViewHiRes": toggleHiRes(toolbarItem);	break;
		case "tbiPermalink": togglePermalink(toolbarItem);	break;
	}
}

function tbMediaObjectActions_onItemSelect(sender, eventArgs)
{
	var toolbarItem = eventArgs.get_item();
	switch(toolbarItem.get_id())
	{
		case "tbiSlideshow": toggleSlideshow(toolbarItem);	break;
		case "tbiDelete": deleteObject(toolbarItem);	break;
	}
}
	
function toggleInfo(toolbarItem)
{
	if(dgMediaObjectInfo.get_isShowing())
	{
		dgMediaObjectInfo.Close();
	}
	else
	{
		setMediaObjectInfoDialogSize();			
		refreshMetadata($get('moid').value);
		dgMediaObjectInfo.Show();
	}

	updateProfile(toolbarItem.get_checked());
}
	
function toggleSlideshow(toolbarItem)
{
	// Toggle the slideshow icon.
	if (toolbarItem.get_imageUrl() == 'play.png')
	{
		_timer = new Gsp.Timer(showNextMediaObject);
		_timer.set_interval(_ssDelay);
		_ssInProgress = true;
		toolbarItem.set_imageUrl('pause.png');
		_timer.start();
		showNextMediaObject();
	}
	else
	{
		if (_timer)
		{
			_timer.stop();
			_ssInProgress = false;
		}
		toolbarItem.set_imageUrl('play.png');
	}
}

function updateProfile(showMetadata)
{
	Sys.Services.ProfileService.properties["ShowMediaObjectMetadata"] = showMetadata;
	Sys.Services.ProfileService.save(null, null, null, null);
}

function setMediaObjectInfoDialogSize()
{
	if (_inPrefetch)
	{
		window.setTimeout("setMediaObjectInfoDialogSize()", 100);
		return;
	}
	var minWidth = 200;
	var defaultWidth = 400;
	var maxWidth = 600;
	var minHeight = 400;
	var maxHeight = document.body.clientHeight ? document.body.clientHeight : 600;
	var xBuffer = 52; // Horizontal buffer space
	var dgWidth = document.body.clientWidth - _moInfo.Width - xBuffer;
	var dgHeight = _moInfo.Height;
	
	if ($get('hr').value.toLowerCase() == "true")
	{ 
		dgWidth = defaultWidth; /* Viewing hi-res, so use default */ 
	}
	else if (dgWidth < minWidth) dgWidth = minWidth;
	else if (dgWidth > maxWidth) dgWidth = maxWidth;
	
	if (dgHeight < minHeight) dgHeight = minHeight;
	else if (dgHeight > maxHeight) dgHeight = maxHeight;
	
	dgMediaObjectInfo.set_width(dgWidth);
	dgMediaObjectInfo.set_height(dgHeight);
}

function getDisplayType()
{
	// (0=Unknown,1=Thumbnail,2=Optimized,3=Original)
	if (_viewHiRes)
		return 3;
	else
		return 2;
}

function showMediaObject(id)
{
	if (typeof(dgEditCaption) != 'undefined')
	{
		if (dgEditCaption.get_isShowing()) dgEditCaption.close();
	}

	_showCurrentMO = true;
	_inPrefetch = true;
	PageMethods.GetMediaObjectHtml(id, getDisplayType(), getMediaObjectHtmlCompleted, getMediaObjectHtmlFailure);
}

function getMediaObjectHtmlCompleted(results, context, methodName)
{
	_moInfo = results;
	if (results.NextSSUrl && results.NextSSUrl.length > 0)
		_moCacheImg.src = results.NextSSUrl;
	_inPrefetch = false;

	if (_nextMORequested)
	{
		_nextMORequested = false;
		
		if (_ssInProgress && !_timer.isRunning)
		{
			// We may have stopped the timer in showNextMediaObject() because the timer is ticking faster than our callback
			// can complete. Now that the callback is done, we can restart the timer to resume the slideshow.
			_timer.start();
		}
			
		showNextMediaObject();
	}
	
	if (_prevMORequested)
	{
		_prevMORequested = false;
		showPrevMediaObject();
	}
	
	if (_showCurrentMO)
	{
		_showCurrentMO = false;
		showCurrentMediaObject();
	}
}

function listComponents()
{
	// Debug use: Show current components. Better not be a Silverlight mediaplayer when attempting to create a new one.
  var c = Sys.Application.getComponents();
  var s = "";
  for (var i=0; i<c.length; i++) {
      var id = c[i].get_id();
      var type = Object.getType(c[i]).getName();
      s += 'Item ' + i + ': id=' + id + ', type=' + type + '\n';
  }
  alert(s);
}

function disposeAjaxObject(element)
{
	var o = $find(element);
	if (o) o.dispose();
}

function showCurrentMediaObject()
{
	//listComponents();
	Array.forEach(_mediaObjectsToDispose, disposeAjaxObject);
	Array.clear(_mediaObjectsToDispose);
	//listComponents();
	
	$get(_mo).innerHTML = _moInfo.HtmlOutput;
	if (_moInfo.ScriptOutput.length > 0)
	{
		eval(_moInfo.ScriptOutput);
	}
		
	$get(_moTitle).innerHTML = _moInfo.Title;
	$get('lblMoPosition').innerHTML = _moInfo.Index + 1;
	$get('lblMoCount').innerHTML = _moInfo.NumObjectsInAlbum;
	
	if (_tbIsVisible)
	{
		var pLink = tbMediaObjectActions.get_items().getItemById('tbiPermalink');
		if (pLink) pLink.set_checked(false);
		Sys.UI.DomElement.removeCssClass($get('divPermalink'), "visible");
		Sys.UI.DomElement.addCssClass($get('divPermalink'), "invisible");
		
		var tbiHiRes = tbMediaObjectActions.get_items().getItemById('tbiViewHiRes');
		if (tbiHiRes)
		{
			tbiHiRes.set_visible(_moInfo.HiResAvailable);

			if (tbiHiRes.get_checked())
			{
				$get('divMoView').style.width = _moInfo.Width + "px";
			}
		}

		var tbiDownload = tbMediaObjectActions.get_items().getItemById('tbiDownload');
		if (tbiDownload)
		    tbiDownload.set_visible(_moInfo.IsDownloadable);
	}
		
	document.body.style.cursor = "default";
}

function showNextMediaObject()
{
	if (_inPrefetch)
	{
		// A PageMethods.GetMediaObjectHtml() is in progress. Set flag so the callback method can re-execute
		// this method when it is complete.
		document.body.style.cursor = "wait";
		_nextMORequested = true;
		
		if (_ssInProgress) // Timer ticking faster than our prefetch can complete. Temporarily stop timer. We'll resume in callback complete method.
			_timer.stop();
			
		return;
	}
		
	//listComponents();
	Array.forEach(_mediaObjectsToDispose, disposeAjaxObject);
	Array.clear(_mediaObjectsToDispose);
	//listComponents();

	if ((_moInfo.NextId == 0) || (_ssInProgress && (_moInfo.NextSSId == 0))) { redirectToHomePage(); return; }
			
	document.body.style.cursor = "wait";

	if (typeof(dgEditCaption) != 'undefined')
	{
		if (dgEditCaption.get_isShowing()) dgEditCaption.close();
	}

	$get('moid').value = (_ssInProgress ? _moInfo.NextSSId : _moInfo.NextId);
	$get(_mo).innerHTML = (_ssInProgress ? _moInfo.NextSSHtmlOutput : _moInfo.NextHtmlOutput);

	if ((_ssInProgress) && (_moInfo.NextSSScriptOutput.length > 0))
		eval(_moInfo.NextSSScriptOutput);
	else if ((!_ssInProgress) && (_moInfo.NextScriptOutput.length > 0))
		eval(_moInfo.NextScriptOutput);

	var moImg = $get('mo_img');
	if (moImg)
	{
		moImg.src = _moCacheImg.src;
		if (_fadeInMoAnimation)
		{
			_fadeInMoAnimation.set_target(moImg);
			_fadeInMoAnimation.play();
		}
	}

	$get(_moTitle).innerHTML = (_ssInProgress ? _moInfo.NextSSTitle : _moInfo.NextTitle);
	$get('lblMoPosition').innerHTML = (_ssInProgress ? _moInfo.NextSSIndex + 1 : _moInfo.Index + 2);
	$get('lblMoCount').innerHTML = _moInfo.NumObjectsInAlbum;
	
	if (_tbIsVisible)
	{
		var pLink = tbMediaObjectActions.get_items().getItemById('tbiPermalink');
		if (pLink) pLink.set_checked(false);
		Sys.UI.DomElement.removeCssClass($get('divPermalink'), "visible");
		Sys.UI.DomElement.addCssClass($get('divPermalink'), "invisible");
				
		var tbiHiRes = tbMediaObjectActions.get_items().getItemById('tbiViewHiRes');
		if (tbiHiRes)
		{
			if (_ssInProgress)
				tbiHiRes.set_visible(_moInfo.NextSSHiResAvailable);
			else
				tbiHiRes.set_visible(_moInfo.NextHiResAvailable);

			if (tbiHiRes.get_checked())
			{
				$get('divMoView').style.width = (_ssInProgress ? _moInfo.NextSSWidth : _moInfo.NextWidth) + "px";
			}
		}
		
    var tbiDownload = tbMediaObjectActions.get_items().getItemById('tbiDownload');
    if (tbiDownload) 
    {
    	if (_ssInProgress)
    		tbiDownload.set_visible(_moInfo.NextSSIsDownloadable);
    	else
    		tbiDownload.set_visible(_moInfo.NextIsDownloadable);
    }
	}

	if(dgMediaObjectInfo.get_isShowing())
	{
		if (_ssInProgress)
			refreshMetadata(_moInfo.NextSSId);
		else
			refreshMetadata(_moInfo.NextId);
	}
		
	document.body.style.cursor = "default";

	// Pre-fetch data in anticipation of the next request
	_inPrefetch = true;
	var moidToPrefetch = (_ssInProgress ? _moInfo.NextSSId : _moInfo.NextId);
	PageMethods.GetMediaObjectHtml(moidToPrefetch, getDisplayType(), getMediaObjectHtmlCompleted, getMediaObjectHtmlFailureOnNavigate);
}

function getMediaObjectHtmlFailureOnNavigate(error, context, methodName)
{
  if (error.get_exceptionType() == "GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException")
  {
    // The next/previous media object may have been deleted. Try again with the current ID.
    PageMethods.GetMediaObjectHtml(_moInfo.Id, getDisplayType(), getMediaObjectHtmlCompleted, getMediaObjectHtmlFailure);
  }
  else
  {
    alert(error.get_exceptionType() + ": " + error.get_message());
    _inPrefetch = false;
  }
}

function getMediaObjectHtmlFailure(error, context, methodName)
{
  if (error.get_exceptionType() == "GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException")
    alert(error.get_message());
  else
    alert(error.get_exceptionType() + ": " + error.get_message());

  _inPrefetch = false;
}

function showPrevMediaObject()
{
	if (_inPrefetch)
	{
		// A PageMethods.GetMediaObjectHtml() is in progress. Set flag so the callback method can re-execute
		// this method when it is complete.
		document.body.style.cursor = "wait";
		_prevMORequested = true;
		return;
	}
	
	//listComponents();
	Array.forEach(_mediaObjectsToDispose, disposeAjaxObject);
	Array.clear(_mediaObjectsToDispose);
	//listComponents();

	if (_moInfo.PrevId == 0) { redirectToHomePage(); return; }
	
	document.body.style.cursor = "wait";

	if (typeof(dgEditCaption) != 'undefined')
	{
		if (dgEditCaption.get_isShowing()) dgEditCaption.close();
	}

	$get('moid').value = _moInfo.PrevId;
	$get(_mo).innerHTML = _moInfo.PrevHtmlOutput;
	if (_moInfo.PrevScriptOutput.length > 0)
	{
		eval(_moInfo.PrevScriptOutput);
	}

	var moImg = $get('mo_img');
	if (moImg && _fadeInMoAnimation)
	{
		_fadeInMoAnimation.set_target(moImg);
		_fadeInMoAnimation.play();
	}
	$get(_moTitle).innerHTML = _moInfo.PrevTitle;
	$get('lblMoPosition').innerHTML = _moInfo.Index;
	$get('lblMoCount').innerHTML = _moInfo.NumObjectsInAlbum;
									
	if (_tbIsVisible)
	{
		var pLink = tbMediaObjectActions.get_items().getItemById('tbiPermalink');
		if (pLink) pLink.set_checked(false);
		Sys.UI.DomElement.removeCssClass($get('divPermalink'), "visible");
		Sys.UI.DomElement.addCssClass($get('divPermalink'), "invisible");

		var tbiHiRes = tbMediaObjectActions.get_items().getItemById('tbiViewHiRes');
		if (tbiHiRes)
		{
			tbiHiRes.set_visible(_moInfo.PrevHiResAvailable);

			if (tbiHiRes.get_checked())
			{
				$get('divMoView').style.width = _moInfo.PrevWidth + "px";
			}
		}
		
		var tbiDownload = tbMediaObjectActions.get_items().getItemById('tbiDownload');
		if (tbiDownload)
			tbiDownload.set_visible(_moInfo.PrevIsDownloadable);
	}

	if(dgMediaObjectInfo.get_isShowing())
		refreshMetadata(_moInfo.PrevId);
		
	document.body.style.cursor = "default";

	// Pre-fetch data in anticipation of the next request
	_inPrefetch = true;
	PageMethods.GetMediaObjectHtml(_moInfo.PrevId, getDisplayType(), getMediaObjectHtmlCompleted, getMediaObjectHtmlFailureOnNavigate);
}

function dgMediaObjectInfo_OnClose(sender, eventArgs)
{
	if (_tbIsVisible)
	{
		var info = tbMediaObjectActions.get_items().getItemById('tbiInfo');
		if (info) info.set_checked(false);
	}
	updateProfile(false);
}

function refreshMetadata(moid)
{
  PageMethods.GetMetadataItems(moid, refreshMetadataCompleted);
}

function refreshMetadataCompleted(result)
{
	gdmeta.load(result);
	gdmeta.render();
}

function gdmeta_onLoad(sender, eventArgs)
{
	var info = tbMediaObjectActions.get_items().getItemById('tbiInfo');
	if ((info) && info.get_checked())
	{{
		setMediaObjectInfoDialogSize();			
		gdmeta.render();
		dgMediaObjectInfo.Show();
	}}
}

function setDialogSize(dialog, width, height)
{
	// Set width & height of CA dialog control.
	var dialogDiv = document.getElementById(dialog.get_id());
	dialog.beginUpdate();
	dialog.Width = width; // Doesn't work in Firefox, so hack below is needed (for height, too)
	dialog.Height = height;
	if (Sys.Browser.agent != Sys.Browser.InternetExplorer)
	{
		// Only needed for Firefox (maybe others, too?)
		dialogDiv.style.width = width + "px";
		dialogDiv.style.height = height + "px";
	}
	dialog.endUpdate();
}

Sys.Application.notifyScriptLoaded();
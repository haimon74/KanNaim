using System;
using System.Collections.Generic;
using System.Globalization;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.task
{
	public partial class synchronize : GspPage
	{
		#region Private Fields

		private static ISynchronizationStatus _synchStatus = SynchronizationStatus.Instance;

		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.Synchronize);

			if (!IsPostBack)
			{
				ConfigureControls();
					
				RegisterJavascript();
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Synch_Header_Text;
			Master.TaskBody = "";
			Master.OkButtonText = Resources.GalleryServerPro.Task_Synch_Ok_Button_Text;
			Master.OkButtonToolTip = Resources.GalleryServerPro.Task_Synch_Ok_Button_Tooltip;

			Master.OkButtonTop.OnClientClick = "startSynch();return false;";
			Master.OkButtonBottom.OnClientClick = "startSynch();return false;";

			dgSynch.AnimationDirectionElement = Master.OkButtonBottom.ClientID;

			lblAlbumTitle.Text = this.GetAlbum().Title;

			if (!Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.EnableImageMetadata)
			{
				chkRegenerateMetadata.Enabled = false;
				chkRegenerateMetadata.CssClass = "disabledtext";
			}
		}

		private void RegisterJavascript()
		{
			string taskSynchProgressSkippedObjectsMaxExceeded_Msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Synch_Progress_Skipped_Objects_Max_Exceeded_Msg, GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch);
			
			string script = String.Format(CultureInfo.InvariantCulture, @"

var _progressManager = null;
var _synchId = '{0}';
var _albumId = {1};
var _synchStartTime;
var _totalFiles;
var _curFile = '';
var _curFileIndex;
var _headerMsg;
var _errorMsg = '';
var _synchFailed = false;
var _synchCancelled = false;

function startSynch()
{{
	document.body.style.cursor = 'wait';

	_synchStartTime = new Date();
	PageMethods.Synchronize(_albumId, _synchId, $get('chkIncludeChildAlbums').checked, $get('chkOverwriteThumbnails').checked, $get('chkOverwriteCompressed').checked, $get('{2}').checked, synchCompleted, synchFailed);

	setText($get('synchPopupHeader'), '{3}');
	setText($get('status'), '{4}');
	setText($get('synchEtl'), '');
	setText($get('synchRate'), '');
	$get('errorMessage').innerHTML = '';
	$get('progressbar').style.width = '1%';
	$get('btnCancel').disabled = false;
	$get('btnClose').disabled = true;
	dgSynch.Show();
	if ($get('synchAnimation').style.visibility == 'hidden') $get('synchAnimation').style.visibility= 'visible';
	_progressManager.startMonitor(_synchId, 2000, checkProgressStarted, checkProgressComplete, updateProgressCompleted);
}}

function checkProgressStarted()
{{
	document.body.style.cursor = 'wait';
}}

function checkProgressComplete(results)
{{
	document.body.style.cursor = 'default';
	if (results.SynchId != _synchId)
		return;
		
	if (_totalFiles == null) _totalFiles = results.TotalFileCount;
	_curFileIndex = (results.Status == 'SynchronizingFiles' ? results.CurrentFileIndex : _totalFiles);	
	if (results.CurrentFile != null)
		_curFile = results.CurrentFile;
	
	$get('progressbar').style.width = results.PercentComplete + '%';
	setText($get('synchEtl'), calculateSynchEtl());
	setText($get('synchRate'), calculateSynchRate());
	setText($get('status'), results.StatusForUI);
	$get('errorMessage').innerHTML = '<p class=\'fss\'>{21} <span class=\'msgfriendly\'>' + _curFile + '</span></p>';
}}

function synchFailed(results, context, methodName)
{{
	document.body.style.cursor = 'default';
	if (results.get_exceptionType() == 'GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException')
	{{
		_headerMsg = '{5}';
		_errorMsg = '<p class=\'msgwarning\'>{6}</p>';
	}}
	else
	{{
		_headerMsg = '{7}';
		_errorMsg = _errorMsg + '<p class=\'fss\'>{21} <span class=\'msgfriendly\'>' + _curFile + '</span></p><p class=\'msgwarning fss\'>{8}' + results.get_exceptionType() + ': ' + results.get_message() + '</p><p class=\'fss\'>{22}</p><p class=\'fss\'>' + results.get_stackTrace() + '</p>';
	}}
	_progressManager.stopMonitor();
	_synchFailed = true;
}}

function synchCompletedUpdateUI(results, context, methodName)
{{
	// Synch finished! Update the UI.
	document.body.style.cursor = 'default';
	if (_totalFiles == null) _totalFiles = results.TotalFileCount;

	if (!_synchFailed && !_synchCancelled)
	{{
		_headerMsg = '{9}';
		_curFileIndex = _totalFiles;
		if (_curFileIndex)
		{{
			setText($get('synchEtl'), calculateSynchEtl());
			setText($get('synchRate'), calculateSynchRate());
		}}
		if (results.SkippedFiles.length > 0)
		{{
			var skippedFiles = results.SkippedFiles;
			var sb = new Sys.StringBuilder();
			
			if (results.SkippedFiles.length >= {23})
				sb.append('<p class=\'msgwarning fss\'>{24}');
			else
				sb.append('<p class=\'msgwarning fss\'>{10}' + skippedFiles.length + '{11}</p>');

			sb.append('<ul class=\'fss\'>');
			for (var i = 0; i < skippedFiles.length; i++)
			{{
				var skippedFile = skippedFiles[i];
				sb.append('<li>' + skippedFile.Key + ': <span class=\'msgdark\'>' + skippedFile.Value + '</span></li>');
			}}
			sb.append('</ul>');
			sb.append('<p class=\'msgfriendly fss\'>{12}</p>');
			_errorMsg = sb.toString();
		}}
	}}
	setText($get('status'), _headerMsg);
	setText($get('synchPopupHeader'), _headerMsg);
	$get('errorMessage').innerHTML = _errorMsg;
		
	$get('progressbar').style.width = '100%';
	$get('synchAnimation').style.visibility = 'hidden';
	$get('btnCancel').disabled = true;
	$get('btnClose').disabled = false;
	$get('btnClose').focus();
	_curFileIndex = null;
	_totalFiles = null;
	_headerMsg = '';
	_errorMsg = '';
	_synchFailed = false;
	_synchCancelled = false;
}}

function calculateSynchRate()
{{
	if (_curFileIndex == 0) return '';
	
	var synchRate = _curFileIndex / getElapsedTimeSeconds();
	return (Math.round(synchRate * 10) / 10).toFixed(1) + '{13}';
}}

function calculateSynchEtl()
{{
	if (_curFileIndex == 0) return '';
	if (_totalFiles == 0) return '{14}';
	
	var elapsedTime = getElapsedTimeSeconds();
	var estimatedTotalTime = (elapsedTime * _totalFiles) / _curFileIndex;
	var timeLeft = new Date(0,0,0,0,0,0,(estimatedTotalTime - elapsedTime) * 1000);
	var etl = '';
	if (timeLeft.getHours() > 0) etl = timeLeft.getHours() + '{15}';

	etl = etl + timeLeft.getMinutes() + '{16}' + timeLeft.getSeconds() + '{17}' + _curFileIndex + '{18}' + _totalFiles + '{19}';
	return etl;
}}

function cancelSynch()
{{
	_progressManager.abortTask(_synchId);

	_headerMsg = '{5}';
	_errorMsg = '<p class=\'msgwarning\'>{20}</p>';
	_synchCancelled = true;
}}

function synchPageLoad(sender, args)
{{
	_progressManager = new Gsp.Progress();
}}

function synchCompleted(results, context, methodName)
{{
	document.body.style.cursor = 'default';
	_progressManager.stopMonitor();
}}

function updateProgressCompleted()
{{
	document.body.style.cursor = 'wait';
	PageMethods.GetCurrentStatus(synchCompletedUpdateUI, synchCompletedUpdateUIFailure);
}}

function synchCompletedUpdateUIFailure(results, context, methodName)
{{
	document.body.style.cursor = 'default';
	alert(results.get_message());
}}

function closeSynchWindow()
{{
	dgSynch.close();
}}

function getElapsedTimeSeconds()
{{
	return ((new Date().getTime() - _synchStartTime.getTime()) / 1000);
}}

function setText(node, newText)
{{
	var childNodes = node.childNodes;
	for (var i=0; i < childNodes.length; i++)
	{{
		node.removeChild(childNodes[i]);
	}}
	if ((newText != null) && (newText.length > 0))
		node.appendChild(document.createTextNode(newText));
}}

",
				Guid.NewGuid().ToString(), // 0
				GetAlbum().Id, // 1
				chkRegenerateMetadata.ClientID, // 2
				Resources.GalleryServerPro.Task_Synch_Progress_SynchInProgress_Hdr, // 3
				Resources.GalleryServerPro.Task_Synch_Progress_Status_SynchInProgress_Msg, // 4
				Resources.GalleryServerPro.Task_Synch_Progress_SynchCancelled_Hdr, // 5
				Resources.GalleryServerPro.Task_Synch_Progress_Status_SynchInProgressException_Msg, // 6
				Resources.GalleryServerPro.Task_Synch_Progress_SynchError_Hdr, // 7
				Resources.GalleryServerPro.Task_Synch_Progress_SynchError_Msg_Prefix, // 8
				Resources.GalleryServerPro.Task_Synch_Progress_SynchComplete_Hdr, // 9
				Resources.GalleryServerPro.Task_Synch_Progress_Skipped_Objects_Msg_1_of_3, // 10
				Resources.GalleryServerPro.Task_Synch_Progress_Skipped_Objects_Msg_2_of_3, // 11
				Resources.GalleryServerPro.Task_Synch_Progress_Skipped_Objects_Msg_3_of_3, // 12
				Resources.GalleryServerPro.Task_Synch_Progress_SynchRate_Units, // 13
				Resources.GalleryServerPro.Task_Synch_Progress_ETL_Initial_Value, // 14
				Resources.GalleryServerPro.Task_Synch_Progress_ETL_1_of_5, // 15
				Resources.GalleryServerPro.Task_Synch_Progress_ETL_2_of_5, // 16
				Resources.GalleryServerPro.Task_Synch_Progress_ETL_3_of_5, // 17
				Resources.GalleryServerPro.Task_Synch_Progress_ETL_4_of_5, // 18
				Resources.GalleryServerPro.Task_Synch_Progress_ETL_5_of_5, // 19
				Resources.GalleryServerPro.Task_Synch_Progress_SynchCancelled_Bdy, // 20
				Resources.GalleryServerPro.Task_Synch_Progress_Current_File_Hdr, // 21
				Resources.GalleryServerPro.Task_Synch_Progress_SynchError_Stack_Trace_Label, // 22
				GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch, // 23
				taskSynchProgressSkippedObjectsMaxExceeded_Msg // 24
				);

			System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, typeof(GspPage), "synchScript", script, true);
		}
		#endregion

		#region Public Static Methods (WebMethods)

		[System.Web.Services.WebMethod]
		public static void Synchronize(int albumId, string synchId, bool isRecursive, bool overwriteThumb, bool overwriteOpt, bool regenerateMetadata)
		{
			// Refresh the synch status static variable. Each time we access the Instance property of the singleton, it gets its
			// properties refreshed with the latest values from the data store.
			_synchStatus = SynchronizationStatus.Instance;

			SynchronizationManager synchMgr = new SynchronizationManager();

			synchMgr.IsRecursive = isRecursive;
			synchMgr.OverwriteThumbnail = overwriteThumb;
			synchMgr.OverwriteOptimized = overwriteOpt;
			synchMgr.RegenerateMetadata = regenerateMetadata;

			try
			{
				synchMgr.Synchronize(synchId, Factory.LoadAlbumInstance(albumId, true), System.Web.HttpContext.Current.User.Identity.Name);
			}
			catch (Exception ex)
			{
				GalleryServerPro.ErrorHandler.AppErrorHandler.RecordErrorInfo(ex);
				throw;
			}
		}

		[System.Web.Services.WebMethod]
		public static SynchStatusWebEntity GetCurrentStatus()
		{
			SynchStatusWebEntity synchStatusWeb = new SynchStatusWebEntity();

			synchStatusWeb.SynchId = _synchStatus.SynchId;
			synchStatusWeb.TotalFileCount = _synchStatus.TotalFileCount;
			synchStatusWeb.CurrentFileIndex = _synchStatus.CurrentFileIndex + 1;

			if ((_synchStatus.CurrentFilePath != null) && (_synchStatus.CurrentFileName != null))
				synchStatusWeb.CurrentFile = System.IO.Path.Combine(_synchStatus.CurrentFilePath, _synchStatus.CurrentFileName);

			synchStatusWeb.Status = _synchStatus.Status.ToString();
			synchStatusWeb.StatusForUI = GetFriendlyStatusText(_synchStatus.Status);
			synchStatusWeb.PercentComplete = CalculatePercentComplete(_synchStatus);

			// Update the Skipped Files, but only when the synch is complete.
			if (_synchStatus.Status == SynchronizationState.Complete)
			{
				if (_synchStatus.SkippedMediaObjects.Count > GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch)
				{
					// We have a large number of skipped media objects. We don't want to send it all to the browsers, because it might take
					// too long or cause an error if it serializes to a string longer than int.MaxValue, so let's trim it down.
					_synchStatus.SkippedMediaObjects.RemoveRange(GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch, _synchStatus.SkippedMediaObjects.Count - GlobalConstants.MaxNumberOfSkippedObjectsToDisplayAfterSynch);
				}
				synchStatusWeb.SkippedFiles = _synchStatus.SkippedMediaObjects;
			}

			return synchStatusWeb;
		}

		[System.Web.Services.WebMethod]
		public static void TerminateTask(string taskId)
		{
			if (_synchStatus.SynchId == taskId)
			{
				_synchStatus.ShouldTerminate = true;
			}
		}

		#endregion

		#region Private Static Methods

		private static string GetFriendlyStatusText(SynchronizationState status)
		{
			switch (status)
			{
				case SynchronizationState.AnotherSynchronizationInProgress: return Resources.GalleryServerPro.Task_Synch_Progress_Status_SynchInProgressException_Hdr;
				case SynchronizationState.Complete: return status.ToString();
				case SynchronizationState.Error: return status.ToString();
				case SynchronizationState.PersistingToDataStore: return Resources.GalleryServerPro.Task_Synch_Progress_Status_PersistingToDataStore_Hdr;
				case SynchronizationState.SynchronizingFiles: return Resources.GalleryServerPro.Task_Synch_Progress_Status_SynchInProgress_Hdr;
				default: throw new System.ComponentModel.InvalidEnumArgumentException("The GetFriendlyStatusText() method in synchronize.aspx encountered a SynchronizationState enum value it was not designed for. This method must be updated.");
			}
		}

		private static int CalculatePercentComplete(ISynchronizationStatus synchStatus)
		{
			if (synchStatus.Status == SynchronizationState.SynchronizingFiles)
				return (int)(((double)synchStatus.CurrentFileIndex / (double)synchStatus.TotalFileCount) * 100);
			else
				return 100;
		}

		/// <summary>
		/// Generate a comma-delimited string containing a list of the file extensions that are in the specified filenames.
		/// If more than one filename has the same extension, the extension is listed just once in the output.
		/// </summary>
		/// <param name="filenames">A list of filenames.</param>
		/// <returns>Returns a comma-delimited string containing a list of the file extensions that are in the specified filenames.</returns>
		private static string GetFileExtensions(System.Collections.Specialized.StringCollection filenames)
		{
			System.Configuration.CommaDelimitedStringCollection fileExts = new System.Configuration.CommaDelimitedStringCollection();

			foreach (string filename in filenames)
			{
				string fileExt = System.IO.Path.GetExtension(filename);
				if (!fileExts.Contains(fileExt))
				{
					fileExts.Add(fileExt);
				}
			}

			return fileExts.ToString();
		}

		#endregion
	}

}

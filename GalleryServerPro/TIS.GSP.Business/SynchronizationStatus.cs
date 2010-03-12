using System;
using System.Collections.Generic;
using GalleryServerPro.Business.Interfaces;
using System.Globalization;
using GalleryServerPro.Business.Properties;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Provides functionality for retrieving and storing the status of a synchronization.
	/// </summary>
	/// <remarks>This class is managed as a singleton, which means only once instance exists for the current app 
	/// domain. To get this object to act as a global singleton which maintains state across all applications
	/// that access the Gallery Server data store, the <see cref="Start" /> and <see cref="Finish" /> methods store the current state to the
	/// data store. This allows other applications to be aware of in-progress synchronizations. Note that updating
	/// the properties to new values do not get persisted to the data store.</remarks>
	public class SynchronizationStatus : ISynchronizationStatus
	{
		#region Static fields

		private static ISynchronizationStatus _instance;
		private static object sharedLock = new object();

		#endregion

		#region Private fields

		private string _synchId;
		private int _totalFileCount;
		private int _currentFileIndex;
		private readonly List<KeyValuePair<string, string>> _skippedMediaObjects = new List<KeyValuePair<string, string>>();
		private bool _shouldTerminate;
		private SynchronizationState _synchState;
		private string _currentFileName;
		private string _currentFilePath;

		#endregion

		#region Constructors

		private SynchronizationStatus()
		{
		}
		
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a GUID that uniquely identifies the current synchronization. Note that setting this property
		/// does not persist it to the Synchronize table in the data store; only this instance is updated.
		/// </summary>
		/// <value>The GUID that uniquely identifies the current synchronization.</value>
		public String SynchId
		{
			get
			{
				return this._synchId.Trim();
			}
			set
			{
				this._synchId = value;
			}
		}

		/// <summary>
		/// The value that uniquely identifies the current gallery. Each web application is associated with a single gallery.
		/// </summary>
		/// <value>The value that uniquely identifies the current gallery.</value>
		public int GalleryId
		{
			get 
			{
				return GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId; ;
			}
		}

		/// <summary>
		/// Gets or sets the total number of files in the directory or directories that are being processed in the current
		/// synchronization. The number includes all files, not just ones that Gallery Server Pro recognizes as
		/// valid media objects. Note that setting this property does not persist it to the Synchronize table in the
		/// data store. Only this instance is updated.
		/// </summary>
		/// <value>
		/// The total number of files in the directory or directories that are being processed in the current
		/// synchronization.
		/// </value>
		public int TotalFileCount
		{
			get 
			{ 
				return this._totalFileCount;
			}
			set 
			{ 
				if (value < 0)
					throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resources.SynchronizationStatus_TotalFileCount_Ex_Msg, value));

				this._totalFileCount = value;
			}
		}

		/// <summary>
		/// Gets or sets the zero-based index value of the current file being processed. This is a number from 0 to <see cref="TotalFileCount"/> - 1.
		/// Note that setting this property does not persist it to the Synchronize table in the
		/// data store; only this instance is updated.
		/// </summary>
		/// <value>The zero-based index value of the current file being processed.</value>
		public int CurrentFileIndex
		{
			get 
			{ 
				return this._currentFileIndex;
			}
			set 
			{
				if ((value < 0) || ((value > 0) && (value >= this._totalFileCount)))
					throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resources.SynchronizationStatus_CurrentFileIndex_Ex_Msg, value, this._totalFileCount));

				this._currentFileIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the current file being processed.
		/// </summary>
		/// <value>The name of the current file being processed.</value>
		public string CurrentFileName
		{
			get
			{
				return this._currentFileName;
			}
			set
			{
				this._currentFileName = value;
			}
		}

		/// <summary>
		/// Gets or sets the path to the current file being processed. The path is relative to the media objects 
		/// directory. For example, if the media objects directory is C:\mypics\ and the file currently being processed is
		/// in C:\mypics\vacations\india\, this property is vacations\india\.
		/// </summary>
		/// <value>The path to the current file being processed, relative to the media objects directory (e.g. vacations\india\).</value>
		public string CurrentFilePath
		{
			get
			{
				return this._currentFilePath;
			}
			set
			{
				this._currentFilePath = value;
			}
		}

		/// <summary>
		/// Gets a list of all files that were encountered during the synchronization but were not added. The key contains
		/// the name of the file; the value contains the reason why the object was skipped. Guaranteed to not return null.
		/// </summary>
		/// <value>The list of all files that were encountered during the synchronization but were not added.</value>
		public List<KeyValuePair<string, string>> SkippedMediaObjects
		{
			get 
			{
				return this._skippedMediaObjects;
			}
		}

		/// <summary>
		/// Gets or sets the current synchronization status.
		/// </summary>
		/// <value>The status of the current synchronization.</value>
		public SynchronizationState Status
		{
			get
			{
				return this._synchState;
			}
			set
			{
				this._synchState = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current synchronization should be terminated. This is typically set
		/// by code that is observing the synchronization, such as a progress indicator. This property is periodically
		/// queried by the code running the synchronization to discover if a cancellation has been requested, and
		/// subsequently carrying out the request if it has.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the current synchronization should be terminated; otherwise, <c>false</c>.
		/// </value>
		public bool ShouldTerminate
		{
			get
			{
				return this._shouldTerminate;
			}
			set
			{
				this._shouldTerminate = value;
			}
		}

		/// <summary>
		/// Gets a reference to the <see cref="SynchronizationStatus" /> singleton for this app domain.
		/// The properties are refreshed with the latest data from the data store.
		/// </summary>
		/// <value>The <see cref="ISynchronizationStatus" /> singleton instance.</value>
		public static ISynchronizationStatus Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (sharedLock)
					{
						if (_instance == null)
						{
							ISynchronizationStatus tempStatus = new SynchronizationStatus();
							// Ensure that writes related to instantiation are flushed.
							System.Threading.Thread.MemoryBarrier();
							_instance = tempStatus;
						}
					}
				}
				// Refresh instance with data from the data store.
				Factory.GetDataProvider().Synchronize_UpdateStatusFromDataStore(_instance);

				return _instance;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Begins the process of a new synchronization by updating the status object and the Synchronize table in the
		/// data store. Throws an exception if another synchronization is already in process.
		/// </summary>
		/// <param name="synchId">A GUID string that uniquely identifies the current synchronization.</param>
		/// <param name="totalFileCount">The total number of files in the directory or directories that are being
		/// processed in the current synchronization.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException">
		/// Thrown when a synchronization with another synchId is already in progress.</exception>
		public void Start(string synchId, int totalFileCount)
		{
			if ((synchId != this._synchId) && (this._synchState != SynchronizationState.Complete))
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException();

			this._synchId = synchId;
			this._totalFileCount = totalFileCount;
			this._currentFileIndex = 0;
			this._synchState = SynchronizationState.SynchronizingFiles;
			this._skippedMediaObjects.Clear();

			// Save to data store. Even though it might have been valid to start the synchronizing above, by the time
			// we try to save to the data store, someone else may have started. So the data provider will check one more
			// time just before saving our data, throwing an exception if necessary.
			Factory.GetDataProvider().Synchronize_SaveStatus(this);
		}

		/// <summary>
		/// Completes the current synchronization by updating the status object and the Synchronize table in the
		/// data store. Calling this method is required before subsequent synchronizations can be performed.
		/// </summary>
		/// <param name="synchId">A GUID string that uniquely identifies the current synchronization.</param>
		public void Finish(string synchId)
		{
			// Updates database to show synchronization is no longer occuring.
			// Should be called when synchronization is finished.
			this._synchState = SynchronizationState.Complete;

			// Don't reset the file counts in case the UI wants to know how many files were processed.
			//this._currentFileIndex = 0;
			//this._totalFileCount = 0;

			Factory.GetDataProvider().Synchronize_SaveStatus(this);
		}

		#endregion
	}
}

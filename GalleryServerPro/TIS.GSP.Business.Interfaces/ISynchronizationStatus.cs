namespace GalleryServerPro.Business.Interfaces
{
	/// <summary>
	/// Indicates the current state of a synchronization process.
	/// </summary>
	public enum SynchronizationState
	{
		/// <summary>
		/// The synchronization is complete and there is no current activity.
		/// </summary>
		Complete = 0,
		/// <summary>
		/// Indicates the current user is performing a synchronization. During this state no changes will be
		/// persisted to the data store. The changes will be saved to the data store in the next state
		/// PersistingToDataStore.
		/// </summary>
		SynchronizingFiles = 1,
		/// <summary>
		/// Indicates the files have been synchronized and now the changes are being persisted to the data store.
		/// </summary>
		PersistingToDataStore = 2,
		/// <summary>
		/// An error occured during the most recent synchronization.
		/// </summary>
		Error = 3,
		/// <summary>
		/// Indicates another synchronization is already in progress.
		/// </summary>
		AnotherSynchronizationInProgress = 4
	}

	/// <summary>
	/// Provides functionality for retrieving and storing the status of a synchronization.
	/// </summary>
	/// <remarks>This class is managed as a singleton, which means only once instance exists for the current app 
	/// domain. To get this object to act as a global singleton which maintains state across all applications
	/// that access the Gallery Server data store, the <see cref="Start" /> and <see cref="Finish" /> methods store the current state to the
	/// data store. This allows other applications to be aware of in-progress synchronizations. Note that updating
	/// the properties to new values do not get persisted to the data store.</remarks>
	public interface ISynchronizationStatus
	{
		/// <summary>
		/// Gets or sets the zero-based index value of the current file being processed. This is a number from 0 to <see cref="TotalFileCount"/> - 1.
		/// Note that setting this property does not persist it to the Synchronize table in the
		/// data store; only this instance is updated.
		/// </summary>
		/// <value>The zero-based index value of the current file being processed.</value>
		int CurrentFileIndex { get; set;}
		
		/// <summary>
		/// Gets or sets the name of the current file being processed (e.g. DesertSun.jpg).
		/// </summary>
		/// <value>The name of the current file being processed (e.g. DesertSun.jpg).</value>
		string CurrentFileName { get; set;}
		
		/// <summary>
		/// Gets or sets the path to the current file being processed. The path is relative to the media objects 
		/// directory. For example, if the media objects directory is C:\mypics\ and the file currently being processed is
		/// in C:\mypics\vacations\india\, this property is vacations\india\.
		/// </summary>
		/// <value>The path to the current file being processed, relative to the media objects directory (e.g. vacations\india\).</value>
		string CurrentFilePath { get; set;}

		/// <summary>
		/// Gets or sets the status of the current synchronization. This property will never return
		/// <see cref="GalleryServerPro.Business.Interfaces.SynchronizationState.AnotherSynchronizationInProgress"/>. To find out if another
		/// synchronization is in progress, call the <see cref="Start"/> method and catch
		/// GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException. Note that setting this property does not
		/// persist it to the Synchronize table in the data store; only this instance is updated.
		/// </summary>
		/// <value>The status of the current synchronization.</value>
		SynchronizationState Status { get; set;}

		/// <summary>
		/// Gets or sets a GUID that uniquely identifies the current synchronization. Note that setting this property
		/// does not persist it to the Synchronize table in the data store; only this instance is updated.
		/// </summary>
		/// <value>The GUID that uniquely identifies the current synchronization.</value>
		string SynchId { get; set; }

		/// <summary>
		/// The value that uniquely identifies the current gallery. Each web application is associated with a single gallery.
		/// </summary>
		/// <value>The value that uniquely identifies the current gallery.</value>
		int GalleryId { get; }

		/// <summary>
		/// Gets or sets the total number of files in the directory or directories that are being processed in the current
		/// synchronization. The number includes all files, not just ones that Gallery Server Pro recognizes as
		/// valid media objects. Note that setting this property does not persist it to the Synchronize table in the
		/// data store. Only this instance is updated.
		/// </summary>
		/// <value>The total number of files in the directory or directories that are being processed in the current
		/// synchronization.</value>
		int TotalFileCount { get; set;}

		/// <summary>
		/// Gets a list of all files that were encountered during the synchronization but were not added. The key contains
		/// the name of the file; the value contains the reason why the object was skipped. Guaranteed to not return null.
		/// </summary>
		/// <value>The list of all files that were encountered during the synchronization but were not added.</value>
		System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> SkippedMediaObjects { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the current synchronization should be terminated. This is typically set
		/// by code that is observing the synchronization, such as a progress indicator. This property is periodically
		/// queried by the code running the synchronization to discover if a cancellation has been requested, and
		/// subsequently carrying out the request if it has.
		/// </summary>
		/// <value><c>true</c> if the current synchronization should be terminated; otherwise, <c>false</c>.</value>
		bool ShouldTerminate { get; set;}

		/// <summary>
		/// Completes the current synchronization by updating the status object and the Synchronize table in the
		/// data store. Calling this method is required before subsequent synchronizations can be performed.
		/// </summary>
		/// <param name="synchId">A GUID string that uniquely identifies the current synchronization.</param>
		void Finish(string synchId);

		/// <summary>
		/// Begins the process of a new synchronization by updating the status object and the Synchronize table in the 
		/// data store. Throws an exception if another synchronization is already in process.
		/// </summary>
		/// <param name="synchId">A GUID string that uniquely identifies the current synchronization.</param>
		/// <param name="totalFileCount">The total number of files in the directory or directories that are being 
		/// processed in the current synchronization.</param>
		/// <exception>Throws a GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException when 
		/// a synchronization with another <paramref name="synchId"/> is already in progress.</exception>
		void Start(string synchId, int totalFileCount);
	}
}

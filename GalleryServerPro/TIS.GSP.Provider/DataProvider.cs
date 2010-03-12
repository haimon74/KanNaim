using System;
using System.Data;
using System.Collections.Generic;

using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Provider
{
	/// <summary>
	/// Provides the abstract class implementation for retrieving and persisting information to the data store.
	/// </summary>
	public abstract class DataProvider : System.Configuration.Provider.ProviderBase, GalleryServerPro.Provider.Interfaces.IDataProvider
	{
		#region Public Properties

		/// <summary>
		/// Gets the friendly name used to refer to the provider during configuration.
		/// </summary>
		/// <value></value>
		/// <returns>The friendly name used to refer to the provider during configuration.</returns>
		public new abstract string Name { get; }

		/// <summary>
		/// Gets a brief, friendly description suitable for display in administrative tools or other user interfaces (UIs).
		/// </summary>
		/// <value></value>
		/// <returns>A brief, friendly description suitable for display in administrative tools or other UIs.</returns>
		public new abstract string Description { get; }

		/// <summary>
		/// Gets the name of the connection string.
		/// </summary>
		/// <value>The name of the connection string.</value>
		public abstract string ConnectionStringName { get; }

		/// <summary>
		/// Gets or sets the name of the application to store and retrieve Gallery Server data for.
		/// </summary>
		/// <value>
		/// The name of the application to store and retrieve Gallery Server data for.
		/// </value>
		public abstract string ApplicationName { get; set; }

		#endregion

		#region Gallery Methods

		/// <summary>
		/// Gets an <see cref="IGallery" /> representing the current gallery. The return value is the same reference 
		/// as the parameter.
		/// </summary>
		/// <param name="gallery">An <see cref="IGallery"/> object to populate with the information about the
		/// current gallery. This parameter is required because the library that implements this interface does not have
		/// the ability to instantiate any object that implements <see cref="IGallery"/>.
		/// information from the data store.</param>
		/// <returns>Returns an <see cref="IGallery" /> representing the current gallery. The returned object is the
		/// same object in memory as the <paramref name="gallery"/> parameter.</returns>
		public abstract IGallery Gallery_GetCurrentGallery(IGallery gallery);

		#endregion

		#region Album Methods

		/// <summary>
		/// Persist the specified album to the data store. Return the ID of the album.
		/// </summary>
		/// <param name="album">An instance of <see cref="IAlbum"/> to persist to the data store.</param>
		/// <returns>
		/// Return the ID of the album. If this is a new album and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.
		/// </returns>
		public abstract int Album_Save(IAlbum album);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the child albums contained within the album specified by
		/// albumId parameter. If no matching objects are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the album for which to return the child albums
		/// contained within.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object containing a field named ID that contains the IDs of all albums
		/// directly within the album represented by albumId.
		/// </returns>
		public abstract IDataReader Album_GetDataReaderChildAlbumsById(int albumId);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the child gallery objects contained within the album specfified by
		/// albumId parameter. If no matching objects are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the desired album.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object containing all relevant fields for the gallery objects.
		/// </returns>
		public abstract IDataReader Album_GetDataReaderChildMediaObjectsById(int albumId);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the album for the specified albumId. If no matching object
		/// is found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the desired album.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all album fields.
		/// </returns>
		public abstract IDataReader Album_GetDataReaderAlbumById(int albumId);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the top-level album in the gallery. This method is guaranteed
		/// to return an instance with one record representing the top-level album, since a default root
		/// album is created if one does not exist.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all album fields.
		/// </returns>
		public abstract IDataReader Album_GetDataReaderRootAlbum(int galleryId);

		/// <summary>
		/// Permanently delete the specified album from the data store, including any
		/// child albums and media objects (cascading delete). This action cannot be undone.
		/// </summary>
		/// <param name="album">The <see cref="IAlbum"/> to delete from the data store.</param>
		public abstract void Album_Delete(IAlbum album);

		#endregion

		#region Media Object Methods

		/// <summary>
		/// Persist the specified media object to the data store. Return the ID of the media object.
		/// </summary>
		/// <param name="mediaObject">An instance of <see cref="IGalleryObject"/> to persist to the data store.</param>
		/// <returns>
		/// Return the ID of the media object. If this is a new media object and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.
		/// </returns>
		public abstract int MediaObject_Save(IGalleryObject mediaObject);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the media object for the specified mediaObjectId. If no matching object
		/// is found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the desired media object.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all media object fields.
		/// </returns>
		public abstract IDataReader MediaObject_GetDataReaderMediaObjectById(int mediaObjectId);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the metadata items for the specified mediaObjectId. If no matching object
		/// is found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the desired media object.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all metadata items.
		/// </returns>
		public abstract IDataReader MediaObject_GetDataReaderMetadataItemsByMediaObjectId(int mediaObjectId);

		/// <summary>
		/// Permanently delete the specified media object from the data store. This action cannot
		/// be undone.
		/// </summary>
		/// <param name="mediaObject">The <see cref="IGalleryObject"/> to delete from the data store.</param>
		public abstract void MediaObject_Delete(IGalleryObject mediaObject);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the hash keys for all media objects in the specified gallery.
		/// </summary>
		/// <param name="galleryId">The value that uniquely identifies the current gallery.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with one field named "HashKey" containing the hash keys
		/// for all media objects in the current gallery.
		/// </returns>
		public abstract IDataReader MediaObject_GetAllHashKeys(int galleryId);

		#endregion

		#region Synchronize Methods

		/// <summary>
		/// Persist the synchronization information to the data store.
		/// </summary>
		/// <param name="synchStatus">An <see cref="ISynchronizationStatus" /> object containing the synchronization information
		/// to persist to the data store.</param>
		/// <exception>Throws a GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException if the data 
		/// store indicates another synchronization is already in progress for this gallery.</exception>
		public abstract void Synchronize_SaveStatus(ISynchronizationStatus synchStatus);

		/// <summary>
		/// Retrieve the most recent synchronization information from the data store and set the relevant properties
		/// on the <paramref name="synchStatus"/> parameter. The return value is the same reference as the parameter.
		/// </summary>
		/// <param name="synchStatus">An <see cref="ISynchronizationStatus"/> object to populate with the most recent synchronization
		/// information from the data store.</param>
		/// <returns>
		/// Returns an <see cref="ISynchronizationStatus"/> object with updated properties based on what was retrieved
		/// from the data store.
		/// </returns>
		public abstract ISynchronizationStatus Synchronize_UpdateStatusFromDataStore(ISynchronizationStatus synchStatus);

		#endregion

		#region General methods

		/// <summary>
		/// Perform any needed data store operations to get Gallery Server ready to go. This includes verifying the
		/// database has the minimum default records in certain tables and/or upgrading the database to the version 
		/// required by this provider. If no records are configured for the current gallery ID, they are created as 
		/// necessary. Thus, this method can be used to create a new gallery. The gallery ID that is used is the one 
		/// specified in the config file (galleryserverpro.config).
		/// </summary>
		public abstract void InitializeDataStore();

		/// <summary>
		/// Return gallery objects that match the specified search string. A gallery object is considered a match when
		/// all search terms are found in the relevant fields.
		/// For albums, the title and summary fields are searched. For media objects, the title, original filename,
		/// and metadata are searched. The contents of documents are not searched (e.g. the text of a Word or PDF file).
		/// If no matches are found, <paramref name="matchingAlbumIds"/> and <paramref name="matchingMediaObjectIds"/>
		/// will be empty, not null collections.
		/// </summary>
		/// <param name="searchTerms">A string array of search terms. Specify a single word for each item of the array, or
		/// combine words in an element to force a phase match. Items with more than one word indicate an exact
		/// phrase match is required. Example: There are three items where item 1="cat", item 2="0 step", and item 3="Mom".
		/// This method will match all gallery objects that contain the strings "cat", "0 step", and "Mom". It will also
		/// match partial words, such as Mom on steps at cathedral</param>
		/// <param name="matchingAlbumIds">The album IDs for all albums that match the search terms.</param>
		/// <param name="matchingMediaObjectIds">The media object IDs for all media objects that match the search terms.</param>
		/// <example>
		/// 	<para>Example 1</para>
		/// 	<para>The search terms are three elements: "cat", "step", and "Mom". All gallery objects that contain all
		/// three strings will be returned, such as an image with the caption "Mom and cat sitting on steps" (Notice the
		/// successful partial match between step and steps. However, the inverse is not true - searching for "steps"
		/// will not match "step".) Also matched would be an image with a caption "Mom at cathedral" and the exposure
		/// compensation metadata is "0 step".</para>
		/// 	<para>Example 2</para>
		/// 	<para>The search terms are two elements: "at the beach" and "Joey". All gallery objects that contain the
		/// phrase "at the beach" and "Joey" will be returned, such as a video with the caption "Joey at the beach with Mary".
		/// An image with the caption "Joey on the beach at Mary's house" will not match because the phrase "at the beach"
		/// is not present.
		/// </para>
		/// </example>
		public abstract void SearchGallery(string[] searchTerms, out List<int> matchingAlbumIds, out List<int> matchingMediaObjectIds);

		/// <summary>
		/// Begins a new database transaction. All subsequent database actions occur within the context of this transaction.
		/// Use <see cref="CommitTransaction"/> to commit this transaction or RollbackTransaction to abort it. If a transaction
		/// is already in progress, then this method returns without any action, which preserves the original transaction.
		/// </summary>
		public abstract void BeginTransaction();

		/// <summary>
		/// Commits the current transaction, if one exists. A transaction is created with the <see cref="BeginTransaction"/> method.
		/// If there is not an existing transaction, no action is taken. If this method is called when a datareader is open, the
		/// actual commit is delayed until all datareaders are disposed.
		/// </summary>
		public abstract void CommitTransaction();

		/// <summary>
		/// Aborts the current transaction, if one exists. A transaction is created with the <see cref="BeginTransaction"/> method.
		/// If there is not an existing transaction, no action is taken.
		/// </summary>
		public abstract void RollbackTransaction();

		/// <summary>
		/// Imports the Gallery Server Pro data into the current database, overwriting any existing data. Does not import the actual media
		/// files; they must be imported manually with a utility such as Windows Explorer. This method makes changes only to the database tables;
		/// no files in the media objects directory are affected. If both the <paramref name="importMembershipData"/> and <paramref name="importGalleryData"/>
		/// parameters are false, then no action is taken.
		/// </summary>
		/// <param name="filePath">The full file path to the XML file containing the data (e.g. "D:\mybackups\GalleryServerBackup_2008-06-22_141336.xml").</param>
		/// <param name="importMembershipData">If set to <c>true</c>, user accounts and other membership data will be imported.
		/// Current membership data will first be deleted.</param>
		/// <param name="importGalleryData">If set to <c>true</c>, albums, media objects, and other gallery data will be imported.
		/// Current gallery data will first be deleted.</param>
		public abstract void ImportGalleryData(string filePath, bool importMembershipData, bool importGalleryData);

		/// <summary>
		/// Exports the Gallery Server Pro data in the current database to an XML file at the location specified by the <paramref name="filePath"/>
		/// parameter. Does not export the actual media files; they must be copied manually with a utility such as Windows Explorer. This method
		/// does not make any changes to the database tables or the files in the media objects directory.
		/// </summary>
		/// <param name="filePath">The full file path specifying where the XML file will be written to. If a file with the same name already
		/// exists, it will first be deleted. Example: "D:\mybackups\GalleryServerBackup_2008-06-22_141336.xml".</param>
		/// <param name="exportMembershipData">If set to <c>true</c>, user accounts and other membership data will be exported.</param>
		/// <param name="exportGalleryData">If set to <c>true</c>, albums, media objects, and other gallery data will be exported.</param>
		public abstract void ExportGalleryData(string filePath, bool exportMembershipData, bool exportGalleryData);

		/// <summary>
		/// Validates that the backup file specified in the <see cref="IBackupFile.FilePath"/> property of the <paramref name="backupFile"/>
		/// parameter is valid and populates the remaining properties with information about the file.
		/// </summary>
		/// <param name="backupFile">An instance of <see cref="IBackupFile"/> that with only the <see cref="IBackupFile.FilePath"/>
		/// property assigned. The remaining properties should be uninitialized since they will be assigned in this method.</param>
		public abstract void ValidateBackupFile(ref IBackupFile backupFile);

		#endregion

		#region Security Methods

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the roles in the current gallery. If no matching objects
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing the roles in the current gallery.
		/// </returns>
		public abstract IDataReader Roles_GetDataReaderRoles();

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the root album IDs associated with the specified role name. If no matching data
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="roleName">The role name for which root album IDs should be returned.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing the root album IDs associated with the specified role name.
		/// </returns>
		public abstract IDataReader Roles_GetDataReaderRoleRootAlbums(string roleName);

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing all album IDs associated with the specified role name. If no matching data
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="roleName">The role name for which all album IDs should be returned.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing all album IDs associated with the specified role name.
		/// </returns>
		public abstract IDataReader Roles_GetDataReaderRoleAllAlbums(string roleName);

		/// <summary>
		/// Persist this gallery server role to the data store. The list of top-level albums this role applies to, which is stored
		/// in the <see cref="IGalleryServerRole.RootAlbumIds"/> property, must also be saved. The data provider automatically
		/// repopulates the <see cref="IGalleryServerRole.AllAlbumIds"/> property.
		/// </summary>
		/// <param name="role">An instance of IGalleryServerRole to persist to the data store.</param>
		public abstract void Role_Save(IGalleryServerRole role);

		/// <summary>
		/// Permanently delete this gallery server role from the data store, including the list of role/album relationships
		/// associated with this role. This action cannot be undone.
		/// </summary>
		/// <param name="role">An instance of <see cref="IGalleryServerRole"/> to delete from the data store.</param>
		public abstract void Role_Delete(IGalleryServerRole role);

		#endregion

		#region App Error methods

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the application errors. If no matching objects are found
		/// in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object with all application error fields.
		/// </returns>
		public abstract IDataReader AppError_GetAppErrors();

		/// <summary>
		/// Persist the specified application error to the data store. Return the ID of the error.
		/// </summary>
		/// <param name="appError">The application error to persist to the data store.</param>
		/// <returns>
		/// Return the ID of the error. If this is a new error object and a new ID has been
		/// assigned, then this value has also been assigned to the ID property of the object.
		/// </returns>
		public abstract int AppError_Save(IAppError appError);

		/// <summary>
		/// Delete the application error from the data store.
		/// </summary>
		/// <param name="appErrorId">The value that uniquely identifies this application error (<see cref="IAppError.AppErrorId"/>).</param>
		public abstract void AppError_Delete(int appErrorId);

		/// <summary>
		/// Permanently delete all errors from the data store.
		/// </summary>
		public abstract void AppError_ClearLog();

		#endregion
	}
}

using System;
using System.Globalization;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.NullObjects;
using GalleryServerPro.Business.Properties;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Represents a gallery object, which is an item that is managed by Gallery Server Pro. Examples include
	/// albums, images, videos, audio files, and documents.
	/// </summary>
	public abstract class GalleryObject : IGalleryObject, IComparable
	{
		#region Private Fields

		private bool _isNew;
		private bool _isInflated;
		private int _id;
		private string _title;
		private int _sequence;
		private DateTime _dateAdded;
		private string _hashkey;
		private bool _hashKeyHasChanged;
		private bool _hasChanges;
		private bool _regenerateThumbnailOnSave;
		private bool _regenerateOptimizedOnSave;
		private IDisplayObject _thumbnail;
		private IDisplayObject _optimized;
		private IDisplayObject _original;
		private IGalleryObject _parent;
		private ISaveBehavior _saveBehavior;
		private IDeleteBehavior _deleteBehavior;
		private IGalleryObjectMetadataItemCollection _metadataItems;
		private System.Drawing.RotateFlipType _rotation = System.Drawing.RotateFlipType.RotateNoneFlipNone;
		private string _createdByUsername;
		private string _lastModifiedByUsername;
		private DateTime _dateLastModified;
		private bool _isPrivate;
		private bool _isSynchronized;
		private bool _hasBeenDisposed; // Used by Dispose() methods

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="GalleryObject"/> class.
		/// </summary>
		protected GalleryObject()
		{
			this._parent = new NullObjects.NullGalleryObject();
			this._thumbnail = new NullObjects.NullDisplayObject();
			this._optimized = new NullObjects.NullDisplayObject();
			this._original = new NullObjects.NullDisplayObject();

			// Default IsSynchronized to true. It is set to false during a synchronization.
			this.IsSynchronized = true;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the unique identifier for this gallery object.
		/// </summary>
		/// <value>The unique identifier for this gallery object.</value>
		public int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._isNew = (value == int.MinValue ? true : false);
				this._hasChanges = (this._id == value ? this._hasChanges : true);
				this._id = value;
			}
		}

		/// <summary>
		/// Gets or sets the object that contains this gallery object.
		/// </summary>
		/// <value>The object that contains this gallery object.</value>
		public IGalleryObject Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value", Resources.GalleryObject_Parent_Ex_Msg);

				this._hasChanges = (this._parent == value ? this._hasChanges : true);
				this._parent.Remove(this);
				value.DoAdd(this);
				this._parent = value;

				RecalculateFilePaths();
			}
		}

		/// <summary>
		/// Gets or sets the title for this gallery object.
		/// </summary>
		/// <value>The title for this gallery object.</value>
		public string Title
		{
			get
			{
				VerifyObjectIsInflated(this._title);
				return this._title;
			}
			set
			{
				value = ValidateTitle(value);

				this._hasChanges = (this._title == value ? _hasChanges : true);
				this._title = value;
			}
		}

		/// <summary>
		/// The value that uniquely identifies the current gallery. Each web application is associated with a single gallery.
		/// </summary>
		/// <value>The value that uniquely identifies the current gallery.</value>
		public int GalleryId
		{
			get { return GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this object has changes that have not been persisted to the database.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has changes; otherwise, <c>false</c>.
		/// </value>
		public bool HasChanges
		{
			get
			{
				return this._hasChanges;
			}
			set
			{
				this._hasChanges = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this object is new and has not yet been persisted to the data store.
		/// </summary>
		/// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			protected set
			{
				this._isNew = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this object has been fully populated with data from the data store.
		/// Once assigned a true value, it remains true for the lifetime of the object.
		/// Returns false for newly created objects that have not been saved to the data store. Set to true after an object
		/// is saved.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is inflated; otherwise, <c>false</c>.
		/// </value>
		public bool IsInflated
		{
			get { return this._isInflated; }
			set
			{
				if (this._isInflated)
				{
					throw new System.InvalidOperationException(Resources.GalleryObject_IsInflated_Ex_Msg);
				}
				_isInflated = value;
			}
		}

		/// <summary>
		/// Gets or sets the thumbnail information for this gallery object.
		/// </summary>
		/// <value>The thumbnail information for this gallery object.</value>
		public IDisplayObject Thumbnail
		{
			get
			{
				VerifyThumbnailIsInflated(this._thumbnail);

				return this._thumbnail;
			}
			set
			{
				if (value == null)
					throw new BusinessException("Attempted to set GalleryObject.Thumbnail to null for MOID " + this.Id);

				this._hasChanges = (this._thumbnail == value ? this._hasChanges : true);
				this._thumbnail = value;
			}
		}

		/// <summary>
		/// Gets or sets the optimized information for this gallery object.
		/// </summary>
		/// <value>The optimized information for this gallery object.</value>
		public IDisplayObject Optimized
		{
			get
			{
				return this._optimized;
			}
			set
			{
				if (value == null)
					throw new BusinessException("Attempted to set GalleryObject.Optimized to null for MOID " + this.Id);

				this._hasChanges = (this._optimized == value ? this._hasChanges : true);
				this._optimized = value;
			}
		}

		/// <summary>
		/// Gets or sets the information representing the original media object. (For example, the uncompressed photo, or the video / audio file.)
		/// </summary>
		/// <value>The information representing the original media object.</value>
		public IDisplayObject Original
		{
			get
			{
				return this._original;
			}
			set
			{
				if (value == null)
					throw new BusinessException("Attempted to set GalleryObject.Original to null for MOID " + this.Id);

				this._hasChanges = (this._original == value ? this._hasChanges : true);
				this._original = value;
			}
		}

		/// <summary>
		/// Gets the physical path to this object. Does not include the trailing slash.
		/// Example: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets
		/// </summary>
		/// <value>The full physical path to this object.</value>
		public virtual string FullPhysicalPath
		{
			get
			{
				return this._parent.FullPhysicalPath;
			}
		}

		/// <summary>
		/// Gets or sets the full physical path for this object as it currently exists on the hard drive. This property
		/// is updated when the object is loaded from the hard drive and when it is saved to the hard drive.
		/// <note type="caution"> Do not set this property from any class other than one that implements <see cref="IGalleryObject"/>!
		/// Does not include the trailing slash.
		/// Example: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets</note>
		/// </summary>
		/// <value>The full physical path on disk.</value>
		public virtual string FullPhysicalPathOnDisk
		{
			get
			{
				return this._parent.FullPhysicalPathOnDisk;
			}
			set
			{
				throw new System.NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the MIME type for this media object. The MIME type is determined from the extension of the Filename on the <see cref="Original" /> property.
		/// </summary>
		/// <value>The MIME type for this media object.</value>
		public IMimeType MimeType
		{
			get
			{
				return this._original.MimeType;
			}
		}

		/// <summary>
		/// Gets or sets the sequence of this gallery object within the containing album.
		/// </summary>
		/// <value>The sequence of this gallery object within the containing album.</value>
		public int Sequence
		{
			get
			{
				VerifyObjectIsInflated(this._sequence);
				return this._sequence;
			}
			set
			{
				this._hasChanges = (this._sequence == value ? this._hasChanges : true);
				this._sequence = value;
			}
		}

		/// <summary>
		/// Gets or sets the date this gallery object was created.
		/// </summary>
		/// <value>The date this gallery object was created.</value>
		public DateTime DateAdded
		{
			get
			{
				VerifyObjectIsInflated(this._dateAdded);
				return this._dateAdded;
			}
			set
			{
				this._hasChanges = (this._dateAdded == value ? this._hasChanges : true);
				this._dateAdded = value;
			}
		}

		/// <summary>
		/// Gets or sets the hash key for the file associated with this galley object. Not applicable for <see cref="Album" /> objects.
		/// </summary>
		/// <value>The hash key for the file associated with this object.</value>
		public string Hashkey
		{
			get
			{
				return this._hashkey;
			}
			set
			{
				if (this._hashkey != value)
				{
					this._hashKeyHasChanged = true;
					this._hasChanges = true;
					this._hashkey = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the thumbnail file is regenerated and overwritten on the file system. This value does not affect whether or how the data store is updated during a Save operation. This property is ignored for instances of the <see cref="Album" /> class.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the thumbnail file is regenerated and overwritten on the file system when this object is saved; otherwise, <c>false</c>.
		/// </value>
		public bool RegenerateThumbnailOnSave
		{
			get
			{
				return this._regenerateThumbnailOnSave;
			}
			set
			{
				this._hasChanges = (this._regenerateThumbnailOnSave == value ? this._hasChanges : true);
				this._regenerateThumbnailOnSave = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the optimized file is regenerated and overwritten on the file system during a Save operation. This value does not affect whether or how the data store is updated. This property is ignored for instances of the <see cref="Album" /> class.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the optimized file is regenerated and overwritten on the file system when this object is saved; otherwise, <c>false</c>.
		/// </value>
		public bool RegenerateOptimizedOnSave
		{
			get
			{
				return this._regenerateOptimizedOnSave;
			}
			set
			{
				this._hasChanges = (this._regenerateOptimizedOnSave == value ? this._hasChanges : true);
				this._regenerateOptimizedOnSave = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether, during a <see cref="Save" /> operation, metadata embedded in the original media object file is
		/// extracted and persisted to the data store, overwriting any previous extracted metadata. This property is a pass-through
		/// to the <see cref="IGalleryObjectMetadataItemCollection.RegenerateAllOnSave" /> property of the <see cref="MetadataItems" /> 
		/// property of this object, which in turn is calculated based on the <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave" />
		/// property on each metadata item in the collection. Specifically, this property returns true if <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave" /> =
		/// true for *every* metadata item in the collection; otherwise it returns false. Setting this property causes the
		/// <see cref="IGalleryObjectMetadataItem.ExtractFromFileOnSave" /> property to be set to the specified value for *every* metadata item in the collection.
		/// This property is ignored for Albums.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if metadata embedded in the original media object file is
		/// extracted and persisted to the data store when this object is saved; otherwise, <c>false</c>.
		/// </value>
		public bool RegenerateMetadataOnSave
		{
			get
			{
				return this.MetadataItems.RegenerateAllOnSave;
			}
			set
			{
				this._hasChanges = (this.MetadataItems.RegenerateAllOnSave == value ? this._hasChanges : true);
				this.MetadataItems.RegenerateAllOnSave = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current object is synchronized with the data store.
		/// This value is set to false at the beginning of a synchronization and set to true when it is
		/// synchronized with its corresponding file(s) on disk. At the conclusion of the synchronization,
		/// all objects where IsSynchronized = false are deleted. This property defaults to true for new instances.
		/// This property is not persisted in the data store, as it is only relevant during a synchronization.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is synchronized; otherwise, <c>false</c>.
		/// </value>
		public bool IsSynchronized
		{
			get { return this._isSynchronized; }
			set { this._isSynchronized = value; }
		}

		/// <summary>
		/// Gets the metadata items associated with this gallery object.
		/// </summary>
		/// <value>The metadata items.</value>
		public IGalleryObjectMetadataItemCollection MetadataItems
		{
			get
			{
				if (this._metadataItems == null)
				{
					this._metadataItems = new Metadata.GalleryObjectMetadataItemCollection();
				}

				return this._metadataItems;
			}
		}

		/// <summary>
		/// Gets or sets the amount of rotation to be applied to this gallery object when it is saved. Applies only to <see cref="Image" /> objects;
		/// all others throw a <see cref="NotSupportedException" />.
		/// </summary>
		/// <value>
		/// The amount of rotation to be applied to this gallery object when it is saved.
		/// </value>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type does not allow rotation.</exception>
		public System.Drawing.RotateFlipType Rotation
		{
			get
			{
				return this._rotation;
			}
			set
			{
				this._hasChanges = (this._rotation == value ? this._hasChanges : true);
				this._rotation = value;
			}
		}

		/// <summary>
		/// Gets or sets the user name of the user who created this gallery object.
		/// </summary>
		/// <value>The name of the created by user.</value>
		public string CreatedByUserName
		{
			get
			{
				VerifyObjectIsInflated(this._createdByUsername);
				return this._createdByUsername;
			}
			set
			{
				this._hasChanges = (this._createdByUsername == value ? this._hasChanges : true);
				this._createdByUsername = value;
			}
		}

		/// <summary>
		/// Gets or sets the user name of the user who last modified this gallery object.
		/// </summary>
		/// <value>The user name of the user who last modified this object.</value>
		public string LastModifiedByUserName
		{
			get
			{
				VerifyObjectIsInflated(this._lastModifiedByUsername);
				return this._lastModifiedByUsername;
			}
			set
			{
				this._hasChanges = (this._lastModifiedByUsername == value ? this._hasChanges : true);
				this._lastModifiedByUsername = value;
			}
		}

		/// <summary>
		/// Gets or sets the date and time this gallery object was last modified.
		/// </summary>
		/// <value>The date and time this gallery object was last modified.</value>
		public DateTime DateLastModified
		{
			get
			{
				VerifyObjectIsInflated(this._dateLastModified);
				return this._dateLastModified;
			}
			set
			{
				this._hasChanges = (this._dateLastModified == value ? this._hasChanges : true);
				this._dateLastModified = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this gallery object is hidden from anonymous users.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is private; otherwise, <c>false</c>.
		/// </value>
		public bool IsPrivate
		{
			get
			{
				VerifyObjectIsInflated();
				return this._isPrivate;
			}
			set
			{
				this._hasChanges = (this._isPrivate == value ? this._hasChanges : true);
				this._isPrivate = value;
			}
		}

		#endregion

		#region Protected Properties

		/// <summary>
		/// Gets or sets the save behavior.
		/// </summary>
		/// <value>The save behavior.</value>
		protected ISaveBehavior SaveBehavior
		{
			get
			{
				return this._saveBehavior;
			}
			set
			{
				this._saveBehavior = value;
			}
		}

		/// <summary>
		/// Gets or sets the delete behavior.
		/// </summary>
		/// <value>The delete behavior.</value>
		protected IDeleteBehavior DeleteBehavior
		{
			get
			{
				return this._deleteBehavior;
			}
			set
			{
				this._deleteBehavior = value;
			}
		}

		#endregion

		#region Public Events

		/// <summary>
		/// Occurs when the <see cref="Save"/> method has been invoked, but before the object has been saved. Validation within
		/// the GalleryObject class has occured prior to this event.
		/// </summary>
		public event EventHandler Saving;

		/// <summary>
		/// Occurs when the <see cref="Save"/> method has been invoked and after the object has been saved.
		/// </summary>
		public event EventHandler Saved;

		#endregion

		#region Public Virtual Methods (throw exception)

		/// <summary>
		/// Adds the specified gallery object as a child of this gallery object.
		/// </summary>
		/// <param name="galleryObject">The IGalleryObject to add as a child of this
		/// gallery object.</param>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual void Add(IGalleryObject galleryObject)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Adds the specified gallery object as a child of this gallery object. This method is called by the <see cref="Add"/> method and should not be called directly.
		/// </summary>
		/// <param name="galleryObject">The gallery object to add as a child of this gallery object.</param>
		public virtual void DoAdd(IGalleryObject galleryObject)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Removes the specified gallery object from the collection of child objects
		/// of this gallery object.
		/// </summary>
		/// <param name="galleryObject">The IGalleryObject to remove as a child of this
		/// gallery object.</param>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		/// <exception cref="System.ArgumentException">Thrown when the specified
		/// gallery object is not child of this gallery object.</exception>
		public virtual void Remove(IGalleryObject galleryObject)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a collection of gallery objects that are direct children of the current gallery object. Returns
		/// an empty list (Count = 0) if there are no child objects. Use the galleryObjectType
		/// parameter to return objects of the desired type. Use the sortBySequence parameter
		/// to sort the collection by sequence number. If the sortBySequence is not specified, the collection is
		/// not sorted in any particular order. Use the excludePrivateObjects parameter to optionally filter out private
		/// objects (if not specified, private objects are returned).
		/// </summary>
		/// <returns>
		/// Returns a collection of objects of type IGalleryObject whose
		/// parent is the current gallery object.
		/// </returns>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual IGalleryObjectCollection GetChildGalleryObjects()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a collection of gallery objects that are direct children of the current gallery object. Returns
		/// an empty list (Count = 0) if there are no child objects. Use the overload with the galleryObjectType
		/// parameter to return objects of the desired type. Use the overload with the sortBySequence parameter
		/// to sort the collection by sequence number. If the sortBySequence is not specified, the collection is
		/// not sorted in any particular order. Use the excludePrivateObjects parameter to optionally filter out private
		/// objects (if not specified, private objects are returned).
		/// </summary>
		/// <param name="sortBySequence">Indicates whether to sort the child gallery objects by the Sequence property.</param>
		/// <returns>
		/// Returns a collection of objects of type IGalleryObject whose
		/// parent is the current gallery object and are of the specified type.
		/// </returns>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual IGalleryObjectCollection GetChildGalleryObjects(bool sortBySequence)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a collection of gallery objects that are direct children of the current gallery object. Returns
		/// an empty list (Count = 0) if there are no child objects. Use the overload with the galleryObjectType
		/// parameter to return objects of the desired type. Use the overload with the sortBySequence parameter
		/// to sort the collection by sequence number. If the sortBySequence is not specified, the collection is
		/// not sorted in any particular order. Use the excludePrivateObjects parameter to optionally filter out private
		/// objects (if not specified, private objects are returned).
		/// </summary>
		/// <param name="sortBySequence">Indicates whether to sort the child gallery objects by the Sequence property.</param>
		/// <param name="excludePrivateObjects">Indicates whether to exclude objects that are marked as private (IsPrivate = true).
		/// Objects that are private should not be shown to anonymous users.</param>
		/// <returns>
		/// Returns a collection of objects of type IGalleryObject whose
		/// parent is the current gallery object and are of the specified type.
		/// </returns>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual IGalleryObjectCollection GetChildGalleryObjects(bool sortBySequence, bool excludePrivateObjects)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a collection of gallery objects that are direct children of the current gallery object. Returns
		/// an empty list (Count = 0) if there are no child objects. Use the overload with the galleryObjectType
		/// parameter to return objects of the desired type. Use the overload with the sortBySequence parameter
		/// to sort the collection by sequence number. If the sortBySequence is not specified, the collection is
		/// not sorted in any particular order. Use the excludePrivateObjects parameter to optionally filter out private
		/// objects (if not specified, private objects are returned).
		/// </summary>
		/// <param name="galleryObjectType">A GalleryObjectType enum indicating the
		/// desired type of child objects to return.</param>
		/// <returns>
		/// Returns a collection of objects of type IGalleryObject whose
		/// parent is the current gallery object and are of the specified type.
		/// </returns>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual IGalleryObjectCollection GetChildGalleryObjects(GalleryObjectType galleryObjectType)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a collection of gallery objects that are direct children of the current gallery object. Returns
		/// an empty list (Count = 0) if there are no child objects. Use the overload with the galleryObjectType
		/// parameter to return objects of the desired type. Use the overload with the sortBySequence parameter
		/// to sort the collection by sequence number. If the sortBySequence is not specified, the collection is
		/// not sorted in any particular order. Use the excludePrivateObjects parameter to optionally filter out private
		/// objects (if not specified, private objects are returned).
		/// </summary>
		/// <param name="galleryObjectType">A GalleryObjectType enum indicating the
		/// desired type of child objects to return.</param>
		/// <param name="sortBySequence">Indicates whether to sort the child gallery objects by the Sequence property.</param>
		/// <returns>
		/// Returns a collection of objects of type IGalleryObject whose
		/// parent is the current gallery object and are of the specified type.
		/// </returns>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual IGalleryObjectCollection GetChildGalleryObjects(GalleryObjectType galleryObjectType, bool sortBySequence)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns a collection of gallery objects that are direct children of the current gallery object. Returns
		/// an empty list (Count = 0) if there are no child objects. Use the overload with the galleryObjectType
		/// parameter to return objects of the desired type. Use the overload with the sortBySequence parameter
		/// to sort the collection by sequence number. If the sortBySequence is not specified, the collection is
		/// not sorted in any particular order. Use the excludePrivateObjects parameter to optionally filter out private
		/// objects (if not specified, private objects are returned).
		/// </summary>
		/// <param name="galleryObjectType">A GalleryObjectType enum indicating the
		/// desired type of child objects to return.</param>
		/// <param name="sortBySequence">Indicates whether to sort the child gallery objects by the Sequence property.</param>
		/// <param name="excludePrivateObjects">Indicates whether to exclude objects that are marked as private (IsPrivate = true).
		/// Objects that are private should not be shown to anonymous users.</param>
		/// <returns>
		/// Returns a collection of objects of type IGalleryObject whose
		/// parent is the current gallery object and are of the specified type.
		/// </returns>
		/// <exception cref="System.NotSupportedException">Thrown when an inherited type
		/// does not allow the addition of child gallery objects.</exception>
		public virtual IGalleryObjectCollection GetChildGalleryObjects(GalleryObjectType galleryObjectType, bool sortBySequence, bool excludePrivateObjects)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Protected Virtual Methods

		/// <summary>
		/// This method provides an opportunity for a derived class to verify the thumbnail information for this instance has 
		/// been retrieved from the data store. This method is empty.
		/// </summary>
		/// <param name="thumbnail">A reference to the thumbnail display object for this instance.</param>
		protected virtual void VerifyThumbnailIsInflated(IDisplayObject thumbnail)
		{
			// Overridden in Album class.
		}

		/// <summary>
		/// Verifies the sequence of this instance within the album has been assigned. If the sequence has not yet been assigned, 
		/// default it to 1 higher than the highest sequence among its brothers and sisters.
		/// </summary>
		protected virtual void ValidateSequence()
		{
			if (this.Sequence == int.MinValue)
			{
				IGalleryObjectCollection siblingObjects = this.Parent.GetChildGalleryObjects(true);
				if (siblingObjects.Count > 0)
				{
					int maxSequence = siblingObjects[siblingObjects.Count - 1].Sequence;
					this.Sequence = (maxSequence > 0 ? maxSequence + 1 : 1);
				}
				else
					this.Sequence = 1;
			}
		}

		/// <summary>
		/// Verifies that the thumbnail image for this instance maps to an existing image file on disk. If not, set the
		///  <see cref="RegenerateThumbnailOnSave" />
		/// property to true so that the thumbnail image is created during the <see cref="Save" /> operation.
		/// <note type="implementnotes">The <see cref="Album" /> class overrides this method with an empty implementation, because albums don't have thumbnail
		/// images, at least not in the strictest sense.</note>
		/// </summary>
		protected virtual void CheckForThumbnailImage()
		{
			if (!System.IO.File.Exists(this.Thumbnail.FileNamePhysicalPath))
			{
				this.RegenerateThumbnailOnSave = true;
			}
		}

		/// <summary>
		/// This method provides an opportunity for a derived class to verify the optimized image maps to an existing file on disk.
		/// This method is empty.
		/// </summary>
		protected virtual void CheckForOptimizedImage()
		{
			// Overridden in Image class.
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Persist this gallery object to the data store.
		/// </summary>
		public void Save()
		{
			// Verify it is valid to save this object.
			ValidateSave();

			// Raise the Saving event.
			if (Saving != null)
			{
				Saving(this, new EventArgs());
			}

			// Persist to data store if the object is new (has not yet been saved) or it
			// has unsaved changes. The save behavior also updates the album's thumbnail if needed.
			if ((this._isNew) || (_hasChanges))
				this._saveBehavior.Save();

			if (this._hashKeyHasChanged)
			{
				// Since we just updated the hash key, clear out the application-wide list of hash keys so 
				// the next time they are requested, they are retrieved from the data store.
				MediaObjectHashKeys.Clear();
				this._hashKeyHasChanged = false;
			}

			ValidateThumbnailsAfterSave();

			this.HasChanges = false;
			this.IsNew = false;
			this.RegenerateThumbnailOnSave = false;
			this.RegenerateOptimizedOnSave = false;
			if (!this.IsInflated)
				this.IsInflated = true;

			// Raise the Saved event.
			if (Saved != null)
			{
				Saved(this, new EventArgs());
			}
		}

		/// <summary>
		/// Permanently delete this object from the data store and disk.
		/// </summary>
		public void Delete()
		{
			this.Delete(true);
		}

		/// <summary>
		/// Permanently delete this object from the data store, but leave it's associated file or directory on the hard disk.
		/// </summary>
		public void DeleteFromGallery()
		{
			this.Delete(false);
		}

		/// <summary>
		/// Set the parent of this gallery object to an instance of <see cref="NullGalleryObject" />.
		/// </summary>
		public void SetParentToNullObject()
		{
			this._parent = new NullObjects.NullGalleryObject();
		}

		/// <summary>
		/// Copy the current object and places it in the specified destination album. This method creates a completely separate copy
		/// of the original, including copying the physical files associated with this object. The copy is persisted to the data
		/// store and then returned to the caller. When copying albums, all the album's children, grandchildren, etc are copied,
		/// and any role permissions that are explicitly assigned to the source album are copied to the destination album, unless
		/// the copied album inherits the role throught the destination parent album. Inherited role permissions are not copied.
		/// </summary>
		/// <param name="destinationAlbum">The album to which the current object should be copied.</param>
		/// <param name="userName">The user name of the currently logged on user. This will be used for the audit fields of the
		/// copied objects.</param>
		/// <returns>
		/// Returns a new gallery object that is an exact copy of the original, except that it resides in the specified
		/// destination album, and of course has a new ID. Child objects are recursively copied.
		/// </returns>
		public virtual IGalleryObject CopyTo(IAlbum destinationAlbum, string userName)
		{
			IGalleryObject goCopy = Copy();

			string destPath = destinationAlbum.FullPhysicalPathOnDisk;
			bool doesOptimizedImageExistAndIsDifferentThanOriginalImage = (!String.IsNullOrEmpty(this.Optimized.FileName) && (this.Optimized.FileName != this.Original.FileName));

			#region Copy original file

			if (this.Original.DisplayType != DisplayObjectType.External)
			{
				string destOriginalFilename = HelperFunctions.ValidateFileName(destPath, this.Original.FileName);
				string destOriginalPath = System.IO.Path.Combine(destPath, destOriginalFilename);
				System.IO.File.Copy(this.Original.FileNamePhysicalPath, destOriginalPath);

				// Assign newly created copy of media file to the copy of our media object instance and update
				// various properties.
				goCopy.Original.FileInfo = new System.IO.FileInfo(destOriginalPath);
				goCopy.Hashkey = HelperFunctions.GetHashKeyUnique(goCopy.Original.FileInfo);
			}

			#endregion

			#region Copy optimized file

			// Determine path where optimized should be saved. If no optimized path is specified in the config file,
			// use the same directory as the original. Don't do anything if no optimized filename is specified or it's
			// the same file as the original.
			// FYI: Currently the optimized image is never external (only the original may be), but we test it anyway for future bullet-proofing.
			if ((this.Optimized.DisplayType != DisplayObjectType.External) && doesOptimizedImageExistAndIsDifferentThanOriginalImage)
			{
				string destOptimizedPathWithoutFilename = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(destPath, AppSetting.Instance.OptimizedPath);
				string destOptimizedFilepath = System.IO.Path.Combine(destOptimizedPathWithoutFilename, HelperFunctions.ValidateFileName(destOptimizedPathWithoutFilename, this.Optimized.FileName));
				if (System.IO.File.Exists(this.Optimized.FileNamePhysicalPath))
				{
					System.IO.File.Copy(this.Optimized.FileNamePhysicalPath, destOptimizedFilepath);
				}

				// Assign newly created copy of optimized image to the copy of our media object instance and update
				// various properties.
				goCopy.Optimized.FileInfo = new System.IO.FileInfo(destOptimizedFilepath);
				goCopy.Optimized.Width = this.Optimized.Width;
				goCopy.Optimized.Height = this.Optimized.Height;
				goCopy.Optimized.FileSizeKB = this.Optimized.FileSizeKB;
			}

			#endregion

			#region Copy thumbnail file

			// Determine path where thumbnail should be saved. If no thumbnail path is specified in the config file,
			// use the same directory as the original.
			// FYI: Currently the thumbnail image is never external (only the original may be), but we test it anyway for future bullet-proofing.
			if (this.Thumbnail.DisplayType != DisplayObjectType.External)
			{
				string destThumbnailPathWithoutFilename = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(destPath, AppSetting.Instance.ThumbnailPath);
				string destThumbnailFilepath = System.IO.Path.Combine(destThumbnailPathWithoutFilename, HelperFunctions.ValidateFileName(destThumbnailPathWithoutFilename, this.Thumbnail.FileName));
				if (System.IO.File.Exists(this.Thumbnail.FileNamePhysicalPath))
				{
					System.IO.File.Copy(this.Thumbnail.FileNamePhysicalPath, destThumbnailFilepath);
				}

				// Assign newly created copy of optimized image to the copy of our media object instance and update
				// various properties.
				goCopy.Thumbnail.FileInfo = new System.IO.FileInfo(destThumbnailFilepath);
				goCopy.Thumbnail.Width = this.Thumbnail.Width;
				goCopy.Thumbnail.Height = this.Thumbnail.Height;
				goCopy.Thumbnail.FileSizeKB = this.Thumbnail.FileSizeKB;
			}

			#endregion

			goCopy.Parent = destinationAlbum;

			HelperFunctions.UpdateAuditFields(goCopy, userName);
			goCopy.Save();

			return goCopy;
		}

		/// <summary>
		/// Copy the current object and return a reference to a new object that points to the physical directory or file for
		/// the current object. The copied object is new and not yet persisted to the data store. This method is typically called
		/// by the <see cref="CopyTo" /> method as the first step of creating a distinct, independent copy of an object.
		/// </summary>
		/// <returns>
		/// Returns a reference to a new object that is a copy of the current gallery object.
		/// </returns>
		public virtual IGalleryObject Copy()
		{
			IGalleryObject goCopy = Factory.CreateMediaObjectInstance(this.Original.FileInfo, (IAlbum)this.Parent, true, this.Original.ExternalHtmlSource, this.Original.ExternalType);

			goCopy.Title = this.Title;
			goCopy.IsPrivate = this.IsPrivate;

			return goCopy;
		}

		/// <summary>
		/// Move the current object to the specified destination album. This method moves the physical files associated with this
		/// object to the destination album's physical directory. This instance's <see cref="Save" /> method is invoked to persist the changes to the
		/// data store. When moving albums, all the album's children, grandchildren, etc are also moved.
		/// </summary>
		/// <param name="destinationAlbum">The album to which the current object should be moved.</param>
		public virtual void MoveTo(IAlbum destinationAlbum)
		{
			// Get list of albums whose thumbnails we'll update after the move operation.
			IGalleryObjectCollection albumsNeedingNewThumbnails = ValidateAlbumThumbnailsForMove(this.Id, destinationAlbum.Id);

			string destPath = destinationAlbum.FullPhysicalPathOnDisk;

			#region Move original file

			string destOriginalPath = String.Empty;
			if (System.IO.File.Exists(this.Original.FileNamePhysicalPath))
			{
				string destOriginalFilename = HelperFunctions.ValidateFileName(destPath, this.Original.FileName);
				destOriginalPath = System.IO.Path.Combine(destPath, destOriginalFilename);
				System.IO.File.Move(this.Original.FileNamePhysicalPath, destOriginalPath);
			}

			#endregion

			#region Move optimized file

			// Determine path where optimized should be saved. If no optimized path is specified in the config file,
			// use the same directory as the original.
			string destOptimizedFilepath = String.Empty;
			if ((!String.IsNullOrEmpty(this.Optimized.FileName)) && (!this.Optimized.FileName.Equals(this.Original.FileName)))
			{
				string destOptimizedPathWithoutFilename = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(destPath, AppSetting.Instance.OptimizedPath);
				destOptimizedFilepath = System.IO.Path.Combine(destOptimizedPathWithoutFilename, HelperFunctions.ValidateFileName(destOptimizedPathWithoutFilename, this.Optimized.FileName));
				if (System.IO.File.Exists(this.Optimized.FileNamePhysicalPath))
				{
					System.IO.File.Move(this.Optimized.FileNamePhysicalPath, destOptimizedFilepath);
				}
			}

			#endregion

			#region Move thumbnail file

			// Determine path where thumbnail should be saved. If no thumbnail path is specified in the config file,
			// use the same directory as the original.
			string destThumbnailPathWithoutFilename = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(destPath, AppSetting.Instance.ThumbnailPath);
			string destThumbnailFilepath = System.IO.Path.Combine(destThumbnailPathWithoutFilename, HelperFunctions.ValidateFileName(destThumbnailPathWithoutFilename, this.Thumbnail.FileName));
			if (System.IO.File.Exists(this.Thumbnail.FileNamePhysicalPath))
			{
				System.IO.File.Move(this.Thumbnail.FileNamePhysicalPath, destThumbnailFilepath);
			}

			#endregion

			this.Parent = destinationAlbum;
			this.Sequence = int.MinValue; // Reset the sequence so that it will be assigned a new value placing it at the end.

			// Update the FileInfo properties for the original, optimized and thumbnail objects. This is necessary in order to update
			// the filename, in case they were changed because the destination directory already had files with the same name.
			if (System.IO.File.Exists(destOriginalPath))
				this.Original.FileInfo = new System.IO.FileInfo(destOriginalPath);

			if (System.IO.File.Exists(destOptimizedFilepath))
				this.Optimized.FileInfo = new System.IO.FileInfo(destOptimizedFilepath);

			if (System.IO.File.Exists(destThumbnailFilepath))
				this.Thumbnail.FileInfo = new System.IO.FileInfo(destThumbnailFilepath);

			Save();

			// Now assign new thumbnails (if needed) to the albums we moved FROM. (The thumbnail for the destination album was updated in 
			// the Save() method.)
			foreach (IAlbum album in albumsNeedingNewThumbnails)
			{
				Album.AssignAlbumThumbnail(album, false, false, this.LastModifiedByUserName);
			}
		}

		#endregion

		#region Public Abstract Methods

		/// <summary>
		/// Inflate the current object by loading all properties from the data store. If the object is already inflated (<see cref="IsInflated" />=true), no action is taken.
		/// </summary>
		public abstract void Inflate();

		#endregion

		#region Public Override Methods

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="GalleryObject"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="GalleryObject"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Concat(base.ToString(), "; ID = ", this.Id, "; (", this.Title, ")");
		}

		/// <summary>
		/// Serves as a hash function for a particular type. The hash code is based on <see cref="Id" />.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Verifies, and corrects if necessary, the length and content of the title parameter.
		/// conforms to business rules. If the maximum length is exceeded, it is shortened as required.
		/// <note type="implementnotes">The <see cref="Album" /> class overrides this method.</note>
		/// </summary>
		/// <param name="title">The title.</param>
		/// <returns>Returns the title parameter, modified if necessary, so that it conforms to business rules.</returns>
		protected virtual string ValidateTitle(string title)
		{
			// Validate that the title is less than the maximum limit. Truncate it if necessary.
			// Fyi: The Album subclass does its own validation, so this method won't be executed when it is an album.

			int maxLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.MediaObjectTitleLength;

			if (title.Length > maxLength)
			{
				title = title.Substring(0, maxLength).Trim();
			}
			return title;
		}

		/// <summary>
		/// Prepare a list of albums whose thumbnail images will need updating after the move operation completes. Only applies to the album
		/// hierarchy the object is moved FROM (the thumbnail in the destination album is updated by code in the <see cref="Save" /> method). It works by
		/// analyzing the parent albums, recursively, of the current gallery object, until reaching either the root album or the specified
		/// destination album, looking for any album that has a thumbnail matching the specified thumbnailMediaObjectId. For each match,
		/// clear out the ThumbnailMediaObjectId property, add the album to a list, and finally return it. The caller is responsible 
		/// for iterating through this list and calling <see cref="Album.AssignAlbumThumbnail" /> for each album after the move operation is complete.
		/// This method should be called before the move operation takes place.
		/// </summary>
		/// <param name="mediaObjectId">The ID of a media object. If this media object is assigned as the thumbnail for any
		/// albums in the current gallery object's parent albums, then add those albums to a list and return it to the caller.</param>
		/// <param name="destinationAlbumId">The ID of the album the current gallery object will be in after the move operation completes.</param>
		/// <returns>Return a list of albums whose thumbnail images will need updating after the move operation completes.</returns>
		protected IGalleryObjectCollection ValidateAlbumThumbnailsForMove(int mediaObjectId, int destinationAlbumId)
		{
			IGalleryObjectCollection albumsNeedingNewThumbnails = new GalleryObjectCollection();
			IGalleryObject albumInSourceHierarchy = this.Parent;
			IAlbum albumToTest;

			while (!(albumInSourceHierarchy is NullObjects.NullGalleryObject))
			{
				// If we're at the same level as the destination album, don't go any further since there isn't any need to clear
				// out the album thumbnails for any albums above this point.
				if (albumInSourceHierarchy.Id == destinationAlbumId)
					break;

				albumToTest = (IAlbum)albumInSourceHierarchy;
				if ((mediaObjectId > 0) && (mediaObjectId == albumToTest.ThumbnailMediaObjectId))
				{
					albumToTest.ThumbnailMediaObjectId = 0;
					albumsNeedingNewThumbnails.Add(albumToTest);
				}

				albumInSourceHierarchy = albumInSourceHierarchy.Parent;
			}

			return albumsNeedingNewThumbnails;
		}

		#endregion

		#region Private Methods

		private void RecalculateFilePaths()
		{
			string albumPath = this._parent.FullPhysicalPathOnDisk;

			// Thumbnail
			if (!String.IsNullOrEmpty(this._thumbnail.FileName))
				this._thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(albumPath, this._thumbnail.FileName);
			else
				this._thumbnail.FileNamePhysicalPath = String.Empty;

			// Optimized
			if (!String.IsNullOrEmpty(this._optimized.FileName))
				this._optimized.FileNamePhysicalPath = System.IO.Path.Combine(albumPath, this._optimized.FileName);
			else
				this._optimized.FileNamePhysicalPath = String.Empty;

			// Original
			if (!String.IsNullOrEmpty(this._original.FileName))
				this._original.FileNamePhysicalPath = System.IO.Path.Combine(albumPath, this._original.FileName);
			else
				this._original.FileNamePhysicalPath = String.Empty;
		}

		private void VerifyObjectIsInflated(string propertyValue)
		{
			// If the string is empty, and this is not a new object, and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((String.IsNullOrEmpty(propertyValue)) && (!this.IsNew) && (!this.IsInflated))
			{
				this.Inflate();
			}
		}

		private void VerifyObjectIsInflated(DateTime propertyValue)
		{
			// If the string is empty, and this is not a new object, and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((propertyValue == DateTime.MinValue) && (!this.IsNew) && (!this.IsInflated))
			{
				this.Inflate();
			}
		}

		private void VerifyObjectIsInflated(int propertyValue)
		{
			// If the int = int.MinValue, and this is not a new object, and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((propertyValue == int.MinValue) && (!this.IsNew) && (!this.IsInflated))
			{
				this.Inflate();
			}
		}

		private void VerifyObjectIsInflated()
		{
			// If this is a pre-existing object (i.e. one that exists in the data store), and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((!this.IsNew) && (!this.IsInflated))
			{
				this.Inflate();
			}
		}

		private void ValidateSave()
		{
			if ((!this.IsNew) && (!this.IsInflated))
			{
				throw new System.InvalidOperationException(Resources.GalleryObject_ValidateSave_Ex_Msg);
			}

			ValidateSequence();

			// Set RegenerateThumbnailOnSave to true if thumbnail image doesn't exist.
			CheckForThumbnailImage();

			// Set RegenerateOptimizedOnSave to true if optimized image doesn't exist. This is an empty virtual method
			// that is overridden in the Image class. That is, this method does nothing for non-images.
			CheckForOptimizedImage();

			// Make sure the audit fields have been set.
			ValidateAuditFields();
		}

		private void ValidateAuditFields()
		{
			if (String.IsNullOrEmpty(this.CreatedByUserName))
				throw new BusinessException("The property CreatedByUsername must be set to the currently logged on user before this object can be saved.");

			if (this.DateAdded == DateTime.MinValue)
				throw new BusinessException("The property DateAdded must be assigned a valid date before this object can be saved.");

			if (String.IsNullOrEmpty(this.LastModifiedByUserName))
				throw new BusinessException("The property LastModifiedByUsername must be set to the currently logged on user before this object can be saved.");

			DateTime aFewMomentsAgo = DateTime.Now.Subtract(new TimeSpan(0, 10, 0)); // 10 minutes ago
			if (this.HasChanges && (this.DateLastModified < aFewMomentsAgo))
				throw new BusinessException("The property DateLastModified must be assigned the current date before this object can be saved.");

			// Make sure a valid date is assigned to the DateAdded property. If it is still DateTime.MinValue,
			// update it with the current date/time.
			//System.Diagnostics.Debug.Assert((this.IsNew || ((!this.IsNew) && (this.DateAdded > DateTime.MinValue))),
			//  String.Format(CultureInfo.CurrentCulture, "Media objects and albums that have been saved to the data store should never have the property DateAdded=MinValue. IsNew={0}; DateAdded={1}",
			//  this.IsNew, this.DateAdded.ToLongDateString()));

			//if (this.DateAdded == DateTime.MinValue)
			//{
			//  this.DateAdded = DateTime.Now;
			//}
		}

		private void ValidateThumbnailsAfterSave()
		{
			// Update the album's thumbnail if necessary.
			IAlbum parentAlbum = this.Parent as IAlbum;
			if ((parentAlbum != null) && (parentAlbum.ThumbnailMediaObjectId == 0))
			{
				Album.AssignAlbumThumbnail(parentAlbum, true, false, this.LastModifiedByUserName);
			}
		}

		private void Delete(bool deleteFromFileSystem)
		{
			this._deleteBehavior.Delete(deleteFromFileSystem);

			if (!(this._parent is NullObjects.NullGalleryObject))
			{
				IAlbum albumParent = (IAlbum)this._parent;
				this._parent.Remove(this);

				Album.AssignAlbumThumbnail(albumParent, true, false, this.LastModifiedByUserName);
			}
		}

		#endregion

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="obj"/> is not the same type as this instance. </exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			else
			{
				IGalleryObject other = obj as IGalleryObject;
				if (other != null)
					return this.Sequence.CompareTo(other.Sequence);
				else
					return 1;
			}
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this._hasBeenDisposed)
			{
				// Dispose of resources held by this instance.
				if (this._thumbnail != null)
				{
					this._thumbnail.Dispose();
				}

				if (this._optimized != null)
				{
					this._optimized.Dispose();
				}

				if (this._original != null)
				{
					this._original.Dispose();
				}

				if (this._parent != null)
				{
					this._parent.Dispose();
				}

				// Set the sentinel.
				this._hasBeenDisposed = true;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}

using System;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Represents a role that encapsulates a set of permissions for one or more albums in Gallery Server. Each user
	/// is assigned to zero or more roles.
	/// </summary>
	public class GalleryServerRole : IGalleryServerRole, IComparable
	{
		#region Private Fields

		private string _roleName;
		private bool _allowViewAlbumOrMediaObject;
		private bool _allowViewOriginalImage;
		private bool _allowAddMediaObject;
		private bool _allowAddChildAlbum;
		private bool _allowEditMediaObject;
		private bool _allowEditAlbum;
		private bool _allowDeleteMediaObject;
		private bool _allowDeleteChildAlbum;
		private bool _allowSynchronize;
		private bool _allowAdministerSite;
		private bool _hideWatermark;

		private readonly IIntegerCollection _rootAlbumIds;
		private readonly IIntegerCollection _allAlbumIds;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a string that uniquely identifies the role.
		/// </summary>
		/// <value>The name of the role.</value>
		public string RoleName
		{
			get { return _roleName; }
			set { _roleName = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to view albums and media objects.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to view albums and media objects; otherwise, <c>false</c>.
		/// </value>
		public bool AllowViewAlbumOrMediaObject
		{
			get { return _allowViewAlbumOrMediaObject; }
			set { _allowViewAlbumOrMediaObject = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to view the original,
		/// high resolution version of an image. This setting applies only to images. It has no effect if there are no
		/// high resolution images in the album or albums to which this role applies.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to view the original,
		/// high resolution version of an image; otherwise, <c>false</c>.
		/// </value>
		public bool AllowViewOriginalImage
		{
			get { return _allowViewOriginalImage; }
			set { _allowViewOriginalImage = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to add media objects to an album.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to add media objects to an album; otherwise, <c>false</c>.
		/// </value>
		public bool AllowAddMediaObject
		{
			get { return _allowAddMediaObject; }
			set { _allowAddMediaObject = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to create child albums.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to create child albums; otherwise, <c>false</c>.
		/// </value>
		public bool AllowAddChildAlbum
		{
			get { return _allowAddChildAlbum; }
			set { _allowAddChildAlbum = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to edit a media object.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to edit a media object; otherwise, <c>false</c>.
		/// </value>
		public bool AllowEditMediaObject
		{
			get { return _allowEditMediaObject; }
			set { _allowEditMediaObject = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to edit an album.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to edit an album; otherwise, <c>false</c>.
		/// </value>
		public bool AllowEditAlbum
		{
			get { return _allowEditAlbum; }
			set { _allowEditAlbum = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to delete media objects within an album.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to delete media objects within an album; otherwise, <c>false</c>.
		/// </value>
		public bool AllowDeleteMediaObject
		{
			get { return _allowDeleteMediaObject; }
			set { _allowDeleteMediaObject = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to delete child albums.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to delete child albums; otherwise, <c>false</c>.
		/// </value>
		public bool AllowDeleteChildAlbum
		{
			get { return _allowDeleteChildAlbum; }
			set { _allowDeleteChildAlbum = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has permission to synchronize an album.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has permission to synchronize an album; otherwise, <c>false</c>.
		/// </value>
		public bool AllowSynchronize
		{
			get { return _allowSynchronize; }
			set { _allowSynchronize = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user has administrative permission for all albums. This permission
		/// automatically applies to all albums; it cannot be selectively applied.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user has administrative permission for all albums; otherwise, <c>false</c>.
		/// </value>
		public bool AllowAdministerSite
		{
			get { return _allowAdministerSite; }
			set { _allowAdministerSite = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user assigned to this role has a watermark applied to images.
		/// This setting has no effect if watermarks are not used. A true value means the user does not see the watermark;
		/// a false value means the watermark is applied.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the user assigned to this role has a watermark applied to images; otherwise, <c>false</c>.
		/// </value>
		public bool HideWatermark
		{
			get { return _hideWatermark; }
			set { _hideWatermark = value; }
		}

		/// <summary>
		/// Gets the list of all top-level album IDs for which this role applies. Does not include any descendents
		/// of the album. Setting this property causes the AllAlbumIds property to be cleared out (Count = 0) since a different
		/// list of root album IDs implies the exploded list is also different. Validation code in the AllAlbumIds getter
		/// will throw an exception if it is called after it has been cleared. The AllAlbumIds property is automatically reloaded
		/// from the data store during Save(). Note that adding or removing items to this list does not cause AllAlbumIds to
		/// be cleared out, although calling Save() will still reload the list from the data store.
		/// </summary>
		/// <value>The list of all top-level album IDs for which this role applies.</value>
		public IIntegerCollection RootAlbumIds
		{
			get { return _rootAlbumIds; }
			//set 
			//{
			//  // Clear out the list of all album IDs if setting this property after it was previously set, since a different
			//  // list of root album IDs implies the exploded list is also different. Validation code in the AllAlbumIds getter
			//  // will throw an exception if it is called after it has been cleared.
			//  if ((_rootAlbumIds != null) && (_rootAlbumIds.Count > 0))
			//  {
			//    _allAlbumIds.Clear();
			//  }

			//  _rootAlbumIds = value; 
			//}
		}

		/// <summary>
		/// Gets the list of all album IDs for which this role applies. Includes all descendents of all applicable albums.
		/// Calling the Save() method automatically reloads this property from the data store.
		/// </summary>
		/// <value>The list of all album IDs for which this role applies.</value>
		/// <exception cref="BusinessException">Thrown when <see cref="RootAlbumIds"/> has more than one item but the internal
		/// field for this property (_allAlbumIds) is empty.</exception>
		public IIntegerCollection AllAlbumIds
		{
			get
			{
				if ((_allAlbumIds.Count == 0) && (_rootAlbumIds.Count > 0))
				{
					throw new BusinessException(GalleryServerPro.Business.Properties.Resources.GalleryServerRole_AllAlbumIds_Ex_Msg);
				}

				return _allAlbumIds;
			}
			//set { _allAlbumIds = value; }
		}

		#endregion

		#region Constructors

		private GalleryServerRole() { } // Hide default constructor

		/// <summary>
		/// Create a GalleryServerRole instance corresponding to the specified parameters. Throws an exception if a role with the
		/// specified name already exists in the data store.
		/// </summary>
		/// <param name="roleName">A string that uniquely identifies the role.</param>
		/// <param name="allowViewAlbumOrMediaObject">A value indicating whether the user assigned to this role has permission to view albums
		/// and media objects.</param>
		/// <param name="allowViewOriginalImage">A value indicating whether the user assigned to this role has permission to view the original,
		/// high resolution version of an image. This setting applies only to images. It has no effect if there are no
		/// high resolution images in the album or albums to which this role applies.</param>
		/// <param name="allowAddMediaObject">A value indicating whether the user assigned to this role has permission to add media objects to an album.</param>
		/// <param name="allowAddChildAlbum">A value indicating whether the user assigned to this role has permission to create child albums.</param>
		/// <param name="allowEditMediaObject">A value indicating whether the user assigned to this role has permission to edit a media object.</param>
		/// <param name="allowEditAlbum">A value indicating whether the user assigned to this role has permission to edit an album.</param>
		/// <param name="allowDeleteMediaObject">A value indicating whether the user assigned to this role has permission to delete media objects within an album.</param>
		/// <param name="allowDeleteChildAlbum">A value indicating whether the user assigned to this role has permission to delete child albums.</param>
		/// <param name="allowSynchronize">A value indicating whether the user assigned to this role has permission to synchronize an album.</param>
		/// <param name="allowAdministerSite">A value indicating whether the user has administrative permission for all albums. This permission
		/// automatically applies to all albums; it cannot be selectively applied.</param>
		/// <param name="hideWatermark">A value indicating whether the user assigned to this role has a watermark applied to images.
		/// This setting has no effect if watermarks are not used. A true value means the user does not see the watermark;
		/// a false value means the watermark is applied.</param>
		/// <returns>Returns a GalleryServerRole instance corresponding to the specified parameters.</returns>
		public GalleryServerRole(string roleName, bool allowViewAlbumOrMediaObject, bool allowViewOriginalImage, bool allowAddMediaObject, bool allowAddChildAlbum, bool allowEditMediaObject, bool allowEditAlbum, bool allowDeleteMediaObject, bool allowDeleteChildAlbum, bool allowSynchronize, bool allowAdministerSite, bool hideWatermark)
		{
			this._roleName = roleName;
			this._allowViewAlbumOrMediaObject = allowViewAlbumOrMediaObject;
			this._allowViewOriginalImage = allowViewOriginalImage;
			this._allowAddMediaObject = allowAddMediaObject;
			this._allowAddChildAlbum = allowAddChildAlbum;
			this._allowEditMediaObject = allowEditMediaObject;
			this._allowEditAlbum = allowEditAlbum;
			this._allowDeleteMediaObject = allowDeleteMediaObject;
			this._allowDeleteChildAlbum = allowDeleteChildAlbum;
			this._allowSynchronize = allowSynchronize;
			this._allowAdministerSite = allowAdministerSite;
			this._hideWatermark = hideWatermark;

			this._rootAlbumIds = new IntegerCollection();
			this._rootAlbumIds.Cleared += new EventHandler(_rootAlbumIds_Cleared);

			this._allAlbumIds = new IntegerCollection();
		}

		#endregion

		#region Event Handlers

		void _rootAlbumIds_Cleared(object sender, EventArgs e)
		{
			// We need to smoke the all albums list whenever the list of root albums has been cleared.
			if (this._allAlbumIds != null)
				this._allAlbumIds.Clear();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Add the specified album to the list of all album IDs. This is used by data and business layer code to
		/// populate the list when it is instantiated or saved.
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the album to add to the list.</param>
		public void AddToAllAlbumIds(int albumId)
		{
			this._allAlbumIds.Add(albumId);
		}

		/// <summary>
		/// Clears the list of album IDs stored in the <see cref="AllAlbumIds"/> property.
		/// </summary>
		public void ClearAllAlbumIds()
		{
			this._allAlbumIds.Clear();
		}

		/// <summary>
		/// Persist this gallery server role to the data store. The list of top-level albums this role applies to, which is stored
		/// in the <see cref="RootAlbumIds"/> property, is also saved. The data provider automatically repopulates the <see cref="AllAlbumIds"/> property.
		/// </summary>
		public void Save()
		{
			Factory.GetDataProvider().Role_Save(this);
		}

		/// <summary>
		/// Permanently delete this gallery server role from the data store, including the list of role/album relationships
		/// associated with this role.
		/// </summary>
		public void Delete()
		{
			Factory.GetDataProvider().Role_Delete(this);
		}

		/// <summary>
		/// Creates a deep copy of this instance, including the RootAlbumIds and AllAlbumIds properties. The RoleName property
		/// of the copied object is empty and must be assigned before persisting the copy to the data store.
		/// </summary>
		/// <returns>Returns a deep copy of this instance.</returns>
		public IGalleryServerRole Copy()
		{
			IGalleryServerRole role = Factory.CreateGalleryServerRoleInstance(String.Empty, AllowViewAlbumOrMediaObject, AllowViewOriginalImage,
			                                                                  AllowAddMediaObject, AllowAddChildAlbum, AllowEditMediaObject, AllowEditAlbum,
			                                                                  AllowDeleteMediaObject, AllowDeleteChildAlbum, AllowSynchronize, 
			                                                                  AllowAdministerSite, HideWatermark);

			role.AllAlbumIds.AddRange(AllAlbumIds);
			role.RootAlbumIds.AddRange(RootAlbumIds);
			
			return role;
		}

		#endregion

		#region Private Methods


		#endregion

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: 
		/// Less than zero: This instance is less than <paramref name="obj"/>. Zero: This instance is equal to <paramref name="obj"/>. 
		/// Greater than zero: This instance is greater than <paramref name="obj"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="obj"/> is not the same type as this instance. </exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			else
			{
				GalleryServerRole other = (GalleryServerRole)obj;
				return String.Compare(this.RoleName, other.RoleName, StringComparison.CurrentCulture);
			}
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="GalleryServerRole"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return (this.RoleName.GetHashCode() ^ this.AllAlbumIds.GetHashCode()
			        ^ this.AllowAddChildAlbum.GetHashCode() ^ this.AllowAddMediaObject.GetHashCode()
			        ^ this.AllowAdministerSite.GetHashCode() ^ this.AllowDeleteChildAlbum.GetHashCode()
			        ^ this.AllowDeleteMediaObject.GetHashCode() ^ this.AllowEditAlbum.GetHashCode()
			        ^ this.AllowEditMediaObject.GetHashCode() ^ this.AllowSynchronize.GetHashCode()
			        ^ this.AllowViewAlbumOrMediaObject.GetHashCode() ^ this.AllowViewOriginalImage.GetHashCode()
			        ^ this.HideWatermark.GetHashCode());
		}

		#endregion
	}

}

using System;
using System.Globalization;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Properties;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// The <see cref="ExternalMediaObject" /> class represents a media object within Gallery Server Pro that references an 
	/// externally stored object, such as a video on YouTube or Silverlight.streaming.net.
	/// </summary>
	public class ExternalMediaObject : GalleryObject
	{
		#region Private Fields


		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of a ExternalMediaObject object.
		/// </summary>
		/// <param name="externalHtmlSource">The HTML that defines an externally stored media object, such as one hosted at 
		/// YouTube or Silverlight.live.com.</param>
		/// <param name="mimeType">Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
		/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg").</param>
		/// <param name="parentAlbum">The album that contains this object. May be null.</param>
		internal ExternalMediaObject(string externalHtmlSource, MimeTypeCategory mimeType, IAlbum parentAlbum)
			: this(int.MinValue, parentAlbum != null ? parentAlbum.Id : int.MinValue, parentAlbum, string.Empty, string.Empty, string.Empty,
			       int.MinValue, int.MinValue, int.MinValue, string.Empty, int.MinValue, int.MinValue, int.MinValue, externalHtmlSource, mimeType, int.MinValue,
			       String.Empty, DateTime.MinValue, String.Empty, DateTime.MinValue, parentAlbum != null ? parentAlbum.IsPrivate : false, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of a ExternalMediaObject object.
		/// </summary>
		/// <param name="id">The ID that uniquely identifies this object. Specify int.MinValue for a new object.</param>
		/// <param name="parentId">The ID of the album that contains this object.</param>
		/// <param name="parentAlbum">The album that contains this object.</param>
		/// <param name="title">The title of this image.</param>
		/// <param name="hashKey">The hash key that uniquely identifies the original file.</param>
		/// <param name="thumbnailFilename">The filename of the thumbnail image.</param>
		/// <param name="thumbnailWidth">The width (px) of the thumbnail image.</param>
		/// <param name="thumbnailHeight">The height (px) of the thumbnail image.</param>
		/// <param name="thumbnailSizeKb">The size (KB) of the thumbnail image.</param>
		/// <param name="originalFilename">The filename of the original image.</param>
		/// <param name="originalWidth">The width (px) of the original image.</param>
		/// <param name="originalHeight">The height (px) of the original image.</param>
		/// <param name="originalSizeKb">The size (KB) of the original image.</param>
		/// <param name="externalHtmlSource">The HTML that defines an externally stored media object, such as one hosted at 
		/// YouTube or Silverlight.live.com.</param>
		/// <param name="mimeType">Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
		/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg").</param>
		/// <param name="sequence">An integer that represents the order in which this image should appear when displayed.</param>
		/// <param name="createdByUsername">The user name of the account that originally added this object to the data store.</param>
		/// <param name="dateAdded">The date this image was added to the data store.</param>
		/// <param name="lastModifiedByUsername">The user name of the account that last modified this object.</param>
		/// <param name="dateLastModified">The date this object was last modified.</param>
		/// <param name="isPrivate">Indicates whether this object should be hidden from un-authenticated (anonymous) users.</param>
		/// <param name="isInflated">A bool indicating whether this object is fully inflated.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException">Thrown when 
		/// the file parameter is specified (not null) and the file it refers to is not in the same directory
		/// as the parent album's directory.</exception>
		internal ExternalMediaObject(int id, int parentId, IAlbum parentAlbum, string title, string hashKey, string thumbnailFilename,
		                             int thumbnailWidth, int thumbnailHeight, int thumbnailSizeKb, string originalFilename,
		                             int originalWidth, int originalHeight, int originalSizeKb, string externalHtmlSource, MimeTypeCategory mimeType, int sequence,
		                             string createdByUsername, DateTime dateAdded, string lastModifiedByUsername, DateTime dateLastModified,
		                             bool isPrivate, bool isInflated)
		{
			this.Id = id;
			this.Title = title;
			this.Sequence = sequence;
			this.Hashkey = hashKey;
			this.CreatedByUserName = createdByUsername;
			this.DateAdded = dateAdded;
			this.LastModifiedByUserName = lastModifiedByUsername;
			this.DateLastModified = dateLastModified;
			this.IsPrivate = isPrivate;

			// Set parent to album instance if present, otherwise create default Album instance if the album ID
			// is specified. If neither are specified, do not set parent (client will, however, need to set the
			// parent before saving).
			if (parentAlbum != null)
				this.Parent = parentAlbum;
			else if (parentId != int.MinValue)
				this.Parent = Factory.CreateAlbumInstance(parentId);

			string parentPhysicalPath = this.Parent.FullPhysicalPathOnDisk;

			// Thumbnail image
			this.Thumbnail = DisplayObject.CreateInstance(this, thumbnailFilename, thumbnailWidth, thumbnailHeight, DisplayObjectType.Thumbnail, new ExternalThumbnailCreator(this));
			this.Thumbnail.FileSizeKB = thumbnailSizeKb;
			if (thumbnailFilename.Length > 0)
			{
				// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
				string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(parentPhysicalPath, AppSetting.Instance.ThumbnailPath);
				this.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, thumbnailFilename);
			}

			// ExternalMediaObject instances do not have an optimized version.
			this.Optimized = new NullObjects.NullDisplayObject();

			// Original file
			this.Original = DisplayObject.CreateInstance(this, DisplayObjectType.External, mimeType);
			this.Original.ExternalHtmlSource = externalHtmlSource;

			switch (mimeType)
			{
				case MimeTypeCategory.Audio:
					this.Original.Width = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.DefaultAudioPlayerWidth;
					this.Original.Height = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.DefaultAudioPlayerHeight;
					break;
				case MimeTypeCategory.Video:
					this.Original.Width = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.DefaultVideoPlayerWidth;
					this.Original.Height = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.DefaultVideoPlayerHeight;
					break;
				case MimeTypeCategory.Image:
				case MimeTypeCategory.Other:
					this.Original.Width = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.DefaultGenericObjectWidth;
					this.Original.Height = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.DefaultGenericObjectHeight;
					break;
			}

			this.SaveBehavior = Factory.GetMediaObjectSaveBehavior(this);
			this.DeleteBehavior = Factory.GetMediaObjectDeleteBehavior(this);

			this.IsInflated = isInflated;

			// Setting the previous properties has caused HasChanges = true, but we don't want this while
			// we're instantiating a new object. Reset to false.
			this.HasChanges = false;

			// Set up our event handlers.
			//this.Saving += new EventHandler(Image_Saving); // Don't need
			this.Saved += new EventHandler(Image_Saved);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Inflate the current object by loading all properties from the data store. If the object is already inflated 
		/// (<see cref="IGalleryObject.IsInflated"/>=true), no action is taken.
		/// </summary>
		public override void Inflate()
		{
			// If this is not a new object, and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((!this.IsNew) && (!this.IsInflated))
			{
				Factory.LoadExternalMediaObjectInstance(this);

				if ((!this.IsInflated) || (this.HasChanges))
					throw new System.InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.GenericMediaObject_Inflate_Ex_Msg, this.IsInflated, this.HasChanges));
			}
		}

		#endregion

		#region Event Handlers

		void Image_Saved(object sender, EventArgs e)
		{
			// This event is fired when the Save() method is called, after all data is saved.

			#region Assign DisplayObject.MediaObjectId

			// If the MediaObjectId has not yet been assigned, do so now. This will occur after a media object is first
			// saved, since that is when the ID is generated.
			if (this.Thumbnail.MediaObjectId == int.MinValue)
			{
				this.Thumbnail.MediaObjectId = this.Id;
			}

			if (this.Optimized.MediaObjectId == int.MinValue)
			{
				this.Optimized.MediaObjectId = this.Id;
			}

			if (this.Original.MediaObjectId == int.MinValue)
			{
				this.Original.MediaObjectId = this.Id;
			}

			#endregion
		}

		#endregion
	}
}

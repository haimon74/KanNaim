using System;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using TechInfoSystems.TracingTools;
using tt = TechInfoSystems.TracingTools;
using GalleryServerPro.Business.Properties;
using GalleryServerPro.Configuration;
using System.Collections.Generic;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Contains functionality for creating and retrieving various business objects. Use methods in this class instead of instantiating
	/// certain objects directly. This includes instances of <see cref="Image" />, <see cref="Video" />, <see cref="Audio" />, 
	/// <see cref="GenericMediaObject" />, and <see cref="Album" />.
	/// </summary>
	public static class Factory
	{
		#region Private Fields


		#endregion

		#region Gallery Object Methods

		/// <overloads>Create a fully inflated, properly typed gallery object instance based on the specified parameters.</overloads>
		/// <summary>
		/// Create a fully inflated, properly typed instance based on the specified <see cref="IGalleryObject.Id">ID</see>. An 
		/// additional call to the data store is made to determine the object's type. When you know the type you want (<see cref="Album" />,
		/// <see cref="Image" />, etc), use the overload that takes the galleryObjectType parameter, or call the specific Factory method that 
		/// loads the desired type, as those are more efficient. This method is guaranteed to not return null. If no object is found
		/// that matches the ID, an <see cref="UnsupportedMediaObjectTypeException" /> exception is thrown. If both a media object and an 
		/// album exist with the <paramref name="id"/>, the media object reference is returned.
		/// </summary>
		/// <param name="id">An integer representing the <see cref="IGalleryObject.Id">ID</see> of the media object or album to retrieve from the
		/// data store.</param>
		/// <returns>Returns an <see cref="IGalleryObject" /> object for the <see cref="IGalleryObject.Id">ID</see>. This method is guaranteed to not
		/// return null.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when no media object with the specified <see cref="IGalleryObject.Id">ID</see> 
		/// is found in the data store.</exception>
		public static IGalleryObject LoadGalleryObjectInstance(int id)
		{
			// Figure out what type the ID refers to (album, image, video, etc) and then call the overload of this method.
			return LoadGalleryObjectInstance(id, HelperFunctions.DetermineGalleryObjectType(id));
		}

		/// <summary>
		/// Create a fully inflated, properly typed instance based on the specified parameters. If the galleryObjectType
		/// parameter is All, None, or Unknown, then an additional call to the data store is made
		/// to determine the object's type. If no object is found that matches the ID and gallery object type, an 
		/// <see cref="UnsupportedMediaObjectTypeException" /> exception is thrown. When you know the type you want (<see cref="Album" />,
		/// <see cref="Image" />, etc), specify the exact galleryObjectType, or call the specific Factory method that 
		/// loads the desired type, as that is more efficient. This method is guaranteed to not return null.
		/// </summary>
		/// <param name="id">An integer representing the <see cref="IGalleryObject.Id">ID</see> of the media object or album to retrieve from the
		/// data store.</param>
		/// <param name="galleryObjectType">The type of gallery object that the id parameter represents. If the type is 
		/// unknown, the Unknown enum value can be specified. Specify the actual type if possible (e.g. Video, Audio, Image, 
		/// etc.), as it is more efficient.</param>
		/// <returns>Returns an <see cref="IGalleryObject" /> based on the ID. This method is guaranteed to not return null.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when a particular media object type is requested (e.g. Image, Video, etc.), 
		/// but no media object with the specified ID is found in the data store.</exception>
		/// <exception cref="InvalidAlbumException">Thrown when an album is requested but no album with the specified ID is found in the data store.</exception>
		public static IGalleryObject LoadGalleryObjectInstance(int id, GalleryObjectType galleryObjectType)
		{
			// If the gallery object type is vague, we need to figure it out.
			if ((galleryObjectType == GalleryObjectType.All) || (galleryObjectType == GalleryObjectType.None) || (galleryObjectType == GalleryObjectType.Unknown))
			{
				galleryObjectType = HelperFunctions.DetermineGalleryObjectType(id);
			}

			IGalleryObject go = new NullObjects.NullGalleryObject();

			switch (galleryObjectType)
			{
				case GalleryObjectType.Album:
					{
						go = Factory.LoadAlbumInstance(id, false); break;
					}
				case GalleryObjectType.Image:
					{
						go = Factory.LoadImageInstance(id); break;
					}
				case GalleryObjectType.Video:
					{
						go = Factory.LoadVideoInstance(id); break;
					}
				case GalleryObjectType.Audio:
					{
						go = Factory.LoadAudioInstance(id); break;
					}
				case GalleryObjectType.Generic:
				case GalleryObjectType.Unknown:
					{
						go = Factory.LoadGenericMediaObjectInstance(id); break;
					}
				default:
					{
						throw new UnsupportedMediaObjectTypeException();
					}
			}

			return go;
		}

		#endregion

		#region Media Object Methods

		#region General Media Object Methods

		/// <overloads>
		/// Create a properly typed Gallery Object instance (e.g. <see cref="Image" />, <see cref="Video" />, etc.) from the specified parameters.
		/// </overloads>
		/// <summary>
		/// Create a properly typed Gallery Object instance (e.g. <see cref="Image" />, <see cref="Video" />, etc.) for the media file
		/// represented by <paramref name="mediaObjectFilePath"/> and belonging to the album specified by <paramref name="parentAlbum"/>.
		/// </summary>
		/// <param name="mediaObjectFilePath">The fully qualified name of the media object file, or the relative filename.
		/// The file must already exist in the album's directory. If the file has a matching record in the data store,
		/// a reference to the existing object is returned. Otherwise, a new instance is returned. For new instances,
		/// call <see cref="IGalleryObject.Save" /> to persist the object to the data store. A
		/// <see cref="UnsupportedMediaObjectTypeException" /> is thrown when the specified file cannot 
		/// be added to Gallery Server, perhaps because it is an unsupported type or the file is corrupt.</param>
		/// <param name="parentAlbum">The album in which the media object exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns a properly typed Gallery Object instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when <paramref name="mediaObjectFilePath"/> has a file 
		/// extension that Gallery Server Pro is configured to reject.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when the  
		/// mediaObjectFilePath parameter refers to a file that is not in the same directory as the parent album's directory.</exception>
		public static IGalleryObject CreateMediaObjectInstance(string mediaObjectFilePath, IAlbum parentAlbum)
		{
			return CreateMediaObjectInstance(new System.IO.FileInfo(mediaObjectFilePath), parentAlbum);
		}

		/// <summary>
		/// Create a properly typed Gallery Object instance (e.g. <see cref="Image" />, <see cref="Video" />, etc.) for the media file
		/// represented by <paramref name="mediaObjectFile"/> and belonging to the album specified by <paramref name="parentAlbum"/>.
		/// </summary>
		/// <param name="mediaObjectFile">A <see cref="System.IO.FileInfo" /> object representing a supported media object type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object with no assigned parent album is always returned, regardless of the 
		/// existence of the file. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store. A
		/// <see cref="UnsupportedMediaObjectTypeException" /> is thrown when the specified file cannot 
		/// be added to Gallery Server, perhaps because it is an unsupported type or the file is corrupt.</param>
		/// <param name="parentAlbum">The album in which the media object exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns a properly typed Gallery Object instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when <paramref name="mediaObjectFile"/> has a file 
		/// extension that Gallery Server Pro is configured to reject.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when   
		/// <paramref name="mediaObjectFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <remarks>This method is marked internal to ensure it is not called from the web layer. It was noticed that
		/// calling this method from the web layer caused the file referenced in the mediaObjectFile parameter to remain
		/// locked beyond the conclusion of the page lifecycle, preventing manual deletion using Windows Explorer. Note 
		/// that restarting IIS (iisreset.exe) released the file lock, and presumably the next garbage collection would 
		/// have released it as well. The web page was modified to call the overload of this method that takes the filepath
		/// as a string parameter and then instantiates a <see cref="System.IO.FileInfo" /> object. I am not sure why, 
		/// but instantiating the <see cref="System.IO.FileInfo" /> object within this DLL in this way caused the file 
		/// lock to be released at the end of the page lifecycle.</remarks>
		internal static IGalleryObject CreateMediaObjectInstance(System.IO.FileInfo mediaObjectFile, IAlbum parentAlbum)
		{
			return CreateMediaObjectInstance(mediaObjectFile, parentAlbum, false, String.Empty, MimeTypeCategory.NotSet);
		}

		/// <summary>
		/// Create a properly typed Gallery Object instance (e.g. <see cref="Image" />, <see cref="Video" />, etc.). If 
		/// <paramref name="externalHtmlSource"/> is specified, then an <see cref="ExternalMediaObject" /> is created with the
		/// specified <paramref name="mimeTypeCategory"/>; otherwise a new instance is created based on <paramref name="mediaObjectFile"/>,
		/// where the exact type (e.g. <see cref="Image" />, <see cref="Video" />, etc.) is determined by the file's extension.
		/// </summary>
		/// <param name="mediaObjectFile">A <see cref="System.IO.FileInfo" /> object representing a supported media object type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the <paramref name="forceNew"/> parameter is specified and is
		/// set to true, then a new, unsaved media object is always returned, regardless of the existence of the file. For 
		/// new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store. A
		/// <see cref="UnsupportedMediaObjectTypeException" /> is thrown when the specified file cannot 
		/// be added to Gallery Server, perhaps because it is an unsupported type or the file is corrupt. Do not specify this parameter
		/// when using the <paramref name="externalHtmlSource"/> parameter.</param>
		/// <param name="parentAlbum">The album in which the media object exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store). This parameter is ignored when <paramref name="forceNew"/> is true.</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the mediaObjectFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object. When true, <paramref name="parentAlbum"/> is ignored and must be assigned by the
		/// calling method.</param>
		/// <param name="externalHtmlSource">The HTML that defines an externally stored media object, such as one hosted at 
		/// Silverlight.net or youtube.com. Using this parameter also requires specifying <paramref name="mimeTypeCategory"/>
		/// and passing null for <paramref name="mediaObjectFile"/>.</param>
		/// <param name="mimeTypeCategory">Specifies the category to which an externally stored media object belongs. 
		/// Must be set to a value other than MimeTypeCategory.NotSet when the <paramref name="externalHtmlSource"/> is specified.</param>
		/// <returns>Returns a properly typed Gallery Object instance corresponding to the specified parameters.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="mediaObjectFile"/> and <paramref name="externalHtmlSource"/>
		/// are either both specified, or neither.</exception>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when <paramref name="mediaObjectFile"/> has a file 
		/// extension that Gallery Server Pro is configured to reject.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when the  
		/// mediaObjectFile parameter refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <remarks>This method is marked internal to ensure it is not called from the web layer. It was noticed that
		/// calling this method from the web layer caused the file referenced in the mediaObjectFile parameter to remain
		/// locked beyond the conclusion of the page lifecycle, preventing manual deletion using Windows Explorer. Note 
		/// that restarting IIS (iisreset.exe) released the file lock, and presumably the next garbage collection would 
		/// have released it as well. The web page was modified to call the overload of this method that takes the filepath
		/// as a string parameter and then instantiates a FileInfo object. I am not sure why, but instantiating the FileInfo 
		/// object within this DLL in this way caused the file lock to be released at the end of the page lifecycle.</remarks>
		internal static IGalleryObject CreateMediaObjectInstance(FileInfo mediaObjectFile, IAlbum parentAlbum, bool forceNew, string externalHtmlSource, MimeTypeCategory mimeTypeCategory)
		{
			#region Validation

			// Either mediaObjectFile or externalHtmlSource must be specified, but not both.
			if ((mediaObjectFile == null) && (String.IsNullOrEmpty(externalHtmlSource)))
				throw new ArgumentException("The method GalleryServerPro.Business.Factory.CreateMediaObjectInstance was invoked with invalid parameters. The parameters mediaObjectFile and externalHtmlSource cannot both be null or empty. One of these - but not both - must be populated.");

			if ((mediaObjectFile != null) && (!String.IsNullOrEmpty(externalHtmlSource)))
				throw new ArgumentException("The method GalleryServerPro.Business.Factory.CreateMediaObjectInstance was invoked with invalid parameters. The parameters mediaObjectFile and externalHtmlSource cannot both be specified.");

			if ((!String.IsNullOrEmpty(externalHtmlSource)) && (mimeTypeCategory == MimeTypeCategory.NotSet))
				throw new ArgumentException("The method GalleryServerPro.Business.Factory.CreateMediaObjectInstance was invoked with invalid parameters. The parameters mimeTypeCategory must be set to a value other than MimeTypeCategory.NotSet when the externalHtmlSource parameter is specified.");

			#endregion

			if (String.IsNullOrEmpty(externalHtmlSource))
				return CreateLocalMediaObjectInstance(mediaObjectFile, parentAlbum, forceNew);
			else
				return CreateExternalMediaObjectInstance(externalHtmlSource, mimeTypeCategory, parentAlbum, forceNew);
		}

		/// <summary>
		/// Create a properly typed Gallery Object instance (e.g. <see cref="Image" />, <see cref="Video" />, etc.) from the specified parameters.
		/// </summary>
		/// <param name="mediaObjectFile">A <see cref="System.IO.FileInfo" /> object representing a supported media object type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object is always returned, regardless of the existence of the file. For 
		/// new instances, call <see cref="IGalleryObject.Save"/> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the media object exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the mediaObjectFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object.</param>
		/// <returns>Returns a properly typed Gallery Object instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when <paramref name="mediaObjectFile"/> has a file 
		/// extension that Gallery Server Pro is configured to reject.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when   
		/// <paramref name="mediaObjectFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		private static IGalleryObject CreateLocalMediaObjectInstance(FileInfo mediaObjectFile, IAlbum parentAlbum, bool forceNew)
		{
			if (mediaObjectFile == null)
				throw new ArgumentNullException("mediaObjectFile");

			IGalleryObject go;

			GalleryObjectType goType = HelperFunctions.DetermineMediaObjectType(mediaObjectFile.Name);

			if (goType == GalleryObjectType.Unknown)
			{
				bool allowUnspecifiedMimeTypes = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.AllowUnspecifiedMimeTypes;
				// If we have an unrecognized media object type (because no MIME type element exists in the configuration
				// file that matches the file extension), then treat the object as a generic media object, but only if
				// the "allowUnspecifiedMimeTypes" configuration setting allows adding unknown media object types.
				// If allowUnspecifiedMimeTypes = false, goType remains "Unknown", and we'll be throwing an 
				// UnsupportedMediaObjectTypeException at the end of this method.
				if (allowUnspecifiedMimeTypes)
				{
					goType = GalleryObjectType.Generic;
				}
			}

			switch (goType)
			{
				case GalleryObjectType.Image:
					{
						try
						{
							go = Factory.CreateImageInstance(mediaObjectFile, parentAlbum, forceNew); break;
						}
						catch (UnsupportedImageTypeException)
						{
							go = Factory.CreateGenericObjectInstance(mediaObjectFile, parentAlbum, forceNew); break;
						}
					}
				case GalleryObjectType.Video:
					{
						go = Factory.CreateVideoInstance(mediaObjectFile, parentAlbum, forceNew); break;
					}
				case GalleryObjectType.Audio:
					{
						go = Factory.CreateAudioInstance(mediaObjectFile, parentAlbum, forceNew); break;
					}
				case GalleryObjectType.Generic:
					{
						go = Factory.CreateGenericObjectInstance(mediaObjectFile, parentAlbum, forceNew); break;
					}
				default:
					{
						throw new UnsupportedMediaObjectTypeException(mediaObjectFile);
					}
			}

			return go;
		}

		/// <summary>
		/// Create a fully inflated, properly typed media object instance based on the 
		/// <see cref="IGalleryObject.Id">ID</see>. If <see cref="IGalleryObject.Id">ID</see> is an
		/// image, video, audio, etc, then the appropriate object is returned. An exception is thrown if the
		/// <see cref="IGalleryObject.Id">ID</see> refers to an <see cref="Album" /> (use the 
		/// <see cref="LoadGalleryObjectInstance" /> or <see cref="LoadAlbumInstance" /> method if
		/// the <see cref="IGalleryObject.Id">ID</see> refers to an album). An exception is also thrown if no matching record exists for this <see cref="IGalleryObject.Id">ID</see>.
		/// If the galleryObjectType parameter is not specified, or is All, None, or Unknown, then an additional 
		/// call to the data store is made to determine the object's type. When you know the type you want (<see cref="Image" />, <see cref="Video" />, 
		/// etc), use the overload that takes the galleryObjectType parameter, or call the specific Factory method that 
		/// loads the desired type, as those are more efficient. This method is guaranteed to never return null.</summary>
		/// <param name="id">An integer representing the <see cref="IGalleryObject.Id">ID</see> of the media object to retrieve
		/// from the data store.</param>
		/// <returns>Returns a fully inflated, properly typed media object instance based on the <see cref="IGalleryObject.Id">ID</see>.</returns>
		/// <exception cref="System.ArgumentException">Thrown when the id parameter refers to an album. This method 
		/// should be used only for media objects (image, video, audio, etc).</exception>
		/// <exception cref="InvalidMediaObjectException">
		/// Thrown when no record exists in the data store for the specified <see cref="IGalleryObject.Id">ID</see>.</exception>
		public static IGalleryObject LoadMediaObjectInstance(int id)
		{
#if DEBUG
			tt.Tools.StartingMethod(id);
#endif

			return RetrieveMediaObject(id);
		}

		/// <summary>
		/// Create a fully inflated, properly typed media object instance based on the <see cref="IGalleryObject.Id">ID</see>. If <see cref="IGalleryObject.Id">ID</see> is an
		/// image, video, audio, etc, then the appropriate object is returned. An exception is thrown if the
		/// <see cref="IGalleryObject.Id">ID</see> refers to an <see cref="Album" /> (use the <see cref="LoadGalleryObjectInstance" /> or <see cref="LoadAlbumInstance" /> method if
		/// the <see cref="IGalleryObject.Id">ID</see> refers to an album). An exception is also thrown if no matching record exists for this <see cref="IGalleryObject.Id">ID</see>.
		/// If the galleryObjectType parameter is not specified, or is All, None, or Unknown, then an additional 
		/// call to the data store is made to determine the object's type. When you know the type you want (<see cref="Image" />, <see cref="Video" />, 
		/// etc), use the overload that takes the galleryObjectType parameter, or call the specific Factory method that 
		/// loads the desired type, as those are more efficient. This method is guaranteed to never return null.</summary>
		/// <param name="id">An integer representing the <see cref="IGalleryObject.Id">ID</see> of the media object to retrieve
		/// from the data store.</param>
		/// <param name="galleryObjectType">The type of gallery object that the id parameter represents. If the type is 
		/// unknown, the Unknown enum value can be specified. Specify the actual type if possible (e.g. Video, Audio, Image, 
		/// etc.), as it is more efficient. An exception is thrown if the Album enum value is specified, since this method
		/// is designed only for media objects.</param>
		/// <returns>Returns a fully inflated, properly typed media object instance based on the <see cref="IGalleryObject.Id">ID</see>.</returns>
		/// <exception cref="InvalidMediaObjectException">
		/// Thrown when no record exists in the data store for the specified id, or when the id parameter refers to an album.</exception>
		public static IGalleryObject LoadMediaObjectInstance(int id, GalleryObjectType galleryObjectType)
		{
			return RetrieveMediaObject(id, galleryObjectType);
		}

		/// <summary>
		/// Create a fully inflated, properly typed media object instance based on the specified data record and
		/// belonging to the specified parent album.
		/// </summary>
		/// <param name="dr">A data record containing information about the media object.</param>
		/// <param name="parentAlbum">The album that contains the media obect to be returned.</param>
		/// <returns>Returns a fully inflated, properly typed media object instance based on the specified data 
		/// record and belonging to the specified parent album.</returns>
		public static IGalleryObject LoadMediaObjectInstance(IDataRecord dr, IAlbum parentAlbum)
		{
			// SQL:
			// SELECT
			//  mo.MediaObjectID, mo.FKAlbumID, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
			//  mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
			//  mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.ExternalHtmlSource, mo.ExternalType, mo.Seq, 
			//  mo.CreatedBy, mo.DateAdded, mo.LastModifiedBy, mo.DateLastModified, mo.IsPrivate
			// FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
			// WHERE mo.MediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID

			if (dr == null)
				throw new ArgumentNullException("dr");

			GalleryObjectType goType = HelperFunctions.DetermineMediaObjectType(dr["OriginalFilename"].ToString(), Convert.ToInt32(dr["OriginalWidth"]), Convert.ToInt32(dr["OriginalHeight"]), dr["ExternalHtmlSource"].ToString());
			IGalleryObject go = new NullObjects.NullGalleryObject();

			switch (goType)
			{
				case GalleryObjectType.Image:
					{
						#region Create Image
						go = new Image(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum,
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OptimizedFilename"].ToString().Trim(),
							Int32.Parse(dr["OptimizedWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OptimizedHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OptimizedSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);

						AddMediaObjectMetadata(go);
						break;

						#endregion
					}
				case GalleryObjectType.Video:
					{
						#region Create Video

						go = new Video(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
						break;

						#endregion
					}
				case GalleryObjectType.Audio:
					{
						#region Create Audio

						go = new Audio(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
						break;

						#endregion
					}
				case GalleryObjectType.Generic:
				case GalleryObjectType.Unknown:
					{
						#region Create Generic Media Object

						go = new GenericMediaObject(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
						break;

						#endregion
					}
				case GalleryObjectType.External:
					{
						#region Create External

						go = new ExternalMediaObject(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["ExternalHtmlSource"].ToString().Trim(),
							MimeTypeEnumHelper.ParseMimeTypeCategory(dr["ExternalType"].ToString().Trim()),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true);
						break;

						#endregion
					}
				default:
					{
						throw new UnsupportedMediaObjectTypeException(Path.Combine(parentAlbum.FullPhysicalPath, dr["OriginalFilename"].ToString()));
					}
			}
			return go;
		}

		/// <summary>
		/// Returns an object that knows how to persist media objects to the data store.
		/// </summary>
		/// <param name="galleryObject">A media object to which the save behavior applies. Must be a valid media
		/// object such as <see cref="Image" />, <see cref="Video" />, etc. Do not pass an <see cref="Album" />.</param>
		/// <returns>Returns an object that implements ISaveBehavior.</returns>
		public static ISaveBehavior GetMediaObjectSaveBehavior(IGalleryObject galleryObject)
		{
			System.Diagnostics.Debug.Assert((!(galleryObject is Album)), "It is invalid to pass an album as a parameter to the GetMediaObjectSaveBehavior() method.");

			return new MediaObjectSaveBehavior(galleryObject as GalleryObject);
		}

		/// <summary>
		/// Returns an object that knows how to delete media objects from the data store.
		/// </summary>
		/// <param name="galleryObject">A media object to which the delete behavior applies. Must be a valid media
		/// object such as Image, Video, etc. Do not pass an Album; use <see cref="GetAlbumDeleteBehavior" /> for configuring <see cref="Album" /> objects.</param>
		/// <returns>Returns an object that implements <see cref="IDeleteBehavior" />.</returns>
		public static IDeleteBehavior GetMediaObjectDeleteBehavior(IGalleryObject galleryObject)
		{
			System.Diagnostics.Debug.Assert((!(galleryObject is Album)), "It is invalid to pass an album as a parameter to the GetMediaObjectDeleteBehavior() method.");

			return new MediaObjectDeleteBehavior(galleryObject);
		}

		#endregion

		#region Image Methods

		/// <summary>
		/// Create a minimally populated <see cref="Image" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="imageFile">A <see cref="System.IO.FileInfo" /> object representing a supported image type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the image exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns an <see cref="Image" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when 
		/// <paramref name="imageFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="imageFile"/> has a file extension that Gallery Server Pro is configured to reject, or it is
		/// associated with a non-image MIME type.</exception>
		/// <exception cref="UnsupportedImageTypeException">Thrown when the 
		/// .NET Framework is unable to load an image file into the <see cref="System.Drawing.Bitmap"/> class. This is 
		/// probably because it is corrupted, not an image supported by the .NET Framework, or the server does not have 
		/// enough memory to process the image. The file cannot, therefore, be handled using the <see cref="Image"/> 
		/// class; use <see cref="GenericMediaObject"/> instead.</exception>
		public static IGalleryObject CreateImageInstance(System.IO.FileInfo imageFile, IAlbum parentAlbum)
		{
			return CreateImageInstance(imageFile, parentAlbum, false);
		}

		/// <summary>
		/// Create a minimally populated <see cref="Image" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="imageFile">A <see cref="System.IO.FileInfo" /> object representing a supported image type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object with no assigned parent album is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the image exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the imageFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object.</param>
		/// <returns>Returns an <see cref="Image" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when 
		/// <paramref name="imageFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="imageFile"/> has a file extension that Gallery Server Pro is configured to reject, or it is
		/// associated with a non-image MIME type.</exception>
		/// <exception cref="UnsupportedImageTypeException">Thrown when the 
		/// .NET Framework is unable to load an image file into the <see cref="System.Drawing.Bitmap"/> class. This is 
		/// probably because it is corrupted, not an image supported by the .NET Framework, or the server does not have 
		/// enough memory to process the image. The file cannot, therefore, be handled using the <see cref="Image"/> 
		/// class; use <see cref="GenericMediaObject"/> instead.</exception>
		public static IGalleryObject CreateImageInstance(System.IO.FileInfo imageFile, IAlbum parentAlbum, bool forceNew)
		{
#if DEBUG
			tt.Tools.StartingMethod(imageFile, parentAlbum, forceNew);
#endif

			// Validation check: Make sure the configuration settings allow for this particular type of file to be added.
			if (!HelperFunctions.IsFileAuthorizedForAddingToGallery(imageFile.Name))
				throw new UnsupportedMediaObjectTypeException(imageFile.FullName);

			// If the file belongs to an existing media object, return a reference to it.
			if (!forceNew)
			{
				foreach (IGalleryObject childMediaObject in parentAlbum.GetChildGalleryObjects(GalleryObjectType.Image))
				{
					if (childMediaObject.Original.FileNamePhysicalPath == imageFile.FullName)
						return childMediaObject;
				}
			}

			if (forceNew) parentAlbum = null;

			// Create a new image object, which will cause a new record to be inserted in the data store when Save() is called.
			return new Image(imageFile, parentAlbum);
		}

		/// <summary>
		/// Create a fully inflated image instance based on the <see cref="IGalleryObject.Id">ID</see> of the image parameter. Overwrite
		/// properties of the image parameter with the retrieved values from the data store. The returned image
		/// is the same object reference as the image parameter.
		/// </summary>
		/// <param name="image">The image whose properties should be overwritten with the values from the data store.</param>
		/// <returns>Returns an inflated image instance with all properties set to the values from the data store.
		/// </returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an image is not found in the data store that matches the <see cref="IGalleryObject.Id">ID</see> of the image parameter in the current gallery.</exception>
		public static IGalleryObject LoadImageInstance(IGalleryObject image)
		{
#if DEBUG
			tt.Tools.StartingMethod(image);
#endif

			if (image == null)
				throw new ArgumentNullException("image");

			IGalleryObject retrievedImage = RetrieveMediaObject(image.Id, GalleryObjectType.Image, (IAlbum)image.Parent);

			image.Title = retrievedImage.Title;
			image.CreatedByUserName = retrievedImage.CreatedByUserName;
			image.DateAdded = retrievedImage.DateAdded;
			image.LastModifiedByUserName = retrievedImage.LastModifiedByUserName;
			image.DateLastModified = retrievedImage.DateLastModified;
			image.IsPrivate = retrievedImage.IsPrivate;
			image.Hashkey = retrievedImage.Hashkey;
			image.Sequence = retrievedImage.Sequence;
			image.MetadataItems.Clear();
			image.MetadataItems.AddRange(retrievedImage.MetadataItems.Copy());

			string albumPhysicalPath = image.Parent.FullPhysicalPathOnDisk;

			#region Thumbnail

			image.Thumbnail.MediaObjectId = retrievedImage.Id;
			image.Thumbnail.FileName = retrievedImage.Thumbnail.FileName;
			image.Thumbnail.Height = retrievedImage.Thumbnail.Height;
			image.Thumbnail.Width = retrievedImage.Thumbnail.Width;

			// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPhysicalPath, AppSetting.Instance.ThumbnailPath);
			image.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, image.Thumbnail.FileName);

			#endregion

			#region Optimized

			image.Optimized.MediaObjectId = retrievedImage.Id;
			image.Optimized.FileName = retrievedImage.Optimized.FileName;
			image.Optimized.Height = retrievedImage.Optimized.Height;
			image.Optimized.Width = retrievedImage.Optimized.Width;

			// Calcululate the full file path to the optimized image. If the optimized filename is equal to the original filename, then no
			// optimized version exists, and we'll just point to the original. If the names are different, then there is a separate optimized
			// image file, and it is stored in either the album's physical path or an alternate location (if optimizedPath config setting is specified).
			string optimizedPath = albumPhysicalPath;

			if (retrievedImage.Optimized.FileName != retrievedImage.Original.FileName)
				optimizedPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPhysicalPath, AppSetting.Instance.OptimizedPath);

			image.Optimized.FileNamePhysicalPath = System.IO.Path.Combine(optimizedPath, image.Optimized.FileName);

			#endregion

			#region Original

			image.Original.MediaObjectId = retrievedImage.Id;
			image.Original.FileName = retrievedImage.Original.FileName;
			image.Original.Height = retrievedImage.Original.Height;
			image.Original.Width = retrievedImage.Original.Width;
			image.Original.FileNamePhysicalPath = System.IO.Path.Combine(albumPhysicalPath, image.Original.FileName);
			image.Original.ExternalHtmlSource = retrievedImage.Original.ExternalHtmlSource;
			image.Original.ExternalType = retrievedImage.Original.ExternalType;

			#endregion

			image.IsInflated = true;
			image.HasChanges = false;

			return image;
		}

		/// <summary>
		/// Create a fully inflated image instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing image media object.</param>
		/// <returns>Returns an inflated image instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an image is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadImageInstance(int mediaObjectId)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId);
#endif

			return LoadImageInstance(mediaObjectId, null);
		}

		/// <summary>
		/// Create a fully inflated image instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing image media object.</param>
		/// <param name="parentAlbum">The album containing the media object specified by mediaObjectId. Specify
		/// null if a reference to the album is not available, and it will be created based on the parent album
		/// specified in the data store.</param>
		/// <returns>Returns an inflated image instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an image is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadImageInstance(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			return RetrieveMediaObject(mediaObjectId, GalleryObjectType.Image, parentAlbum);
		}

		#endregion

		#region Video Methods

		/// <summary>
		/// Create a minimally populated <see cref="Video" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="videoFile">A <see cref="System.IO.FileInfo" /> object representing a supported video type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the video exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns a <see cref="Video" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="videoFile"/> has a file extension that Gallery Server Pro is configured to reject, or it is
		/// associated with a non-video MIME type.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when   
		/// <paramref name="videoFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		public static IGalleryObject CreateVideoInstance(System.IO.FileInfo videoFile, IAlbum parentAlbum)
		{
			return CreateVideoInstance(videoFile, parentAlbum, false);
		}

		/// <summary>
		/// Create a minimally populated <see cref="Video" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="videoFile">A <see cref="System.IO.FileInfo" /> object representing a supported video type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object with no assigned parent album is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the video exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the imageFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object.</param>
		/// <returns>Returns a <see cref="Video" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="videoFile"/> has a file extension that Gallery Server Pro is configured to reject, or it is
		/// associated with a non-video MIME type.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when   
		/// <paramref name="videoFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		public static IGalleryObject CreateVideoInstance(System.IO.FileInfo videoFile, IAlbum parentAlbum, bool forceNew)
		{
#if DEBUG
			tt.Tools.StartingMethod(videoFile, parentAlbum, forceNew);
#endif

			// Validation check: Make sure the configuration settings allow for this particular type of file to be added.
			if (!HelperFunctions.IsFileAuthorizedForAddingToGallery(videoFile.Name))
				throw new UnsupportedMediaObjectTypeException(videoFile.FullName);

			// If the file belongs to an existing media object, return a reference to it.
			if (!forceNew)
			{
				foreach (IGalleryObject childMediaObject in parentAlbum.GetChildGalleryObjects(GalleryObjectType.Video))
				{
					if (childMediaObject.Original.FileNamePhysicalPath == videoFile.FullName)
						return childMediaObject;
				}
			}

			if (forceNew) parentAlbum = null;

			// Create a new video object, which will cause a new record to be inserted in the data store when Save() is called.
			return new Video(videoFile, parentAlbum);
		}

		/// <summary>
		/// Create a fully inflated <see cref="Video" /> instance based on the <see cref="IGalleryObject.Id">ID</see> of the video parameter. Overwrite
		/// properties of the video parameter with the retrieved values from the data store. The returned video
		/// is the same object reference as the video parameter.
		/// </summary>
		/// <param name="video">The video whose properties should be overwritten with the values from the data store.</param>
		/// <returns>Returns an inflated <see cref="Video" /> instance with all properties set to the values from the data store.
		/// </returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when a video is not found in the data store that matches the 
		/// <see cref="IGalleryObject.Id">ID</see> of the video parameter in the current gallery.</exception>
		public static IGalleryObject LoadVideoInstance(IGalleryObject video)
		{
#if DEBUG
			tt.Tools.StartingMethod(video);
#endif

			if (video == null)
				throw new ArgumentNullException("video");

			IGalleryObject retrievedVideo = RetrieveMediaObject(video.Id, GalleryObjectType.Video, (IAlbum)video.Parent);

			video.Title = retrievedVideo.Title;
			video.CreatedByUserName = retrievedVideo.CreatedByUserName;
			video.DateAdded = retrievedVideo.DateAdded;
			video.LastModifiedByUserName = retrievedVideo.LastModifiedByUserName;
			video.DateLastModified = retrievedVideo.DateLastModified;
			video.IsPrivate = retrievedVideo.IsPrivate;
			video.Hashkey = retrievedVideo.Hashkey;
			video.Sequence = retrievedVideo.Sequence;

			string albumPhysicalPath = video.Parent.FullPhysicalPathOnDisk;

			#region Thumbnail

			video.Thumbnail.MediaObjectId = retrievedVideo.Id;
			video.Thumbnail.FileName = retrievedVideo.Thumbnail.FileName;
			video.Thumbnail.Height = retrievedVideo.Thumbnail.Height;
			video.Thumbnail.Width = retrievedVideo.Thumbnail.Width;

			// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPhysicalPath, AppSetting.Instance.ThumbnailPath);
			video.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, video.Thumbnail.FileName);

			#endregion

			#region Optimized

			// Video objects do not have an optimized object.

			#endregion

			#region Original

			video.Original.MediaObjectId = retrievedVideo.Id;
			video.Original.FileName = retrievedVideo.Original.FileName;
			video.Original.Height = retrievedVideo.Original.Height;
			video.Original.Width = retrievedVideo.Original.Width;
			video.Original.FileNamePhysicalPath = System.IO.Path.Combine(albumPhysicalPath, video.Original.FileName);
			video.Original.ExternalHtmlSource = retrievedVideo.Original.ExternalHtmlSource;
			video.Original.ExternalType = retrievedVideo.Original.ExternalType;

			#endregion

			video.IsInflated = true;
			video.HasChanges = false;

			return video;
		}

		/// <summary>
		/// Create a fully inflated <see cref="Video" /> instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing video object.</param>
		/// <returns>Returns an inflated <see cref="Video" /> instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// a video is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadVideoInstance(int mediaObjectId)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId);
#endif

			return LoadVideoInstance(mediaObjectId, null);
		}

		/// <summary>
		/// Create a fully inflated <see cref="Video" /> instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing video object.</param>
		/// <param name="parentAlbum">The album containing the media object specified by mediaObjectId. Specify
		/// null if a reference to the album is not available, and it will be created based on the parent album
		/// specified in the data store.</param>
		/// <returns>Returns an inflated <see cref="Video" /> instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// a video is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadVideoInstance(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			return RetrieveMediaObject(mediaObjectId, GalleryObjectType.Video, parentAlbum);
		}

		#endregion

		#region Audio Methods

		/// <summary>
		/// Create a minimally populated <see cref="Audio" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="audioFile">A <see cref="System.IO.FileInfo" /> object representing a supported audio type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the audio exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns an <see cref="Audio" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when 
		/// <paramref name="audioFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="audioFile"/> has a file extension that Gallery Server Pro is configured to reject, or it is
		/// associated with a non-audio MIME type.</exception>
		public static IGalleryObject CreateAudioInstance(System.IO.FileInfo audioFile, IAlbum parentAlbum)
		{
			return CreateAudioInstance(audioFile, parentAlbum, false);
		}

		/// <summary>
		/// Create a minimally populated <see cref="Audio" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="audioFile">A <see cref="System.IO.FileInfo" /> object representing a supported audio type. The file must already
		/// exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object with no assigned parent album is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the audio exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the imageFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object.</param>
		/// <returns>Returns an <see cref="Audio" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when 
		/// <paramref name="audioFile"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="audioFile"/> has a file extension that Gallery Server Pro is configured to reject, or it is
		/// associated with a non-audio MIME type.</exception>
		public static IGalleryObject CreateAudioInstance(System.IO.FileInfo audioFile, IAlbum parentAlbum, bool forceNew)
		{
#if DEBUG
			tt.Tools.StartingMethod(audioFile, parentAlbum, forceNew);
#endif

			// Validation check: Make sure the configuration settings allow for this particular type of file to be added.
			if (!HelperFunctions.IsFileAuthorizedForAddingToGallery(audioFile.Name))
				throw new UnsupportedMediaObjectTypeException(audioFile.FullName);

			// If the file belongs to an existing media object, return a reference to it.
			if (!forceNew)
			{
				foreach (IGalleryObject childMediaObject in parentAlbum.GetChildGalleryObjects(GalleryObjectType.Audio))
				{
					if (childMediaObject.Original.FileNamePhysicalPath == audioFile.FullName)
						return childMediaObject;
				}
			}

			if (forceNew) parentAlbum = null;

			// Create a new audio object, which will cause a new record to be inserted in the data store when Save() is called.
			return new Audio(audioFile, parentAlbum);
		}

		/// <summary>
		/// Create a fully inflated <see cref="Audio" /> instance based on the <see cref="IGalleryObject.Id">ID</see> of the audio parameter. Overwrite
		/// properties of the audio parameter with the retrieved values from the data store. The returned audio
		/// is the same object reference as the audio parameter.
		/// </summary>
		/// <param name="audio">The <see cref="Audio" /> instance whose properties should be overwritten with the values from the data store.</param>
		/// <returns>Returns an inflated <see cref="Audio" /> instance with all properties set to the values from the data store.
		/// </returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when a audio file is not found in the data store that matches the 
		/// <see cref="IGalleryObject.Id">ID</see> of the audio parameter in the current gallery.</exception>
		public static IGalleryObject LoadAudioInstance(IGalleryObject audio)
		{
#if DEBUG
			tt.Tools.StartingMethod(audio);
#endif

			if (audio == null)
				throw new ArgumentNullException("audio");

			IGalleryObject retrievedAudio = RetrieveMediaObject(audio.Id, GalleryObjectType.Audio, (IAlbum)audio.Parent);

			audio.Title = retrievedAudio.Title;
			audio.CreatedByUserName = retrievedAudio.CreatedByUserName;
			audio.DateAdded = retrievedAudio.DateAdded;
			audio.LastModifiedByUserName = retrievedAudio.LastModifiedByUserName;
			audio.DateLastModified = retrievedAudio.DateLastModified;
			audio.IsPrivate = retrievedAudio.IsPrivate;
			audio.Hashkey = retrievedAudio.Hashkey;
			audio.Sequence = retrievedAudio.Sequence;

			string albumPhysicalPath = audio.Parent.FullPhysicalPathOnDisk;

			#region Thumbnail

			audio.Thumbnail.MediaObjectId = retrievedAudio.Id;
			audio.Thumbnail.FileName = retrievedAudio.Thumbnail.FileName;
			audio.Thumbnail.Height = retrievedAudio.Thumbnail.Height;
			audio.Thumbnail.Width = retrievedAudio.Thumbnail.Width;

			// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPhysicalPath, AppSetting.Instance.ThumbnailPath);
			audio.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, audio.Thumbnail.FileName);

			#endregion

			#region Optimized

			// Audio objects do not have an optimized object.

			#endregion

			#region Original

			audio.Original.MediaObjectId = retrievedAudio.Id;
			audio.Original.FileName = retrievedAudio.Original.FileName;
			audio.Original.Height = retrievedAudio.Original.Height;
			audio.Original.Width = retrievedAudio.Original.Width;
			audio.Original.FileNamePhysicalPath = System.IO.Path.Combine(albumPhysicalPath, audio.Original.FileName);
			audio.Original.ExternalHtmlSource = retrievedAudio.Original.ExternalHtmlSource;
			audio.Original.ExternalType = retrievedAudio.Original.ExternalType;

			#endregion

			audio.IsInflated = true;
			audio.HasChanges = false;

			return audio;
		}

		/// <summary>
		/// Create a fully inflated <see cref="Audio" /> instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing audio object.</param>
		/// <returns>Returns an inflated <see cref="Audio" /> instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an audio file is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadAudioInstance(int mediaObjectId)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId);
#endif

			return LoadAudioInstance(mediaObjectId, null);
		}

		/// <summary>
		/// Create a fully inflated <see cref="Audio" /> instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing audio object.</param>
		/// <param name="parentAlbum">The album containing the media object specified by mediaObjectId. Specify
		/// null if a reference to the album is not available, and it will be created based on the parent album
		/// specified in the data store.</param>
		/// <returns>Returns an inflated <see cref="Audio" /> instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when an audio file is not found in the data store that matches the 
		/// mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadAudioInstance(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			return RetrieveMediaObject(mediaObjectId, GalleryObjectType.Audio, parentAlbum);
		}

		#endregion

		#region Generic Media Object Methods

		/// <summary>
		/// Create a minimally populated <see cref="GenericMediaObject" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="file">A <see cref="System.IO.FileInfo" /> object representing a file to be managed by Gallery Server Pro. The file must 
		/// already exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the file exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns a <see cref="GenericMediaObject" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="file"/> has a file extension that Gallery Server Pro is configured to reject.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when the file parameter refers to a file that is not in the same 
		/// directory as the parent album's directory.</exception>
		public static IGalleryObject CreateGenericObjectInstance(System.IO.FileInfo file, IAlbum parentAlbum)
		{
			return CreateGenericObjectInstance(file, parentAlbum, false);
		}

		/// <summary>
		/// Create a minimally populated <see cref="GenericMediaObject" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="file">A <see cref="System.IO.FileInfo" /> object representing a file to be managed by Gallery Server Pro. The file must 
		/// already exist in the album's directory. If the file has a matching record in the data store, a reference to the existing 
		/// object is returned; otherwise, a new instance is returned. However, if the forceNew parameter is specified and is
		/// set to true, then a new, unsaved media object with no assigned parent album is always returned, regardless of the existence of the file.
		/// Otherwise, a new instance is returned. For new instances, call <see cref="IGalleryObject.Save" /> to persist the object to the data store.</param>
		/// <param name="parentAlbum">The album in which the file exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the imageFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object.</param>
		/// <returns>Returns a <see cref="GenericMediaObject" /> instance corresponding to the specified parameters.</returns>
		/// <exception cref="UnsupportedMediaObjectTypeException">Thrown when
		/// <paramref name="file"/> has a file extension that Gallery Server Pro is configured to reject.</exception>
		/// <exception cref="InvalidMediaObjectException">Thrown when   
		/// <paramref name="file"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		public static IGalleryObject CreateGenericObjectInstance(System.IO.FileInfo file, IAlbum parentAlbum, bool forceNew)
		{
#if DEBUG
			tt.Tools.StartingMethod(file, parentAlbum, forceNew);
#endif

			// Validation check: Make sure the configuration settings allow for this particular type of file to be added.
			if (!HelperFunctions.IsFileAuthorizedForAddingToGallery(file.Name))
				throw new UnsupportedMediaObjectTypeException(file.FullName);

			// If the file belongs to an existing media object, return a reference to it.
			if (!forceNew)
			{
				foreach (IGalleryObject childMediaObject in parentAlbum.GetChildGalleryObjects(GalleryObjectType.Generic))
				{
					if (childMediaObject.Original.FileNamePhysicalPath == file.FullName)
						return childMediaObject;
				}
			}

			if (forceNew) parentAlbum = null;

			// Create a new generic media object, which will cause a new record to be inserted in the data store when Save() is called.
			return new GenericMediaObject(file, parentAlbum);
		}

		/// <summary>
		/// Create a fully inflated <see cref="GenericMediaObject" /> instance based on the <see cref="IGalleryObject.Id">ID</see> of the 
		/// <paramref name="genericMediaObject"/> parameter. 
		/// Overwrite properties of the <paramref name="genericMediaObject"/> parameter with the retrieved values from the data store. 
		/// The returned instance is the same object reference as the <paramref name="genericMediaObject"/> parameter.
		/// </summary>
		/// <param name="genericMediaObject">The object whose properties should be overwritten with the values from 
		/// the data store.</param>
		/// <returns>Returns an inflated <see cref="GenericMediaObject" /> instance with all properties set to the values from the 
		/// data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when a record is not found in the data store that matches the 
		/// <see cref="IGalleryObject.Id">ID</see> of the <paramref name="genericMediaObject"/> parameter in the current gallery.</exception>
		public static IGalleryObject LoadGenericMediaObjectInstance(IGalleryObject genericMediaObject)
		{
#if DEBUG
			tt.Tools.StartingMethod(genericMediaObject);
#endif

			if (genericMediaObject == null)
				throw new ArgumentNullException("genericMediaObject");

			IGalleryObject retrievedGenericMediaObject = RetrieveMediaObject(genericMediaObject.Id, GalleryObjectType.Generic, (IAlbum)genericMediaObject.Parent);

			genericMediaObject.Title = retrievedGenericMediaObject.Title;
			genericMediaObject.CreatedByUserName = retrievedGenericMediaObject.CreatedByUserName;
			genericMediaObject.DateAdded = retrievedGenericMediaObject.DateAdded;
			genericMediaObject.LastModifiedByUserName = retrievedGenericMediaObject.LastModifiedByUserName;
			genericMediaObject.DateLastModified = retrievedGenericMediaObject.DateLastModified;
			genericMediaObject.IsPrivate = retrievedGenericMediaObject.IsPrivate;
			genericMediaObject.Hashkey = retrievedGenericMediaObject.Hashkey;
			genericMediaObject.Sequence = retrievedGenericMediaObject.Sequence;

			string albumPhysicalPath = genericMediaObject.Parent.FullPhysicalPathOnDisk;

			#region Thumbnail

			genericMediaObject.Thumbnail.MediaObjectId = retrievedGenericMediaObject.Id;
			genericMediaObject.Thumbnail.FileName = retrievedGenericMediaObject.Thumbnail.FileName;
			genericMediaObject.Thumbnail.Height = retrievedGenericMediaObject.Thumbnail.Height;
			genericMediaObject.Thumbnail.Width = retrievedGenericMediaObject.Thumbnail.Width;

			// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPhysicalPath, AppSetting.Instance.ThumbnailPath);
			genericMediaObject.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, genericMediaObject.Thumbnail.FileName);

			#endregion

			#region Optimized

			// No optimized object for a generic media object.

			#endregion

			#region Original

			genericMediaObject.Original.MediaObjectId = retrievedGenericMediaObject.Id;
			genericMediaObject.Original.FileName = retrievedGenericMediaObject.Original.FileName;
			genericMediaObject.Original.Height = retrievedGenericMediaObject.Original.Height;
			genericMediaObject.Original.Width = retrievedGenericMediaObject.Original.Width;
			genericMediaObject.Original.FileNamePhysicalPath = System.IO.Path.Combine(albumPhysicalPath, genericMediaObject.Original.FileName);
			genericMediaObject.Original.ExternalHtmlSource = retrievedGenericMediaObject.Original.ExternalHtmlSource;
			genericMediaObject.Original.ExternalType = retrievedGenericMediaObject.Original.ExternalType;

			#endregion

			genericMediaObject.IsInflated = true;
			genericMediaObject.HasChanges = false;

			return genericMediaObject;
		}

		/// <summary>
		/// Create a fully inflated <see cref="GenericMediaObject" /> instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing 
		/// <see cref="GenericMediaObject" /> object.</param>
		/// <returns>Returns an inflated <see cref="GenericMediaObject" /> instance with all properties set to the values from the 
		/// data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when a record is not found in the data store that matches the 
		/// mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadGenericMediaObjectInstance(int mediaObjectId)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId);
#endif

			return LoadGenericMediaObjectInstance(mediaObjectId, null);
		}

		/// <summary>
		/// Create a fully inflated <see cref="GenericMediaObject" /> instance based on the mediaObjectId.
		/// </summary>
		/// <param name="mediaObjectId">An <see cref="IGalleryObject.Id">ID</see> that uniquely represents an existing <see cref="GenericMediaObject" /> object.</param>
		/// <param name="parentAlbum">The album containing the media object specified by mediaObjectId. Specify
		/// null if a reference to the album is not available, and it will be created based on the parent album
		/// specified in the data store.</param>
		/// <returns>Returns an inflated <see cref="GenericMediaObject" /> instance with all properties set to the values from the 
		/// data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when a record is not found in the data store that matches the 
		/// mediaObjectId parameter and the current gallery.</exception>
		public static IGalleryObject LoadGenericMediaObjectInstance(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			return RetrieveMediaObject(mediaObjectId, GalleryObjectType.Generic, parentAlbum);
		}

		#endregion

		#region External Media Object Methods

		/// <overloads>
		/// Create a minimally populated <see cref="ExternalMediaObject" /> instance from the specified parameters. 
		/// </overloads>
		/// <summary>
		/// Create a minimally populated <see cref="ExternalMediaObject" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="externalHtmlSource">The HTML that defines an externally stored media object, such as one hosted at 
		/// YouTube or Silverlight.live.com.</param>
		/// <param name="mimeType">Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
		/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg").</param>
		/// <param name="parentAlbum">The album in which the file exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <returns>Returns a minimally populated <see cref="ExternalMediaObject" /> instance from the specified parameters.</returns>
		public static IGalleryObject CreateExternalMediaObjectInstance(string externalHtmlSource, MimeTypeCategory mimeType, IAlbum parentAlbum)
		{
			return CreateExternalMediaObjectInstance(externalHtmlSource, mimeType, parentAlbum, false);
		}

		/// <summary>
		/// Create a minimally populated <see cref="ExternalMediaObject" /> instance from the specified parameters. 
		/// </summary>
		/// <param name="externalHtmlSource">The HTML that defines an externally stored media object, such as one hosted at 
		/// YouTube or Silverlight.live.com.</param>
		/// <param name="mimeType">Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
		/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg").</param>
		/// <param name="parentAlbum">The album in which the file exists (for media objects that already exist
		/// in the data store), or should be added to (for new media objects which need to be inserted into the 
		/// data store).</param>
		/// <param name="forceNew">Indicates whether to initialize a new, unsaved media object even if the imageFile
		/// parameter refers to an existing file in the album's directory. Typically used when copying an existing media 
		/// object where a subsequent operation will copy the existing file to the destination album, thus resulting in a
		/// new, independent media object.</param>
		/// <returns>Returns a minimally populated <see cref="ExternalMediaObject" /> instance from the specified parameters.</returns>
		public static IGalleryObject CreateExternalMediaObjectInstance(string externalHtmlSource, MimeTypeCategory mimeType, IAlbum parentAlbum, bool forceNew)
		{
#if DEBUG
			Tools.StartingMethod(externalHtmlSource, mimeType, parentAlbum);
#endif

			// If the file belongs to an existing media object, return a reference to it.
			if (!forceNew)
			{
				foreach (IGalleryObject childMediaObject in parentAlbum.GetChildGalleryObjects(GalleryObjectType.External))
				{
					if (childMediaObject.Original.ExternalHtmlSource == externalHtmlSource)
						return childMediaObject;
				}
			}

			if (forceNew) parentAlbum = null;

			// Create a new generic media object, which will cause a new record to be inserted in the data store when Save() is called.
			return new ExternalMediaObject(externalHtmlSource, mimeType, parentAlbum);
		}

		/// <summary>
		/// Create a fully inflated <see cref="ExternalMediaObject" /> instance based on the <see cref="IGalleryObject.Id">ID</see> of the 
		/// <paramref name="externalMediaObject"/> parameter. 
		/// Overwrite properties of the <paramref name="externalMediaObject"/> parameter with the retrieved values from the data store. 
		/// The returned instance is the same object reference as the <paramref name="externalMediaObject"/> parameter.
		/// </summary>
		/// <param name="externalMediaObject">The object whose properties should be overwritten with the values from 
		/// the data store.</param>
		/// <returns>Returns an inflated <see cref="ExternalMediaObject" /> instance with all properties set to the values from the 
		/// data store.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when a record is not found in the data store that matches the 
		/// <see cref="IGalleryObject.Id">ID</see> of the <paramref name="externalMediaObject"/> parameter in the current gallery.</exception>
		public static IGalleryObject LoadExternalMediaObjectInstance(IGalleryObject externalMediaObject)
		{
#if DEBUG
			tt.Tools.StartingMethod(externalMediaObject);
#endif

			if (externalMediaObject == null)
				throw new ArgumentNullException("externalMediaObject");

			IGalleryObject retrievedGenericMediaObject = RetrieveMediaObject(externalMediaObject.Id, GalleryObjectType.External, (IAlbum)externalMediaObject.Parent);

			externalMediaObject.Title = retrievedGenericMediaObject.Title;
			externalMediaObject.CreatedByUserName = retrievedGenericMediaObject.CreatedByUserName;
			externalMediaObject.DateAdded = retrievedGenericMediaObject.DateAdded;
			externalMediaObject.LastModifiedByUserName = retrievedGenericMediaObject.LastModifiedByUserName;
			externalMediaObject.DateLastModified = retrievedGenericMediaObject.DateLastModified;
			externalMediaObject.IsPrivate = retrievedGenericMediaObject.IsPrivate;
			externalMediaObject.Hashkey = retrievedGenericMediaObject.Hashkey;
			externalMediaObject.Sequence = retrievedGenericMediaObject.Sequence;

			string albumPhysicalPath = externalMediaObject.Parent.FullPhysicalPathOnDisk;

			#region Thumbnail

			externalMediaObject.Thumbnail.FileName = retrievedGenericMediaObject.Thumbnail.FileName;
			externalMediaObject.Thumbnail.Height = retrievedGenericMediaObject.Thumbnail.Height;
			externalMediaObject.Thumbnail.Width = retrievedGenericMediaObject.Thumbnail.Width;

			// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
			string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(albumPhysicalPath, AppSetting.Instance.ThumbnailPath);
			externalMediaObject.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, externalMediaObject.Thumbnail.FileName);

			#endregion

			#region Optimized

			// No optimized image for a generic media object.

			#endregion

			#region Original

			externalMediaObject.Original.FileName = retrievedGenericMediaObject.Original.FileName;
			externalMediaObject.Original.Height = retrievedGenericMediaObject.Original.Height;
			externalMediaObject.Original.Width = retrievedGenericMediaObject.Original.Width;
			externalMediaObject.Original.FileNamePhysicalPath = System.IO.Path.Combine(albumPhysicalPath, externalMediaObject.Original.FileName);
			externalMediaObject.Original.ExternalHtmlSource = retrievedGenericMediaObject.Original.ExternalHtmlSource;
			externalMediaObject.Original.ExternalType = retrievedGenericMediaObject.Original.ExternalType;

			#endregion

			externalMediaObject.IsInflated = true;
			externalMediaObject.HasChanges = false;

			return externalMediaObject;
		}

		#endregion

		#endregion

		#region Album Methods

		/// <overloads>Create a minimally populated <see cref="Album" /> instance.</overloads>
		/// <summary>
		/// Create a new <see cref="Album" /> instance with an unassigned <see cref="IGalleryObject.Id">ID</see> and properties set to default values. 
		/// A valid <see cref="IGalleryObject.Id">ID</see> will be generated when the object is persisted to the data store during 
		/// the <see cref="IGalleryObject.Save" /> method. Use this overload when creating a new album and it has not yet been persisted
		/// to the data store.
		/// </summary>
		/// <returns>Returns an <see cref="Album" /> instance corresponding to the specified parameters.</returns>
		public static IAlbum CreateAlbumInstance()
		{
#if DEBUG
			tt.Tools.StartingMethod();
#endif

			return new Album();
		}

		/// <summary>
		/// Create a minimally populated <see cref="Album" /> instance corresponding to the specified albumId. Use this overload when the album 
		/// already exists in the data store but you do not necessarily need to retrieve its properties. A lazy load 
		/// is performed the first time a property is accessed.
		/// </summary>
		/// <param name="albumId">The <see cref="IGalleryObject.Id">ID</see> that uniquely identifies an existing album.</param>
		/// <returns>Returns an instance that implements <see cref="IAlbum" /> corresponding to the specified parameters.</returns>
		public static IAlbum CreateAlbumInstance(int albumId)
		{
#if DEBUG
			tt.Tools.StartingMethod(albumId);
#endif

			return new Album(albumId);
		}

		/// <summary>
		/// Load the top-level album from the data store. If this album contains child objects, they are added 
		/// but not inflated. Child objects are automatically inflated when any of the <see cref="IGalleryObject.GetChildGalleryObjects" /> 
		/// overloaded methods are called.
		/// </summary>
		/// <returns>Returns an instance that implements <see cref="IAlbum" /> with all properties set to the values from the data store.</returns>
		public static IAlbum LoadRootAlbumInstance()
		{
			tt.Tools.StartingMethod();
			IAlbum album = null;

			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().Album_GetDataReaderRootAlbum(ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId))
				{
					album = GetAlbumFromDataReader(dr);
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			if (album == null)
				throw new InvalidAlbumException(string.Format(CultureInfo.CurrentCulture, Resources.Factory_LoadRootAlbumInstance_Ex_Msg, GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId));

			// Since we've just loaded this object from the data store, set the corresponding property.
			album.FullPhysicalPathOnDisk = album.FullPhysicalPath;

			// Add child albums and objects, if they exist.
			AddChildObjects(album);

			return album;
		}

		/// <summary>
		/// Return all top-level albums where the <paramref name="roles"/> provide the requested <paramref name="permissions"/>.
		/// If more than one album is found, they are wrapped in a virtual container album where the IsVirtualAlbum property is
		/// set to true. Returns null if no matching albums are found.
		/// </summary>
		/// <param name="permissions">The permissions that must be provided by the roles.</param>
		/// <param name="roles">The roles belonging to a user.</param>
		/// <returns>Returns an <see cref="IAlbum"/> that is or contains the top-lvel album(s) that the <paramref name="roles"/> 
		/// provide the requested <paramref name="permissions"/>. Returns null if no matching albums are found.</returns>
		public static IAlbum LoadRootAlbum(SecurityActions permissions, IGalleryServerRoleCollection roles)
		{
			#region Step 1: Compile a list of album IDs having the requested permissions

			List<int> allAlbumIds = new List<int>();
			foreach (IGalleryServerRole role in roles)
			{
				foreach (SecurityActions permission in SecurityActionEnumHelper.ParseSecurityAction(permissions))
				{
					switch (permission)
					{
						case SecurityActions.ViewAlbumOrMediaObject:
							if (role.AllowViewAlbumOrMediaObject) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.ViewOriginalImage:
							if (role.AllowViewOriginalImage) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.AddChildAlbum:
							if (role.AllowAddChildAlbum) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.AddMediaObject:
							if (role.AllowAddMediaObject) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.AdministerSite:
							if (role.AllowAdministerSite) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.DeleteAlbum:
							// It is OK to delete the album if the AllowDeleteChildAlbum permission is true and one of the following is true:
							// 1. The album is the root album and its ID is in the list of targeted albums (Note that we never actually delete the root album.
							//    Instead, we delete all objects within the album. But the idea of deleting the top level album to clear out all objects in the
							//    gallery is useful to the user.)
							// 2. The album is not the root album and its parent album's ID is in the list of targeted albums.
							if (role.AllowDeleteChildAlbum)
							{
								foreach (int albumId in role.RootAlbumIds)
								{
									IAlbum album = LoadAlbumInstance(albumId, false);
									if (album.IsRootAlbum)
									{
										if (!role.AllAlbumIds.Contains(album.Id))
											allAlbumIds.Add(albumId);
										break;
									}
									else if (!role.AllAlbumIds.Contains(album.Parent.Id))
										allAlbumIds.Add(albumId);
									break;
								}
							}
							break;
						case SecurityActions.DeleteChildAlbum:
							if (role.AllowDeleteChildAlbum) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.DeleteMediaObject:
							if (role.AllowDeleteMediaObject) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.EditAlbum:
							if (role.AllowEditAlbum) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.EditMediaObject:
							if (role.AllowEditMediaObject) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.HideWatermark:
							if (role.HideWatermark) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
						case SecurityActions.Synchronize:
							if (role.AllowSynchronize) AddRoleAlbumsToListIfNotPresent(role, allAlbumIds);
							break;
					}
				}
			}

			#endregion

			#region Step 2: Convert previous list to contain ONLY top-level albums
			
			// Step 2: Loop through our list of albums. If any album has an ancestor that is also in the list, then remove it. 
			// We only want a list of top level albums.
			List<int> rootAlbumIds = new List<int>(allAlbumIds);

			List<int> albumIdsToRemove = new List<int>(rootAlbumIds.Count);
			foreach (int viewableAlbumId in allAlbumIds)
			{
				IGalleryObject album = LoadAlbumInstance(viewableAlbumId, false);
				while (true)
				{
					album = album.Parent as IAlbum;
					if (album == null)
						break;

					if (allAlbumIds.Contains(album.Id))
					{
						albumIdsToRemove.Add(viewableAlbumId);
						break;
					}
				}
			}

			foreach (int albumId in albumIdsToRemove)
			{
				rootAlbumIds.Remove(albumId);
			}

			#endregion

			#region Step 3: Package results into an album container

			// Step 3: If there is only one viewable root album, then just create an instance of that album.
			// Otherwise, create a virtual root album to contain the multiple viewable albums.
			IAlbum rootAlbum = null;
			if (rootAlbumIds.Count == 1)
			{
				rootAlbum = LoadAlbumInstance(rootAlbumIds[0], true);
			}
			else
			{
				// Create virtual album to serve as a container for the child albums the user has permission to view.
				rootAlbum = CreateAlbumInstance();
				rootAlbum.IsVirtualAlbum = true;
				rootAlbum.Title = Resources.Virtual_Album_Title;
				foreach (int albumId in rootAlbumIds)
				{
					rootAlbum.Add(LoadAlbumInstance(albumId, false));
				}
			}

			#endregion

			return rootAlbum;
		}

		private static void AddRoleAlbumsToListIfNotPresent(IGalleryServerRole role, ICollection<int> albumIds)
		{
			foreach (int albumId in role.RootAlbumIds)
			{
				if (!albumIds.Contains(albumId))
					albumIds.Add(albumId);
			}
		}

		/// <overloads>
		/// Generate an inflated <see cref="IAlbum" /> instance with optionally inflated child media objects.
		/// </overloads>
		/// <summary>
		/// Generate an inflated <see cref="IAlbum" /> instance with optionally inflated child media objects. The album's <see cref="IAlbum.ThumbnailMediaObjectId" />
		/// property is set to its value from the data store, but the <see cref="IGalleryObject.Thumbnail" /> property is only inflated when accessed.
		/// </summary>
		/// <param name="album">The album whose properties should be overwritten with the values from the data store.</param>
		/// <param name="inflateChildMediaObjects">When true, the child media objects of the album are added and inflated.
		/// Child albums are added but not inflated. When false, they are not added or inflated.</param>
		/// <exception cref="InvalidAlbumException">Thrown when an album is not found in the data store that matches the 
		/// <see cref="IGalleryObject.Id">ID</see> of the album parameter.</exception>
		public static void LoadAlbumInstance(IAlbum album, bool inflateChildMediaObjects)
		{
#if DEBUG
			tt.Tools.StartingMethod(album, inflateChildMediaObjects);
#endif

			if (album == null)
				throw new ArgumentNullException("album");

			if (album.IsInflated && !inflateChildMediaObjects) throw new InvalidOperationException(Resources.Factory_LoadAlbumInstance_Ex_Msg);

			#region Inflate the album, but only if it's not already inflated.

			if (!(album.IsInflated))
			{
				IDataReader dr = null;
				try
				{
					using (dr = Factory.GetDataProvider().Album_GetDataReaderAlbumById(album.Id))
					{
						InflateAlbumFromDataReader(album, dr);

						album.IsInflated = true;
					}
				}
				finally
				{
					if (dr != null) dr.Close();
				}

				System.Diagnostics.Debug.Assert(album.ThumbnailMediaObjectId > int.MinValue, "The album's ThumbnailMediaObjectId should have been assigned in this method.");

				// Since we've just loaded this object from the data store, set the corresponding property.
				album.FullPhysicalPathOnDisk = album.FullPhysicalPath;

				album.HasChanges = false;
			}

			#endregion

			#region Add child objects (CreateInstance)

			// Add child albums and objects, if they exist.
			if (inflateChildMediaObjects)
			{
				AddChildObjects(album);
			}

			#endregion

			if (!album.IsInflated)
				throw new InvalidAlbumException(album.Id);
		}

		/// <summary>
		/// Generate an inflated <see cref="IAlbum" /> instance with optionally inflated child media objects. The album's <see cref="IAlbum.ThumbnailMediaObjectId" />
		/// property is set to its value from the data store, but the <see cref="IGalleryObject.Thumbnail" /> property is only inflated when accessed.
		/// </summary>
		/// <param name="albumId">The <see cref="IGalleryObject.Id">ID</see> that uniquely identifies the album to retrieve.</param>
		/// <param name="inflateChildMediaObjects">When true, the child media objects of the album are added and inflated.
		/// Child albums are added but not inflated. When false, they are not added or inflated.</param>
		/// <returns>Returns an inflated album instance with all properties set to the values from the data store.</returns>
		/// <exception cref="InvalidAlbumException">Thrown when an album with the specified <paramref name="albumId"/> 
		/// is not found in the data store.</exception>
		public static IAlbum LoadAlbumInstance(int albumId, bool inflateChildMediaObjects)
		{
#if DEBUG
			tt.Tools.StartingMethod(albumId, inflateChildMediaObjects);
#endif

			IAlbum album = RetrieveAlbum(albumId);

			// Add child albums and objects, if they exist, and if needed.
			if ((inflateChildMediaObjects) && (!album.AreChildrenInflated))
			{
				AddChildObjects(album);
			}

			return album;
		}

		/// <summary>
		/// Returns an instance of an object that knows how to persist albums to the data store.
		/// </summary>
		/// <param name="albumObject">An <see cref="IAlbum" /> to which the save behavior applies.</param>
		/// <returns>Returns an object that implements <see cref="ISaveBehavior" />.</returns>
		public static ISaveBehavior GetAlbumSaveBehavior(IAlbum albumObject)
		{
			return new AlbumSaveBehavior(albumObject);
		}

		/// <summary>
		/// Returns an instance of an object that knows how to delete albums from the data store.
		/// </summary>
		/// <param name="albumObject">An <see cref="IAlbum" /> to which the delete behavior applies.</param>
		/// <returns>Returns an object that implements <see cref="IDeleteBehavior" />.</returns>
		public static IDeleteBehavior GetAlbumDeleteBehavior(IAlbum albumObject)
		{
			return new AlbumDeleteBehavior(albumObject);
		}

		#endregion

		#region Security Methods

		/// <summary>
		/// Create a Gallery Server Pro role corresponding to the specified parameters. Throws an exception if a role with the
		/// specified name already exists in the data store. The role is not persisted to the data store until the 
		/// <see cref="IGalleryServerRole.Save" /> method is called.
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
		/// <returns>Returns an <see cref="IGalleryServerRole" /> object corresponding to the specified parameters.</returns>
		/// <exception cref="InvalidGalleryServerRoleException">Thrown when a role with the specified role name already exists in the data store.</exception>
		public static IGalleryServerRole CreateGalleryServerRoleInstance(string roleName, bool allowViewAlbumOrMediaObject, bool allowViewOriginalImage, bool allowAddMediaObject, bool allowAddChildAlbum, bool allowEditMediaObject, bool allowEditAlbum, bool allowDeleteMediaObject, bool allowDeleteChildAlbum, bool allowSynchronize, bool allowAdministerSite, bool hideWatermark)
		{
#if DEBUG
			tt.Tools.StartingMethod(roleName, allowViewAlbumOrMediaObject, allowViewOriginalImage, allowAddMediaObject, allowAddChildAlbum, allowEditMediaObject, allowEditAlbum, allowDeleteMediaObject, allowDeleteChildAlbum, allowSynchronize, allowAdministerSite, hideWatermark);
#endif

			if (LoadGalleryServerRole(roleName) != null)
			{
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.InvalidGalleryServerRoleException(Resources.Factory_CreateGalleryServerRoleInstance_Ex_Msg);
			}

			return new GalleryServerRole(roleName, allowViewAlbumOrMediaObject, allowViewOriginalImage, allowAddMediaObject, allowAddChildAlbum, allowEditMediaObject, allowEditAlbum, allowDeleteMediaObject, allowDeleteChildAlbum, allowSynchronize, allowAdministerSite, hideWatermark);
		}

		/// <overloads>Retrieve a collection of Gallery Server roles.</overloads>
		/// <summary>
		/// Retrieve a collection of all Gallery Server roles for the current gallery. The roles may be returned from a cache.
		/// </summary>
		/// <returns>Returns an <see cref="IGalleryServerRoleCollection" /> object that contains all Gallery Server roles for the current gallery.</returns>
		/// <remarks>The collection of all Gallery Server roles for the current gallery are stored in a cache to improve
		/// performance. <note type="implementnotes">Note to developer: Any code that modifies the roles in the data store should purge the cache so 
		/// that they can be freshly retrieved from the data store during the next request. The cache is identified by the
		/// <see cref="CacheItem.GalleryServerRoles" /> enum.</note></remarks>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryServerRoleCollection LoadGalleryServerRoles()
		{
			Dictionary<string, IGalleryServerRoleCollection> rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)HelperFunctions.GetCache(CacheItem.GalleryServerRoles);

			IGalleryServerRoleCollection roles;

			if ((rolesCache != null) && (rolesCache.TryGetValue(GlobalConstants.GalleryServerRoleAllRolesCacheKey, out roles)))
			{
				return roles;
			}

			// No roles in the cache, so get from data store and add to cache.
			roles = GetGalleryServerRolesFromDataStore();

			roles.Sort();

			ValidateRoleIntegrity(roles);

			rolesCache = new Dictionary<string, IGalleryServerRoleCollection>();
			rolesCache.Add(GlobalConstants.GalleryServerRoleAllRolesCacheKey, roles);
			HelperFunctions.SetCache(CacheItem.GalleryServerRoles, rolesCache);

			return roles;
		}

		/// <summary>
		/// Retrieve a collection of Gallery Server roles that match the specified role names. The roles may be returned from a cache.
		/// </summary>
		/// <param name="roleNames">The names of the roles to return. The count of the returned collection will
		/// match the length of the roleNames array.</param>
		/// <returns>Returns an <see cref="IGalleryServerRoleCollection" /> object that contains all Gallery Server roles that
		/// match the specified role names..</returns>
		/// <remarks>The collection of all Gallery Server roles for the current gallery are stored in a cache to improve
		/// performance. <note type="implementnotes">Note to developer: Any code that modifies the roles in the data store should purge the cache so 
		/// that they can be freshly retrieved from the data store during the next request. The cache is identified by the
		/// <see cref="CacheItem.GalleryServerRoles" /> enum.</note></remarks>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryServerRoleCollection LoadGalleryServerRoles(string[] roleNames)
		{
			return LoadGalleryServerRoles().GetRolesByRoleNames(roleNames);
		}

		/// <summary>
		/// Retrieve the Gallery Server role that matches the specified role name. The role may be returned from a cache.
		/// Returns null if no matching role is found.</summary>
		/// <param name="roleName">The name of the role to return. </param>
		/// <returns>Returns an <see cref="IGalleryServerRole" /> object that matches the specified role name, or null if no matching role is found.</returns>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public static IGalleryServerRole LoadGalleryServerRole(string roleName)
		{
			return LoadGalleryServerRoles().GetRoleByRoleName(roleName);
		}

		#endregion

		#region AppError Methods

		/// <summary>
		/// Gets a collection of all application errors from the data store. The items are sorted in descending order on the
		/// <see cref="IAppError.TimeStamp"/> property, so the most recent error is first. Returns an empty collection if no
		/// errors exist.
		/// </summary>
		/// <returns>Returns a collection of all application errors from the data store.</returns>
		public static IAppErrorCollection GetAppErrors()
		{
			IAppErrorCollection appErrors = (IAppErrorCollection)HelperFunctions.GetCache(CacheItem.AppErrors);

			if (appErrors != null)
			{
				return appErrors;
			}

			// No errors in the cache, so get from data store and add to cache.
			appErrors = ErrorHandler.Error.GetAppErrors();

			HelperFunctions.SetCache(CacheItem.AppErrors, appErrors);

			return appErrors;
		}

		#endregion

		#region Data Access

		/// <summary>
		/// Gets the data provider for Gallery Server Pro. The provider contains functionality for interacting with the data store.
		/// </summary>
		/// <returns>Returns an <see cref="GalleryServerPro.Provider.Interfaces.IDataProvider" /> object.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public static GalleryServerPro.Provider.Interfaces.IDataProvider GetDataProvider()
		{
			return GalleryServerPro.Provider.DataProviderManager.Provider;
		}

		#endregion

		#region General

		/// <summary>
		/// Gets an instance of the HTML validator.
		/// </summary>
		/// <param name="html">The HTML to pass to the HTML validator.</param>
		/// <returns>Returns an instance of the HTML validator.</returns>
		public static IHtmlValidator GetHtmlValidator(string html)
		{
			return new HtmlValidator(html);
		}

		#endregion

		#region Private Static Methods

		private static IAlbum GetAlbumFromDataReader(IDataReader dr)
		{
			// SQL:
			// SELECT
			// AlbumID, FKGalleryID as GalleryID, AlbumParentID, Title, DirectoryName, Summary, ThumbnailMediaObjectID, 
			//  Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
			// FROM [gs_Album]
			// WHERE AlbumId = @AlbumId AND FKGalleryId = @GalleryId
			IAlbum album = null;
			while (dr.Read())
			{
				// A parent ID = 0 indicates the root album. Use int.MinValue to send to Album constructor.
				//int albumParentId = (Int32.Parse(dr["AlbumParentID"].ToString(), CultureInfo.InvariantCulture) == 0 ? int.MinValue : Int32.Parse(dr["AlbumParentID"].ToString(), CultureInfo.InvariantCulture));
				int albumParentId = Int32.Parse(dr["AlbumParentID"].ToString(), CultureInfo.InvariantCulture);

				album = new Album(Int32.Parse(dr["AlbumID"].ToString(), CultureInfo.InvariantCulture),
				                  albumParentId,
				                  dr["Title"].ToString(),
				                  dr["DirectoryName"].ToString(),
				                  dr["Summary"].ToString(),
				                  Int32.Parse(dr["ThumbnailMediaObjectID"].ToString(), CultureInfo.InvariantCulture),
				                  Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
				                  HelperFunctions.ToDateTime((dr["DateStart"])),
				                  HelperFunctions.ToDateTime(dr["DateEnd"]),
				                  dr["CreatedBy"].ToString().Trim(),
				                  HelperFunctions.ToDateTime(dr["DateAdded"]),
				                  dr["LastModifiedBy"].ToString().Trim(),
				                  HelperFunctions.ToDateTime(dr["DateLastModified"]),
				                  dr["OwnedBy"].ToString().Trim(),
				                  dr["OwnerRoleName"].ToString().Trim(),
				                  Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.InvariantCulture));
			}

			if (album == null)
			{
				throw new InvalidAlbumException();
			}

			album.IsInflated = true;

			return album;
		}

		/// <summary>
		/// Retrieve the specified album. Child albums and media objects are not added. The album is retrieved from the
		/// cache if it is there. If not, it is retrieved from the data store.
		/// </summary>
		/// <param name="albumId">The <see cref="IGalleryObject.Id">ID</see> that uniquely identifies the album to retrieve.</param>
		/// <returns>Returns the specified album without child albums or media objects.</returns>
		/// <exception cref="InvalidAlbumException">Thrown when an album with the specified <paramref name="albumId"/> 
		/// is not found in the data store.</exception>
		private static IAlbum RetrieveAlbum(int albumId)
		{
			Dictionary<int, IAlbum> albumCache = (Dictionary<int, IAlbum>)HelperFunctions.GetCache(CacheItem.Albums);

			IAlbum album;
			if (albumCache != null)
			{
				if (!albumCache.TryGetValue(albumId, out album))
				{
					// The cache exists, but there is no item matching the desired album ID. Retrieve from data store and add to cache.
					album = RetrieveAlbumFromDataStore(albumId);

					lock (albumCache)
					{
						if (!albumCache.ContainsKey(albumId))
						{
							albumCache.Add(albumId, album);
							HelperFunctions.SetCache(CacheItem.Albums, albumCache);
						}
					}
				}
#if DEBUG
				else
				{
					tt.Tools.MarkSpot(String.Format(CultureInfo.CurrentCulture, "Album {0} retrieved from cache.", albumId));
				}
#endif
			}
			else
			{
				// There is no cache item. Retrieve from data store and create cache item so it's there next time we want it.
				album = RetrieveAlbumFromDataStore(albumId);

				albumCache = new Dictionary<int, IAlbum>();
				albumCache.Add(albumId, album);

				HelperFunctions.SetCache(CacheItem.Albums, albumCache);
			}
			return album;
		}

		/// <summary>
		/// Retrieve the specified media object. It is retrieved from the cache if it is there. 
		/// If not, it is retrieved from the data store.
		/// </summary>
		/// <param name="mediaObjectId">The <see cref="IGalleryObject.Id">ID</see> that uniquely identifies the media object to retrieve.</param>
		/// <returns>Returns the specified media object.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an image is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		private static IGalleryObject RetrieveMediaObject(int mediaObjectId)
		{
			return RetrieveMediaObject(mediaObjectId, GalleryObjectType.Unknown, null);
		}

		/// <summary>
		/// Retrieve the specified media object. It is retrieved from the cache if it is there. 
		/// If not, it is retrieved from the data store.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object to retrieve.</param>
		/// <param name="galleryObjectType">The type of gallery object that the mediaObjectId parameter represents. If the type is 
		/// unknown, the Unknown enum value can be specified. Specify the actual type if possible (e.g. Video, Audio, Image, 
		/// etc.), as it is more efficient. An exception is thrown if the Album enum value is specified, since this method
		/// is designed only for media objects.</param>
		/// <returns>Returns the specified media object.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an image is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		private static IGalleryObject RetrieveMediaObject(int mediaObjectId, GalleryObjectType galleryObjectType)
		{
			return RetrieveMediaObject(mediaObjectId, galleryObjectType, null);
		}

		/// <summary>
		/// Retrieve the specified media object. It is retrieved from the cache if it is there. 
		/// If not, it is retrieved from the data store.
		/// </summary>
		/// <param name="mediaObjectId">The ID that uniquely identifies the media object to retrieve.</param>
		/// <param name="galleryObjectType">The type of gallery object that the mediaObjectId parameter represents. If the type is 
		/// unknown, the Unknown enum value can be specified. Specify the actual type if possible (e.g. Video, Audio, Image, 
		/// etc.), as it is more efficient. An exception is thrown if the Album enum value is specified, since this method
		/// is designed only for media objects.</param>
		/// <param name="parentAlbum">The album containing the media object specified by mediaObjectId. Specify
		/// null if a reference to the album is not available, and it will be created based on the parent album
		/// specified in the data store.</param>
		/// <returns>Returns the specified media object.</returns>
		/// <exception cref="InvalidMediaObjectException">Thrown when
		/// an image is not found in the data store that matches the mediaObjectId parameter and the current gallery.</exception>
		private static IGalleryObject RetrieveMediaObject(int mediaObjectId, GalleryObjectType galleryObjectType, IAlbum parentAlbum)
		{
			// <exception cref="InvalidAlbumException">Thrown when an 
			// album with the specified album ID is not found in the data store.</exception>
			Dictionary<int, IGalleryObject> mediaObjectCache = (Dictionary<int, IGalleryObject>)HelperFunctions.GetCache(CacheItem.MediaObjects);

			IGalleryObject mediaObject;
			if (mediaObjectCache != null)
			{
				if (!mediaObjectCache.TryGetValue(mediaObjectId, out mediaObject))
				{
					// The cache exists, but there is no item matching the desired media object ID. Retrieve from data store and add to cache.
					mediaObject = RetrieveMediaObjectFromDataStore(mediaObjectId, galleryObjectType, parentAlbum);

					lock (mediaObjectCache)
					{
						if (!mediaObjectCache.ContainsKey(mediaObjectId))
						{
							mediaObjectCache.Add(mediaObjectId, mediaObject);
							HelperFunctions.SetCache(CacheItem.MediaObjects, mediaObjectCache);
						}
					}
				}
#if DEBUG
				else
				{
					tt.Tools.MarkSpot(String.Format(CultureInfo.CurrentCulture, "Media object {0} retrieved from cache.", mediaObjectId));
				}
#endif
			}
			else
			{
				// There is no cache item. Retrieve from data store and create cache item so it's there next time we want it.
				mediaObject = RetrieveMediaObjectFromDataStore(mediaObjectId, galleryObjectType, parentAlbum);

				mediaObjectCache = new Dictionary<int, IGalleryObject>();
				mediaObjectCache.Add(mediaObjectId, mediaObject);

				HelperFunctions.SetCache(CacheItem.MediaObjects, mediaObjectCache);
			}
			return mediaObject;
		}

		private static IGalleryObject RetrieveMediaObjectFromDataStore(int id, GalleryObjectType galleryObjectType, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(id, galleryObjectType);
#endif

			// If the gallery object type is vague, we need to figure it out.
			if ((galleryObjectType == GalleryObjectType.All) || (galleryObjectType == GalleryObjectType.None) || (galleryObjectType == GalleryObjectType.Unknown))
			{
				galleryObjectType = HelperFunctions.DetermineGalleryObjectType(id);
			}

			IGalleryObject go = new NullObjects.NullGalleryObject();

			switch (galleryObjectType)
			{
				case GalleryObjectType.Image:
					{
						go = RetrieveImageFromDataStore(id, parentAlbum); break;
					}
				case GalleryObjectType.Video:
					{
						go = RetrieveVideoFromDataStore(id, parentAlbum); break;
					}
				case GalleryObjectType.Audio:
					{
						go = RetrieveAudioFromDataStore(id, parentAlbum); break;
					}
				case GalleryObjectType.External:
					{
						go = RetrieveExternalFromDataStore(id, parentAlbum); break;
					}
				case GalleryObjectType.Generic:
				case GalleryObjectType.Unknown:
					{
						go = RetrieveGenericMediaObjectFromDataStore(id, parentAlbum); break;
					}
				default:
					{
						throw new InvalidMediaObjectException(id);
					}
			}

			return go;
		}

		private static IGalleryObject RetrieveImageFromDataStore(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			IGalleryObject image = null;
			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().MediaObject_GetDataReaderMediaObjectById(mediaObjectId))
				{
					// SQL:
					//SELECT
					//  mo.MediaObjectID, mo.FKAlbumID, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
					//  mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
					//  mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.ExternalHtmlSource, mo.ExternalType, mo.Seq, 
					//  mo.CreatedBy, mo.DateAdded, mo.LastModifiedBy, mo.DateLastModified, mo.IsPrivate
					//FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
					//WHERE mo.MediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID
					while (dr.Read())
					{
						image = new Image(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OptimizedFilename"].ToString().Trim(),
							Int32.Parse(dr["OptimizedWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OptimizedHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OptimizedSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
					}
				}

				AddMediaObjectMetadata(image);
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			if (image == null)
				throw new InvalidMediaObjectException(mediaObjectId);

			return image;
		}

		private static IGalleryObject RetrieveVideoFromDataStore(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			IGalleryObject video = null;
			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().MediaObject_GetDataReaderMediaObjectById(mediaObjectId))
				{
					// SQL:
					//SELECT
					//  mo.MediaObjectID, mo.FKAlbumID, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
					//  mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
					//  mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.ExternalHtmlSource, mo.ExternalType, mo.Seq, 
					//  mo.CreatedBy, mo.DateAdded, mo.LastModifiedBy, mo.DateLastModified, mo.IsPrivate
					//FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
					//WHERE mo.MediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID
					while (dr.Read())
					{
						video = new Video(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			if (video == null)
				throw new InvalidMediaObjectException(mediaObjectId);

			return video;
		}

		private static IGalleryObject RetrieveAudioFromDataStore(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			IGalleryObject audio = null;
			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().MediaObject_GetDataReaderMediaObjectById(mediaObjectId))
				{
					// SQL:
					// SELECT
					//  MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
					//  ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
					//  OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, Synchronized, 
					//  DateAdded, FilenameHasChanged
					// FROM MediaObject
					// WHERE MediaObjectID = @MediaObjectId
					while (dr.Read())
					{
						audio = new Audio(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			if (audio == null)
				throw new InvalidMediaObjectException(mediaObjectId);

			return audio;
		}

		private static IGalleryObject RetrieveExternalFromDataStore(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			IGalleryObject externalGalleryObject = null;
			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().MediaObject_GetDataReaderMediaObjectById(mediaObjectId))
				{
					// SQL:
					// SELECT
					//  MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
					//  ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
					//  OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, Synchronized, 
					//  DateAdded, FilenameHasChanged
					// FROM MediaObject
					// WHERE MediaObjectID = @MediaObjectId
					while (dr.Read())
					{
						externalGalleryObject = new ExternalMediaObject(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance.
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["ExternalHtmlSource"].ToString().Trim(),
							MimeTypeEnumHelper.ParseMimeTypeCategory(dr["ExternalType"].ToString().Trim()),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true);
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			if (externalGalleryObject == null)
				throw new InvalidMediaObjectException(mediaObjectId);

			return externalGalleryObject;
		}

		private static IGalleryObject RetrieveGenericMediaObjectFromDataStore(int mediaObjectId, IAlbum parentAlbum)
		{
#if DEBUG
			tt.Tools.StartingMethod(mediaObjectId); // Can't specify parentAlbum because this bombs when it's null.
#endif

			IGalleryObject genericMediaObject = null;
			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().MediaObject_GetDataReaderMediaObjectById(mediaObjectId))
				{
					// SQL:
					//SELECT
					//  mo.MediaObjectID, mo.FKAlbumID, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
					//  mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
					//  mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.ExternalHtmlSource, mo.ExternalType, mo.Seq, 
					//  mo.CreatedBy, mo.DateAdded, mo.LastModifiedBy, mo.DateLastModified, mo.IsPrivate
					//FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
					//WHERE mo.MediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID
					while (dr.Read())
					{
						genericMediaObject = new GenericMediaObject(
							Int32.Parse(dr["MediaObjectId"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["FKAlbumID"].ToString(), CultureInfo.InvariantCulture),
							parentAlbum, // If null, Image ctor uses the previous parameter to do an Album CreateInstance().
							dr["Title"].ToString().Trim(),
							dr["HashKey"].ToString().Trim(),
							dr["ThumbnailFilename"].ToString(),
							Int32.Parse(dr["ThumbnailWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["ThumbnailSizeKB"].ToString(), CultureInfo.InvariantCulture),
							dr["OriginalFilename"].ToString().Trim(),
							Int32.Parse(dr["OriginalWidth"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalHeight"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["OriginalSizeKB"].ToString(), CultureInfo.InvariantCulture),
							Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture),
							dr["CreatedBy"].ToString().Trim(),
							Convert.ToDateTime(dr["DateAdded"].ToString(), CultureInfo.CurrentCulture),
							dr["LastModifiedBy"].ToString().Trim(),
							HelperFunctions.ToDateTime(dr["DateLastModified"]),
							Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.CurrentCulture),
							true,
							null);
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			if (genericMediaObject == null)
				throw new InvalidMediaObjectException(mediaObjectId);

			return genericMediaObject;
		}

		/// <summary>
		/// Retrieve the specified album from the data store. Child albums and media objects are not added. 
		/// </summary>
		/// <param name="albumId">The ID that uniquely identifies the album to retrieve.</param>
		/// <returns>Returns the specified album without child albums or media objects.</returns>
		/// <exception cref="InvalidAlbumException">Thrown when an 
		/// album with the specified album ID is not found in the data store.</exception>
		private static IAlbum RetrieveAlbumFromDataStore(int albumId)
		{
#if DEBUG
			tt.Tools.StartingMethod(albumId);
#endif

			IAlbum album = null;

			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().Album_GetDataReaderAlbumById(albumId))
				{
					album = GetAlbumFromDataReader(dr);
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			// Since we've just loaded this object from the data store, set the corresponding property.
			album.FullPhysicalPathOnDisk = album.FullPhysicalPath;

			System.Diagnostics.Debug.Assert(album.ThumbnailMediaObjectId > int.MinValue, "The album's ThumbnailMediaObjectId should have been assigned in this method.");

			if (album == null)
				throw new InvalidAlbumException(albumId);

			return album;
		}

		private static void AddChildObjects(IAlbum album)
		{
			#region Add child albums

			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().Album_GetDataReaderChildAlbumsById(album.Id))
				{
					// SQL:
					// SELECT AlbumID
					// FROM Album
					// WHERE AlbumParentID = @AlbumId
					while (dr.Read())
					{
						album.Add(Factory.CreateAlbumInstance(Convert.ToInt32(dr[0], CultureInfo.InvariantCulture)));
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			#endregion

			#region Add child media objects

			dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().Album_GetDataReaderChildMediaObjectsById(album.Id))
				{
					// SQL:
					// SELECT 
					//  MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
					//  ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
					//  OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, mo.Seq, 
					//  CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
					// FROM [gs_MediaObject]
					// WHERE FKAlbumID = @AlbumId
					while (dr.Read())
					{
						album.Add(Factory.LoadMediaObjectInstance((IDataRecord)dr, album));
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}
			#endregion

			album.AreChildrenInflated = true;
		}

		/// <summary>
		/// Add metadata items to the specified gallery object. Only Image objects have metadata associated with them, so this
		/// method has no effect on other types.
		/// </summary>
		/// <param name="go">The gallery object for which metadata items should be added to the MetadataItems collection.</param>
		private static void AddMediaObjectMetadata(IGalleryObject go)
		{
			if (!(go is Image))
				return;

			IDataReader dr = null;

			try
			{
				using (dr = Factory.GetDataProvider().MediaObject_GetDataReaderMetadataItemsByMediaObjectId(go.Id))
				{
					// SQL:
					// SELECT
					//  md.MediaObjectMetadataId, md.FKMediaObjectId, md.MetadataNameIdentifier, md.Description, md.Value
					// FROM MediaObjectMetadata md JOIN MediaObject mo ON md.FkMediaObjectId = mo.MediaObjectId
					//  JOIN Album a ON mo.FKAlbumID = a.AlbumID
					// WHERE md.FKMediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID
					while (dr.Read())
					{
						Metadata.FormattedMetadataItemName metaItemName = (Metadata.FormattedMetadataItemName)Int32.Parse(dr["MetadataNameIdentifier"].ToString(), CultureInfo.InvariantCulture);

						go.MetadataItems.Add(new Metadata.GalleryObjectMetadataItem(
						                     	Int32.Parse(dr["MediaObjectMetadataId"].ToString(), CultureInfo.InvariantCulture),
						                     	metaItemName,
						                     	dr["Description"].ToString().Trim(),
						                     	dr["Value"].ToString().Trim(),
						                     	false));
					}
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

		}

		private static void InflateAlbumFromDataReader(IAlbum album, System.Data.IDataReader dr)
		{
			// SQL:
			// SELECT
			//  AlbumID, FKGalleryID as GalleryID, AlbumParentID, Title, DirectoryName, Summary, ThumbnailMediaObjectID, 
			//  Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
			// FROM [gs_Album]
			// WHERE AlbumId = @AlbumId AND FKGalleryId = @GalleryId

			while (dr.Read())
			{
				// A parent ID = 0 indicates the root album. Use int.MinValue to send to Album constructor.
				int albumParentId = (Int32.Parse(dr["AlbumParentID"].ToString(), CultureInfo.InvariantCulture) == 0 ? int.MinValue : Int32.Parse(dr["AlbumParentID"].ToString(), CultureInfo.InvariantCulture));

				// Assign parent if it hasn't already been assigned.
				if ((album.Parent.Id == int.MinValue) && (albumParentId > int.MinValue))
				{
					album.Parent = Factory.CreateAlbumInstance(albumParentId);
				}

				album.Title = dr["Title"].ToString();
				album.DirectoryName = dr["DirectoryName"].ToString();
				album.Summary = dr["Summary"].ToString();
				album.Sequence = Int32.Parse(dr["Seq"].ToString(), CultureInfo.InvariantCulture);
				album.DateStart = HelperFunctions.ToDateTime((dr["DateStart"]));
				album.DateEnd = HelperFunctions.ToDateTime((dr["DateEnd"]));
				album.CreatedByUserName = dr["CreatedBy"].ToString().Trim();
				album.DateAdded = HelperFunctions.ToDateTime((dr["DateAdded"]));
				album.LastModifiedByUserName = dr["LastModifiedBy"].ToString().Trim();
				album.DateLastModified = HelperFunctions.ToDateTime(dr["DateLastModified"]);
				album.OwnerUserName = dr["OwnedBy"].ToString().Trim();
				album.OwnerRoleName = dr["OwnerRoleName"].ToString().Trim();
				album.IsPrivate = Convert.ToBoolean(dr["IsPrivate"].ToString(), CultureInfo.InvariantCulture);

				// Set the album's thumbnail media object ID. Setting this property sets an internal flag that will cause
				// the media object info to be retrieved when the Thumbnail property is accessed. That's why we don't
				// need to set any of the thumbnail properties.
				// WARNING: No matter what, do not call DisplayObject.CreateInstance() because that creates a new object, 
				// and we might be  executing this method from within our Thumbnail display object. Trust me, this 
				// creates hard to find bugs!
				album.ThumbnailMediaObjectId = Int32.Parse(dr["ThumbnailMediaObjectID"].ToString(), CultureInfo.InvariantCulture);

			}
		}

		private static IGalleryServerRoleCollection GetRolesFromDataReader(IDataReader dr)
		{
			IGalleryServerRoleCollection roles = new GalleryServerRoleCollection();

			//SELECT RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
			//  AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteAlbum, 
			//  AllowDeleteChildAlbum, AllowDeleteMediaObject,	AllowSynchronize, HideWatermark, 
			//  AllowAdministerSite
			//FROM gs_Roles
			//WHERE (FKGalleryId = @GalleryId)
			while (dr.Read())
			{
				roles.Add(new GalleryServerRole(dr["RoleName"].ToString(),
				                                Convert.ToBoolean(dr["AllowViewAlbumsAndObjects"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowViewOriginalImage"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowAddMediaObject"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowAddChildAlbum"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowEditMediaObject"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowEditAlbum"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowDeleteMediaObject"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowDeleteChildAlbum"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowSynchronize"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["AllowAdministerSite"], CultureInfo.InvariantCulture),
				                                Convert.ToBoolean(dr["HideWatermark"], CultureInfo.InvariantCulture)));
			}

			return roles;
		}

		/// <summary>
		/// Get all Gallery Server roles for the current gallery.
		/// </summary>
		/// <returns>Returns all Gallery Server roles for the current gallery.</returns>
		private static IGalleryServerRoleCollection GetGalleryServerRolesFromDataStore()
		{
			IGalleryServerRoleCollection roles = null;

			IDataReader dr = null;
			try
			{
				using (dr = Factory.GetDataProvider().Roles_GetDataReaderRoles())
				{
					// Create the roles.
					roles = GetRolesFromDataReader(dr);
				}

				if (roles == null)
				{
					roles = new GalleryServerRoleCollection();
				}

				#region Add complete album list

				// Add the entire list of albums that are affected by this role.
				foreach (IGalleryServerRole role in roles)
				{
					using (dr = Factory.GetDataProvider().Roles_GetDataReaderRoleAllAlbums(role.RoleName))
					{
						// Contains one field - "AlbumId"
						while (dr.Read())
						{
							role.AddToAllAlbumIds(dr.GetInt32(0));
						}
					}
				}

				#endregion

				#region Add top level albums

				// Add the list of top level albums that are affected by this role.
				foreach (IGalleryServerRole role in roles)
				{
					using (dr = Factory.GetDataProvider().Roles_GetDataReaderRoleRootAlbums(role.RoleName))
					{
						// Contains one field - "FKAlbumId"
						//SELECT gs_Role_Album.FKAlbumId
						//FROM gs_Role_Album INNER JOIN gs_Album ON gs_Role_Album.FKAlbumId = gs_Album.AlbumID
						//WHERE (gs_Role_Album.FKRoleName = @RoleName) AND (gs_Album.FKGalleryID = @GalleryId)
						while (dr.Read())
						{
							role.RootAlbumIds.Add(dr.GetInt32(0));
						}
					}
				}

				#endregion

			}
			finally
			{
				if (dr != null) dr.Close();
			}

			return roles;
		}

		/// <summary>
		/// Verify the specified roles conform to business rules. Specificially, if any of the roles have administrative permissions
		/// (AllowAdministerSite = true):
		/// 1. Make sure there is a single root album ID set to the root album.
		/// 2. Make sure all role permissions - except HideWatermark - are set to true.
		/// If anything needs updating, update the object and persist the changes to the data store. This helps keep the data store 
		/// valid in cases where the user is directly editing the tables (for example, deleting records from the gs_Role_Album table).
		/// </summary>
		/// <param name="roles">A collection of gallery server roles for the current gallery.</param>
		private static void ValidateRoleIntegrity(IGalleryServerRoleCollection roles)
		{
			foreach (IGalleryServerRole role in roles)
			{
				if (role.AllowAdministerSite)
				{
					bool hasChanges = false;

					if (!role.AllowAddChildAlbum) { role.AllowAddChildAlbum = true; hasChanges = true; }
					if (!role.AllowAddMediaObject) { role.AllowAddMediaObject = true; hasChanges = true; }
					if (!role.AllowDeleteChildAlbum) { role.AllowDeleteChildAlbum = true; hasChanges = true; }
					if (!role.AllowDeleteMediaObject) { role.AllowDeleteMediaObject = true; hasChanges = true; }
					if (!role.AllowEditAlbum) { role.AllowEditAlbum = true; hasChanges = true; }
					if (!role.AllowEditMediaObject) { role.AllowEditMediaObject = true; hasChanges = true; }
					if (!role.AllowSynchronize) { role.AllowSynchronize = true; hasChanges = true; }
					if (!role.AllowViewAlbumOrMediaObject) { role.AllowViewAlbumOrMediaObject = true; hasChanges = true; }
					if (!role.AllowViewOriginalImage) { role.AllowViewOriginalImage = true; hasChanges = true; }
					// Don't change role.HideWatermark; administrator may prefer to see watermark.

					int rootAlbumId = Factory.LoadRootAlbumInstance().Id;
					if ((role.RootAlbumIds.Count != 1) || ((role.RootAlbumIds.Count == 1) && (!role.RootAlbumIds.Contains(rootAlbumId))))
					{
						// User is an administrator but the root album has not been specified as the sole root album ID. Since admins always 
						// have complete access to all albums, let's update the data store to give the user permission to the root album.
						role.RootAlbumIds.Clear();
						role.RootAlbumIds.Add(rootAlbumId);
						hasChanges = true;
					}

					if (hasChanges)
					{
						role.Save();
					}
				}
			}
		}

		#endregion
	}
}

using System;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using GalleryServerPro.ICSharpCode.SharpZipLib.Zip;

using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Contains methods for creating and extracting ZIP archives.
	/// </summary>
	public class ZipUtility : IDisposable
	{
		#region Private Fields

		private readonly List<KeyValuePair<string, string>> _skippedFiles = new List<KeyValuePair<string, string>>();
		private ZipInputStream _zipStream;
		private bool _hasBeenDisposed; // Used by Dispose() methods
		private Hashtable _albumAndDirectoryNamesLookupTable;
		private readonly string _userName;
		private readonly IGalleryServerRoleCollection _roles;
		private readonly bool _isAuthenticated;
		private bool _discardOriginalImage;

		// 1 - 9, with 9 being the highest compression. We use the lowest compression because most gallery objects are already
		// in compressed formats (jpg, wmv, wma, mp3, etc.). Presumably the lower compression results in better performance, 
		// although I can't find any documentation to support this. In my own testing there was very little difference in the
		// resulting file size whether I used 1 or 9 (I did not attempt to measure performance.)
		private const int ZIP_COMPRESSION_LEVEL = 1;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a <see cref="ZipUtility" /> instance with the specified parameters.
		/// </summary>
		/// <param name="userName">The user name of the logged on user. May be null or empty, although some functions, 
		/// such as <see cref="ExtractZipFile"/>, require a valid user and will throw an exception if not present.</param>
		/// <param name="roles">The gallery server roles the logged on user belongs to.</param>
		public ZipUtility(string userName, IGalleryServerRoleCollection roles)
		{
			userName = userName ?? String.Empty;

			this._userName = userName;
			this._roles = roles;
			this._isAuthenticated = !String.IsNullOrEmpty(this._userName);

			Initialize();
		}

		#region Destructor (finalizer)

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="ZipUtility"/> is reclaimed by garbage collection.
		/// </summary>
		~ZipUtility()
		{
			Dispose(false);
		}

		#endregion

		#endregion

		#region Public Methods

		/// <summary>
		/// Analyze the specified ZIP file for embedded files and directories. Create albums and media objects from the
		/// files. Skip any files whose type is not enabled within Gallery Server Pro. Return a list of skipped files
		/// and the reason why they were skipped.
		/// </summary>
		/// <param name="fileStream">A stream representing a ZIP file containing directories and files to be extracted
		/// to the Gallery Server Pro library.</param>
		/// <param name="parentAlbum">The album that should contain the top-level directories and files found in the ZIP
		/// file.</param>
		/// <param name="discardOriginalImage">Indicates whether to delete the original image file after the thumbnail/
		/// original images have been created. Ignored for non-image files.</param>
		/// <returns>
		/// Returns a <see cref="System.Collections.Generic.List{T}"/> where the key is the name
		/// of the skipped file and the value is the reason for the file being skipped.
		/// </returns>
		public List<KeyValuePair<string, string>> ExtractZipFile(Stream fileStream, IAlbum parentAlbum, bool discardOriginalImage)
		{
			if (String.IsNullOrEmpty(this._userName))
				throw new InvalidOperationException("A username was not specified in the ZipUtility constructor. Media objects extracted from a ZIP archive must be associated with a logged on user.");
			
			this._albumAndDirectoryNamesLookupTable = new Hashtable(10);

			try
			{
				this._zipStream = new ZipInputStream(fileStream);
				this._discardOriginalImage = discardOriginalImage;
				ZipEntry zipContentFile;
				while ((zipContentFile = this._zipStream.GetNextEntry()) != null)
				{
					IAlbum album = VerifyAlbumExistsAndReturnReference(zipContentFile, parentAlbum);

					if (Path.GetExtension(zipContentFile.Name).ToUpperInvariant() == ".ZIP")
					{
						// We have a ZIP file embedded within the parent zip file. Recursively extract the contents of this file.
						ExtractEmbeddedZipFile(zipContentFile, parentAlbum);
					}
					else
					{
						AddMediaObjectToGallery(zipContentFile, album);
					}
				}
			}
			finally
			{
				this._zipStream.Close();
				this._zipStream = null;

				// Clear the list of hash keys to free up memory.
				MediaObjectHashKeys.Clear();
			}

			return this._skippedFiles;
		}

		/// <summary>
		/// Creates a ZIP archive, returned as a <see cref="ZipOutputStream"/>, containing the specified <paramref name="albumIds">albums
		/// </paramref> and <paramref name="mediaObjectIds">media objects</paramref>. Only media objects associated with a 
		/// physical file are included (in other words, external media objects are excluded). The archive is created in memory
		/// and is not stored on disk.
		/// </summary>
		/// <param name="parentAlbumId">The ID of the album containing the <paramref name="albumIds"/> and <paramref name="mediaObjectIds"/>.
		/// When <paramref name="albumIds"/> or <paramref name="mediaObjectIds"/> belong to more than one album, such as when a user is 
		/// downloading multiple albums contained within a virtual album, specify <see cref="Int32.MinValue"/>.</param>
		/// <param name="albumIds">The ID's of the albums to add to the ZIP archive. It's child albums and media objects are recursively 
		/// added. Each album must exist within the parent album, but does not have to be an immediate child (it can be a grandchild, etc).</param>
		/// <param name="mediaObjectIds">The ID's of the media objects to add to the archive. Each media object must exist within the parent album,
		/// but does not have to be an immediate child (it can be a grandchild, etc).</param>
		/// <param name="imageSize">Size of the image to add to the ZIP archive. This parameter applies only to <see cref="Image"/> 
		/// media objects.</param>
		/// <returns>Returns a <see cref="MemoryStream"/> of a ZIP archive that contains the specified albums and media objects.</returns>
		public Stream CreateZipStream(int parentAlbumId, List<int> albumIds, List<int> mediaObjectIds, DisplayObjectType imageSize)
		{
			string currentItemBasePath;
			string basePath = null;
			bool applyWatermark = true; // Will be overwritten later
			try
			{
				// Get the path to the parent album. This will fail when parentAlbumId does not refer to a valid album.
				basePath = String.Concat(Factory.LoadAlbumInstance(parentAlbumId, false).FullPhysicalPathOnDisk, Path.DirectorySeparatorChar);
				
				applyWatermark = this.DetermineIfWatermarkIsToBeApplied(parentAlbumId);
			}
			catch (ErrorHandler.CustomExceptions.InvalidAlbumException) { /* Ignore for now; we'll check basePath later */ }

			MemoryStream ms = new MemoryStream();
			ZipOutputStream zos = new ZipOutputStream(ms);

			zos.SetLevel(ZIP_COMPRESSION_LEVEL);

			if (albumIds != null)
			{
				foreach (int albumId in albumIds)
				{
					IAlbum album = Factory.LoadAlbumInstance(albumId, true);

					if (String.IsNullOrEmpty(basePath))
					{
						// The base path wasn't assigned because albumParentId does not refer to a valid album. Instead we will use the path
						// of the current album's parent.
						currentItemBasePath = String.Concat(album.Parent.FullPhysicalPathOnDisk, Path.DirectorySeparatorChar);

						applyWatermark = DetermineIfWatermarkIsToBeApplied(albumId);
					}
					else
					{
						currentItemBasePath = basePath;
					}

					AddZipEntry(zos, album, imageSize, currentItemBasePath, applyWatermark);
				}
			}

			if (mediaObjectIds != null)
			{
				foreach (int mediaObjectId in mediaObjectIds)
				{
					IGalleryObject mediaObject = Factory.LoadMediaObjectInstance(mediaObjectId);

					if (String.IsNullOrEmpty(basePath))
					{
						// The base path wasn't assigned because albumParentId does not refer to a valid album. Instead we will use the path
						// of the current media object's album.
						currentItemBasePath = String.Concat(mediaObject.Parent.FullPhysicalPathOnDisk, Path.DirectorySeparatorChar);

						applyWatermark = DetermineIfWatermarkIsToBeApplied(mediaObject.Parent.Id);
					}
					else
					{
						currentItemBasePath = basePath;
					}

					AddFileZipEntry(zos, mediaObject, imageSize, currentItemBasePath, applyWatermark);
				}
			}

			zos.Finish();

			return ms;
		}

		/// <summary>
		/// Creates a ZIP archive, returned as a <see cref="ZipOutputStream"/>, containing the specified <paramref name="filePath"/>.
		/// The archive is created in memory and is not stored on disk. If filePath is an image, no attempt is made to apply a
		/// watermark, even if watermarking is enabled.
		/// </summary>
		/// <param name="filePath">The full path to a file to be added to a ZIP archive.</param>
		/// <returns>Returns a <see cref="MemoryStream"/> of a ZIP archive that contains the specified file.</returns>
		public Stream CreateZipStream(string filePath)
		{
			MemoryStream ms = new MemoryStream();
			ZipOutputStream zos = new ZipOutputStream(ms);

			zos.SetLevel(ZIP_COMPRESSION_LEVEL);

			AddFileZipEntry(zos, filePath, null, Path.GetDirectoryName(filePath), false, false);

			zos.Finish();

			return ms;
		}

		/// <summary>
		/// Gets the full path to the media object file. If <paramref name="mediaObject"/>
		/// is an <see cref="Image"/>, then return the thumbnail, compressed, or original file as specified in <paramref name="imageSize"/>.
		/// If a media object does not have a physical file (for example, external media objects), then return <see cref="String.Empty"/>.
		/// Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets\sonorandesert.jpg
		/// </summary>
		/// <param name="mediaObject">The media object for which to return a path to the media file.</param>
		/// <param name="imageSize">Size of the image to return. This parameter applies only to <see cref="Image"/> media objects.</param>
		/// <returns>Returns the full path to the media object file.</returns>
		private static string GetMediaFilePath(IGalleryObject mediaObject, DisplayObjectType imageSize)
		{
			string filePath = String.Empty;

			if (mediaObject is Image)
			{
				switch (imageSize)
				{
					case DisplayObjectType.Thumbnail:
						filePath = mediaObject.Thumbnail.FileNamePhysicalPath;
						break;
					case DisplayObjectType.Optimized:
						filePath = mediaObject.Optimized.FileNamePhysicalPath;
						break;
					case DisplayObjectType.Original:
						filePath = mediaObject.Original.FileNamePhysicalPath;
						break;
				}
			}

			if (String.IsNullOrEmpty(filePath))
			{
				filePath = mediaObject.Original.FileNamePhysicalPath;
			}

			return filePath;
		}

		/// <summary>
		/// Adds the media objects in the <paramref name="album"/> to the ZIP archive. Only media objects associated with a 
		/// physical file are added (that is, external media objects are excluded).
		/// </summary>
		/// <param name="zos">The ZipOutputStream (ZIP archive) the media object file is to be added to.</param>
		/// <param name="album">The album to be added to the ZIP archive.</param>
		/// <param name="imageSize">Size of the image to add to the ZIP archive. This parameter applies only to <see cref="Image"/> 
		/// media objects.</param>
		/// <param name="basePath">The full path to the directory containing the highest-level media file to be added
		/// to the ZIP archive. Must include trailing slash. Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\</param>
		/// <param name="applyWatermark">Indicates whether to apply a watermark to images as they are added to the archive.
		/// Applies only for media objects in the <see cref="album"/> that are an <see cref="Image"/>.</param>
		private void AddZipEntry(ZipOutputStream zos, IAlbum album, DisplayObjectType imageSize, string basePath, bool applyWatermark)
		{
			foreach (IAlbum childAlbum in album.GetChildGalleryObjects(GalleryObjectType.Album, false, !this._isAuthenticated))
			{
				AddZipEntry(zos, childAlbum, imageSize, basePath, applyWatermark);
			}

			foreach (IGalleryObject mediaObject in album.GetChildGalleryObjects(GalleryObjectType.MediaObject, false, !this._isAuthenticated))
			{
				AddFileZipEntry(zos, mediaObject, imageSize, basePath, applyWatermark);
			}
		}

		/// <summary>
		/// Adds the file associated with the <paramref name="mediaObject"/> to the ZIP archive.
		/// </summary>
		/// <param name="zos">The ZipOutputStream (ZIP archive) the media object file is to be added to.</param>
		/// <param name="mediaObject">The media object to be added to the ZIP archive.</param>
		/// <param name="imageSize">Size of the image to add to the ZIP archive. This parameter applies only to <see cref="Image"/> 
		/// media objects.</param>
		/// <param name="basePath">The full path to the directory containing the highest-level media file to be added
		/// to the ZIP archive. Must include trailing slash. Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\</param>
		/// <param name="applyWatermark">Indicates whether to apply a watermark to images as they are added to the archive.
		/// Applies only when <paramref name="mediaObject"/> is an <see cref="Image"/>.</param>
		private static void AddFileZipEntry(ZipOutputStream zos, IGalleryObject mediaObject, DisplayObjectType imageSize, string basePath, bool applyWatermark)
		{
			// Get the path to the file we'll be adding to the zip file.
			string filePath = GetMediaFilePath(mediaObject, imageSize);

			// Always use the file name of the original for naming the entry in the zip file.
			string fileNameForZip = mediaObject.Original.FileNamePhysicalPath;

			if ((!String.IsNullOrEmpty(filePath)) && (!String.IsNullOrEmpty(fileNameForZip)))
			{
				AddFileZipEntry(zos, filePath, fileNameForZip, basePath, (mediaObject is Image), applyWatermark);
			}
		}

		/// <summary>
		/// Adds the file specified in <paramref name="filePath"/> to the ZIP archive. If <paramref name="fileNameForZip"/>
		/// is specified, use that filename as the name of the file in the ZIP archive.
		/// </summary>
		/// <param name="zos">The ZipOutputStream (ZIP archive) the media object file is to be added to.</param>
		/// <param name="filePath">The full path to the media object file to be added to the ZIP archive.
		/// Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets\zOpt_sonorandesert.jpg</param>
		/// <param name="fileNameForZip">The full path to the file whose name is to be used to name the file specified
		/// by <paramref name="filePath"/> in the ZIP archive. If null or empty, the actual filename is used. This path
		/// does not have to refer to an existing file on disk, but it must begin with <paramref name="basePath"/>.
		/// Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets\sonorandesert.jpg</param>
		/// <param name="basePath">The full path to the directory containing the highest-level media file to be added
		/// to the ZIP archive. Must include trailing slash. Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\</param>
		/// <param name="isImage">Indicates whether the file specified in <paramref name="filePath"/> is an image. If it
		/// is, and <paramref name="applyWatermark"/> is <c>true</c>, a watermark is applied to the image as it is inserted
		/// into the archive.</param>
		/// <param name="applyWatermark">Indicates whether to apply a watermark to images as they are added to the archive.
		/// Applies only when <paramref name="isImage"/> is <c>true</c>.</param>
		private static void AddFileZipEntry(ZipOutputStream zos, string filePath, string fileNameForZip, string basePath, bool isImage, bool applyWatermark)
		{
			int bufferSize = Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectDownloadBufferSize;
			byte[] buffer = new byte[bufferSize];

			#region Determine ZIP entry name

			// Get name of the file as it will be stored in the ZIP archive. This is the fragment of the full file path
			// after the base path. Ex: If basePath="C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\"
			// and filePath="C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets\zOpt_sonorandesert.jpg",
			// then zipEntryName="desert sunsets\zOpt_sonorandesert.jpg". The ZIP algorithm will automatically sense the 
			// embedded directory ("desert sunsets") and create it.
			string zipEntryName;
			if (String.IsNullOrEmpty(fileNameForZip))
			{
				zipEntryName = filePath.Replace(basePath, String.Empty);
			}
			else
			{
				zipEntryName = fileNameForZip.Replace(basePath, String.Empty);
			}

			#endregion

			using (Stream stream = CreateStream(filePath, isImage, applyWatermark))
			{
				ZipEntry entry = new ZipEntry(zipEntryName);
				entry.Size = stream.Length;
				zos.PutNextEntry(entry);

				int byteCount;
				while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
				{
					zos.Write(buffer, 0, byteCount);
				}
			}
		}

		/// <summary>
		/// Creates a stream for the specified <paramref name="filePath"/>. If <paramref name="isImage"/> is <c>true</c> and
		/// <paramref name="applyWatermark"/> is <c>true</c>, then a <see cref="MemoryStream"/> is created containing a 
		/// watermarked version of the file. Otherwise, a <see cref="FileStream"/> is returned.
		/// </summary>
		/// <param name="filePath">The full path to the media object file to be added to the ZIP archive.
		/// Ex: C:\Inetpub\wwwroot\galleryserverpro\mediaobjects\Summer 2005\sunsets\desert sunsets\zOpt_sonorandesert.jpg</param>
		/// <param name="isImage">Indicates whether the file specified in <paramref name="filePath"/> is an image. If it
		/// is, and <paramref name="applyWatermark"/> is <c>true</c>, a watermark is applied to the image as it is inserted
		/// into the archive.</param>
		/// <param name="applyWatermark">Indicates whether to apply a watermark to images as they are added to the archive.
		/// Applies only when <paramref name="isImage"/> is <c>true</c>.</param>
		/// <returns>Returns a <see cref="MemoryStream"/> or <see cref="FileStream"/> for the specified <paramref name="filePath"/>
		/// The position of the stream is zero to allow it to be read.</returns>
		private static Stream CreateStream(string filePath, bool isImage, bool applyWatermark)
		{
			Stream stream;

			if (isImage && applyWatermark)
			{
				// Apply watermark to file and return.
				stream = new MemoryStream();

				using (System.Drawing.Image watermarkedImage = ImageHelper.AddWatermark(filePath))
				{
					watermarkedImage.Save(stream, ImageFormat.Jpeg);
					stream.Position = 0;
				}
			}
			else
			{
				stream = File.OpenRead(filePath);
			}

			return stream;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Adds the <paramref name="zipContentFile"/> as a media object to the <paramref name="album"/>.
		/// </summary>
		/// <param name="zipContentFile">A reference to a file in a ZIP archive.</param>
		/// <param name="album">The album to which the file should be added as a media object.</param>
		private void AddMediaObjectToGallery(ZipEntry zipContentFile, IAlbum album)
		{
			string zipFileName = Path.GetFileName(zipContentFile.Name).Trim();

			if (zipFileName.Length == 0)
				return;

			string uniqueFilename = HelperFunctions.ValidateFileName(album.FullPhysicalPathOnDisk, zipFileName);
			string uniqueFilepath = Path.Combine(album.FullPhysicalPathOnDisk, uniqueFilename);

			// Extract the file from the zip stream and save as the specified filename.
			ExtractMediaObjectFile(uniqueFilepath);

			// Get the file we just saved to disk.
			FileInfo mediaObjectFile = new FileInfo(uniqueFilepath);

			try
			{
				IGalleryObject mediaObject = Factory.CreateMediaObjectInstance(mediaObjectFile, album);
				HelperFunctions.UpdateAuditFields(mediaObject, this._userName);
				mediaObject.Save();

				if ((_discardOriginalImage) && (mediaObject is Business.Image))
				{
					((Business.Image)mediaObject).DeleteHiResImage();
					mediaObject.Save();
				}
			}
			catch (ErrorHandler.CustomExceptions.UnsupportedMediaObjectTypeException ex)
			{
				this._skippedFiles.Add(new KeyValuePair<string, string>(mediaObjectFile.Name, ex.Message));
				File.Delete(mediaObjectFile.FullName);
			}
		}

		private void ExtractMediaObjectFile(string uniqueFilepath)
		{
			FileStream streamWriter = null;
			try
			{
				streamWriter = File.Create(uniqueFilepath);
				int bufferSize = Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectDownloadBufferSize;
				byte[] data = new byte[bufferSize];

				int byteCount;
				while ((byteCount = this._zipStream.Read(data, 0, data.Length)) > 0)
				{
					streamWriter.Write(data, 0, byteCount);
				}
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
				}
			}
		}

		private IAlbum VerifyAlbumExistsAndReturnReference(ZipEntry zipContentFile, IAlbum rootParentAlbum)
		{
			// Get the directory path of the next file or directory within the zip file.
			// Ex: album1\album2\album3, album1
			string zipDirectoryPath = Path.GetDirectoryName(zipContentFile.Name);

			string[] directoryNames = zipDirectoryPath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			string albumFullPhysicalPath = rootParentAlbum.FullPhysicalPathOnDisk;
			IAlbum currentAlbum = rootParentAlbum;

			foreach (string directoryNameFromZip in directoryNames)
			{
				string shortenedDirName = GetPreviouslyCreatedTruncatedAlbumName(albumFullPhysicalPath, directoryNameFromZip);

				// Ex: c:\inetpub\wwwroot\galleryserver\mypics\2006\album1
				albumFullPhysicalPath = System.IO.Path.Combine(albumFullPhysicalPath, shortenedDirName);

				IAlbum newAlbum = null;
				if (Directory.Exists(albumFullPhysicalPath))
				{
					// Directory exists, so there is probably an album corresponding to it. Find it.
					IGalleryObjectCollection childGalleryObjects = currentAlbum.GetChildGalleryObjects(GalleryObjectType.Album);
					foreach (IGalleryObject childGalleryObject in childGalleryObjects)
					{
						if (childGalleryObject.FullPhysicalPathOnDisk == albumFullPhysicalPath)
						{
							newAlbum = childGalleryObject as Album; break;
						}
					}

					if (newAlbum == null)
					{
						// No album in the database matches that directory. Add it.

						// Before we add the album, we need to make sure the user has permission to add the album. Check if user
						// is authenticated and if the current album is the one passed into this method. It can be assumed that any
						// other album we encounter has been created by this method and we checked for permission when it was created.
						if (this._isAuthenticated && (currentAlbum.Id == rootParentAlbum.Id))
							SecurityManager.ThrowIfUserNotAuthorized(SecurityActions.AddChildAlbum, this._roles, currentAlbum.Id, this._isAuthenticated, currentAlbum.IsPrivate);

						newAlbum = Factory.CreateAlbumInstance();
						newAlbum.Parent = currentAlbum;
						newAlbum.IsPrivate = currentAlbum.IsPrivate;
						newAlbum.DirectoryName = directoryNameFromZip;
						HelperFunctions.UpdateAuditFields(newAlbum, this._userName);
						newAlbum.Save();
					}
				}
				else
				{
					// The directory doesn't exist. Create an album.

					// Before we add the album, we need to make sure the user has permission to add the album. Check if user
					// is authenticated and if the current album is the one passed into this method. It can be assumed that any
					// other album we encounter has been created by this method and we checked for permission when it was created.
					if (this._isAuthenticated && (currentAlbum.Id == rootParentAlbum.Id))
						SecurityManager.ThrowIfUserNotAuthorized(SecurityActions.AddChildAlbum, this._roles, currentAlbum.Id, this._isAuthenticated, currentAlbum.IsPrivate);

					newAlbum = Factory.CreateAlbumInstance();
					newAlbum.IsPrivate = currentAlbum.IsPrivate;
					newAlbum.Parent = currentAlbum;
					newAlbum.Title = directoryNameFromZip;
					HelperFunctions.UpdateAuditFields(newAlbum, this._userName);
					newAlbum.Save();

					// If the directory name written to disk is different than the name from the zip file, add it to
					// our hash table.
					if (!directoryNameFromZip.Equals(newAlbum.DirectoryName))
					{
						this._albumAndDirectoryNamesLookupTable.Add(Path.Combine(currentAlbum.FullPhysicalPathOnDisk, directoryNameFromZip), Path.Combine(currentAlbum.FullPhysicalPathOnDisk, newAlbum.DirectoryName));
					}

				}
				currentAlbum = newAlbum;
			}

			return currentAlbum;
		}

		/// <summary>
		/// Return the shortened directory name, if one exists, corresponding to the directory name from the zip 
		/// file. If no shortened version exists, or it hasn't yet been created, return the directoryNameFromZip 
		/// parameter.
		/// </summary>
		/// <param name="directoryPathOnDisk">The full path of the directory, as currently stored on the disk,
		/// that contains (or will contain) the directory specified in the directoryNameFromZip parameter.</param>
		/// <param name="directoryNameFromZip">The directory name as retrieved from the zip file.</param>
		/// <returns>Return the shortened directory name, if one exists, corresponding to the directory name from the zip 
		/// file. If no shortened version exists, or it hasn't yet been created, return the directoryNameFromZip 
		/// parameter.</returns>
		/// <example>Say a zip file contains a directory named 'ThisIsAReallyLongDirectoryName'. When we first
		/// encounter this directory name, a shortened version is automatically created when the album is created,
		/// such as 'ThisIsAReallyLong'. (The purpose is to prevent too many long-named nested directories
		/// from exceeding the OS's  limit.) A record of this is added to the hash table. Now, as we process
		/// subsequent items in the zip file within the same directory, this method will return the shortened
		/// version. This is used by the calling method to add these subsequent items to this directory rather than 
		/// to a new one.</example>
		private string GetPreviouslyCreatedTruncatedAlbumName(string directoryPathOnDisk, string directoryNameFromZip)
		{
			// The directory name (directoryNameFromZip), as it comes from the zip file, may exceed our maximum length.
			// When this happens, a record is inserted into the _albumAndDirectoryNamesLookupTable hash table
			// with the original directory name as it comes from the zip file (key) and the shortened 
			// version that is used as the actual directory name in the media objects directory (value).
			// (Note that full directory paths are stored in the hash table to differentiate directories with the 
			// same names but at different heirarchies.)
			string fullDirectoryPath = Path.Combine(directoryPathOnDisk, directoryNameFromZip);
			string shortenedDirectoryName = directoryNameFromZip;

			foreach (DictionaryEntry de in this._albumAndDirectoryNamesLookupTable)
			{
				if (de.Key.ToString().Equals(fullDirectoryPath))
				{
					string shortenedPath = de.Value.ToString();
					shortenedDirectoryName = shortenedPath.Substring(shortenedPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
				}
			}
			return shortenedDirectoryName;
		}

		/// <summary>
		/// Process a ZIP file that is embedded within the parent ZIP file. Its contents are extracted and turned into 
		/// albums and media objects just like items in the parent ZIP file.
		/// </summary>
		/// <param name="zipFile">A reference to a ZIP file contained within the parent ZIP file. Notice that we don't do
		/// anything with this parameter other than verify that its extension is "ZIP". That's because we actually extract
		/// the file from the parent ZIP file by calling the ExtractMediaObjectFile method, which extracts the file from 
		/// the class-level member variable _zipStream</param>
		/// <param name="parentAlbum">The album that should contain the top-level directories and files found in the ZIP
		/// file.</param>
		private void ExtractEmbeddedZipFile(ZipEntry zipFile, IAlbum parentAlbum)
		{
			#region Validation

			if (Path.GetExtension(zipFile.Name).ToUpperInvariant() != ".ZIP")
			{
				throw new ArgumentException(String.Concat("The zipFile parameter of the method ExtractEmbeddedZipFile in class ZipUtility must be a ZIP file. Instead, it had the file extension ", Path.GetExtension(zipFile.Name), "."));
			}

			if (parentAlbum == null)
			{
				throw new ArgumentNullException("parentAlbum");
			}

			#endregion

			string filepath = Path.Combine(parentAlbum.FullPhysicalPathOnDisk, Guid.NewGuid().ToString("N") + ".config");
			try
			{
				ExtractMediaObjectFile(filepath);
				using (ZipUtility zip = new ZipUtility(this._userName, this._roles))
				{
					this._skippedFiles.AddRange(zip.ExtractZipFile(new FileInfo(filepath).OpenRead(), parentAlbum, true));
				}
			}
			finally
			{
				File.Delete(filepath);
			}
		}

		private static void Initialize()
		{
			// Clear the list of hash keys so we're starting with a fresh load from the data store.
			MediaObjectHashKeys.Clear();
		}

		/// <summary>
		/// Determines if watermark is to be applied to images in the album specified by <paramref name="albumId"/>.
		/// </summary>
		/// <param name="albumId">The album ID.</param>
		/// <returns>Returns <c>true</c> if a watermark must be applied, <c>false</c> if not.</returns>
		private bool DetermineIfWatermarkIsToBeApplied(int albumId)
		{
			bool applyWatermark = false;
			bool applyWatermarkConfig = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ApplyWatermark;
			bool userHasNoWatermarkPermission = SecurityManager.IsUserAuthorized(SecurityActions.HideWatermark, this._roles, albumId, this._isAuthenticated, false);

			if (AppSetting.Instance.IsInReducedFunctionalityMode || (applyWatermarkConfig && !userHasNoWatermarkPermission))
			{
				applyWatermark = true;
			}
			return applyWatermark;
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this._hasBeenDisposed)
			{
				// Dispose of resources held by this instance.
				if (this._zipStream != null)
				{
					this._zipStream.Dispose();
					this._zipStream = null;
				}

				// Set the sentinel.
				this._hasBeenDisposed = true;
			}
		}

		#endregion

	}
}

/*  Copyright 2005 Roger Martin, Tech Info Systems
  
    Gallery Server Pro is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    Gallery Server Pro is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA */

using System;
using System.IO;
using System.Security.Permissions;
using GalleryServerPro.ErrorHandler.Properties;
using System.Globalization;

namespace GalleryServerPro.ErrorHandler.CustomExceptions
{
	/// <summary>
	/// The exception that is thrown when a general error occurs in the GalleryServerPro.Web namespace.
	/// </summary>
	[Serializable]
	public class WebException : Exception
	{
		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Web namespace.
		/// </summary>
		public WebException()
			: base(Resources.Web_Ex_Msg) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Web namespace.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public WebException(string msg)
			: base(msg) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Web namespace.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public WebException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Web namespace.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected WebException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
	
	/// <summary>
	/// The exception that is thrown when a general error occurs in the GalleryServerPro.Business namespace.
	/// </summary>
	[Serializable]
	public class BusinessException : Exception
	{
		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Business namespace.
		/// </summary>
		public BusinessException()
			: base(Resources.Business_Ex_Msg) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Business namespace.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public BusinessException(string msg)
			: base(msg) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Business namespace.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public BusinessException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Business namespace.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected BusinessException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when a general error occurs in the GalleryServerPro.Data namespace.
	/// </summary>
	[Serializable]
	public class DataException : Exception
	{
		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Data namespace.
		/// </summary>
		public DataException()
			: base(Resources.Data_Ex_Msg) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Data namespace.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public DataException(string msg)
			: base(msg) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Data namespace.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public DataException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// The exception that is thrown when a general error occurs in the GalleryServerPro.Data namespace.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected DataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when an invalid media object is referenced.
	/// </summary>
	[Serializable]
	public class ApplicationNotInitializedException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate Gallery Server has not been properly intialized.
		/// </summary>
		public ApplicationNotInitializedException()
			: base(Resources.ApplicationNotInitialized_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate Gallery Server has not been properly intialized.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public ApplicationNotInitializedException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate Gallery Server has not been properly intialized.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public ApplicationNotInitializedException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate Gallery Server has not been properly intialized.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected ApplicationNotInitializedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when a user attempts to perform an action the user does not have authorization to perform.
	/// </summary>
	[Serializable]
	public class GallerySecurityException : Exception
	{
		/// <summary>
		/// Throws an exception when a user attempts to perform an action the user does not have authorization to perform.
		/// </summary>
		public GallerySecurityException()
			: base(Resources.GallerySecurity_Ex_Msg) { }

		/// <summary>
		/// Throws an exception when a user attempts to perform an action the user does not have authorization to perform.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public GallerySecurityException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception when a user attempts to perform an action the user does not have authorization to perform.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public GallerySecurityException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception when a user attempts to perform an action the user does not have authorization to perform.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected GallerySecurityException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when an invalid media object is referenced.
	/// </summary>
	[Serializable]
	public class InvalidMediaObjectException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate an invalid media object.
		/// </summary>
		public InvalidMediaObjectException()
			: base(Resources.InvalidMediaObject_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate an invalid media object.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public InvalidMediaObjectException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate an invalid media object.
		/// </summary>
		/// <param name="mediaObjectId">The ID of the media object that is not valid.</param>
		public InvalidMediaObjectException(int mediaObjectId)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidMediaObject_Ex_Msg2, mediaObjectId)) { }

		/// <summary>
		/// Throws an exception to indicate an invalid media object.
		/// </summary>
		/// <param name="mediaObjectId">The ID of the media object that is not valid.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public InvalidMediaObjectException(int mediaObjectId, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidMediaObject_Ex_Msg2, mediaObjectId), innerException) { }

		/// <summary>
		/// Throws an exception to indicate an invalid media object.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public InvalidMediaObjectException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate an invalid media object.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected InvalidMediaObjectException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when an invalid album is referenced.
	/// </summary>
	[Serializable]
	public class InvalidAlbumException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate an invalid album.
		/// </summary>
		public InvalidAlbumException()
			: base(Resources.InvalidAlbum_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate an invalid album.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public InvalidAlbumException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate an invalid album.
		/// </summary>
		/// <param name="albumId">The ID of the album that is not valid.</param>
		public InvalidAlbumException(int albumId)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidAlbum_Ex_Msg2, albumId)) { }

		/// <summary>
		/// Throws an exception to indicate an invalid album.
		/// </summary>
		/// <param name="albumId">The ID of the album that is not valid.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public InvalidAlbumException(int albumId, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidAlbum_Ex_Msg2, albumId), innerException) { }

		/// <summary>
		/// Throws an exception to indicate an invalid album.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public InvalidAlbumException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate an invalid album.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected InvalidAlbumException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when the user album feature is enabled but the album ID that is specified for
	/// containing the user albums does not exist.
	/// </summary>
	[Serializable]
	public class CannotDeleteAlbumException : Exception
	{
		/// <summary>
		/// Throws an exception when an album cannot be deleted.
		/// </summary>
		public CannotDeleteAlbumException()
			: base(Resources.CannotDeleteAlbum_Ex_Msg) { }

		/// <summary>
		/// Throws an exception when an album cannot be deleted.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public CannotDeleteAlbumException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception when an album cannot be deleted.
		/// </summary>
		/// <param name="albumId">The ID of the album that cannot be deleted.</param>
		public CannotDeleteAlbumException(int albumId)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.CannotDeleteAlbum_Ex_Msg2, albumId)) { }

		/// <summary>
		/// Throws an exception when an album cannot be deleted.
		/// </summary>
		/// <param name="albumId">The ID of the album that cannot be deleted.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public CannotDeleteAlbumException(int albumId, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.CannotDeleteAlbum_Ex_Msg2, albumId), innerException) { }

		/// <summary>
		/// Throws an exception when an album cannot be deleted.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public CannotDeleteAlbumException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception when an album cannot be deleted.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected CannotDeleteAlbumException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when the .NET Framework is unable to load an image file into the System.Drawing.Bitmap 
	/// class. This is probably because it is corrupted, not an image supported by the .NET Framework, or the server does
	/// not have enough memory to process the image.
	/// </summary>
	[Serializable]
	public class UnsupportedImageTypeException : Exception, System.Runtime.Serialization.ISerializable
	{
		[NonSerializedAttribute]
		private GalleryServerPro.Business.Interfaces.IGalleryObject _mediaObject;

		/// <summary>
		/// Throws an exception to indicate the .NET Framework is unable to load an image file into the System.Drawing.Bitmap 
		/// class. This is probably because it is corrupted, not an image supported by the .NET Framework, or the server does
		/// not have enough memory to process the image.
		/// </summary>
		public UnsupportedImageTypeException()
			: base(Resources.UnsupportedImageType_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate the .NET Framework is unable to load an image file into the System.Drawing.Bitmap 
		/// class. This is probably because it is corrupted, not an image supported by the .NET Framework, or the server does
		/// not have enough memory to process the image.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public UnsupportedImageTypeException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate the .NET Framework is unable to load an image file into the System.Drawing.Bitmap 
		/// class. This is probably because it is corrupted, not an image supported by the .NET Framework, or the server does
		/// not have enough memory to process the image.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public UnsupportedImageTypeException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate the .NET Framework is unable to load an image file into the System.Drawing.Bitmap 
		/// class. This is probably because it is corrupted, not an image supported by the .NET Framework, or the server does
		/// not have enough memory to process the image.
		/// </summary>
		/// <param name="mediaObject">The media object that contains the unsupported image file.</param>
		public UnsupportedImageTypeException(GalleryServerPro.Business.Interfaces.IGalleryObject mediaObject)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedImageType_Ex_Msg2, ((mediaObject != null) && (mediaObject.Original != null) ? mediaObject.Original.FileName : String.Empty)))
		{
			this._mediaObject = mediaObject;
		}

		/// <summary>
		/// Throws an exception to indicate the .NET Framework is unable to load an image file into the System.Drawing.Bitmap 
		/// class. This is probably because it is corrupted, not an image supported by the .NET Framework, or the server does
		/// not have enough memory to process the image.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected UnsupportedImageTypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			if (info == null) throw new ArgumentNullException("info");

			this._mediaObject = (GalleryServerPro.Business.Interfaces.IGalleryObject)info.GetValue("MediaObject", typeof(GalleryServerPro.Business.Interfaces.IGalleryObject));
		}

		/// <summary>
		/// Gets the media object that is causing the exception.
		/// </summary>
		public GalleryServerPro.Business.Interfaces.IGalleryObject MediaObject
		{
			get
			{
				return this._mediaObject;
			}
		}

		/// <summary>
		/// When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo with
		/// information about the exception.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			if (info == null) throw new ArgumentNullException("info");

			// Serialize each field.
			info.AddValue("MediaObject", _mediaObject);

			// Let the base object serialize its fields.
			base.GetObjectData(info, context);
		}

	}

	/// <summary>
	/// The exception that is thrown when Gallery Server encounters a file it does not recognize as 
	/// a valid media object (e.g. image, video, audio, etc.). This may be because the file is a type that
	/// is disabled, or it may have an unrecognized file extension and the allowUnspecifiedMimeTypes configuration
	/// setting is false.
	/// </summary>
	[Serializable]
	public class UnsupportedMediaObjectTypeException : Exception, System.Runtime.Serialization.ISerializable
	{
		[NonSerializedAttribute]
		private string _mediaObjectFilePath;

		/// <summary>
		/// Throws an exception to indicate a file that is not recognized as a valid media object supported by 
		/// Gallery Server Pro. This may be because the file is a type that is disabled, or it may have an 
		/// unrecognized file extension and the allowUnspecifiedMimeTypes configuration setting is false.
		/// </summary>
		public UnsupportedMediaObjectTypeException()
			: base(Resources.UnsupportedMediaObjectType_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate a file that is not recognized as a valid media object supported by 
		/// Gallery Server Pro. This may be because the file is a type that is disabled, or it may have an 
		/// unrecognized file extension and the allowUnspecifiedMimeTypes configuration setting is false.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public UnsupportedMediaObjectTypeException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate a file that is not recognized as a valid media object supported by 
		/// Gallery Server Pro. This may be because the file is a type that is disabled, or it may have an 
		/// unrecognized file extension and the allowUnspecifiedMimeTypes configuration setting is false.
		/// </summary>
		/// <param name="filePath">The full filepath to the file that is not recognized as a valid media object
		/// (ex: C:\inetpub\wwwroot\gs\mediaobjects\myvacation\utah\bikingslickrock.jpg).</param>
		public UnsupportedMediaObjectTypeException(string filePath)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedMediaObjectType_Ex_Msg2, Path.GetExtension(filePath)))
		{
			this._mediaObjectFilePath = filePath;
		}

		/// <summary>
		/// Throws an exception to indicate a file that is not recognized as a valid media object supported by 
		/// Gallery Server Pro. This may be because the file is a type that is disabled, or it may have an 
		/// unrecognized file extension and the allowUnspecifiedMimeTypes configuration setting is false.
		/// </summary>
		/// <param name="file">The FileInfo object that is not recognized as a valid media object.</param>
		public UnsupportedMediaObjectTypeException(FileSystemInfo file)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedMediaObjectType_Ex_Msg2, (file != null ? Path.GetExtension(file.FullName) : String.Empty)))
		{
			this._mediaObjectFilePath = (file == null ? Resources.DefaultFilename : file.FullName);
		}

		/// <summary>
		/// Throws an exception to indicate a file that is not recognized as a valid media object supported by 
		/// Gallery Server Pro. This may be because the file is a type that is disabled, or it may have an 
		/// unrecognized file extension and the allowUnspecifiedMimeTypes configuration setting is false.
		/// </summary>
		/// <param name="file">The FileInfo object that is not recognized as a valid media object.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public UnsupportedMediaObjectTypeException(FileSystemInfo file, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedMediaObjectType_Ex_Msg2, (file != null ? Path.GetExtension(file.FullName) : String.Empty)), innerException)
		{
			this._mediaObjectFilePath = (file == null ? Resources.DefaultFilename : file.FullName);
		}

		/// <summary>
		/// Throws an exception to indicate a file that is not recognized as a valid media object supported by 
		/// Gallery Server Pro. This may be because the file is a type that is disabled, or it may have an 
		/// unrecognized file extension and the allowUnspecifiedMimeTypes configuration setting is false.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected UnsupportedMediaObjectTypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			if (info == null) throw new ArgumentNullException("info");

			this._mediaObjectFilePath = info.GetString("MediaObjectFileName");
		}

		/// <summary>
		/// Gets the filename of the media object that is causing the exception. Example:
		/// C:\mypics\vacation\grandcanyon.jpg, grandcanyon.jpg
		/// </summary>
		public string MediaObjectFilePath
		{
			get
			{
				return this._mediaObjectFilePath;
			}
		}

		/// <summary>
		/// When overridden in a derived class, sets the System.Runtime.Serialization.SerializationInfo with
		/// information about the exception.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			if (info == null) throw new ArgumentNullException("info");

			// Serialize each field.
			info.AddValue("MediaObjectFilePath", _mediaObjectFilePath);

			// Let the base object serialize its fields.
			base.GetObjectData(info, context);
		}
	}

	/// <summary>
	/// The exception that is thrown when Gallery Server Pro cannot find a directory.
	/// </summary>
	[Serializable]
	public class InvalidMediaObjectDirectoryException : Exception
	{
		[NonSerializedAttribute]
		private string _mediaObjectPath;

		/// <summary>
		/// Throws an exception to indicate an invalid media objects directory.
		/// </summary>
		public InvalidMediaObjectDirectoryException()
			: base(Resources.InvalidMediaObjectsDirectory_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate an invalid media objects directory.
		/// </summary>
		/// <param name="mediaObjectPath">The media object directory that is not valid.</param>
		public InvalidMediaObjectDirectoryException(string mediaObjectPath)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidMediaObjectsDirectory_Ex_Msg2, mediaObjectPath))
		{
			this._mediaObjectPath = (mediaObjectPath ?? Resources.DefaultDirectoryPath);
		}

		/// <summary>
		/// Throws an exception to indicate an invalid media objects directory.
		/// </summary>
		/// <param name="mediaObjectPath">The media object directory that is not valid.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public InvalidMediaObjectDirectoryException(string mediaObjectPath, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.InvalidMediaObjectsDirectory_Ex_Msg2, mediaObjectPath), innerException)
		{
			this._mediaObjectPath = (mediaObjectPath ?? Resources.DefaultDirectoryPath);
		}

		/// <summary>
		/// Throws an exception to indicate an invalid media objects directory.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected InvalidMediaObjectDirectoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			if (info == null) throw new ArgumentNullException("info");

			this._mediaObjectPath = info.GetString("MediaObjectPath");
		}

		/// <summary>
		/// Gets the media object directory that cannot be written to. Example: C:\inetput\wwwroot\mediaobjects
		/// </summary>
		public string MediaObjectPath
		{
			get
			{
				return this._mediaObjectPath;
			}
		}
	}

	/// <summary>
	/// The exception that is thrown when Gallery Server Pro is unable to write to, or delete from, a directory.
	/// </summary>
	[Serializable]
	public class CannotWriteToDirectoryException : Exception
	{
		[NonSerializedAttribute]
		private string _directoryPath;

		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to write to, or delete from, a directory.
		/// </summary>
		public CannotWriteToDirectoryException()
			: base(Resources.CannotWriteToDirectory_Ex_Msg) { }

		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to write to, or delete from, a directory.
		/// </summary>
		/// <param name="directoryPath">The directory that cannot be written to.</param>
		public CannotWriteToDirectoryException(string directoryPath)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.CannotWriteToDirectory_Ex_Msg2, directoryPath))
		{
			this._directoryPath = (directoryPath ?? Resources.DefaultDirectoryPath);
		}


		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to write to, or delete from, a directory.
		/// </summary>
		/// <param name="directoryPath">The directory that cannot be written to.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public CannotWriteToDirectoryException(string directoryPath, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.CannotWriteToDirectory_Ex_Msg2, directoryPath), innerException)
		{
			this._directoryPath = (directoryPath ?? Resources.DefaultDirectoryPath);
		}

		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to write to, or delete from, a directory.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected CannotWriteToDirectoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) 
		{
			if (info == null) throw new ArgumentNullException("info");

			this._directoryPath = info.GetString("DirectoryPath");
		}

		/// <summary>
		/// Gets the directory that cannot be written to. Example: C:\inetput\wwwroot\mediaobjects
		/// </summary>
		public string DirectoryPath
		{
			get
			{
				return this._directoryPath;
			}
		}

	}

	/// <summary>
	/// The exception that is thrown when Gallery Server Pro is unable to read from a directory.
	/// </summary>
	[Serializable]
	public class CannotReadFromDirectoryException : Exception
	{
		[NonSerializedAttribute]
		private string _directoryPath;

		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to read from a directory.
		/// </summary>
		public CannotReadFromDirectoryException()
			: base(Resources.CannotReadFromDirectory_Ex_Msg) { }

		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to read from a directory.
		/// </summary>
		/// <param name="directoryPath">The directory that cannot be read from.</param>
		public CannotReadFromDirectoryException(string directoryPath)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.CannotReadFromDirectory_Ex_Msg2, directoryPath))
		{
			this._directoryPath = (directoryPath ?? Resources.DefaultDirectoryPath);
		}


		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to read from a directory.
		/// </summary>
		/// <param name="directoryPath">The directory that cannot be read from.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public CannotReadFromDirectoryException(string directoryPath, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Resources.CannotReadFromDirectory_Ex_Msg2, directoryPath), innerException)
		{
			this._directoryPath = (directoryPath ?? Resources.DefaultDirectoryPath);
		}

		/// <summary>
		/// Throws an exception when Gallery Server Pro is unable to read from a directory.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected CannotReadFromDirectoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) 
		{
			if (info == null) throw new ArgumentNullException("info");

			this._directoryPath = info.GetString("DirectoryPath");
		}

		/// <summary>
		/// Gets the directory that cannot be read from. Example: C:\inetput\wwwroot\mediaobjects
		/// </summary>
		public string DirectoryPath
		{
			get
			{
				return this._directoryPath;
			}
		}

	}

	/// <summary>
	/// The exception that is thrown when Gallery Server encounters a query string parameter it does not recognize.
	/// </summary>
	[Serializable]
	public class UnexpectedQueryStringException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate an unexpected query string value.
		/// </summary>
		public UnexpectedQueryStringException()
			: base(Resources.UnexpectedQueryString_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate an unexpected query string value.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public UnexpectedQueryStringException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate an unexpected query string value.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public UnexpectedQueryStringException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate an unexpected query string value.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected UnexpectedQueryStringException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when Gallery Server encounters form data in a web page it does not recognize.
	/// </summary>
	[Serializable]
	public class UnexpectedFormValueException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate an unexpected form data in a web page.
		/// </summary>
		public UnexpectedFormValueException()
			: base(Resources.UnexpectedFormData_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate an unexpected form data in a web page.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public UnexpectedFormValueException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate an unexpected form data in a web page.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public UnexpectedFormValueException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate an unexpected form data in a web page.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected UnexpectedFormValueException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when an invalid gallery server role is referenced, or one is attempted to be created
	/// with invalid parameters.
	/// </summary>
	[Serializable]
	public class InvalidGalleryServerRoleException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate when an invalid gallery server role is referenced, or one is attempted to be created
		/// with invalid parameters.
		/// </summary>
		public InvalidGalleryServerRoleException()
			: base(Resources.InvalidGalleryServerRole_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate when an invalid gallery server role is referenced, or one is attempted to be created
		/// with invalid parameters.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public InvalidGalleryServerRoleException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate when an invalid gallery server role is referenced, or one is attempted to be created
		/// with invalid parameters.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public InvalidGalleryServerRoleException(string msg, Exception innerException)
			: base(msg, innerException) { }

		/// <summary>
		/// Throws an exception to indicate when an invalid gallery server role is referenced, or one is attempted to be created
		/// with invalid parameters.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected InvalidGalleryServerRoleException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	//public class UnexpectedFormValueException : Exception
	//{
	//  //This exception should be called whenever a page is processing form data and encounters
	//  //invalid data. For example, there may be a hidden SPAN tag that stores an object's ID.
	//  //If that value is invalid (which should only happen if the user manipulated the HTML source
	//  //or the record has been deleted since the page was generated), then this exception should
	//  //be called.
	//  public UnexpectedFormValueException()
	//    : base("Invalid Form Data: The form contains unexpected data.") {}
	//  public UnexpectedFormValueException(Exception innerException)
	//    : base("Invalid Form Data: The form contains unexpected data.", innerException) { }
	//}

	//public class UnexpectedViewStateException : Exception
	//{
	//  //This exception should be called whenever a page is processing form data and encounters
	//  //invalid data. For example, there may be a hidden SPAN tag that stores an object's ID.
	//  //If that value is invalid (which should only happen if the user manipulated the HTML source
	//  //or the record has been deleted since the page was generated), then this exception should
	//  //be called.
	//  public UnexpectedViewStateException()
	//    : base("Invalid View State: The view state contains unexpected data.") {}
	//  public UnexpectedViewStateException(Exception innerException)
	//    : base("Invalid View State: The view state contains unexpected data.", innerException) {}
	//}

	//public class CannotAccessFileException : Exception
	//{
	//  //This exception should be called whenever the operating system prevents the code from 
	//  //manipulating a file on the hard drive. This includes deleting objects, renaming
	//  //objects, etc.
	//  public CannotAccessFileException()
	//    : base("Cannot access the file: The operating system won't allow access to the object, and your request "
	//    + "cannot be completed. This can occur when the web server user account has insufficient permissions, or "
	//    + "the object no longer exists.") {}
	//  public CannotAccessFileException(Exception innerException)
	//    : base("Cannot access the file: The operating system won't allow access to the object, and your request "
	//    + "cannot be completed. This can occur when the web server user account has insufficient permissions, or "
	//    + "the object no longer exists.", innerException) { }
	//}

	///// <summary>
	///// The exception that is thrown when Gallery Server Pro is unable to add a media object to the physical directory
	///// or the data store.
	///// </summary>
	//public class CannotAddObjectException : Exception
	//{
	//  //This exception should be called whenever an object cannot be added to the database.
	//  private string _objectFilename;
	//  private const string exMessage = "Cannot add the object named {0} to the database. This can occur when the file is corrupted, missing, or is a type not supported by Gallery Server Pro.";

	//  /// <summary>
	//  /// Throws an exception to indicate Gallery Server Pro is unable to add a media object to the physical directory
	//  /// or the data store.
	//  /// </summary>
	//  /// <param name="ObjectFilename">The filename of the media object that is causing the exception. Example:
	//  /// C:\mypics\vacation\grandcanyon.jpg, grandcanyon.jpg</param>
	//  public CannotAddObjectException(string ObjectFilename)
	//    : base(string.Format(exMessage, ObjectFilename))
	//  {
	//    _objectFilename = ObjectFilename;
	//  }

	//  /// <summary>
	//  /// Throws an exception to indicate Gallery Server Pro is unable to add a media object to the physical directory
	//  /// or the data store.
	//  /// </summary>
	//  /// <param name="ObjectFilename">The filename of the media object that is causing the exception. Example:
	//  /// C:\mypics\vacation\grandcanyon.jpg, grandcanyon.jpg</param>
	//  /// <param name="innerException">The exception that is the cause of the current exception. If the 
	//  /// innerException parameter is not a null reference, the current exception is raised in a catch
	//  /// block that handles the inner exception.</param>
	//  public CannotAddObjectException(string ObjectFilename, Exception innerException)
	//    : base(string.Format(exMessage, ObjectFilename), innerException)
	//  {
	//    this._objectFilename = ObjectFilename;
	//  }

	//  /// <summary>
	//  /// Gets the filename of the media object that is causing the exception. Example:
	//  /// C:\mypics\vacation\grandcanyon.jpg, grandcanyon.jpg
	//  /// </summary>
	//  public string ObjectFileName
	//  {
	//    get
	//    {
	//      return this._objectFilename;
	//    }
	//  }
	//}

	/// <summary>
	/// The exception that is thrown when a user attempts to begin a synchronization when another one is already
	/// in progress.
	/// </summary>
	[Serializable]
	public class SynchronizationInProgressException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate the requested synchronization cannot be started because another one is
		/// in progress.
		/// </summary>
		public SynchronizationInProgressException()
			: base(Resources.SynchronizationInProgress_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate the requested synchronization cannot be started because another one is
		/// in progress.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public SynchronizationInProgressException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate the requested synchronization cannot be started because another one is
		/// in progress.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public SynchronizationInProgressException(string msg, Exception innerException)
			: base(msg, innerException)
		{ }

		/// <summary>
		/// Throws an exception to indicate the requested synchronization cannot be started because another one is
		/// in progress.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected SynchronizationInProgressException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when a user requests the cancellation of an in-progress synchronization.
	/// </summary>
	[Serializable]
	public class SynchronizationTerminationRequestedException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate when a user requests the cancellation of an in-progress synchronization.
		/// </summary>
		public SynchronizationTerminationRequestedException()
			: base(Resources.SynchronizationTerminationRequested_Ex_Msg) { }

		/// <summary>
		/// Throws an exception to indicate when a user requests the cancellation of an in-progress synchronization.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public SynchronizationTerminationRequestedException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate when a user requests the cancellation of an in-progress synchronization.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public SynchronizationTerminationRequestedException(string msg, Exception innerException)
			: base(msg, innerException)
		{ }

		/// <summary>
		/// Throws an exception to indicate when a user requests the cancellation of an in-progress synchronization.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected SynchronizationTerminationRequestedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when a user tries to transfer (either by moving or copying)
	/// an album to one of its own directories, whether direct or nested. For example,
	/// a user cannot copy an album from D:\gs_store\folder1 to D:\gs_store\folder1\folder2.
	/// </summary>
	[Serializable]
	public class CannotTransferAlbumToNestedDirectoryException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate the requested move or copy album command cannot take place because the destination
		/// album is a child album of the album we are copying, or is the same album as the one we are copying.
		/// </summary>
		public CannotTransferAlbumToNestedDirectoryException()
			: base(Resources.CannotTransferAlbumToNestedDirectoryException_Ex_msg) { }

		/// <summary>
		/// Throws an exception to indicate the requested move or copy album command cannot take place because the destination
		/// album is a child album of the album we are copying, or is the same album as the one we are copying.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public CannotTransferAlbumToNestedDirectoryException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate the requested move or copy album command cannot take place because the destination
		/// album is a child album of the album we are copying, or is the same album as the one we are copying.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public CannotTransferAlbumToNestedDirectoryException(string msg, Exception innerException)
			: base(msg, innerException)
		{ }

		/// <summary>
		/// Throws an exception to indicate the requested move or copy album command cannot take place because the destination
		/// album is a child album of the album we are copying, or is the same album as the one we are copying.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected CannotTransferAlbumToNestedDirectoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	/// <summary>
	/// The exception that is thrown when a user tries to move a directory but the operating system
	/// won't allow it. This can happen if the user is viewing the contents of the directory in Windows Explorer.
	/// </summary>
	[Serializable]
	public class CannotMoveDirectoryException : Exception
	{
		/// <summary>
		/// Throws an exception to indicate the application is unable to move a directory on the hard drive due to
		/// a restriction by the operating system.
		/// </summary>
		public CannotMoveDirectoryException()
			: base(Resources.CannotMoveDirectoryException_Ex_msg) { }

		/// <summary>
		/// Throws an exception to indicate the application is unable to move a directory on the hard drive due to
		/// a restriction by the operating system.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		public CannotMoveDirectoryException(string msg)
			: base(msg) { }

		/// <summary>
		/// Throws an exception to indicate the application is unable to move a directory on the hard drive due to
		/// a restriction by the operating system.
		/// </summary>
		/// <param name="msg">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the 
		/// innerException parameter is not a null reference, the current exception is raised in a catch
		/// block that handles the inner exception.</param>
		public CannotMoveDirectoryException(string msg, Exception innerException)
			: base(msg, innerException)
		{ }

		/// <summary>
		/// Throws an exception to indicate the application is unable to move a directory on the hard drive due to
		/// a restriction by the operating system.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
		/// data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
		/// information about the source or destination.</param>
		protected CannotMoveDirectoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	///// <summary>
	///// The exception that is thrown when a user attempts to upload a file(s) whose size exceeds
	///// the maximum request length (specified in web.config).
	///// </summary>
	//[Serializable]
	//public class MaxRequestLengthExceededException : Exception
	//{
	//  /// <summary>
	//  /// Throws an exception to indicate the user attempted to upload a file(s) whose size exceeds
	//  /// the maximum request length.
	//  /// </summary>
	//  public MaxRequestLengthExceededException() 
	//    : base("File(s) cannot be uploaded: The maximum upload size has been exceeded.") { }

	//  /// <summary>
	//  /// Throws an exception to indicate the user attempted to upload a file(s) whose size exceeds
	//  /// the maximum request length.
	//  /// </summary>
	//  /// <param name="msg">A message that describes the error.</param>
	//  public MaxRequestLengthExceededException(string msg)
	//    : base(msg) { }

	//  /// <summary>
	//  /// Throws an exception to indicate the user attempted to upload a file(s) whose size exceeds
	//  /// the maximum request length.
	//  /// </summary>
	//  /// <param name="maxRequestLength">The maximum allowed size of an upload request, in bytes.</param>
	//  /// <param name="actualRequestLength">The actual size of the upload request, in bytes.</param>
	//  public MaxRequestLengthExceededException(int maxRequestLength, long actualRequestLength)
	//    : base(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The maximum upload size "
	//    + "has been exceeded. The total size of the upload was {0} KB, which exceeds the limit of "
	//    + "{1} KB specified in web.config (or machine.config, if not "
	//    + "specified in web.config). Either reduce the size of your file(s) or, if you are the "
	//    + "administrator, adjust the value of maxRequestLength in web.config.", actualRequestLength, maxRequestLength))
	//  { }

	//  /// <summary>
	//  /// Throws an exception to indicate the user attempted to upload a file(s) whose size exceeds
	//  /// the maximum request length.
	//  /// </summary>
	//  /// <param name="maxRequestLength">The maximum allowed size of an upload request, in bytes.</param>
	//  /// <param name="actualRequestLength">The actual size of the upload request, in bytes.</param>
	//  /// <param name="innerException">The exception that is the cause of the current exception. If the 
	//  /// innerException parameter is not a null reference, the current exception is raised in a catch
	//  /// block that handles the inner exception.</param>
	//  public MaxRequestLengthExceededException(int maxRequestLength, long actualRequestLength, Exception innerException)
	//    : base(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The maximum upload size "
	//    + "has been exceeded. The total size of the upload was {0} KB, which exceeds the limit of "
	//    + "{1} KB specified in web.config (or machine.config, if not "
	//    + "specified in web.config). Either reduce the size of your file(s) or, if you are the "
	//    + "administrator, adjust the value of maxRequestLength in web.config.", actualRequestLength, maxRequestLength), innerException)
	//  {}

	//  /// <summary>
	//  /// Throws an exception to indicate the user attempted to upload a file(s) whose size exceeds
	//  /// the maximum request length.
	//  /// </summary>
	//  /// <param name="msg">A message that describes the error.</param>
	//  /// <param name="innerException">The exception that is the cause of the current exception. If the 
	//  /// innerException parameter is not a null reference, the current exception is raised in a catch
	//  /// block that handles the inner exception.</param>
	//  public MaxRequestLengthExceededException(string msg, Exception innerException)
	//    : base(msg, innerException) { }

	//  /// <summary>
	//  /// Throws an exception to indicate the user attempted to upload a file(s) whose size exceeds
	//  /// the maximum request length.
	//  /// </summary>
	//  /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
	//  /// data about the exception being thrown.</param>
	//  /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
	//  /// information about the source or destination.</param>
	//  protected MaxRequestLengthExceededException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
	//    : base(info, context) {}
	//}
}

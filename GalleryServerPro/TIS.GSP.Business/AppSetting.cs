using System;
using System.IO;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Contains application level settings used by Gallery Server Pro. This class must be initialized by the calling assembly early in the 
	/// application life cycle. It is initialized by calling <see cref="Initialize" />. In the case of the Gallery 
	/// Server Pro web application, <see cref="Initialize" /> is called from the static constructor of the GspPage base page.
	/// </summary>
	public class AppSetting
	{
		#region Private Static Fields

		private static AppSetting _instance;
		private static object sharedLock = new object();

		#endregion

		#region Private Fields

		private string _mediaObjectPhysicalPath;
		private string _tempUploadDirectory;
		private string _thumbnailPath;
		private string _optimizedPath;
		private string _physicalAppPath;
		private string _applicationName;
		private ApplicationTrustLevel _trustLevel = ApplicationTrustLevel.None;
		private bool _isDotNet3Installed;
		private bool _isInitialized;
		private bool _isInTrialPeriod;
		private bool _isRegistered;
		private readonly System.Collections.Specialized.StringCollection _verifiedFilePaths = new System.Collections.Specialized.StringCollection();

		#endregion

		#region Constructors

		private AppSetting()
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the full physical path to the directory containing the media objects. Example:
		/// "C:\inetpub\wwwroot\galleryserverpro\mediaobjects"
		/// </summary>
		public string MediaObjectPhysicalPath
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _mediaObjectPhysicalPath;
			}
			protected set
			{
				// Validate the path. Will throw an exception if a problem is found.
				try
				{
					bool isReadOnly = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.MediaObjectPathIsReadOnly;
					if (!this._verifiedFilePaths.Contains(value))
					{
						if (isReadOnly)
							HelperFunctions.ValidatePhysicalPathExistsAndIsReadable(value);
						else
						{
							HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(value);
							this._verifiedFilePaths.Add(value);
						}
					}
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
				{
					// Mark this app as not initialized so when user attempts to fix issue and refreshes the page, the initialize 
					// sequence will run again.
					this._isInitialized = false;
					throw;
				}

				this._mediaObjectPhysicalPath = value;
			}
		}

		/// <summary>
		/// Gets the full physical path to the directory where files can be temporarily stored. Example:
		/// "C:\inetpub\wwwroot\galleryserverpro\App_Data\_Temp"
		/// </summary>
		public string TempUploadDirectory
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _tempUploadDirectory;
			}
			protected set
			{
				// Validate the path. Will throw an exception if a problem is found.
				try
				{
					if (!this._verifiedFilePaths.Contains(value))
					{
						HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(value);
						this._verifiedFilePaths.Add(value);
					}
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
				{
					// Mark this app as not initialized so when user attempts to fix issue and refreshes the page, the initialize 
					// sequence will run again.
					this._isInitialized = false;
					throw;
				}

				this._tempUploadDirectory = value;
			}
		}

		/// <summary>
		/// Gets the full physical path to the directory where Gallery Server stores the thumbnail images of media objects.
		/// If no directory is specified in the configuration file, returns the main media object path (that is, returns
		/// the same value as the <see cref="MediaObjectPhysicalPath" /> property).
		/// Example: "C:\inetpub\wwwroot\galleryserverpro\mediaobjects"
		/// </summary>
		public string ThumbnailPath
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _thumbnailPath;
			}
			protected set
			{
				if (String.IsNullOrEmpty(value))
					throw new ArgumentNullException("ThumbnailPath");
				
				// Validate the path. Will throw an exception if a problem is found.
				try
				{
					if (!this._verifiedFilePaths.Contains(value))
					{
						HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(value);
						this._verifiedFilePaths.Add(value);
					}
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
				{
					// Mark this app as not initialized so when user attempts to fix issue and refreshes the page, the initialize 
					// sequence will run again.
					this._isInitialized = false;
					throw;
				}

				this._thumbnailPath = value;
			}
		}

		/// <summary>
		/// Gets the full physical path to the directory where Gallery Server stores the optimized images of media objects.
		/// If no directory is specified in the configuration file, returns the main media object path (that is, returns
		/// the same value as the <see cref="MediaObjectPhysicalPath" /> property).
		/// Example: "C:\inetpub\wwwroot\galleryserverpro\mediaobjects"
		/// </summary>
		public string OptimizedPath
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _optimizedPath;
			}
			protected set
			{
				if (String.IsNullOrEmpty(value))
					throw new ArgumentNullException("OptimizedPath");

				// Validate the path. Will throw an exception if a problem is found.
				try
				{
					if (!this._verifiedFilePaths.Contains(value))
					{
						HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(value);
						this._verifiedFilePaths.Add(value);
					}
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
				{
					// Mark this app as not initialized so when user attempts to fix issue and refreshes the page, the initialize 
					// sequence will run again.
					this._isInitialized = false;
					throw;
				}
				this._optimizedPath = value;
			}
		}

		/// <summary>
		/// Gets the physical application path of the currently running application. For web applications this will be equal to
		/// the Request.PhysicalApplicationPath property.
		/// </summary>
		public string PhysicalApplicationPath
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _physicalAppPath;
			}
			protected set
			{
				this._physicalAppPath = value;
			}
		}

		/// <summary>
		/// Gets the trust level of the currently running application. 
		/// </summary>
		public ApplicationTrustLevel AppTrustLevel
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _trustLevel;
			}
			protected set
			{
				this._trustLevel = value;
			}
		}

		/// <summary>
		/// Gets the name of the currently running application. Default is "Gallery Server Pro".
		/// </summary>
		public string ApplicationName
		{
			get
			{
				if (!this._isInitialized)
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.ApplicationNotInitializedException();
				}

				return _applicationName;
			}
			protected set { _applicationName = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the Microsoft .NET Framework 3.0 is installed on the current system.
		/// </summary>
		public bool IsDotNet3Installed
		{
			get
			{
				return this._isDotNet3Installed;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current library has been populated with data from the calling assembly.
		/// This library is initialized by calling <see cref="Initialize" />.
		/// </summary>
		public bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current gallery is currently in the initial 30-day trial period.
		/// </summary>
		public bool IsInTrialPeriod
		{
			get
			{
				return this._isInTrialPeriod;
			}
		}

		/// <summary>
		/// Gets a value indicating whether a valid product key has been entered for the current gallery.
		/// </summary>
		public bool IsRegistered
		{
			get
			{
				return this._isRegistered;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the initial 30-day trial for the current gallery has expired and no valid product key 
		/// has been entered.
		/// </summary>
		public bool IsInReducedFunctionalityMode
		{
			get
			{
				return (!IsInTrialPeriod && !IsRegistered);
			}
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Gets a reference to the <see cref="AppSetting" /> singleton for this app domain.
		/// </summary>
		public static AppSetting Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (sharedLock)
					{
						if (_instance == null)
						{
							AppSetting tempAppSetting = new AppSetting();

							// Ensure that writes related to instantiation are flushed.
							System.Threading.Thread.MemoryBarrier();
							_instance = tempAppSetting;
						}
					}
				}

				return _instance;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Assign various application-wide properties to be used during the lifetime of the application. This method
		/// should be called once when the application first starts.
		/// </summary>
		/// <param name="trustLevel">The trust level of the current application.</param>
		/// <param name="physicalAppPath">The physical path of the currently executing application. For web applications
		/// this will be equal to the Request.PhysicalApplicationPath property.</param>
		/// <param name="appName">The name of the currently running application.</param>
		/// <exception cref="System.InvalidOperationException">Thrown when this method is called more than once during 
		/// the application's lifetime.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if the trustLevel parameter has the value
		/// ApplicationTrustLevel.None.</exception>
		/// <exception cref="System.ArgumentNullException">Thrown if any parameters are null or empty.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException">
		/// Thrown when Gallery Server Pro is unable to write to, or delete from, a directory. This may be the media objects
		/// directory, thumbnail or optimized directory, the temporary directory (defined in 
		/// <see cref="GlobalConstants.TempUploadDirectory"/>), or the App_Data directory.</exception>
		public void Initialize(ApplicationTrustLevel trustLevel, string physicalAppPath, string appName)
		{
			#region Validation

			if (this._isInitialized)
			{
				throw new System.InvalidOperationException("The AppSetting instance has already been initialized. It cannot be initialized more than once.");
			}

			if (trustLevel == ApplicationTrustLevel.None)
			{
				throw new System.ComponentModel.InvalidEnumArgumentException("Invalid ApplicationTrustLevel value. ApplicationTrustLevel.None is not valid. Use ApplicationTrustLevel.Unknown if the trust level cannot be calculated.");
			}

			if (String.IsNullOrEmpty(physicalAppPath)) throw new ArgumentNullException("physicalAppPath");

			if (String.IsNullOrEmpty(appName)) throw new ArgumentNullException("appName");

			#endregion

			this.AppTrustLevel = trustLevel;
			this.PhysicalApplicationPath = physicalAppPath;
			this.ApplicationName = appName;

			Core core = ConfigManager.GetGalleryServerProConfigSection().Core;

			string mediaObjectPath = core.MediaObjectPath;
			string thumbnailPath = (String.IsNullOrEmpty(core.ThumbnailPath) ? mediaObjectPath : core.ThumbnailPath);
			string optimizedPath = (String.IsNullOrEmpty(core.OptimizedPath) ? mediaObjectPath : core.OptimizedPath);

			if (core.MediaObjectPathIsReadOnly)
				ValidateReadOnlyGallery(mediaObjectPath, thumbnailPath, optimizedPath);

			// Setting the MediaObjectPhysicalPath property will throw an exception if the directory does not exist or is not writeable.
			this.MediaObjectPhysicalPath = HelperFunctions.CalculateFullPath(physicalAppPath, mediaObjectPath);

			// The property setter for the ThumbnailPath and OptimizedPath propertys will throw an exception if the directory 
			// does not exist or is not writeable.
			this.ThumbnailPath = HelperFunctions.CalculateFullPath(physicalAppPath, thumbnailPath);
			this.OptimizedPath = HelperFunctions.CalculateFullPath(physicalAppPath, optimizedPath);

			ConfigureAppDataDirectory(physicalAppPath);

			ConfigureTempDirectory(physicalAppPath);

			try
			{
				// Verify the database has the minimum default records and the latest data schema.
				Factory.GetDataProvider().InitializeDataStore();
			}
			catch (Exception ex)
			{
				// In certain situations the method ConfigureAppDataDirectory (above) will determine that the App_Data directory is writeable yet SQLite
				// is still unable to use it. It is unclear exactly why this occurs: I observed that the method was able to create and delete the test
				// file even when the Effective Permissions showed there was no Delete permission. To handle this, we'll check the exception and, if 
				// it is the one that is thrown when there are not enough permissions, we'll re-throw it as a CannotWriteToDirectoryException. The
				// global error handler in Gallery.cs will catch this and show a user-friendly error.
				if (ex.Message.StartsWith("Attempt to write a read-only database"))
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException(Path.Combine(physicalAppPath, GlobalConstants.AppDataDirectory), ex);
				}
				else
				{
					throw;
				}
			}

			this._isDotNet3Installed = DetermineIfDotNet3IsInstalled();

			this._isRegistered = (GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.ProductKey.Equals(GlobalConstants.ProductKey));

			DateTime galleryCreationDate = Factory.GetDataProvider().Gallery_GetCurrentGallery(new Gallery()).CreationDate;
			this._isInTrialPeriod = (galleryCreationDate.AddDays(GlobalConstants.TrialNumberOfDays) > DateTime.Now);
			
			this._isInitialized = true;
		}

		private static void ValidateReadOnlyGallery(string mediaObjectPath, string thumbnailPath, string optimizedPath)
		{
			// When a gallery is read only, the following must be true:
			// 1. The thumbnail and optimized path must be different than the media object path.
			// 2. The SynchAlbumTitleAndDirectoryName setting must be false.
			// 3. The EnableUserAlbum setting must be false.
			if ((mediaObjectPath.Equals(thumbnailPath, StringComparison.OrdinalIgnoreCase)) ||
					(mediaObjectPath.Equals(optimizedPath, StringComparison.OrdinalIgnoreCase)))
			{
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.BusinessException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "Invalid configuration. A read-only gallery requires that the thumbnail and optimized file paths be different than the original media objects path. mediaObjectPath={0}; thumbnailPath={1}; optimizedPath={2}", mediaObjectPath, thumbnailPath, optimizedPath));
			}

			if (ConfigManager.GetGalleryServerProConfigSection().Core.SynchAlbumTitleAndDirectoryName)
			{
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.BusinessException("Invalid configuration. A read-only gallery requires that the automatic renaming of directory names be disabled. Set the synchAlbumTitleAndDirectoryName property to false in config\\galleryserverpro.config.");
			}

			if (ConfigManager.GetGalleryServerProConfigSection().Core.EnableUserAlbum)
			{
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.BusinessException("Invalid configuration. A read-only gallery requires that user albums be disabled. Set the enableUserAlbum property to false in config\\galleryserverpro.config.");
			}
		}

		private void ConfigureTempDirectory(string physicalAppPath)
		{
			this.TempUploadDirectory = Path.Combine(physicalAppPath, GlobalConstants.TempUploadDirectory);

			try
			{
				// Clear out all directories and files in the temp directory. If an IOException error occurs, perhaps due to a locked file,
				// record it but do not let it propagate up the stack.
				DirectoryInfo di = new DirectoryInfo(this._tempUploadDirectory);
				foreach (FileInfo file in di.GetFiles())
				{
					if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
					{
						file.Delete();
					}
				}
				foreach (DirectoryInfo dirInfo in di.GetDirectories())
				{
					if ((dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
					{
						dirInfo.Delete(true);
					}
				}
			}
			catch (IOException ex)
			{
				GalleryServerPro.ErrorHandler.Error.Record(ex);
				HelperFunctions.PurgeCache();
			}
			catch (UnauthorizedAccessException ex)
			{
				GalleryServerPro.ErrorHandler.Error.Record(ex);
				HelperFunctions.PurgeCache();
			}
		}

		private void ConfigureAppDataDirectory(string physicalAppPath)
		{
			// Validate that the App_Data path is read-writeable. Will throw an exception if a problem is found.
			string appDataDirectory = Path.Combine(physicalAppPath, GlobalConstants.AppDataDirectory);
			try
			{
				HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(appDataDirectory);
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
			{
				// Mark this app as not initialized so when user attempts to fix issue and refreshes the page, the initialize 
				// sequence will run again.
				this._isInitialized = false;
				throw;
			}
		}

		/// <summary>
		/// Return a value indicating whether the .NET Framework 3.0 is installed on the current system. This is determined
		/// by invoking a method in the GalleryServerPro.Business.Wpf assembly. This assembly has a reference to 
		/// PresentationCore.dll and WindowsBase.dll, both .NET 3.0 assemblies. If the method throws a TargetInvocationException,
		/// we'll assume the framework is not installed.
		/// </summary>
		/// <returns>Returns true if .NET Framework 3.0 is installed; otherwise returns false.</returns>
		/// <remarks>The technique used here is fairly fragile. Any exception thrown by the invoked method is received as a
		/// TargetInvocationException, even if .NET 3.0 is installed. However, this was perceived to be the best available
		/// technique.
		/// </remarks>
		private static bool DetermineIfDotNet3IsInstalled()
		{
			bool isDotNet3Installed = false;

			//try
			//{
			//  GalleryServerPro.Business.Wpf.WpfMetadataExtractor.AddWpfBitmapMetadata(String.Empty, null);
			//  isDotNet3Installed = true;
			//}
			//catch (Exception ex)
			//{
			//  GalleryServerPro.ErrorHandler.Error.Record(ex, System.Diagnostics.EventLogEntryType.Information);
			//}

			System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("GalleryServerPro.Business.Wpf");

			// Get reference to static WpfMetadataExtractor.AddWpfBitmapMetadata() method.
			Type[] parmTypes = new Type[2];
			parmTypes[0] = typeof(string);
			parmTypes[1] = typeof(GalleryServerPro.Business.Interfaces.IGalleryObjectMetadataItemCollection);
			Type metadataExtractor = assembly.GetType("GalleryServerPro.Business.Wpf.WpfMetadataExtractor");
			System.Reflection.MethodInfo addMetadataMethod = metadataExtractor.GetMethod("AddWpfBitmapMetadata", parmTypes);

			// Prepare parameters to pass to WpfMetadataExtractor.AddWpfBitmapMetadata() method.
			object[] parameters = new object[2];
			parameters[0] = String.Empty;
			parameters[1] = null;

			try
			{
				addMetadataMethod.Invoke(null, parameters);
				isDotNet3Installed = true;
			}
			catch (System.Reflection.TargetInvocationException ex)
			{
				string msg = GalleryServerPro.Business.Properties.Resources.DotNet_3_Or_Higher_Not_Found_Ex_Msg;
				Exception ex2 = new ErrorHandler.CustomExceptions.BusinessException(msg, ex);
				ErrorHandler.Error.Record(ex2);
				HelperFunctions.PurgeCache();

				GalleryServerPro.ErrorHandler.Error.Record(ex);
			}

			return isDotNet3Installed;
		}

		#endregion
	}
}

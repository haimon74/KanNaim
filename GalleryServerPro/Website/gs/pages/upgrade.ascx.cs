using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;
using MimeType = GalleryServerPro.Business.MimeType;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class upgrade : System.Web.UI.UserControl
	{
		#region Member classes

		/// <summary>
		/// Contains functionality for importing settings from a previous version of web.config to the 
		/// current one. The following data is imported: (1) SQL Server and SQLite connection strings, 
		/// (2) membership, roles, and profile provider names.
		/// </summary>
		private class WebConfigImporter
		{
			#region Private Fields

			private readonly string _sourceConfigPath;
			private string _membershipProvider;
			private string _roleProvider;
			private string _profileProvider;
			private string _sqlServerCnString;
			private string _sqliteCnString;
			private readonly bool _updateRequired;

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="WebConfigImporter"/> class.
			/// </summary>
			/// <param name="sourceConfigPath">The full path to the web.config file containing the source data.
			/// Ex: C:\inetpub\wwwroot\gallery\gs\web_old.config</param>
			public WebConfigImporter(string sourceConfigPath)
			{
				this._sourceConfigPath = sourceConfigPath;

				ExtractSourceConfigData();

				this._updateRequired = IsUpdateRequired();
			}

			#endregion

			#region Public Properties

			public bool UpdateRequired
			{
				get { return this._updateRequired; }
			}

			/// <summary>
			/// Gets the path to the current application's web.config file.
			/// </summary>
			/// <value>The path to the current application's web.config file.</value>
			public string WebConfigPath
			{
				get { return HttpContext.Current.Server.MapPath("~/web.config"); }
			}

			// Properties aren't currently needed, but we can uncomment them if necessary.
			//public string MembershipProvider
			//{
			//  get { return this._membershipProvider; }
			//  set { this._membershipProvider = value; }
			//}

			//public string RoleProvider
			//{
			//  get { return this._roleProvider; }
			//  set { this._roleProvider = value; }
			//}

			//public string ProfileProvider
			//{
			//  get { return this._profileProvider; }
			//  set { this._profileProvider = value; }
			//}

			//public string SqlServerConnectionString
			//{
			//  get { return this._sqlServerCnString; }
			//  set { this._sqlServerCnString = value; }
			//}

			//public string SQLiteConnectionString
			//{
			//  get { return this._sqliteCnString; }
			//  set { this._sqliteCnString = value; }
			//}

			#endregion

			#region Public Methods

			/// <summary>
			/// Update web.config with the connection strings and provider names for membership, roles, and profile.
			/// </summary>
			public void Save()
			{
				if (this._updateRequired)
					SaveToConfigFile();
			}

			/// <summary>
			/// Determines whether the installer has permission to update the web.config file.
			/// </summary>
			/// <returns>Returns <c>true</c> if the installer has permission to update the web.config file; otherwise returns <c>false</c>.</returns>
			/// <remarks>This method also exists in isntall.ascx.</remarks>
			public bool InstallerIsAbleToUpdateWebConfig()
			{
				try
				{
					// When modifying the file using XML techniques, we only care about test 2 below. The other tests are required when using the 
					// .NET 2.0 Configuration API, which have increased permission requirements (such as Full Trust).

					// Test 1: Can we write to the directory containing web.config? This will fail when NTFS permissions prevent it.
					//HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(Server.MapPath(Request.ApplicationPath));

					// Test 2: Can we open and save the web.config file? This will fail when the app is running under Medium Trust and when
					// the web.config file does not have 'write' NTFS permission.
					using (FileStream fs = File.OpenWrite(this.WebConfigPath)) { }

					// Test 3: Do we have 'modify' NTFS permission on web.config? We test this by temporarily renaming web.config.
					//string newWebConfigFilename = Guid.NewGuid().ToString();
					//File.Move(webConfigFullPath, Path.Combine(webConfigPath, newWebConfigFilename));
					//File.Move(Path.Combine(webConfigPath, newWebConfigFilename), webConfigFullPath);

					// We could use the following but it includes all applicable config files (like galleryserverpro.config), and we
					// want to test ONLY web.config.
					//System.Configuration.Configuration configSection = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
					//configSection.Save(ConfigurationSaveMode.Minimal, true);

					return true;
				}
				catch
				{
					return false;
				}
			}

			/// <summary>
			/// Determines whether any settings in the current web.config are different than the source web.config. If they are,
			/// then an update is required and this function returns true.
			/// </summary>
			/// <returns>Returns <c>true</c> if any settings in the current web.config are different than the source web.config;
			/// otherwise returns <c>false</c>.</returns>
			private bool IsUpdateRequired()
			{
				string sqliteCnString, sqlServerCnString, membershipProvider, roleProvider, profileProvider;
				ExtractConfigData(this.WebConfigPath, out sqliteCnString, out sqlServerCnString, out membershipProvider, out roleProvider, out profileProvider);

				// Compare the connection strings and provider names. We use a case-sensitive comparison for the connection strings because
				// they might contain a password, but the provider names are not case-sensitive.
				bool configFilesAreEqual = ((sqliteCnString.Equals(this._sqliteCnString, StringComparison.Ordinal))
					&& (sqlServerCnString.Equals(this._sqlServerCnString, StringComparison.Ordinal))
				&& (membershipProvider.Equals(this._membershipProvider, StringComparison.InvariantCultureIgnoreCase))
				&& (roleProvider.Equals(this._roleProvider, StringComparison.InvariantCultureIgnoreCase))
				&& (profileProvider.Equals(this._profileProvider, StringComparison.InvariantCultureIgnoreCase)));

				bool updateRequired = !configFilesAreEqual;

				return updateRequired;
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Extracts configuration settings from the source web.config file and stores them in member variables.
			/// </summary>
			private void ExtractSourceConfigData()
			{
				string sqliteCnString, sqlServerCnString, membershipProvider, roleProvider, profileProvider;
				ExtractConfigData(this._sourceConfigPath, out sqliteCnString, out sqlServerCnString, out membershipProvider, out roleProvider, out profileProvider);

				this._sqliteCnString = sqliteCnString;
				this._sqlServerCnString = sqlServerCnString;
				this._membershipProvider = membershipProvider;
				this._roleProvider = roleProvider;
				this._profileProvider = profileProvider;
			}

			private static void ExtractConfigData(string configPath, out string sqliteCnString, out string sqlServerCnString,
																		 out string membershipProvider, out string roleProvider, out string profileProvider)
			{
				sqliteCnString = string.Empty;
				sqlServerCnString = string.Empty;
				membershipProvider = string.Empty;
				roleProvider = string.Empty;
				profileProvider = string.Empty;

				using (FileStream fs = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						XmlReader r = XmlReader.Create(sr);
						while (r.Read())
						{
							if (r.Name == "connectionStrings")
							{
								XmlReader cnStrings = r.ReadSubtree();
								while (cnStrings.Read())
								{
									if ((cnStrings.Name == "add") && cnStrings.MoveToAttribute("name"))
									{
										if ((cnStrings.Value == "SQLiteDbConnection") && cnStrings.MoveToAttribute("connectionString"))
											sqliteCnString = cnStrings.Value;

										if ((cnStrings.Value == "SqlServerDbConnection") && cnStrings.MoveToAttribute("connectionString"))
											sqlServerCnString = cnStrings.Value;
									}
								}
							}

							else if ((r.Name == "membership") && r.MoveToAttribute("defaultProvider"))
								membershipProvider = r.Value;

							else if ((r.Name == "roleManager") && r.MoveToAttribute("defaultProvider"))
								roleProvider = r.Value;

							else if ((r.Name == "profile") && r.MoveToAttribute("defaultProvider"))
								profileProvider = r.Value;
						}
					}
				}
			}

			/// <summary>
			/// Update web.config with the connection strings and provider names for membership, roles, and profile.
			/// </summary>
			private void SaveToConfigFile()
			{
				WebConfigEntity webConfigEntity = WebConfigController.GetWebConfigEntity();

				webConfigEntity.SQLiteConnectionStringValue = this._sqliteCnString;
				webConfigEntity.SqlServerConnectionStringValue = this._sqlServerCnString;
				webConfigEntity.MembershipDefaultProvider = this._membershipProvider;
				webConfigEntity.RoleDefaultProvider = this._roleProvider;
				webConfigEntity.ProfileDefaultProvider = this._profileProvider;

				WebConfigController.Save(webConfigEntity);
			}

			///// <summary>
			///// Update web.config with the connection strings and provider names for membership, roles, and profile.
			///// Requires Full Trust
			///// </summary>
			//  private void SaveToConfigFile()
			//  {
			//    System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);

			//    // Step 1: Update connection strings.
			//    foreach (ConnectionStringSettings cnString in config.ConnectionStrings.ConnectionStrings)
			//    {
			//      switch (cnString.Name)
			//      {
			//        case SQL_SERVER_CN_STRING_NAME:
			//          cnString.ConnectionString = this._sqlServerCnString;
			//          break;
			//        case SQLITE_CN_STRING_NAME:
			//          cnString.ConnectionString = this._sqliteCnString;
			//          break;
			//        default:
			//          break; // Ignore other connection strings
			//      }
			//    }

			//    // Step 2: Update membership, role, and profile provider names.
			//    MembershipSection membershipSection = ConfigManager.OpenMembershipConfigSection(config);
			//    RoleManagerSection roleSection = ConfigManager.OpenRoleConfigSection(config);
			//    ProfileSection profileSection = ConfigManager.OpenProfileConfigSection(config);

			//    membershipSection.DefaultProvider = this._membershipProvider;
			//    roleSection.DefaultProvider = this._roleProvider;
			//    profileSection.DefaultProvider = this._profileProvider;

			//    config.Save();
			//  }

			#endregion
		}

		/// <summary>
		/// Contains functionality for importing settings from a previous version of galleryserverpro.config to the 
		/// current one. Only the gallery data provider name and the values in the &lt;core ...&gt; section are imported.
		/// </summary>
		private class GspConfigImporter
		{
			#region Member Classes

			private class MimeTypeEntity
			{
				#region Private Fields

				private readonly string _extension;
				private readonly string _browserId;
				private readonly string _fullMimeType;
				private readonly string _browserMimeType;
				private readonly bool _allowAddToGallery;

				#endregion

				#region Constructors

				/// <summary>
				/// Initializes a new instance of the <see cref="MimeType"/> class.
				/// </summary>
				/// <param name="extension">A string representing the file's extension, including the period (e.g. ".jpg", ".avi").
				/// It is not case sensitive.</param>
				/// <param name="fullMimeType">The full mime type (e.g. image/jpeg, video/quicktime).</param>
				/// <param name="browserId">The id of the browser for the default browser as specified in the .Net Framework's browser definition file. 
				/// This should always be the string "default", which means it will match all browsers. Once this instance is created, additional
				/// values that specify more specific browsers or browser families can be added to the private _browserMimeTypes member variable.</param>
				/// <param name="browserMimeType">The MIME type that can be understood by the browser for displaying this media object. The value will be applied
				/// to the browser specified in <paramref name="browserId"/>. Specify null or <see cref="String.Empty" /> if the MIME type appropriate for the 
				/// browser is the same as <paramref name="fullMimeType"/>.</param>
				/// <param name="allowAddToGallery">Indicates whether a file having this MIME type can be added to Gallery Server Pro.</param>
				public MimeTypeEntity(string extension, string fullMimeType, string browserId, string browserMimeType, bool allowAddToGallery)
				{
					this._extension = extension;
					this._browserId = browserId;
					this._fullMimeType = fullMimeType;
					this._browserMimeType = browserMimeType;
					this._allowAddToGallery = allowAddToGallery;
				}

				#endregion

				#region Properties

				/// <summary>
				/// Gets the file extension this mime type is associated with.
				/// </summary>
				/// <value>The file extension this mime type is associated with.</value>
				public string Extension
				{
					get
					{
						return this._extension;
					}
				}

				/// <summary>
				/// Gets the id of the browser for which the <see cref="BrowserMimeType" /> property applies.
				/// </summary>
				/// <value>
				/// The id of the browser for which the <see cref="BrowserMimeType" /> property applies.
				/// </value>
				public string BrowserId
				{
					get
					{
						return this._browserId;
					}
				}

				/// <summary>
				/// Gets the full mime type (e.g. image/jpeg, video/quicktime).
				/// </summary>
				/// <value>The full mime type.</value>
				public string FullMimeType
				{
					get
					{
						return this._fullMimeType;
					}
				}

				/// <summary>
				/// Gets the MIME type that can be understood by the browser for displaying this media object.
				/// </summary>
				/// <value>
				/// The MIME type that can be understood by the browser for displaying this media object.
				/// </value>
				public string BrowserMimeType
				{
					get
					{
						return this._browserMimeType;
					}
				}

				/// <summary>
				/// Gets a value indicating whether objects of this MIME type can be added to Gallery Server Pro.
				/// </summary>
				/// <value>
				/// 	<c>true</c> if objects of this MIME type can be added to Gallery Server Pro; otherwise, <c>false</c>.
				/// </value>
				public bool AllowAddToGallery
				{
					get
					{
						return this._allowAddToGallery;
					}
				}

				#endregion

			}

			#endregion

			#region Private Fields

			private readonly string _sourceConfigPath;
			private string _galleryDataProvider;
			private readonly Dictionary<string, string> _coreValues = new Dictionary<string, string>();
			private readonly IList<MimeTypeEntity> _mimeTypes = new List<MimeTypeEntity>();
			private static readonly List<String> _deletedCoreAttributes = new List<String>();

			#endregion

			#region Constructors

			static GspConfigImporter()
			{
				_deletedCoreAttributes.Add("websiteTitle");
				_deletedCoreAttributes.Add("defaultAlbumDirectoryName");
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="GspConfigImporter"/> class.
			/// </summary>
			/// <param name="sourceConfigPath">The full path to the galleryserverpro.config file containing the source data.
			/// Ex: C:\inetpub\wwwroot\gallery\gs\config\galleryserverpro_old.config</param>
			public GspConfigImporter(string sourceConfigPath)
			{
				this._sourceConfigPath = sourceConfigPath;

				ExtractConfigData();
			}

			#endregion

			#region Public Methods

			/// <summary>
			/// Update galleryserverpro.config with the gallery data provider and the &lt;core ...&gt; attributes stored in _coreValues.
			/// </summary>
			public void Save()
			{
				SaveToConfigFile();
			}

			/// <summary>
			/// Determines whether the installer has permission to update the galleryserverpro.config file.
			/// </summary>
			/// <returns>Returns <c>true</c> if the installer has permission to update the galleryserverpro.config file; otherwise returns <c>false</c>.</returns>
			/// <remarks>This method also exists in install.ascx.</remarks>
			public static bool InstallerIsAbleToUpdateGalleryServerProConfig()
			{
				try
				{
					// When modifying the file using XML techniques, we only care about test 2 below. The other tests are required when using the 
					// .NET 2.0 Configuration API, which have increased permission requirements (such as Full Trust).

					// Test 1: Can we write to the directory containing galleryserverpro.config? This will fail when NTFS permissions prevent it.
					//HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(galleryServerProConfigPath);

					// Test 2: Can we open and save the galleryserverpro.config file? This will fail when the app is running under Medium Trust and when
					// the galleryserverpro.config file does not have 'write' NTFS permission.
					using (FileStream fs = File.OpenWrite(Util.GalleryServerProConfigFilePath)) { }

					// Test 3: Do we have 'modify' NTFS permission on galleryserverpro.config? We test this by temporarily renaming galleryserverpro.config.
					//string newGalleryServerProConfigFilename = Guid.NewGuid().ToString();
					//File.Move(galleryServerProConfigFullPath, Path.Combine(galleryServerProConfigPath, newGalleryServerProConfigFilename));
					//File.Move(Path.Combine(galleryServerProConfigPath, newGalleryServerProConfigFilename), galleryServerProConfigFullPath);

					return true;
				}
				catch
				{
					return false;
				}
			}
			
			#endregion

			#region Private Methods


			/// <summary>
			/// Extracts configuration settings from the source galleryserverpro.config file and stores them in member variables.
			/// </summary>
			private void ExtractConfigData()
			{
				using (FileStream fs = new FileStream(this._sourceConfigPath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						XmlReader r = XmlReader.Create(sr);
						while (r.Read())
						{
							if (r.Name == "core")
							{
								// Get core attributes.
								while (r.MoveToNextAttribute())
								{
									if (!_deletedCoreAttributes.Contains(r.Name))
									{
										this._coreValues.Add(r.Name, r.Value);
									}
								}
							}

							else if (r.Name == "mimeTypes")
							{
								// Get mime types.
								XmlReader mimeTypes = r.ReadSubtree();

								while (mimeTypes.Read())
								{
									if (mimeTypes.Name == "mimeType")
									{
										// Get fileExtension
										if (!mimeTypes.MoveToAttribute("fileExtension"))
											throw new WebException(String.Format("Could not find fileExtension attribute in mimeType element of {0}.", _sourceConfigPath));

										string fileExtension = mimeTypes.Value;

										// Get browserId
										if (!mimeTypes.MoveToAttribute("browserId"))
											throw new WebException(String.Format("Could not find browserId attribute in mimeType element of {0}. fileExtension={1}", _sourceConfigPath, fileExtension));

										string browserId = mimeTypes.Value;

										// Get type
										if (!mimeTypes.MoveToAttribute("type"))
											throw new WebException(String.Format("Could not find type attribute in mimeType element of {0}. fileExtension={1}", _sourceConfigPath, fileExtension));

										string type = mimeTypes.Value;

										// Get browserMimeType. It is optional.
										string browserMimeType = String.Empty;
										if (mimeTypes.MoveToAttribute("browserMimeType"))
											browserMimeType = mimeTypes.Value;

										// Get allowAddToGallery
										if (!mimeTypes.MoveToAttribute("allowAddToGallery"))
											throw new WebException(String.Format("Could not find allowAddToGallery attribute in mimeType element of {0}. fileExtension={1}", _sourceConfigPath, fileExtension));

										bool allowAddToGallery = Convert.ToBoolean(mimeTypes.Value);

										_mimeTypes.Add(new MimeTypeEntity(fileExtension, type, browserId, browserMimeType, allowAddToGallery));
									}
								}
							}

							else if ((r.Name == "dataProvider") && r.MoveToAttribute("defaultProvider"))
							{
								// Get gallery data provider
								this._galleryDataProvider = r.Value;
							}
						}
					}
				}
			}

			/// <summary>
			/// Update galleryserverpro.config with the gallery data provider, the &lt;core ...&gt; attributes stored in _coreValues,
			/// and the list of enabled MIME types.
			/// </summary>
			private void SaveToConfigFile()
			{
				XmlDocument xmlDoc = GspConfigController.LoadGalleryServerProConfigFromDisk();

				this.UpdateCoreAttributesInConfigFile(xmlDoc);

				this.UpdateMimeTypesInConfigFile(xmlDoc);

				this.UpdateDataProviderInConfigFile(xmlDoc);

				GspConfigController.SaveGalleryServerProConfigToDisk(xmlDoc);

				// Full Trust technique:
				//System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
				//GalleryServerProConfigSettings configSettings = ConfigManager.OpenGalleryServerProConfigSection(config);

				//UpdateCoreAttributesInConfigFile(configSettings);

				//UpdateMimeTypesInConfigFile(configSettings);

				//UpdateDataProviderInConfigFile(configSettings);

				//config.Save();
			}

			private void UpdateCoreAttributesInConfigFile(XmlNode gspConfig)
			{
				// Update the attributes of the <core ...> element with the values from the source config file.
				XmlElement core = (XmlElement)gspConfig.SelectSingleNode(@"galleryServerPro/core");

				// Loop through each attribute from the source and assign the value to the current .config file. Ignore any failures
				// (perhaps the setting was removed, renamed, or its data type changed).
				foreach (KeyValuePair<string, string> coreSource in this._coreValues)
				{
					core.SetAttribute(coreSource.Key, coreSource.Value);
				}
			}

			/// <summary>
			/// Updates the allowAddToGallery attribute of MIME types in config file with the data from the source config file. 
			/// Other attributes, such as browserMimeType and type, are left alone because they might have been updated in the
			/// new release. If a MIME type is present in the source but not the destination, it is not imported. If a MIME type is in
			/// the destination but not the source it is not modified.
			/// </summary>
			/// <param name="gspConfig">The galleryserverpro.config file.</param>
			private void UpdateMimeTypesInConfigFile(XmlNode gspConfig)
			{
				// Loop through each source MIME type.
				foreach (MimeTypeEntity mimeTypeSource in this._mimeTypes)
				{
					// Try to find the matching MIME type in the config file. Each mimeType element is uniquely identified by the 
					// fileExtension and browserIf attributes.
					XmlElement mimeType = (XmlElement)gspConfig.SelectSingleNode(String.Format(@"galleryServerPro/galleryObject/mimeTypes/mimeType[@fileExtension=""{0}"" and @browserId=""{1}""]", mimeTypeSource.Extension, mimeTypeSource.BrowserId));

					if (mimeType != null)
					{
						// Found it.
						mimeType.SetAttribute("allowAddToGallery", mimeTypeSource.AllowAddToGallery.ToString().ToLower());

						// We could update the remaining properties, but we choose not to because they might have been updated in the release.
						//if (!String.IsNullOrEmpty(mimeTypeSource.BrowserMimeType))
						//	mimeType.SetAttribute("browserMimeType", mimeTypeSource.BrowserMimeType); // Optional attribute

						//mimeType.SetAttribute("type", mimeTypeSource.FullMimeType);
					}
				}
			}

			private void UpdateDataProviderInConfigFile(XmlNode gspConfig)
			{
				// Update the gallery data provider.
				XmlElement core = (XmlElement)gspConfig.SelectSingleNode(@"galleryServerPro/dataProvider");

				core.SetAttribute("defaultProvider", this._galleryDataProvider);
			}

			#region Full Trust Technique for updating config file

			///// <summary>
			///// Updates the allowAddToGallery attribute of MIME types in config file with the data from the source config file. 
			///// Other attributes, such as browserMimeType and type, are left alone because they might have been updated in the
			///// new release. If a MIME type is present in the source but not the destination, it is not imported. If a MIME type is in
			///// the destination but not the source it is not modified.
			///// </summary>
			///// <param name="configSettings">The galleryServerPro custom configuration section in galleryserverpro.config.</param>
			///// <remarks>Only works in Full Trust!</remarks>
			//private void UpdateMimeTypesInConfigFile(GalleryServerProConfigSettings configSettings)
			//{
			//  MimeTypeCollection mimeTypes = configSettings.GalleryObject.MimeTypes;

			//  // Loop through each source MIME type.
			//  foreach (MimeTypeEntity mimeTypeSource in this._mimeTypes)
			//  {
			//    // Try to find the matching MIME type in the config file. The key is defined as a concatenation of the file
			//    // extension and the browser ID.
			//    Configuration.MimeType mimeType = mimeTypes[String.Concat(mimeTypeSource.Extension, "|", mimeTypeSource.BrowserId)];

			//    if (mimeType != null)
			//    {
			//      // Found it.
			//      mimeType.AllowAddToGallery = mimeTypeSource.AllowAddToGallery;

			//      // We could update the remaining properties, but we choose not to because they might have been updated in the release.
			//      //if (!String.IsNullOrEmpty(mimeTypeSource.BrowserMimeType))
			//      //  mimeType.BrowserMimeType = mimeTypeSource.BrowserMimeType; // Optional attribute

			//      //mimeType.Type = mimeTypeSource.FullMimeType;
			//    }
			//    else
			//    {
			//      // Mime type exists in source but not destination. We could add it but let's not because it might have been
			//      // intentionally deleted in the new release.
			//      //Configuration.MimeType newMimeType = new Configuration.MimeType();
			//      //newMimeType.FileExtension = mimeTypeSource.Extension;
			//      //newMimeType.BrowserId = mimeTypeSource.BrowserId;
			//      //newMimeType.Type = mimeTypeSource.FullMimeType;

			//      //if (!String.IsNullOrEmpty(mimeTypeSource.BrowserMimeType))
			//      //  newMimeType.BrowserMimeType = mimeTypeSource.BrowserMimeType;

			//      //newMimeType.AllowAddToGallery = mimeTypeSource.AllowAddToGallery;
			//      //mimeTypes.Add(newMimeType);
			//    }
			//  }
			//}

			// Only works in Full Trust!
			//private void UpdateCoreAttributesInConfigFile(GalleryServerProConfigSettings configSettings)
			//{
			//  Core core = configSettings.Core;

			//  // Loop through each attribute from the source and assign the value to the current .config file. Ignore any failures
			//  // (perhaps the setting was removed, renamed, or its data type changed).
			//  foreach (KeyValuePair<string, string> coreSource in this._coreValues)
			//  {
			//    core.TrySet(coreSource.Key, coreSource.Value);
			//  }
			//}

			// Only works in Full Trust!
			//private void UpdateDataProviderInConfigFile(GalleryServerProConfigSettings configSettings)
			//{
			//  // Update the gallery data provider.
			//  configSettings.DataProvider.DefaultProvider = this._galleryDataProvider;
			//}

			#endregion

			#endregion
		}

		#endregion

		#region Enum declarations

		public enum UpgradeWizardPanel
		{
			Welcome,
			PrepareConfigFiles,
			ReadyToUpgrade,
			Finished,
		}

		#endregion

		#region Private Fields

		private bool _webConfigSuccessfullyUpdated;
		private bool _webConfigUpdateRequired;
		private string _webConfigUpdateErrorMsg;
		private string _webConfigUpdateErrorCallStack;
		private bool _gspConfigSuccessfullyUpdated;
		private string _gspConfigUpdateErrorMsg;
		private string _gspConfigUpdateErrorCallStack;

		#endregion

		#region Public Properties

		public UpgradeWizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (UpgradeWizardPanel)ViewState["WizardPanel"];

				return UpgradeWizardPanel.Welcome;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}

		public string WebConfigSourcePath
		{
			get
			{
				return Server.MapPath("~/web.config").Replace("web.config", "web_old.config");
			}
		}

		public string WebConfigDestinationPath
		{
			get
			{
				return Server.MapPath("~/web.config");
			}
		}

		public string GspConfigSourcePath
		{
			get
			{
				return Server.MapPath(Util.GetUrl("/config/galleryserverpro.config")).Replace("galleryserverpro.config", "galleryserverpro_old.config");
			}
		}

		public string GspConfigDestinationPath
		{
			get
			{
				return Server.MapPath(Util.GetUrl("/config/galleryserverpro.config"));
			}
		}

		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			bool setupEnabled;
			if (Boolean.TryParse(ENABLE_SETUP.Value, out setupEnabled) && setupEnabled)
			{
				if (!Page.IsPostBack)
				{
					SetCurrentPanel(UpgradeWizardPanel.Welcome, Welcome);
				}

				ConfigureControls();
			}
			else
			{
				Response.Write(String.Format(CultureInfo.CurrentCulture, "<h1>{0}</h1>", Resources.GalleryServerPro.Installer_Disabled_Msg));
				Response.Flush();
				Response.End();
			}
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
				ShowNextPanel();
		}

		protected void btnPrevious_Click(object sender, EventArgs e)
		{
			ShowPreviousPanel();
		}

		protected void lbTryAgain_Click(object sender, EventArgs e)
		{
			ConfigureReadyToUpgradeControls();
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			if (!IsPostBack)
				ConfigureControlsFirstTime();

			Page.Form.DefaultFocus = btnNext.ClientID;
		}

		private void ConfigureControlsFirstTime()
		{
			string version = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Gsp_Version_Text, Util.GetGalleryServerVersion());
			litVersion.Text = version;
		}

		private void SetCurrentPanel(UpgradeWizardPanel panel, Control controlToShow)
		{
			Panel currentPanel = wizCtnt.FindControl(CurrentWizardPanel.ToString()) as Panel;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case UpgradeWizardPanel.Welcome:
					btnPrevious.Enabled = false;
					break;
				case UpgradeWizardPanel.Finished:
					btnNext.Enabled = false;
					break;
				default:
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					break;
			}

			controlToShow.Visible = true;
			CurrentWizardPanel = panel;
		}

		private void ShowNextPanel()
		{
			switch (this.CurrentWizardPanel)
			{
				case UpgradeWizardPanel.Welcome:
					{
						SetCurrentPanel(UpgradeWizardPanel.PrepareConfigFiles, PrepareConfigFiles);
						break;
					}
				case UpgradeWizardPanel.PrepareConfigFiles:
					{
						SetCurrentPanel(UpgradeWizardPanel.ReadyToUpgrade, ReadyToUpgrade);
						ConfigureReadyToUpgradeControls();
						break;
					}
				case UpgradeWizardPanel.ReadyToUpgrade:
					{
						ImportConfigSettings();

						SetCurrentPanel(UpgradeWizardPanel.Finished, Finished);

						ConfigureFinishedControls();
					}
					break;
			}
		}

		/// <summary>
		/// Configures the controls on the Finished step of the wizard. This appears after the upgrade is complete. If an
		/// error occurred, show the error message and call stack.
		/// </summary>
		private void ConfigureFinishedControls() 
		{
			if (this._webConfigSuccessfullyUpdated && this._gspConfigSuccessfullyUpdated)
			{
				// No errors! Yippee!
				this.imgFinishedIcon.ImageUrl = Util.GetUrl("/images/ok_26x26.png");
				this.imgFinishedIcon.Width = Unit.Pixel(26);
				this.imgFinishedIcon.Height = Unit.Pixel(26);
				this.l61.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_No_Addl_Steps_Reqd, Util.GetCurrentPageUrl());
			}
			else
			{
				// One or both config files were not updated.
				this.imgFinishedIcon.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
				this.imgFinishedIcon.Width = Unit.Pixel(32);
				this.imgFinishedIcon.Height = Unit.Pixel(32);
				this.l61.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_Addl_Steps_Reqd, Util.GetCurrentPageUrl());
			}

			// web.config related controls
			if (this._webConfigSuccessfullyUpdated)
			{
				this.imgFinishedWebConfigStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
				this.imgFinishedWebConfigStatus.Width = Unit.Pixel(13);
				this.imgFinishedWebConfigStatus.Height = Unit.Pixel(12);

				if (this._webConfigUpdateRequired)
				{
					this.lblFinishedWebConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_Config_OK_Msg, this.WebConfigDestinationPath);
					this.lblFinishedWebConfigStatus.CssClass = "gsp_msgfriendly";
				}
				else
				{
					this.lblFinishedWebConfigStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Config_Status_No_Import_Msg;
					this.lblFinishedWebConfigStatus.CssClass = "gsp_msgfriendly";
				}
			}
			else
			{
				this.imgFinishedWebConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				this.imgFinishedWebConfigStatus.Width = Unit.Pixel(16);
				this.imgFinishedWebConfigStatus.Height = Unit.Pixel(16);
				this.lblFinishedWebConfigStatus.Text = this._webConfigUpdateErrorMsg;
				this.lblFinishedWebConfigStatus.CssClass = "gsp_msgattention";

				this.lblFinishedWebConfigCallStack.Text = this._webConfigUpdateErrorCallStack;
			}

			// galleryserverpro.config related controls
			if (this._gspConfigSuccessfullyUpdated)
			{
				this.imgFinishedGspConfigStatus.ImageUrl = Util.GetUrl("/images/green_check_13x12.png");
				this.imgFinishedGspConfigStatus.Width = Unit.Pixel(13);
				this.imgFinishedGspConfigStatus.Height = Unit.Pixel(12);
				this.lblFinishedGspConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_Finished_Config_OK_Msg, this.GspConfigDestinationPath);
				this.lblFinishedGspConfigStatus.CssClass = "gsp_msgfriendly";
			}
			else
			{
				this.imgFinishedGspConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				this.imgFinishedGspConfigStatus.Width = Unit.Pixel(16);
				this.imgFinishedGspConfigStatus.Height = Unit.Pixel(16);
				this.lblFinishedGspConfigStatus.Text = this._gspConfigUpdateErrorMsg;
				this.lblFinishedGspConfigStatus.CssClass = "gsp_msgattention";

				this.lblFinishedGspConfigCallStack.Text = this._gspConfigUpdateErrorCallStack;
			}
		}

		private void ConfigureReadyToUpgradeControls()
		{
			btnNext.Text = Resources.GalleryServerPro.Installer_Finish_Button_Text;

			#region web.config

			// Check permissions on web.config
			WebConfigImporter webCfg = null;
			try
			{
				webCfg = new WebConfigImporter(this.WebConfigSourcePath);
			}
			catch (FileNotFoundException ex)
			{
				lblReadyToUpgradeWebConfigStatus.Text = ex.Message;
				lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgwarning";
				imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
			}

			if (webCfg != null)
			{
				if (webCfg.UpdateRequired)
				{
					if (webCfg.InstallerIsAbleToUpdateWebConfig())
					{
						// Web.config has different settings than the source file, so an update is needed. And we have the necessary write
						// permission to update the file, so we are good to go!
						lblReadyToUpgradeWebConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Config_Status_OK_Msg, WebConfigSourcePath);
						lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgfriendly";
						imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/ok_16x16.png");
					}
					else
					{
						// Web.config file needs updating, but installer doesn't have the required write permission.
						lblReadyToUpgradeWebConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Config_Status_No_Perm_Msg, WebConfigDestinationPath);
						lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgwarning";
						imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
					}
				}
				else
				{
					// web.config has same settings as the source web.config. No update needed.
					lblReadyToUpgradeWebConfigStatus.Text = Resources.GalleryServerPro.Installer_Upgrade_Config_Status_No_Import_Msg;
					lblReadyToUpgradeWebConfigStatus.CssClass = "gsp_msgfriendly";
					imgReadyToUpgradeWebConfigStatus.ImageUrl = Util.GetUrl("/images/ok_16x16.png");
				}
			}

			#endregion

			#region galleryserverpro.config

			// Check permissions on galleryserverpro.config
			GspConfigImporter gspCfg = null;
			try
			{
				gspCfg = new GspConfigImporter(this.GspConfigSourcePath);
			}
			catch (FileNotFoundException ex)
			{
				lblReadyToUpgradeGspConfigStatus.Text = ex.Message;
				lblReadyToUpgradeGspConfigStatus.CssClass = "gsp_msgwarning";
				imgReadyToUpgradeGspConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
			}

			if (gspCfg != null)
			{
				if (GspConfigImporter.InstallerIsAbleToUpdateGalleryServerProConfig())
				{
					lblReadyToUpgradeGspConfigStatus.Text = String.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Config_Status_OK_Msg, GspConfigSourcePath);
					lblReadyToUpgradeGspConfigStatus.CssClass = "gsp_msgfriendly";
					imgReadyToUpgradeGspConfigStatus.ImageUrl = Util.GetUrl("/images/ok_16x16.png");
				}
				else
				{
					lblReadyToUpgradeGspConfigStatus.Text = string.Format(Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Config_Status_No_Perm_Msg, GspConfigDestinationPath);
					lblReadyToUpgradeGspConfigStatus.CssClass = "gsp_msgwarning";
					imgReadyToUpgradeGspConfigStatus.ImageUrl = Util.GetUrl("/images/error_16x16.png");
				}
			}

			#endregion

			// Update various labels, images, and buttons as needed
			if ((webCfg == null) || (gspCfg == null))
			{
				// A FileNotFoundException exception must have been thrown for one or both files.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Cannot_Upgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_File_Missing_Dtl1;
				lbTryAgain.Visible = true;
				btnNext.Enabled = false;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(32);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(32);
			}
			else if (!webCfg.UpdateRequired && GspConfigImporter.InstallerIsAbleToUpdateGalleryServerProConfig())
			{
				// Only galleryserverpro.config must be updated, and we have permission to do so.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_OK_Dtl1;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/ok_26x26.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(26);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(26);
			}
			else if (webCfg.UpdateRequired && !webCfg.InstallerIsAbleToUpdateWebConfig() && !GspConfigImporter.InstallerIsAbleToUpdateGalleryServerProConfig())
			{
				// Both must be updated, but we don't have permission for either.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Cannot_Upgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_No_Perm_Dtl1;
				lbTryAgain.Visible = true;
				btnNext.Enabled = false;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(32);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(32);
			}
			else if ((webCfg.UpdateRequired && !webCfg.InstallerIsAbleToUpdateWebConfig() && GspConfigImporter.InstallerIsAbleToUpdateGalleryServerProConfig()) // web.config no perm, gsp.config OK
				|| (!webCfg.UpdateRequired || (webCfg.UpdateRequired && webCfg.InstallerIsAbleToUpdateWebConfig()) && !GspConfigImporter.InstallerIsAbleToUpdateGalleryServerProConfig())) // web.config OK, gsp.config no perm
			{
				// We have permission for one of the files, but not the other.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Missing_Perm_Dtl1;
				lblReadyToUpgradeDetail2Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Missing_Perm_Dtl2;
				lbTryAgain.Visible = true;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(32);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(32);
			}
			else
			{
				// If we get here one of the following must be true:
				// 1. Both files must be updated and installer has permission for both.
				// 2. Only galleryservepro.config must be updated, but we have permission to do so.
				lblReadyToUpgradeHdrMsg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_Hdr;
				lblReadyToUpgradeDetail1Msg.Text = Resources.GalleryServerPro.Installer_Upgrade_ReadyToUpgrade_OK_Dtl1;
				imgReadyToUpgradeStatus.ImageUrl = Util.GetUrl("/images/ok_26x26.png");
				imgReadyToUpgradeStatus.Width = Unit.Pixel(26);
				imgReadyToUpgradeStatus.Height = Unit.Pixel(26);
			}
		}

		private void ImportConfigSettings()
		{
			try
			{
				WebConfigImporter configImporter = new WebConfigImporter(this.WebConfigSourcePath);
				this._webConfigUpdateRequired = configImporter.UpdateRequired;
				configImporter.Save();
				_webConfigSuccessfullyUpdated = true;
			}
			catch (Exception ex)
			{
				this._webConfigUpdateErrorMsg = String.Format("{0}: {1}", ex.GetType(), ex.Message);
				this._webConfigUpdateErrorCallStack = ex.StackTrace;
			}

			try
			{
				GspConfigImporter gspConfigImporter = new GspConfigImporter(this.GspConfigSourcePath);
				gspConfigImporter.Save();
				_gspConfigSuccessfullyUpdated = true;
			}
			catch (Exception ex)
			{
				this._gspConfigUpdateErrorMsg = String.Format("{0}: {1}", ex.GetType(), ex.Message);
				this._gspConfigUpdateErrorCallStack = ex.StackTrace;
			}
		}

		//private void ShowErrorMsgThatOccurredDuringInstallation(string errorMsg, string callStack, HtmlContainerControl errorMsgControl, HtmlContainerControl errorCallStackControl)
		//{
		//  #region Show error message

		//  if (errorMsgControl != null)
		//  {
		//    if (!String.IsNullOrEmpty(errorMsg))
		//    {
		//      errorMsgControl.InnerHtml = errorMsg;
		//      errorMsgControl.Attributes["class"] = "gsp_msgwarning gsp_visible";
		//    }
		//    else
		//    {
		//      errorMsgControl.InnerHtml = String.Empty;
		//      errorMsgControl.Attributes["class"] = "gsp_invisible";
		//    }
		//  }

		//  #endregion

		//  #region Show callstack

		//  if (errorCallStackControl != null)
		//  {
		//    if (!String.IsNullOrEmpty(callStack))
		//    {
		//      errorCallStackControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='gsp_bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Sql_Error_CallStack_Label, callStack);
		//      errorCallStackControl.Attributes["class"] = "gsp_visible";
		//    }
		//    else
		//    {
		//      errorCallStackControl.InnerHtml = String.Empty;
		//      errorCallStackControl.Attributes["class"] = "gsp_invisible";
		//    }
		//  }

		//  #endregion

		//  //if (errorMsgControl.ID == lblErrMsgReadyToUpgrade.ID)
		//  //{
		//  //  lblReadyToUpgradeHeaderMsg.InnerText = Resources.GalleryServerPro.Installer_Install_Error_Hdr;
		//  //  pReadyToUpgradeDetail1Msg.Attributes["class"] = "gsp_invisible";
		//  //}
		//}

		private void ShowPreviousPanel()
		{
			switch (this.CurrentWizardPanel)
			{
				case UpgradeWizardPanel.Welcome: break;
				case UpgradeWizardPanel.PrepareConfigFiles:
					{
						SetCurrentPanel(UpgradeWizardPanel.Welcome, Welcome);
						break;
					}

				case UpgradeWizardPanel.ReadyToUpgrade:
					{
						SetCurrentPanel(UpgradeWizardPanel.PrepareConfigFiles, PrepareConfigFiles);
						break;
					}
				case UpgradeWizardPanel.Finished:
					{
						SetCurrentPanel(UpgradeWizardPanel.ReadyToUpgrade, ReadyToUpgrade);
						break;
					}
			}
		}

		#endregion
	}
}
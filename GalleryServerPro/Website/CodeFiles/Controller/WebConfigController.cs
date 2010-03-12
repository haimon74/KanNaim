using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for interacting with the web.config configuration file, including write operations.
	/// All write operations work in Medium Trust as long as the IIS process identity has write NTFS permissions on the file.
	/// </summary>
	public static class WebConfigController
	{
		#region Private Fields

		private static readonly string _webConfigPath = HttpContext.Current.Server.MapPath("~/web.config");

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Gets an instance of <see cref="WebConfigEntity"/> that contains commonly referenced settings from web.config. The 
		/// entity can be updated with new values and then passed to the <see cref="Save"/> method for persisting back to the file system.
		/// </summary>
		/// <returns>Returns an instance of <see cref="WebConfigEntity"/> that contains commonly referenced settings from web.config.</returns>
		public static WebConfigEntity GetWebConfigEntity()
		{
			WebConfigEntity wce = new WebConfigEntity();

			using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/web.config"), FileMode.Open, FileAccess.Read, FileShare.Read))
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
									if ((cnStrings.Value == Constants.SQLITE_CN_STRING_NAME) && cnStrings.MoveToAttribute("connectionString"))
									{
										wce.SQLiteConnectionStringValue = cnStrings.Value;
									}

									if ((cnStrings.Value == Constants.SQL_SERVER_CN_STRING_NAME) && cnStrings.MoveToAttribute("connectionString"))
									{
										wce.SqlServerConnectionStringValue = cnStrings.Value;
									}
								}
							}
						}

						if ((r.Name == "membership") && r.MoveToAttribute("defaultProvider"))
							wce.MembershipDefaultProvider = r.Value;

						if ((r.Name == "roleManager") && r.MoveToAttribute("defaultProvider"))
							wce.RoleDefaultProvider = r.Value;

						if ((r.Name == "profile") && r.MoveToAttribute("defaultProvider"))
							wce.ProfileDefaultProvider = r.Value;
					}
				}
			}

			return wce;
		}

		/// <summary>
		/// Persist the configuration data to web.config.
		/// </summary>
		/// <param name="webConfigEntity">An instance of <see cref="WebConfigEntity"/> that contains data to save to web.config.</param>
		/// <exception cref="UnauthorizedAccessException">Thrown when the IIS application pool identity does not have
		/// write access to web.config.</exception>
		public static void Save(WebConfigEntity webConfigEntity)
		{
			#region Load web.config and configure namespace

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(_webConfigPath);

			if (xmlDoc == null)
				throw new WebException(string.Format("Could not load {0}.", _webConfigPath));

			if (xmlDoc.DocumentElement == null)
				throw new WebException(string.Format("Could not find the root element of {0}.", _webConfigPath));

			// If the root element has a namespace, save it to a temporary variable and then set it to an empty string. 
			// This will allow us to locate nodes without having to specify a namespace in the xpath. Normally there shouldn't 
			// be a namespace on the <configuration> element of web.config, but versions of the ASP.NET Configuration Tool 
			// before VS 2008 incorrectly added the following: xmlns="http://schemas.microsoft.com/.NetConfiguration/v2.0"
			// We'll add it back before saving it so there isn't any change to the file as stored on disk.
			string ns = String.Empty;
			if (xmlDoc.DocumentElement.HasAttribute("xmlns"))
			{
				ns = xmlDoc.DocumentElement.Attributes["xmlns"].Value;
				xmlDoc.DocumentElement.Attributes["xmlns"].Value = "";
				xmlDoc.LoadXml(xmlDoc.DocumentElement.OuterXml);
			}

			#endregion

			#region Update connection strings

			// Update SQLite and SQL Server connection strings.
			WriteConnectionStringToWebConfig(xmlDoc, WebConfigEntity.SQLiteConnectionStringName, webConfigEntity.SQLiteConnectionStringValue);
			WriteConnectionStringToWebConfig(xmlDoc, WebConfigEntity.SqlServerConnectionStringName, webConfigEntity.SqlServerConnectionStringValue);

			#endregion

			#region Update membership, role and profile

			// Update membership
			XmlNode membershipNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/membership");

			if (membershipNode == null)
				throw new WebException("Could not find the membership section in web.config.");

			membershipNode.Attributes["defaultProvider"].Value = webConfigEntity.MembershipDefaultProvider;

			// Update roles
			XmlNode roleNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/roleManager");

			if (roleNode == null)
				throw new WebException("Could not find the roleManager section in web.config.");

			roleNode.Attributes["defaultProvider"].Value = webConfigEntity.RoleDefaultProvider;

			// Update profile
			XmlNode profileNode = xmlDoc.SelectSingleNode(@"/configuration/system.web/profile");

			if (profileNode == null)
				throw new WebException("Could not find the profile section in web.config.");

			profileNode.Attributes["defaultProvider"].Value = webConfigEntity.ProfileDefaultProvider;

			#endregion

			#region Save to disk

			// If the config file had a root namespace, restore it now.
			if (!String.IsNullOrEmpty(ns))
				xmlDoc.DocumentElement.Attributes["xmlns"].Value = ns;

			// Persist changes to disk.
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.Indent = true;
			xws.Encoding = new UTF8Encoding(false);

			XmlWriter writer = XmlWriter.Create(_webConfigPath, xws);

			xmlDoc.Save(writer);

			#endregion
		}

		#endregion

		#region Private Static Methods

		private static void WriteConnectionStringToWebConfig(XmlDocument xmlDoc, string cnName, string cnValue)
		{
			if (String.IsNullOrEmpty(cnValue))
				return;

			XmlNode cnStringNode = xmlDoc.SelectSingleNode(String.Format(@"/configuration/connectionStrings/add[@name=""{0}""]", cnName));

			if (cnStringNode != null)
			{
				// Update connection string.
				cnStringNode.Attributes["connectionString"].Value = cnValue;
			}
			else
			{
				// Add connection string.
				cnStringNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", null);

				XmlAttribute nameAttribute = xmlDoc.CreateAttribute("name");
				nameAttribute.Value = cnName;
				cnStringNode.Attributes.SetNamedItem(nameAttribute);

				XmlAttribute cnStringAttribute = xmlDoc.CreateAttribute("connectionString");
				cnStringAttribute.Value = cnValue;
				cnStringNode.Attributes.SetNamedItem(cnStringAttribute);

				XmlElement cnStringsElement = (XmlElement)xmlDoc.SelectSingleNode(@"/configuration/connectionStrings");

				if (cnStringsElement == null)
					throw new WebException("Could not find the connectionStrings section in web.config.");

				cnStringsElement.AppendChild(cnStringNode);
			}
		}

		#endregion
	}
}

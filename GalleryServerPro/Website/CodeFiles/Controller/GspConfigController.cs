using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for interacting with the galleryserverpro.config configuration file, including write operations.
	/// All write operations work in Medium Trust as long as the IIS process identity has write NTFS permissions on the file.
	/// </summary>
	public static class GspConfigController
	{
		#region Public Static Methods

		/// <summary>
		/// Gets an instance of <see cref="GspCoreEntity"/> that contains data from the &lt;core .../&gt; section of 
		/// galleryserverpro.config. The entity can be updated with new values and then passed to the <see cref="SaveCore"/>
		/// method for persisting back to the file system.
		/// </summary>
		/// <param name="core">An instance of <see cref="GalleryServerPro.Configuration.Core"/> that contains configuration
		/// data stored in the &lt;core .../&gt; section of galleryserverpro.config.</param>
		/// <returns>Returns an instance of <see cref="GspCoreEntity"/> that is contains data from the &lt;core .../&gt; section of 
		/// galleryserverpro.config.</returns>
		public static GspCoreEntity GetGspCoreEntity(Core core)
		{
			GspCoreEntity gce = new GspCoreEntity();

			Type coreType = typeof(Core);
			Type gceType = typeof(GspCoreEntity);

			// Iterate through each property of the core parameter and copy the value to the entity. Each property of the entity
			// has been configurated so that its data type matches that in the parameter.
			foreach (PropertyInfo coreProperty in coreType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				System.Diagnostics.Debug.WriteLine(coreProperty.Name);
				FieldInfo aseProperty = gceType.GetField(ToFirstCharLower(coreProperty.Name));

				if (aseProperty.FieldType == typeof(bool))
				{
					aseProperty.SetValue(gce, Convert.ToBoolean(coreProperty.GetValue(core, null), System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (aseProperty.FieldType == typeof(string))
				{
					aseProperty.SetValue(gce, Convert.ToString(coreProperty.GetValue(core, null), System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (aseProperty.FieldType == typeof(int))
				{
					aseProperty.SetValue(gce, Convert.ToInt32(coreProperty.GetValue(core, null), System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (aseProperty.FieldType == typeof(Single))
				{
					aseProperty.SetValue(gce, Convert.ToSingle(coreProperty.GetValue(core, null), System.Globalization.CultureInfo.InvariantCulture));
				}
				else
				{
					throw new ArgumentOutOfRangeException(string.Format("GspConfigController.GetGspCoreEntity is not designed to process a property of type {0} (encountered in GspCoreEntity.{1})", aseProperty.FieldType, aseProperty.Name));
				}
			}

			return gce;
		}

		/// <summary>
		/// Gets an instance of <see cref="GspDataProviderEntity"/> that contains data from the &lt;dataProvider .../&gt; section of 
		/// galleryserverpro.config. The entity can be updated with new values and then passed to the <see cref="SaveDataProvider"/>
		/// method for persisting back to the file system.
		/// </summary>
		/// <param name="dataProvider">An instance of <see cref="GalleryServerPro.Configuration.DataProvider"/> that contains configuration
		/// data stored in the &lt;dataProvider .../&gt; section of galleryserverpro.config.</param>
		/// <returns>Returns an instance of <see cref="GspDataProviderEntity"/> that is contains data from the &lt;dataProvider .../&gt; section of 
		/// galleryserverpro.config.</returns>
		public static GspDataProviderEntity GetGspDataProviderEntity(DataProvider dataProvider)
		{
			GspDataProviderEntity dpe = new GspDataProviderEntity();

			dpe.defaultProvider = dataProvider.DefaultProvider;

			return dpe;
		}

		/// <summary>
		/// Persist the configuration data to the &lt;core .../&gt; section of galleryserverpro.config.
		/// </summary>
		/// <param name="core">An instance of <see cref="GspCoreEntity"/> that contains data to save to the
		/// &lt;core .../&gt; section of galleryserverpro.config.</param>
		/// <exception cref="UnauthorizedAccessException">Thrown when the IIS application pool identity does not have
		/// write access to galleryserverpro.config.</exception>
		public static void SaveCore(GspCoreEntity core)
		{
			XmlDocument xmlDoc = LoadGalleryServerProConfigFromDisk();

			// Update the attributes of the <core ...> element with the values from the source config file.
			XmlElement coreElement = (XmlElement)xmlDoc.SelectSingleNode(@"galleryServerPro/core");

			// Loop through each entity property and assign the value to the matching element in the <core> section of
			// galleryserverpro.config.
			Type type = typeof(GspCoreEntity);
			foreach (FieldInfo field in type.GetFields())
			{
				string attValue = Convert.ToString(field.GetValue(core), System.Globalization.CultureInfo.InvariantCulture);

				if (field.FieldType == typeof(bool))
				{
					// Bool fields should be stored in lower case; all others can be stored without modification.
					attValue = attValue.ToLowerInvariant();
				}

				coreElement.SetAttribute(field.Name, attValue);
			}

			SaveGalleryServerProConfigToDisk(xmlDoc);
		}

		/// <summary>
		/// Persist the configuration data to the &lt;dataProvider .../&gt; section of galleryserverpro.config.
		/// </summary>
		/// <param name="dataProvider">An instance of <see cref="GspDataProviderEntity"/> that contains data to save to the
		/// &lt;dataProvider .../&gt; section of galleryserverpro.config.</param>
		/// <exception cref="UnauthorizedAccessException">Thrown when the IIS application pool identity does not have
		/// write access to galleryserverpro.config.</exception>
		public static void SaveDataProvider(GspDataProviderEntity dataProvider)
		{
			XmlDocument xmlDoc = LoadGalleryServerProConfigFromDisk();

			// Update the attributes of the <core ...> element with the values from the source config file.
			XmlElement coreElement = (XmlElement)xmlDoc.SelectSingleNode(@"galleryServerPro/dataProvider");

			// Loop through each entity property and assign the value to the matching element in the <core> section of
			// galleryserverpro.config.
			Type type = typeof(GspDataProviderEntity);
			foreach (FieldInfo field in type.GetFields())
			{
				string attValue = Convert.ToString(field.GetValue(dataProvider), System.Globalization.CultureInfo.InvariantCulture);
				//string attValue = field.GetValue(dataProvider).ToString();

				if (field.FieldType == typeof(bool))
				{
					// Bool fields should be stored in lower case; all others can be stored without modification.
					attValue = attValue.ToLowerInvariant();
				}

				coreElement.SetAttribute(field.Name, attValue);
			}

			SaveGalleryServerProConfigToDisk(xmlDoc);
		}

		/// <summary>
		/// Persist the configuration data to the &lt;mimeTypes .../&gt; section of galleryserverpro.config. Only the
		/// AllowAddToGallery attribute for each mimeType is updated, and no items are added or deleted.
		/// </summary>
		/// <param name="mimeTypes">A Dictionary containing MIME type data to persist to the &lt;mimeTypes .../&gt; section 
		/// of galleryserverpro.config. The dictionary key must be the file extension (e.g. ".jpg") and the dictionary
		/// value must be the string "true" or "false" to store in the AllowAddToGallery attribute. The dictionary may
		/// have less items than exist in the config file.</param>
		/// <exception cref="UnauthorizedAccessException">Thrown when the IIS application pool identity does not have
		/// write access to galleryserverpro.config.</exception>
		public static void SaveMimeTypes(Dictionary<string, bool> mimeTypes)
		{
			XmlDocument xmlDoc = LoadGalleryServerProConfigFromDisk();

			// Update the attributes of the <mimeTypes ...> element with the values from the source config file.
			XmlElement mimeTypesElement = (XmlElement)xmlDoc.SelectSingleNode(@"galleryServerPro/galleryObject/mimeTypes");

			// Loop through each mime type and assign the value to the matching element in the .config file.
			foreach (KeyValuePair<string, bool> keyValuePair in mimeTypes)
			{
				string xPath = string.Format(@"mimeType[@fileExtension='{0}']", keyValuePair.Key);
				
				// Each mimeType element is uniquely identified by fileExtension and browserId, so make sure we update 
				// *all* of them that match fileExtension (although in most cases there will be only one).
				foreach (XmlElement mimeTypeElement in mimeTypesElement.SelectNodes(xPath))
				{
					mimeTypeElement.SetAttribute("allowAddToGallery", keyValuePair.Value.ToString().ToLowerInvariant());
				}
			}
			
			SaveGalleryServerProConfigToDisk(xmlDoc);
		}

		#endregion

		#region Private Static Methods

		private static string ToFirstCharLower(string value)
		{
			if (value == null)
				return null;

			if (value == string.Empty)
				return string.Empty;

			if (value.Length == 1)
				return value.ToLowerInvariant();

			return string.Concat(value[0].ToString().ToLowerInvariant(), value.Substring(1));
		}

		public static XmlDocument LoadGalleryServerProConfigFromDisk()
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(Util.GalleryServerProConfigFilePath);

			if (xmlDoc == null)
				throw new WebException(string.Format("Could not load {0}.", Util.GalleryServerProConfigFilePath));
			return xmlDoc;
		}

		public static void SaveGalleryServerProConfigToDisk(XmlDocument xmlDoc)
		{
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.Indent = true;
			xws.NewLineOnAttributes = true;
			xws.Encoding = new UTF8Encoding(false);

			XmlWriter writer = XmlWriter.Create(Util.GalleryServerProConfigFilePath, xws);

			if (writer == null)
				throw new InvalidOperationException("Null value returned from XmlWriter.Create in AppSettingController.Save().");

			xmlDoc.Save(writer);

			writer.Close();
		}

		#endregion
	}
}

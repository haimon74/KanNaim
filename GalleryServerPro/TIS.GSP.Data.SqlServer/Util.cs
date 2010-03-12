using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Data.SqlServer
{
	#region Enum Declarations

	internal enum SqlVersion
	{
		Unknown,
		PreSql2000,
		Sql2000,
		Sql2005,
		Sql2008,
		PostSql2008
	}

	/// <summary>
	/// References a version of the database schema used by Gallery Server Pro. A new schema version is added for any
	/// release that requires a database change. Data schemas earlier than 2.1.3162 are not supported.
	/// </summary>
	internal enum GalleryDataSchemaVersion
	{
		// IMPORTANT: When modifying these values, be sure to update the functions ConvertGalleryDataSchemaVersionToString and
		// ConvertGalleryDataSchemaVersionToEnum as well!
		/// <summary>
		/// Gets the Unknown data schema version.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Gets the schema version for 2.1.3162.
		/// </summary>
		V2_1_3162 = 1,
		/// <summary>
		/// Gets the schema version for 2.3.3421.
		/// </summary>
		V2_3_3421 = 2
	}

	#endregion

	/// <summary>
	/// Contains functionality for commonly used functions used throughout this assembly.
	/// </summary>
	/// <remarks>This is the same class as the identically named one in the SQLite project. Any changes made to this class
	/// should be made to that one as well.</remarks>
	internal static class Util
	{
		#region Private Fields

		private static string _schema;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the SQL server schema that each database object is to belong to.  Each instance of {schema} in any
		/// SQL that is executed is replaced with this value prior to execution.
		/// </summary>
		/// <value>The SQL server schema.</value>
		internal static String SqlServerSchema
		{
			get
			{
				return _schema;
			}
			set
			{
				_schema = value;
				
				if (!_schema.EndsWith("."))
				{
					_schema += ".";
				}
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Validates that the backup file specified in the <see cref="IBackupFile.FilePath"/> property of the <paramref name="backupFile"/>
		/// parameter is valid and populates the remaining properties with information about the file.
		/// </summary>
		/// <param name="backupFile">An instance of <see cref="IBackupFile"/> that with only the <see cref="IBackupFile.FilePath"/>
		/// property assigned. The remaining properties should be uninitialized since they will be assigned in this method.</param>
		/// <remarks>Note that this function attempts to extract the number of records from each table in the backup file. Any exceptions
		/// that occur during this process are caught and trigger the <see cref="IBackupFile.IsValid" /> property to be set to false. If the extraction is 
		/// successful, then the file is assumed to be valid and the <see cref="IBackupFile.IsValid" /> property is set to <c>true</c>.</remarks>
		internal static void ValidateBackupFile(ref IBackupFile backupFile)
		{
			try
			{
				using (DataSet ds = GenerateDataSet(backupFile.FilePath))
				{
					string[] tableNames = new string[] { "aspnet_Applications", "aspnet_Profile", "aspnet_Roles", "aspnet_Users", "aspnet_UsersInRoles", "gs_Gallery", "gs_Album", "gs_MediaObject", "gs_MediaObjectMetadata", "gs_Role_Album", "gs_Role", "gs_AppError" };

					foreach (string tableName in tableNames)
					{
						DataTable table = ds.Tables[tableName];

						backupFile.DataTableRecordCount.Add(tableName, table.Rows.Count);
					}

					const string schemaVersionTableName = "gs_SchemaVersion";
					DataTable schemaTable = ds.Tables[schemaVersionTableName];
					DataRow schemaRow = schemaTable.Rows[0];

					backupFile.SchemaVersion = schemaRow["SchemaVersion"].ToString();

					if (backupFile.SchemaVersion == ConvertGalleryDataSchemaVersionToString(GalleryDataSchemaVersion.V2_3_3421))
					{
						backupFile.IsValid = true;
					}
				}
			}
			catch
			{
				backupFile.IsValid = false;
			}
		}

		/// <summary>
		/// Gets the version of SQL Server currently being used.
		/// </summary>
		/// <returns>Returns an enumeration value that indicates the version of SQL Server the web installer is connected to.</returns>
		/// <remarks>This function is a nearly identical copy of the one used in the install wizard.</remarks>
		internal static SqlVersion GetSqlVersion()
		{
			SqlVersion version = SqlVersion.Unknown;

			using (SqlConnection cn = SqlDataProvider.GetDbConnection())
			{
				using (SqlCommand cmd = new SqlCommand("SELECT SERVERPROPERTY('productversion')", cn))
				{
					cn.Open();
					using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						while (dr.Read())
						{
							string sqlVersion = dr.GetString(0);
							if (!String.IsNullOrEmpty(sqlVersion))
							{
								int majorVersion;
								if (Int32.TryParse(sqlVersion.Substring(0, sqlVersion.IndexOf(".")), out majorVersion))
								{
									if (majorVersion < 7) version = SqlVersion.PreSql2000;
									else if (majorVersion == 8) version = SqlVersion.Sql2000;
									else if (majorVersion == 9) version = SqlVersion.Sql2005;
									else if (majorVersion == 10) version = SqlVersion.Sql2008;
									else if (majorVersion > 10) version = SqlVersion.PostSql2008;
								}
							}
							break;
						}
						dr.Close();
					}
				}
			}

			return version;
		}

		/// <summary>
		/// Convert <paramref name="version"/> to its string equivalent. Example: Return "2.1.3162" when <paramref name="version"/> 
		/// is <see cref="GalleryDataSchemaVersion.V2_1_3162"/>. This is a lookup function and does not return the current version 
		/// of the database or application schema requirements.
		/// </summary>
		/// <param name="version">The version of the gallery's data schema for which a string representation is to be returned.</param>
		/// <returns>Returns the string equivalent of the specified <see cref="GalleryDataSchemaVersion"/> value.</returns>
		internal static string ConvertGalleryDataSchemaVersionToString(GalleryDataSchemaVersion version)
		{
			switch (version)
			{
				case GalleryDataSchemaVersion.V2_1_3162:
					return "2.1.3162";
					break;
				case GalleryDataSchemaVersion.V2_3_3421:
					return "2.3.3421";
					break;
				default:
					throw new InvalidEnumArgumentException(String.Format("The function GalleryServerPro.Data.SQLite.SQLiteGalleryServerProProvider.ConvertGalleryDataSchemaVersionToString was not designed to handle the GalleryDataSchemaVersion enumeration value {0}. A developer must update this method to handle this value.", version));
			}
		}

		/// <summary>
		/// Convert <paramref name="version"/> to its <see cref="GalleryDataSchemaVersion"/> equivalent. Example: Return 
		/// <see cref="GalleryDataSchemaVersion.V2_1_3162"/> when <paramref name="version"/> is "2.1.3162". This is a 
		/// lookup function and does not return the current version of the database or application schema requirements.
		/// </summary>
		/// <param name="version">The version of the gallery's data schema.</param>
		/// <returns>Returns the <see cref="GalleryDataSchemaVersion"/> equivalent of the specified string.</returns>
		internal static GalleryDataSchemaVersion ConvertGalleryDataSchemaVersionToEnum(string version)
		{
			switch (version)
			{
				case "2.1.3162":
					return GalleryDataSchemaVersion.V2_1_3162;
					break;
				case "2.3.3421":
					return GalleryDataSchemaVersion.V2_3_3421;
					break;
				default:
					return GalleryDataSchemaVersion.Unknown;
			}
		}

		/// <summary>
		/// Gets the schema qualified name for the specified <paramref name="rawName">database object</paramref>. For example,
		/// if the schema is "dbo." and the database object is "gs_AlbumSelect", this function returns "dbo.gs_AlbumSelect".
		/// </summary>
		/// <param name="rawName">Name of the database object. Brackets (e.g. "[gs_AlbumSelect]" are required if the object 
		/// contains a space.</param>
		/// <returns>Returns the schema qualified name for the specified <paramref name="rawName">database object</paramref>.</returns>
		internal static String GetSqlName(string rawName)
		{
			return String.Concat(SqlServerSchema, rawName);
		}

		#endregion

		#region Private Functions

		private static DataSet GenerateDataSet(string filePath)
		{
			DataSet ds = new DataSet("GalleryServerData");
			ds.ReadXml(filePath, XmlReadMode.Auto);

			return ds;
		}

		#endregion
	}
}

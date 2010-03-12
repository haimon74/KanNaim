using System;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace GalleryServerPro.Data.SQLite
{
	/// <summary>
	/// Contains functionality for importing and exporting gallery and membership data.
	/// </summary>
	internal static class DataUtility
	{
		private static readonly string[] _sqliteMembershipTableNames = new string[] { "aspnet_Applications", "aspnet_Roles", "aspnet_Users", "aspnet_UsersInRoles", "aspnet_Profile" }; // Does not have aspnet_Membership;
		private static readonly string[] _schemaMembershipTableNames = new string[] { "aspnet_Applications", "aspnet_Membership", "aspnet_Roles", "aspnet_Users", "aspnet_UsersInRoles", "aspnet_Profile" };
		private static readonly string[] _galleryTableNames = new string[] { "gs_Gallery", "gs_Album", "gs_MediaObject", "gs_MediaObjectMetadata", "gs_Role_Album", "gs_Role", "gs_AppError", "gs_SchemaVersion" };

		/// <summary>
		/// Imports the data from <paramref name="filePath"/> to the SQLite database specified in <paramref name="connectionString"/>.
		/// </summary>
		/// <param name="filePath">The filename (including the path) from which to read.</param>
		/// <param name="importMembershipData">if set to <c>true</c> import membership data.</param>
		/// <param name="importGalleryData">if set to <c>true</c> import gallery data.</param>
		/// <param name="connectionString">The connection string to the SQLite database.</param>
		internal static void ImportData(string filePath, bool importMembershipData, bool importGalleryData, string connectionString)
		{
			if (String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException("filePath");

			if (String.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException("connectionString");

			SQLiteTransaction tran = null;
			SQLiteConnection cn = new SQLiteConnection(connectionString);
			try
			{
				cn.Open();
				tran = cn.BeginTransaction();

				ClearData(importMembershipData, importGalleryData, cn);

				using (DataSet ds = GenerateDataSet(filePath))
				{
					if (importMembershipData)
					{
						InsertApplications(ds, cn);

						InsertProfiles(ds, cn);

						InsertRoles(ds, cn);

						InsertUsers(ds, cn);

						InsertUsersInRoles(ds, cn);
					}

					if (importGalleryData)
					{
						InsertGalleries(ds, cn);

						InsertAlbums(ds, cn);

						InsertRolesAlbums(ds, cn);

						InsertMediaObjects(ds, cn);

						InsertMediaObjectMetadata(ds, cn);

						InsertGalleryRoles(ds, cn);

						InsertAppErrors(ds, cn);
					}
				}

				tran.Commit();
			}
			catch
			{
				if (tran != null)
					tran.Rollback();

				throw;
			}
		}

		/// <summary>
		/// Exports the data from the SQLite database specified in <paramref name="connectionString"/> to <paramref name="filePath"/>.
		/// </summary>
		/// <param name="filePath">The file name (including the path) to which to write.</param>
		/// <param name="exportMembershipData">if set to <c>true</c> export membership data.</param>
		/// <param name="exportGalleryData">if set to <c>true</c> export gallery data.</param>
		/// <param name="connectionString">The connection string to the SQLite database.</param>
		internal static void ExportData(string filePath, bool exportMembershipData, bool exportGalleryData, string connectionString)
		{
			DataSet ds = new DataSet("GalleryServerData");

			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			using (System.IO.Stream stream = asm.GetManifestResourceStream("GalleryServerPro.Data.SQLite.GalleryServerProSchema.xml"))
			{
				ds.ReadXmlSchema(stream);
			}

			using (SQLiteConnection cn = new SQLiteConnection(connectionString))
			{
				using (SQLiteCommand cmd = cn.CreateCommand())
				{
					cn.Open();

					if (exportMembershipData)
					{
						string sql = @"
SELECT ApplicationName, lower(ApplicationName) AS LoweredApplicationName, ApplicationId, Description
FROM aspnet_Applications;

SELECT ApplicationId, UserId, Password, CASE PasswordFormat WHEN 'Clear' THEN 0 WHEN 'Hashed' THEN 1 WHEN 'Encrypted' THEN 2 ELSE 99 END AS PasswordFormat,
 PasswordSalt, NULL AS MobilePIN, Email, LoweredEmail, PasswordQuestion, PasswordAnswer, IsApproved, 
 IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, 
 FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, Comment
FROM aspnet_Users;

SELECT ApplicationId, RoleId, RoleName, LoweredRoleName, '' AS Description
FROM aspnet_Roles;

SELECT ApplicationId, UserId, Username, LoweredUserName, null as MobileAlias, IsAnonymous, LastActivityDate
FROM aspnet_Users;

SELECT UserId, RoleId
FROM aspnet_UsersInRoles;";

						cmd.CommandText = sql;

						using (IDataReader dr = cmd.ExecuteReader())
						{
							ds.Load(dr, LoadOption.OverwriteChanges, GetMembershipTableNames(false));
						}

						// Load aspnet_Profile table, but only if there are records. Initially I had included this table in the SELECT statements above, but
						// it failed when there weren't any records (Error: "Inconvertible type mismatch between SourceColumn 'PropertyValuesBinary' of Object and 
						// the DataColumn 'PropertyValuesBinary' of Byte[]"). It may be possible to construct the SQL so that it can be included in the 
						// above statements but I couldn't figure it out (adding a WHERE EXISTS clause didn't work).

						// Step 1: Determine if aspnet_Profile is empty.
						sql = "SELECT EXISTS (SELECT * FROM aspnet_Profile);";
						cmd.Parameters.Clear();
						cmd.CommandText = sql;
						bool profileRecordsExist = Convert.ToBoolean(cmd.ExecuteScalar());

						// Step 2: Load table if it has records.
						if (profileRecordsExist)
						{
							sql = "SELECT UserId, PropertyNames, PropertyValuesString, zeroblob(0) AS PropertyValuesBinary, LastUpdatedDate FROM aspnet_Profile;";

							// Step 3: Execute SQL and load into dataset table.
							cmd.CommandText = sql;
							using (IDataReader dr = cmd.ExecuteReader())
							{
								ds.Load(dr, LoadOption.OverwriteChanges, new string[] { "aspnet_Profile" });
							}
						}
					}

					if (exportGalleryData)
					{
						foreach (string tableName in _galleryTableNames)
						{
							cmd.CommandText = String.Concat(@"SELECT * FROM ", tableName, ";");
							ds.Load(cmd.ExecuteReader(), LoadOption.OverwriteChanges, tableName);
						}
					}

					// We always want to get the schema into the dataset, even when we're not getting the rest of the gallery data.
					cmd.CommandText = @"SELECT * FROM gs_SchemaVersion;";
					ds.Load(cmd.ExecuteReader(), LoadOption.OverwriteChanges, "gs_SchemaVersion");

					ds.WriteXml(filePath, XmlWriteMode.WriteSchema);
				}
			}
		}

		/// <summary>
		/// Gets the names of the ASP.NET membership tables, optionally include the profile table.
		/// </summary>
		/// <param name="includeProfileTable">If set to <c>true</c> include the profile table (aspnet_Profile).</param>
		/// <returns>Gets the names of the ASP.NET membership tables, optionally include the profile table.</returns>
		private static string[] GetMembershipTableNames(bool includeProfileTable)
		{
			if (includeProfileTable)
			{
				return _schemaMembershipTableNames;
			}
			else
			{
				string[] tableNames = _schemaMembershipTableNames;

				if (((String)tableNames.GetValue(tableNames.Length - 1)) != "aspnet_Profile")
					throw new DataException("The table name 'aspnet_Profile' must be the last item in the string array.");

				Array.Resize(ref tableNames, tableNames.Length - 1);

				return tableNames;
			}
		}

		private static void ClearData(bool deleteMembershipData, bool deleteGalleryData, SQLiteConnection cn)
		{
			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				if (deleteMembershipData)
				{
					foreach (string tableName in _sqliteMembershipTableNames)
					{
						cmd.CommandText = String.Concat("DELETE FROM ", tableName, ";");
						cmd.ExecuteNonQuery();
					}
				}
				if (deleteGalleryData)
				{
					foreach (string tableName in _galleryTableNames)
					{
						if (tableName == "gs_SchemaVersion")
							continue; // Don't delete data in gs_SchemaVersion

						cmd.CommandText = String.Concat("DELETE FROM ", tableName, ";");
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

		private static void InsertApplications(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["aspnet_Applications"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO aspnet_Applications (ApplicationId, ApplicationName, Description)"
													+ " VALUES ($ApplicationId, $ApplicationName, $Description)";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();

				prm1.ParameterName = "$ApplicationId";
				prm2.ParameterName = "$ApplicationName";
				prm3.ParameterName = "$Description";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["ApplicationId"].ToString();
					prm2.Value = row["ApplicationName"];
					prm3.Value = row["Description"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertProfiles(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["aspnet_Profile"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO aspnet_Profile (UserId, PropertyNames, PropertyValuesString, LastUpdatedDate)"
													+ " VALUES ($UserId, $PropertyNames, $PropertyValuesString, $LastUpdatedDate)";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();

				prm1.ParameterName = "$UserId";
				prm2.ParameterName = "$PropertyNames";
				prm3.ParameterName = "$PropertyValuesString";
				prm4.ParameterName = "$LastUpdatedDate";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["UserId"].ToString();
					prm2.Value = row["PropertyNames"];
					prm3.Value = row["PropertyValuesString"];
					prm4.Value = row["LastUpdatedDate"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertRoles(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["aspnet_Roles"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO aspnet_Roles"
													+ " (RoleId, RoleName, LoweredRoleName, ApplicationId) "
													+ " Values ($RoleId, $RoleName, $LoweredRoleName, $ApplicationId)";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();

				prm1.ParameterName = "$RoleId";
				prm2.ParameterName = "$RoleName";
				prm3.ParameterName = "$LoweredRoleName";
				prm4.ParameterName = "$ApplicationId";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);

				if (cn.State == ConnectionState.Closed)
					cn.Open();

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["RoleId"].ToString();
					prm2.Value = row["RoleName"];
					prm3.Value = row["LoweredRoleName"].ToString();
					prm4.Value = row["ApplicationId"].ToString();

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertUsers(DataSet ds, SQLiteConnection cn)
		{
			DataTable dtUsers = ds.Tables["aspnet_Users"];
			DataTable dtMembership = ds.Tables["aspnet_Membership"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO aspnet_Users" +
													" (UserId, Username, LoweredUsername, ApplicationId, Email, LoweredEmail, Comment, Password, " +
													" PasswordFormat, PasswordSalt, PasswordQuestion, PasswordAnswer, IsApproved, IsAnonymous, " +
													" LastActivityDate, LastLoginDate, LastPasswordChangedDate, CreateDate, " +
													" IsLockedOut, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, " +
													" FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart) " +
													" Values ($UserId, $Username, $LoweredUsername, $ApplicationId, $Email, $LoweredEmail, $Comment, $Password, " +
													" $PasswordFormat, $PasswordSalt, $PasswordQuestion, $PasswordAnswer, $IsApproved, $IsAnonymous, " +
													" $LastActivityDate, $LastLoginDate, $LastPasswordChangedDate, $CreateDate, " +
													" $IsLockedOut, $LastLockoutDate, $FailedPasswordAttemptCount, $FailedPasswordAttemptWindowStart, " +
													" $FailedPasswordAnswerAttemptCount, $FailedPasswordAnswerAttemptWindowStart)";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();
				DbParameter prm5 = cmd.CreateParameter();
				DbParameter prm6 = cmd.CreateParameter();
				DbParameter prm7 = cmd.CreateParameter();
				DbParameter prm8 = cmd.CreateParameter();
				DbParameter prm9 = cmd.CreateParameter();
				DbParameter prm10 = cmd.CreateParameter();
				DbParameter prm11 = cmd.CreateParameter();
				DbParameter prm12 = cmd.CreateParameter();
				DbParameter prm13 = cmd.CreateParameter();
				DbParameter prm14 = cmd.CreateParameter();
				DbParameter prm15 = cmd.CreateParameter();
				DbParameter prm16 = cmd.CreateParameter();
				DbParameter prm17 = cmd.CreateParameter();
				DbParameter prm18 = cmd.CreateParameter();
				DbParameter prm19 = cmd.CreateParameter();
				DbParameter prm20 = cmd.CreateParameter();
				DbParameter prm21 = cmd.CreateParameter();
				DbParameter prm22 = cmd.CreateParameter();
				DbParameter prm23 = cmd.CreateParameter();
				DbParameter prm24 = cmd.CreateParameter();

				prm1.ParameterName = "$UserId";
				prm2.ParameterName = "$Username";
				prm3.ParameterName = "$LoweredUsername";
				prm4.ParameterName = "$ApplicationId";
				prm5.ParameterName = "$Email";
				prm6.ParameterName = "$LoweredEmail";
				prm7.ParameterName = "$Comment";
				prm8.ParameterName = "$Password";
				prm9.ParameterName = "$PasswordFormat";
				prm10.ParameterName = "$PasswordSalt";
				prm11.ParameterName = "$PasswordQuestion";
				prm12.ParameterName = "$PasswordAnswer";
				prm13.ParameterName = "$IsApproved";
				prm14.ParameterName = "$IsAnonymous";
				prm15.ParameterName = "$LastActivityDate";
				prm16.ParameterName = "$LastLoginDate";
				prm17.ParameterName = "$LastPasswordChangedDate";
				prm18.ParameterName = "$CreateDate";
				prm19.ParameterName = "$IsLockedOut";
				prm20.ParameterName = "$LastLockoutDate";
				prm21.ParameterName = "$FailedPasswordAttemptCount";
				prm22.ParameterName = "$FailedPasswordAttemptWindowStart";
				prm23.ParameterName = "$FailedPasswordAnswerAttemptCount";
				prm24.ParameterName = "$FailedPasswordAnswerAttemptWindowStart";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);
				cmd.Parameters.Add(prm5);
				cmd.Parameters.Add(prm6);
				cmd.Parameters.Add(prm7);
				cmd.Parameters.Add(prm8);
				cmd.Parameters.Add(prm9);
				cmd.Parameters.Add(prm10);
				cmd.Parameters.Add(prm11);
				cmd.Parameters.Add(prm12);
				cmd.Parameters.Add(prm13);
				cmd.Parameters.Add(prm14);
				cmd.Parameters.Add(prm15);
				cmd.Parameters.Add(prm16);
				cmd.Parameters.Add(prm17);
				cmd.Parameters.Add(prm18);
				cmd.Parameters.Add(prm19);
				cmd.Parameters.Add(prm20);
				cmd.Parameters.Add(prm21);
				cmd.Parameters.Add(prm22);
				cmd.Parameters.Add(prm23);
				cmd.Parameters.Add(prm24);

				// Note: The table aspnet_Users contains all users, including anonymous users who only have their profile stored
				// (that is, no username or password). The table aspnet_Membership only contains users with usernames and passwords.

				// Create a row to represent the anonymous user.
				DataRow anonymousMembershipRow = CreateAnonymousMembershipRow(dtMembership);

				foreach (DataRow userRow in dtUsers.Rows)
				{
					// Find the matching row in the membership table. If it doesn't exist, 
					DataRow membershipRow;
					DataRow[] membershipRows = dtMembership.Select("UserId = '" + userRow["UserId"] + "'");
					if ((membershipRows != null) && (membershipRows.Length > 0))
						membershipRow = membershipRows[0];
					else
						membershipRow = anonymousMembershipRow;

					prm1.Value = userRow["UserId"].ToString(); ;
					prm2.Value = userRow["UserName"];
					prm3.Value = userRow["LoweredUserName"];
					prm4.Value = userRow["ApplicationId"].ToString(); ;
					prm5.Value = membershipRow["Email"];
					prm6.Value = membershipRow["LoweredEmail"];
					prm7.Value = membershipRow["Comment"];
					prm8.Value = membershipRow["Password"];
					prm9.Value = GetPasswordFormat(Convert.ToInt32(membershipRow["PasswordFormat"]));
					prm10.Value = membershipRow["PasswordSalt"];
					prm11.Value = membershipRow["PasswordQuestion"];
					prm12.Value = membershipRow["PasswordAnswer"];
					prm13.Value = membershipRow["IsApproved"];
					prm14.Value = userRow["IsAnonymous"];
					prm15.Value = userRow["LastActivityDate"];
					prm16.Value = membershipRow["LastLoginDate"];
					prm17.Value = membershipRow["LastPasswordChangedDate"];
					prm18.Value = membershipRow["CreateDate"];
					prm19.Value = membershipRow["IsLockedOut"];
					prm20.Value = membershipRow["LastLockoutDate"];
					prm21.Value = membershipRow["FailedPasswordAttemptCount"];
					prm22.Value = membershipRow["FailedPasswordAttemptWindowStart"];
					prm23.Value = membershipRow["FailedPasswordAnswerAttemptCount"];
					prm24.Value = membershipRow["FailedPasswordAnswerAttemptWindowStart"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertUsersInRoles(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["aspnet_UsersInRoles"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO aspnet_UsersInRoles"
													+ " (UserId, RoleId)"
													+ " VALUES ($UserId, $RoleId);";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();

				prm1.ParameterName = "$UserId";
				prm2.ParameterName = "$RoleId";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);

				foreach (DataRow roleRow in dt.Rows)
				{
					prm1.Value = roleRow["UserId"].ToString(); ;
					prm2.Value = roleRow["RoleId"].ToString(); ;

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertGalleries(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_Gallery"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = @"
INSERT INTO [gs_Gallery] (GalleryId, Description, DateAdded)
VALUES ($GalleryId, $Description, $DateAdded);";

				DbParameter prmGalleryId = cmd.CreateParameter();
				DbParameter prmDescription = cmd.CreateParameter();
				DbParameter prmDateAdded = cmd.CreateParameter();

				prmGalleryId.ParameterName = "$GalleryId";
				prmDescription.ParameterName = "$Description";
				prmDateAdded.ParameterName = "$DateAdded";

				cmd.Parameters.Add(prmGalleryId);
				cmd.Parameters.Add(prmDescription);
				cmd.Parameters.Add(prmDateAdded);

				foreach (DataRow row in dt.Rows)
				{
					prmGalleryId.Value = row["GalleryId"];
					prmDescription.Value = row["Description"];
					prmDateAdded.Value = row["DateAdded"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertMediaObjects(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_MediaObject"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
INSERT INTO [gs_MediaObject] (MediaObjectId, HashKey, FKAlbumId, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
 ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
 OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Title, Seq, CreatedBy, 
 DateAdded, LastModifiedBy, DateLastModified, IsPrivate)
VALUES ($MediaObjectId, $HashKey, $FKAlbumId, $ThumbnailFilename, $ThumbnailWidth, $ThumbnailHeight,
 $ThumbnailSizeKB, $OptimizedFilename, $OptimizedWidth, $OptimizedHeight, $OptimizedSizeKB,
 $OriginalFilename, $OriginalWidth, $OriginalHeight, $OriginalSizeKB, $ExternalHtmlSource, $ExternalType, $Title, $Seq, $CreatedBy, 
 $DateAdded, $LastModifiedBy, $DateLastModified, $IsPrivate);";
				cmd.CommandText = sql;

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();
				DbParameter prm5 = cmd.CreateParameter();
				DbParameter prm6 = cmd.CreateParameter();
				DbParameter prm7 = cmd.CreateParameter();
				DbParameter prm8 = cmd.CreateParameter();
				DbParameter prm9 = cmd.CreateParameter();
				DbParameter prm10 = cmd.CreateParameter();
				DbParameter prm11 = cmd.CreateParameter();
				DbParameter prm12 = cmd.CreateParameter();
				DbParameter prm13 = cmd.CreateParameter();
				DbParameter prm14 = cmd.CreateParameter();
				DbParameter prm15 = cmd.CreateParameter();
				DbParameter prm16 = cmd.CreateParameter();
				DbParameter prm17 = cmd.CreateParameter();
				DbParameter prm18 = cmd.CreateParameter();
				DbParameter prm19 = cmd.CreateParameter();
				DbParameter prm20 = cmd.CreateParameter();
				DbParameter prm21 = cmd.CreateParameter();
				DbParameter prm22 = cmd.CreateParameter();
				DbParameter prm23 = cmd.CreateParameter();
				DbParameter prm24 = cmd.CreateParameter();

				prm1.ParameterName = "$MediaObjectId";
				prm2.ParameterName = "$HashKey";
				prm3.ParameterName = "$FKAlbumId";
				prm4.ParameterName = "$ThumbnailFilename";
				prm5.ParameterName = "$ThumbnailWidth";
				prm6.ParameterName = "$ThumbnailHeight";
				prm7.ParameterName = "$ThumbnailSizeKB";
				prm8.ParameterName = "$OptimizedFilename";
				prm9.ParameterName = "$OptimizedWidth";
				prm10.ParameterName = "$OptimizedHeight";
				prm11.ParameterName = "$OptimizedSizeKB";
				prm12.ParameterName = "$OriginalFilename";
				prm13.ParameterName = "$OriginalWidth";
				prm14.ParameterName = "$OriginalHeight";
				prm15.ParameterName = "$OriginalSizeKB";
				prm16.ParameterName = "$ExternalHtmlSource";
				prm17.ParameterName = "$ExternalType";
				prm18.ParameterName = "$Title";
				prm19.ParameterName = "$Seq";
				prm20.ParameterName = "$CreatedBy";
				prm21.ParameterName = "$DateAdded";
				prm22.ParameterName = "$LastModifiedBy";
				prm23.ParameterName = "$DateLastModified";
				prm24.ParameterName = "$IsPrivate";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);
				cmd.Parameters.Add(prm5);
				cmd.Parameters.Add(prm6);
				cmd.Parameters.Add(prm7);
				cmd.Parameters.Add(prm8);
				cmd.Parameters.Add(prm9);
				cmd.Parameters.Add(prm10);
				cmd.Parameters.Add(prm11);
				cmd.Parameters.Add(prm12);
				cmd.Parameters.Add(prm13);
				cmd.Parameters.Add(prm14);
				cmd.Parameters.Add(prm15);
				cmd.Parameters.Add(prm16);
				cmd.Parameters.Add(prm17);
				cmd.Parameters.Add(prm18);
				cmd.Parameters.Add(prm19);
				cmd.Parameters.Add(prm20);
				cmd.Parameters.Add(prm21);
				cmd.Parameters.Add(prm22);
				cmd.Parameters.Add(prm23);
				cmd.Parameters.Add(prm24);

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["MediaObjectId"];
					prm2.Value = row["HashKey"];
					prm3.Value = row["FKAlbumId"];
					prm4.Value = row["ThumbnailFilename"];
					prm5.Value = row["ThumbnailWidth"];
					prm6.Value = row["ThumbnailHeight"];
					prm7.Value = row["ThumbnailSizeKB"];
					prm8.Value = row["OptimizedFilename"];
					prm9.Value = row["OptimizedWidth"];
					prm10.Value = row["OptimizedHeight"];
					prm11.Value = row["OptimizedSizeKB"];
					prm12.Value = row["OriginalFilename"];
					prm13.Value = row["OriginalWidth"];
					prm14.Value = row["OriginalHeight"];
					prm15.Value = row["OriginalSizeKB"];
					prm16.Value = row["ExternalHtmlSource"];
					prm17.Value = row["ExternalType"];
					prm18.Value = row["Title"];
					prm19.Value = row["Seq"];
					prm20.Value = row["CreatedBy"];
					prm21.Value = row["DateAdded"];
					prm22.Value = row["LastModifiedBy"];
					prm23.Value = row["DateLastModified"];
					prm24.Value = row["IsPrivate"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertMediaObjectMetadata(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_MediaObjectMetadata"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = @"
INSERT INTO [gs_MediaObjectMetadata] (MediaObjectMetadataId, FKMediaObjectId, MetadataNameIdentifier, Description, Value)
VALUES ($MediaObjectMetadataId, $FKMediaObjectId, $MetadataNameIdentifier, $Description, $Value);";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();
				DbParameter prm5 = cmd.CreateParameter();

				prm1.ParameterName = "$MediaObjectMetadataId";
				prm2.ParameterName = "$FKMediaObjectId";
				prm3.ParameterName = "$MetadataNameIdentifier";
				prm4.ParameterName = "$Description";
				prm5.ParameterName = "$Value";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);
				cmd.Parameters.Add(prm5);

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["MediaObjectMetadataId"];
					prm2.Value = row["FKMediaObjectId"];
					prm3.Value = row["MetadataNameIdentifier"];
					prm4.Value = row["Description"];
					prm5.Value = row["Value"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertAlbums(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_Album"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
INSERT INTO [gs_Album] (AlbumId, AlbumParentId, FKGalleryId, Title, DirectoryName, 
Summary, ThumbnailMediaObjectId, Seq, DateStart, DateEnd, 
CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, 
OwnerRoleName, IsPrivate)
VALUES ($AlbumId, $AlbumParentId, $GalleryId, $Title, $DirectoryName, 
$Summary, $ThumbnailMediaObjectId, $Seq, $DateStart, $DateEnd, 
$CreatedBy, $DateAdded, $LastModifiedBy, $DateLastModified, $OwnedBy, 
$OwnerRoleName, $IsPrivate);";
				cmd.CommandText = sql;

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();
				DbParameter prm5 = cmd.CreateParameter();
				DbParameter prm6 = cmd.CreateParameter();
				DbParameter prm7 = cmd.CreateParameter();
				DbParameter prm8 = cmd.CreateParameter();
				DbParameter prm9 = cmd.CreateParameter();
				DbParameter prm10 = cmd.CreateParameter();
				DbParameter prm11 = cmd.CreateParameter();
				DbParameter prm12 = cmd.CreateParameter();
				DbParameter prm13 = cmd.CreateParameter();
				DbParameter prm14 = cmd.CreateParameter();
				DbParameter prm15 = cmd.CreateParameter();
				DbParameter prm16 = cmd.CreateParameter();
				DbParameter prm17 = cmd.CreateParameter();

				prm1.ParameterName = "$AlbumId";
				prm2.ParameterName = "$AlbumParentId";
				prm3.ParameterName = "$GalleryId";
				prm4.ParameterName = "$Title";
				prm5.ParameterName = "$DirectoryName";
				prm6.ParameterName = "$Summary";
				prm7.ParameterName = "$ThumbnailMediaObjectId";
				prm8.ParameterName = "$Seq";
				prm9.ParameterName = "$DateStart";
				prm10.ParameterName = "$DateEnd";
				prm11.ParameterName = "$CreatedBy";
				prm12.ParameterName = "$DateAdded";
				prm13.ParameterName = "$LastModifiedBy";
				prm14.ParameterName = "$DateLastModified";
				prm15.ParameterName = "$OwnedBy";
				prm16.ParameterName = "$OwnerRoleName";
				prm17.ParameterName = "$IsPrivate";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);
				cmd.Parameters.Add(prm5);
				cmd.Parameters.Add(prm6);
				cmd.Parameters.Add(prm7);
				cmd.Parameters.Add(prm8);
				cmd.Parameters.Add(prm9);
				cmd.Parameters.Add(prm10);
				cmd.Parameters.Add(prm11);
				cmd.Parameters.Add(prm12);
				cmd.Parameters.Add(prm13);
				cmd.Parameters.Add(prm14);
				cmd.Parameters.Add(prm15);
				cmd.Parameters.Add(prm16);
				cmd.Parameters.Add(prm17);

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["AlbumId"];
					prm2.Value = row["AlbumParentId"];
					prm3.Value = row["FKGalleryId"];
					prm4.Value = row["Title"];
					prm5.Value = row["DirectoryName"];
					prm6.Value = row["Summary"];
					prm7.Value = row["ThumbnailMediaObjectId"];
					prm8.Value = row["Seq"];
					prm9.Value = row["DateStart"];
					prm10.Value = row["DateEnd"];
					prm11.Value = row["CreatedBy"];
					prm12.Value = row["DateAdded"];
					prm13.Value = row["LastModifiedBy"];
					prm14.Value = row["DateLastModified"];
					prm15.Value = row["OwnedBy"];
					prm16.Value = row["OwnerRoleName"];
					prm17.Value = row["IsPrivate"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertAppErrors(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_AppError"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
INSERT INTO [gs_AppError]
  (AppErrorId, FKGalleryId, TimeStamp, ExceptionType, Message, Source, TargetSite, StackTrace, ExceptionData, InnerExType, 
  InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
  FormVariables, Cookies, SessionVariables, ServerVariables)
VALUES ($AppErrorId, $GalleryId, $TimeStamp, $ExceptionType, $Message, $Source, $TargetSite, $StackTrace, $ExceptionData, $InnerExType, 
  $InnerExMessage, $InnerExSource, $InnerExTargetSite, $InnerExStackTrace, $InnerExData, $Url,
  $FormVariables, $Cookies, $SessionVariables, $ServerVariables);";

				cmd.CommandText = sql;

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();
				DbParameter prm5 = cmd.CreateParameter();
				DbParameter prm6 = cmd.CreateParameter();
				DbParameter prm7 = cmd.CreateParameter();
				DbParameter prm8 = cmd.CreateParameter();
				DbParameter prm9 = cmd.CreateParameter();
				DbParameter prm10 = cmd.CreateParameter();
				DbParameter prm11 = cmd.CreateParameter();
				DbParameter prm12 = cmd.CreateParameter();
				DbParameter prm13 = cmd.CreateParameter();
				DbParameter prm14 = cmd.CreateParameter();
				DbParameter prm15 = cmd.CreateParameter();
				DbParameter prm16 = cmd.CreateParameter();
				DbParameter prm17 = cmd.CreateParameter();
				DbParameter prm18 = cmd.CreateParameter();
				DbParameter prm19 = cmd.CreateParameter();
				DbParameter prm20 = cmd.CreateParameter();

				prm1.ParameterName = "$AppErrorId";
				prm2.ParameterName = "$GalleryId";
				prm3.ParameterName = "$TimeStamp";
				prm4.ParameterName = "$ExceptionType";
				prm5.ParameterName = "$Message";
				prm6.ParameterName = "$Source";
				prm7.ParameterName = "$TargetSite";
				prm8.ParameterName = "$StackTrace";
				prm9.ParameterName = "$ExceptionData";
				prm10.ParameterName = "$InnerExType";
				prm11.ParameterName = "$InnerExMessage";
				prm12.ParameterName = "$InnerExSource";
				prm13.ParameterName = "$InnerExTargetSite";
				prm14.ParameterName = "$InnerExStackTrace";
				prm15.ParameterName = "$InnerExData";
				prm16.ParameterName = "$Url";
				prm17.ParameterName = "$FormVariables";
				prm18.ParameterName = "$Cookies";
				prm19.ParameterName = "$SessionVariables";
				prm20.ParameterName = "$ServerVariables";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);
				cmd.Parameters.Add(prm5);
				cmd.Parameters.Add(prm6);
				cmd.Parameters.Add(prm7);
				cmd.Parameters.Add(prm8);
				cmd.Parameters.Add(prm9);
				cmd.Parameters.Add(prm10);
				cmd.Parameters.Add(prm11);
				cmd.Parameters.Add(prm12);
				cmd.Parameters.Add(prm13);
				cmd.Parameters.Add(prm14);
				cmd.Parameters.Add(prm15);
				cmd.Parameters.Add(prm16);
				cmd.Parameters.Add(prm17);
				cmd.Parameters.Add(prm18);
				cmd.Parameters.Add(prm19);
				cmd.Parameters.Add(prm20);

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["AppErrorId"];
					prm2.Value = row["FKGalleryId"];
					prm3.Value = row["TimeStamp"];
					prm4.Value = row["ExceptionType"];
					prm5.Value = row["Message"];
					prm6.Value = row["Source"];
					prm7.Value = row["TargetSite"];
					prm8.Value = row["StackTrace"];
					prm9.Value = row["ExceptionData"];
					prm10.Value = row["InnerExType"];
					prm11.Value = row["InnerExMessage"];
					prm12.Value = row["InnerExSource"];
					prm13.Value = row["InnerExTargetSite"];
					prm14.Value = row["InnerExStackTrace"];
					prm15.Value = row["InnerExData"];
					prm16.Value = row["Url"];
					prm17.Value = row["FormVariables"];
					prm18.Value = row["Cookies"];
					prm19.Value = row["SessionVariables"];
					prm20.Value = row["ServerVariables"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertRolesAlbums(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_Role_Album"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				cmd.CommandText = @"
INSERT INTO [gs_Role_Album] (FKRoleName, FKAlbumId)
VALUES ($RoleName, $AlbumId);";

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();

				prm1.ParameterName = "$RoleName";
				prm2.ParameterName = "$AlbumId";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["FKRoleName"];
					prm2.Value = row["FKAlbumId"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static void InsertGalleryRoles(DataSet ds, SQLiteConnection cn)
		{
			DataTable dt = ds.Tables["gs_Role"];

			using (SQLiteCommand cmd = cn.CreateCommand())
			{
				const string sql = @"
INSERT INTO [gs_Role] (FKGalleryId, RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
	AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, AllowDeleteMediaObject, 
	AllowSynchronize, HideWatermark, AllowAdministerSite)
VALUES ($GalleryId, $RoleName, $AllowViewAlbumsAndObjects, $AllowViewOriginalImage, $AllowAddChildAlbum,
	$AllowAddMediaObject, $AllowEditAlbum, $AllowEditMediaObject, $AllowDeleteChildAlbum, $AllowDeleteMediaObject, 
	$AllowSynchronize, $HideWatermark, $AllowAdministerSite);";
				cmd.CommandText = sql;

				DbParameter prm1 = cmd.CreateParameter();
				DbParameter prm2 = cmd.CreateParameter();
				DbParameter prm3 = cmd.CreateParameter();
				DbParameter prm4 = cmd.CreateParameter();
				DbParameter prm5 = cmd.CreateParameter();
				DbParameter prm6 = cmd.CreateParameter();
				DbParameter prm7 = cmd.CreateParameter();
				DbParameter prm8 = cmd.CreateParameter();
				DbParameter prm9 = cmd.CreateParameter();
				DbParameter prm10 = cmd.CreateParameter();
				DbParameter prm11 = cmd.CreateParameter();
				DbParameter prm12 = cmd.CreateParameter();
				DbParameter prm13 = cmd.CreateParameter();

				prm1.ParameterName = "$GalleryId";
				prm2.ParameterName = "$RoleName";
				prm3.ParameterName = "$AllowViewAlbumsAndObjects";
				prm4.ParameterName = "$AllowViewOriginalImage";
				prm5.ParameterName = "$AllowAddChildAlbum";
				prm6.ParameterName = "$AllowAddMediaObject";
				prm7.ParameterName = "$AllowEditAlbum";
				prm8.ParameterName = "$AllowEditMediaObject";
				prm9.ParameterName = "$AllowDeleteChildAlbum";
				prm10.ParameterName = "$AllowDeleteMediaObject";
				prm11.ParameterName = "$AllowSynchronize";
				prm12.ParameterName = "$HideWatermark";
				prm13.ParameterName = "$AllowAdministerSite";

				cmd.Parameters.Add(prm1);
				cmd.Parameters.Add(prm2);
				cmd.Parameters.Add(prm3);
				cmd.Parameters.Add(prm4);
				cmd.Parameters.Add(prm5);
				cmd.Parameters.Add(prm6);
				cmd.Parameters.Add(prm7);
				cmd.Parameters.Add(prm8);
				cmd.Parameters.Add(prm9);
				cmd.Parameters.Add(prm10);
				cmd.Parameters.Add(prm11);
				cmd.Parameters.Add(prm12);
				cmd.Parameters.Add(prm13);

				foreach (DataRow row in dt.Rows)
				{
					prm1.Value = row["FKGalleryId"];
					prm2.Value = row["RoleName"];
					prm3.Value = row["AllowViewAlbumsAndObjects"];
					prm4.Value = row["AllowViewOriginalImage"];
					prm5.Value = row["AllowAddChildAlbum"];
					prm6.Value = row["AllowAddMediaObject"];
					prm7.Value = row["AllowEditAlbum"];
					prm8.Value = row["AllowEditMediaObject"];
					prm9.Value = row["AllowDeleteChildAlbum"];
					prm10.Value = row["AllowDeleteMediaObject"];
					prm11.Value = row["AllowSynchronize"];
					prm12.Value = row["HideWatermark"];
					prm13.Value = row["AllowAdministerSite"];

					cmd.ExecuteNonQuery();
				}
			}
		}

		private static DataSet GenerateDataSet(string filePath)
		{
			DataSet ds = new DataSet("GalleryServerData");
			ds.ReadXml(filePath, XmlReadMode.Auto);

			return ds;
		}

		private static DataRow CreateAnonymousMembershipRow(DataTable dtMembership)
		{
			DataRow dr = dtMembership.NewRow();

			DateTime nullDate = DateTime.MinValue;
			DateTime nowDate = DateTime.UtcNow;

			dr["ApplicationId"] = DBNull.Value;
			dr["UserId"] = Guid.NewGuid().ToString();
			dr["Password"] = Guid.NewGuid().ToString();
			dr["PasswordFormat"] = System.Web.Security.Membership.Provider.PasswordFormat;
			dr["PasswordSalt"] = String.Empty;
			dr["Email"] = String.Empty;
			dr["LoweredEmail"] = String.Empty;
			dr["PasswordQuestion"] = null;
			dr["PasswordAnswer"] = null;
			dr["IsApproved"] = false;
			dr["IsLockedOut"] = true;
			dr["CreateDate"] = nowDate;
			dr["LastLoginDate"] = nullDate;
			dr["LastPasswordChangedDate"] = nullDate;
			dr["LastLockoutDate"] = nullDate;
			dr["FailedPasswordAttemptCount"] = 0;
			dr["FailedPasswordAttemptWindowStart"] = nullDate;
			dr["FailedPasswordAnswerAttemptCount"] = 0;
			dr["FailedPasswordAnswerAttemptWindowStart"] = nullDate;

			return dr;
		}

		private static string GetPasswordFormat(int passwordFormatInt)
		{
			string passwordFormatString;

			switch (passwordFormatInt)
			{
				case 0: passwordFormatString = "Clear"; break;
				case 1: passwordFormatString = "Hashed"; break;
				case 2: passwordFormatString = "Encrypted"; break;
				default: throw new ProviderException(String.Format("Unrecognized password format: {0}", passwordFormatInt));
			}

			return passwordFormatString;
		}
	}
}

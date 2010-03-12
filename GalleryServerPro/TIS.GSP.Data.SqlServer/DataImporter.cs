using System;
using System.Data;
using System.Data.SqlClient;

namespace GalleryServerPro.Data.SqlServer
{
	internal static partial class DataUtility
	{
		/// <summary>
		/// Imports the data from <paramref name="filePath"/> to the SQL Server database.
		/// </summary>
		/// <param name="filePath">The filename (including the path) from which to read.</param>
		/// <param name="importMembershipData">if set to <c>true</c> import membership data.</param>
		/// <param name="importGalleryData">if set to <c>true</c> import gallery data.</param>
		internal static void ImportData(string filePath, bool importMembershipData, bool importGalleryData)
		{
			if (String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException("filePath");

			using (DataSet ds = GenerateDataSet(filePath))
			{
				SqlConnection cn = SqlDataProvider.GetDbConnection();
				SqlTransaction tran = null;
				try
				{
					cn.Open();

					using (tran = cn.BeginTransaction())
					{
						ClearData(cn, tran, importMembershipData, importGalleryData);


						if (importMembershipData)
						{
							string[] aspnet_TableNames = new string[] { "aspnet_Applications", "aspnet_Membership", "aspnet_Profile", "aspnet_Roles", "aspnet_Users", "aspnet_UsersInRoles" };

							// SqlBulkCopy requires SQL permissions equivalent to that provided in the db_ddladmin or db_owner roles.
							using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.KeepIdentity, tran))
							{
								foreach (string tableName in aspnet_TableNames)
								{
									bulkCopy.DestinationTableName = string.Concat(Util.SqlServerSchema, tableName);

									// Write from the source to the destination.
									using (IDataReader dr = ds.Tables[tableName].CreateDataReader())
									{
										try
										{
											bulkCopy.WriteToServer(dr);
										}
										catch (Exception ex)
										{
											// Add a little info to exception and re-throw.
											if (!ex.Data.Contains("SQL Bulk copy error"))
											{
												ex.Data.Add("SQL Bulk copy error", String.Format("Error occurred while importing table {0}.", tableName));
											}
											throw;
										}
									}
								}
							}
						}

						if (importGalleryData)
						{
							string[] gs_TableNames = new string[] { "gs_Gallery", "gs_Album", "gs_Role_Album", "gs_MediaObject", "gs_MediaObjectMetadata", "gs_Role", "gs_AppError" };

							// SqlBulkCopy requires SQL permissions equivalent to that provided in the db_ddladmin or db_owner roles.
							using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.KeepIdentity, tran))
							{
								foreach (string tableName in gs_TableNames)
								{
									bulkCopy.DestinationTableName = string.Concat(Util.SqlServerSchema, tableName);

									// Write from the source to the destination.
									using (IDataReader dr = ds.Tables[tableName].CreateDataReader())
									{
										try
										{
											bulkCopy.WriteToServer(dr);
										}
										catch (Exception ex)
										{
											// Add a little info to exception and re-throw.
											if (!ex.Data.Contains("SQL Bulk copy error"))
											{
												ex.Data.Add("SQL Bulk copy error", String.Format("Error occurred while importing table {0}.", tableName));
											}
											throw;
										}
									}
								}
							}
						}
						tran.Commit();
					}
					cn.Close();
				}
				catch
				{
					if (tran != null)
						tran.Rollback();

					throw;
				}
				finally
				{
					if (cn != null)
						cn.Dispose();
				}
			}
		}

		private static DataSet GenerateDataSet(string filePath)
		{
			DataSet ds = new DataSet("GalleryServerData");
			ds.ReadXml(filePath, XmlReadMode.Auto);

			return ds;
		}

		private static void ClearData(SqlConnection cn, SqlTransaction tran, bool deleteMembershipData, bool deleteGalleryData)
		{
			using (SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_DeleteData"), cn))
			{
				cmd.Transaction = tran;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@DeleteMembershipData", deleteMembershipData);
				cmd.Parameters.AddWithValue("@DeleteGalleryData", deleteGalleryData);
				cmd.ExecuteNonQuery();
			}
		}
	}
}

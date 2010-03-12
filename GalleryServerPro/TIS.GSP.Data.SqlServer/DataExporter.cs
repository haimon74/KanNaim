using System.Data;
using System.Data.SqlClient;

namespace GalleryServerPro.Data.SqlServer
{
	internal static partial class DataUtility
	{
		/// <summary>
		/// Exports the data from the SQL Server database to <paramref name="filePath"/>.
		/// </summary>
		/// <param name="filePath">The file name (including the path) to which to write.</param>
		/// <param name="exportMembershipData">if set to <c>true</c> export membership data.</param>
		/// <param name="exportGalleryData">if set to <c>true</c> export gallery data.</param>
		internal static void ExportData(string filePath, bool exportMembershipData, bool exportGalleryData)
		{
			DataSet ds = new DataSet("GalleryServerData");

			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			using (System.IO.Stream stream = asm.GetManifestResourceStream("GalleryServerPro.Data.SqlServer.GalleryServerProSchema.xml"))
			{
				ds.ReadXmlSchema(stream);
			}

			using (SqlConnection cn = SqlDataProvider.GetDbConnection())
			{
				if (cn.State == ConnectionState.Closed)
					cn.Open();

				if (exportMembershipData)
				{
					string[] aspnet_TableNames = new string[] { "aspnet_Applications", "aspnet_Membership", "aspnet_Profile", "aspnet_Roles", "aspnet_Users", "aspnet_UsersInRoles" };
					using (SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_ExportMembership"), cn))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						using (IDataReader dr = cmd.ExecuteReader())
						{
							ds.Load(dr, LoadOption.OverwriteChanges, aspnet_TableNames);
						}
					}
				}

				if (exportGalleryData)
				{
					string[] gs_TableNames = new string[] { "gs_Album", "gs_Gallery", "gs_MediaObject", "gs_MediaObjectMetadata", "gs_Role", "gs_Role_Album", "gs_AppError", "gs_SchemaVersion" };
					using (SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_ExportGalleryData"), cn))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						using (IDataReader dr = cmd.ExecuteReader())
						{
							ds.Load(dr, LoadOption.OverwriteChanges, gs_TableNames);
						}
					}

					ds.WriteXml(filePath, XmlWriteMode.WriteSchema);
					//ds.WriteXmlSchema(filePath);
				}
			}
		}
	}
}

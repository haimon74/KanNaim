using System.Data.SqlClient;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Contains database-related functionality.
	/// </summary>
  internal static partial class DataUtility
  {
		/// <summary>
		/// Get a reference to an unopened database connection.
		/// </summary>
		/// <returns>A SqlConnection object.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public static SqlConnection GetDBConnection()
    {
			string connString = Configuration.ConfigManager.GetConnectionString(GalleryServerPro.Provider.DataProviderManager.Provider.ConnectionStringName);
      SqlConnection cn = new SqlConnection(connString);

      return cn;
    }
	}
}

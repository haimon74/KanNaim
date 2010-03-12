using System;
using System.Data;
using System.Data.SqlClient;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using DataException=GalleryServerPro.ErrorHandler.CustomExceptions.DataException;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Contains functionality for persisting / retrieving error information to / from the SQL Server data store.
	/// </summary>
	internal class Error
	{
		private Error() { }

		#region Public Static Methods

		/// <summary>
		/// Return an <see cref="IDataReader" /> representing the application errors associated with the current gallery. If no records
		/// are found in the data store, an empty <see cref="IDataReader" /> is returned.
		/// </summary>
		/// <returns>Returns an <see cref="IDataReader" /> object with all error fields.</returns>
		internal static IDataReader GetDataReaderAppErrors()
		{
			IDataReader dr = GetCommandAppErrors().ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Persist the specified application error to the data store. Return the ID assigned to the error. Does not save if database
		/// is SQL Server 2000 or earlier; instead it just returns int.MinValue. (SQL Server 2000 has a max row length of 8000 bytes,
		/// and the error data very likely requires more than this.)
		/// </summary>
		/// <param name="appError">An instance of <see cref="IAppError" /> to persist to the data store. Must be a new error
		/// (AppErrorId == int.MinValue) that has not previously been saved to the data store.</param>
		/// <returns>Return the ID assigned to the error. The ID is also assigned to the AppErrorId property of <paramref name="appError"/>.</returns>
		/// <exception cref="DataException">Thrown when <see cref="IAppError.AppErrorId"/> is greater than <see cref="int.MinValue"/>.</exception>
		internal static int Save(IAppError appError)
		{
			if (Util.GetSqlVersion() < SqlVersion.Sql2005)
				return int.MinValue;

			PersistToDataStore(appError);

			return appError.AppErrorId;
		}

		/// <summary>
		/// Permanently delete the specified error from the data store. This action cannot be undone.
		/// </summary>
		/// <param name="appErrorId">The ID that uniquely identifies the <see cref="IAppError" /> to delete from the data store.</param>
		internal static void Delete(int appErrorId)
		{
			DeleteFromDataStore(appErrorId);
		}

		/// <summary>
		/// Permanently delete all errors from the data store. This action cannot be undone.
		/// </summary>
		internal static void DeleteAll()
		{
			DeleteAllFromDataStore();
		}

		#endregion

		#region Private Static Methods

		private static SqlCommand GetCommandAppErrors()
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AppErrorSelect"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static void PersistToDataStore(GalleryServerPro.Business.Interfaces.IAppError appError)
		{
			if (appError.AppErrorId == int.MinValue)
			{
				SqlCommand cmd = GetCommandErrorInsert(appError);
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
				cmd.Connection.Close();

				int ID = Convert.ToInt32(cmd.Parameters["@Identity"].Value, System.Globalization.NumberFormatInfo.CurrentInfo);

				if (appError.AppErrorId != ID)
					appError.AppErrorId = ID;
			}
			else
			{
				throw new DataException("Cannot save a previously existing application error to the data store.");
			}
		}

		private static SqlCommand GetCommandErrorInsert(IAppError appError)
		{
//INSERT [gs_AppError]
//  (FKGalleryId, TimeStamp, ExceptionType, Message, Source, TargetSite, StackTrace, ExceptionData, InnerExType, 
//  InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
//  FormVariables, Cookies, SessionVariables, ServerVariables)
//VALUES (@GalleryId, @TimeStamp, @ExceptionType, @Message, @Source, @TargetSite, @StackTrace, @ExceptionData, @InnerExType, 
//  @InnerExMessage, @InnerExSource, @InnerExTargetSite, @InnerExStackTrace, @InnerExData, @Url,
//  @FormVariables, @Cookies, @SessionVariables, @ServerVariables)
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AppErrorInsert"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			const int nvarcharMAX = -1; // The parameter length mapped to nvarchar(max) definitions

			cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = appError.GalleryId;
			cmd.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = appError.TimeStamp;
			cmd.Parameters.Add("@ExceptionType", SqlDbType.NVarChar, dataStoreConfig.ErrorExTypeLength).Value = appError.ExceptionType;
			cmd.Parameters.Add("@Message", SqlDbType.NVarChar, dataStoreConfig.ErrorExMsgLength).Value = appError.Message;
			cmd.Parameters.Add("@Source", SqlDbType.NVarChar, dataStoreConfig.ErrorExSourceLength).Value = appError.Source;
			cmd.Parameters.Add("@TargetSite", SqlDbType.NVarChar, nvarcharMAX).Value = appError.TargetSite;
			cmd.Parameters.Add("@StackTrace", SqlDbType.NVarChar, nvarcharMAX).Value = appError.StackTrace;
			cmd.Parameters.Add("@ExceptionData", SqlDbType.NVarChar, nvarcharMAX).Value = ErrorHandler.Error.Serialize(appError.ExceptionData);
			cmd.Parameters.Add("@InnerExType", SqlDbType.NVarChar, dataStoreConfig.ErrorExTypeLength).Value = appError.InnerExType;
			cmd.Parameters.Add("@InnerExMessage", SqlDbType.NVarChar, dataStoreConfig.ErrorExMsgLength).Value = appError.InnerExMessage;
			cmd.Parameters.Add("@InnerExSource", SqlDbType.NVarChar, dataStoreConfig.ErrorExSourceLength).Value = appError.InnerExSource;
			cmd.Parameters.Add("@InnerExTargetSite", SqlDbType.NVarChar, nvarcharMAX).Value = appError.InnerExTargetSite;
			cmd.Parameters.Add("@InnerExStackTrace", SqlDbType.NVarChar, nvarcharMAX).Value = appError.InnerExStackTrace;
			cmd.Parameters.Add("@InnerExData", SqlDbType.NVarChar, nvarcharMAX).Value = ErrorHandler.Error.Serialize(appError.InnerExData);
			cmd.Parameters.Add("@Url", SqlDbType.NVarChar, dataStoreConfig.ErrorUrlLength).Value = appError.Url;
			cmd.Parameters.Add("@FormVariables", SqlDbType.NVarChar, nvarcharMAX).Value = ErrorHandler.Error.Serialize(appError.FormVariables);
			cmd.Parameters.Add("@Cookies", SqlDbType.NVarChar, nvarcharMAX).Value = ErrorHandler.Error.Serialize(appError.Cookies);
			cmd.Parameters.Add("@SessionVariables", SqlDbType.NVarChar, nvarcharMAX).Value = ErrorHandler.Error.Serialize(appError.SessionVariables);
			cmd.Parameters.Add("@ServerVariables", SqlDbType.NVarChar, nvarcharMAX).Value = ErrorHandler.Error.Serialize(appError.ServerVariables);
			
			SqlParameter prm = new SqlParameter("@Identity", SqlDbType.Int, 0, "AppErrorId");
			prm.Direction = ParameterDirection.Output;
			cmd.Parameters.Add(prm);

			return cmd;
		}

		private static void DeleteAllFromDataStore()
		{
			SqlCommand cmd = GetCommandErrorDeleteAll();
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		private static void DeleteFromDataStore(int appErrorId)
		{
			SqlCommand cmd = GetCommandErrorDelete(appErrorId);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		private static SqlCommand GetCommandErrorDeleteAll()
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AppErrorDeleteAll"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int, 0, "GalleryId"));
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			return cmd;
		}

		private static SqlCommand GetCommandErrorDelete(int appErrorId)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_AppErrorDelete"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@AppErrorId", SqlDbType.Int, 0, "AppErrorId"));
			cmd.Parameters["@AppErrorId"].Value = appErrorId;

			return cmd;
		}

		#endregion
	}
}

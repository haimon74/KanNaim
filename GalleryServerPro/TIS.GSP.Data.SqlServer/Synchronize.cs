using System;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;

using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Data.SqlServer.Properties;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Contains functionality for persisting / retrieving synchronization information to / from the SQL Server data store.
	/// </summary>
	public static class Synchronize
	{
		#region Public Static Methods

		/// <summary>
		/// Persist the synchronization information to the data store.
		/// </summary>
		/// <param name="synchStatus">An <see cref="ISynchronizationStatus"/> object containing the synchronization information
		/// to persist to the data store.</param>
		/// <exception>Throws a GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException if the data
		/// store indicates another synchronization is already in progress for this gallery.</exception>
		public static void SaveStatus(ISynchronizationStatus synchStatus)
		{
			SqlCommand cmd = GetCommandSynchronizeSave(synchStatus);

			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			int returnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value, CultureInfo.InvariantCulture);
			cmd.Connection.Close();

			if (returnValue == 250000)
			{
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.SynchronizationInProgressException();
			}
		}

		/// <summary>
		/// Retrieve the most recent synchronization information from the data store and set the relevant properties
		/// on the <paramref name="synchStatus"/> parameter. The return value is the same reference as the parameter.
		/// </summary>
		/// <param name="synchStatus">An <see cref="ISynchronizationStatus"/> object to populate with the most recent synchronization
		/// information from the data store.</param>
		/// <returns>
		/// Returns an <see cref="ISynchronizationStatus"/> object with updated properties based on what was retrieved
		/// from the data store.
		/// </returns>
		public static ISynchronizationStatus UpdateStatusFromDataStore(ISynchronizationStatus synchStatus)
		{
			bool processedFirstRecord = false;

			using (IDataReader dr = GetDataReaderSynchronizeSelect(synchStatus))
			{
				while (dr.Read())
				{
					if (processedFirstRecord)
					{
						// We found more than one record matching the gallery ID in the Synchronize table. Throw exception.
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Synchronize_UpdateStatusFromDataStore_ExMsg1, synchStatus.GalleryId));
					}

					synchStatus.SynchId = dr["SynchId"].ToString();
					synchStatus.Status = (SynchronizationState)Enum.Parse(typeof(SynchronizationState), dr["SynchState"].ToString());
					synchStatus.TotalFileCount = (int)dr["TotalFiles"];
					synchStatus.CurrentFileIndex = (int)dr["CurrentFileIndex"];

					processedFirstRecord = true;
					break;
				}
			}

			if (!processedFirstRecord)
			{
				throw new System.Data.RowNotInTableException(string.Format(CultureInfo.CurrentCulture, Resources.Synchronize_UpdateStatusFromDataStore_ExMsg2, synchStatus.GalleryId));
			}

			return synchStatus;
		}
		
		#endregion

		#region Private Static Methods

		private static IDataReader GetDataReaderSynchronizeSelect(ISynchronizationStatus synchStatus)
		{
			IDataReader dr = GetCommandSynchronizeSelect(synchStatus).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		private static SqlCommand GetCommandSynchronizeSelect(ISynchronizationStatus synchStatus)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_SynchronizeSelect"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@GalleryId"].Value = synchStatus.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandSynchronizeSave(ISynchronizationStatus synchStatus)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_SynchronizeSave"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@SynchId", SqlDbType.NChar, 50));
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@SynchState", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@TotalFiles", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@CurrentFileIndex", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int));

			cmd.Parameters["@SynchId"].Value = synchStatus.SynchId;
			cmd.Parameters["@GalleryId"].Value = synchStatus.GalleryId;
			cmd.Parameters["@SynchState"].Value = synchStatus.Status;
			cmd.Parameters["@TotalFiles"].Value = synchStatus.TotalFileCount;
			cmd.Parameters["@CurrentFileIndex"].Value = synchStatus.CurrentFileIndex;
			cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.ReturnValue;

			return cmd;
		}
 
		#endregion
	}
}

using System;
using System.Data;
using System.Data.SqlClient;

using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using System.Collections.Generic;

namespace GalleryServerPro.Data.SqlServer
{
	/// <summary>
	/// Contains functionality for persisting / retrieving role information to / from the SQL Server data store.
	/// </summary>
	internal class Role
	{
		private Role() { }

		#region Public Static Methods

		/// <summary>
		/// Persist this gallery server role to the data store. The list of top-level albums this role applies to, which is stored
		/// in the <see cref="IGalleryServerRole.RootAlbumIds" /> property, is also saved. The <see cref="IGalleryServerRole.AllAlbumIds" /> 
		/// property is reloaded with the latest list of albums from the data store.
		/// </summary>
		/// <param name="role">An instance of <see cref="IGalleryServerRole" /> to persist to the data store.</param>
		public static void Save(IGalleryServerRole role)
		{
			PersistRoleAlbumRelationshipsToDataStore(role);

			PersistRoleToDataStore(role);

			ReloadAllAlbumIds(role);
		}

		/// <summary>
		/// Permanently delete this gallery server role from the data store, including the list of role/album relationships
		/// associated with this role. This action cannot be undone.
		/// </summary>
		/// <param name="role">An instance of <see cref="IGalleryServerRole" /> to delete from the data store.</param>
		public static void Delete(IGalleryServerRole role)
		{
			DeleteFromDataStore(role);
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the roles in the current gallery. If no matching objects
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing the roles in the current gallery.
		/// </returns>
		public static IDataReader GetDataReaderRoles()
		{
			IDataReader dr = GetCommandRoles().ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing the root album IDs associated with the specified role name. If no matching data
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="roleName">The role name for which root album IDs should be returned.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing the root album IDs associated with the specified role name.
		/// </returns>
		public static IDataReader GetDataReaderRoleRootAlbums(string roleName)
		{
			IDataReader dr = GetCommandRoleRootAlbumsByRoleName(roleName).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		/// <summary>
		/// Return an <see cref="System.Data.IDataReader"/> representing all album IDs associated with the specified role name. If no matching data
		/// are found in the data store, an empty <see cref="System.Data.IDataReader"/> is returned.
		/// </summary>
		/// <param name="roleName">The role name for which all album IDs should be returned.</param>
		/// <returns>
		/// Returns an <see cref="System.Data.IDataReader"/> object representing all album IDs associated with the specified role name.
		/// </returns>
		public static IDataReader GetDataReaderRoleAllAlbums(string roleName)
		{
			IDataReader dr = GetCommandRoleAllAlbumsByRoleName(roleName).ExecuteReader(CommandBehavior.CloseConnection);
			return dr;
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Save the list of root album IDs to the data store. The table gs_Role_Album contains one record for each role/album
		/// relationship. This procedure adds and deletes records as needed.
		/// </summary>
		/// <param name="role">The gallery server role containing the list of root Album IDs to persist to the data store.</param>
		private static void PersistRoleAlbumRelationshipsToDataStore(IGalleryServerRole role)
		{
			// Step 1: Copy the list of root album IDs to a new list. We'll be removing items from the list as we process them,
			// so we don't want to mess with the actual list attached to the object.
			List<int> roleAlbumRelationshipsToPersist = new List<int>();
			foreach (int albumId in role.RootAlbumIds)
			{
				roleAlbumRelationshipsToPersist.Add(albumId);
			}


			// Step 2: Get a datareader containing a list of all root album IDs in the data store. The result set contains a single
			// column of integers named "FKAlbumId".
			IDataReader dr = GetDataReaderRoleRootAlbums(role.RoleName);

			// Step 3: Iterate through each role/album relationship that is stored in the data store. If it is in our list, then
			// remove it from the list (see step 3 why). If not, the user must have unchecked it so add it to a list of 
			// relationships to be deleted.
			List<int> roleAlbumRelationshipsToDelete = new List<int>();
			while (dr.Read())
			{
				if (roleAlbumRelationshipsToPersist.Contains(dr.GetInt32(0)))
				{
					roleAlbumRelationshipsToPersist.Remove(dr.GetInt32(0));
				}
				else
				{
					roleAlbumRelationshipsToDelete.Add(dr.GetInt32(0));
				}
			}
			dr.Close();

			// Step 4: Delete the records we accumulated in our list.
			SqlCommand cmd = GetCommandGalleryServerRole_AlbumDelete(role);
			cmd.Connection.Open();
			foreach (int albumId in roleAlbumRelationshipsToDelete)
			{
				cmd.Parameters["@AlbumId"].Value = albumId;
				cmd.ExecuteNonQuery();
			}
			cmd.Connection.Close();

			// Step 5: Any items still left in the roleAlbumRelationshipsToPersist list must be new ones checked by the user. Add them.
			cmd = GetCommandGalleryServerRole_AlbumInsert(role);
			cmd.Connection.Open();
			foreach (int albumId in roleAlbumRelationshipsToPersist)
			{
				cmd.Parameters["@AlbumId"].Value = albumId;
				cmd.ExecuteNonQuery();
			}

			cmd.Connection.Close();
		}

		private static void PersistRoleToDataStore(IGalleryServerRole role)
		{
			// The update stored procedure will automatically call the insert stored procedure if it does not 
			// find a matching role to update.
			SqlCommand cmd = GetCommandGalleryServerRoleUpdate(role);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		/// <summary>
		/// Permanently delete the specified gallery server role from the data store. The stored procedure deletes the record
		/// in the gs_Role table corresponding to this role and also all records in the gs_Role_Album table that reference
		/// this role.
		/// </summary>
		/// <param name="role">An instance of IGalleryServerRole to delete from the data store.</param>
		private static void DeleteFromDataStore(IGalleryServerRole role)
		{
			SqlCommand cmd = GetCommandGalleryServerRoleDelete(role);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		private static SqlCommand GetCommandGalleryServerRoleUpdate(IGalleryServerRole role)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_RoleUpdate"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, dataStoreConfig.RoleNameLength));
			cmd.Parameters.Add(new SqlParameter("@AllowViewAlbumsAndObjects", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowViewOriginalImage", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowAddChildAlbum", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowAddMediaObject", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowEditAlbum", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowEditMediaObject", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowDeleteChildAlbum", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowDeleteMediaObject", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowSynchronize", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@HideWatermark", SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@AllowAdministerSite", SqlDbType.Bit));

			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;
			cmd.Parameters["@RoleName"].Value = role.RoleName;
			cmd.Parameters["@AllowViewAlbumsAndObjects"].Value = role.AllowViewAlbumOrMediaObject;
			cmd.Parameters["@AllowViewOriginalImage"].Value = role.AllowViewOriginalImage;
			cmd.Parameters["@AllowAddChildAlbum"].Value = role.AllowAddChildAlbum;
			cmd.Parameters["@AllowAddMediaObject"].Value = role.AllowAddMediaObject;
			cmd.Parameters["@AllowEditAlbum"].Value = role.AllowEditAlbum;
			cmd.Parameters["@AllowEditMediaObject"].Value = role.AllowEditMediaObject;
			cmd.Parameters["@AllowDeleteChildAlbum"].Value = role.AllowDeleteChildAlbum;
			cmd.Parameters["@AllowDeleteMediaObject"].Value = role.AllowDeleteMediaObject;
			cmd.Parameters["@AllowSynchronize"].Value = role.AllowSynchronize;
			cmd.Parameters["@HideWatermark"].Value = role.HideWatermark;
			cmd.Parameters["@AllowAdministerSite"].Value = role.AllowAdministerSite;

			return cmd;
		}

		private static SqlCommand GetCommandGalleryServerRoleDelete(IGalleryServerRole role)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_RoleDelete"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, dataStoreConfig.RoleNameLength));

			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;
			cmd.Parameters["@RoleName"].Value = role.RoleName;

			return cmd;
		}

		private static SqlCommand GetCommandGalleryServerRole_AlbumDelete(IGalleryServerRole role)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_Role_AlbumDelete"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, dataStoreConfig.RoleNameLength));
			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int));

			cmd.Parameters["@RoleName"].Value = role.RoleName;

			return cmd;
		}

		private static SqlCommand GetCommandGalleryServerRole_AlbumInsert(IGalleryServerRole role)
		{
			DataStore dataStoreConfig = ConfigManager.GetGalleryServerProConfigSection().DataStore;

			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_Role_AlbumInsert"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, dataStoreConfig.RoleNameLength));
			cmd.Parameters.Add(new SqlParameter("@AlbumId", SqlDbType.Int));

			cmd.Parameters["@RoleName"].Value = role.RoleName;

			return cmd;
		}

		private static SqlCommand GetCommandRoles()
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_RoleSelect"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandRoleRootAlbumsByRoleName(string roleName)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_Role_AlbumSelectRootAlbumsByRoleName"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, 256));
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@RoleName"].Value = roleName;
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static SqlCommand GetCommandRoleAllAlbumsByRoleName(string roleName)
		{
			SqlCommand cmd = new SqlCommand(Util.GetSqlName("gs_Role_AlbumSelectAllAlbumsByRoleName"), SqlDataProvider.GetDbConnection());
			cmd.CommandType = CommandType.StoredProcedure;

			// Add parameters
			cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, 256));
			cmd.Parameters.Add(new SqlParameter("@GalleryId", SqlDbType.Int));

			cmd.Parameters["@RoleName"].Value = roleName;
			cmd.Parameters["@GalleryId"].Value = ConfigManager.GetGalleryServerProConfigSection().Core.GalleryId;

			cmd.Connection.Open();

			return cmd;
		}

		private static void ReloadAllAlbumIds(IGalleryServerRole role)
		{
			role.ClearAllAlbumIds();

			IDataReader dr = GetDataReaderRoleAllAlbums(role.RoleName);
			while (dr.Read())
			{
				role.AddToAllAlbumIds(dr.GetInt32(0));
			}
			dr.Close();
		}

		#endregion
	}
}

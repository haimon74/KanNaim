using System;
using System.Net.Configuration;
using System.Web.Configuration;

namespace GalleryServerPro.Configuration
{
	/// <summary>
	/// Provides methods for read/write access to the Gallery Server Pro config file (galleryserverpro.config).
	/// </summary>
	public static class ConfigManager
	{
		#region Private Static Fields
		
    private static GalleryServerProConfigSettings _galleryServerProConfigSection;

		#endregion

		#region Constructors

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Return the connection string to the data store for Gallery Server Pro. Returns null if no connection string is
		/// specified, such as when a data provider is used that does not need a connection string.
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		/// <returns>
		/// Returns a string that can be used to connect to the Gallery Server Pro data store.
		/// </returns>
		public static string GetConnectionString(string connectionStringName)
		{
			System.Configuration.ConnectionStringSettings connectionStringsSection;
			if (System.Web.HttpContext.Current == null)
			{
				connectionStringsSection = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];
			}
			else
			{
				connectionStringsSection = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[connectionStringName];
			}
			
			if (connectionStringsSection == null)
				throw new System.Configuration.ConfigurationErrorsException(String.Format("The <connectionStrings> section in web.config does not have a connection string named {0}.", connectionStringName));

			return connectionStringsSection.ConnectionString;
		}

		/// <summary>
		/// Returns a read-only reference to the galleryServerPro custom configuration section in galleryserverpro.config.
		/// </summary>
    /// <returns>Returns a <see cref="GalleryServerPro.Configuration.GalleryServerProConfigSettings" /> object.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public static GalleryServerProConfigSettings GetGalleryServerProConfigSection()
		{
			if (_galleryServerProConfigSection == null)
			{
				if (System.Web.HttpContext.Current == null)
				{
					_galleryServerProConfigSection = (GalleryServerProConfigSettings)System.Configuration.ConfigurationManager.GetSection("galleryServerPro");
				}
				else
				{
					_galleryServerProConfigSection = (GalleryServerProConfigSettings)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/galleryServerPro");
				}
			}

			return _galleryServerProConfigSection;
		}

		/// <summary>
		/// Returns a reference to a read/write instance of the galleryServerPro custom configuration section in galleryserverpro.config.
		/// To save changes to the config file, call the <see cref="System.Configuration.Configuration.Save" /> method on the <see cref="System.Configuration.Configuration" /> object
		/// used in the parameter list. For web applications, one can use System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
		/// to get a reference to the Configuration object to use for the parameter.
		/// </summary>
		/// <param name="configuration">An instance of a <see cref="System.Configuration.Configuration" /> object.</param>
		/// <returns>Returns a read/write <see cref="GalleryServerPro.Configuration.GalleryServerProConfigSettings" /> object.</returns>
		public static GalleryServerProConfigSettings OpenGalleryServerProConfigSection(System.Configuration.Configuration configuration)
		{
			return (GalleryServerProConfigSettings)configuration.GetSection("system.web/galleryServerPro");
		}

		/// <summary>
		/// Returns a reference to a read/write instance of the membership configuration section in web.config.
		/// To save changes to the config file, call the <see cref="System.Configuration.Configuration.Save" /> method on the <see cref="System.Configuration.Configuration" /> object
		/// used in the parameter list. For web applications, one can use System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
		/// to get a reference to the Configuration object to use for the parameter.
		/// </summary>
		/// <param name="configuration">An instance of a <see cref="System.Configuration.Configuration" /> object.</param>
		/// <returns>Returns a read/write <see cref="GalleryServerPro.Configuration.GalleryServerProConfigSettings" /> object.</returns>
		public static MembershipSection OpenMembershipConfigSection(System.Configuration.Configuration configuration)
		{
			return (MembershipSection)configuration.GetSection("system.web/membership");
		}

		/// <summary>
		/// Returns a reference to a read/write instance of the roleManager configuration section in web.config.
		/// To save changes to the config file, call the <see cref="System.Configuration.Configuration.Save" /> method on the <see cref="System.Configuration.Configuration" /> object
		/// used in the parameter list. For web applications, one can use System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
		/// to get a reference to the Configuration object to use for the parameter.
		/// </summary>
		/// <param name="configuration">An instance of a <see cref="System.Configuration.Configuration" /> object.</param>
		/// <returns>Returns a read/write <see cref="GalleryServerPro.Configuration.GalleryServerProConfigSettings" /> object.</returns>
		public static RoleManagerSection OpenRoleConfigSection(System.Configuration.Configuration configuration)
		{
			return (RoleManagerSection)configuration.GetSection("system.web/roleManager");
		}

		/// <summary>
		/// Returns a reference to a read/write instance of the profile configuration section in web.config.
		/// To save changes to the config file, call the <see cref="System.Configuration.Configuration.Save" /> method on the <see cref="System.Configuration.Configuration" /> object
		/// used in the parameter list. For web applications, one can use System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
		/// to get a reference to the Configuration object to use for the parameter.
		/// </summary>
		/// <param name="configuration">An instance of a <see cref="System.Configuration.Configuration" /> object.</param>
		/// <returns>Returns a read/write <see cref="GalleryServerPro.Configuration.GalleryServerProConfigSettings" /> object.</returns>
		public static ProfileSection OpenProfileConfigSection(System.Configuration.Configuration configuration)
		{
			return (ProfileSection)configuration.GetSection("system.web/profile");
		}

		#endregion
	}
}

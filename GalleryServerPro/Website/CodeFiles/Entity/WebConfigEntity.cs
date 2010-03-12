namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains several configuration settings in web.config.
	/// This entity is designed to be an updateable object whose properties can be changed and passed to the 
	/// <see cref="GalleryServerPro.Web.Controller.WebConfigController"/> for persisting back to the configuration file on disk.
	/// Therefore, this entity is typically used only in scenarios where we must persist changes to the config file, such as 
	/// in the Install Wizard.
	/// </summary>
	public class WebConfigEntity
	{
		public const string SqlServerConnectionStringName = Constants.SQL_SERVER_CN_STRING_NAME;
		public const string SQLiteConnectionStringName = Constants.SQLITE_CN_STRING_NAME;
		public string SqlServerConnectionStringValue;
		public string SQLiteConnectionStringValue;
		public string MembershipDefaultProvider;
		public string RoleDefaultProvider;
		public string ProfileDefaultProvider;
	}
}

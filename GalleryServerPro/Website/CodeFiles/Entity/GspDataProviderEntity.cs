namespace GalleryServerPro.Web.Entity
{
	/// <summary>
	/// A simple object that contains the configuration settings in the &lt;dataProvider .../&gt; section of galleryserverpro.config.
	/// This entity is designed to be an updateable object whose properties can be changed and passed to the 
	/// <see cref="GalleryServerPro.Web.Controller.GspConfigController"/> for persisting back to the configuration file on disk.
	/// Therefore, this entity is typically used only in scenarios where we must persist changes to the config file, such as 
	/// in the Install Wizard. If the custom configuration section <see cref="GalleryServerPro.Configuration.DataProvider"/> were 
	/// updateable, this entity would have been unnecessary.
	/// </summary>
	public class GspDataProviderEntity
	{
		public string defaultProvider;
	}
}

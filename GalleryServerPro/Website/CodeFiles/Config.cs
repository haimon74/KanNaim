using GalleryServerPro.Configuration;

namespace GalleryServerPro.Web
{
	/// <summary>
	/// Contains functionality related to the configuration data.
	/// </summary>
	public static class Config
	{
		/// <summary>
		/// Returns a read only reference to the &lt;core&gt; configuration section in galleryserverpro.config.
		/// </summary>
		/// <returns>Returns a read only reference to the &lt;core&gt; configuration section in galleryserverpro.config.</returns>
		public static Core GetCore()
		{
			return ConfigManager.GetGalleryServerProConfigSection().Core;
		}

		/// <summary>
		/// Returns a read only reference to the &lt;galleryObject&gt; configuration section in galleryserverpro.config.
		/// </summary>
		/// <returns>Returns a read only reference to the &lt;galleryObject&gt; configuration section in galleryserverpro.config.</returns>
		public static GalleryObject GetGalleryObject()
		{
			return ConfigManager.GetGalleryServerProConfigSection().GalleryObject;
		}

		/// <summary>
		/// Returns a read only reference to the &lt;dataStore&gt; configuration section in galleryserverpro.config.
		/// </summary>
		/// <returns>Returns a read only reference to the &lt;dataStore&gt; configuration section in galleryserverpro.config.</returns>
		public static DataStore GetDataStore()
		{
			return ConfigManager.GetGalleryServerProConfigSection().DataStore;
		}

		/// <summary>
		/// Returns a read only reference to the &lt;dataProvider&gt; configuration section in galleryserverpro.config.
		/// </summary>
		/// <returns>Returns a read only reference to the &lt;dataProvider&gt; configuration section in galleryserverpro.config.</returns>
		public static DataProvider GetDataProvider()
		{
			return ConfigManager.GetGalleryServerProConfigSection().DataProvider;
		}
	}
}

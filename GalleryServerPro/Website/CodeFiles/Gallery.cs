using System;
using System.Web.UI;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web
{
	/// <summary>
	/// The top level user control that acts as a container for other user controls used in Gallery Server Pro.
	/// </summary>
	[ToolboxData("<{0}:Gallery runat=\"server\"></{0}:Gallery>")]
	public class Gallery : UserControl
	{
		#region Private Fields

		private bool? _showLogin;
		private bool? _showSearch;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Gallery"/> class.
		/// </summary>
		public Gallery()
		{
			this.Init += Gallery_Init;
		}

		static Gallery()
		{
		}

		#endregion

		#region Event Handlers

		void Gallery_Init(object sender, EventArgs e)
		{
			this.ID = "gsp";

			// Set up our "global" error handling. Since every page in GSP passes through this event handler, attaching error handling 
			// code to the page's Error event handler is roughly equivalent to the global error handling in web.config. We prefer to
			// set up our error handling this way so as not to interfere with the user's own error handling configuration she may be
			// using.
			this.Page.Error += Gallery_Error;

			// Check the query string for the desired page and add it to the page's controls.
			this.LoadRequestedPage();
		}

		void Gallery_Error(object sender, EventArgs e)
		{
			// Grab a handle to the exception that is giving us grief.
			Exception ex = Server.GetLastError();

			if (Context != null)
			{
				ex = Context.Error;
			}

			AppErrorController.HandleGalleryException(ex);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Indicates whether to show the login controls at the top right of each page. When false, no login controls
		/// are shown, but the user can navigate directly to the login page to log on. If not specified, this
		/// property inherits the value in the config file. 
		/// </summary>
		/// <value><c>true</c> if login controls are visible; otherwise, <c>false</c>.</value>
		public bool ShowLogin
		{
			get
			{
				if (!_showLogin.HasValue)
					this._showLogin = Config.GetCore().ShowLogin;

				return _showLogin.Value;
			}
			set
			{
				this._showLogin = value;
			}
		}

		/// <summary>
		/// Indicates whether to show the search box at the top right of each page. If not specified, this
		/// property inherits the value in the config file. 
		/// </summary>
		/// <value><c>true</c> if the search box is visible; otherwise, <c>false</c>.</value>
		public bool ShowSearch
		{
			get
			{
				if (!_showSearch.HasValue)
					this._showSearch = Config.GetCore().ShowSearch;

				return _showSearch.Value;
			}
			set
			{
				this._showSearch = value;
			}
		}

		#endregion

		#region Private Methods

		private void LoadRequestedPage()
		{
			// Check query string for a requested page. If present, load up that page. Otherwise, default to the album view.

			PageId page = Util.GetPage();

			string src = String.Concat(Util.GalleryRoot, "/pages/", page, ".ascx");
			if (src.IndexOf("/admin_") >= 0)
				src = src.Replace("/admin_", "/admin/");
			if (src.IndexOf("/error") >= 0)
				src = src.Replace("/error_", "/error/");
			if (src.IndexOf("/task_") >= 0)
				src = src.Replace("/task_", "/task/");

			try
			{
				Control control = LoadControl(src);

				// If the control is an instance of Pages.GalleryPage, then assign its GalleryPage property to the current instance.
				// This gives the control convenient access to the Page property.
				Pages.GalleryPage galleryControl = control as Pages.GalleryPage;
				if (galleryControl != null)
				{
					galleryControl.GalleryControl = this;
				}

				this.Controls.Add(control);
			}
			catch (System.IO.FileNotFoundException)
			{
				throw new ApplicationException(String.Format(Resources.GalleryServerPro.Error_Cannot_Load_User_Control_Ex_Msg, src));
			}
		}

		#endregion
	}
}

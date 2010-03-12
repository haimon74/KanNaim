using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GalleryServerPro.Configuration;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.Pages
{
	/// <summary>
	/// The base class that is used for administration pages.
	/// </summary>
	public abstract class AdminPage : Pages.GalleryPage
	{
		#region Private Fields

		private PlaceHolder _phAdminHeader;
		private Controls.admin.adminheader _adminHeader;
		private PlaceHolder _phAdminFooter;
		private Controls.admin.adminfooter _adminFooter;
		private System.Configuration.Configuration _config;
		private GalleryServerProConfigSettings _gspConfig;
		private GspCoreEntity _coreConfig;
		private bool? _hasEditConfigPermission;
		private Controls.admin.adminmenu _adminMenu;
		private bool isDoublePostBack;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AdminPage"/> class.
		/// </summary>
		protected AdminPage()
		{
			//this.Init += AdminPage_Init;
			this.Load += AdminPage_Load;
			this.BeforeHeaderControlsAdded += AdminPage_BeforeHeaderControlsAdded;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the location for the <see cref="Controls.admin.adminheader"/> user control. Classes that inherit 
		/// <see cref="Pages.AdminPage"/> should set this property to the <see cref="PlaceHolder"/> that is to contain
		/// the admin header control. If this property is not assigned by the inheriting class, the admin header control
		/// is not added to the page output.
		/// </summary>
		/// <value>The <see cref="Controls.admin.adminheader"/> user control.</value>
		public PlaceHolder AdminHeaderPlaceHolder
		{
			get
			{
				return this._phAdminHeader;
			}
			set
			{
				this._phAdminHeader = value;
			}
		}

		/// <summary>
		/// Gets the admin header user control that is rendered near the top of the administration pages. This control contains the 
		/// page title and the top Save/Cancel buttons. (The bottom Save/Cancel buttons are in the <see cref="Controls.admin.adminfooter"/> user control.
		/// </summary>
		/// <value>The admin header user control that is rendered near the top of the administration pages.</value>
		public Controls.admin.adminheader AdminHeader
		{
			get
			{
				return this._adminHeader;
			}
		}

		/// <summary>
		/// Gets or sets the location for the <see cref="Controls.admin.adminfooter"/> user control. Classes that inherit 
		/// <see cref="Pages.AdminPage"/> should set this property to the <see cref="PlaceHolder"/> that is to contain
		/// the admin footer control. If this property is not assigned by the inheriting class, the admin footer control
		/// is not added to the page output.
		/// </summary>
		/// <value>The <see cref="Controls.admin.adminfooter"/> user control.</value>
		public PlaceHolder AdminFooterPlaceHolder
		{
			get
			{
				return this._phAdminFooter;
			}
			set
			{
				this._phAdminFooter = value;
			}
		}

		/// <summary>
		/// Gets the admin footer user control that is rendered near the bottom of the administration pages. This control contains the 
		/// bottom Save/Cancel buttons. (The top Save/Cancel buttons are in the <see cref="Controls.admin.adminheader"/> user control.
		/// </summary>
		/// <value>The admin footer user control that is rendered near the bottom of the administration pages.</value>
		public Controls.admin.adminfooter AdminFooter
		{
			get
			{
				return this._adminFooter;
			}
		}

		/// <summary>
		/// Gets / sets the page title text (e.g. Site Settings - General).
		/// </summary>
		public string AdminPageTitle
		{
			get
			{
				return this._adminHeader.AdminPageTitle;
			}
			set
			{
				this._adminHeader.AdminPageTitle = value;
			}
		}

		/// <summary>
		/// Gets / sets the text that appears on the top and bottom Ok buttons on the page. This is rendered as the value
		/// attribute of the input HTML tag.
		/// </summary>
		public string OkButtonText
		{
			get
			{
				return this.AdminHeader.OkButtonText;
			}
			set
			{
				this.AdminHeader.OkButtonText = value;
				this.AdminFooter.OkButtonText = value;
			}
		}

		/// <summary>
		/// Gets / sets the ToolTip for the top and bottom Ok buttons on the page. The ToolTip is rendered as 
		/// the title attribute of the input HTML tag.
		/// </summary>
		public string OkButtonToolTip
		{
			get
			{
				return this.AdminHeader.OkButtonToolTip;
			}
			set
			{
				this.AdminHeader.OkButtonToolTip = value;
				this.AdminFooter.OkButtonToolTip = value;
			}
		}

		/// <summary>
		/// Gets / sets the text that appears on the top and bottom Cancel buttons on the page. This is rendered as the value
		/// attribute of the input HTML tag.
		/// </summary>
		public string CancelButtonText
		{
			// This is the text that appears on the top and bottom Cancel buttons.
			get
			{
				return this.AdminHeader.CancelButtonText;
			}
			set
			{
				this.AdminHeader.CancelButtonText = value;
				this.AdminFooter.CancelButtonText = value;
			}
		}

		/// <summary>
		/// Gets / sets the ToolTip for the top and bottom Cancel buttons on the page. The ToolTip is rendered as 
		/// the title attribute of the HTML tag.
		/// </summary>
		public string CancelButtonToolTip
		{
			get
			{
				return this.AdminHeader.CancelButtonToolTip;
			}
			set
			{
				this.AdminHeader.CancelButtonToolTip = value;
				this.AdminFooter.CancelButtonToolTip = value;
			}
		}

		/// <summary>
		/// Gets / sets the visibility of the top and bottom Ok buttons on the page. When true, the buttons
		/// are visible. When false, they are not visible (not rendered in the page output.)
		/// </summary>
		public bool OkButtonIsVisible
		{
			get
			{
				return this.AdminHeader.OkButtonIsVisible;
			}
			set
			{
				this.AdminHeader.OkButtonIsVisible = value;
				this.AdminFooter.OkButtonIsVisible = value;
			}
		}

		/// <summary>
		/// Gets / sets the visibility of the top and bottom Cancel buttons on the page. When true, the buttons
		/// are visible. When false, they are not visible (not rendered in the page output.)
		/// </summary>
		public bool CancelButtonIsVisible
		{
			get
			{
				return this.AdminHeader.CancelButtonIsVisible;
			}
			set
			{
				this.AdminHeader.CancelButtonIsVisible = value;
				this.AdminFooter.CancelButtonIsVisible = value;
			}
		}

		/// <summary>
		/// Gets a reference to the top button that initiates the completion of the task.
		/// </summary>
		public Button OkButtonTop
		{
			get
			{
				return this.AdminHeader.OkButtonTop;
			}
		}

		/// <summary>
		/// Gets a reference to the bottom button that initiates the completion of the task.
		/// </summary>
		public Button OkButtonBottom
		{
			get
			{
				return this.AdminFooter.OkButtonBottom;
			}
		}

		/// <summary>
		/// Gets a reference to an <see cref="GspCoreEntity"/> that exposes the &lt;core...&gt; section of the galleryServerPro 
		/// custom configuration section in galleryserverpro.config.
		/// </summary>
		/// <value>A reference to an <see cref="GspCoreEntity"/> that exposes the &lt;core...&gt; section of the galleryServerPro 
		/// custom configuration section in galleryserverpro.config.</value>
		public GspCoreEntity CoreConfig
		{
			get
			{
				if (this._coreConfig == null)
				{
					this._coreConfig = Controller.GspConfigController.GetGspCoreEntity(Web.Config.GetCore());
				}

				return this._coreConfig;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the configuration for this application can be programmatically updated. This property will be
		/// false when the IIS worker process does not have 'write' NTFS permission. When true, galleryserverpro.config can be 
		/// modified by the application.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the configuration file can be programmatically updated; otherwise, <c>false</c>.
		/// </value>
		public bool HasEditConfigPermission
		{
			get
			{
				if (!this._hasEditConfigPermission.HasValue)
				{
					try
					{
						// Can we open the galleryserverpro.config file for writing? This will fail when the galleryserverpro.config file does not have 'write' NTFS permission.
						using (System.IO.File.OpenWrite(Util.GalleryServerProConfigFilePath)) { }

						this._hasEditConfigPermission = true;
					}
					catch(UnauthorizedAccessException)
					{
						this._hasEditConfigPermission = false;
					}
				}

				return this._hasEditConfigPermission.Value;
			}
		}

		/// <summary>
		/// Gets a reference to the <see cref="Controls.admin.adminmenu"/> control on the page.
		/// </summary>
		/// <value>The <see cref="Controls.admin.adminmenu"/> control on the page.</value>
		public Controls.admin.adminmenu AdminMenu
		{
			get
			{
				return this._adminMenu;
			}
		}

		#endregion

		#region Event Handlers

		void AdminPage_Load(object sender, EventArgs e)
		{
			AddUserControls();

			ConfigureControls();
		}

		protected void AdminPage_BeforeHeaderControlsAdded(object sender, EventArgs e)
		{
			// Add the admin menu to the page. Note that if you use any index other than 0 in the AddAt method, the viewstate
			// is not preserved across postbacks. This is the reason why the <see cref="BeforeHeaderControlsAdded"/> event was created in 
			// <see cref="GalleryPage"/> and handled here. We need to add the admin menu *before* <see cref="GalleryPage"/> adds the album breadcrumb
			// menu and the gallery header.
			Controls.admin.adminmenu adminMenu = (Controls.admin.adminmenu)LoadControl(Util.GetUrl("/controls/admin/adminmenu.ascx"));
			this._adminMenu = adminMenu;
			this.Controls.AddAt(0, adminMenu);
			//this.Controls.AddAt(Controls.IndexOf(AlbumMenu) + 1, adminMenu); // Do not use: viewstate is not preserved
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Generates script that executes the <paramref name="scriptToRun"/> after the DOM is fully loaded. This is needed
		/// because simply passing the script to ScriptManager.RegisterStartupScript may cause it to run before the DOM is 
		/// fully initialized. This method should only be called once for the page because it hard-codes a javascript function
		/// named adminPageLoad.
		/// </summary>
		/// <param name="scriptToRun">The script to run.</param>
		/// <returns>Returns a string that can be passed to the ScriptManager.RegisterStartupScript method. Does not include
		/// the script tags.</returns>
		protected static string GetPageLoadScript(string scriptToRun)
		{
			return String.Format(CultureInfo.InvariantCulture,
@"
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(adminPageLoad);

function adminPageLoad(sender, args)
{{
	{0}
}}
",
				scriptToRun);
		}

		#endregion

		#region Private Methods

		private void AddUserControls()
		{
			Controls.admin.adminheader adminHeader = (Controls.admin.adminheader)LoadControl(Util.GetUrl("/controls/admin/adminheader.ascx"));
			this._adminHeader = adminHeader;
			if (this.AdminHeaderPlaceHolder != null)
				this.AdminHeaderPlaceHolder.Controls.Add(adminHeader);

			Controls.admin.adminfooter adminFooter = (Controls.admin.adminfooter)LoadControl(Util.GetUrl("/controls/admin/adminfooter.ascx"));
			this._adminFooter = adminFooter;
			if (this.AdminFooterPlaceHolder != null)
				this.AdminFooterPlaceHolder.Controls.Add(adminFooter);
		}

		private void ConfigureControls()
		{
			if ((this.AdminHeaderPlaceHolder != null) && (this.AdminHeader != null) && (this.AdminHeader.OkButtonTop != null))
				this.Page.Form.DefaultButton = this.AdminHeader.OkButtonTop.UniqueID;
		}

		#endregion
	}
}

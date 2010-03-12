using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Web.Master
{
	public partial class adminMstr : System.Web.UI.MasterPage
	{
		#region Private Fields
		
		private System.Configuration.Configuration _config;
		private GalleryServerProConfigSettings _gspConfig;
		private Core _coreConfig;
		private bool? _hasEditConfigPermission;
		
		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControlsEveryTime();

			if (!Page.IsPostBack)
				ConfigureControls();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			((GspPage)this.Page).RedirectToReferringPage();
		}
		
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets / sets the page title text (e.g. Site Settings - General).
		/// </summary>
		public string AdminPageTitle
		{
			get
			{
				return lblAdminPageHeader.Text;
			}
			set
			{
				lblAdminPageHeader.Text = value;
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
				return btnOkTop.Text;
			}
			set
			{
				// Ensure value is less than 25 characters.
				string btnText = value.PadRight(25).Substring(0, 25).Trim();
				btnOkTop.Text = btnText;
				btnOkBottom.Text = btnText;
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
				return btnOkTop.ToolTip;
			}
			set
			{
				// Ensure value is less than 250 characters.
				string tooltipText = value.PadRight(250).Substring(0, 250).Trim();
				btnOkTop.ToolTip = tooltipText;
				btnOkBottom.ToolTip = tooltipText;
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
				return btnCancelTop.Text;
			}
			set
			{
				// Ensure value is less than 25 characters.
				string btnText = value.PadRight(25).Substring(0, 25).Trim();
				btnCancelTop.Text = btnText;
				btnCancelBottom.Text = btnText;
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
				return btnCancelTop.ToolTip;
			}
			set
			{
				// Ensure value is less than 250 characters.
				string tooltipText = value.PadRight(250).Substring(0, 250).Trim();
				btnCancelTop.ToolTip = tooltipText;
				btnCancelBottom.ToolTip = tooltipText;
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
				return btnOkTop.Visible;
			}
			set
			{
				btnOkTop.Visible = value;
				btnOkBottom.Visible = value;
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
				return btnCancelTop.Visible;
			}
			set
			{
				btnCancelTop.Visible = value;
				btnCancelBottom.Visible = value;
			}
		}

		/// <summary>
		/// Gets a reference to the top button that initiates the saving of the page settings.
		/// </summary>
		public Button OkButtonTop
		{
			get
			{
				return btnOkTop;
			}
		}

		/// <summary>
		/// Gets a reference to the bottom button that initiates the saving of the page settings.
		/// </summary>
		public Button OkButtonBottom
		{
			get
			{
				return btnOkBottom;
			}
		}
		
		public GalleryServerProConfigSettings GspConfig
		{
			get
			{
				if (this._gspConfig == null)
				{
					if (HasEditConfigPermission)
					{
						this._gspConfig = GalleryServerPro.Configuration.ConfigManager.OpenGalleryServerProConfigSection(this._config);
					}
					else
					{
						this._gspConfig = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection();
					}
				}

				return this._gspConfig;
			}
		}

		public Core CoreConfig
		{
			get
			{
				if (this._coreConfig == null)
				{
					this._coreConfig = this.GspConfig.Core;
				}

				return this._coreConfig;
			}
		}

		public System.Configuration.Configuration Config
		{
			get
			{
				if (this.HasEditConfigPermission)
				{
					return this._config;
				}
				else
				{
					return null;
				}
			}
		}

		public bool HasEditConfigPermission
		{
			get
			{
				if (!this._hasEditConfigPermission.HasValue)
				{
					try
					{
						_config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
						this._hasEditConfigPermission = true;
					}
					catch (Exception ex)
					{
						if ((ex is System.Security.SecurityException) || ((ex.InnerException != null) && (ex.InnerException is System.Security.SecurityException)))
						{
							this._hasEditConfigPermission = false;
						}
						else
						{
							throw;
						}
					}
				}

				return this._hasEditConfigPermission.Value;
			}
		}

		#endregion

		#region Public Methods

		//public void VerifyUserHasConfigurationPermission()
		//{
		//  try
		//  {
		//    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		//  }
		//  catch (System.Security.SecurityException)
		//  {
		//    Response.Redirect("manageusers.aspx?msg=1", false);
		//  }
		//  catch (System.Configuration.ConfigurationErrorsException ex)
		//  {
		//    if ((ex.InnerException != null) && (ex.InnerException is System.Security.SecurityException))
		//    {
		//      Response.Redirect("manageusers.aspx?msg=1", false);
		//    }
		//  }

		//}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			// Modify the content div tag so that it has a left margin to allow for the navbar on the left.
			HtmlGenericControl contentDiv = this.Master.ContentDiv;
			contentDiv.Attributes["class"] = "indentedContent";
		}

		private void ConfigureControlsEveryTime()
		{
			this.Page.Form.DefaultButton = btnOkBottom.UniqueID;
		}
		
		#endregion
	}
}

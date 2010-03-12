using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Web.Controller;
using System.Collections.Generic;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class mediaobjects : Pages.AdminPage
	{
		private bool _validateReadOnlyGalleryHasExecuted;
		private bool _validateReadOnlyGalleryResult;
		private readonly Dictionary<String, bool> _pathsThatHaveBeenTestedForWriteability = new Dictionary<string, bool>();

		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			ConfigureControlsEveryTime();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				SaveSettings();
			}

			return true;
		}

		protected bool wwDataBinder_ValidateControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// Validate various settings to make sure they don't conflict with each other.

			// Validate the media object path.
			if (item.ControlInstance == this.txtMoPath)
			{
				if (this.chkPathIsReadOnly.Checked)
					return ValidateReadOnlyGallery(item) && ValidatePathIsReadable(item, this.txtMoPath.Text, this.lblMoPath);
				else
					return ValidatePathIsWriteable(item, this.txtMoPath.Text, this.lblMoPath);
			}

				// Validate the media object thumbnail path.
			else if (item.ControlInstance == this.txtThumbnailCachePath)
			{
				string pathToTest = this.txtThumbnailCachePath.Text.Trim();
				if (String.IsNullOrEmpty(pathToTest))
					pathToTest = this.txtMoPath.Text;

				if (this.chkPathIsReadOnly.Checked)
					return ValidateReadOnlyGallery(item) && ValidatePathIsWriteable(item, pathToTest, this.lblThumbnailCachePath);
				else
					return ValidatePathIsWriteable(item, pathToTest, this.lblThumbnailCachePath);
			}

			// Validate the media object optimized image path.
			else if (item.ControlInstance == this.txtOptimizedCachePath)
			{
				string pathToTest = this.txtOptimizedCachePath.Text.Trim();
				if (String.IsNullOrEmpty(pathToTest))
					pathToTest = this.txtMoPath.Text;

				if (this.chkPathIsReadOnly.Checked)
					return ValidateReadOnlyGallery(item) && ValidatePathIsWriteable(item, pathToTest, this.lblOptimizedCachePath);
				else
					return ValidatePathIsWriteable(item, pathToTest, this.lblOptimizedCachePath);
			}

			// Validate the "media files are read-only" option
			else if (item.ControlInstance == this.chkPathIsReadOnly)
			{
				if (this.chkPathIsReadOnly.Checked)
					return ValidateReadOnlyGallery(item) && ValidatePathIsReadable(item, this.txtMoPath.Text, this.lblMoPath);
				else
					return ValidatePathIsWriteable(item, this.txtMoPath.Text, this.lblMoPath);
			}

			return true;
		}

		protected bool wwDataBinder_BeforeUnbindControl(WebControls.wwDataBindingItem item)
		{
			if (!this.chkEnableSlideShow.Checked)
			{
				// When the slide show is unchecked, the slide show interval textbox is disabled via javascript. Disabled HTML items are not
				// posted during a postback, so we don't have accurate information about their states. For this control don't save
				// anything by returning false. Furthermore, to prevent these child controls from incorrectly reverting to an
				// empty or unchecked state in the UI, assign their properties to their config setting. 
				if (item.ControlId == this.txtSlideShowInterval.ID)
				{
					this.txtSlideShowInterval.Text = Core.SlideshowInterval.ToString();
					return false;
				}
			}

			if (!this.chkEnableMetadata.Checked)
			{
				// When the metadata feature is unchecked, the enhanced metadata checkbox is disabled via javascript. Disabled HTML items are not
				// posted during a postback, so we don't have accurate information about their states. For this control don't save
				// anything by returning false. Furthermore, to prevent these child controls from incorrectly reverting to an
				// empty or unchecked state in the UI, assign their properties to their config setting. 
				if (item.ControlId == this.chkEnableWpfMetadataExtraction.ID)
				{
					this.chkEnableWpfMetadataExtraction.Checked = Core.EnableWpfMetadataExtraction;
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_Media_Objects_General_Page_Header;
		}

		private void ConfigureControlsFirstTime()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_Media_Objects_General_Page_Header;

			if (!HasEditConfigPermission)
			{
				wwMessage.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;

				foreach (System.Web.UI.Control ctl in this.Controls)
				{
					if ((ctl is CheckBox) || (ctl is TextBox) || (ctl is DropDownList))
					{
						((WebControl)ctl).Enabled = false;
					}
				}
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}

			this.wwDataBinder.DataBind();

			ddlTransType.DataSource = Enum.GetValues(typeof(MediaObjectTransitionType));
			ddlTransType.DataBind();

			lblMoPath.Text = AppSetting.Instance.MediaObjectPhysicalPath;
			lblThumbnailCachePath.Text = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(AppSetting.Instance.MediaObjectPhysicalPath, AppSetting.Instance.ThumbnailPath);
			lblOptimizedCachePath.Text = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(AppSetting.Instance.MediaObjectPhysicalPath, AppSetting.Instance.OptimizedPath);
		}

		private void SaveSettings()
		{
			this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();

				return;
			}

			GspConfigController.SaveCore(this.CoreConfig);

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Save_Success_Text);
		}

		private static bool ValidatePathIsReadable(GalleryServerPro.WebControls.wwDataBindingItem item, string pathToTest, Label pathLabel)
		{
			// Verify that the IIS process identity has read permission to the specified path.
			bool isValid = false;

			string fullPhysicalPath = HelperFunctions.CalculateFullPath(AppSetting.Instance.PhysicalApplicationPath, pathToTest);
			try
			{
				HelperFunctions.ValidatePhysicalPathExistsAndIsReadable(fullPhysicalPath);
				isValid = true;
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotReadFromDirectoryException ex)
			{
				item.BindingErrorMessage = ex.Message;
			}

			if (isValid)
			{
				pathLabel.Text = fullPhysicalPath;
				pathLabel.CssClass = "gsp_msgfriendly";
			}
			else
			{
				pathLabel.Text = String.Concat("&lt;", Resources.GalleryServerPro.Admin_MediaObjects_InvalidPath, "&gt;");
				pathLabel.CssClass = "gsp_msgwarning";
			}

			return isValid;
		}

		private bool ValidatePathIsWriteable(GalleryServerPro.WebControls.wwDataBindingItem item, string pathToTest, Label pathLabel)
		{
			// Verify that the IIS process identity has write permission to the specified path.

			// We only need to execute this once for each unique path. If we already tested this path, then return that test result. This helps
			// prevent the same error message from being shown multiple times.
			if (_pathsThatHaveBeenTestedForWriteability.ContainsKey(pathToTest))
				return _pathsThatHaveBeenTestedForWriteability[pathToTest];

			bool isValid = false;

			string fullPhysicalPath = HelperFunctions.CalculateFullPath(AppSetting.Instance.PhysicalApplicationPath, pathToTest);
			try
			{
				HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(fullPhysicalPath);
				isValid = true;
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException ex)
			{
				item.BindingErrorMessage = ex.Message;
			}

			if (isValid)
			{
				pathLabel.Text = fullPhysicalPath;
				pathLabel.CssClass = "gsp_msgfriendly";
			}
			else
			{
				pathLabel.Text = String.Concat("&lt;", Resources.GalleryServerPro.Admin_MediaObjects_InvalidPath, "&gt;");
				pathLabel.CssClass = "gsp_msgwarning";
			}

			// Set the flag so we don't have to repeat the validation later in the page lifecycle.
			_pathsThatHaveBeenTestedForWriteability.Add(pathToTest, isValid);

			return isValid;
		}

		private bool ValidateReadOnlyGallery(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// When a gallery is read only, the following must be true:
			// 1. The thumbnail and optimized path must be different than the media object path.
			// 2. The SynchAlbumTitleAndDirectoryName setting must be false.
			// 3. User albums must be disabled.

			// We only need to execute this once on a postback. If we already ran it, then return our previous result. This helps
			// prevent the same error message from being shown multiple times.
			if (_validateReadOnlyGalleryHasExecuted)
				return _validateReadOnlyGalleryResult;

			bool isValid = true;

			string mediaObjectPath = this.txtMoPath.Text;
			string thumbnailPath = (String.IsNullOrEmpty(this.txtThumbnailCachePath.Text) ? mediaObjectPath : this.txtThumbnailCachePath.Text);
			string optimizedPath = (String.IsNullOrEmpty(this.txtOptimizedCachePath.Text) ? mediaObjectPath : this.txtOptimizedCachePath.Text);

			// 1. The thumbnail and optimized path must be different than the media object path.
			if ((mediaObjectPath.Equals(thumbnailPath, StringComparison.OrdinalIgnoreCase)) ||
				(mediaObjectPath.Equals(optimizedPath, StringComparison.OrdinalIgnoreCase)))
			{
				isValid = false;
				item.BindingErrorMessage = string.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_MediaObjects_Cannot_Set_MO_Path_Read_Only_Cache_Location_Not_Set, mediaObjectPath, thumbnailPath, optimizedPath);
			}

			// 2. The SynchAlbumTitleAndDirectoryName setting must be false.
			if (chkSynchAlbumTitleAndDirectoryName.Checked)
			{
				isValid = false;
				item.BindingErrorMessage = Resources.GalleryServerPro.Admin_MediaObjects_Cannot_Set_MO_Path_Read_Only_Synch_Title_And_Directory_Enabled;
			}

			// 3. User albums must be disabled.
			if (Configuration.ConfigManager.GetGalleryServerProConfigSection().Core.EnableUserAlbum)
			{
				isValid = false;
				item.BindingErrorMessage = Resources.GalleryServerPro.Admin_MediaObjects_Cannot_Set_MO_Path_Read_Only_User_Albums_Enabled;
			}

			// Set the flag so we don't have to repeat the validation later in the page lifecycle.
			this._validateReadOnlyGalleryHasExecuted = true;
			this._validateReadOnlyGalleryResult = isValid;

			return isValid;
		}

		#endregion
	}
}
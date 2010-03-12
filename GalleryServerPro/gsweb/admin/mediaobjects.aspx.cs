using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;

namespace GalleryServerPro.Web.admin
{
	public partial class mediaobjects : GspPage
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			if (!IsPostBack)
			{
				ConfigureControls();
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

		protected bool wwDataBinder_ValidateControl(GalleryServerPro.WebControls.wwDataBindingItem Item)
		{
			if (Item.ControlInstance == this.txtMoPath)
			{
				return ValidatePath(Item, this.txtMoPath.Text, this.lblMoPath);
			}
			else if (Item.ControlInstance == this.txtThumbnailCachePath)
			{
				return ValidatePath(Item, this.txtThumbnailCachePath.Text, this.lblThumbnailCachePath);
			}
			else if (Item.ControlInstance == this.txtOptimizedCachePath)
			{
				return ValidatePath(Item, this.txtOptimizedCachePath.Text, this.lblOptimizedCachePath);
			}

			return true;
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.AdminPageTitle = Resources.GalleryServerPro.Admin_Media_Objects_General_Page_Header;

			if (!Master.HasEditConfigPermission)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
				Master.OkButtonBottom.Enabled = false;
				Master.OkButtonTop.Enabled = false;
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
				Master.OkButtonBottom.Enabled = false;
				Master.OkButtonTop.Enabled = false;
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
				this.wwMessage.CssClass = "wwErrorFailure msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			#region Save with error trapping

			string msgResult = string.Empty;
			try
			{
				Master.Config.Save(System.Configuration.ConfigurationSaveMode.Minimal);

				msgResult = "Your changes have been saved.";
				this.wwMessage.CssClass = "wwErrorSuccess msgfriendly bold";
			}
			catch (System.Configuration.ConfigurationErrorsException ex)
			{
				if ((ex.InnerException != null) && (ex.InnerException is UnauthorizedAccessException))
				{
					// We get here under IIS7/Vista when the IIS account doesn't have permission to galleryserverpro.config.
					string configDir = System.IO.Path.GetDirectoryName(ex.Filename);
					msgResult = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Save_ConfigurationErrorsException_Msg, configDir, System.IO.Path.GetFileName(ex.Filename));
				}
				else
				{
					msgResult = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_Error_Hdr, ex.Message);
				}
				this.wwMessage.CssClass = "wwErrorFailure msgwarning";
			}
			catch (UnauthorizedAccessException ex)
			{
				// We get here under IIS6/Win2003 when the IIS account doesn't have permission to galleryserverpro.config.
				msgResult = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_UnauthorizedAccessException_Msg, ex.Message);
				this.wwMessage.CssClass = "wwErrorFailure msgwarning";
			}
			finally
			{
				this.wwMessage.ShowMessage(msgResult);
			}

			#endregion
		}

		private bool ValidatePath(GalleryServerPro.WebControls.wwDataBindingItem Item, string pathToTest, System.Web.UI.WebControls.Label pathLabel)
		{
			bool isValid = false;

			string fullPhysicalPath = HelperFunctions.CalculateFullPath(AppSetting.Instance.PhysicalApplicationPath, pathToTest);
			try
			{
				HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(fullPhysicalPath);
				isValid = true;
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectDirectoryException)
			{
				string msg = String.Format(CultureInfo.CurrentCulture, "Invalid media object directory: Gallery Server Pro cannot find or does not have permission to access the media object path {0}. Verify that the setting corresponds to a valid directory and that the web application has read, write and modify permission to this location.", Server.HtmlEncode(fullPhysicalPath));
				Item.BindingErrorMessage = msg;
			}
			catch (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException ex)
			{
				Item.BindingErrorMessage = ex.Message;
			}

			if (isValid)
			{
				pathLabel.Text = fullPhysicalPath;
				pathLabel.CssClass = "msgfriendly";
			}
			else
			{
				pathLabel.Text = "&lt;Invalid path&gt;";
				pathLabel.CssClass = "msgwarning";
			}
			return isValid;
		}

		#endregion
	}
}

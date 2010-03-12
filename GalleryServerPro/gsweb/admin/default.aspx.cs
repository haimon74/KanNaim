using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.admin
{
	public partial class _default : GspPage
	{
		private bool? _isInTrialPeriod;
		private bool? _isRegistered;
		private string _messageText;
		private string _messageCssClass;
		private bool _messageIsError;

		public bool IsInTrialPeriod
		{
			get
			{
				if (!this._isInTrialPeriod.HasValue)
				{
					this._isInTrialPeriod = AppSetting.Instance.IsInTrialPeriod;
				}
				return _isInTrialPeriod.Value;
			}
			set { _isInTrialPeriod = value; }
		}

		public bool IsRegistered
		{
			get
			{
				if (!this._isRegistered.HasValue)
				{
					this._isRegistered = AppSetting.Instance.IsRegistered;
				}
				return _isRegistered.Value;
			}
			set { _isRegistered = value; }
		}

		public string MessageText
		{
			get { return _messageText; }
			set { _messageText = value; }
		}

		public string MessageCssClass
		{
			get { return _messageCssClass; }
			set { _messageCssClass = value; }
		}

		public bool MessageIsError
		{
			get { return _messageIsError; }
			set { _messageIsError = value; }
		}

		public bool SavingIsEnabled
		{
			get
			{
				return (Master.HasEditConfigPermission && (this.IsInTrialPeriod || this.IsRegistered));
			}
		}

		#region Properties


		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		protected void btnOk_Click(object sender, EventArgs args)
		{
			SaveSettings();
		}

		protected void btnEnterProductKey_Click(object sender, EventArgs args)
		{
			SaveProductKey();
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

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.AdminPageTitle = Resources.GalleryServerPro.Admin_Site_Settings_General_Page_Header;
			lblVersion.Text = String.Concat("v", WebsiteController.GetGalleryServerVersion());
			txtProductKey.Text = Master.CoreConfig.ProductKey;

			Master.OkButtonBottom.Enabled = this.SavingIsEnabled;
			Master.OkButtonTop.Enabled = this.SavingIsEnabled;

			this.wwDataBinder.DataBind();

			DetermineMessage();

			UpdateUI();
		}

		private void DetermineMessage()
		{
			if (!Master.HasEditConfigPermission)
			{
				this.MessageText = Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg;
				this.MessageCssClass = "wwErrorSuccess msgwarning";
			}

			bool isInTrialMode = (this.IsInTrialPeriod && !this.IsRegistered);
			if (isInTrialMode)
			{
				IGallery gallery = Factory.GetDataProvider().Gallery_GetCurrentGallery(new Gallery());
				int daysLeftInTrial = (gallery.CreationDate.AddDays(GlobalConstants.TrialNumberOfDays) - DateTime.Today).Days;
				this.MessageText = string.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_In_Trial_Period_Msg, daysLeftInTrial);
				this.MessageCssClass = "wwErrorSuccess msgfriendly";
			}

			bool trialPeriodExpired = (!this.IsInTrialPeriod && !this.IsRegistered);
			if (trialPeriodExpired)
			{
				this.MessageText = Resources.GalleryServerPro.Admin_Need_Product_Key_Msg;
				this.MessageCssClass = "wwErrorSuccess msgwarning";
			}
		}

		private void UpdateUI()
		{
			if (!String.IsNullOrEmpty(this.MessageText))
			{
				wwMessage.CssClass = this.MessageCssClass;

				if (this.MessageIsError)
					wwMessage.Text = this.MessageText;
				else
					wwMessage.ShowMessage(this.MessageText);
			}

			UpdateProductKeyValidationMessage();
		}

		private void UpdateProductKeyValidationMessage()
		{
			string productKey = txtProductKey.Text;

			if (String.IsNullOrEmpty(productKey))
			{
				lblProductKeyValidationMsg.Text = Resources.GalleryServerPro.Admin_Default_ProductKey_NotEntered_Label;
				lblProductKeyValidationMsg.CssClass = "msgfriendly";
				imgProductKeyValidation.ImageUrl = "~/images/info.gif";
				imgProductKeyValidation.Visible = true;
			}
			else if (GlobalConstants.ProductKey.Equals(txtProductKey.Text))
			{
				lblProductKeyValidationMsg.Text = Resources.GalleryServerPro.Admin_Default_ProductKey_Correct_Label;
				lblProductKeyValidationMsg.CssClass = "msgfriendly";
				imgProductKeyValidation.ImageUrl = String.Concat(this.ThemePath, "/images/ok_16x16.png");
				imgProductKeyValidation.Visible = true;
			}
			else
			{
				lblProductKeyValidationMsg.Text = Resources.GalleryServerPro.Admin_Default_ProductKey_Incorrect_Label;
				lblProductKeyValidationMsg.CssClass = "msgwarning";
				imgProductKeyValidation.ImageUrl = String.Concat(this.ThemePath, "/images/error_16x16.png");
				imgProductKeyValidation.Visible = true;
			}
		}

		private void SaveProductKey()
		{
			string productKey = txtProductKey.Text;

			if ((!String.IsNullOrEmpty(productKey)) && !GlobalConstants.ProductKey.Equals(txtProductKey.Text))
				wwDataBinder.AddBindingError(Resources.GalleryServerPro.Admin_Default_ProductKey_Incorrect_Msg, txtProductKey);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.MessageText = wwDataBinder.BindingErrors.ToHtml();
				this.MessageCssClass = "wwErrorFailure msgwarning";
				this.MessageIsError = true;
				UpdateUI();
				return;
			}

			#region Save with error trapping

			try
			{
				Master.CoreConfig.ProductKey = productKey;
				Master.Config.Save(System.Configuration.ConfigurationSaveMode.Minimal);

				if (String.IsNullOrEmpty(productKey))
				{
					this.IsRegistered = false;
					DetermineMessage();
				}
				else
				{
					this.IsRegistered = true;
					this.MessageText = Resources.GalleryServerPro.Admin_Save_ProductKey_Success_Text;
					this.MessageCssClass = "wwErrorSuccess msgfriendly bold";
				}

				// Enable or disable the Save buttons
				Master.OkButtonBottom.Enabled = this.SavingIsEnabled;
				Master.OkButtonTop.Enabled = this.SavingIsEnabled;
			}
			catch (System.Configuration.ConfigurationErrorsException ex)
			{
				if ((ex.InnerException != null) && (ex.InnerException is UnauthorizedAccessException))
				{
					// We get here under IIS7/Vista when the IIS account doesn't have permission to galleryserverpro.config.
					string configDir = System.IO.Path.GetDirectoryName(ex.Filename);
					this.MessageText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Save_ProductKey_Failure_ConfigurationErrorsException_Msg, configDir, System.IO.Path.GetFileName(ex.Filename));
				}
				else
				{
					this.MessageText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Save_ProductKey_Error_Hdr, ex.Message);
				}
				this.MessageIsError = true;
				this.MessageCssClass = "wwErrorFailure msgwarning";
			}
			catch (UnauthorizedAccessException ex)
			{
				// We get here under IIS6/Win2003 when the IIS account doesn't have permission to galleryserverpro.config.
				this.MessageText = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_ProductKey_Failure_UnauthorizedAccessException_Msg, ex.Message);
				this.MessageCssClass = "wwErrorFailure msgwarning";
				this.MessageIsError = true;
			}
			finally
			{
				UpdateUI();
			}

			#endregion
		}

		private void SaveSettings()
		{
			this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.MessageText = wwDataBinder.BindingErrors.ToHtml();
				this.MessageCssClass = "wwErrorFailure msgwarning";
				this.MessageIsError = true;
				UpdateUI();
				return;
			}

			#region Save with error trapping

			try
			{
				Master.Config.Save(System.Configuration.ConfigurationSaveMode.Minimal);

				this.MessageText = Resources.GalleryServerPro.Admin_Save_Success_With_Refresh_Note_Text;
				this.MessageCssClass = "wwErrorSuccess msgfriendly bold";
			}
			catch (System.Configuration.ConfigurationErrorsException ex)
			{
				if ((ex.InnerException != null) && (ex.InnerException is UnauthorizedAccessException))
				{
					// We get here under IIS7/Vista when the IIS account doesn't have permission to galleryserverpro.config.
					string configDir = System.IO.Path.GetDirectoryName(ex.Filename);
					this.MessageText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Save_ConfigurationErrorsException_Msg, configDir, System.IO.Path.GetFileName(ex.Filename));
				}
				else
				{
					this.MessageText = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_Error_Hdr, ex.Message);
				}
				this.MessageCssClass = "wwErrorFailure msgwarning";
			}
			catch (UnauthorizedAccessException ex)
			{
				// We get here under IIS6/Win2003 when the IIS account doesn't have permission to galleryserverpro.config.
				this.MessageText = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_UnauthorizedAccessException_Msg, ex.Message);
				this.MessageCssClass = "wwErrorFailure msgwarning";
			}
			finally
			{
				UpdateUI();
			}

			#endregion
		}

		#endregion
	}
}

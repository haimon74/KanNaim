using System;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;

namespace GalleryServerPro.Web.admin
{
	public partial class albums : GspPage
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

		protected void cvColor_ServerValidate(object sender, ServerValidateEventArgs args)
		{
			try
			{
				HelperFunctions.GetColor(args.Value.Trim());
				args.IsValid = true;
			}
			catch(ArgumentNullException) { args.IsValid = false; }
			catch(ArgumentOutOfRangeException) { args.IsValid = false; }
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.AdminPageTitle = Resources.GalleryServerPro.Admin_Albums_General_Page_Header;

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

				msgResult = Resources.GalleryServerPro.Admin_Save_Success_Text;
				this.wwMessage.CssClass = "wwErrorSuccess msgfriendly bold";
			}
			catch (System.Configuration.ConfigurationErrorsException ex)
			{
				if ((ex.InnerException != null) && (ex.InnerException is UnauthorizedAccessException))
				{
					string configDir = System.IO.Path.GetDirectoryName(ex.Filename);
					msgResult = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Save_ConfigurationErrorsException_Msg, configDir, System.IO.Path.GetFileName(ex.Filename));
				}
				else
				{
					msgResult = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Admin_Save_Error_Hdr, ex.Message);
				}
				this.wwMessage.CssClass = "wwErrorFailure msgwarning";
			}
			finally
			{
				this.wwMessage.ShowMessage(msgResult);
			}

			#endregion
		}

		#endregion
	}
}

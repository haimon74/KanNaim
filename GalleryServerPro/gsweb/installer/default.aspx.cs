using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using GalleryServerPro.Business;
using GalleryServerPro.Configuration;

namespace GalleryServerPro.Web.installer
{
	#region Enum declarations

	public enum WizardPanel
	{
		Welcome,
		License,
		DataProvider,
		DbAdmin,
		ChooseDb,
		UpgradeFromVersion1,
		DbRuntime,
		SetupOptions,
		GsAdmin,
		ReadyToInstall,
		Finished,
	}

	public enum SqlVersion
	{
		Unknown,
		PreSql2000,
		Sql2000,
		Sql2005,
		Sql2008,
		PostSql2008
	}

	public enum ProviderDataStore
	{
		SQLite,
		SqlServer
	}

	#endregion

	public partial class _default : System.Web.UI.Page
	{
		#region Private Fields

		private bool _webConfigSuccessfullyUpdated;
		private bool _gspConfigSuccessfullyUpdated;

		protected const string CN_STRING_NAME = "SqlServerDbConnection"; // Name of connection string in web.config. Note that if you change this,
		// also change it in the Installer_Finished_WebCfg_Need_Updating_Dtl resource setting.
		private const string APP_NAME = "Gallery Server Pro"; // The application name to be specified in the connection string and Membership configuration

		private static readonly string _encryptionKey = GenerateNewEncryptionKey(); // The encryption key to be written to encryptionKey setting in galleryserverpro.config.

		#endregion

		#region Public Properties

		public WizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (WizardPanel)ViewState["WizardPanel"];

				return WizardPanel.Welcome;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}

		protected string DbAdminPassword
		{
			get
			{
				string dbPwd = ViewState["dbAdminPwd"] as string;
				if (dbPwd == null)
				{
					dbPwd = String.Empty;
					ViewState["dbAdminPwd"] = dbPwd;
				}
				return dbPwd;
			}
			set
			{
				ViewState["dbAdminPwd"] = value;
			}
		}

		protected string DbRuntimePassword
		{
			get
			{
				string dbPwd = ViewState["dbRuntimePwd"] as string;
				if (dbPwd == null)
				{
					dbPwd = String.Empty;
					ViewState["dbRuntimePwd"] = dbPwd;
				}
				return dbPwd;
			}
			set
			{
				ViewState["dbRuntimePwd"] = value;
			}
		}

		protected string GsAdminPassword
		{
			get
			{
				string gsPwd = ViewState["gsAdminPwd"] as string;
				if (gsPwd == null)
				{
					gsPwd = String.Empty;
					ViewState["gsAdminPwd"] = gsPwd;
				}
				return gsPwd;
			}
			set
			{
				ViewState["gsAdminPwd"] = value;
			}
		}

		protected static string IisIdentity
		{
			get
			{
				return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			}
		}

		protected static string EncryptionKey
		{
			get
			{
				return _encryptionKey;
			}
		}

		protected ProviderDataStore ProviderDb
		{
			get
			{
				return (rbDataProviderSQLite.Checked ? ProviderDataStore.SQLite : ProviderDataStore.SqlServer);
			}
		}

		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			bool setupEnabled;
			if (Boolean.TryParse(ENABLE_SETUP.Value, out setupEnabled) && setupEnabled)
			{
				if (!Page.IsPostBack)
				{
					// Make sure the App_Data directory is writeable. We might need to write a file here as part of the installation.
					HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(Server.MapPath("~\\" + GlobalConstants.AppDataDirectory));

					SetCurrentPanel(WizardPanel.Welcome, Welcome);
				}

				ConfigureControls();
			}
			else
			{
				Response.Write(String.Format(CultureInfo.CurrentCulture, "<h1>{0}</h1>", Resources.GalleryServerPro.Installer_Disabled_Msg));
				Response.Flush();
				Response.End();
			}
		}

		protected void cvDbAdminSqlLogOn_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if ((rblDbAdminConnectType.SelectedIndex == 1) && (String.IsNullOrEmpty(txtDbAdminUserName.Text)))
				args.IsValid = false;
			else
				args.IsValid = true;
		}

		protected void cvDbAdminSqlPassword_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if ((rblDbAdminConnectType.SelectedIndex == 1) && (String.IsNullOrEmpty(txtDbAdminPassword.Text)))
				args.IsValid = false;
			else
				args.IsValid = true;
		}

		protected void cvDbRuntimeSqlLogOn_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if ((rblDbRuntimeConnectType.SelectedIndex == 2) && (String.IsNullOrEmpty(txtDbRuntimeUserName.Text)))
				args.IsValid = false;
			else
				args.IsValid = true;
		}

		protected void cvDbRuntimeSqlPassword_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if ((rblDbRuntimeConnectType.SelectedIndex == 2) && (String.IsNullOrEmpty(txtDbRuntimePassword.Text)))
				args.IsValid = false;
			else
				args.IsValid = true;
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
				ShowNextPanel();
		}

		protected void btnPrevious_Click(object sender, EventArgs e)
		{
			ShowPreviousPanel();
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			if (!IsPostBack)
				ConfigureControlsFirstTime();

			Page.Form.DefaultFocus = btnNext.ClientID;

			#region Configure Validators

			string validationMsg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_GsAdmin_Pwd_Min_Length_Msg, System.Web.Security.Membership.MinRequiredPasswordLength);
			rfvGsAdminPassword.ErrorMessage = validationMsg;

			regGsAdminPassword.ValidationExpression = String.Format(CultureInfo.InvariantCulture, @"\S{{{0},128}}", System.Web.Security.Membership.MinRequiredPasswordLength);
			regGsAdminPassword.ErrorMessage = validationMsg;

			#endregion
		}

		private void ConfigureControlsFirstTime()
		{
			rblDbAdminConnectType.Items.Add(new ListItem(String.Format(CultureInfo.CurrentCulture, "{0} ({1})", Resources.GalleryServerPro.Installer_DbAdmin_Connect_Type_Item1, IisIdentity, "0")));
			rblDbAdminConnectType.Items.Add(new ListItem(Resources.GalleryServerPro.Installer_DbAdmin_Connect_Type_Item2, "1"));
			rblDbAdminConnectType.SelectedIndex = 0;

			rblDbRuntimeConnectType.Items.Add(new ListItem(Resources.GalleryServerPro.Installer_DbRuntime_Connect_Type_Item1, "0"));
			rblDbRuntimeConnectType.Items.Add(new ListItem(String.Format(CultureInfo.CurrentCulture, "{0} ({1})", Resources.GalleryServerPro.Installer_DbRuntime_Connect_Type_Item2, IisIdentity, "1")));
			rblDbRuntimeConnectType.Items.Add(new ListItem(Resources.GalleryServerPro.Installer_DbRuntime_Connect_Type_Item3, "2"));
			rblDbRuntimeConnectType.SelectedIndex = 0;

			string version = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Logo_Tooltip, WebsiteController.GetGalleryServerVersion());
			litVersion.Text = version;
		}

		private void SetCurrentPanel(WizardPanel panel, Control controlToShow)
		{
			Panel currentPanel = Page.Form.FindControl("c1").FindControl(CurrentWizardPanel.ToString()) as Panel;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case WizardPanel.Welcome:
					btnPrevious.Enabled = false;
					License.Visible = false;
					break;
				case WizardPanel.Finished:
					btnNext.Enabled = false;
					btnPrevious.Enabled = false;
					break;
				default:
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					break;
			}

			controlToShow.Visible = true;
			CurrentWizardPanel = panel;
		}

		private void ShowNextPanel()
		{
			switch (this.CurrentWizardPanel)
			{
				case WizardPanel.Welcome:
					{
						SetCurrentPanel(WizardPanel.License, License);
						break;
					}
				case WizardPanel.License:
					{
						if (chkLicenseAgreement.Checked)
						{
							ConfigureDbEngineChoices();
							SetCurrentPanel(WizardPanel.DataProvider, DataProvider);
						}
						break;
					}
				case WizardPanel.DataProvider:
					{
						if (ValidateDataProvider())
						{
							if (rbDataProviderSQLite.Checked)
								SetCurrentPanel(WizardPanel.GsAdmin, GsAdmin);
							else
								SetCurrentPanel(WizardPanel.DbAdmin, DbAdmin);
						}
						break;
					}
				case WizardPanel.DbAdmin:
					{
						this.DbAdminPassword = txtDbAdminPassword.Text;
						try
						{
							BindDatabaseDropdownlist();
							SetCurrentPanel(WizardPanel.ChooseDb, ChooseDb);
						}
						catch (Exception ex)
						{
							lblErrMsgDbAdmin.InnerText = ex.Message;
							lblErrMsgDbAdmin.Attributes["class"] = "msgwarning visible";
						}
						break;
					}
				case WizardPanel.ChooseDb:
					{
						if (ValidateChooseDb())
						{
							if (GalleryServer1TablesExistAndTheyHaveRecords())
							{
								SetCurrentPanel(WizardPanel.UpgradeFromVersion1, UpgradeFromVersion1);
							}
							else
							{
								SetCurrentPanel(WizardPanel.DbRuntime, DbRuntime);
							}
						}
						break;
					}
				case WizardPanel.UpgradeFromVersion1:
					{
						SetCurrentPanel(WizardPanel.DbRuntime, DbRuntime);
						break;
					}
				case WizardPanel.DbRuntime:
					{
						this.DbRuntimePassword = txtDbRuntimePassword.Text;

						if (ValidateRuntimeLogin())
							SetCurrentPanel(WizardPanel.SetupOptions, SetupOptions);

						break;
					}
				case WizardPanel.SetupOptions:
					{
						SetCurrentPanel(WizardPanel.GsAdmin, GsAdmin);
						break;
					}
				case WizardPanel.GsAdmin:
					{
						this.GsAdminPassword = txtGsAdminPassword.Text;
						SetCurrentPanel(WizardPanel.ReadyToInstall, ReadyToInstall);
						break;
					}
				case WizardPanel.ReadyToInstall:
					{
						if (ExecuteInstallation())
						{
							SetCurrentPanel(WizardPanel.Finished, Finished);

							if (_webConfigSuccessfullyUpdated && _gspConfigSuccessfullyUpdated)
							{
								imgFinishedIcon.ImageUrl = String.Concat(WebsiteController.GetThemePathUrl(this.Theme), "/images/ok_26x26.png");
								imgFinishedIcon.Width = Unit.Pixel(26);
								imgFinishedIcon.Height = Unit.Pixel(26);
								l61.Text = Resources.GalleryServerPro.Installer_Finished_No_Addl_Steps_Reqd;
							}
							else
							{
								imgFinishedIcon.ImageUrl = String.Concat(WebsiteController.GetThemePathUrl(this.Theme), "/images/warning_32x32.png");
								imgFinishedIcon.Width = Unit.Pixel(32);
								imgFinishedIcon.Height = Unit.Pixel(32);
								l61.Text = Resources.GalleryServerPro.Installer_Finished_Addl_Steps_Reqd;
							}
							pnlWebConfigNeedUpdating.Visible = !_webConfigSuccessfullyUpdated;
							pnlGalleryServerProConfigNeedUpdating.Visible = !_gspConfigSuccessfullyUpdated;
						}
						break;
					}
			}
		}

		/// <summary>
		/// Check web.config and galleryserverpro.config to make sure they contain references to the SQLite and SQL Server providers.
		/// Disable the radio button for one or both if they are missing configuration info. This *does not* check whether any 
		/// particular provider is selected, nor does it verify the installer has permission to edit the config files; that is done
		/// in the ValidateDataProvider method.
		/// </summary>
		private void ConfigureDbEngineChoices()
		{
			if ((AreProvidersAvailableInWebConfig("SQLite")) && (IsProviderAvailableInGalleryServerProConfig("SQLite")))
			{
				// Web.config and galleryserverpro.config contain references to SQLite providers, so it is a valid choice. Select it, unless one 
				// of the radio buttons is already selected. (If the app was running at Medium Trust and the SQLite dll wasn't in 
				// the GAC, then the app would have generated an error before we even get to this point. So we know that, just 
				// by getting to this point, that if web.config and galleryserverpro.config contain references to SQLite, it is a valid choice.)
				if (!rbDataProviderSQLite.Checked && !rbDataProviderSqlServer.Checked)
					rbDataProviderSQLite.Checked = true;
			}
			else
				rbDataProviderSQLite.Enabled = false;

			if ((AreProvidersAvailableInWebConfig("SqlServer")) && (IsProviderAvailableInGalleryServerProConfig("SqlServer")))
			{
				// Web.config and galleryserverpro.config contain references to SQL Server providers, so it is a valid choice. 
				// Select it, unless one of the radio buttons is already selected.
				if (!rbDataProviderSQLite.Checked && !rbDataProviderSqlServer.Checked)
					rbDataProviderSqlServer.Checked = true;
			}
			else
				rbDataProviderSqlServer.Enabled = false;

			if ((!rbDataProviderSQLite.Enabled) && (rbDataProviderSqlServer.Enabled))
			{
				// SQLite is disabled and SQL Server is enabled. Give user message why.
				lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_SQLite_Not_Available_Msg;
				lblErrMsgChooseDbEngine.Attributes["class"] = "msgfriendly";
				pnlDbEngineMsg.Visible = true;
			}
			else if ((rbDataProviderSQLite.Enabled) && (!rbDataProviderSqlServer.Enabled))
			{
				// SQLite is enabled and SQL Server is disabled. Give user message why.
				lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_SqlServer_Not_Available_Msg;
				lblErrMsgChooseDbEngine.Attributes["class"] = "msgfriendly";
				pnlDbEngineMsg.Visible = true;
			}
			else if ((!rbDataProviderSQLite.Enabled) && (!rbDataProviderSqlServer.Enabled))
			{
				// Both SQLite and SQL Server are disabled. Give user message why.
				lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_SQLite_And_SqlServer_Not_Available_Msg;
				lblErrMsgChooseDbEngine.Attributes["class"] = "msgwarning";
				pnlDbEngineMsg.Visible = true;
				btnNext.Enabled = false;
			}
		}

		/// <summary>
		/// Using the information gathered from the user, execute the installation.
		/// </summary>
		/// <returns>Returns true if the installation is successful or false if an error occurred.</returns>
		private bool ExecuteInstallation()
		{
			if (rbDataProviderSQLite.Checked)
				return ExecuteSQLiteInstallation();
			else
				return ExecuteSqlServerInstallation();
		}

		private bool ExecuteSQLiteInstallation()
		{
			try
			{
				UpdateWebConfigFile();

				UpdateGalleryServerProConfigFile();

				InitializeSQLiteData();

				return true;
			}
			catch (Exception ex)
			{
				ShowErrorMsgThatOccurredDuringInstallation(ex.Message, null, ex.StackTrace, lblErrMsgReadyToInstall, null, lblErrMsgReadyToInstallCallStack);
				return false;
			}
		}

		private void InitializeSQLiteData()
		{
			// We need to set up the sys admin role and user, but we can't do this at the moment because web.config may have
			// been referencing SQL Server, and even if we just updated it to SQLite, it won't take effect until the app restarts.
			// We also can't update the tables directly because that requires a reference to the SQLite dll, which we don't want
			// to do. So instead we write a small file to the App_Data directory that will be noticed at the next application
			// restart. This file will trigger the code to create the role and user for us.
			string filePath = Path.Combine(Request.PhysicalApplicationPath, Path.Combine(GlobalConstants.AppDataDirectory, GlobalConstants.InstallerFileName));

			File.Delete(filePath);
      
			using (StreamWriter sw = File.CreateText(filePath))
			{
				sw.WriteLine(txtGsAdminUserName.Text);
				sw.WriteLine(GsAdminPassword);
				sw.WriteLine(txtGsAdminEmail.Text);
			}  
		}

		private bool ExecuteSqlServerInstallation()
		{
			if (chkScriptMembership.Checked)
			{
				if (!ConfigureAspNetMembership()) return false;
			}

			if (!ConfigureGalleryServerSchemaForSqlServer()) return false;

			if (!ConfigureRuntimeLogin()) return false;

			UpdateWebConfigFile();

			UpdateGalleryServerProConfigFile();

			return true;
		}

		private bool ConfigureRuntimeLogin()
		{
			#region Step 1: Ensure login exists in the database

			// Can the account log in to the data server?
			if (!CanConnectToDatabase(GetDbRuntimeConnectionString(true)))
			{
				try
				{
					// Account does not exist or is not valid. Create it.
					if (!ExecuteSql(GetAddSqlUserScript()))
					{
						return false;
					}
				}
				catch
				{
					string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Cannot_Add_User_Msg, GetRuntimeSqlAccountName());
					msg = String.Format(CultureInfo.CurrentCulture, "{0} {1}", msg, lblErrMsgReadyToInstall.InnerText);
					ShowErrorMsgThatOccurredDuringInstallation(msg, null, null, lblErrMsgReadyToInstall, null, null);
					return false;
				}
			}

			#endregion

			#region Step 2: Grant login appropriate permission to database objects

			// The SQL role named gs_GalleryServerProRole was configured in the script InstallGalleryServerPro2000.sql 
			// (or InstallGalleryServerPro2005.sql for SQL Server 2005 and later) to have all appropriate permissions, 
			// so all we have to do is add the user to this SQL role. If the user is sa, do not add it to the role because
			// it already has permission (trying to do so throws the error "User or role 'sa' does not exist in this database.")
			if (!GetRuntimeSqlAccountName().Equals("sa", StringComparison.OrdinalIgnoreCase))
			{
				string sql = String.Format(CultureInfo.CurrentCulture, "EXEC sp_addrolemember @rolename=N'gs_GalleryServerProRole', @membername=N'{0}'", GetRuntimeSqlAccountName());

				try
				{
					if (!ExecuteSql(sql))
					{
						return false;
					}
				}
				catch
				{
					string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Cannot_Add_User_To_Role_Msg, GetRuntimeSqlAccountName());
					msg = String.Format(CultureInfo.CurrentCulture, "{0} {1}", msg, lblErrMsgReadyToInstall.InnerText);
					ShowErrorMsgThatOccurredDuringInstallation(msg, null, null, lblErrMsgReadyToInstall, null, null);
					return false;
				}
			}
			#endregion

			return true;
		}

		private void UpdateWebConfigFile()
		{
			try
			{
				UpdateConnectionStringInConfigFile();

				UpdateDataProvidersInWebConfigFile();

				_webConfigSuccessfullyUpdated = true;
			}
			catch { } // Swallow exception; we will be able to notice this failed because the webConfigSuccessfullyUpdated flag remains false.
		}

		private void UpdateConnectionStringInConfigFile()
		{
			if (this.ProviderDb == ProviderDataStore.SQLite)
				return; // Nothing to configure for SQLite
			
			System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
			System.Configuration.ConnectionStringSettings cnString = config.ConnectionStrings.ConnectionStrings[CN_STRING_NAME];

			if (cnString != null)
			{
				// Update existing connection string.
				cnString.ConnectionString = GetDbRuntimeConnectionString(true);
				config.Save();
			}
			else
			{
				// Didn't find connection string. Add a new one.
				cnString = new System.Configuration.ConnectionStringSettings(CN_STRING_NAME, GetDbRuntimeConnectionString(true));
				config.ConnectionStrings.ConnectionStrings.Add(cnString);
				config.Save();
			}
		}

		private void UpdateGalleryServerProConfigFile()
		{
			try
			{
				System.Configuration.Configuration configSection = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
				GalleryServerProConfigSettings configSettings = ConfigManager.OpenGalleryServerProConfigSection(configSection);
				configSettings.Core.EncryptionKey = EncryptionKey;

				configSettings.DataProvider.DefaultProvider = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteGalleryServerProProvider" : "SqlServerGalleryServerProProvider");

				configSection.Save();
				_gspConfigSuccessfullyUpdated = true;
			}
			catch { } // Swallow exception; we will be able to notice this failed because the webConfigSuccessfullyUpdated flag remains false.
		}

		private void UpdateDataProvidersInWebConfigFile()
		{
			if (!AreProvidersSpecifiedInWebConfig())
			{
				string membershipProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
				string roleProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteRoleProvider" : "SqlRoleProvider");
				string profileProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteProfileProvider" : "SqlProfileProvider");

				System.Configuration.Configuration configSection = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
				MembershipSection membershipSection = Configuration.ConfigManager.OpenMembershipConfigSection(configSection);
				RoleManagerSection roleSection = Configuration.ConfigManager.OpenRoleConfigSection(configSection);
				ProfileSection profileSection = Configuration.ConfigManager.OpenProfileConfigSection(configSection);

				membershipSection.DefaultProvider = membershipProviderName;
				roleSection.DefaultProvider = roleProviderName;
				profileSection.DefaultProvider = profileProviderName;

				configSection.Save();
			}
		}

		/// <summary>
		/// Verify that the specified Win/SQL login can used as the runtime account for Gallery Server Pro.	Validation is 
		/// performed by trying to connect using the specified runtime connection setting. If it connects, the method returns
		/// true. If not, and if the database is SQL Server 2005 or later, the method attempts to create the account and then 
		/// roll back the transaction that created it. If an error occurs, the appropriate message is assigned to the label 
		/// on the DbRuntime panel. Always returns true if the DbAdmin account is specified (rblDbRuntimeConnectType.SelectedIndex == 0). 
		/// Note: SQL Server 2000 and earlier does not support wrapping add user scripts in a transaction, so this method will
		/// return true even if the admin account does not have permission to create the specified runtime login. In this case
		/// the user will receive an error when executing the actual install script.
		/// </summary>
		/// <returns>Returns true if the Win/SQL login can be used as the runtime account for Gallery Server Pro;
		/// otherwise returns false.</returns>
		private bool ValidateRuntimeLogin()
		{
			string sql = String.Empty;
			string msg = String.Empty;

			#region Test 1: Try to connect

			if (CanConnectToDatabase(GetDbRuntimeConnectionString(false)))
				return true; // Login account already exists and we're able to log in, so no further testing is needed.

			#endregion

			#region Test 2: If user specified a SQL login, see if it exists

			SqlVersion sqlVersion = GetSqlVersion();
			if (rblDbRuntimeConnectType.SelectedIndex == 2) // SQL login
			{
				// See if SQL login exists.
				bool loginExists = false;
				switch (sqlVersion)
				{
					case SqlVersion.Sql2000: sql = "SELECT COUNT(*) FROM master.dbo.syslogins WHERE loginname = @UserName"; break;
					case SqlVersion.Sql2005:
					case SqlVersion.Sql2008:
					case SqlVersion.PostSql2008: sql = "SELECT COUNT(*) FROM sys.server_principals WHERE name = @UserName AND type = 'S'"; break;
				}

				using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(false)))
				{
					using (SqlCommand cmd = new SqlCommand(sql, cn))
					{
						cmd.Parameters.Add(new SqlParameter("@UserName", txtDbRuntimeUserName.Text));
						cn.Open();
						using (System.Data.IDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
							if ((dr != null) && dr.Read() && (dr.GetInt32(0) > 0))
							{
								loginExists = true;
							}
						}
					}
				}

				if (loginExists)
				{
					// If we get here that means the SQL login exists but the user-supplied password is incorrect. Show error and exit.
					msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Invalid_Pwd_For_Existing_Sql_Login_Msg, txtDbRuntimeUserName.Text);
					lblErrMsgDbRuntime.InnerText = msg;
					lblErrMsgDbRuntime.Attributes["class"] = "msgwarning visible";
					return false;
				}
			}

			#endregion

			#region Test 3: Run the add user script wrapped in a rollback transaction

			// Wrapping the add user script in a rollback transaction fails in SQL Server 2000 and earlier, so
			// only run this test for SQL Server 2005 or later.
			if ((sqlVersion == SqlVersion.Sql2005) || (sqlVersion == SqlVersion.Sql2008) || (sqlVersion == SqlVersion.PostSql2008))
			{
				sql = String.Format(CultureInfo.CurrentCulture, "BEGIN TRAN {0} ROLLBACK TRAN", GetAddSqlUserScript());

				if (!ExecuteSql(sql))
				{
					msg = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Installer_Cannot_Validate_User_Msg, lblErrMsgDbRuntime.InnerText);
					lblErrMsgDbRuntime.InnerText = msg;
					lblErrMsgDbRuntime.Attributes["class"] = "msgwarning visible";
					return false;
				}
			}

			#endregion

			return true;
		}

		private void ShowErrorMsgThatOccurredDuringInstallation(string errorMsg, string sqlThatCausedError, string callStack,
			System.Web.UI.HtmlControls.HtmlGenericControl errorMsgControl,
			System.Web.UI.HtmlControls.HtmlGenericControl errorSqlControl,
			System.Web.UI.HtmlControls.HtmlGenericControl errorCallStackControl)
		{
			#region Show error message

			if (errorMsgControl != null)
			{
				if (!String.IsNullOrEmpty(errorMsg))
				{
					errorMsgControl.InnerHtml = errorMsg;
					errorMsgControl.Attributes["class"] = "msgwarning visible";
				}
				else
				{
					errorMsgControl.InnerHtml = String.Empty;
					errorMsgControl.Attributes["class"] = "invisible";
				}
			}

			#endregion

			#region Show SQL

			if (errorSqlControl != null)
			{
				if (!String.IsNullOrEmpty(sqlThatCausedError))
				{
					errorSqlControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Sql_Error_Msg, sqlThatCausedError);
					errorSqlControl.Attributes["class"] = "visible";
				}
				else
				{
					errorSqlControl.InnerHtml = String.Empty;
					errorSqlControl.Attributes["class"] = "invisible";
				}
			}

			#endregion

			#region Show callstack

			if (errorCallStackControl != null)
			{
				if (!String.IsNullOrEmpty(callStack))
				{
					errorCallStackControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Sql_Error_CallStack_Label, callStack);
					errorCallStackControl.Attributes["class"] = "visible";
				}
				else
				{
					errorCallStackControl.InnerHtml = String.Empty;
					errorCallStackControl.Attributes["class"] = "invisible";
				}
			}

			#endregion

			if (errorMsgControl.ID == lblErrMsgReadyToInstall.ID)
			{
				lblReadyToInstallHeaderMsg.InnerText = Resources.GalleryServerPro.Installer_Install_Error_Hdr;
				lblReadyToInstallDetailMsg.Attributes["class"] = "invisible";
			}
		}

		/// <summary>
		/// Get the Windows account or SQL login the user specified to be used during runtime operation of Gallery Server.
		/// Ex: DOMAIN\SqlUser, GalleryServerWebUser
		/// </summary>
		/// <returns>Returns the Windows account or SQL login the user specified to be used during runtime operation of Gallery Server.</returns>
		private string GetRuntimeSqlAccountName()
		{
			string userName = String.Empty;
			if (rblDbRuntimeConnectType.SelectedIndex == 0) // Use same connection as previously specified
			{
				if (rblDbAdminConnectType.SelectedIndex == 0)
					userName = IisIdentity;
				else
					userName = txtDbAdminUserName.Text;
			}
			else if (rblDbRuntimeConnectType.SelectedIndex == 1) // Use Win Authentication
			{
				userName = IisIdentity;
			}
			else // SQL Authentication
			{
				userName = txtDbRuntimeUserName.Text;
			}
			return userName;
		}

		/// <summary>
		/// Get the full path to the script that adds the user to the database.
		/// </summary>
		/// <returns>Returns the full path to the script that adds the user to the database.</returns>
		private string GetAddSqlUserScriptPath()
		{
			string sqlScriptName = String.Empty;
			bool isSqlLogin = rblDbRuntimeConnectType.SelectedIndex == 2 ? true : false;

			switch (this.GetSqlVersion())
			{
				case SqlVersion.Sql2000: sqlScriptName = (isSqlLogin ? "sql/AddSqlLoginSql2000.sql" : "sql/AddWinLoginSql2000.sql"); break;
				case SqlVersion.Sql2005:
				case SqlVersion.Sql2008:
				case SqlVersion.PostSql2008: sqlScriptName = (isSqlLogin ? "sql/AddSqlLoginSql2005.sql" : "sql/AddWinLoginSql2005.sql"); break;
			}

			return Server.MapPath(sqlScriptName);
		}

		/// <summary>
		/// Get the script that adds the user to the database. The script is generated from .sql files in the sql directory.
		/// The placeholder variables in the script for user name, password, and default database have been replaced with
		/// their actual values.
		/// </summary>
		/// <returns>Returns a script that adds the user to the database.</returns>
		private string GetAddSqlUserScript()
		{
			// <param name="sourceScriptName">The name of the script used as the template for creating the SQL. This parameter
			// can be used in any error messages should a problem occur when running the script.</param>
			string sql = String.Empty;
			using (System.IO.StreamReader stream = new System.IO.StreamReader(GetAddSqlUserScriptPath()))
			{
				sql = stream.ReadToEnd();
			}

			sql = sql.Replace("#GalleryServerWebUserName#", MakeSqlSafe(GetRuntimeSqlAccountName()));
			sql = sql.Replace("#GalleryServerWebUserPwd#", MakeSqlSafe(this.DbRuntimePassword));
			sql = sql.Replace("#DbName#", MakeSqlSafe(ddlDbList.SelectedValue));

			return sql;
		}

		private static string MakeSqlSafe(string sql)
		{
			return sql.Replace("'", "''");
		}

		private static bool CanConnectToDatabase(string cnString)
		{
			bool canConnect = false;
			try
			{
				using (SqlConnection cn = new SqlConnection(cnString))
				{
					cn.Open();
					cn.Close();
					canConnect = true;
				}
			}
			catch (InvalidOperationException) { }
			catch (SqlException) { }

			return canConnect;
		}

		private bool ConfigureAspNetMembership()
		{
			if (!ExecuteSqlInFile(Server.MapPath("sql/InstallCommon.sql"))) return false;
			if (!ExecuteSqlInFile(Server.MapPath("sql/InstallMembership.sql"))) return false;
			if (!ExecuteSqlInFile(Server.MapPath("sql/InstallProfile.sql"))) return false;
			if (!ExecuteSqlInFile(Server.MapPath("sql/InstallRoles.sql"))) return false;

			return true;
		}

		private bool ConfigureGalleryServerSchemaForSqlServer()
		{
			string sqlScriptName = String.Empty;

			switch (this.GetSqlVersion())
			{
				case SqlVersion.Sql2000: sqlScriptName = "sql/InstallGalleryServerProSql2000.sql"; break;
				case SqlVersion.Sql2005:
				case SqlVersion.Sql2008:
				case SqlVersion.PostSql2008: sqlScriptName = "sql/InstallGalleryServerProSql2005.sql"; break;
			}

			if (!ExecuteSqlInFile(Server.MapPath(sqlScriptName))) return false;

			if (chkUpgradeFromVersion1.Checked)
			{
				if (!ExecuteSqlInFile(Server.MapPath("sql/GS_1.x_to_2_upgrade.sql"))) return false;
			}

			if (!InitializeGalleryServerForSqlServer()) return false;

			return true;
		}

		private bool InitializeGalleryServerForSqlServer()
		{
			using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(true)))
			{
				using (SqlCommand cmd = new SqlCommand("gs_InitializeGalleryServerPro", cn))
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;

					cmd.Parameters.Add("@GalleryId", System.Data.SqlDbType.Int).Value = 1;
					cmd.Parameters.Add("@ApplicationName", System.Data.SqlDbType.NVarChar, 512).Value = APP_NAME;
					cmd.Parameters.Add("@SysAdminRoleName", System.Data.SqlDbType.NVarChar, 256).Value = Resources.GalleryServerPro.Installer_Sys_Admin_Role_Name;
					cmd.Parameters.Add("@SysAdminRoleNameDescription", System.Data.SqlDbType.NVarChar, 256).Value = Resources.GalleryServerPro.Installer_Sys_Admin_Role_Name_Description;
					cmd.Parameters.Add("@AdminEmail", System.Data.SqlDbType.NVarChar, 256).Value = txtGsAdminEmail.Text;
					cmd.Parameters.Add("@AdminUserName", System.Data.SqlDbType.NVarChar, 256).Value = txtGsAdminUserName.Text;
					cmd.Parameters.Add("@AdminPassword", System.Data.SqlDbType.NVarChar, 256).Value = this.GsAdminPassword;
					cmd.Parameters.Add("@PasswordFormat", System.Data.SqlDbType.Int).Value = 0;
					cmd.Parameters.Add("@CreateSamples", System.Data.SqlDbType.Bit).Value = (chkUpgradeFromVersion1.Checked ? 0 : 1);

					try
					{
						cn.Open();
						cmd.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						string msg = String.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.GalleryServerPro.Installer_Initialize_Db_Error_Msg, ex.Message);
						ShowErrorMsgThatOccurredDuringInstallation(msg, null, ex.StackTrace, lblErrMsgReadyToInstall, null, lblErrMsgReadyToInstallCallStack);
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>
		/// Validate that the selected database can be logged in to by the user-specified credentials and that
		/// it does not already contain Gallery Server tables. If not valid, the screen is updated with the relevant
		/// message.
		/// </summary>
		/// <returns>Returns true if the selected database can be logged in to by the user-specified credentials and that
		/// it does not already contain Gallery Server tables; otherwise returns false.</returns>
		private bool ValidateChooseDb()
		{
			string msg;
			bool isValid = false;

			if (CanConnectToDatabaseInAdminMode(out msg))
			{
				if (DbDoesNotContainGalleryServerTablesOrTheyAreEmpty())
				{
					// We can connect to the database and the tables don't exist or are empty. Ok to move to next step.
					isValid = true;
				}
				else
				{
					msg = Resources.GalleryServerPro.Installer_ChooseDb_Existing_Data_Found_Msg;
				}
			}

			if (!String.IsNullOrEmpty(msg))
			{
				lblErrMsgChooseDb.InnerText = msg;
				lblErrMsgChooseDb.Attributes["class"] = "msgwarning visible";
			}

			return isValid;
		}

		/// <summary>
		/// Verify the SQLite or SQL Server providers are configured in web.config for membership, roles, and profile, and the
		/// data provider is configured in galleryserverpro.config. The requested provider does not have to be specified as the 
		/// defaultProvider, but if not, then make sure the installer has permission to edit the file. If the provider is not configured, 
		/// or if the installer does not have permission to edit the file, alert the user with a message.
		/// </summary>
		/// <returns>Returns <c>true</c> if the requested provider is already configured, or the installer has permission to change it if
		/// it is not configured; otherwise returns <c>false</c>.</returns>
		private bool ValidateDataProvider()
		{
			// web.config: Check to see if the selected provider is specified as the default provider or, if it is not, that the installer
			// has permission to edit it.
			if (!AreProvidersSpecifiedInWebConfig())
			{
				if (WebsiteController.GetCurrentTrustLevel() != ApplicationTrustLevel.Full)
				{
					if (rbDataProviderSQLite.Checked)
						lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_WebConfig_Not_UpdateableToSQLite_ReducedTrust;
					else
						lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_WebConfig_Not_UpdateableToSqlServer_ReducedTrust;
					
					lblErrMsgChooseDbEngine.Attributes["class"] = "msgwarning";
					pnlDbEngineMsg.Visible = true;
					return false;
				}
				else if (!InstallerIsAbleToUpdateWebConfig())
				{
					if (rbDataProviderSQLite.Checked)
						lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_WebConfig_Not_UpdateableToSQLite_InsufficientPermission;
					else
						lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_WebConfig_Not_UpdateableToSqlServer_InsufficientPermission;

					lblErrMsgChooseDbEngine.Attributes["class"] = "msgwarning";
					pnlDbEngineMsg.Visible = true;
					return false;
				}
			}

			// galleryserverpro.config: Check to see if the selected provider is specified as the default provider or, if it is not, that the installer
			// has permission to edit the file.
			if (!IsProviderSpecifiedInGalleryServerProConfig())
			{
				if (WebsiteController.GetCurrentTrustLevel() != ApplicationTrustLevel.Full)
				{
					lblErrMsgChooseDbEngine.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_DataProvider_GalleryServerProConfig_Not_Updateable_ReducedTrust, this.ProviderDb);

					lblErrMsgChooseDbEngine.Attributes["class"] = "msgwarning";
					pnlDbEngineMsg.Visible = true;
					return false;
				}
				else if (!InstallerIsAbleToUpdateGalleryServerProConfig())
				{
					lblErrMsgChooseDbEngine.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_DataProvider_GalleryServerProConfig_Not_Updateable_InsufficientPermission, this.ProviderDb);

					lblErrMsgChooseDbEngine.Attributes["class"] = "msgwarning";
					pnlDbEngineMsg.Visible = true;
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether the membership, role, and profile providers in web.config are configured for the desired data provider.
		/// </summary>
		/// <returns>Returns <c>true</c> if the membership, role, and profile providers in web.config are configured for the data provider
		/// selected by the user; otherwise returns <c>false</c>.</returns>
		/// <remarks>Unfortunately, we cannot use WebConfigurationManager.GetSection, as that works only in Full Trust.</remarks>
		private bool AreProvidersSpecifiedInWebConfig()
		{
			string membershipProviderInConfig = String.Empty;
			string roleProviderInConfig = String.Empty;
			string profileProviderInConfig = String.Empty;

			using (FileStream fs = new FileStream(Server.MapPath("~/web.config"), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader sr = new StreamReader(fs))
				{
					XmlReader r = XmlReader.Create(sr);
					while (r.Read())
					{
						if ((r.Name == "membership") && r.MoveToAttribute("defaultProvider"))
							membershipProviderInConfig = r.Value;

						if ((r.Name == "roleManager") && r.MoveToAttribute("defaultProvider"))
							roleProviderInConfig = r.Value;

						if ((r.Name == "profile") && r.MoveToAttribute("defaultProvider"))
							profileProviderInConfig = r.Value;
					}
				}
			}

			string membershipProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
			string roleProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteRoleProvider" : "SqlRoleProvider");
			string profileProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteProfileProvider" : "SqlProfileProvider");

			return ((membershipProviderInConfig.Equals(membershipProviderName, StringComparison.InvariantCultureIgnoreCase))
							&& (roleProviderInConfig.Equals(roleProviderName, StringComparison.InvariantCultureIgnoreCase))
							&& (profileProviderInConfig.Equals(profileProviderName, StringComparison.InvariantCultureIgnoreCase)));
		}

		/// <summary>
		/// Determines whether the web.config file contains entries for the selected data provider for the membership, role, 
		/// and profile providers. Note that this does not verify the provider is selected (that is, is specified in the 
		/// defaultProvider attribute); it only verifies that the entry exists within those sections. Use the method
		/// AreProvidersSpecifiedInWebConfig to determine whether the selected provider is selected as the currently active
		/// provider.
		/// </summary>
		/// <param name="providerName">The provider to check for in web.config (e.b. SQLite or SqlServer).</param>
		/// <returns>Returns <c>true</c> if the membership, role, and profile providers in web.config contain entries
		/// corresponding to the data provider selected by the user; otherwise returns <c>false</c>.</returns>
		/// <remarks>Unfortunately, we cannot use WebConfigurationManager.GetSection, as that works only in Full Trust.</remarks>
		private static bool AreProvidersAvailableInWebConfig(string providerName)
		{
			string membershipProviderName = (providerName == "SQLite" ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
			string roleProviderName = (providerName == "SQLite" ? "SQLiteRoleProvider" : "SqlRoleProvider");
			string profileProviderName = (providerName == "SQLite" ? "SQLiteProfileProvider" : "SqlProfileProvider");

			bool membershipProviderIsAvailable = false;
			bool roleProviderIsAvailable = false;
			bool profileProviderIsAvailable = false;

			using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/web.config"), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader sr = new StreamReader(fs))
				{
					XmlReader r = XmlReader.Create(sr);
					while (r.Read())
					{
						if (r.Name == "membership")
							membershipProviderIsAvailable = (r.ReadInnerXml().IndexOf(String.Format("name=\"{0}\"", membershipProviderName)) > -1);

						if (r.Name == "roleManager")
							roleProviderIsAvailable = (r.ReadInnerXml().IndexOf(String.Format("name=\"{0}\"", roleProviderName)) > -1);

						if (r.Name == "profile")
							profileProviderIsAvailable = (r.ReadInnerXml().IndexOf(String.Format("name=\"{0}\"", profileProviderName)) > -1);
					}
				}
			}

			return (membershipProviderIsAvailable && roleProviderIsAvailable && profileProviderIsAvailable);
		}

		/// <summary>
		/// Determines whether the galleryserverpro.config file contains entries for the selected data provider. Note that this does 
		/// not verify the provider is selected (that is, is specified in the defaultProvider attribute); it only verifies that the 
		/// entry exists within the section. Use the method IsProviderSpecifiedInGalleryServerProConfig to determine whether the 
		/// selected provider is configured as the currently active provider.
		/// </summary>
		/// <param name="providerName">The provider to check for in web.config (e.b. SQLite or SqlServer).</param>
		/// <returns>Returns <c>true</c> if the data provider in galleryserverpro.config contains an entry
		/// corresponding to the data provider selected by the user; otherwise returns <c>false</c>.</returns>
		/// <remarks>We can use WebConfigurationManager.GetSection, even when running under Medium Trust, because we are querying
		/// a custom section. This is in stark contrast to discovering the providers in web.config, which require the user of an
		/// XmlReader to work in Medium Trust.</remarks>
		private static bool IsProviderAvailableInGalleryServerProConfig(string providerName)
		{
			string dataProviderName = (providerName == "SQLite" ? "SQLiteGalleryServerProProvider" : "SqlServerGalleryServerProProvider");

			GalleryServerProConfigSettings _galleryServerProConfigSection = (GalleryServerProConfigSettings)WebConfigurationManager.GetSection("system.web/galleryServerPro");

			foreach (ProviderSettings provider in _galleryServerProConfigSection.DataProvider.Providers)
			{
				if (provider.Name.Equals(dataProviderName))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether the data provider in galleryserverpro.config is configured for the desired data provider.
		/// </summary>
		/// <returns>Returns <c>true</c> if the data provider in galleryserverpro.config is configured for the data provider
		/// selected by the user; otherwise returns <c>false</c>.</returns>
		/// <remarks>We can use WebConfigurationManager.GetSection, even when running under Medium Trust, because we are querying
		/// a custom section. This is in stark contrast to discovering the providers in web.config, which require the user of an
		/// XmlReader to work in Medium Trust.</remarks>
		private bool IsProviderSpecifiedInGalleryServerProConfig()
		{
			string dataProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteGalleryServerProProvider" : "SqlServerGalleryServerProProvider");

			GalleryServerProConfigSettings _galleryServerProConfigSection = (GalleryServerProConfigSettings)WebConfigurationManager.GetSection("system.web/galleryServerPro");

			return _galleryServerProConfigSection.DataProvider.DefaultProvider.Equals(dataProviderName);
		}

		#region These work in Full Trust only. Grrr...

		//private bool AreProvidersAvailableInWebConfig()
		//{
		//  //return true;
		//  // This only works in Full Trust. :-(
		//  string membershipProviderName = (this.ProviderDb == "SQLite" ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
		//  string roleProviderName = (this.ProviderDb == "SQLite" ? "SQLiteRoleProvider" : "SqlRoleProvider");
		//  string profileProviderName = (this.ProviderDb == "SQLite" ? "SQLiteProfileProvider" : "SqlProfileProvider");

		//  bool membershipProviderIsAvailable = false;
		//  bool roleProviderIsAvailable = false;
		//  bool profileProviderIsAvailable = false;

		//  MembershipSection membershipSection = (MembershipSection)WebConfigurationManager.GetSection("system.web/membership");
		//  foreach (System.Configuration.ProviderSettings provider in membershipSection.Providers)
		//  {
		//    if (provider.Name.Equals(membershipProviderName))
		//    {
		//      membershipProviderIsAvailable = true;
		//      break;
		//    }
		//  }

		//  RoleManagerSection roleSection = (RoleManagerSection)WebConfigurationManager.GetSection("system.web/roleManager");
		//  foreach (System.Configuration.ProviderSettings provider in roleSection.Providers)
		//  {
		//    if (provider.Name.Equals(roleProviderName))
		//    {
		//      roleProviderIsAvailable = true;
		//      break;
		//    }
		//  }

		//  ProfileSection profileSection = (ProfileSection)WebConfigurationManager.GetSection("system.web/profile");
		//  foreach (System.Configuration.ProviderSettings provider in profileSection.Providers)
		//  {
		//    if (provider.Name.Equals(profileProviderName))
		//    {
		//      profileProviderIsAvailable = true;
		//      break;
		//    }
		//  }

		//  return (membershipProviderIsAvailable && roleProviderIsAvailable && profileProviderIsAvailable);
		//}

		/// <summary>
		/// Determines whether the membership, role, and profile providers in web.config are configured for the desired data provider.
		/// </summary>
		/// <returns>Returns <c>true</c> if the membership, role, and profile providers in web.config are configured for the data provider
		/// selected by the user; otherwise returns <c>false</c>.</returns>
		//private bool AreProvidersSpecifiedInWebConfig()
		//{
		//  //return true;
		//  try
		//  {
		//    MembershipSection membershipSection = (MembershipSection)WebConfigurationManager.GetSection("system.web/membership");
		//    RoleManagerSection roleManagerSection = (RoleManagerSection)WebConfigurationManager.GetSection("system.web/roleManager");
		//    ProfileSection profileSection = (ProfileSection)WebConfigurationManager.GetSection("system.web/profile");

		//    if (this.ProviderDb == "SQLite")
		//    {
		//      if ((membershipSection.DefaultProvider.Equals("SQLiteMembershipProvider"))
		//          && (roleManagerSection.DefaultProvider.Equals("SQLiteRoleProvider"))
		//          && (profileSection.DefaultProvider.Equals("SQLiteProfileProvider")))
		//        return true;
		//      else
		//        return false;
		//    }
		//    else if (this.ProviderDb == "SQL Server")
		//    {
		//      if ((membershipSection.DefaultProvider.Equals("SqlMembershipProvider"))
		//          && (roleManagerSection.DefaultProvider.Equals("SqlRoleProvider"))
		//          && (profileSection.DefaultProvider.Equals("SqlProfileProvider")))
		//        return true;
		//      else
		//        return false;
		//    }
		//    else
		//      throw new ArgumentException("providerDb");
		//  }
		//  catch (Exception ex)
		//  {
		//    if ((ex is System.Security.SecurityException) || ((ex.InnerException != null) && (ex.InnerException is System.Security.SecurityException)))
		//    {
		//      return false;
		//    }
		//    else
		//    {
		//      throw;
		//    }
		//  }
		//}

		#endregion

		/// <summary>
		/// Determines whether the installer has permission to update the web.config file.
		/// </summary>
		/// <returns>Returns <c>true</c> if the installer has permission to update the web.config file; otherwise returns <c>false</c>.</returns>
		private bool InstallerIsAbleToUpdateWebConfig()
		{
			try
			{
				// Test 1: Can we write to the directory containing web.config? This will fail when NTFS permissions prevent it.
				HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(Server.MapPath(Request.ApplicationPath));
				
				// Test 2: Can we open and save the web.config file? This will fail when the app is running under Medium Trust and when
				// the web.config file does not have 'write' NTFS permission.
				const string webConfigFilename = "web.config";
				string webConfigPath = Server.MapPath(Request.ApplicationPath); // C:\inetpub\wwwroot\gs
				string webConfigFullPath = Path.Combine(webConfigPath, webConfigFilename); // C:\inetpub\wwwroot\gs\web.config
				using (FileStream fs = File.OpenWrite(webConfigFullPath)) {}

				// Test 3: Do we have 'modify' NTFS permission on web.config? We test this by temporarily renaming web.config.
				string newWebConfigFilename = Guid.NewGuid().ToString();
				File.Move(webConfigFullPath, Path.Combine(webConfigPath, newWebConfigFilename));
				File.Move(Path.Combine(webConfigPath, newWebConfigFilename), webConfigFullPath);

				// We could use the following but it includes all applicable config files (like galleryserverpro.config), and we
				// want to test ONLY web.config.
				//System.Configuration.Configuration configSection = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
				//configSection.Save(ConfigurationSaveMode.Minimal, true);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Determines whether the installer has permission to update the galleryserverpro.config file.
		/// </summary>
		/// <returns>Returns <c>true</c> if the installer has permission to update the galleryserverpro.config file; otherwise returns <c>false</c>.</returns>
		private bool InstallerIsAbleToUpdateGalleryServerProConfig()
		{
			try
			{
				// Test 1: Can we write to the directory containing galleryserverpro.config? This will fail when NTFS permissions prevent it.
				HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(Server.MapPath("~/config/"));

				// Test 2: Can we open and save the galleryserverpro.config file? This will fail when the app is running under Medium Trust and when
				// the galleryserverpro.config file does not have 'write' NTFS permission.
				const string galleryServerProConfigFilename = "galleryserverpro.config";
				string galleryServerProConfigPath = Path.Combine(Server.MapPath(Request.ApplicationPath), "config"); // C:\inetpub\wwwroot\gs\config
				string galleryServerProConfigFullPath = Path.Combine(galleryServerProConfigPath, galleryServerProConfigFilename); // C:\inetpub\wwwroot\gs\config\galleryserverpro.config
				using (FileStream fs = File.OpenWrite(galleryServerProConfigFullPath)) { }

				// Test 3: Do we have 'modify' NTFS permission on galleryserverpro.config? We test this by temporarily renaming galleryserverpro.config.
				string newGalleryServerProConfigFilename = Guid.NewGuid().ToString();
				File.Move(galleryServerProConfigFullPath, Path.Combine(galleryServerProConfigPath, newGalleryServerProConfigFilename));
				File.Move(Path.Combine(galleryServerProConfigPath, newGalleryServerProConfigFilename), galleryServerProConfigFullPath);

				return true;
			}
			catch
			{
				return false;
			}
		}

		private void ShowPreviousPanel()
		{
			switch (this.CurrentWizardPanel)
			{
				case WizardPanel.Welcome: break;
				case WizardPanel.License:
					{
						SetCurrentPanel(WizardPanel.Welcome, Welcome);
						break;
					}
				case WizardPanel.DataProvider:
					{
						SetCurrentPanel(WizardPanel.License, License);
						break;
					}
				case WizardPanel.DbAdmin:
					{
						ConfigureDbEngineChoices();
						SetCurrentPanel(WizardPanel.DataProvider, DataProvider);
						break;
					}
				case WizardPanel.ChooseDb:
					{
						SetCurrentPanel(WizardPanel.DbAdmin, DbAdmin);
						break;
					}
				case WizardPanel.DbRuntime:
					{
						if (GalleryServer1TablesExistAndTheyHaveRecords())
						{
							SetCurrentPanel(WizardPanel.UpgradeFromVersion1, UpgradeFromVersion1);
						}
						else
						{
							SetCurrentPanel(WizardPanel.ChooseDb, ChooseDb);
						}
						break;
					}
				case WizardPanel.UpgradeFromVersion1:
					{
						SetCurrentPanel(WizardPanel.ChooseDb, ChooseDb);
						break;
					}
				case WizardPanel.SetupOptions:
					{
						SetCurrentPanel(WizardPanel.DbRuntime, DbRuntime);
						break;
					}
				case WizardPanel.GsAdmin:
					{
						if (rbDataProviderSQLite.Checked)
						{
							ConfigureDbEngineChoices();
							SetCurrentPanel(WizardPanel.DataProvider, DataProvider);
						}
						else
							SetCurrentPanel(WizardPanel.SetupOptions, SetupOptions);
						break;
					}
				case WizardPanel.ReadyToInstall:
					{
						SetCurrentPanel(WizardPanel.GsAdmin, GsAdmin);
						break;
					}
			}
		}

		/// <summary>
		/// Get a SQL Server connection string to be used during the configuration of Gallery Server. If the includeDatabaseName
		/// parameter is true, then include the database selected during the "Choose Database" wizard step. The connection string is
		/// generated from user supplied data in one of the wizard steps.
		/// </summary>
		/// <param name="includeDatabaseName">A value indicating whether the connection string includes the database selected during
		/// the "Choose Database" wizard step.</param>
		/// <returns>Returns a connection string that can be used to connect to the specified SQL Server.</returns>
		private string GetDbAdminConnectionString(bool includeDatabaseName)
		{
			string useWinAuthentication = (rblDbAdminConnectType.SelectedIndex == 0 ? "yes" : "no");

			if (includeDatabaseName)
			{
				return String.Format(CultureInfo.CurrentCulture, "server={0};uid={1};pwd={2};Trusted_Connection={3};database={4};Application Name={5}", txtDbSqlName.Text, txtDbAdminUserName.Text, this.DbAdminPassword, useWinAuthentication, ddlDbList.SelectedValue, APP_NAME);
			}
			else
			{
				return String.Format(CultureInfo.CurrentCulture, "server={0};uid={1};pwd={2};Trusted_Connection={3};Application Name={4}", txtDbSqlName.Text, txtDbAdminUserName.Text, this.DbAdminPassword, useWinAuthentication, APP_NAME);
			}
		}

		/// <summary>
		/// Get a SQL Server connection string to be used during normal operation of Gallery Server. If the includeDatabaseName
		/// parameter is true, then include the database selected during the "Choose Database" wizard step. The connection string is
		/// generated from user supplied data in one of the wizard steps. This method generates the connection string that will be 
		/// written to the web.config file.
		/// </summary>
		/// <param name="includeDatabaseName">A value indicating whether the connection string includes the database selected during
		/// the "Choose Database" wizard step.</param>
		/// <returns>Returns a connection string that can be used to connect to the specified SQL Server.</returns>
		protected string GetDbRuntimeConnectionString(bool includeDatabaseName)
		{
			if (rblDbRuntimeConnectType.SelectedIndex == 0)
			{
				// User wants to use the same account used to configure the database.
				return GetDbAdminConnectionString(includeDatabaseName);
			}

			string useWinAuthentication = (rblDbRuntimeConnectType.SelectedIndex == 1 ? "yes" : "no");

			if (includeDatabaseName)
			{
				return String.Format(CultureInfo.CurrentCulture, "server={0};uid={1};pwd={2};Trusted_Connection={3};database={4}", txtDbSqlName.Text, txtDbRuntimeUserName.Text, this.DbRuntimePassword, useWinAuthentication, ddlDbList.SelectedValue);
			}
			else
			{
				return String.Format(CultureInfo.CurrentCulture, "server={0};uid={1};pwd={2};Trusted_Connection={3}", txtDbSqlName.Text, txtDbRuntimeUserName.Text, this.DbRuntimePassword, useWinAuthentication);
			}
		}

		private bool CanConnectToDatabaseInAdminMode(out string errorMessage)
		{
			try
			{
				using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(true)))
				{
					cn.Open();
					cn.Close();
				}

				errorMessage = "";

				return true;
			}
			catch (SqlException ex)
			{
				switch (ex.Number)
				{
					case 4060:	// 4060 = Login failure
						if (rblDbAdminConnectType.SelectedIndex == 0)
						{
							string webAccount = IisIdentity;
							errorMessage = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_DbAdmin_WinAuth_Error_Msg, ddlDbList.SelectedValue, webAccount);
						}
						else
						{
							string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_DbAdmin_SqlAuth_Error_Msg, txtDbAdminUserName.Text, ddlDbList.SelectedValue);
							errorMessage = String.Concat(msg, " ", ex.Message);
						}
						break;
					default:
						errorMessage = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_DbAdmin_SqlConnect_Error_Msg, ddlDbList.SelectedValue, ex.Number, ex.Message);
						break;
				}
				return false;
			}
			catch (InvalidOperationException ex)
			{
				errorMessage = ex.Message;
				return false;
			}
		}

		private void BindDatabaseDropdownlist()
		{
			using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(false)))
			{
				cn.Open();

				using (SqlCommand cmd = new SqlCommand("select name from master..sysdatabases order by name asc", cn))
				{
					using (System.Data.IDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
					{
						ddlDbList.Items.Clear();

						while (dr.Read())
						{
							string dbName = dr["name"] as String;
							if (dbName != null)
							{
								if (!(dbName == "master" || dbName == "msdb" || dbName == "tempdb" || dbName == "model"))
								{
									ddlDbList.Items.Add(dbName); // Only add non-system databases
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns true if the specified database does not contain Gallery Server related tables or, if it does, they are
		/// empty. Specifically, this method checks the number of rows in the gs_Album table. If the table does not exist
		/// or the number of rows is zero, we assume it is okay to install and this method returns true. Otherwise, we 
		/// return false.
		/// </summary>
		/// <returns>Returns true if the specified database does not contain Gallery Server related tables or, if it does, 
		/// they are empty.</returns>
		public bool DbDoesNotContainGalleryServerTablesOrTheyAreEmpty()
		{
			bool dbDoesNotContainGalleryServerTablesOrTheyAreEmpty = true;

			using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(true)))
			{
				cn.Open();

				try
				{
					using (SqlCommand cmd = new SqlCommand("select count(*) from gs_Album", cn))
					{
						using (System.Data.IDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
							if ((dr != null) && dr.Read() && (dr.GetInt32(0) > 0))
							{
								dbDoesNotContainGalleryServerTablesOrTheyAreEmpty = false;
							}
						}
					}
				}
				catch { /* Swallow exception */ }
			}

			return dbDoesNotContainGalleryServerTablesOrTheyAreEmpty;
		}

		private bool GalleryServer1TablesExistAndTheyHaveRecords()
		{
			bool galleryServer1TablesExist = false;

			using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(true)))
			{
				cn.Open();

				try
				{
					using (SqlCommand cmd = new SqlCommand("select count(*) from Album", cn))
					{
						using (System.Data.IDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
							if ((dr != null) && dr.Read() && (dr.GetInt32(0) > 0))
							{
								galleryServer1TablesExist = true;
							}
						}
					}
				}
				catch { /* Swallow exception */ }
			}

			return galleryServer1TablesExist;
		}

		private bool ExecuteSql(string sql)
		{
			using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(sql)))
			{
				return ExecuteSqlInStream(memStream);
			}
		}

		private bool ExecuteSqlInFile(string pathToSqlFile)
		{
			bool executeSuccess = false;
			using (System.IO.Stream stream = System.IO.File.OpenRead(pathToSqlFile))
			{
				executeSuccess = ExecuteSqlInStream(stream);
			}

			if ((CurrentWizardPanel == WizardPanel.ReadyToInstall) && (!String.IsNullOrEmpty(pathToSqlFile)))
			{
				lblErrMsgReadyToInstall.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Sql_In_File_Error_Msg, System.IO.Path.GetFileName(pathToSqlFile), lblErrMsgReadyToInstall.InnerText);
			}

			return executeSuccess;
		}

		/// <summary>
		/// Execute the SQL statements in the specified stream.
		/// </summary>
		/// <param name="stream">A stream containing a series of SQL statements separated by the word GO.</param>
		/// <returns>Returns true if the SQL executed without error; otherwise returns false.</returns>
		private bool ExecuteSqlInStream(System.IO.Stream stream)
		{
			bool executeSuccessful = false;
			int timeout = 600; // Timeout for SQL Execution (seconds)
			System.IO.StreamReader sr = null;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			try
			{
				sr = new System.IO.StreamReader(stream);
				using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(true)))
				{
					cn.Open();

					while (!sr.EndOfStream)
					{
						if (sb.Length > 0) sb.Remove(0, sb.Length); // Clear out string builder

						using (SqlCommand cmd = cn.CreateCommand())
						{
							while (!sr.EndOfStream)
							{
								string s = sr.ReadLine();
								if (s != null && s.ToUpperInvariant().Trim().Equals("GO"))
								{
									break;
								}

								sb.AppendLine(s);
							}

							// Execute T-SQL against the target database
							cmd.CommandText = sb.ToString();
							cmd.CommandTimeout = timeout;

							cmd.ExecuteNonQuery();
						}
					}
				}

				executeSuccessful = true;
			}
			catch (Exception ex)
			{
				switch (this.CurrentWizardPanel)
				{
					case WizardPanel.DbAdmin:
						ShowErrorMsgThatOccurredDuringInstallation(ex.Message, null, null, lblErrMsgDbAdmin, null, null);
						break;
					case WizardPanel.ChooseDb:
						ShowErrorMsgThatOccurredDuringInstallation(ex.Message, null, null, lblErrMsgChooseDb, null, null);
						break;
					case WizardPanel.DbRuntime:
						ShowErrorMsgThatOccurredDuringInstallation(ex.Message, null, null, lblErrMsgDbRuntime, null, null);
						break;
					case WizardPanel.ReadyToInstall:
						ShowErrorMsgThatOccurredDuringInstallation(ex.Message, sb.ToString(), ex.StackTrace, lblErrMsgReadyToInstall, lblErrMsgReadyToInstallSql, lblErrMsgReadyToInstallCallStack);
						break;
				}
			}
			finally
			{
				if (sr != null)
					sr.Close();
			}

			return executeSuccessful;
		}

		/// <summary>
		/// Gets the version of SQL Server based on the authentication settings supplied by the user. Throws an exception if a version
		/// earlier than SQL Server 2000 is found.
		/// </summary>
		/// <returns>Returns an enumeration value that indicates the version of SQL Server the web installer is connected to.</returns>
		/// <exception cref="System.NotSupportedException">Thrown for versions of SQL Server earlier than SQL Server 2000.</exception>
		private SqlVersion GetSqlVersion()
		{
			SqlVersion version = SqlVersion.Unknown;

			using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(false)))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT SERVERPROPERTY('productversion')", cn))
				{
					cn.Open();
					using (SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
					{
						while (dr.Read())
						{
							string sqlVersion = dr.GetString(0);
							if (!String.IsNullOrEmpty(sqlVersion))
							{
								int majorVersion;
								if (Int32.TryParse(sqlVersion.Substring(0, sqlVersion.IndexOf(".")), out majorVersion))
								{
									if (majorVersion < 7) version = SqlVersion.PreSql2000;
									else if (majorVersion == 8) version = SqlVersion.Sql2000;
									else if (majorVersion == 9) version = SqlVersion.Sql2005;
									else if (majorVersion == 10) version = SqlVersion.Sql2008;
									else if (majorVersion > 10) version = SqlVersion.PostSql2008;
								}
							}
							break;
						}
						dr.Close();
					}
				}
			}

			if (version == SqlVersion.PreSql2000)
				throw new NotSupportedException(Resources.GalleryServerPro.Installer_PreSql2000_Error_Msg);

			return version;
		}

		private static string GenerateNewEncryptionKey()
		{
			const int encryptionKeyLength = 24;
			const int numberOfNonAlphaNumericCharactersInEncryptionKey = 3;
			string encryptionKey = Membership.GeneratePassword(encryptionKeyLength, numberOfNonAlphaNumericCharactersInEncryptionKey);

			// An ampersand (&) is invalid, since it is used as an escape character in XML files. Replace any instands with an 'X'.
			return encryptionKey.Replace("&", "X");
		}

		#endregion
	}
}

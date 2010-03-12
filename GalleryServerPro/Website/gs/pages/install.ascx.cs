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
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.gs.pages
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

	public partial class install : System.Web.UI.UserControl
	{
		#region Private Fields

		private bool _webConfigSuccessfullyUpdated;
		private bool _gspConfigSuccessfullyUpdated;
		private static string _schema;

		protected const string SQL_SERVER_CN_STRING_NAME = "SqlServerDbConnection"; // Name of Sql Server connection string in web.config. Note that if you change this,
		// also change it in the Installer_Finished_WebCfg_Need_Updating_Dtl resource setting.
		private const string APP_NAME = "Gallery Server Pro"; // The application name to be specified in the connection string
		private const string EX_SQL = "SQL";

		private static readonly string _encryptionKey = Util.GenerateNewEncryptionKey(); // The encryption key to be written to encryptionKey setting in galleryserverpro.config.

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the SQL server schema that each database object is to belong to. This value is pulled from the SqlServerSchema setting
		/// in the AppSettings section of web.config if present; otherwise it defaults to "dbo.". Each instance of {schema} in the 
		/// SQL script is replaced with this value prior to execution.
		/// </summary>
		/// <value>The SQL server schema.</value>
		private static String SqlServerSchema
		{
			get
			{
				if (String.IsNullOrEmpty(_schema))
				{
					_schema = WebConfigurationManager.AppSettings["SqlServerSchema"] ?? "dbo";
					if (!_schema.EndsWith("."))
					{
						_schema += ".";
					}
				}

				return _schema;
			}
		}

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
					HelperFunctions.ValidatePhysicalPathExistsAndIsReadWritable(Server.MapPath(Path.Combine(Util.AppRoot, GlobalConstants.AppDataDirectory)));

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

			string validationMsg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_GsAdmin_Pwd_Min_Length_Msg, UserController.MinRequiredPasswordLength);
			rfvGsAdminPassword.ErrorMessage = validationMsg;

			regGsAdminPassword.ValidationExpression = String.Format(CultureInfo.InvariantCulture, @"\S{{{0},128}}", UserController.MinRequiredPasswordLength);
			regGsAdminPassword.ErrorMessage = validationMsg;

			#endregion
		}

		private void ConfigureControlsFirstTime()
		{
			// Make sure user is logged off. If the user remains logged in during installation, he may end up with an "Insufficient Permission"
			// error after it is complete, with no access to any gallery objects.
			Controller.UserController.LogOffUser();

			rblDbAdminConnectType.Items.Add(new ListItem(String.Format(CultureInfo.CurrentCulture, "{0} ({1})", Resources.GalleryServerPro.Installer_DbAdmin_Connect_Type_Item1, IisIdentity), "0"));
			rblDbAdminConnectType.Items.Add(new ListItem(Resources.GalleryServerPro.Installer_DbAdmin_Connect_Type_Item2, "1"));
			rblDbAdminConnectType.SelectedIndex = 0;

			rblDbRuntimeConnectType.Items.Add(new ListItem(Resources.GalleryServerPro.Installer_DbRuntime_Connect_Type_Item1, "0"));
			rblDbRuntimeConnectType.Items.Add(new ListItem(String.Format(CultureInfo.CurrentCulture, "{0} ({1})", Resources.GalleryServerPro.Installer_DbRuntime_Connect_Type_Item2, IisIdentity), "1"));
			rblDbRuntimeConnectType.Items.Add(new ListItem(Resources.GalleryServerPro.Installer_DbRuntime_Connect_Type_Item3, "2"));
			rblDbRuntimeConnectType.SelectedIndex = 0;

			string version = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Gsp_Version_Text, Util.GetGalleryServerVersion());
			litVersion.Text = version;
		}

		private void SetCurrentPanel(WizardPanel panel, Control controlToShow)
		{
			Panel currentPanel = wizCtnt.FindControl(CurrentWizardPanel.ToString()) as Panel;
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
							ShowErrorMsgThatOccurredDuringInstallation(ex.Message, null, null, lblErrMsgDbAdmin, null, null);
						}
						break;
					}
				case WizardPanel.ChooseDb:
					{
						try
						{
							ValidateChooseDb();

							if (GalleryServer1TablesExistAndTheyHaveRecords())
							{
								SetCurrentPanel(WizardPanel.UpgradeFromVersion1, UpgradeFromVersion1);
							}
							else
							{
								SetCurrentPanel(WizardPanel.DbRuntime, DbRuntime);
							}
						}
						catch (Exception ex)
						{
							ShowErrorMsgThatOccurredDuringInstallation(ex.Message, null, null, lblErrMsgChooseDb, null, null);
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

						try
						{
							ValidateRuntimeLogin();

							SetCurrentPanel(WizardPanel.SetupOptions, SetupOptions);
						}
						catch (Exception ex)
						{
							string msg = String.Concat(Resources.GalleryServerPro.Installer_Cannot_Validate_User_Msg, " ", ex.Message);
							ShowErrorMsgThatOccurredDuringInstallation(msg, null, null, lblErrMsgDbRuntime, null, null);
						}

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
						try
						{
							ExecuteInstallation();

							SetCurrentPanel(WizardPanel.Finished, Finished);

							if (_webConfigSuccessfullyUpdated && _gspConfigSuccessfullyUpdated)
							{
								imgFinishedIcon.ImageUrl = Util.GetUrl("/images/ok_26x26.png");
								imgFinishedIcon.Width = Unit.Pixel(26);
								imgFinishedIcon.Height = Unit.Pixel(26);
								l61.Text = String.Format(Resources.GalleryServerPro.Installer_Finished_No_Addl_Steps_Reqd, Util.GetCurrentPageUrl());
							}
							else
							{
								imgFinishedIcon.ImageUrl = Util.GetUrl("/images/warning_32x32.png");
								imgFinishedIcon.Width = Unit.Pixel(32);
								imgFinishedIcon.Height = Unit.Pixel(32);
								l61.Text = String.Format(Resources.GalleryServerPro.Installer_Finished_Addl_Steps_Reqd, Util.GetCurrentPageUrl());
							}
							pnlWebConfigNeedUpdating.Visible = !_webConfigSuccessfullyUpdated;
							pnlGalleryServerProConfigNeedUpdating.Visible = !_gspConfigSuccessfullyUpdated;
						}
						catch (Exception ex)
						{
							string sql = null;
							if (ex.Data.Contains(EX_SQL))
								sql = ex.Data[EX_SQL].ToString();

							ShowErrorMsgThatOccurredDuringInstallation(ex.Message, sql, ex.StackTrace, lblErrMsgReadyToInstall, lblErrMsgReadyToInstallSql, lblErrMsgReadyToInstallCallStack);
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
				lblErrMsgChooseDbEngine.Attributes["class"] = "gsp_msgfriendly";
				pnlDbEngineMsg.Visible = true;
			}
			else if ((rbDataProviderSQLite.Enabled) && (!rbDataProviderSqlServer.Enabled))
			{
				// SQLite is enabled and SQL Server is disabled. Give user message why.
				lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_SqlServer_Not_Available_Msg;
				lblErrMsgChooseDbEngine.Attributes["class"] = "gsp_msgfriendly";
				pnlDbEngineMsg.Visible = true;
			}
			else if ((!rbDataProviderSQLite.Enabled) && (!rbDataProviderSqlServer.Enabled))
			{
				// Both SQLite and SQL Server are disabled. Give user message why.
				lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_SQLite_And_SqlServer_Not_Available_Msg;
				lblErrMsgChooseDbEngine.Attributes["class"] = "gsp_msgwarning";
				pnlDbEngineMsg.Visible = true;
				btnNext.Enabled = false;
			}
		}

		/// <summary>
		/// Using the information gathered from the user, execute the installation.
		/// </summary>
		private void ExecuteInstallation()
		{
			if (rbDataProviderSQLite.Checked)
			{
				ExecuteSQLiteInstallation();
			}
			else
			{
				ExecuteSqlServerInstallation();
			}

			SetFlagForMembershipConfiguration();
		}

		private void ExecuteSQLiteInstallation()
		{
			UpdateWebConfigFile();

			UpdateGalleryServerProConfigFile();
		}

		/// <summary>
		/// We need to set up the sys admin role and user, but we can't do this at the moment because web.config may have
		/// been updated and it won't take effect until the app restarts. We want to use the Membership and Role API to 
		/// configure the membership rather than update the tables directly. So instead we write a small file to the 
		/// App_Data directory that will be noticed at the next application restart. This file will trigger the code 
		/// to create the role and user for us.
		/// </summary>
		private void SetFlagForMembershipConfiguration()
		{
			string filePath = Path.Combine(Request.PhysicalApplicationPath, Path.Combine(GlobalConstants.AppDataDirectory, GlobalConstants.InstallerFileName));

			File.Delete(filePath);

			using (StreamWriter sw = File.CreateText(filePath))
			{
				sw.WriteLine(txtGsAdminUserName.Text);
				sw.WriteLine(GsAdminPassword);
				sw.WriteLine(txtGsAdminEmail.Text);
			}
		}

		private void ExecuteSqlServerInstallation()
		{
			if (chkScriptMembership.Checked)
			{
				ConfigureAspNetMembership();
			}

			ConfigureGalleryServerSchemaForSqlServer();

			ConfigureRuntimeLogin();

			UpdateWebConfigFile();

			UpdateGalleryServerProConfigFile();
		}

		private void ConfigureRuntimeLogin()
		{
			#region Step 1: Ensure login exists in the database

			// Can the account log in to the data server?
			if (!CanConnectToDatabase(GetDbRuntimeConnectionString(true)))
			{
				// Account does not exist or is not valid. Create it.
				ExecuteSql(GetAddSqlUserScript());
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
					ExecuteSql(sql);
				}
				catch
				{
					// An error occurred adding the user to the role. This can happen if the user is the database owner. Let's check
					// for this condition, and if she is, then it's ok that the SQL failed, and we can continue.
					using (SqlConnection cn = new SqlConnection(GetDbRuntimeConnectionString(true)))
					{
						bool isOwner = IsInSqlRole(cn, "db_owner");
						if (!isOwner)
							throw;
					}
				}
			}
			#endregion
		}

		private void UpdateWebConfigFile()
		{
			try
			{
				bool webConfigNeedsUpdating = false;

				WebConfigEntity webConfigEntity = WebConfigController.GetWebConfigEntity();

				if (this.ProviderDb == ProviderDataStore.SqlServer)
				{
					// Update SQL Server connection string. (The SQLite connection string never changes, so we don't have to update it.)
					string cnString = GetDbRuntimeConnectionString(true);
					webConfigNeedsUpdating = (webConfigEntity.SqlServerConnectionStringValue != cnString);
					webConfigEntity.SqlServerConnectionStringValue = cnString;
				}

				if (!AreProvidersSpecifiedInWebConfig())
				{
					// Update membership, role, and profile providers.
					string membershipProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
					string roleProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteRoleProvider" : "SqlRoleProvider");
					string profileProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteProfileProvider" : "SqlProfileProvider");

					webConfigEntity.MembershipDefaultProvider = membershipProviderName;
					webConfigEntity.RoleDefaultProvider = roleProviderName;
					webConfigEntity.ProfileDefaultProvider = profileProviderName;

					webConfigNeedsUpdating = true;
				}

				if (webConfigNeedsUpdating)
				{
					WebConfigController.Save(webConfigEntity);
				}

				_webConfigSuccessfullyUpdated = true;
			}
			catch (Exception ex)
			{
				// Record exception and swallow; we will be able to notice this failed because the _webConfigSuccessfullyUpdated flag remains false.
				try { ErrorHandler.Error.Record(ex); }
				catch { }
			}
		}

		private void UpdateGalleryServerProConfigFile()
		{
			try
			{
				GspCoreEntity coreEntity = GspConfigController.GetGspCoreEntity(Config.GetCore());
				coreEntity.encryptionKey = EncryptionKey;
				GspConfigController.SaveCore(coreEntity);

				GspDataProviderEntity dataProviderEntity = GspConfigController.GetGspDataProviderEntity(Config.GetDataProvider());
				dataProviderEntity.defaultProvider = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteGalleryServerProProvider" : "SqlServerGalleryServerProProvider");
				GspConfigController.SaveDataProvider(dataProviderEntity);

				_gspConfigSuccessfullyUpdated = true;
			}
			catch (Exception ex)
			{
				// Record exception and swallow; we will be able to notice this failed because the _gspConfigSuccessfullyUpdated flag remains false.
				try { ErrorHandler.Error.Record(ex); }
				catch { }
			}
		}

		/// <summary>
		/// Verify that the specified Win/SQL login can used as the runtime account for Gallery Server Pro.	An exception is
		/// thrown if the account cannot be used.</summary>
		private void ValidateRuntimeLogin()
		{
			string sql = String.Empty;

			#region Test 1: Try to connect

			if (CanConnectToDatabase(GetDbRuntimeConnectionString(false)))
				return; // Login account already exists and we're able to log in, so no further testing is needed.

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
					string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Invalid_Pwd_For_Existing_Sql_Login_Msg, txtDbRuntimeUserName.Text);
					throw new WebException(msg);
				}
			}

			#endregion

			#region Test 3: Run the add user script wrapped in a rollback transaction

			// Wrapping the add user script in a rollback transaction fails in SQL Server 2000 and earlier, so
			// only run this test for SQL Server 2005 or later.
			if (sqlVersion > SqlVersion.Sql2000)
			{
				sql = String.Format(CultureInfo.CurrentCulture, "BEGIN TRAN {0} ROLLBACK TRAN", GetAddSqlUserScript());

				ExecuteSql(sql);
			}

			#endregion
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
					errorMsgControl.Attributes["class"] = "gsp_msgwarning gsp_visible";
				}
				else
				{
					errorMsgControl.InnerHtml = String.Empty;
					errorMsgControl.Attributes["class"] = "gsp_invisible";
				}
			}

			#endregion

			#region Show SQL

			if (errorSqlControl != null)
			{
				if (!String.IsNullOrEmpty(sqlThatCausedError))
				{
					errorSqlControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='gsp_bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Sql_Error_Msg, sqlThatCausedError);
					errorSqlControl.Attributes["class"] = "gsp_visible";
				}
				else
				{
					errorSqlControl.InnerHtml = String.Empty;
					errorSqlControl.Attributes["class"] = "gsp_invisible";
				}
			}

			#endregion

			#region Show callstack

			if (errorCallStackControl != null)
			{
				if (!String.IsNullOrEmpty(callStack))
				{
					errorCallStackControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='gsp_bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Sql_Error_CallStack_Label, callStack);
					errorCallStackControl.Attributes["class"] = "gsp_visible";
				}
				else
				{
					errorCallStackControl.InnerHtml = String.Empty;
					errorCallStackControl.Attributes["class"] = "gsp_invisible";
				}
			}

			#endregion

			if (errorMsgControl.ID == lblErrMsgReadyToInstall.ID)
			{
				lblReadyToInstallHeaderMsg.InnerText = Resources.GalleryServerPro.Installer_Install_Error_Hdr;
				lblReadyToInstallDetailMsg.Attributes["class"] = "gsp_invisible";
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
				case SqlVersion.Sql2000: sqlScriptName = (isSqlLogin ? "AddSqlLoginSql2000.sql" : "AddWinLoginSql2000.sql"); break;
				case SqlVersion.Sql2005:
				case SqlVersion.Sql2008:
				case SqlVersion.PostSql2008: sqlScriptName = (isSqlLogin ? "AddSqlLoginSql2005.sql" : "AddWinLoginSql2005.sql"); break;
			}

			return Server.MapPath(Util.GetUrl(String.Concat("/pages/installer/sql/", sqlScriptName)));
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
			string sql;
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

		private void ConfigureAspNetMembership()
		{
			ExecuteSqlInFile(GetSqlPath("InstallCommon.sql"));
			ExecuteSqlInFile(GetSqlPath("InstallMembership.sql"));
			ExecuteSqlInFile(GetSqlPath("InstallProfile.sql"));
			ExecuteSqlInFile(GetSqlPath("InstallRoles.sql"));
		}

		private void ConfigureGalleryServerSchemaForSqlServer()
		{
			string sqlScriptName = String.Empty;

			switch (this.GetSqlVersion())
			{
				case SqlVersion.Sql2000: sqlScriptName = "InstallGalleryServerProSql2000.sql"; break;
				case SqlVersion.Sql2005:
				case SqlVersion.Sql2008:
				case SqlVersion.PostSql2008: sqlScriptName = "InstallGalleryServerProSql2005.sql"; break;
			}

			ExecuteSqlInFile(GetSqlPath(sqlScriptName));

			if (chkUpgradeFromVersion1.Checked)
			{
				ExecuteSqlInFile(GetSqlPath("GS_1.x_to_2_upgrade.sql"));
			}
		}

		/// <summary>
		/// Validate that the selected database can be logged in to by the user-specified credentials and that
		/// it does not already contain Gallery Server tables. If not valid, an exception is thrown.
		/// </summary>
		private void ValidateChooseDb()
		{
			ConnectToDatabaseInAdminMode();

			if (DbContainsGalleryServerTables())
			{
				throw new WebException(Resources.GalleryServerPro.Installer_ChooseDb_Existing_Data_Found_Msg);
			}
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
				if (!InstallerIsAbleToUpdateWebConfig())
				{
					if (rbDataProviderSQLite.Checked)
						lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_WebConfig_Not_UpdateableToSQLite_InsufficientPermission;
					else
						lblErrMsgChooseDbEngine.InnerText = Resources.GalleryServerPro.Installer_DataProvider_WebConfig_Not_UpdateableToSqlServer_InsufficientPermission;

					lblErrMsgChooseDbEngine.Attributes["class"] = "gsp_msgwarning";
					pnlDbEngineMsg.Visible = true;
					return false;
				}
			}

			// galleryserverpro.config: Check to see if the selected provider is specified as the default provider or, if it is not, that the installer
			// has permission to edit the file.
			if (!IsProviderSpecifiedInGalleryServerProConfig())
			{
				if (!InstallerIsAbleToUpdateGalleryServerProConfig())
				{
					lblErrMsgChooseDbEngine.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_DataProvider_GalleryServerProConfig_Not_Updateable_InsufficientPermission, String.Concat(Util.GalleryRoot, "/config/galleryserverpro.config"), this.ProviderDb);

					lblErrMsgChooseDbEngine.Attributes["class"] = "gsp_msgwarning";
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
		private bool AreProvidersSpecifiedInWebConfig()
		{
			WebConfigEntity wce = WebConfigController.GetWebConfigEntity();

			string membershipProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteMembershipProvider" : "SqlMembershipProvider");
			string roleProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteRoleProvider" : "SqlRoleProvider");
			string profileProviderName = (this.ProviderDb == ProviderDataStore.SQLite ? "SQLiteProfileProvider" : "SqlProfileProvider");

			return ((wce.MembershipDefaultProvider.Equals(membershipProviderName, StringComparison.InvariantCultureIgnoreCase))
							&& (wce.RoleDefaultProvider.Equals(roleProviderName, StringComparison.InvariantCultureIgnoreCase))
							&& (wce.ProfileDefaultProvider.Equals(profileProviderName, StringComparison.InvariantCultureIgnoreCase)));
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

			GalleryServerProConfigSettings galleryServerProConfigSection = (GalleryServerProConfigSettings)WebConfigurationManager.GetSection("system.web/galleryServerPro");

			foreach (ProviderSettings provider in galleryServerProConfigSection.DataProvider.Providers)
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

			GalleryServerProConfigSettings galleryServerProConfigSection = (GalleryServerProConfigSettings)WebConfigurationManager.GetSection("system.web/galleryServerPro");

			return galleryServerProConfigSection.DataProvider.DefaultProvider.Equals(dataProviderName);
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

		///// <summary>
		///// Determines whether the membership, role, and profile providers in web.config are configured for the desired data provider.
		///// </summary>
		///// <returns>Returns <c>true</c> if the membership, role, and profile providers in web.config are configured for the data provider
		///// selected by the user; otherwise returns <c>false</c>.</returns>
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
		/// <remarks>This method also exists in upgrade.ascx.</remarks>
		private bool InstallerIsAbleToUpdateWebConfig()
		{
			try
			{
				// Can we open the web.config file for writing? This will fail when the web.config file does not have 'write' NTFS permission.
				const string webConfigFilename = "web.config";
				string webConfigPath = Server.MapPath(Request.ApplicationPath); // C:\inetpub\wwwroot\gs
				string webConfigFullPath = Path.Combine(webConfigPath, webConfigFilename); // C:\inetpub\wwwroot\gs\web.config

				using (File.OpenWrite(webConfigFullPath)) { }

				return true;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
		}

		/// <summary>
		/// Determines whether the installer has permission to update the galleryserverpro.config file.
		/// </summary>
		/// <returns>Returns <c>true</c> if the installer has permission to update the galleryserverpro.config file; otherwise returns <c>false</c>.</returns>
		/// <remarks>This method also exists in upgrade.ascx.</remarks>
		private static bool InstallerIsAbleToUpdateGalleryServerProConfig()
		{
			try
			{
				// Can we open the galleryserverpro.config file for writing? This will fail when the galleryserverpro.config file does not have 'write' NTFS permission.
				using (File.OpenWrite(Util.GalleryServerProConfigFilePath)) { }

				return true;
			}
			catch (UnauthorizedAccessException)
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

		private void ConnectToDatabaseInAdminMode()
		{
			try
			{
				using (SqlConnection cn = new SqlConnection(GetDbAdminConnectionString(true)))
				{
					cn.Open();
					cn.Close();
				}
			}
			catch (SqlException ex)
			{
				string errorMessage;
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
				throw new WebException(errorMessage);
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
		/// Returns true if the specified database contain non-empty Gallery Server related tables. Specifically, it
		/// checks for the existence of a table named gs_Album and requests a count of its records. If there is at least
		/// one record, the function returns true. If it has zero records or the table is not found, it returns false.
		/// </summary>
		/// <returns>Returns true if the database contains a table named gs_Album and it has at least one record; otherwise
		/// returns false.</returns>
		public bool DbContainsGalleryServerTables()
		{
			bool dbContainsGalleryServerTables = false;

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
								dbContainsGalleryServerTables = true;
							}
						}
					}
				}
				catch { /* Swallow exception */ }
			}

			return dbContainsGalleryServerTables;
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

		/// <summary>
		/// Indicates whether the user in the specified <paramref name="cn">connection</paramref> is a member of a
		/// Microsoft Windows group or Microsoft SQL Server database <paramref name="roleName">role</paramref>.
		/// </summary>
		/// <param name="cn">The SQL Server connection.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <returns>
		/// 	<c>true</c> if the user in the connection is a member of the specified role; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsInSqlRole(SqlConnection cn, string roleName)
		{
			cn.Open();

			try
			{
				using (SqlCommand cmd = new SqlCommand(String.Format(CultureInfo.InvariantCulture, "SELECT IS_MEMBER ('{0}')", roleName), cn))
				{
					return Convert.ToBoolean(cmd.ExecuteScalar());
				}
			}
			finally
			{
				cn.Close();
			}
		}

		private void ExecuteSql(string sql)
		{
			using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(sql)))
			{
				ExecuteSqlInStream(memStream);
			}
		}

		private void ExecuteSqlInFile(string pathToSqlFile)
		{
			using (System.IO.Stream stream = System.IO.File.OpenRead(pathToSqlFile))
			{
				ExecuteSqlInStream(stream);
			}

			if ((CurrentWizardPanel == WizardPanel.ReadyToInstall) && (!String.IsNullOrEmpty(pathToSqlFile)))
			{
				lblErrMsgReadyToInstall.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Sql_In_File_Error_Msg, System.IO.Path.GetFileName(pathToSqlFile), lblErrMsgReadyToInstall.InnerText);
			}
		}

		/// <summary>
		/// Execute the SQL statements in the specified stream.
		/// </summary>
		/// <param name="stream">A stream containing a series of SQL statements separated by the word GO.</param>
		/// <returns>Returns true if the SQL executed without error; otherwise returns false.</returns>
		private void ExecuteSqlInStream(System.IO.Stream stream)
		{
			const int timeout = 600; // Timeout for SQL Execution (seconds)
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

							// Replace any replacement parameters with their intended values.
							sb.Replace("{schema}", SqlServerSchema);
							sb.Replace("{objectQualifier}", String.Empty); // {objectQualifier} is not used in GSP; it is present only for the DotNetNuke module

							// Execute T-SQL against the target database
							cmd.CommandText = sb.ToString();
							cmd.CommandTimeout = timeout;

							cmd.ExecuteNonQuery();
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (!ex.Data.Contains(EX_SQL))
				{
					ex.Data.Add(EX_SQL, sb.ToString());
				}
				throw;
			}
			finally
			{
				if (sr != null)
					sr.Close();
			}
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

		/// <summary>
		/// Returns the full path to the specified SQL script. Ex: C:\inetpub\wwwroot\gallery\gs\pages\installer\sql\InstallCommon.sql
		/// </summary>
		/// <param name="sqlScriptName">Name of the SQL script. Ex: InstallCommon.sql</param>
		/// <returns>Returns the full path to the specified SQL script.</returns>
		private static string GetSqlPath(string sqlScriptName)
		{
			const string sqlPath = "/pages/installer/sql/";

			return System.Web.HttpContext.Current.Server.MapPath(Util.GetUrl(String.Concat(sqlPath, sqlScriptName)));
		}

		#endregion
	}
}
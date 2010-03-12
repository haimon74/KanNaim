using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Xml;

namespace GalleryServerPro.Web.installer
{
	#region Enum declarations

	public enum UpgradeWizardPanel
	{
		Welcome,
		ReadyToUpgrade,
		Finished,
	}
	#endregion

	public partial class upgrade : System.Web.UI.Page
	{

		#region Public Properties

		public UpgradeWizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (UpgradeWizardPanel)ViewState["WizardPanel"];

				return UpgradeWizardPanel.Welcome;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			bool setupEnabled;
			if (Boolean.TryParse(ENABLE_SETUP.Value, out setupEnabled) && setupEnabled)
			{
				if (!Page.IsPostBack)
				{
					SetCurrentPanel(UpgradeWizardPanel.Welcome, Welcome);
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
		}

		private void ConfigureControlsFirstTime()
		{
			if (!GetMembershipProvider().Equals("SqlMembershipProvider"))
			{
				Response.Write(String.Format(CultureInfo.CurrentCulture, "<h1>{0}</h1>", Resources.GalleryServerPro.Installer_Upgrade_Not_Needed_Hdr));
				Response.Write(String.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", Resources.GalleryServerPro.Installer_Upgrade_Not_Needed_Bdy));
				Response.Flush();
				Response.End();
			}
			else
			{
				lblDetectedSqlDbVersion.Text = GetGalleryServerDbVersion();
				lblDetectedWebVersion.Text = WebsiteController.GetGalleryServerVersion();

				string version = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Footer_Logo_Tooltip, WebsiteController.GetGalleryServerVersion());
				litVersion.Text = version;
			}
		}

		private string GetMembershipProvider()
		{
			string membershipProvider = String.Empty;

			using (FileStream fs = new FileStream(Server.MapPath("~/web.config"), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader sr = new StreamReader(fs))
				{
					XmlReader r = XmlReader.Create(sr);
					while (r.Read())
					{
						if ((r.Name == "membership") && r.MoveToAttribute("defaultProvider"))
							membershipProvider = r.Value;
					}
				}
			}

			return membershipProvider;
		}

		private static string GetGalleryServerDbVersion()
		{
			if (!IsGalleryServerVersion2Detected())
				return Resources.GalleryServerPro.Installer_Upgrade_GS2_Not_Detected_Msg;

			string version = String.Empty;

			using (SqlConnection cn = new SqlConnection(GetSqlConnectionString()))
			{
				cn.Open();

				try
				{
					using (SqlCommand cmd = new SqlCommand("SELECT [dbo].[gs_GetVersion] ()", cn))
					{
						version = cmd.ExecuteScalar().ToString();
					}
				}
				catch 
				{
					// Version 2.0.2830 did not have the function, so if we get an exception we'll assume we have that version.
					version = "2.0.2830";
				}
			}

			return version;
		}

		private void SetCurrentPanel(UpgradeWizardPanel panel, Control controlToShow)
		{
			Panel currentPanel = Page.Form.FindControl("c1").FindControl(CurrentWizardPanel.ToString()) as Panel;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case UpgradeWizardPanel.Welcome:
					btnPrevious.Enabled = false;
					ReadyToUpgrade.Visible = false;
					break;
				case UpgradeWizardPanel.Finished:
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
				case UpgradeWizardPanel.Welcome:
					{
						lblSelectedSqlScript.Text = ddlSqlUpgradeScripts.SelectedItem.Text;
						SetCurrentPanel(UpgradeWizardPanel.ReadyToUpgrade, ReadyToUpgrade);
						break;
					}
				case UpgradeWizardPanel.ReadyToUpgrade:
					{
						if (ExecuteSqlScript(ddlSqlUpgradeScripts.SelectedItem.Text))
						{
							SetCurrentPanel(UpgradeWizardPanel.Finished, Finished);
						}
						break;
					}
			}
		}

		private void ShowPreviousPanel()
		{
			switch (this.CurrentWizardPanel)
			{
				case UpgradeWizardPanel.Welcome: break;
				case UpgradeWizardPanel.ReadyToUpgrade:
					{
						SetCurrentPanel(UpgradeWizardPanel.Welcome, Welcome);
						break;
					}
			}
		}

		private bool ExecuteSqlScript(string sqlScriptName)
		{
			return ExecuteSqlInFile(Server.MapPath(String.Concat("sql/", sqlScriptName)));
		}

		private bool ExecuteSqlInFile(string pathToSqlFile)
		{
			bool executeSuccess = false;
			using (System.IO.Stream stream = System.IO.File.OpenRead(pathToSqlFile))
			{
				executeSuccess = ExecuteSqlInStream(stream);
			}

			if ((CurrentWizardPanel == UpgradeWizardPanel.ReadyToUpgrade) && (!String.IsNullOrEmpty(pathToSqlFile)))
			{
				lblErrMsgReadyToUpgrade.InnerText = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Installer_Sql_In_File_Error_Msg, System.IO.Path.GetFileName(pathToSqlFile), lblErrMsgReadyToUpgrade.InnerText);
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
			const int timeout = 600; // Timeout for SQL Execution (seconds)
			System.IO.StreamReader sr = null;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			try
			{
				sr = new System.IO.StreamReader(stream);
				using (SqlConnection cn = new SqlConnection(GetSqlConnectionString()))
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
					case UpgradeWizardPanel.ReadyToUpgrade:
						ShowErrorMsgThatOccurredDuringUpgrade(ex.Message, sb.ToString(), ex.StackTrace, lblErrMsgReadyToUpgrade, lblErrMsgReadyToUpgradeSql, lblErrMsgReadyToUpgradeCallStack);
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

		private void ShowErrorMsgThatOccurredDuringUpgrade(string errorMsg, string sqlThatCausedError, string callStack,
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
					errorSqlControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Upgrade_Sql_Error_Msg, sqlThatCausedError);
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
					errorCallStackControl.InnerHtml = String.Format(CultureInfo.CurrentCulture, "<span class='bold'>{0}</span> {1}", Resources.GalleryServerPro.Installer_Upgrade_Sql_Error_CallStack_Label, callStack);
					errorCallStackControl.Attributes["class"] = "visible";
				}
				else
				{
					errorCallStackControl.InnerHtml = String.Empty;
					errorCallStackControl.Attributes["class"] = "invisible";
				}
			}

			#endregion

			if (errorMsgControl.ID == lblErrMsgReadyToUpgrade.ID)
			{
				lblReadyToUpgradeHeaderMsg.InnerText = Resources.GalleryServerPro.Installer_Upgrade_Sql_Error_Hdr;
				lblReadyToUpgradeDetailMsg.Attributes["class"] = "invisible";
				lblReadyToUpgradePermWarningMsg.Attributes["class"] = "invisible";
			}
		}

		/// <summary>
		/// Determine if the current database has Gallery Server Pro 2.0 tables. This is accomplished by executing the
		/// stored proc gs_SelectRootAlbum and seeing if the returned value is greater or equal to zero.
		/// </summary>
		/// <returns>Returns true if the current database, as defined in the web.config connection string, contain Galley Server
		/// Pro 2.0 tables; otherwise returns false.</returns>
		/// <remarks>The technique to determine if 2.0 tables exist works even if the galleryId specified in galleryserverpro.config
		/// has just been changed and does not currently have any associated records in gs_Album. This is because the stored proc
		/// automatically executes the stored pro gs_VerifyMinimumRecords if it doesn't detect any records in gs_Album for the
		/// specified gallery ID.</remarks>
		private static bool IsGalleryServerVersion2Detected()
		{
			bool galleryServer2Detected = false;

			using (SqlConnection cn = new SqlConnection(GetSqlConnectionString()))
			{
				cn.Open();

				try
				{
					using (SqlCommand cmd = new SqlCommand("dbo.gs_SelectRootAlbum", cn))
					{
						cmd.CommandType = System.Data.CommandType.StoredProcedure;
						cmd.Parameters.Add(new SqlParameter("@GalleryId", System.Data.SqlDbType.Int));
						cmd.Parameters["@GalleryId"].Value = WebsiteController.GetGalleryServerProConfigSection().Core.GalleryId;

						using (System.Data.IDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
							if ((dr != null) && dr.Read() && (dr.GetInt32(0) >= 0))
							{
								galleryServer2Detected = true;
							}
						}
					}
				}
				catch { /* Swallow exception */ }
			}

			return galleryServer2Detected;
		}

		private static string GetSqlConnectionString()
		{
			return System.Configuration.ConfigurationManager.ConnectionStrings["SqlServerDbConnection"].ConnectionString;
		}


		#endregion

	}
}

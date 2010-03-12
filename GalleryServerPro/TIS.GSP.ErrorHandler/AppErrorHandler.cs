using System;
using System.Net.Mail;
using System.Web;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

using GalleryServerPro.Configuration;
using System.Globalization;

namespace GalleryServerPro.ErrorHandler
{
	/// <summary>
	/// Contains error handling functionality for Gallery Server Pro.
	/// </summary>
	public sealed class AppErrorHandler
	{
		private AppErrorHandler() { }

		/// <summary>
		/// Email detailed information about the error to the specified administrator.
		/// </summary>
		/// <param name="ex">The exception that caused the error.</param>
		/// <param name="eventLogException">This exception that occurs, if any, when the error is written to the event log.</param>
		static private void SendErrorReport(Exception ex, Exception eventLogException)
		{
			string errorInfoHtml = "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\"><body>";

			if (eventLogException != null)
				errorInfoHtml += "<p>NOTE: An attempt was made to store this error information in the Application "
					+ "Event Log of the server, but an error occurred. Scroll to the end of this email for more "
					+ "information.";

			errorInfoHtml += getErrorInfoHtml(ex);

			if (eventLogException != null)
			{
				string msg = "<p>NOTE: As mentioned at the beginning, an error occurred while trying to store "
					+ "this error information to the application event log of the server. This often happens when "
					+ "using a hosting service that restricts access to the log. This can also happen if the "
					+ "'Gallery Server Pro' event source was not created during the installation of Gallery Server. "
					+ "If you have access to the web server's registry, you can manually create the event source:</p>"
					+ "<p>1. Click Start, and then click Run.</p>"
					+ "<p>2. In the Open text box, type regedit.</p>"
					+ "<p>3. Locate the following registry subkey: HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Eventlog\\Application</p>"
					+ "<p>4. Right-click the Application subkey, point to New, and then click Key.</p>"
					+ "<p>5. Type Gallery Server Pro for the key name.</p>"
					+ "<p>6. Close Registry Editor.</p>"
					+ "<p style='font-weight:bold;'>Detailed exception information:</p><p>" + eventLogException.ToString() + "</p>";
				errorInfoHtml += msg;
				errorInfoHtml += "</body></html>";
			}
			Core coreConfig = ConfigManager.GetGalleryServerProConfigSection().Core;

			MailAddress emailRecipient = new MailAddress(coreConfig.EmailToAddress, coreConfig.EmailToName);
			MailAddress emailSender = new MailAddress(coreConfig.EmailFromAddress, coreConfig.EmailFromName);

			string exceptionType = String.Empty;
			if (ex.GetType() != null)
				exceptionType = ex.GetType().ToString();

			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(emailSender, emailRecipient);

			mail.Subject = (String.IsNullOrEmpty(exceptionType) ? "Error Report" : String.Format(CultureInfo.CurrentCulture, "Error: {0}", exceptionType));
			mail.Body = errorInfoHtml;
			mail.IsBodyHtml = true;

			SmtpClient smtpClient = new SmtpClient();

			// Specify SMTP server if it's in galleryserverpro.config. The server might have been assigned via web.config,
			// so only update this if we have a config setting.
			if (!String.IsNullOrEmpty(coreConfig.SmtpServer))
			{
				smtpClient.Host = coreConfig.SmtpServer;
			}

			// Specify port number if it's in galleryserverpro.config and it's not the default value of 25. The port 
			// might have been assigned via web.config, so only update this if we have a config setting.
			int smtpServerPort;
			if (!Int32.TryParse(coreConfig.SmtpServerPort, out smtpServerPort))
				smtpServerPort = int.MinValue;

			if ((smtpServerPort > 0) && (smtpServerPort != 25))
			{
				smtpClient.Port = smtpServerPort;
			}

			try
			{
				smtpClient.Send(mail);
			}
			finally
			{
				mail.Dispose();
			}
		}

		static private string getErrorInfoPlainText(Exception ex)
		{
			// Return plain formatted error message.
			System.Web.HttpContext context = System.Web.HttpContext.Current;

			StringBuilder sb = new StringBuilder(10000);
			sb.Append("An exception has occurred in the application.\r\n\r\n");

			// Extract exception information and append to string builder.
			string exceptionType = String.Empty;
			string exceptionMessage = String.Empty;
			string exceptionSource = String.Empty;
			string exceptionTargetSite = String.Empty;
			string exceptionStackTrace = String.Empty;

			if (ex != null)
			{
				exceptionType = (ex.GetType() == null) ? "<unknown>" : ex.GetType().ToString();
				exceptionMessage = (ex.Message == null) ? "<unknown>" : ex.Message;
				exceptionSource = (ex.Source == null) ? "<unknown>" : ex.Source;
				exceptionTargetSite = (ex.TargetSite == null) ? "<unknown>" : ex.TargetSite.ToString();
				exceptionStackTrace = (ex.StackTrace == null) ? "<unknown>" : ex.StackTrace;
			}

			sb.Append("Error Information:\r\nType: " + exceptionType);
			sb.Append("\r\nMessage: " + exceptionMessage);
			sb.Append("\r\nSource: " + exceptionSource);
			sb.Append("\r\nTargetSite: " + exceptionTargetSite);
			sb.Append("\r\nStackTrace: " + exceptionStackTrace);

			// If the Http Context was passed, then get info from that as well.
			if (context != null)
			{
				if (HttpContext.Current.Request.QueryString != null)
				{
					// QueryString Collection
					sb.Append("\r\n\r\nQueryString Collection:\r\n");
					sb.Append(CollectionToPlainTextTable(HttpContext.Current.Request.QueryString));
				}

				if (HttpContext.Current.Request.Form != null)
				{
					// Form Collection
					sb.Append("\r\n\r\nForm Collection:\r\n");
					sb.Append(CollectionToPlainTextTable(HttpContext.Current.Request.Form));
				}

				if (HttpContext.Current.Request.Cookies != null)
				{
					// Cookies Collection
					sb.Append("\r\n\r\nCookies Collection:\r\n");
					sb.Append(CollectionToPlainTextTable(HttpContext.Current.Request.Cookies));
				}

				if (HttpContext.Current.Session != null)
				{
					// Session Variables
					sb.Append("\r\n\r\nSession Variables:\r\n");
					sb.Append(CollectionToPlainTextTable(HttpContext.Current.Session));
				}

				if (HttpContext.Current.Request.ServerVariables != null)
				{
					// Server Variables
					sb.Append("\r\n\r\nServer Variables:\r\n");
					sb.Append(CollectionToPlainTextTable(HttpContext.Current.Request.ServerVariables));
				}
			}


			sb.Append("\r\n");

			return sb.ToString();
		}
		static private string getErrorInfoHtml(Exception ex)
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;

			// Returns an HTML formatted error message.
			StringBuilder sb = new StringBuilder(25000);

			// Heading Template
			string heading = "<TABLE BORDER=\"0\" WIDTH=\"100%\" CELLPADDING=\"1\" CELLSPACING=\"0\"><TR><TD bgcolor=\"black\" COLSPAN=\"2\"><FONT face=\"Arial\" color=\"white\"><B> <!--HEADER--></B></FONT></TD></TR></TABLE>";
			// Error Message Header
			sb.Append("<FONT face=\"Arial\" size=\"5\" color=\"red\">Error - " + ex.Message + "</FONT><BR><BR>");

			// Extract exception information and append to string builder
			string exceptionType = String.Empty;
			string exceptionMessage = String.Empty;
			string exceptionSource = String.Empty;
			string exceptionTargetSite = String.Empty;
			string exceptionStackTrace = String.Empty;

			if (ex != null)
			{
				exceptionType = (ex.GetType() == null) ? "<unknown>" : ex.GetType().ToString();
				exceptionMessage = (ex.Message == null) ? "<unknown>" : cleanHTML(ex.Message);
				exceptionSource = (ex.Source == null) ? "<unknown>" : cleanHTML(ex.Source);
				exceptionTargetSite = (ex.TargetSite == null) ? "<unknown>" : cleanHTML(ex.TargetSite.ToString());
				exceptionStackTrace = (ex.StackTrace == null) ? "<unknown>" : cleanHTML(ex.StackTrace);
			}

			// Populate Error Information Collection
			NameValueCollection error_info = new NameValueCollection();
			error_info.Add("Type", exceptionType);
			error_info.Add("Message", exceptionMessage);
			error_info.Add("Source", exceptionSource);
			error_info.Add("TargetSite", exceptionTargetSite);
			error_info.Add("StackTrace", exceptionStackTrace);

			// Error Information
			sb.Append(heading.Replace("<!--HEADER-->", "Error Information"));
			sb.Append(CollectionToHtmlTable(error_info));

			// If the Http Context was passed, then get info from that as well.
			if (context != null)
			{
				if (context.Request.QueryString != null)
				{
					// QueryString Collection
					sb.Append("<BR><BR>" + heading.Replace("<!--HEADER-->", "QueryString Collection"));
					sb.Append(CollectionToHtmlTable(context.Request.QueryString));
				}

				if (context.Request.Form != null)
				{
					// Form Collection
					sb.Append("<BR><BR>" + heading.Replace("<!--HEADER-->", "Form Collection"));
					sb.Append(CollectionToHtmlTable(context.Request.Form));
				}

				if (context.Request.Cookies != null)
				{
					// Cookies Collection
					sb.Append("<BR><BR>" + heading.Replace("<!--HEADER-->", "Cookies Collection"));
					sb.Append(CollectionToHtmlTable(context.Request.Cookies));
				}

				if (context.Session != null)
				{
					// Session Variables
					sb.Append("<BR><BR>" + heading.Replace("<!--HEADER-->", "Session Variables"));
					sb.Append(CollectionToHtmlTable(context.Session));
				}

				if (context.Request.ServerVariables != null)
				{
					// Server Variables
					sb.Append("<BR><BR>" + heading.Replace("<!--HEADER-->", "Server Variables"));
					sb.Append(CollectionToHtmlTable(context.Request.ServerVariables));
				}
			}
			return sb.ToString();
		}
		static private string CollectionToHtmlTable(NameValueCollection collection)
		{
			// <TD>...</TD> Template
			const string TD = "<TD><FONT face=\"Arial\" size=\"2\"><!--VALUE--></FONT></TD>";
			// Table Header
			string html = "\n<TABLE width=\"100%\">\n"
				+ " <TR bgcolor=\"#C0C0C0\">" + TD.Replace("<!--VALUE-->", " <B>Name</B>")
				+ " " + TD.Replace("<!--VALUE-->", " <B>Value</B>") + "</TR>\n";
			// No Body? -> N/A
			if (collection.Count == 0)
			{
				collection = new NameValueCollection();
				collection.Add("N/A", "");
			}
			// Table Body
			for (int i = 0; i < collection.Count; i++)
			{
				html += "<TR valign=\"top\" bgcolor=\"" + ((i % 2 == 0) ? "white" : "#EEEEEE") + "\">"
					+ TD.Replace("<!--VALUE-->", collection.Keys[i]) + "\n"
					+ TD.Replace("<!--VALUE-->", collection[i]) + "</TR>\n";
			}
			// Table Footer
			return html + "</TABLE>";
		}
		static private string CollectionToHtmlTable(HttpCookieCollection collection)
		{
			// Overload for HttpCookieCollection collection.
			// Converts HttpCookieCollection to NameValueCollection
			NameValueCollection NVC = new NameValueCollection();
			foreach (string item in collection) NVC.Add(item, collection[item].Value);
			return CollectionToHtmlTable(NVC);
		}
		static private string CollectionToHtmlTable(System.Web.SessionState.HttpSessionState collection)
		{
			// Overload for HttpSessionState collection.
			// Converts HttpSessionState to NameValueCollection
			NameValueCollection NVC = new NameValueCollection();
			foreach (string item in collection) NVC.Add(item, collection[item].ToString());
			return CollectionToHtmlTable(NVC);
		}
		static private string CollectionToPlainTextTable(NameValueCollection collection)
		{
			StringBuilder sb = new StringBuilder();

			if (collection.Count == 0)
			{
				collection = new NameValueCollection();
				collection.Add("N/A", "");
			}
			for (int i = 0; i < collection.Count; i++)
			{
				sb.Append(collection.Keys[i] + ": " + collection[i] + "\r\n");
			}
			return sb.ToString();
		}

		static private string CollectionToPlainTextTable(HttpCookieCollection collection)
		{
			// Overload for HttpCookieCollection collection.
			// Converts HttpCookieCollection to NameValueCollection
			NameValueCollection NVC = new NameValueCollection();
			foreach (string item in collection) NVC.Add(item, collection[item].Value);
			return CollectionToPlainTextTable(NVC);
		}
		static private string CollectionToPlainTextTable(System.Web.SessionState.HttpSessionState collection)
		{
			// Overload for HttpSessionState collection.
			// Converts HttpSessionState to NameValueCollection
			NameValueCollection NVC = new NameValueCollection();
			foreach (string item in collection) NVC.Add(item, collection[item].ToString());
			return CollectionToPlainTextTable(NVC);
		}
		static private string cleanHTML(string Html)
		{
			// Cleans the string for HTML friendly display
			return (Html.Length == 0) ? "" : Html.Replace("<", "<").Replace("\r\n", "<BR>").Replace("&", "&").Replace(" ", " ");
		}

		/// <summary>
		/// If email reporting has been turned on, send detailed error report to the specified administrator via e-mail.
		/// </summary>
		/// <param name="ex">The exception to be recorded.</param>
		static public void RecordErrorInfo(Exception ex)
		{
			storeErrorInfo(ex);
		}

		static private void storeErrorInfo(Exception ex)
		{
			//If email reporting has been turned on, send detailed error report.
			Core coreConfig = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().Core;
			bool sendEmailOnError = coreConfig.SendEmailOnError;
			if (sendEmailOnError)
			{
				SendErrorReport(ex, null);
			}
		}
	}
}

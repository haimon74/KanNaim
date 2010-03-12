using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Xml.Serialization;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.Provider;
using GalleryServerPro.ErrorHandler.Properties;

namespace GalleryServerPro.ErrorHandler
{
	/// <summary>
	/// Contains error handling functionality for Gallery Server Pro.
	/// </summary>
	public static class Error
	{
		#region Public Methods

		/// <summary>
		/// Gets a collection of all application errors from the data store. The items are sorted in descending order on the
		/// <see cref="IAppError.TimeStamp"/> property, so the most recent error is first. Returns an empty collection if no
		/// errors exist.
		/// </summary>
		/// <returns>Returns a collection of all application errors from the data store.</returns>
		public static IAppErrorCollection GetAppErrors()
		{
			IAppErrorCollection appErrors = null;

			IDataReader dr = null;
			try
			{
				using (dr = DataProviderManager.Provider.AppError_GetAppErrors())
				{
					// Create the roles.
					appErrors = GetAppErrorsFromDataReader(dr);
				}
			}
			finally
			{
				if (dr != null) dr.Close();
			}

			appErrors.Sort();

			return appErrors;
		}

		/// <summary>
		/// Persist information about the specified <paramref name="ex">exception</paramref> to the data store and return 
		/// the ID that is assigned to it.
		/// </summary>
		/// <param name="ex">The exception to be recorded to the data store.</param>
		/// <returns>Returns an integer that uniquely identifies this application error (<see cref="IAppError.AppErrorId"/>).</returns>
		public static int Record(Exception ex)
		{
			IAppError appError = new AppError(ex);

			SendEmail(appError);

			int appErrorId = DataProviderManager.Provider.AppError_Save(appError);

			ValidateLogSize();

			return appErrorId;
		}

		/// <summary>
		/// Permanently remove the specified error from the data store.
		/// </summary>
		/// <param name="appErrorId">The value that uniquely identifies this application error (<see cref="IAppError.AppErrorId"/>).</param>
		public static void Delete(int appErrorId)
		{
			DataProviderManager.Provider.AppError_Delete(appErrorId);
		}

		/// <summary>
		/// Permanently delete all errors from the data store.
		/// </summary>
		public static void ClearErrorLog()
		{
			DataProviderManager.Provider.AppError_ClearLog();
		}

		/// <summary>
		/// Serializes the specified collection into an XML string. The data can be converted back into a collection using
		/// the <see cref="Deserialize"/> method.
		/// </summary>
		/// <param name="list">The collection to serialize to XML.</param>
		/// <returns>Returns an XML string.</returns>
		public static string Serialize(ICollection<KeyValuePair<string, string>> list)
		{
			if ((list == null) || (list.Count == 0))
				return String.Empty;

			DataTable dt = new DataTable("Collection");
			dt.Columns.Add("key");
			dt.Columns.Add("value");

			foreach (KeyValuePair<string, string> pair in list)
			{
				DataRow dr = dt.NewRow();
				dr[0] = pair.Key;
				dr[1] = pair.Value;
				dt.Rows.Add(dr);
			}

			XmlSerializer ser = new XmlSerializer(typeof(DataTable));
			using (StringWriter writer = new StringWriter())
			{
				ser.Serialize(writer, dt);

				return writer.ToString();
			}
		}

		/// <summary>
		/// Deserializes <paramref name="xmlToDeserialize"/> into a collection. This method assumes the XML was serialized 
		/// using the <see cref="Serialize"/> method.
		/// </summary>
		/// <param name="xmlToDeserialize">The XML to deserialize.</param>
		/// <returns>Returns a collection.</returns>
		public static List<KeyValuePair<string, string>> Deserialize(string xmlToDeserialize)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

			if (String.IsNullOrEmpty(xmlToDeserialize))
				return list;

			DataTable dt = new DataTable("Collection");
			dt.ReadXml(new StringReader(xmlToDeserialize));

			foreach (DataRow row in dt.Rows)
			{
				list.Add(new KeyValuePair<string, string>(row[0].ToString(), row[1].ToString()));
			}

			return list;
		}

		/// <summary>
		/// If automatic log size trimming is enabled and the log contains more items than the specified limit, remove error records.
		/// </summary>
		public static void ValidateLogSize()
		{
			int maxNumItems = ConfigManager.GetGalleryServerProConfigSection().Core.MaxNumberErrorItems;

			if (maxNumItems == 0)
				return; // Auto trimming is disabled, so just return.

			IAppErrorCollection errors = GetAppErrors();
			int numErrors = errors.Count;

			while (numErrors > maxNumItems)
			{
				// Find oldest error and delete it.
				Delete(errors[numErrors - 1].AppErrorId);

				numErrors--;
			}
		}

		/// <summary>
		/// Gets a human readable text representation for the specified <paramref name="enumItem"/>. The text is returned from the resource
		/// file. Example: If <paramref name="enumItem"/> = ErrorItem.StackTrace, the text "Stack Trace" is used.
		/// </summary>
		/// <param name="enumItem">The enum value for which to get human readable text.</param>
		/// <returns>Returns human readable text representation for the specified <paramref name="enumItem"/></returns>
		internal static string GetFriendlyEnum(ErrorItem enumItem)
		{
			switch (enumItem)
			{
				case ErrorItem.AppErrorId: return Resources.Err_AppErrorId_Lbl;
				case ErrorItem.Url: return Resources.Err_Url_Lbl;
				case ErrorItem.Timestamp: return Resources.Err_Timestamp_Lbl;
				case ErrorItem.ExceptionType: return Resources.Err_ExceptionType_Lbl;
				case ErrorItem.Message: return Resources.Err_Message_Lbl;
				case ErrorItem.Source: return Resources.Err_Source_Lbl;
				case ErrorItem.TargetSite: return Resources.Err_TargetSite_Lbl;
				case ErrorItem.StackTrace: return Resources.Err_StackTrace_Lbl;
				case ErrorItem.ExceptionData: return Resources.Err_ExceptionData_Lbl;
				case ErrorItem.InnerExType: return Resources.Err_InnerExType_Lbl;
				case ErrorItem.InnerExMessage: return Resources.Err_InnerExMessage_Lbl;
				case ErrorItem.InnerExSource: return Resources.Err_InnerExSource_Lbl;
				case ErrorItem.InnerExTargetSite: return Resources.Err_InnerExTargetSite_Lbl;
				case ErrorItem.InnerExStackTrace: return Resources.Err_InnerExStackTrace_Lbl;
				case ErrorItem.InnerExData: return Resources.Err_InnerExData_Lbl;
				case ErrorItem.GalleryId: return Resources.Err_GalleryId_Lbl;
				case ErrorItem.HttpUserAgent: return Resources.Err_HttpUserAgent_Lbl;
				case ErrorItem.FormVariables: return Resources.Err_FormVariables_Lbl;
				case ErrorItem.Cookies: return Resources.Err_Cookies_Lbl;
				case ErrorItem.SessionVariables: return Resources.Err_SessionVariables_Lbl;
				case ErrorItem.ServerVariables: return Resources.Err_ServerVariables_Lbl;
				default: throw new Exception(String.Format("Encountered unexpected ErrorItem enum value {0}. Error.GetFriendlyEnum is not designed to handle this enum value. The function must be updated.", enumItem));
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the app errors from data reader. Returns an empty collection if no errors are in the data reader.
		/// </summary>
		/// <param name="dr">The data reader containing the error records.</param>
		/// <returns>Returns an IAppErrorCollection.</returns>
		private static IAppErrorCollection GetAppErrorsFromDataReader(IDataReader dr)
		{
			IAppErrorCollection appErrors = new AppErrorCollection();

			//SELECT
			//  AppErrorId, FKGalleryId, [TimeStamp], ExceptionType, Message, Source, TargetSite, StackTrace, ExceptionData, 
			//  InnerExType, InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
			//  FormVariables, Cookies, SessionVariables, ServerVariables
			//FROM gs_AppError
			//WHERE FKGalleryId = @GalleryId
			while (dr.Read())
			{
				appErrors.Add(new AppError(Int32.Parse(dr["AppErrorId"].ToString(), CultureInfo.InvariantCulture),
				                           Int32.Parse(dr["FKGalleryId"].ToString(), CultureInfo.InvariantCulture),
				                           ToDateTime(dr["TimeStamp"]),
				                           dr["ExceptionType"].ToString(),
				                           dr["Message"].ToString(),
				                           dr["Source"].ToString(),
				                           dr["TargetSite"].ToString(),
				                           dr["StackTrace"].ToString(),
				                           Deserialize(dr["ExceptionData"].ToString()),
				                           dr["InnerExType"].ToString(),
				                           dr["InnerExMessage"].ToString(),
				                           dr["InnerExSource"].ToString(),
				                           dr["InnerExTargetSite"].ToString(),
				                           dr["InnerExStackTrace"].ToString(),
				                           Deserialize(dr["InnerExData"].ToString()),
				                           dr["Url"].ToString(),
				                           Deserialize(dr["FormVariables"].ToString()),
				                           Deserialize(dr["Cookies"].ToString()),
				                           Deserialize(dr["SessionVariables"].ToString()),
				                           Deserialize(dr["ServerVariables"].ToString())));
			}

			return appErrors;
		}

		/// <summary>
		/// Convert the specified object to System.DateTime. Use this object when retrieving
		/// values from a database. If the object is of type System.TypeCode.DBNull,
		/// DateTime.MinValue is returned.
		/// </summary>
		/// <param name="value">The object to convert to System.DateTime. An exception is thrown
		/// if the object cannot be converted.</param>
		/// <returns>Returns a System.DateTime value.</returns>
		private static DateTime ToDateTime(object value)
		{
			return Convert.IsDBNull(value) ? DateTime.MinValue : Convert.ToDateTime(value, NumberFormatInfo.CurrentInfo);
		}

		private static void SendEmail(IAppError appError)
		{
			//If email reporting has been turned on, send detailed error report.
			Core coreConfig = ConfigManager.GetGalleryServerProConfigSection().Core;

			if (!coreConfig.SendEmailOnError)
				return;

			MailAddress emailRecipient = new MailAddress(coreConfig.EmailToAddress, coreConfig.EmailToName);
			MailAddress emailSender = new MailAddress(coreConfig.EmailFromAddress, coreConfig.EmailFromName);
			try
			{
				using (MailMessage mail = new MailMessage(emailSender, emailRecipient))
				{
					if (String.IsNullOrEmpty(appError.ExceptionType)) 
						mail.Subject = Resources.Email_Subject_When_No_Ex_Type_Present;
					else
						mail.Subject = String.Concat(Resources.Email_Subject_Prefix_When_Ex_Type_Present, " ", appError.ExceptionType);

					mail.Body = appError.ToHtmlPage();
					mail.IsBodyHtml = true;

					SmtpClient smtpClient = new SmtpClient();
					smtpClient.EnableSsl = coreConfig.SendEmailUsingSsl;

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

					smtpClient.Send(mail);
				}
			}
			catch (Exception ex2)
			{
				string errorMsg = String.Concat(ex2.GetType(), ": ", ex2.Message);

				if (ex2.InnerException != null)
					errorMsg += String.Concat(" ", ex2.InnerException.GetType(), ": ", ex2.InnerException.Message);

				appError.ExceptionData.Add(new KeyValuePair<string, string>(Resources.Cannot_Send_Email_Lbl, errorMsg));
			}
		}

		#endregion

	}
}

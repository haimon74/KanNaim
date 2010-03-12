using System;
using System.Data;
using System.Web;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using GalleryServerPro.ErrorHandler;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality for interacting with the error handling layer. Objects in the web layer should use these
	/// methods rather than directly invoking the objects in the error handling layer.
	/// </summary>
	public static class AppErrorController
	{
		#region Public Methods

		/// <summary>
		/// Gets a DataSet containing all application errors. It consists of two DataTables: AppErrors that contains summary
		/// information about each error, and AppErrorItems that contains all information about each error. The DataSet
		/// is designed to easily bind to a hierarchical ComponentArtGrid control.
		/// </summary>
		/// <returns>Returns a DataSet containing all application errors.</returns>
		public static DataSet GetAppErrorsDataSet()
		{
			DataTable appErrors = new DataTable("AppErrors");
			appErrors.Columns.Add(new DataColumn("AppErrorId", typeof (Int32)));
			appErrors.Columns.Add(new DataColumn("TimeStamp", typeof(DateTime)));
			appErrors.Columns.Add(new DataColumn("ExceptionType", typeof(string)));
			appErrors.Columns.Add(new DataColumn("Message", typeof(string)));

			DataTable appErrorItems = new DataTable("AppErrorItems");
			appErrorItems.Columns.Add(new DataColumn("FKAppErrorId", typeof(Int32)));
			appErrorItems.Columns.Add(new DataColumn("Name", typeof(string)));
			appErrorItems.Columns.Add(new DataColumn("Value", typeof(string)));

			foreach (IAppError err in Factory.GetAppErrors())
			{
				DataRow errRow = appErrors.NewRow();
				errRow[0] = err.AppErrorId;
				errRow[1] = err.TimeStamp.ToString();
				errRow[2] = err.ToHtmlValue(ErrorItem.ExceptionType);
				errRow[3] = err.ToHtmlValue(ErrorItem.Message);
				appErrors.Rows.Add(errRow);

				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.Url));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.Timestamp));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.ExceptionType));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.Message));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.Source));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.TargetSite));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.StackTrace));

				if (err.ExceptionData.Count > 0)
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.ExceptionData));

				if (!String.IsNullOrEmpty(err.InnerExType))
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.InnerExType));

				if (!String.IsNullOrEmpty(err.InnerExMessage))
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.InnerExMessage));

				if (!String.IsNullOrEmpty(err.InnerExSource))
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.InnerExSource));

				if (!String.IsNullOrEmpty(err.InnerExTargetSite))
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.InnerExTargetSite));

				if (!String.IsNullOrEmpty(err.InnerExStackTrace))
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.InnerExStackTrace));

				if (err.InnerExData.Count > 0)
					appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.InnerExData));

				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.AppErrorId));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.GalleryId));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.HttpUserAgent));

				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.FormVariables));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.Cookies));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.SessionVariables));
				appErrorItems.Rows.Add(AddDataRow(appErrorItems.NewRow(), err, ErrorItem.ServerVariables));
			}

			DataSet ds = new DataSet();
			ds.Tables.Add(appErrors);
			ds.Tables.Add(appErrorItems);

			ds.Relations.Add(ds.Tables["AppErrors"].Columns["AppErrorId"], ds.Tables["AppErrorItems"].Columns["FKAppErrorId"]);

			return ds;
		}

		/// <summary>
		/// Record the error and optionally notify an administrator via e-mail.
		/// </summary>
		/// <param name="ex">The exception to record.</param>
		/// <returns>Returns an integer that uniquely identifies this application error (<see cref="IAppError.AppErrorId"/>).</returns>
		public static int LogError(Exception ex)
		{
			int errorId = Error.Record(ex);

			HelperFunctions.PurgeCache();

			return errorId;
		}

		#endregion

		#region Private Methods

		private static DataRow AddDataRow(DataRow dr, IAppError err, ErrorItem item)
		{
			dr[0] = err.AppErrorId;
			dr[1] = err.ToHtmlName(item);
			dr[2] = err.ToHtmlValue(item);

			return dr;
		}

		#endregion

		/// <summary>
		/// Handles an exception that occurs. First, the error is recorded. Certain types, such as security exceptions and directory permission
		/// errors, are are rendered to the user with user-friendly text. For other exceptions, a generic message is displayed, unless
		/// the system is configured to show detailed error messages (showErrorDetails="true" in galleryserverpro.config), in which
		/// case full details about the exception is displayed. If the user has disabled the exception handler 
		/// (enableExceptionHandler="false"), then the error is recorded but no other action is taken. This allows
		/// global error handling in web.config or global.asax to deal with it.
		/// </summary>
		/// <param name="ex">The exception.</param>
		public static void HandleGalleryException(Exception ex)
		{
			try
			{
				LogError(ex);
			}
			catch (Exception errHandlingEx)
			{
				if (!ex.Data.Contains("Error Handling Exception"))
				{
					ex.Data.Add("Error Handling Exception", String.Format("The following error occurred while handling the exception: {0} - {1} Stack trace: {2}", errHandlingEx.GetType(), errHandlingEx.Message, errHandlingEx.StackTrace));
				}
			}

			// If the error is security related, go to a special page that offers a friendly error message.
			if (ex is ErrorHandler.CustomExceptions.GallerySecurityException)
			{
				// User is not allowed to access the requested page. Redirect to home page.
				HttpContext.Current.Server.ClearError();
				Util.Redirect(PageId.album);
			}
			else if (ex is ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
			{
				// Gallery Server cannot write to a directory. Application startup code checks for this condition,
				// so we'll get here most often when Gallery Server is first configured and the required permissions were not given.
				// Provide friendly, customized message to help the user resolve the situation.
				HttpContext.Current.Server.ClearError();
				HttpContext.Current.Items["CurrentException"] = ex;
				Util.Transfer(PageId.error_cannotwritetodirectory);
			}
			else
			{
				// An unexpected exception is happening.
				// If Gallery Server's exception handling is enabled, clear the error and display the relevant error message.
				// Otherwise, don't do anything, which lets it propogate up the stack, thus allowing for error handling code in
				// global.asax and/or web.config (e.g. <customErrors...> or some other global error handler) to handle it.
				if (ConfigManager.GetGalleryServerProConfigSection().Core.EnableExceptionHandler)
				{
					// Redirect to generic error page.
					HttpContext.Current.Server.ClearError();
					HttpContext.Current.Items["CurrentAppError"] = AppError.Create(ex);
					Util.Transfer(PageId.error_generic);
				}
			}
		}
	}
}

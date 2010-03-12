using System;
using System.Globalization;

namespace GalleryServerPro.Web
{
	public class Global : System.Web.HttpApplication
	{
		#region Event Handlers

		protected void Application_Start(object sender, EventArgs e)
		{
			// Code that runs on application startup
			Application["ComponentArtWebUI_AppKey"] = "This edition of ComponentArt Web.UI is licensed for Gallery Server Pro application only."; 
		}

		//protected void Application_End(object sender, EventArgs e)
		//{
		//}

		protected void Application_Error(Object sender, EventArgs e)
		{
			// Grab a handle to the exception that is giving us grief.
			Exception ex = Server.GetLastError();

			if (Context != null)
			{
				if (Context.Error.InnerException != null)
					ex = Context.Error.InnerException;
				else if (Context.Error.GetBaseException() != null)
					ex = Context.Error.GetBaseException();
				else
					ex = Context.Error;
			}

			// Send administrator email, if that feature is turned on.
			GalleryServerPro.ErrorHandler.AppErrorHandler.RecordErrorInfo(ex);

			// If the error is security related, go to a special page that offers a friendly error message.
			if (ex is GalleryServerPro.ErrorHandler.CustomExceptions.GallerySecurityException)
			{
				// User is not allowed to access the requested page. Provide friendly, customized message.
				Server.ClearError();
				Response.Redirect("~/error/error_unauthorized.aspx", false);
				System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
			else if (ex is GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)
			{
				// Gallery Server cannot write to a directory. Application startup code checks for this condition,
				// so we'll get here most often when Gallery Server is first configured and the required permissions were not given.
				// Provide friendly, customized message to help the user resolve the situation.
				Server.ClearError();
				GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException ex2 = (GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException)ex;
				string mediaObjectPath = ex2.DirectoryPath;
				string redirectUrl = String.Format(CultureInfo.CurrentCulture, "~/error/error_cannot_write_to_media_object_dir.aspx?path={0}", Server.UrlEncode(mediaObjectPath));
				Response.Redirect(redirectUrl, false);
				System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
			else if (ex is GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectDirectoryException)
			{
				// Gallery Server cannot write to a directory. Application startup code checks for this condition,
				// so we'll get here most often when Gallery Server is first configured and the required permissions were not given.
				// Provide friendly, customized message to help the user resolve the situation.
				Server.ClearError();
				GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectDirectoryException ex2 = (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectDirectoryException)ex;
				string mediaObjectPath = ex2.MediaObjectPath;
				string redirectUrl = String.Format(CultureInfo.CurrentCulture, "~/error/error_cannot_write_to_media_object_dir.aspx?path={0}", Server.UrlEncode(mediaObjectPath));
				Response.Redirect(redirectUrl, false);
				System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
		}

		public void Session_OnStart()
		{
			// Add a dummy value to session so that the session ID remains constant. (This is required by WebController.GetRolesForUser().)
			Session.Add("1", "1");
		}

		public void Session_OnEnd()
		{
			GalleryServerPro.Business.HelperFunctions.CacheManager.Remove(Session.SessionID + "_roles");
		}

		//public void Profile_ProfileAutoSaving(Object sender, System.Web.Profile.ProfileAutoSaveEventArgs e)
		//{
		//  e.ContinueWithProfileAutoSave = false;
		//}

		#endregion

		#region Private Methods
		
		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using Gsp = GalleryServerPro.Business;

namespace GalleryServerPro.Web
{
	/// <summary> Contains general purpose routines useful for this website as well as a convenient
	/// gateway to functionality provided in other business layers.
	/// </summary>
	public class WebsiteController
	{
		#region Private Fields

		private int _mediaObjectId;
		private IAlbum _album;
		private IGalleryObject _mediaObject;

		#endregion

		#region Private Static Fields

		private static GalleryServerPro.Configuration.GalleryServerProConfigSettings _galleryServerProConfigSettings;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="WebsiteController"/> class.
		/// </summary>
		public WebsiteController()
		{
			_mediaObjectId = int.MinValue;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the media object ID from the query string. Returns int.MinValue if no valid media object ID is found,
		/// it is not a valid integer, or it is less than or equal to zero. Setting this value nulls out the private media object
		/// and album variables, allowing them to be regenerated using the new ID when calling <see cref="GetAlbum" /> or <see cref="GetMediaObject" />.
		/// </summary>
		public int MediaObjectId
		{
			get
			{
				if (this._mediaObjectId > int.MinValue)
				{
					return this._mediaObjectId;
				}
				else
				{
					return GetQueryStringParameterInt32("moid");
				}
			}
			set
			{
				this._mediaObjectId = value;
				this._mediaObject = null;
				this._album = null;
			}
		}

		#endregion

		#region Public Static Properties

		#endregion

		#region Public Methods

		/// <summary>
		/// Create a fully inflated album instance for the album containing the media object ID specified in the query
		/// string (moid). If the moid query string parameter does not refer to a valid media object, the user is redirected 
		/// to the home page. If the moid query string parameter doesn't exist, the query string is parsed for the album ID 
		/// (aid). If the aid query string parameter doesn't match an album in the database for the current gallery, an
		/// InvalidAlbumException exception is thrown. If the aid query string parameter doesn't exist, then null is 
		/// returned. Album properties are retrieved from the data store. If this album contains 
		/// child objects, they are added but not inflated. NOTE: No security authorization checks are made to ensure
		/// the user is authorized to access the specified album! The developer calling this code is responsible for
		/// enforcing security!
		/// </summary>
		/// <returns>Returns an IAlbum object.</returns>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidAlbumException">Thrown when an 
		/// album with the specified album ID is not found in the data store.</exception>
		public IAlbum GetAlbum()
		{
			// 1. If we have a loaded media object, return a reference to its parent.
			if (this._mediaObject != null)
			{
				return (IAlbum)this._mediaObject.Parent;
			}

				// 2. If we have a loaded album, return it.
			else if (this._album != null)
			{
				return this._album;
			}

				// 3. If moid is specified in the query string, inflate the media object and return a ref to its parent.
			else if (GetQueryStringParameterInt32("moid") > int.MinValue)
			{
				return (IAlbum)this.GetMediaObject().Parent;
			}

				// 4. If aid is specified in the query string, inflate the album and return.
			else if (GetQueryStringParameterInt32("aid") > int.MinValue)
			{
				this._album = Gsp.Factory.LoadAlbumInstance(GetQueryStringParameterInt32("aid"), false);

				return this._album;
			}

				// 5. If none of the above succeeded, return null. This will happen when neither 'aid' nor 'moid' is
			// specified on the query string.
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Create a fully inflated, properly typed media object instance based on the media object ID in
		/// the query string (moid). Guaranteed to not return an Album instance, even if moid contains a value
		/// that represents an album. If the moid query string parameter is 
		/// not present, does not represent a valid integer, or no media object exists in the data store that 
		/// matches the specified value and current gallery, the web application is redirected to the root page.
		/// </summary>
		/// <returns>Returns a GalleryObject object that represents the relevant derived media object type
		/// (e.g. Image, Video, etc). Note that an album is not considered a media object type, so it will never be
		/// returned.</returns>
		public IGalleryObject GetMediaObject()
		{
			if (this._mediaObject == null)
			{
				IGalleryObject tempMediaObject = new Gsp.NullObjects.NullGalleryObject();

				try
				{
					tempMediaObject = Gsp.Factory.LoadMediaObjectInstance(this.MediaObjectId);
				}
				catch (ArgumentException)
				{
					string redirectUrl = AddQueryStringParameter(GetAppRootUrl(), "msg=" + (int)Message.MediaObjectDoesNotExist);
					System.Web.HttpContext.Current.Response.Redirect(redirectUrl, true); // Use 'true' to force immediate end to page processing
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException)
				{
					string redirectUrl = AddQueryStringParameter(GetAppRootUrl(), "msg=" + (int)Message.MediaObjectDoesNotExist);
					System.Web.HttpContext.Current.Response.Redirect(redirectUrl, true); // Use 'true' to force immediate end to page processing
				}

				this._mediaObject = tempMediaObject;
			}

			return this._mediaObject;
		}

		///// <summary>
		///// Get a list of roles that the currently logged-on user is in. Returns null if no user is logged in.
		///// </summary>
		///// <returns>Returns an array of strings representing the roles that the currently logged-on user is in. Returns null 
		///// if no user is logged in. Returns an empty array if the user is logged in but not assigned to any roles.</returns>
		//public string[] GetRolesForUser()
		//{
		//  if (!HttpContext.Current.User.Identity.IsAuthenticated)
		//    return null;

		//  string[] roleNames;

		//  if (HttpContext.Current.Session != null)
		//  {
		//    // See if the roles for this user are in the cache. The cache key is uniquely identified by contatenating
		//    // the user's session ID and the string "_roles". If not in the cache, retrieve it from the Role API and 
		//    // then insert into the cache for easy access next time.
		//    string userCacheName = HttpContext.Current.Session.SessionID + "_roles";
		//    roleNames = (string[])CacheManager.GetData(userCacheName);

		//    if ((roleNames == null) || (roleNames.Length == 0))
		//    {
		//      roleNames = Roles.GetRolesForUser();
		//      CacheManager.Add(userCacheName, roleNames);
		//    }
		//  }
		//  else
		//  {
		//    // Session is null. We could be on a page like synchronize.aspx where session is disabled to allow asynchronous
		//    // page methods to work.
		//    roleNames = Roles.GetRolesForUser();
		//  }

		//  return roleNames;
		//}

		/// <summary>
		/// Gets Gallery Server roles representing the roles for the currently logged-on user. Returns an 
		/// empty collection if no user is logged in or the user is logged in but not assigned to any roles (Count = 0).
		/// </summary>
		/// <returns>Returns a collection of Gallery Server roles representing the roles for the currently logged-on user. 
		/// Returns an empty collection if no user is logged in or the user is logged in but not assigned to any roles (Count = 0).</returns>
		public static IGalleryServerRoleCollection GetRolesForUser()
		{
			if (!HttpContext.Current.User.Identity.IsAuthenticated)
				return new Gsp.GalleryServerRoleCollection();

			// Get cached dictionary entry matching logged on user. If not found, retrieve from business layer and add to cache.
			Dictionary<string, IGalleryServerRoleCollection> rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)Gsp.HelperFunctions.CacheManager.GetData(Gsp.CacheItem.GalleryServerRoles.ToString());

			IGalleryServerRoleCollection roles;

			if ((rolesCache != null) && (HttpContext.Current.Session != null) && (rolesCache.TryGetValue(HttpContext.Current.Session.SessionID, out roles)))
			{
				return roles;
			}

			// No roles in the cache, so get from business layer and add to cache.
			roles = Gsp.Factory.LoadGalleryServerRoles(Roles.GetRolesForUser());

			if (rolesCache == null)
			{
				// The factory method should have created a cache item, so try again.
				rolesCache = (Dictionary<string, IGalleryServerRoleCollection>)Gsp.HelperFunctions.CacheManager.GetData(Gsp.CacheItem.GalleryServerRoles.ToString());
				if (rolesCache == null)
				{
					GalleryServerPro.ErrorHandler.AppErrorHandler.RecordErrorInfo(new GalleryServerPro.ErrorHandler.CustomExceptions.WebException("The method Factory.LoadGalleryServerRoles() should have created a cache entry, but none was found. This is not an issue if it occurs occasionally, but should be addressed if it is frequent."));
					return roles;
				}
			}

			// Add to the cache, but only if we have access to the session ID. Some pages, such as synchronize.aspx, turn off session 
			// so that asynchronous page methods can work.
			if (HttpContext.Current.Session != null)
			{
				lock (rolesCache)
				{
					if (!rolesCache.ContainsKey(HttpContext.Current.Session.SessionID))
					{
						rolesCache.Add(HttpContext.Current.Session.SessionID, roles);
					}
				}
				Gsp.HelperFunctions.CacheManager.Add(Gsp.CacheItem.GalleryServerRoles.ToString(), rolesCache);
			}

			return roles;
		}

		/// <summary>
		/// Determine whether user has permission to perform the specified security action. Un-authenticated users (anonymous users) are 
		/// always considered NOT authorized (that is, this method returns false) except when the requested security action is 
		/// ViewAlbumOrMediaObject or ViewOriginalImage, since Gallery Server is configured by default to allow anonymous viewing access
		/// but it does not allow anonymous editing of any kind. This method will continue to work correctly if the webmaster configures
		/// Gallery Server to require users to log in in order to view objects, since at that point there will be no such thing as 
		/// un-authenticated users, and the standard gallery server role functionality applies. This method behaves similarly to CheckUserSecurity()
		/// except that it returns true or false instead of throwing an exception when the user is not authorized.
		/// </summary>
		/// <param name="securityActions">The security action represents the action being carried out by the web page.</param>
		/// <param name="roles">A collection of Gallery Server roles to which the currently logged-on user belongs. This parameter is ignored
		/// for anonymous users. The parameter may be null.</param>
		/// <param name="albumId">The album ID to which the security action applies.</param>
		/// <param name="isPrivate">Indicates whether the specified album is private (hidden from anonymous users). The parameter
		/// is ignored for logged on users.</param>
		/// <returns>Returns true when the user is authorized to perform the specified security action against the specified album;
		/// otherwise returns false.</returns>
		public static bool IsUserAuthorized(Gsp.SecurityActions securityActions, IGalleryServerRoleCollection roles, int albumId, bool isPrivate)
		{
			return Gsp.SecurityManager.IsUserAuthorized(securityActions, roles, albumId, HttpContext.Current.User.Identity.IsAuthenticated, isPrivate);
		}

		#endregion

		#region Private Methods

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Returns a reference to the galleryServerPro custom configuration section in galleryServerPro.config.
		/// </summary>
		/// <returns>Returns a GalleryServerPro.Configuration.GalleryServerProConfigSettings object.</returns>
		public static GalleryServerPro.Configuration.GalleryServerProConfigSettings GetGalleryServerProConfigSection()
		{
			if (_galleryServerProConfigSettings == null)
			{
				_galleryServerProConfigSettings = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection();
			}

			return _galleryServerProConfigSettings;
		}

		/// <summary>
		/// Determine the trust level of the currently running application.
		/// </summary>
		/// <returns>Returns the trust level of the currently running application.</returns>
		public static Gsp.ApplicationTrustLevel GetCurrentTrustLevel()
		{
			AspNetHostingPermissionLevel aspnetTrustLevel = AspNetHostingPermissionLevel.None;

			foreach (AspNetHostingPermissionLevel aspnetTrustLevelIterator in
				new AspNetHostingPermissionLevel[] {
				                                   	AspNetHostingPermissionLevel.Unrestricted,
				                                   	AspNetHostingPermissionLevel.High,
				                                   	AspNetHostingPermissionLevel.Medium,
				                                   	AspNetHostingPermissionLevel.Low,
				                                   	AspNetHostingPermissionLevel.Minimal 
				                                   })
			{
				try
				{
					new AspNetHostingPermission(aspnetTrustLevelIterator).Demand();
					aspnetTrustLevel = aspnetTrustLevelIterator;
					break;
				}
				catch (System.Security.SecurityException)
				{
					continue;
				}
			}

			Gsp.ApplicationTrustLevel trustLevel = Gsp.ApplicationTrustLevel.None;

			switch (aspnetTrustLevel)
			{
				case AspNetHostingPermissionLevel.Minimal: trustLevel = Gsp.ApplicationTrustLevel.Minimal; break;
				case AspNetHostingPermissionLevel.Low: trustLevel = Gsp.ApplicationTrustLevel.Low; break;
				case AspNetHostingPermissionLevel.Medium: trustLevel = Gsp.ApplicationTrustLevel.Medium; break;
				case AspNetHostingPermissionLevel.High: trustLevel = Gsp.ApplicationTrustLevel.High; break;
				case AspNetHostingPermissionLevel.Unrestricted: trustLevel = Gsp.ApplicationTrustLevel.Full; break;
				default: trustLevel = Gsp.ApplicationTrustLevel.Unknown; break;
			}

			return trustLevel;
		}

		/// <summary>
		/// Get the virtual application root path on the server. Does not include the default page or the trailing slash. Ex: /galleryserverpro
		/// </summary>
		/// <returns>Returns the virtual application root path on the server.</returns>
		public static string GetAppRootPathUrl()
		{
			string appPath = HttpContext.Current.Request.ApplicationPath;

			if (appPath == "/")
				appPath = String.Empty;

			return appPath;
		}

		/// <summary>
		/// Get the virtual application path to the current theme's folder. Does not include the trailing slash. Ex: /galleryserverpro/App_Themes/HelixBlue
		/// </summary>
		/// <param name="themeName">The name of the current theme.</param>
		/// <returns>Returns the virtual application path to the current theme's folder.</returns>
		public static string GetThemePathUrl(string themeName)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}/App_Themes/{1}", GetAppRootPathUrl(), themeName);
		}

		/// <summary>
		/// Get the virtual application root path on the server, including the default page. Ex: /galleryserverpro/default.aspx
		/// </summary>
		/// <returns>Returns the virtual application root path on the server, including the default page.</returns>
		public static string GetAppRootUrl()
		{
			return String.Concat(GetAppRootPathUrl(), "/default.aspx");
		}

		/// <summary>
		/// Get the absolute application root path on the server, including the default page. Ex: http://www.techinfosystems.com/gs/default.aspx
		/// </summary>
		/// <returns>Returns the absolute application root path on the server, including the default page.</returns>
		/// <remarks>This value is retrieved from the user's session. If not present in the session, such as when the user first arrives, it
		/// is calculated by passing the appropriate pieces from HttpContext.Current.Request.Url and HttpContext.Current.Request.ApplicationPath
		/// to a UriBuilder object. The path is calculated on a per-user basis because the URL may be different for different users 
		/// (a local admin's URL may be http://localhost/gs/default.aspx, someone on the intranet may get the server's name
		/// (http://Server1/gs/default.aspx), and someone on the internet may get the full name (http://www.bob.com/gs/default.aspx).</remarks>
		public static string GetAppRootAbsoluteUrl()
		{
			string appRootUrl = null;
			
			if (HttpContext.Current.Session != null)
			{
				object appRootUrlSession = HttpContext.Current.Session["AppRootUrl"];
				if (appRootUrlSession != null)
					appRootUrl = appRootUrlSession.ToString();
			}

			if (String.IsNullOrEmpty(appRootUrl))
			{
				// Calculate the URL.
				Uri uri = HttpContext.Current.Request.Url;
				UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host);

				if (!uri.IsDefaultPort)
					uriBuilder.Port = uri.Port;

				uriBuilder.Path = GetAppRootUrl();

				appRootUrl = uriBuilder.ToString();

				if (HttpContext.Current.Session != null)
					HttpContext.Current.Session["AppRootUrl"] = appRootUrl;
			}

			return appRootUrl;
		}

		/// <summary>
		/// Send an email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. If the emailRecipient parameter is not specified, the email
		/// is sent to the address configured in the emailToAddress setting in the configuration file.
		/// </summary>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		/// <param name="isBodyHtml">Indicates whether the body of the email is in HTML format. When false, the body is
		/// assumed to be plain text.</param>
		public static void SendEmail(string subject, string body, bool isBodyHtml)
		{
			MailAddress recipient = new MailAddress(GetGalleryServerProConfigSection().Core.EmailToAddress, GetGalleryServerProConfigSection().Core.EmailToName);
			SendEmail(recipient, subject, body, isBodyHtml);
		}

		/// <summary>
		/// Send an email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. If the emailRecipient parameter is not specified, the email
		/// is sent to the address configured in the emailToAddress setting in the configuration file.
		/// </summary>
		/// <param name="emailRecipient">The recipient of the email.</param>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		/// <param name="isBodyHtml">Indicates whether the body of the email is in HTML format. When false, the body is
		/// assumed to be plain text.</param>
		public static void SendEmail(MailAddress emailRecipient, string subject, string body, bool isBodyHtml)
		{
			MailAddressCollection mailAddresses = new MailAddressCollection();
			mailAddresses.Add(emailRecipient);

			SendEmail(mailAddresses, subject, body, isBodyHtml);
		}

		/// <summary>
		/// Send an email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. If the emailRecipient parameter is not specified, the email
		/// is sent to the address configured in the emailToAddress setting in the configuration file.
		/// </summary>
		/// <param name="emailRecipients">The email recipients.</param>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		/// <param name="isBodyHtml">Indicates whether the body of the email is in HTML format. When false, the body is
		/// assumed to be plain text.</param>
		public static void SendEmail(MailAddressCollection emailRecipients, string subject, string body, bool isBodyHtml)
		{
			Core coreConfig = GetGalleryServerProConfigSection().Core;

			MailAddress emailSender = new MailAddress(coreConfig.EmailFromAddress, coreConfig.EmailFromName);
			string smtpServer = coreConfig.SmtpServer;
			int smtpServerPort;
			if (!Int32.TryParse(coreConfig.SmtpServerPort, out smtpServerPort))
				smtpServerPort = int.MinValue;

			coreConfig = null;

			MailMessage mail = new MailMessage();
			foreach (MailAddress mailAddress in emailRecipients)
			{
				mail.To.Add(mailAddress);
			}
			mail.From = emailSender;
			mail.Subject = subject;
			mail.Body = body;
			mail.IsBodyHtml = isBodyHtml;

			SmtpClient smtpClient = new SmtpClient();

			// Specify SMTP server if it's in galleryserverpro.config. The server might have been assigned via web.config,
			// so only update this if we have a config setting.
			if (!String.IsNullOrEmpty(smtpServer))
			{
				smtpClient.Host = smtpServer;
			}

			// Specify port number if it's in galleryserverpro.config and it's not the default value of 25. The port 
			// might have been assigned via web.config, so only update this if we have a config setting.
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


		/// <summary>
		/// Retrieves the specified query string parameter value from the query string. Returns int.MinValue if
		/// the parameter is not found, it is not a valid integer, or it is &lt;= 0.
		/// </summary>
		/// <param name="parameterName">The name of the query string parameter for which to retrieve it's value.</param>
		/// <returns>Returns the value of the specified query string parameter.</returns>
		public static int GetQueryStringParameterInt32(string parameterName)
		{
			string parm = System.Web.HttpContext.Current.Request.QueryString[parameterName];

			if ((String.IsNullOrEmpty(parm)) || (!Gsp.HelperFunctions.IsInt32(parm) || (Convert.ToInt32(parm, CultureInfo.InvariantCulture) <= 0)))
			{
				return int.MinValue;
			}
			else
			{
				return Convert.ToInt32(parm, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Retrieves the specified query string parameter value from the query string. If no URI is specified, the current 
		/// request URL is used. Returns int.MinValue if the parameter is not found, it is not a valid integer, or it is &lt;= 0.
		/// </summary>
		/// <param name="uri">The URI containing the query string parameter to retrieve.</param>
		/// <param name="parameterName">The name of the query string parameter for which to retrieve it's value.</param>
		/// <returns>Returns the value of the specified query string parameter.</returns>
		public static int GetQueryStringParameterInt32(Uri uri, string parameterName)
		{
			string parm = null;
			if (uri == null)
			{
				parm = System.Web.HttpContext.Current.Request.QueryString[parameterName];
			}
			else
			{
				string qs = uri.Query.TrimStart(new char[] { '?' });
				foreach (string nameValuePair in qs.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
				{
					string[] nameValue = nameValuePair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
					if (nameValue.Length > 1)
					{
						if (String.Equals(nameValue[0], parameterName))
						{
							parm = nameValue[1];
							break;
						}
					}
				}
			}

			if ((String.IsNullOrEmpty(parm)) || (!Gsp.HelperFunctions.IsInt32(parm) || (Convert.ToInt32(parm, CultureInfo.InvariantCulture) <= 0)))
			{
				return int.MinValue;
			}
			else
			{
				return Convert.ToInt32(parm, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Retrieves the specified query string parameter value from the query string. Returns string.Empty if
		/// the parameter is not found.
		/// </summary>
		/// <param name="parameterName">The name of the query string parameter for which to retrieve it's value.</param>
		/// <returns>Returns the value of the specified query string parameter.</returns>
		public static string GetQueryStringParameterString(string parameterName)
		{
			object parm = System.Web.HttpContext.Current.Request.QueryString[parameterName];

			if (parm == null)
			{
				return string.Empty;
			}
			else
			{
				return parm.ToString();
			}
		}

		/// <summary>
		/// Retrieves the specified query string parameter value from the query string. The values "true" and "1"
		/// are returned as true; any other value is returned as false. It is not case sensitive. The bool is not
		/// set if the parameter is not present in the query string (i.e. the HasValue property is false).
		/// </summary>
		/// <param name="parameterName">The name of the query string parameter for which to retrieve it's value.</param>
		/// <returns>Returns the value of the specified query string parameter.</returns>
		public static bool? GetQueryStringParameterBoolean(string parameterName)
		{
			bool? parmValue = null;

			object parm = System.Web.HttpContext.Current.Request.QueryString[parameterName];

			if (parm != null)
			{
				string parmString = parm.ToString().ToUpperInvariant();
				if ((parmString == "TRUE") || (parmString == "1"))
				{
					parmValue = true;
				}
				else
				{
					parmValue = false;
				}
			}

			return parmValue;
		}

		/// <summary>
		/// Append the string to the url as a query string parameter
		/// Example:
		/// Url = "www.galleryserverpro.com/index.aspx?aid=5&amp;msg=3"
		/// QueryStringParameterNameValue = "moid=27"
		/// Return value: www.galleryserverpro.com/index.aspx?aid=5&amp;msg=3&amp;moid=27
		/// </summary>
		/// <param name="url">The Url to which the query string parameter should be added
		/// (e.g. www.galleryserverpro.com/index.aspx?aid=5&amp;msg=3).</param>
		/// <param name="queryStringParameterNameValue">The query string parameter and value to add to the Url
		/// (e.g. "moid=27").</param>
		/// <returns>Returns a new Url containing the specified query string parameter.</returns>
		public static string AddQueryStringParameter(string url, string queryStringParameterNameValue)
		{
			string rv = url;

			if (url.IndexOf("?", StringComparison.Ordinal) < 0)
			{
				rv += "?" + queryStringParameterNameValue;
			}
			else
			{
				rv += "&" + queryStringParameterNameValue;
			}
			return rv;
		}

		/// <summary>
		/// Remove the specified query string parameter from the url.
		/// Example:
		/// Url = "www.galleryserverpro.com/index.aspx?aid=5&amp;msg=3&amp;moid=27"
		/// QueryStringParameterName = "msg"
		/// Return value: www.galleryserverpro.com/index.aspx?aid=5&amp;moid=27
		/// </summary>
		/// <param name="url">The Url containing the query string parameter to remove
		/// (e.g. www.galleryserverpro.com/index.aspx?aid=5&amp;msg=3&amp;moid=27).</param>
		/// <param name="queryStringParameterName">The query string parameter name to remove from the Url
		/// (e.g. "msg").</param>
		/// <returns>Returns a new Url with the specified query string parameter removed.</returns>
		public static string RemoveQueryStringParameter(string url, string queryStringParameterName)
		{
			string newUrl;

			// Get the location of the question mark so we can separate the base url from the query string
			int separator = url.IndexOf("?", StringComparison.Ordinal);

			if (separator < 0)
			{
				// No query string exists on the url. Simply return the original url.
				newUrl = url;
			}
			else
			{
				// We have a query string. Separate the base url from the query string, and process the query string.

				// Add the base url (e.g. "www.galleryserverpro.com/index.aspx?")
				newUrl = String.Concat(url.Substring(0, separator), "?");

				string queryString = url.Substring(separator + 1);

				if (queryString.Length > 0)
				{
					// Url has a query string. Split each name/value pair into a string array, and rebuild the
					// query string, leaving out the parm passed to the function.
					string[] queryItems = queryString.Split(new char[] { '&' });

					for (int i = 0; i < queryItems.Length; i++)
					{
						if (!queryItems[i].StartsWith(queryStringParameterName, StringComparison.OrdinalIgnoreCase))
						{
							// Query parm doesn't match, so include it as we rebuilt the new query string
							newUrl += String.Concat(queryItems[i], "&");
						}
					}
				}
				// Trim any trailing '&' or '?'.
				newUrl = newUrl.TrimEnd(new char[] { '&', '?' });
			}

			return newUrl;
		}

		/// <summary>
		/// Returns a value indicating whether the specified query string parameter value is part of the query string. 
		/// </summary>
		/// <param name="parameterName">The name of the query string parameter to check for.</param>
		/// <returns>Returns true if the specified query string parameter value is part of the query string; otherwise 
		/// returns false. </returns>
		public static bool IsQueryStringParameterPresent(string parameterName)
		{
			return (System.Web.HttpContext.Current.Request.QueryString[parameterName] != null);
		}

		/// <overloads>Remove all HTML tags from the specified string.</overloads>
		/// <summary>
		/// Remove all HTML tags from the specified string.
		/// </summary>
		/// <param name="html">The string containing HTML tags to remove.</param>
		/// <returns>Returns a string with all HTML tags removed.</returns>
		public static string RemoveHtmlTags(string html)
		{
			return RemoveHtmlTags(html, false);
		}

		/// <summary>
		/// Remove all HTML tags from the specified string. If <paramref name="escapeQuotes"/> is true, then all 
		/// apostrophes and quotation marks are replaced with &quot; and &apos; so that the string can be specified in HTML 
		/// attributes such as title tags. If the escapeQuotes parameter is not specified, no replacement is performed.
		/// </summary>
		/// <param name="html">The string containing HTML tags to remove.</param>
		/// <param name="escapeQuotes">When true, all apostrophes and quotation marks are replaced with &quot; and &apos;.</param>
		/// <returns>Returns a string with all HTML tags removed.</returns>
		public static string RemoveHtmlTags(string html, bool escapeQuotes)
		{
			return Gsp.HtmlScrubber.RemoveAllTags(html, escapeQuotes);
		}

		/// <summary>
		/// Clean the potentially dangerous HTML so that unauthorized HTML and Javascript is removed. If the configuration
		/// setting allowHtmlInTitlesAndCaptions is true, then the input is "cleaned" so that all HTML tags that are not in
		/// a predefined list of acceptable HTML tags are HTML-encoded, and all attributes not found in the white list
		/// are removed (e.g. onclick, onmouseover). If allowHtmlInTitlesAndCaptions = "false", then all HTML tags are
		/// removed. Regardless of the configuration setting, all &lt;script&gt; tags are escaped and all instances of 
		/// "javascript:" are removed. 
		/// </summary>
		/// <param name="html">The string containing the potentially dangerous HTML tags.</param>
		/// <returns>Returns a string with potentially dangerous HTML tags HTML-encoded or removed.</returns>
		public static string CleanHtmlTags(string html)
		{
			return Gsp.HtmlScrubber.SmartClean(html);
		}

		/// <summary>
		/// Returns the current version of Gallery Server.
		/// </summary>
		/// <returns>Returns a string representing the version (e.g. "1.0.0").</returns>
		public static string GetGalleryServerVersion()
		{
			string appVersion;
			object version = System.Web.HttpContext.Current.Application["GalleryServerVersion"];
			if (version != null)
			{
				// Version was found in Application cache. Return.
				appVersion = version.ToString();
			}
			else
			{
				// Version was not found in application cache. Get it using Reflection, store in
				// application cache so it is available for future requests, and return.
				appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
				//appVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;

				// Trim the version from the 3rd period to the end. Ex: If appVersion = 1.2.3432.38725, then return 1.2.3432
				appVersion = appVersion.Substring(0, appVersion.LastIndexOf(".", StringComparison.Ordinal));
				System.Web.HttpContext.Current.Application["GalleryServerVersion"] = appVersion;
			}

			return appVersion;
		}

		/// <summary>
		/// Truncate the specified string to the desired length. Any HTML tags that exist in the beginning portion
		/// of the string are preserved as long as no HTML tags exist in the part that is truncated.
		/// </summary>
		/// <param name="text">The string to be truncated. It may contain HTML tags.</param>
		/// <param name="maxLength">The maximum length of the string to be returned. If HTML tags are returned,
		/// their length is not counted - only the length of the "visible" text is counted.</param>
		/// <returns>Returns a string whose length - not counting HTML tags - does not exceed the specified length.</returns>
		public static string TruncateTextForWeb(string text, int maxLength)
		{
			// Example 1: Because no HTML tags are present in the truncated portion of the string, the HTML at the
			// beginning is preserved. (We know we won't be splitting up HTML tags, so we don't mind including the HTML.)
			// text = "Meet my <a href='http://www.cnn.com'>friend</a>. He works at the YMCA."
			// maxLength = 20
			// returns: "Meet my <a href='http://www.cnn.com'>friend</a>. He w"
			//
			// Example 2: The truncated portion has <b> tags, so all HTML is stripped. (This function isn't smart
			// enough to know whether it might be truncating in the middle of a tag, so it takes the safe route.)
			// text = "Meet my <a href='http://www.cnn.com'>friend</a>. He works at the <b>YMCA<b>."
			// maxLength = 20
			// returns: "Meet my friend. He w"
			if (text == null)
				return string.Empty;

			if (text.Length < maxLength)
				return text;

			// Remove all HTML tags from entire string.
			string cleanText = RemoveHtmlTags(text);

			// If the clean text length is less than our maximum, return the raw text.
			if (cleanText.Length <= maxLength)
				return text;

			// Get the text that will be removed.
			string cleanTruncatedPortion = cleanText.Substring(maxLength);

			// If the clean truncated text doesn't match the end of the raw text, the raw text must have HTML tags.
			bool truncatedPortionHasHtml = (!(text.EndsWith(cleanTruncatedPortion, StringComparison.OrdinalIgnoreCase)));

			string truncatedText;
			if (truncatedPortionHasHtml)
			{
				// Since the truncated portion has HTML tags, and we don't want to risk returning malformed HTML,
				// return text without ANY HTML.
				truncatedText = cleanText.Substring(0, maxLength);
			}
			else
			{
				// Since the truncated portion does not have HTML tags, we can safely return the first part of the
				// string, even if it has HTML tags.
				truncatedText = text.Substring(0, text.Length - cleanTruncatedPortion.Length);
			}
			return truncatedText;
		}

		/// <summary>
		/// Initialize the Gallery Server Pro application. This method is designed to be run at application startup. The business layer
		/// is initialized with the current trust level and a few configuration settings. The business layer also initializes
		/// the data store, including verifying a minimal level of data integrity, such as at least one record for the root album.
		/// Old anonymous profiles are removed from the profile provider. Roles are synchronized between the ASP.NET roles provider 
		/// and the Gallery Server roles data store (e.g. the gs_roles table when using GalleryServerPro.Data.SqlDataProvider).
		/// </summary>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectDirectoryException">
		/// Thrown when Gallery Server Pro cannot find, or is unable to create, a directory 
		/// corresponding to the specified media objects path.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException">
		/// Thrown when Gallery Server Pro is unable to write to, or delete from, the media objects directory.</exception>
		public static void InitializeApplication()
		{
			// Set web-related variables in the business layer and initialize the data store.
			InitializeBusinessLayer();

			// Delete anonymous profiles, if they exist, to minimize the database clutter.
			DeleteAnonymousProfiles();

			// Validate users, roles and profiles.
			ValidateMembership();
		}

		/// <summary>
		/// Persist the gallery object to the data store. This method updates the audit fields before saving. All gallery objects should be
		/// saved through this method rather than directly invoking the gallery object's Save method, unless you want to 
		/// manually update the audit fields yourself.
		/// </summary>
		/// <param name="galleryObject">The gallery object to persist to the data store.</param>
		public static void SaveGalleryObject(IGalleryObject galleryObject)
		{
			string currentUser = HttpContext.Current.User.Identity.Name;
			DateTime currentTimestamp = DateTime.Now;

			if (galleryObject.IsNew)
			{
				galleryObject.CreatedByUserName = currentUser;
				galleryObject.DateAdded = currentTimestamp;
			}

			if (galleryObject.HasChanges)
			{
				galleryObject.LastModifiedByUserName = currentUser;
				galleryObject.DateLastModified = currentTimestamp;
			}

			galleryObject.Save();
		}

		/// <summary>
		/// Move the specified object to the specified destination album. This method moves the physical files associated with this
		/// object to the destination album's physical directory. The object's Save() method is invoked to persist the changes to the
		/// data store. When moving albums, all the album's children, grandchildren, etc are also moved. 
		/// The audit fields are automatically updated before saving.
		/// </summary>
		/// <param name="galleryObjectToMove">The gallery object to move.</param>
		/// <param name="destinationAlbum">The album to which the current object should be moved.</param>
		public static void MoveGalleryObject(IGalleryObject galleryObjectToMove, IAlbum destinationAlbum)
		{
			string currentUser = HttpContext.Current.User.Identity.Name;
			DateTime currentTimestamp = DateTime.Now;

			galleryObjectToMove.LastModifiedByUserName = currentUser;
			galleryObjectToMove.DateLastModified = currentTimestamp;

			galleryObjectToMove.MoveTo(destinationAlbum);
		}

		/// <summary>
		/// Copy the specified object and place it in the specified destination album. This method creates a completely separate copy
		/// of the original, including copying the physical files associated with this object. The copy is persisted to the data
		/// store and then returned to the caller. When copying albums, all the album's children, grandchildren, etc are also copied.
		/// The audit fields of the copied objects are automatically updated before saving.
		/// </summary>
		/// <param name="galleryObjectToCopy">The gallery object to copy.</param>
		/// <param name="destinationAlbum">The album to which the current object should be copied.</param>
		/// <returns>
		/// Returns a new gallery object that is an exact copy of the original, except that it resides in the specified
		/// destination album, and of course has a new ID. Child objects are recursively copied.
		/// </returns>
		public static IGalleryObject CopyGalleryObject(IGalleryObject galleryObjectToCopy, IAlbum destinationAlbum)
		{
			string currentUser = HttpContext.Current.User.Identity.Name;

			return galleryObjectToCopy.CopyTo(destinationAlbum, currentUser);
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Delete all anonymous profiles older than today. This is typically called during application startup to clean up the 
		/// profiles data store.
		/// </summary>
		private static void DeleteAnonymousProfiles()
		{
			GalleryServerPro.Business.HelperFunctions.BeginTransaction();

			try
			{
				foreach (System.Web.Profile.ProfileInfo profile in System.Web.Profile.ProfileManager.GetAllInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption.Anonymous, DateTime.Today))
				{
					Membership.DeleteUser(profile.UserName, true);
				}
				GalleryServerPro.Business.HelperFunctions.CommitTransaction();
			}
			catch
			{
				GalleryServerPro.Business.HelperFunctions.RollbackTransaction();
				throw;
			}

			// Don't use the following technique. It only deletes records in the profile table. Records in the users table are not deleted.
			//System.Web.Profile.ProfileManager.DeleteInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption.Anonymous, DateTime.Today);
		}

		/// <summary>
		/// Validate the integrity of the membership, roles, and profile configuration.
		/// </summary>
		private static void ValidateMembership()
		{
			ProcessInstallerFile();

			ValidateRoles();
		}

		/// <summary>
		/// In certain cases, the web-based installer creates a text file in the App Data directory that is meant as a signal to this
		/// code that additional setup steps are required. If this file is found, carry out the additional actions. This file is
		/// created in the InitializeSQLiteData() method of ~\installer\default.aspx.cs.
		/// </summary>
		private static void ProcessInstallerFile()
		{
			string filePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Gsp.GlobalConstants.AppDataDirectory, Gsp.GlobalConstants.InstallerFileName));

			if (!File.Exists(filePath))
				return;

			string adminUserName;
			string adminPwd;
			string adminEmail;
			using (StreamReader sw = File.OpenText(filePath))
			{
				adminUserName = sw.ReadLine();
				adminPwd = sw.ReadLine();
				adminEmail = sw.ReadLine();
			}

			Gsp.HelperFunctions.BeginTransaction();

			#region Create the Sys Admin role.

			// Create the Sys Admin role. If it already exists, make sure it has AllowAdministerSite permission.
			string sysAdminRoleName = Resources.GalleryServerPro.Installer_Sys_Admin_Role_Name;
			if (!Roles.RoleExists(sysAdminRoleName))
				Roles.CreateRole(sysAdminRoleName);

			IGalleryServerRole role = Gsp.Factory.LoadGalleryServerRole(sysAdminRoleName);
			if (role == null)
			{
				role = Gsp.Factory.CreateGalleryServerRoleInstance(sysAdminRoleName, true, true, true, true, true, true, true, true, true, true, true, true);
				role.RootAlbumIds.Add(Gsp.Factory.LoadRootAlbumInstance().Id);
				role.Save();
			}
			else
			{
				// Role already exists. Make sure it has Sys Admin permission.
				if (!role.AllowAdministerSite)
				{
					role.AllowAdministerSite = true;
					role.Save();
				}
			}

			#endregion

			#region Create the Sys Admin user account.

			// Create the Sys Admin user account. Will throw an exception if the name is already in use.
			try
			{
				Membership.CreateUser(adminUserName, adminPwd, adminEmail);
			}
			catch (MembershipCreateUserException ex)
			{
				if (ex.StatusCode == MembershipCreateStatus.DuplicateUserName)
				{
					// The user already exists. Update the password and email address to our values.
					MembershipUser user = Membership.GetUser(adminUserName);
					user.ChangePassword(user.GetPassword(), adminPwd);
					user.Email = adminEmail;
					Membership.UpdateUser(user);
				}
			}

			// Add the Sys Admin user to the Sys Admin role.
			if (!Roles.IsUserInRole(adminUserName, sysAdminRoleName))
				Roles.AddUserToRole(adminUserName, sysAdminRoleName);

			#endregion

			#region Create sample album and image

			DateTime currentTimestamp = DateTime.Now;
			IAlbum sampleAlbum = null;
			foreach (IAlbum album in Gsp.Factory.LoadRootAlbumInstance().GetChildGalleryObjects(GalleryObjectType.Album))
			{
				if (album.DirectoryName == "Samples")
				{
					sampleAlbum = album;
					break;
				}
			}
			if (sampleAlbum == null)
			{
				sampleAlbum = Gsp.Factory.CreateAlbumInstance();
				sampleAlbum.Parent = Gsp.Factory.LoadRootAlbumInstance();
				sampleAlbum.Title = "Samples";
				sampleAlbum.DirectoryName = "Samples";
				sampleAlbum.Summary = "Welcome to Gallery Server Pro!";
				sampleAlbum.CreatedByUserName = adminUserName;
				sampleAlbum.DateAdded = currentTimestamp;
				sampleAlbum.LastModifiedByUserName = adminUserName;
				sampleAlbum.DateLastModified = currentTimestamp;
				sampleAlbum.Save();
			}

			IGalleryObject sampleImage = null;
			foreach (IGalleryObject image in sampleAlbum.GetChildGalleryObjects(GalleryObjectType.Image))
			{
				if (image.Original.FileName == "RogerMartin&Family.jpeg")
				{
					sampleImage = image;
					break;
				}
			}
			if (sampleImage == null)
			{
				string sampleImagePath = Path.Combine(Path.Combine(Gsp.AppSetting.Instance.MediaObjectPhysicalPath, sampleAlbum.DirectoryName), "RogerMartin&Family.jpeg");
				if (File.Exists(sampleImagePath))
				{
					IGalleryObject image = Gsp.Factory.CreateImageInstance(new FileInfo(sampleImagePath), sampleAlbum);
					image.Title = "Roger, Margaret and Skyler Martin (August 2008)";
					image.CreatedByUserName = adminUserName;
					image.DateAdded = currentTimestamp;
					image.LastModifiedByUserName = adminUserName;
					image.DateLastModified = currentTimestamp;
					image.Save();
				}
			}

			#endregion

			Gsp.HelperFunctions.CommitTransaction();
			Gsp.HelperFunctions.PurgeCache();

			File.Delete(filePath);
		}

		/// <summary>
		/// Make sure the list of ASP.NET roles is synchronized with the Gallery Server roles. If any are missing from 
		/// either, add it.
		/// </summary>
		private static void ValidateRoles()
		{
			System.Collections.Generic.List<IGalleryServerRole> validatedRoles = new System.Collections.Generic.List<IGalleryServerRole>();
			IGalleryServerRoleCollection galleryRoles = GalleryServerPro.Business.Factory.LoadGalleryServerRoles();
			bool needToPurgeCache = false;

			foreach (string roleName in Roles.GetAllRoles())
			{
				IGalleryServerRole galleryRole = galleryRoles.GetRoleByRoleName(roleName);
				if (galleryRole == null)
				{
					// This is an ASP.NET role that doesn't exist in our list of gallery server roles. Add it with minimum permissions
					// applied to zero albums.
					IGalleryServerRole newRole = GalleryServerPro.Business.Factory.CreateGalleryServerRoleInstance(roleName, false, false, false, false, false, false, false, false, false, false, false, false);
					newRole.Save();
					needToPurgeCache = true;
				}
				validatedRoles.Add(galleryRole);
			}

			// Now check to see if there are gallery roles that are not ASP.NET roles. Add if necessary.
			foreach (IGalleryServerRole galleryRole in galleryRoles)
			{
				if (!validatedRoles.Contains(galleryRole))
				{
					// Need to create an ASP.NET role for this gallery role.
					System.Web.Security.Roles.CreateRole(galleryRole.RoleName);
					needToPurgeCache = true;
				}
			}

			if (needToPurgeCache)
			{
				Gsp.HelperFunctions.PurgeCache();
			}
		}

		/// <summary>
		/// Set up the business layer with information about this web application, such as its trust level and a few settings
		/// from the configuration file.
		/// </summary>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectDirectoryException">
		/// Thrown when Gallery Server Pro cannot find, or is unable to create, a directory 
		/// corresponding to the specified media objects path.</exception>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException">
		/// Thrown when Gallery Server Pro is unable to write to, or delete from, the media objects directory.</exception>
		private static void InitializeBusinessLayer()
		{
			// Determine the trust level this web application is running in and set to a global variable. This will be used 
			// throughout the application to gracefully degrade when we are not at Full trust.
			GalleryServerPro.Business.ApplicationTrustLevel trustLevel = GalleryServerPro.Web.WebsiteController.GetCurrentTrustLevel();

			// Get the application path so that the business layer (and any dependent layers) has access to it.
			string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

			// Get a few settings from the configuration file.
			Core coreConfig = GetGalleryServerProConfigSection().Core;
			string mediaObjectPath = coreConfig.MediaObjectPath;
			string thumbnailPath = (String.IsNullOrEmpty(coreConfig.ThumbnailPath) ? mediaObjectPath : coreConfig.ThumbnailPath);
			string optimizedPath = (String.IsNullOrEmpty(coreConfig.OptimizedPath) ? mediaObjectPath : coreConfig.OptimizedPath);

			// Get the application name from the membership provider.
			string appName = Membership.ApplicationName;

			// Pass these values to our global app settings instance, where the values can be used throughout the application.
			GalleryServerPro.Business.AppSetting.Instance.Initialize(
				trustLevel,
				physicalApplicationPath,
				mediaObjectPath,
				thumbnailPath,
				optimizedPath,
				appName);
		}

		#endregion
	}
}
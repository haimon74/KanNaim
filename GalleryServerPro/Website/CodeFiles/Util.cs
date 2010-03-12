using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Entity;
using GalleryServerPro.Web.Pages;

namespace GalleryServerPro.Web
{
	/// <summary>
	/// Contains general purpose routines useful for this website as well as a convenient
	/// gateway to functionality provided in other business layers.
	/// </summary>
	public class Util
	{
		#region Private Fields

		private int _mediaObjectId;
		private IAlbum _album;
		private IGalleryObject _mediaObject;

		#endregion

		#region Private Static Fields

		private static object sharedLock = new object();
		private static string _galleryRoot;
		private static string _jQueryPath;
		private static string _gspConfigFilePath;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Util"/> class.
		/// </summary>
		public Util()
		{
			_mediaObjectId = int.MinValue;
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Gets or sets the name of the current user. This property becomes available immediately after a user logs in, even within
		/// the current page's life cycle. This property is preferred over HttpContext.Current.User.Identity.Name, which does not
		/// contain the user's name until the next page load. This property should be set only when the user logs in. When the 
		/// property is not explicitly assigned, it automatically returns the value of HttpContext.Current.User.Identity.Name.
		/// </summary>
		/// <value>The name of the current user.</value>
		public static string UserName
		{
			get
			{
				object userName = HttpContext.Current.Items["UserName"];
				if (userName != null)
				{
					return userName.ToString();
				}
				else
				{
					return HttpContext.Current.User.Identity.Name;
				}
			}
			set { HttpContext.Current.Items["UserName"] = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the current user is authenticated. This property becomes true available immediately after 
		/// a user logs in, even within the current page's life cycle. This property is preferred over 
		/// HttpContext.Current.User.Identity.IsAuthenticated, which does not become true until the next page load. 
		/// This property should be set only when the user logs in. When the property is not explicitly assigned, it automatically 
		/// returns the value of HttpContext.Current.User.Identity.IsAuthenticated.
		/// </summary>
		public static bool IsAuthenticated
		{
			get
			{
				bool isAuthenticated;
				object objIsAuthenticated = HttpContext.Current.Items["IsAuthenticated"];

				if ((objIsAuthenticated != null) && Boolean.TryParse(objIsAuthenticated.ToString(), out isAuthenticated))
				{
					return isAuthenticated;
				}
				else
				{
					return HttpContext.Current.User.Identity.IsAuthenticated;
				}
			}
			set { HttpContext.Current.Items["IsAuthenticated"] = value; }
		}

		/// <summary>
		/// Gets the URL builder.
		/// </summary>
		/// <value>The URL builder.</value>
		static public IUrlBuilder UrlBuilder
		{
			get
			{
				if (HttpContext.Current.Application["gsp_UrlBuilder"] == null)
				{
					string urlAssembly;

					//if (IsRainbow)
					//{
					//  urlAssembly = "yaf_rainbow.RainbowUrlBuilder,yaf_rainbow";
					//}
					//else if (IsDotNetNuke)
					//{
					//  urlAssembly = "yaf_dnn.DotNetNukeUrlBuilder,yaf_dnn";
					//}
					//else if (IsPortal)
					//{
					//  urlAssembly = "Portal.UrlBuilder,Portal";
					//}
					//else if (EnableURLRewriting == "true")
					//{
					//  urlAssembly = "yaf.RewriteUrlBuilder,yaf";
					//}
					//else
					//{
					urlAssembly = "GalleryServerPro.Web.UrlBuilder,GalleryServerPro.Web";
					//}

					HttpContext.Current.Application["gsp_UrlBuilder"] = Activator.CreateInstance(Type.GetType(urlAssembly));
				}

				return (IUrlBuilder)HttpContext.Current.Application["gsp_UrlBuilder"];
			}
		}

		/// <summary>
		/// Get the path, relative to the web site root, to the directory containing the Gallery Server Pro user controls and 
		/// other resources. Does not include the containing page or the trailing slash. Example: If GSP is installed at 
		/// C:\inetpub\wwwroot\dev\gallery, where C:\inetpub\wwwroot\ is the parent web site, and the gallery support files are in 
		/// the gsp directory, this property returns /dev/gallery/gsp. Guaranteed to not return null.
		/// </summary>
		/// <value>Returns the path, relative to the web site root, to the directory containing the Gallery Server Pro user 
		/// controls and other resources.</value>
		public static string GalleryRoot
		{
			get
			{
				if (_galleryRoot == null)
				{
					_galleryRoot = CalculateGalleryRoot();
				}

				return _galleryRoot;
			}
		}

		/// <summary>
		/// Gets the fully qualified file path to galleryserverpro.config. Guaranteed to not return null.
		/// Example: C:\inetpub\wwwroot\gallery\gs\config\galleryserverpro.config
		/// </summary>
		/// <value>The fully qualified file path to galleryserverpro.config.</value>
		public static string GalleryServerProConfigFilePath
		{
			get
			{
				if (_gspConfigFilePath == null)
				{
					_gspConfigFilePath = CalculateGspConfigFilePath();
				}

				return _gspConfigFilePath;
			}
		}

		/// <summary>
		/// Gets the path to the jQuery javascript file. May be an absolute or relative URL, depending on how the jQueryScriptPath 
		/// property is specified in the configuration file. Absolute paths will be in URI form. Relative paths will typically
		/// be relative to the web site root (NOT the application root). This value is not verified to point to a valid location, 
		/// nor is it guranteed to be in the form described. If the user entered the path "C:\scripts\jquery.js" in the config 
		/// file, this property will return it, even though it is not valid. Guaranteed to not return null. 
		/// Examples: /dev/gsweb/gs/script/jquery-1.3.2.min.js, http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js
		/// </summary>
		/// <value>Returns the path to the jQuery javascript file.</value>
		public static string JQueryPath
		{
			get
			{
				if (_jQueryPath == null)
				{
					_jQueryPath = CalculateJQueryPath();
				}

				return _jQueryPath;
			}
		}

		/// <summary>
		/// Get the path, relative to the web site root, to the current web application. Does not include the containing page 
		/// or the trailing slash. Example: If GSP is installed at C:\inetpub\wwwroot\dev\gallery, and C:\inetpub\wwwroot\ is 
		/// the parent web site, this property returns /dev/gallery. Guaranteed to not return null.
		/// </summary>
		/// <value>Get the path, relative to the web site root, to the current web application.</value>
		public static string AppRoot
		{
			get
			{
				return HttpContext.Current.Request.ApplicationPath.TrimEnd(new char[] { '/' });
			}
		}

		/// <summary>
		/// Gets or sets the URI of the previous page the user was viewing. The value is stored in the user's session, and 
		/// can be used after a user has completed a task to return to the original page. If the Session object is not available,
		/// no value is saved in the setter and a null is returned in the getter.
		/// </summary>
		/// <value>The URI of the previous page the user was viewing.</value>
		public static Uri PreviousUri
		{
			get
			{
				if (HttpContext.Current.Session != null)
					return (Uri)HttpContext.Current.Session["ReferringUrl"];
				else
					return null;
			}
			set
			{
				if (HttpContext.Current.Session == null)
					return; // Session is disabled for this page.

				HttpContext.Current.Session["ReferringUrl"] = value;
			}
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
				this._album = Factory.LoadAlbumInstance(GetQueryStringParameterInt32("aid"), false);

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
		/// Guaranteed to not return null.
		/// </summary>
		/// <returns>Returns a IGalleryObject object that represents the relevant derived media object type
		/// (e.g. Image, Video, etc). Note that an album is not considered a media object type, so it will never be
		/// returned.</returns>
		public IGalleryObject GetMediaObject()
		{
			if (this._mediaObject == null)
			{
				IGalleryObject tempMediaObject = new Business.NullObjects.NullGalleryObject();

				try
				{
					tempMediaObject = Factory.LoadMediaObjectInstance(this.MediaObjectId);
				}
				catch (ArgumentException)
				{
					Redirect(AddQueryStringParameter(GetCurrentPageUrl(), "msg=" + (int)Message.MediaObjectDoesNotExist));
				}
				catch (GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException)
				{
					Redirect(AddQueryStringParameter(GetCurrentPageUrl(), "msg=" + (int)Message.MediaObjectDoesNotExist));
				}

				this._mediaObject = tempMediaObject;
			}

			return this._mediaObject;
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
		public static bool IsUserAuthorized(SecurityActions securityActions, IGalleryServerRoleCollection roles, int albumId, bool isPrivate)
		{
			return SecurityManager.IsUserAuthorized(securityActions, roles, albumId, Util.IsAuthenticated, isPrivate);
		}

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Determine the trust level of the currently running application.
		/// </summary>
		/// <returns>Returns the trust level of the currently running application.</returns>
		public static ApplicationTrustLevel GetCurrentTrustLevel()
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

			ApplicationTrustLevel trustLevel = ApplicationTrustLevel.None;

			switch (aspnetTrustLevel)
			{
				case AspNetHostingPermissionLevel.Minimal: trustLevel = ApplicationTrustLevel.Minimal; break;
				case AspNetHostingPermissionLevel.Low: trustLevel = ApplicationTrustLevel.Low; break;
				case AspNetHostingPermissionLevel.Medium: trustLevel = ApplicationTrustLevel.Medium; break;
				case AspNetHostingPermissionLevel.High: trustLevel = ApplicationTrustLevel.High; break;
				case AspNetHostingPermissionLevel.Unrestricted: trustLevel = ApplicationTrustLevel.Full; break;
				default: trustLevel = ApplicationTrustLevel.Unknown; break;
			}

			return trustLevel;
		}

		/// <summary>
		/// Get the path, relative to the web site root, to the specified resource. Example: If the web application is at
		/// /dev/gsweb/, the directory containing the resources is /gs/, and the desired resource is /images/info.gif, this function
		/// will return /dev/gsweb/gs/images/info.gif.
		/// </summary>
		/// <param name="resource">A path relative to the directory containing the Gallery Server Pro resource files (ex: images/info.gif).
		/// The leading forward slash ('/') is optional.</param>
		/// <returns>Returns the path, relative to the web site root, to the specified resource.</returns>
		public static string GetUrl(string resource)
		{
			if (!resource.StartsWith("/"))
				resource = resource.Insert(0, "/"); // Make sure it starts with a '/'

			resource = String.Concat(GalleryRoot, resource);

			//#if DEBUG
			//      if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(resource)))
			//        throw new System.IO.FileNotFoundException(String.Format("No file exists at {0}.", resource), resource);
			//#endif

			return resource;
		}

		/// <overloads>Get an URL relative to the application root for the requested page.</overloads>
		/// <summary>
		/// Get an URL relative to the application root for the requested <paramref name="page"/>. Example: If 
		/// <paramref name="page"/> is PageId.album and the current page is /dev/gs/gallery.aspx, this function 
		/// returns /dev/gs/gallery.aspx?g=album.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		/// <returns>Returns an URL relative to the application root for the requested <paramref name="page"/>.</returns>
		public static string GetUrl(PageId page)
		{
			return Util.UrlBuilder.BuildUrl(String.Concat("g=", page));
		}

		/// <summary>
		/// Get an URL relative to the application root for the requested <paramref name="page"/> and with the specified 
		/// <paramref name="args"/> appended as query string parameters. Example: If <paramref name="page"/> is PageId.task_addobjects, 
		/// the current page is /dev/gs/gallery.aspx, <paramref name="format"/> is "aid={0}", and <paramref name="args"/>
		/// is "23", this function returns /dev/gs/gallery.aspx?g=task_addobjects&amp;aid=23. If the <paramref name="page"/> is
		/// <see cref="PageId.album"/> or <see cref="PageId.mediaobject"/>, don't include the "g" query string parameter, since 
		/// we can deduce it by looking for the aid or moid query string parms.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		/// <param name="format">A format string whose placeholders are replaced by values in <paramref name="args"/>. Do not use a '?'
		/// or '&amp;' at the beginning of the format string. Example: "msg={0}".</param>
		/// <param name="args">The values to be inserted into the <paramref name="format"/> string.</param>
		/// <returns>Returns an URL relative to the application root for the requested <paramref name="page"/></returns>
		public static string GetUrl(PageId page, string format, params object[] args)
		{
			if ((page == PageId.album) || (page == PageId.mediaobject))
			{
				// Don't use the "g" parameter for album or mediaobject pages, since we can deduce it by looking for the 
				// aid or moid query string parms. This results in a shorter, cleaner URL.
				return Util.UrlBuilder.BuildUrl(string.Format(format, args));
			}
			else
				return Util.UrlBuilder.BuildUrl(String.Concat("g=", page, "&", string.Format(format, args)));
		}

		/// <summary>
		/// Get the physical path to the <paramref name="resource"/>. Example: If the web application is at
		/// C:\inetpub\wwwroot\dev\gsweb\, the directory containing the resources is \gs\, and the desired resource is
		/// /templates/AdminNotificationAccountCreated.txt, this function will return 
		/// C:\inetpub\wwwroot\dev\gsweb\gs\templates\AdminNotificationAccountCreated.txt.
		/// </summary>
		/// <param name="resource">A path relative to the directory containing the Gallery Server Pro resource files (ex: images/info.gif).
		/// The slash may be forward (/) or backward (\), although there is a slight performance improvement if it is forward (/).
		/// The parameter does not require a leading slash, although there is a slight performance improvement if it is present.</param>
		/// <returns>Returns the physical path to the requested <paramref name="resource"/>.</returns>
		public static string GetPath(string resource)
		{
			// Convert back slash (\) to forward slash, if present.
			resource = resource.Replace(Path.DirectorySeparatorChar, '/');

			return HttpContext.Current.Server.MapPath(GetUrl(resource));
		}

		/// <summary>
		/// Gets the URL, relative to the application root and without any query string parameters, to the current page.
		/// This method is a wrapper for a call to HttpContext.Current.Request.FilePath. Example: /dev/gs/gallery.aspx
		/// </summary>
		/// <returns>Returns the URL, relative to the application root and without any query string parameters, to the current page.</returns>
		public static string GetCurrentPageUrl()
		{
			return GetCurrentPageUrl(false);
		}

		/// <summary>
		/// Gets the URL, relative to the application root and including any query string parameters, to the current page.
		/// This method is a wrapper for a call to HttpContext.Current.Request.Url.PathAndQuery. Example: /dev/gs/gallery.aspx?g=admin_email&aid=2389
		/// </summary>
		/// <param name="includeQueryString">When <c>true</c> the query string is included.</param>
		/// <returns>
		/// Returns the URL, relative to the application root and including any query string parameters, to the current page.
		/// </returns>
		public static string GetCurrentPageUrl(bool includeQueryString)
		{
			if (includeQueryString)
				return HttpContext.Current.Request.Url.PathAndQuery;
			else
				return HttpContext.Current.Request.FilePath;
		}

		/// <summary>
		/// Get the full path to the current web page. Does not include any query string parms. Ex: http://www.techinfosystems.com/gs/default.aspx
		/// </summary>
		/// <returns>Returns the full path to the current web page.</returns>
		/// <remarks>This value is retrieved from the user's session. If not present in the session, such as when the user first arrives, it
		/// is calculated by passing the appropriate pieces from HttpContext.Current.Request.Url and HttpContext.Current.Request.FilePath
		/// to a UriBuilder object. The path is calculated on a per-user basis because the URL may be different for different users 
		/// (a local admin's URL may be http://localhost/gs/default.aspx, someone on the intranet may get the server's name
		/// (http://Server1/gs/default.aspx), and someone on the internet may get the full name (http://www.bob.com/gs/default.aspx).</remarks>
		public static string GetCurrentPageUrlFull()
		{
			string appRootUrl = null;

			if (HttpContext.Current.Session != null)
			{
				object appRootUrlSession = HttpContext.Current.Session["PageUrl"];
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

				uriBuilder.Path = GetCurrentPageUrl();

				appRootUrl = uriBuilder.ToString();

				if (HttpContext.Current.Session != null)
					HttpContext.Current.Session["PageUrl"] = appRootUrl;
			}

			return appRootUrl;
		}

		/// <summary>
		/// Get the URI scheme, DNS host name or IP address, and port number for the current application. 
		/// Examples: http://www.site.com, http://localhost, http://127.0.0.1, http://godzilla
		/// </summary>
		/// <returns>Returns the URI scheme, DNS host name or IP address, and port number for the current application.</returns>
		/// <remarks>This value is retrieved from the user's session. If not present in the session, such as when the user first arrives, it
		/// is calculated by parsing the appropriate pieces from HttpContext.Current.Request.Url to a UriBuilder object. The path is 
		/// calculated on a per-user basis because the URL may be different for different users (a local admin's URL may be 
		/// http://localhost, someone on the intranet may get the server's name (http://Server1), and someone on the internet may get 
		/// the full name (http://www.bob.com).</remarks>
		public static string GetHostUrl()
		{
			string hostUrl = null;

			if (HttpContext.Current.Session != null)
				hostUrl = (String)HttpContext.Current.Session["HostUrl"];

			if (String.IsNullOrEmpty(hostUrl))
			{
				Uri uri = HttpContext.Current.Request.Url;
				hostUrl = String.Concat(uri.Scheme, "://", uri.Authority);

				if (HttpContext.Current.Session != null)
					HttpContext.Current.Session["HostUrl"] = hostUrl;
			}

			return hostUrl;
		}

		/// <overloads>Redirects the user to the specified <paramref name="page"/>.</overloads>
		/// <summary>
		/// Redirects the user to the specified <paramref name="page"/>. The redirect occurs immediately.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		public static void Redirect(PageId page)
		{
			HttpContext.Current.Response.Redirect(GetUrl(page), true);
		}

		/// <summary>
		/// Redirects the user, using Response.Redirect, to the specified <paramref name="page"/>. If <paramref name="endResponse"/> is true, the redirect occurs 
		/// when the page has finished processing all events. When false, the redirect occurs immediately.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		/// <param name="endResponse">When <c>true</c> the redirect occurs immediately. When false, the redirect is delayed until the
		/// page processing is complete.</param>
		public static void Redirect(PageId page, bool endResponse)
		{
			HttpContext.Current.Response.Redirect(GetUrl(page), endResponse);
		}

		/// <summary>
		/// Redirects the user, using Response.Redirect, to the specified <paramref name="page"/> and with the specified 
		/// <paramref name="args"/> appended as query string parameters. Example: If <paramref name="page"/> is PageId.album, 
		/// the current page is /dev/gs/gallery.aspx, <paramref name="format"/> is "aid={0}", and <paramref name="args"/>
		/// is "23", this function redirects to /dev/gs/gallery.aspx?g=album&amp;aid=23.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		/// <param name="format">A format string whose placeholders are replaced by values in <paramref name="args"/>. Do not use a '?'
		/// or '&amp;' at the beginning of the format string. Example: "msg={0}".</param>
		/// <param name="args">The values to be inserted into the <paramref name="format"/> string.</param>
		public static void Redirect(PageId page, string format, params object[] args)
		{
			HttpContext.Current.Response.Redirect(GetUrl(page, format, args), true);
		}

		/// <summary>
		/// Redirects the user, using Response.Redirect, to the specified <paramref name="url"/>
		/// </summary>
		/// <param name="url">The URL to redirect the user to.</param>
		public static void Redirect(string url)
		{
			HttpContext.Current.Response.Redirect(url, true);
		}

		/// <summary>
		/// Transfers the user, using Server.Transfer, to the specified <paramref name="page"/>.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		public static void Transfer(PageId page)
		{
			try
			{
				HttpContext.Current.Server.Transfer(GetUrl(page));
			}
			catch (System.Threading.ThreadAbortException) { }
		}

		/// <summary>
		/// Redirects the user to the specified <paramref name="page"/> and with the specified 
		/// <paramref name="args"/> appended as query string parameters. Example: If <paramref name="page"/> is PageId.album, 
		/// the current page is /dev/gs/gallery.aspx, <paramref name="format"/> is "aid={0}", and <paramref name="args"/>
		/// is "23", this function redirects to /dev/gs/gallery.aspx?g=album&amp;aid=23.
		/// </summary>
		/// <param name="page">A <see cref="PageId"/> enumeration that represents the desired <see cref="GalleryPage"/>.</param>
		/// <param name="endResponse">When <c>true</c> the redirect occurs immediately. When false, the redirect is delayed until the
		/// page processing is complete.</param>
		/// <param name="format">A format string whose placeholders are replaced by values in <paramref name="args"/>. Do not use a '?'
		/// or '&amp;' at the beginning of the format string. Example: "msg={0}".</param>
		/// <param name="args">The values to be inserted into the <paramref name="format"/> string.</param>
		public static void Redirect(PageId page, bool endResponse, string format, params object[] args)
		{
			HttpContext.Current.Response.Redirect(GetUrl(page, format, args), endResponse);
			HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		/// <summary>
		/// Gets detailed information about the <paramref name="ex"/> that can be presented to an administrator. This is essentially 
		/// a string that combines the exception type with its message. It recursively checks for an InnerException and appends that 
		/// type and message if present. It does not include stack trace or other information. Callers to this method should ensure 
		/// that this information is shown to the user only if he or she is a system administrator and/or the ShowErrorDetails setting 
		/// of the configuration file to true.
		/// </summary>
		/// <param name="ex">The exception for which detailed information is to be returned.</param>
		/// <returns>Returns detailed information about the <paramref name="ex"/> that can be presented to an administrator.</returns>
		public static string GetExceptionDetails(Exception ex)
		{
			string exMsg = String.Concat(ex.GetType(), ": ", ex.Message);
			Exception innerException = ex.InnerException;
			while (innerException != null)
			{
				exMsg += String.Concat(" ", innerException.GetType(), ": ", innerException.Message);
				innerException = innerException.InnerException;
			}

			return exMsg;
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

			if ((String.IsNullOrEmpty(parm)) || (!HelperFunctions.IsInt32(parm) || (Convert.ToInt32(parm, CultureInfo.InvariantCulture) <= 0)))
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

			if ((String.IsNullOrEmpty(parm)) || (!HelperFunctions.IsInt32(parm) || (Convert.ToInt32(parm, CultureInfo.InvariantCulture) <= 0)))
			{
				return int.MinValue;
			}
			else
			{
				return Convert.ToInt32(parm, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Retrieves the specified query string parameter value from the query string. Returns string.Empty 
		/// if the parameter is not found.
		/// </summary>
		/// <param name="parameterName">The name of the query string parameter for which to retrieve it's value.</param>
		/// <returns>Returns the value of the specified query string parameter.</returns>
		/// <remarks>Do not call UrlDecode on the string, as it appears that .NET already does this.</remarks>
		public static string GetQueryStringParameterString(string parameterName)
		{
			object parm = HttpContext.Current.Request.QueryString[parameterName];

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
		/// Retrieves the specified query string parameter value from the specified <paramref name="uri"/>. Returns 
		/// string.Empty if the parameter is not found.
		/// </summary>
		/// <param name="uri">The URI to search.</param>
		/// <param name="parameterName">The name of the query string parameter for which to retrieve it's value.</param>
		/// <returns>Returns the value of the specified query string parameter found in the <paramref name="uri"/>.</returns>
		public static string GetQueryStringParameterString(Uri uri, string parameterName)
		{
			string parm = null;
			if (uri == null)
			{
				parm = HttpContext.Current.Request.QueryString[parameterName];
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

			if (parm == null)
			{
				return string.Empty;
			}
			else
			{
				return parm;
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

		/// <overloads>
		/// Gets the value that identifies the type of gallery page.
		/// </overloads>
		/// <summary>
		/// Gets the value that identifies the type of gallery page that is currently being displayed. This value is 
		/// retrieved from the "g" query string parameter. If the parameter is not present, the query string is searched
		/// for a "moid" parameter. If "moid" is found, the page is <see cref="Web.PageId.mediaobject"/>. If
		/// not found, the page is <see cref="Web.PageId.album"/>.
		/// </summary>
		/// <returns>Returns the value that identifies the type of gallery page that is currently being displayed.</returns>
		public static PageId GetPage()
		{
			PageId page;
			string requestedPage = Util.GetQueryStringParameterString("g");

			if (String.IsNullOrEmpty(requestedPage))
			{
				// No 'g' query string parm. Look for 'moid' parameter, which might be present without the 'g' parm. Default
				// to album if 'moid' parameter is missing.
				if (Util.GetQueryStringParameterInt32("moid") > int.MinValue)
					page = PageId.mediaobject;
				else
					page = PageId.album;
			}
			else
			{
				try
				{
					page = (PageId)System.Enum.Parse(typeof(PageId), requestedPage, true);
				}
				catch (ArgumentException)
				{
					page = PageId.album;
				}
			}
			return page;
		}

		/// <summary>
		/// Gets the value that identifies the type of gallery page that is specified in the <paramref name="uri"/>. This value is 
		/// retrieved from the "g" query string parameter. If the parameter is not present, the query string is searched
		/// for a "moid" parameter. If "moid" is found, the page is <see cref="Web.PageId.mediaobject"/>. If
		/// not found, the page is <see cref="Web.PageId.album"/>.
		/// </summary>
		/// <returns>Returns the value that identifies the type of gallery page that is currently being displayed.</returns>
		public static PageId GetPage(Uri uri)
		{
			PageId page;
			string requestedPage = Util.GetQueryStringParameterString(uri, "g");

			if (String.IsNullOrEmpty(requestedPage))
			{
				// No 'g' query string parm. Look for 'moid' parameter, which might be present without the 'g' parm. Default
				// to album if 'moid' parameter is missing.
				if (Util.GetQueryStringParameterInt32(uri, "moid") > int.MinValue)
					page = PageId.mediaobject;
				else
					page = PageId.album;
			}
			else
			{
				try
				{
					page = (PageId)System.Enum.Parse(typeof(PageId), requestedPage, true);
				}
				catch (ArgumentException)
				{
					page = PageId.album;
				}
			}
			return page;
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
		/// Returns a value indicating whether the specified query string parameter name is part of the query string. 
		/// </summary>
		/// <param name="parameterName">The name of the query string parameter to check for.</param>
		/// <returns>Returns true if the specified query string parameter value is part of the query string; otherwise 
		/// returns false. </returns>
		public static bool IsQueryStringParameterPresent(string parameterName)
		{
			return (System.Web.HttpContext.Current.Request.QueryString[parameterName] != null);
		}

		/// <summary>
		/// Returns a value indicating whether the specified query string parameter name is part of the query string
		/// of the <paramref name="uri"/>. 
		/// </summary>
		/// <param name="uri">The URI to check for the present of the <paramref name="parameterName">query string parameter name</paramref>.</param>
		/// <param name="parameterName">Name of the query string parameter.</param>
		/// <returns>Returns true if the specified query string parameter value is part of the query string; otherwise 
		/// returns false. </returns>
		public static bool IsQueryStringParameterPresent(Uri uri, string parameterName)
		{
			return (uri.Query.Contains("?" + parameterName + "=") || uri.Query.Contains("&" + parameterName + "="));
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
			return HtmlValidator.RemoveHtml(html, escapeQuotes);
		}

		/// <summary>
		/// Removes or makes harmless potentially dangerous HTML and Javascript in <paramref name="html"/>. If the configuration
		/// setting allowHtmlInTitlesAndCaptions is true, then the input is "cleaned" so that all HTML tags that are not in a 
		/// predefined list are HTML-encoded and invalid HTML attributes are deleted. If allowHtmlInTitlesAndCaptions is false, 
		/// then all HTML tags are encoded. If the setting allowUserEnteredJavascript is true, then script tags and the text "javascript:"
		/// is allowed. Note that if script is not in the list of valid HTML tags defined in allowedHtmlTags, it will be encoded even when
		/// allowUserEnteredJavascript is true. When the setting is false, all script tags are encoded and instances of the
		/// text "javascript:" are deleted.
		/// </summary>
		/// <param name="html">The string containing the HTML tags.</param>
		/// <returns>Returns a string with potentially dangerous HTML tags HTML-encoded.</returns>
		public static string CleanHtmlTags(string html)
		{
			return HtmlValidator.Clean(html);
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
		/// <remarks>This is the only method, apart from those invoked through web services, that are not handled by the global error
		/// handling routine in Gallery.cs. This method wraps its calls in a try..catch that passes any exceptions to
		/// <see cref="AppErrorController.HandleGalleryException"/>. If that method does not transfer the user to a friendly error page, the exception
		/// is re-thrown.</remarks>
		public static void InitializeApplication()
		{
			lock (sharedLock)
			{
				if (AppSetting.Instance.IsInitialized)
					return;

				try
				{
					// Set application key so ComponentArt knows it is properly licensed.
					HttpContext.Current.Application["ComponentArtWebUI_AppKey"] =
						"This edition of ComponentArt Web.UI is licensed for Gallery Server Pro application only.";

					// Add a dummy value to session so that the session ID remains constant. (This is required by Util.GetRolesForUser())
					// Check for null session first. It will be null when this is triggered by a web method that does not have
					// session enabled (that is, the [WebMethod(EnableSession = true)] attribute). That's OK because the roles functionality
					// will still work (we might have to an extra data call, though), and we don't want the overhead of session for some web methods.
					if (HttpContext.Current.Session != null)
						HttpContext.Current.Session.Add("1", "1");

					// Set web-related variables in the business layer and initialize the data store.
					InitializeBusinessLayer();

					// Delete anonymous accounts, if they exist, to minimize the database clutter.
					UserController.DeleteAnonymousUsers();

					// Validate users, roles and profiles.
					UserController.ValidateMembership();

					// Make sure installation has its own unique encryption key.
					ValidateEncryptionKey();
				}
				catch (System.Threading.ThreadAbortException) { }
				catch (Exception ex)
				{
					// Let the error handler deal with it. It will decide whether to transfer the user to a friendly error page.
					// If the function returns, that means it didn't redirect, so we should re-throw the exception.
					AppErrorController.HandleGalleryException(ex);
					throw;
				}
			}
		}

		/// <summary>
		/// Generates a pseudo-random 24 character string that can be as an encryption key.
		/// </summary>
		/// <returns>A pseudo-random 24 character string that can be as an encryption key.</returns>
		public static string GenerateNewEncryptionKey()
		{
			const int encryptionKeyLength = 24;
			const int numberOfNonAlphaNumericCharactersInEncryptionKey = 3;
			string encryptionKey = Membership.GeneratePassword(encryptionKeyLength, numberOfNonAlphaNumericCharactersInEncryptionKey);

			// An ampersand (&) is invalid, since it is used as an escape character in XML files. Replace any instances with an 'X'.
			return encryptionKey.Replace("&", "X");
		}

		/// <summary>
		/// HtmlEncodes a string using System.Web.HttpUtility.HtmlEncode().
		/// </summary>
		/// <param name="html">The text to HTML encode.</param>
		/// <returns>Returns <paramref name="html"/> as an HTML-encoded string.</returns>
		public static string HtmlEncode(string html)
		{
			return HttpUtility.HtmlEncode(html);
		}

		/// <summary>
		/// HtmlDecodes a string using System.Web.HttpUtility.HtmlDecode().
		/// </summary>
		/// <param name="html">The text to HTML decode.</param>
		/// <returns>Returns <paramref name="html"/> as an HTML-decoded string.</returns>
		public static string HtmlDecode(string html)
		{
			return HttpUtility.HtmlDecode(html);
		}

		/// <summary>
		/// UrlEncodes a string using System.Uri.EscapeDataString().
		/// </summary>
		/// <param name="text">The text to URL encode.</param>
		/// <returns>Returns <paramref name="text"/> as an URL-encoded string.</returns>
		public static string UrlEncode(string text)
		{
			return Uri.EscapeDataString(text);
		}

		/// <summary>
		/// UrlDecodes a string using System.Uri.UnescapeDataString().
		/// </summary>
		/// <param name="text">The text to URL decode.</param>
		/// <returns>Returns text as an URL-decoded string.</returns>
		public static string UrlDecode(string text)
		{
			// Pre-process for + sign space formatting since System.Uri doesn't handle it
			// plus literals are encoded as %2b normally so this should be safe.
			text = text.Replace("+", " ");
			return Uri.UnescapeDataString(text);
		}

		/// <summary>
		/// Force the current application to recycle by updating the last modified timestamp on galleryserverpro.config.
		/// </summary>
		public static void ForceAppRecycle()
		{
			FileInfo fi = new FileInfo(GalleryServerProConfigFilePath);
			fi.LastWriteTimeUtc = DateTime.UtcNow;
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Set up the business layer with information about this web application, such as its trust level and a few settings
		/// from the configuration file.
		/// </summary>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.CannotWriteToDirectoryException">
		/// Thrown when Gallery Server Pro is unable to write to, or delete from, the media objects directory.</exception>
		private static void InitializeBusinessLayer()
		{
			// Determine the trust level this web application is running in and set to a global variable. This will be used 
			// throughout the application to gracefully degrade when we are not at Full trust.
			GalleryServerPro.Business.ApplicationTrustLevel trustLevel = Util.GetCurrentTrustLevel();

			// Get the application path so that the business layer (and any dependent layers) has access to it.
			string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

			// Pass these values to our global app settings instance, where the values can be used throughout the application.
			AppSetting.Instance.Initialize(
				trustLevel,
				physicalApplicationPath,
				Constants.APP_NAME);
		}

		/// <summary>
		/// Calculates the path, relative to the web site root, to the directory containing the Gallery Server Pro user 
		/// controls and other resources. Does not include the default page or the trailing slash. Ex: /dev/gsweb/gsp
		/// </summary>
		/// <returns>Returns the path to the directory containing the Gallery Server Pro user controls and other resources.</returns>
		private static string CalculateGalleryRoot()
		{
			string appPath = AppRoot;
			string galleryPath = GetGalleryPathFromWebConfig();

			if (!String.IsNullOrEmpty(galleryPath))
			{
				galleryPath = galleryPath.Replace("\\", "/");

				if (!galleryPath.StartsWith("/"))
					galleryPath = String.Concat("/", galleryPath); // Make sure it starts with a '/'

				appPath = String.Concat(appPath, galleryPath);
			}

			return appPath;
		}

		/// <summary>
		/// Calculates the path to the jQuery javascript file. If the value in the jQueryScriptPath configuration setting starts
		/// with "http", it is returned without further manipulation. Otherwise, it is scrubbed by replacing any backward slashes
		/// ("\") with forward slashes ("/"), verifying that it begins with a leading forward slash, and modifying it to be 
		/// a URI relative to the web site root. Examples: /dev/gsweb/gs/script/jquery-1.3.2.min.js,
		/// http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js
		/// </summary>
		/// <returns>Returns the path to the jQuery javascript file.</returns>
		private static string CalculateJQueryPath()
		{
			string jQueryScriptPath = Config.GetCore().JQueryScriptPath;

			if (jQueryScriptPath.StartsWith("http"))
			{
				return jQueryScriptPath;
			}

			jQueryScriptPath = jQueryScriptPath.Replace(Path.DirectorySeparatorChar.ToString(), "/");

			if (!jQueryScriptPath.StartsWith("/"))
			{
				jQueryScriptPath = String.Concat("/", jQueryScriptPath);
			}

			if (File.Exists(GetPath(jQueryScriptPath)))
			{
				jQueryScriptPath = GetUrl(jQueryScriptPath);
			}

			return jQueryScriptPath;
		}

		/// <summary>
		/// Calculates the fully qualified file path to galleryserverpro.config. Guaranteed to not return null. This function
		/// assumes the file is located in a directory named config located within the Gallery Server Pro resource directory
		/// (see the <see cref="GalleryRoot"/> property). This function throws a <see cref="FileNotFoundException"/> exception
		/// if the file is not in the expected location.
		/// Example: C:\inetpub\wwwroot\gallery\gs\config\galleryserverpro.config
		/// </summary>
		/// <returns>The fully qualified file path to galleryserverpro.config.</returns>
		/// <exception cref="FileNotFoundException">Thrown when galleryserverpro.config is not in the expected location.</exception>
		private static string CalculateGspConfigFilePath()
		{
			string gspPath = HttpContext.Current.Server.MapPath(Util.GetUrl("/config/galleryserverpro.config"));

			if (!File.Exists(gspPath))
			{
				throw new FileNotFoundException(string.Format("Could not find galleryserverpro.config at {0}.", Util.GetUrl("/config/galleryserverpro.config")));
			}

			return gspPath;
		}

		/// <summary>
		/// Gets the path, relative to the current application, to the directory containing the Gallery Server Pro
		/// resources such as images, user controls, scripts, etc. The value is calculated based on the path to the
		/// galleryserverpro.config file specified in web.config. For example, if the config file is at "gs\config\galleryserverpro.config",
		/// then the path to the resources is "gs".
		/// </summary>
		/// <returns>Returns the path, relative to the current application, to the directory containing the Gallery Server Pro
		/// resources such as images, user controls, scripts, etc.</returns>
		/// <remarks>This method assumes that galleryserverpro.config is in a directory named "config" and that it is at
		/// the same directory level as the other folders, such as controls, handler, images, pages, script, etc. This
		/// assumption will be valid as long as Gallery Server Pro is always deployed with the entire contents of the "gs"
		/// directory as a single block.</remarks>
		private static string GetGalleryPathFromWebConfig()
		{
			string galleryServerProConfigPath = String.Empty;

			// Search web.config for <galleryServerPro configSource="..." />
			using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/web.config"), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader sr = new StreamReader(fs))
				{
					System.Xml.XmlReader r = System.Xml.XmlReader.Create(sr);
					while (r.Read())
					{
						if ((r.Name == "galleryServerPro") && r.MoveToAttribute("configSource"))
						{
							galleryServerProConfigPath = r.Value; // "gs\config\galleryserverpro.config"
							break;
						}
					}
				}
			}

			if (String.IsNullOrEmpty(galleryServerProConfigPath))
				throw new ErrorHandler.CustomExceptions.WebException("The web.config file for this application does not contain a <galleryServerPro ...> configuration element. This is required for Gallery Server Pro.");

			const string gallerySubPath = @"config\galleryserverpro.config";
			if (!galleryServerProConfigPath.EndsWith(gallerySubPath))
				throw new ErrorHandler.CustomExceptions.WebException(String.Format("The configuration file galleryserverpro.config must reside in a directory named config. The path discovered in web.config was {0}.", galleryServerProConfigPath));

			// Remove the "\config\galleryserverpro.config" from the path, so we are left with, for example, "gs".
			return galleryServerProConfigPath.Remove(galleryServerProConfigPath.IndexOf(gallerySubPath)).TrimEnd(new char[] { System.IO.Path.DirectorySeparatorChar });
		}

		/// <summary>
		/// Verify that the encryption key in galleryserverpro.config has been changed from its original, default value. The key is 
		/// updated with a new value if required. Each installation should have a unique key.
		/// </summary>
		private static void ValidateEncryptionKey()
		{
			if (Config.GetCore().EncryptionKey == Constants.ENCRYPTION_KEY)
			{
				GspCoreEntity core = GspConfigController.GetGspCoreEntity(Config.GetCore());
				string key = GenerateNewEncryptionKey();
				core.encryptionKey = key;
 
				try
				{
					GspConfigController.SaveCore(core);
				}
				catch (UnauthorizedAccessException ex)
				{
					if (!ex.Data.Contains("config"))
						ex.Data.Add("config", String.Format(CultureInfo.CurrentCulture, "Cannot save new encryption key to galleryserverpro.config. Give the IIS application pool identity write access to this file or manually update the encryptionKey attribute to {0}.", key));

					AppErrorController.LogError(ex);
				}
			}
		}

		#endregion
	}
}

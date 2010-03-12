using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.SessionState;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.handler
{
	/// <summary>
	/// Defines a handler that sends the specified media object to the output stream.
	/// </summary>
	[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
	[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]
	public class getmediaobject : IHttpHandler, IReadOnlySessionState
	{
		HttpContext _context;
		int _mediaObjectId;
		int _albumId;
		string _filepath;
		string _filename;
		bool _isPrivate;
		MimeTypeCategory _mimeTypeCategory;
		DisplayObjectType _displayType;
		bool _isUserAuthenticated;

		public void ProcessRequest(HttpContext context)
		{
			// Send the specified media object to the output stream.
			// Expected format:
			// /dev/gs/handler/getmediaobject.ashx?moid=34&aid=8&mo=C%3A%5Cgs%5Cmypics%5Cbirthday.jpeg&mtc=1&dt=1&isp=false
			// OR getmediaobject.ashx?moid=101&aid=7&mo=C:\gs\mypics\birthday.jpg&mtc=1&dt=2&isp=false
			// moid: The media object ID. Is int.MinValue if object is an album.
			// aid: The album ID for the album containing the media object, or for the album if the object is an album.
			// mo: The absolute path to the media object, including the object's name.
			// mtc: The mime type category. It is an integer that can be converted to the MimeTypeCategory enum.
			// dt: The display type. It is an integer that maps to the enum GalleryServerPro.Business.DisplayObjectType.
			// (0=Unknown,1=Thumbnail,2=Optimized,3=Original) At present all values other than 3 (Original) are ignored. If 3,
			// a security check is done to make sure user is authorized to view original images.
			// If URL encryption is enabled (encryptMediaObjectUrlOnClient = true in galleryserverpro.config), then the entire query
			// string portion is encrypted.
			if (!AppSetting.Instance.IsInitialized)
			{
				WebsiteController.InitializeApplication();
			}

			if (InitializeVariables(context) && IsUserAuthorized())
			{
				ShowMediaObject();
			}
			else
				this._context.Response.End();
		}

		/// <summary>
		/// Initialize the class level variables with information from the query string. Returns false if the variables cannot 
		/// be properly initialized.
		/// </summary>
		/// <param name="context">The HttpContext for the current request.</param>
		/// <returns>Returns true if all variables were initialized; returns false if there was a problem and one or more variables
		/// could not be set.</returns>
		private bool InitializeVariables(HttpContext context)
		{
			this._context = context;

			if (!ExtractQueryStringParms(context.Request.Url.Query))
				return false;

			this._isUserAuthenticated = context.User.Identity.IsAuthenticated;
			this._filename = Path.GetFileName(this._filepath);

			if ((_albumId > 0) &&
				(!String.IsNullOrEmpty(_filepath)) &&
				(!String.IsNullOrEmpty(_filename)) &&
				(MimeTypeEnumHelper.IsValidMimeTypeCategory(this._mimeTypeCategory)) &&
				(DisplayObjectTypeEnumHelper.IsValidDisplayObjectType(this._displayType)))
			{
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Extract information from the query string and assign to our class level variables. Return false if something goes wrong
		/// and the variables cannot be set. This will happen when the query string is in an unexpected format.
		/// </summary>
		/// <param name="queryString">The query string for the current request. Can be populated with HttpContext.Request.Url.Query.
		/// Must start with a question mark (?).</param>
		/// <returns>Returns true if all relevant variables were assigned from the query string; returns false if there was a problem.</returns>
		private bool ExtractQueryStringParms(string queryString)
		{
			if (String.IsNullOrEmpty(queryString)) return false;

			queryString = queryString.Remove(0, 1); // Strip off the ?

			bool filepathIsEncrypted = WebsiteController.GetGalleryServerProConfigSection().Core.EncryptMediaObjectUrlOnClient;
			if (filepathIsEncrypted)
			{
				// Decode, then decrypt the query string. Note that we must replace spaces with a '+'. This is required when the the URL is
				// used in javascript to create the Silverlight media player. Apparently, Silverlight or the media player javascript decodes
				// the query string when it requests the URL, so that means any instances of '%2b' are decoded into '+' before it gets here.
				// Ideally, we wouldn't even call UrlDecode in this case, but we don't have a way of knowing that it has already been decoded.
				// So we decode anyway, which doesn't cause any harm *except* it converts '+' to a space, so we need to convert them back.
				queryString = HelperFunctions.Decrypt(HttpUtility.UrlDecode(queryString).Replace(" ", "+"));
			}

			//moid={0}&aid={1}&mo={2}&mtc={3}&dt={4}&isp={5}
			foreach (string nameValuePair in queryString.Split(new char[] { '&' }))
			{
				string[] nameOrValue = nameValuePair.Split(new char[] { '=' });
				switch (nameOrValue[0])
				{
					case "moid":
						{
							int moid;
							if (Int32.TryParse(nameOrValue[1], out moid))
								_mediaObjectId = moid;
							else
								return false;
							break;
						}
					case "aid":
						{
							int aid;
							if (Int32.TryParse(nameOrValue[1], out aid))
								_albumId = aid;
							else
								return false;
							break;
						}
					case "mo": _filepath = Uri.UnescapeDataString(nameOrValue[1]); break;
					case "mtc":
						{
							int mtcInt;
							if (Int32.TryParse(nameOrValue[1], out mtcInt))
							{
								if (MimeTypeEnumHelper.IsValidMimeTypeCategory((MimeTypeCategory)mtcInt))
								{
									_mimeTypeCategory = (MimeTypeCategory)mtcInt; break;
								}
								else
									return false;
							}
							else
								return false;
						}
					case "dt":
						{
							int dtInt;
							if (Int32.TryParse(nameOrValue[1], out dtInt))
							{
								if (DisplayObjectTypeEnumHelper.IsValidDisplayObjectType((DisplayObjectType)dtInt))
								{
									_displayType = (DisplayObjectType)dtInt; break;
								}
								else
									return false;
							}
							else
								return false;
						}
					case "isp":
						{
							bool isPrivate;

							if (Boolean.TryParse(nameOrValue[1], out isPrivate))
								_isPrivate = isPrivate;
							else
								_isPrivate = true;

							break;
						}
					default: return false; // Unexpected query string parm. Return false so execution is aborted.
				}
			}

			return true;
		}

		private bool IsUserAuthorized()
		{
			// Show the media object if one of the following is true:
			// 1. The user is not authenticated. If we get to this point and the user is not authenticated, that means forms 
			// authenticated has been configured to allow anonymous access to this handler. So go ahead and show media object.
			// 2. The user is authenticated, AND the user is authorized to view media objects within the specified album.
			// 3. The user is authenticated, the user is requesting a hi-res image, AND the user is authorized to view
			//    hi-res images.
			bool userCanViewRegularImage = false;
			bool userCanViewHiResImage = false;
			if (this._isUserAuthenticated)
			{
				IGalleryServerRoleCollection roles = WebsiteController.GetRolesForUser();
				userCanViewRegularImage = SecurityManager.IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, roles, this._albumId, this._isUserAuthenticated, this._isPrivate);
				if (this._displayType == DisplayObjectType.Original)
				{
					userCanViewHiResImage = SecurityManager.IsUserAuthorized(SecurityActions.ViewOriginalImage, roles, this._albumId, this._isUserAuthenticated, this._isPrivate);
				}
			}

			bool isUserAuthorized = ((!this._isUserAuthenticated)
				|| ((this._displayType != DisplayObjectType.Original) && userCanViewRegularImage)
				|| ((this._displayType == DisplayObjectType.Original) && userCanViewHiResImage));

			return isUserAuthorized;
		}

		private void ShowMediaObject()
		{
			if (this._filename == GalleryServerPro.Business.GlobalConstants.DefaultFileName)
			{
				// A filename matching the DefaultFilename constant is our signal to generate the
				// default album thumbnail and send to client.
				ProcessDefaultThumbnail();
			}
			else
			{
				if (!MimeTypeEnumHelper.IsValidMimeTypeCategory(this._mimeTypeCategory))
				{
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.UnexpectedQueryStringException();
				}

				if ((this._mimeTypeCategory != MimeTypeCategory.Image) && (this._mediaObjectId > int.MinValue))
				{
					// We never apply the watermark to non-image media objects.
					ProcessMediaObject();
				}
				else
				{
					// Apply watermark to thumbnails only when the config setting applyWatermarkToThumbnails = true.
					// Apply watermark to optimized and original images only when applyWatermark = true.
					bool applyWatermark = WebsiteController.GetGalleryServerProConfigSection().Core.ApplyWatermark;
					bool applyWatermarkToThumbnails = WebsiteController.GetGalleryServerProConfigSection().Core.ApplyWatermarkToThumbnails;
					bool isThumbnail = (_displayType == DisplayObjectType.Thumbnail);

					if (AppSetting.Instance.IsInReducedFunctionalityMode && !isThumbnail)
					{
						ProcessMediaObjectWithWatermark();
					}

					if ((applyWatermark && !isThumbnail) || (applyWatermark && applyWatermarkToThumbnails && isThumbnail))
					{
						// If the user belongs to a role with watermarks set to visible, then show it; otherwise don't show the watermark.
						if (SecurityManager.IsUserAuthorized(SecurityActions.HideWatermark, WebsiteController.GetRolesForUser(), _albumId, this._isUserAuthenticated, _isPrivate))
						{
							// Show the image without the watermark.
							ProcessMediaObject();
						}
						else
						{
							// Overlay watermark on image before sending it to client.
							ProcessMediaObjectWithWatermark();
						}
					}
					else
					{
						ProcessMediaObject();
					}
				}
			}
		}

		private void ProcessDefaultThumbnail()
		{
			// Generate the default album thumbnail and send to client.
			Bitmap bmp = null;
			try
			{
				bmp = GetDefaultThumbnailBitmap();
				bmp.Save(_context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			finally
			{
				if (bmp != null)
					bmp.Dispose();
				_context.Response.End();
			}
		}

		private static Bitmap GetDefaultThumbnailBitmap()
		{
			//Return a bitmap of a default album image.  This will be used when no actual
			//image is available to serve as the pictorial view of the album.

			// First get the properties from galleryserverpro.config.
			GalleryServerPro.Configuration.Core coreConfig = WebsiteController.GetGalleryServerProConfigSection().Core;
			float ratio = coreConfig.EmptyAlbumThumbnailWidthToHeightRatio;
			int maxLength = coreConfig.MaxThumbnailLength;
			string imageText = coreConfig.EmptyAlbumThumbnailText;
			string fontName = coreConfig.EmptyAlbumThumbnailFontName;
			int fontSize = coreConfig.EmptyAlbumThumbnailFontSize;
			System.Drawing.Color bgColor = HelperFunctions.GetColor(coreConfig.EmptyAlbumThumbnailBackgroundColor);
			System.Drawing.Color fontColor = HelperFunctions.GetColor(coreConfig.EmptyAlbumThumbnailFontColor);

			int rctWidth, rctHeight; //Image width and height
			int x; //Starting point from left for the text
			int y; //Start point from top for the text

			if (ratio > 1)
			{
				rctWidth = maxLength;
				rctHeight = Convert.ToInt32((float)maxLength / ratio);
			}
			else
			{
				rctHeight = maxLength;
				rctWidth = Convert.ToInt32((float)maxLength * ratio);
			}

			Bitmap bmp = null;
			Graphics g = null;
			try
			{
				// If the font name does not match an installed font, .NET will substitute Microsoft Sans Serif.
				Font fnt = new Font(fontName, fontSize);
				Rectangle rct = new Rectangle(0, 0, rctWidth, rctHeight);
				bmp = new Bitmap(rct.Width, rct.Height);
				g = Graphics.FromImage(bmp);

				// Calculate x and y offset for text
				Size textSize = g.MeasureString(imageText, fnt).ToSize();

				x = (rctWidth - textSize.Width) / 2;
				y = (rctHeight - textSize.Height) / 2;

				if (x < 0) x = 0;
				if (y < 0) y = 0;

				// Generate image
				g.FillRectangle(new SolidBrush(bgColor), rct);
				g.DrawString(imageText, fnt, new SolidBrush(fontColor), x, y);
			}
			finally
			{
				if (g != null)
					g.Dispose();
			}

			return bmp;
		}

		private void ProcessMediaObject()
		{
			// Send the specified file to the client.
			FileStream fileStream = null;
			try
			{
				IMimeType mimeType = MimeType.LoadInstanceByFilePath(this._filename);

				this._context.Response.Clear();
				this._context.Response.ContentType = mimeType.FullType;
				this._context.Response.Buffer = false;

				HttpCachePolicy cachePolicy = this._context.Response.Cache;
				cachePolicy.SetExpires(System.DateTime.Now.AddSeconds(2592000)); // 30 days
				cachePolicy.SetCacheability(HttpCacheability.Public);
				cachePolicy.SetValidUntilExpires(true);

				int bufferSize = WebsiteController.GetGalleryServerProConfigSection().Core.MediaObjectDownloadBufferSize;
				byte[] buffer = new byte[bufferSize];
				long byteCount;
				fileStream = File.OpenRead(this._filepath);
				while ((byteCount = fileStream.Read(buffer, 0, buffer.Length)) > 0)
				{
					if (this._context.Response.IsClientConnected)
					{
						this._context.Response.OutputStream.Write(buffer, 0, buffer.Length);
						this._context.Response.Flush();
					}
					else
					{
						return;
					}
				}
			}
			catch (Exception ex)
			{
				GalleryServerPro.ErrorHandler.AppErrorHandler.RecordErrorInfo(ex);
			}
			finally
			{
				if (fileStream != null)
					fileStream.Close();

				this._context.Response.End();
			}
		}

		private void ProcessMediaObjectWithWatermark()
		{
			// Send the specified file to the client with the watermark overlayed on top.
			IMimeType mimeType = MimeType.LoadInstanceByFilePath(this._filename);

			this._context.Response.Clear();
			this._context.Response.ContentType = mimeType.FullType;

			System.Drawing.Image watermarkedImage = null;
			try
			{
				try
				{
					watermarkedImage = ImageHelper.AddWatermark(this._filepath);
				}
				catch (Exception ex)
				{
					// Can't apply watermark to image. Substitute an error image and send that to the user.
					GalleryServerPro.ErrorHandler.AppErrorHandler.RecordErrorInfo(ex);
					watermarkedImage = System.Drawing.Image.FromFile(this._context.Request.MapPath(String.Concat(WebsiteController.GetAppRootPathUrl(), "/images/error_48x48.png")));
				}

				watermarkedImage.Save(this._context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			finally
			{
				if (watermarkedImage != null)
					watermarkedImage.Dispose();
			}

			this._context.Response.End();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

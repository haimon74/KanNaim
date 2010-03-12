using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.task
{
	public partial class rotateimages : GspPage
	{
		#region Private Fields


		#endregion

		#region Protected Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.EditMediaObject);

			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from a control has bubbled up.  If it's the Ok button, then run the
			//code to synchronize; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				int msg = btnOkClicked();

				if (msg > int.MinValue)
					this.RedirectToReferringPage("msg", msg.ToString(CultureInfo.InvariantCulture));
				else
					this.RedirectToReferringPage("msg", ((int)Message.ObjectsSuccessfullyRotated).ToString(CultureInfo.InvariantCulture));
			}

			return true;
		}

		#endregion

		#region Public Properties

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Rotate_Images_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Rotate_Images_Body_Text;
			Master.OkButtonText = Resources.GalleryServerPro.Task_Rotate_Images_Ok_Button_Text;
			Master.OkButtonToolTip = Resources.GalleryServerPro.Task_Rotate_Images_Ok_Button_Tooltip;

			IGalleryObjectCollection albumChildren = this.WebController.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Image, true);

			if (albumChildren.Count > 0)
			{
				const int widthBuffer = 20; // Extra width padding to allow room for the icon images.
				const int heightBuffer = 20; // Extra height padding to allow room for the icon images.
				SetThumbnailCssStyle(albumChildren, widthBuffer, heightBuffer, "thmbRotate");

				rptr.DataSource = albumChildren;
				rptr.DataBind();
			}
			else
			{
				this.RedirectToAlbumViewPage("msg", ((int)Message.CannotRotateNoRotateableObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
			}
		}

		private int btnOkClicked()
		{
			return rotateImages();
		}

		private int rotateImages()
		{
			// Rotate any images on the hard drive according to the user's wish.
			int returnValue = int.MinValue;
			
			Dictionary<int, RotateFlipType> imagesToRotate = retrieveUserSelections();

			try
			{
				HelperFunctions.BeginTransaction();
				foreach (KeyValuePair<int, RotateFlipType> kvp in imagesToRotate)
				{
					IGalleryObject image = Factory.LoadImageInstance(kvp.Key);
					image.Rotation = kvp.Value;

					try
					{
						WebsiteController.SaveGalleryObject(image);
					}
					catch (UnsupportedImageTypeException)
					{
						returnValue = (int)Message.CannotRotateInvalidImage;
					}
				}
				HelperFunctions.CommitTransaction();
			}
			catch
			{
				HelperFunctions.RollbackTransaction();
				throw;
			}

			HelperFunctions.PurgeCache();

			return returnValue;
		}

		private Dictionary<int, RotateFlipType> retrieveUserSelections()
		{
			// Iterate through all the objects, retrieving the orientation of each image. If the
			// orientation has changed (it is no longer set to 't' for top), then add it to an array.
			// The media object IDs are stored in a hidden input tag.
			HtmlInputHidden rotateTag;
			HtmlInputHidden moidTag;

			Dictionary<int, RotateFlipType> imagesToRotate = new Dictionary<int, RotateFlipType>();

			foreach (RepeaterItem rptrItem in rptr.Items)
			{
				rotateTag = (HtmlInputHidden)rptrItem.FindControl("txtSelectedSide"); // The hidden <input> tag with an empty string or t, r, b, l

				if (rotateTag.Value.Trim().Length < 1)
					continue;

				char newOrientation = Convert.ToChar(rotateTag.Value.Trim().Substring(0, 1), CultureInfo.InvariantCulture);
				// If the orientation value isn't valid, throw an exception.
				if ((newOrientation != 't') && (newOrientation != 'r') && (newOrientation != 'b') && (newOrientation != 'l'))
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.UnexpectedFormValueException();

				RotateFlipType rft;
				if (newOrientation != 't')
				{
					// User selected an orientation other than t(top). Add to array.
					switch (newOrientation)
					{
						case 'r': rft = RotateFlipType.Rotate270FlipNone; break;
						case 'b': rft = RotateFlipType.Rotate180FlipNone; break;
						case 'l': rft = RotateFlipType.Rotate90FlipNone; break;
						default: rft = RotateFlipType.RotateNoneFlipNone; break; // Should never get here because of our if condition above, but let's be safe
					}

					// User selected an orientation other than t(top). Add to dictionary.
					moidTag = (HtmlInputHidden)rptrItem.FindControl("moid"); // The hidden <input> tag with the media object ID
					int moid;
					if (Int32.TryParse(moidTag.Value, out moid))
					{
						imagesToRotate.Add(Convert.ToInt32(moidTag.Value, CultureInfo.InvariantCulture), rft);
					}
					else
						throw new GalleryServerPro.ErrorHandler.CustomExceptions.UnexpectedFormValueException();
				}
			}
			return imagesToRotate;
		}

		#endregion
	}
}

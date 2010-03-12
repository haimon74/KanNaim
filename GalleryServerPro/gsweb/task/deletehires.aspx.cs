using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using System.IO;

namespace GalleryServerPro.Web.task
{
	public partial class deletehires : GspPage
	{
		#region Private Fields

		private long _totalHiResSizeKB = long.MinValue; // Holds the total size of all hi-res images in this album

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
				if (btnOkClicked())
				{
					this.RedirectToAlbumViewPage("msg", ((int)Message.HiResImagesSuccessfullyDeleted).ToString(CultureInfo.InvariantCulture));
				}
			}

			return true;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the total file size, in KB, of all the high resolution images in the current album. The total does
		/// not include images that do not have a high resolution version, nor does it include file size of any other
		/// type of media object, such as video or audio files.
		/// </summary>
		public long TotalFileSizeKbAllHiResImages
		{
			get
			{
				if (_totalHiResSizeKB == long.MinValue)
				{
					long sumTotal = 0;
					foreach (IGalleryObject go in this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Image))
					{
						if (go.Original.FileName != go.Optimized.FileName)
							sumTotal += go.Original.FileSizeKB;
					}

					_totalHiResSizeKB = sumTotal;
				}

				return _totalHiResSizeKB;
			}
		}

		#endregion

		#region Protected Methods

		protected static string GetTitle(string title)
		{
			// Truncate the Title if it is too long
			int maxLength = WebsiteController.GetGalleryServerProConfigSection().Core.MaxMediaObjectThumbnailTitleDisplayLength;
			int numCharsToRemoveToMakeRoomForCheckbox = 4;
			string truncatedText = WebsiteController.TruncateTextForWeb(title, maxLength - numCharsToRemoveToMakeRoomForCheckbox);
			string titleText;
			if (truncatedText.Length != title.Length)
				titleText = string.Format(CultureInfo.CurrentCulture, "{0}...", truncatedText);
			else
				titleText = truncatedText;

			return titleText;
		}

		protected static bool DoesHiResImageExist(string optimizedFileName, string originalFileName)
		{
			// A hi-res version of an image exists if the optimized and original filenames are different.
			return (!optimizedFileName.ToUpperInvariant().Equals(originalFileName.ToUpperInvariant()));
		}

		protected static string GetNoHiResImageText(string optimizedFileName, string originalFileName)
		{
			// Images without hi-res versions should get a message saying so.
			return (DoesHiResImageExist(optimizedFileName, originalFileName) ? String.Empty : String.Format(CultureInfo.CurrentCulture, "<span class='msgwarning'>{0}</span>", Resources.GalleryServerPro.Task_Delete_HiRes_No_HiRes_Image_Text));
		}

		protected string GetPotentialSavings()
		{
			return String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Delete_HiRes_Potential_Savings_Text, this.TotalFileSizeKbAllHiResImages, ((float)this.TotalFileSizeKbAllHiResImages / 1024F).ToString("N", CultureInfo.CurrentCulture));
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.TaskHeader = Resources.GalleryServerPro.Task_Delete_HiRes_Header_Text;
			Master.TaskBody = Resources.GalleryServerPro.Task_Delete_HiRes_Body_Text;
			Master.OkButtonText = Resources.GalleryServerPro.Task_Delete_HiRes_Ok_Button_Text;
			Master.OkButtonToolTip = Resources.GalleryServerPro.Task_Delete_HiRes_Ok_Button_Tooltip;

			IGalleryObjectCollection albumChildren = this.WebController.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Image, true);

			if (albumChildren.Count > 0)
			{
				SetThumbnailCssStyle(albumChildren);

				rptr.DataSource = albumChildren;
				rptr.DataBind();
			}
			else
			{
				this.RedirectToAlbumViewPage("msg", ((int)Message.CannotDeleteHiResImagesNoObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
			}
		}

		private bool btnOkClicked()
		{
			//User clicked 'Delete images'.  Delete the selected images.
			List<int> selectedItems = retrieveUserSelections();
			if (selectedItems.Count == 0)
			{
				// No images were selected. Inform user and exit function.
				string msg = String.Format(CultureInfo.CurrentCulture, "<p class='msgwarning'><span class='bold'>{0} </span>{1}</p>", Resources.GalleryServerPro.Task_Delete_Objects_No_Objects_Selected_Hdr, Resources.GalleryServerPro.Task_Delete_Objects_No_Objects_Selected_Dtl);
				phMsg.Controls.Clear();
				phMsg.Controls.Add(new System.Web.UI.LiteralControl(msg));

				return false;
			}

			try
			{
				HelperFunctions.BeginTransaction();

				foreach (int moid in selectedItems)
				{
					IGalleryObject image = Factory.LoadImageInstance(moid);
					if (!image.Original.FileName.Equals(image.Optimized.FileName, StringComparison.OrdinalIgnoreCase))
					{
						string originalPath = image.Original.FileNamePhysicalPath;
						string originalExtension = Path.GetExtension(originalPath); // Ex: .bmp
						string optimizedExtension = Path.GetExtension(image.Optimized.FileNamePhysicalPath); // Ex: .jpg

						// Delete the original image file
						File.Delete(originalPath);

						if (!originalExtension.Equals(optimizedExtension, StringComparison.OrdinalIgnoreCase))
						{
							// The original has a different file extension than the optimized, so update the original file name with 
							// the extension from the optimized file. For example, this can happen when the original is an image that
							// is not a JPEG (BMP, TIF, etc).
							originalPath = Path.ChangeExtension(originalPath, optimizedExtension);
						}

						// Rename the optimized file to the original file. This is required because
						// optimized file names can be slightly different than the original file names. For example, optimized images
						// are prefixed with "zOpt_" and are always a JPEG file type, while the original does not have a special prefix
						// and may be BMP, TIF, etc.
						File.Move(image.Optimized.FileNamePhysicalPath, originalPath);

						image.Original.FileInfo = new System.IO.FileInfo(originalPath);

						image.Hashkey = HelperFunctions.GetHashKeyUnique(image.Original.FileInfo);

						image.Optimized.FileInfo = image.Original.FileInfo;

						image.Original.Width = image.Optimized.Width;
						image.Original.Height = image.Optimized.Height;
						image.Original.FileSizeKB = image.Optimized.FileSizeKB;

						WebsiteController.SaveGalleryObject(image);
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

			return true;
		}

		private List<int> retrieveUserSelections()
		{
			// Iterate through all the checkboxes, saving checked ones to an array.
			// The media object IDs are stored in a hidden input tag.
			CheckBox chkbx;
			HtmlInputHidden gc;
			List<int> moids = new List<int>();

			// Loop through each item in the repeater control. If an item is checked, extract the ID.
			foreach (RepeaterItem rptrItem in rptr.Items)
			{
				chkbx = (CheckBox)rptrItem.Controls[1]; // The <INPUT TYPE="CHECKBOX"> tag
				if (chkbx.Checked)
				{
					// Checkbox is checked. Save media object ID to array.
					gc = (HtmlInputHidden)rptrItem.Controls[3]; // The hidden <input> tag
					moids.Add(Convert.ToInt32(gc.Value, CultureInfo.InvariantCulture));
				}
			}

			return moids;
		}

		#endregion
	}
}

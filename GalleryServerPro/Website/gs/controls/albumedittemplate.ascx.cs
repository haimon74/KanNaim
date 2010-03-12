using System;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Configuration;
using System.Data;
using GalleryServerPro.Web.Controller;

namespace GalleryServerPro.Web.Controls
{
	public partial class albumedittemplate : GalleryUserControl
	{
		#region Private Fields

		private string _inheritedOwners;

		private const int _initialUserDropDownCount = 15; // The # of users to initially add to the album owner drop down list

		#endregion

		#region Properties

		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureControlsEveryTime();

			if (this.GalleryPage.IsNewPageLoad && !cboOwner.CausedCallback)
			{
				ConfigureControlsFirstTime();
			}
		}

		private void cboOwner_DataRequested(object sender, ComboBoxDataRequestedEventArgs e)
		{
			// An AJAX callback is requesting more users for the album owner drop down list.
			LoadUsersDropDown(e.StartIndex, e.NumItems, e.Filter);
		}

		#endregion

		#region Public Properties

		protected static string CalendarIconUrl
		{
			get
			{
				return String.Concat(Util.GetUrl("/images/componentart/calendar/btn_calendar.gif"));
			}
		}

		protected static int AlbumSummaryMaxLength
		{
			get
			{
				return GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumSummaryLength;
			}
		}

		protected static string ExampleDatePrefix
		{
			get
			{
				return Resources.GalleryServerPro.UC_Album_Header_Edit_Album_Example_Date_Prefix;
			}
		}

		protected static string ExampleDateSuffix
		{
			get
			{
				return Resources.GalleryServerPro.UC_Album_Header_Edit_Album_Example_Date_Suffix;
			}
		}

		/// <summary>
		/// Gets a comma separated list of album owners inherited by the current album. The highest owner is listed first.
		/// Returns an empty string if there are no inherited owners.
		/// </summary>
		/// <value>A comma separated list of album owners inherited by the current album.</value>
		protected string InheritedOwners
		{
			get
			{
				if (this._inheritedOwners == null)
				{
					this._inheritedOwners = String.Empty;

					IAlbum album = this.GalleryPage.GetAlbum().Parent as IAlbum;
					while (album != null)
					{
						if (!String.IsNullOrEmpty(album.OwnerUserName))
						{
							_inheritedOwners = album.OwnerUserName + ", " + _inheritedOwners;
						}

						album = album.Parent as IAlbum; // Will be null when it gets to the top album, since NullGalleryObject can't cast to IAlbum
					}

					// Strip off the trailing comma and space (if present).
					_inheritedOwners = _inheritedOwners.TrimEnd(new char[] { ',', ' ' });
				}

				return _inheritedOwners;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
			int albumTitleMaxLength = ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;

			txtTitle.MaxLength = albumTitleMaxLength;

			string albumTitleMaxLengthInfo = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_Album_Header_Album_Title_Max_Length_Text, albumTitleMaxLength);
			lblMaxTitleLengthInfo.Text = albumTitleMaxLengthInfo;

			string albumSummaryMaxLengthInfo = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_Album_Header_Album_Summary_Max_Length_Text, AlbumSummaryMaxLength);
			phAlbumSummaryMaxLengthInfo.Controls.Add(new LiteralControl(albumSummaryMaxLengthInfo));

			// Configure Calendar.
			string imagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/calendar/");
			cdrBeginDate.ImagesBaseUrl = imagesBaseUrl;
			cdrEndDate.ImagesBaseUrl = imagesBaseUrl;

			// Configure ComboBox.
			cboOwner.DropHoverImageUrl = Util.GetUrl("/images/componentart/combobox/ddn-hover.png");
			cboOwner.DropImageUrl = Util.GetUrl("/images/componentart/combobox/ddn.png");

			if (GalleryPage.GetAlbum().Parent.IsPrivate)
			{
				lblPrivateAlbumIsInherited.Text = Resources.GalleryServerPro.UC_Album_Header_Edit_Album_Is_Private_Disabled_Text;
			}

			this.ConfigureAlbumOwnerControls();
		}

		private void ConfigureAlbumOwnerControls()
		{
			if (this.GalleryPage.IsUserAuthorized(SecurityActions.AdministerSite))
			{
				this.mvOwner.ActiveViewIndex = 0;
				this.ConfigureUsersDropDownFirstTime();
			}
			else
			{
				this.mvOwner.ActiveViewIndex = 1;
				this.ShowAlbumOwner();
			}

			if (!string.IsNullOrEmpty(InheritedOwners))
			{
				pnlInheritedOwner.Visible = true;
				litInheritedOwner.Text = InheritedOwners;
			}
		}

		private void ShowAlbumOwner()
		{
			string owner = this.GalleryPage.GetAlbum().OwnerUserName;
			string currentUser = Util.UserName;

			if (currentUser == owner)
			{
				litOwnerReadOnly.Text = String.Format(Resources.GalleryServerPro.UC_Album_Header_Edit_Album_Current_User_Is_Owner_Text, currentUser);
			}
			else
			{
				litOwnerReadOnly.Text = String.IsNullOrEmpty(owner) ? Resources.GalleryServerPro.UC_Album_Header_Edit_Album_No_Owner_Text : owner;
			}
		}

		private void ConfigureUsersDropDownFirstTime()
		{

			#region Bug workarounds

			// Set the cache size to the number of items it might eventually hold. A bug in the ComboBox causes it to stop issuing callbacks
			// in certain cases when the items exceed's the default cache size of 200. Steps to repro:
			// 1. Do not set CacheSize so default of 200 is used.
			// 2. Scroll down the list until the # of items exceeds the cache size.
			// 3. Scroll back to the top. This invokes a callback to re-populate the beginning.
			// 4. Scroll down to the end. No callback occurs and the list no longer grows.
			// More info: http://www.componentart.com/forums/ShowPost.aspx?PostID=43428
			cboOwner.CacheSize = UserController.GetUserNames().Rows.Count;

			// The loading text does not display, so there is no point in setting this property. If and when it is fixed, a resource can
			// be created and this property can be assigned.
			// More info: http://www.componentart.com/forums/ShowPost.aspx?PostID=33027
			//cboOwner.LoadingText = "Loading...";

			// Some users on the ComponentArt forums report the ComboBox not issuing callbacks after the user types 3 characters.
			// I have not been able to reproduce this, but if it occurs the workaround is to set a couple properties as seen here.
			// More info: http://www.componentart.com/forums/ShowPost.aspx?PostID=36435
			//cboOwner.CacheMapEnabled = true;
			//cboOwner.CacheMapWidth = 0;

			#endregion

			// Initialize list of users with a few names.
			LoadUsersDropDown(0, _initialUserDropDownCount, String.Empty);

			// Add the (no owner) option. This allows the admin to remove ownership if desired.
			cboOwner.Items.Insert(0, new ComboBoxItem(Resources.GalleryServerPro.UC_Album_Header_Edit_Album_No_Owner_Text));

			if (String.IsNullOrEmpty(this.GalleryPage.GetAlbum().OwnerUserName))
			{
				// No album owner. Select the (no owner) item.
				cboOwner.SelectedIndex = 0;
			}
			else
			{
				// The album has an owner. Since the drop down is loaded on-demand, the owner's name may not be in the list,
				// so just set the text of the textbox.
				cboOwner.Text = this.GalleryPage.GetAlbum().OwnerUserName;
			}
		}

		/// <summary>
		/// Adds users to the album owner ComboBox that match the requested parameters.
		/// </summary>
		/// <param name="startIndex">Specifies the location within the list of users that marks the beginning of the users
		/// to be added. The list of users may be the entire list as retured by <see cref="UserController.GetUserNames()"/> or it
		/// may be a subset whose user names begin with the string <paramref name="filter"/>.</param>
		/// <param name="numUsers">The number of users to add to the ComboBox.</param>
		/// <param name="filter">A string to use to filter the list of users. The filter is used to match
		/// the beginning of the user name. Example: If filter="Ma", all user names beginning with "Ma" are added (but not
		/// to exceed <paramref name="numUsers"/>).</param>
		private void LoadUsersDropDown(int startIndex, int numUsers, string filter)
		{
			// Safety check
			if (filter.Length > 255) return;

			cboOwner.Items.Clear();

			DataRow[] userNames = (filter.Length > 0 ? UserController.GetUserNames().Select("UserName LIKE '" + filter + "%'") : UserController.GetUserNames().Select());

			int endIndex = Math.Min(startIndex + numUsers, userNames.Length);

			for (int i = startIndex; i < endIndex && i < userNames.Length; i++)
			{
				cboOwner.Items.Add(new ComboBoxItem(userNames[i][0].ToString()));
			}

			cboOwner.ItemCount = Math.Min(userNames.Length, endIndex + cboOwner.DropDownPageSize);
		}

		private void ConfigureControlsEveryTime()
		{
			cboOwner.DataRequested += cboOwner_DataRequested;

			DetectAndFixNonGregorianCalendar();
		}

		private static void DetectAndFixNonGregorianCalendar()
		{
			// The Calendar control does not work when used in a culture having a non-Gregorian calendar. Detect
			// this situation and force the current thread to use the Gregorian calendar.
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Calendar.GetType() != typeof(GregorianCalendar))
			{
				CultureInfo ci = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
				ci.DateTimeFormat.Calendar = new GregorianCalendar();
				System.Threading.Thread.CurrentThread.CurrentCulture = ci;
				System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
			}
		}

		#endregion
	}
}
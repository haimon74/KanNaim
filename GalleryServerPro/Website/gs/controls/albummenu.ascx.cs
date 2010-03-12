using System;
using System.Globalization;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.Controls
{
	public partial class albummenu : GalleryUserControl
	{
		#region Private Fields

		private bool? _showActionMenu;

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			AddActionMenu();

			if (!this.GalleryPage.IsCallback)
			{
				phMenu.Controls.Add(new System.Web.UI.LiteralControl(buildMenuString()));
			}
		}

		#endregion

		#region Protected Methods

		protected string GetAlbumMenuClass()
		{
			if (this.ShowActionMenu)
			{
				return "albumMenu indented"; // Add the indented CSS class to make room for the action menu.
			}
			else
			{
				return "albumMenu";
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a value indicating whether the action menu should be displayed. Always returns false for anonymous users.
		/// Returns true for logged on users if they have permission to execute at least one of the commands in the menu
		/// against the current album.
		/// </summary>
		public bool ShowActionMenu
		{
			get
			{
				if (!this._showActionMenu.HasValue)
				{
					if (this.GalleryPage.IsAnonymousUser)
					{
						this._showActionMenu = false; // Always returns false for anonymous users.
					}
					else
					{
						// Return true if logged on user has permission for at least one menu item.
						return (this.GalleryPage.UserCanAdministerSite ||
							this.GalleryPage.UserCanEditMediaObject ||
							this.GalleryPage.UserCanEditAlbum ||
							this.GalleryPage.UserCanDeleteCurrentAlbum ||
							this.GalleryPage.UserCanDeleteMediaObject ||
							this.GalleryPage.UserCanSynchronize ||
							this.GalleryPage.UserCanAddAlbumToAtLeastOneAlbum ||
							this.GalleryPage.UserCanAddMediaObjectToAtLeastOneAlbum);
					}
				}

				return this._showActionMenu.Value;
			}
		}

		#endregion

		#region Private Methods

		private void AddActionMenu()
		{
			if (this.ShowActionMenu)
				phActionMenu.Controls.Add(Page.LoadControl(Util.GetUrl("/controls/actionmenu.ascx")));
		}

		private string buildMenuString()
		{
			string menuString = string.Empty;
			string appPath = Util.GetCurrentPageUrl();

			IAlbum album = GalleryPage.GetAlbum();
			IGalleryServerRoleCollection roles = this.GalleryPage.GetGalleryServerRolesForUser();
			string dividerText = Resources.GalleryServerPro.UC_Album_Menu_Album_Divider_Text;
			bool foundTopAlbum = false;
			bool foundBottomAlbum = false;
			while (!foundTopAlbum)
			{
				// Iterate through each album and it's parents, working the way toward the top. For each album, build up a breadcrumb menu item.
				// Eventually we will reach one of three situations: (1) a virtual album that contains the child albums, (2) an album the current
				// user does not have permission to view, or (3) the actual top-level album.
				if (album.IsVirtualAlbum) 
				{
					menuString = menuString.Insert(0, string.Format(CultureInfo.CurrentCulture, " {0} <a href=\"{1}\">{2}</a>", dividerText, appPath, Resources.GalleryServerPro.Site_Virtual_Album_Title));
					foundTopAlbum = true;
				}
				else if (!Util.IsUserAuthorized(SecurityActions.ViewAlbumOrMediaObject, roles, album.Id, album.IsPrivate))
				{
					// User is not authorized to view this album. If the user has permission to view more than one top-level album, then we want
					// to display an "All albums" link. To determine this, load the root album. If a virtual album is returned, then we know the
					// user has access to more than one top-level album. If it is an actual album (with a real ID and persisted in the data store),
					// that means that album is the only top-level album the user can view, and thus we do not need to create a link that is one
					// "higher" than that album.
					IAlbum rootAlbum = Factory.LoadRootAlbum(SecurityActions.ViewAlbumOrMediaObject | SecurityActions.ViewOriginalImage, GalleryPage.GetGalleryServerRolesForUser());
					if (rootAlbum.IsVirtualAlbum)
					{
						menuString = menuString.Insert(0, string.Format(CultureInfo.CurrentCulture, " {0} <a href=\"{1}\">{2}</a>", dividerText, appPath, Resources.GalleryServerPro.Site_Virtual_Album_Title));
					}
					foundTopAlbum = true;
				}
				else
				{
					// Regular album somewhere in the hierarchy. Create a breadcrumb link.
					string hyperlinkIdString = String.Empty;
					if (!foundBottomAlbum)
					{
						hyperlinkIdString = " id=\"currentAlbumLink\""; // ID is referenced when inline editing an album's title
						foundBottomAlbum = true;
					}
					menuString = menuString.Insert(0, string.Format(CultureInfo.CurrentCulture, " {0} <a{1} href=\"{2}?aid={3}\">{4}</a>", dividerText, hyperlinkIdString, appPath, album.Id, Util.RemoveHtmlTags(album.Title)));
				}

				if (album.Parent is GalleryServerPro.Business.NullObjects.NullGalleryObject)
					foundTopAlbum = true;
				else
					album = (IAlbum)album.Parent;
			}

			menuString = menuString.Substring(dividerText.Length + 2); // Remove the first divider character

			return menuString;
		}

		#endregion

	}
}
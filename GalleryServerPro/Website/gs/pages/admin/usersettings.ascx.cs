using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.WebControls;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class usersettings : Pages.AdminPage
	{
		private List<String> _defaultRolesForNewUsers;
		private List<String> _usersToNotifyForNewAccounts;
		private List<String> _usersWithAdminPermission;

		#region Public Properties

		/// <summary>
		/// Gets the default roles for self registered users. The value is a collection that is parsed from the comma-delimited string 
		/// stored in the DefaultRolesForSelfRegisteredUser configuration setting. During postbacks the value is retrieved from the combobox.
		/// </summary>
		/// <value>The default roles for self registered users.</value>
		public List<String> DefaultRolesForNewUsers
		{
			get
			{
				if (this._defaultRolesForNewUsers == null)
				{
					string defaultRoles = (IsPostBack ? cboUserRoles.Text : Util.HtmlDecode(Core.DefaultRolesForSelfRegisteredUser));

					this._defaultRolesForNewUsers = new List<string>();

					foreach (string roleName in defaultRoles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						this._defaultRolesForNewUsers.Add(roleName.Trim());
					}
				}

				return this._defaultRolesForNewUsers;
			}
		}

		/// <summary>
		/// Gets the list of user names of accounts to notify when an account is created. The value is a collection that is 
		/// parsed from the comma-delimited string stored in the UsersToNotifyWhenAccountIsCreated configuration setting.
		/// During postbacks the value is retrieved from the combobox.
		/// </summary>
		/// <value>The list of user names of accounts to notify when an account is created.</value>
		public List<String> UsersToNotifyForNewAccounts
		{
			get
			{
				if (this._usersToNotifyForNewAccounts == null)
				{
					string usersToNotify = (IsPostBack ? cboUsersToNotify.Text : Util.HtmlDecode(Core.UsersToNotifyWhenAccountIsCreated));

					this._usersToNotifyForNewAccounts = new List<string>();

					foreach (string userName in usersToNotify.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						this._usersToNotifyForNewAccounts.Add(userName.Trim());
					}
				}

				return this._usersToNotifyForNewAccounts;
			}
		}

		/// <summary>
		/// Gets the list of administrators for the current gallery. That is, it returns the user names of accounts 
		/// belonging to roles with AllowAdministerSite permission.
		/// </summary>
		/// <value>The list of administrators for the current gallery.</value>
		public List<String> UsersWithAdminPermission
		{
			get
			{
				if (this._usersWithAdminPermission == null)
				{
					this._usersWithAdminPermission = new List<string>();

					foreach (IGalleryServerRole role in RoleController.GetGalleryServerRoles())
					{
						if (role.AllowAdministerSite)
						{
							foreach (string userName in RoleController.GetUsersInRole(role.RoleName))
							{
								if (!this._usersWithAdminPermission.Contains(userName))
								{
									this._usersWithAdminPermission.Add(userName);
								}
							}
						}
					}
				}

				return this._usersWithAdminPermission;
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			this.AdminHeaderPlaceHolder = phAdminHeader;
			this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);
			
			ConfigureControlsEveryTime();

			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}

			RegisterJavascript();
		}

		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from the control has bubbled up.  If it's the Ok button, then run the
			//code to save the data to the database; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				SaveSettings();
			}

			ConfigureUsersToNotifyComboBoxFirstTime();

			ConfigureDefaultRolesComboBoxFirstTime();

			return true;
		}

		protected void wwDataBinder_AfterBindControl(GalleryServerPro.WebControls.wwDataBindingItem item)
		{
			// We need to HTML decode the role name that appears in the combo box
			if (item.ControlId == cboUserRoles.ID)
			{
				cboUserRoles.Text = Util.HtmlDecode(cboUserRoles.Text);
			}
		}

		protected bool wwDataBinder_BeforeUnbindControl(WebControls.wwDataBindingItem item)
		{
			if (!BeforeUnbind_ProcessEnableSelfRegistrationControls(item))
				return false;

			if (!BeforeUnbind_ProcessEnableUserAlbumsControls(item))
				return false;

			if (!BeforeUnbind_ProcessUserAccountControls(item))
				return false;

			return true;
		}

		#endregion

		#region Private Methods

		private bool BeforeUnbind_ProcessUserAccountControls(wwDataBindingItem item)
		{
			// When allow HTML is unchecked, several child items are disabled via javascript. Disabled HTML items are not
			// posted during a postback, so we don't have accurate information about their states. For these controls don't save
			// anything by returning false. Furthermore, to prevent these child controls from incorrectly reverting to an
			// empty or unchecked state in the UI, assign their properties to their config setting. 
			
			// Step 1: Handle the "allow HTML" checkbox
			if (!chkAllowHtml.Checked)
			{
				if (item.ControlId == txtAllowedHtmlTags.ID)
				{
					txtAllowedHtmlTags.Text = Core.AllowedHtmlTags;
					return false;
				}

				if (item.ControlId == txtAllowedHtmlAttributes.ID)
				{
					txtAllowedHtmlAttributes.Text = Core.AllowedHtmlAttributes;
					return false;
				}
			}
			else
			{
				// User may have hit Return while editing one of the textboxes. Remove any return characters to be safe.
				if (item.ControlId == txtAllowedHtmlTags.ID)
				{
					txtAllowedHtmlTags.Text = txtAllowedHtmlTags.Text.Replace("\r\n", String.Empty);
				}

				if (item.ControlId == txtAllowedHtmlAttributes.ID)
				{
					txtAllowedHtmlAttributes.Text = txtAllowedHtmlAttributes.Text.Replace("\r\n", String.Empty);
				}
			}

			// Step 2: Handle the "allow user account management" checkbox
			if (!this.chkAllowManageAccount.Checked)
			{
				if (item.ControlId == this.chkAllowDeleteOwnAccount.ID)
				{
					this.chkAllowDeleteOwnAccount.Checked = Core.AllowDeleteOwnAccount;
					return false;
				}
			}
			
			return true;
		}

		private bool BeforeUnbind_ProcessEnableSelfRegistrationControls(wwDataBindingItem item)
		{
			if (!this.chkEnableSelfRegistration.Checked)
			{
				// When self registration is unchecked, several child items are disabled via javascript. Disabled HTML items are not
				// posted during a postback, so we don't have accurate information about their states. For these controls don't save
				// anything by returning false. Furthermore, to prevent these child controls from incorrectly reverting to an
				// empty or unchecked state in the UI, assign their properties to their config setting. 
				if (item.ControlId == this.chkRequireEmailValidation.ID)
				{
					this.chkRequireEmailValidation.Checked = Core.RequireEmailValidationForSelfRegisteredUser;
					return false;
				}

				if (item.ControlId == this.chkRequireAdminApproval.ID)
				{
					this.chkRequireAdminApproval.Checked = Core.RequireApprovalForSelfRegisteredUser;
					return false;
				}

				if (item.ControlId == this.chkUseEmailForAccountName.ID)
				{
					this.chkUseEmailForAccountName.Checked = Core.UseEmailForAccountName;
					return false;
				}
			}

			return true;
		}

		private bool BeforeUnbind_ProcessEnableUserAlbumsControls(wwDataBindingItem item)
		{
			if (!this.chkEnableUserAlbums.Checked)
			{
				// When user albums is unchecked, several child items are disabled via javascript. Disabled HTML items are not
				// posted during a postback, so we don't have accurate information about their states. For these controls don't save
				// anything by returning false. Furthermore, to prevent these child controls from incorrectly reverting to an
				// empty or unchecked state in the UI, assign their properties to their config setting. 
				if (item.ControlId == this.chkRedirectAfterLogin.ID)
				{
					this.chkRedirectAfterLogin.Checked = Core.RedirectToUserAlbumAfterLogin;
					return false;
				}

				if (item.ControlId == this.txtAlbumNameTemplate.ID)
				{
					this.txtAlbumNameTemplate.Text = Core.UserAlbumNameTemplate;
					return false;
				}

				if (item.ControlId == this.txtAlbumSummaryTemplate.ID)
				{
					this.txtAlbumSummaryTemplate.Text = Core.UserAlbumSummaryTemplate;
					return false;
				}
			}

			return true;
		}

		protected bool wwDataBinder_ValidateControl(WebControls.wwDataBindingItem item)
		{
			if (!ValidateDefaultRolesForSelfRegisteredUser(item))
				return false;

			if (!ValidateUsersToNotifyWhenAccountIsCreated(item))
				return false;

			if (!ValidateUserAlbums(item))
				return false;

			return true;
		}

		private bool ValidateDefaultRolesForSelfRegisteredUser(wwDataBindingItem item)
		{
			if ((item.ControlInstance == this.cboUserRoles) && (this.cboUserRoles.Text != Core.DefaultRolesForSelfRegisteredUser))
			{
				// User has updated the list of default roles. Make sure they represent valid roles.
				foreach (string roleName in this.cboUserRoles.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (!RoleController.RoleExists(Util.HtmlDecode(roleName)))
					{
						item.BindingErrorMessage = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_User_Settings_Invalid_Role_Name_Msg, roleName);
						return false;
					}
				}
			}
			return true;
		}

		private bool ValidateUsersToNotifyWhenAccountIsCreated(wwDataBindingItem item)
		{
			if ((item.ControlInstance == this.cboUsersToNotify) && (this.cboUsersToNotify.Text != Core.UsersToNotifyWhenAccountIsCreated))
			{
				// User has updated the list of users to notify. Make sure they represent valid user account names.
				foreach (string userName in this.cboUsersToNotify.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (UserController.GetUser(Util.HtmlDecode(userName), false) == null)
					{
						item.BindingErrorMessage = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_User_Settings_Invalid_User_Name_Msg, userName);
						return false;
					}
				}
			}
			return true;
		}

		private bool ValidateUserAlbums(wwDataBindingItem item)
		{
			if ((item.ControlInstance == this.cboUserAlbumParent) && (chkEnableUserAlbums.Checked))
			{
				// User albums are selected. Make sure an album has been chosen to serve as the container for the user albums.
				int albumId;

				if ((tvUC.SelectedNode != null) && (Int32.TryParse(tvUC.SelectedNode.Value, out albumId)))
				{
					return true;
				}
				else
				{
					item.BindingErrorMessage = Resources.GalleryServerPro.Admin_User_Settings_Invalid_UserAlbumParent_Msg;
					return false;
				}
			}

			return true;
		}

		private void ConfigureControlsEveryTime()
		{
			this.PageTitle = Resources.GalleryServerPro.Admin_User_Settings_Page_Header;
		}

		private void ConfigureControlsFirstTime()
		{
			AdminPageTitle = Resources.GalleryServerPro.Admin_User_Settings_Page_Header;
			
			if (!HasEditConfigPermission)
			{
				wwMessage.ShowMessage(String.Format(Resources.GalleryServerPro.Admin_Config_Security_Ex_Msg, Util.GalleryServerProConfigFilePath));
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;

				foreach (System.Web.UI.Control ctl in this.Controls)
				{
					if ((ctl is CheckBox) || (ctl is TextBox) || (ctl is DropDownList))
					{
						((System.Web.UI.WebControls.WebControl)ctl).Enabled = false;
					}
				}
			}

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				OkButtonBottom.Enabled = false;
				OkButtonTop.Enabled = false;
			}

			this.wwDataBinder.DataBind();

			ConfigureComboBoxesFirstTime();
		}

		private void ConfigureComboBoxesFirstTime()
		{
			ConfigureUsersToNotifyComboBoxFirstTime();

			ConfigureDefaultRolesComboBoxFirstTime();

			ConfigureUserAlbumParentComboBoxFirstTime();
		}

		private void ConfigureUsersToNotifyComboBoxFirstTime()
		{
			// Configure the USER LIST ComboBox.
			cboUsersToNotify.DropHoverImageUrl = Util.GetUrl("/images/componentart/combobox/ddn-hover.png");
			cboUsersToNotify.DropImageUrl = Util.GetUrl("/images/componentart/combobox/ddn.png");
			cboUsersToNotify.Text = String.Join(", ", this.UsersToNotifyForNewAccounts.ToArray()).TrimEnd(new char[] { ' ', ',' });

			// Add the users to the list, pre-selecting any that are specified in the config file
			List<ListItem> userListItems = new List<ListItem>();

			foreach (System.Data.DataRow user in UserController.GetUserNames().Rows)
			{
				userListItems.Add(new ListItem(Util.HtmlEncode(user[0].ToString()), Util.HtmlEncode(user[0].ToString())));

				if (this.UsersToNotifyForNewAccounts.Contains(user[0].ToString()))
				{
					userListItems[userListItems.Count - 1].Selected = true;
				}

				if (!this.UsersWithAdminPermission.Contains(user[0].ToString()))
				{
					userListItems[userListItems.Count - 1].Attributes["class"] = "gsp_j_notadmin";
				}

			}
			cblU.Items.Clear();
			cblU.Items.AddRange(userListItems.ToArray());
		}

		private void ConfigureDefaultRolesComboBoxFirstTime()
		{
			// Configure the ROLE LIST ComboBox.
			cboUserRoles.DropHoverImageUrl = Util.GetUrl("/images/componentart/combobox/ddn-hover.png");
			cboUserRoles.DropImageUrl = Util.GetUrl("/images/componentart/combobox/ddn.png");
			cboUserRoles.Text = String.Join(", ", this.DefaultRolesForNewUsers.ToArray()).TrimEnd(new char[] { ' ', ',' });

			// Add the roles to the list, pre-selecting any that are specified in the config file
			List<ListItem> roleListItems = new List<ListItem>();
			foreach (IGalleryServerRole role in RoleController.GetGalleryServerRoles())
			{
				roleListItems.Add(new ListItem(Util.HtmlEncode(role.RoleName), Util.HtmlEncode(role.RoleName)));

				if (this.DefaultRolesForNewUsers.Contains(role.RoleName))
				{
					roleListItems[roleListItems.Count - 1].Selected = true;
				}

				if (RoleController.IsRoleAnAlbumOwnerRole(role.RoleName))
				{
					roleListItems[roleListItems.Count - 1].Attributes["class"] = "gsp_j_albumownerrole";
				}

			}
			cblR.Items.Clear();
			cblR.Items.AddRange(roleListItems.ToArray());
		}

		private void ConfigureUserAlbumParentComboBoxFirstTime()
		{
			// Configure the album treeview ComboBox.
			this.cboUserAlbumParent.DropHoverImageUrl = Util.GetUrl("/images/componentart/combobox/ddn-hover.png");
			this.cboUserAlbumParent.DropImageUrl = Util.GetUrl("/images/componentart/combobox/ddn.png");

			string cboText;
			int albumId = Core.UserAlbumParentAlbumId;
			IAlbum albumToSelect = null;
			if (albumId > 0)
			{
				try
				{
					albumToSelect = Factory.LoadAlbumInstance(albumId, false);
					cboText = albumToSelect.Title;
					this.tvUC.BindTreeView(albumToSelect);
				}
				catch (InvalidAlbumException)
				{
					cboText = Resources.GalleryServerPro.Admin_User_Settings_User_Album_Parent_Is_Invalid_Text;
					this.tvUC.BindTreeView();
				}
			}
			else
			{
				this.tvUC.BindTreeView();
				cboText = Resources.GalleryServerPro.Admin_User_Settings_User_Album_Parent_Not_Assigned_Text;
			}

			this.cboUserAlbumParent.Text = cboText;
		}

		private void SaveSettings()
		{
			// Step 1: Update config manually with those items that are not managed via the wwDataBinder
			int albumId;

			if ((tvUC.SelectedNode != null) && (Int32.TryParse(tvUC.SelectedNode.Value, out albumId)))
			{
				CoreConfig.userAlbumParentAlbumId = albumId;
			}

			// Step 2: Save
			this.wwDataBinder.Unbind(this);

			if (wwDataBinder.BindingErrors.Count > 0)
			{
				this.wwMessage.CssClass = "wwErrorFailure gsp_msgwarning";
				this.wwMessage.Text = wwDataBinder.BindingErrors.ToHtml();
				return;
			}

			GspConfigController.SaveCore(this.CoreConfig);

			this.wwMessage.CssClass = "wwErrorSuccess gsp_msgfriendly gsp_bold";
			this.wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Save_Success_Text);
		}

		private void RegisterJavascript()
		{
			ScriptManager sm = ScriptManager.GetCurrent(this.Page);
			if (sm != null)
			{
				sm.Scripts.Add(new ScriptReference(Util.JQueryPath));
			}
		}

		#endregion
	}
}
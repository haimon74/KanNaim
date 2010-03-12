using System;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.admin
{
	public partial class manageroles : GspPage
	{
		#region Public Properties

		public CheckBox chkAdministerSite
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkAdministerSite");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkAdministerSite.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkViewObject
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkViewObject");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkViewObject.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkViewHiResImage
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkViewHiResImage");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkViewHiResImage.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkAddAlbum
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkAddAlbum");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkAddAlbum.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkAddMediaObject
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkAddMediaObject");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkAddMediaObject.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkEditAlbum
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkEditAlbum");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkEditAlbum.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkEditMediaObject
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkEditMediaObject");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkEditMediaObject.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkDeleteChildAlbum
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkDeleteChildAlbum");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkDeleteChildAlbum.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkDeleteMediaObject
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkDeleteMediaObject");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkDeleteMediaObject.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkSynchronize
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkSynchronize");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkSynchronize.");
				return (CheckBox)ctrl;
			}
		}

		public CheckBox chkHideWatermark
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "chkHideWatermark");
				if (ctrl == null) throw new WebException("Cannot find checkbox chkHideWatermark.");
				return (CheckBox)ctrl;
			}
		}

		public PlaceHolder phMessage
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "phMessage");
				if (ctrl == null) throw new WebException("Cannot find PlaceHolder phMessage.");
				return (PlaceHolder)ctrl;
			}
		}

		public Label lblRoleName
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "lblRoleName");
				if (ctrl == null) throw new WebException("Cannot find Label lblRoleName.");
				return (Label)ctrl;
			}
		}

		public TextBox txtRoleName
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "txtRoleName");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtRoleName.");
				return (TextBox)ctrl;
			}
		}

		public CallBack cbEditRole
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "cbEditRole");
				if (ctrl == null) throw new WebException("Cannot find CallBack cbEditRole.");
				return (CallBack)ctrl;
			}
		}

		public GalleryServerPro.Web.uc.albumtreeview tvUC
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditRole, "tvUC");
				if (ctrl == null) throw new WebException("Cannot find user control tvUC.");
				return (GalleryServerPro.Web.uc.albumtreeview)ctrl;
			}
		}

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			if (!IsPostBack)
			{
				ConfigureControls();
			}
		}

		protected void cbEditRole_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
		{
			// There should be two strings in the Parameters property. The first indicates the requested operation
			// (edit, save, etc). The second is the name of the role.
			try
			{
				if (e.Parameters.Length != 2)
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The cbEditRole_Callback is expecting two items in the parameter list, but received {0} items. The first item should be a string equal to 'add', 'edit', or 'save', and the second item should be a string containing the role name (should be empty if the first parameter = 'add').", e.Parameters.Length));

				string roleName = e.Parameters[1];
				switch (e.Parameter)
				{
					case "add":
					case "edit":
						{
							SetControlVisibility(roleName);

							PopulateControlsWithRoleData(roleName);

							// Render the controls with the updated information.
							FindControlRecursive(dgEditRole, "pnlDialogContent").RenderControl(e.Output);

							break;
						}
					case "save":
						{
							ValidateSaveRole(roleName);

							SaveRole(roleName);

							// Empty the cache so the next request will pull them from the data store.
							HelperFunctions.PurgeCache();

							// Write a little javascript to set a page variable that will be used during the OnCallbackComplete
							// event to close the dialog when the call is successful.
							e.Output.Write("<script type=\"text/javascript\">callbackStatus = 'success';</script>");

							break;
						}
					case "adminSelected":
						{
							// The user selected the Administer site permission. Since this permission applies to all albums, we need 
							// to select all the checkboxes for all albums. I couldn't get this to work on the client by calling checkAll(),
							// so I resorted to invoking this callback.
							BindAlbumTreeview(true);

							// Render the controls with the updated information.
							FindControlRecursive(dgEditRole, "pnlDialogContent").RenderControl(e.Output);

							break;
						}
				}
			}
			catch (Exception ex)
			{
				GalleryServerPro.Web.uc.usermessage msgBox = (GalleryServerPro.Web.uc.usermessage)LoadControl("~/uc/usermessage.ascx");
				msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Error;
				msgBox.MessageTitle = Resources.GalleryServerPro.Admin_Manage_Roles_Cannot_Save_Role_Msg;
				msgBox.MessageDetail = ex.Message;
				phMessage.Controls.Add(msgBox);

				FindControlRecursive(dgEditRole, "pnlDialogContent").RenderControl(e.Output);
			}
		}

		protected void gdRoles_DeleteCommand(object sender, ComponentArt.Web.UI.GridItemEventArgs e)
		{
			// Delete the gallery server role. This includes the Gallery Server role and the corresponding ASP.NET role.
			// If an exception is thrown, such as during ValidateDeleteUser(), the client side function 
			// gdUsers_CallbackError will catch it and display the message to the user.
			string roleName = e.Item["RoleName"].ToString();

			ValidateDeleteRole(roleName);

			try
			{
				DeleteGalleryServerRole(roleName);
			}
			finally
			{
				try
				{
					DeleteAspnetRole(roleName);
				}
				finally
				{
					HelperFunctions.PurgeCache();
				}
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			Master.OkButtonIsVisible = false;
			Master.CancelButtonIsVisible = false;
			Master.AdminPageTitle = Resources.GalleryServerPro.Admin_Manage_Roles_Page_Header;

			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess msgwarning";
				pAddRole.Visible = false;
			}

			ConfigureDialog();

			ConfigureGrid();

			RegisterJavaScript();
		}

		private void ConfigureDialog()
		{
			AddLoadingPanelClientTemplate();
		}

		private void AddLoadingPanelClientTemplate()
		{
			// Add loading panel client template
			ComponentArt.Web.UI.ClientTemplate loadingPanelClientTemplate = new ComponentArt.Web.UI.ClientTemplate();

			loadingPanelClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"<h3 style=""margin-top: 200px; text-align: center;"">
						{0}</h3>
					<p style=""text-align: center;"">
						<img src=""../images/componentart/callback/spinner.gif"" style=""width: 16px; height: 16px;""
							alt="""" /></p>
",
				Resources.GalleryServerPro.Admin_Manage_Roles_Processing_Text);

			cbEditRole.LoadingPanelClientTemplate = loadingPanelClientTemplate;
		}

		private void ConfigureGrid()
		{
			if (!AppSetting.Instance.IsInReducedFunctionalityMode)
				AddEditColumnClientTemplate();
		}

		private void AddEditColumnClientTemplate()
		{
			ComponentArt.Web.UI.ClientTemplate roleEditColumn = new ComponentArt.Web.UI.ClientTemplate();

			roleEditColumn.ID = "roleEditColumn";
			
			roleEditColumn.Text = String.Format(CultureInfo.InvariantCulture, @"<p>
						<a id=""## getRoleName(DataItem) ##"" href=""javascript:editRole('## getRoleName(DataItem) ##');""
							title=""{0}"">{1}</a> <a href=""javascript:hideUserMessage();if (ConfirmDelete('## getRoleName(DataItem) ##')) gdRoles.deleteItem(gdRoles.getItemFromKey(0, '## getRoleName(DataItem) ##'));"">
								{2}</a>
					</p>",
				Resources.GalleryServerPro.Admin_Manage_Roles_Edit_Role_Tooltip_Text,
				Resources.GalleryServerPro.Admin_Manage_Roles_Edit_Role_Hyperlink_Text,
				Resources.GalleryServerPro.Admin_Manage_Roles_Delete_Role_Hyperlink_Text);

			gdRoles.ClientTemplates.Add(roleEditColumn);
		}

		/// <summary>
		/// Bind the checkbox list and the treeview to the specified role. If adding a new role, pass null or an empty
		/// string to the roleName parameter.
		/// </summary>
		/// <param name="roleName">The name of the role to be bound to the checkbox list and treeview, or null if adding
		/// a new role.</param>
		private void PopulateControlsWithRoleData(string roleName)
		{
			// Gets called by the Callback control when the user clicks Add or Edit for a particular role.
			// Populate the controls with data specific to this role, especially in regard to checking the appropriate
			// checkboxes to indicate the level of permission granted to this role (nothing will be checked when 
			// adding a new role).

			IGalleryServerRole role = null;
			if (!String.IsNullOrEmpty(roleName))
			{
				role = Factory.LoadGalleryServerRole(roleName);

				if (role == null)
					throw new GalleryServerPro.ErrorHandler.CustomExceptions.InvalidGalleryServerRoleException();

				lblRoleName.Text = role.RoleName;
			}

			BindRolePermissionCheckboxes(role);

			BindAlbumTreeview(role);
		}

		/// <summary>
		/// Select (check) the permissions checkboxes corresponding to the permissions of the specified role. Specify null
		/// when adding a new role and the checkboxes will be set to their default values (unselected.)
		/// </summary>
		/// <param name="role">The Gallery Server role to be bound to the checkbox list of permissions, or null if adding
		/// a new role.</param>
		private void BindRolePermissionCheckboxes(IGalleryServerRole role)
		{
			if (role == null)
			{
				chkAddAlbum.Checked = false;
				chkAddMediaObject.Checked = false;
				chkAdministerSite.Checked = false;
				chkDeleteChildAlbum.Checked = false;
				chkDeleteMediaObject.Checked = false;
				chkEditAlbum.Checked = false;
				chkEditMediaObject.Checked = false;
				chkSynchronize.Checked = false;
				chkViewHiResImage.Checked = false;
				chkViewObject.Checked = false;
				chkHideWatermark.Checked = false;
			}
			else
			{
				chkAddAlbum.Checked = role.AllowAddChildAlbum;
				chkAddMediaObject.Checked = role.AllowAddMediaObject;
				chkAdministerSite.Checked = role.AllowAdministerSite;
				chkDeleteChildAlbum.Checked = role.AllowDeleteChildAlbum;
				chkDeleteMediaObject.Checked = role.AllowDeleteMediaObject;
				chkEditAlbum.Checked = role.AllowEditAlbum;
				chkEditMediaObject.Checked = role.AllowEditMediaObject;
				chkSynchronize.Checked = role.AllowSynchronize;
				chkViewHiResImage.Checked = role.AllowViewOriginalImage;
				chkViewObject.Checked = role.AllowViewAlbumOrMediaObject;
				chkHideWatermark.Checked = role.HideWatermark;
			}
		}

		/// <summary>
		/// Fill the treeview with all albums. All nodes representing albums for which the specified role has permission
		/// will be checked. If the overload that doesn't take a role parameter is used, then check all checkboxes if the
		/// isAdministratorChecked parameter is true.
		/// </summary>
		/// <param name="isAdministratorChecked">Indicates whether the administrator permission checkbox has been
		/// checked. Since this permission applies to all albums, when this parameter is true, all checkboxes for
		/// all albums will be checked.</param>
		private void BindAlbumTreeview(bool isAdministratorChecked)
		{
			BindAlbumTreeview(null, isAdministratorChecked);
		}

		/// <summary>
		/// Fill the treeview with all albums. All nodes representing albums for which the specified role has permission
		/// will be checked. If the overload that doesn't take a role parameter is used, then check all checkboxes if the
		/// isAdministratorChecked parameter is true.
		/// </summary>
		/// <param name="role">The role to be updated. If adding a new role, then set this parameter to null.</param>
		private void BindAlbumTreeview(IGalleryServerRole role)
		{
			bool isAdmin = ((role != null) && (role.AllowAdministerSite));
			BindAlbumTreeview(role, isAdmin);
		}

		/// <summary>
		/// Fill the treeview with all albums. All nodes representing albums for which the specified role has permission
		/// will be checked. If the overload that doesn't take a role parameter is used, then check all checkboxes if the
		/// isAdministratorChecked parameter is true.
		/// </summary>
		/// <param name="role">The role to be updated. If adding a new role, then set this parameter to null.</param>
		/// <param name="isAdministrator">Indicates whether the administrator permission checkbox has been
		/// checked or the specified role has administrative permission. Since administrative permission applies to all 
		/// albums, when this parameter is true, all checkboxes for all albums will be checked. An exception is thrown
		/// if the role.AllowAdministerSite property and the isAdministrator parameter do not match.</param>
		private void BindAlbumTreeview(IGalleryServerRole role, bool isAdministrator)
		{
			if ((role != null) && (role.AllowAdministerSite != isAdministrator))
			{
				throw new ArgumentException("Invalid arguments passed to BindAlbumTreeview method: The role.AllowAdministerSite property and the isAdministrator parameter must match.");
			}

			if (role != null) // Role will be null when user is adding a new role
			{
				IIntegerCollection albumIds = tvUC.AlbumIdsToCheck;
				albumIds.Clear();
				albumIds.AddRange(role.RootAlbumIds);

				if (role.RootAlbumIds.Contains(Factory.LoadRootAlbumInstance().Id))
				{
					// The role applies to all albums. Since the treeview initially renders to two levels, we need
					// to add the album IDs for the root album's child albums.
					foreach (IGalleryObject album in Factory.LoadRootAlbumInstance().GetChildGalleryObjects(GalleryObjectType.Album))
					{
						albumIds.Add(album.Id);
					}
				}		 
			}

			tvUC.DataBind();
		}

		private void SetControlVisibility(string roleName)
		{
			if (String.IsNullOrEmpty(roleName))
			{
				lblRoleName.Visible = false;
				txtRoleName.Visible = true;
			}
			else
			{
				lblRoleName.Visible = true;
				txtRoleName.Visible = false;
			}
		}

		private void SaveRole(string roleName)
		{
			// Gets called by the Callback control when the user clicks Save.
			try
			{
				if (String.IsNullOrEmpty(roleName))
				{
					AddNewRole();
				}
				else
				{
					UpdateExistingRole(roleName);
				}
			}
			finally
			{
				HelperFunctions.PurgeCache();
			}
		}

		private void UpdateExistingRole(string roleName)
		{
			IGalleryServerRole role = Factory.LoadGalleryServerRole(roleName);

			if (role == null)
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.InvalidGalleryServerRoleException();

			role.AllowAddChildAlbum = chkAddAlbum.Checked;
			role.AllowAddMediaObject = chkAddMediaObject.Checked;
			role.AllowAdministerSite = chkAdministerSite.Checked;
			role.AllowDeleteChildAlbum = chkDeleteChildAlbum.Checked;
			role.AllowDeleteMediaObject = chkDeleteMediaObject.Checked;
			role.AllowEditAlbum = chkEditAlbum.Checked;
			role.AllowEditMediaObject = chkEditMediaObject.Checked;
			role.AllowSynchronize = chkSynchronize.Checked;
			role.AllowViewOriginalImage = chkViewHiResImage.Checked;
			role.AllowViewAlbumOrMediaObject = chkViewObject.Checked;
			role.HideWatermark = chkHideWatermark.Checked;

			UpdateRoleAlbumRelationships(role);

			role.Save();
		}

		private void AddNewRole()
		{
			string roleName = txtRoleName.Text.Trim();
			System.Web.Security.Roles.CreateRole(roleName);

			IGalleryServerRole role = Factory.CreateGalleryServerRoleInstance(roleName, chkViewObject.Checked, chkViewHiResImage.Checked,
				chkAddMediaObject.Checked, chkAddAlbum.Checked, chkEditMediaObject.Checked, chkEditAlbum.Checked, chkDeleteMediaObject.Checked,
				false, chkDeleteChildAlbum.Checked, chkSynchronize.Checked, chkAdministerSite.Checked, chkHideWatermark.Checked);

			UpdateRoleAlbumRelationships(role);

			role.Save();
		}

		/// <summary>
		/// Update the list of root album IDs associated with this role based on the user's selection.
		/// Note that clearing the RootAlbumIds collection causes the AllAlbumIds property to be
		/// cleared out (Count = 0).
		/// </summary>
		/// <param name="role">The role whose root album/role relationships should be updated. When editing
		/// an existing role, specify this.GalleryRole. For new roles, pass the newly created role before
		/// saving it.</param>
		private void UpdateRoleAlbumRelationships(IGalleryServerRole role)
		{
			role.RootAlbumIds.Clear();

			if (role.AllowAdministerSite)
			{
				// Administer site permission automatically applies to all albums, so all we need to do is get
				// a reference to the root album ID.
				role.RootAlbumIds.Add(Factory.LoadRootAlbumInstance().Id);
			}
			else
			{
				role.RootAlbumIds.AddRange(tvUC.TopLevelCheckedAlbumIds);
			}
		}

		private static void DeleteAspnetRole(string roleName)
		{
			string[] usersInRole = System.Web.Security.Roles.GetUsersInRole(roleName);
			if ((usersInRole != null) && (usersInRole.Length > 0))
			{
				System.Web.Security.Roles.RemoveUsersFromRole(usersInRole, roleName);
			}
			if (Roles.RoleExists(roleName))
			{
				System.Web.Security.Roles.DeleteRole(roleName);
			}
		}

		private static IGalleryServerRole DeleteGalleryServerRole(string roleName)
		{
			IGalleryServerRole role = Factory.LoadGalleryServerRole(roleName);

			if (role != null)
			{
				role.Delete();
			}
			return role;
		}

		private void ValidateSaveRole(string roleName)
		{
			// Make sure the loggod-on person has authority to save the role and that 
			// h/she isn't doing anything stupid, like removing Administer site permission from 
			// the only role.
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			if (String.IsNullOrEmpty(roleName))
				return; // Role name will be empty when adding a new one, so the validation below doesn't apply.

			IGalleryServerRole roleToUpdate = Factory.LoadGalleryServerRole(roleName);

			if (roleToUpdate == null)
				throw new GalleryServerPro.ErrorHandler.CustomExceptions.InvalidGalleryServerRoleException();

			if (roleToUpdate.AllowAdministerSite && !chkAdministerSite.Checked)
			{
				// User is trying to remove administer site permission from this role. Make sure
				// at least one other role has this permission, and that the role has at least one member.
				bool atLeastOneOtherRoleHasAdminSitePermission = false;
				foreach (IGalleryServerRole role in GspPage.GetGalleryServerRoles())
				{
					if ((!role.RoleName.Equals(roleToUpdate.RoleName, StringComparison.OrdinalIgnoreCase) && role.AllowAdministerSite))
					{
						if (Roles.GetUsersInRole(role.RoleName).Length > 0)
						{
							atLeastOneOtherRoleHasAdminSitePermission = true;
							break;
						}
					}
				}

				if (!atLeastOneOtherRoleHasAdminSitePermission)
				{
					throw new WebException(Resources.GalleryServerPro.Admin_Manage_Roles_Cannot_Remove_Admin_Perm_Msg);
				}
			}
		}

		private void ValidateDeleteRole(string roleName)
		{
			// Make sure the loggod-on person has authority to delete the role and that 
			// h/she isn't doing anything stupid, like deleting the only role with Administer
			// site permission.
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			IGalleryServerRole roleToDelete = Factory.LoadGalleryServerRole(roleName);

			if (roleToDelete == null)
				return;

			if (roleToDelete.AllowAdministerSite)
			{
				// User is trying to delete a role with administer site permission. Make sure
				// at least one other role has this permission, and that the role has at least one member.
				bool atLeastOneOtherRoleHasAdminSitePermission = false;
				foreach (IGalleryServerRole role in GspPage.GetGalleryServerRoles())
				{
					if ((!role.RoleName.Equals(roleToDelete.RoleName, StringComparison.OrdinalIgnoreCase) && role.AllowAdministerSite))
					{
						if (Roles.GetUsersInRole(role.RoleName).Length > 0)
						{
							atLeastOneOtherRoleHasAdminSitePermission = true;
							break;
						}
					}
				}

				if (!atLeastOneOtherRoleHasAdminSitePermission)
				{
					throw new WebException(Resources.GalleryServerPro.Admin_Manage_Roles_Cannot_Delete_Role_Msg);
				}
			}

		}

		private void RegisterJavaScript()
		{
			string script = String.Format(CultureInfo.InvariantCulture, @"
			var callbackStatus = ''; // Used during and after callbacks to indicate success or failure
			var gridNeedsRefresh = false; // Set to true when adding a role so that the grid reloads.
			var isAddingRole = false; // Set to true when preparing to add a role; used by callback onComplete event

			function setText(node, newText)
			{{
				var childNodes = node.childNodes;
				for (var i=0; i < childNodes.length; i++)
				{{
					node.removeChild(childNodes[i]);
				}}
				if ((newText != null) && (newText.length > 0))
					node.appendChild(document.createTextNode(newText));
			}}

			function addRole()
			{{
				hideUserMessage();

				if(dgEditRole.get_isShowing())
				{{
					dgEditRole.Close();
				}}
				else
				{{
					isAddingRole = true;
					cbEditRole.callback('add', '');
					setText($get('dialogHeader'), '{0}');
					dgEditRole.beginUpdate();
					dgEditRole.set_animationDirectionElement('addnewrole');
					dgEditRole.set_value('');
					dgEditRole.set_title('{0}');
					dgEditRole.endUpdate();
					dgEditRole.Show();
				}}
			}}

			function editRole(roleName)
			{{
				hideUserMessage();

				if(dgEditRole.get_isShowing())
				{{
					dgEditRole.Close();
				}}
				else
				{{
					cbEditRole.callback('edit', roleName);
					setText($get('dialogHeader'), '{1} - ' + roleName);
					dgEditRole.beginUpdate();
					dgEditRole.set_animationDirectionElement(roleName);
					dgEditRole.set_value(roleName);
					dgEditRole.set_title('{1} - ' + roleName);
					dgEditRole.endUpdate();
					dgEditRole.Show();
				}}
			}}

			function ConfirmDelete(roleName)
			{{
				return confirm('{2} ' + roleName + '?');
			}}

			function IsAdministerSiteChecked()
			{{
				// Invoked from the onclick event of checkboxes that should remain checked when the 
				// 'Administer Site' checkbox is selected. Returning true cancels the onclick event.
				var adminCheckBox = document.getElementById('{3}');
				if (!adminCheckBox)
					return false;
				else		
					return adminCheckBox.checked;
			}}

			function chkAdministerSite_CheckChanged(adminCheckBox)
			{{
				if (!adminCheckBox) return;
				
				var isAdminChecked = adminCheckBox.checked;
				if (!isAdminChecked) return;
				
				$get('{4}').checked = true;
				$get('{5}').checked = true;
				$get('{6}').checked = true;
				$get('{7}').checked = true;
				$get('{8}').checked = true;
				$get('{9}').checked = true;
				$get('{10}').checked = true;
				$get('{11}').checked = true;
				$get('{12}').checked = true;

				// Select all albums
				{15}.checkAll();
			}}

			function saveRole()
			{{
				var roleName = dgEditRole.get_value();
				cbEditRole.callback('save', roleName);
				
				if (roleName == '') gridNeedsRefresh = true;
			}}

			function cbEditRole_OnCallbackComplete(sender, eventArgs)
			{{
				if (callbackStatus == 'success')
				{{
					dgEditRole.Close();
					if (gridNeedsRefresh)	gdRoles.callback();
					gridNeedsRefresh = false;
					callbackStatus = '';
				}}

				if (isAddingRole)
				{{
					$get('{14}').focus();
					isAddingRole = false;
				}}
				
				popupInfoRender();
			}}

			function gdRoles_CallbackError(sender, args)
			{{
				showUserMessage(args.get_errorMessage());

				gdRoles.callback();
			}}

			function showUserMessage(msg)
			{{
				setText($get('{16}'), msg);

				var usrMsg = $get('{17}');
				if (Sys.UI.DomElement.containsCssClass(usrMsg, 'invisible'))
				{{
					Sys.UI.DomElement.removeCssClass(usrMsg, 'invisible');
					Sys.UI.DomElement.addCssClass(usrMsg, 'visible');
				}}
			}}

			function hideUserMessage()
			{{
				var usrMsg = $get('{17}');
				if (Sys.UI.DomElement.containsCssClass(usrMsg, 'visible'))
				{{
					Sys.UI.DomElement.removeCssClass(usrMsg, 'visible');
					Sys.UI.DomElement.addCssClass(usrMsg, 'invisible');
				}}
			}}

			function getRoleName(dataItem)
			{{
				var roleName = dataItem.getMember('RoleName').get_value();
				// escape quotes and apostrophes
				roleName = roleName.replace(/""/g, '\\""');
				roleName = roleName.replace(/'/g, ""\\'"");
				return roleName;
			}}

			", Resources.GalleryServerPro.Admin_Dialog_Title_Add_Role, // 0
			Resources.GalleryServerPro.Admin_Dialog_Title_Edit_Role, // 1
			Resources.GalleryServerPro.Admin_Manage_Roles_Confirm_Delete_Text, // 2
			chkAdministerSite.ClientID, // 3
			chkViewObject.ClientID, // 4
			chkViewHiResImage.ClientID, // 5
			chkAddAlbum.ClientID, // 6
			chkAddMediaObject.ClientID, // 7
			chkEditAlbum.ClientID, // 8
			chkEditMediaObject.ClientID, // 9
			chkDeleteChildAlbum.ClientID, // 10
			chkDeleteMediaObject.ClientID, // 11
			chkSynchronize.ClientID, // 12
			Resources.GalleryServerPro.Site_Callback_Error_Text, // 13
			txtRoleName.ClientID, // 14
			tvUC.TreeView.ClientObjectId, // 15
			ucUserMessage.MessageDetailContainer.ClientID, // 16
			ucUserMessage.MessageContainer.ClientID // 17
			);

			ScriptManager.RegisterClientScriptBlock(this, typeof(GspPage), "pageFunctions", script, true);
		}

		#endregion
	}
}

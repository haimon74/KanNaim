using System;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using GalleryServerPro.Web.Entity;

namespace GalleryServerPro.Web.gs.pages.admin
{
	public partial class manageusers : Pages.AdminPage
	{
		#region Protected Events

		protected void Page_Init(object sender, EventArgs e)
		{
			// Using dynamically added header and footer controls cause the CallBack to error with "could not load data."
			// The Callback works if it doesn't have the OnCallbackComplete client event defined, but since we need it, 
			// we have to do without the header and footer controls.
			//this.AdminHeaderPlaceHolder = phAdminHeader;
			//this.AdminFooterPlaceHolder = phAdminFooter;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			// Without this line, the edit user callback fails with a "The data could not be loaded" error.
			// I posted this potential bug to the CA forums at http://www.componentart.com/forums/ShowPost.aspx?PostID=31137
			// Update 28 Sep 2007: This bug appears to have been fixed by CA, so we'll comment it out.
			//ControlCollection controls = dgEditUser.Controls;

			ProcessQueryString();

			ConfigureControls();
		}

		protected void gdUsers_DeleteCommand(object sender, ComponentArt.Web.UI.GridItemEventArgs e)
		{
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			// Remove the user from all roles, and then delete the user. If an exception is thrown, such as during
			// ValidateDeleteUser(), the client side function gdUsers_CallbackError will catch it and display
			// the message to the user.
			UserController.DeleteGalleryServerProUser(e.Item["UserName"].ToString(), true);
		}

		protected void cbValidateNewUserName_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
		{
			// The user just typed in a user name in the Add New User wizard. Let's check to see if it already
			// exists and let the user know the result.
			string requestedUsername = e.Parameter;

			if (String.IsNullOrEmpty(requestedUsername))
			{
				lblUserNameValidationResult.Text = String.Empty;
			}
			else
			{
				UserEntity user = UserController.GetUser(requestedUsername, false);

				bool userNameIsInUse = (user != null);

				if (userNameIsInUse)
				{
					lblUserNameValidationResult.Text = Resources.GalleryServerPro.Admin_Manage_Users_Username_Already_In_Use_Msg;
					lblUserNameValidationResult.CssClass = "gsp_msgwarning";
				}
				else
				{
					lblUserNameValidationResult.Text = Resources.GalleryServerPro.Admin_Manage_Users_Username_Already_Is_Valid_Msg;
					lblUserNameValidationResult.CssClass = "gsp_msgfriendly";
				}
			}

			lblUserNameValidationResult.RenderControl(e.Output);
		}

		protected void cbAddUser_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
		{
			try
			{
				switch (e.Parameter)
				{
					case "createUser":
						{
							AddUser();
							e.Output.Write("<script type=\"text/javascript\">callbackStatus = 'success';</script>");
							break;
						}
				}
			}
			catch (MembershipCreateUserException ex)
			{
				// Just in case we created the user and the exception occured at a later step, like adding the roles, delete the user,
				// but only if the user exists AND the error wasn't 'DuplicateUserName'.
				if ((ex.StatusCode != MembershipCreateStatus.DuplicateUserName) && (UserController.GetUser(txtNewUserUserName.Text, false) != null))
				{
					UserController.DeleteUser(txtNewUserEmail.Text);
				}

				GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
				msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Error;
				msgBox.MessageTitle = Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Create_User_Msg;
				msgBox.MessageDetail = UserController.GetAddUserErrorMessage(ex.StatusCode);
				pnlAddUserMessage.Controls.Add(msgBox);

				FindControlRecursive(dgAddUser, "pnlAddUserMessage").RenderControl(e.Output);
			}
			catch (Exception ex)
			{
				// Just in case we created the user and the exception occured at a later step, like ading the roles, delete the user.
				if (UserController.GetUser(txtNewUserUserName.Text, false) != null)
				{
					UserController.DeleteUser(txtNewUserUserName.Text);
				}

				GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
				msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Error;
				msgBox.MessageTitle = Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Create_User_Msg;
				msgBox.MessageDetail = ex.Message;
				pnlAddUserMessage.Controls.Add(msgBox);

				FindControlRecursive(dgAddUser, "pnlAddUserMessage").RenderControl(e.Output);
			}

		}

		protected void cbEditUser_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
		{
			// There should be two strings in the Parameters property. The first indicates the requested operation
			// (edit, save, etc). The second is the name of the role.
			string userName = String.Empty;
			try
			{
				if (e.Parameters.Length != 2)
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The cbEditUser_Callback is expecting two items in the parameter list, but received {0} items. The first item should be a string equal to 'add', 'edit', or 'save', and the second item should be a string containing the user name (should be empty if the first parameter = 'add').", e.Parameters.Length));

				userName = e.Parameters[1];
				switch (e.Parameter)
				{
					case "edit":
						{
							PopulateControlsWithUserData(userName, true);

							//// Render the controls with the updated information.
							FindControlRecursive(dgEditUser, "pnlEditUserDialogContent").RenderControl(e.Output);

							break;
						}
					case "save":
						{
							SaveUser(userName);

							// Write a little javascript to set a page variable that will be used during the OnCallbackComplete
							// event to close the dialog when the call is successful.
							e.Output.Write("<script type=\"text/javascript\">callbackStatus = 'success';</script>");

							break;
						}
					case "updatePassword":
						{
							UpdatePassword(userName);

							PopulateControlsWithUserData(userName, false);

							// Render the controls with the updated information.
							FindControlRecursive(dgEditUser, "pnlEditUserDialogContent").RenderControl(e.Output);

							break;
						}
					case "unlockUser":
						{
							UnlockUser(userName);

							PopulateControlsWithUserData(userName, false);

							// Render the controls with the updated information.
							FindControlRecursive(dgEditUser, "pnlEditUserDialogContent").RenderControl(e.Output);

							break;
						}
				}
			}
			catch (Exception ex)
			{
				GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
				msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Error;
				msgBox.MessageTitle = Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Save_User_Hdr;
				msgBox.MessageDetail = ex.Message;
				phEditUserMessage.Controls.Add(msgBox);

				lblUserName.Text = userName; // Not persisted by viewstate, so we have to set again
				FindControlRecursive(dgEditUser, "pnlEditUserDialogContent").RenderControl(e.Output);
			}
		}

		protected void cblAvailableRolesForExistingUser_DataBound(object sender, EventArgs e)
		{
			foreach (ListItem item in cblAvailableRolesForExistingUser.Items)
			{
				// HTML encode the values.
				item.Text = Util.HtmlEncode(item.Text);
				item.Value = Util.HtmlEncode(item.Value);
			}
		}

		protected void cblAvailableRolesForNewUser_DataBound(object sender, EventArgs e)
		{
			foreach (ListItem checkbox in cblAvailableRolesForNewUser.Items)
			{
				// Mark all roles with a special class so that jQuery can show/hide them.
				if (RoleController.IsRoleAnAlbumOwnerRole(checkbox.Value))
				{
					checkbox.Attributes["class"] = "gsp_j_aaor";
				}

				// HTML encode the values.
				checkbox.Text = Util.HtmlEncode(checkbox.Text);
				checkbox.Value = Util.HtmlEncode(checkbox.Value);
			}
		}

		#endregion

		#region Public Properties

		#region Controls in dgEditUser

		public TabStrip tsEditUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "tsEditUser");
				if (ctrl == null) throw new WebException("Cannot find TabStrip tsEditUser.");
				return (TabStrip)ctrl;
			}
		}

		public PlaceHolder phEditUserMessage
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "phEditUserMessage");
				if (ctrl == null) throw new WebException("Cannot find PlaceHolder phEditUserMessage.");
				return (PlaceHolder)ctrl;
			}
		}

		public PlaceHolder phDialogMessagePasswordTab
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "phDialogMessagePasswordTab");
				if (ctrl == null) throw new WebException("Cannot find PlaceHolder phDialogMessagePasswordTab.");
				return (PlaceHolder)ctrl;
			}
		}

		public Label lblUserName
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "lblUserName");
				if (ctrl == null) throw new WebException("Cannot find Label lblUserName.");
				return (Label)ctrl;
			}
		}

		public TextBox txtComment
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "txtComment");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtComment.");
				return (TextBox)ctrl;
			}
		}

		public TextBox txtEmail
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "txtEmail");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtEmail.");
				return (TextBox)ctrl;
			}
		}

		public RadioButton rbApprovedYes
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "rbApprovedYes");
				if (ctrl == null) throw new WebException("Cannot find RadioButton rbApprovedYes.");
				return (RadioButton)ctrl;
			}
		}

		public RadioButton rbApprovedNo
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "rbApprovedNo");
				if (ctrl == null) throw new WebException("Cannot find RadioButton rbApprovedNo.");
				return (RadioButton)ctrl;
			}
		}

		public HtmlTableRow trUserAlbumTableRow
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "trUserAlbumTableRow");
				if (ctrl == null) throw new WebException("Cannot find HtmlTableRow trUserAlbumTableRow.");
				return (HtmlTableRow)ctrl;
			}
		}

		public RadioButton rbUserAlbumYes
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "rbUserAlbumYes");
				if (ctrl == null) throw new WebException("Cannot find RadioButton rbUserAlbumYes.");
				return (RadioButton)ctrl;
			}
		}

		public RadioButton rbUserAlbumNo
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "rbUserAlbumNo");
				if (ctrl == null) throw new WebException("Cannot find RadioButton rbUserAlbumNo.");
				return (RadioButton)ctrl;
			}
		}

		public Label lblLastActivityDate
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "lblLastActivityDate");
				if (ctrl == null) throw new WebException("Cannot find Label lblLastActivityDate.");
				return (Label)ctrl;
			}
		}

		public Label lblLastLogOnDate
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "lblLastLogOnDate");
				if (ctrl == null) throw new WebException("Cannot find Label lblLastLogOnDate.");
				return (Label)ctrl;
			}
		}

		public Label lblLastPasswordChangedDate
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "lblLastPasswordChangedDate");
				if (ctrl == null) throw new WebException("Cannot find Label lblLastPasswordChangedDate.");
				return (Label)ctrl;
			}
		}

		public Label lblCreationDate
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "lblCreationDate");
				if (ctrl == null) throw new WebException("Cannot find Label lblCreationDate.");
				return (Label)ctrl;
			}
		}

		public CheckBoxList cblAvailableRolesForExistingUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "cblAvailableRolesForExistingUser");
				if (ctrl == null) throw new WebException("Cannot find CheckBoxList cblAvailableRolesForExistingUser.");
				return (CheckBoxList)ctrl;
			}
		}

		public RadioButton rbResetPassword
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "rbResetPassword");
				if (ctrl == null) throw new WebException("Cannot find RadioButton rbResetPassword.");
				return (RadioButton)ctrl;
			}
		}

		public RadioButton rbChangePassword
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "rbChangePassword");
				if (ctrl == null) throw new WebException("Cannot find RadioButton rbChangePassword.");
				return (RadioButton)ctrl;
			}
		}

		public TextBox txtPassword1
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "txtPassword1");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtPassword1.");
				return (TextBox)ctrl;
			}
		}

		public TextBox txtPassword2
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "txtPassword2");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtPassword2.");
				return (TextBox)ctrl;
			}
		}

		public Label lblNotMatchingPasswords
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "lblNotMatchingPasswords");
				if (ctrl == null) throw new WebException("Cannot find Label lblNotMatchingPasswords.");
				return (Label)ctrl;
			}
		}

		public CheckBox chkEmailNewPasswordToUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "chkEmailNewPasswordToUser");
				if (ctrl == null) throw new WebException("Cannot find CheckBox chkEmailNewPasswordToUser.");
				return (CheckBox)ctrl;
			}
		}

		public CallBack cbEditUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "cbEditUser");
				if (ctrl == null) throw new WebException("Cannot find CallBack cbEditUser.");
				return (CallBack)ctrl;
			}
		}

		public CheckBox chkShowOwnerRoles
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgEditUser, "chkShowOwnerRoles");
				if (ctrl == null) throw new WebException("Cannot find CheckBox chkShowOwnerRoles.");
				return (CheckBox)ctrl;
			}
		}

		#endregion

		#region Controls in dgAddUser

		public CallBack cbAddUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "cbAddUser");
				if (ctrl == null) throw new WebException("Cannot find CallBack cbAddUser.");
				return (CallBack)ctrl;
			}
		}

		public Panel pnlAddUserMessage
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "pnlAddUserMessage");
				if (ctrl == null) throw new WebException("Cannot find Panel pnlAddUserMessage.");
				return (Panel)ctrl;
			}
		}

		public TextBox txtNewUserUserName
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "txtNewUserUserName");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtNewUserUserName.");
				return (TextBox)ctrl;
			}
		}

		public Label lblUserNameValidationResult
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "lblUserNameValidationResult");
				if (ctrl == null) throw new WebException("Cannot find Label lblUserNameValidationResult.");
				return (Label)ctrl;
			}
		}

		public TextBox txtNewUserEmail
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "txtNewUserEmail");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtNewUserEmail.");
				return (TextBox)ctrl;
			}
		}

		public TextBox txtNewUserPassword1
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "txtNewUserPassword1");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtNewUserPassword1.");
				return (TextBox)ctrl;
			}
		}

		public TextBox txtNewUserPassword2
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "txtNewUserPassword2");
				if (ctrl == null) throw new WebException("Cannot find TextBox txtNewUserPassword2.");
				return (TextBox)ctrl;
			}
		}

		public CheckBoxList cblAvailableRolesForNewUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "cblAvailableRolesForNewUser");
				if (ctrl == null) throw new WebException("Cannot find CheckBoxList cblAvailableRolesForNewUser.");
				return (CheckBoxList)ctrl;
			}
		}

		public MultiPage mpAddUser
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "mpAddUser");
				if (ctrl == null) throw new WebException("Cannot find MultiPage mpAddUser.");
				return (MultiPage)ctrl;
			}
		}

		public HtmlInputButton btnAddUserNext
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "btnAddUserNext");
				if (ctrl == null) throw new WebException("Cannot find HtmlInputButton btnAddUserNext.");
				return (HtmlInputButton)ctrl;
			}
		}

		public HtmlInputButton btnAddUserComplete
		{
			get
			{
				Control ctrl = this.FindControlRecursive(dgAddUser, "btnAddUserComplete");
				if (ctrl == null) throw new WebException("Cannot find HtmlInputButton btnAddUserComplete.");
				return (HtmlInputButton)ctrl;
			}
		}

		#endregion

		#endregion

		#region Private Methods

		private void ProcessQueryString()
		{
			if (Util.GetQueryStringParameterString("action") == "add")
			{
				// We want to invoke the add user dialog, so call the addUser() javascript function when the page loads.
				string script = GetPageLoadScript("addUser();");
				ScriptManager.RegisterStartupScript(this, this.GetType(), "startupScript", script, true);
			}
			else if ((Util.GetQueryStringParameterString("action") == "edit") && (!String.IsNullOrEmpty(Util.GetQueryStringParameterString("user"))))
			{
				// We want to invoke the edit user dialog, so call the editUser() javascript function when the page loads.
				try
				{
					string userName = HelperFunctions.Decrypt(Util.GetQueryStringParameterString("user"));

					string script = GetPageLoadScript(String.Format(CultureInfo.InvariantCulture, "editUser('{0}');", userName));

					ScriptManager.RegisterStartupScript(this, this.GetType(), "startupScript", script, true);
				}
				catch (FormatException ex)
				{
					AppErrorController.LogError(ex);
				}
			}
		}

		private void ConfigureControls()
		{
			if (AppSetting.Instance.IsInReducedFunctionalityMode)
			{
				wwMessage.ShowMessage(Resources.GalleryServerPro.Admin_Need_Product_Key_Msg2);
				wwMessage.CssClass = "wwErrorSuccess gsp_msgwarning";
				pAddUser.Visible = false;
			}

			this.PageTitle = Resources.GalleryServerPro.Admin_Manage_Users_Page_Header;

			ConfigureGrid();

			ConfigureDialogs();

			ConfigureTabStrip();

			RegisterJavaScript();
		}

		private void ConfigureDialogs()
		{
			AddLoadingPanelClientTemplate();

			trUserAlbumTableRow.Visible = Core.EnableUserAlbum;
		}

		private void AddLoadingPanelClientTemplate()
		{
			// Add loading panel client template
			ComponentArt.Web.UI.ClientTemplate loadingPanelClientTemplate = new ComponentArt.Web.UI.ClientTemplate();

			loadingPanelClientTemplate.Text = String.Format(CultureInfo.InvariantCulture, @"
<p class=""gsp_h3 gsp_textcenter"" style=""margin-top: 200px;"">{0}</p>
<p class=""gsp_textcenter""><img src=""{1}"" style=""width: 16px; height: 16px;""	alt="""" /></p>",
			                                                Resources.GalleryServerPro.Admin_Manage_Users_Processing_Text,
			                                                Util.GetUrl("images/componentart/callback/spinner.gif"));

			cbEditUser.LoadingPanelClientTemplate = loadingPanelClientTemplate;
			cbAddUser.LoadingPanelClientTemplate = loadingPanelClientTemplate;
		}

		private void ConfigureGrid()
		{
			gdUsers.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/grid/");
			gdUsers.PagerImagesFolderUrl = String.Concat(Util.GalleryRoot, "/images/componentart/grid/pager/");

			if (UserController.GetUserNames().Rows.Count > gdUsers.PageSize)
				gdUsers.AutoFocusSearchBox = false;

			if (!AppSetting.Instance.IsInReducedFunctionalityMode)
				AddEditColumnClientTemplate();
		}

		private ComponentArt.Web.UI.ClientTemplate AddEditColumnClientTemplate()
		{
			// Add edit column client template
			ComponentArt.Web.UI.ClientTemplate userEditColumn = new ComponentArt.Web.UI.ClientTemplate();

			userEditColumn.ID = "userEditColumn";
			userEditColumn.Text = String.Format(CultureInfo.InvariantCulture, @"<p>
						<a id=""## makeValidForId(DataItem.getMember('UserName').get_value()) ##"" href=""javascript:editUser(decodeURI('## getUserName(DataItem) ##'));""
							title=""{0}"">{1}</a> <a href=""javascript:hideUserMessage();if (ConfirmDelete(decodeURI('## getUserName(DataItem) ##'))) gdUsers.deleteItem(gdUsers.getItemFromKey(0, '## getUserNameNoEncode(DataItem) ##'));"">
								{2}</a>
					</p>",
			                                    Resources.GalleryServerPro.Admin_Manage_Roles_Edit_User_Tooltip_Text,
			                                    Resources.GalleryServerPro.Admin_Manage_Roles_Edit_User_Hyperlink_Text,
			                                    Resources.GalleryServerPro.Admin_Manage_Roles_Delete_User_Hyperlink_Text);

			gdUsers.ClientTemplates.Add(userEditColumn);
			return userEditColumn;
		}

		//    private void AddPagerClientTemplate()
		//    {
		//      // Add pager client template
		//      ComponentArt.Web.UI.ClientTemplate pagerTemplate = new ComponentArt.Web.UI.ClientTemplate();

		//      pagerTemplate.ID = "pagerTemplate";
		//      pagerTemplate.Text = String.Format(@"<div class='gd0SliderPopup'>
		//						<div class='sliderHeader'>
		//							<p>
		//								## DataItem.GetMember('UserName').Value ##</p>
		//						</div>
		//						<div class='sliderFooter'>
		//								<table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0'>
		//									<tr>
		//										<td class='gsp_addleftpadding2'>
		//											{0} <b>## DataItem.PageIndex + 1 ##</b> {1} <b>## gdUsers.PageCount ##</b>
		//										</td>
		//										<td class='gsp_addrightpadding2' style='text-align:right;'>
		//											{2} <b>## DataItem.Index + 1 ##</b> {3} <b>## gdUsers.RecordCount ##</b>
		//										</td>
		//									</tr>
		//								</table>
		//						</div>
		//					</div>",
		//        Resources.GalleryServerPro.Admin_Manage_Users_Grid_Pager_Page_Text,
		//        Resources.GalleryServerPro.Admin_Manage_Users_Grid_Pager_Page_Spacer_Text,
		//        Resources.GalleryServerPro.Admin_Manage_Users_Grid_Pager_Message_Text,
		//        Resources.GalleryServerPro.Admin_Manage_Users_Grid_Pager_Page_Spacer_Text);

		//      gdUsers.ClientTemplates.Add(pagerTemplate);
		//    }

		private void ConfigureTabStrip()
		{
			tsEditUser.ImagesBaseUrl = String.Concat(Util.GalleryRoot, "/images/componentart/tabstrip/");
			tsEditUser.TopGroupSeparatorImagesFolderUrl = String.Concat(Util.GalleryRoot, "/images/componentart/tabstrip/");

			foreach (TabStripTab tab in tsEditUser.Tabs)
			{
				switch (tab.ID)
				{
					case "tabGeneral": tab.Text = Resources.GalleryServerPro.Admin_Manage_Users_Edit_User_General_Tab_Text; break;
					case "tabRoles": tab.Text = Resources.GalleryServerPro.Admin_Manage_Users_Edit_User_Roles_Tab_Text; break;
					case "tabPassword": tab.Text = Resources.GalleryServerPro.Admin_Manage_Users_Edit_User_Password_Tab_Text; break;
				}
			}
		}

		/// <summary>
		/// Set the properties of the controls in the dialog with user-specific data. This method assumes the controls, such as
		/// the CheckBoxList of roles have already been databound. The CallBack control automatically preserves viewstate of 
		/// user-writeable controls such as textboxes and radio buttons, so during callbacks within the open dialog - such as 
		/// changing the user's password - we only want to refresh the read-only controls such as labels (since they lost 
		/// their info during the postback). Use the updateWriteableControls to control this.
		/// </summary>
		/// <param name="userName">The user name whose information should be bound to the controls.</param>
		/// <param name="updateWriteableControls">Indicates whether read-only controls such as labels should be updated, or all 
		/// controls, even writeable ones such as textboxes should be updated. Specify true to update all controls or false
		/// to only update read-only controls.</param>
		private void PopulateControlsWithUserData(string userName, bool updateWriteableControls)
		{
			UserEntity user = UserController.GetUser(userName, false);

			if (user == null)
				throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Invalid_User_Text);

			CheckForLockedUser(user);

			BindUserInfoControls(user, updateWriteableControls);

			BindRolePermissionCheckboxes(user);

			BindPasswordControls(HelperFunctions.IsValidEmail(user.Email));
		}

		private void CheckForLockedUser(UserEntity user)
		{
			if (!user.IsLockedOut) return;

			string msgHeader = Resources.GalleryServerPro.Admin_Manage_Users_Locked_User_Hdr;
			string msgDetail = String.Format(CultureInfo.CurrentCulture, "{0} <a href=\"javascript:unlockUser()\">{1}</a>", Resources.GalleryServerPro.Admin_Manage_Users_Locked_User_Dtl, Resources.GalleryServerPro.Admin_Manage_Users_Unlock_User_Hyperlink_Text);

			GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
			msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Information;
			msgBox.MessageTitle = msgHeader;
			msgBox.MessageDetail = msgDetail;
			phEditUserMessage.Controls.Add(msgBox);
		}

		private void BindUserInfoControls(UserEntity user, bool updateWriteableControls)
		{
			lblUserName.Text = Util.HtmlEncode(user.UserName);

			if (updateWriteableControls)
			{
				txtComment.Text = user.Comment;
				txtEmail.Text = user.Email;
				rbApprovedYes.Checked = user.IsApproved;
				rbApprovedNo.Checked = !user.IsApproved;

				ProfileEntity profile = ProfileController.GetProfile(user.UserName);
				rbUserAlbumYes.Checked = profile.EnableUserAlbum;
				rbUserAlbumNo.Checked = !profile.EnableUserAlbum;
			}

			try
			{
				lblCreationDate.Text = String.Format(CultureInfo.CurrentCulture, "{0:F} (GMT)", user.CreationDate);
				lblLastActivityDate.Text = String.Format(CultureInfo.CurrentCulture, "{0:F} (GMT)", user.LastActivityDate);
				lblLastLogOnDate.Text = String.Format(CultureInfo.CurrentCulture, "{0:F} (GMT)", user.LastLoginDate);
				lblLastPasswordChangedDate.Text = String.Format(CultureInfo.CurrentCulture, "{0:F} (GMT)", user.LastPasswordChangedDate);
			}
			catch (NotSupportedException) { /* Ignore if provider does not support one or more properties */}
		}

		/// <summary>
		/// Select the checkboxes corresponding to the roles to which the specified user belong. This method assumes the checkboxlist 
		/// has already been created and databound with the objectdatasource.
		/// </summary>
		/// <param name="user">An instance of <see cref="UserEntity"/> that represents a user in the application.</param>
		private void BindRolePermissionCheckboxes(UserEntity user)
		{
			cblAvailableRolesForExistingUser.ClearSelection();
			string[] rolesForUser = RoleController.GetRolesForUser(user.UserName);
			foreach (ListItem checkbox in cblAvailableRolesForExistingUser.Items)
			{
				string checkboxRoleName = Util.HtmlDecode(checkbox.Value);
				if (Array.Exists(rolesForUser, delegate(string roleName)
				                               	{
				                               		return (roleName == checkboxRoleName);
				                               	}))
				{
					checkbox.Selected = true;
				}

				if (RoleController.IsRoleAnAlbumOwnerRole(checkboxRoleName))
				{
					checkbox.Attributes["class"] = "gsp_j_eaor";
				}
			}
		}

		private void BindPasswordControls(bool userHasValidEmail)
		{
			bool needToShowMsg = false;
			GalleryServerPro.Web.MessageStyle iconStyle = GalleryServerPro.Web.MessageStyle.Information;
			string msg = "<ul style='list-style-type:disc;'>";

			if (UserController.RequiresQuestionAndAnswer)
			{
				msg += String.Format(CultureInfo.CurrentCulture, "<li>{0}</li>", Resources.GalleryServerPro.Admin_Manage_Users_Question_Answer_Enabled_Msg);
				needToShowMsg = true;
				// disable 1 and 2
				iconStyle = GalleryServerPro.Web.MessageStyle.Warning;
				rbChangePassword.Checked = false;
				rbChangePassword.Enabled = false;
				rbResetPassword.Checked = false;
				rbResetPassword.Enabled = false;
				txtPassword1.Enabled = false;
				txtPassword2.Enabled = false;
				chkEmailNewPasswordToUser.Enabled = false;
				chkEmailNewPasswordToUser.CssClass = "gsp_disabledtext";
			}

			if (!UserController.EnablePasswordReset)
			{
				msg = String.Format(CultureInfo.CurrentCulture, "<li>{0}</li>", Resources.GalleryServerPro.Admin_Manage_Users_Pwd_Rest_Disabled_Msg);
				needToShowMsg = true;
				// disable 1
				rbResetPassword.Checked = false;
				rbResetPassword.Enabled = false;
				rbChangePassword.Checked = rbChangePassword.Enabled & true;
			}

			if (!UserController.EnablePasswordRetrieval)
			{
				msg += String.Format(CultureInfo.CurrentCulture, "<li>{0}</li>", Resources.GalleryServerPro.Admin_Manage_Users_Pwd_Retrieval_Disabled_Msg);
				needToShowMsg = true;
				// disable 2
				rbChangePassword.Checked = false;
				rbChangePassword.Enabled = false;
				txtPassword1.Enabled = false;
				txtPassword2.Enabled = false;
				rbResetPassword.Checked = rbResetPassword.Enabled & true;
			}

			if (!userHasValidEmail)
			{
				msg += String.Format(CultureInfo.CurrentCulture, "<li>{0}</li>", Resources.GalleryServerPro.Admin_Manage_Users_No_User_Email_Msg);
				needToShowMsg = true;
				chkEmailNewPasswordToUser.Enabled = false;
				chkEmailNewPasswordToUser.CssClass = "gsp_disabledtext";
			}

			// Don't need to check the following situation: When web.config is set this way, it throws a Configuration Exception during app startup.
			//if ((Membership.EnablePasswordRetrieval) && (Membership.Provider.PasswordFormat == MembershipPasswordFormat.Hashed))
			//  msg += "<li>Cannot change password. Gallery Server is configured to store passwords in a hashed format, which means they cannot be retrieved. Since the Membership provider requires the original password when changing it, you are unable to change the password. You are able, however, to reset the password.</li>";

			if (needToShowMsg)
			{
				msg += "</ul>";
				GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
				msgBox.IconStyle = iconStyle;
				msgBox.MessageTitle = Resources.GalleryServerPro.Admin_Manage_Users_Msg_Hdr;
				msgBox.MessageDetail = msg;
				msgBox.CssClass = "um1ContainerCss";
				msgBox.HeaderCssClass = "um1HeaderCss";
				msgBox.DetailCssClass = "um1DetailCss";
				phDialogMessagePasswordTab.Controls.Add(msgBox);
			}
		}

		private void SaveUser(string userName)
		{
			ValidateSaveUser(userName);

			// Step 1: Update general info (1st tab).
			UserEntity user = UserController.GetUser(userName, false);

			user.Email = txtEmail.Text;
			user.Comment = txtComment.Text;

			if (!user.IsApproved && rbApprovedYes.Checked)
			{
				// Administrator is approving user. Send notification e-mail to user.
				EmailController.SendNotificationEmail(user, EmailTemplateForm.UserNotificationAccountCreatedApprovalGiven);
			}

			user.IsApproved = rbApprovedYes.Checked;

			UserController.UpdateUser(user);

			// Step 2: Update role membership (2nd tab).
			foreach (ListItem checkbox in cblAvailableRolesForExistingUser.Items)
			{
				string roleName = Util.HtmlDecode(checkbox.Value);
				if (checkbox.Selected)
				{
					// Make sure user is in this role.
					if (!RoleController.IsUserInRole(userName, roleName))
					{
						RoleController.AddUserToRole(userName, roleName);
					}
				}
				else
				{
					// Make sure user is NOT in this role.
					if (RoleController.IsUserInRole(userName, roleName))
					{
						RoleController.RemoveUserFromRole(userName, roleName);

						ValidateRemoveUserFromRole(userName, roleName);
					}
				}
			}

			// Step 3: Update user's profile.
			SaveProfile(userName);
		}

		private static void ValidateRemoveUserFromRole(string userName, string roleName)
		{
			// If role is an album owner role, then check all albums in that role to see if current user is
			// an owner for any. If he is, clear out the ownership field.
			if (RoleController.IsRoleAnAlbumOwnerRole(roleName))
			{
				IGalleryServerRole role = Factory.LoadGalleryServerRole(roleName);
				foreach (int albumId in role.RootAlbumIds)
				{
					IAlbum album = Factory.LoadAlbumInstance(albumId, false);
					if (album.OwnerUserName == userName)
					{
						album.OwnerUserName = String.Empty;
						GalleryObjectController.SaveGalleryObject(album);
					}
				}
			}
		}

		private void ValidateSaveUser(string userName)
		{
			// Make sure the loggod-on person has authority to save the user info and that 
			// h/she isn't doing anything stupid, like removing admin permission from his
			// or her own account.
			this.CheckUserSecurity(SecurityActions.AdministerSite);

			if (userName.Equals(Util.UserName, StringComparison.OrdinalIgnoreCase))
			{
				// The logged on person is updating their own account. They are not allowed to revoke approval
				// and they must remain in at least one role that has Administer Site permission.
				bool isApproved = rbApprovedYes.Checked;

				if (!isApproved)
				{
					throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Revoke_Approval_Msg);
				}

				bool hasAdminPermission = false;
				foreach (ListItem checkbox in cblAvailableRolesForExistingUser.Items)
				{
					string roleName = Util.HtmlDecode(checkbox.Value);
					if (checkbox.Selected)
					{
						IGalleryServerRole galleryRole = Factory.LoadGalleryServerRole(roleName);
						if (galleryRole.AllowAdministerSite)
						{
							hasAdminPermission = true;
							break;
						}
					}
				}

				if (!hasAdminPermission)
				{
					throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Cannot_Save_User_Msg);
				}
			}
		}

		private void SaveProfile(string userName)
		{
			if (!Core.EnableUserAlbum)
				return; // User albums are disabled system-wide, so there is nothing to save.

			ProfileEntity userProfile = ProfileController.GetProfile(userName);

			userProfile.EnableUserAlbum = rbUserAlbumYes.Checked;

			if (!rbUserAlbumYes.Checked)
			{
				userProfile.UserAlbumId = 0;
			}

			if (!userProfile.EnableUserAlbum)
			{
				AlbumController.DeleteAlbum(UserController.GetUserAlbum(userName));
			}

			ProfileController.SaveProfile(userProfile);
		}

		private void UpdatePassword(string userName)
		{
			GalleryServerPro.Web.MessageStyle iconStyle = GalleryServerPro.Web.MessageStyle.Information;
			string msgTitle = Resources.GalleryServerPro.Admin_Manage_Users_Pwd_Changed_Text;
			string msgDetail = String.Empty;

			#region Update Password

			UserEntity user = UserController.GetUser(userName, false);
			if (rbResetPassword.Checked)
			{
				string newPassword = UserController.ResetPassword(userName);
				string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Manage_Users_New_Pwd_Text, newPassword);
				msgDetail = String.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", msg);
			}
			else if (rbChangePassword.Checked)
			{
				string newPassword = this.txtPassword1.Text;
				if (newPassword != txtPassword2.Text)
				{
					lblNotMatchingPasswords.Visible = true;
					return;
				}

				if (!String.IsNullOrEmpty(newPassword))
				{
					UserController.ChangePassword(userName, UserController.GetPassword(userName), newPassword);
				}
				else
				{
					msgTitle = Resources.GalleryServerPro.Admin_Manage_Users_No_Pwd_Supplied_Hdr;
					msgDetail = Resources.GalleryServerPro.Admin_Manage_Users_No_Pwd_Supplied_Dtl;
					iconStyle = GalleryServerPro.Web.MessageStyle.Warning;
				}
			}

			#endregion

			#region Email User

			if (chkEmailNewPasswordToUser.Checked)
			{
				if (HelperFunctions.IsValidEmail(user.Email))
				{
					try
					{
						EmailController.SendNotificationEmail(user, EmailTemplateForm.UserNotificationPasswordChangedByAdmin, false);
					}
					catch (Exception ex)
					{
						string msg = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Admin_Manage_Users_Pwd_Change_Email_Error_Msg, Util.GetExceptionDetails(ex));
						msgDetail += String.Concat("<p>", msg, "</p>");
					}
				}
				else
				{
					msgDetail += String.Format(CultureInfo.CurrentCulture, "<p>{0}</p>", Resources.GalleryServerPro.Admin_Manage_Users_Pwd_Change_Email_Invalid_Msg);
				}
			}

			#endregion

			#region Render Confirmation Message

			phDialogMessagePasswordTab.Controls.Clear();
			GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
			msgBox.IconStyle = iconStyle;
			msgBox.MessageTitle = msgTitle;
			msgBox.MessageDetail = msgDetail;
			msgBox.CssClass = "um1ContainerCss";
			msgBox.HeaderCssClass = "um1HeaderCss";
			msgBox.DetailCssClass = "um1DetailCss";
			phDialogMessagePasswordTab.Controls.Add(msgBox);

			#endregion
		}

		private void UnlockUser(string userName)
		{
			UserController.UnlockUser(userName);

			#region Render Confirmation Message

			string msgTitle = Resources.GalleryServerPro.Admin_Manage_Users_User_Unlocked_Msg;

			phEditUserMessage.Controls.Clear();
			GalleryServerPro.Web.Controls.usermessage msgBox = (GalleryServerPro.Web.Controls.usermessage)LoadControl(Util.GetUrl("/controls/usermessage.ascx"));
			msgBox.IconStyle = GalleryServerPro.Web.MessageStyle.Information;
			msgBox.MessageTitle = msgTitle;
			phEditUserMessage.Controls.Add(msgBox);

			#endregion
		}

		private void AddUser()
		{
			string newUserName = txtNewUserUserName.Text;
			string newUserPassword1 = txtNewUserPassword1.Text;
			string newUserPassword2 = txtNewUserPassword2.Text;

			if (newUserPassword1 != newUserPassword2)
				throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Passwords_Not_Matching_Error);

			UserController.CreateUser(newUserName, newUserPassword1, txtNewUserEmail.Text, GetSelectedRolesForNewUser(), false);
		}

		private void RegisterJavaScript()
		{
			// Add reference to a few script files.
			ScriptManager sm = ScriptManager.GetCurrent(this.Page);
			if (sm != null)
			{
				sm.Scripts.Add(new ScriptReference(Util.JQueryPath));
			}

			string script = String.Format(CultureInfo.InvariantCulture, @"
			var callbackStatus = ''; // Used during and after callbacks to indicate success or failure
			var gridNeedsRefresh = false; // Set to true when adding a user so that the grid reloads.

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

			function ConfirmDelete(userName)
			{{
				return confirm('{0} ' + userName + '?');
			}}

			function editUser(userName)
			{{
				hideUserMessage();

				if(dgEditUser.get_isShowing())
				{{
					dgEditUser.Close();
				}}
				else if (dgAddUser.get_isShowing())
				{{
					dgAddUser.Close();
				}}
				else
				{{
					setText($get('editUserDialogHeader'), '{1} - ' + userName);
					dgEditUser.beginUpdate();

					if (gdUsersLoaded)
						dgEditUser.set_animationDirectionElement(makeValidForId(userName));
					else
						dgEditUser.set_animationDirectionElement('addnewuser');

					dgEditUser.set_value(userName);
					dgEditUser.set_title('{1}: ' + userName);
					dgEditUser.endUpdate();
					dgEditUser.Show();
					cbEditUser.callback('edit', userName);
				}}
			}}

			function cbEditUser_OnCallbackComplete(sender, e)
			{{
				if (callbackStatus == 'success')
				{{
					dgEditUser.Close();
					callbackStatus = '';
				}}

				var chkShowOwnerRoles = $get('{13}');
				if ((chkShowOwnerRoles != null) && (chkShowOwnerRoles.checked == false))
					hideOwnerRoles();
			}}

			function cbEditUser_OnBeforeCallback(sender, e)
			{{
				if (tsEditUser != null)
					tsEditUser.dispose();

				if (mpEditUser != null)
					mpEditUser.dispose();
			}}

			function saveUser()
			{{
				var userName = dgEditUser.get_value();
				cbEditUser.callback('save', userName);
			}}

			function updatePassword()
			{{
				var userName = dgEditUser.get_value();
				cbEditUser.callback('updatePassword', userName);
			}}

			function unlockUser()
			{{
				var userName = dgEditUser.get_value();
				cbEditUser.callback('unlockUser', userName);
			}}
	
			function addUser()
			{{
				if (dgEditUser.get_isShowing())
				{{
					dgEditUser.Close();
				}}
				else if(dgAddUser.get_isShowing())
				{{
					dgAddUser.Close();
				}}
				else
				{{
					$get('{3}').disabled = true;
					mpAddUser.goFirst();
					dgAddUser.Show();
					$get('{2}').focus();
				}}
			}}
	
			function createAnotherUser()
			{{
				var qs = removeQSParm(window.location.search, 'action')
				window.location.search = qs + '&action=add';
			}}

			function removeQSParm(url, param)
			{{
				// Note: Requires param to be after a '&'
				var re = new RegExp('(&)' + param + '=.*?(&|$)', 'i');
				if (url.match(re))
					return url.replace(re, '$2');
				else
					return url;
			}}

			function addUserWizard_onCompleteStep1()
			{{
					validateNewUser();
					mpAddUser.goNext();
			}}

			function addUserWizard_onCompleteStep2()
			{{
				cbAddUser.callback('createUser');
			}}

			function addUserWizard_onPreviousClickStep2()
			{{
				hideAddUserMessage();
				mpAddUser.goFirst();
			}}

			function closeAddUserWizard()
			{{
				hideAddUserMessage();
				dgAddUser.Close('cancelled');
			}}
		
			function hideAddUserMessage()
			{{
				var pnl = $get('{8}');
				if (pnl)
					pnl.style.display = 'none';
			}}

			function cbAddUser_BeforeCallback(sender, e)
			{{
				Sys.UI.DomElement.addCssClass($get('{12}'), 'gsp_invisible');
			}}

			function cbAddUser_OnCallbackComplete(sender, e)
			{{
				Sys.UI.DomElement.removeCssClass($get('{12}'), 'gsp_invisible');
				if (callbackStatus == 'success')
				{{
					mpAddUser.goLast();
					gdUsers.callback();
					callbackStatus = '';
					setText($get('newUsername'), $get('{9}').value);
				}}
			}}

			function validateNewUserName(userNameTextbox)
			{{
				var newUserName = userNameTextbox.value;
				cbValidateNewUserName.callback(newUserName);
			}}
	
			function validateNewUser()
			{{
				var foundValidationError = false;
				var newUsername = $get('{4}').value;
				var newUserPW1 = $get('{5}').value;
				var newUserPW2 = $get('{6}').value;
				
				if (newUsername.length == 0) foundValidationError = true;
				if (newUserPW1.length == 0) foundValidationError = true;
				if (newUserPW2.length == 0) foundValidationError = true;
				
				if (newUserPW1 != newUserPW2) foundValidationError = true;
				
				// Make sure at least one role is checked
				var roleCheckBoxContainer = $get('{7}');
				var inputTags = roleCheckBoxContainer.getElementsByTagName('input');
				var atLeastOneRoleIsChecked = false;
				for(var iterator=0; iterator < inputTags.length; iterator++)
				{{
					if ((inputTags[iterator].type == 'checkbox') && (inputTags[iterator].checked == true))
					{{
						atLeastOneRoleIsChecked = true; break;
					}}
				}}
				
				if (!atLeastOneRoleIsChecked) foundValidationError = true;
				
				// If we get here we have passed validation. Enable Create User button.
				var createUserBtn = $get('{3}');
				createUserBtn.disabled = foundValidationError;
			}}

			function gdUsers_CallbackError(sender, args)
			{{
				showUserMessage(args.get_errorMessage());

				gdUsers.callback();
			}}

			function showUserMessage(msg)
			{{
				setText($get('{10}'), msg);

				var usrMsg = $get('{11}');
				if (Sys.UI.DomElement.containsCssClass(usrMsg, 'gsp_invisible'))
				{{
					Sys.UI.DomElement.removeCssClass(usrMsg, 'gsp_invisible');
				}}
			}}

			function hideUserMessage()
			{{
				var usrMsg = $get('{11}');
				if (!Sys.UI.DomElement.containsCssClass(usrMsg, 'gsp_invisible'))
				{{
					Sys.UI.DomElement.addCssClass(usrMsg, 'gsp_invisible');
				}}
			}}

			function makeValidForId(userName)
			{{
				// Remove quotes, apostrophes, and <. Remove encoded < symbol (#%cLt#%) caused by CA.
				return userName.replace(/""/g, '').replace(/'/g, '').replace(/</g, '').replace(/#%cLt#%/g, '');
			}}

			function getUserName(dataItem)
			{{
				var userName = dataItem.getMember('UserName').get_value();
				// Escape quotes and apostrophes. Replace encoded < symbol (#%cLt#%) caused by CA with <
				return encodeURI(userName.replace(/""/g, '\\\""').replace(/\'/g, ""\\'"").replace(/#%cLt#%/g, '<'));
			}}

			function getUserNameNoEncode(dataItem)
			{{
				var userName = dataItem.getMember('UserName').get_value();
				// Escape quotes and apostrophes
				return userName.replace(/""/g, '\\\""').replace(/\'/g, ""\\'"");
			}}

",
			                              Resources.GalleryServerPro.Admin_Manage_Users_Confirm_Delete_Text, // 0
			                              Resources.GalleryServerPro.Admin_Dialog_Title_Edit_User, // 1
			                              txtNewUserUserName.ClientID, // 2
			                              btnAddUserComplete.ClientID, // 3
			                              txtNewUserUserName.ClientID, // 4
			                              txtNewUserPassword1.ClientID, // 5
			                              txtNewUserPassword2.ClientID, // 6
			                              cblAvailableRolesForNewUser.ClientID, // 7
			                              pnlAddUserMessage.ClientID, // 8
			                              txtNewUserUserName.ClientID, // 9
			                              ucUserMessage.MessageDetailContainer.ClientID, // 10
			                              ucUserMessage.MessageContainer.ClientID, // 11
			                              mpAddUser.ClientID, // 12
			                              chkShowOwnerRoles.ClientID // 13
				);

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "pageFunctions", script, true);
		}

		private string[] GetSelectedRolesForNewUser()
		{
			//// Step 2: Add roles to user.
			System.Collections.Generic.List<string> selectedRoleNames = new System.Collections.Generic.List<string>();
			foreach (ListItem roleItem in cblAvailableRolesForNewUser.Items)
			{
				if (roleItem.Selected)
				{
					selectedRoleNames.Add(roleItem.Value);
				}
			}

			if (selectedRoleNames.Count == 0)
				throw new WebException(Resources.GalleryServerPro.Admin_Manage_Users_Create_User_No_Role_Selected_Msg);

			string[] roleNames = new string[selectedRoleNames.Count];
			selectedRoleNames.CopyTo(roleNames);

			return roleNames;
		}

		#endregion
	}
}
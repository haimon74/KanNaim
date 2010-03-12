<%@ Page Language="C#" MasterPageFile="~/master/global.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="GalleryServerPro.Web.installer._default"
	Title="Gallery Server Pro Setup Wizard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="server">
	<!-- Enable or disable this installer by changing the value of the ENABLE_SETUP hidden field to true or false. -->
	<asp:HiddenField ID="ENABLE_SETUP" runat="server" Value="true" />
	<div id="wizInstall">
		<div id="wizHdr">
			<img src="../images/gsp_logo_313x75.png" style="width: 313px; height: 75px;" alt="Gallery Server Pro logo" />
		</div>
		<div id="wizCtnt">
			<asp:Panel ID="Welcome" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Hdr %>" />
				</h2>
				<p>
					<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Dtl %>" />
				</p>
				<p class="msgfriendly">
					<asp:Literal ID="l2b" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Dtl2 %>" />
				</p>
				<p class="bold addtopmargin5">
					<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Req_Hdr %>" />
				</p>
				<ul style="list-style-type: disc; margin-left: 3em;">
					<li>
						<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Req1 %>" /></li>
					<li>
						<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Req2 %>" /></li>
					<li>
						<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Req3 %>" /></li>
				</ul>
				<p class="bold addleftmargin5 addtopmargin5">
					<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Opt_Req_Hdr %>" /></p>
				<ul style="list-style-type: disc; margin-left: 3em;">
					<li>
						<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Opt_Req1 %>" /></li>
					<li>
						<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Server_Opt_Req2 %>" /></li>
				</ul>
				<p class="bold addtopmargin5">
					<asp:Literal ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Client_Req_Hdr %>" />
				</p>
				<ul style="list-style-type: disc; margin-left: 3em;">
					<li>
						<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Welcome_Client_Req1 %>" /></li>
				</ul>
			</asp:Panel>
			<asp:Panel ID="License" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l11" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_License_Hdr %>" />
				</h2>
				<p style="text-align: right; margin-top: -1em;">
					<a href="eula.txt" target="_blank">
						<asp:Literal ID="l12" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_License_Printable_Version %>" /></a></p>
				<iframe frameborder="1" scrolling="yes" src="eula.htm" style="width: 100%; height: 350px;">
				</iframe>
				<p style="text-align: right;">
					<asp:CheckBox ID="chkLicenseAgreement" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_License_Agree_Text %>" /></p>
			</asp:Panel>
			<asp:Panel ID="DataProvider" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l57" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DataProvider_Hdr %>" />
				</h2>
				<p id="lblErrMsgDataProvider" runat="server" class="invisible" enableviewstate="false" />
				<p>
					<asp:Literal ID="l58" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DataProvider_Dtl1 %>" />
				</p>
				<p>
					<asp:Literal ID="l59" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DataProvider_Dtl2 %>" />
				</p>
				<p class="bold addtopmargin5">
					<asp:Literal ID="l60" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DataProvider_Select_Hdr %>" />
				</p>
				<div class="addleftpadding5">
					<p>
						<asp:RadioButton ID="rbDataProviderSQLite" runat="server" GroupName="DataProvider" Text="SQLite" /></p>
					<p>
						<asp:RadioButton ID="rbDataProviderSqlServer" runat="server" GroupName="DataProvider" Text="SQL Server (2000, 2005, or 2008)" /></p>
				</div>
				<asp:Panel ID="pnlDbEngineMsg" runat="server" CssClass="addleftpadding5" Visible="false" EnableViewState="false">
					<img src="<%= GalleryServerPro.Web.WebsiteController.GetThemePathUrl(this.Theme) %>/images/info_32x32.png" alt="" style="width: 32px;
						height: 32px; float: left; padding-top: 0.6em;" />
					<div style="margin-left: 40px;">
						<p id="lblErrMsgChooseDbEngine" runat="server" enableviewstate="false" />
					</div>
				</asp:Panel>
			</asp:Panel>
			<asp:Panel ID="DbAdmin" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l13" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Hdr %>" />
				</h2>
				<p id="lblErrMsgDbAdmin" runat="server" class="invisible" enableviewstate="false" />
				<p>
					<asp:Literal ID="l14" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Dtl1 %>" />
				</p>
				<p class="msgfriendly">
					<asp:Literal ID="l15" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Dtl2 %>" />
				</p>
				<div>
					<p class="bold addtopmargin5">
						<asp:Literal ID="l16" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Hdr %>" />
					</p>
					<div class="addleftpadding5">
						<p class="nomargin nopadding">
							<asp:TextBox ID="txtDbSqlName" runat="server" Text="(local)" CssClass="textcol" /></p>
						<p class="fss minimargin">
							<asp:Literal ID="l17" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Name_Examples %>" />
						</p>
					</div>
					<p class="bold addtopmargin5">
						<asp:Literal ID="l18" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Auth_Hdr %>" />
					</p>
					<div class="addleftpadding5">
						<asp:RadioButtonList ID="rblDbAdminConnectType" runat="server">
						</asp:RadioButtonList>
						<div class="addtopmargin2">
							<table style="padding-left: 50px;">
								<tr>
									<td>
										<asp:Literal ID="l19" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Auth_UserName_Label %>" />
									</td>
									<td>
										<asp:TextBox ID="txtDbAdminUserName" runat="server" />&nbsp;<asp:CustomValidator ID="cvDbAdminSqlLogOn" runat="server" CssClass="msgwarning"
											Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Auth_UserName_Val_Err %>" OnServerValidate="cvDbAdminSqlLogOn_ServerValidate" />
									</td>
								</tr>
								<tr>
									<td>
										<asp:Literal ID="l20" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Auth_Pwd_Label %>" />
									</td>
									<td>
										<asp:TextBox ID="txtDbAdminPassword" runat="server" TextMode="Password" />&nbsp;<asp:CustomValidator ID="cvDbAdminSqlPassword"
											runat="server" CssClass="msgwarning" Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Installer_DbAdmin_Sql_Auth_Pwd_Val_Err %>"
											OnServerValidate="cvDbAdminSqlPassword_ServerValidate" />
									</td>
								</tr>
							</table>
						</div>
					</div>
				</div>
			</asp:Panel>
			<asp:Panel ID="ChooseDb" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l21" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_ChooseDb_Hdr %>" />
				</h2>
				<p>
					<asp:Literal ID="l22" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_ChooseDb_Dtl %>" />
				</p>
				<div style="padding-left: 20px; padding-top: 20px">
					<div style="padding-left: 20px; padding-top: 10px">
						<asp:Literal ID="l23" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_ChooseDb_Avail_Db_Label %>" />
						<asp:DropDownList ID="ddlDbList" runat="server" />
					</div>
					<p id="lblErrMsgChooseDb" runat="server" class="invisible" enableviewstate="false" />
				</div>
			</asp:Panel>
			<asp:Panel ID="UpgradeFromVersion1" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l24" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_UpgradeFromVer1_Hdr %>" />
				</h2>
				<p>
					<asp:Literal ID="l25" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_UpgradeFromVer1_Dtl1 %>" />
				</p>
				<p class="addtopmargin5">
					<asp:CheckBox ID="chkUpgradeFromVersion1" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_UpgradeFromVer1_Label %>" /></p>
				<p class="msgfriendly">
					<asp:Literal ID="l26" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_UpgradeFromVer1_Dtl2 %>" />
				</p>
			</asp:Panel>
			<asp:Panel ID="DbRuntime" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l27" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Hdr %>" />
				</h2>
				<p id="lblErrMsgDbRuntime" runat="server" class="invisible" enableviewstate="false" />
				<p class="bold">
					<asp:Literal ID="l28" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Dtl1 %>" />
				</p>
				<p>
					<asp:Literal ID="l29" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Dtl2 %>" />
				</p>
				<p class="bold addtopmargin5">
					<asp:Literal ID="l30" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Sql_Auth_Hdr %>" />
				</p>
				<div class="addleftpadding5">
					<asp:RadioButtonList ID="rblDbRuntimeConnectType" runat="server">
					</asp:RadioButtonList>
					<div class="addtopmargin2">
						<table style="padding-left: 50px;">
							<tr>
								<td>
									<asp:Literal ID="l31" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Sql_Auth_UserName_Label %>" />
								</td>
								<td>
									<asp:TextBox ID="txtDbRuntimeUserName" runat="server" />&nbsp;<asp:CustomValidator ID="cvDbRuntimeSqlLogOn" runat="server"
										CssClass="msgwarning" Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Sql_Auth_UserName_Val_Err %>"
										OnServerValidate="cvDbRuntimeSqlLogOn_ServerValidate" />
								</td>
							</tr>
							<tr>
								<td>
									<asp:Literal ID="l32" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Sql_Auth_Pwd_Label %>" />
								</td>
								<td>
									<asp:TextBox ID="txtDbRuntimePassword" runat="server" TextMode="Password" />&nbsp;<asp:CustomValidator ID="cvDbRuntimeSqlPassword"
										runat="server" CssClass="msgwarning" Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Sql_Auth_Pwd_Val_Err %>"
										OnServerValidate="cvDbRuntimeSqlPassword_ServerValidate" />
								</td>
							</tr>
						</table>
					</div>
				</div>
				<p class="msgfriendly">
					<asp:Literal ID="l33" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_DbRuntime_Dtl3 %>" />
				</p>
			</asp:Panel>
			<asp:Panel ID="SetupOptions" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l34" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_SetupOptions_Hdr %>" />
				</h2>
				<p>
					<asp:Literal ID="l35" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_SetupOptions_Dtl1 %>" />
				</p>
				<div class="addleftpadding5 addtoppadding5">
					<asp:CheckBox ID="chkScriptMembership" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_SetupOptions_Script_Label %>"
						Checked="True" CssClass="bold"></asp:CheckBox>
					<p class="addleftpadding6">
						<asp:Literal ID="l36" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_SetupOptions_Dtl2 %>" />
					</p>
					<p class="addleftpadding6">
						<asp:Literal ID="l37" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_SetupOptions_Dtl3 %>" />
					</p>
					<p class="msgfriendly addleftpadding6">
						<asp:Literal ID="l38" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_SetupOptions_Dtl4 %>" />
					</p>
				</div>
			</asp:Panel>
			<asp:Panel ID="GsAdmin" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l39" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Hdr %>" />
				</h2>
				<p>
					<asp:Literal ID="l40" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Dtl1 %>" />
				</p>
				<p>
					<asp:Literal ID="l41" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Dtl2 %>" />
				</p>
				<asp:Panel ID="pnlMembershipWarning" runat="server"></asp:Panel>
				<p class="msgfriendly addleftpadding6" style="display: <%= (this.ProviderDb == GalleryServerPro.Web.installer.ProviderDataStore.SqlServer ? "block;" : "none;") %>">
					<asp:Literal ID="l42" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Dtl3 %>" />
				</p>
				<div class="addtopmargin5">
					<table style="padding-left: 50px;" class="standardTable">
						<tr>
							<td class="nowrap">
								<asp:Literal ID="l43" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_UserName_Label %>" />
							</td>
							<td>
								<asp:TextBox CssClass="dataentry" ID="txtGsAdminUserName" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_UserName_Default %>"></asp:TextBox>
							</td>
							<td class="fs">
								<asp:RequiredFieldValidator ID="rfvGsAdminUserName" runat="server" ControlToValidate="txtGsAdminUsername" Enabled="true"
									Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" />
							</td>
						</tr>
						<tr>
							<td class="nowrap">
								<asp:Literal ID="l44" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Pwd_Label %>" />
							</td>
							<td>
								<asp:TextBox CssClass="dataentry" ID="txtGsAdminPassword" runat="server" TextMode="Password"></asp:TextBox>
							</td>
							<td class="fs">
								<asp:RequiredFieldValidator ID="rfvGsAdminPassword" runat="server" ControlToValidate="txtGsAdminPassword" Enabled="true"
									Display="Dynamic" />
								<asp:RegularExpressionValidator ID="regGsAdminPassword" runat="server" ControlToValidate="txtGsAdminPassword" Display="dynamic"></asp:RegularExpressionValidator>
							</td>
						</tr>
						<tr>
							<td class="nowrap">
								<asp:Literal ID="l45" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Pwd_Confirm_Label %>" />
							</td>
							<td>
								<asp:TextBox ID="txtGsAdminPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox>
							</td>
							<td class="fs">
								<asp:RequiredFieldValidator ID="rfvGsAdminPasswordConfirm" runat="server" ControlToValidate="txtGsAdminPasswordConfirm" Enabled="true"
									Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Site_Field_Required_Text %>" />
								<asp:CompareValidator EnableClientScript="True" Enabled="True" ID="ComparePassword" runat="server" ControlToValidate="txtGsAdminPasswordConfirm"
									ControlToCompare="txtGsAdminPassword" CssClass="msgwarning" Display="Dynamic" ErrorMessage="<%$ Resources:GalleryServerPro, Admin_Manage_Users_Passwords_Not_Matching_Error %>" />
							</td>
						</tr>
						<tr>
							<td class="nowrap">
								<asp:Literal ID="l46" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_GsAdmin_Email_Label %>" />
							</td>
							<td>
								<asp:TextBox ID="txtGsAdminEmail" runat="server" />
							</td>
							<td>
							</td>
						</tr>
					</table>
				</div>
			</asp:Panel>
			<asp:Panel ID="ReadyToInstall" runat="server" Visible="false">
				<h2 id="lblReadyToInstallHeaderMsg" runat="server" enableviewstate="false">
					<asp:Literal ID="l47" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_ReadyToInstall_Hdr %>" />
				</h2>
				<p id="lblErrMsgReadyToInstall" runat="server" class="invisible" enableviewstate="false" />
				<p id="lblErrMsgReadyToInstallSql" runat="server" class="invisible" enableviewstate="false" />
				<p id="lblErrMsgReadyToInstallCallStack" runat="server" class="invisible" enableviewstate="false" />
				<p id="lblReadyToInstallDetailMsg" runat="server" enableviewstate="false">
					<asp:Literal ID="l48" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_ReadyToInstall_Dtl1 %>" />
				</p>
			</asp:Panel>
			<asp:Panel ID="Finished" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l49" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Finished_Hdr %>" />
				</h2>
				<div>
					<asp:Image ID="imgFinishedIcon" runat="server" AlternateText="" Style="float: left; padding-top: 0.6em;" />
					<p style="margin-left: 50px;">
						<asp:Literal ID="l61" runat="server" />
					</p>
					<asp:Panel ID="pnlWebConfigNeedUpdating" runat="server" Visible="False" CssClass="addtopmargin5">
						<img src="<%= GalleryServerPro.Web.WebsiteController.GetThemePathUrl(this.Theme) %>/images/go_14x14.png" alt="" style="width: 14px;
							height: 14px; float: left; padding-top: 0.6em;" />
						<div style="margin-left: 25px;">
							<p>
								<span class="bold">Web.config: </span>
								<asp:Literal ID="l62" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Finished_WebCfg_Need_Updating_Dtl %>" />
							</p>
							<pre class="msgfriendly">&lt;add name="<%= CN_STRING_NAME %>" connectionString="<%= GetDbRuntimeConnectionString(true)%>" /&gt;</pre>
						</div>
					</asp:Panel>
					<asp:Panel ID="pnlGalleryServerProConfigNeedUpdating" runat="server" Visible="False" CssClass="addtopmargin5">
						<img src="<%= GalleryServerPro.Web.WebsiteController.GetThemePathUrl(this.Theme) %>/images/go_14x14.png" alt="" style="width: 14px;
							height: 14px; float: left; padding-top: 0.6em;" />
						<div style="margin-left: 25px;">
							<p>
								<span class="bold">Galleryserverpro.config: </span>
								<asp:Literal ID="l63" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Finished_GspCfg_Need_Updating_Dtl %>" />&nbsp;<span
									class="bold"><%= EncryptionKey%></span>
							</p>
						</div>
					</asp:Panel>
					<p class="msgfriendly" style="clear: left;">
						<span class="bold">
							<asp:Literal ID="l54" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Finished_Reminder_Hdr %>" /></span>
						<asp:Literal ID="l55" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Finished_Reminder_Dtl %>" /></p>
				</div>
			</asp:Panel>
		</div>
		<div id="wizFtr">
			<a href="http://www.techinfosystems.com" title='<asp:Literal ID="l56" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Footer_TIS_Link_Tooltip %>" />'
				tabindex="-1" style="display: block; float: left;">
				<img src="../images/tis_logo.gif" alt="" style="width: 132px; height: 76px;" /></a>
			<p class="fss" style="margin: 50px auto auto 175px;">
				<asp:Literal ID="litVersion" runat="server" /></p>
			<div class="rightBottomAbsolute">
				<p class="minimargin">
					<asp:Button ID="btnPrevious" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Previous_Button_Text %>" CausesValidation="false"
						OnClick="btnPrevious_Click" TabIndex="0" />&nbsp;
					<asp:Button ID="btnNext" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Next_Button_Text %>" OnClick="btnNext_Click"
						TabIndex="0" /></p>
			</div>
		</div>
	</div>
</asp:Content>

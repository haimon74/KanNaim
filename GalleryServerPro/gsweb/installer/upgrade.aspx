<%@ Page Language="C#" MasterPageFile="~/master/global.Master" AutoEventWireup="true"
	CodeBehind="upgrade.aspx.cs" Inherits="GalleryServerPro.Web.installer.upgrade"
	Title="Gallery Server Pro Upgrade Wizard" %>

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
					<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Welcome_Hdr %>" />
				</h2>
				<p>
					<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Welcome_Dtl %>" />
				</p>
				<p class="bold" style="margin-top: 50px;">
					<asp:Literal ID="l3" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Detected_Sql_Version_Hdr %>" />
					<asp:Label ID="lblDetectedSqlDbVersion" runat="server" CssClass="msgfriendly" />
				</p>
				<p class="bold addtopmargin5">
					<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Detected_Web_Version_Hdr %>" />
					<asp:Label ID="lblDetectedWebVersion" runat="server" CssClass="msgfriendly" />
				</p>
				<p class="addtopmargin5">
					<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Avail_Scripts_Hdr %>" />
				</p>
				<div class="addleftpadding5">
					<asp:DropDownList ID="ddlSqlUpgradeScripts" runat="server">
						<asp:ListItem Text="GS_2.0.2886_to_2.1.3162_upgrade.sql" Value="0" />
					</asp:DropDownList>
				</div>
			</asp:Panel>
			<asp:Panel ID="ReadyToUpgrade" runat="server" Visible="false">
				<h2 id="lblReadyToUpgradeHeaderMsg" runat="server" enableviewstate="false">
					<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_ReadyToUpgrade_Hdr %>" />
				</h2>
				<p id="lblReadyToUpgradeDetailMsg" runat="server" enableviewstate="false">
					<asp:Literal ID="l48" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_ReadyToUpgrade_Dtl %>" />
				</p>
				<p class="bold addtopmargin5 addleftpadding5">
					<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_ReadyToUpgrade_SelectedSqlScript_Hdr %>" />
					<asp:Label ID="lblSelectedSqlScript" runat="server" CssClass="msgfriendly" />
				</p>
				<p id="lblReadyToUpgradePermWarningMsg" runat="server" style="margin-top:50px;" enableviewstate="false" class="msgfriendly">
					<asp:Literal ID="l7b" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_ReadyToUpgrade_Permission_Warning %>" />
				</p>
				<p id="lblErrMsgReadyToUpgrade" runat="server" class="invisible" enableviewstate="false" />
				<p id="lblErrMsgReadyToUpgradeSql" runat="server" class="invisible" enableviewstate="false" />
				<p id="lblErrMsgReadyToUpgradeCallStack" runat="server" class="invisible" enableviewstate="false" />
			</asp:Panel>
			<asp:Panel ID="Finished" runat="server" Visible="false">
				<h2>
					<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Hdr %>" />
				</h2>
				<div>
					<p>
						<asp:Literal ID="l9" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Dtl1 %>" />
					</p>
					<p>
						<asp:Literal ID="l10" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Dtl2 %>" />
					</p>
					<p class="msgfriendly">
						<span class="bold">
							<asp:Literal ID="l11" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Reminder_Hdr %>" /></span>
						<asp:Literal ID="l12" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Reminder_Dtl %>" /></p>
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
					<asp:Button ID="btnPrevious" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Previous_Button_Text %>"
						CausesValidation="false" OnClick="btnPrevious_Click" TabIndex="0" />&nbsp;
					<asp:Button ID="btnNext" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Next_Button_Text %>"
						OnClick="btnNext_Click" TabIndex="0" /></p>
			</div>
		</div>
	</div>
</asp:Content>

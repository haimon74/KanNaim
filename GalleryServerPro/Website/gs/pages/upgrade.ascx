<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="upgrade.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.upgrade" %>
<%@ Import Namespace="GalleryServerPro.Web" %>
<link href="<%= Util.GalleryRoot %>/styles/gallery.css" rel="stylesheet" type="text/css" />
<link href="<%= Util.GalleryRoot %>/styles/ca_styles.css" rel="stylesheet" type="text/css" />
<div id="css" runat="server" visible="false">
	<link href="../styles/gallery.css" rel="stylesheet" type="text/css" />
</div>
<!-- Enable or disable this installer by changing the value of the ENABLE_SETUP hidden field to true or false. -->
<asp:HiddenField ID="ENABLE_SETUP" runat="server" Value="true" />
<div class="gsp_ns">
	<div id="wizInstall" class="gsp_rounded10">
		<div id="wizHdr" class="gsp_rounded10">
			<img src="<%= Util.GalleryRoot %>/images/gsp_logo_313x75.png" style="width: 313px;
				height: 75px;" alt="Gallery Server Pro logo" />
		</div>
		<div id="wizCtnt" class="wizCtnt" runat="server">
			<asp:Panel ID="Welcome" runat="server" Visible="false">
				<p class="gsp_h2">
					<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Welcome_Hdr %>" />
				</p>
				<p class="gsp_bold">
					<asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Welcome_Dtl1 %>" />
				</p>
				<p>
					<asp:Literal ID="l2b" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Welcome_Dtl2 %>" />
				</p>
				<p class="gsp_bold gsp_addleftmargin5 gsp_addtopmargin5">
					<img src="<%= Util.GalleryRoot %>/images/go_14x14.png" alt="" style="width: 14px;
						height: 14px; padding-right: .5em;" />web.config
				</p>
				<ul style="list-style-type: disc; margin-left: 3em;">
					<li>
						<asp:Literal ID="l2c" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_WebConfig_Imported_Items1 %>" />
					</li>
				</ul>
				<p class="gsp_bold gsp_addleftmargin5 gsp_addtopmargin5">
					<img src="<%= Util.GalleryRoot %>/images/go_14x14.png" alt="" style="width: 14px;
						height: 14px; padding-right: .5em;" />galleryserverpro.config
				</p>
				<ul style="list-style-type: disc; margin-left: 3em;">
					<li>
						<asp:Literal ID="l4" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_GspConfig_Imported_Items1 %>" /></li>
					<li>
						<asp:Literal ID="l5" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_GspConfig_Imported_Items2 %>" /></li>
					<li>
						<asp:Literal ID="l6" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_GspConfig_Imported_Items3 %>" /></li>
				</ul>
				<p class="gsp_msgattention gsp_addtopmargin5">
					<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Some_Items_May_Not_Be_Imported_Warning %>" /></p>
			</asp:Panel>
			<asp:Panel ID="PrepareConfigFiles" runat="server" Visible="false">
				<p class="gsp_h2">
					<asp:Literal ID="l7" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_PrepareConfigFiles_Hdr %>" />
				</p>
				<p class="gsp_addtopmargin5">
					<asp:Literal ID="l8" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_PrepareConfigFiles_Dtl %>" />
				</p>
				<p class="gsp_msgattention">
					<%= WebConfigSourcePath%></p>
				<p class="gsp_msgattention">
					<%= GspConfigSourcePath%></p>
				<p id="lblErrMsgPrepareConfigFiles" runat="server" class="gsp_invisible" enableviewstate="false" />
			</asp:Panel>
			<asp:Panel ID="ReadyToUpgrade" runat="server" Visible="false">
				<p id="lblReadyToUpgradeHeaderMsg" runat="server" class="gsp_h2" enableviewstate="false">
					<asp:Label ID="lblReadyToUpgradeHdrMsg" runat="server" />
				</p>
				<p id="pReadyToUpgradeDetail1Msg" runat="server" class="gsp_addtopmargin5" enableviewstate="false">
					<asp:Image ID="imgReadyToUpgradeStatus" runat="server" Style="float: left; padding-right: 0.5em;"
						AlternateText="" />
					<asp:Label ID="lblReadyToUpgradeDetail1Msg" runat="server" />
					<asp:LinkButton ID="lbTryAgain" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_ReadyToUpgrade_Try_Again_Text %>"
						OnClick="lbTryAgain_Click" Visible="false" EnableViewState="false" />
				</p>
				<p id="pReadyToUpgradeDetail2Msg" runat="server" enableviewstate="false">
					<asp:Label ID="lblReadyToUpgradeDetail2Msg" runat="server" />
				</p>
				<asp:Panel ID="pnlReadyToUpgradeConfigInfo" runat="server">
					<p class="gsp_bold gsp_addtopmargin5">
						<asp:Image ID="imgReadyToUpgradeWebConfigStatus" runat="server" Width="16" Height="16"
							Style="padding-right: 0.5em;" AlternateText="" />web.config</p>
					<p class="gsp_addleftmargin5">
						<asp:Label ID="lblReadyToUpgradeWebConfigStatus" runat="server" />
					</p>
					<p class="gsp_bold gsp_addtopmargin5">
						<asp:Image ID="imgReadyToUpgradeGspConfigStatus" runat="server" Width="16" Height="16"
							Style="padding-right: 0.5em;" AlternateText="" />galleryserverpro.config</p>
					<p class="gsp_addleftmargin5">
						<asp:Label ID="lblReadyToUpgradeGspConfigStatus" runat="server" />
					</p>
				</asp:Panel>
			</asp:Panel>
			<asp:Panel ID="Finished" runat="server" Visible="false">
				<p class="gsp_h2">
					<asp:Literal ID="l49" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Hdr %>" />
				</p>
				<div>
					<asp:Image ID="imgFinishedIcon" runat="server" AlternateText="" Style="float: left;
						padding-top: 0.6em;" />
					<p style="margin-left: 50px; min-height: 50px;">
						<asp:Literal ID="l61" runat="server" />
					</p>
					<p class="gsp_bold">
						<asp:Image ID="imgFinishedWebConfigStatus" runat="server" Style="padding-right: .5em;"
							AlternateText="" />web.config</p>
					<p style="margin-left: 25px;">
						<asp:Label ID="lblFinishedWebConfigStatus" runat="server" />
					</p>
					<p style="margin-left: 25px;">
						<asp:Label ID="lblFinishedWebConfigCallStack" runat="server" />
					</p>
					<p class="gsp_bold gsp_addtopmargin5">
						<asp:Image ID="imgFinishedGspConfigStatus" runat="server" Style="padding-right: .5em;"
							AlternateText="" />galleryserverpro.config</p>
					<p style="margin-left: 25px;">
						<asp:Label ID="lblFinishedGspConfigStatus" runat="server" />
					</p>
					<p style="margin-left: 25px;">
						<asp:Label ID="lblFinishedGspConfigCallStack" runat="server" />
					</p>
					<p class="gsp_msgattention gsp_addtopmargin5">
						<span class="gsp_bold">
							<asp:Literal ID="l54" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Reminder_Hdr %>" /></span>
						<asp:Literal ID="l55" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Upgrade_Finished_Reminder_Dtl %>" /></p>
				</div>
			</asp:Panel>
		</div>
		<div id="wizFtr">
			<a href="http://www.techinfosystems.com" title='<asp:Literal ID="l56" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Footer_TIS_Link_Tooltip %>" />'
				tabindex="-1" style="display: block; float: left;">
				<img src="<%= Util.GalleryRoot %>/images/tis_logo.gif" alt="" style="width: 132px;
					height: 76px;" /></a>
			<p class="gsp_fss" style="margin: 50px auto auto 175px;">
				<asp:Literal ID="litVersion" runat="server" /></p>
			<div class="gsp_rightBottomAbsolute">
				<p class="gsp_minimargin">
					<asp:Button ID="btnPrevious" runat="server" Text="<%$ Resources:GalleryServerPro, Installer_Previous_Button_Text %>"
						CausesValidation="false" OnClick="btnPrevious_Click" TabIndex="0" />&nbsp;
					<asp:Button ID="btnNext" runat="server" EnableViewState="false" Text="<%$ Resources:GalleryServerPro, Installer_Next_Button_Text %>"
						OnClick="btnNext_Click" TabIndex="0" /></p>
			</div>
		</div>
	</div>
</div>

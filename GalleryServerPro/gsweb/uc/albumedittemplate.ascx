<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="albumedittemplate.ascx.cs"
	Inherits="GalleryServerPro.Web.uc.albumedittemplate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../uc/popupinfo.ascx" TagName="popup" TagPrefix="uc1" %>
<%@ Register Assembly="GalleryServerPro.WebControls" Namespace="GalleryServerPro.WebControls"
	TagPrefix="tis" %>

<script type="text/javascript">

Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(editAlbumTemplatePageLoad);

var _cdrBeginDateId = '<% = cdrBeginDate.ClientObjectId %>';
var _cdrEndDateId = '<% = cdrEndDate.ClientObjectId %>';
var _txtTitleId = '<% = txtTitle.ClientID %>';

function editAlbumTemplatePageLoad(sender, args)
{
	Sys.Application.addComponent(cdrBeginDate);
	Sys.Application.addComponent(cdrEndDate);
	RenderExampleDate();
}

function RenderExampleDate()
{
	var exDate = new Date();
	//var exDateMsg = exDate.localeFormat(_dateFormatString);
	var exDateMsg = "(e.g. " + exDate.localeFormat(_dateFormatString) + ")";
	$get('bdEx').appendChild(document.createTextNode(exDateMsg));
	$get('edEx').appendChild(document.createTextNode(exDateMsg));
}

function beginDate_OnClick(alignElement, calendar)
{
  if (calendar.get_popUpShowing())
  {
    calendar.hide();
  }
  else
  {
    var beginDate = Date.parseLocale($get('beginDate').value), _dateFormatString;
    if (beginDate == null)
      beginDate = new Date();
      
    calendar.setSelectedDate(beginDate);
    calendar.show(alignElement);
  }
}

function cdrBeginDate_onSelectionChanged(sender, eventArgs)
{
  _beginDate = new Date(sender.getSelectedDate());
	$get('beginDate').value = _beginDate.localeFormat(_dateFormatString);
}

function endDate_OnClick(alignElement, calendar)
{
  if (calendar.get_popUpShowing())
  {
    calendar.hide();
  }
  else
  {
    var endDate = Date.parseLocale($get('endDate').value), _dateFormatString;
    if (endDate == null)
      endDate = new Date();
      
    calendar.setSelectedDate(endDate);
    calendar.show(alignElement);
  }
}

function cdrEndDate_onSelectionChanged(sender, eventArgs)
{
  _endDate = new Date(sender.getSelectedDate());
	$get('endDate').value = _endDate.localeFormat(_dateFormatString);
}

function closeEditDialog()
{
	if (!Page_ClientValidate())
	{
		// Force the error popup to close by putting a dummy value in the album title textbox and revalidating
		$get(_txtTitleId).value = '1';
		Page_ClientValidate();
	}
	dgEditAlbum.close();
}
</script>

<table class='standardTable addpadding1' onkeypress="javascript:return WebForm_FireDefaultButton(event, 'btnSave')">
	<tr>
		<td class='bold nowrap' style="vertical-align: middle;">
			<asp:Label ID="lblTitle" runat="server" AssociatedControlID="txtTitle" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Title_Header_Text %>" />
		</td>
		<td>
			<asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" />
			<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtTitle"
				Display="None" ErrorMessage="<%$ Resources:GalleryServerPro, Task_Create_Album_Missing_Title_Text %>">
			</asp:RequiredFieldValidator>
			<cc1:ValidatorCalloutExtender runat="server" ID="rfv1E" TargetControlID="rfv1" HighlightCssClass="validatorCalloutHighlight" />
		</td>
		<td class='fs' style="vertical-align: middle;">
			<asp:Label ID="lblMaxTitleLengthInfo" runat="server" AssociatedControlID="txtTitle"
				CssClass="fs" />
			<asp:PlaceHolder ID="phAlbumTitleMaxLengthInfo" runat="server" />
		</td>
	</tr>
	<tr>
		<td class='bold'>
			<label for="albumSummary">
				<asp:Literal ID="litSummary" runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Summary_Header_Text %>" /></label></td>
		<td>
			<textarea id='albumSummary' rows='12' cols='70' class='textarea2'>&nbsp;</textarea>
		</td>
		<td class='fs'>
			<asp:PlaceHolder ID="phAlbumSummaryMaxLengthInfo" runat="server" />
		</td>
	</tr>
	<tr>
		<td class='bold nowrap' style="vertical-align: middle;">
			<label for="beginDate">
				<asp:Literal ID="litBeginDate" runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Begin_Date_Header_Text %>" /></label></td>
		<td colspan="2">
			<input id='beginDate' type='text' />&nbsp;<img alt="" style="vertical-align: bottom;"
				onclick="beginDate_OnClick(this, <%= cdrBeginDate.ClientObjectId %>)" class="calendar_button"
				src="<%= CalendarIconUrl %>" width="25" height="22" />
			<CA:Calendar runat="server" ID="cdrBeginDate" AllowMultipleSelection="false" AllowWeekSelection="false"
				AllowMonthSelection="false" ControlType="Calendar" PopUp="Custom" CalendarTitleCssClass="title"
				DayHeaderCssClass="dayheader" DayCssClass="day" DayHoverCssClass="dayhover" OtherMonthDayCssClass="othermonthday"
				SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
				NextPrevHoverCssClass="nextprevhover" MonthCssClass="month" SwapSlide="Linear"
				SwapDuration="300" DayNameFormat="FirstTwoLetters" ImagesBaseUrl="~/images/componentart/calendar/"
				PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
				<ClientEvents>
					<SelectionChanged EventHandler="cdrBeginDate_onSelectionChanged" />
				</ClientEvents>
			</CA:Calendar>
			<span id="bdEx" class="fs em" />
		</td>
	</tr>
	<tr>
		<td class='bold nowrap' style="vertical-align: middle;">
			<label for="endDate">
				<asp:Literal ID="litEndDate" runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_End_Date_Header_Text %>" /></label></td>
		<td colspan="2">
			<input id='endDate' type='text' />&nbsp;<img alt="" style="vertical-align: bottom;"
				onclick="endDate_OnClick(this, <%= cdrEndDate.ClientObjectId %>)" class="calendar_button"
				src="<%= CalendarIconUrl %>" width="25" height="22" />
			<CA:Calendar runat="server" ID="cdrEndDate" AllowMultipleSelection="false" AllowWeekSelection="false"
				AllowMonthSelection="false" ControlType="Calendar" PopUp="Custom" CalendarTitleCssClass="title"
				DayHeaderCssClass="dayheader" DayCssClass="day" DayHoverCssClass="dayhover" OtherMonthDayCssClass="othermonthday"
				SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
				NextPrevHoverCssClass="nextprevhover" MonthCssClass="month" SwapSlide="Linear"
				SwapDuration="300" DayNameFormat="FirstTwoLetters" ImagesBaseUrl="~/images/componentart/calendar/"
				PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
				<ClientEvents>
					<SelectionChanged EventHandler="cdrEndDate_onSelectionChanged" />
				</ClientEvents>
			</CA:Calendar>
			<span id="edEx" class="fs em"></span>
		</td>
	</tr>
	<tr>
		<td>
		</td>
		<td>
			<img src="<%= this.PageBase.ThemePath %>/images/lock_24x24.png" style="width:24px;height:24px;vertical-align:bottom;" alt="" /><input id="private" type="checkbox" /> <asp:Label ID="lblPrivateAlbum" runat="server" /></td>
	</tr>
</table>
<div class='okCancelContainer'>
	<input id="btnSave" type='button' value='<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Ok_Button_Text%>" />' onclick="if(Page_ClientValidate()) saveAlbumInfo()" />&nbsp;<input
		onclick="closeEditDialog()" type='button' value='<asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Cancel_Button_Text%>" />' />
</div>
<tis:PopupInfo ID="PopupInfo" runat="server" DefaultDialogControlId="dgPopup" DefaultDialogTitleCss="dg5ContentTitleCss"
	DefaultDialogBodyCss="dg5ContentBodyCss">
	<PopupInfoItems>
		<tis:PopupInfoItem ID="PopupInfoItem1" runat="server" ControlId="lblPrivateAlbum"
			DialogTitle="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Is_Private_Info_Hdr %>"
			DialogBody="<%$ Resources:GalleryServerPro, UC_Album_Header_Edit_Album_Is_Private_Info_Bdy %>" />
	</PopupInfoItems>
</tis:PopupInfo>
<uc1:popup ID="ucPopupContainer" runat="server" />

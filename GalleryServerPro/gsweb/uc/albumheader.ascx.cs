using System;
using System.Globalization;
using System.Web.UI;
using ComponentArt.Web.UI;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.uc
{
	public partial class albumheader : System.Web.UI.UserControl
	{
		#region Private Fields

		private bool _enableInlineEditing;

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!cbDummy.IsCallback)
			{
				ConfigureControls();

				ConfigureSecurityRelatedControls();
			}

			if (!IsPostBack)
			{
				RegisterJavascript();
			}
		}

		protected string GetAlbumPrivateCssClass()
		{
			if (this.PageBase.GetAlbum().IsPrivate)
				return String.Empty;
			else
				return "invisible";
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		public GspPage PageBase
		{
			get { return (GspPage)this.Page; }
		}

		public bool EnableInlineEditing
		{
			get
			{
				return this._enableInlineEditing;
			}
			set
			{
				this._enableInlineEditing = value;
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			IAlbum album = this.PageBase.GetAlbum();
			lblTitle.Text = album.Title;
			lblSummary.Text = album.Summary;
			lblStats.Text = getAlbumStats(album);
			lblDate.Text = getAlbumDates(album);

			if (String.IsNullOrEmpty(lblDate.Text))
			{
				dateContainer.Attributes["class"] = "invisible minimargin";
			}
			else
			{
				dateContainer.Attributes["class"] = "minimargin";
			}
		}

		private void ConfigureSecurityRelatedControls()
		{
			// If the user has permission to edit the media object, configure the caption so that when it is double-clicked,
			// a dialog window appears that lets the user edit and save the caption. Note that this code is dependent on the
			// saveCaption javascript function, which is added in the RegisterJavascript method.
			if (this.EnableInlineEditing && this.PageBase.UserCanEditAlbum)
			{
				pnlAlbumHeader.ToolTip = Resources.GalleryServerPro.Site_Editable_Content_Tooltip;
				pnlAlbumHeader.CssClass = "albumHeader albumHeaderEditableContentOff";
				pnlAlbumHeader.Attributes.Add("onmouseover", "this.className='albumHeader albumHeaderEditableContentOn';");
				pnlAlbumHeader.Attributes.Add("onmouseout", "this.className='albumHeader albumHeaderEditableContentOff';");
				pnlAlbumHeader.Attributes.Add("ondblclick", "editAlbumInfo();");

				AddEditAlbumInfoDialog();
			}
		}

		private void AddEditAlbumInfoDialog()
		{
			Dialog dgEditAlbum = new Dialog();

			dgEditAlbum.ContentTemplate = Page.LoadTemplate("~/uc/albumedittemplate.ascx");

			#region Set Dialog Properties

			dgEditAlbum.ID = "dgEditAlbum";
			dgEditAlbum.AnimationDirectionElement = pnlAlbumHeader.ClientID;
			dgEditAlbum.CloseTransition = TransitionType.Fade;
			dgEditAlbum.ShowTransition = TransitionType.Fade;
			dgEditAlbum.AnimationSlide = SlideType.Linear;
			dgEditAlbum.AnimationType = DialogAnimationType.Live;
			dgEditAlbum.AnimationPath = SlidePath.Direct;
			dgEditAlbum.AnimationDuration = 400;
			dgEditAlbum.TransitionDuration = 400;
			dgEditAlbum.Icon = "pencil.gif";
			dgEditAlbum.Alignment = DialogAlignType.MiddleCentre;
			dgEditAlbum.AllowResize = true;
			dgEditAlbum.ContentCssClass = "dg0ContentCss";
			dgEditAlbum.HeaderCssClass = "dg0HeaderCss";
			dgEditAlbum.CssClass = "dg0DialogCss";
			dgEditAlbum.FooterCssClass = "dg0FooterCss";
			dgEditAlbum.ZIndex = 900;

			dgEditAlbum.HeaderClientTemplateId = "dgEditAlbumHeaderTemplate";

			#endregion

			#region Header Template

			ClientTemplate ctHeader = new ClientTemplate();
			ctHeader.ID = "dgEditAlbumHeaderTemplate";

			ctHeader.Text = @"
		<div onmousedown='dgEditAlbum.StartDrag(event);'>
			<img id='dg0DialogCloseImage' onclick=""dgEditAlbum.Close('cancelled');"" src='images/componentart/dialog/close.gif' /><img
				id='dg0DialogIconImage' src='images/componentart/dialog/pencil.gif' style='width:27px;height:30px;' />
				## Parent.Title ##
		</div>";

			dgEditAlbum.ClientTemplates.Add(ctHeader);

			#endregion

			phEditAlbumDialog.Controls.Add(dgEditAlbum);
		}

		private static string getAlbumDates(IAlbum album)
		{
			//If there are two different valid dates, then display both.  Otherwise, display
			//the one that is valid, or return blank if neither are valid.
			string dateText = string.Empty;
			if ((album.DateStart > DateTime.MinValue) && (album.DateEnd > DateTime.MinValue))
				//Both dates are valid.  Combine them.
				dateText = string.Format(CultureInfo.CurrentCulture, "{0:d} {1} {2:d}", album.DateStart, Resources.GalleryServerPro.UC_Album_Header_Album_Date_Range_Separator_Text, album.DateEnd);
			else if (album.DateStart > DateTime.MinValue)
				//The start date is valid.  Since the end date is invalid (we know this because if 
				//it was valid we would have gone through the previous branch rather than this one)
				//only display the start date.
				dateText = album.DateStart.ToShortDateString();
			else if (album.DateEnd > DateTime.MinValue)
				//The end date is valid.  Since the start date is invalid (we know this because if 
				//it was valid we would have gone through the first branch rather than this one)
				//only display the end date.
				dateText = album.DateEnd.ToShortDateString();

			return dateText;
		}

		private string getAlbumStats(IAlbum album)
		{
			//Create a string like: (12 objects, created 3/24/04)
			int numObjects = album.GetChildGalleryObjects(false, this.PageBase.IsAnonymousUser).Count;

			if (album.IsVirtualAlbum)
				return string.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_Album_Header_Stats_Without_Date_Text, numObjects);
			else
				return string.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_Album_Header_Stats_Text, numObjects, album.DateAdded);
		}

		private void RegisterJavascript()
		{
			IAlbum album = this.PageBase.GetAlbum();

			string beginDateVarDeclaration = (album.DateStart == DateTime.MinValue ? "null" : String.Format(CultureInfo.CurrentCulture, "new Date({0}, {1}, {2})", album.DateStart.Year, album.DateStart.Month - 1, album.DateStart.Day));
			string endDateVarDeclaration = (album.DateEnd == DateTime.MinValue ? "null" : String.Format(CultureInfo.CurrentCulture, "new Date({0}, {1}, {2})", album.DateEnd.Year, album.DateEnd.Month - 1, album.DateEnd.Day));

			string script = String.Format(CultureInfo.InvariantCulture, @"

	var _albumId = {0};
	var _beginDate = {1};
	var _endDate = {2};
  var _dateFormatString = 'd'; // Short date format
			",
			album.Id, // 0
			beginDateVarDeclaration, // 1
			endDateVarDeclaration // 2
			);

			// Add a few more functions for users with edit album permission.
			if (this.EnableInlineEditing && this.PageBase.UserCanEditAlbum)
			{
				script += String.Format(CultureInfo.InvariantCulture, @"

	function editAlbumInfo()
	{{
		dgEditAlbum.show();
		$get(_txtTitleId).focus();
		var title = $get('{0}').innerHTML;
		dgEditAlbum.set_title('{1}: ' + title);
		$get(_txtTitleId).value = title;

		$get('albumSummary').value = $get('{2}').innerHTML;
		var isPrivate = !Sys.UI.DomElement.containsCssClass($get('albumPrivateIcon'), 'invisible');
		$get('private').checked = isPrivate;
		$get('private').disabled = {3};

    if (_beginDate != null)
    {{
		  $get('beginDate').value = _beginDate.localeFormat(_dateFormatString);
			var cdrDateStart = $find(_cdrBeginDateId);
		  cdrDateStart.setSelectedDate(_beginDate);		
    }}

    if (_endDate != null)
    {{
		  $get('endDate').value = _endDate.localeFormat(_dateFormatString);
		  var cdrDateEnd = $find(_cdrEndDateId);
		  cdrDateEnd.setSelectedDate(_endDate);		
    }}
	}}
	
	function saveAlbumInfo()
	{{
		document.body.style.cursor = 'wait';
		var albumEntity = new Gsp.AlbumWebEntity();
		albumEntity.Id = _albumId;
		albumEntity.Title = $get(_txtTitleId).value;
		albumEntity.Summary = $get('albumSummary').value;
		albumEntity.IsPrivate = $get('private').checked;
		
    var beginDate = Date.parseLocale($get('beginDate').value, _dateFormatString);
    if (beginDate != null)
    {{
      _beginDate = beginDate;
		  albumEntity.DateStart = beginDate;
		}}
    else
    {{
      _beginDate = null;
    }}

    var endDate = Date.parseLocale($get('endDate').value, _dateFormatString);
    if (endDate != null)
    {{
      _endDate = endDate;
		  albumEntity.DateEnd = endDate;
    }}
    else
    {{
      _endDate = null;
    }}

		PageMethods.UpdateAlbumInfo(albumEntity, updateAlbumInfoCompleted);
		dgEditAlbum.close();
	}}

	function updateAlbumInfoCompleted(results, context, methodName)
	{{
		$get('{4}').innerHTML = results.Title;
		$get('{5}').innerHTML = results.Summary;
    
    UpdateDateLabel(_beginDate, _endDate);

		setText($get('currentAlbumLink'), results.Title);

		if (results.IsPrivate)
			Sys.UI.DomElement.removeCssClass($get('albumPrivateIcon'), 'invisible');
		else
			Sys.UI.DomElement.addCssClass($get('albumPrivateIcon'), 'invisible');

		document.body.style.cursor = 'default';
	}}

	function UpdateDateLabel(beginDate, endDate)
	{{
	  var dateLabelText = '';
		var showDate = true;
		if ((_beginDate != null) && (_endDate != null))
			//Both dates are valid.  Combine them.
			dateLabelText = _beginDate.localeFormat(_dateFormatString) + ' {6} ' + _endDate.localeFormat(_dateFormatString);
		else if (_beginDate != null)
			dateLabelText = _beginDate.localeFormat(_dateFormatString);
		else if (_endDate != null)
			dateLabelText = _endDate.localeFormat(_dateFormatString);
		else
			showDate = false;

    $get('{7}').innerHTML = dateLabelText;

		var dateElement = $get('{8}');
		if (showDate)
		{{
			Sys.UI.DomElement.removeCssClass(dateElement, 'invisible');
			Sys.UI.DomElement.addCssClass(dateElement, 'visible');
		}}
		else
		{{
			Sys.UI.DomElement.removeCssClass(dateElement, 'visible');
			Sys.UI.DomElement.addCssClass(dateElement, 'invisible');
		}}
	}}

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

				",
				lblTitle.ClientID, // 0
				Resources.GalleryServerPro.UC_Album_Header_Dialog_Title_Edit_Album, // 1
				lblSummary.ClientID, // 2
				album.Parent.IsPrivate.ToString().ToLowerInvariant(), // 3
				lblTitle.ClientID, // 4
				lblSummary.ClientID, // 5
				Resources.GalleryServerPro.UC_Album_Header_Album_Date_Range_Separator_Text, // 6
				lblDate.ClientID, // 7
				dateContainer.ClientID // 8
				);
			}

			ScriptManager.RegisterClientScriptBlock(this, typeof(GspPage), "albumHeaderScript", script, true);
		}
		
		#endregion
	}
}
using System;
using System.Globalization;
using System.Web.UI;

namespace GalleryServerPro.Web.uc
{
	public partial class albumedittemplate : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack && (!this.Page.IsCallback))
			{
        //cdrBeginDate.Culture = System.Globalization.CultureInfo.CurrentCulture;
				ConfigureControls();
			}
		}

		#region Public Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		public GspPage PageBase
		{
			get { return (GspPage)this.Page; }
		}

		public string CalendarIconUrl
		{
			get
			{
				return ResolveUrl("~/images/componentart/calendar/btn_calendar.gif");
			}
		}

    public static int AlbumSummaryMaxLength
		{
			get
			{
				return GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumSummaryLength;
			}
		}

		#endregion

		private void ConfigureControls()
		{
			int albumTitleMaxLength = GalleryServerPro.Configuration.ConfigManager.GetGalleryServerProConfigSection().DataStore.AlbumTitleLength;

			txtTitle.MaxLength = albumTitleMaxLength;

			string albumTitleMaxLengthInfo = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_Album_Header_Album_Title_Max_Length_Text, albumTitleMaxLength);
			lblMaxTitleLengthInfo.Text = albumTitleMaxLengthInfo;
			//phAlbumTitleMaxLengthInfo.Controls.Add(new LiteralControl(albumTitleMaxLengthInfo));

			string albumSummaryMaxLengthInfo = String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.UC_Album_Header_Album_Summary_Max_Length_Text, AlbumSummaryMaxLength);
			phAlbumSummaryMaxLengthInfo.Controls.Add(new LiteralControl(albumSummaryMaxLengthInfo));

			if (this.PageBase.GetAlbum().Parent.IsPrivate)
			{
				lblPrivateAlbum.Text = Resources.GalleryServerPro.UC_Album_Header_Edit_Album_Is_Private_Disabled_Text;
			}
			else
			{
				lblPrivateAlbum.Text = Resources.GalleryServerPro.UC_Album_Header_Edit_Album_Is_Private_Text;
			}
		}
	}
}
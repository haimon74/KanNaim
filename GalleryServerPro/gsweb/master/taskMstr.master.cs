using System;
using System.Web.UI.WebControls;

namespace GalleryServerPro.Web.Master
{
	public partial class taskMstr : System.Web.UI.MasterPage
	{
		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
				ConfigureControls();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.PageBase.RedirectToReferringPage();
		}

		#endregion

		#region Protected Properties

		/// <summary>
		/// Get a reference to the base page.
		/// </summary>
		protected GspPage PageBase
		{
			get { return (GspPage)this.Page; }
		}
		
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets / sets the task header text (e.g. Task: Delete an album).
		/// </summary>
		public string TaskHeader
		{
			get
			{
				return lblTaskHeader.Text;
			}
			set
			{
				lblTaskHeader.Text = value;
			}
		}

		/// <summary>
		/// Gets / sets the task body text (e.g. Confirm the deletion of this album by clicking 'Delete album'.).
		/// </summary>
		public string TaskBody
		{
			get
			{
				return lblTaskBody.Text;
			}
			set
			{
				lblTaskBody.Text = value;
			}
		}

		/// <summary>
		/// Gets / sets the text that appears on the top and bottom Ok buttons on the page. This is rendered as the value
		/// attribute of the input HTML tag.
		/// </summary>
		public string OkButtonText
		{
			get
			{
				return btnOkTop.Text;
			}
			set
			{
				// Ensure value is less than 25 characters.
				string btnText = value.PadRight(25).Substring(0, 25).Trim();
				btnOkTop.Text = btnText;
				btnOkBottom.Text = btnText;
			}
		}

		/// <summary>
		/// Gets / sets the ToolTip for the top and bottom Ok buttons on the page. The ToolTip is rendered as 
		/// the title attribute of the input HTML tag.
		/// </summary>
		public string OkButtonToolTip
		{
			get
			{
				return btnOkTop.ToolTip;
			}
			set
			{
				// Ensure value is less than 250 characters.
				string tooltipText = value.PadRight(250).Substring(0, 250).Trim();
				btnOkTop.ToolTip = tooltipText;
				btnOkBottom.ToolTip = tooltipText;
			}
		}

		/// <summary>
		/// Gets / sets the text that appears on the top and bottom Cancel buttons on the page. This is rendered as the value
		/// attribute of the input HTML tag.
		/// </summary>
		public string CancelButtonText
		{
			// This is the text that appears on the top and bottom Cancel buttons.
			get
			{
				return btnCancelTop.Text;
			}
			set
			{
				// Ensure value is less than 25 characters.
				string btnText = value.PadRight(25).Substring(0, 25).Trim();
				btnCancelTop.Text = btnText;
				btnCancelBottom.Text = btnText;
			}
		}

		/// <summary>
		/// Gets / sets the ToolTip for the top and bottom Cancel buttons on the page. The ToolTip is rendered as 
		/// the title attribute of the HTML tag.
		/// </summary>
		public string CancelButtonToolTip
		{
			get
			{
				return btnCancelTop.ToolTip;
			}
			set
			{
				// Ensure value is less than 250 characters.
				string tooltipText = value.PadRight(250).Substring(0, 250).Trim();
				btnCancelTop.ToolTip = tooltipText;
				btnCancelBottom.ToolTip = tooltipText;
			}
		}

		/// <summary>
		/// Gets / sets the visibility of the top and bottom Ok buttons on the page. When true, the buttons
		/// are visible. When false, they are not visible (not rendered in the page output.)
		/// </summary>
		public bool OkButtonIsVisible
		{
			get
			{
				return btnOkTop.Visible;
			}
			set
			{
				btnOkTop.Visible = value;
				btnOkBottom.Visible = value;
			}
		}

		/// <summary>
		/// Gets / sets the visibility of the top and bottom Cancel buttons on the page. When true, the buttons
		/// are visible. When false, they are not visible (not rendered in the page output.)
		/// </summary>
		public bool CancelButtonIsVisible
		{
			get
			{
				return btnCancelTop.Visible;
			}
			set
			{
				btnCancelTop.Visible = value;
				btnCancelBottom.Visible = value;
			}
		}

		/// <summary>
		/// Gets a reference to the top button that initiates the completion of the task.
		/// </summary>
		public Button OkButtonTop
		{
			get
			{
				return btnOkTop;
			}
		}

		/// <summary>
		/// Gets a reference to the bottom button that initiates the completion of the task.
		/// </summary>
		public Button OkButtonBottom
		{
			get
			{
				return btnOkBottom;
			}
		}
		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			this.Page.Form.DefaultButton = btnOkTop.UniqueID;
		}

		#endregion
	}
}

using System;
using GalleryServerPro.Web.Controls;

namespace GalleryServerPro.Web.gs.pages
{
	public partial class album : Pages.GalleryPage
	{
		#region Private Fields


		#endregion

		#region Public Properties

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			ShowMessage();
		}

		#endregion

		#region Private Static Methods

		#endregion

		/// <summary>
		/// Renders the <see cref="Message"/>. No action is taken if <see cref="Message"/> is Message.None.
		/// </summary>
		private void ShowMessage()
		{
			if (this.Message == Message.None)
				return;

			usermessage msgBox = this.GetMessageControl();

			phMessage.Controls.Add(msgBox);
		}
	}
}

namespace GalleryServerPro.Web.Entity
{
	public enum EmailTemplateForm
	{
		AdminNotificationAccountCreated,
		AdminNotificationAccountCreatedRequiresApproval,
		UserNotificationAccountCreated,
		UserNotificationAccountCreatedApprovalGiven,
		UserNotificationAccountCreatedNeedsApproval,
		UserNotificationAccountCreatedNeedsVerification,
		UserNotificationPasswordChanged,
		UserNotificationPasswordChangedByAdmin,
		UserNotificationPasswordRecovery
	}
	
	public class EmailTemplate
	{
		/// <summary>
		/// The e-mail subject.
		/// </summary>
		public string Subject;

		/// <summary>
		/// The e-mail body.
		/// </summary>
		public string Body;

		/// <summary>
		/// A value that identifies the type of e-mail template.
		/// </summary>
		public EmailTemplateForm EmailTemplateId;
	}
}

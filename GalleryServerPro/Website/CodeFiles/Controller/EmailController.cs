using System;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Web;
using GalleryServerPro.Business;
using GalleryServerPro.Configuration;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Entity;
using System.Web.Security;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains e-mail related functionality.
	/// </summary>
	public static class EmailController
	{
		#region Public Static Methods

		/// <overloads>
		/// Send an e-mail message.
		/// </overloads>
		/// <summary>
		/// Send a plain text email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. The email
		/// is sent to the address configured in the emailToAddress setting in the configuration file. If
		/// <paramref name="sendOnBackgroundThread"/> is true, the e-mail is sent on a background thread and the function 
		/// returns immediately. An exception is thrown if an error occurs while sending the e-mail, unless <paramref name="sendOnBackgroundThread"/>
		/// is true, in which case the error is logged but the exception does not propagate back to the UI thread.
		/// </summary>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		/// <param name="sendOnBackgroundThread">If set to <c>true</c> send e-mail on a background thread. This causes any errors
		/// to be silently handled by the error logging system, so if it is important for any errors to propogate to the UI,
		/// such as when testing the e-mail function in the Site Administration area, set to <c>false</c>.</param>
		/// <exception cref="WebException">Thrown when a SMTP Server is not specified. (Not thrown when 
		/// <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		/// <exception cref="SmtpException">Thrown when the connection to the SMTP server failed, authentication failed,
		/// or the operation timed out. (Not thrown when <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		/// <exception cref="SmtpFailedRecipientsException">The message could not be delivered to one or more 
		/// recipients. (Not thrown when <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		public static void SendEmail(string subject, string body, bool sendOnBackgroundThread)
		{
			MailAddress recipient = new MailAddress(Config.GetCore().EmailToAddress, Config.GetCore().EmailToName);
			
			SendEmail(recipient, subject, body, sendOnBackgroundThread);
		}

		/// <summary>
		/// Send a plain text email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. If the emailRecipient parameter is not specified, the email
		/// is sent to the address configured in the emailToAddress setting in the configuration file. The e-mail is sent on a 
		/// background thread, so if an error occurs on that thread no exception bubbles to the caller (the error, however, is
		/// recorded in the error log). If it is important to know if the e-mail was successfully sent, use the overload of this
		/// method that specifies a sendOnBackgroundThread parameter.
		/// </summary>
		/// <param name="emailRecipient">The recipient of the email.</param>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		public static void SendEmail(MailAddress emailRecipient, string subject, string body)
		{
			MailAddressCollection mailAddresses = new MailAddressCollection();
			mailAddresses.Add(emailRecipient);

			SendEmail(mailAddresses, subject, body, false, true);
		}

		/// <summary>
		/// Send an email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. If the emailRecipient parameter is not specified, the email
		/// is sent to the address configured in the emailToAddress setting in the configuration file. If
		/// <paramref name="sendOnBackgroundThread"/> is true, the e-mail is sent on a background thread and the function 
		/// returns immediately. An exception is thrown if an error occurs while sending the e-mail, unless <paramref name="sendOnBackgroundThread"/>
		/// is true, in which case the error is logged but the exception does not propagate back to the UI thread.
		/// </summary>
		/// <param name="emailRecipient">The recipient of the email.</param>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		/// <param name="sendOnBackgroundThread">If set to <c>true</c> send e-mail on a background thread. This causes any errors
		/// to be silently handled by the error logging system, so if it is important for any errors to propogate to the UI,
		/// such as when testing the e-mail function in the Site Administration area, set to <c>false</c>.</param>
		/// <exception cref="WebException">Thrown when a SMTP Server is not specified. (Not thrown when 
		/// <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		/// <exception cref="SmtpException">Thrown when the connection to the SMTP server failed, authentication failed,
		/// or the operation timed out. (Not thrown when <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		/// <exception cref="SmtpFailedRecipientsException">The message could not be delivered to the
		/// <paramref name="emailRecipient"/>. (Not thrown when <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		public static void SendEmail(MailAddress emailRecipient, string subject, string body, bool sendOnBackgroundThread)
		{
			MailAddressCollection mailAddresses = new MailAddressCollection();
			mailAddresses.Add(emailRecipient);

			SendEmail(mailAddresses, subject, body, false, sendOnBackgroundThread);
		}

		/// <summary>
		/// Send an email with the specified properties. The email will appear to come from the name and email specified in the
		/// EmailFromName and EmailFromAddress configuration settings. If the emailRecipient parameter is not specified, the email
		/// is sent to the address configured in the emailToAddress setting in the configuration file. If
		/// <paramref name="sendOnBackgroundThread"/> is true, the e-mail is sent on a background thread and the function 
		/// returns immediately. An exception is thrown if an error occurs while sending the e-mail, unless <paramref name="sendOnBackgroundThread"/>
		/// is true, in which case the error is logged but the exception does not propagate back to the UI thread.
		/// </summary>
		/// <param name="emailRecipients">The email recipients.</param>
		/// <param name="subject">The text to appear in the subject of the email.</param>
		/// <param name="body">The body of the email. If the body is HTML, specify true for the isBodyHtml parameter.</param>
		/// <param name="isBodyHtml">Indicates whether the body of the email is in HTML format. When false, the body is
		/// assumed to be plain text.</param>
		/// <param name="sendOnBackgroundThread">If set to <c>true</c> send e-mail on a background thread. This causes any errors
		/// to be silently handled by the error logging system, so if it is important for any errors to propogate to the UI,
		/// such as when testing the e-mail function in the Site Administration area, set to <c>false</c>.</param>
		/// <exception cref="WebException">Thrown when a SMTP Server is not specified. (Not thrown when 
		/// <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		/// <exception cref="SmtpException">Thrown when the connection to the SMTP server failed, authentication failed,
		/// or the operation timed out. (Not thrown when <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		/// <exception cref="SmtpFailedRecipientsException">The message could not be delivered to one or more of the
		/// <paramref name="emailRecipients"/>. (Not thrown when <paramref name="sendOnBackgroundThread"/> is true.)</exception>
		public static void SendEmail(MailAddressCollection emailRecipients, string subject, string body, bool isBodyHtml, bool sendOnBackgroundThread)
		{
			Core coreConfig = Config.GetCore();

			MailAddress emailSender = new MailAddress(coreConfig.EmailFromAddress, coreConfig.EmailFromName);

			MailMessage mail = new MailMessage();

			try
			{
				foreach (MailAddress mailAddress in emailRecipients)
				{
					mail.To.Add(mailAddress);
				}
				mail.From = emailSender;
				mail.Subject = subject;
				mail.Body = body;
				mail.IsBodyHtml = isBodyHtml;

				// Because sending the e-mail takes a long time, spin off a thread to send it, unless caller specifically doesn't want to.
				if (sendOnBackgroundThread)
				{
					Thread notifyAdminThread = new Thread(SendEmail);
					notifyAdminThread.Start(mail);
				}
				else
				{
					SendEmail(mail, false);
					mail.Dispose();
				}
			}
			catch
			{
				mail.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Gets the email template. Replacement parameters in the template are replaced with their appropriate values. The data
		/// in the template can be used to construct an e-mail.
		/// </summary>
		/// <param name="template">The template to retrieve.</param>
		/// <param name="user">The user associated with the template.</param>
		/// <returns>Returns an e-mail template.</returns>
		public static EmailTemplate GetEmailTemplate(EmailTemplateForm template, UserEntity user)
		{
			EmailTemplate emailTemplate = new EmailTemplate();
			emailTemplate.EmailTemplateId = template;

			string filePath = Util.GetPath(String.Format(CultureInfo.InvariantCulture, "/templates/{0}.txt", template));

			// Step 1: Get subject and body from text file and assign to fields.
			using (StreamReader sr = File.OpenText(filePath))
			{
				while (sr.Peek() >= 0)
				{
					string lineText = sr.ReadLine().Trim();

					if (lineText == "[Subject]")
						emailTemplate.Subject = sr.ReadLine();

					if (lineText == "[Body]")
						emailTemplate.Body = sr.ReadToEnd();
				}
			}

			// Step 2: Update replacement parameters with real values.
			emailTemplate.Body = emailTemplate.Body.Replace("{CurrentPageUrlFull}", Util.GetCurrentPageUrlFull());
			emailTemplate.Body = emailTemplate.Body.Replace("{UserName}", user.UserName);
			emailTemplate.Body = emailTemplate.Body.Replace("{Email}", String.IsNullOrEmpty(user.Email) ? Resources.GalleryServerPro.Email_Template_No_Email_For_User_Replacement_Text : user.Email);

			if (emailTemplate.Body.Contains("{VerificationUrl}"))
				emailTemplate.Body = emailTemplate.Body.Replace("{VerificationUrl}", GenerateVerificationLink(user.UserName));

			if (emailTemplate.Body.Contains("{Password}"))
				emailTemplate.Body = emailTemplate.Body.Replace("{Password}", UserController.GetPassword(user.UserName));

			if (emailTemplate.Body.Contains("{ManageUserUrl}"))
				emailTemplate.Body = emailTemplate.Body.Replace("{ManageUserUrl}", GenerateManageUserLink(user.UserName));

			return emailTemplate;
		}

		/// <summary>
		/// Sends an e-mail based on the <paramref name="templateForm"/> to the specified <paramref name="user"/>.
		/// No action is taken if the user's e-mail is null or empty. The e-mail is sent on a 
		/// background thread, so if an error occurs on that thread no exception bubbles to the caller (the error, however, is
		/// recorded in the error log).
		/// </summary>
		/// <param name="user">The user to receive the e-mail.</param>
		/// <param name="templateForm">The template form specifying the type of e-mail to send.</param>
		public static void SendNotificationEmail(UserEntity user, EmailTemplateForm templateForm)
		{
			SendNotificationEmail(user, templateForm, true);
		}

		/// <summary>
		/// Sends an e-mail based on the <paramref name="templateForm"/> to the specified <paramref name="user"/>.
		/// No action is taken if the user's e-mail is null or empty. The e-mail is sent on a 
		/// background thread, so if an error occurs on that thread no exception bubbles to the caller (the error, however, is
		/// recorded in the error log). If <paramref name="sendOnBackgroundThread"/> is true, the e-mail is sent on a background 
		/// thread and the function returns immediately. An exception is thrown if an error occurs while sending the e-mail, 
		/// unless <paramref name="sendOnBackgroundThread"/> is true, in which case the error is logged but the exception does 
		/// not propagate back to the UI thread.
		/// </summary>
		/// <param name="user">The user to receive the e-mail.</param>
		/// <param name="templateForm">The template form specifying the type of e-mail to send.</param>
		/// <param name="sendOnBackgroundThread">If set to <c>true</c> send e-mail on a background thread. This causes any errors
		/// to be silently handled by the error logging system, so if it is important for any errors to propogate to the UI,
		/// such as when testing the e-mail function in the Site Administration area, set to <c>false</c>.</param>
		public static void SendNotificationEmail(UserEntity user, EmailTemplateForm templateForm, bool sendOnBackgroundThread)
		{
			if (String.IsNullOrEmpty(user.Email))
				return;

			EmailTemplate emailTemplate = GetEmailTemplate(templateForm, user);

			MailAddress emailRecipient = new MailAddress(user.Email, user.UserName);

			SendEmail(emailRecipient, emailTemplate.Subject, emailTemplate.Body, sendOnBackgroundThread);
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Sends the email. Any exception that occurs is recorded but not allowed to propagate up the stack. Use this overload
		/// to send an e-mail on a background thread. The <paramref name="mail"/> object is disposed at the end of the function.
		/// </summary>
		/// <param name="mail">The mail message to send.</param>
		private static void SendEmail(object mail)
		{
			MailMessage mailMessage = mail as MailMessage;

			if (mail == null)
				throw new ArgumentException("mail");

			SendEmail(mailMessage, true);

			if (mailMessage != null)
				mailMessage.Dispose();
		}

		/// <summary>
		/// Sends the e-mail. If <paramref name="suppressException"/> is <c>true</c>, record any exception that occurs but do not 
		/// let it propagate out of this function. When <c>false</c>, record the exception and re-throw it. The caller is 
		/// responsible for disposing the <paramref name="mail"/> object.
		/// </summary>
		/// <param name="mail">The mail message to send.</param>
		/// <param name="suppressException">If <c>true</c>, record any exception that occurs but do not 
		/// let it propagate out of this function. When <c>false</c>, record the exception and re-throw it.</param>
		private static void SendEmail(MailMessage mail, bool suppressException)
		{
			try
			{
				if (mail == null)
					throw new ArgumentException("mail");

				Core core = Config.GetCore();

				SmtpClient smtpClient = new SmtpClient();
				smtpClient.EnableSsl = core.SendEmailUsingSsl;

				string smtpServer = core.SmtpServer;
				int smtpServerPort;
				if (!Int32.TryParse(core.SmtpServerPort, out smtpServerPort))
					smtpServerPort = Int32.MinValue;

				// Specify SMTP server if it's in galleryserverpro.config. The server might have been assigned via web.config,
				// so only update this if we have a config setting.
				if (!String.IsNullOrEmpty(smtpServer))
				{
					smtpClient.Host = smtpServer;
				}

				// Specify port number if it's in galleryserverpro.config and it's not the default value of 25. The port 
				// might have been assigned via web.config, so only update this if we have a config setting.
				if ((smtpServerPort > 0) && (smtpServerPort != 25))
				{
					smtpClient.Port = smtpServerPort;
				}

				if (String.IsNullOrEmpty(smtpClient.Host))
					throw new WebException(@"Cannot send e-mail because a SMTP Server is not specified. This can be configured in any of the following places: (1) Site Admin - E-mail page (preferred), (2) smtpServer attribute of galleryserverpro.config (this is where the value on the Site Admin page is stored), or (3) web.config (Ex: configuration/system.net/mailSettings/smtp/network host='your SMTP server').");

				smtpClient.Send(mail);
			}
			catch (Exception ex)
			{
				AppErrorController.LogError(ex);

				if (!suppressException)
					throw;
			}
		}

		private static string GenerateVerificationLink(string userName)
		{
			return String.Concat(Util.GetHostUrl(), Util.GetUrl(PageId.createaccount, "verify={0}", Util.UrlEncode(HelperFunctions.Encrypt(userName))));
		}

		private static string GenerateManageUserLink(string userName)
		{
			return String.Concat(Util.GetHostUrl(), Util.GetUrl(PageId.admin_manageusers, "action=edit&user={0}", Util.UrlEncode(HelperFunctions.Encrypt(userName))));
		}

		#endregion
	}
}

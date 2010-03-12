using System;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Specifies the type of the display object.
	/// </summary>
	public enum DisplayObjectType
	{
		/// <summary>
		/// Gets the Unknown display object type.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Gets the Thumbnail display object type.
		/// </summary>
		Thumbnail = 1,
		/// <summary>
		/// Gets the Optimized display object type.
		/// </summary>
		Optimized = 2,
		/// <summary>
		/// Gets the Original display object type.
		/// </summary>
		Original = 3,
		/// <summary>
		/// Gets the display object type that represents a media object that is external to Gallery Server Pro (e.g. YouTube, Silverlight).
		/// </summary>
		External
	}

	/// <summary>
	/// Contains functionality to support the <see cref="DisplayObjectType" /> enumeration.
	/// </summary>
	public static class DisplayObjectTypeEnumHelper
	{
		/// <summary>
		/// Determines if the displayType parameter is one of the defined enumerations. This method is more efficient than using
		/// <see cref="Enum.IsDefined" />, since <see cref="Enum.IsDefined" /> uses reflection.
		/// </summary>
		/// <param name="displayType">A <see cref="DisplayObjectType" /> to test.</param>
		/// <returns>Returns true if displayType is one of the defined items in the enumeration; otherwise returns false.</returns>
		public static bool IsValidDisplayObjectType(DisplayObjectType displayType)
		{
			switch (displayType)
			{
				case DisplayObjectType.External:
				case DisplayObjectType.Optimized:
				case DisplayObjectType.Original:
				case DisplayObjectType.Thumbnail:
				case DisplayObjectType.Unknown:
					break;

				default:
					return false;
			}
			return true;
		}
	}

	/// <summary>
	/// Contains functionality to support the <see cref="MediaObjectTransitionType" /> enumeration.
	/// </summary>
	public static class MediaObjectTransitionTypeEnumHelper
	{
		/// <summary>
		/// Determines if the transitionType parameter is one of the defined enumerations. This method is more efficient than using
		/// <see cref="Enum.IsDefined" />, since <see cref="Enum.IsDefined" /> uses reflection.
		/// </summary>
		/// <param name="transitionType">An instance of <see cref="MediaObjectTransitionType" /> to test.</param>
		/// <returns>Returns true if transitionType is one of the defined items in the enumeration; otherwise returns false.</returns>
		public static bool IsValidMediaObjectTransitionType(MediaObjectTransitionType transitionType)
		{
			switch (transitionType)
			{
				case MediaObjectTransitionType.None:
				case MediaObjectTransitionType.Fade:
					break;

				default:
					return false;
			}
			return true;
		}
	}

	/// <summary>
	/// Specifies the category to which this mime type belongs. This usually corresponds to the first portion of 
	/// the full mime type description. (e.g. "image" if the full mime type is "image/jpeg") The one exception to 
	/// this is the "Other" enumeration, which represents any category not represented by the others. If a value
	/// has not yet been assigned, it defaults to the NotSet value.
	/// </summary>
	public enum MimeTypeCategory
	{
		/// <summary>
		/// Gets the NotSet mime type name, which indicates that no assignment has been made.
		/// </summary>
		NotSet = 0,
		/// <summary>
		/// Gets the Other mime type name.
		/// </summary>
		Other,
		/// <summary>
		/// Gets the Image mime type name.
		/// </summary>
		Image,
		/// <summary>
		/// Gets the Video mime type name.
		/// </summary>
		Video,
		/// <summary>
		/// Gets the Audio mime type name.
		/// </summary>
		Audio
	}

	/// <summary>
	/// Contains functionality to support the <see cref="MimeTypeCategory" /> enumeration.
	/// </summary>
	public static class MimeTypeEnumHelper
	{
		/// <summary>
		/// Determines if the mimeTypeCategory parameter is one of the defined enumerations. This method is more efficient than using
		/// <see cref="Enum.IsDefined" />, since <see cref="Enum.IsDefined" /> uses reflection.
		/// </summary>
		/// <param name="mimeTypeCategory">An instance of <see cref="MimeTypeCategory" /> to test.</param>
		/// <returns>Returns true if mimeTypeCategory is one of the defined items in the enumeration; otherwise returns false.</returns>
		public static bool IsValidMimeTypeCategory(MimeTypeCategory mimeTypeCategory)
		{
			switch (mimeTypeCategory)
			{
				case MimeTypeCategory.NotSet:
				case MimeTypeCategory.Audio:
				case MimeTypeCategory.Image:
				case MimeTypeCategory.Other:
				case MimeTypeCategory.Video:
					break;

				default:
					return false;
			}
			return true;
		}

		/// <summary>
		/// Parses the string into an instance of <see cref="MimeTypeCategory" />. If <paramref name="mimeTypeCategory"/> is null or empty, then 
		/// MimeTypeCategory.NotSet is returned.
		/// </summary>
		/// <param name="mimeTypeCategory">The MIME type category to parse into an instance of <see cref="MimeTypeCategory" />.</param>
		/// <returns>Returns an instance of <see cref="MimeTypeCategory" />.</returns>
		public static MimeTypeCategory ParseMimeTypeCategory(string mimeTypeCategory)
		{
			if (String.IsNullOrEmpty(mimeTypeCategory))
			{
				return MimeTypeCategory.NotSet;
			}

			return (MimeTypeCategory)Enum.Parse(typeof(MimeTypeCategory), mimeTypeCategory.Trim(), true);
		}
	}

	/// <summary>
	/// Specifies the position for a pager rendered to a UI. A pager is a control that allows a user to navigate
	/// large collections of objects. It typically has next and previous buttons, and my contain buttons for quickly
	/// accessing intermediate pages.
	/// </summary>
	public enum PagerPosition
	{
		/// <summary>
		/// A pager positioned at the top of the control.
		/// </summary>
		Top = 0,
		/// <summary>
		/// A pager positioned at the bottom of the control.
		/// </summary>
		Bottom,
		/// <summary>
		/// Pagers positioned at both the top and the bottom of the control.
		/// </summary>
		TopAndBottom
	}

	/// <summary>
	/// Contains functionality to support the <see cref="PagerPosition" /> enumeration.
	/// </summary>
	public static class PagerPositionEnumHelper
	{
		/// <summary>
		/// Determines if the <paramref name="pagerPosition"/> is one of the defined enumerations. This method is more efficient than using
		/// <see cref="Enum.IsDefined" />, since <see cref="Enum.IsDefined" /> uses reflection.
		/// </summary>
		/// <param name="pagerPosition">An instance of <see cref="PagerPosition" /> to test.</param>
		/// <returns>Returns true if <paramref name="pagerPosition"/> is one of the defined items in the enumeration; otherwise returns false.</returns>
		public static bool IsValidPagerPosition(PagerPosition pagerPosition)
		{
			switch (pagerPosition)
			{
				case PagerPosition.Top:
				case PagerPosition.Bottom:
				case PagerPosition.TopAndBottom:
					break;

				default:
					return false;
			}
			return true;
		}

		/// <summary>
		/// Parses the string into an instance of <see cref="PagerPosition" />. If <paramref name="pagerPosition"/> is null or empty, an 
		/// <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <param name="pagerPosition">The pager position to parse into an instance of <see cref="PagerPosition" />.</param>
		/// <returns>Returns an instance of <see cref="PagerPosition" />.</returns>
		public static PagerPosition ParsePagerPosition(string pagerPosition)
		{
			if (String.IsNullOrEmpty(pagerPosition))
				throw new ArgumentException("Invalid PagerPosition value: " + pagerPosition, "pagerPosition");

			return (PagerPosition)Enum.Parse(typeof(PagerPosition), pagerPosition.Trim(), true);
		}
	}

	/// <summary>
	/// Specifies the trust level of the current application domain. For web applications, this maps to the
	/// AspNetHostingPermissionLevel.
	/// </summary>
	public enum ApplicationTrustLevel
	{
		/// <summary>Specifies that this enumeration has not been assigned a value.</summary>
		None = 0,
		/// <summary>Gets the Unknown trust level. This is used when the trust level cannot be determined.</summary>
		Unknown = 10,
		/// <summary>Gets the Minimal trust level.</summary>
		Minimal = 20,
		/// <summary>Gets the Low trust level.</summary>
		Low = 30,
		/// <summary>Gets the Medium trust level.</summary>
		Medium = 40,
		/// <summary>Gets the High trust level.</summary>
		High = 50,
		/// <summary>Gets the Full trust level.</summary>
		Full = 60
	}

	/// <summary>
	/// Specifies one or more security-related action within Gallery Server Pro. A user may or may not have authorization to
	/// perform each security action. A user's authorization is determined by the role or roles to which he or she
	/// belongs. This enumeration is defined with the Flags attribute, so one can combine multiple security actions by
	/// performing a bitwise OR.
	/// </summary>
	[Flags]
	public enum SecurityActions
	{
		/// <summary>
		/// Represents the ability to view an album or media object. Does not include the ability to view high resolution
		/// versions of images. Includes the ability to download the media object and view a slide show.
		/// </summary>
		ViewAlbumOrMediaObject = 1,
		/// <summary>
		/// Represents the ability to create a new album within the current album. This includes the ability to move or
		/// copy an album into the current album.
		/// </summary>
		AddChildAlbum = 2,
		/// <summary>
		/// Represents the ability to add a new media object to the current album. This includes the ability to move or
		/// copy a media object into the current album.
		/// </summary>
		AddMediaObject = 4,
		/// <summary>
		/// Represents the ability to edit an album's title, summary, and begin and end dates. Also includes rearranging the
		/// order of objects within the album and assigning the album's thumbnail image. Does not include the ability to
		/// add or delete child albums or media objects.
		/// </summary>
		EditAlbum = 8,
		/// <summary>
		/// Represents the ability to edit a media object's caption, rotate it, and delete the high resolution version of
		/// an image.
		/// </summary>
		EditMediaObject = 16,
		/// <summary>
		/// Represents the ability to delete the current album. This permission is required to move 
		/// albums to another album, since it is effectively deleting it from the current album's parent.
		/// </summary>
		DeleteAlbum = 32,
		/// <summary>
		/// Represents the ability to delete child albums within the current album.
		/// </summary>
		DeleteChildAlbum = 64,
		/// <summary>
		/// Represents the ability to delete media objects within the current album. This permission is required to move 
		/// media objects to another album, since it is effectively deleting it from the current album.
		/// </summary>
		DeleteMediaObject = 128,
		/// <summary>
		/// Represents the ability to synchronize media objects on the hard drive with records in the data store.
		/// </summary>
		Synchronize = 256,
		/// <summary>
		/// Represents the ability to administer all aspects of Gallery Server Pro. Automatically includes all other permissions.
		/// </summary>
		AdministerSite = 512,
		/// <summary>
		/// Represents the ability to not render a watermark over media objects.
		/// </summary>
		HideWatermark = 1024,
		/// <summary>
		/// Represents the ability to view the original high resolution version of images.
		/// </summary>
		ViewOriginalImage = 2048,
		/// <summary>
		/// Represents all possible permissions. Note: This enum value is defined to contain ALL POSSIBLE enum values to ensure
		/// the <see cref="SecurityActionEnumHelper.IsValidSecurityAction(SecurityActions)" /> method properly works. If a developer adds or removes
		/// items from this enum, this item must be updated to reflect the ORed list of all possible values.
		/// </summary>
		All = (ViewAlbumOrMediaObject | AddChildAlbum | AddMediaObject | EditAlbum | EditMediaObject | DeleteAlbum | DeleteChildAlbum | DeleteMediaObject | Synchronize | AdministerSite | HideWatermark | ViewOriginalImage)
	}

	/// <summary>
	/// Contains functionality to support the <see cref="SecurityActions" /> enumeration.
	/// </summary>
	public static class SecurityActionEnumHelper
	{
		/// <summary>
		/// Determines if the securityActions parameter is one of the defined enumerations or a valid combination of valid enumeration
		/// values (since <see cref="SecurityActions" /> is defined with the Flags attribute). <see cref="Enum.IsDefined" /> cannot be used since it does not return
		/// true when the enumeration contains more than one enum value. This method requires the <see cref="SecurityActions" /> enum to have a member
		/// All that contains every enum value ORed together.
		/// </summary>
		/// <param name="securityActions">A <see cref="SecurityActions" />. It may be a single value or some
		/// combination of valid enumeration values.</param>
		/// <returns>Returns true if securityActions is one of the defined items in the enumeration or is a valid combination of
		/// enumeration values; otherwise returns false.</returns>
		public static bool IsValidSecurityAction(SecurityActions securityActions)
		{
			if ((securityActions != 0) && ((securityActions & SecurityActions.All) == securityActions))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Determines if the securityActions parameter is one of the defined enumerations or a valid combination of valid enumeration
		/// values (since <see cref="SecurityActions" /> is defined with the Flags attribute). <see cref="Enum.IsDefined" /> cannot be used since it does not return
		/// true when the enumeration contains more than one enum value. This method requires the <see cref="SecurityActions" /> enum to have a member
		/// All that contains every enum value ORed together.
		/// </summary>
		/// <param name="securityActions">An integer representing a <see cref="SecurityActions" />.</param>
		/// <returns>Returns true if securityAction is one of the defined items in the enumeration or is a valid combination of
		/// enumeration values; otherwise returns false.</returns>
		public static bool IsValidSecurityAction(int securityActions)
		{
			if ((securityActions != 0) && ((securityActions & (int)SecurityActions.All) == securityActions))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Determines if the specified value is a single, valid enumeration value. Since the <see cref="SecurityActions" /> enum has the 
		/// Flags attribute and may contain a bitwise combination of more than one value, this function is useful in
		/// helping the developer decide if the enum value is just one value or it must be parsed into its constituent
		/// parts with the GalleryServerPro.Business.SecurityManager.ParseSecurityAction method.
		/// </summary>
		/// <param name="securityActions">A <see cref="SecurityActions" />. It may be a single value or some
		/// combination of valid enumeration values.</param>
		/// <returns>Returns true if securityAction is a valid, single bit flag; otherwise return false.</returns>
		public static bool IsSingleSecurityAction(SecurityActions securityActions)
		{
			if (IsValidSecurityAction(securityActions) && (securityActions == SecurityActions.ViewAlbumOrMediaObject)
			    || (securityActions == SecurityActions.ViewOriginalImage) || (securityActions == SecurityActions.AddMediaObject)
			    || (securityActions == SecurityActions.AdministerSite) || (securityActions == SecurityActions.DeleteAlbum)
			    || (securityActions == SecurityActions.DeleteChildAlbum) || (securityActions == SecurityActions.DeleteMediaObject)
			    || (securityActions == SecurityActions.EditAlbum) || (securityActions == SecurityActions.EditMediaObject)
			    || (securityActions == SecurityActions.HideWatermark) || (securityActions == SecurityActions.Synchronize)
			    || (securityActions == SecurityActions.AddChildAlbum))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Parses the security action into one or more <see cref="SecurityActions"/>. Since the <see cref="SecurityActions" /> 
		/// enum has the Flags attribute and may contain a bitwise combination of more than one value, this function is useful
		/// in creating a list of the values that can be enumerated.
		/// </summary>
		/// <param name="securityActionsToParse">A <see cref="SecurityActions" />. It may be a single value or some
		/// combination of valid enumeration values.</param>
		/// <returns>Returns a list of <see cref="SecurityActions"/> that can be enumerated.</returns>
		public static System.Collections.Generic.List<SecurityActions> ParseSecurityAction(SecurityActions securityActionsToParse)
		{
			System.Collections.Generic.List<SecurityActions> securityActions = new System.Collections.Generic.List<SecurityActions>(2);

			foreach (SecurityActions securityActionIterator in Enum.GetValues(typeof(SecurityActions)))
			{
				if ((securityActionsToParse & securityActionIterator) == securityActionIterator)
				{
					securityActions.Add(securityActionIterator);
				}
			}

			return securityActions;
		}
	}

	/// <summary>
	/// Specifies the visual transition effect to use when moving from one media object to another.
	/// </summary>
	public enum MediaObjectTransitionType
	{
		/// <summary>
		/// No visual transition effect.
		/// </summary>
		None = 0,
		/// <summary>
		/// Fading from the old to the new media object.
		/// </summary>
		Fade = 10
	}

	/// <summary>
	/// Specifies a particular item within an application error (<see cref="GalleryServerPro.Business.Interfaces.IAppError"/>).
	/// </summary>
	public enum ErrorItem
	{
		/// <summary>
		/// The value that uniquely identifies an application error.
		/// </summary>
		AppErrorId,
		/// <summary>
		/// The URL where the error occurred.
		/// </summary>
		Url,
		/// <summary>
		/// The date and time of the error.
		/// </summary>
		Timestamp,
		/// <summary>
		/// The type of the exception (e.g. System.InvalidOperationException).
		/// </summary>
		ExceptionType,
		/// <summary>
		/// The message associated with the exception. This is usually mapped to <see cref="Exception.Message"/>.
		/// </summary>
		Message,
		/// <summary>
		/// The source of the exception. This is usually mapped to <see cref="Exception.Source"/>.
		/// </summary>
		Source,
		/// <summary>
		/// The target site of the exception. This is usually mapped to <see cref="Exception.TargetSite"/>.
		/// </summary>
		TargetSite,
		/// <summary>
		/// The stack trace of the exception. This is usually mapped to <see cref="Exception.StackTrace"/>.
		/// </summary>
		StackTrace,
		/// <summary>
		/// The exception data, if any, associated with the exception. This is usually mapped to <see cref="Exception.Data"/>.
		/// </summary>
		ExceptionData,
		/// <summary>
		/// The type of the inner exception (e.g. System.InvalidOperationException).
		/// </summary>
		InnerExType,
		/// <summary>
		/// The message associated with the inner exception. This is usually mapped to <see cref="Exception.Message"/>.
		/// </summary>
		InnerExMessage,
		/// <summary>
		/// The source of the inner exception. This is usually mapped to <see cref="Exception.Source"/>.
		/// </summary>
		InnerExSource,
		/// <summary>
		/// The target site of the inner exception. This is usually mapped to <see cref="Exception.TargetSite"/>.
		/// </summary>
		InnerExTargetSite,
		/// <summary>
		/// The stack trace of the inner exception. This is usually mapped to <see cref="Exception.StackTrace"/>.
		/// </summary>
		InnerExStackTrace,
		/// <summary>
		/// The exception data, if any, associated with the exception. This is usually mapped to <see cref="Exception.Data"/>.
		/// </summary>
		InnerExData,
		/// <summary>
		/// The ID of the gallery where the error occurred.
		/// </summary>
		GalleryId,
		/// <summary>
		/// The HTTP user agent (that is, the browser) the user was using when the error occurred.
		/// </summary>
		HttpUserAgent,
		/// <summary>
		/// Refers to the collection of form variables on the web page when the error occurred.
		/// </summary>
		FormVariables,
		/// <summary>
		/// Refers to the cookies associated with the user when the error occurried.
		/// </summary>
		Cookies,
		/// <summary>
		/// Refers to the collection of session variables on the web page when the error occurred.
		/// </summary>
		SessionVariables,
		/// <summary>
		/// Refers to the collection of server variables on the web page when the error occurred.
		/// </summary>
		ServerVariables
	}
}

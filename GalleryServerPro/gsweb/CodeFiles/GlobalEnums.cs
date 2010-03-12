using System;

namespace GalleryServerPro.Web
{
	#region Public Enums

	/// <summary>
	/// Specifies a particular message that is to be displayed to the user. The text of the message is extracted from the resource file.
	/// </summary>
	public enum Message
	{
		None = 0,
		ThumbnailSuccessfullyAssigned = 1,
		CannotAssignThumbnailNoObjectsExistInAlbum = 2,
		CannotEditCaptionsNoEditableObjectsExistInAlbum = 3,
		CannotRearrangeNoObjectsExistInAlbum = 4,
		CannotRotateNoRotateableObjectsExistInAlbum = 5,
		CannotMoveNoObjectsExistInAlbum = 6,
		CannotCopyNoObjectsExistInAlbum = 7,
		CannotDeleteHiResImagesNoObjectsExistInAlbum = 8,
		CannotDeleteObjectsNoObjectsExistInAlbum = 9,
		OneOrMoreCaptionsExceededMaxLength = 10,
		CaptionExceededMaxLength = 11,
		MediaObjectDoesNotExist = 12,
		AlbumDoesNotExist = 13,
		NoScriptDefaultText = 14,
		SynchronizationSuccessful = 15,
		SynchronizationFailure = 16,
		ObjectsSuccessfullyDeleted = 17,
		HiResImagesSuccessfullyDeleted = 18,
		UserNameOrPasswordIncorrect = 19,
		AlbumNameExceededMaxLength = 20,
		AlbumSummaryExceededMaxLength = 21,
		AlbumNameAndSummaryExceededMaxLength = 22,
		InsufficientPermissionCannotViewAlbum = 23,
		NoAuthorizedAlbumForUser = 24,
		CannotOverlayWatermarkOnImage = 25,
		CannotRotateObjectNotRotatable = 26,
		ObjectsSuccessfullyMoved = 27,
		ObjectsSuccessfullyCopied = 28,
		ObjectsSuccessfullyRearranged = 29,
		ObjectsSuccessfullyRotated = 30,
		ObjectsSkippedDuringUpload = 31,
		CannotRotateInvalidImage = 32
	}

	/// <summary>
	/// Specifies the style of message to be displayed to the user.
	/// </summary>
	public enum MessageStyle
	{
		None,
		Information,
		Warning,
		Error
	}

	#endregion
}

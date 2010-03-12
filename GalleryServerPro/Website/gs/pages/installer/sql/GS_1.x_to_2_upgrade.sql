DECLARE @username nvarchar(256)
DECLARE @GalleryID int
SET @username = 'System'
SET @GalleryID = 1

/* Make sure a default gallery exists. */
IF NOT EXISTS (SELECT * FROM [gs_Gallery] WHERE GalleryID = @GalleryID)
BEGIN
	INSERT gs_Gallery (GalleryId, Description, DateAdded)
	VALUES (@GalleryId, 'My Gallery', GETDATE())
END

/* Copy Album records into new album table. */
SET IDENTITY_INSERT [dbo].[gs_Album] ON

INSERT gs_Album (
	AlbumID, FKGalleryID, AlbumParentID, Title, 
	DirectoryName, 
	Summary, ThumbnailMediaObjectID, Seq, DateStart, DateEnd, DateAdded, CreatedBy, 
	LastModifiedBy, DateLastModified, OwnedBy, IsPrivate)
SELECT 
	AlbumID, 1, AlbumParentID, Title,

	CASE LEN(LTRIM(RTRIM(RelativePath)))
	 WHEN 0 THEN ''
	 ELSE
	  CASE CHARINDEX('/', REVERSE(RelativePath), 2)
	   WHEN 0 THEN LEFT(RelativePath, CHARINDEX('/', RelativePath) - 1)
	   ELSE REVERSE(SUBSTRING(REVERSE(RelativePath), 2, CHARINDEX('/', REVERSE(RelativePath), 2) - 2))
	  END
	 END,

	Summary, 0, Seq, DateStart, DateEnd, DateAdded, @username,
	@username, GETDATE(), @username, 0
 FROM Album

SET IDENTITY_INSERT [dbo].[gs_Album] OFF

/* Update Album thumbnail media object ID field. Find album whose RelativePath = ThumbnailRelativePath, 
then query mediaobjects table for the ID with this album ID and where ThumbnailFilename matches 
this field in this table. */
UPDATE gs_Album
SET ThumbnailMediaObjectID = thumbMOID
FROM
	(SELECT mo.MOID as thumbMOID, albumInner.AlbumID, thumbAlbum.AlbumID as thumbnailAlbumID, albumInner.ThumbnailFilename
	FROM Album thumbAlbum JOIN Album albumInner ON thumbAlbum.RelativePath = albumInner.ThumbnailRelativePath
	  JOIN MediaObject mo ON thumbAlbum.AlbumID = mo.FKAlbumID
	WHERE thumbAlbum.ThumbnailFilename = mo.ThumbnailFilename) TB
WHERE gs_Album.AlbumID = TB.AlbumID

/* Copy media object records into new table. */
SET IDENTITY_INSERT [dbo].[gs_MediaObject] ON

INSERT [gs_MediaObject]
	(MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, CreatedBy, 
	DateAdded, LastModifiedBy, DateLastModified, IsPrivate)
SELECT 
	MOID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, '', 'NotSet', Seq, @username, 
	DateAdded, @username, GETDATE(), 0
FROM MediaObject

SET IDENTITY_INSERT [dbo].[gs_MediaObject] OFF
	
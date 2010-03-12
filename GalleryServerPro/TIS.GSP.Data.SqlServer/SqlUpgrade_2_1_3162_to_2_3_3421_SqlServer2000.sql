/*
SQL SERVER 2000: This script updates the database schema from 2.1.3162 to 2.3.3421.
This script provides the following changes:

Step 1. Create table gs_AppError, supporting stored procedures, and add procs to roles.

Step 2. Add column OwnerRoleName to table gs_Album.

Step 3. Clear the contents of gs_Album.OwnedBy.

Step 4. Drop the following index (not needed):
   * IDX_gs_Album_AlbumParentId
   
Step 5. Create the following index to improve performance:
   * IDX_gs_Album_AlbumParentId_FKGalleryId
   
Step 6. Modify the following stored procs to reflect new table gs_AppError.
   * gs_ExportGalleryData
   * gs_DeleteData
   
Step 7. Modify the following stored procs to reflect new columns in gs_Album.
   * gs_VerifyMinimumRecords
   * gs_AlbumInsert
   * gs_AlbumSelect
   * gs_AlbumUpdate
   * gs_SelectRootAlbum

Step 8. Delete the following stored procedures (not needed).
   * gs_InitializeGalleryServerPro
   
Step 9: Update the user-defined function gs_GetVersion to reflect new database schema version.

*/

/* Step 1: Add table gs_AppError and supporting stored procedures. */

/* First delete the stored procedures and tables if they exist. We will recreate them. */
/****** Object:  StoredProcedure [dbo].[gs_AppErrorSelect] ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorSelect]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_AppErrorSelect]
GO
/****** Object:  StoredProcedure [dbo].[gs_AppErrorInsert] ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_AppErrorInsert]
GO
/****** Object:  StoredProcedure [dbo].[gs_AppErrorDelete] ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_AppErrorDelete]
GO
/****** Object:  StoredProcedure [dbo].[gs_AppErrorDeleteAll] ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorDeleteAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_AppErrorDeleteAll]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppError]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[gs_AppError]
GO

/* Now create the table, indexes, and stored procedures. */
CREATE TABLE [dbo].[gs_AppError] (
	[AppErrorId] [int] IDENTITY(1,1) NOT NULL,
	[FKGalleryId] [int] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[ExceptionType] [nvarchar] (1000) NOT NULL,
	[Message] [nvarchar] (4000) NOT NULL,
	[Source] [nvarchar] (1000) NOT NULL,
	[TargetSite] [nvarchar] (4000) NOT NULL,
	[StackTrace] [nvarchar] (4000) NOT NULL,
	[ExceptionData] [nvarchar] (4000) NOT NULL,
	[InnerExType] [nvarchar] (1000) NOT NULL,
	[InnerExMessage] [nvarchar] (4000) NOT NULL,
	[InnerExSource] [nvarchar] (1000) NOT NULL,
	[InnerExTargetSite] [nvarchar] (4000) NOT NULL,
	[InnerExStackTrace] [nvarchar] (4000) NOT NULL,
	[InnerExData] [nvarchar] (4000) NOT NULL,
	[Url] [nvarchar] (1000) NOT NULL,
	[FormVariables] [nvarchar] (4000) NOT NULL,
	[Cookies] [nvarchar] (4000) NOT NULL,
	[SessionVariables] [nvarchar] (4000) NOT NULL,
	[ServerVariables] [nvarchar] (4000) NOT NULL
 CONSTRAINT [PK_gs_AppError] PRIMARY KEY CLUSTERED 
(
	[AppErrorId] ASC
)
)
GO

/****** Object:  ForeignKey [FK_gs_AppError_gs_Gallery] ******/
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_gs_AppError_gs_Gallery]') AND type = 'F')
ALTER TABLE [dbo].[gs_AppError] WITH CHECK ADD CONSTRAINT [FK_gs_AppError_gs_Gallery] FOREIGN KEY([FKGalleryId])
REFERENCES [dbo].[gs_Gallery] ([GalleryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[gs_AppError] CHECK CONSTRAINT [FK_gs_AppError_gs_Gallery]
GO

/****** Object:  Non-clustered index [dbo].[gs_AppError].[IDX_gs_AppError_FKGalleryId]  ******/
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[gs_AppError]') AND name = N'IDX_gs_AppError_FKGalleryId')
	CREATE NONCLUSTERED INDEX [IDX_gs_AppError_FKGalleryId] ON [dbo].[gs_AppError] ([FKGalleryId] ASC )
GO

/****** Object:  StoredProcedure [dbo].[gs_AppErrorInsert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AppErrorInsert]
	(@GalleryId int, @TimeStamp datetime, @ExceptionType nvarchar (1000),
	@Message nvarchar (4000), @Source nvarchar (1000), @TargetSite nvarchar (4000),
	@StackTrace nvarchar (4000), @ExceptionData nvarchar (4000), @InnerExType nvarchar (1000),
	@InnerExMessage nvarchar (4000), @InnerExSource nvarchar (1000), @InnerExTargetSite nvarchar (4000),
	@InnerExStackTrace nvarchar (4000), @InnerExData nvarchar (4000), @Url nvarchar (1000),
	@FormVariables nvarchar (4000), @Cookies nvarchar (4000), @SessionVariables nvarchar (4000),
	@ServerVariables nvarchar (4000), @Identity int OUT)
AS

/* Insert a new application error. */
INSERT [gs_AppError]
  (FKGalleryId, [TimeStamp], ExceptionType, [Message], [Source], TargetSite, StackTrace, ExceptionData, InnerExType, 
  InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
  FormVariables, Cookies, SessionVariables, ServerVariables)
VALUES (@GalleryId, @TimeStamp, @ExceptionType, @Message, @Source, @TargetSite, @StackTrace, @ExceptionData, @InnerExType, 
  @InnerExMessage, @InnerExSource, @InnerExTargetSite, @InnerExStackTrace, @InnerExData, @Url,
  @FormVariables, @Cookies, @SessionVariables, @ServerVariables)

SET @Identity = SCOPE_IDENTITY()

RETURN
' 
END
GO

/****** Object:  StoredProcedure [dbo].[gs_AppErrorSelect] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorSelect]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AppErrorSelect]
(
	@GalleryId int
)
AS
SET NOCOUNT ON;

SELECT
  AppErrorId, FKGalleryId, [TimeStamp], ExceptionType, [Message], [Source], TargetSite, StackTrace, ExceptionData, 
  InnerExType, InnerExMessage, InnerExSource, InnerExTargetSite, InnerExStackTrace, InnerExData, Url, 
  FormVariables, Cookies, SessionVariables, ServerVariables
FROM [gs_AppError]
WHERE FKGalleryId = @GalleryId
' 
END
GO

/****** Object:  StoredProcedure [dbo].[gs_AppErrorDelete] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AppErrorDelete]
(
	@AppErrorId int
)
AS
/* Delete application error */
DELETE [gs_AppError]
WHERE AppErrorId = @AppErrorId
' 
END
GO

/****** Object:  StoredProcedure [dbo].[gs_AppErrorDeleteAll] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_AppErrorDeleteAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AppErrorDeleteAll]
(
	@GalleryId int
)
AS
/* Delete all application errors for the gallery */
DELETE [gs_AppError]
WHERE FKGalleryId = @GalleryId
'
END
GO

/* Grant permission to execute the new stored procedures to the GSP role. */
GRANT EXECUTE ON [dbo].[gs_AppErrorSelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AppErrorInsert] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AppErrorDelete] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AppErrorDeleteAll] TO [gs_GalleryServerProRole]
GO

/* Step 2: Add column OwnerRoleName to table gs_Album. Adding the column using ALTER TABLE adds it as the last
column, but we need it in the second to last position. We achieve this by taking the following steps after
adding the OwnerRoleName column: (1) Rename the original last column - IsPrivate - to a temp name. (2) Create
a new column named IsPrivate. (3) Copy the data from the old IsPrivate column to the new one. (4) Delete the 
original IsPrivate column. The OwnerRoleName column is second to last and IsPrivate is last, just like we want it.
*/
IF NOT EXISTS(SELECT * FROM dbo.syscolumns
WHERE [name]='OwnerRoleName' AND id=OBJECT_ID(N'[dbo].[gs_Album]'))
BEGIN
	ALTER TABLE [dbo].[gs_Album] ADD
		OwnerRoleName [nvarchar](256) NOT NULL CONSTRAINT DF_gs_Album_OwnerRoleName DEFAULT ''
		
	ALTER TABLE [dbo].[gs_Album]
		DROP CONSTRAINT DF_gs_Album_OwnerRoleName
END
GO

ALTER TABLE [dbo].[gs_Album]
	DROP CONSTRAINT DF_gs_Album_IsPrivate
GO
EXECUTE sp_rename N'dbo.gs_Album.IsPrivate', N'Tmp_IsPrivate', 'COLUMN' 
GO
ALTER TABLE [dbo].[gs_Album] ADD
	IsPrivate [bit] NOT NULL CONSTRAINT DF_gs_Album_IsPrivate DEFAULT 0
GO
UPDATE [dbo].[gs_Album]
SET IsPrivate = Tmp_IsPrivate
GO
ALTER TABLE [dbo].[gs_Album]
	DROP COLUMN Tmp_IsPrivate
GO

/* Step 3. Clear the contents of gs_Album.OwnedBy. */
UPDATE [dbo].[gs_Album]
SET OwnedBy = ''
GO

/* Step 4. Drop the index IDX_gs_Album_AlbumParentId. */

/* Delete the index IDX_gs_Album_AlbumParentId */
IF  EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'IDX_gs_Album_AlbumParentId')
DROP INDEX [dbo].[gs_Album].[IDX_gs_Album_AlbumParentId]
GO

/* Step 5. Create the index IDX_gs_Album_AlbumParentId_FKGalleryId. */

/****** Object:  Non-clustered index [dbo].[gs_Album].[IDX_gs_Album_AlbumParentId_FKGalleryId]  ******/
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'IDX_gs_Album_AlbumParentId_FKGalleryId')
CREATE NONCLUSTERED INDEX [IDX_gs_Album_AlbumParentId_FKGalleryId] ON [dbo].[gs_Album] 
(
 [FKGalleryId] ASC,
 [AlbumParentId] ASC
)
GO

/* Step 6: Modify the following stored procs to reflect new table gs_AppError. */

/****** Object:  StoredProcedure [dbo].[gs_ExportGalleryData] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_ExportGalleryData]
AS
SET NOCOUNT ON

SELECT AlbumId, FKGalleryId, AlbumParentId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, Seq, DateStart, DateEnd, DateAdded, CreatedBy, 
 LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
FROM gs_Album

SELECT GalleryId, Description, DateAdded
FROM gs_Gallery

SELECT MediaObjectId, FKAlbumId, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, 
 OptimizedHeight, OptimizedSizeKB, OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, CreatedBy, 
 DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM gs_MediaObject

SELECT MediaObjectMetadataId, FKMediaObjectId, MetadataNameIdentifier, Description, Value
FROM gs_MediaObjectMetadata

SELECT RoleName, FKGalleryId, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum, AllowAddMediaObject, AllowEditAlbum, 
 AllowEditMediaObject, AllowDeleteChildAlbum, AllowDeleteMediaObject, AllowSynchronize, HideWatermark, AllowAdministerSite
FROM gs_Role

SELECT FKRoleName, FKAlbumId
FROM gs_Role_Album

SELECT AppErrorId, FKGalleryId, TimeStamp, ExceptionType, Message, Source, TargetSite, StackTrace, ExceptionData, InnerExType, InnerExMessage, InnerExSource, 
 InnerExTargetSite, InnerExStackTrace, InnerExData, Url, FormVariables, Cookies, SessionVariables, ServerVariables
FROM gs_AppError

SELECT [dbo].[gs_GetVersion]() AS SchemaVersion

RETURN'
GO

/****** Object:  StoredProcedure [dbo].[gs_DeleteData] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_DeleteData]
(	@DeleteMembershipData bit, @DeleteGalleryData bit	)

AS
SET NOCOUNT ON

IF @DeleteMembershipData = 1
BEGIN
	DELETE FROM aspnet_UsersInRoles
	DELETE FROM aspnet_Profile
	DELETE FROM aspnet_Membership
	DELETE FROM aspnet_Users
	DELETE FROM aspnet_Roles
	DELETE FROM aspnet_Applications
END

IF @DeleteGalleryData = 1 
BEGIN
	DELETE FROM gs_MediaObjectMetadata
	DELETE FROM gs_MediaObject
	DELETE FROM gs_Role
	DELETE FROM gs_Role_Album
	DELETE FROM gs_Album
	DELETE FROM gs_Gallery
	DELETE FROM gs_AppError
END

RETURN'
GO

/* Step 7. Modify the following stored procs to reflect new columns in gs_Album. */

/****** Object:  StoredProcedure [dbo].[gs_VerifyMinimumRecords] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_VerifyMinimumRecords]
(
	@GalleryId int
)
AS
SET NOCOUNT ON
/* Verify that certain tables have the required records, inserting them if necessary. This proc is designed 
to be run each time the application starts. This proc is especially important the first time Gallery Server 
is run as Gallery Server depends on this proc to install the default set of data.

Gallery: This table requires at least one record to represent the current gallery. If no records are
			found, one is inserted with a ID equal to the @GalleryId parameter.
Album: This table requires at least one record to represent the root album. If no records are found, 
       a new record representing the root album is added.
Synchronize: This table requires one record for each gallery. It stores the current state of a synchronization,
			if one is in progress. When a synchronization is not in progress, the SynchState field should be zero
			for this gallery.
*/

/* ******************************************************* */
--          Check the Gallery table
/* ******************************************************* */
BEGIN TRAN
IF NOT EXISTS (SELECT * FROM [gs_Gallery] WITH (UPDLOCK, HOLDLOCK) WHERE GalleryId = @GalleryId)
BEGIN
	INSERT gs_Gallery (GalleryId, Description, DateAdded)
	VALUES (@GalleryId, ''My Gallery'', GETDATE())
END
COMMIT

/* ******************************************************* */
--          Check the Album table
/* ******************************************************* */
BEGIN TRAN
IF NOT EXISTS (SELECT * FROM [gs_Album] WITH (UPDLOCK, HOLDLOCK) WHERE AlbumParentId = 0 AND FKGalleryId = @GalleryId)
BEGIN
  /* The album table does not have a root album for the specified gallery. Insert one. */
  INSERT [gs_Album] (AlbumParentId, FKGalleryId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, Seq, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate)
  VALUES (0, @GalleryId, ''All albums'', '''',''Welcome to Gallery Server Pro!'', 0, 0, ''System'', GETDATE(), ''System'', GETDATE(), '''', '''', 0)
END 
COMMIT

/* ******************************************************* */
--          Check the Synchronize table
/* ******************************************************* */
BEGIN TRAN
IF EXISTS (SELECT * FROM [gs_Synchronize] WITH (UPDLOCK, HOLDLOCK) WHERE FKGalleryId = @GalleryId)
BEGIN -- Reset record to clear out any previous synchronization
	UPDATE gs_Synchronize
	SET SynchId = '''',
	SynchState = 0,
	TotalFiles = 0,
	CurrentFileIndex = 0
	WHERE FKGalleryId = @GalleryId
END
ELSE
BEGIN
	INSERT INTO gs_Synchronize (SynchId, FKGalleryId, SynchState, TotalFiles, CurrentFileIndex)
	VALUES ('''',@GalleryId, 0, 0, 0)
END
COMMIT

RETURN'
GO

/****** Object:  StoredProcedure [dbo].[gs_AlbumInsert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_AlbumInsert]
(@AlbumParentId int, @GalleryId int, @Title nvarchar(200), @DirectoryName nvarchar(255),
 @Summary nvarchar(1500), @ThumbnailMediaObjectId int, @Seq int, 
 @DateStart datetime, @DateEnd datetime, @CreatedBy nvarchar(256), @DateAdded datetime, 
 @LastModifiedBy nvarchar(256), @DateLastModified datetime, @OwnedBy nvarchar(256),
 @OwnerRoleName nvarchar(256), @IsPrivate bit, @Identity int OUT)
AS
/* Insert a new album. */
INSERT [gs_Album] (AlbumParentId, FKGalleryId, Title, DirectoryName, 
 Summary, ThumbnailMediaObjectId, Seq, DateStart, DateEnd, 
 CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, 
 OwnerRoleName, IsPrivate)
VALUES (@AlbumParentId, @GalleryId, @Title, @DirectoryName, 
 @Summary, @ThumbnailMediaObjectId, @Seq, @DateStart, @DateEnd, 
 @CreatedBy, @DateAdded, @LastModifiedBy, @DateLastModified, @OwnedBy, 
 @OwnerRoleName, @IsPrivate)

SET @Identity = SCOPE_IDENTITY()'
GO

/****** Object:  StoredProcedure [dbo].[gs_AlbumSelect] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_AlbumSelect]
(
	@AlbumId int
)
AS
SET NOCOUNT ON

SELECT
	AlbumId, FKGalleryId as GalleryId, AlbumParentId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, 
	Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
FROM [gs_Album]
WHERE AlbumId = @AlbumId

RETURN'
GO

/****** Object:  StoredProcedure [dbo].[gs_AlbumUpdate] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_AlbumUpdate]
(@AlbumId int, @AlbumParentId int, @Title nvarchar(200), @DirectoryName nvarchar(255),
 @Summary nvarchar(1500), @ThumbnailMediaObjectId int,  @Seq int, 
 @DateStart datetime, @DateEnd datetime, @LastModifiedBy nvarchar(256), 
 @DateLastModified datetime, @OwnedBy nvarchar(256), @OwnerRoleName nvarchar(256), @IsPrivate bit)
 
AS
SET NOCOUNT ON

UPDATE [gs_Album]
SET
	AlbumParentId = @AlbumParentId,
	Title = @Title,
	DirectoryName = @DirectoryName,
	Summary = @Summary,
	ThumbnailMediaObjectId = @ThumbnailMediaObjectId,
	Seq = @Seq,
	DateStart = @DateStart,
	DateEnd = @DateEnd,
	LastModifiedBy = @LastModifiedBy,
	DateLastModified = @DateLastModified,
	OwnedBy = @OwnedBy,
	OwnerRoleName = @OwnerRoleName,
	IsPrivate = @IsPrivate
WHERE (AlbumId = @AlbumId)

RETURN'
GO

/****** Object:  StoredProcedure [dbo].[gs_SelectRootAlbum] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[gs_SelectRootAlbum]
(
	@GalleryId int
)
AS
SET NOCOUNT ON

/* Return the root album. First, check to make sure it exists.
If not, call the stored proc that will insert a default record. */

IF NOT EXISTS (SELECT * FROM [gs_Album] WHERE AlbumParentId = 0 AND FKGalleryId = @GalleryId)
	EXEC dbo.gs_VerifyMinimumRecords @GalleryId

SELECT
	AlbumId, AlbumParentId, Title, DirectoryName, Summary, ThumbnailMediaObjectId, 
	Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, OwnerRoleName, IsPrivate
FROM [gs_Album]
WHERE AlbumParentId = 0 AND FKGalleryId = @GalleryId'
GO

/* Step 8. Delete the stored procedure gs_InitializeGalleryServerPro (not needed). */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_InitializeGalleryServerPro]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_InitializeGalleryServerPro]
GO

/* Step 9: Update the user-defined function gs_GetVersion to reflect new database schema version. */
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_GetVersion]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[gs_GetVersion]
GO
CREATE FUNCTION [dbo].[gs_GetVersion]()
RETURNS
   VARCHAR(255)
AS
BEGIN
   RETURN '2.3.3421'
END
GO
GRANT EXECUTE ON [dbo].[gs_GetVersion] TO [gs_GalleryServerProRole]
GO
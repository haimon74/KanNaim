/*
Run this script on the database containing version 2.0.2886 of Gallery Server Pro.
This script provides the following changes:
1. Add index to table gs_MediaObject.
2. Add one column to table gs_Gallery.
3. Add two columns to table gs_MediaObject.
4. Modify the stored proc gs_VerifyMinimumRecords to reflect the new 'DateAdded' column in gs_Gallery.
5. Modify the following stored procs to reflect new columns in gs_MediaObject:
   * gs_MediaObjectSelect
   * gs_MediaObjectInsert
   * gs_MediaObjectUpdate
   * gs_SelectChildMediaObjects
   * gs_InitializeGalleryServerPro
6. Modify the stored proc gs_Role_AlbumSelectAllAlbumsByRoleName to implement performance improvement.
7. Create stored procs gs_GallerySelect, gs_ExportMembership, gs_ExportGalleryData, and gs_DeleteData.
8. Updated the user-defined function gs_GetVersion to reflect new database schema version.
*/

-- ****************************************************
PRINT N'Creating index [IDX_gs_Album_AlbumParentID]'
GO
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'IDX_gs_Album_AlbumParentID')
	CREATE NONCLUSTERED INDEX [IDX_gs_Album_AlbumParentID] ON [dbo].[gs_Album] ([AlbumParentID] ASC	)
GO

-- ****************************************************
PRINT N'Altering [dbo].[gs_Gallery]'
GO
-- Add column DateAdded to gs_Gallery with default value equal to today's date
IF NOT EXISTS(SELECT * FROM dbo.syscolumns
WHERE [name]='DateAdded' AND id=OBJECT_ID(N'[dbo].[gs_Gallery]'))
BEGIN
	ALTER TABLE dbo.gs_Gallery ADD
		DateAdded datetime NOT NULL CONSTRAINT DF_gs_Gallery_DateAdded DEFAULT GETDATE()
	ALTER TABLE dbo.gs_Gallery
		DROP CONSTRAINT DF_gs_Gallery_DateAdded
END

-- ****************************************************
PRINT N'Altering [dbo].[gs_MediaObject]'
GO
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT FK_gs_MediaObject_gs_Album
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_Title
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_HashKey
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_ThumbnailFilename
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_ThumbnailWidth
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_ThumbnailHeight
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_OptimizedFilename
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_OptimizedWidth
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_OptimizedHeight
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_OriginalFilename
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_OriginalWidth
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_OriginalHeight
GO
ALTER TABLE dbo.gs_MediaObject
	DROP CONSTRAINT DF_gs_MediaObject_IsPrivate
GO
CREATE TABLE dbo.Tmp_gs_MediaObject
	(
	MediaObjectID int NOT NULL IDENTITY (1, 1),
	FKAlbumID int NOT NULL,
	Title nvarchar(1000) NOT NULL,
	HashKey nchar(47) NOT NULL,
	ThumbnailFilename nvarchar(255) NOT NULL,
	ThumbnailWidth int NOT NULL,
	ThumbnailHeight int NOT NULL,
	ThumbnailSizeKB int NOT NULL,
	OptimizedFilename nvarchar(255) NOT NULL,
	OptimizedWidth int NOT NULL,
	OptimizedHeight int NOT NULL,
	OptimizedSizeKB int NOT NULL,
	OriginalFilename nvarchar(255) NOT NULL,
	OriginalWidth int NOT NULL,
	OriginalHeight int NOT NULL,
	OriginalSizeKB int NOT NULL,
	ExternalHtmlSource nvarchar(4000) NOT NULL,
	ExternalType nvarchar(15) NOT NULL,
	Seq int NOT NULL,
	CreatedBy nvarchar(256) NOT NULL,
	DateAdded datetime NOT NULL,
	LastModifiedBy nvarchar(256) NOT NULL,
	DateLastModified datetime NOT NULL,
	IsPrivate bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_Title DEFAULT ('') FOR Title
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_HashKey DEFAULT ('') FOR HashKey
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_ThumbnailFilename DEFAULT ('') FOR ThumbnailFilename
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_ThumbnailWidth DEFAULT ((0)) FOR ThumbnailWidth
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_ThumbnailHeight DEFAULT ((0)) FOR ThumbnailHeight
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_OptimizedFilename DEFAULT ('') FOR OptimizedFilename
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_OptimizedWidth DEFAULT ((0)) FOR OptimizedWidth
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_OptimizedHeight DEFAULT ((0)) FOR OptimizedHeight
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_OriginalFilename DEFAULT ('') FOR OriginalFilename
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_OriginalWidth DEFAULT ((0)) FOR OriginalWidth
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_OriginalHeight DEFAULT ((0)) FOR OriginalHeight
GO
ALTER TABLE dbo.Tmp_gs_MediaObject ADD CONSTRAINT
	DF_gs_MediaObject_IsPrivate DEFAULT ((0)) FOR IsPrivate
GO
SET IDENTITY_INSERT dbo.Tmp_gs_MediaObject ON
GO
IF EXISTS(SELECT * FROM dbo.gs_MediaObject)
	 EXEC('INSERT INTO dbo.Tmp_gs_MediaObject (MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate)
		SELECT MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, '''', ''NotSet'', Seq, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate FROM dbo.gs_MediaObject WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_gs_MediaObject OFF
GO
ALTER TABLE dbo.gs_MediaObjectMetadata
	DROP CONSTRAINT FK_gs_MediaObjectMetadata_gs_MediaObject
GO
DROP TABLE dbo.gs_MediaObject
GO
EXECUTE sp_rename N'dbo.Tmp_gs_MediaObject', N'gs_MediaObject', 'OBJECT' 
GO
ALTER TABLE dbo.gs_MediaObject ADD CONSTRAINT
	PK_gs_MediaObject PRIMARY KEY CLUSTERED 
	(
	MediaObjectID
	)

GO
CREATE NONCLUSTERED INDEX IDX_gs_MediaObject_FKAlbumID ON dbo.gs_MediaObject
	(
	FKAlbumID
	)
GO
ALTER TABLE dbo.gs_MediaObject ADD CONSTRAINT
	FK_gs_MediaObject_gs_Album FOREIGN KEY
	(
	FKAlbumID
	) REFERENCES dbo.gs_Album
	(
	AlbumID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.gs_MediaObjectMetadata ADD CONSTRAINT
	FK_gs_MediaObjectMetadata_gs_MediaObject FOREIGN KEY
	(
	FKMediaObjectId
	) REFERENCES dbo.gs_MediaObject
	(
	MediaObjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_VerifyMinimumRecords]'
GO
ALTER PROCEDURE [dbo].[gs_VerifyMinimumRecords]
(
	@GalleryID int
)
AS
SET NOCOUNT, XACT_ABORT ON
/* Verify that certain tables have the required records, inserting them if necessary. This proc is designed 
to be run each time the application starts. This proc is especially important the first time Gallery Server 
is run as Gallery Server depends on this proc to install the default set of data.

Gallery: This table requires at least one record to represent the current gallery. If no records are
			found, one is inserted with a ID equal to the @GalleryID parameter.
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
IF NOT EXISTS (SELECT * FROM [gs_Gallery] WITH (UPDLOCK, HOLDLOCK) WHERE GalleryID = @GalleryID)
BEGIN
	INSERT gs_Gallery (GalleryId, Description, DateAdded)
	VALUES (@GalleryId, 'My Gallery', GETDATE())
END
COMMIT

/* ******************************************************* */
--          Check the Album table
/* ******************************************************* */
BEGIN TRAN
IF NOT EXISTS (SELECT * FROM [gs_Album] WITH (UPDLOCK, HOLDLOCK) WHERE AlbumParentID = 0 AND FKGalleryID = @GalleryID)
BEGIN
  /* The album table does not have a root album for the specified gallery. Insert one. */
  INSERT [gs_Album] (AlbumParentID, FKGalleryId, Title, DirectoryName, Summary, ThumbnailMediaObjectID, Seq, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, IsPrivate)
  VALUES (0, @GalleryID, 'All albums', '','Welcome to Gallery Server Pro!', 0, 0, 'gs_VerifyMinimumRecords', GETDATE(), 'gs_VerifyMinimumRecords', GETDATE(), 'gs_VerifyMinimumRecords', 0)
END 
COMMIT

/* ******************************************************* */
--          Check the Synchronize table
/* ******************************************************* */
BEGIN TRAN
IF EXISTS (SELECT * FROM [gs_Synchronize] WITH (UPDLOCK, HOLDLOCK) WHERE FKGalleryID = @GalleryID)
BEGIN -- Reset record to clear out any previous synchronization
	UPDATE gs_Synchronize
	SET SynchID = '',
	SynchState = 0,
	TotalFiles = 0,
	CurrentFileIndex = 0
	WHERE FKGalleryID = @GalleryId
END
ELSE
BEGIN
	INSERT INTO gs_Synchronize (SynchID, FKGalleryID, SynchState, TotalFiles, CurrentFileIndex)
	VALUES ('',@GalleryId, 0, 0, 0)
END
COMMIT

RETURN
GO

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_MediaObjectSelect]'
GO
ALTER PROCEDURE [dbo].[gs_MediaObjectSelect]
(
	@MediaObjectId int, @GalleryId int
)
AS
SET NOCOUNT ON

SELECT
	mo.MediaObjectID, mo.FKAlbumID, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
	mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
	mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.ExternalHtmlSource, mo.ExternalType, mo.Seq, 
	mo.CreatedBy, mo.DateAdded, mo.LastModifiedBy, mo.DateLastModified, mo.IsPrivate
FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
WHERE mo.MediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID

RETURN
GO

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_MediaObjectUpdate]'
GO
ALTER PROCEDURE [dbo].[gs_MediaObjectUpdate]
(
 @MediaObjectId int, @HashKey char(47), @FKAlbumID int, 
 @ThumbnailFilename nvarchar(255), @ThumbnailWidth int, @ThumbnailHeight int, @ThumbnailSizeKB int,
 @OriginalFilename nvarchar(255),	@OriginalWidth int, @OriginalHeight int, @OriginalSizeKB int, 
 @OptimizedFilename nvarchar(255),	@OptimizedWidth int, @OptimizedHeight int, @OptimizedSizeKB int, 
 @ExternalHtmlSource nvarchar(1000), @ExternalType nvarchar(15),
 @Title nvarchar(1000), @Seq int, @LastModifiedBy nvarchar(256), @DateLastModified datetime, @IsPrivate bit
)
AS
SET NOCOUNT ON

/* Update the media object. */
UPDATE [gs_MediaObject]
SET HashKey = @HashKey, FKAlbumID = @FKAlbumID,
 ThumbnailFilename = @ThumbnailFilename, ThumbnailWidth = @ThumbnailWidth, 
 ThumbnailHeight = @ThumbnailHeight, ThumbnailSizeKB = @ThumbnailSizeKB,
 OptimizedFilename = @OptimizedFilename, OptimizedWidth = @OptimizedWidth,
 OptimizedHeight = @OptimizedHeight, OptimizedSizeKB = @OptimizedSizeKB, 
 OriginalFilename = @OriginalFilename, OriginalWidth = @OriginalWidth,
 OriginalHeight = @OriginalHeight, OriginalSizeKB = @OriginalSizeKB, 
 ExternalHtmlSource = @ExternalHtmlSource, ExternalType = @ExternalType,
 Title = @Title, Seq = @Seq, LastModifiedBy = @LastModifiedBy, 
 DateLastModified = @DateLastModified, IsPrivate = @IsPrivate
WHERE MediaObjectID = @MediaObjectId
GO

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_MediaObjectInsert]'
GO
ALTER PROCEDURE [dbo].[gs_MediaObjectInsert]
(@HashKey char(47), @FKAlbumID int, @ThumbnailFilename nvarchar(255), 
 @ThumbnailWidth int, @ThumbnailHeight int,
 @ThumbnailSizeKB int, @OptimizedFilename nvarchar(255), 
 @OptimizedWidth int, @OptimizedHeight int,
 @OptimizedSizeKB int, @OriginalFilename nvarchar(255),	 
 @OriginalWidth int, @OriginalHeight int, @OriginalSizeKB int, 
 @ExternalHtmlSource nvarchar(1000), @ExternalType nvarchar(15),
 @Title nvarchar(1000), @Seq int, @CreatedBy nvarchar(256), @DateAdded datetime, 
 @LastModifiedBy nvarchar(256), @DateLastModified datetime, @IsPrivate bit,
 @Identity int OUT)
AS

/* Insert media object information. */
 INSERT [gs_MediaObject] (HashKey, FKAlbumID, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
 ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
 OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Title, Seq, CreatedBy, 
 DateAdded, LastModifiedBy, DateLastModified, IsPrivate)
VALUES (@HashKey, @FKAlbumID, @ThumbnailFilename, @ThumbnailWidth, @ThumbnailHeight,
 @ThumbnailSizeKB, @OptimizedFilename, @OptimizedWidth, @OptimizedHeight, @OptimizedSizeKB,
 @OriginalFilename, @OriginalWidth, @OriginalHeight, @OriginalSizeKB, @ExternalHtmlSource, @ExternalType, @Title, @Seq, @CreatedBy, 
 @DateAdded, @LastModifiedBy, @DateLastModified, @IsPrivate)
 
SET @Identity = SCOPE_IDENTITY()
GO

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_SelectChildMediaObjects]'
GO
ALTER PROCEDURE [dbo].[gs_SelectChildMediaObjects]
(
	@AlbumId int
)
AS
SET NOCOUNT ON

SELECT 
	MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, 
	CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM [gs_MediaObject]
WHERE FKAlbumID = @AlbumId

RETURN
GO

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_InitializeGalleryServerPro]'
GO
ALTER PROCEDURE [gs_InitializeGalleryServerPro]
(
	@GalleryID int,
	@ApplicationName nvarchar(512),
	@SysAdminRoleName nvarchar(256),
	@SysAdminRoleNameDescription nvarchar(256),
	@AdminEmail nvarchar(256),
	@AdminUserName nvarchar(256),
	@AdminPassword nvarchar(256),
	@PasswordFormat int = 0,	-- 0 = clear text, 1 = encrypted
	@CreateSamples bit = 1
)
AS
SET NOCOUNT ON

/* This procedure is called from the web installer. It sets up the administrator user and ensures 
certain tables have default records. */

/* ******************************************************* */
--     Verify certain tables have default records
/* ******************************************************* */
EXEC dbo.gs_VerifyMinimumRecords @GalleryID

/* ******************************************************* */
--                  Membership Routines
/* ******************************************************* */
DECLARE @ApplicationID uniqueidentifier
DECLARE @ApplicationAlreadyExists bit
DECLARE @RootAlbumId int
DECLARE @AdminUserId uniqueidentifier

IF (NOT EXISTS (SELECT name FROM sysobjects WHERE (name = N'aspnet_Applications') AND (type = 'U')))
	RETURN -- No Membership tables in this database. User presumably wants to connect to a Membership infrastructure in 
				 -- a remote database. This must be done manually so let's just exit.
	
-- Get root album ID
SELECT @RootAlbumId = AlbumID FROM gs_Album WHERE (AlbumParentID = 0)

--Get or Create the Application ID.
--An ApplicationName/ID can exist in more than one site, so it does not need to be unique
Set @ApplicationID = null

Select @ApplicationID = ApplicationId FROM aspnet_Applications where  LoweredApplicationName = Lower(@ApplicationName)


IF(@ApplicationID is  null)
BEGIN /* Application doesn't exist so let's create it */
	EXEC aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId = @ApplicationID OUTPUT
	Set @ApplicationAlreadyExists = 0
END
ELSE
	Set @ApplicationAlreadyExists = 1

/* ******************************************************* */
/* If Gallery Server Pro app doesn't exist, create sys admin 
	 role and admin user; otherwise create the role and user
	 only if they don't already exist. */
/* ******************************************************* */
IF(@ApplicationAlreadyExists = 0)
BEGIN
	/* ******************************************************* */
	--			Gallery Server Pro app exists
	--     Create system administrator role
	/* ******************************************************* */
	INSERT INTO [aspnet_Roles] ([RoleId], [ApplicationId], [RoleName], [LoweredRoleName], [Description])
	VALUES (newID(), @ApplicationID, @SysAdminRoleName, Lower(@SysAdminRoleName), @SysAdminRoleNameDescription)	

	/* ******************************************************* */
	--                     Create admin user
	/* ******************************************************* */
	Set @AdminUserId = newid()

	INSERT INTO [aspnet_Users] ([UserId], [ApplicationId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) 
		VALUES (@AdminUserId, @ApplicationID, @AdminUserName, Lower(@AdminUserName), NULL, 0, getdate())

	INSERT INTO [aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [Comment], FailedPasswordAnswerAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAttemptCount, LastLockoutDate, IsLockedOut) 
		VALUES (@ApplicationID, @AdminUserId,  @AdminPassword, @PasswordFormat,  N'DVZTktxeMzDtXR7eik7Cdw==', NULL, @AdminEmail,    NULL,            NULL,               NULL,            1,            getdate(),    getdate(),       getdate(),                 NULL,                 '1753-01-01',                              0,                               '1753-01-01',                       0,                          '1753-01-01',      0)

	/* ******************************************************* */
	-- Add new admin user to all roles for this application, which 
	-- will probably just be the system admin role we created.
	/* ******************************************************* */
	INSERT INTO aspnet_UsersInRoles (UserId, RoleId) 
	SELECT @AdminUserId, RoleId FROM aspnet_Roles WHERE ApplicationId = @ApplicationID

	/* ******************************************************* */
	-- Create gallery server role that applies to root album.
	/* ******************************************************* */
	INSERT INTO [gs_Role] (FKGalleryId, RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
		AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, AllowDeleteMediaObject, 
		AllowSynchronize, HideWatermark, AllowAdministerSite)
	VALUES (@GalleryID, @SysAdminRoleName, 1, 1, 1,	1, 1, 1, 1, 1, 1, 0, 1)
	
	INSERT INTO [gs_Role_Album] (FKRoleName, FKAlbumId)
	VALUES (@SysAdminRoleName, @RootAlbumId)
END
ELSE -- Gallery Server Pro app already exists (@ApplicationAlreadyExists = 1)
BEGIN
	-- Does admin user exist for this application? Create it if not. 
	Set @AdminUserId = NULL
	SELECT @AdminUserId = UserId FROM aspnet_Users WHERE (UserName = @AdminUserName) AND (ApplicationId = @ApplicationID)
	
	IF @AdminUserId = NULL
	BEGIN -- Admin user does not exist. Create it.
		Set @AdminUserId = newid()

		INSERT INTO [aspnet_Users] ([UserId], [ApplicationId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) 
			VALUES (@AdminUserId, @ApplicationID, @AdminUserName, Lower(@AdminUserName), NULL, 0, getdate())

		INSERT INTO [aspnet_Membership] ([ApplicationId], [UserId], [Password],     [PasswordFormat], [PasswordSalt],              [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [Comment], FailedPasswordAnswerAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAttemptCount, LastLockoutDate, IsLockedOut) 
			VALUES (@ApplicationID, @AdminUserId,  @AdminPassword, @PasswordFormat,   N'DVZTktxeMzDtXR7eik7Cdw==', NULL,       @AdminEmail,    NULL,            NULL,               NULL,            1,            getdate(),    getdate(),       getdate(),                 NULL,                 '1753-01-01',                              0,                               '1753-01-01',                       0,                          '1753-01-01',      0)
	END
	ELSE
	BEGIN -- Admin user exists. Make sure password matches the one passed to this procedure
		UPDATE aspnet_Membership
		SET Password = @AdminPassword, PasswordFormat = @PasswordFormat
		WHERE UserId = @AdminUserId
	END

	-- Does sys admin role exist for this application? Create it if not.
	IF NOT EXISTS(SELECT * FROM [aspnet_Roles] WHERE RoleName = @SysAdminRoleName AND ApplicationId = @ApplicationID)
		INSERT INTO [aspnet_Roles] ([RoleId], [ApplicationId], [RoleName], [LoweredRoleName], [Description])
			VALUES (newID(), @ApplicationID, @SysAdminRoleName, Lower(@SysAdminRoleName), @SysAdminRoleNameDescription)	
		 
	-- Make sure admin user is part of sys admin role.
	IF NOT EXISTS(SELECT aspnet_UsersInRoles.*
								FROM aspnet_UsersInRoles INNER JOIN aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId
								WHERE aspnet_UsersInRoles.UserId = @AdminUserId AND aspnet_Roles.RoleName = @SysAdminRoleName AND aspnet_Roles.ApplicationId = @ApplicationID)
		INSERT INTO aspnet_UsersInRoles (UserId, RoleId) 
		SELECT @AdminUserId, RoleId FROM aspnet_Roles WHERE RoleName = @SysAdminRoleName AND ApplicationId = @ApplicationID

	-- Make sure sys admin role is a gallery server role assigned to the root album.
	IF NOT EXISTS(SELECT * FROM [gs_Role] WHERE RoleName = @SysAdminRoleName AND FKGalleryId = @GalleryID)
		INSERT INTO [gs_Role] (FKGalleryId, RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
			AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, AllowDeleteMediaObject, 
			AllowSynchronize, HideWatermark, AllowAdministerSite)
		VALUES (@GalleryID, @SysAdminRoleName, 1, 1, 1,	1, 1, 1, 1, 1, 1, 0, 1)
	
	IF NOT EXISTS(SELECT * FROM [gs_Role_Album] WHERE FKRoleName = @SysAdminRoleName)
		INSERT INTO [gs_Role_Album] (FKRoleName, FKAlbumId)
		VALUES (@SysAdminRoleName, @RootAlbumId)
END	

IF @CreateSamples = 1
BEGIN
	DECLARE @newAlbumId int
	DECLARE @newMediaObjectId int
	
	/* Create an album called Samples. */
	INSERT [gs_Album] (AlbumParentID, FKGalleryID, Title, DirectoryName, Summary, ThumbnailMediaObjectID, Seq, DateStart, DateEnd, CreatedBy, DateAdded, LastModifiedBy, DateLastModified, OwnedBy, IsPrivate)
	VALUES (@RootAlbumId, @GalleryID, 'Samples', 'Samples', 'Welcome to Gallery Server Pro', 0, 1, GETDATE(), DATEADD(week, 2, GETDATE()), @AdminUserName, GETDATE(), @AdminUserName, GETDATE(), @AdminUserName, 0)

	SET @newAlbumId = SCOPE_IDENTITY()
	
	INSERT [gs_MediaObject] (HashKey, FKAlbumID, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
	 ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
	 OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, Title, Seq, CreatedBy, 
	 DateAdded, LastModifiedBy, DateLastModified, IsPrivate)
	VALUES ('7B-F7-3D-BA-F1-CC-26-62-8C-0C-CD-1D-53-09-CF-59', @newAlbumId, 'zThumb_RogerMartin&Family.jpeg', 114, 86,
	 2, 'RogerMartin&Family.jpeg', 639, 479, 60, 'RogerMartin&Family.jpeg', 639, 479, 60, 'Roger, Margaret and Skyler Martin (August 2008)', 1, @AdminUserName,
	 GETDATE(), @AdminUserName, GETDATE(), 0)
 
	SET @newMediaObjectId = SCOPE_IDENTITY()
	
	/* Set this new image as the thumbnail for the album */
	UPDATE [gs_Album] SET ThumbnailMediaObjectID = @newMediaObjectId WHERE AlbumID = @newAlbumId
END
RETURN
GO

-- ****************************************************
PRINT N'Modifying stored procedure[dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]'
GO
ALTER PROCEDURE [dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]
(	@RoleName nvarchar(256), @GalleryId int )

AS
SET NOCOUNT ON

/* Retrieve all the album IDs that are affected by the specified role name. The album IDs that are stored in
   the gs_Role_Album table only hold the highest ranking album ID. */
   
/* If the role is applied to the root album, then we can just return all albums in the gallery. This is 
much cheaper than drilling down by album.  */
DECLARE @RootAlbumId int
SELECT @RootAlbumId = AlbumID FROM gs_Album WHERE AlbumParentID = 0 AND FKGalleryID = @GalleryId
 
IF EXISTS (SELECT * FROM gs_Role_Album WHERE FKRoleName = @RoleName AND FKAlbumId = @RootAlbumId)
BEGIN
	SELECT AlbumID FROM gs_Album WHERE FKGalleryID = @GalleryId
END
ELSE
BEGIN
	/* The role applies to an album or albums below the root album, so we need to drill down and retrieve all
   the children. Start by creating a temporary table to hold our data. */
	DECLARE @AlbumList table
		(AlbumID int not null,
		AlbumParentID int not null,
		AlbumDepth int not null)

	/* Insert the top level album IDs. */
	INSERT @AlbumList (AlbumID, AlbumParentID, AlbumDepth)
	SELECT FKAlbumID, 0, 1
	FROM gs_Role_Album INNER JOIN gs_Album ON gs_Role_Album.FKAlbumId = gs_Album.AlbumID
	WHERE (gs_Role_Album.FKRoleName = @RoleName) AND (gs_Album.FKGalleryID = @GalleryId)

	/* Continue drilling down, level by level, until we reach a level where there are no more child albums. */
	WHILE (@@rowcount > 0) BEGIN
		INSERT @AlbumList (AlbumId, AlbumParentId, AlbumDepth)
		SELECT a.AlbumId, a.AlbumParentId, al.AlbumDepth + 1
		FROM gs_Album a JOIN @AlbumList al ON a.AlbumParentId = al.AlbumId
		WHERE al.AlbumDepth = (SELECT MAX(AlbumDepth) FROM @AlbumList)
	END

	/* Retrieve the list of album IDs. */
	SELECT AlbumId
	FROM @AlbumList
END

RETURN
GO

-- ****************************************************
PRINT N'Creating stored procedure [dbo].[gs_GallerySelect]'
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_GallerySelect]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_GallerySelect]
GO
CREATE PROCEDURE [dbo].[gs_GallerySelect]
(
	@GalleryId int
)
AS
SET NOCOUNT ON

SELECT GalleryId, Description, DateAdded
FROM [gs_Gallery]
WHERE GalleryId = @GalleryId

RETURN
GO

-- ****************************************************
PRINT N'Creating stored procedure [dbo].[gs_ExportMembership]'
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_ExportMembership]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_ExportMembership]
GO
CREATE PROCEDURE [dbo].[gs_ExportMembership]
AS
SET NOCOUNT ON

SELECT ApplicationName, LoweredApplicationName, ApplicationId, Description
FROM aspnet_Applications

SELECT ApplicationId, UserId, Password, PasswordFormat, PasswordSalt, MobilePIN, Email, LoweredEmail, PasswordQuestion, PasswordAnswer, IsApproved, 
 IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, 
 FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, Comment
FROM aspnet_Membership

SELECT UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate
FROM aspnet_Profile

SELECT ApplicationId, RoleId, RoleName, LoweredRoleName, Description
FROM aspnet_Roles

SELECT ApplicationId, UserId, UserName, LoweredUserName, MobileAlias, IsAnonymous, LastActivityDate
FROM aspnet_Users

SELECT UserId, RoleId
FROM aspnet_UsersInRoles

RETURN
GO

-- ****************************************************
PRINT N'Creating stored procedure [dbo].[gs_ExportGalleryData]'
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_ExportGalleryData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_ExportGalleryData]
GO
CREATE PROCEDURE [dbo].[gs_ExportGalleryData]
AS
SET NOCOUNT ON

SELECT AlbumID, FKGalleryID, AlbumParentID, Title, DirectoryName, Summary, ThumbnailMediaObjectID, Seq, DateStart, DateEnd, DateAdded, CreatedBy, 
 LastModifiedBy, DateLastModified, OwnedBy, IsPrivate
FROM gs_Album

SELECT GalleryID, Description, DateAdded
FROM gs_Gallery

SELECT MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, 
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

SELECT [dbo].[gs_GetVersion]() AS SchemaVersion

RETURN
GO

-- ****************************************************
PRINT N'Creating stored procedure [dbo].[gs_DeleteData]'
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_DeleteData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[gs_DeleteData]
GO
CREATE PROCEDURE [dbo].[gs_DeleteData]
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
END

RETURN
GO

-- ****************************************************
PRINT N'Modifying user defined function [dbo].[gs_GetVersion]'
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[gs_GetVersion]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[gs_GetVersion]
GO
CREATE FUNCTION [dbo].[gs_GetVersion]()
RETURNS
   VARCHAR(255)
AS
BEGIN
   RETURN '2.1.3162'
END
GO

-- ****************************************************
PRINT N'Giving execute permission for the new stored procs to the [gs_GalleryServerProRole] role'
GRANT EXECUTE ON [dbo].[gs_GallerySelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_ExportMembership] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_ExportGalleryData] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_DeleteData] TO [gs_GalleryServerProRole]
GO

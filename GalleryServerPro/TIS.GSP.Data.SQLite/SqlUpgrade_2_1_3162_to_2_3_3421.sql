/*
SQLITE: This script updates the database schema from 2.1.3162 to 2.3.3421.
This script provides the following changes:

1. Add column OwnerRoleName to table gs_Album. Clear the contents of gs_Album.OwnedBy.
2. Add column LoweredRoleName to table aspnet_Roles.
3. Add table gs_AppError.
4. Create indexes for better performance.

*/

/* Step 1: Add column OwnerRoleName to table gs_Album. This is done by renaming the original table, creating the new
one with the new column, copying data from the old table to the new one, and deleting the original table. Any 
required indexes are dropped and recreated as part of the process. */
DROP INDEX IF EXISTS [idxAlbum_AlbumParentId];

ALTER TABLE [gs_Album] RENAME TO [gs_Album_old];

CREATE TABLE [gs_Album] (
 [AlbumId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
 [FKGalleryId] INTEGER NOT NULL,
 [AlbumParentId] INTEGER NOT NULL,
 [Title] NVARCHAR (200) NOT NULL,
 [DirectoryName] NVARCHAR (255) NOT NULL,
 [Summary] NVARCHAR (1500) NOT NULL,
 [ThumbnailMediaObjectId] INTEGER NOT NULL,
 [Seq] INTEGER NOT NULL,
 [DateStart] TIMESTAMP NULL,
 [DateEnd] TIMESTAMP NULL,
 [DateAdded] TIMESTAMP NOT NULL,
 [CreatedBy] NVARCHAR (256) NOT NULL,
 [LastModifiedBy] NVARCHAR (256) NOT NULL,
 [DateLastModified] TIMESTAMP NOT NULL,
 [OwnedBy] NVARCHAR (256) NOT NULL,
 [OwnerRoleName] NVARCHAR (256) NOT NULL,
 [IsPrivate] BOOL NOT NULL
);

INSERT INTO [gs_Album] (AlbumId,FKGalleryId,AlbumParentId,Title,DirectoryName,Summary,ThumbnailMediaObjectId,Seq,DateAdded,CreatedBy,LastModifiedBy,DateLastModified,OwnedBy,OwnerRoleName,IsPrivate)
SELECT AlbumId,FKGalleryId,AlbumParentId,Title,DirectoryName,Summary,ThumbnailMediaObjectId,Seq,DateAdded,CreatedBy,LastModifiedBy,DateLastModified,'','',IsPrivate
FROM [gs_Album_old];

CREATE INDEX idxAlbum_AlbumParentId ON [gs_Album] ( 'AlbumParentId' );

DROP TABLE IF EXISTS [gs_Album_old];
GO

/* Step 2: Add column LoweredRoleName to table aspnet_Roles. This follows the same process as adding the column to gs_Album above. */
DROP INDEX IF EXISTS [idxRoles];

ALTER TABLE [aspnet_Roles] RENAME TO [aspnet_Roles_old];

CREATE TABLE [aspnet_Roles]
(
  [RoleId] NVARCHAR (36) PRIMARY KEY NOT NULL,
  [RoleName] NVARCHAR (256) NOT NULL,
  [LoweredRoleName] NVARCHAR (256) NOT NULL,
  [ApplicationId] NVARCHAR (36) NOT NULL
);

INSERT INTO [aspnet_Roles] (RoleId, RoleName, LoweredRoleName, ApplicationId)
SELECT RoleId, RoleName, lower(RoleName), ApplicationId
FROM [aspnet_Roles_old];

CREATE UNIQUE INDEX idxRoles ON [aspnet_Roles] ( 'LoweredRoleName' , 'ApplicationId' );

DROP TABLE IF EXISTS [aspnet_Roles_old];
GO

/* Step 3: Add table gs_AppError. */
DROP INDEX IF EXISTS idxAppError_GalleryId;
DROP TABLE IF EXISTS [gs_AppError];
GO

CREATE TABLE [gs_AppError] (
[AppErrorId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[FKGalleryId] INTEGER NOT NULL,
[TimeStamp] TIMESTAMP NOT NULL,
[ExceptionType] NVARCHAR (1000) NOT NULL,
[Message] NVARCHAR (8000) NOT NULL,
[Source] NVARCHAR (1000) NOT NULL,
[TargetSite] NVARCHAR NOT NULL,
[StackTrace] NVARCHAR NOT NULL,
[ExceptionData] NVARCHAR NOT NULL,
[InnerExType] NVARCHAR (1000) NOT NULL,
[InnerExMessage] NVARCHAR (8000) NOT NULL,
[InnerExSource] NVARCHAR (1000) NOT NULL,
[InnerExTargetSite] NVARCHAR NOT NULL,
[InnerExStackTrace] NVARCHAR NOT NULL,
[InnerExData] NVARCHAR NOT NULL,
[Url] NVARCHAR (1000) NOT NULL,
[FormVariables] NVARCHAR NOT NULL,
[Cookies] NVARCHAR NOT NULL,
[SessionVariables] NVARCHAR NOT NULL,
[ServerVariables] NVARCHAR NOT NULL
);


CREATE INDEX idxAppError_GalleryId ON [gs_AppError] ( 'FKGalleryId' );
GO

/* Step 4: Create indexes for better performance. Delete first if they exist. */
DROP INDEX IF EXISTS [idxAlbum_AlbumId_FKGalleryId];
DROP INDEX IF EXISTS [idxAlbum_AlbumParentId_FKGalleryId];
DROP INDEX IF EXISTS [idxMediaObject];

CREATE INDEX [idxAlbum_AlbumId_FKGalleryId] ON [gs_Album] ( 'AlbumId', 'FKGalleryId' );
CREATE INDEX [idxAlbum_AlbumParentId_FKGalleryId] ON [gs_Album] ('AlbumParentId', 'FKGalleryId');
CREATE INDEX [idxMediaObject] ON [gs_MediaObject] ( 'FKAlbumId' );
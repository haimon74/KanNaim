CREATE TABLE [aspnet_Applications]
(
	[ApplicationId] NVARCHAR (36) PRIMARY KEY NOT NULL,
	[ApplicationName] NVARCHAR (256) UNIQUE NOT NULL,
	[Description] NVARCHAR (256) NULL
);

CREATE TABLE [aspnet_Roles]
(
  [RoleId] NVARCHAR (36) PRIMARY KEY NOT NULL,
  [RoleName] NVARCHAR (256) NOT NULL,
  [LoweredRoleName] NVARCHAR (256) NOT NULL,
  [ApplicationId] NVARCHAR (36) NOT NULL
);

CREATE TABLE [aspnet_UsersInRoles]
(
  [UserId] NVARCHAR (36) NOT NULL,
  [RoleId] NVARCHAR (36) NOT NULL
);

CREATE TABLE [aspnet_Profile] (
 [UserId] NVARCHAR (36) UNIQUE NOT NULL,
 [LastUpdatedDate] TIMESTAMP NOT NULL,
 [PropertyNames] TEXT (6000) NOT NULL,
 [PropertyValuesString] TEXT (6000) NOT NULL
);

CREATE TABLE [aspnet_Users] (
[UserId] NVARCHAR (36) UNIQUE NOT NULL,
[Username] NVARCHAR (256) NOT NULL,
[LoweredUsername] NVARCHAR (256) NOT NULL,
[ApplicationId] NVARCHAR (36) NOT NULL,
[Email] NVARCHAR (256) NULL,
[LoweredEmail] NVARCHAR (256) NULL,
[Comment] NVARCHAR (3000) NULL,
[Password] NVARCHAR (128) NOT NULL,
[PasswordFormat] NVARCHAR (128) NOT NULL,
[PasswordSalt] NVARCHAR (128) NOT NULL,
[PasswordQuestion] NVARCHAR (256) NULL,
[PasswordAnswer] NVARCHAR (128) NULL,
[IsApproved] BOOL NOT NULL,
[IsAnonymous] BOOL  NOT NULL,
[LastActivityDate] TIMESTAMP  NOT NULL,
[LastLoginDate] TIMESTAMP NOT NULL,
[LastPasswordChangedDate] TIMESTAMP NOT NULL,
[CreateDate] TIMESTAMP  NOT NULL,
[IsLockedOut] BOOL NOT NULL,
[LastLockoutDate] TIMESTAMP NOT NULL,
[FailedPasswordAttemptCount] INTEGER NOT NULL,
[FailedPasswordAttemptWindowStart] TIMESTAMP NOT NULL,
[FailedPasswordAnswerAttemptCount] INTEGER NOT NULL,
[FailedPasswordAnswerAttemptWindowStart] TIMESTAMP NOT NULL
);

CREATE TABLE [gs_Role] (
	[RoleName] NVARCHAR (256) NOT NULL,
	[FKGalleryId] INTEGER NOT NULL,
	[AllowViewAlbumsAndObjects] BOOL NOT NULL,
	[AllowViewOriginalImage] BOOL NOT NULL,
	[AllowAddChildAlbum] BOOL NOT NULL,
	[AllowAddMediaObject] BOOL NOT NULL,
	[AllowEditAlbum] BOOL NOT NULL,
	[AllowEditMediaObject] BOOL NOT NULL,
	[AllowDeleteChildAlbum] BOOL NOT NULL,
	[AllowDeleteMediaObject] BOOL NOT NULL,
	[AllowSynchronize] BOOL NOT NULL,
	[HideWatermark] BOOL NOT NULL,
	[AllowAdministerSite] BOOL NOT NULL
);

CREATE TABLE [gs_Album] (
	[AlbumId] INTEGER PRIMARY KEY IDENTITY(1,1) NOT NULL,
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

CREATE TABLE [gs_MediaObject] (
	[MediaObjectId] INTEGER PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[FKAlbumId] INTEGER NOT NULL,
	[Title] NVARCHAR (1000) NOT NULL,
	[HashKey] [NCHAR](47) NOT NULL,
	[ThumbnailFilename] NVARCHAR (255) NOT NULL,
	[ThumbnailWidth] INTEGER NOT NULL,
	[ThumbnailHeight] INTEGER NOT NULL,
	[ThumbnailSizeKB] INTEGER NOT NULL,
	[OptimizedFilename] NVARCHAR (255) NOT NULL,
	[OptimizedWidth] INTEGER NOT NULL,
	[OptimizedHeight] INTEGER NOT NULL,
	[OptimizedSizeKB] INTEGER NOT NULL,
	[OriginalFilename] NVARCHAR (255) NOT NULL,
	[OriginalWidth] INTEGER NOT NULL,
	[OriginalHeight] INTEGER NOT NULL,
	[OriginalSizeKB] INTEGER NOT NULL,
	[ExternalHtmlSource] NVARCHAR (1000) NOT NULL,
	[ExternalType] NVARCHAR (15) NOT NULL,
	[Seq] INTEGER NOT NULL,
	[CreatedBy] NVARCHAR (256) NOT NULL,
	[DateAdded] TIMESTAMP NOT NULL,
	[LastModifiedBy] NVARCHAR (256) NOT NULL,
	[DateLastModified] TIMESTAMP NOT NULL,
	[IsPrivate] BOOL NOT NULL
);

CREATE TABLE [gs_Role_Album] (
	[FKRoleName] NVARCHAR (256) NOT NULL,
	[FKAlbumId] INTEGER NOT NULL
);

CREATE TABLE [gs_MediaObjectMetadata] (
	[MediaObjectMetadataId] INTEGER PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[FKMediaObjectId] INTEGER NOT NULL,
	[MetadataNameIdentifier] INTEGER NOT NULL,
	[Description] NVARCHAR (100) NOT NULL,
	[Value] NVARCHAR (2000) NOT NULL
);

CREATE TABLE [gs_Gallery] (
	[GalleryId] INTEGER PRIMARY KEY NOT NULL,
	[Description] NVARCHAR (100) NOT NULL,
	[DateAdded] TIMESTAMP NOT NULL
);

CREATE TABLE [gs_Synchronize] (
	[SynchId] NCHAR(50) NOT NULL,
	[FKGalleryId] INTEGER NOT NULL,
	[SynchState] INTEGER NOT NULL,
	[TotalFiles] INTEGER NOT NULL,
	[CurrentFileIndex] INTEGER NOT NULL
);

CREATE TABLE [gs_AppError] (
	[AppErrorId] INTEGER PRIMARY KEY IDENTITY(1,1) NOT NULL,
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

CREATE TABLE [gs_SchemaVersion] (
	[SchemaVersion] NVARCHAR (255) PRIMARY KEY NOT NULL
);

CREATE UNIQUE INDEX idxUsers ON [aspnet_Users] ( 'LoweredUsername' , 'ApplicationId' );

CREATE INDEX idxUsersAppId ON [aspnet_Users] ( 'ApplicationId' );

CREATE INDEX idxAlbum_AlbumParentId ON [gs_Album] ( 'AlbumParentId' );

CREATE INDEX idxAlbum_AlbumId_FKGalleryId ON [gs_Album] ( 'AlbumId', 'FKGalleryId' );

CREATE INDEX idxAlbum_AlbumParentId_FKGalleryId ON [gs_Album] ('AlbumParentId', 'FKGalleryId');

CREATE INDEX idxMediaObject ON [gs_MediaObject] ( 'FKAlbumId' );

CREATE UNIQUE INDEX idxRoles ON [aspnet_Roles] ( 'LoweredRoleName' , 'ApplicationId' );

CREATE UNIQUE INDEX idxRoles2 ON [gs_Role] ( 'RoleName' , 'FKGalleryId' );

CREATE UNIQUE INDEX idxUsersInRoles ON [aspnet_UsersInRoles] ( 'UserId', 'RoleId');

CREATE UNIQUE INDEX idxProfile ON [aspnet_Profile] ( 'UserId' );

CREATE UNIQUE INDEX idxRoleAlbum ON [gs_Role_Album] ( 'FKRoleName' , 'FKAlbumId' );

CREATE UNIQUE INDEX idxSynchronize ON [gs_Synchronize] ( 'SynchId' , 'FKGalleryId' );

CREATE INDEX idxMediaObjectMetadata ON [gs_MediaObjectMetadata] ( 'FKMediaObjectId' );

CREATE INDEX idxAppError_GalleryId ON [gs_AppError] ( 'FKGalleryId' );

CREATE UNIQUE INDEX idxSchemaVersion ON [gs_SchemaVersion] ( 'SchemaVersion' );

INSERT INTO gs_SchemaVersion (SchemaVersion)
VALUES ('2.3.3421');
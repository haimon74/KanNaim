/****** Object:  ForeignKey [FK_gs_Album_gs_Album]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_Album_gs_Album]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
ALTER TABLE [dbo].[gs_Album] DROP CONSTRAINT [FK_gs_Album_gs_Album]
GO
/****** Object:  ForeignKey [FK_gs_Album_gs_Gallery]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_Album_gs_Gallery]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
ALTER TABLE [dbo].[gs_Album] DROP CONSTRAINT [FK_gs_Album_gs_Gallery]
GO
/****** Object:  ForeignKey [FK_gs_MediaObjectMetadata_gs_MediaObject]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_MediaObjectMetadata_gs_MediaObject]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadata]'))
ALTER TABLE [dbo].[gs_MediaObjectMetadata] DROP CONSTRAINT [FK_gs_MediaObjectMetadata_gs_MediaObject]
GO
/****** Object:  ForeignKey [FK_gs_Role_Album_gs_Album]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_Role_Album_gs_Album]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Role_Album]'))
ALTER TABLE [dbo].[gs_Role_Album] DROP CONSTRAINT [FK_gs_Role_Album_gs_Album]
GO
/****** Object:  Default [DF_gs_Album_Title]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] DROP CONSTRAINT [DF_gs_Album_Title]

End
GO
/****** Object:  Default [DF_gs_Album_DirectoryName]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_DirectoryName]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] DROP CONSTRAINT [DF_gs_Album_DirectoryName]

End
GO
/****** Object:  Default [DF_gs_Album_Summary]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_Summary]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] DROP CONSTRAINT [DF_gs_Album_Summary]

End
GO
/****** Object:  Default [DF_gs_Album_ThumbnailMediaObjectID]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_ThumbnailMediaObjectID]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] DROP CONSTRAINT [DF_gs_Album_ThumbnailMediaObjectID]

End
GO
/****** Object:  Default [DF_gs_MediaObject_Title]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_Title]

End
GO
/****** Object:  Default [DF_gs_MediaObject_HashKey]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_HashKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_HashKey]

End
GO
/****** Object:  Default [DF_gs_MediaObject_ThumbnailFilename]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_ThumbnailFilename]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_ThumbnailFilename]

End
GO
/****** Object:  Default [DF_gs_MediaObject_ThumbnailWidth]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_ThumbnailWidth]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_ThumbnailWidth]

End
GO
/****** Object:  Default [DF_gs_MediaObject_ThumbnailHeight]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_ThumbnailHeight]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_ThumbnailHeight]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OptimizedFilename]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OptimizedFilename]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_OptimizedFilename]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OptimizedWidth]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OptimizedWidth]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_OptimizedWidth]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OptimizedHeight]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OptimizedHeight]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_OptimizedHeight]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OriginalFilename]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OriginalFilename]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_OriginalFilename]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OriginalWidth]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OriginalWidth]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_OriginalWidth]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OriginalHeight]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OriginalHeight]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_OriginalHeight]

End
GO
/****** Object:  Default [DF_gs_MediaObject_FilenameHasChanged]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_FilenameHasChanged]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] DROP CONSTRAINT [DF_gs_MediaObject_FilenameHasChanged]

End
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleUpdate]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_RoleUpdate]
GO
/****** Object:  StoredProcedure [dbo].[gs_SelectRootAlbum]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SelectRootAlbum]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_SelectRootAlbum]
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleInsert]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_RoleInsert]
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleSelect]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_RoleSelect]
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumDelete]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_Role_AlbumDelete]
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumInsert]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_Role_AlbumInsert]
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleDelete]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_RoleDelete]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectInsert]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectInsert]
GO
/****** Object:  StoredProcedure [dbo].[gs_SelectChildMediaObjects]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SelectChildMediaObjects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_SelectChildMediaObjects]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectDelete]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectDelete]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectUpdate]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectUpdate]
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectSelectHashKeys]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectSelectHashKeys]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectSelectHashKeys]
GO
/****** Object:  StoredProcedure [dbo].[gs_SelectChildAlbums]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SelectChildAlbums]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_SelectChildAlbums]
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumUpdate]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_AlbumUpdate]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectSelect]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectSelect]
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumSelect]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_AlbumSelect]
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumInsert]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_AlbumInsert]
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName]
GO
/****** Object:  StoredProcedure [dbo].[gs_SynchronizeSelect]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SynchronizeSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_SynchronizeSelect]
GO
/****** Object:  StoredProcedure [dbo].[gs_SynchronizeSave]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SynchronizeSave]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_SynchronizeSave]
GO
/****** Object:  StoredProcedure [dbo].[gs_VerifyMinimumRecords]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_VerifyMinimumRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_VerifyMinimumRecords]
GO
/****** Object:  StoredProcedure [dbo].[gs_InitializeGalleryServerPro]    Script Date: 07/17/2007 11:48:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_InitializeGalleryServerPro]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_InitializeGalleryServerPro]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataSelect]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataSelect]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectMetadataSelect]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataInsert]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectMetadataInsert]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataUpdate]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataUpdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectMetadataUpdate]
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataDelete]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_MediaObjectMetadataDelete]
GO
/****** Object:  Table [dbo].[gs_MediaObjectMetadata]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadata]') AND type in (N'U'))
DROP TABLE [dbo].[gs_MediaObjectMetadata]
GO
/****** Object:  Table [dbo].[gs_Role_Album]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_Album]') AND type in (N'U'))
DROP TABLE [dbo].[gs_Role_Album]
GO
/****** Object:  Table [dbo].[gs_Album]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Album]') AND type in (N'U'))
DROP TABLE [dbo].[gs_Album]
GO
/****** Object:  UserDefinedFunction [dbo].[gs_func_convert_string_array_to_table]    Script Date: 07/17/2007 10:05:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_func_convert_string_array_to_table]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[gs_func_convert_string_array_to_table]
GO
/****** Object:  Table [dbo].[gs_Role]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role]') AND type in (N'U'))
DROP TABLE [dbo].[gs_Role]
GO
/****** Object:  StoredProcedure [dbo].[gs_SearchGallery]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SearchGallery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_SearchGallery]
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumDelete]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_AlbumDelete]
GO
/****** Object:  Table [dbo].[gs_MediaObject]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]') AND type in (N'U'))
DROP TABLE [dbo].[gs_MediaObject]
GO
/****** Object:  Table [dbo].[gs_Synchronize]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Synchronize]') AND type in (N'U'))
DROP TABLE [dbo].[gs_Synchronize]
GO
/****** Object:  Table [dbo].[gs_Gallery]    Script Date: 07/17/2007 10:05:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Gallery]') AND type in (N'U'))
DROP TABLE [dbo].[gs_Gallery]
GO
/****** Object:  Table [dbo].[gs_Gallery]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Gallery]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_Gallery](
	[GalleryID] [int] NOT NULL,
	[Description] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_gs_Gallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[gs_Synchronize]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Synchronize]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_Synchronize](
	[SynchID] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FKGalleryID] [int] NOT NULL,
	[SynchState] [int] NOT NULL,
	[TotalFiles] [int] NOT NULL,
	[CurrentFileIndex] [int] NOT NULL,
 CONSTRAINT [PK_gs_Synchronize] PRIMARY KEY CLUSTERED 
(
	[SynchID] ASC,
	[FKGalleryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[gs_MediaObject]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_MediaObject](
	[MediaObjectID] [int] IDENTITY(1,1) NOT NULL,
	[FKAlbumID] [int] NOT NULL,
	[Title] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[HashKey] [nchar](47) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ThumbnailFilename] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ThumbnailWidth] [int] NOT NULL,
	[ThumbnailHeight] [int] NOT NULL,
	[ThumbnailSizeKB] [int] NOT NULL,
	[OptimizedFilename] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OptimizedWidth] [int] NOT NULL,
	[OptimizedHeight] [int] NOT NULL,
	[OptimizedSizeKB] [int] NOT NULL,
	[OriginalFilename] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OriginalWidth] [int] NOT NULL,
	[OriginalHeight] [int] NOT NULL,
	[OriginalSizeKB] [int] NOT NULL,
	[Seq] [int] NOT NULL,
	[DateAdded] [smalldatetime] NOT NULL,
	[FilenameHasChanged] [bit] NOT NULL,
 CONSTRAINT [PK_gs_MediaObject] PRIMARY KEY CLUSTERED 
(
	[MediaObjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]') AND name = N'ForeignKeyIndex')
CREATE NONCLUSTERED INDEX [ForeignKeyIndex] ON [dbo].[gs_MediaObject] 
(
	[FKAlbumID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumDelete]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AlbumDelete]
	@AlbumID int
AS
SET NOCOUNT ON
/* Delete the specified album and its objects, including any child albums.  Instead of using 
built-in cascading delete features of the database, we delete all objects manually. This is
primarily because SQL Server does not support cascade delete for hierarchal tables, which is
how album data is stored (the AlbumParentID field refers to the AlbumID field).*/

/* First, build a table containing this album ID and all child album IDs. */
CREATE TABLE #a (aid int, apid int, processed int)

/* Insert this album into our temporary table. */
INSERT #a 
	SELECT AlbumID, AlbumParentID, 0
	FROM [gs_Album] WHERE AlbumID = @AlbumID

/* Set up a loop where we insert the children of the first album, and their children, and so on, until no 
children are left. The end result is that the table is filled with info about @AlbumID and all his descendents.
The processed field in #a represents the # of levels from the bottom. Thus the records
with the MAX processed value is @AlbumID, and the records with the MIN level (should always be 1)
represent the most distant descendents. */
WHILE EXISTS (SELECT * FROM #a WHERE processed = 0)
BEGIN
	/* Insert the children of all albums in #a into the table. We use the ''processed = 0'' criterion because we
	only want to get the children once. Each loop increases the value of the processed field by one.  */
	INSERT #a SELECT AlbumID, AlbumParentID, -1
		FROM [gs_Album] WHERE AlbumParentID IN 
			(SELECT aid FROM #a WHERE processed = 0)
	
	/* Increment the processed value to preserve the heiarchy of albums. */
	UPDATE #a SET processed = processed + 1
END

/* At this point #a contains info about @AlbumID and all his descendents. Delete all media objects 
and roles associated with these albums, and then delete the albums. */
BEGIN TRAN
	DELETE [gs_MediaObject] WHERE FKAlbumID IN (SELECT aid FROM #a)
	
	DELETE [gs_Role_Album] WHERE FKAlbumID IN (SELECT aid FROM #a)
	
	/* Only delete albums that are not the root album (apid <> 0). */
	DELETE [gs_Album] WHERE AlbumID IN (SELECT aid FROM #a WHERE apid <> 0)
COMMIT TRAN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_SearchGallery]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SearchGallery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_SearchGallery]
(	@SearchTerms nvarchar(4000), @GalleryId int)
AS
SET NOCOUNT ON

/* Search for albums and media objects that match the specified search terms. The album or media object must match
ALL search terms to be considered a match. There is no ''OR'' capability. For albums, the Title and Summary columns 
are searched. For media objects, the Title and OriginalFilename columns and all metadata in the MediaObjectMetadata 
tables is searched.

Inputs:
@SearchTerms - A comma-delimited set of search terms. May include spaces. Ex: "cat,dog", "cat videos, dog videos"
  Multiple words in a single search term (such as "cat videos" in the previous example) are treated as a phrase
  that must be matched, exactly like how Google treats phrases contained in quotation marks. That is, "cat videos"
  requires the phrase "cat videos" to appear somewhere in the data, and it will not match "cat and dog videos"
  (to match "cat and dog videos", you can use "cat,videos").
@GalleryId - The ID of the gallery to search.

Returns:
Returns a set of records with two columns:
gotype - A single character containing either ''a'' for album or ''m'' for media object. Indicates whether the id
         stored in the second column is an album or media object.
id - The ID of a matching album or media object.

Algorithm:
The search follows these steps:
1. Create a temporary table #searchTerms and insert the search terms into it, prepending and appending the wildcard
   character (%). Ex: If @SearchTerms = "cat videos,dog,fish", #searchTerms will get 3 records: %cat videos%,
   %dog%, %fish%.
2. Create a second temporary table #searchResults to hold intermediate search results.
3. Insert into #searchResults all albums where the title matches one of more search terms. There will be one record
   inserted for each matching search term. Ex: If @SearchTerms = "cat videos,dog,fish" and the album title =
   "My dog and cat videos", there will be two records inserted into #searchResults, one with matchingSearchTerm =
   "%cat videos%" and the other "%dog%" (gotype=''a'', id=album ID, fieldname=''Album.Title'' for both).
4. Repeat the above step for other fields: Album.Summary, MediaObject.Title, MediaObject.OriginalFilename, and
   all media object metadata for each media object
5. Select those records from #searchResults where we made a successful match for EVERY search term for each album or
   media object.
   
Note: The fieldname column is #searchResults is not used except for manual debugging purposes. This column can
be removed if desired. 
*/

CREATE TABLE #searchTerms (searchTerm nvarchar(4000))
CREATE TABLE #searchResults (gotype char(1), id int, fieldname nvarchar(50), matchingSearchTerm nvarchar(3000))

INSERT #searchTerms
SELECT ''%'' + nstr + ''%'' FROM dbo.gs_func_convert_string_array_to_table(@SearchTerms, '','')

-- Search album title
INSERT #searchResults
SELECT ''a'', gs_Album.AlbumId, ''Album.Title'', ''%'' + SUBSTRING(gs_Album.Title, PATINDEX(#searchTerms.searchTerm, gs_Album.Title), LEN(#searchTerms.searchTerm) - 2) + ''%''
 FROM gs_Album CROSS JOIN #searchTerms
 WHERE gs_Album.FKGalleryId = @GalleryId AND gs_Album.Title LIKE #searchTerms.searchTerm

-- Search album summary
INSERT #searchResults
SELECT ''a'', a.AlbumId, ''Album.Summary'', ''%'' + SUBSTRING(a.Summary, PATINDEX(#searchTerms.searchTerm, a.Summary), LEN(#searchTerms.searchTerm) - 2) + ''%''
 FROM gs_Album a CROSS JOIN #searchTerms
 WHERE a.FKGalleryId = @GalleryId AND a.Summary LIKE #searchTerms.searchTerm

-- Search media object title
INSERT #searchResults
SELECT ''m'', gs_MediaObject.MediaObjectId, ''MediaObject.Title'', ''%'' + SUBSTRING(gs_MediaObject.Title, PATINDEX(#searchTerms.searchTerm, gs_MediaObject.Title), LEN(#searchTerms.searchTerm) - 2) + ''%''
 FROM gs_MediaObject JOIN gs_Album
 ON gs_Album.AlbumId = gs_MediaObject.FKAlbumId CROSS JOIN #searchTerms
 WHERE gs_Album.FKGalleryId = @GalleryId AND gs_MediaObject.Title LIKE #searchTerms.searchTerm --AND 0=1

-- Search media object original filename
INSERT #searchResults
SELECT ''m'', gs_MediaObject.MediaObjectId, ''MediaObject.OriginalFilename'', ''%'' + SUBSTRING(gs_MediaObject.OriginalFilename, PATINDEX(#searchTerms.searchTerm, gs_MediaObject.OriginalFilename), LEN(#searchTerms.searchTerm) - 2) + ''%''
 FROM gs_MediaObject JOIN gs_Album ON gs_Album.AlbumId = gs_MediaObject.FKAlbumId CROSS JOIN #searchTerms
 WHERE gs_Album.FKGalleryId = @GalleryId AND gs_MediaObject.OriginalFilename LIKE #searchTerms.searchTerm --AND 0=1

-- Search media object metadata
INSERT #searchResults
SELECT DISTINCT ''m'', gs_MediaObject.MediaObjectId, ''MediaObjectMetadata'', ''%'' + SUBSTRING(gs_MediaObjectMetadata.Value, PATINDEX(#searchTerms.searchTerm, gs_MediaObjectMetadata.Value), LEN(#searchTerms.searchTerm) - 2) + ''%''
 FROM gs_MediaObjectMetadata JOIN gs_MediaObject
 ON gs_MediaObjectMetadata.FKMediaObjectId = gs_MediaObject.MediaObjectId JOIN gs_Album
 ON gs_Album.AlbumId = gs_MediaObject.FKAlbumId CROSS JOIN #searchTerms
 WHERE gs_Album.FKGalleryId = @GalleryId AND gs_MediaObjectMetadata.Value LIKE #searchTerms.searchTerm

-- Uncomment for debug purposes:
--SELECT * from #searchTerms
--SELECT * FROM #searchResults

SELECT sr.gotype, sr.id
FROM #searchTerms AS st INNER JOIN (SELECT DISTINCT gotype, id, matchingSearchTerm FROM #searchResults) AS sr ON st.searchTerm = sr.matchingSearchTerm
GROUP BY sr.gotype, sr.id
HAVING (COUNT(*) >= (SELECT COUNT(*) FROM #searchTerms))

RETURN
' 
END
GO
/****** Object:  Table [dbo].[gs_Role]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_Role](
	[RoleName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FKGalleryId] [int] NOT NULL,
	[AllowViewAlbumsAndObjects] [bit] NOT NULL,
	[AllowViewOriginalImage] [bit] NOT NULL,
	[AllowAddChildAlbum] [bit] NOT NULL,
	[AllowAddMediaObject] [bit] NOT NULL,
	[AllowEditAlbum] [bit] NOT NULL,
	[AllowEditMediaObject] [bit] NOT NULL,
	[AllowDeleteChildAlbum] [bit] NOT NULL,
	[AllowDeleteMediaObject] [bit] NOT NULL,
	[AllowSynchronize] [bit] NOT NULL,
	[HideWatermark] [bit] NOT NULL,
	[AllowAdministerSite] [bit] NOT NULL,
 CONSTRAINT [PK_gs_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC,
	[FKGalleryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  UserDefinedFunction [dbo].[gs_func_convert_string_array_to_table]    Script Date: 07/17/2007 10:05:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_func_convert_string_array_to_table]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[gs_func_convert_string_array_to_table]
                 (@list      nvarchar(MAX),
                  @delimiter nchar(1) = N'','')
      RETURNS @tbl TABLE (listpos int IDENTITY(1, 1) NOT NULL,
                          str     varchar(4000)      NOT NULL,
                          nstr    nvarchar(2000)     NOT NULL) AS

BEGIN
   DECLARE @endpos   int,
           @startpos int,
           @textpos  int,
           @chunklen smallint,
           @tmpstr   nvarchar(4000),
           @leftover nvarchar(4000),
           @tmpval   nvarchar(4000)

   SET @textpos = 1
   SET @leftover = ''''
   WHILE @textpos <= datalength(@list) / 2
   BEGIN
      SET @chunklen = 4000 - datalength(@leftover) / 2
      SET @tmpstr = @leftover + substring(@list, @textpos, @chunklen)
      SET @textpos = @textpos + @chunklen

      SET @startpos = 0
      SET @endpos = charindex(@delimiter COLLATE Slovenian_BIN2, @tmpstr)

      WHILE @endpos > 0
      BEGIN
         SET @tmpval = ltrim(rtrim(substring(@tmpstr, @startpos + 1,
                                             @endpos - @startpos - 1)))
         INSERT @tbl (str, nstr) VALUES(@tmpval, @tmpval)
         SET @startpos = @endpos
         SET @endpos = charindex(@delimiter COLLATE Slovenian_BIN2,
                                 @tmpstr, @startpos + 1)
      END

      SET @leftover = right(@tmpstr, datalength(@tmpstr) / 2 - @startpos)
   END

   INSERT @tbl(str, nstr)
      VALUES (ltrim(rtrim(@leftover)), ltrim(rtrim(@leftover)))
   RETURN
END' 
END
GO
/****** Object:  Table [dbo].[gs_Album]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Album]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_Album](
	[AlbumID] [int] IDENTITY(1,1) NOT NULL,
	[FKGalleryID] [int] NOT NULL,
	[AlbumParentID] [int] NOT NULL,
	[Title] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DirectoryName] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Summary] [nvarchar](1500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ThumbnailMediaObjectID] [int] NOT NULL,
	[Seq] [int] NOT NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [datetime] NULL,
	[DateAdded] [datetime] NOT NULL,
 CONSTRAINT [PK_gs_Album] PRIMARY KEY CLUSTERED 
(
	[AlbumID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[gs_Role_Album]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_Album]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_Role_Album](
	[FKRoleName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FKAlbumId] [int] NOT NULL,
 CONSTRAINT [PK_gs_Role_Album] PRIMARY KEY CLUSTERED 
(
	[FKRoleName] ASC,
	[FKAlbumId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[gs_MediaObjectMetadata]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadata]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[gs_MediaObjectMetadata](
	[MediaObjectMetadataId] [int] IDENTITY(1,1) NOT NULL,
	[FKMediaObjectId] [int] NOT NULL,
	[MetadataNameIdentifier] [int] NOT NULL,
	[Description] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Value] [nvarchar](2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_gs_MediaObjectMetadata] PRIMARY KEY CLUSTERED 
(
	[MediaObjectMetadataId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataDelete]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectMetadataDelete]
( @MediaObjectMetadataId int )
AS
/* Delete a new media object meta data item */
DELETE [gs_MediaObjectMetadata]
WHERE MediaObjectMetadataId = @MediaObjectMetadataId' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataUpdate]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectMetadataUpdate]
(@FKMediaObjectId int, @MetadataNameIdentifier int, @Description nvarchar(100), @Value nvarchar(2000),
 @MediaObjectMetadataId int)
AS
/* Update a new media object meta data item */
UPDATE [gs_MediaObjectMetadata]
SET FKMediaObjectId = @FKMediaObjectId,
 MetadataNameIdentifier = @MetadataNameIdentifier,
 Description = @Description,
 Value = @Value
WHERE MediaObjectMetadataId = @MediaObjectMetadataId' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId]
( @MediaObjectId int )
AS
/* Delete all new metadata items belonging to the specified media object ID. */
DELETE [gs_MediaObjectMetadata]
WHERE FKMediaObjectId = @MediaObjectId' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataInsert]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectMetadataInsert]
(@FKMediaObjectId int, @MetadataNameIdentifier int, @Description nvarchar(100), @Value nvarchar(2000),
 @Identity int OUT)
AS
/* Insert a new media object meta data item */
INSERT [gs_MediaObjectMetadata] (FKMediaObjectId, MetadataNameIdentifier, Description, Value)
VALUES (@FKMediaObjectId, @MetadataNameIdentifier, @Description, @Value)

SET @Identity = SCOPE_IDENTITY()' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectMetadataSelect]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadataSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectMetadataSelect]
(
	@MediaObjectId int, @GalleryId int
)
AS
SET NOCOUNT ON

SELECT
	md.MediaObjectMetadataId, md.FKMediaObjectId, md.MetadataNameIdentifier, md.Description, md.Value
FROM [gs_MediaObjectMetadata] md JOIN [gs_MediaObject] mo ON md.FkMediaObjectId = mo.MediaObjectId
	JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
WHERE md.FKMediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_VerifyMinimumRecords]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_VerifyMinimumRecords]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_VerifyMinimumRecords]
(
	@GalleryID int
)
AS
SET NOCOUNT ON
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
IF NOT EXISTS (SELECT * FROM [gs_Gallery] WHERE GalleryID = @GalleryID)
BEGIN
	INSERT gs_Gallery (GalleryId, Description)
	VALUES (@GalleryId, ''My Gallery'')
END

/* ******************************************************* */
--          Check the Album table
/* ******************************************************* */
IF NOT EXISTS (SELECT * FROM [gs_Album] WHERE AlbumParentID = 0 AND FKGalleryID = @GalleryID)
BEGIN
  /* The album table does not have a root album for the specified gallery. Insert one. */
  INSERT [gs_Album] (AlbumParentID, FKGalleryId, Title, DirectoryName, Summary, ThumbnailMediaObjectID, Seq, DateAdded)
  VALUES (0, @GalleryID, ''All albums'', '''',''Welcome to Gallery Server Pro!'', 0, 0, GETDATE())
END 

/* ******************************************************* */
--          Check the Synchronize table
/* ******************************************************* */
IF NOT EXISTS (SELECT * FROM [gs_Synchronize] WHERE FKGalleryID = @GalleryID)
BEGIN
	INSERT INTO gs_Synchronize (SynchID, FKGalleryID, SynchState, TotalFiles, CurrentFileIndex)
	VALUES ('''',@GalleryId, 0, 0, 0)
END
ELSE
BEGIN -- Reset record to clear out any previous synchronization
	UPDATE gs_Synchronize
	SET SynchID = '''',
	SynchState = 0,
	TotalFiles = 0,
	CurrentFileIndex = 0
	WHERE FKGalleryID = @GalleryId
END

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_InitializeGalleryServerPro]    Script Date: 07/17/2007 11:48:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_InitializeGalleryServerPro]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_InitializeGalleryServerPro] 
(
	@GalleryID int,
	@ApplicationName nvarchar(512),
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
DECLARE @SysAdminRoleName nvarchar(256)
DECLARE @RootAlbumId int
DECLARE @AdminUserId uniqueidentifier

SET @SysAdminRoleName = N''System Administrator''

IF (NOT EXISTS (SELECT name FROM sysobjects WHERE (name = N''aspnet_Applications'') AND (type = ''U'')))
	RETURN -- No Membership tables in this database. User presumably wants to connect to a Membership infrastructure in 
				 -- a remote database. This must be done manually so let''s just exit.
	
-- Get root album ID
SELECT @RootAlbumId = AlbumID FROM gs_Album WHERE (AlbumParentID = 0)

--Get or Create the Application ID.
--An ApplicationName/ID can exist in more than one site, so it does not need to be unique
Set @ApplicationID = null

Select @ApplicationID = ApplicationId FROM aspnet_Applications where  LoweredApplicationName = Lower(@ApplicationName)


IF(@ApplicationID is  null)
BEGIN /* Application doesn''t exist so let''s create it */
	EXEC aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId = @ApplicationID OUTPUT
	Set @ApplicationAlreadyExists = 0
END
ELSE
	Set @ApplicationAlreadyExists = 1

/* ******************************************************* */
/* If Gallery Server Pro app doesn''t exist, create sys admin 
	 role and admin user; otherwise create the role and user
	 only if they don''t already exist. */
/* ******************************************************* */
IF(@ApplicationAlreadyExists = 0)
BEGIN
	/* ******************************************************* */
	--			Gallery Server Pro app exists
	--     Create system administrator role
	/* ******************************************************* */
	INSERT INTO [aspnet_Roles] ([RoleId], [ApplicationId], [RoleName], [LoweredRoleName], [Description])
	VALUES (newID(), @ApplicationID, @SysAdminRoleName, N''systemadministrator'', N''Global Administration role.'')	

	/* ******************************************************* */
	--                     Create admin user
	/* ******************************************************* */
	Set @AdminUserId = newid()

	INSERT INTO [aspnet_Users] ([UserId], [ApplicationId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) 
		VALUES (@AdminUserId, @ApplicationID, @AdminUserName, Lower(@AdminUserName), NULL, 0, getdate())

	INSERT INTO [aspnet_Membership] ([ApplicationId], [UserId], [Password],     [PasswordFormat], [PasswordSalt],              [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [Comment], FailedPasswordAnswerAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAttemptCount, LastLockoutDate, IsLockedOut) 
		VALUES (@ApplicationID, @AdminUserId,  @AdminPassword, @PasswordFormat,   N''DVZTktxeMzDtXR7eik7Cdw=='', NULL,       @AdminEmail,    NULL,            NULL,               NULL,            1,            getdate(),    getdate(),       getdate(),                 NULL,                 ''1753-01-01'',                              0,                               ''1753-01-01'',                       0,                          ''1753-01-01'',      0)

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
			VALUES (@ApplicationID, @AdminUserId,  @AdminPassword, @PasswordFormat,   N''DVZTktxeMzDtXR7eik7Cdw=='', NULL,       @AdminEmail,    NULL,            NULL,               NULL,            1,            getdate(),    getdate(),       getdate(),                 NULL,                 ''1753-01-01'',                              0,                               ''1753-01-01'',                       0,                          ''1753-01-01'',      0)
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
			VALUES (newID(), @ApplicationID, @SysAdminRoleName, N''systemadministrator'', N''Global Administration role.'')	
		 
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
	INSERT [gs_Album] (AlbumParentID, FKGalleryID, Title, DirectoryName, Summary, ThumbnailMediaObjectID, Seq, DateStart, DateEnd, DateAdded)
	VALUES (@RootAlbumId, @GalleryID, ''Samples'', ''Samples'', ''Death Valley'', 0, 1, GETDATE(), DATEADD(week, 2, GETDATE()), GETDATE())

	SET @newAlbumId = SCOPE_IDENTITY()
	
	INSERT [gs_MediaObject] (HashKey, FKAlbumID, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
	 ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
	 OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, Title, Seq, DateAdded)
	VALUES (''4C-A8-D6-E6-88-29-28-BE-3A-79-EF-0B-F5-9B-2D-EE'', @newAlbumId, ''zThumb_DeathValley.jpeg'', 115, 86,
	 2, ''DeathValley.jpeg'', 640, 480, 48, ''DeathValley.jpeg'', 640, 480, 48, ''Death Valley, California'', 1, GETDATE())
 
	SET @newMediaObjectId = SCOPE_IDENTITY()
	
	/* Set this new image as the thumbnail for the album */
	UPDATE [gs_Album] SET ThumbnailMediaObjectID = @newMediaObjectId WHERE AlbumID = @newAlbumId
END
RETURN
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_SynchronizeSave]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SynchronizeSave]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_SynchronizeSave]
	(@SynchID nchar(50), @GalleryId int, @SynchState int, @TotalFiles int, @CurrentFileIndex int)
AS
SET NOCOUNT ON
/* UPDATE the synchronize table with the specified data. */

/* Check if another synchronization is in progress. Return with error code if it is. */
IF EXISTS(SELECT * FROM [gs_Synchronize] WHERE FKGalleryID = @GalleryId AND
																					SynchID <> @SynchID AND 
																					(SynchState = 1 OR SynchState = 2))
BEGIN
	RETURN 250000
END

IF EXISTS (SELECT * FROM [gs_Synchronize] WHERE FKGalleryID = @GalleryId)
	UPDATE [gs_Synchronize]
	SET SynchID = @SynchID,
		FKGalleryId = @GalleryId,
		SynchState = @SynchState,
		TotalFiles = @TotalFiles,
		CurrentFileIndex = @CurrentFileIndex
	WHERE FKGalleryID = @GalleryId
ELSE
	INSERT [gs_Synchronize] (SynchID, FKGalleryID, SynchState, TotalFiles, CurrentFileIndex)
	VALUES (@SynchID, @GalleryId, @SynchState, @TotalFiles, @CurrentFileIndex)
	
RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_SynchronizeSelect]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SynchronizeSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_SynchronizeSelect]
(@GalleryId int)
AS
SET NOCOUNT ON

/* Return the synchronize data for the specified gallery. It should contain 1 record. */
SELECT SynchID, FKGalleryID, SynchState, TotalFiles, CurrentFileIndex
FROM [gs_Synchronize]
WHERE FKGalleryID = @GalleryId

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName]
(	@RoleName nvarchar(256), @GalleryId int )

AS
SET NOCOUNT ON

SELECT gs_Role_Album.FKAlbumId
FROM gs_Role_Album INNER JOIN gs_Album ON gs_Role_Album.FKAlbumId = gs_Album.AlbumID
WHERE (gs_Role_Album.FKRoleName = @RoleName) AND (gs_Album.FKGalleryID = @GalleryId)

RETURN
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumInsert]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AlbumInsert]
(@AlbumParentID int, @GalleryID int, @Title nvarchar(200), @DirectoryName nvarchar(25),
 @Summary nvarchar(1500), @ThumbnailMediaObjectID int, @Seq int, 
 @DateStart datetime, @DateEnd datetime, @DateAdded datetime, @Identity int OUT)
AS
/* Insert a new album. */
INSERT [gs_Album] (AlbumParentID, FKGalleryID, Title, DirectoryName, 
 Summary, ThumbnailMediaObjectID, Seq, 
 DateStart, DateEnd, DateAdded)
VALUES (@AlbumParentID, @GalleryID, @Title, @DirectoryName, 
 @Summary, @ThumbnailMediaObjectID, @Seq, 
 @DateStart, @DateEnd, @DateAdded)

SET @Identity = SCOPE_IDENTITY()' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumSelect]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AlbumSelect]
(
	@AlbumId int, @GalleryId int
)
AS
SET NOCOUNT ON

SELECT
	a.AlbumID, a.FKGalleryID as GalleryID, a.AlbumParentID, a.Title, a.DirectoryName, a.Summary, a.ThumbnailMediaObjectID, 
	a.Seq, a.DateStart, a.DateEnd, a.DateAdded
FROM [gs_Album] a
WHERE a.AlbumId = @AlbumId AND a.FKGalleryId = @GalleryId

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectSelect]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectSelect]
(
	@MediaObjectId int, @GalleryId int
)
AS
SET NOCOUNT ON

SELECT
	mo.MediaObjectID, mo.FKAlbumID, mo.Title, mo.HashKey, mo.ThumbnailFilename, mo.ThumbnailWidth, mo.ThumbnailHeight, 
	mo.ThumbnailSizeKB, mo.OptimizedFilename, mo.OptimizedWidth, mo.OptimizedHeight, mo.OptimizedSizeKB, 
	mo.OriginalFilename, mo.OriginalWidth, mo.OriginalHeight, mo.OriginalSizeKB, mo.Seq, 
	mo.DateAdded, mo.FilenameHasChanged
FROM [gs_MediaObject] mo JOIN [gs_Album] a ON mo.FKAlbumID = a.AlbumID
WHERE mo.MediaObjectID = @MediaObjectId AND a.FKGalleryID = @GalleryID

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_AlbumUpdate]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_AlbumUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_AlbumUpdate]
(@AlbumID int, @AlbumParentID int, @Title nvarchar(200), @DirectoryName nvarchar(25),
 @Summary nvarchar(1500), @ThumbnailMediaObjectID int,  @Seq int, 
 @DateStart datetime, @DateEnd datetime)
 
AS
SET NOCOUNT ON
UPDATE [gs_Album]
SET
	AlbumParentID = @AlbumParentID,
	Title = @Title,
	DirectoryName = @DirectoryName,
	Summary = @Summary,
	ThumbnailMediaObjectID = @ThumbnailMediaObjectID,
	Seq = @Seq,
	DateStart = @DateStart,
	DateEnd = @DateEnd
WHERE (AlbumID = @AlbumID)

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_SelectChildAlbums]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SelectChildAlbums]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_SelectChildAlbums]
(
	@AlbumId int, @GalleryId int
)
AS
SET NOCOUNT ON

SELECT AlbumID
FROM [gs_Album]
WHERE AlbumParentID = @AlbumId AND FKGalleryID = @GalleryId

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectSelectHashKeys]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectSelectHashKeys]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectSelectHashKeys]
(
	@GalleryId int
)
AS
SET NOCOUNT ON

SELECT [gs_MediaObject].HashKey
FROM [gs_MediaObject] INNER JOIN [gs_Album] ON [gs_MediaObject].FKAlbumID = [gs_Album].AlbumID
WHERE [gs_Album].FKGalleryID = @GalleryId

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName]
(	@RoleName nvarchar(256), @GalleryId int )

AS
SET NOCOUNT ON

/* Retrieve all the album IDs that are affected by the specified role name. The album IDs that are stored in
   the gs_Role_Album table only hold the highest ranking album ID, so we need to drill down and retrieve all
   the children. */

/* Create a temporary table to hold our data. */
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

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectUpdate]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectUpdate]
(
 @MediaObjectId int, @HashKey char(47), @FKAlbumID int, 
 @ThumbnailFilename nvarchar(255), @ThumbnailWidth int, @ThumbnailHeight int, @ThumbnailSizeKB int,
 @OriginalFilename nvarchar(255),	@OriginalWidth int, @OriginalHeight int, @OriginalSizeKB int, 
 @OptimizedFilename nvarchar(255),	@OptimizedWidth int, @OptimizedHeight int, @OptimizedSizeKB int, 
 @Title nvarchar(1000), @Seq int
)
AS
SET NOCOUNT ON

/* Used by the synchronization method to update the media objects. */
UPDATE [gs_MediaObject]
SET HashKey = @HashKey, FKAlbumID = @FKAlbumID,
 ThumbnailFilename = @ThumbnailFilename, ThumbnailWidth = @ThumbnailWidth, 
 ThumbnailHeight = @ThumbnailHeight, ThumbnailSizeKB = @ThumbnailSizeKB,
 OptimizedFilename = @OptimizedFilename, OptimizedWidth = @OptimizedWidth,
 OptimizedHeight = @OptimizedHeight, OptimizedSizeKB = @OptimizedSizeKB, 
 OriginalFilename = @OriginalFilename, OriginalWidth = @OriginalWidth,
 OriginalHeight = @OriginalHeight, OriginalSizeKB = @OriginalSizeKB, 
 Title = @Title, Seq = @Seq
WHERE MediaObjectID = @MediaObjectId' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectDelete]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectDelete]
(
	@MediaObjectID int
)
AS
SET NOCOUNT ON
/* Delete the specified media object. */
DELETE [gs_MediaObject]
WHERE MediaObjectID = @MediaObjectID

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_SelectChildMediaObjects]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SelectChildMediaObjects]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_SelectChildMediaObjects]
(
	@AlbumId int
)
AS
SET NOCOUNT ON

SELECT 
	MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, Seq, 
	DateAdded, FilenameHasChanged
FROM [gs_MediaObject]
WHERE FKAlbumID = @AlbumId

RETURN' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_MediaObjectInsert]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_MediaObjectInsert]
(@HashKey char(47), @FKAlbumID int, @ThumbnailFilename nvarchar(255), 
 @ThumbnailWidth int, @ThumbnailHeight int,
 @ThumbnailSizeKB int, @OptimizedFilename nvarchar(255), 
 @OptimizedWidth int, @OptimizedHeight int,
 @OptimizedSizeKB int, @OriginalFilename nvarchar(255),	 
 @OriginalWidth int, @OriginalHeight int, @OriginalSizeKB int, 
 @Title nvarchar(1000), @Seq int, @DateAdded smalldatetime,
 @Identity int OUT)
AS

/* Insert media object information. Called during MediaObject.Save() when the object is new. */
 
 INSERT [gs_MediaObject] (HashKey, FKAlbumID, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight,
 ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB,
 OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, Title, Seq, DateAdded)
VALUES (@HashKey, @FKAlbumID, @ThumbnailFilename, @ThumbnailWidth, @ThumbnailHeight,
 @ThumbnailSizeKB, @OptimizedFilename, @OptimizedWidth, @OptimizedHeight, @OptimizedSizeKB,
 @OriginalFilename, @OriginalWidth, @OriginalHeight, @OriginalSizeKB,
 @Title, @Seq, @DateAdded)
 
SET @Identity = SCOPE_IDENTITY()' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleDelete]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_RoleDelete]
( @GalleryId int, @RoleName nvarchar(256) )
AS
/* Delete a gallery server role. This procedure only deletes it from the custom gallery server tables,
not the ASP.NET role membership table(s). The web application code that invokes this procedure also
uses the standard ASP.NET technique to delete the role from the membership table(s). */

-- First delete the records from the role/album association table.
DELETE [gs_Role_Album]
WHERE FKRoleName = @RoleName

-- Finally delete the role.
DELETE [gs_Role]
WHERE FKGalleryId = @GalleryId AND RoleName = @RoleName
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumInsert]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_Role_AlbumInsert]
(
	@RoleName nvarchar(256), @AlbumId int
)
AS
SET NOCOUNT ON

INSERT [gs_Role_Album] (FKRoleName, FKAlbumId)
VALUES (@RoleName, @AlbumId)

RETURN
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_Role_AlbumDelete]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_Role_AlbumDelete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_Role_AlbumDelete]
(
	@RoleName nvarchar(256), @AlbumId int
)
AS
SET NOCOUNT ON

DELETE FROM [gs_Role_Album]
WHERE FKRoleName = @RoleName AND FKAlbumId = @AlbumId

RETURN
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleSelect]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleSelect]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_RoleSelect]
(@GalleryId int)

AS
SET NOCOUNT ON

SELECT RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
	AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, 
	AllowDeleteMediaObject, AllowSynchronize, HideWatermark, AllowAdministerSite
FROM gs_Role
WHERE (FKGalleryId = @GalleryId)

RETURN
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleInsert]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleInsert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_RoleInsert]
(
	@GalleryId int, @RoleName nvarchar(256), @AllowViewAlbumsAndObjects bit, @AllowViewOriginalImage bit,
	@AllowAddChildAlbum bit, @AllowAddMediaObject bit, @AllowEditAlbum bit, @AllowEditMediaObject bit,
	@AllowDeleteChildAlbum bit, @AllowDeleteMediaObject bit, @AllowSynchronize bit, @HideWatermark bit,
	@AllowAdministerSite bit
)
AS
SET NOCOUNT ON

INSERT [gs_Role] (FKGalleryId, RoleName, AllowViewAlbumsAndObjects, AllowViewOriginalImage, AllowAddChildAlbum,
	AllowAddMediaObject, AllowEditAlbum, AllowEditMediaObject, AllowDeleteChildAlbum, AllowDeleteMediaObject, 
	AllowSynchronize, HideWatermark, AllowAdministerSite)
VALUES (@GalleryId, @RoleName, @AllowViewAlbumsAndObjects, @AllowViewOriginalImage, @AllowAddChildAlbum,
	@AllowAddMediaObject, @AllowEditAlbum, @AllowEditMediaObject, @AllowDeleteChildAlbum, @AllowDeleteMediaObject, 
	@AllowSynchronize, @HideWatermark, @AllowAdministerSite)

RETURN
' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_SelectRootAlbum]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_SelectRootAlbum]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_SelectRootAlbum]
(
	@GalleryId int
)
AS
SET NOCOUNT ON

/* Return the root album. First, check to make sure it exists.
If not, call the stored proc that will insert a default record. */

IF NOT EXISTS (SELECT * FROM [gs_Album] WHERE AlbumParentID = 0 AND FKGalleryID = @GalleryId)
	EXEC dbo.gs_VerifyMinimumRecords @GalleryId

SELECT
	AlbumID, AlbumParentID, Title, DirectoryName, Summary, ThumbnailMediaObjectID, 
	Seq, DateStart, DateEnd, DateAdded
FROM [gs_Album]
WHERE AlbumParentID = 0 AND FKGalleryID = @GalleryId' 
END
GO
/****** Object:  StoredProcedure [dbo].[gs_RoleUpdate]    Script Date: 07/17/2007 10:05:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_RoleUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[gs_RoleUpdate] 
(
	@GalleryId int, @RoleName nvarchar(256), @AllowViewAlbumsAndObjects bit, @AllowViewOriginalImage bit,
	@AllowAddChildAlbum bit, @AllowAddMediaObject bit, @AllowEditAlbum bit, @AllowEditMediaObject bit,
	@AllowDeleteChildAlbum bit, @AllowDeleteMediaObject bit, @AllowSynchronize bit, @HideWatermark bit,
	@AllowAdministerSite bit
)
AS
SET NOCOUNT ON

/* Update the specified role. If the role does not exist, assume it is new and call the insert proc. */
IF EXISTS(SELECT * FROM [gs_Role] WHERE FKGalleryId = @GalleryId AND RoleName = @RoleName)
BEGIN
	UPDATE [gs_Role]
	SET AllowViewAlbumsAndObjects = @AllowViewAlbumsAndObjects,
		AllowViewOriginalImage = @AllowViewOriginalImage,
		AllowAddChildAlbum = @AllowAddChildAlbum,
		AllowAddMediaObject = @AllowAddMediaObject,
		AllowEditAlbum = @AllowEditAlbum,
		AllowEditMediaObject = @AllowEditMediaObject,
		AllowDeleteChildAlbum = @AllowDeleteChildAlbum,
		AllowDeleteMediaObject = @AllowDeleteMediaObject,
		AllowSynchronize = @AllowSynchronize,
		HideWatermark = @HideWatermark, 
		AllowAdministerSite = @AllowAdministerSite
	WHERE FKGalleryId = @GalleryId AND RoleName = @RoleName
END
ELSE
BEGIN
	EXECUTE [dbo].[gs_RoleInsert] 
		@GalleryId,
		@RoleName,
		@AllowViewAlbumsAndObjects,
		@AllowViewOriginalImage,
		@AllowAddChildAlbum,
		@AllowAddMediaObject,
		@AllowEditAlbum,
		@AllowEditMediaObject,
		@AllowDeleteChildAlbum,
		@AllowDeleteMediaObject,
		@AllowSynchronize,
		@HideWatermark,
		@AllowAdministerSite
END
RETURN
' 
END
GO
/****** Object:  Default [DF_gs_Album_Title]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] ADD  CONSTRAINT [DF_gs_Album_Title]  DEFAULT ('') FOR [Title]

End
GO
/****** Object:  Default [DF_gs_Album_DirectoryName]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_DirectoryName]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] ADD  CONSTRAINT [DF_gs_Album_DirectoryName]  DEFAULT ('') FOR [DirectoryName]

End
GO
/****** Object:  Default [DF_gs_Album_Summary]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_Summary]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] ADD  CONSTRAINT [DF_gs_Album_Summary]  DEFAULT ('') FOR [Summary]

End
GO
/****** Object:  Default [DF_gs_Album_ThumbnailMediaObjectID]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_Album_ThumbnailMediaObjectID]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
Begin
ALTER TABLE [dbo].[gs_Album] ADD  CONSTRAINT [DF_gs_Album_ThumbnailMediaObjectID]  DEFAULT ((0)) FOR [ThumbnailMediaObjectID]

End
GO
/****** Object:  Default [DF_gs_MediaObject_Title]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_Title]  DEFAULT ('') FOR [Title]

End
GO
/****** Object:  Default [DF_gs_MediaObject_HashKey]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_HashKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_HashKey]  DEFAULT ('') FOR [HashKey]

End
GO
/****** Object:  Default [DF_gs_MediaObject_ThumbnailFilename]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_ThumbnailFilename]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_ThumbnailFilename]  DEFAULT ('') FOR [ThumbnailFilename]

End
GO
/****** Object:  Default [DF_gs_MediaObject_ThumbnailWidth]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_ThumbnailWidth]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_ThumbnailWidth]  DEFAULT ((0)) FOR [ThumbnailWidth]

End
GO
/****** Object:  Default [DF_gs_MediaObject_ThumbnailHeight]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_ThumbnailHeight]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_ThumbnailHeight]  DEFAULT ((0)) FOR [ThumbnailHeight]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OptimizedFilename]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OptimizedFilename]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_OptimizedFilename]  DEFAULT ('') FOR [OptimizedFilename]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OptimizedWidth]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OptimizedWidth]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_OptimizedWidth]  DEFAULT ((0)) FOR [OptimizedWidth]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OptimizedHeight]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OptimizedHeight]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_OptimizedHeight]  DEFAULT ((0)) FOR [OptimizedHeight]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OriginalFilename]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OriginalFilename]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_OriginalFilename]  DEFAULT ('') FOR [OriginalFilename]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OriginalWidth]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OriginalWidth]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_OriginalWidth]  DEFAULT ((0)) FOR [OriginalWidth]

End
GO
/****** Object:  Default [DF_gs_MediaObject_OriginalHeight]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_OriginalHeight]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_OriginalHeight]  DEFAULT ((0)) FOR [OriginalHeight]

End
GO
/****** Object:  Default [DF_gs_MediaObject_FilenameHasChanged]    Script Date: 07/17/2007 10:05:40 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_gs_MediaObject_FilenameHasChanged]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]'))
Begin
ALTER TABLE [dbo].[gs_MediaObject] ADD  CONSTRAINT [DF_gs_MediaObject_FilenameHasChanged]  DEFAULT ((0)) FOR [FilenameHasChanged]

End
GO
/****** Object:  ForeignKey [FK_gs_Album_gs_Album]    Script Date: 07/17/2007 10:05:40 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_Album_gs_Album]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
ALTER TABLE [dbo].[gs_Album]  WITH CHECK ADD  CONSTRAINT [FK_gs_Album_gs_Album] FOREIGN KEY([AlbumID])
REFERENCES [dbo].[gs_Album] ([AlbumID])
GO
ALTER TABLE [dbo].[gs_Album] CHECK CONSTRAINT [FK_gs_Album_gs_Album]
GO
/****** Object:  ForeignKey [FK_gs_Album_gs_Gallery]    Script Date: 07/17/2007 10:05:40 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_Album_gs_Gallery]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Album]'))
ALTER TABLE [dbo].[gs_Album]  WITH CHECK ADD  CONSTRAINT [FK_gs_Album_gs_Gallery] FOREIGN KEY([FKGalleryID])
REFERENCES [dbo].[gs_Gallery] ([GalleryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[gs_Album] CHECK CONSTRAINT [FK_gs_Album_gs_Gallery]
GO
/****** Object:  ForeignKey [FK_gs_MediaObjectMetadata_gs_MediaObject]    Script Date: 07/17/2007 10:05:40 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_MediaObjectMetadata_gs_MediaObject]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadata]'))
ALTER TABLE [dbo].[gs_MediaObjectMetadata]  WITH CHECK ADD  CONSTRAINT [FK_gs_MediaObjectMetadata_gs_MediaObject] FOREIGN KEY([FKMediaObjectId])
REFERENCES [dbo].[gs_MediaObject] ([MediaObjectID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[gs_MediaObjectMetadata] CHECK CONSTRAINT [FK_gs_MediaObjectMetadata_gs_MediaObject]
GO
/****** Object:  ForeignKey [FK_gs_Role_Album_gs_Album]    Script Date: 07/17/2007 10:05:40 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_gs_Role_Album_gs_Album]') AND parent_object_id = OBJECT_ID(N'[dbo].[gs_Role_Album]'))
ALTER TABLE [dbo].[gs_Role_Album]  WITH CHECK ADD  CONSTRAINT [FK_gs_Role_Album_gs_Album] FOREIGN KEY([FKAlbumId])
REFERENCES [dbo].[gs_Album] ([AlbumID])
GO
ALTER TABLE [dbo].[gs_Role_Album] CHECK CONSTRAINT [FK_gs_Role_Album_gs_Album]
GO

/* Create SQL role that will have permission to execute Gallery Server related objects */
IF ( NOT EXISTS ( SELECT name
                  FROM sysusers
                  WHERE issqlrole = 1
                  AND name = N'gs_GalleryServerProRole'  ) )
EXEC sp_addrole N'gs_GalleryServerProRole'
GO

/* Grant permission to SQL role. */
GRANT SELECT ON [dbo].[gs_func_convert_string_array_to_table] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AlbumDelete] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AlbumInsert] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AlbumSelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_AlbumUpdate] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectDelete] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectInsert] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectMetadataDelete] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectMetadataDeleteByMediaObjectId] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectMetadataInsert] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectMetadataSelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectMetadataUpdate] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectSelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectSelectHashKeys] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_MediaObjectUpdate] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_Role_AlbumDelete] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_Role_AlbumInsert] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_Role_AlbumSelectAllAlbumsByRoleName] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_Role_AlbumSelectRootAlbumsByRoleName] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_RoleDelete] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_RoleInsert] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_RoleSelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_RoleUpdate] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_SearchGallery] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_SelectChildAlbums] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_SelectChildMediaObjects] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_SelectRootAlbum] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_SynchronizeSave] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_SynchronizeSelect] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_VerifyMinimumRecords] TO [gs_GalleryServerProRole]
GO
GRANT EXECUTE ON [dbo].[gs_InitializeGalleryServerPro] TO [gs_GalleryServerProRole]
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Membership_BasicAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Membership_BasicAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Membership_FullAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Membership_FullAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Membership_ReportingAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Membership_ReportingAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Profile_BasicAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Profile_BasicAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Profile_FullAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Profile_FullAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Profile_ReportingAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Profile_ReportingAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Roles_BasicAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Roles_BasicAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Roles_FullAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Roles_FullAccess', @membername=N'gs_GalleryServerProRole'
GO

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'aspnet_Roles_ReportingAccess')
	EXEC sys.sp_addrolemember @rolename=N'aspnet_Roles_ReportingAccess', @membername=N'gs_GalleryServerProRole'
GO
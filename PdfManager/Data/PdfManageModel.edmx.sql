
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/05/2016 19:16:08
-- Generated from EDMX file: E:\Code\Desktop\PdfManager\PdfManager\Data\PdfManageModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PdfDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserPdfFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PdfFileSet] DROP CONSTRAINT [FK_UserPdfFile];
GO
IF OBJECT_ID(N'[dbo].[FK_TagPdfFile_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TagPdfFile] DROP CONSTRAINT [FK_TagPdfFile_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_TagPdfFile_PdfFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TagPdfFile] DROP CONSTRAINT [FK_TagPdfFile_PdfFile];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[PdfFileSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PdfFileSet];
GO
IF OBJECT_ID(N'[dbo].[TagSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TagSet];
GO
IF OBJECT_ID(N'[dbo].[TagPdfFile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TagPdfFile];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [LastLoginTime] datetime  NOT NULL
);
GO

-- Creating table 'PdfFileSet'
CREATE TABLE [dbo].[PdfFileSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [Year] int  NULL,
    [FileId] bigint  NULL,
    [Other1] nvarchar(max)  NULL,
    [Other2] nvarchar(max)  NULL,
    [CreateBy_Id] int  NOT NULL
);
GO

-- Creating table 'TagSet'
CREATE TABLE [dbo].[TagSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TagPdfFile'
CREATE TABLE [dbo].[TagPdfFile] (
    [Tags_Id] int  NOT NULL,
    [PdfFiles_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PdfFileSet'
ALTER TABLE [dbo].[PdfFileSet]
ADD CONSTRAINT [PK_PdfFileSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TagSet'
ALTER TABLE [dbo].[TagSet]
ADD CONSTRAINT [PK_TagSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Tags_Id], [PdfFiles_Id] in table 'TagPdfFile'
ALTER TABLE [dbo].[TagPdfFile]
ADD CONSTRAINT [PK_TagPdfFile]
    PRIMARY KEY CLUSTERED ([Tags_Id], [PdfFiles_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CreateBy_Id] in table 'PdfFileSet'
ALTER TABLE [dbo].[PdfFileSet]
ADD CONSTRAINT [FK_UserPdfFile]
    FOREIGN KEY ([CreateBy_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPdfFile'
CREATE INDEX [IX_FK_UserPdfFile]
ON [dbo].[PdfFileSet]
    ([CreateBy_Id]);
GO

-- Creating foreign key on [Tags_Id] in table 'TagPdfFile'
ALTER TABLE [dbo].[TagPdfFile]
ADD CONSTRAINT [FK_TagPdfFile_Tag]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[TagSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PdfFiles_Id] in table 'TagPdfFile'
ALTER TABLE [dbo].[TagPdfFile]
ADD CONSTRAINT [FK_TagPdfFile_PdfFile]
    FOREIGN KEY ([PdfFiles_Id])
    REFERENCES [dbo].[PdfFileSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TagPdfFile_PdfFile'
CREATE INDEX [IX_FK_TagPdfFile_PdfFile]
ON [dbo].[TagPdfFile]
    ([PdfFiles_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
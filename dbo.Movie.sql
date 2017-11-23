USE [Movies]
GO

/****** Object: Table [dbo].[Movie] Script Date: 23/11/2017 00:51:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Movie] (
    [Id]          INT           NOT NULL,
    [Title]       VARCHAR (100) NOT NULL,
    [ReleaseDate] DATETIME      NOT NULL,
    [Director]    VARCHAR (100) NOT NULL
);



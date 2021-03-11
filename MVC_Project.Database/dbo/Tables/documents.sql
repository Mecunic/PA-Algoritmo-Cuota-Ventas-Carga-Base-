CREATE TABLE [dbo].[documents] (
    [DocumentId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Uuid]          NVARCHAR (250) NOT NULL,
    [URL]           NVARCHAR (500) NULL,
    [URL_Secondary] NVARCHAR (500) NULL,
    [Name]          NVARCHAR (250) NULL,
    [UserId]        BIGINT         NULL,
    [creation_date] DATETIME2 (7)  NULL,
    [type]          NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([DocumentId] ASC)
);


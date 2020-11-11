CREATE TABLE [calendar].[events] (
    [eventId]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [title]         NVARCHAR (255) NULL,
    [uuid]          NVARCHAR (50)  NULL,
    [startdate]     DATETIME2 (7)  NULL,
    [enddate]       DATETIME2 (7)  NULL,
    [userId]        BIGINT         NULL,
    [creation_date] DATETIME2 (7)  NULL,
    [description]   NVARCHAR (500) NULL,
    [isFullDay]     BIT            NULL,
    [categoryId]    INT            NULL,
    PRIMARY KEY CLUSTERED ([eventId] ASC)
);


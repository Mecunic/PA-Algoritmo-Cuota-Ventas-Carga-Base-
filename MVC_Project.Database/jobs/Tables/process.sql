CREATE TABLE [jobs].[process] (
    [code]              VARCHAR (255)  NOT NULL,
    [description]       VARCHAR (255)  NULL,
    [status]            BIT            NULL,
    [running]           BIT            NULL,
    [last_execution_at] DATETIME2 (7)  NULL,
    [result]            NVARCHAR (MAX) NULL,
    [processId]         BIGINT         IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([processId] ASC)
);


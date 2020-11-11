CREATE TABLE [jobs].[process_execution] (
    [processExecutionId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [processId]          BIGINT         NOT NULL,
    [start_at]           DATETIME2 (7)  NULL,
    [end_at]             DATETIME2 (7)  NULL,
    [status]             BIT            NULL,
    [success]            BIT            NULL,
    [result]             NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([processExecutionId] ASC)
);


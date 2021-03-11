CREATE TABLE [nothub].[applications] (
    [id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [name]       NVARCHAR (250) NOT NULL,
    [api_key]    NVARCHAR (250) NOT NULL,
    [account_id] BIGINT         NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT FK_nothub_applications_account_id FOREIGN KEY ([account_id]) REFERENCES [nothub].[accounts] ([id]),
);


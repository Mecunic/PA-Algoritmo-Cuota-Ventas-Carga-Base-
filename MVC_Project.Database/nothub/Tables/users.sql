CREATE TABLE [nothub].[users] (
    [id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [uuid]       NVARCHAR (50)  NOT NULL,
    [email]      NVARCHAR (100) NOT NULL,
    [first_name] NVARCHAR (100) NOT NULL,
    [last_name]  NVARCHAR (100) NULL,
    [account_id] BIGINT         NOT NULL,
    [password]   NVARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT FK_nothub_users_account_id FOREIGN KEY ([account_id]) REFERENCES [nothub].[accounts] ([id])
);


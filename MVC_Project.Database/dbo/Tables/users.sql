CREATE TABLE [dbo].[users] (
    [UserId]              INT            IDENTITY (1, 1) NOT NULL,
    [first_name]          NVARCHAR (255) NOT NULL,
    [email]               NVARCHAR (255) NOT NULL,
    [password]            NVARCHAR (255) NOT NULL,
    [created_at]          DATETIME2 (7)  NOT NULL,
    [updated_at]          DATETIME2 (7)  NOT NULL,
    [removed_at]          DATETIME2 (7)  NULL,
    [RoleId]              INT            NULL,
    [last_name]           NVARCHAR (255) NULL,
    [uuid]                NVARCHAR (50)  NOT NULL,
    [status]              BIT            NULL,
    [token]               NVARCHAR (255) NULL,
    [expira_token]        DATETIME2 (7)  NULL,
    [apikey]              NVARCHAR (255) NULL,
    [expira_apikey]       DATETIME2 (7)  NULL,
    [last_login_at]       DATETIME2 (7)  NULL,
    [password_expiration] DATETIME2 (7)  NULL,
    [username]            NVARCHAR (100) NULL,
    [language]            NVARCHAR (20)  NULL,
    [employee_identifier] NVARCHAR (50)  NULL,
    [mobile_number]       NVARCHAR (50)  NULL,
    [profile]             NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK2C1C7C0518C473ED] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[roles] ([RoleId])
);


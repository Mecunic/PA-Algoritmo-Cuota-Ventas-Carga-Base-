CREATE TABLE [dbo].[roles] (
    [RoleId]      INT            IDENTITY (1, 1) NOT NULL,
    [code]        NVARCHAR (255) NOT NULL,
    [name]        NVARCHAR (255) NOT NULL,
    [description] NVARCHAR (255) NOT NULL,
    [created_at]  DATETIME2 (7)  NOT NULL,
    [updated_at]  DATETIME2 (7)  NOT NULL,
    [removed_at]  DATETIME2 (7)  NULL,
    [uuid]        NVARCHAR (50)  NOT NULL,
    [status]      BIT            NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);


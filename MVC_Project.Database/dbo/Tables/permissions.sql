CREATE TABLE [dbo].[permissions] (
    [PermissionId] INT            IDENTITY (1, 1) NOT NULL,
    [description]  NVARCHAR (255) NOT NULL,
    [controller]   NVARCHAR (255) NOT NULL,
    [action]       NVARCHAR (255) NOT NULL,
    [created_at]   DATETIME2 (7)  NOT NULL,
    [updated_at]   DATETIME2 (7)  NOT NULL,
    [removed_at]   DATETIME2 (7)  NULL,
    [module]       VARCHAR (100)  NULL,
    PRIMARY KEY CLUSTERED ([PermissionId] ASC)
);


CREATE TABLE [dbo].[permission_role] (
    [PermissionRoleId] INT IDENTITY (1, 1) NOT NULL,
    [RoleId]           INT NOT NULL,
    [PermissionId]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([PermissionRoleId] ASC),
    CONSTRAINT [FK716BFA8A18C473ED] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[roles] ([RoleId]),
    CONSTRAINT [FK716BFA8ADDEA5583] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[permissions] ([PermissionId])
);


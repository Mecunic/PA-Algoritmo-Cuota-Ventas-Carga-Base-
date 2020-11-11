CREATE TABLE [dbo].[permission_user] (
    [UserId]       INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT [FKDC5280A9861EBAD5] FOREIGN KEY ([UserId]) REFERENCES [dbo].[users] ([UserId]),
    CONSTRAINT [FKDC5280A9DDEA5583] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[permissions] ([PermissionId])
);


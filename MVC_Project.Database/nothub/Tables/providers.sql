CREATE TABLE [nothub].[providers] (
    [id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [code]          NVARCHAR (100) NOT NULL,
    [name]          NVARCHAR (250) NULL,
    [type]          NVARCHAR (50)  NULL,
    [assembly]      NVARCHAR (250) NULL,
    [service_class] NVARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


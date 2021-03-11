CREATE TABLE [nothub].[notifications] (
    [id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [application_id] BIGINT         NOT NULL,
    [provider_id]    BIGINT         NOT NULL,
    [type]           NVARCHAR (100) NOT NULL,
    [template_id]    BIGINT         NULL,
    [recipient]      NVARCHAR (100) NOT NULL,
    [created_at]     DATETIME       NULL,
    [sent_date]      DATETIME       NULL,
    [custom_params]  NVARCHAR (MAX) NULL,
    [uuid]           NVARCHAR (100) NULL,
    [delivered]      BIT            DEFAULT ((0)) NOT NULL,
    [result]         NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


﻿CREATE TABLE [nothub].[notification_message] (
    [id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [uuid]            NVARCHAR (50)  NOT NULL,
    [application_id]  BIGINT         NOT NULL,
    [sg_message_id]   NVARCHAR (255) NULL,
    [processed]       DATETIME       NULL,
    [dropped]         DATETIME       NULL,
    [delivered]       DATETIME       NULL,
    [bounced]         DATETIME       NULL,
    [open]            DATETIME       NULL,
    [click]           DATETIME       NULL,
    [spamreport]      DATETIME       NULL,
    [unsubscribe]     DATETIME       NULL,
    [useragent]       NVARCHAR (255) NULL,
    [ip_address]      NVARCHAR (50)  NULL,
    [bounced_reason]  NVARCHAR (MAX) NULL,
    [dropped_reason]  NVARCHAR (MAX) NULL,
    [created_at]      DATETIME       NULL,
    [updated_at]      DATETIME       NULL,
    [smtp_id]         NVARCHAR (255) NULL,
    [url]             NVARCHAR (MAX) NULL,
    [custom_param1]   NVARCHAR (MAX) NULL,
    [custom_param2]   NVARCHAR (MAX) NULL,
    [custom_param3]   NVARCHAR (MAX) NULL,
    [custom_param4]   NVARCHAR (MAX) NULL,
    [custom_param5]   NVARCHAR (MAX) NULL,
    [custom_param6]   NVARCHAR (MAX) NULL,
    [custom_param7]   NVARCHAR (MAX) NULL,
    [custom_param8]   NVARCHAR (MAX) NULL,
    [custom_param9]   NVARCHAR (MAX) NULL,
    [custom_param10]  NVARCHAR (MAX) NULL,
    [receptor_sent]   NVARCHAR (255) NULL,
    [device_sent]     NVARCHAR (255) NULL,
    [phone_sent]      NVARCHAR (255) NULL,
    [provider_id]     BIGINT         NULL,
    [sender_name]     NVARCHAR (255) NULL,
    [subject]         NVARCHAR (255) NULL,
    [notification_id] BIGINT         NULL,
    [email_sent]      NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT FK_nothub_notification_message_application_id FOREIGN KEY ([application_id]) REFERENCES [nothub].[applications] ([id]),
    CONSTRAINT FK_nothub_notification_message_notification_id FOREIGN KEY ([notification_id]) REFERENCES [nothub].[notifications] ([id])
);


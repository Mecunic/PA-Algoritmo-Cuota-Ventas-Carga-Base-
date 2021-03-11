CREATE TABLE [nothub].[templates] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [uuid]           NVARCHAR (50)  NOT NULL,
    [name]           NVARCHAR (255) NOT NULL,
    [description]    NVARCHAR (MAX) NULL,
    [sender]         NVARCHAR (255) NOT NULL,
    [from_name]      NVARCHAR (255) NOT NULL,
    [reply_to_email] NVARCHAR (255) NULL,
    [cc]             NVARCHAR (255) NULL,
    [bcc]            NVARCHAR (255) NULL,
    [subject]        NVARCHAR (255) NOT NULL,
    [body_text]      NVARCHAR (MAX) NOT NULL,
    [body_html]      NVARCHAR (MAX) NOT NULL,
    [status]         BIT            DEFAULT ((1)) NOT NULL,
    [application_id] BIGINT         NOT NULL,
    [event_code]     NVARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


CREATE TABLE [calendar].[event_categories] (
    [eventCalendarId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [name]            VARCHAR (255) NULL,
    [bgcolor]         VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([eventCalendarId] ASC)
);


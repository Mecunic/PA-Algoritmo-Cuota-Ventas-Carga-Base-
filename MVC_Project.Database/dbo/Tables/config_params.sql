CREATE TABLE [dbo].[config_params] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [name]        VARCHAR (100) NOT NULL,
    [value]       VARCHAR (100) NOT NULL,
    [description] VARCHAR (255) NULL
);


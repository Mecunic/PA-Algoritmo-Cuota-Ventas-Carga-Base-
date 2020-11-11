CREATE TABLE [dbo].[payments] (
    [PaymentId]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [creation_date]      DATETIME2 (7)  NOT NULL,
    [order_id]           NVARCHAR (100) NOT NULL,
    [status]             NVARCHAR (50)  NOT NULL,
    [confirmation_date]  DATETIME2 (7)  NULL,
    [providerId]         NVARCHAR (100) NULL,
    [UserId]             BIGINT         NULL,
    [amount]             DECIMAL (18)   NULL,
    [authorization_code] NVARCHAR (100) NULL,
    [due_date]           DATETIME2 (7)  NULL,
    [log_data]           NVARCHAR (MAX) NULL,
    [method]             NVARCHAR (50)  NULL,
    [transaction_type]   NVARCHAR (100) NULL,
    [confirmation_email] NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([PaymentId] ASC)
);


CREATE TABLE [dbo].[payment_applications] (
    [paymentApplicationId]  INT            IDENTITY (1, 1) NOT NULL,
    [app_key]               NVARCHAR (50)  NOT NULL,
    [name]                  NVARCHAR (255) NULL,
    [active]                BIT            NULL,
    [PublicKey]             VARCHAR (255)  NULL,
    [PrivateKey]            VARCHAR (255)  NULL,
    [MerchantId]            VARCHAR (255)  NULL,
    [DashboardURL]          VARCHAR (255)  NULL,
    [SecureVerificationURL] VARCHAR (255)  NULL,
    [ClientId]              VARCHAR (255)  NULL,
    [UserId]                INT            NULL,
    [ReturnURL]             VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([paymentApplicationId] ASC)
);


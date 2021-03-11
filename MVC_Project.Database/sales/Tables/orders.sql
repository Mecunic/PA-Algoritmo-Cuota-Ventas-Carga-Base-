CREATE TABLE [sales].[orders] (
    [order_id]      INT     IDENTITY (1, 1) NOT NULL,
    [customer_id]   INT     NULL,
    [order_status]  TINYINT NOT NULL,
    [order_date]    DATE    NOT NULL,
    [required_date] DATE    NOT NULL,
    [shipped_date]  DATE    NULL,
    [store_id]      INT     NOT NULL,
    [staff_id]      INT     NOT NULL,
    PRIMARY KEY CLUSTERED ([order_id] ASC),
    CONSTRAINT FK_sales_orders_customer_id FOREIGN KEY ([customer_id]) REFERENCES [sales].[customers] ([customer_id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_sales_orders_staff_id FOREIGN KEY ([staff_id]) REFERENCES [sales].[staffs] ([staff_id]),
    CONSTRAINT FK_sales_orders_store_id FOREIGN KEY ([store_id]) REFERENCES [sales].[stores] ([store_id]) ON DELETE CASCADE ON UPDATE CASCADE
);


CREATE TABLE [sales].[order_items] (
    [order_id]   INT             NOT NULL,
    [item_id]    INT             NOT NULL,
    [product_id] INT             NOT NULL,
    [quantity]   INT             NOT NULL,
    [list_price] DECIMAL (10, 2) NOT NULL,
    [discount]   DECIMAL (4, 2)  DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([order_id] ASC, [item_id] ASC),
    CONSTRAINT FK_sales_order_items_order_id FOREIGN KEY ([order_id]) REFERENCES [sales].[orders] ([order_id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_sales_order_items_product_id FOREIGN KEY ([product_id]) REFERENCES [production].[products] ([product_id]) ON DELETE CASCADE ON UPDATE CASCADE
);


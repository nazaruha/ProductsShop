IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblBasket]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblBasket](
  [ClientId] [int] NOT NULL,
  [ProductId] [int] NOT NULL,
  [Count] [int] NOT NULL,

  CONSTRAINT [FK_tblBasket_tblClients] FOREIGN KEY([ClientId])
   REFERENCES [dbo].[tblClients] ([Id]),

  COSNTRAINT [FK_tblBasket_tblProducts] FOREIGN KEY([ProductId])
   REFERENCES [dbo].[tblProducts] ([Id])
);'
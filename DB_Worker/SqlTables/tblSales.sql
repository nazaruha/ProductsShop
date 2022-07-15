IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblSales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblSales](
  [ClientId] [int] NOT NULL,
  [ProductId] [int] NOT NULL,
  [Count] [int] NOT NULL,

  CONSTRAINT [FK_tblSales_tblClients] FOREIGN KEY([ClientId])
   REFERENCES [dbo].[tblClients] ([Id]),

  CONSTRAINT [FK_tblSales_tblProducts] FOREIGN KEY([ProductId])
   REFERENCES [dbo].[tblProducts] ([Id])
);'
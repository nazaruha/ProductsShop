IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblProducts]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblProducts](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Count] [int],
  [Price] [int] NOT NULL,
  [CategoryId] [int] NOT NULL,
  [SubCategoryId] [int] NOT NULL,

  CONSTRAINT [FK_tblProducts_tblCategories] FOREIGN KEY([CategoryId])
   REFERENCES [dbo].[tblCategories] ([Id]),

  CONSTRAINT [FK_tblProducts_tblSubCategories] FOREIGN KEY([SubCategoryId])
   REFERENCES [dbo].[tblSubCategories] ([Id])
);'
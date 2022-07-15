IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblSubCategories]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblSubCategories](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [CategoryId] [int] NOT NULL,
  [Name] [nvarchar](100) NOT NULL,

  CONSTRAINT [FK_tblSubCategories_tblCategories] FOREIGN KEY([CategoryId])
   REFERENCES [dbo].[tblCategories] ([Id])
);'
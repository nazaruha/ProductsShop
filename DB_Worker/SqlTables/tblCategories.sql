IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblCategories]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblCategories](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
);'
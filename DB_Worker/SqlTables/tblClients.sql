IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblClients]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblClients](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Phone] [nvarchar](50) NOT NULL,
);'
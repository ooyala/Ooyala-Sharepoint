USE [master]
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$(dbname)')
CREATE DATABASE $(dbname)
GO

USE $(dbname)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblUser]') AND type in (N'U'))
DROP TABLE [dbo].[tblUser]
GO

CREATE TABLE [dbo].[tblUser](
	[PartnerCode] [nvarchar](50) NULL,
	[SecretKey] [nvarchar](50) NULL,
	[APIKey] [nvarchar](50) NULL,
	[UserID] [nvarchar](50) NULL
) 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateOrModifyAPISettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateOrModifyAPISettings]
GO

CREATE PROCEDURE [dbo].[CreateOrModifyAPISettings](@PartnerCode varchar(100), @APIKey varchar(100), @SecretKey varchar(100), @UserID varchar(50))	
AS
BEGIN
	IF EXISTS(SELECT 1 FROM tblUser WITH(NOLOCK) WHERE UserID=@UserID)
	BEGIN
			UPDATE tblUser SET PartnerCode=@PartnerCode,
				SecretKey=@SecretKey, APIKey=@APIKey 
			WHERE UserID=@UserID
    END
    ELSE
    BEGIN
    INSERT INTO tblUser(PartnerCode,SecretKey,APIKey,UserID) 
            Values (@PartnerCode,@SecretKey,@APIKey,@UserID) 
	END
END

GO

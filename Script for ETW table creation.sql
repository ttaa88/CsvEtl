USE [ITCOperational]
GO


IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'etw' ) 
    EXEC('CREATE SCHEMA [etw] AUTHORIZATION [dbo]');

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'etw' 
                 AND  TABLE_NAME = 'DispatchEvent'))
BEGIN

DROP TABLE etw.DispatchEvent

END

CREATE TABLE [etw].[DispatchEvent](
	[DispatchEventId] int IDENTITY(1,1) PRIMARY KEY,
	[EventName] [varchar](max) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[EventId] [int] NULL
) 

GO

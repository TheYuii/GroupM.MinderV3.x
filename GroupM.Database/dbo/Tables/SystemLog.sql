CREATE TABLE [dbo].[SystemLog] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [LoginName]    VARCHAR (100) NULL,
    [ComputerName] VARCHAR (50)  NULL,
    [ScreenName]   VARCHAR (100) NULL,
    [Action]       VARCHAR (100) NULL,
    [Description]  VARCHAR (500) NULL,
    [CreateDate]   DATETIME      CONSTRAINT [DF_Media_Log_CreateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Media_Log] PRIMARY KEY CLUSTERED ([ID] ASC)
);


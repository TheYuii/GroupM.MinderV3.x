CREATE TABLE [dbo].[UserScreen] (
    [UserScreenId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserScreenName] NVARCHAR (225) NOT NULL,
    [CreateBy]       NVARCHAR (50)  NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF_UserScreen_CreateDate] DEFAULT (getdate()) NULL,
    [UpdateBy]       NVARCHAR (50)  NULL,
    [UpdateDate]     DATETIME       CONSTRAINT [DF_UserScreen_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserScreen] PRIMARY KEY CLUSTERED ([UserScreenName] ASC)
);




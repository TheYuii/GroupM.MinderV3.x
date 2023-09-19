CREATE TABLE [dbo].[UserGroupScreenMapping] (
    [UserGroupScreenMappingID] INT           IDENTITY (1, 1) NOT NULL,
    [UserScreenID]             INT           NOT NULL,
    [UserGroupID]              INT           NOT NULL,
    [CreateBy]                 NVARCHAR (50) NULL,
    [CreateDate]               DATETIME      CONSTRAINT [DF_UserGroupScreenMapping_CreateDate] DEFAULT (getdate()) NULL,
    [UpdateBy]                 NVARCHAR (50) NULL,
    [UpdateDate]               DATETIME      CONSTRAINT [DF_UserGroupScreenMapping_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserGroupScreenMapping] PRIMARY KEY CLUSTERED ([UserGroupScreenMappingID] ASC)
);


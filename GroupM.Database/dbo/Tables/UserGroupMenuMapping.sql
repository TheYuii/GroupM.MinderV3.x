CREATE TABLE [dbo].[UserGroupMenuMapping] (
    [UserGroupMenuMappingID] INT           IDENTITY (1, 1) NOT NULL,
    [UserMenuID]             INT           NOT NULL,
    [UserGroupID]            INT           NOT NULL,
    [CreateBy]               NVARCHAR (50) NULL,
    [CreateDate]             DATETIME      CONSTRAINT [DF_UserGroupMenuMapping_CreateDate] DEFAULT (getdate()) NULL,
    [UpdateBy]               NVARCHAR (50) NULL,
    [UpdateDate]             DATETIME      CONSTRAINT [DF_UserGroupMenuMapping_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserGroupMenuMapping] PRIMARY KEY CLUSTERED ([UserGroupMenuMappingID] ASC)
);


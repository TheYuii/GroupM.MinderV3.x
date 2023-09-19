CREATE TABLE [dbo].[UserGroupMapping] (
    [UserGroupMappingID] INT           IDENTITY (1, 1) NOT NULL,
    [UserProfileID]      INT           NOT NULL,
    [UserGroupID]        INT           NOT NULL,
    [CreateBy]           NVARCHAR (50) NOT NULL,
    [CreateDate]         DATETIME      CONSTRAINT [DF_UserGroupMapping_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateBy]           NVARCHAR (50) NULL,
    [UpdateDate]         DATETIME      CONSTRAINT [DF_UserGroupMapping_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserGroupMapping] PRIMARY KEY CLUSTERED ([UserProfileID] ASC, [UserGroupID] ASC)
);




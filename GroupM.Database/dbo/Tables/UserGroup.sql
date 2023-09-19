CREATE TABLE [dbo].[UserGroup] (
    [UserGroupID]   INT            IDENTITY (1, 1) NOT NULL,
    [UserGroupName] NVARCHAR (100) NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [IsActive]      BIT            NOT NULL,
    [CreateBy]      NVARCHAR (50)  CONSTRAINT [DF_UserGroup_CreateBy] DEFAULT ((0)) NOT NULL,
    [CreateDate]    DATETIME       CONSTRAINT [DF_UserGroup_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateBy]      NVARCHAR (50)  NULL,
    [UpdateDate]    DATETIME       CONSTRAINT [DF_UserGroup_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED ([UserGroupName] ASC)
);




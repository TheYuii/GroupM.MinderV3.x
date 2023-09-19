CREATE TABLE [dbo].[UserMenu] (
    [UserMenuId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserMenuName] NVARCHAR (225) NOT NULL,
    [CreateBy]     NVARCHAR (50)  NOT NULL,
    [CreateDate]   DATETIME       CONSTRAINT [DF_UserMenu_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateBy]     NVARCHAR (50)  NULL,
    [UpdateDate]   DATETIME       CONSTRAINT [DF_UserMenu_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserMenu] PRIMARY KEY CLUSTERED ([UserMenuName] ASC)
);




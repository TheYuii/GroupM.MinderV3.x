CREATE TABLE [dbo].[UserProfile] (
    [UserProfileID] INT            IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (100) NOT NULL,
    [Password]      NVARCHAR (100) NULL,
    [DisplayName]   NVARCHAR (100) NULL,
    [FirstName]     NVARCHAR (100) NULL,
    [LastName]      NVARCHAR (100) NULL,
    [Domain]        NVARCHAR (100) NOT NULL,
    [IsActive]      BIT            NULL,
    [CreateBy]      NVARCHAR (50)  CONSTRAINT [DF_UserProfile_CreateBy] DEFAULT ((0)) NOT NULL,
    [CreateDate]    DATETIME       CONSTRAINT [DF_UserProfile_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateBy]      NVARCHAR (50)  NULL,
    [UpdateDate]    DATETIME       CONSTRAINT [DF_UserProfile_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([UserName] ASC)
);






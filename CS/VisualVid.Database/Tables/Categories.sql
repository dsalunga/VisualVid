CREATE TABLE [dbo].[Categories] (
    [CategoryID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (255) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);


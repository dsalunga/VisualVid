CREATE TABLE [dbo].[Countries] (
    [CountryCode] INT            NOT NULL,
    [Name]        NVARCHAR (255) NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryCode] ASC)
);


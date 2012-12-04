CREATE TABLE [dbo].[Members] (
    [UserId]      UNIQUEIDENTIFIER CONSTRAINT [DF_Members_UserId] DEFAULT (newid()) NOT NULL,
    [Gender]      BIT              NULL,
    [CountryCode] INT              NULL,
    [BirthDate]   DATETIME         NULL,
    [Watched]     INT              NULL,
    CONSTRAINT [PK_Members_1] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


CREATE TABLE [dbo].[Videos] (
    [Title]             NVARCHAR (255)   NULL,
    [Description]       NVARCHAR (4000)  NULL,
    [DateAdded]         DATETIME         NULL,
    [Tags]              NVARCHAR (4000)  NULL,
    [Views]             INT              NULL,
    [Ratings]           INT              NULL,
    [RatingTicks]       INT              NULL,
    [CategoryID]        INT              NULL,
    [Length]            INT              NULL,
    [Thumbnail]         IMAGE            NULL,
    [Filename]          NVARCHAR (255)   NULL,
    [IsActive]          BIT              NULL,
    [OriginalExtension] NVARCHAR (64)    NULL,
    [UserId]            UNIQUEIDENTIFIER NULL,
    [Pending]           BIT              NULL,
    [VideoId]           UNIQUEIDENTIFIER CONSTRAINT [DF_Videos_VideoId] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Videos_1] PRIMARY KEY CLUSTERED ([VideoId] ASC)
);


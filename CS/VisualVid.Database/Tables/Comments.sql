CREATE TABLE [dbo].[Comments] (
    [CommentID]   INT              IDENTITY (1, 1) NOT NULL,
    [DatePosted]  DATETIME         NULL,
    [Content]     NTEXT            NULL,
    [ReplyFromID] INT              NULL,
    [UserId]      UNIQUEIDENTIFIER NULL,
    [VideoId]     UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Comments_1] PRIMARY KEY CLUSTERED ([CommentID] ASC)
);


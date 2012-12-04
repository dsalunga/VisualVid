CREATE PROCEDURE dbo.UPDATE_Comments 
	(
		@Content ntext = null,
		@UserId uniqueidentifier = null,
		@ReplyFromId int = null,
		@VideoId uniqueidentifier = null
	)
AS
	SET NOCOUNT ON
	
	INSERT INTO Comments
	                         (Content, UserId, VideoId, ReplyFromID, DatePosted)
	VALUES        (@Content,@UserId,@VideoId,@ReplyFromId, GETDATE())
	
	RETURN

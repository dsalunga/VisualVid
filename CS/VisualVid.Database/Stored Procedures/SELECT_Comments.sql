CREATE PROCEDURE dbo.SELECT_Comments 
	(
		@VideoId uniqueidentifier = null
	)
AS
	SET NOCOUNT ON
	
	SELECT        C.DatePosted, C.Content, U.UserName, C.UserId, C.CommentID
	FROM            Comments AS C INNER JOIN
	                         aspnet_Users AS U ON C.UserId = U.UserId
	WHERE VideoId=@VideoId
	
	RETURN

CREATE PROCEDURE dbo.SELECT_Videos_Pending 
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
	SET NOCOUNT ON
	
	SELECT        V.VideoId, V.OriginalExtension, V.UserId, M.Email, V.Title
	FROM            Videos AS V INNER JOIN
	                         aspnet_Membership AS M ON V.UserId = M.UserId
	WHERE        (V.Pending = 1)
	
	RETURN

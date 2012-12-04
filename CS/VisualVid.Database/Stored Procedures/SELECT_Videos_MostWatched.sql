CREATE PROCEDURE dbo.SELECT_Videos_MostWatched
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
	SET NOCOUNT ON
	
	SELECT TOP 10 V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.Length, V.Thumbnail, V.Filename, V.UserId, V.VideoId, U.UserName FROM Videos AS V INNER JOIN aspnet_Users AS U ON V.UserId = U.UserId WHERE (V.IsActive = 1) ORDER BY V.Views DESC, V.DateAdded DESC
	RETURN
CREATE PROCEDURE dbo.SELECT_Videos
	(
		@UserId uniqueidentifier = null,
		@VideoId uniqueidentifier = null,
		@CategoryID int = null,
		@IsActive bit = null,
		@UpdateViews bit = null,
		@Keyword nvarchar(255) = null,
		@Sort nvarchar(256) = null
	)
AS
	SET NOCOUNT ON

	if(@IsActive is not null)
		begin
			if(@Keyword is not null)
				begin
					if(@Sort is not null)
						begin
							if(@Sort = 'DateAdded')
								begin
									SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
															 V.OriginalExtension, V.UserId, V.Pending, U.UserName
									FROM            Videos AS V INNER JOIN
															 aspnet_Users AS U ON V.UserId = U.UserId
									WHERE        (V.IsActive = @IsActive) AND (V.Tags LIKE '%' + @Keyword + '%')
									ORDER BY V.DateAdded DESC
								end
							else if(@Sort = 'Views')
								begin
									SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
															 V.OriginalExtension, V.UserId, V.Pending, U.UserName
									FROM            Videos AS V INNER JOIN
															 aspnet_Users AS U ON V.UserId = U.UserId
									WHERE        (V.IsActive = @IsActive) AND (V.Tags LIKE '%' + @Keyword + '%')
									ORDER BY V.Views DESC
								end
						end
					else
						begin
							SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
													 V.OriginalExtension, V.UserId, V.Pending, U.UserName
							FROM            Videos AS V INNER JOIN
													 aspnet_Users AS U ON V.UserId = U.UserId
							WHERE        (V.IsActive = @IsActive) AND (V.Tags LIKE '%' + @Keyword + '%')
							ORDER BY V.DateAdded DESC
						end
				end
			else if(@UserId is not null)
				begin
					SELECT V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, V.OriginalExtension, V.UserId, V.Pending, U.UserName 
					FROM Videos AS V INNER JOIN aspnet_Users AS U ON V.UserId = U.UserId WHERE (V.UserId = @UserId) AND (V.IsActive = @IsActive) ORDER BY V.DateAdded DESC
				end
			else if(@CategoryID is not null)
				begin
					SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
					                         V.OriginalExtension, V.UserId, V.Pending, U.UserName
					FROM            Videos AS V INNER JOIN
					                         aspnet_Users AS U ON V.UserId = U.UserId
					WHERE        (V.IsActive = @IsActive) AND V.CategoryID = @CategoryID
				end
			else
				begin
					if(@Sort is not null)
						begin
							if(@Sort = 'DateAdded')
								begin
									SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
												 V.OriginalExtension, V.UserId, V.Pending, U.UserName
									FROM            Videos AS V INNER JOIN
														 aspnet_Users AS U ON V.UserId = U.UserId
									WHERE        (V.IsActive = @IsActive)
									ORDER BY V.DateAdded DESC
								end
							else if(@Sort = 'Views')
								begin
									SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
									                         V.OriginalExtension, V.UserId, V.Pending, U.UserName
									FROM            Videos AS V INNER JOIN
									                         aspnet_Users AS U ON V.UserId = U.UserId
									WHERE        (V.IsActive = @IsActive)
									ORDER BY V.Views DESC
								end
						end
					else
						begin
							SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
												 V.OriginalExtension, V.UserId, V.Pending, U.UserName
							FROM            Videos AS V INNER JOIN
												 aspnet_Users AS U ON V.UserId = U.UserId
							WHERE        (V.IsActive = @IsActive)
							ORDER BY V.DateAdded DESC
						end
					
				end
		end
	else
		begin
			if(@VideoId is not null)
				begin
					if(@UpdateViews is not null)
						begin
							UPDATE Videos SET Views = Views + 1 WHERE VideoId=@VideoId
						end
						
					SELECT        V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, 
					                         V.OriginalExtension, V.UserId, V.Pending, U.UserName
					FROM            Videos AS V INNER JOIN
					                         aspnet_Users AS U ON V.UserId = U.UserId
					WHERE        (V.VideoId = @VideoId)
				end
			else if(@UserId is not null)
				begin
					SELECT V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, V.OriginalExtension, V.UserId, V.Pending, U.UserName FROM Videos AS V INNER JOIN aspnet_Users AS U ON V.UserId = U.UserId WHERE (V.UserId = @UserId) ORDER BY V.DateAdded DESC
				end
			else if(@CategoryID is not null)
				begin
					SELECT * FROM Videos WHERE CategoryID = @CategoryID
				end
			else
				begin
					SELECT V.VideoId, V.Title, V.Description, V.DateAdded, V.Tags, V.Views, V.Ratings, V.RatingTicks, V.CategoryID, V.Length, V.Thumbnail, V.IsActive, V.OriginalExtension, V.UserId, V.Pending, U.UserName FROM Videos AS V INNER JOIN aspnet_Users AS U ON V.UserId = U.UserId ORDER BY V.DateAdded DESC
				end
		end	
	
	RETURN

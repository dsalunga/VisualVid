CREATE PROCEDURE dbo.UPDATE_Videos 
	(
		@VideoId uniqueidentifier = null,
		@UserId uniqueidentifier = null,
		@Title nvarchar(255) = null,
		@Description  nvarchar(4000) = null,
		@Tags nvarchar(4000) = null,
		@CategoryID int = null,
		@IsActive bit = 0,
		@Views int = 0,
		@Ratings int = 0,
		@RatingTicks int = 0,
		@Length int = 0,
		@Thumbnail image = null,
		@OriginalExtension nvarchar(64) = null,
		@Pending bit = 1
	)
AS
	SET NOCOUNT ON
	declare @@VideoId uniqueidentifier
	
	if(@VideoId is not null)
		begin
			/* Update */
			UPDATE       Videos
			SET                Title = @Title, Description = @Description, Tags = @Tags, CategoryID = @CategoryID
			WHERE        (VideoId = @VideoId)
			
			set @@VideoId = @VideoId
		end
	else
		begin
			set @@VideoId = NEWID()
			
			INSERT INTO Videos
			                         (Title, Description, Tags, CategoryID, IsActive, DateAdded, Views, Ratings, RatingTicks, Length, Thumbnail, OriginalExtension, UserId, Pending, VideoId)
			VALUES        (@Title,@Description,@Tags,@CategoryID,@IsActive, 
			                         GETDATE(),@Views,@Ratings,@RatingTicks,@Length,@Thumbnail,@OriginalExtension,@UserId,@Pending, @@VideoId)
		end
	
	select @@VideoId
	
	RETURN

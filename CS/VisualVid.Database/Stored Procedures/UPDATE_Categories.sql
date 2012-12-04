CREATE PROCEDURE dbo.UPDATE_Categories 
	(
		@CategoryID int = null,
		@Name nvarchar(255) = null
	)
AS
	SET NOCOUNT ON
	declare @@CategoryID int
	
	if(@CategoryID is not null)
		begin
			/* UPDATE */
			UPDATE       Categories
			SET                Name = @Name
			WHERE        (CategoryID = @CategoryID)
			
			set @@CategoryID = @CategoryID
		end
	else
		begin
			INSERT INTO Categories
			                         (Name)
			VALUES        (@Name)
			
			set @@CategoryID = @@IDENTITY
		end
		
	SELECT @@CategoryID
	
	RETURN

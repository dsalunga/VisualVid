CREATE PROCEDURE dbo.SELECT_Categories 
	(
		@CategoryID int = null
	)
AS
	SET NOCOUNT ON
	
	if(@CategoryID is not null)
		begin
			SELECT * FROM Categories WHERE CategoryID=@CategoryID
		end
	else
		begin
			SELECT * FROM Categories
			ORDER BY [Name]
		end
	
	RETURN

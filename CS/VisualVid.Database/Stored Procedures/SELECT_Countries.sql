CREATE PROCEDURE dbo.SELECT_Countries 
	(
		@CountryCode int = null
	)
AS
	SET NOCOUNT ON
	
	if(@CountryCode is not null)
		begin
			SELECT * FROM Countries WHERE CountryCode=@CountryCode
		end
	else
		begin
			SELECT * FROM Countries
			ORDER BY CountryCode
		end
	
	RETURN

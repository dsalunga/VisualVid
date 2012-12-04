CREATE PROCEDURE dbo.UPDATE_Members 
	(
		@UserId uniqueidentifier = null,
		@Gender bit = null,
		@CountryCode int = null,
		@BirthDate datetime = null
	)
AS
	SET NOCOUNT ON
	declare @@UserId uniqueidentifier
	
	set @@UserId = (SELECT UserId FROM Members WHERE UserId=@UserId)
	
	if(@@UserId is not null)
		begin
			/* Update */
			UPDATE       Members
			SET                Gender = @Gender, CountryCode = @CountryCode, BirthDate = @BirthDate
			WHERE        (UserId = @UserId)
		end
	else
		begin
			INSERT INTO Members
			                         (UserId, Gender, CountryCode, BirthDate, Watched)
			VALUES        (@UserId,@Gender,@CountryCode,@BirthDate, 0)
		end
	RETURN

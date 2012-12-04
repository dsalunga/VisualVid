CREATE PROCEDURE dbo.SELECT_Members 
	(
		@UserId uniqueidentifier = null
	)
AS
	SET NOCOUNT ON
	
	if(@UserId is not null)	
		begin
			SELECT        M.UserId, M.Gender, M.CountryCode, M.BirthDate, M.Watched, aM.LastLoginDate, aM.CreateDate, C.Name AS CountryName, U.UserName
			FROM            Members AS M INNER JOIN
			                         Countries AS C ON M.CountryCode = C.CountryCode INNER JOIN
			                         aspnet_Membership AS aM ON M.UserId = aM.UserId INNER JOIN
			                         aspnet_Users AS U ON M.UserId = U.UserId
			WHERE        (M.UserId = @UserId)
		end
	else
		begin
			SELECT        M.UserId, M.Gender, M.BirthDate, aM.Email, aM.IsApproved, aM.IsLockedOut, aM.LastLoginDate, C.Name AS CountryName, aU.UserName,
			                             (SELECT        COUNT(*) AS Videos
			                               FROM            Videos
			                               WHERE        (UserId = M.UserId)) AS Videos, aU.LastActivityDate, aM.CreateDate, M.Watched
			FROM            aspnet_UsersInRoles AS UR INNER JOIN
			                         aspnet_Roles AS R ON UR.RoleId = R.RoleId INNER JOIN
			                         Members AS M INNER JOIN
			                         aspnet_Membership AS aM ON M.UserId = aM.UserId ON UR.UserId = M.UserId INNER JOIN
			                         Countries AS C ON M.CountryCode = C.CountryCode INNER JOIN
			                         aspnet_Users AS aU ON UR.UserId = aU.UserId AND aM.UserId = aU.UserId
			WHERE        (R.RoleName = 'Members')
		end
	
	RETURN

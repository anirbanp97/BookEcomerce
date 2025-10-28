USE BookstoreDB;
GO

-- ==============================================
-- USER MANAGEMENT STORED PROCEDURES
-- ==============================================

IF OBJECT_ID('sp_RegisterUser') IS NOT NULL DROP PROCEDURE sp_RegisterUser;
GO
CREATE PROCEDURE sp_RegisterUser
    @UserName NVARCHAR(100),
    @Email NVARCHAR(200),
    @Password NVARCHAR(200),
    @RoleName NVARCHAR(50) = 'Customer',
    @OutUserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleId INT;
    SELECT @RoleId = RoleId FROM Roles WHERE RoleName = @RoleName;

    IF @RoleId IS NULL
        RAISERROR('Invalid role name', 16, 1);

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        RAISERROR('Email already registered', 16, 1);

    DECLARE @PwdHash VARBINARY(64);
    SET @PwdHash = HASHBYTES('SHA2_256', CONVERT(VARBINARY(MAX), @Password));

    INSERT INTO Users (UserName, Email, PasswordHash, RoleId)
    VALUES (@UserName, @Email, @PwdHash, @RoleId);

    SET @OutUserId = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID('sp_GetUserByEmail') IS NOT NULL DROP PROCEDURE sp_GetUserByEmail;
GO
CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserId, UserName, Email, PasswordHash, RoleId, CreatedAt
    FROM Users
    WHERE Email = @Email;
END
GO
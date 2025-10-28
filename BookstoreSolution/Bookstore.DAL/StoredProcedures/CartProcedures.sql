USE BookstoreDB;
GO

-- ==============================================
-- CART MANAGEMENT STORED PROCEDURES
-- ==============================================

IF OBJECT_ID('sp_AddToCart') IS NOT NULL DROP PROCEDURE sp_AddToCart;
GO
CREATE PROCEDURE sp_AddToCart
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CartId INT;
    SELECT @CartId = CartId FROM Carts WHERE UserId = @UserId;

    IF @CartId IS NULL
    BEGIN
        INSERT INTO Carts (UserId) VALUES (@UserId);
        SET @CartId = SCOPE_IDENTITY();
    END

    DECLARE @Price DECIMAL(10,2);
    SELECT @Price = Price FROM Books WHERE BookId = @BookId;

    IF EXISTS (SELECT 1 FROM CartItems WHERE CartId = @CartId AND BookId = @BookId)
    BEGIN
        UPDATE CartItems
        SET Quantity = Quantity + @Quantity
        WHERE CartId = @CartId AND BookId = @BookId;
    END
    ELSE
    BEGIN
        INSERT INTO CartItems (CartId, BookId, Quantity, UnitPrice)
        VALUES (@CartId, @BookId, @Quantity, @Price);
    END
END
GO

IF OBJECT_ID('sp_UpdateCartItem') IS NOT NULL DROP PROCEDURE sp_UpdateCartItem;
GO
CREATE PROCEDURE sp_UpdateCartItem
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CartId INT;
    SELECT @CartId = CartId FROM Carts WHERE UserId = @UserId;

    IF @Quantity <= 0
        DELETE FROM CartItems WHERE CartId = @CartId AND BookId = @BookId;
    ELSE
        UPDATE CartItems SET Quantity = @Quantity WHERE CartId = @CartId AND BookId = @BookId;
END
GO

IF OBJECT_ID('sp_RemoveCartItem') IS NOT NULL DROP PROCEDURE sp_RemoveCartItem;
GO
CREATE PROCEDURE sp_RemoveCartItem
    @UserId INT,
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CartId INT;
    SELECT @CartId = CartId FROM Carts WHERE UserId = @UserId;

    DELETE FROM CartItems WHERE CartId = @CartId AND BookId = @BookId;
END
GO

IF OBJECT_ID('sp_GetCartByUser') IS NOT NULL DROP PROCEDURE sp_GetCartByUser;
GO
CREATE PROCEDURE sp_GetCartByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ci.CartItemId, ci.BookId, b.Title, b.Author,
           ci.Quantity, ci.UnitPrice, (ci.Quantity * ci.UnitPrice) AS LineTotal
    FROM Carts c
    JOIN CartItems ci ON c.CartId = ci.CartId
    JOIN Books b ON ci.BookId = b.BookId
    WHERE c.UserId = @UserId;
END
GO
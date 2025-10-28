USE BookstoreDB;
GO

-- ==============================================
-- ORDER MANAGEMENT STORED PROCEDURES
-- ==============================================

-- TVP Type (Only once)
IF TYPE_ID(N'dbo.OrderItemType') IS NULL
    CREATE TYPE dbo.OrderItemType AS TABLE
    (
        BookId INT,
        Quantity INT,
        UnitPrice DECIMAL(10,2)
    );
GO

IF OBJECT_ID('sp_PlaceOrder') IS NOT NULL DROP PROCEDURE sp_PlaceOrder;
GO
CREATE PROCEDURE sp_PlaceOrder
    @UserId INT,
    @ShippingAddress NVARCHAR(400),
    @Items dbo.OrderItemType READONLY,
    @OutOrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Total DECIMAL(12,2) = 0;
        SELECT @Total = SUM(UnitPrice * Quantity) FROM @Items;

        INSERT INTO Orders (UserId, TotalAmount, Status, ShippingAddress)
        VALUES (@UserId, @Total, 'Placed', @ShippingAddress);
        SET @OutOrderId = SCOPE_IDENTITY();

        INSERT INTO OrderItems (OrderId, BookId, Quantity, UnitPrice)
        SELECT @OutOrderId, BookId, Quantity, UnitPrice FROM @Items;

        -- Decrease stock
        UPDATE b
        SET b.StockQuantity = b.StockQuantity - i.Quantity
        FROM Books b
        JOIN @Items i ON b.BookId = i.BookId;

        -- Clear user's cart
        DELETE ci
        FROM CartItems ci
        JOIN Carts c ON ci.CartId = c.CartId
        WHERE c.UserId = @UserId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

IF OBJECT_ID('sp_GetOrdersByUser') IS NOT NULL DROP PROCEDURE sp_GetOrdersByUser;
GO
CREATE PROCEDURE sp_GetOrdersByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT OrderId, UserId, OrderDate, TotalAmount, Status, ShippingAddress
    FROM Orders
    WHERE UserId = @UserId
    ORDER BY OrderDate DESC;
END
GO

IF OBJECT_ID('sp_GetOrderDetails') IS NOT NULL DROP PROCEDURE sp_GetOrderDetails;
GO
CREATE PROCEDURE sp_GetOrderDetails
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT o.OrderId, o.UserId, o.OrderDate, o.TotalAmount, o.Status, o.ShippingAddress,
           oi.OrderItemId, oi.BookId, b.Title, b.Author, oi.Quantity, oi.UnitPrice
    FROM Orders o
    JOIN OrderItems oi ON o.OrderId = oi.OrderId
    JOIN Books b ON oi.BookId = b.BookId
    WHERE o.OrderId = @OrderId;
END
GO
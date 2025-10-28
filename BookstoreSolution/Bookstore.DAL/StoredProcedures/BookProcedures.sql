USE BookstoreDB;
GO

-- ==============================================
-- BOOKS CRUD STORED PROCEDURES
-- ==============================================

IF OBJECT_ID('sp_AddBook') IS NOT NULL DROP PROCEDURE sp_AddBook;
GO
CREATE PROCEDURE sp_AddBook
    @Title NVARCHAR(200),
    @Author NVARCHAR(150),
    @ISBN NVARCHAR(50) = NULL,
    @Price DECIMAL(10,2),
    @StockQuantity INT,
    @OutBookId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Books (Title, Author, ISBN, Price, StockQuantity)
    VALUES (@Title, @Author, @ISBN, @Price, @StockQuantity);
    SET @OutBookId = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID('sp_UpdateBook') IS NOT NULL DROP PROCEDURE sp_UpdateBook;
GO
CREATE PROCEDURE sp_UpdateBook
    @BookId INT,
    @Title NVARCHAR(200),
    @Author NVARCHAR(150),
    @ISBN NVARCHAR(50) = NULL,
    @Price DECIMAL(10,2),
    @StockQuantity INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Books
    SET Title = @Title,
        Author = @Author,
        ISBN = @ISBN,
        Price = @Price,
        StockQuantity = @StockQuantity
    WHERE BookId = @BookId;
END
GO

IF OBJECT_ID('sp_DeleteBook') IS NOT NULL DROP PROCEDURE sp_DeleteBook;
GO
CREATE PROCEDURE sp_DeleteBook
    @BookId INT
AS
BEGIN
    DELETE FROM Books WHERE BookId = @BookId;
END
GO

IF OBJECT_ID('sp_GetBookById') IS NOT NULL DROP PROCEDURE sp_GetBookById;
GO
CREATE PROCEDURE sp_GetBookById
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT BookId, Title, Author, ISBN, Price, StockQuantity, CreatedAt
    FROM Books
    WHERE BookId = @BookId;
END
GO

IF OBJECT_ID('sp_GetBooks') IS NOT NULL DROP PROCEDURE sp_GetBooks;
GO
CREATE PROCEDURE sp_GetBooks
    @Search NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT BookId, Title, Author, ISBN, Price, StockQuantity, CreatedAt
    FROM Books
    WHERE (@Search IS NULL OR Title LIKE '%' + @Search + '%' OR Author LIKE '%' + @Search + '%')
    ORDER BY Title
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
CREATE PROCEDURE [dbo].[USPCreateBook]
    @BookName    NVARCHAR(150),
    @ReleaseDate DATE,
    @Rate       INT,
    @PageNumber INT,
    @AuthorIds BigIntArrayType READONLY
AS
BEGIN
    DECLARE @BookId BIGINT;

    INSERT INTO Book (Name, ReleaseDate, Rate, PageNumber)
    VALUES (@BookName, @ReleaseDate, @Rate, @PageNumber)

    SET @BookId = SCOPE_IDENTITY();

    INSERT INTO BookAuthor (BookId, AuthorId)
    SELECT @BookId, Item
    FROM @AuthorIds;

END

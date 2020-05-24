CREATE PROCEDURE [dbo].[USPUpdateBook]
    @BookId      BIGINT,
    @BookName    NVARCHAR(150),
    @ReleaseDate DATE,
    @Rate       INT,
    @PageNumber INT,
    @AuthorIds BigIntArrayType READONLY
AS
BEGIN
    UPDATE Book
    SET Name = @BookName,
        ReleaseDate = @ReleaseDate,
        Rate = @Rate,
        PageNumber = @PageNumber
    WHERE Id = @BookId

    MERGE BookAuthor ba USING @AuthorIds ids
    ON (ba.BookId = @BookId AND ba.AuthorId = ids.Item)
    WHEN NOT MATCHED BY TARGET
        THEN INSERT (BookId, AuthorId) VALUES (@BookId, ids.Item)
    WHEN NOT MATCHED BY SOURCE AND BookId = @BookId
        THEN DELETE;

END

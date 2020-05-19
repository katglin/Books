CREATE TABLE [dbo].[BookAuthor]
(
    [Id] BIGINT IDENTITY (1,1) NOT NULL, 

    [BookId] BIGINT NOT NULL, 
    [AuthorId] BIGINT NOT NULL, 

    CONSTRAINT PK_BookAuthor PRIMARY KEY (Id),
    CONSTRAINT FK_BookAuthor_BookId FOREIGN KEY (BookId) REFERENCES Book(Id),
    CONSTRAINT FK_BookAuthor_AuthorId FOREIGN KEY (AuthorId) REFERENCES Author(Id)
)

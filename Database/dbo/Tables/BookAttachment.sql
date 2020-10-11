CREATE TABLE [dbo].[BookAttachment]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1),
    [BookId] BIGINT NOT NULL,
    [FileS3Key] NVARCHAR(MAX) NOT NULL,
    [FileName] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT PK_BookAttachment PRIMARY KEY (Id),
    CONSTRAINT FK_BookAttachment_BookId FOREIGN KEY (BookId) REFERENCES Book(Id),
)

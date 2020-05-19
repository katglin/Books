CREATE TABLE [dbo].[Book]
(
    [Id] BIGINT IDENTITY (1,1) NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [ReleaseDate] DATE NOT NULL,
    [Rate] TINYINT NOT NULL, 
    [PageNumber] INT NOT NULL,

	CONSTRAINT PK_Book PRIMARY KEY (Id), 
)

DELETE FROM BookAuthor;
DELETE FROM Book;
DELETE FROM Author;

INSERT INTO Author (FirstName, LastName)
VALUES 
    ('Tom', 'Smitt'),
    ('Sansa', 'Stark'),
    ('Robert', 'Barateon'),
    ('John', 'Snow'),
    ('Peter', 'Pan'),
    ('Jack', 'London');

INSERT INTO Book (Name, ReleaseDate, Rate, PageNumber)
VALUES 
    ('Book 1', GETDATE(), 10, 365),
    ('Longer Book Name 2', '1996-07-26', 1, 1043),
    ('The Longest Name Of All The Books 3', '2010-01-14', 5, 15),
    ('Test Book 4', '2017-01-14', 3, 176),
    ('Test Book Name 5', '2019-06-06', 7, 534);

INSERT INTO BookAuthor (BookId, AuthorId)
VALUES 
    (1, 1),
    (1, 2),
    (1, 3),
    (2, 4),
    (3, 5),
    (4, 1),
    (4, 3),
    (5, 2),
    (5, 3);
using Dapper;
using Data.Extensions;
using DTO;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Data.Repositories
{
    public class BookRepository: RepositoryDapper<BookDTO, long>, IBookRepository
    {
        public BookDTO GetBook(long id)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"SELECT b.Id, b.Name, b.ReleaseDate, b.Rate, b.PageNumber, b.ImageS3Key, 
                                      att.Id, att.BookId, att.FileS3Key, att.FileName,
                                      a.Id, a.FirstName, a.LastName, ba.BookId, ba.AuthorId
                               FROM Book b 
                               LEFT JOIN BookAttachment att ON b.Id = att.BookId
                               JOIN BookAuthor ba ON b.Id = ba.BookId
                               JOIN Author a ON a.Id = ba.AuthorId
                               WHERE b.Id = {id}
                               ORDER BY b.Id DESC";

                var books = connection.Query<BookDTO, AttachmentDTO, AuthorDTO, BookDTO>(sql,
                    (book, attachment, author) =>
                    {
                        book.Authors.Add(author);
                        if (attachment != null)
                        {
                            book.Attachments.Add(attachment);
                        }
                        return book;
                    },
                    splitOn: "Id")
                    .GroupBy(b => b.Id)
                    .Select(group =>
                    {
                        var combinedBook = group.First();
                        combinedBook.Authors = group.Select(book => book.Authors.Single())
                            .OrderBy(a => a.FirstName + a.LastName).GroupBy(a => a.Id).Select(a => a.First()).ToList();
                        if (combinedBook.Attachments.Any())
                        {
                            combinedBook.Attachments = group.Select(book => book.Attachments.Single())
                                .OrderBy(a => a.FileName).GroupBy(a => a.Id).Select(a => a.First()).ToList();
                        }
                        return combinedBook;
                    });

                return books.FirstOrDefault();
            }
        }

        public IEnumerable<BookDTO> GetBooks()
        {
            using (var connection = ConnectionProvider())
            {
                string sql = @"SELECT b.Id, b.Name, b.ReleaseDate, b.Rate, b.PageNumber, b.ImageS3Key, 
                                      att.Id, att.BookId, att.FileS3Key, att.FileName,
                                      a.Id, a.FirstName, a.LastName, ba.BookId, ba.AuthorId
                               FROM Book b 
                               LEFT JOIN BookAttachment att ON b.Id = att.BookId
                               JOIN BookAuthor ba ON b.Id = ba.BookId
                               JOIN Author a ON a.Id = ba.AuthorId
                               ORDER BY b.Id DESC";

                var books = connection.Query<BookDTO, AttachmentDTO, AuthorDTO, BookDTO>(sql,
                    (book, attachment, author) =>
                    {
                        book.Authors.Add(author);
                        if (attachment != null)
                        {
                            book.Attachments.Add(attachment);
                        }
                        return book;
                    },
                    splitOn: "Id")
                    .GroupBy(b => b.Id)
                    .Select(group =>
                    {
                        var combinedBook = group.First();
                        combinedBook.Authors = group.Select(book => book.Authors.Single())
                            .OrderBy(a => a.FirstName + a.LastName).GroupBy(a => a.Id).Select(a => a.First()).ToList();
                        if (combinedBook.Attachments.Any())
                        {
                            combinedBook.Attachments = group.Select(book => book.Attachments.Single())
                                .OrderBy(a => a.FileName).GroupBy(a => a.Id).Select(a => a.First()).ToList();
                        }
                        return combinedBook;
                    });

                return books;
            }
        }

        public long CreateBook(BookDTO book)
        {
            using (var connection = ConnectionProvider())
            {
                var SP = "USPCreateBook";
                var queryParameters = new DynamicParameters();
                var ids = book.AuthorIds.AsDataTableParam();

                queryParameters.Add("@BookName", book.Name);
                queryParameters.Add("@ReleaseDate", book.ReleaseDate);
                queryParameters.Add("@Rate", book.Rate);
                queryParameters.Add("@PageNumber", book.PageNumber);
                queryParameters.Add("@AuthorIds", ids.AsTableValuedParameter("BigIntArrayType"));

                return connection.Query<long>(SP, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void UpdateBookTitle(int bookId, string bookTitleKey)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"UPDATE Book SET ImageS3Key='{bookTitleKey}' WHERE Id={bookId}";
                connection.Query(sql);
            }
        }

        public void UpdateBook(BookDTO book)
        {
            using (var connection = ConnectionProvider())
            {
                var SP = "USPUpdateBook";
                var queryParameters = new DynamicParameters();
                var ids = book.AuthorIds.AsDataTableParam();

                queryParameters.Add("@BookId", book.Id);
                queryParameters.Add("@BookName", book.Name);
                queryParameters.Add("@ReleaseDate", book.ReleaseDate);
                queryParameters.Add("@Rate", book.Rate);
                queryParameters.Add("@PageNumber", book.PageNumber);
                queryParameters.Add("@AuthorIds", ids.AsTableValuedParameter("BigIntArrayType"));

                connection.Query(SP, queryParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteBook(long id)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"BEGIN TRAN DeleteBookWithDeps
                                BEGIN TRY
                                    DELETE BookAuthor WHERE BookId = {id};
                                    DELETE BookAttachment WHERE BookId = {id}
                                    DELETE Book WHERE Id = {id}
                                    COMMIT TRAN DeleteBookWithDeps
                                END TRY
                                BEGIN CATCH
                                      ROLLBACK TRANSACTION DeleteBookWithDeps
                                END CATCH";
                connection.Query(sql);
            }
        }
    }
}

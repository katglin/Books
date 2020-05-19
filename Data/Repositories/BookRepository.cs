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
                string sql = $@"SELECT b.Id, b.Name, b.ReleaseDate, b.Rate, b.PageNumber, a.Id, a.FirstName, a.LastName, ba.BookId, ba.AuthorId
                               FROM Book b 
                               JOIN BookAuthor ba ON b.Id = ba.BookId
                               JOIN Author a ON a.Id = ba.AuthorId
                               WHERE b.Id = {id}";

                var books = connection.Query<BookDTO, AuthorDTO, BookDTO>(sql,
                    (book, author) =>
                    {
                        book.Authors.Add(author);
                        return book;
                    },
                    splitOn: "Id")
                    .GroupBy(b => b.Id)
                    .Select(group =>
                    {
                        var combinedBook = group.First();
                        combinedBook.Authors = group.Select(book => book.Authors.Single()).ToList();
                        return combinedBook;
                    });

                return books.FirstOrDefault();
            }
        }

        public IEnumerable<BookDTO> GetBooks()
        {
            using (var connection = ConnectionProvider())
            {
                string sql = @"SELECT b.Id, b.Name, b.ReleaseDate, b.Rate, b.PageNumber, a.Id, a.FirstName, a.LastName, ba.BookId, ba.AuthorId
                               FROM Book b 
                               JOIN BookAuthor ba ON b.Id = ba.BookId
                               JOIN Author a ON a.Id = ba.AuthorId";

                var books = connection.Query<BookDTO, AuthorDTO, BookDTO>(sql,
                    (book, author) =>
                    {
                        book.Authors.Add(author);
                        return book;
                    },
                    splitOn: "Id")
                    .GroupBy(b => b.Id)
                    .Select(group =>
                    {
                        var combinedBook = group.First();
                        combinedBook.Authors = group.Select(book => book.Authors.Single()).ToList();
                        return combinedBook;
                    });

                return books;
            }
        }

        public void CreateBook(BookDTO book)
        {
            using (var connection = ConnectionProvider())
            {
                var SP = "USPCreateBook";
                var queryParameters = new DynamicParameters();
                var ids = book.Authors.Select(a => a.Id).AsDataTableParam();

                queryParameters.Add("@BookName", book.Name);
                queryParameters.Add("@ReleaseDate", book.ReleaseDate);
                queryParameters.Add("@Rate", book.Rate);
                queryParameters.Add("@PageNumber", book.PageNumber);
                queryParameters.Add("@AuthorIds", ids.AsTableValuedParameter("BigIntArrayType"));

                connection.Query(SP, queryParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateBook(BookDTO book)
        {
            using (var connection = ConnectionProvider())
            {
                var SP = "USPUpdateBook";
                var queryParameters = new DynamicParameters();
                var ids = book.Authors.Select(a => a.Id).AsDataTableParam();

                queryParameters.Add("@BookId", book.Id);
                queryParameters.Add("@BookName", book.Name);
                queryParameters.Add("@ReleaseDate", book.ReleaseDate);
                queryParameters.Add("@Rate", book.Rate);
                queryParameters.Add("@PageNumber", book.PageNumber);
                queryParameters.Add("@AuthorIds", ids.AsTableValuedParameter("BigIntList"));

                connection.Query(SP, queryParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteBook(long id)
        {
            using (var connection = ConnectionProvider())
            {
                string sql = $@"DELETE Book 
                                WHERE Id = {id}";
                connection.Query(sql);
            }
        }
    }
}

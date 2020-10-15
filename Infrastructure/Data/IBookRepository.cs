using DTO;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public interface IBookRepository: IRepository<BookDTO, long>
    {
        BookDTO GetBook(long id);

        IEnumerable<BookDTO> GetBooks();

        long CreateBook(BookDTO Book);

        void UpdateBookTitle(int bookId, string bookTitleKey);

        void UpdateBook(BookDTO Book);

        void DeleteBook(long id);
    }
}

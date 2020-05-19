using DTO;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public interface IBookRepository: IRepository<BookDTO, long>
    {
        BookDTO GetBook(long id);

        IEnumerable<BookDTO> GetBooks();

        void CreateBook(BookDTO Book);

        void UpdateBook(BookDTO Book);

        void DeleteBook(long id);
    }
}

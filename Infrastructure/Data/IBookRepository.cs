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

        void AddAttachment(long bookId, string fileName, string fileS3Key);

        void DeleteAttachment(string fileS3Key);

        void UpdateBook(BookDTO Book);

        void DeleteBook(long id);
    }
}

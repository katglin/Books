using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ViewModels;

namespace Infrastructure.Business
{
    public interface IBookDM: IDisposable
    {
        Book GetBook(long id);

        IEnumerable<Book> GetBooks();

        void CreateBook(Book Book);

        void UpdateBook(Book Book);

        void DeleteBook(long id);

        Task<string> UploadImageAsync(int bookId, string fileName, Stream file);
    }
}

using Infrastructure.Business;
using Infrastructure.Data;
using System.Collections.Generic;
using ViewModels;
using AutoMapper;
using DTO;

namespace Business
{
    public class BookDM: BaseDM, IBookDM
    {
        public BookDM(Infrastructure.IServiceProvider service) : base(service)
        {

        }

        public Book GetBook(long id)
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            {
                var author = bookRepo.GetBook(id);
                return Mapper.Map<Book>(author);
            }
        }

        public IEnumerable<Book> GetBooks()
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            {
                var books = bookRepo.GetBooks();
                return Mapper.Map<IEnumerable<Book>>(books);
            }
        }

        public void CreateBook(Book book)
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            {
                var entity = Mapper.Map<BookDTO>(book);
                bookRepo.CreateBook(entity);
            }
        }

        public void UpdateBook(Book book)
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            {
                var entity = Mapper.Map<BookDTO>(book);
                bookRepo.UpdateBook(entity);
            }
        }

        public void DeleteBook(long id)
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            {
                bookRepo.DeleteBook(id);
            }
        }
    }
}

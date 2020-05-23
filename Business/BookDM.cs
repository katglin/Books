using Infrastructure.Business;
using Infrastructure.Data;
using System.Collections.Generic;
using ViewModels;
using AutoMapper;
using DTO;
using System.Linq;

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
                var book = bookRepo.GetBook(id);
                book.AuthorIds = book.Authors.Select(a => a.Id);
                return Mapper.Map<Book>(book);
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

        public long CreateBook(Book book)
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            {
                var entity = Mapper.Map<BookDTO>(book);
                return bookRepo.CreateBook(entity);
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

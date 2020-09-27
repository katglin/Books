using Infrastructure.Business;
using Infrastructure.Data;
using System.Collections.Generic;
using ViewModels;
using AutoMapper;
using DTO;
using System.Linq;
using SNSSender;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

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
            using (var s3Service = ServiceProvider.GetService<IAwsS3Service>())
            {
                var books = bookRepo.GetBooks();
                var bookVMs = Mapper.Map<IEnumerable<Book>>(books);
                var deffaultUrl = s3Service.GetStaticImage("no-image.png");
                foreach (var book in bookVMs)
                {
                    book.ImageUrl = !string.IsNullOrEmpty(book.ImageS3Key) ? s3Service.GeneratePreSignedURL(book.ImageS3Key, "book-title-images") : deffaultUrl;
                }
                return bookVMs;
            }
        }

        public async Task<string> UploadImageAsync(int bookId, string fileName, Stream file)
        {
            string bucketName = "book-title-images";
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            using (var s3Service = ServiceProvider.GetService<IAwsS3Service>())
            {
                var imageS3Key = await s3Service.UploadFileAsync(fileName, bucketName, file);

                var book = bookRepo.GetBook(bookId);
                if (!string.IsNullOrEmpty(book.ImageS3Key)) {
                    await s3Service.DeleteFileAsync(book.ImageS3Key, bucketName);
                }
                bookRepo.UpdateBookTitle(bookId, imageS3Key);
                return s3Service.GeneratePreSignedURL(imageS3Key, bucketName);
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

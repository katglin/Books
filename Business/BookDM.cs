using Infrastructure.Business;
using Infrastructure.Data;
using System.Collections.Generic;
using ViewModels;
using AutoMapper;
using DTO;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Business
{
    public class BookDM: BaseDM, IBookDM
    {
        private const string _defaultImageName = "no-image.png";

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
                var deffaultUrl = s3Service.GetStaticImage(_defaultImageName);
                foreach (var book in bookVMs)
                {
                    book.ImageUrl = !string.IsNullOrEmpty(book.ImageS3Key) ? s3Service.GeneratePreSignedURL(book.ImageS3Key, this.BookTitlesBucket) : deffaultUrl;
                    foreach(var attachment in book.Attachments)
                    {
                        attachment.FileUrl = s3Service.GeneratePreSignedURL(attachment.FileS3Key, this.BookAttachmentsBucket);
                    }
                }
                return bookVMs;
            }
        }

        public async Task<string> UploadImageAsync(int bookId, string fileName, Stream file)
        {
            string bucketName = this.BookTitlesBucket;
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

        public async Task DeleteBookAsync(long bookId)
        {
            using (var bookRepo = ServiceProvider.GetService<IBookRepository>())
            using (var s3Service = ServiceProvider.GetService<IAwsS3Service>())
            {
                string titleBucketName = this.BookTitlesBucket;
                string attachmentsBucketName = this.BookAttachmentsBucket;
                var book = bookRepo.GetBook(bookId);
                if (!string.IsNullOrEmpty(book.ImageS3Key))
                {
                    await s3Service.DeleteFileAsync(book.ImageS3Key, titleBucketName);
                }
                foreach (var attachment in book.Attachments)
                {
                    await s3Service.DeleteFileAsync(attachment.FileS3Key, attachmentsBucketName);
                }
                bookRepo.DeleteBook(bookId);
            }
        }
    }
}

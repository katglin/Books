﻿using Infrastructure.Business;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using ViewModels;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Web.Controllers
{
    public class BookController : BaseController
    {
        public ActionResult Index()
        {
            using (var bookDM = ServiceProvider.GetService<IBookDM>())
            {
                var books = bookDM.GetBooks();
                return View(books);
            }
        }

        public JsonResult Get(long id)
        {
            using (var bookDM = ServiceProvider.GetService<IBookDM>())
            {
                var book = bookDM.GetBook(id);
                return Json(book, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(Book model)
        {
            if (ModelState.IsValid)
            {
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    var bookId = bookDM.CreateBook(model);
                    return Json(bookDM.GetBook(bookId));
                }
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<string> UploadImageAsync()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                Stream fileStream = file.InputStream;
                var bookId = Int32.Parse(Request.Params["BookId"]);
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    return await bookDM.UploadImageAsync(bookId, fileName, fileStream);
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult Edit(Book model)
        {
            if (ModelState.IsValid)
            {
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    bookDM.UpdateBook(model);
                    return Json(bookDM.GetBook((long)model.Id));
                }
            }
            return null;
        }

        [HttpPost]
        public async Task DeleteAsync(long id)
        {
            using (var bookDM = ServiceProvider.GetService<IBookDM>())
            {
                await bookDM.DeleteBookAsync(id);
            }
        }
    }
}

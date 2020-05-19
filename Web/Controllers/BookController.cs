﻿using Infrastructure.Business;
using System.Web.Mvc;
using ViewModels;

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

        [HttpPost]
        public bool Create(Book model)
        {
            if (ModelState.IsValid)
            {
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    bookDM.CreateBook(model);
                    return true;
                }
            }
            else
            {
                return false;
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
        public ActionResult Edit(Book model)
        {
            if (ModelState.IsValid)
            {
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    bookDM.UpdateBook(model);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
        }

        public void Delete(long id)
        {
            using (var bookDM = ServiceProvider.GetService<IBookDM>())
            {
                bookDM.DeleteBook(id);
            }
        }
    }
}

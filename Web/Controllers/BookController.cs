using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ViewModels;

namespace Web.Controllers
{
    public class BookController : BaseController
    {
        public ActionResult Index()
        {
            List<Author> authors = new List<Author> {
                new Author { Id = 1, FirstName = "Sansa", LastName = "Stark", BookNumber = 2 },
                new Author { Id = 2, FirstName = "John", LastName = "Stark", BookNumber = 1 }
            };
            List<Book> books = new List<Book> {
                new Book { Id = 1, Name = "Book 1", PageNumber = 100, Rate = 10, ReleaseDate = DateTime.Now, Authors = authors }
            };
            return View(books);
        }


        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

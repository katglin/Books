using Infrastructure.Business;
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
        public JsonResult Create(Book model)
        {
            if (ModelState.IsValid)
            {
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    model.Id = bookDM.CreateBook(model);
                    return Json(bookDM.GetBook((int)model.Id));
                }
            }
            else
            {
                return null;
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
        public JsonResult Edit(Book model)
        {
            if (ModelState.IsValid)
            {
                using (var bookDM = ServiceProvider.GetService<IBookDM>())
                {
                    bookDM.UpdateBook(model);
                    return Json(bookDM.GetBook((int)model.Id));
                }
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public void Delete(long id)
        {
            using (var bookDM = ServiceProvider.GetService<IBookDM>())
            {
                bookDM.DeleteBook(id);
            }
        }
    }
}

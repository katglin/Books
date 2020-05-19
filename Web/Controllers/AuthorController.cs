﻿using Infrastructure.Business;
using System.Web.Mvc;
using ViewModels;

namespace Web.Controllers
{
    public class AuthorController : BaseController
    {
        public ActionResult Index()
        {
            using (var authorDM = ServiceProvider.GetService<IAuthorDM>())
            {
                var authors = authorDM.GetAuthors();
                return View(authors);
            }
            
        }

        public ActionResult Create()
        {
            var author = new Author();
            return View(author);
        }

        [HttpPost]
        public ActionResult Create(Author model)
        {
            if (ModelState.IsValid)
            {
                using (var authorDM = ServiceProvider.GetService<IAuthorDM>())
                {
                    authorDM.CreateAuthor(model);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Edit(long id, string firstName, string lastName)
        {
            using (var authorDM = ServiceProvider.GetService<IAuthorDM>())
            {
                var author = authorDM.GetAuthor(id);
                return View(author);
            }
        }

        [HttpPost]
        public ActionResult Edit(Author model)
        {
            if (ModelState.IsValid)
            {
                using (var authorDM = ServiceProvider.GetService<IAuthorDM>())
                {
                    authorDM.UpdateAuthor(model);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Delete(long id)
        {
            using (var authorDM = ServiceProvider.GetService<IAuthorDM>())
            {
                authorDM.DeleteAuthor(id);
                return RedirectToAction("Index");
            }
        }
    }
}

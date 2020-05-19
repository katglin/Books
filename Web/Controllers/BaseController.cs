using Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected Infrastructure.IServiceProvider ServiceProvider { get; set; }

        public BaseController()
        {
            ServiceProvider = new ServiceProvider();
        }
    }
}
using Bootstrap;
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
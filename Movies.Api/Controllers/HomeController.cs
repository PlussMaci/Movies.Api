using System.Web.Mvc;

namespace Movies.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           return View();
        }
    }
}

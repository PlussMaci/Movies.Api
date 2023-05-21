using Movies.Api.Models;
using System.Web.Mvc;

namespace Movies.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult LoginControl()
        {
            return PartialView(new AccountModel());
        }
    }
}

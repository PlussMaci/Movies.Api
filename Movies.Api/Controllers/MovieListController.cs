using Movies.Api.Common.Entities;
using Movies.Api.Models;
using System.Web.Mvc;

namespace Movies.Api.Controllers
{
    public class MovieListController : Controller
    {
        public ActionResult SearchControl()
        {
            return PartialView(new SearchModel());
        }

        public ActionResult EditControl()
        {
            return PartialView(new MovieList()
            {
                movies = new System.Collections.Generic.List<Movie>() {
                    new Movie {}
                }
            });
        }

    }
}
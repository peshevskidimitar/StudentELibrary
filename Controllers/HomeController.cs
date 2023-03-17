using StudentELibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentELibrary.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var resources = db.Resources.OrderBy(resource => resource.DatePublished)
                .Take(8)
                .ToList();
            return View(resources);
        }
    }
}
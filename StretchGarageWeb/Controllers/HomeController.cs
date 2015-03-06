using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StretchGarageWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var db = new StretchGarageDataLayer.StretchGarageWeb_dbEntities();
            var zone = db.ParkingPlaces.First(a => a.Id == 0);
            zone.ZoneId = 0;

            db.SaveChanges();

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
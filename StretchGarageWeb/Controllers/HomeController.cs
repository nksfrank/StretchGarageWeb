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
            var pp = db.ParkingPlaces.Create();
            pp.ParkingSpots = 4;

            db.ParkingPlaces.Add(pp);
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
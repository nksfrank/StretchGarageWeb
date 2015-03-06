using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StretchGarageWeb.Controllers
{
    public class HomeController : MainController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var pp = DB.ParkingPlaces.First(a => a.Id == 0);

            ViewBag.Message = pp.ParkedCars.Count() + "/" + pp.ParkingSpots;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
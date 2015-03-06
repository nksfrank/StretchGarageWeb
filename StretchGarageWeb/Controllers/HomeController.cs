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
            var zone = DB.ParkingPlaces.First(a => a.Id == 0);
            zone.ZoneId = 0;

            

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
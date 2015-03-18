using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StretchGarageWeb.Controllers
{
    public class ParkingPlaceController : Controller
    {
        // GET: ParkingPlace
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }
            return View("Contact");
        }
    }
}
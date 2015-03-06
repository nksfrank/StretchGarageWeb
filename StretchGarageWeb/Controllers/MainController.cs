using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StretchGarageWeb.Controllers
{
    public abstract class MainController : Controller
    {
        public DataLayer.Database.dbDataContext DB = new DataLayer.Database.dbDataContext();
    }
}
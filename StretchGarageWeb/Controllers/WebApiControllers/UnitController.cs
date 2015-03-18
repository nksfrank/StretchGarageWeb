using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StretchGarageWeb.Controllers
{
    public class UnitController : ApiController
    {
        //Get /api/id/
        // GET api/CreateUnit
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/CreateUnit/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class ParkedCarsController : ApiController
    {
        // GET api/ParkedCars 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ParkedCars/5
        public string Get(int id)
        {
            return "value"; //HJA
        }
    }
}
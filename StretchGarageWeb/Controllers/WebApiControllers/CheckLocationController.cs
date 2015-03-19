using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class CheckLocationController : ApiController
    {
        // GET api/CheckLocation/
        public IEnumerable<string> Get()
        {
            var y = Request.GetHashCode();
            return new string[] { "This", "Works" };
        }

        // GET api/api/CheckLocation/5
        public string Get(int id, double latitude, double longitude)
        {
            string test = "You want a location?";
            return test;
        }
    }
}
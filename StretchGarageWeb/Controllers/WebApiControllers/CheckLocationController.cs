using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            return new string[] { "This", "Works" };
        }

        // GET api/api/CheckLocation/5
        public string Get(int id)
        {
            string test = "You want a location?";
            return test; 
        }

        // POST api/<controller> 
        public void Post([FromBody]string value)
        {
            //Ska ta emot double latitude och longitude

        }
    }
}
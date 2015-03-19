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
using Objects;
using Objects.WebApiResponse;

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
        public HttpResponseMessage Get(int id, double latitude, double longitude)
        {
            var res = BusinessLayer.LocationManager.LocationManager.CheckLocationTest(id, latitude, longitude);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse<ApiResponse>(HttpStatusCode.OK, (ApiResponse)res);
        }
    }
}
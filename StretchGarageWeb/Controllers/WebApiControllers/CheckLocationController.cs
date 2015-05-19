using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Script.Serialization;
using BusinessLayer.LocationManager;
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
        public HttpResponseMessage Get(int id, [FromUri]double[] latitude, [FromUri]double[] longitude, [FromUri]double[] speed)
        {
            var locMgr = new LocationManager();
            var res = locMgr.ProcessLocationRequest(id, latitude, longitude, speed);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);
            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)res);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]CheckLocationRequest value)
        {
            var carId = value.Id;
            var carLat = value.Lat;
            var carLong = value.Long;

            var locMgr = new LocationManager();
            var res = locMgr.ProcessLocationRequest(carId, carLat, carLong);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
    [DataContract]
    public class CheckLocationRequest
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Long { get; set; }
    }
}
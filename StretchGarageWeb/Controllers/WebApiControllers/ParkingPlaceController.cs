using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Objects.WebApiResponse;
using BusinessLayer.ParkingManager;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class ParkingPlaceController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            ParkingPlaceManager.GetAllParkingPlaces(id);
            var result = new ApiResponse();
            return Request.CreateResponse<ApiResponse>(HttpStatusCode.OK, result);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "");
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "");
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "");
        }
    }
}
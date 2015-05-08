using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Objects;
using Objects.WebApiResponse;
using BusinessLayer.ParkingManager;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class ParkingPlaceController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var parkingMgr = new ParkingPlaceManager();
            var res = parkingMgr.GetAllParkingPlaces();
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)res);
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            var parkingMgr = new ParkingPlaceManager();
            var res = parkingMgr.GetParkingPlace(id);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)res);
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
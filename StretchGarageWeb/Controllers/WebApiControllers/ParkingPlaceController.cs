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
        public HttpResponseMessage Get()
        {
            var res = (ApiResponse)ParkingPlaceManager.GetAllParkingPlaces();
            if (!res.Success)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);
            }

            return Request.CreateResponse<ApiResponse>(HttpStatusCode.OK, res);
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "");
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
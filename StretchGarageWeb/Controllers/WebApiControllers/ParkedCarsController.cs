using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using BusinessLayer.ParkingManager;
using Objects;
using Objects.WebApiResponse;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class ParkedCarsController : ApiController
    {

        // GET api/ParkedCars/
        // params(id for parking place)
        public HttpResponseMessage Get(int id)
        {
            ParkCarManager parkMgr = new ParkCarManager();
            var cars = parkMgr.GetParkedCars(id);
            if (cars is Error)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, cars.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)cars);
        }
    }
}
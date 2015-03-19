using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Objects.WebApiResponse;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class ParkedCarsController : ApiController
    {

        // GET api/ParkedCars/
        // params(id for parking place)
        public HttpResponseMessage Get(int id)
        {
            var cars = BusinessLayer.ParkingManager.ParkCarManager.GetParkedCars(id);
            return Request.CreateResponse(HttpStatusCode.OK, cars);
        }
    }
}
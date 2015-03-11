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
            var cars = new List<ParkedCar>();
            for (var i = 0; i < 5; i++)
            {
                cars.Add(new ParkedCar(true, "Free", "green"));
                cars.Add(new ParkedCar(false, "Taken", "red"));
            }
            var response = new ApiResponse(true, "Hello World", cars);
            return Request.CreateResponse<ApiResponse>(HttpStatusCode.OK, response);

            //BusinessLayer.ParkingManager.ParkingPlaceManager.AvaiableSpaces(0);
        }
    }
}
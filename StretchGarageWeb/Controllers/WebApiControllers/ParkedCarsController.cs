using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace StretchGarageWeb.Controllers.WebApiControllers
{
    public class Car {
        public Car(bool isAvailable, string status, string cssClass)
        {
            IsAvailable = isAvailable;
            Status = status;
            CssClass = cssClass;
        }
        public bool IsAvailable { get; set; }
        public string Status{ get; set; }
        public string CssClass { get; set; }
    }
    public class ParkedCarsController : ApiController
    {
        
        // GET api/ParkedCars 
        //public IEnumerable<string> Get()
        public IEnumerable<Car> Get()
        {
            var cars = new List<Car> {new Car(true, "Free", "green")};
            return cars;
        }

        // GET api/ParkedCars/5
        public HttpResponseMessage Get(int id)
        {
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            var cars = new List<Car>();
            for (var i = 0; i < 5; i++)
            {
                cars.Add(new Car(true, "Free", "green"));
                cars.Add(new Car(false, "Taken", "red"));
            }

            return Request.CreateResponse<IEnumerable<Car>>(HttpStatusCode.BadRequest, cars);
            
            //return cars;

            /*response.Content = new ObjectContent(IEnumerable<Car>, cars,);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };
            return response;*/
            //Test creation
            

            //BusinessLayer.ParkingManager.ParkingPlaceManager.AvaiableSpaces(0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Interface;
using Objects.WebApiResponse;

namespace BusinessLayer.LocationManager
{
    public class LocationManager : MainHandler
    {
        public static double CalculateDistanceToParkingPlace()
        {
            throw new NotImplementedException();
        }

        public static IError CheckLocationTest(int id, double latitude, double longitude)
        {
            CheckLocationResponse response = new CheckLocationResponse(5, true);
            ApiResponse api = new ApiResponse(true, "", response);
            return api;
        }
    }
}

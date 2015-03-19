using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;
using Objects;
using Objects.Interface;
using Objects.WebApiResponse;

namespace BusinessLayer.LocationManager
{
    public class LocationManager : MainHandler
    {
        public static IError ProcessLocationRequest(int carId, double carLat, double carLong)
        {
            var pId = GetClosestParkingPlaceId(carLat, carLong);
            var dist = GetDistanceToParkingPlace(carLat, carLong, pId);

            if (dist < 20)
            {
                ParkedCar car = new ParkedCar {UnitId = carId, ParkingPlaceId = pId};
                DB.ParkedCars.InsertOnSubmit(car);

                try
                {
                    DB.SubmitChanges();
                }
                catch (Exception)
                {
                    return new Error { Message = "Kunde inte parkera bilen", Success = false };
                }
            }
            return new CheckLocationResponse { Interval = 10, CheckSpeed = false, Message = "", Success = true};
        }
        public static int GetClosestParkingPlaceId(double carLat, double carLong)
        {
            var parkingPlaces = DB.ParkingPlaces.Select(a => new {a.Id, a.Lat, a.Long}).ToList();

            var closest = double.MaxValue;
            var parkingPlaceId = -1;
            foreach (var p in parkingPlaces)
            {
                var dist = GetDistanceToParkingPlace(carLat, carLong, (double)p.Lat, (double)p.Long);

                if (!(dist < closest)) continue;

                closest = dist;
                parkingPlaceId = p.Id;
            }
            return parkingPlaceId;
        }

        public static double GetDistanceToParkingPlace(double carLat, double carLong, int parkingPlaceId)
        {
            var p = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new {a.Lat, a.Long}).First();
            return GetDistanceToParkingPlace(carLat, carLong, (double)p.Lat, (double)p.Long);
        }

        private static double GetDistanceToParkingPlace(double carLat, double carLong, double pLat, double pLong)
        {
            return Utilities.Gps.DistanceBetweenPlacesM(carLat, carLong, pLat, pLong);
        }
    }
}

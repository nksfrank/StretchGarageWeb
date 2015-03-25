using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.ParkingManager;
using DataLayer.Database;
using Objects;
using Objects.Interface;
using Objects.WebApiResponse;

namespace BusinessLayer.LocationManager
{
    public class LocationManager
    {
        private static readonly dbDataContext DB = new dbDataContext();
        public static IError ProcessLocationRequest(int carId, double carLat, double carLong)
        {
            if (!DB.Units.Any(a => a.Id == carId))
            {
                return new Error { Message = "Det finns ingen enhet i databasen", Success = false};
            }

            var pId = GetClosestParkingPlaceId(carLat, carLong);
            var dist = GetDistanceToParkingPlace(carLat, carLong, pId);
            var parked = false;

            if (dist < 20)
            {
                var resp = ParkCarManager.ParkCar(carId, pId);
                if (!resp.Success)
                {
                    return resp;
                }
                parked = true;
            }
            else
            {
                ParkCarManager.UnParkCar(carId);
            }
            var content = new CheckLocationResponse { Interval = 10, CheckSpeed = false, IsParked = parked};
            return new ApiResponse(true, "", content);
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

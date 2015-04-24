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
        //Meters per second
        private const double SPEED = 11.11;// = 40km/h
        private const double FRACTION = 0.66;//Two thirds
        private const double MAXINTERVAL = 1800000;//30 minutes in ms

        private static readonly dbDataContext DB = new dbDataContext();
        public static IError ProcessLocationRequest(int carId, double carLat, double carLong)
        {
            if (!DB.Units.Any(a => a.Id == carId))
            {
                return new Error { Message = "Det finns ingen enhet i databasen", Success = false};
            }

            var parkingPlaceClosest = GetClosestParkingPlace(carLat, carLong);
            if (parkingPlaceClosest == null) return new Error() { Success = false, Message = "No parking place was found" };
            var dist = GetDistanceToParkingPlace(carLat, carLong, (double)parkingPlaceClosest.Lat, (double)parkingPlaceClosest.Long);
            var parked = false;

            if (dist <= parkingPlaceClosest.Size)
            {
                var resp = ParkCarManager.ParkCar(carId, parkingPlaceClosest.Id);
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

            var interval = CalculateUpdateInterval(dist);
            var checkSpeed = false;
            if (interval < 120000)
                checkSpeed = true;
            var content = new CheckLocationResponse { Interval = interval, CheckSpeed = checkSpeed, IsParked = parked };
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

        public static ParkingPlace GetClosestParkingPlace(double carLat, double carLong)
        {
            var parkingPlaces = DB.ParkingPlaces.ToList();
            var closest = double.MaxValue;
            ParkingPlace parkingPlace = null;
            foreach (var p in parkingPlaces)
            {
                var dist = GetDistanceToParkingPlace(carLat, carLong, (double) p.Lat, (double) p.Long);
                if (!(dist < closest)) continue;

                closest = dist;
                parkingPlace = p;
            }

            return parkingPlace;
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

        /// <summary>
        /// Returns a value of milliseconds calculated from distance based on SPEED
        /// </summary>
        /// <param name="dist">Distance in meters</param>
        /// <returns>Interval in milliseconds</returns>
        public static int CalculateUpdateInterval(double dist)
        {
            var calcTotalSeconds = dist / SPEED;
            var takeSlice = calcTotalSeconds * FRACTION;
            var timeInMs = takeSlice * 1000;
            return timeInMs < MAXINTERVAL ? (int)timeInMs : MAXINTERVAL;
        }
    }
}

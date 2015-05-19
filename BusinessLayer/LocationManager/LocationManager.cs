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
        private const int MAXINTERVAL = 1800000;//30 minutes in ms
        private const int MININTERVAL = 2000;

        private dbDataContext DB = new dbDataContext();
        public IError ProcessLocationRequest(int carId, double carLat, double carLong)
        {
            if (!DB.Units.Any(a => a.Id == carId)) { //checks if car exists
                return new Error { Message = "Det finns ingen enhet i databasen", Success = false};
            }
            var parkingPlaceClosest = GetClosestParkingPlace(carLat, carLong); //returns parkingplace with shortest distance
            if (parkingPlaceClosest == null) return new Error() { Success = false, Message = "No parking place was found" };
            
            var response = new CheckLocationResponse();
            var dist = GetDistanceToParkingPlace(carLat, carLong, (double)parkingPlaceClosest.Lat, (double)parkingPlaceClosest.Long);

            if (dist <= parkingPlaceClosest.Size)
            {
                var parkMgr = new ParkCarManager();
                var resp = parkMgr.ParkCar(carId, parkingPlaceClosest.Id);
                if (!resp.Success)
                {
                    return resp;
                }
                response.IsParked = true;
            }
            else
            {
                ValidateUnparkingCar(carId, parkingPlaceClosest);
                //NOTE:Add checks to see if car har moved according to agreements for unparking
                //ParkCarManager.UnParkCar(carId);
            }

            response.Interval = CalculateUpdateInterval(dist, parkingPlaceClosest.OuterBound);
            response.CheckSpeed = dist < parkingPlaceClosest.OuterBound;
            return new ApiResponse(true, "", response);
        }

        private void ValidateUnparkingCar(int carId, ParkingPlace parkingPlaceClosest)
        {
            var parkMgr = new ParkCarManager();
            parkMgr.UnParkCar(carId);
        }

        public int GetClosestParkingPlaceId(double carLat, double carLong)
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

        public ParkingPlace GetClosestParkingPlace(double carLat, double carLong)
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

        public double GetDistanceToParkingPlace(double carLat, double carLong, int parkingPlaceId)
        {
            var p = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new {a.Lat, a.Long}).First();
            return GetDistanceToParkingPlace(carLat, carLong, (double)p.Lat, (double)p.Long);
        }

        private double GetDistanceToParkingPlace(double carLat, double carLong, double pLat, double pLong)
        {
            return Utilities.Gps.DistanceBetweenPlacesM(carLat, carLong, pLat, pLong);
        }

        /// <summary>
        /// Returns a value of milliseconds calculated from distance based on SPEED
        /// </summary>
        /// <param name="dist">Distance in meters</param>
        /// <param name="outerBound">Outerbound value of parkingplace, in meters</param>
        /// <returns>Interval in milliseconds</returns>
        public int CalculateUpdateInterval(double dist, int outerBound)
        {
            if (dist <= outerBound)
                return MININTERVAL;
            var calcTotalSeconds = dist / SPEED;
            var takeSlice = calcTotalSeconds * FRACTION;
            var timeInMs = takeSlice * 1000;
            return timeInMs < MAXINTERVAL ? (int)timeInMs : MAXINTERVAL;
        }
    }
}

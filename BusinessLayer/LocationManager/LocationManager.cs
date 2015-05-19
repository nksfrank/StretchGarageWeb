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
        public IError ProcessLocationRequest(int carId, double[] carLat, double[] carLong, double[] speed)
        {
            if (!DB.Units.Any(a => a.Id == carId)) { //checks if car exists
                return new Error { Message = "Det finns ingen enhet i databasen", Success = false};
            }
            var parkingPlaceClosest = GetClosestParkingPlace(carLat[carLat.Length - 1], carLong[carLat.Length - 1]); //returns parkingplace with shortest distance
            if (parkingPlaceClosest == null) return new Error() { Success = false, Message = "No parking place was found" };
            
            var response = new CheckLocationResponse(); //creates obj
            var directionDistance = CheckDirectionAndDistance(parkingPlaceClosest, carLat, carLong); //gets direction(Key) and distance(Value)
            
            //Add checkSpeed function here to validate that it's a car
            bool speedOK = CheckSpeed(parkingPlaceClosest, directionDistance, speed, carId);

            if (directionDistance.Value <= parkingPlaceClosest.Size && speedOK) //Park car
            {
                var parkMgr = new ParkCarManager();
                var resp = parkMgr.ParkCar(carId, parkingPlaceClosest.Id);
                if (!resp.Success) //error
                    return resp; 
                response.IsParked = true;
            }
            else if(!directionDistance.Key && speedOK)//unpark car since it's traveling away from parkingplace
            {
                ValidateUnparkingCar(carId, parkingPlaceClosest);
                //NOTE:Add checks to see if car har moved according to agreements for unparking
                //ParkCarManager.UnParkCar(carId);
            }
            response.Interval = CalculateUpdateInterval(directionDistance.Value, parkingPlaceClosest.OuterBound);
            response.CheckSpeed = directionDistance.Value < parkingPlaceClosest.OuterBound;
            return new ApiResponse(true, "", response);
        }

        private bool CheckSpeed(ParkingPlace parkingPlaceClosest, KeyValuePair<bool, double> directionDistance, double[] speed, int carId)
        {
            var currentUnit = DB.Units.FirstOrDefault(unit => unit.Id == carId);
            
            if (directionDistance.Key) //Towards parkingplace
            {
                if (directionDistance.Value <= parkingPlaceClosest.OuterBound && speed.Any(x => x > 30))
                    currentUnit.FarSpeed = DateTime.UtcNow;

                if (directionDistance.Value <= parkingPlaceClosest.Size && speed.Any(x => x > 10))
                    currentUnit.CloseSpeed = DateTime.UtcNow;
            }
            else //Away from parkingplace
            {
                if (speed.Any(x => x > 30))
                    currentUnit.FarSpeed = DateTime.UtcNow;
            }
            DB.SubmitChanges();
            return true;
        }

        private KeyValuePair<bool, double> CheckDirectionAndDistance(ParkingPlace parkingPlaceClosest, double[] carLat, double[] carLong)
        {
            double lastdist = double.MinValue;
            bool direction = false; //default is away from parkingplace
            for (int i = 0; i < carLat.Length; i++)
            {
                var newerdist = GetDistanceToParkingPlace(carLat[i], carLong[i], (double)parkingPlaceClosest.Lat, (double)parkingPlaceClosest.Long);
                direction = newerdist < lastdist; //sets towards location if true
                lastdist = newerdist;
            }
            return new KeyValuePair<bool, double>(direction, lastdist);
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

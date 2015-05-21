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
        private const double FARSPEED = 8.33; // = 30km/h
        private const double CLOSESPEED = 2.77; // = 10km/h
        private const int FARTIME = -10; // = 10km/h
        private const int CLOSETIME = -1; // = 10km/h

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
            var speedCheck = CheckSpeed(parkingPlaceClosest, directionDistance, speed, carId);

            if (speedCheck.Item1 != null) return speedCheck.Item1; //Database failed to submit changes

            if (directionDistance.Value <= parkingPlaceClosest.Size && speedCheck.Item2 && speedCheck.Item3) //Park car if within area and speed ok both far and close
            {
                speedCheck.Item4.FarSpeed = speedCheck.Item4.CloseSpeed = DateTime.UtcNow.AddDays(-1);
                var dbResp = SubmitUnitChangesToDb();
                if (dbResp != null)
                    return dbResp; 

                var parkMgr = new ParkCarManager();
                var resp = parkMgr.ParkCar(carId, parkingPlaceClosest.Id);
                if (!resp.Success) //error
                    return resp; 
                response.IsParked = true;
            }
            else if (directionDistance.Value >= parkingPlaceClosest.Size && !directionDistance.Key && speedCheck.Item3)//unpark car since it's traveling away from parkingplace over 30km/h
            {
                var parkMgr = new ParkCarManager();
                var resp = parkMgr.UnParkCar(carId);
                if (!resp.Success) //error
                    return resp;
            }

            response.Interval = CalculateUpdateInterval(directionDistance.Value, parkingPlaceClosest.OuterBound);
            response.CheckSpeed = directionDistance.Value < parkingPlaceClosest.OuterBound;
            return new ApiResponse(true, "", response);
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

        private Tuple<Error, bool, bool, Unit> CheckSpeed(ParkingPlace parkingPlaceClosest, KeyValuePair<bool, double> directionDistance, double[] speed, int carId)
        {
            var currentUnit = DB.Units.FirstOrDefault(unit => unit.Id == carId);

            if (directionDistance.Value >= parkingPlaceClosest.Size && speed.Any(x => x >= FARSPEED)) //speed above 30km/h outside parkinplace
                currentUnit.FarSpeed = DateTime.UtcNow;

            if (directionDistance.Value <= parkingPlaceClosest.InnerBound && speed.Any(x => x >= CLOSESPEED)) //speed above 10km/h within 
                currentUnit.CloseSpeed = DateTime.UtcNow;

            bool farSpeedOk = currentUnit.FarSpeed > DateTime.UtcNow.AddMinutes(FARTIME);
            bool closeSpeedOk = currentUnit.CloseSpeed > DateTime.UtcNow.AddMinutes(CLOSETIME);

            var dbResp = SubmitUnitChangesToDb();
            if (dbResp != null)
                return new Tuple<Error, bool, bool, Unit>(dbResp, farSpeedOk, closeSpeedOk, currentUnit); 
            /*try
            {
                SubmitUnitChangesToDb();
                //DB.SubmitChanges();
            }
            catch (Exception)
            {
                return new Tuple<Error, bool, bool, Unit>(new Error { Message = "Kunde inte spara ändringarna till databasen", Success = false },
                    farSpeedOk, closeSpeedOk, currentUnit); 
            }*/

            //item1 = error, item2 = FarSpeed within 10 minuts, item3 = CloseSpeed within 1 minutes
            return new Tuple<Error, bool, bool, Unit>(null, farSpeedOk, closeSpeedOk, currentUnit);

        }

        private Error SubmitUnitChangesToDb()
        {
            try
            {
                DB.SubmitChanges();
            }
            catch (Exception)
            {
                return new Error {Message = "Kunde inte spara ändringarna till databasen", Success = false};
            }
            return null;
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

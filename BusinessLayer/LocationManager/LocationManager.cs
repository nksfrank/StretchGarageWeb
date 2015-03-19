using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;

namespace BusinessLayer.LocationManager
{
    public class LocationManager : MainHandler
    {
        public static int GetClosestParkingPlaceId(double carLat, double carLong)
        {
            var parkingPlaces = DB.ParkingPlaces.Select(a => new {a.Id, a.Lat, a.Long}).ToList();

            var closest = double.MaxValue;
            var parkingPlaceId = -1;
            foreach (var p in parkingPlaces)
            {
                var dist = Utilities.Gps.DistanceBetweenPlacesM(carLat, carLong, (double)p.Lat, (double)p.Long);

                if (!(dist < closest)) continue;

                closest = dist;
                parkingPlaceId = p.Id;
            }
            return parkingPlaceId;
        }

        public static double GetDistanceToParkingPlace(int parkingPlaceId, double carLat, double carLong)
        {
            var p = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new {a.Lat, a.Long}).First();
            return Utilities.Gps.DistanceBetweenPlacesM(carLat, carLong, (double)p.Lat, (double)p.Long);
        }
    }
}

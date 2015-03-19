using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utilities
{
    public class Gps
    {
        const double D2R = 0.0174532925; // Pi / 180
        const double Radius = 6378.16;

        private static double DistanceBetweenPlaces(double carLat, double carLong, double parkingLat, double parkingLong)
        {
            double dlat = (parkingLat - carLat) * D2R;
            double dlon = (parkingLong - carLong) * D2R;
            double a = (Math.Pow(Math.Sin(dlat / 2.0), 2)) + Math.Cos(carLat * D2R) * Math.Cos(carLong * D2R) * (Math.Pow(Math.Sin(dlon / 2.0), 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * Radius;
        }

        public static double DistanceBetweenPlacesKm(double carLat, double carLong, double parkingLat, double parkingLong)
        {
            return DistanceBetweenPlaces(carLat, carLong, parkingLat, parkingLong);
        }

        public static double DistanceBetweenPlacesM(double carLat, double carLong, double parkingLat, double parkingLong)
        {
            return DistanceBetweenPlaces(carLat, carLong, parkingLat, parkingLong) * 1000;
        }
    }
}

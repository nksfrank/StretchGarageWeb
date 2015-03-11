using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Database;
using Objects;
using Objects.Interface;

namespace BusinessLayer.ParkingManager
{
    public class ParkingPlaceManager : MainHandler
    {
        public static IQueryable AvaiableSpaces(int parkingPlaceID)
        {
            return DB.ParkingPlaces.Where(a => a.Id == parkingPlaceID).Select(a => new { spots = a.ParkingSpots, parked = a.ParkedCars.Where(b => b.IsParked == true)});
        }

        public static int ParkingSpots(ParkingPlace parkingPlace)
        {
            return DB.ParkingPlaces.First(a => a.Id == parkingPlace.Id).ParkingSpots;
        }
    }
}

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
        public int AvaiableSpaces(ParkingPlace parkingPlace)
        {
            return DB.ParkingPlaces.First(a => a.Id == parkingPlace.Id).ParkedCars.Count(a => a.IsParked == true);
        }

        public int ParkingSpots(ParkingPlace parkingPlace)
        {
            return DB.ParkingPlaces.First(a => a.Id == parkingPlace.Id).ParkingSpots;
        }
    }
}

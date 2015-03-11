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
        internal class FrontPageResponse
        {
            public bool IsAvailable {get; set;}
            public string CssClass { get; set; }
            public string Status { get; set; }
        }
        public static IError Get(int ParkingPlaceID)
        {
            if (!DB.ParkingPlaces.Any(a => a.Id == ParkingPlaceID))
                return new Error { 
                    Message = "There is no parking place with id " + ParkingPlaceID,
                    Success = false
                };

            var parking = DB.ParkingPlaces.Where(a => a.Id == ParkingPlaceID).Select(a => new {spots = a.ParkingSpots, numOfParkedCars = a.ParkedCars.Count(b => b.IsParked == true && b.ParkingDate.Date == DateTime.Now.Date) }).FirstOrDefault();

            var cars = new List<FrontPageResponse>();
            for (int i = 0; i < parking.spots; i++)
            {
                var car = i < parking.numOfParkedCars
                    ? new FrontPageResponse {
                        IsAvailable = false,
                        CssClass = "red",
                        Status = "Occupied",
                    }
                    : new FrontPageResponse {
                        IsAvailable = false,
                        CssClass = "red",
                        Status = "Occupied",
                    };
                cars.Add(car);
            }

            throw new NotImplementedException();
        }

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

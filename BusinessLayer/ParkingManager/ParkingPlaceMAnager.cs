using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Database;
using Objects;
using Objects.Interface;
using Objects.WebApiResponse;

namespace BusinessLayer.ParkingManager
{
    public class ParkingPlaceManager : MainHandler
    {
        public static IError Get(int ParkingPlaceID)
        {
            if (!DB.ParkingPlaces.Any(a => a.Id == ParkingPlaceID))
                return new Error { 
                    Message = "There is no parking place with id " + ParkingPlaceID,
                    Success = false
                };

            var parking = DB.ParkingPlaces.Where(a => a.Id == ParkingPlaceID).Select(a => new {spots = a.ParkingSpots, numOfParkedCars = a.ParkedCars.Count(b => b.IsParked == true && b.ParkingDate.Date == DateTime.Now.Date) }).FirstOrDefault();

            var cars = new List<Objects.WebApiResponse.ParkedCarResponse>();
            for (int i = 0; i < parking.spots; i++)
            {
                var car = i < parking.numOfParkedCars
                    ? new Objects.WebApiResponse.ParkedCarResponse(false, "Occupied", "red")
                    : new Objects.WebApiResponse.ParkedCarResponse(false, "Vaccant", "green");
                cars.Add(car);
            }

            return new Objects.WebApiResponse.ApiResponse(true, "", cars);
        }

        public static IError GetAllParkingPlaces()
        {
            if(DB.ParkingPlaces.Count() <= 0) {
                return new Error {
                    Message = "There are no parking places",
                    Success = false,
                };
            }
            var places = DB.ParkingPlaces.Select(a => new ParkingPlaceResponse { Id = a.Id, Name = a.Name, ParkingSpots = a.ParkingSpots, CssClass = "" }).ToList();
            return new ApiResponse(true, "", places);
        }

        public static IQueryable AvailableSpaces(int parkingPlaceID)
        {
            return DB.ParkingPlaces.Where(a => a.Id == parkingPlaceID).Select(a => new { spots = a.ParkingSpots, parked = a.ParkedCars.Where(b => b.IsParked == true)});
        }

        public static int ParkingSpots(int parkingPlaceId)
        {
            return DB.ParkingPlaces.First(a => a.Id == parkingPlaceId).ParkingSpots;
        }


    }
}

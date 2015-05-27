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
    public class ParkingPlaceManager
    {
        private dbDataContext DB = new dbDataContext();
        public IError GetAllParkingPlaces()
        {
            if(!DB.ParkingPlaces.Any())
                return new Error {
                    Message = "Det finns inga parkeringsplatser",
                    Success = false,
                };
            var places = DB.ParkingPlaces.Select(a => new ParkingPlaceResponse { Id = a.Id, Name = a.Name, ParkingSpots = a.ParkingSpots, CssClass = "" }).ToList();
            return new ApiResponse(true, "", places);
        }

        public IError GetParkingPlace(int parkingPlaceId)
        {
            //Check if Parking Place exists
            if (!DB.ParkingPlaces.Any(a => a.Id == parkingPlaceId))
            {
                return new Error
                {
                    Success = false,
                    Message = "Där finns ingen parkeringsplats med id " + parkingPlaceId,
                };
            }
            //Get Parking Place information
            ParkingPlaceResponse place = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new ParkingPlaceResponse { Id = a.Id, Name = a.Name, ParkingSpots = a.ParkingSpots }).FirstOrDefault();
            //Get Cars parked in Parking Place
            var parkMgr = new ParkCarManager();
            var cars = ((ApiResponse)parkMgr.GetParkedCars(parkingPlaceId)).Content;
            place.ParkedCars = (List<ParkedCarResponse>)cars;
            return new ApiResponse(true, "", place);
        }

        public IQueryable AvailableSpaces(int parkingPlaceID)
        {
            return DB.ParkingPlaces.Where(a => a.Id == parkingPlaceID).Select(a => new { spots = a.ParkingSpots, parked = a.ParkedCars.Where(b => b.IsParked)});
        }

        public int ParkingSpots(int parkingPlaceId)
        {
            return DB.ParkingPlaces.First(a => a.Id == parkingPlaceId).ParkingSpots;
        }
    }
}

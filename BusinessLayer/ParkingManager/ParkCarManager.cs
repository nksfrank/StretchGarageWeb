using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Database;
using Objects;
using Objects.Interface;
using Objects.WebApiResponse;

namespace BusinessLayer.ParkingManager
{
    public class ParkCarManager
    {
        private static dbDataContext DB = new dbDataContext();
        public static IError ParkCar(int carId, int parkingPlaceId)
        {
            return ParkCar(carId, parkingPlaceId, DateTime.UtcNow);
        }

        public static IError ParkCar(int carId, int parkingPlaceId, DateTime date)
        {
            if(DB.ParkedCars.Any(a => a.UnitId == carId && a.IsParked))
                return new Error() { Message = "Car is already parked", Success = true };
            
            var car = DB.Units.FirstOrDefault(a => a.Id == carId);
            
            if(car == null)
                return new Error { Message = "There is no car with id " + carId, Success = false };
            car.EnteredZone = date;
            //Remove any occurance of where the car might still be logged as parked.
            car.ParkedCars.Where(a => a.IsParked && a.ParkingDate.Date < date.Date).All(a => { a.IsParked = false; return true; });

            var park = new ParkedCar
            {
                ParkingDate = date,
                IsParked = true,
                ParkingPlaceId = parkingPlaceId,
                UnitId = carId,
            };

            DB.ParkedCars.InsertOnSubmit(park);

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception)
            {
                return new Error() { Success = false, Message = "Could not add car " + carId + " to parking place " + parkingPlaceId };
            }
            return new Error() { Success = true, Message = "" };
        }

        public static IError UnParkCar(int carId)
        {
            var park = DB.ParkedCars.Where(a => a.UnitId == carId && a.IsParked).ToArray();
            //if car is not parked return true
            if (!park.Any()) return new Error() { Success = true, Message = "" };

            park.All(a => { a.IsParked = false; return true; });

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception)
            {
                return new Error() { Success = false, Message = "Could not remove car " + carId };
            }
            return new Error() { Success = true, Message = "" };
        }

        public static IError UnParkCarFromParkingPlace(int carId, int parkingPlaceId)
        {
            if (!IsParked(carId)) return new Error() { Success = true, Message = "" };

            var park = DB.ParkedCars.Where(a => a.UnitId == carId && a.ParkingPlace.Id == parkingPlaceId && a.IsParked).First(a => a.IsParked);
            park.IsParked = false;
            DB.ParkedCars.InsertOnSubmit(park);

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception)
            {
                return new Error() { Success = false, Message = "Could not remove car " + carId + " from parking place " + parkingPlaceId };
            }
            return new Error() { Success = true, Message = "" };
        }

        public static IError GetParkedCars(int parkingPlaceId)
        {
            if (!DB.ParkingPlaces.Any(a => a.Id == parkingPlaceId))
                return new Error
                {
                    Message = "There is no parking place with id " + parkingPlaceId,
                    Success = false
                };

            var parking = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new { spots = a.ParkingSpots, numOfParkedCars = a.ParkedCars.Count(b => b.IsParked && b.ParkingDate.Date == DateTime.UtcNow.Date) }).First();

            var cars = new List<ParkedCarResponse>();
            for (int i = 0; i < parking.spots; i++)
            {
                var car = i < parking.numOfParkedCars
                    ? new ParkedCarResponse(false, "Occupied", "red")
                    : new ParkedCarResponse(true, "Vaccant", "green");
                cars.Add(car);
            }

            return new ApiResponse(true, "", cars);
        }

        public static bool IsParked(int carId)
        {
            return DB.ParkedCars.Any(a => a.UnitId == 0 && a.IsParked);
        }

        public static void ClearOldHistory()
        {
            var history = DB.ParkedCars.Where(a => a.ParkingDate < DateTime.UtcNow.AddDays(-30));
            if (!history.Any()) return;
            DB.ParkedCars.DeleteAllOnSubmit(history);
            DB.SubmitChanges();
        }
    }
}

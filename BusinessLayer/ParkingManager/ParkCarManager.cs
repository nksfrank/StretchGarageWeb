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
        private dbDataContext DB = new dbDataContext();
        public IError ParkCar(int carId, int parkingPlaceId)
        {
            return ParkCar(carId, parkingPlaceId, DateTime.UtcNow);
        }

        public IError ParkCar(int carId, int parkingPlaceId, DateTime date)
        {
            if(DB.ParkedCars.Any(a => a.UnitId == carId && a.IsParked && a.ParkingDate.Date == date.Date && a.ParkingPlaceId == parkingPlaceId))
                return new Error() { Message = "Bilen är redan parkerad", Success = true };
            
            var car = DB.Units.FirstOrDefault(a => a.Id == carId);
            
            if(car == null)
                return new Error { Message = "Det finns ingen bil med id " + carId, Success = false };
            car.Settled = date;
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
                return new Error() { Success = false, Message = "Kunde inte parkerea bil med id " + carId + " till parkeringsplats " + parkingPlaceId };
            }
            return new Error() { Success = true, Message = "" };
        }

        public IError UnParkCar(int carId)
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
                return new Error() { Success = false, Message = "Kunde inte avparkera bil med id " + carId };
            }
            return new Error() { Success = true, Message = "Bilen har blivit avparkerad" };
        }

        public IError UnParkCarFromParkingPlace(int carId, int parkingPlaceId)
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
                return new Error() { Success = false, Message = "Kunde inte parkerea bil med id " + carId + " till parkeringsplats " + parkingPlaceId };
            }
            return new Error() { Success = true, Message = "" };
        }

        public IError GetParkedCars(int parkingPlaceId)
        {
            if (!DB.ParkingPlaces.Any(a => a.Id == parkingPlaceId))
                return new Error
                {
                    Message = "Det finns ingen parkeringsplats med id " + parkingPlaceId,
                    Success = false
                };

            var parking = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new { spots = a.ParkingSpots, numOfParkedCars = a.ParkedCars.Count(b => b.IsParked && b.ParkingDate.Date == DateTime.UtcNow.Date) }).First();
            var users = DB.ParkedCars.Where(a => a.ParkingPlaceId == parkingPlaceId && a.IsParked && a.ParkingDate.Date == DateTime.UtcNow.Date).Select(a => a.Unit.Name).ToList();

            var cars = new List<ParkedCarResponse>();
            for (int i = 0; i < parking.spots; i++)
            {
                var car = new ParkedCarResponse(true, "Ledig", "green");
                cars.Add(car);
            }
            for (int i = 0; i < users.Count; i++)
            {
                cars[i].IsAvailable = false;
                cars[i].Status = users[i];
                cars[i].CssClass = "red";
            }

            return new ApiResponse(true, "", cars);
        }

        public bool IsParked(int carId)
        {
            return DB.ParkedCars.Any(a => a.UnitId == carId && a.IsParked);
        }

        public void ClearOldHistory()
        {
            var history = DB.ParkedCars.Where(a => a.ParkingDate < DateTime.UtcNow.AddDays(-30));
            if (!history.Any()) return;
            DB.ParkedCars.DeleteAllOnSubmit(history);
            DB.SubmitChanges();
        }
    }
}

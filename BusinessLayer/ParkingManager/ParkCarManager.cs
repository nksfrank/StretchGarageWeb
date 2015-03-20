﻿using System;
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
    public class ParkCarManager
    {
        private static dbDataContext DB = new dbDataContext();
        public static IError ParkCar(int carId, int parkingPlaceId)
        {
            return ParkCar(carId, parkingPlaceId, DateTime.Now);
        }

        public static IError ParkCar(int carId, int parkingPlaceId, DateTime date)
        {
            if (IsParked(carId)) {
                UnParkCar(carId);
            }

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
            //if car is not parked return true
            if (!IsParked(carId)) return new Error() { Success = true, Message = "" };

            var park = DB.ParkedCars.Where(a => a.UnitId == carId && a.IsParked);
            foreach (var car in park) {
                car.IsParked = false;
            }

            //DB.ParkedCars.InsertAllOnSubmit(park);

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

            ParkedCar park = DB.ParkedCars.Where(a => a.UnitId == carId && a.ParkingPlace.Id == parkingPlaceId && a.IsParked).First(a => a.IsParked);
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

        public static IError GetParkedCars(int ParkingPlaceID)
        {
            if (!DB.ParkingPlaces.Any(a => a.Id == ParkingPlaceID))
                return new Error
                {
                    Message = "There is no parking place with id " + ParkingPlaceID,
                    Success = false
                };

            var parking = DB.ParkingPlaces.Where(a => a.Id == ParkingPlaceID).Select(a => new { spots = a.ParkingSpots, numOfParkedCars = a.ParkedCars.Count(b => b.IsParked && b.ParkingDate.Date == DateTime.Now.Date) }).FirstOrDefault();

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
    }
}

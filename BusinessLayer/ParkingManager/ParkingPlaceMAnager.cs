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
    public class ParkingPlaceManager
    {
        private static dbDataContext DB = new dbDataContext();
        public static IError GetAllParkingPlaces()
        {
            if(!DB.ParkingPlaces.Any())
                return new Error {
                    Message = "There are no parking places",
                    Success = false,
                };
            var places = DB.ParkingPlaces.Select(a => new ParkingPlaceResponse { Id = a.Id, Name = a.Name, ParkingSpots = a.ParkingSpots, CssClass = "" }).ToList();
            return new ApiResponse(true, "", places);
        }

        public static IError GetParkingPlace(int parkingPlaceId)
        {
            //Check if Parking Place exists
            if (!DB.ParkingPlaces.Any(a => a.Id == parkingPlaceId))
            {
                return new Error
                {
                    Success = false,
                    Message = "There is no Parking Place with that id",
                };
            }
            //Get Parking Place information
            ParkingPlaceResponse place = DB.ParkingPlaces.Where(a => a.Id == parkingPlaceId).Select(a => new ParkingPlaceResponse { Id = a.Id, Name = a.Name, ParkingSpots = a.ParkingSpots }).FirstOrDefault();
            //Get Cars parked in Parking Place
            var cars = ((ApiResponse)ParkCarManager.GetParkedCars(parkingPlaceId)).Content;
            place.ParkedCars = (List<ParkedCarResponse>)cars;
            return new ApiResponse(true, "", place);
        }

        public static IQueryable AvailableSpaces(int parkingPlaceID)
        {
            return DB.ParkingPlaces.Where(a => a.Id == parkingPlaceID).Select(a => new { spots = a.ParkingSpots, parked = a.ParkedCars.Where(b => b.IsParked)});
        }

        public static int ParkingSpots(int parkingPlaceId)
        {
            return DB.ParkingPlaces.First(a => a.Id == parkingPlaceId).ParkingSpots;
        }
    }
}
